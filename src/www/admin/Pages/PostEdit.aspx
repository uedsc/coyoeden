<%@ Page Language="C#" MasterPageFile="~/themes/admin/site.master" AutoEventWireup="true" CodeFile="PostEdit.aspx.cs" Inherits="PostEdit" ValidateRequest="False" EnableSessionState="True" %>
<%@ Import Namespace="CoyoEden.Core" %>
<asp:Content ID="Content0" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">

<div id="tagselector" style="display: none">
    <a href="#" style="color:Black;float:right" id="lnkTagsCloser">Close</a>
    <div style="clear:both"></div>
    <asp:PlaceHolder runat="server" ID="phTags" />
    <div style="clear:both"></div>
  </div>
  <div id="divDrafts" runat="server" visible="False" enableviewstate="False" style="margin-bottom: 10px">
    <a id="aDrafts" runat="server" href="#" />
    <ul id="ulDrafts" runat="server" style="display:none;list-style-type:circle" />
  </div>

  <label for="<%=txtTitle.ClientID %>"><%=Resources.labels.title %></label>
  <asp:TextBox runat="server" ID="txtTitle" Width="450px" />&nbsp;&nbsp;&nbsp;
  
  <label for="<%=ddlAuthor.ClientID %>"><%=Resources.labels.author %></label>
  <asp:DropDownList runat="Server" ID="ddlAuthor" />&nbsp;&nbsp;&nbsp;
  
  <label for="<%=txtDate.ClientID %>"><%=Resources.labels.date %></label>
  <asp:TextBox runat="server" ID="txtDate" Width="110px" />
  
  
  <asp:CheckBox runat="server" ID="cbUseRaw" Text="Use HTML editor" AutoPostBack="true" />
  
  <asp:RegularExpressionValidator runat="server" ControlToValidate="txtDate" ValidationExpression="[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9] [0-9][0-9]:[0-9][0-9]" ErrorMessage="Please enter a valid date (yyyy-mm-dd hh:mm)" Display="dynamic" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDate" ErrorMessage="Please enter a date (yyyy-mm-dd hh:mm)" Display="Dynamic" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" ErrorMessage="Please enter a title" Display="Dynamic" />
  <br /><br />
  <asp:TextBox runat="Server" ID="txtEditorHost" CssClass="post" Width="100%" Height="250px" TextMode="MultiLine" />
  <asp:TextBox runat="server" ID="txtRawContent" Width="100%" TextMode="multiLine" Height="300px" Visible="false" />
  <br />
  
  <table id="entrySettings">
    <tr>
      <td class="label"><%=Resources.labels.uploadImage %></td>
      <td>
        <asp:FileUpload runat="server" ID="txtUploadImage" Width="400" size="50" ValidationGroup="imageupload" />
        <asp:Button runat="server" ID="btnUploadImage" Text="Upload" ValidationGroup="imageupload" />
        <asp:RequiredFieldValidator runat="Server" ControlToValidate="txtUploadImage" ErrorMessage="<%$ Resources:labels, required %>" ValidationGroup="imageupload" />
      </td>
    </tr>
    <tr>
      <td class="label"><%=Resources.labels.uploadFile %></td>
      <td>
        <asp:FileUpload runat="server" ID="txtUploadFile" Width="400" size="50" />
        <asp:Button runat="server" ID="btnUploadFile" Text="Upload" ValidationGroup="fileUpload" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUploadFile" ErrorMessage="<%$ Resources:labels, required %>" ValidationGroup="fileUpload" />
      </td>
    </tr>    
    <tr>
      <td class="label">Slug (optional)</td>
      <td>
        <asp:TextBox runat="server" ID="txtSlug" Width="400" />
        <a href="#" id="lnkGetSlug" title="">Extract from title</a>
      </td>
    </tr>
    <tr>
      <td class="label"><%=Resources.labels.description %></td>
      <td><asp:TextBox runat="server" ID="txtDescription" TextMode="multiLine" Columns="50" Rows="3" Width="400" Height="32px" /></td>
    </tr>
    <tr>
      <td class="label"><%=Resources.labels.categories %></td>
      <td>
        <asp:TextBox runat="server" ID="txtCategory" ValidationGroup="category" />
        <asp:Button runat="server" ID="btnCategory" Text="<%$ Resources:labels, add %>" ValidationGroup="category" />
        <asp:CustomValidator runat="Server" ID="valExist" ValidationGroup="category" ControlToValidate="txtCategory" ErrorMessage="The category already exist" Display="dynamic" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCategory" ErrorMessage="Required" ValidationGroup="category" /><br />
        <div style="width:400px">
        <asp:CheckBoxList runat="server" Width="400" ID="cblCategories" CssClass="cblCategories" RepeatLayout="flow" RepeatDirection="Horizontal" />
        </div>
      </td>
    </tr>
    <tr>
      <td class="label">Tags</td>
      <td>
        <asp:TextBox runat="server" ID="txtTags" Width="400" />
        <a href="#" id="lnkShowTags">Show selector</a>
        <span><%=Resources.labels.separateTagsWitComma %></span>
      </td>
    </tr>
    <tr>
      <td class="label"><%=Resources.labels.settings %></td>
      <td>
        <asp:CheckBox runat="server" ID="cbEnableComments" Text="<%$ Resources:labels, enableComments %>" Checked="true" />
        <asp:CheckBox runat="server" ID="cbPublish" Text="<%$ Resources:labels, publish %>" Checked="true" />
      </td>
    </tr>
  </table>  
  
  <div style="text-align:right">  
    <asp:Button runat="server" ID="btnSave" />
  </div>
  <br />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFooter" Runat="Server">
<vs:TextEditor runat="server" id="txtContent"/>
<vs:SiteJScript ID="appJS1" ScriptRelativeToRoot="Assets/js/local/admin.postedit.js" runat="server"/>
<script type="text/javascript">
	//<![CDATA[
	PostEditApp.Init({
		ulDraftsID: '<%=ulDrafts.ClientID%>',
		txtTitleID: '<%=txtTitle.ClientID %>',
		txtSlugID: '<%=txtSlug.ClientID %>',
		txtRawContentID: '<%=txtRawContent.ClientID %>',
		txtDescriptionID: '<%=txtDescription.ClientID %>',
		txtTagsID: '<%=txtTags.ClientID %>',
		tagselectorID: 'tagselector',
		lnkGetSlugID: 'lnkGetSlug',
		id: '<%=Request.QueryString["id"] %>'
	});
	//]]>
</script>
</asp:Content>

