<%@ Control Language="C#" Inherits="CoyoEden.UI.Views.ViewBase"%>
<%if(!IsDebug){ %>
<vs:SiteJScript ID="vs_core" ScriptRelativeToRoot="Assets/js/released/SystemX.js" runat="server"/>
<%} else{%>
<asp:PlaceHolder ID="phDebugJS" runat="server">
<vs:SiteJScript ID="jquery" ScriptRelativeToRoot="Assets/js/jquery/jquery-1.3.2.min.js" runat="server"/>
<vs:SiteJScript ID="jquery_tools" ScriptRelativeToRoot="Assets/js/jquery/jquery.tools.min.js" runat="server"/>
<vs:SiteJScript ID="jquery_json" ScriptRelativeToRoot="Assets/js/jquery/jquery.json-1.3.min.js" runat="server"/>
<vs:SiteJScript ID="jquery_blockUI" ScriptRelativeToRoot="Assets/js/jquery/jquery.blockUI.js" runat="server"/>
<vs:SiteJScript ID="jquery_string" ScriptRelativeToRoot="Assets/js/jquery/jquery.string.1.0.js" runat="server"/>
<vs:SiteJScript ID="jquery_validate" ScriptRelativeToRoot="Assets/js/jquery/jquery.validate.min.js" runat="server"/>
<vs:SiteJScript ID="jquery_pngfix" ScriptRelativeToRoot="Assets/js/jquery/jquery.ifixpng2.js" runat="server"/>
<vs:SiteJScript ID="jquery_drag" ScriptRelativeToRoot="Assets/js/jquery/jquery.event.drag-1.5.min.js" runat="server"/>
<vs:SiteJScript ID="jquery_jqModal" ScriptRelativeToRoot="Assets/js/jquery/jqModal.js" runat="server"/>
<vs:SiteJScript ID="jquery_jgrowl" ScriptRelativeToRoot="Assets/js/jquery/jquery.jgrowl.min.js" runat="server"/>
<vs:SiteJScript ID="jquery_dateEntry" ScriptRelativeToRoot="Assets/js/jquery/jquery.dateentry.min.js" runat="server"/>
<vs:SiteJScript ID="jquery_countdown" ScriptRelativeToRoot="Assets/js/jquery/jquery.countdown.min.js" runat="server"/>
<vs:SiteJScript ID="date" ScriptRelativeToRoot="Assets/js/date.js" runat="server"/>
<vs:SiteJScript ID="vivasky_com" ScriptRelativeToRoot="Assets/js/Vivasky.com.js" runat="server"/>
<vs:SiteJScript ID="stringutils" ScriptRelativeToRoot="Assets/js/Vivasky.StringUtils.js" runat="server"/>
<vs:SiteJScript ID="local_common" ScriptRelativeToRoot="Assets/js/local/Local.common.js" runat="server"/>
</asp:PlaceHolder>
<%} %>
<script type="text/javascript">
    //<![CDATA[
    LocalApp.Today = '<%=DateTime.Now.ToString("yyyy-MM-dd") %>';
    LocalApp.WebRoot = '<%=AbsoluteWebRoot %>';
    LocalApp.ASPSESSID = '<%=Session.SessionID %>';
    //store the jqModal layout template
    $(".dialog_jqm").data("t", $("#jqModal1 .dialog_main").html());
    $.dateEntry.setDefaults({ spinnerImage: '<%=AbsoluteWebRoot %>assets/img/spinnerDefault.png' });
    //]]>
</script>