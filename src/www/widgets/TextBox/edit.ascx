<%@ Control Language="C#" AutoEventWireup="true" CodeFile="edit.ascx.cs" Inherits="widgets_TextBox_edit" %>
<%@ Import Namespace="CoyoEden.Core" %>
<%@ Import Namespace="Vivasky.Core" %>
<vs:SiteJScript ID="jquery_tinyMCE" ScriptRelativeToRoot="Assets/editors/tiny_mce/jquery.tinymce.js" runat="server"/>
<script type="text/javascript">
	//<![CDATA[
	$(document).ready(function() {
		$('#<%=txtText.ClientID %>').tinymce({
			script_url:'<%=Utils.AbsoluteWebRoot %>assets/editors/tiny_mce/tiny_mce.js',
			//general options
			theme:'advanced',
			plugins: "inlinepopups,fullscreen,contextmenu,emotions,table,iespell",
			convert_urls:false,
			// Theme options
			theme_advanced_buttons1: "fullscreen,code,|,cut,copy,paste,|,undo,redo,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,bullist,numlist,outdent,indent,|,iespell,link,unlink,sub,sup,removeformat,cleanup,charmap,emotions",
			theme_advanced_buttons2: "",
			theme_advanced_toolbar_location: "top",
			theme_advanced_toolbar_align: "left",
			theme_advanced_statusbar_location: "bottom",
			theme_advanced_resizing: true,
			theme_advanced_source_editor_height: 425,

			tab_focus: ":prev,:next"
		});
	});
	//]]>
</script>
<asp:TextBox runat="server" ID="txtText" TextMode="multiLine" Columns="100" Rows="10" style="width:600px;height:360px" /><br />