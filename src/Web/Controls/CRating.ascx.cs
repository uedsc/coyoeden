// Author:					Joe Audette
// Created:				    2008-10-05
// Last Modified:		    2010-03-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Resources;
using AjaxControlToolkit;

namespace Cynthia.Web.UI
{
   
    public partial class CRating : UserControl
    {
        #region Properties

        private bool enabled = true;
        private bool allowAnonymousRating = true;
        private bool allowFeedback = false;
        private int anonymousIpVoteWindowMinutes = 15;
        private ScriptManager scriptController = null;
        private bool showPrompt = true;
        private bool showRatingCount = true;
        private string commentLabel = string.Empty;
        private string promptText = string.Empty;
        protected string jQueryBasePath = WebConfigSettings.jQueryBasePath;
        private string jsonServiceUrl = string.Empty;
        
        public Guid ContentGuid
        {
            get 
            {
                
                if (hdnContentGuid.Value.Length == 36) { return new Guid(hdnContentGuid.Value); }
                if (ViewState["ContentGuid"] != null)
                {
                    return new Guid(ViewState["ContentGuid"].ToString());
                }
                return Guid.Empty ; 
            }
            set 
            {
                ViewState["ContentGuid"] = value; 
                hdnContentGuid.Value = value.ToString();
            }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool ShowPrompt
        {
            get { return showPrompt; }
            set { showPrompt = value; }
        }

        public bool ShowRatingCount
        {
            get { return showRatingCount; }
            set { showRatingCount = value; }
        }

        public bool AllowAnonymousRating
        {
            get { return allowAnonymousRating; }
            set { allowAnonymousRating = value; }
        }

        public bool AllowFeedback
        {
            get { return allowFeedback; }
            set { allowFeedback = value; }
        }

        public int AnonymousIpVoteWindowMinutes
        {
            get { return anonymousIpVoteWindowMinutes; }
            set { anonymousIpVoteWindowMinutes = value; }
        }

        protected ScriptManager ScriptController
        {
            get
            {
                if (scriptController == null)
                {
                    scriptController = (ScriptManager)Page.Master.FindControl("ScriptManager1");
                }

                return scriptController;

            }
        }

        public string CommentLabel
        {
            get { return commentLabel; }
            set { commentLabel = value; }
        }

        public string PromptText
        {
            get { return promptText; }
            set { promptText = value; }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!enabled)
            {
                this.Visible = false;
                return;
            }

            if (WebConfigSettings.DisablejQuery)
            {
                this.Visible = false;
                return;
            }

            if (ContentGuid == Guid.Empty)
            {
                this.Visible = false;
                return;
            }

            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            jsonServiceUrl = SiteUtils.GetNavigationSiteRoot() + "/Services/ContentRatingService.ashx";
            litPrompt.Text = Resource.RatingPrompt;
            spnTotal.Visible = showRatingCount;
            spnPrompt.Visible = showPrompt;
            UserRating.AutoPostBack = false;
            UserRating.BehaviorID = UserRating.ClientID + "Behavior";
           
            pnlComments.Visible = allowFeedback;

            PopulateLabels();
            
            SetupCommentScript();

            if (!IsPostBack)
            {
                BindRating();
            }

        }

        private void BindRating()
        {
            ContentRatingStats ratingStats = ContentRating.GetStats(ContentGuid);
            UserRating.CurrentRating = ratingStats.AverageRating;
            UserRating.JsonUrl = jsonServiceUrl;
            UserRating.ContentId = ContentGuid.ToString();
            UserRating.TotalVotesElementId = lblRatingVotes.ClientID;
            UserRating.CommentsEnabled = allowFeedback;
            
            litRating.Text = ratingStats.AverageRating.ToString(CultureInfo.InvariantCulture);
            lblRatingVotes.Text = string.Format(CultureInfo.InvariantCulture,
                Resource.RatingCountFormat,
                ratingStats.TotalVotes);

            if (WebUser.IsAdminOrContentAdmin)
            {
                lnkRatingsReview.Text = lblRatingVotes.Text;
                UserRating.TotalVotesElementId = lnkRatingsReview.ClientID;
                lblRatingVotes.Visible = false;
                lnkRatingsReview.Visible = true;
                lnkRatingsReview.NavigateUrl = SiteUtils.GetNavigationSiteRoot() + "/Dialog/ContentRatingsDialog.aspx?c=" + ContentGuid.ToString();
                lnkRatingsReview.ToolTip = Resource.RatingReviewsLink;
                lnkRatingsReview.DialogCloseText = Resource.CloseDialogButton;
            }

        }

        ///// <summary>
        ///// this is not really used since we are providing a json service url
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void UserRating_Changed(object sender, AjaxRatingEventArgs e)
        //{
        //    if (!allowFeedback)
        //    {
        //        DoRating(Convert.ToInt32(e.Value));
        //    }
        //    else
        //    {
        //        BindRating();

        //        upRating.Update();
        //    }

        //}

        /// <summary>
        /// this is only used when comments are enabled
        /// </summary>
        /// <param name="rating"></param>
        private void DoRating(int rating)
        {
            if (ContentGuid == Guid.Empty) { return; }
            if (rating <= 0) 
            {
                BindRating();
                upRating.Update();
                return; 
            }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            Guid userGuid = Guid.Empty;
            string emailAddress = txtEmail.Text;
            
            if (Request.IsAuthenticated)
            {
                SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser != null)
                {
                    userGuid = currentUser.UserGuid;
                    if (emailAddress.Length == 0) { emailAddress = currentUser.Email; }
                }
            }

            ContentRating.RateContent(
                siteSettings.SiteGuid,
                ContentGuid,
                userGuid,
                rating,
                emailAddress,
                txtComments.Text,
                SiteUtils.GetIP4Address(),
                WebConfigSettings.MinutesBetweenAnonymousRatings
                );

            hdnRating.Value = string.Empty;
            txtComments.Text = string.Empty;
            txtEmail.Text = string.Empty;

            BindRating();

            upRating.Update();

        }

        void btnComments_Click(object sender, EventArgs e)
        {

            Button btnClicked = sender as Button;
            if (btnClicked != null)
            {
                if (btnClicked.CommandArgument == "NoThanks") 
                {
                    BindRating();
                    upRating.Update();
                    return; 
                }
            }
            
           
            if (hdnRating.Value.Length > 0)
            {
                int rate = -1;
                Int32.TryParse(hdnRating.Value, out rate);
                if (rate > -1)
                {
                    DoRating(rate);
                   
                }
            }
            else
            {
                DoRating(UserRating.CurrentRating);
            }

        }


        private void SetupCommentScript()
        {

            if (!allowFeedback) { return; }

            //setup main scrip functions
            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\">\n");

            script.Append("function CRefreshRating(commentPanel, ratingPanel, ratingBehavior, hiddenRating)");
            script.Append("{");
            script.Append("$(commentPanel).hide();");

            script.Append("$(ratingPanel).click(function() {");

            script.Append("$(commentPanel).animate({ width: 'show', height: 'show', opacity: 'toggle' }, 700);");

            script.Append("var ratingValue = $find(ratingBehavior).get_Rating();");
            script.Append("$(hiddenRating).val(ratingValue);");

            script.Append("return false;");
            script.Append("});");

            script.Append("$(commentPanel).hover(function() {");

            script.Append("try{");
            script.Append("$find(commentPanel).animate({ width: 'show', height: 'show', opacity: 'toggle' }, 700);");
            script.Append("}catch(err){}");
            script.Append("},");
            script.Append("function() {");
            script.Append("try{");
            script.Append("$find(commentPanel).animate({ width: 'hide', height: 'hide', opacity: 'toggle' }, 700);");
            script.Append("}catch(err){}");
            script.Append("});");
            script.Append("}");


            script.Append("function CCloseRatingPanel(commentPanel){");
            script.Append("$(commentPanel).hide();");

            script.Append("}");

            //script.Append("function CDoRating(cid,rating, email, comments){");
            //script.Append("var ratingData = 'cid=' + cid + '&r=' + rating + '&email=' + email + '&comments=' + comments;");
            //script.Append("$.ajax({");
            //script.Append("type: \"POST\",");
            //script.Append("async: false,");
            //script.Append("processData: false,");
            //script.Append("url: '" + jsonServiceUrl + "',");

            //jsonServiceUrl

           // script.Append("}");
            

           

            script.Append("\n</script>");


            Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "Crating",
                script.ToString());

            //create instance script
            
            script = new StringBuilder();
            script.Append("<script type=\"text/javascript\">\n");

            script.Append("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function(){");
            script.Append("CCloseRatingPanel($('#" + pnlComments.ClientID + "'));");
            script.Append("});");

            script.Append("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function(){");
            script.Append("CRefreshRating($('#" + pnlComments.ClientID + "'),$('#" + UserRating.ClientID + "'),'" + UserRating.ClientID + "Behavior',$('#" + hdnRating.ClientID + "'));");
            script.Append("});");

            //script.Append("$(document).ready(CCloseRatingPanel($('#" + pnlComments.ClientID + "')));");

            script.Append("$(document).ready(CRefreshRating($('#" + pnlComments.ClientID + "'),$('#" + UserRating.ClientID + "'),'" + UserRating.ClientID + "Behavior',$('#" + hdnRating.ClientID + "')));");

            script.Append("\n</script>");

            Page.ClientScript.RegisterStartupScript(
                typeof(Page),
                this.UniqueID,
                script.ToString());
            

            
            
            


            

        }

        private void PopulateLabels()
        {
            if (commentLabel.Length > 0)
            {
                lblComments.Text = commentLabel;
            }
            else
            {
                lblComments.Text = Resource.RatingCommentsLabel;
            }

            if (promptText.Length > 0)
            {
                litPrompt.Text = promptText;
            }
            else
            {
                litPrompt.Text = Resource.RatingPrompt;
            }

            lblEmail.Text = Resource.RatingEmailLabel;
            btnComments.Text = Resource.RatingSubmitButton;
            btnNoThanks.Text = Resource.RatingNoThanksButton;


            
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.EnableViewState = true;
            this.Load += new EventHandler(Page_Load);

            // workaround for Mono bugs
            if (btnComments == null){ btnComments = (CButton)upRating.FindControl("btnComments"); }
            if (btnNoThanks == null) { btnNoThanks = (CButton)upRating.FindControl("btnNoThanks"); }
            if (btnComments == null) { return; }
            if (btnNoThanks == null) { return; }


            btnComments.Click += new EventHandler(btnComments_Click);
            btnNoThanks.Click += new EventHandler(btnComments_Click);
              
        }

        

        
    }
}