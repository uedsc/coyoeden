<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WidgetEditor.aspx.cs" Inherits="User_controls_WidgetEditor" ValidateRequest="false" %>
<%@ Import Namespace="Vivasky.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
  <title>Widget Editor</title>
  <link rel="Stylesheet" rev="Stylesheet" type="text/css" href="<%=Utils.AbsoluteWebRoot%>assets/css/local.widgeteditor.css" />
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
      <input type="button" value="Cancel"/>      
    </div>
  </form>
</body>
</html>
