<%@ Page Title="XProperties" Language="C#" MasterPageFile="~/themes/admin/site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_XProperties_Default" %>
<%@ Register Src="~/Views/XPropertyList.ascx" TagName="XProList" TagPrefix="vs" %>
<%@ Import Namespace="CoyoEden.Core.DataContracts" %>
<asp:Content ID="Content0" ContentPlaceHolderID="cphHead" Runat="Server">
<vs:SiteCSS ID="cssYPager" runat="server" CSSRelativeToRoot="Assets/css/Vivasky.YPager.css" />
<vs:SiteCSS ID="SiteCSS1" runat="server" CSSRelativeToRoot="themes/admin/xproperty.css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
<div class="c695_left">
	<div class="c695_l_navi">
		<ul class="c695_navigate clearfix">
		<li><a class="current" title="" href="Default.aspx">所有的扩展属性</a></li>
		<li><a title="" href="default.aspx?<%=QStrData.Set("a",1).Set("v",true) %>">在用的扩展属性</a></li>
		<li><a title="" href="default.aspx?<%=QStrData.Set("a",1).Set("v",false) %>">停用的扩展属性</a></li>
		</ul>
	</div>
	<div class="c695_l_toolbar clearfix">
		<span class="counts">共<%=Xprolist.ItemCount %>个</span>
		<div class="p_filter_more clearfix"> 
			<span class="selects">
			<label for="lang">排序:</label>
			<select name="order" id="listOrderType">
				<option selected="" value="1" title="更新时间">默认</option>
				<option value="2">创建时间</option>
				<option value="3">按名称排</option>
			</select>
			</span> 
			<span class="viewmode clearfix"> 
				<a id="showBlock" class="byList" href="javascript:void(0)" title="图片显示">图片显示</a> 
				<a id="showList" class="byBlock_active" href="javascript:void(0)" title="列表显示">列表显示</a> 
			</span> 
		</div>
	</div>
	<div class="c695_l_content ilist_root">
		<vs:XProList ID="Xprolist" runat="server" />
		<vs:YPager ID="yPager1" runat="server" EdgeLinks="1" ItemsPerPage="3" OnPageChanged="yPager1_OnPageChanged" />
	</div>
</div>
<div class="c265_side">
	<div class="c265_side_filter pad20 blank40">
		<h3 class="Ptitle">属性</h3>
		<div class="library_tag_list pool5"> 
			<a title="" href="default.aspx" class="current">总数 <span>(<%=Xprolist.ItemCount %>)</span></a> 
		</div>
	</div>
	<div class="c265_side_filter pad20 blank60">
		<h3 class="Ptitle">标签</h3>
		<div class="library_tag_list pool5"> 
			<a title="" href="default.aspx" class="current">全部</a>
			<a title="" href="default.aspx?<%=QStrData.Set("a",1).Set("t0",DateTime.Now.Year) %>"><%=DateTime.Now.Year %></a>
			<a title="" href="default.aspx?<%=QStrData.Set("a",1).Set("t0",DateTime.Now.AddYears(-1).Year) %>"><%=DateTime.Now.AddYears(-1).Year %></a>
		</div>
	</div>
	<div class="side_actions pad20 blank80"> 
		<a title="" href="show.aspx?<%=QStringData.New("a-2")%>" class="relate_links">新建扩展属性</a>
	</div>
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFooter" Runat="Server">
<vs:SiteJScript ID="jsXproperty" runat="server" ScriptRelativeToRoot="assets/js/local/admin.xproperty.js" />
<script type="text/javascript">
	//<![CDATA[
	XPropertyApp.Init({});
//]]>
</script>
</asp:Content>

