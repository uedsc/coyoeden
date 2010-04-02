<%@ Page CodeBehind="EditAccessDenied.aspx.cs" Language="c#" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="Cynthia.Web.UI.Pages.EditAccessDenied" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="accessdenied">
        <h2>
            <cy:SiteLabel ID="lblEditAccessDeniedLabel" runat="server" ConfigKey="EditAccessDeniedLabel"
                CssClass="txterror" UseLabelTag="false"> </cy:SiteLabel>
        </h2>
        <p>
            <asp:HyperLink ID="lnkHome" runat="server" />
        </p>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
