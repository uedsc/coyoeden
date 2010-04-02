<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentSystemSetting.ascx.cs" Inherits="Cynthia.Web.UI.CommentSystemSetting" %>
<asp:DropDownList ID="dd" runat="server" >
    <asp:ListItem Value="internal" Text="<%$ Resources:Resource, CommentSystemInternal %>" />
    <asp:ListItem Value="intensedebate" Text="<%$ Resources:Resource, CommentSystemIntenseDebate %>" />
    <asp:ListItem Value="disqus" Text="<%$ Resources:Resource, CommentSystemDisqus %>" />
</asp:DropDownList>
