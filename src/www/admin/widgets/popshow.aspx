<%@ Page Language="C#" AutoEventWireup="true" CodeFile="popshow.aspx.cs" Inherits="admin_widgets_popshow" ValidateRequest="false" %>
<%@ Import Namespace="Vivasky.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
  <title>Widget Editor</title>
  <link rel="Stylesheet" rev="Stylesheet" type="text/css" href="<%=Utils.AbsoluteWebRoot%>assets/css/local.widgeteditor.css" />
  <vs:SiteJScript ID="jquery" ScriptRelativeToRoot="Assets/js/jquery/jquery-1.3.2.min.js" runat="server"/>
</head>
<body>
  <form id="form1" runat="server">
    <div id="title">
      <label for="<%=txtTitle.ClientID %>"><%=Resources.labels.title %></label>&nbsp;&nbsp;&nbsp;
      <asp:TextBox runat="server" ID="txtTitle" Width="300px" />
      <asp:CheckBox runat="Server" ID="cbShowTitle" Text="Show title" />
    </div>
    <div runat="server" ID="phEdit" />
    <div id="bottom">
      <asp:Button runat="server" ID="btnSave" Text="Save" /> 
    </div>
    <%if (ShowHtmlEditor)
  { %>
  <vs:TextEditor 
    runat="server" 
    id="txtContent"
    TinyMCEOpts="{
        plugins: 'inlinepopups,fullscreen,contextmenu,emotions,table,iespell',
        theme_advanced_buttons1:'fullscreen,code,|,cut,copy,paste,|,undo,redo,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,bullist,numlist,outdent,indent,|,iespell,link,unlink,sub,sup,removeformat,cleanup,charmap,emotions',
        theme_advanced_source_editor_height: 425
    }"
    />
<%} %>
  </form>
</body>
</html>
