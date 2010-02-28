<%@ Control Language="C#" EnableViewState="False" Inherits="CoyoEden.UI.Views.CommentView" %>

<div id="id_<%=Comment.Id %>" class="vcard comment<%= Post.Author.Equals(Comment.Author, StringComparison.OrdinalIgnoreCase) ? " self" : "" %>">
  <p class="date"><%= Comment.DateCreated.Value.ToString("MMMM d. yyyy HH:mm") %></p>
  <p class="gravatar"><%= Gravatar(80)%></p>
  <p class="content"><%= Text%></p>
  <p class="author">
    <%= Comment.Website != null ? "<a href=\"" + Comment.Website + "\" class=\"url fn\">" + Comment.Author + "</a>" : "<span class=\"fn\">" +Comment.Author + "</span>" %>
    <%= Flag %>
    <%= AdminLinks %>
  </p>
</div>