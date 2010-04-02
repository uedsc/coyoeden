// Author:					Joe Audette
// Created:					2009-08-13
// Last Modified:			2009-08-13
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


namespace mojoPortal.Web.Editor
{
    public class TinyMCETextBox : TextBox
    {

        #region Configuration Properties

        private string basePath = "/ClientScript/tiny_mce/";

        public string BasePath
        {
            get { return basePath; }
            set { basePath = value; }
        }

        private bool autoFocus = false;
        /// <summary>
        /// This option enables you to auto focus an editor instance. The 
        /// value of this option should be an editor instance id. Editor 
        /// instance ids are specified as "mce_editor_<index>", where 
        /// index is a value starting from 0. So if there are 3 editor 
        /// instances on a page, they would have the following 
        /// ids - mce_editor_0, mce_editor_1 and mce_editor_2.
        /// </summary>
        public bool AutoFocus
        {
            get { return autoFocus; }
            set { autoFocus = value; }
        }

        private bool accessibilityFocus = true;
        /// <summary>
        /// If true, some accessibility focus will be available to all buttons: 
        /// you will be able to tab through them all. If false, focus will be 
        /// placed inside the text area when you tab through the interface. 
        /// The default is true.
        /// </summary>
        public bool AccessibilityFocus
        {
            get { return accessibilityFocus; }
            set { accessibilityFocus = value; }
        }

        private bool accessibilityWarnings = true;
        /// <summary>
        /// If this option is set to true some accessibility warnings will be 
        /// presented to the user if they miss specifying that information. 
        /// This option is set to true default, since we should all try to 
        /// make this world a better place for disabled people. But if you 
        /// are annoyed with the warnings, set this option to false.
        /// </summary>
        public bool AccessibilityWarnings
        {
            get { return accessibilityWarnings; }
            set { accessibilityWarnings = value; }
        }

        private string browsers = "msie,gecko,safari,opera";
        /// <summary>
        /// This option should contain a comma separated list of supported 
        /// browsers. This enables you, for example, to disable the editor 
        /// while running on Safari. The default value of this option 
        /// is: msie,gecko,safari,opera. Since the support for Safari is 
        /// very limited, a warning message will appear until a better 
        /// version is released. The possible values of this option 
        /// are msie, gecko, safari and opera.
        /// </summary>
        public string Browsers
        {
            get { return browsers; }
            set { browsers = value; }
        }

        private bool customShortcuts = true;
        /// <summary>
        /// This option enables you to disable/enable the custom keyboard 
        /// shortcuts, which plugins and themes may register. The value 
        /// of this option is set to true by default.
        /// </summary>
        public bool CustomShortcuts
        {
            get { return customShortcuts; }
            set { customShortcuts = value; }
        }

        private string dialogType = "window";
        /// <summary>
        /// This option enables you to specify how dialogs/popups should be 
        /// opened, possible values are "window" and "modal", where the 
        /// window option opens a normal window and the dialog option opens 
        /// a modal dialog. This option is set to "window" by default.
        /// </summary>
        public string DialogType
        {
            get { return dialogType; }
            set { dialogType = value; }
        }

        private string textDirection = "ltr";
        /// <summary>
        /// This option specifies the default writing direction, some languages 
        /// (Like Hebrew, Arabic, Urdu...) write from right to left instead 
        /// of left to right. The default value of this option is "ltr" but 
        /// if you want to use from right to left mode specify "rtl" instead.
        /// </summary>
        public string TextDirection
        {
            get { return textDirection; }
            set { textDirection = value; }
        }

        
        private string deSelectorCSSClass = "mceNoEditor";
        /// <summary>
        /// This option enables you to specify a CSS class name that will 
        /// deselect textareas from being converted into editor instances. 
        /// If this option isn't set to a value, this option will not have 
        /// any effect, and the mode option will choose textareas instead. 
        /// The default value of this option is "mceNoEditor", so if 
        /// mceNoEditor is added to the class attribute of a textarea it will 
        /// be excluded for conversion. This option also enables you to use 
        /// regular expressions like myEditor|myOtherEditor or .*editor.
        /// </summary>
        public string DeSelectorCSSClass
        {
            get { return deSelectorCSSClass; }
            set { deSelectorCSSClass = value; }
        }

        private bool enableGeckoSpellCheck = true;
        /// <summary>
        /// This option enables you toggle the internal Gecko/Firefox 
        /// spellchecker logic. This option is set to false by default and 
        /// will then remove the spellchecker from TinyMCE.
        /// mojoPortal note: not sure why this is false by default in TinyMCE
        /// I'm making it true by default here.
        /// </summary>
        public bool EnableGeckoSpellCheck
        {
            get { return enableGeckoSpellCheck; }
            set { enableGeckoSpellCheck = value; }
        }

       
        private string language = "en";
        /// <summary>
        /// This option should contain a language code of the language pack to 
        /// use with TinyMCE. These codes are in ISO-639-1 format to see if 
        /// your language is available check the contents of 
        /// "tinymce/jscripts/tiny_mce/langs". The default value of this 
        /// option is "en" for English.
        /// </summary>
        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        private string mode = "exact";
        /// <summary>
        /// This option specifies how elements are converted into TinyMCE WYSIWYG editor instances. This option can be set to any of the values below.
        /// exact is the default and uses just the specific textarea
        /// textarea will make all textareas on the page into editors
        /// </summary>
        public string Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        private bool enableObjectResizing = true;
        /// <summary>
        /// This true/false option gives you the ability to turn on/off the 
        /// inline resizing controls of tables and images in Firefox/Mozilla. 
        /// These are enabled by default.
        /// </summary>
        public bool EnableObjectResizing
        {
            get { return enableObjectResizing; }
            set { enableObjectResizing = value; }
        }

        private string plugins = string.Empty;
        /// <summary>
        /// This option should contain a comma separated list of plugins. Plugins 
        /// are loaded from the "tinymce/jscripts/tiny_mce/plugins" directory, 
        /// and the plugin name matches the name of the directory. TinyMCE is 
        /// shipped with some core plugins; these are described in greater 
        /// detail in the Plugins reference.
        /// http://wiki.moxiecode.com/index.php/TinyMCE:Plugins
        /// 
        /// TinyMCE also supports the ability to have plugins added from a external 
        /// resource. These plugins need to be self registering and loaded 
        /// after the tinyMCE.init call. You should also prefix these plugins 
        /// with a "-" character, so that TinyMCE doesn't try to load it from 
        /// the TinyMCE plugins directory.
        /// 
        /// There are many third party plugins for TinyMCE; some of these may be 
        /// found under "Plugins" at SourceForge, and, if you have developed 
        /// one of your own, please contribute it to this project by uploading 
        /// it to SourceForge.
        /// </summary>
        public string Plugins
        {
            get { return plugins; }
            set { plugins = value; }
        }

        private bool useStrictLoadingMode = false;
        /// <summary>
        /// This option will force TinyMCE to load script using a DOM insert 
        /// method, instead of document.write, on Gecko browsers. Since this 
        /// results in asynchronous script loading, a build in synchronized 
        /// will ensure that themes, plugins and language packs files are 
        /// loaded in the correct order. This will, on the other hand, make 
        /// the initialization procedure of TinyMCE a bit slower, and that's 
        /// why this isn't the default behavior. This option is set to true 
        /// by default, if the document content type is application/xhtml+xml.
        /// 
        /// </summary>
        public bool UseStrictLoadingMode
        {
            get { return useStrictLoadingMode; }
            set { useStrictLoadingMode = value; }
        }

        private string theme = "advanced";
        /// <summary>
        /// This option enables you to specify what theme to use when rendering 
        /// the TinyMCE WYSIWYG editor instances. This name matches the 
        /// directories located in tinymce/jscripts/tiny_mce/themes. The 
        /// default value of this option is "advanced". TinyMCE has two 
        /// built-in themes described below.
        /// 
        /// advanced
        /// This theme enables users to add/remove buttons and panels and is a 
        /// lot more flexible than the simple theme. For more information about 
        /// this theme's specific options check the advanced theme 
        /// configuration section. This is the default theme.
        /// 
        /// simple
        /// This is the most simple theme for TinyMCE. It contains only 
        /// the basic functions.
        /// 
        /// Example Usage
        /// 
        ///     tinyMCE.init({
        ///         ...
        ///     theme : "advanced",
        ///     theme_advanced_buttons3_add_before : "tablecontrols,separator"
        ///     });
        /// </summary>
        public string Theme
        {
            get { return theme; }
            set { theme = value; }
        }

        private bool enableUndoRedo = true;
        /// <summary>
        /// This option is a true/false option that enables you to 
        /// disable/enable the custom undo/redo logic within TinyMCE. 
        /// This option is enabled by default, if you disable it some 
        /// operations may not be undo able.
        /// </summary>
        public bool EnableUndoRedo
        {
            get { return enableUndoRedo; }
            set { enableUndoRedo = value; }
        }

        private int unDoLevels = -1;
        /// <summary>
        /// This option should contain the number of undo levels to keep in 
        /// memory. This is set to -1 by default and such a value tells 
        /// TinyMCE to use a unlimited number of undo levels. But this 
        /// steals lots of memory so for low end systems a value of 10 may 
        /// be better.
        /// </summary>
        public int UnDoLevels
        {
            get { return unDoLevels; }
            set { unDoLevels = value; }
        }

        private string advancedLayoutManager = "SimpleLayout";
        /// <summary>
        /// This option enables you to switch button and panel layout 
        /// functionality.
        /// 
        /// There are three different layout manager options:
        /// 
        /// SimpleLayout is the default layout manager,
        /// RowLayout is a more advanced layout manager, and
        /// CustomLayout executes a custom layout manager function.
        /// 
        /// Each of these layout managers have different options and can be 
        /// configured in different ways. This option is only available if the "advanced" theme is used.
        /// </summary>
        public string AdvancedLayoutManager
        {
            get { return advancedLayoutManager; }
            set { advancedLayoutManager = value; }
        }

        private string advancedBlockFormats = "p,address,pre,h1,h2,h3,h4,h5,h6";
        /// <summary>
        /// This option should contain a comma separated list of formats that 
        /// will be available in the format drop down list. The default value 
        /// of this option is 
        /// "p,address,pre,h1,h2,h3,h4,h5,h6". 
        /// This option is only available if the advanced theme is used.
        /// </summary>
        public string AdvancedBlockFormats
        {
            get { return advancedBlockFormats; }
            set { advancedBlockFormats = value; }
        }

        private string advancedStyles = string.Empty;
        /// <summary>
        /// This option should contain a semicolon separated list of class 
        /// titles and class names separated by =. The titles will be 
        /// presented to the user in the styles dropdown list and the 
        /// class names will be inserted. If this option is not defined, 
        /// TinyMCE imports the classes from the content_css.
        /// http://wiki.moxiecode.com/index.php/TinyMCE:Configuration/content_css
        /// 
        /// </summary>
        public string AdvancedStyles
        {
            get { return advancedStyles; }
            set { advancedStyles = value; }
        }

        private string advancedSourceEditorWidth = "500";
        /// <summary>
        /// This option is used to define the width of the source editor 
        /// dialog. This option is set to 500 by default.
        /// </summary>
        public string AdvancedSourceEditorWidth
        {
            get { return advancedSourceEditorWidth; }
            set { advancedSourceEditorWidth = value; }
        }

        private string advancedSourceEditorHeight = "400";
        /// <summary>
        /// This option is used to define the height of the source editor 
        /// dialog. This option is set to 400 by default.
        /// </summary>
        public string AdvancedSourceEditorHeight
        {
            get { return advancedSourceEditorHeight; }
            set { advancedSourceEditorHeight = value; }
        }

        private bool advancedSourceEditorWrap = true;
        /// <summary>
        /// This option enables you to force word wrap for the source editor, 
        /// this option is set to true by default.
        /// </summary>
        public bool AdvancedSourceEditorWrap
        {
            get { return advancedSourceEditorWrap; }
            set { advancedSourceEditorWrap = value; }
        }

        private string advancedToolbarLocation = "top";
        /// <summary>
        /// This option enables you to specify where the toolbar should be located. 
        /// This option can be set to "top" or "bottom" (the default) 
        /// or "external". This option can only be used when theme is 
        /// set to advanced and when the theme_advanced_layout_manager 
        /// option is set to the default value of "SimpleLayout".
        /// 
        /// Choosing the "external" location adds the toolbar to a DIV 
        /// element and sets the class of this DIV to "mceToolbarExternal". 
        /// This enables you to freely specify the location of the toolbar.
        /// 
        /// In mojoPortal I made the default "top", which seems a better default
        /// to me.
        /// </summary>
        public string AdvancedToolbarLocation
        {
            get { return advancedToolbarLocation; }
            set { advancedToolbarLocation = value; }
        }

        private string advancedToolbarAlign = "left";
        /// <summary>
        /// This option enables you to specify the alignment of the toolbar, 
        /// this value can be "left", "right" or "center" (the default). 
        /// This option can only be used when theme is set to advanced and 
        /// when the theme_advanced_layout_manager option is set to the 
        /// default value of "SimpleLayout".
        /// 
        /// In mojoPortal I changed the default to left.
        /// </summary>
        public string AdvancedToolbarAlign
        {
            get { return advancedToolbarAlign; }
            set { advancedToolbarAlign = value; }
        }

        private string advancedStatusBarLocation = "none";
        /// <summary>
        /// This option enables you to specify where the element statusbar 
        /// with the path and resize tool should be located. This option can 
        /// be set to "top", "bottom" or "none". The default value is set to 
        /// "none". This option can only be used when the theme is set to 
        /// "advanced" and when the theme_advanced_layout_manager option is 
        /// set to the default value of "SimpleLayout".
        /// 
        /// </summary>
        public string AdvancedStatusBarLocation
        {
            get { return advancedStatusBarLocation; }
            set { advancedStatusBarLocation = value; }
        }

        private string advancedRow1Buttons = "bold,italic,underline,strikethrough,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,formatselect,styleselect";
        /// <summary>
        /// This option should contain a comma separated list of button/control 
        /// names to insert into the toolbar.  
        /// This property populates the first row of 3 allowed rows.
        /// Below is a list of built in 
        /// controls, plugins may include other controls names that can be 
        /// inserted but these are documented in the individual plugins. 
        /// This option can only be used when theme is set to advanced and when 
        /// the theme_advanced_layout_manager option is set to the default 
        /// value of "SimpleLayout".
        /// A list of possible options can be found here:
        /// http://wiki.moxiecode.com/index.php/TinyMCE:Control_reference
        /// </summary>
        public string AdvancedRow1Buttons
        {
            get { return advancedRow1Buttons; }
            set { advancedRow1Buttons = value; }
        }

        private string advancedRow2Buttons = "bullist,numlist,separator,outdent,indent,separator,undo,redo,separator,link,unlink,anchor,image,cleanup,help,code";
        /// <summary>
        /// This option should contain a comma separated list of button/control 
        /// names to insert into the toolbar.  
        /// This property populates row 2 of 3 allowed rows.
        /// Below is a list of built in 
        /// controls, plugins may include other controls names that can be 
        /// inserted but these are documented in the individual plugins. 
        /// This option can only be used when theme is set to advanced and when 
        /// the theme_advanced_layout_manager option is set to the default 
        /// value of "SimpleLayout".
        /// A list of possible options can be found here:
        /// http://wiki.moxiecode.com/index.php/TinyMCE:Control_reference
        /// </summary>
        public string AdvancedRow2Buttons
        {
            get { return advancedRow2Buttons; }
            set { advancedRow2Buttons = value; }
        }

        private string advancedRow3Buttons = "hr,removeformat,visualaid,separator,sub,sup,separator,charmap";
        /// <summary>
        /// This option should contain a comma separated list of button/control 
        /// names to insert into the toolbar.  
        /// This property populates row 2 of 3 allowed rows.
        /// Below is a list of built in 
        /// controls, plugins may include other controls names that can be 
        /// inserted but these are documented in the individual plugins. 
        /// This option can only be used when theme is set to advanced and when 
        /// the theme_advanced_layout_manager option is set to the default 
        /// value of "SimpleLayout".
        /// A list of possible options can be found here:
        /// http://wiki.moxiecode.com/index.php/TinyMCE:Control_reference
        /// </summary>
        public string AdvancedRow3Buttons
        {
            get { return advancedRow3Buttons; }
            set { advancedRow3Buttons = value; }
        }


        private bool cleanup = true;
        /// <summary>
        /// This option enables or disables the built-in clean up functionality. 
        /// TinyMCE is equipped with powerful clean up functionality that 
        /// enables you to specify what elements and attributes are allowed 
        /// and how HTML contents should be generated. This option is set to 
        /// true by default, but if you want to disable it you may set it to 
        /// false.
        /// Notice: It's not recommended to disable this feature.
        /// 
        /// It might be worth mentioning that the browser usually messes with the 
        /// HTML. The clean up not only fixes several problems with the 
        /// browsers' parsed HTML document, like paths etc., it also makes 
        /// sure it is a correct XHTML document, with all tags closed, the " 
        /// at the right places, and things like that.
        /// </summary>
        public bool Cleanup
        {
            get { return cleanup; }
            set { cleanup = value; }
        }

        private bool cleanupOnStart = false;
        /// <summary>
        /// If you set this option to true, TinyMCE will perform a HTML cleanup 
        /// call when the editor loads. This option is set to false by default.
        /// </summary>
        public bool CleanupOnStart
        {
            get { return cleanupOnStart; }
            set { cleanupOnStart = value; }
        }

        private string editorAreaCSS = string.Empty;
        /// <summary>
        /// This option enables you to specify a custom CSS file that extends 
        /// the theme content CSS. This CSS file is the one used within the 
        /// editor (the editable area). The default location of this CSS file 
        /// is within the current theme. This option can also be a comma 
        /// separated list of URLs.
        /// </summary>
        public string EditorAreaCSS
        {
            get { return editorAreaCSS; }
            set { editorAreaCSS = value; }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.EnableViewState = true;
            this.EnableViewState = true;
            this.TextMode = TextBoxMode.MultiLine;
            basePath = ConfigurationManager.AppSettings["TinyMCE:BasePath"];
           
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
                "tinymcemain",
                "<script type=\"text/javascript\" src=\""
                + ResolveUrl(this.BasePath + "tiny_mce.js") + "\"></script>");

            StringBuilder setupScript = new StringBuilder();
            setupScript.Append("\n<script type=\"text/javascript\">");
            setupScript.Append("tinyMCE.init({");

            setupScript.Append("mode : \"" + this.Mode + "\"");
            setupScript.Append(", elements : \"" + this.UniqueID + "\"");
            //setupScript.Append(", elements : \"" + this.ClientID + "\"");

            //deprecated
            //if (Debug)
            //{
            //    setupScript.Append(", debug : true ");
            //}

            if (!AccessibilityFocus) // true is default
            {
                setupScript.Append(", accessibility_focus : false ");
            }
            if (!AccessibilityWarnings)
            {
                setupScript.Append(", accessibility_warnings : false ");
            }

            //deprecated
            //if (!AutoResetDesignMode)
            //{
            //    setupScript.Append(", auto_reset_designmode : false ");
            //}

            setupScript.Append(", browsers : \"" + this.Browsers + "\"");

            //deprecated
            //if (ButtonTileMap)
            //{
            //    setupScript.Append(", button_tile_map : true ");
            //}

            if (!CustomShortcuts)
            {
                setupScript.Append(", custom_shortcuts : false ");
            }

            setupScript.Append(", dialog_type : \"" + this.DialogType + "\"");

            setupScript.Append(", directionality : \"" + this.TextDirection + "\"");

            //deprecated
            // setupScript.Append(", docs_language : \"" + this.DocsLanguage + "\"");

            setupScript.Append(", editor_deselector : \"" + this.DeSelectorCSSClass + "\"");

            //deprecated
            //  setupScript.Append(", event_elements : \"" + this.EventElements + "\"");

            if (EnableGeckoSpellCheck)
            {
                setupScript.Append(", gecko_spellcheck : true ");
            }

            //deprecated
            //if (HideSelectsOnSubmit)
            //{
            //    setupScript.Append(", hide_selects_on_submit : true ");
            //}

            setupScript.Append(", language : \"" + this.Language + "\"");

            if (!EnableObjectResizing)
            {
                setupScript.Append(", object_resizing : false ");
            }

            if (Plugins.Length > 0)
            {
                setupScript.Append(", plugins : \"" + this.Plugins + "\"");
            }

            //if (UseStrictLoadingMode)
            //{
            //    setupScript.Append(", strict_loading_mode : true ");
            //}

            setupScript.Append(", theme : \"" + this.Theme + "\"");

            if (!EnableUndoRedo)
            {
                setupScript.Append(", custom_undo_redo : false ");
            }

            if (EnableUndoRedo)
            {
                setupScript.Append(", custom_undo_redo_levels : " + this.UnDoLevels.ToString());
            }

            if (Theme == "advanced")
            {
                setupScript.Append(", layout_manager : \"" + this.AdvancedLayoutManager + "\"");
                setupScript.Append(", theme_advanced_blockformats : \"" + this.AdvancedBlockFormats + "\"");
                if (AdvancedStyles.Length > 0)
                {
                    setupScript.Append(", theme_advanced_styles : \"" + this.AdvancedStyles + "\"");
                }

                setupScript.Append(", theme_advanced_source_editor_width : \"" + this.AdvancedSourceEditorWidth + "\"");
                setupScript.Append(", theme_advanced_source_editor_height : \"" + this.AdvancedSourceEditorHeight + "\"");
                if (!AdvancedSourceEditorWrap)
                {
                    setupScript.Append(", theme_advanced_source_editor_wrap : false ");
                }

                if (AdvancedLayoutManager == "SimpleLayout")
                {
                    setupScript.Append(", theme_advanced_toolbar_location : \"" + this.AdvancedToolbarLocation + "\"");
                    setupScript.Append(", theme_advanced_toolbar_align : \"" + this.AdvancedToolbarAlign + "\"");
                    setupScript.Append(", theme_advanced_statusbar_location : \"" + this.AdvancedStatusBarLocation + "\"");

                    setupScript.Append(", theme_advanced_buttons1 : \"" + this.AdvancedRow1Buttons + "\"");
                    setupScript.Append(", theme_advanced_buttons2 : \"" + this.AdvancedRow2Buttons + "\"");
                    setupScript.Append(", theme_advanced_buttons3 : \"" + this.AdvancedRow3Buttons + "\"");

                    //setupScript.Append(", theme_advanced_buttons1_add : \"pastetext,pasteword,selectall\"");

                }

                if (!Cleanup)
                {
                    setupScript.Append(", cleanup : false ");
                }

                if (CleanupOnStart)
                {
                    setupScript.Append(", cleanup_on_startup : true ");
                }

                if (EditorAreaCSS.Length > 0)
                {
                    setupScript.Append(", content_css : \"" + this.EditorAreaCSS + "\"");
                }

                if (AutoFocus)
                {
                    // this is kind of a crappy implementation in TinyMCE
                    // because it requires knowing if more than one editor is
                    // on the page. I should be able to specify an
                    // instance more directly
                    setupScript.Append(", auto_focus : \"mce_editor_0\" ");
                }

            }

            setupScript.Append("});</script>");

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                this.UniqueID,
                setupScript.ToString());


        }



    }
}
