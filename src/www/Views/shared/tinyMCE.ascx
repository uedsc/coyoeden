<%@ Control Language="C#" AutoEventWireup="true" CodeFile="tinyMCE.ascx.cs" Inherits="Views_shared_tinyMCE" %>
<%@ Import Namespace="CoyoEden.Core" %>
<%@ Import Namespace="Vivasky.Core" %>
<vs:SiteJScript ID="jquery_tinyMCE" ScriptRelativeToRoot="Assets/editors/tiny_mce/jquery.tinymce.js" runat="server"/>
<script type="text/javascript">
	//<![CDATA[
	$(document).ready(function() {
		var targetID = '<%=Target.ClientID %>';
		$('#'+targetID).tinymce({
			script_url: '<%=Utils.AbsoluteWebRoot %>assets/editors/tiny_mce/tiny_mce.js',
			// General options
			mode: "exact",
			theme: "advanced",
			plugins: "inlinepopups,fullscreen,contextmenu,emotions,table,iespell,advlink",
			convert_urls: false,

			// Theme options
			theme_advanced_buttons1: "fullscreen,code,|,cut,copy,paste,|,undo,redo,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,bullist,numlist,outdent,indent,|,iespell,link,unlink,sub,sup,removeformat,cleanup,charmap,emotions,|,formatselect,fontselect,fontsizeselect",
			theme_advanced_buttons2: "",
			theme_advanced_toolbar_location: "top",
			theme_advanced_toolbar_align: "left",
			theme_advanced_statusbar_location: "bottom",
			theme_advanced_resizing: true,

			tab_focus: ":prev,:next"
		});
	});
	//]]>
</script>
<asp:PlaceHolder id="phEditorWrapper" runat="server" Visible="false">
<asp:TextBox runat="Server" ID="txtContent" CssClass="post" Width="100%" Height="250px" TextMode="MultiLine" />
</asp:PlaceHolder>
