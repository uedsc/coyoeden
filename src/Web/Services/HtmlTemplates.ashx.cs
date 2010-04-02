// Author:		        Joe Audette
// Created:            2008-01-17
// Last Modified:      2009-06-04
// 


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
    
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class HtmlTemplates : IHttpHandler
    {

        private SiteSettings siteSettings = null;
        private string imagebaseUrl = string.Empty;

        

        public void ProcessRequest(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {
                //TODO: should we return some xml with an error message?
                return;
            }

       
            imagebaseUrl = WebUtils.GetSiteRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) +"/htmltemplateimages/";

            RenderXml(context);
               
        }

        private void RenderXml(HttpContext context)
        {
            context.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, encoding))
            {
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.WriteStartDocument();

                xmlTextWriter.WriteStartElement("Templates");
                xmlTextWriter.WriteAttributeString("imagesBasePath", imagebaseUrl);

                if (WebConfigSettings.AddSystemContentTemplatesAboveSiteTemplates) //true by default
                {
                    RenderSystemTemplates(context, xmlTextWriter);
                }

                RenderSiteTemplates(context, xmlTextWriter);

                if (WebConfigSettings.AddSystemContentTemplatesBelowSiteTemplates) //false by default
                {
                    RenderSystemTemplates(context, xmlTextWriter);
                }
                
                xmlTextWriter.WriteEndElement(); //Templates
     

            }

        }

        private void RenderSiteTemplates(HttpContext context, XmlTextWriter xmlTextWriter)
        {
            List<ContentTemplate> templates = ContentTemplate.GetAll(siteSettings.SiteGuid);

            foreach (ContentTemplate template in templates)
            {
                if (!WebUser.IsInRoles(template.AllowedRoles)) { continue; }
                
                xmlTextWriter.WriteStartElement("Template");
                xmlTextWriter.WriteAttributeString("title", template.Title);
                xmlTextWriter.WriteAttributeString("image", template.ImageFileName);
                xmlTextWriter.WriteStartElement("Description");
                xmlTextWriter.WriteValue(template.Description);
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("Html");
                xmlTextWriter.WriteCData(template.Body);
                xmlTextWriter.WriteEndElement(); //Html
                xmlTextWriter.WriteEndElement(); //Template
                
            }

        }

        private void RenderSystemTemplates(HttpContext context, XmlTextWriter xmlTextWriter)
        {
            //jQuery Accordion
            xmlTextWriter.WriteStartElement("Template");
            xmlTextWriter.WriteAttributeString("title", Resource.TemplatejQueryAccordionTitle);
            xmlTextWriter.WriteAttributeString("image", "jquery-accordion.gif");
            xmlTextWriter.WriteStartElement("Description");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Html");
            string templateContent = @"<p>Paragraph before the accordion</p>
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

            xmlTextWriter.WriteCData(templateContent);
            xmlTextWriter.WriteEndElement(); //Html
            xmlTextWriter.WriteEndElement(); //Template

            //jQuery Accordion no auto height
            xmlTextWriter.WriteStartElement("Template");
            xmlTextWriter.WriteAttributeString("title", Resource.TemplatejQueryAccordionNoAutoHeightTitle);
            xmlTextWriter.WriteAttributeString("image", "jquery-accordion.gif");
            xmlTextWriter.WriteStartElement("Description");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Html");
            templateContent = @"<p>Paragraph before the accordion</p>
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

            xmlTextWriter.WriteCData(templateContent);
            xmlTextWriter.WriteEndElement(); //Html
            xmlTextWriter.WriteEndElement(); //Template

            //jQuery Tabs
            xmlTextWriter.WriteStartElement("Template");
            xmlTextWriter.WriteAttributeString("title", Resource.TemplatejQueryTabsTitle);
            xmlTextWriter.WriteAttributeString("image", "jquerytabs.gif");
            xmlTextWriter.WriteStartElement("Description");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Html");
            templateContent = @"<p>Paragraph before the tabs</p>
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

            xmlTextWriter.WriteCData(templateContent);
            xmlTextWriter.WriteEndElement(); //Html
            xmlTextWriter.WriteEndElement(); //Template

            if (WebConfigSettings.AlwaysLoadYuiTabs)
            {
                //YUI Tabs
                xmlTextWriter.WriteStartElement("Template");
                xmlTextWriter.WriteAttributeString("title", Resource.TemplateYUITabsTitle);
                xmlTextWriter.WriteAttributeString("image", "yui-tabs.gif");
                xmlTextWriter.WriteStartElement("Description");
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("Html");
                templateContent = @"<p>Paragraph before the tabs</p>
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

                xmlTextWriter.WriteCData(templateContent);
                xmlTextWriter.WriteEndElement(); //Html
                xmlTextWriter.WriteEndElement(); //Template

            }


            //3 columns over 1 column
            xmlTextWriter.WriteStartElement("Template");
            xmlTextWriter.WriteAttributeString("title", Resource.Template3ColumnsOver1ColumnTitle);
            xmlTextWriter.WriteAttributeString("image", "columns3over1.gif");
            xmlTextWriter.WriteStartElement("Description");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Html");
            templateContent = @"<div class='floatpanel'>
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

            xmlTextWriter.WriteCData(templateContent);
            xmlTextWriter.WriteEndElement(); //Html
            xmlTextWriter.WriteEndElement(); //Template



            //4 columns over 1 column
            xmlTextWriter.WriteStartElement("Template");
            xmlTextWriter.WriteAttributeString("title", Resource.Template4ColumnsOver1ColumnTitle);
            xmlTextWriter.WriteAttributeString("image", "columns4over1.gif");
            xmlTextWriter.WriteStartElement("Description");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Html");
            templateContent = @"<div class='floatpanel'>
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

            xmlTextWriter.WriteCData(templateContent);
            xmlTextWriter.WriteEndElement(); //Html
            xmlTextWriter.WriteEndElement(); //Template



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
