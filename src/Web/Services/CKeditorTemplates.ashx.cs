// Author:		        Joe Audette
// Created:            2009-08-31
// Last Modified:      2009-12-20
//
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.IO;
using System.Web;
using System.Collections;

//using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Cynthia.Web.Editor;
using Resources;

namespace Cynthia.Web.Services
{
    /// <summary>
    /// Returns content templates in a json format consumable by CKeditor
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CKeditorTemplates : IHttpHandler
    {

        private SiteSettings siteSettings = null;
        private string siteRoot = string.Empty;
        private string comma = string.Empty;

        
        public void ProcessRequest(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {

                return;
            }

            RenderJsonList(context);

        }

        private void RenderJsonList(HttpContext context)
        {
            context.Response.ContentType = "text/javascript";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            siteRoot = SiteUtils.GetNavigationSiteRoot();

            // Register a templates definition set named "C".
            context.Response.Write("CKEDITOR.addTemplates( 'C',{");

            context.Response.Write("imagesPath:'" + siteRoot + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/htmltemplateimages/" + "'");
            context.Response.Write(", templates :");
            context.Response.Write("[");


            if (WebConfigSettings.AddSystemContentTemplatesAboveSiteTemplates) //true by default
            {
                RenderSystemTemplateItems(context);
            }

            
            List<ContentTemplate> templates = ContentTemplate.GetAll(siteSettings.SiteGuid);
            foreach (ContentTemplate t in templates)
            {
                context.Response.Write(comma);
                context.Response.Write("{");

                context.Response.Write("title:'" + t.Title + "'");
                context.Response.Write(",image:'" + t.ImageFileName + "'");
                context.Response.Write(",description:'" + t.Description.RemoveLineBreaks() + "'");
                // is this going to work?
                context.Response.Write(",html:'" + t.Body.RemoveLineBreaks() + "'");

                context.Response.Write("}");



                comma = ",";


            }

            if (WebConfigSettings.AddSystemContentTemplatesBelowSiteTemplates) //false by default
            {
                RenderSystemTemplateItems(context);
            }

            context.Response.Write("]");

            context.Response.Write("});");


        }


        private void RenderSystemTemplateItems(HttpContext context)
        {
            // jQuery Accordion
            string templateContent = "<p>Paragraph before the accordion</p><div class=\"C-accordion\"><h3><a href=\"#\">Section 1</a></h3><div><p>Section 1 content.</p></div><h3><a href=\"#\">Section 2</a></h3><div><p>Section 2 content</p></div><h3><a href=\"#\">Section 3</a></h3><div><p>Section 3 content</p></div><h3><a href=\"#\">Section 4</a></h3><div><p>Section 4 content</p></div></div><p>Paragraph after the accordion</p>";
            context.Response.Write(comma);
            context.Response.Write("{");
            context.Response.Write("title:'" + Resource.TemplatejQueryAccordionTitle + "'");
            context.Response.Write(",image:'jquery-accordion.gif'");
            context.Response.Write(",description:''");
            
            context.Response.Write(",html:'" + templateContent + "'");
            context.Response.Write("}");
            comma = ",";

            // jQuery Accordion NoHeight
            templateContent = "<p>Paragraph before the accordion</p><div class=\"C-accordion-nh\"><h3><a href=\"#\">Section 1</a></h3><div><p>Section 1 content.</p></div><h3><a href=\"#\">Section 2</a></h3><div><p>Section 2 content</p></div><h3><a href=\"#\">Section 3</a></h3><div><p>Section 3 content</p></div><h3><a href=\"#\">Section 4</a></h3><div><p>Section 4 content</p></div></div><p>Paragraph after the accordion</p>";
            context.Response.Write(comma);
            context.Response.Write("{");
            context.Response.Write("title:'" + Resource.TemplatejQueryAccordionNoAutoHeightTitle + "'");
            context.Response.Write(",image:'jquery-accordion.gif'");
            context.Response.Write(",description:''");

            context.Response.Write(",html:'" + templateContent + "'");
            context.Response.Write("}");

            
            // jQuery Tabs
            templateContent = "<p>Paragraph before the tabs</p><div class=\"C-tabs\"><ul><li><a href=\"#tab1\">Tab 1</a></li><li><a href=\"#tab2\">Tab 2</a></li><li><a href=\"#tab3\">Tab 3</a></li></ul><div id=\"tab1\"><p>Tab 1 content</p></div><div id=\"tab2\"><p>Tab 2 content</p></div><div id=\"tab3\"><p>Tab 3 content</p></div></div><p>Paragraph after the tabs</p>";
            context.Response.Write(comma);
            context.Response.Write("{");
            context.Response.Write("title:'" + Resource.TemplatejQueryTabsTitle + "'");
            context.Response.Write(",image:'jquerytabs.gif'");
            context.Response.Write(",description:''");

            context.Response.Write(",html:'" + templateContent + "'");
            context.Response.Write("}");

            if (WebConfigSettings.AlwaysLoadYuiTabs)
            {
                // Yui Tabs
                templateContent = "<p>Paragraph before the tabs</p><div class=\"yui-skin-sam\"><div class=\"yui-navset\"><ul class=\"yui-nav\"><li class=\"selected\"><a href=\"#tab1\"><em>Tab One Label</em></a></li><li><a href=\"#tab2\"><em>Tab Two Label</em></a></li><li><a href=\"#tab3\"><em>Tab Three Label</em></a></li></ul><div class=\"yui-content\"><div id=\"tab1\"><p>Tab One Content</p></div><div id=\"tab2\"><p>Tab Two Content</p></div><div id=\"tab3\"><p>Tab Three Content</p></div></div></div></div><p>Paragraph after the tabs</p>";
                context.Response.Write(comma);
                context.Response.Write("{");
                context.Response.Write("title:'" + Resource.TemplateYUITabsTitle + "'");
                context.Response.Write(",image:'yui-tabs.gif'");
                context.Response.Write(",description:''");

                context.Response.Write(",html:'" + templateContent + "'");
                context.Response.Write("}");
            }


            // 3 columns over 1
            templateContent = "<div class=\"floatpanel\"><div class=\"floatpanel section\" style=\"width: 31%;\"><h2>Lorem Ipsum</h2><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent varius varius est, id dictum lectus aliquet non. Fusce laoreet auctor facilisis. Nullam eget tortor at leo pellentesque pellentesque. Nunc tortor neque, elementum varius pretium sit amet, vulputate at erat. Duis nec nisi mauris, in gravida sapien.</p></div><div class=\"floatpanel section\" style=\"width: 31%;\"><h2>Duis a Mauris</h2><p>Duis a mauris non felis dapibus cursus. Aliquam eu dignissim purus. Donec at orci vitae sem laoreet molestie sed eu urna. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus magna velit, fringilla egestas vehicula at, adipiscing eget augue. Suspendisse porta, tellus id consequat volutpat.</p></div><div class=\"floatpanel section\" style=\"width: 31%;\"><h2>Vivamus Tristique!</h2><p>Vivamus tristique purus eget nisl sollicitudin varius. Praesent turpis sapien, imperdiet ut vehicula pretium, tristique nec mauris. Quisque eget lacus mi. Quisque adipiscing velit euismod enim venenatis eleifend. Donec commodo purus non mauris ultricies ultricies. Nulla facilisi.</p></div></div><div class=\"clear section\"><h2>Aenean?</h2><p>Aenean at urna nibh. Aliquam euismod tortor ut mauris eleifend ut vehicula neque convallis. Aenean dui orci, luctus non aliquet eu, semper non arcu. Aliquam tincidunt metus at ligula fringilla ornare. Praesent euismod, lacus vel condimentum convallis, massa quam auctor nisl, ut egestas felis sapien eget augue. Etiam eleifend auctor nunc, id facilisis ante ultrices in. Integer sagittis augue a tortor luctus ut tristique metus sagittis.</p></div><p>new paragraph</p>";
            context.Response.Write(comma);
            context.Response.Write("{");
            context.Response.Write("title:'" + Resource.Template3ColumnsOver1ColumnTitle + "'");
            context.Response.Write(",image:'columns3over1.gif'");
            context.Response.Write(",description:''");

            context.Response.Write(",html:'" + templateContent + "'");
            context.Response.Write("}");

            // 4 columns over 1
            templateContent = "<div class=\"floatpanel\"><div class=\"floatpanel section\" style=\"width: 23%;\"><h2>Lorem Ipsum</h2><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent varius varius est, id dictum lectus aliquet non. Fusce laoreet auctor facilisis. Nullam eget tortor at leo pellentesque pellentesque. Nunc tortor neque, elementum varius pretium sit amet, vulputate at erat. Duis nec nisi mauris, in gravida sapien.</p></div><div class=\"floatpanel section\" style=\"width: 23%;\"><h2>Duis a Mauris</h2><p>Duis a mauris non felis dapibus cursus. Aliquam eu dignissim purus. Donec at orci vitae sem laoreet molestie sed eu urna. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus magna velit, fringilla egestas vehicula at, adipiscing eget augue. Suspendisse porta, tellus id consequat volutpat.</p></div><div class=\"floatpanel section\" style=\"width: 23%;\"><h2>Vivamus Tristique!</h2><p>Vivamus tristique purus eget nisl sollicitudin varius. Praesent turpis sapien, imperdiet ut vehicula pretium, tristique nec mauris. Quisque eget lacus mi. Quisque adipiscing velit euismod enim venenatis eleifend. Donec commodo purus non mauris ultricies ultricies. Nulla facilisi.</p></div><div class=\"floatpanel section\" style=\"width: 23%;\"><h2>Sed Varius</h2><p>Sed varius porta consequat. Proin ante neque, mattis sit amet condimentum in, vulputate ac ipsum. Proin eu consequat est. Integer at vehicula lacus. Nulla faucibus dolor ut augue euismod eget volutpat ligula venenatis. Curabitur bibendum consequat orci, sagittis elementum dolor commodo vel.</p></div></div><div class=\"clear section\"><h2>Aenean?</h2><p>Aenean at urna nibh. Aliquam euismod tortor ut mauris eleifend ut vehicula neque convallis. Aenean dui orci, luctus non aliquet eu, semper non arcu. Aliquam tincidunt metus at ligula fringilla ornare. Praesent euismod, lacus vel condimentum convallis, massa quam auctor nisl, ut egestas felis sapien eget augue. Etiam eleifend auctor nunc, id facilisis ante ultrices in. Integer sagittis augue a tortor luctus ut tristique metus sagittis.</p></div><p>new paragraph</p>";
            context.Response.Write(comma);
            context.Response.Write("{");
            context.Response.Write("title:'" + Resource.Template4ColumnsOver1ColumnTitle + "'");
            context.Response.Write(",image:'columns4over1.gif'");
            context.Response.Write(",description:''");

            context.Response.Write(",html:'" + templateContent + "'");
            context.Response.Write("}");

        }



        


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}
