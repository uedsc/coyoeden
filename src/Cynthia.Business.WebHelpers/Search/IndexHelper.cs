// Author:					    Joe Audette
// Created:				        2007-08-30
// Last Modified:			    2009-09-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

//http://www.codeproject.com/KB/cs/lucene_analysis.aspx
//http://www.ifdefined.com/blog/post/2009/02/Full-Text-Search-in-ASPNET-using-LuceneNET.aspx
//http://www.codeproject.com/KB/library/IntroducingLucene.aspx
//http://www.aspfree.com/c/a/BrainDump/Working-with-Lucene-dot-Net/
//http://davidpodhola.blogspot.com/2008/02/how-to-highlight-phrase-on-results-from.html
//http://linqtolucene.codeplex.com/

//example database implementation
//http://www.codeproject.com/KB/database/FulltextFirebird.aspx

//http://vijay.screamingpens.com/archive/2008/07/21/linq-amp-lambda-part-4-lucene.net.aspx
//http://lucene.apache.org/solr/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using log4net;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Highlight;
using ParseException=Lucene.Net.QueryParsers.ParseException;
using Cynthia.Web.Framework;

namespace Cynthia.Business.WebHelpers
{
    public sealed class IndexHelper
    {
        #region Constructors

        private IndexHelper()
        {

        }

        #endregion

        private static readonly ILog log = LogManager.GetLogger(typeof(IndexHelper));

        
        public static IndexItemCollection Search(
            int siteId,
            bool isAdmin,
            List<string> userRoles,
            Guid featureGuid,
            string queryText, 
            bool highlightResults,
            int highlightedFragmentSize,
            int pageNumber, 
            int pageSize,
            out int totalHits,
            out bool invalidQuery)
        {
            invalidQuery = false;
            totalHits = 0;
            string indexPath = GetIndexPath(siteId);
            IndexItemCollection results = new IndexItemCollection();

            if (string.IsNullOrEmpty(queryText))
            {
                return results;
            }

            bool useBackwardCompatibilityMode = true;
            if (
                (ConfigurationManager.AppSettings["SearchUseBackwardCompatibilityMode"] != null)
                && (ConfigurationManager.AppSettings["SearchUseBackwardCompatibilityMode"] == "false")
              )
            {
                useBackwardCompatibilityMode = false;
            }

            bool DisableSearchFeatureFilters = true;

            if (
                (ConfigurationManager.AppSettings["DisableSearchFeatureFilters"] != null)
                && (ConfigurationManager.AppSettings["DisableSearchFeatureFilters"] == "false")
              )
            {
                DisableSearchFeatureFilters = false;
            }

            bool IncludeModuleRoleFilters = false;

            if (
                (ConfigurationManager.AppSettings["SearchIncludeModuleRoleFilters"] != null)
                && (ConfigurationManager.AppSettings["SearchIncludeModuleRoleFilters"] == "true")
              )
            {
                IncludeModuleRoleFilters = true;
            }

          
            if (IndexReader.IndexExists(indexPath))
            {

                if (log.IsDebugEnabled)
                {
                    log.Debug("Entered Search, indexPath = " + indexPath);
                }

                long startTicks = DateTime.Now.Ticks;

                try
                {

                    BooleanQuery mainQuery = new BooleanQuery();

                    if ((!isAdmin) && (!useBackwardCompatibilityMode))
                    {
                        AddRoleQueries(userRoles, mainQuery);
                    }

                    if ((!isAdmin) && (IncludeModuleRoleFilters))
                    {
                        AddModuleRoleQueries(userRoles, mainQuery);
                    }


                    Query multiQuery = MultiFieldQueryParser.Parse(
                        new string[] { queryText, queryText, queryText, queryText, queryText, queryText.Replace("*", string.Empty) },
                        new string[] { "Title", "ModuleTitle", "contents", "PageName", "PageMetaDesc", "Keyword" },
                        new StandardAnalyzer());

                    mainQuery.Add(multiQuery, BooleanClause.Occur.MUST);


                    if (!useBackwardCompatibilityMode)
                    {
                        Term beginDateStart = new Term("PublishBeginDate", DateTime.MinValue.ToString("s"));
                        Term beginDateEnd = new Term("PublishBeginDate", DateTime.UtcNow.ToString("s"));
                        RangeQuery beginDateQuery = new RangeQuery(beginDateStart, beginDateEnd, true);
                        mainQuery.Add(beginDateQuery, BooleanClause.Occur.MUST);

                        Term endDateStart = new Term("PublishEndDate", DateTime.UtcNow.ToString("s"));
                        Term endDateEnd = new Term("PublishEndDate", DateTime.MaxValue.ToString("s"));
                        RangeQuery endDateQuery = new RangeQuery(endDateStart, endDateEnd, true);
                        mainQuery.Add(endDateQuery, BooleanClause.Occur.MUST);
                    }

                    if ((!DisableSearchFeatureFilters) && (featureGuid != Guid.Empty))
                    {
                        BooleanQuery featureFilter = new BooleanQuery();
                        featureFilter.Add(new TermQuery(new Term("FeatureId", featureGuid.ToString())), BooleanClause.Occur.MUST);
                        mainQuery.Add(featureFilter, BooleanClause.Occur.MUST);
                    }


                    IndexSearcher searcher = new IndexSearcher(indexPath);
                    // a 0 based colection
                    Hits hits = searcher.Search(mainQuery);
                    
                    int startHit = 0;

                    if (pageNumber > 1)
                    {
                        startHit = ((pageNumber - 1) * pageSize);
                    }


                    totalHits = hits.Length();
                    int end = startHit + pageSize;
                    if (totalHits <= end)
                    {
                        end = totalHits;
                    }
                    int itemsAdded = 0;
                    int itemsToAdd = end;

                    // in backward compatibility mode if multiple pages of results are found we amy not be showing every user the correct
                    // number of hits they can see as we only filter out the current page
                    //we may decrement total hits if filtering results so keep the original count
                    int actualHits = totalHits;

                    if (!useBackwardCompatibilityMode)
                    {
                        // this new way is much cleaner
                        //all filtering is done by query so the hitcount is true
                        //whereas with the old way it could be wrong since there
                        // were possibly results filtered out after the query returned.

                        QueryScorer scorer = new QueryScorer(multiQuery);
                        Formatter formatter = new SimpleHTMLFormatter("<span class='searchterm'>", "</span>");
                        Highlighter highlighter = new Highlighter(formatter, scorer);
                        highlighter.SetTextFragmenter(new SimpleFragmenter(highlightedFragmentSize));


                        for (int i = startHit; i < itemsToAdd; i++)
                        {
                            IndexItem indexItem = new IndexItem(hits.Doc(i), hits.Score(i));

                            if (highlightResults)
                            {
                                try
                                {
                                    TokenStream stream = new StandardAnalyzer().TokenStream("contents", new StringReader(hits.Doc(i).Get("contents")));
                                    
                                    string highlightedResult = highlighter.GetBestFragment(stream, hits.Doc(i).Get("contents"));
                                    if (highlightedResult != null) { indexItem.Intro = highlightedResult; }
                                }
                                catch (NullReferenceException) { }

                            }

                            results.Add(indexItem);
                            itemsAdded += 1;

                        }


                    }
                    else
                    {
                        //backward compatible with old indexes
                        int filteredItems = 0;
                        for (int i = startHit; i < itemsToAdd; i++)
                        {

                            bool needToDecrementTotalHits = false;
                            if (
                                (isAdmin)
                                || (WebUser.IsContentAdmin)
                                || (WebUser.IsInRoles(hits.Doc(i).Get("ViewRoles")))
                                )
                            {
                                IndexItem indexItem = new IndexItem(hits.Doc(i), hits.Score(i));

                                if (
                                (DateTime.UtcNow > indexItem.PublishBeginDate)
                                && (DateTime.UtcNow < indexItem.PublishEndDate)
                                )
                                {
                                    results.Add(indexItem);
                                }
                                else
                                {
                                    needToDecrementTotalHits = true;
                                }

                            }
                            else
                            {
                                needToDecrementTotalHits = true;
                            }

                            //filtered out a result so need to decrement
                            if (needToDecrementTotalHits)
                            {
                                filteredItems += 1;
                                totalHits -= 1;

                                //we also are not getting as many results as the page size so if there are more items
                                //we should increment itemsToAdd
                                if ((itemsAdded + filteredItems) < actualHits)
                                {
                                    itemsToAdd += 1;
                                }
                            }

                        }
                    }



                    searcher.Close();

                    results.ItemCount = itemsAdded;
                    results.PageIndex = pageNumber;

                    results.ExecutionTime = DateTime.Now.Ticks - startTicks;
                }
                catch (ParseException ex)
                {
                    invalidQuery = true;
                    log.Error("handled error for search terms " + queryText, ex);
                    // these parser exceptions are generally caused by
                    // spambots posting too much junk into the search form
                    // heres an option to automatically ban the ip address
                    HandleSpam(queryText, ex);


                    return results;
                }
                catch (BooleanQuery.TooManyClauses ex)
                {
                    invalidQuery = true;
                    log.Error("handled error for search terms " + queryText, ex);
                    return results;

                }

            }

            return results;
        }

       

        private static void HandleSpam(string queryText, Exception ex)
        {
            bool autoBanSpamBots = ConfigHelper.GetBoolProperty("AutoBanSpambotsOnSearchErrors", false);
           
            if ((autoBanSpamBots)&&(IsSpam(queryText)))
            {
                if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
                {
                    BannedIPAddress b = new BannedIPAddress();
                    b.BannedIP = HttpContext.Current.Request.UserHostAddress;
                    b.BannedReason = "spambot autodetected";
                    b.BannedUtc = DateTime.UtcNow;
                    b.Save();

                    String pathToCacheDependencyFile
                            = HttpContext.Current.Server.MapPath(
                        "~/Data/bannedipcachedependency.config");

                    CacheHelper.TouchCacheFile(pathToCacheDependencyFile);

                    //log.Error(queryText, ex);
                    log.Info("spambot detected, ip address has been banned: " + HttpContext.Current.Request.UserHostAddress);
                }
            }
            else
            {
                //log.Error(queryText, ex);
                log.Info("spambot possibly detected, ip address was: " + HttpContext.Current.Request.UserHostAddress);
            
            }

           


        }

        private static bool IsSpam(string queryText)
        {
            // Commented out the below on 2009-05-25 because now that we are using query string params for search instead
            // of a form field, we can no longer assume abuse simply by the query being longer than 255 chars
            //if (queryText.Length > 255) { return true; }

            // TODO: determine by key words?

            return false;
        }


        private static string GetIndexPath(int siteId)
        {
            if ((HttpContext.Current != null)&&(siteId > -1))
            {
                return HttpContext.Current.Server.MapPath(GetDataFolder(siteId) + "index/");
            }

            return string.Empty;
        }


        public static string GetDataFolder(int siteId)
        {
            return "~/Data/Sites/" + siteId.ToString(CultureInfo.InvariantCulture) + "/";
        }

        public static string GetSearchIndexPath(int siteId)
        {
            if (HttpContext.Current == null) return string.Empty;

            return HttpContext.Current.Server.MapPath(GetDataFolder(siteId) + "index/");
        }


        public static Regex MarkupRegex = new Regex("<[/a-zA-Z]+[^>]*>|<!--(?!-->)*-->");

        public static string ConvertToText(string markup)
        {
            return MarkupRegex.Replace(markup, " ");
        }

        private static void AddRoleQueries(List<string> userRoles, BooleanQuery mainQuery)
        {
            BooleanQuery rolesQuery = new BooleanQuery();
            foreach (string role in userRoles)
            {
                Term term = new Term("Role", role);

                TermQuery termQuery = new TermQuery(term);
                rolesQuery.Add(termQuery, BooleanClause.Occur.SHOULD);

                
            }
            // in a boolean query with multiple should occur items, at least one must occur

            mainQuery.Add(rolesQuery, BooleanClause.Occur.MUST);
            
        }

        private static void AddModuleRoleQueries(List<string> userRoles, BooleanQuery mainQuery)
        {
            BooleanQuery rolesQuery = new BooleanQuery();
            foreach (string role in userRoles)
            {
                Term term = new Term("ModuleRole", role);

                TermQuery termQuery = new TermQuery(term);
                rolesQuery.Add(termQuery, BooleanClause.Occur.SHOULD);


            }
            // in a boolean query with multiple should occur items, at least one must occur

            mainQuery.Add(rolesQuery, BooleanClause.Occur.MUST);

        }

        private static BooleanQuery BuildRoleQuery(List<string> userRoles)
        {
            BooleanQuery bQuery = new BooleanQuery();
            foreach (string role in userRoles)
            {
                bQuery.Add(new TermQuery(new Term("ViewRoles2", role)), BooleanClause.Occur.SHOULD);
            }
            // in a boolean query with multiple should occur items, at least one must occur

            return bQuery;
        }



        private static BooleanQuery BuildQueryFromKeywords(Hashtable keyWords)
        {
            BooleanQuery bQuery = new BooleanQuery();
            foreach (DictionaryEntry keywordFilterTerm in keyWords)
            {
                string field = keywordFilterTerm.Key.ToString();
                string keyword = keywordFilterTerm.Value.ToString();
                //bQuery.Add(
                //    new TermQuery(new Term(field, keyword)), 
                //    true, 
                //    false);

                bQuery.Add(
                    new TermQuery(new Term(field, keyword)),
                    BooleanClause.Occur.SHOULD);
            }

            return bQuery;
        }


        public static void RebuildIndex(IndexItem indexItem)
        {
            if (indexItem == null) return;

            if (indexItem.IndexPath.Length > 0)
            {
                RebuildIndex(indexItem, indexItem.IndexPath);
                return;
            }

            string indexPath = GetIndexPath(indexItem.SiteId);
            RebuildIndex(indexItem, indexPath);
        }


        public static void RebuildIndex(IndexItem indexItem, string indexPath)
        {
            if (indexItem == null) return;
            if (indexPath == null) return;
            if (indexPath.Length == 0) return;

            IndexingQueue queueItem = new IndexingQueue();
            queueItem.IndexPath = indexPath;
            queueItem.ItemKey = indexItem.Key;
            queueItem.RemoveOnly = false;
            queueItem.SerializedItem = SerializationHelper.SerializeToString(indexItem);
            queueItem.Save();

            // the above queues the items to be indexed. Edit page must also call SiteUtils.QueueIndexing(); after the content is saved.

            
            
        }


        public static void RemoveIndex(IndexItem indexItem)
        {
            if (indexItem == null) return;

            if (indexItem.IndexPath.Length > 0)
            {
                RemoveIndex(indexItem, indexItem.IndexPath);
                return;

            }

            indexItem.IndexPath = GetIndexPath(indexItem.SiteId);

            RemoveIndex(indexItem, indexItem.IndexPath);
            return;

            //// delete from the index
            //Term term = new Term("Key", indexItem.Key);
            //try
            //{
            //    IndexReader reader = IndexReader.Open(indexPath);
            //    reader.DeleteDocuments(term);
            //    reader.Close();
            //}
            //catch (IOException ex)
            //{
            //    log.Error(ex);
            //}
        }

        public static void RemoveIndex(IndexItem indexItem, string indexPath)
        {
            if (indexItem == null) return;
            if (indexPath == null) return;
            if (indexPath.Length == 0) return;

            IndexingQueue queueItem = new IndexingQueue();
            queueItem.IndexPath = indexPath;
            queueItem.ItemKey = indexItem.Key;
            queueItem.RemoveOnly = true;
            queueItem.SerializedItem = SerializationHelper.SerializeToString(indexItem);
            queueItem.Save();

            // the above queues the items to be indexed. Edit page must also call SiteUtils.QueueIndexing(); after the content is deleted.

           
        }


   
        public static void RemoveIndexItem(
            int pageId,
            int moduleId, 
            int itemId)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            
            if (siteSettings == null)
            {
                if (log.IsErrorEnabled)
                    log.Error("IndexHelper.RemoveIndexItem tried to obtain a SiteSettings object but it came back null");
                return;
            }

            IndexItem indexItem = new IndexItem();
            indexItem.SiteId = siteSettings.SiteId;
            indexItem.PageId = pageId;
            indexItem.ModuleId = moduleId;
            indexItem.ItemId = itemId;
            indexItem.IndexPath = GetIndexPath(siteSettings.SiteId);

            RemoveIndex(indexItem);

            if (log.IsDebugEnabled) log.Debug("Removed Index ");
        }

        public static void RemoveIndexItem(
            int pageId,
            int moduleId,
            string itemKey)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings == null)
            {
                if (log.IsErrorEnabled)
                    log.Error("IndexHelper.RemoveIndexItem tried to obtain a SiteSettings object but it came back null");
                return;
            }

            IndexItem indexItem = new IndexItem();
            indexItem.SiteId = siteSettings.SiteId;
            indexItem.PageId = pageId;
            indexItem.ModuleId = moduleId;
            indexItem.ItemKey = itemKey;
            indexItem.IndexPath = GetIndexPath(siteSettings.SiteId);

            RemoveIndex(indexItem);

            if (log.IsDebugEnabled) log.Debug("Removed Index ");
        }

        public static void RemoveIndexItem(
            int siteId,
            int pageId,
            int moduleId,
            int itemId,
            string indexPath)
        {
            
            IndexItem indexItem = new IndexItem();
            indexItem.SiteId = siteId;
            indexItem.PageId = pageId;
            indexItem.ModuleId = moduleId;
            indexItem.ItemId = itemId;
            indexItem.IndexPath = indexPath;

            RemoveIndex(indexItem);

            if (log.IsDebugEnabled) log.Debug("Removed Index ");
        }


        

        public static void DeleteSearchIndex(SiteSettings siteSettings)
        {
            string indexPath = GetIndexPath(siteSettings.SiteId);
            if (indexPath.Length == 0) { return; }
            if (!Directory.Exists(indexPath)) { return; }

            try
            {
                DirectoryInfo dir = new DirectoryInfo(indexPath);
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo f in files)
                {
                    File.Delete(f.FullName);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }


        #region ClearPageIndex


        public static void ClearPageIndexAsync(PageSettings pageSettings)
        {
            pageSettings.IndexPath = GetIndexPath(pageSettings.SiteId);

            if (ThreadPool.QueueUserWorkItem(new WaitCallback(ClearPageIndexAsyncCallback), pageSettings))
            {
                if (log.IsDebugEnabled) log.Debug("IndexHelper.ClearPageIndexAsyncCallback queued");
            }
            else
            {
                if (log.IsDebugEnabled) log.Debug("Failed to queue a thread for IndexHelper.ClearPageIndexAsync");
            }
        }


        private static void ClearPageIndexAsyncCallback(object o)
        {
            if (o == null) return;
            if (!(o is PageSettings)) return;

            //try
            //{
                PageSettings pageSettings = (PageSettings)o;
                IndexHelper.ClearPageIndex(pageSettings);
            //}
            //catch (Exception ex)
            //{
            //    if (log.IsErrorEnabled) log.Error("IndexHelper.ClearPageIndexAsyncCallback", ex);
            //}
        }


        private static bool ClearPageIndex(PageSettings pageSettings)
        {
            if (pageSettings == null) return false;
            bool result = false;

            try
            {
                string indexPath = pageSettings.IndexPath;

                if (IndexReader.IndexExists(indexPath))
                {
                    IndexReader reader = IndexReader.Open(indexPath);
                    try
                    {
                        for (int i = 0; i < reader.NumDocs(); i++)
                        {
                            Document doc = reader.Document(i);
                            if (doc.GetField("PageID").StringValue() ==
                                pageSettings.PageId.ToString(CultureInfo.InvariantCulture))
                            {
                                if (log.IsDebugEnabled) log.Debug("ClearPageIndex about to delete doc ");
                                try
                                {
                                    reader.DeleteDocument(i);
                                    result = true;
                                }
                                catch (IOException)
                                {
                                }
                            }
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
            }

            return result;
        }


        #endregion


        #region RebuildPageIndex


        public static void RebuildPageIndexAsync(PageSettings pageSettings)
        {
            pageSettings.IndexPath = GetIndexPath(pageSettings.SiteId);

            if (ThreadPool.QueueUserWorkItem(
                new WaitCallback(RebuildPageIndexAsyncCallback), pageSettings))
            {
                if (log.IsDebugEnabled) log.Debug("IndexHelper.RebuildPageIndexCallback queued");
            }
            else
            {
                if (log.IsDebugEnabled) log.Debug("Failed to queue a thread for IndexHelper.RebuildPageIndexAsync");
            }
        }


        private static void RebuildPageIndexAsyncCallback(object o)
        {
            if (o == null) return;
            if (!(o is PageSettings)) return;

            try
            {
                PageSettings pageSettings = (PageSettings)o;
                IndexHelper.RebuildPageIndex(pageSettings);

                // TODO: could add some form of notification to let the admin know if
                // it was able to index all the content
            }
            catch (TypeInitializationException ex)
            {
                if (log.IsErrorEnabled) log.Error("IndexHelper.RebuildPageIndexAsyncCallback", ex);
            }
        }

        private static bool RebuildPageIndex(PageSettings pageSettings)
        {
            if (pageSettings == null) return false;

            log.Info("IndexHelper.RebuildPageIndex - " + pageSettings.PageName);

            if (IndexBuilderManager.Providers == null)
            {
                log.Info("No IndexBuilderProviders found");
                return false;
            }


            string indexPath = pageSettings.IndexPath;

            ClearPageIndex(pageSettings);

            foreach (IndexBuilderProvider indexBuilder in IndexBuilderManager.Providers)
            {
                indexBuilder.RebuildIndex(pageSettings, indexPath);
            }

            return true;
        }


       
        #endregion


        #region RebuildSiteIndex

        public static bool VerifySearchIndex(SiteSettings siteSettings)
        {
            string indexPath = GetIndexPath(siteSettings.SiteId);
            if (indexPath.Length == 0) { return false; }

            if (!Directory.Exists(indexPath))
            {
                Directory.CreateDirectory(indexPath);
            }

            if (Directory.Exists(indexPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(indexPath);
                int fileCount = directoryInfo.GetFiles().Length;
                int configFileCount = directoryInfo.GetFiles(".config").Length;
                if (
                    (fileCount == 0)
                    || (
                        (fileCount == 1)
                        && (configFileCount == 1)
                        )
                    )
                {
                    int rowsToIndex = IndexingQueue.GetCount();
                    if (rowsToIndex > 0)
                    {
                        // already started the indexing process
                        return true;
                    }
                    // no search index exists so build it
                    IndexHelper.RebuildSiteIndexAsync(indexPath, CacheHelper.GetMenuPages());
                    return false;
                }
            }

            return true;
        }


        public static void RebuildSiteIndexAsync(
            string indexPath, IEnumerable<PageSettings> menuPages)
        {
            ArrayList arrayList = new ArrayList();
            arrayList.Add(indexPath);
            arrayList.Add(menuPages);

            if (ThreadPool.QueueUserWorkItem(
                new WaitCallback(RebuildSiteIndexAsyncCallback), arrayList))
            {
                if (log.IsDebugEnabled) log.Debug("IndexHelper.RebuildSiteIndexAsyncCallback queued");
            }
            else
            {
                if (log.IsDebugEnabled) log.Debug("Failed to queue a thread for IndexHelper.RebuildSiteIndexAsync");
            }
        }


        private static void RebuildSiteIndexAsyncCallback(object objArrayList)
        {
            
            ArrayList arrayList = (ArrayList)objArrayList;
            string indexPath = (string)arrayList[0];

            IEnumerable<PageSettings> menuPages 
                = (IEnumerable<PageSettings>)arrayList[1];

            RebuildSiteIndex(indexPath, menuPages);
        }

        private static bool RebuildSiteIndex(
            string indexPath,
            IEnumerable<PageSettings> menuPages)
        {
            // clean out index entirely
            if (IndexReader.IndexExists(indexPath))
            {
                IndexReader reader = IndexReader.Open(indexPath);
                for (int i = 0; i < reader.NumDocs(); i++)
                {
                    reader.DeleteDocument(i);
                }
                reader.Close();
            }

            log.Info("Rebuilding Search index.");

            if (IndexBuilderManager.Providers == null)
            {
                log.Info("No IndexBuilderProviders found");
                return false;
            }

            // forums can potentially take  long time to index
            // and possibly even time out so index forums after everything else
            foreach (PageSettings pageSettings in menuPages)
            {
                foreach (IndexBuilderProvider indexBuilder in IndexBuilderManager.Providers)
                {
                    if (indexBuilder.Name != "ForumThreadIndexBuilderProvider")
                    {
                        indexBuilder.RebuildIndex(pageSettings, indexPath);
                    }
                }

            }

            log.Info("Finished indexing main features.");

            // now that other modules are don index forums
            foreach (PageSettings pageSettings in menuPages)
            {
                foreach (IndexBuilderProvider indexBuilder in IndexBuilderManager.Providers)
                {
                    if (indexBuilder.Name == "ForumThreadIndexBuilderProvider")
                    {
                        indexBuilder.RebuildIndex(pageSettings, indexPath);
                    }
                }

            }

            log.Info("Finished indexing Forums.");
                    
            return true;
        }


        


        #endregion
    }
}
