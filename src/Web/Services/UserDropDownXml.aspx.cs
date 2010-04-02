/// Author:					Joe Audette
/// Created:				2005-06-19
/// Last Modified:			2007-11-15

using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Xml;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;

namespace Cynthia.Web.Services
{
    
    public partial class UserDropDownXml : Page
    {
        private string query = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(Response.OutputStream, encoding);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("DATA");

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if ((siteSettings != null) && (WebUser.IsAdminOrContentAdmin))
            {

                if (Request.Params.Get("query") != null)
                {
                    query = Request.Params.Get("query");

                    int rowsToGet = 10;

                    using (IDataReader reader = SiteUser.GetSmartDropDownData(siteSettings.SiteId, query, rowsToGet))
                    {
                        while (reader.Read())
                        {
                            xmlTextWriter.WriteStartElement("R");

                            xmlTextWriter.WriteStartElement("V");
                            xmlTextWriter.WriteString(reader["UserID"].ToString());
                            xmlTextWriter.WriteEndElement();

                            xmlTextWriter.WriteStartElement("T");
                            xmlTextWriter.WriteString(reader["SiteUser"].ToString().Trim());
                            xmlTextWriter.WriteEndElement();

                            xmlTextWriter.WriteEndElement();
                        }

                    }
                }
            }

            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();
            xmlTextWriter.Close();

        }
    }
}
