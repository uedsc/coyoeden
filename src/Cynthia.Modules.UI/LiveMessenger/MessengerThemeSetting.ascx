<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessengerThemeSetting.ascx.cs" Inherits="Cynthia.Modules.UI.LiveMessenger.MessengerThemeSetting" %>
<asp:DropDownList ID="ddTheme" runat="server" >
    <asp:ListItem Value="" Text="<%$ Resources:LiveResources, MessengerNoTheme %>" />
    <asp:ListItem Value="blue" Text="<%$ Resources:LiveResources, MessengerBlueTheme %>" />
    <asp:ListItem Value="green" Text="<%$ Resources:LiveResources, MessengerGreenTheme %>" />
    <asp:ListItem Value="orange" Text="<%$ Resources:LiveResources, MessengerOrangeTheme %>" />
    <asp:ListItem Value="pink" Text="<%$ Resources:LiveResources, MessengerPinkTheme %>" />
    <asp:ListItem Value="purple" Text="<%$ Resources:LiveResources, MessengerPurpleTheme %>" />
    <asp:ListItem Value="gray" Text="<%$ Resources:LiveResources, MessengerGrayTheme %>" />
</asp:DropDownList>