// Author:					Joe Audette
// Created:				    2009-09-14
// Last Modified:			2009-11-06
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Web;
using System.Web.Hosting;
using System.Web.Caching;
using System.Xml;
using log4net;

namespace Cynthia.Web.Editor
{
    public class TinyMceConfiguration
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TinyMceConfiguration));

        private TinyMceConfiguration()
        { }

        private string fullToolbarPlugins = "media,template,paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups,spellchecker,wordcount,safari";
        public string FullToolbarPlugins
        {
            get { return fullToolbarPlugins; }
        }

        private string fullToolbarRow1Buttons = "code,separator,selectall,removeformat,cut,copy,separator,paste,pastetext,pasteword,separator,print,separator,undo,redo,separator,search,replace";
        public string FullToolbarRow1Buttons
        {
            get { return fullToolbarRow1Buttons; }
        }

        private string fullToolbarRow2Buttons = "blockquote,bold,italic,underline,strikethrough,separator,sub,sup,separator,bullist,numlist,separator,outdent,indent,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,link,unlink,anchor,image,media,table,hr,emotions,charmap";
        public string FullToolbarRow2Buttons
        {
            get { return fullToolbarRow2Buttons; }
        }

        private string fullToolbarRow3Buttons = "formatselect,styleselect,separator,cleanup,fullscreen,spellchecker";
        public string FullToolbarRow3Buttons
        {
            get { return fullToolbarRow3Buttons; }
        }

        private bool fullToolbarForcePasteAsPlainText = false;
        public bool FullToolbarForcePasteAsPlainText
        {
            get { return fullToolbarForcePasteAsPlainText; }
        }

        private string fullToolbarExtendedValidElements = "iframe[src|width|height|style|name|title|align|scrolling|frameborder|allowtransparency]";
        public string FullToolbarExtendedValidElements
        {
            get { return fullToolbarExtendedValidElements; }
        }

        private string fullWithTemplatesToolbarPlugins = "media,template,paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups,spellchecker,wordcount,safari";
        public string FullWithTemplatesToolbarPlugins
        {
            get { return fullWithTemplatesToolbarPlugins; }
        }

        private string fullWithTemplatesToolbarRow1Buttons = "code,separator,selectall,removeformat,cleanup,cut,copy,separator,paste,pastetext,pasteword,separator,print,separator,undo,redo,separator,search,replace";
        public string FullWithTemplatesToolbarRow1Buttons
        {
            get { return fullWithTemplatesToolbarRow1Buttons; }
        }

        private string fullWithTemplatesToolbarRow2Buttons = "blockquote,bold,italic,underline,strikethrough,separator,sub,sup,separator,bullist,numlist,separator,outdent,indent,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,link,unlink,anchor,image,media,table,hr,emotions,charmap";
        public string FullWithTemplatesToolbarRow2Buttons
        {
            get { return fullWithTemplatesToolbarRow2Buttons; }
        }

        private string fullWithTemplatesToolbarRow3Buttons = "template,formatselect,styleselect,separator,cleanup,fullscreen,spellchecker";
        public string FullWithTemplatesToolbarRow3Buttons
        {
            get { return fullWithTemplatesToolbarRow3Buttons; }
        }

        private string fullWithTemplatesToolbarExtendedValidElements = "iframe[src|width|height|style|name|title|align|scrolling|frameborder|allowtransparency]";
        public string FullWithTemplatesToolbarExtendedValidElements
        {
            get { return fullWithTemplatesToolbarExtendedValidElements; }
        }

        private bool fullWithTemplatesToolbarForcePasteAsPlainText = false;
        public bool FullWithTemplatesToolbarForcePasteAsPlainText
        {
            get { return fullWithTemplatesToolbarForcePasteAsPlainText; }
        }

        private string forumWithImagesToolbarPlugins = "paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups";
        public string GroupWithImagesToolbarPlugins
        {
            get { return forumWithImagesToolbarPlugins; }
        }

        private string forumWithImagesToolbarRow1Buttons = "cut,copy,pastetext,separator,blockquote,bold,italic,underline,separator,bullist,numlist,separator,link,unlink,separator,charmap,emotions";
        public string GroupWithImagesToolbarRow1Buttons
        {
            get { return forumWithImagesToolbarRow1Buttons; }
        }

        private string forumWithImagesToolbarRow2Buttons = string.Empty;
        public string GroupWithImagesToolbarRow2Buttons
        {
            get { return forumWithImagesToolbarRow2Buttons; }
        }

        private string forumWithImagesToolbarRow3Buttons = string.Empty;
        public string GroupWithImagesToolbarRow3Buttons
        {
            get { return forumWithImagesToolbarRow3Buttons; }
        }

        private string forumWithImagesToolbarExtendedValidElements = "";
        public string GroupWithImagesToolbarExtendedValidElements
        {
            get { return forumWithImagesToolbarExtendedValidElements; }
        }

        private bool forumWithImagesToolbarForcePasteAsPlainText = true;
        public bool GroupWithImagesToolbarForcePasteAsPlainText
        {
            get { return forumWithImagesToolbarForcePasteAsPlainText; }
        }

        private string forumToolbarPlugins = "paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups";
        public string GroupToolbarPlugins
        {
            get { return forumToolbarPlugins; }
        }

        private string forumToolbarRow1Buttons = "cut,copy,pastetext,separator,blockquote,bold,italic,underline,separator,bullist,numlist,separator,link,unlink,separator,charmap,emotions";
        public string GroupToolbarRow1Buttons
        {
            get { return forumToolbarRow1Buttons; }
        }

        private string forumToolbarRow2Buttons = string.Empty;
        public string GroupToolbarRow2Buttons
        {
            get { return forumToolbarRow2Buttons; }
        }

        private string forumToolbarRow3Buttons = string.Empty;
        public string GroupToolbarRow3Buttons
        {
            get { return forumToolbarRow3Buttons; }
        }

        private string forumToolbarExtendedValidElements = "";
        public string GroupToolbarExtendedValidElements
        {
            get { return forumToolbarExtendedValidElements; }
        }

        private bool forumToolbarForcePasteAsPlainText = true;
        public bool GroupToolbarForcePasteAsPlainText
        {
            get { return forumToolbarForcePasteAsPlainText; }
        }

        private string anonymousToolbarPlugins = "paste,emotions,directionality,inlinepopups";
        public string AnonymousToolbarPlugins
        {
            get { return anonymousToolbarPlugins; }
        }

        private string anonymousToolbarRow1Buttons = "cut,copy,pastetext,separator,blockquote,bold,italic,separator,bullist,numlist,separator,link,unlink,emotions";
        public string AnonymousToolbarRow1Buttons
        {
            get { return anonymousToolbarRow1Buttons; }
        }

        private string anonymousToolbarRow2Buttons = string.Empty;
        public string AnonymousToolbarRow2Buttons
        {
            get { return anonymousToolbarRow2Buttons; }
        }

        private string anonymousToolbarRow3Buttons = string.Empty;
        public string AnonymousToolbarRow3Buttons
        {
            get { return anonymousToolbarRow3Buttons; }
        }

        private string anonymousToolbarExtendedValidElements = "";
        public string AnonymousToolbarExtendedValidElements
        {
            get { return anonymousToolbarExtendedValidElements; }
        }

        private bool anonymousToolbarForcePasteAsPlainText = true;
        public bool AnonymousToolbarForcePasteAsPlainText
        {
            get { return anonymousToolbarForcePasteAsPlainText; }
        }

        private string advancedFormatBlocks = "p,address,pre,h1,h2,h3,h4,h5,h6";
        public string AdvancedFormatBlocks
        {
            get { return advancedFormatBlocks; }
        }

        private string advancedSourceEditorWidth = "780";
        public string AdvancedSourceEditorWidth
        {
            get { return advancedSourceEditorWidth; }
        }

        private string advancedSourceEditorHeight = "700";
        public string AdvancedSourceEditorHeight
        {
            get { return advancedSourceEditorHeight; }
        }

        private string advancedToolbarLocation = "top";
        public string AdvancedToolbarLocation
        {
            get { return advancedToolbarLocation; }
        }

        private string advancedToolbarAlign = "left";
        public string AdvancedToolbarAlign
        {
            get { return advancedToolbarAlign; }
        }

        private string advancedStatusBarLocation = "bottom";
        public string AdvancedStatusBarLocation
        {
            get { return advancedStatusBarLocation; }
        }

        private bool accessibilityWarnings = true;
        public bool AccessibilityWarnings
        {
            get { return accessibilityWarnings; }
        }

        private string dialogType = "modal";
        public string DialogType
        {
            get { return dialogType; }
        }

        private bool enableObjectResizing = true;
        public bool EnableObjectResizing
        {
            get { return enableObjectResizing; }
        }

        private bool enableUndoRedo = true;
        public bool EnableUndoRedo
        {
            get { return enableUndoRedo; }
        }

        private int unDoLevels = 10;
        public int UnDoLevels
        {
            get { return unDoLevels; }
        }

        private bool cleanup = true;
        public bool Cleanup
        {
            get { return cleanup; }
        }

        private bool cleanupOnStart = false;
        public bool CleanupOnStart
        {
            get { return cleanupOnStart; }
        }

        private bool autoFocus = true;
        public bool AutoFocus
        {
            get { return autoFocus; }
        }


        private string newsletterToolbarPlugins = "fullpage,paste,print,searchreplace,fullscreen,emotions,directionality,table,contextmenu,advimage,inlinepopups,spellchecker,wordcount,safari";
        public string NewsletterToolbarPlugins
        {
            get { return newsletterToolbarPlugins; }
        }

        private string newsletterToolbarRow1Buttons = "code,separator,selectall,removeformat,cut,copy,separator,paste,pastetext,pasteword,separator,print,separator,undo,redo,separator,search,replace";
        public string NewsletterToolbarRow1Buttons
        {
            get { return newsletterToolbarRow1Buttons; }
        }

        private string newsletterToolbarRow2Buttons = "blockquote,bold,italic,underline,strikethrough,separator,sub,sup,separator,bullist,numlist,separator,outdent,indent,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,link,unlink,anchor,image,media,table,hr,charmap";
        public string NewsletterToolbarRow2Buttons
        {
            get { return newsletterToolbarRow2Buttons; }
        }

        private string newsletterToolbarRow3Buttons = "formatselect,fontselect,fontsizeselect,forecolorpicker,backcolorpicker,separator,cleanup,fullscreen,spellchecker,fullpage";
        public string NewsletterToolbarRow3Buttons
        {
            get { return newsletterToolbarRow3Buttons; }
        }

        private string newsletterToolbarExtendedValidElements = "";
        public string NewsletterToolbarExtendedValidElements
        {
            get { return newsletterToolbarExtendedValidElements; }
        }

        private bool newsletterToolbarForcePasteAsPlainText = false;
        public bool NewsletterToolbarForcePasteAsPlainText
        {
            get { return newsletterToolbarForcePasteAsPlainText; }
        }

        private string spellCheckerLanguages = string.Empty;
        public string SpellCheckerLanguages
        {
            get { return spellCheckerLanguages; }
        }

        public static TinyMceConfiguration GetConfig()
        {
            TinyMceConfiguration config = new TinyMceConfiguration();

            try
            {
                if (
                    (HttpRuntime.Cache["CTinyConfiguration"] != null)
                    && (HttpRuntime.Cache["CTinyConfiguration"] is TinyMceConfiguration)
                )
                {
                    return (TinyMceConfiguration)HttpRuntime.Cache["CTinyConfiguration"];
                }

                string pathToConfigFile = HostingEnvironment.MapPath("~/" + GetConfigFileName());
                
                XmlDocument configXml = new XmlDocument();
                configXml.Load(pathToConfigFile);
                config.LoadValuesFromConfigurationXml(configXml.DocumentElement);

                AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();

                string pathToWebConfig = HostingEnvironment.MapPath("~/Web.config");

                aggregateCacheDependency.Add(new CacheDependency(pathToWebConfig));

                HttpRuntime.Cache.Insert(
                    "CTinyConfiguration",
                    config,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    CacheItemPriority.Default,
                    null);

                return (TinyMceConfiguration)HttpRuntime.Cache["CTinyConfiguration"];

            }
            catch (HttpException ex)
            {
                log.Error(ex);

            }
            catch (XmlException ex)
            {
                log.Error(ex);

            }
            catch (ArgumentException ex)
            {
                log.Error(ex);

            }
            catch (NullReferenceException ex)
            {
                log.Error(ex);

            }

            return config;

            
        }

        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            if (node == null) { return; }

            XmlAttributeCollection attributeCollection = node.Attributes;

            if (attributeCollection["FullToolbarPlugins"] != null)
            {
                fullToolbarPlugins = attributeCollection["FullToolbarPlugins"].Value;
            }

            if (attributeCollection["FullToolbarRow1Buttons"] != null)
            {
                fullToolbarRow1Buttons = attributeCollection["FullToolbarRow1Buttons"].Value;
            }

            if (attributeCollection["FullToolbarRow2Buttons"] != null)
            {
                fullToolbarRow2Buttons = attributeCollection["FullToolbarRow2Buttons"].Value;
            }

            if (attributeCollection["FullToolbarRow3Buttons"] != null)
            {
                fullToolbarRow3Buttons = attributeCollection["FullToolbarRow3Buttons"].Value;
            }

            if (attributeCollection["FullToolbarExtendedValidElements"] != null)
            {
                fullToolbarExtendedValidElements = attributeCollection["FullToolbarExtendedValidElements"].Value;
            }

            if (attributeCollection["FullWithTemplatesToolbarPlugins"] != null)
            {
                fullWithTemplatesToolbarPlugins = attributeCollection["FullWithTemplatesToolbarPlugins"].Value;
            }

            if (attributeCollection["FullWithTemplatesToolbarRow1Buttons"] != null)
            {
                fullWithTemplatesToolbarRow1Buttons = attributeCollection["FullWithTemplatesToolbarRow1Buttons"].Value;
            }

            if (attributeCollection["FullWithTemplatesToolbarRow2Buttons"] != null)
            {
                fullWithTemplatesToolbarRow2Buttons = attributeCollection["FullWithTemplatesToolbarRow2Buttons"].Value;
            }

            if (attributeCollection["FullWithTemplatesToolbarRow3Buttons"] != null)
            {
                fullWithTemplatesToolbarRow3Buttons = attributeCollection["FullWithTemplatesToolbarRow3Buttons"].Value;
            }

            if (attributeCollection["FullWithTemplatesToolbarExtendedValidElements"] != null)
            {
                fullWithTemplatesToolbarExtendedValidElements = attributeCollection["FullWithTemplatesToolbarExtendedValidElements"].Value;
            }

            if (attributeCollection["GroupWithImagesToolbarPlugins"] != null)
            {
                forumWithImagesToolbarPlugins = attributeCollection["GroupWithImagesToolbarPlugins"].Value;
            }

            if (attributeCollection["GroupWithImagesToolbarRow1Buttons"] != null)
            {
                forumWithImagesToolbarRow1Buttons = attributeCollection["GroupWithImagesToolbarRow1Buttons"].Value;
            }

            if (attributeCollection["GroupWithImagesToolbarRow2Buttons"] != null)
            {
                forumWithImagesToolbarRow2Buttons = attributeCollection["GroupWithImagesToolbarRow2Buttons"].Value;
            }

            if (attributeCollection["GroupWithImagesToolbarRow3Buttons"] != null)
            {
                forumWithImagesToolbarRow3Buttons = attributeCollection["GroupWithImagesToolbarRow3Buttons"].Value;
            }

            if (attributeCollection["GroupWithImagesToolbarExtendedValidElements"] != null)
            {
                forumWithImagesToolbarExtendedValidElements = attributeCollection["GroupWithImagesToolbarExtendedValidElements"].Value;
            }

            if (attributeCollection["GroupToolbarPlugins"] != null)
            {
                forumToolbarPlugins = attributeCollection["GroupToolbarPlugins"].Value;
            }

            if (attributeCollection["GroupToolbarRow1Buttons"] != null)
            {
                forumToolbarRow1Buttons = attributeCollection["GroupToolbarRow1Buttons"].Value;
            }

            if (attributeCollection["GroupToolbarRow2Buttons"] != null)
            {
                forumToolbarRow2Buttons = attributeCollection["GroupToolbarRow2Buttons"].Value;
            }

            if (attributeCollection["GroupToolbarRow3Buttons"] != null)
            {
                forumToolbarRow3Buttons = attributeCollection["GroupToolbarRow3Buttons"].Value;
            }

            if (attributeCollection["GroupToolbarExtendedValidElements"] != null)
            {
                forumToolbarExtendedValidElements = attributeCollection["GroupToolbarExtendedValidElements"].Value;
            }

            if (attributeCollection["AnonymousToolbarPlugins"] != null)
            {
                anonymousToolbarPlugins = attributeCollection["AnonymousToolbarPlugins"].Value;
            }

            if (attributeCollection["AnonymousToolbarRow1Buttons"] != null)
            {
                anonymousToolbarRow1Buttons = attributeCollection["AnonymousToolbarRow1Buttons"].Value;
            }

            if (attributeCollection["AnonymousToolbarRow2Buttons"] != null)
            {
                anonymousToolbarRow2Buttons = attributeCollection["AnonymousToolbarRow2Buttons"].Value;
            }

            if (attributeCollection["AnonymousToolbarRow3Buttons"] != null)
            {
                anonymousToolbarRow3Buttons = attributeCollection["AnonymousToolbarRow3Buttons"].Value;
            }

            if (attributeCollection["AnonymousToolbarExtendedValidElements"] != null)
            {
                anonymousToolbarExtendedValidElements = attributeCollection["AnonymousToolbarExtendedValidElements"].Value;
            }

            if (attributeCollection["AdvancedFormatBlocks"] != null)
            {
                advancedFormatBlocks = attributeCollection["AdvancedFormatBlocks"].Value;
            }

            if (attributeCollection["AdvancedSourceEditorWidth"] != null)
            {
                advancedSourceEditorWidth = attributeCollection["AdvancedSourceEditorWidth"].Value;
            }

            if (attributeCollection["AdvancedSourceEditorHeight"] != null)
            {
                advancedSourceEditorHeight = attributeCollection["AdvancedSourceEditorHeight"].Value;
            }

            if (attributeCollection["AdvancedToolbarLocation"] != null)
            {
                advancedToolbarLocation = attributeCollection["AdvancedToolbarLocation"].Value;
            }

            if (attributeCollection["AdvancedToolbarAlign"] != null)
            {
                advancedToolbarAlign = attributeCollection["AdvancedToolbarAlign"].Value;
            }

            if (attributeCollection["AdvancedStatusBarLocation"] != null)
            {
                advancedStatusBarLocation = attributeCollection["AdvancedStatusBarLocation"].Value;
            }

            if (attributeCollection["AccessibilityWarnings"] != null)
            {
                accessibilityWarnings = Convert.ToBoolean(attributeCollection["AccessibilityWarnings"].Value);
            }

            if (attributeCollection["DialogType"] != null)
            {
                dialogType = attributeCollection["DialogType"].Value;
            }

            if (attributeCollection["EnableObjectResizing"] != null)
            {
                try
                {
                    enableObjectResizing = Convert.ToBoolean(attributeCollection["EnableObjectResizing"].Value);
                }
                catch (FormatException) { }
            }

            if (attributeCollection["EnableUndoRedo"] != null)
            {
                try
                {
                    enableUndoRedo = Convert.ToBoolean(attributeCollection["EnableUndoRedo"].Value);
                }
                catch (FormatException) { }
            }


            if (attributeCollection["UnDoLevels"] != null)
            {
                try
                {
                    unDoLevels = Convert.ToInt32(attributeCollection["UnDoLevels"].Value);
                }
                catch (FormatException) { }
            }

            if (attributeCollection["Cleanup"] != null)
            {
                try
                {
                    cleanup = Convert.ToBoolean(attributeCollection["Cleanup"].Value);
                }
                catch (FormatException) { }
            }

            if (attributeCollection["CleanupOnStart"] != null)
            {
                try
                {
                    cleanupOnStart = Convert.ToBoolean(attributeCollection["CleanupOnStart"].Value);
                }
                catch (FormatException) { }
            }

            if (attributeCollection["AutoFocus"] != null)
            {
                try
                {
                    autoFocus = Convert.ToBoolean(attributeCollection["AutoFocus"].Value);
                }
                catch (FormatException) { }
            }


            if (attributeCollection["FullToolbarForcePasteAsPlainText"] != null)
            {
                try
                {
                    fullToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["FullToolbarForcePasteAsPlainText"].Value);
                }
                catch (FormatException) { }
            }

            if (attributeCollection["FullWithTemplatesToolbarForcePasteAsPlainText"] != null)
            {
                try
                {
                    fullWithTemplatesToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["FullWithTemplatesToolbarForcePasteAsPlainText"].Value);
                }
                catch (FormatException) { }
            }

            if (attributeCollection["GroupWithImagesToolbarForcePasteAsPlainText"] != null)
            {
                try
                {
                    forumWithImagesToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["GroupWithImagesToolbarForcePasteAsPlainText"].Value);
                }
                catch (FormatException) { }
            }

            if (attributeCollection["GroupToolbarForcePasteAsPlainText"] != null)
            {
                try
                {
                    forumToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["GroupToolbarForcePasteAsPlainText"].Value);
                }
                catch (FormatException) { }
            }

            if (attributeCollection["AnonymousToolbarForcePasteAsPlainText"] != null)
            {
                try
                {
                    anonymousToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["AnonymousToolbarForcePasteAsPlainText"].Value);
                }
                catch (FormatException) { }
            }


            if (attributeCollection["NewsletterToolbarForcePasteAsPlainText"] != null)
            {
                try
                {
                    newsletterToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["NewsletterToolbarForcePasteAsPlainText"].Value);
                }
                catch (FormatException) { }
            }

            if (attributeCollection["NewsletterToolbarPlugins"] != null)
            {
                newsletterToolbarPlugins = attributeCollection["NewsletterToolbarPlugins"].Value;
            }

            if (attributeCollection["NewsletterToolbarRow1Buttons"] != null)
            {
                newsletterToolbarRow1Buttons = attributeCollection["NewsletterToolbarRow1Buttons"].Value;
            }

            if (attributeCollection["NewsletterToolbarRow2Buttons"] != null)
            {
                newsletterToolbarRow2Buttons = attributeCollection["NewsletterToolbarRow2Buttons"].Value;
            }

            if (attributeCollection["NewsletterToolbarRow3Buttons"] != null)
            {
                newsletterToolbarRow3Buttons = attributeCollection["NewsletterToolbarRow3Buttons"].Value;
            }

            if (attributeCollection["NewsletterToolbarExtendedValidElements"] != null)
            {
                newsletterToolbarExtendedValidElements = attributeCollection["NewsletterToolbarExtendedValidElements"].Value;
            }

            if (attributeCollection["SpellCheckerLanguages"] != null)
            {
                spellCheckerLanguages = attributeCollection["SpellCheckerLanguages"].Value;
            }


            
        }



        private static string GetConfigFileName()
        {
            string configFileName = "CTinyMCE.config";


            if (ConfigurationManager.AppSettings["TinyMCE:ConfigFile"] != null)
            {
                configFileName = ConfigurationManager.AppSettings["TinyMCE:ConfigFile"];
            }


            return configFileName;
        }


    }
}
