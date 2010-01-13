<%@ Page Title="" Language="C#" MasterPageFile="~/themes/admin/site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_widgets_Default" %>
<%@ Import Namespace="System.Linq" %>
<%@ Register Src="~/Views/WidgetList.ascx" TagName="WidgetList" TagPrefix="vs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
<vs:SiteCSS ID="cssYPager" runat="server" CSSRelativeToRoot="Assets/css/Vivasky.YPager.css" />
<vs:SiteCSS ID="SiteCSS1" runat="server" CSSRelativeToRoot="themes/admin/xproperty.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" Runat="Server">
<div class="c695_left">
	<div class="c695_l_navi">
		<ul class="c695_navigate clearfix">
		<li><a class="<%=CheckTagCss("","current") %>" title="" href="Default.aspx">All widgets</a></li>
		<%CoyoEden.Core.WidgetZone.AllWidgetZones.ForEach(x =>
		{%>
		<li><a title="" class="<%=CheckTagCss(x.Name,"current") %>" href="default.aspx?d=a^1$t0^<%=x.Name %>"><%=x.Name %></a></li>
		<%});%>
		</ul>
	</div>
	<div class="c695_l_toolbar clearfix">
		<span class="counts">共<%=WidgetList1.ItemCount%>个</span>
		<div class="p_filter_more clearfix"> 
			<span class="selects">
			<label for="lang">排序:</label>
			<select name="order" id="listOrderType">
				<option selected="" value="1" title="按名称排">默认</option>
				<option value="2">按标签排</option>
			</select>
			</span> 
			<span class="viewmode clearfix"> 
				<a id="showBlock" class="byList" href="javascript:void(0)" title="图片显示">图片显示</a> 
				<a id="showList" class="byBlock_active" href="javascript:void(0)" title="列表显示">列表显示</a> 
			</span> 
		</div>
	</div>
	<div class="c695_l_content ilist_root">
		<vs:WidgetList ID="WidgetList1" runat="server" />
		<vs:YPager ID="yPager1" runat="server" EdgeLinks="1" ItemsPerPage="3" OnPageChanged="yPager1_OnPageChanged" />
	</div>
</div>
<div class="c265_side">
	<div class="c265_side_filter pad20 blank40">
		<h3 class="Ptitle">属性</h3>
		<div class="sec_tlist pool5"> 
			<a title="" href="default.aspx" class="current">总数 <span>(<%=WidgetList1.ItemCount%>)</span></a> 
		</div>
	</div>
	<%if (CurZone != null)
   { %>
	<div class="c265_side_filter pad20 blank60">
		<h3 class="Ptitle">新建部件</h3>
		<div class="sec0 sec_tlist pool5" id="widgetAdder"> 
            <vs:SiteDDSelect ID="listWidget" runat="server" Name="WidgetName" Tip="Select a widget" OnLoad="OnWidgetsLoad"/>
			<a title="" id="lnkSave" href="#<%=CurZone.Id.Value %>" rel="<%=CurZone.Id.Value %>">保存</a>
		</div>
	</div>
	<%} %>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" Runat="Server">
<vs:SiteJScript ID="appJS" ScriptRelativeToRoot="Assets/js/local/admin.widget.js" runat="server"/>
<script type="text/javascript">
//<![CDATA[
    WidgetApp.Init({
	    appTip:''
	});
//]]>
</script>
</asp:Content>

