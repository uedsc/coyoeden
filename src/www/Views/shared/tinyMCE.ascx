<%@ Control Language="C#" AutoEventWireup="true" CodeFile="tinyMCE.ascx.cs" Inherits="Views_shared_tinyMCE" %>
<%@ Import Namespace="CoyoEden.Core" %>
<%@ Import Namespace="SystemX" %>
<vs:SiteJScript ID="jquery_tinyMCE" ScriptRelativeToRoot="Assets/editors/tiny_mce/jquery.tinymce.js" runat="server"/>
<vs:SiteJScript ID="htmlEditorJS" ScriptRelativeToRoot="Assets/js/local/app.htmleditor.js" runat="server"/>
<asp:PlaceHolder id="phEditorWrapper" runat="server" Visible="false">
<asp:TextBox runat="Server" ID="txtContent" CssClass="post" Width="100%" Height="250px" TextMode="MultiLine" />
</asp:PlaceHolder>
