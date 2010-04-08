<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchInput.ascx.cs" Inherits="Cynthia.Web.UI.SearchInput" %>
<% if(LinkOnly){ %>
<%if(UseLeftSeparator){ %>
<span class='accent'>|</span> <a href='<%=SearchUrl %>' class='sitelink' title=""><%=Resource.SearchButtonText %></a>
<%}else if(RenderAsListItem){ %>
<li class='<%=ListItemCss %>'><a href='<%=SearchUrl %>' class='sitelink' title=""><%=Resource.SearchButtonText %></a></li>
<%}else{ %>
 <a href='<%=SearchUrl %>' class='sitelink' title=""><%=Resource.SearchButtonText %></a>
<%} %>
<%}else{ %>
<div id="search-bar">
	<%if (!HideFormTag)
   { %>
	<form action="searchresults.aspx" method="GET" class="searchfrm active" id="searchfrm">
	<%} %>
	<div class="form-container" id="frmC">
		<input type="text" class="text-input sprite" name="q" value="<%=Resource.SearchInputWatermark %>" id="txtSearch" />
		<input type="submit" class="submit sprite" value="<%=Resource.SearchButtonText %>" />
		<div class="fancy-panel sprite clearfix" style="display: none;" id="search_cat">
			<label class="main-cat"><input type="checkbox" name="f" value="00000000-0000-0000-0000-000000000000" class="all-cats" checked="checked" id="cbxscall"><span><%=Resource.SearchAllContentItem%></span></label>
			<ul id="mlist">
			<% SearchableModules.ForEach(x=>{%>
			<li><label><input type="checkbox" disabled="disabled" name="f" value="<%=x.FeatureGuid %>" checked="checked" class="sub-cat"/><span><%=ResourceHelper.GetResourceString(x.ResourceFile,x.SearchListName,true)%></span></label></li>
			<%}); %>
			</ul>
		</div>
	</div>
	<div class="arrow arrow-up sprite" id="search_icon">&nbsp;</div>
	<%if (!HideFormTag)
   { %>
	</form>
	<%} %>
</div><!--/#searchbar-->
<%} %>