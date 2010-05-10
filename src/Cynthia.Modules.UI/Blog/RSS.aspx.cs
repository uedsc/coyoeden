

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Xml;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;


namespace Cynthia.Web.FeedUI
{
	
	public partial class RssPage : Page
	{
		private int moduleId = -1;
        private int pageID = -1;
        private PageSettings currentPage = null;
        private Module module = null;
        //private string baseUrl = string.Empty;
        private bool addSignature = false;
        Hashtable moduleSettings = null;
        private bool addCommentsLink = false;
        private string navigationSiteRoot = string.Empty;
        private string blogBaseUrl = string.Empty;
        private string imageSiteRoot = string.Empty;
        private string cssBaseUrl = string.Empty;
        private bool feedIsDisabled = false;
        private bool useExcerptInFeed = false;
        private string ExcerptSuffix = "...";
        private string MoreLinkText = "read more";
        private int excerptLength = 250;
        private bool ShowPostAuthorSetting = false;
        private bool bypassPageSecurity = false;
        private Guid securityBypassGuid = Guid.Empty;
		private int _categoryId;


		protected void Page_Load(object sender, System.EventArgs e)
		{
            // nothing should post here
            if (Page.IsPostBack) return;

            LoadSettings();
            
            
            if ((moduleId > -1)&&(module != null)&&(currentPage != null))
			{	
				RenderRss(moduleId);
				
			}
			else
			{
				RenderError("Invalid Request");
			}

		}

        private void LoadSettings()
        {
            pageID = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
			_categoryId = WebUtils.ParseInt32FromQueryString("cid",-1);
            securityBypassGuid = WebUtils.ParseGuidFromQueryString("g", securityBypassGuid);
            currentPage = CacheHelper.GetCurrentPage();

            // this is for a special case of consuming a feed from a private page using the feed manager
            if ((securityBypassGuid != Guid.Empty) && (securityBypassGuid == WebConfigSettings.InternalFeedSecurityBypassKey))
            {
                bypassPageSecurity = true;
            }

            // TODO: need to make this bring back page view roles
            // so we don't have to get the pagesettings object
            //module = new Module(moduleId, pageID);

            if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
            {
                navigationSiteRoot = SiteUtils.GetNavigationSiteRoot();
                blogBaseUrl = navigationSiteRoot;
                imageSiteRoot = WebUtils.GetSiteRoot();
                cssBaseUrl = imageSiteRoot;
            }
            else
            {
                navigationSiteRoot = WebUtils.GetHostRoot();
                blogBaseUrl = SiteUtils.GetNavigationSiteRoot();
                imageSiteRoot = navigationSiteRoot;
                cssBaseUrl = WebUtils.GetSiteRoot();

            }
            


            object value = GetModule();
            

            if (value != null)
            {
                module = (Module)value;
            }

        }

        private Module GetModule()
        {
            
            Module m = null;
            if (currentPage != null)
            {
                foreach (Module module in currentPage.Modules)
                {
                    if (module.ModuleId == moduleId)
                        m = module;
                }
            }

            if (m == null) return null;
            if (m.ModuleId == -1) return null;

            if (m.ControlSource.ToLower().Contains("blogmodule.ascx"))
            {
                return m;
            }

            return null;
        }

		private void RenderRss(int moduleId)
		{
			/*
			 
			For more info on RSS 2.0
			http://www.feedvalidator.org/docs/rss2.html
			
			Fields not implemented yet:
			<blogChannel:blogRoll>http://radio.weblogs.com/0001015/userland/scriptingNewsLeftLinks.opml</blogChannel:blogRoll>
			<blogChannel:mySubscriptions>http://radio.weblogs.com/0001015/gems/mySubscriptions.opml</blogChannel:mySubscriptions>
			<blogChannel:blink>http://diveintomark.org/</blogChannel:blink>
			<lastBuildDate>Mon, 30 Sep 2002 11:00:00 GMT</lastBuildDate>
			<docs>http://backend.userland.com/rss</docs>
			 
			*/



            Response.Cache.SetExpires(DateTime.Now.AddMinutes(30));
            Response.Cache.SetCacheability(HttpCacheability.Public);
            //Response.Cache.VaryByParams["g"] = true;

			Response.ContentType = "application/xml";
			
			moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

			addSignature = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "RSSAddSignature", false);

            ShowPostAuthorSetting = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "ShowPostAuthorSetting", ShowPostAuthorSetting);

            addCommentsLink = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "RSSAddCommentsLink", false);

            feedIsDisabled = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "BlogDisableFeedSetting", feedIsDisabled);

            useExcerptInFeed = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "UseExcerptInFeedSetting", useExcerptInFeed);

            excerptLength = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "BlogExcerptLengthSetting", excerptLength);

            if (moduleSettings.Contains("BlogExcerptSuffixSetting"))
            {
                ExcerptSuffix = moduleSettings["BlogExcerptSuffixSetting"].ToString();
            }

            if (moduleSettings.Contains("BlogMoreLinkText"))
            {
                MoreLinkText = moduleSettings["BlogMoreLinkText"].ToString();
            }

			Response.ContentEncoding = Encoding.UTF8;

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(Response.OutputStream, Encoding.UTF8))
            {
                xmlTextWriter.Formatting = Formatting.Indented;

                xmlTextWriter.WriteStartDocument();


                //////////////////
                // style for RSS Feed viewed in browsers
                if (ConfigurationManager.AppSettings["RSSCSS"] != null)
                {
                    string rssCss = ConfigurationManager.AppSettings["RSSCSS"].ToString();
                    xmlTextWriter.WriteWhitespace(" ");
					xmlTextWriter.WriteRaw(String.Format("<?xml-stylesheet type=\"text/css\" href=\"{0}{1}\" ?>", cssBaseUrl, rssCss));

                }

                if (ConfigurationManager.AppSettings["RSSXsl"] != null)
                {
                    string rssXsl = ConfigurationManager.AppSettings["RSSXsl"].ToString();
                    xmlTextWriter.WriteWhitespace(" ");
					xmlTextWriter.WriteRaw(String.Format("<?xml-stylesheet type=\"text/xsl\" href=\"{0}{1}\" ?>", cssBaseUrl, rssXsl));

                }
                ///////////////////////////


				xmlTextWriter.WriteComment(String.Format("RSS generated by Cynthia Blog Module V 1.0 on {0}", DateTime.Now.ToLongDateString()));

                xmlTextWriter.WriteStartElement("rss");

                xmlTextWriter.WriteStartAttribute("version", "");
                xmlTextWriter.WriteString("2.0");
                xmlTextWriter.WriteEndAttribute();

                xmlTextWriter.WriteStartElement("channel");
                /*  
                    RSS 2.0
                    Required elements for channel are title link and description
                */

                xmlTextWriter.WriteStartElement("title");
                xmlTextWriter.WriteString(module.ModuleTitle);
                xmlTextWriter.WriteEndElement();

                // this assumes a valid pageid passed in url
                string blogUrl = SiteUtils.GetCurrentPageUrl();

                xmlTextWriter.WriteStartElement("link");
                xmlTextWriter.WriteString(blogUrl);
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("description");
                xmlTextWriter.WriteString(moduleSettings["BlogDescriptionSetting"].ToString());
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("copyright");
                xmlTextWriter.WriteString(moduleSettings["BlogCopyrightSetting"].ToString());
                xmlTextWriter.WriteEndElement();

                // begin optional RSS 2.0 fields

                //ttl = time to live in minutes, how long a channel can be cached before refreshing from the source
                xmlTextWriter.WriteStartElement("ttl");
                xmlTextWriter.WriteString(moduleSettings["BlogRSSCacheTimeSetting"].ToString());
                xmlTextWriter.WriteEndElement();

                //protection from scrapers wnating to add you to the spam list
                string authorEmail = moduleSettings["BlogAuthorEmailSetting"].ToString().Replace("@", "@nospam");

                xmlTextWriter.WriteStartElement("managingEditor");
                xmlTextWriter.WriteString(authorEmail);
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("generator");
                xmlTextWriter.WriteString("Cynthia Blog Module V 1.0");
                xmlTextWriter.WriteEndElement();

                // check if the user has page view permission
                
                if (
                    (!feedIsDisabled)
                    && (currentPage.ContainsModule(moduleId))
                    && ((bypassPageSecurity) ||(WebUser.IsInRoles(currentPage.AuthorizedRoles)))
                    )
                {
                    RenderItems(xmlTextWriter);
                }
                else
                {
                    //beginning of blog entry
                    xmlTextWriter.WriteStartElement("item");
                    xmlTextWriter.WriteStartElement("title");
                    xmlTextWriter.WriteString("this feed is not available");
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteStartElement("link");
                    xmlTextWriter.WriteString(navigationSiteRoot);
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteStartElement("pubDate");
                    xmlTextWriter.WriteString(DateTime.UtcNow.ToString("r"));
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteStartElement("guid");
                    xmlTextWriter.WriteString(navigationSiteRoot);
                    xmlTextWriter.WriteEndElement();

                    //end blog entry
                    xmlTextWriter.WriteEndElement();

                }


                //end of document
                xmlTextWriter.WriteEndElement();

            }



		}

        private void RenderItems(XmlTextWriter xmlTextWriter)
        {
            string blogCommentLabel = ConfigurationManager.AppSettings["BlogCommentCountLabel"];

            using (IDataReader dr = Blog.GetBlogs(moduleId, DateTime.UtcNow,_categoryId))
            {
                //write channel items
                while (dr.Read())
                {
                    string inFeed = dr["IncludeInFeed"].ToString();
                    if (inFeed == "True" || inFeed == "1")
                    {
                        //beginning of blog entry
                        xmlTextWriter.WriteStartElement("item");

                        string blogItemUrl = FormatBlogUrl(dr["ItemUrl"].ToString(), Convert.ToInt32(dr["ItemID"]));

                        /*  
                        RSS 2.0
                        All elements of an item are optional, however at least one of title or description 
                        must be present.
                        */

                        xmlTextWriter.WriteStartElement("title");
                        xmlTextWriter.WriteString(dr["Heading"].ToString());
                        xmlTextWriter.WriteEndElement();

                        xmlTextWriter.WriteStartElement("link");
                        xmlTextWriter.WriteString(blogItemUrl);
                        xmlTextWriter.WriteEndElement();

                        xmlTextWriter.WriteStartElement("pubDate");
                        xmlTextWriter.WriteString(Convert.ToDateTime(dr["StartDate"]).ToString("r"));
                        xmlTextWriter.WriteEndElement();

                        xmlTextWriter.WriteStartElement("guid");
                        xmlTextWriter.WriteString(blogItemUrl);
                        xmlTextWriter.WriteEndElement();

                        if (ShowPostAuthorSetting)
                        {
                            xmlTextWriter.WriteStartElement("author");
                            // techically this is supposed to be an email address
                            // but wouldn't that lead to a lot of spam?
                            xmlTextWriter.WriteString(dr["Name"].ToString());
                            xmlTextWriter.WriteEndElement();

                            
                        }

                        xmlTextWriter.WriteStartElement("comments");
                        xmlTextWriter.WriteString(blogItemUrl);
                        xmlTextWriter.WriteEndElement();

                        string signature = string.Empty;

                        if (addSignature)
                        {
							signature = String.Format("<br /><a href='{0}'>{1}</a>", imageSiteRoot, dr["Name"]);

                        }

                        if (addCommentsLink)
                        {
							signature += String.Format("&nbsp;&nbsp;<a href='{0}'>{1}...</a>", blogItemUrl, blogCommentLabel);
                        }


                        string blogPost = SiteUtils.ChangeRelativeUrlsToFullyQuailifiedUrls(navigationSiteRoot, imageSiteRoot, dr["Description"].ToString());

                        if ((!useExcerptInFeed) || (blogPost.Length <= excerptLength))
                        {
                            xmlTextWriter.WriteStartElement("description");
                            xmlTextWriter.WriteCData(blogPost + signature);
                            xmlTextWriter.WriteEndElement();
                        }
                        else
                        {
                            string excerpt = SiteUtils.ChangeRelativeUrlsToFullyQuailifiedUrls(navigationSiteRoot, imageSiteRoot,dr["Abstract"].ToString());

                            if ((excerpt.Length > 0) && (excerpt != "<p>&#160;</p>"))
                            {
								excerpt = String.Format("{0}{1} <a href='{2}'>{3}</a><div>&nbsp;</div>", excerpt, ExcerptSuffix, blogItemUrl, MoreLinkText);
                            }
                            else
                            {
								excerpt = String.Format("{0} <a href='{1}'>{2}</a><div>&nbsp;</div>", UIHelper.CreateExcerpt(dr["Description"].ToString(), excerptLength, ExcerptSuffix), blogItemUrl, MoreLinkText); ;
                            }

                            xmlTextWriter.WriteStartElement("description");
                            xmlTextWriter.WriteCData(excerpt);
                            xmlTextWriter.WriteEndElement();

                        }


                        //end blog entry
                        xmlTextWriter.WriteEndElement();

                    }
                }
            }

        }

        private string FormatBlogUrl(string itemUrl, int itemId)
        {
            if (itemUrl.Length > 0)
				return String.Format("{0}{1}", blogBaseUrl, itemUrl.Replace("~", string.Empty));

			return String.Format("{0}/Blog/ViewPost.aspx?pageid={1}&ItemID={2}&mid={3}", blogBaseUrl, pageID.ToString(CultureInfo.InvariantCulture), itemId.ToString(CultureInfo.InvariantCulture), moduleId.ToString(CultureInfo.InvariantCulture));

        }

		

		private void RenderError(string message)
		{

			Response.Write(message);
			Response.End();
		}

		

	}
}
