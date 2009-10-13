﻿using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.ComponentModel;
using Vivasky.Core;
using CoyoEden.Core.Web.Extensions;
using CoyoEden.Core.Infrastructure;

namespace CoyoEden.Core
{
    ///<summary>
    ///</summary>
    public static class CommentHandlers
    {
        static ExtensionSettings _filters;
        static ExtensionSettings _customFilters;

        ///<summary>
        /// Initiate adding comment event listener
        ///</summary>
        public static void Listen()
        {
            Post.AddingComment += PostAddingComment;
            InitFilters();
            InitCustomFilters();
        }

        #region Event handlers

        static void PostAddingComment(object sender, CancelEventArgs e)
        {
            if(!BlogSettings.Instance.IsCommentEnabled) return;
            if(!BlogSettings.Instance.EnableCommentsModeration) return;
            // auto-moderation == 1
            if(BlogSettings.Instance.ModerationType < 1) return;

            var comment = (IComment)sender;
            comment.IsApproved = true;
            comment.ModeratedBy = "Auto";

            if(!ModeratedByRule(comment))
            {
                if(!ModeratedByFilter(comment))
                {
                    RunCustomModerators(comment);
                }
            }
        }

        #endregion

        static bool ModeratedByRule(IComment comment)
        {
            // trust authenticated users
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                comment.IsApproved = true;
                comment.ModeratedBy = "Rule:authenticated";
                return true;
            }

            int blackCnt = 0;
            int whiteCnt = 0;

            foreach (Post p in Post.Posts)
            {
                foreach (var c in p.Comments)
                {
                    if (c.Author == comment.Author)
                    {
                        if (c.IsApproved.Value)
                            whiteCnt++;
                        else
                            blackCnt++;
                    }
                }
            }

            // user is in the white list - approve comment
            if (whiteCnt >= BlogSettings.Instance.CommentWhiteListCount)
            {
                comment.IsApproved = true;
                comment.ModeratedBy = "Rule:white list";
                return true;
            }

            // user is in the black list - reject comment
            if (blackCnt >= BlogSettings.Instance.CommentBlackListCount)
            {
                comment.IsApproved = false;
                comment.ModeratedBy = "Rule:black list";
                return true;
            }
            return false;
        }

        static bool ModeratedByFilter(IComment comment)
        {
            DataTable dt = _filters.GetDataTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string action = row["Action"].ToString();
                    string subject = row["Subject"].ToString();
                    string oper = row["Operator"].ToString();
                    string filter = row["Filter"].ToString().Trim().ToLower(CultureInfo.InvariantCulture);

                    string comm = comment.Content.ToLower(CultureInfo.InvariantCulture);
                    string auth = comment.Author.ToLower(CultureInfo.InvariantCulture);

                    string wsite = string.Empty;
                    if(comment.Website != null)
                        wsite = comment.Website.ToString().ToLower(CultureInfo.InvariantCulture);
                    
                    string email = comment.Email.ToLower(CultureInfo.InvariantCulture);

                    bool match = false;

                    if (oper == "Equals")
                    {
                        if (subject == "IP")
                        {
                            if (comment.Ip == filter)
                                match = true;
                        }
                        if (subject == "Author")
                        {
                            if (auth == filter)
                                match = true;
                        }
                        if (subject == "Website")
                        {
                            if (wsite == filter)
                                match = true;
                        }
                        if (subject == "Email")
                        {
                            if (email == filter)
                                match = true;
                        }
                        if (subject == "Comment")
                        {
                            if (comm == filter)
                                match = true;
                        }
                    }
                    else
                    {
                        if (subject == "IP")
                        {
                            if (comment.Ip.Contains(filter))
                                match = true;
                        }
                        if (subject == "Author")
                        {
                            if (auth.Contains(filter))
                                match = true;
                        }
                        if (subject == "Website")
                        {
                            if (wsite.Contains(filter))
                                match = true;
                        }
                        if (subject == "Email")
                        {
                            if (email.Contains(filter))
                                match = true;
                        }
                        if (subject == "Comment")
                        {
                            if (comm.Contains(filter))
                                match = true;
                        }
                    }

                    if (match)
                    {
                        comment.IsApproved = action != "Block";
                        comment.ModeratedBy = "Filter";
                        return true;
                    }
                }
            }
            return false;
        }

        static void RunCustomModerators(IComment comment)
        {
            DataTable dt = _customFilters.GetDataTable();

            foreach (DataRow row in dt.Rows)
            {
                string fileterName = row[0].ToString();

                ICustomFilter customFilter = GetCustomFilter(fileterName);             

                if (customFilter != null && customFilter.Initialize())
                {
                    comment.IsApproved = !customFilter.Check(comment);
                    comment.ModeratedBy = fileterName;

                    UpdateCustomFilter(fileterName, comment.IsApproved.Value);
                }
            }
        }

        #region Settings

        static void InitFilters()
        {
            ExtensionSettings settings = new ExtensionSettings("BeCommentFilters");

            settings.AddParameter("ID", "ID", 20, true, true, ParameterType.Integer);
            settings.AddParameter("Action");
            settings.AddParameter("Subject");
            settings.AddParameter("Operator");
            settings.AddParameter("Filter");

            _filters = ExtensionManager.InitSettings("MetaExtension", settings);
        }

        static void InitCustomFilters()
        {
            ExtensionSettings settings = new ExtensionSettings("BeCustomFilters");

            settings.AddParameter("FullName", "Name", 100, true, true);
            settings.AddParameter("Name");
            settings.AddParameter("Checked");
            settings.AddParameter("Cought");
            settings.AddParameter("Reported");

            _customFilters = ExtensionManager.InitSettings("MetaExtension", settings);

            if(_customFilters != null)
            {
                DataTable dt = _customFilters.GetDataTable();
                ArrayList codeAssemblies = Utils.CodeAssemblies("CoyoEdenExtension");

                foreach (Assembly a in codeAssemblies)
                {
                    Type[] types = a.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.GetInterface("CoyoEden.Core.ICustomFilter") != null)
                        {
                            bool found = false;
                            foreach (DataRow row in dt.Rows)
                            {
                                if(row[0].ToString() == type.Name)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if(!found)
                            {
                                _customFilters.AddValues(new string[] { type.FullName, type.Name, "0", "0", "0" });
                            }
                        }
                    }
                }

                ExtensionManager.SaveSettings(_customFilters);
            }
        }

        static void UpdateCustomFilter(string filter, bool approved)
        {
            DataTable dt = _customFilters.GetDataTable();
            int i = 0;

            foreach (DataRow row in dt.Rows)
            {
                string fileterName = row[0].ToString();

                if (fileterName == filter)
                {
                    int total = int.Parse(_customFilters.Parameters[2].Values[i]);
                    int spam = int.Parse(_customFilters.Parameters[3].Values[i]);

                    _customFilters.Parameters[2].Values[i] = (total + 1).ToString();
                    if (!approved) _customFilters.Parameters[3].Values[i] = (spam + 1).ToString();

                    break;
                }
                i++;
            }
        }

        #endregion

        /// <summary>
        /// Instantiates custom filter object
        /// </summary>
        /// <param name="className">Name of the class to instantiate</param>
        /// <returns>Object as ICustomFilter</returns>
        public static ICustomFilter GetCustomFilter(string className)
        {
            try
            {
                ArrayList codeAssemblies = Utils.CodeAssemblies("CoyoEdenExtension");
                foreach (Assembly a in codeAssemblies)
                {
                    Type t = a.GetType(className);
                    if(t != null) return (ICustomFilter)Activator.CreateInstance(t);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}