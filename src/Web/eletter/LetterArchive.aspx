<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="LetterArchive.aspx.cs" Inherits="Cynthia.Web.ELetterUI.LetterArchivePage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
<span id="spnAdmin" runat="server"><asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" />&nbsp;&gt;</span> 
<asp:HyperLink ID="lnkLetterAdmin" runat="server" CssClass="unselectedcrumb" />&nbsp;&gt;
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlLetter" runat="server" CssClass="art-Post-inner panelwrapper newsletteradmin">
<h2 class="moduletitle"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent yui-skin-sam">
<cy:CGridView ID="grdLetter" runat="server" 
    AllowPaging="false" 
    AllowSorting="false"
    AutoGenerateColumns="false" 
    CssClass="" 
    DataKeyNames="LetterGuid" 
    EnableTheming="false"
   >
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HyperLink ID="lnkEdit" runat="server"
                 Text='<%# Resources.Resource.NewsletterEditLink %>' Tooltip='<%# Eval("Subject") %>'
                  NavigateUrl='<%# SiteRoot + "/eletter/LetterEdit.aspx?l=" + Eval("LetterInfoGuid").ToString() + "&letter=" + Eval("LetterGuid").ToString() %>' ></asp:HyperLink>
                  <portal:GreyBoxHyperlink ID="gb1" runat="server" ClientClick="return GB_showFullScreen(this.title, this.href)" NavigateUrl='<%# SiteRoot + "/eletter/LetterView.aspx?l=" + Eval("LetterInfoGuid") + "&letter=" + Eval("LetterGuid") %>'  Text='<%# Resources.Resource.NewsletterViewLink %>' ToolTip='<%# Eval("Subject") %>'  DialogCloseText='<%# Resources.Resource.DialogCloseLink %>' />
                <%# Eval("Subject") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("SendClickedUTC"), TimeOffset)%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("SendCompleteUTC"), TimeOffset)%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                 <asp:HyperLink ID="lnkView" runat="server"
                 Text='<%# Eval("SendCount") %>' Tooltip='<%# Resources.Resource.NewsletterViewSendLogToolTip %>'
                  NavigateUrl='<%# SiteRoot + "/eletter/SendLog.aspx?letter=" + Eval("LetterGuid").ToString() %>' ></asp:HyperLink>   
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
 </cy:CGridView>
<div class="modulepager" >
    <portal:CCutePager ID="pgrLetter" runat="server" />
</div>
<div class="settingrow">
<asp:Button ID="btnPurgeSendLogs" runat="server" />
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
