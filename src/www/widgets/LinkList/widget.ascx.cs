#region Using

using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using System.Collections.Specialized;
using CoyoEden.Core;
using CoyoEden.UI.Controls;
#endregion

public partial class widgets_LinkList_widget : WidgetBase
{
	public override void LoadWidget()
	{
    XmlDocument doc = new XmlDocument();

	if (CurrentSettings["content"] != null)
		doc.InnerXml = CurrentSettings["content"];
    
		XmlNodeList links = doc.SelectNodes("//link");

		if (links.Count == 0)
		{
			ulLinks.Visible = false;
		}
		else
		{
			foreach (XmlNode node in links)
			{
				HtmlAnchor a = new HtmlAnchor();
					
				if (node.Attributes["url"] != null)
					a.HRef = node.Attributes["url"].InnerText;

				if (node.Attributes["title"] != null)
					a.InnerText = node.Attributes["title"].InnerText;

				if (node.Attributes["newwindow"] != null && node.Attributes["newwindow"].InnerText.Equals("true", StringComparison.OrdinalIgnoreCase))
					a.Target = "_blank";

				HtmlGenericControl li = new HtmlGenericControl("li");
				li.Controls.Add(a);
				ulLinks.Controls.Add(li);
			}
		}
	}

	public override string Name
	{
		get { return "LinkList"; }
	}

	public override bool IsEditable
	{
		get { return true; }
	}

}
