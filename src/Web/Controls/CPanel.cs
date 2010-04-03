
using System;
using System.Web;
using System.Web.UI.WebControls;
using Cynthia.Web.Framework;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// The purpose of this control is to try and support easy use of Artisteer html designs
    /// by adding settings that allow additional unsemantic divs to be added to support Artisteer.
    /// by default no extra markup will be applied. The idea will be to use theme.skin to enable a RenderArtisteer property to trigger the extra markup
    /// by default this control will only render its contents
    /// 
    /// </summary>
    public class CPanel : Panel
    {
        private string columnId = UIHelper.CenterColumnId;
        private string overrideTag = string.Empty;
        private string overrideCss = string.Empty;

        private bool renderArtisteer = false;
        private bool useLowerCaseArtisteerClasses = false;

        public bool RenderArtisteer
        {
            get { return renderArtisteer; }
            set { renderArtisteer = value; }
        }

        private bool renderArtisteerBlockContentDivs = false;

        public bool RenderArtisteerBlockContentDivs
        {
            get { return renderArtisteerBlockContentDivs; }
            set { renderArtisteerBlockContentDivs = value; }
        }

        public bool UseLowerCaseArtisteerClasses
        {
            get { return useLowerCaseArtisteerClasses; }
            set { useLowerCaseArtisteerClasses = value; }
        }

        private string artisteerCssClass = UIHelper.ArtisteerPost;

        public string ArtisteerCssClass
        {
            get { return artisteerCssClass; }
            set { artisteerCssClass = value; }
        }

        /// <summary>
        /// Specify a tag to render as, expected values are the new html 5 elements header footer nav article section
        /// </summary>
        public string OverrideTag
        {
            get { return overrideTag; }
            set { overrideTag = value; }
        }

        /// <summary>
        /// If you need to use different css for the overridden tag than for the non overriden div specify it here.
        /// </summary>
        public string OverrideCss
        {
            get { return overrideCss; }
            set { overrideCss = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (renderArtisteer) 
            { 
                columnId = this.GetColumnId(); 
            
                switch (columnId)
                {
                    case UIHelper.LeftColumnId:
                    case UIHelper.RightColumnId:

                        if (useLowerCaseArtisteerClasses)
                        {
                            if ((artisteerCssClass == UIHelper.ArtisteerPostLower)|(artisteerCssClass == UIHelper.ArtisteerPost))
                            {
                                artisteerCssClass = UIHelper.ArtisteerBlockLower;
                                renderArtisteerBlockContentDivs = true;
                            }

                            if ((artisteerCssClass == UIHelper.ArtisteerPostContentLower)||(artisteerCssClass == UIHelper.ArtisteerPostContent))
                            {
                                artisteerCssClass = UIHelper.ArtisteerBlockContentLower;
                                renderArtisteerBlockContentDivs = true;
                            }
                        }
                        else
                        {
                            if (artisteerCssClass == UIHelper.ArtisteerPost)
                            {
                                artisteerCssClass = UIHelper.ArtisteerBlock;
                                renderArtisteerBlockContentDivs = true;
                            }

                            if (artisteerCssClass == UIHelper.ArtisteerPostContent)
                            {
                                artisteerCssClass = UIHelper.ArtisteerBlockContent;
                                renderArtisteerBlockContentDivs = true;
                            }
                        }

                        break;

                    case UIHelper.CenterColumnId:
                    default:
                        if (useLowerCaseArtisteerClasses) 
                        {
                            if (artisteerCssClass == UIHelper.ArtisteerPost)
                            {
                                artisteerCssClass = UIHelper.ArtisteerPostLower;
                            }
                        }

                        break;

                }
            }

        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write(String.Format("[{0}]", this.ID));
                return;
            }


            if (renderArtisteer)
            {
                writer.Write(String.Format("<div class='{0}'>\n", artisteerCssClass));
                
                if (renderArtisteerBlockContentDivs)
                {
                    writer.Write(String.Format("<div class=\"{0}-tl\"></div>\n", artisteerCssClass));
                    writer.Write(String.Format("<div class=\"{0}-tr\"></div>\n", artisteerCssClass));
                    writer.Write(String.Format("<div class=\"{0}-bl\"></div>\n", artisteerCssClass));
                    writer.Write(String.Format("<div class=\"{0}-br\"></div>\n", artisteerCssClass));
                    writer.Write(String.Format("<div class=\"{0}-tc\"></div>\n", artisteerCssClass));
                    writer.Write(String.Format("<div class=\"{0}-bc\"></div>\n", artisteerCssClass));
                    writer.Write(String.Format("<div class=\"{0}-cl\"></div>\n", artisteerCssClass));
                    writer.Write(String.Format("<div class=\"{0}-cr\"></div>\n", artisteerCssClass));
                    writer.Write(String.Format("<div class=\"{0}-cc\"></div>\n", artisteerCssClass));
                    writer.Write(String.Format("<div class=\"{0}-body\">", artisteerCssClass));
                }

                base.RenderContents(writer);

                if (renderArtisteerBlockContentDivs)
                {
                    writer.Write("\n</div>");
                    writer.Write("<div class=\"cleared\"></div>");
                }
                writer.Write("\n</div>");
                return;

            }


            if (overrideCss.Length > 0) { CssClass = overrideCss; }

            if ((overrideTag.Length > 0))
            {
                writer.Write(String.Format("<{0} class='{1}'>\n", overrideTag, CssClass));

                base.RenderContents(writer);

                writer.Write(String.Format("\n</{0}'>", overrideTag));
                return;
            }

            base.RenderContents(writer);
            return;


        }


    }
}
