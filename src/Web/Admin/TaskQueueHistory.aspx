<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="TaskQueueHistory.aspx.cs" Inherits="Cynthia.Web.AdminUI.TaskQueueHistoryPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkAdvancedTools" runat="server" />&nbsp;&gt;
<asp:HyperLink ID="lnkTaskQueueMonitor" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlTaskQueue" runat="server" CssClass="art-Post-inner panelwrapper admin taskqueue">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent yui-skin-sam">
<cy:CGridView ID="grdTaskQueue" runat="server" 
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
                <asp:ImageButton ID="btnDeletet" runat="server" 
                    CommandName="Delete" ToolTip='<%# Resources.Resource.TaskQueueGridDeleteButton %>'
                    AlternateText='<%# Resources.Resource.TaskQueueGridDeleteButton %>'
                    ImageUrl='<%# DeleteLinkImage %>' />&nbsp;&nbsp;&nbsp;&nbsp;
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("TaskName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("QueuedUTC"), TimeOffset)%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
			<ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("StartUTC"), TimeOffset)%>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("LastStatusUpdateUTC"), TimeOffset)%>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("CompleteUTC"), TimeOffset)%>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# GetPercentComplete(Eval("CompleteRatio"))%>
            </ItemTemplate>
		</asp:TemplateField>
		
    </Columns>
 </cy:CGridView>
<div class="modulepager">
    <portal:CCutePager ID="pgrTaskQueue" runat="server" />
</div>
<div class="settingrow">
	<asp:Label ID="lblStatus" runat="server" />
</div>
<div class="settingrow">
	<br /><asp:HyperLink ID="lnkRefresh" runat="server" />
	<portal:CButton ID="btnClearHistory" runat="server" />
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
