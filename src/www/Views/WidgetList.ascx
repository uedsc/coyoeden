<%@ Control Language="C#"  Inherits="CoyoEden.UI.Views.WidgetListViewM"%>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="CoyoEden.Core" %>
<%@ Import Namespace="SystemX" %>
<%var editPath = "#"; %>
<div class="ilist_w0 common_sec3 pool10 blank10">
	<ul>
	<% ItemsPaged.ForEach(x=>{ %>
	<%editPath = "#"; if (x.Editable) { editPath = "show.aspx?d={i:'" + x.Id.Value + "'}"; } %>
	<li>
	<div class="i_s100_0">
		<div class="info">
			<p class="cover"><span><a href="<%=editPath %>" title="<%=x.Name %>"><img src="<%=x.Icon %>" alt="<%=x.Name %>"/></a></span></p>
			<div class="detail">
				<p class="name"><a href="<%=editPath %>" title="<%=x.Name %>"><strong><%=x.Title %></strong></a></p>
				<p class="author"><span class="time"><%=x.Tag %></span></p>
				<p class="des">Show title:<%=x.ShowTitle.Value %>,Display Index:<%=x.DisplayIndex %></p>
			</div>
		</div>
		<div class="acts">
	        <span class="actions">
			<%if (x.Editable)
	 { %>
	        <a href="<%=editPath %>" title="<%=x.Name %>">Edit</a>
			<%} %>
		    <a href="#<%=x.Id.Value %>" title="">Delete</a>
		    </span>
		</div>
	</div>
	</li>
	<%}); %>
	</ul>
</div>
<div class="ilist_w1 common_sec3 pool10 blank10" style="display: none;">
	<ul class="clearfix">
	<% ItemsPaged.ForEach(x =>
	{ %>
	<%editPath = "#"; if (x.Editable) { editPath = "show.aspx?d={i:'" + x.Id.Value + "'}"; } %>
	<li>
		<div class="i_s100_1">
			<p class="cover"> <span><a title="<%=x.Name %>" href="<%=editPath %>"><img alt="<%=x.Name %>" src="<%=x.Icon %>"/></a></span> </p>
			<p class="name"><a href="<%=editPath %>" title="<%=x.Name %>"><strong><%=x.Title %></strong></a></p>
			<p class="time"><%=x.Tag %></p>
			<%if (x.Editable)
	 { %>
			<p><a href="<%=editPath %>" title="<%=x.Name %>">Edit</a></p>
			<%} %>
		</div>
	</li>
	<%}); %>
	</ul>
</div>
