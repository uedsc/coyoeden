<%@ Control Language="C#" AutoEventWireup="true" CodeFile="htmlEditor.ascx.cs" Inherits="Views_shared_htmlEditor" %>
<%@ Register Src="tinyMCE.ascx" TagName="tinyMCE" TagPrefix="uc1" %>
<%@ Import Namespace="SystemX.Web"%>
<uc1:tinyMCE ID="TinyMCE1" runat="server" />
<script type="text/javascript">
    //<![CDATA[
    HtmlEditor.tempOpts=<%=TinyMCEOpts??"null"%>;
    HtmlEditor.Init({
        hostID:'<%=Target.ClientID %>',
        webRoot:'<%=Utils.AbsoluteWebRoot %>',
        tinymceOpts:HtmlEditor.tempOpts
    });   
	//]]>
</script>