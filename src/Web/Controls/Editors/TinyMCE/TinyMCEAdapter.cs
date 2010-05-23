// Author:		        Joe Audette
// Created:            2007-05-29
// Last Modified:      2009-11-06
//
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.Framework;

namespace Cynthia.Web.Editor
{
    public class TinyMCEAdapter : IWebEditor
    {
        #region Constructors

        public TinyMCEAdapter()
        {
            InitializeEditor();
        }

        #endregion

        #region Private Properties

        private TinyMCE Editor = new TinyMCE();
        private string scriptBaseUrl = string.Empty;
        private string siteRoot = string.Empty;
        private string skinName = string.Empty;
        private Unit editorWidth = Unit.Percentage(98);
        private Unit editorHeight = Unit.Pixel(350);
        private string editorCSSUrl = string.Empty;
        private Direction textDirection = Direction.LeftToRight;
        private ToolBar toolBar = ToolBar.AnonymousUser;
        private bool setFocusOnStart = false;
        private bool fullPageMode = false;
        private bool useFullyQualifiedUrlsForResources = false;
        private TinyMceConfiguration config = null;
        
        #endregion

        #region Public Properties

        public string ControlID
        {
            get
            {
                return Editor.ID;
            }
            set
            {
                Editor.ID = value;
            }
        }

        public string ClientID
        {
            get
            {
                return Editor.ClientID;
            }

        }

        public string Text
        {
            get { return Editor.Text; }
            set { Editor.Text = value; }
        }

        public string ScriptBaseUrl
        {
            get
            {
                return scriptBaseUrl;
            }
            set
            {
                scriptBaseUrl = value;
                Editor.BasePath = scriptBaseUrl + "/tiny_mce/";
                if (ConfigurationManager.AppSettings["TinyMCE:BasePath"] != null)
                {
                    Editor.BasePath = ConfigurationManager.AppSettings["TinyMCE:BasePath"];
                }

                //Editor.SkinPath = virtualRoot + "/FCKeditor/editor/skins/normal/";
            }
        }

        public string SiteRoot
        {
            get
            {
                return siteRoot;
            }
            set
            {
                siteRoot = value;
                //Editor.ImageBrowserURL = siteRoot + "/FCKeditor/editor/filemanager/browser/default/browser.html?Type=Image&Connector=connectors/aspx/connector.aspx";
                //Editor.LinkBrowserURL = siteRoot + "/FCKeditor/editor/filemanager/browser/default/browser.html?Connector=connectors/aspx/connector.aspx";

            }
        }

        public string SkinName
        {
            get { return skinName; }
            set { skinName = value; }
        }

        public string EditorCSSUrl
        {
            get
            {
                return editorCSSUrl;
            }
            set
            {
                editorCSSUrl = value;
                if (editorCSSUrl.Length > 0)
                {
                    Editor.EditorAreaCSS = editorCSSUrl;
                }
            }
        }

        public Unit Width
        {
            get
            {
                return editorWidth;
            }
            set
            {
                editorWidth = value;
                Editor.Width = editorWidth;
            }
        }

        public Unit Height
        {
            get
            {
                return editorHeight;
            }
            set
            {
                editorHeight = value;
                Editor.Height = editorHeight;
                
            }
        }

        public Direction TextDirection
        {
            get
            {
                return textDirection;
            }
            set
            {
                textDirection = value;
                if (value == Direction.RightToLeft)
                {
                    Editor.TextDirection = "rtl";
                }
                
            }
        }

        public ToolBar ToolBar
        {
            get
            {
                return toolBar;
            }
            set
            {
                toolBar = value;
                SetToolBar();
            }
        }

        public bool SetFocusOnStart
        {
            get { return setFocusOnStart; }
            set
            {
                setFocusOnStart = value;
                Editor.AutoFocus = setFocusOnStart;
            }
        }

        public bool FullPageMode
        {
            get { return fullPageMode; }
            set
            {
                fullPageMode = value;
               
            }
        }

        public bool UseFullyQualifiedUrlsForResources
        {
            get { return useFullyQualifiedUrlsForResources; }
            set
            {
                useFullyQualifiedUrlsForResources = value;

            }
        }

        #endregion

        #region Private Methods

        

        private void InitializeEditor()
        {
            
            //this is true because we are using xhtml
            //Editor.UseStrictLoadingMode = true;

            config = TinyMceConfiguration.GetConfig();

            Editor.AdvancedBlockFormats = config.AdvancedFormatBlocks;

            Editor.AdvancedStyles = SiteUtils.BuildStylesListForTinyMce();
            Editor.TemplatesUrl = SiteUtils.GetNavigationSiteRoot() + "/Services/TinyMceTemplates.ashx?cb=" + Guid.NewGuid().ToString(); //cache busting guid
            
        
            Editor.DialogType = config.DialogType;
            
            Editor.Height = editorHeight;
            Editor.Width = editorWidth;

            Editor.AdvancedSourceEditorWidth = config.AdvancedSourceEditorWidth;
            Editor.AdvancedSourceEditorHeight = config.AdvancedSourceEditorHeight;
            Editor.AdvancedToolbarLocation = config.AdvancedToolbarLocation;
            Editor.AdvancedToolbarAlign = config.AdvancedToolbarAlign;
            Editor.AdvancedStatusBarLocation = config.AdvancedStatusBarLocation;
            Editor.SpellCheckerLanguages = config.SpellCheckerLanguages;
            
            if (setFocusOnStart)
            {
                Editor.AutoFocus = true;
            }

            
            Editor.BasePath = WebConfigSettings.TinyMceBasePath;
            Editor.Skin = WebConfigSettings.TinyMceSkin;

            //Editor.ForcedRootBlock = string.Empty;

            SetToolBar();
        }

        private void SetToolBar()
        {
            /*
             http://wiki.moxiecode.com/index.php/TinyMCE:Control_reference
             */

            switch (toolBar)
            {
                case ToolBar.Full:

                    //Editor.Plugins = "media,template,paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups,spellchecker,wordcount,safari";

                    //Editor.AdvancedRow1Buttons = "code,separator,selectall,removeformat,cut,copy,separator,paste,pastetext,pasteword,separator,print,separator,undo,redo,separator,search,replace";

                    //Editor.AdvancedRow2Buttons = "blockquote,bold,italic,underline,strikethrough,separator,sub,sup,separator,bullist,numlist,separator,outdent,indent,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,link,unlink,anchor,image,media,table,hr,emotions,charmap";

                    //Editor.AdvancedRow3Buttons = "template,formatselect,styleselect,separator,cleanup,fullscreen,spellchecker";

                    Editor.Plugins = config.FullToolbarPlugins;
                    Editor.AdvancedRow1Buttons = config.FullToolbarRow1Buttons;
                    Editor.AdvancedRow2Buttons = config.FullToolbarRow2Buttons;
                    Editor.AdvancedRow3Buttons = config.FullToolbarRow3Buttons;
                    Editor.ExtendedValidElements = config.FullToolbarExtendedValidElements;

                    string siteRoot = SiteUtils.GetNavigationSiteRoot();
                    Editor.FileManagerUrl = siteRoot + "/Dialog/FileDialog.aspx";
                    Editor.EnableFileBrowser = true;

                    Editor.SpellCheckerUrl = siteRoot + "/TinyMCEHandler.ashx?rp=spellchecker";
                    Editor.ForcePasteAsPlainText = config.FullToolbarForcePasteAsPlainText;
                    
                    break;

                case ToolBar.FullWithTemplates:

                    //Editor.Plugins = "media,template,paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups,spellchecker,wordcount,safari";

                    //Editor.AdvancedRow1Buttons = "code,separator,selectall,removeformat,cleanup,cut,copy,separator,paste,pastetext,pasteword,separator,print,separator,undo,redo,separator,search,replace";

                    //Editor.AdvancedRow2Buttons = "blockquote,bold,italic,underline,strikethrough,separator,sub,sup,separator,bullist,numlist,separator,outdent,indent,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,link,unlink,anchor,image,media,table,hr,emotions,charmap";

                    //Editor.AdvancedRow3Buttons = "template,formatselect,styleselect,separator,cleanup,fullscreen,spellchecker";

                    Editor.Plugins = config.FullWithTemplatesToolbarPlugins;
                    Editor.AdvancedRow1Buttons = config.FullWithTemplatesToolbarRow1Buttons;
                    Editor.AdvancedRow2Buttons = config.FullWithTemplatesToolbarRow2Buttons;
                    Editor.AdvancedRow3Buttons = config.FullWithTemplatesToolbarRow3Buttons;
                    Editor.ExtendedValidElements = config.FullWithTemplatesToolbarExtendedValidElements;

                    string sRoot = SiteUtils.GetNavigationSiteRoot();
                    Editor.FileManagerUrl = sRoot + "/Dialog/FileDialog.aspx";
                    Editor.EnableFileBrowser = true;

                    Editor.SpellCheckerUrl = sRoot + "/TinyMCEHandler.ashx?rp=spellchecker";

                    Editor.ForcePasteAsPlainText = config.FullWithTemplatesToolbarForcePasteAsPlainText;
                   
                    
                    break;

                case ToolBar.Newsletter:

                    Editor.Plugins = config.NewsletterToolbarPlugins;
                    Editor.AdvancedRow1Buttons = config.NewsletterToolbarRow1Buttons;
                    Editor.AdvancedRow2Buttons = config.NewsletterToolbarRow2Buttons;
                    Editor.AdvancedRow3Buttons = config.NewsletterToolbarRow3Buttons;
                    Editor.ExtendedValidElements = config.NewsletterToolbarExtendedValidElements;

                    string snRoot = SiteUtils.GetNavigationSiteRoot();
                    Editor.FileManagerUrl = snRoot + "/Dialog/FileDialog.aspx";
                    Editor.EnableFileBrowser = true;

                    Editor.SpellCheckerUrl = snRoot + "/TinyMCEHandler.ashx?rp=spellchecker";
                    Editor.ForcePasteAsPlainText = config.NewsletterToolbarForcePasteAsPlainText;
                    
                    //Editor.InlineStyles = false;
                    //Editor.Cleanup = false;

                    break;

                case ToolBar.GroupWithImages:

                    //Editor.Plugins = "paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups";

                    //Editor.AdvancedRow1Buttons = "cut,copy,pastetext,separator,blockquote,bold,italic,underline,separator,bullist,numlist,separator,link,unlink,separator,charmap,emotions";

                    //Editor.AdvancedRow2Buttons = "";

                    //Editor.AdvancedRow3Buttons = "";

                    Editor.Plugins = config.GroupWithImagesToolbarPlugins;
                    Editor.AdvancedRow1Buttons = config.GroupWithImagesToolbarRow1Buttons;
                    Editor.AdvancedRow2Buttons = config.GroupWithImagesToolbarRow2Buttons;
                    Editor.AdvancedRow3Buttons = config.GroupWithImagesToolbarRow3Buttons;
                    Editor.ExtendedValidElements = config.GroupWithImagesToolbarExtendedValidElements;
                    
                    Editor.FileManagerUrl = SiteUtils.GetNavigationSiteRoot() + "/Dialog/FileDialog.aspx";
                    Editor.EnableFileBrowser = true;
                    Editor.ForcePasteAsPlainText = config.GroupWithImagesToolbarForcePasteAsPlainText;

                    break;

                case ToolBar.Group:

                    //Editor.Plugins = "paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups";

                    //Editor.AdvancedRow1Buttons = "cut,copy,pastetext,separator,blockquote,bold,italic,underline,separator,bullist,numlist,separator,link,unlink,separator,charmap,emotions";

                    //Editor.AdvancedRow2Buttons = "";

                    //Editor.AdvancedRow3Buttons = "";

                    Editor.Plugins = config.GroupToolbarPlugins;
                    Editor.AdvancedRow1Buttons = config.GroupToolbarRow1Buttons;
                    Editor.AdvancedRow2Buttons = config.GroupToolbarRow2Buttons;
                    Editor.AdvancedRow3Buttons = config.GroupToolbarRow3Buttons;
                    Editor.ExtendedValidElements = config.GroupToolbarExtendedValidElements;

                    Editor.ForcePasteAsPlainText = config.GroupToolbarForcePasteAsPlainText;
                    
                    
                    break;



                case ToolBar.AnonymousUser:

                    //Editor.Plugins = "paste,emotions,directionality,inlinepopups";

                    //Editor.AdvancedRow1Buttons = "cut,copy,pastetext,separator,blockquote,bold,italic,separator,bullist,numlist,separator,link,unlink,emotions";

                    //Editor.AdvancedRow2Buttons = "";

                    //Editor.AdvancedRow3Buttons = "";

                    Editor.Plugins = config.AnonymousToolbarPlugins;
                    Editor.AdvancedRow1Buttons = config.AnonymousToolbarRow1Buttons;
                    Editor.AdvancedRow2Buttons = config.AnonymousToolbarRow2Buttons;
                    Editor.AdvancedRow3Buttons = config.AnonymousToolbarRow3Buttons;
                    Editor.ExtendedValidElements = config.AnonymousToolbarExtendedValidElements;

                    Editor.ForcePasteAsPlainText = config.AnonymousToolbarForcePasteAsPlainText;


                    break;

                case ToolBar.SimpleWithSource:

                    Editor.Plugins = "paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups";

                    Editor.AdvancedRow1Buttons = "code,cut,copy,pastetext,separator,blockquote,bold,italic,separator,bullist,numlist,separator,link,unlink,emotions";

                    Editor.AdvancedRow2Buttons = "";

                    Editor.AdvancedRow3Buttons = "";

                    break;
            }

        }

        #endregion

        #region Public Methods

        public Control GetEditorControl()
        {
            return Editor;
        }

        

        #endregion

    }
}
