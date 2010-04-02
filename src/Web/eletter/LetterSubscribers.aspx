<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="LetterSubscribers.aspx.cs" Inherits="Cynthia.Web.ELetterUI.LetterSubscribersPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
<span id="spnAdmin" runat="server"><asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" />&nbsp;&gt;</span>
<asp:HyperLink ID="lnkLetterAdmin" runat="server" CssClass="unselectedcrumb" />&nbsp;&gt;
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlLetterSubscriberHx" runat="server" CssClass="art-Post-inner panelwrapper lettersubscriber">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent yui-skin-sam">
<asp:Panel ID="pnlSearchSubscribers" runat="server" DefaultButton="btnSearch" CssClass="settingrow subscribersearch">
<asp:TextBox ID="txtSearchInput" runat="server" CssClass="widetextbox" MaxLength="100" />
<portal:CButton ID="btnSearch" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlDeleteUnVerified" runat="server" DefaultButton="btnDeleteUnVerified" CssClass="settingrow subscribersearch">
<portal:CButton ID="btnDeleteUnVerified" runat="server" />
<asp:TextBox ID="txtDaysOld" runat="server" Text="90" CssClass="smalltextbox" MaxLength="3" />
<cy:SiteLabel id="lblDays" runat="server" CssClass="" ConfigKey="NewslettersDays"> </cy:SiteLabel>
</asp:Panel>

<cy:CGridView ID="grdSubscribers" runat="server" 
    AllowPaging="false" 
    AllowSorting="false"
    AutoGenerateColumns="false" 
    CssClass="" 
    DataKeyNames="Guid" 
    EnableTheming="false"
    >
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
            <asp:HyperLink Text='<%# Eval("Name") %>' id="Hyperlink2" 
						    NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx?u=" + Eval("UserGuid")   %>' 
						    Visible='<%# ShowUserLink(Eval("UserGuid").ToString()) %>' runat="server" />
            <asp:Literal ID="litUser" runat="server" Text='<%# Eval("Name") %>' Visible='<%# !ShowUserLink(Eval("UserGuid").ToString()) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("Email")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("UseHtml")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("BeginUtc"), TimeOffset)%>
                <br /> <%# Eval("IpAddress")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("IsVerified")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
        <ItemTemplate>
            <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteSubscriber" CommandArgument='<%# Eval("Guid") %>' ToolTip='<%# Resources.Resource.NewletterSubscriberDeleteButton %>'
                AlternateText='<%# Resources.Resource.NewletterSubscriberDeleteButton %>' ImageUrl='<%# DeleteLinkImage %>' />
        </ItemTemplate>
         </asp:TemplateField>
    </Columns>
 </cy:CGridView>
<div class="settingrow">
   
</div>
<div class="modulepager">
    <portal:CCutePager ID="pgrLetterSubscriber" runat="server" />
</div>
<br class="clear" />
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
