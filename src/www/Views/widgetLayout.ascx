<%@ Control Language="C#" Inherits="CoyoEden.UI.Views.WidgetLayoutViewM"%>
<%var colorCss = " color-" + Color; %>
<%var movableCss = Movable ? " widget-m" : "";%>
<li id="<%=Id %>" class="widget <%=Name %><%=colorCss%>">
	<div class="widget-head<%=movableCss %>">
	<%if(Collapsable){ %>
		<a class="action collapse" href="#">COLLAPSE</a>
	<%} %>
	<%if (ShowTitle)
   { %>
		<h3 class="title"><%=Title%></h3>
	<%} %>
	<%if (Deletable)
   { %>
		<a class="action remove" href="#">CLOSE</a>
	<%} %>
	<%if (Editable)
   { %>
		<a class="action edit" href="#">EDIT</a>
	<%} %>
	</div>
	<div class="widget-content"><asp:PlaceHolder id="phWidgetCore" runat="server"></asp:PlaceHolder></div>
</li>

