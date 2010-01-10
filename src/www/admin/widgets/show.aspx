<%@ Page Title="Widget Editor" ValidateRequest="false" Language="C#" MasterPageFile="~/themes/admin/site.master" AutoEventWireup="true" CodeFile="show.aspx.cs" Inherits="admin_widgets_show" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" Runat="Server">
<div class="c695_left">
	<div class="c695_l_navi">
		<ul class="c695_navigate clearfix">
		<li><a title="" href="Default.aspx">All widgets</a></li>
		<%CoyoEden.Core.WidgetZone.AllWidgetZones.ForEach(x =>
		{%>
		<li><a title="" href="default.aspx?d=a^1$t0^<%=x.Name %>"><%=x.Name %></a></li>
		<%});%>
		<li><a class="current" title="" href="#">^<%=ViewData.Title %>^</a></li>
		</ul>
	</div>
	<div class="c695_l_toolbar clearfix">
		<label for="<%=txtTitle.ClientID %>"><%=Resources.labels.title %></label>
		<asp:TextBox runat="server" ID="txtTitle" Width="300px" />
		<asp:CheckBox runat="Server" ID="cbShowTitle" Text="Show title" />
	</div>
	<div class="c695_l_content">
		<div runat="server" ID="phEdit" />
	</div>
</div>
<div class="c265_side">
	<div class="c265_side_filter pad20 blank40">
		<h3 class="Ptitle">属性</h3>
		<div class="library_tag_list pool5"> 
			<span class="current">ID:<%=ViewData.Id.Value %></span><br />
			<img alt="" title="" src="<%=ViewData.Icon %>" />
		</div>
	</div>
	<div class="side_actions pad20 blank80"> 
		<asp:Button runat="server" ID="btnSave" Text="Save" />
	</div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" Runat="Server">
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
<vs:SiteJScript ID="appJS" ScriptRelativeToRoot="Assets/js/local/admin.widget.js" runat="server"/>
<script type="text/javascript">
//<![CDATA[
	WidgetApp.Init({
		appTip:'<%=AppTip??"" %>'
	});
//]]>
</script>
</asp:Content>

