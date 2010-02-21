<%@ Control Language="C#" Inherits="CoyoEden.UI.Views.SiteTop"%>
<%@ Import Namespace="SystemX.Web"%>
<!--TODO:Css sprite on SiteTop-->
<div id="toppanelC">
	<div id="toppanel">
		<div id="toppanelBody" class="content clearfix">
		<ul class="tabs clearfix" id="topTabs">
		<li><a href="#" title="Add a Widget">Add Widget</a></li>
		<li><a href="#" title="">Edit widget</a></li>
		</ul>
		<div class="tab_p widgetselector">
			<form id="fmAddWidget" method="post" action="">
			<h1>Add a widget</h1>
			<h2>A,Choose what u wanna add</h2>		
			<p class="grey">
			<vs:SiteDDSelect ID="listWidget" runat="server" Name="WidgetName" Tip="Select a widget" OnLoad="OnWidgetsLoad"/>
			</p>
			<h2>B,Choose where u wanna add</h2>
			<p class="grey">
			<vs:SiteDDSelect ID="listZone" runat="server" Name="WidgetZone" KeyField="Name" DescField="Name" Tip="Select a Zone" OnLoad="OnWidgetZonesLoad"/>
			</p>
			<p>
			<input type="button" id="btnAddWidget" class="bt_silver" value="+Widget"/>
			<input type="hidden" id="txtPageTag" name="WidgetTag" value="<%=Tag %>" />
			</p>
			</form>
		</div>
		<div class="tab_p"><div id="widgetEditor"></div></div>

		</div>
	</div>	
    <!-- The tab on top -->	
	<div class="tab clearfix">
		<ul class="login">
	    	<li class="left"> </li>
	    	<li id="cur_user"><%=UserName??"Hello guest" %></li>
			<li class="sep">|</li>
			<li id="toggle">
				<a href="#" class="open" id="open">Open Panel</a>
				<a href="#" class="close" style="display: none;" id="close">Close Panel</a>			
			</li>
	    	<li class="right"> </li>
		</ul> 
	</div> <!-- / top -->
</div>