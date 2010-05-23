// Author:					Joe Audette
// Created:					2009-04-02
// Last Modified:			2009-08-31
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.UI.WebControls;

namespace Cynthia.Web.Editor
{
    public class CKEditorControl : TextBox
    {
        private string siteRoot = "~/";
        private string basePath = "~/ClientScript/ckeditor/";
        private string customConfigPath = "~/ClientScript/ckeditor-Cynthiaconfig.js";

        private Direction textDirection = Direction.LeftToRight;
        private ToolBar toolBar = ToolBar.AnonymousUser;
        private string editorCSSUrl = string.Empty;
        private bool fullPageMode = false;

        private string templatesJsonUrl = string.Empty;
        //private string templatesXmlUrl = string.Empty;

        private string stylesJsonUrl = string.Empty;

        // kama, v2, office2003
        private string skin = "kama";

        public string CustomConfigPath
        {
            get { return customConfigPath; }
            set { customConfigPath = value; }
        }

       
        public string BasePath
        {
            get { return basePath; }
            set { basePath = value; }
        }

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        public string StylesJsonUrl
        {
            get { return stylesJsonUrl; }
            set { stylesJsonUrl = value; }
        }

        private string templates = string.Empty;
        public string Templates
        {
            get { return templates; }
            set { templates = value; }
        }

        public string TemplatesJsonUrl
        {
            get { return templatesJsonUrl; }
            set { templatesJsonUrl = value; }
        }

        //public string TemplatesXmlUrl
        //{
        //    get { return templatesXmlUrl; }
        //    set { templatesXmlUrl = value; }
        //}

        public string Skin
        {
            get { return skin; }
            set { skin = value; }
        }

        public ToolBar ToolBar
        {
            get { return toolBar; }
            set { toolBar = value; }
        }

        public Direction TextDirection
        {
            get { return textDirection; }
            set { textDirection = value; }
        }

        public string EditorCSSUrl
        {
            get { return editorCSSUrl; }
            set { editorCSSUrl = value; }
        }

        public bool FullPageMode
        {
            get { return fullPageMode; }
            set { fullPageMode = value; }
        }

        private bool enableFileBrowser = false;

        public bool EnableFileBrowser
        {
            get { return enableFileBrowser; }
            set { enableFileBrowser = value; }
        }

        private bool forcePasteAsPlainText = false;

        public bool ForcePasteAsPlainText
        {
            get { return forcePasteAsPlainText; }
            set { forcePasteAsPlainText = value; }
        }

        private string fileManagerUrl = string.Empty;
        public string FileManagerUrl
        {
            get { return fileManagerUrl; }
            set { fileManagerUrl = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            TextMode = TextBoxMode.MultiLine;
            Rows = 10;
            Columns = 70;
            basePath = ConfigurationManager.AppSettings["CKEditor:BasePath"];
            customConfigPath = ConfigurationManager.AppSettings["CKEditor:ConfigPath"];
            if (siteRoot.StartsWith("~/"))
            {
                siteRoot = ResolveUrl(siteRoot);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupScripts();
        }

        private void SetupScripts()
        {
            this.Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(),
                "ckeditormain",
                "\n<script type=\"text/javascript\" src=\""
                + ResolveUrl(this.BasePath + "ckeditor.js") + "\"></script>");

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("var editor" + this.ClientID + " = CKEDITOR.replace('" + this.ClientID + "'");

            script.Append(", { ");

            script.Append("customConfig : '" + ResolveUrl(customConfigPath) + "' ");

            //script.Append("customConfig : '' ");

            

            script.Append(", baseHref : '" + siteRoot + "'");

            if (Height != Unit.Empty)
            {
                script.Append(", height : " + this.Height.ToString().Replace("px", string.Empty));
            }
            else
            {
                script.Append(", height : 350");
            }

            script.Append(",skin:'" + skin + "'");



            CultureInfo defaultCulture = SiteUtils.GetDefaultCulture();
            
            script.Append(",language:'" + defaultCulture.TwoLetterISOLanguageName + "'");

            if ((textDirection == Direction.RightToLeft)||(defaultCulture.TextInfo.IsRightToLeft))
            {
                script.Append(", contentsLangDirection : 'rtl'");

            }

            if (editorCSSUrl.Length > 0)
            {
                script.Append(", contentsCss : '" + editorCSSUrl + "'");
            }

            if ((enableFileBrowser) && (fileManagerUrl.Length > 0))
            {

                script.Append(",filebrowserWindowWidth : 860");
                script.Append(",filebrowserWindowHeight : 700");
                script.Append(",filebrowserBrowseUrl:'" + fileManagerUrl + "?ed=ck&type=file' ");
                script.Append(",filebrowserImageBrowseUrl:'" + fileManagerUrl + "?ed=ck&type=image' ");
                script.Append(",filebrowserFlashBrowseUrl:'" + fileManagerUrl + "?ed=ck&type=media' ");
                
            }

           // script.Append(",ignoreEmptyParagraph:true");

            if (forcePasteAsPlainText)
            {
                script.Append(",forcePasteAsPlainText:true");
            }



            //if (templatesJsonUrl.Length > 0)
            //{
            //    script.Append(",templates_files: ['" + templatesJsonUrl + "']");
            //}

            //if (templatesXmlUrl.Length > 0)
            //{
            //    script.Append(",templates_xml: '" + templatesXmlUrl + "'");
            //}

            //if (stylesJsonUrl.Length > 0)
            //{

            //    script.Append(",stylesCombo_stylesSet: ['C:" + stylesJsonUrl + "']");
            //}

            if (fullPageMode)
            {
                script.Append(",fullPage : true ");

            }

            SetupToolBar(script);
            script.Append("}");

            script.Append("); ");

            if (stylesJsonUrl.Length > 0)
            {
                script.Append("function SetupEditor" + this.ClientID + "( editorObj){");

                //if (fullPageMode)
                //{
                //    script.Append("editorObj.config.fullPage = true; ");

                //}

                //if (stylesJsonUrl.Length > 0)
                //{

                   script.Append("editorObj.config.stylesCombo_stylesSet = 'C:" + stylesJsonUrl + "';");
                //}

               if (templatesJsonUrl.Length > 0)
               {
                   script.Append("editorObj.config.templates = 'C';");
                   script.Append("editorObj.config.templates_files = ['" + templatesJsonUrl + "'];");
                   script.Append("editorObj.config.templates_replaceContent = false;");
               }

                //if ((enableFileBrowser) && (fileManagerUrl.Length > 0))
                //{
                //    script.Append("editorObj.config.filebrowserWindowWidth = 860; ");
                //    script.Append("editorObj.config.filebrowserWindowHeight = 700; ");
                //    script.Append("editorObj.config.filebrowserBrowseUrl = '" + fileManagerUrl + "?ed=ck&type=file'; ");
                //    script.Append("editorObj.config.filebrowserImageBrowseUrl = '" + fileManagerUrl + "?ed=ck&type=image'; ");
                //    script.Append("editorObj.config.filebrowserFlashBrowseUrl = '" + fileManagerUrl + "?ed=ck&type=media' ");

                //}

                script.Append("}");

                script.Append("SetupEditor" + this.ClientID + "(editor" + this.ClientID + ");");
            }

            script.Append("</script>");

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                this.UniqueID,
                script.ToString());


        }

        private void SetupToolBar(StringBuilder script)
        {

            //'basicstyles,blockquote,button,clipboard,colorbutton,contextmenu,elementspath,enterkey,entities,find,flash,font,format,forms,horizontalrule,
            //htmldataprocessor,image,indent,justify,keystrokes,link,list,newpage,pagebreak,pastefromword,pastetext,preview,print,removeformat,save,smiley,showblocks,
            //sourcearea,stylescombo,table,specialchar,tab,templates,toolbar,undo,wysiwygarea,wsc';

            switch (toolBar)
            {
                case ToolBar.FullWithTemplates:

                    script.Append(",toolbar:'FullWithTemplates'");

                    
                    //script.Append(", plugins : '");


                    //script.Append("blockquote");

                    //script.Append(",filebrowser");

                    //script.Append(",button");
                    //script.Append(",clipboard");

                    ////hard coded styles
                    ////script.Append(",colorbutton");
                    ////script.Append(",font");

                    //script.Append(",contextmenu");
                    //script.Append(",elementspath");
                    //script.Append(",enterkey");
                    ////script.Append(",entities");
                    //script.Append(",find");
                    //script.Append(",flash");

                    //script.Append(",format");

                    //script.Append(",forms");
                    //script.Append(",horizontalrule");
                    //script.Append(",htmldataprocessor");
                    //script.Append(",image");
                    //script.Append(",indent");
                    //script.Append(",justify");
                    //script.Append(",keystrokes");
                    //script.Append(",link");
                   
                    //script.Append(",list");
                    ////script.Append(",newpage");
                    ////script.Append(",pagebreak");
                    //script.Append(",pastefromword");
                    //script.Append(",pastetext");
                    ////script.Append(",preview");
                    //script.Append(",print");
                    //script.Append(",removeformat");
                    ////script.Append(",save");
                    //script.Append(",smiley");
                    //script.Append(",showblocks");
                    //script.Append(",sourcearea");

                    //script.Append(",stylescombo");
                    ////script.Append(",basicstyles");
                    //script.Append(",maximize");

                    //script.Append(",table");
                    //script.Append(",specialchar");
                    //script.Append(",tab");
                    //script.Append(",templates");
                    //script.Append(",toolbar");
                    //script.Append(",undo");
                    //script.Append(",wysiwygarea");
                    //script.Append(",wsc");

                    //script.Append("'");

                    break;

                case ToolBar.Full:

                    script.Append(",toolbar:'Full'");

                    //script.Append(", plugins : '");

                    
                    //script.Append("blockquote");
                    //script.Append(",button");
                    //script.Append(",clipboard");

                    ////hard coded styles
                    ////script.Append(",colorbutton");
                    ////script.Append(",font");

                    //script.Append(",contextmenu");
                    //script.Append(",elementspath");
                    //script.Append(",enterkey");
                    ////script.Append(",entities");
                    //script.Append(",find");
                    //script.Append(",flash");
                    
                    //script.Append(",format");
                    ////script.Append(",forms");
                    //script.Append(",horizontalrule");
                    //script.Append(",htmldataprocessor");
                    //script.Append(",image");
                    //script.Append(",indent");
                    //script.Append(",justify");
                    //script.Append(",keystrokes");
                    //script.Append(",link");
                    //script.Append(",list");
                    ////script.Append(",newpage");
                    ////script.Append(",pagebreak");
                    //script.Append(",pastefromword");
                    //script.Append(",pastetext");
                    ////script.Append(",preview");
                    //script.Append(",print");
                    //script.Append(",removeformat");
                    ////script.Append(",save");
                    //script.Append(",smiley");
                    //script.Append(",showblocks");
                    //script.Append(",sourcearea");

                    ////script.Append(",basicstyles");
                    //script.Append(",stylescombo");
                    ////script.Append(",templates");

                    //script.Append(",maximize");

                    //script.Append(",table");
                    //script.Append(",specialchar");
                    //script.Append(",tab");
                   
                    //script.Append(",toolbar");
                    //script.Append(",undo");
                    //script.Append(",wysiwygarea");
                    //script.Append(",wsc");


                    //script.Append("'");

                    break;

                case ToolBar.Newsletter:

                    script.Append(",toolbar:'Newsletter'");

                    break;

                case ToolBar.Group:

                    script.Append(",toolbar:'Group'");

                    //script.Append(", plugins : '");

                    //script.Append("basicstyles");
                    //script.Append(",blockquote");
                    //script.Append(",button");
                    //script.Append(",clipboard");
                    ////script.Append(",colorbutton");
                    //script.Append(",contextmenu");
                    //script.Append(",elementspath");
                    //script.Append(",enterkey");
                    ////script.Append(",entities");
                    ////script.Append(",find");
                    ////script.Append(",flash");
                    ////script.Append(",font");
                    ////script.Append(",format");
                    ////script.Append(",forms");
                    ////script.Append(",horizontalrule");
                    //script.Append(",htmldataprocessor");
                    ////script.Append(",image");
                    //script.Append(",indent");
                    //script.Append(",justify");
                    //script.Append(",keystrokes");
                    //script.Append(",link");
                    //script.Append(",list");
                    ////script.Append(",newpage");
                    ////script.Append(",pagebreak");
                    ////script.Append(",pastefromword");
                    //script.Append(",pastetext");
                    ////script.Append(",preview");
                    ////script.Append(",print");
                    //script.Append(",removeformat");
                    ////script.Append(",save");
                    //script.Append(",smiley");
                    ////script.Append(",showblocks");
                    ////script.Append(",sourcearea");
                    ////script.Append(",stylescombo");
                    ////script.Append(",table");
                    //script.Append(",specialchar");
                    ////script.Append(",tab");
                    ////script.Append(",templates");
                    //script.Append(",toolbar");
                    //script.Append(",undo");
                    //script.Append(",wysiwygarea");
                    //script.Append(",wsc");

                    //script.Append("'");

                    break;

                case ToolBar.GroupWithImages:

                    script.Append(",toolbar:'GroupWithImages'");

                    //script.Append(", plugins : '");

                    //script.Append("basicstyles");
                    //script.Append(",blockquote");
                    //script.Append(",button");
                    //script.Append(",clipboard");
                    ////script.Append(",colorbutton");
                    //script.Append(",contextmenu");
                    //script.Append(",elementspath");
                    //script.Append(",enterkey");
                    ////script.Append(",entities");
                    ////script.Append(",find");
                    ////script.Append(",flash");
                    ////script.Append(",font");
                    ////script.Append(",format");
                    ////script.Append(",forms");
                    ////script.Append(",horizontalrule");
                    //script.Append(",htmldataprocessor");
                    ////script.Append(",image");
                    //script.Append(",indent");
                    //script.Append(",justify");
                    //script.Append(",keystrokes");
                    //script.Append(",link");
                    //script.Append(",list");
                    ////script.Append(",newpage");
                    ////script.Append(",pagebreak");
                    ////script.Append(",pastefromword");
                    //script.Append(",pastetext");
                    ////script.Append(",preview");
                    ////script.Append(",print");
                    //script.Append(",removeformat");
                    ////script.Append(",save");
                    //script.Append(",smiley");
                    ////script.Append(",showblocks");
                    ////script.Append(",sourcearea");
                    ////script.Append(",stylescombo");
                    ////script.Append(",table");
                    //script.Append(",specialchar");
                    ////script.Append(",tab");
                    ////script.Append(",templates");
                    //script.Append(",toolbar");
                    //script.Append(",undo");
                    //script.Append(",wysiwygarea");
                    //script.Append(",wsc");

                    //script.Append("'");

                    break;

                

                case ToolBar.SimpleWithSource:

                    script.Append(",toolbar:'SimpleWithSource'");

                    //script.Append(", plugins : '");

                    //script.Append("basicstyles");
                    //script.Append(",blockquote");
                    //script.Append(",button");
                    //script.Append(",clipboard");
                    ////script.Append(",colorbutton");
                    //script.Append(",contextmenu");
                    //script.Append(",elementspath");
                    //script.Append(",enterkey");
                    ////script.Append(",entities");
                    ////script.Append(",find");
                    ////script.Append(",flash");
                    ////script.Append(",font");
                    ////script.Append(",format");
                    ////script.Append(",forms");
                    ////script.Append(",horizontalrule");
                    //script.Append(",htmldataprocessor");
                    ////script.Append(",image");
                    //script.Append(",indent");
                    //script.Append(",justify");
                    //script.Append(",keystrokes");
                    //script.Append(",link");
                    //script.Append(",list");
                    ////script.Append(",newpage");
                    ////script.Append(",pagebreak");
                    //script.Append(",pastefromword");
                    //script.Append(",pastetext");
                    ////script.Append(",preview");
                    ////script.Append(",print");
                    //script.Append(",removeformat");
                    ////script.Append(",save");
                    //script.Append(",smiley");
                    //script.Append(",showblocks");
                    //script.Append(",sourcearea");
                    ////script.Append(",stylescombo");
                    ////script.Append(",table");
                    //script.Append(",specialchar");
                    ////script.Append(",tab");
                    ////script.Append(",templates");
                    //script.Append(",toolbar");
                    //script.Append(",undo");
                    //script.Append(",wysiwygarea");
                    //script.Append(",wsc");

                    //script.Append("'");

                    break;

                case ToolBar.AnonymousUser:
                default:

                    script.Append(",toolbar:'AnonymousUser'");
                    //script.Append(", plugins : '");

                    //script.Append("basicstyles");
                    //script.Append(",blockquote");
                    ////script.Append(",button");
                    ////script.Append(",clipboard");
                    ////script.Append(",colorbutton");

                    //// this seems to be required
                    //script.Append(",contextmenu");

                    //script.Append(",elementspath");
                    //script.Append(",enterkey");
                    ////script.Append(",entities");
                    ////script.Append(",find");
                    ////script.Append(",flash");
                    ////script.Append(",font");
                    ////script.Append(",format");
                    ////script.Append(",forms");
                    ////script.Append(",horizontalrule");
                    ////script.Append(",htmldataprocessor");
                    ////script.Append(",image");
                    ////script.Append(",indent");
                    ////script.Append(",justify");
                    ////script.Append(",keystrokes");
                    //script.Append(",link");
                    //script.Append(",list");
                    ////script.Append(",newpage");
                    ////script.Append(",pagebreak");
                    ////script.Append(",pastefromword");
                    //script.Append(",pastetext");
                    ////script.Append(",preview");
                    ////script.Append(",print");
                    ////script.Append(",removeformat");
                    ////script.Append(",save");
                    //script.Append(",smiley");
                    ////script.Append(",showblocks");
                    ////script.Append(",sourcearea");
                    ////script.Append(",stylescombo");
                    ////script.Append(",table");
                    ////script.Append(",specialchar");
                    ////script.Append(",tab");
                    ////script.Append(",templates");
                    //script.Append(",toolbar");
                    ////script.Append(",undo");
                    //script.Append(",wysiwygarea");
                    //script.Append(",wsc");

                    //script.Append("'");

                    break;

            }

        }



    }
}
