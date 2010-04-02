// Author:		        Joe Audette
// Created:            2009-08-14
// Last Modified:      2009-08-15
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
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.Services
{
    /// <summary>
    /// When a template guid is passed returns html contentof the template,
    /// otherwise returns a json list of templates
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Index
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Custom_filebrowser
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Configuration
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Plugins/template
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Configuration/theme_advanced_blockformats
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TinyMceTemplates : IHttpHandler
    {
        private SiteSettings siteSettings = null;
        private Guid templateGuid = Guid.Empty;
        private string templateGuidString = string.Empty;
        private string siteRoot = string.Empty;
        private string comma = string.Empty;

        private const string jQueryAccordionGuid = "e110400d-c92d-4d78-a830-236f584af115";
        private const string jQueryAccordionNoHeightGuid = "08e2a92f-d346-416b-b37b-bd82acf51514";
        private const string jQueryTabsGuid = "7efaeb03-a1f9-4b08-9ffd-46237ba993b0";
        private const string YuiTabsGuid = "046dae46-5301-45a5-bcbf-0b87c2d9e919";
        private const string Columns3Over1Guid = "9ac79a8d-7dfd-4485-af3c-b8fdf256bbb8";
        private const string Columns4Over1Guid = "28ae8c68-b619-4e23-8dde-17d0a34ee7c6";

        public void ProcessRequest(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {
                
                return;
            }

            templateGuid = WebUtils.ParseGuidFromQueryString("g", templateGuid);
            if (templateGuid != Guid.Empty) { templateGuidString = templateGuid.ToString(); }

            if (templateGuid != Guid.Empty)
            {
                RenderTemplate(context);

            }
            else
            {
                RenderJsonList(context);

            }

        }

        private void RenderJsonList(HttpContext context)
        {
            context.Response.ContentType = "text/javascript";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            siteRoot = SiteUtils.GetNavigationSiteRoot();

            context.Response.Write("var tinyMCETemplateList = [");

            if (WebConfigSettings.AddSystemContentTemplatesAboveSiteTemplates) //true by default
            {
                RenderSystemTemplateItems(context);
            }

            List<ContentTemplate> templates = ContentTemplate.GetAll(siteSettings.SiteGuid);
            foreach (ContentTemplate t in templates)
            {
                context.Response.Write(comma);
                context.Response.Write("['" + t.Title + "','" + siteRoot + "/Services/TinyMceTemplates.ashx?g=" + t.Guid.ToString() + "','']");
                comma = ",";


            }
            
            if (WebConfigSettings.AddSystemContentTemplatesBelowSiteTemplates) //false by default
            {
                RenderSystemTemplateItems(context);
            }

            context.Response.Write("];");


        }


        private void RenderSystemTemplateItems(HttpContext context)
        {
            context.Response.Write(comma);
            context.Response.Write("['" + Resource.TemplatejQueryAccordionTitle + "','" + siteRoot + "/Services/TinyMceTemplates.ashx?g=e110400d-c92d-4d78-a830-236f584af115','']");
            comma = ",";


            context.Response.Write(comma);
            context.Response.Write("['" + Resource.TemplatejQueryAccordionNoAutoHeightTitle + "','" + siteRoot + "/Services/TinyMceTemplates.ashx?g=08e2a92f-d346-416b-b37b-bd82acf51514','']");

            context.Response.Write(comma);
            context.Response.Write("['" + Resource.TemplatejQueryTabsTitle + "','" + siteRoot + "/Services/TinyMceTemplates.ashx?g=7efaeb03-a1f9-4b08-9ffd-46237ba993b0','']");

            if (WebConfigSettings.AlwaysLoadYuiTabs)
            {
                context.Response.Write(comma);
                context.Response.Write("['" + Resource.TemplateYUITabsTitle + "','" + siteRoot + "/Services/TinyMceTemplates.ashx?g=046dae46-5301-45a5-bcbf-0b87c2d9e919','']");
            }


            context.Response.Write(comma);
            context.Response.Write("['" + Resource.Template3ColumnsOver1ColumnTitle + "','" + siteRoot + "/Services/TinyMceTemplates.ashx?g=9ac79a8d-7dfd-4485-af3c-b8fdf256bbb8','']");

            context.Response.Write(comma);
            context.Response.Write("['" + Resource.Template4ColumnsOver1ColumnTitle + "','" + siteRoot + "/Services/TinyMceTemplates.ashx?g=28ae8c68-b619-4e23-8dde-17d0a34ee7c6','']");


        }



        private void RenderTemplate(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            if (IsSystemTemplateGuid())
            {
                context.Response.Write(GetSystemTemplate());
            }
            else
            {
                ContentTemplate template = ContentTemplate.Get(templateGuid);
                if (template != null)
                {
                    context.Response.Write(template.Body);

                }

            }

        }

        private string GetSystemTemplate()
        {
            switch (templateGuidString)
            {
                case jQueryAccordionGuid:

                    return @"<p>Paragraph before the accordion</p>
			<div class='C-accordion'>
			<h3><a href='#'>Section 1</a></h3>
			<div>
			<p>Section 1 content.</p>
			</div>
			<h3><a href='#'>Section 2</a></h3>
			<div>
			<p>Section 2 content</p>
			</div>
			<h3><a href='#'>Section 3</a></h3>
			<div>
			<p>Section 3 content</p>
			</div>
			<h3><a href='#'>Section 4</a></h3>
			<div>
			<p>Section 4 content</p>
			</div>
			</div>
			<p>Paragraph after the accordion</p>";

                case jQueryAccordionNoHeightGuid:

                    return @"<p>Paragraph before the accordion</p>
			<div class='C-accordion-nh'>
			<h3><a href='#'>Section 1</a></h3>
			<div>
			<p>Section 1 content.</p>
			</div>
			<h3><a href='#'>Section 2</a></h3>
			<div>
			<p>Section 2 content</p>
			</div>
			<h3><a href='#'>Section 3</a></h3>
			<div>
			<p>Section 3 content</p>
			</div>
			<h3><a href='#'>Section 4</a></h3>
			<div>
			<p>Section 4 content</p>
			</div>
			</div>
			<p>Paragraph after the accordion</p>";


                case jQueryTabsGuid:

                    return @"<p>Paragraph before the tabs</p>
			<div class='C-tabs'>
				<ul>
					<li><a href='#tab1'>Tab 1</a></li>
					<li><a href='#tab2'>Tab 2</a></li>
					<li><a href='#tab3'>Tab 3</a></li>
				</ul>
				<div id='tab1'>
					<p>Tab 1 content</p>
				</div>
				<div id='tab2'>
					<p>Tab 2 content</p>
				</div>
				<div id='tab3'>
					<p>Tab 3 content</p>
				</div>
			</div>
			<p>Paragraph after the tabs</p>";

                case YuiTabsGuid:

                    return @"<p>Paragraph before the tabs</p>
			<div class='yui-skin-sam'>
			<div class='yui-navset'>
				<ul class='yui-nav'>
					<li class='selected'><a href='#tab1'><em>Tab One Label</em></a></li>
					<li><a href='#tab2'><em>Tab Two Label</em></a></li>
					<li><a href='#tab3'><em>Tab Three Label</em></a></li>
				</ul>            
				<div class='yui-content'>
					<div id='tab1'><p>Tab One Content</p></div>
					<div id='tab2'><p>Tab Two Content</p></div>
					<div id='tab3'><p>Tab Three Content</p></div>
				</div>
			</div>
			</div>
			<p>Paragraph after the tabs</p>";

                case Columns3Over1Guid:

                    return @"<div class='floatpanel'>
<div class='floatpanel section' style='width: 31%;'>
<h2>Lorem Ipsum</h2>
<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent varius varius est, id dictum lectus aliquet non. Fusce laoreet auctor facilisis. Nullam eget tortor at leo pellentesque pellentesque. Nunc tortor neque, elementum varius pretium sit amet, vulputate at erat. Duis nec nisi mauris, in gravida sapien.</p>
</div>
<div class='floatpanel section' style='width: 31%;'>
<h2>Duis a Mauris</h2>
<p>Duis a mauris non felis dapibus cursus. Aliquam eu dignissim purus. Donec at orci vitae sem laoreet molestie sed eu urna. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus magna velit, fringilla egestas vehicula at, adipiscing eget augue. Suspendisse porta, tellus id consequat volutpat.</p>
</div>
<div class='floatpanel section' style='width: 31%;'>
<h2>Vivamus Tristique!</h2>
<p>Vivamus tristique purus eget nisl sollicitudin varius. Praesent turpis sapien, imperdiet ut vehicula pretium, tristique nec mauris. Quisque eget lacus mi. Quisque adipiscing velit euismod enim venenatis eleifend. Donec commodo purus non mauris ultricies ultricies. Nulla facilisi.</p>
</div>
</div>
<div class='clear section'>
<h2>Aenean?</h2>
<p>Aenean at urna nibh. Aliquam euismod tortor ut mauris eleifend ut vehicula neque convallis. Aenean dui orci, luctus non aliquet eu, semper non arcu. Aliquam tincidunt metus at ligula fringilla ornare. Praesent euismod, lacus vel condimentum convallis, massa quam auctor nisl, ut egestas felis sapien eget augue. Etiam eleifend auctor nunc, id facilisis ante ultrices in. Integer sagittis augue a tortor luctus ut tristique metus sagittis.</p>
</div>
<p>new paragraph</p>";

                case Columns4Over1Guid:

                    return @"<div class='floatpanel'>
<div class='floatpanel section' style='width: 23%;'>
<h2>Lorem Ipsum</h2>
<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent varius varius est, id dictum lectus aliquet non. Fusce laoreet auctor facilisis. Nullam eget tortor at leo pellentesque pellentesque. Nunc tortor neque, elementum varius pretium sit amet, vulputate at erat. Duis nec nisi mauris, in gravida sapien.</p>
</div>
<div class='floatpanel section' style='width: 23%;'>
<h2>Duis a Mauris</h2>
<p>Duis a mauris non felis dapibus cursus. Aliquam eu dignissim purus. Donec at orci vitae sem laoreet molestie sed eu urna. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus magna velit, fringilla egestas vehicula at, adipiscing eget augue. Suspendisse porta, tellus id consequat volutpat.</p>
</div>
<div class='floatpanel section' style='width: 23%;'>
<h2>Vivamus Tristique!</h2>
<p>Vivamus tristique purus eget nisl sollicitudin varius. Praesent turpis sapien, imperdiet ut vehicula pretium, tristique nec mauris. Quisque eget lacus mi. Quisque adipiscing velit euismod enim venenatis eleifend. Donec commodo purus non mauris ultricies ultricies. Nulla facilisi.</p>
</div>
<div class='floatpanel section' style='width: 23%;'>
<h2>Sed Varius</h2>
<p>Sed varius porta consequat. Proin ante neque, mattis sit amet condimentum in, vulputate ac ipsum. Proin eu consequat est. Integer at vehicula lacus. Nulla faucibus dolor ut augue euismod eget volutpat ligula venenatis. Curabitur bibendum consequat orci, sagittis elementum dolor commodo vel.</p>
</div>
</div>
<div class='clear section'>
<h2>Aenean?</h2>
<p>Aenean at urna nibh. Aliquam euismod tortor ut mauris eleifend ut vehicula neque convallis. Aenean dui orci, luctus non aliquet eu, semper non arcu. Aliquam tincidunt metus at ligula fringilla ornare. Praesent euismod, lacus vel condimentum convallis, massa quam auctor nisl, ut egestas felis sapien eget augue. Etiam eleifend auctor nunc, id facilisis ante ultrices in. Integer sagittis augue a tortor luctus ut tristique metus sagittis.</p>
</div>
<p>new paragraph</p>";


            }

            return string.Empty;
        }

        private bool IsSystemTemplateGuid()
        {
            if (templateGuidString == jQueryAccordionGuid) { return true; }
            if (templateGuidString == jQueryAccordionNoHeightGuid) { return true; }
            if (templateGuidString == jQueryTabsGuid) { return true; }
            if (templateGuidString == YuiTabsGuid) { return true; }
            if (templateGuidString == Columns3Over1Guid) { return true; }
            if (templateGuidString == Columns4Over1Guid) { return true; }


            return false;
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
