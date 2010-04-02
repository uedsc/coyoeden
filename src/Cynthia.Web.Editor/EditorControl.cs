using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Cynthia.Web.Editor
{
    /// <summary>
    /// Author:		        Joe Audette
    /// Created:            2007-05-25
    /// Last Modified:      2008-01-17
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public class EditorControl : Panel
    {
        private EditorProvider editorProvider = null;
        private IWebEditor editor;
        private string providerName = "FCKeditorProvider";
        private string failsafeProviderName = "FCKeditorProvider";
        private string providerNameFromViewState = string.Empty;
        private string scriptBaseUrl = "~/ClientScript";

        public IWebEditor WebEditor
        { 
            get { return editor; } 
        }

        public string Text
        {
            get { return editor.Text; }
            set { editor.Text = value; }
        }

        public string ScriptBaseUrl
        {
            get { return scriptBaseUrl; }
            set { scriptBaseUrl = value; }
        }

      
        /// <summary>
        /// This should be set in Page PreInit event
        /// </summary>
        public string ProviderName
        {
            get { return providerName; }
            set 
            { 
                
                if (this.Site != null && this.Site.DesignMode)
                {
                    
                }
                else
                {
                    if (
                    (value != providerName)
                    ||(editorProvider == null)
                    )
                    {
                        providerName = value;
                        SetupProvider();
                     
                    }
                    
                }
                
            }
        }

        public EditorProvider Provider
        {
            get { return editorProvider; }
        }

        protected override void OnInit(EventArgs e)
        {
            
            if (this.Site != null && this.Site.DesignMode)
            {
                
            }
            else
            {
                base.OnInit(e);
                Page.EnableViewState = true;
                Page.RegisterRequiresControlState(this);
                // an exception always happens here in design mode
                // this try is just to fix the display in design view in VS
                if (editorProvider == null)
                {
                    SetupProvider();
                }
                
               
            }
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (providerName != editorProvider.Name)
            {
                SetupProvider();
            }
        }

        

        private void SetupProvider()
        {
            //TODO: need to get rid of this try catch or catch more specific 
            try
            {
                if (EditorManager.Providers[providerName] != null)
                {
                    editorProvider = EditorManager.Providers[providerName];
                }
                else
                {
                    editorProvider = EditorManager.Providers[failsafeProviderName];
                }
                editor = editorProvider.GetEditor();
                editor.ControlID = this.ID + "innerEditor";

                editor.SiteRoot = Page.ResolveUrl("~/");
                editor.ScriptBaseUrl = Page.ResolveUrl(scriptBaseUrl);
                

                this.Controls.Clear();
                this.Controls.Add(editor.GetEditorControl());
            }
            catch { }


        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                base.Render(writer);
            }
        }

        
    }
}
