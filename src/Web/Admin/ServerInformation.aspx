<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ServerInformation.aspx.cs" Inherits="Cynthia.Web.AdminUI.ServerInformation" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx"
            CssClass="unselectedcrumb" />&nbsp;&gt;
        <asp:HyperLink ID="lnkServerInfo" runat="server" CssClass="selectedcrumb" />
    </div>
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlWrapper" runat="server" CssClass="panelwrapper admin serverinformation yui-skin-sam">
        <div class="modulecontent">
            <h2 class="heading">
                <asp:Literal ID="litHeading" runat="server" /></h2>
            <div class="settingrow">
                <span class="settinglabel"><a href="http://www.Vivasky.com" title="Cynthia Web Site">
                    Cynthia</a>&nbsp;<cy:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="AdminMenuVersionLabel"
                        UseLabelTag="false"> </cy:SiteLabel>
                </span>
                <asp:Literal ID="litCodeVersion" runat="server" />
                <asp:Literal ID="litPlatform" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="AdminMenuServerTimeZoneLabel"
                    UseLabelTag="false"> </cy:SiteLabel>
                <asp:Literal ID="litServerTimeZone" runat="server" /> 
                
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="AdminMenuServerLocalTimeLabel"
                    UseLabelTag="false"> </cy:SiteLabel>
                (<cy:SiteLabel ID="SiteLabel4" runat="server"  ConfigKey="GMT"
                    UseLabelTag="false"> </cy:SiteLabel> <asp:Literal ID="litServerGMTOffset" runat="server" />) <asp:Literal ID="litServerLocalTime" runat="server" /> 
                
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="AdminMenuServerPrefferedGMTOffsetLabel"
                    UseLabelTag="false"> </cy:SiteLabel> 
                (<cy:SiteLabel ID="SiteLabel7" runat="server"  ConfigKey="GMT"
                    UseLabelTag="false"> </cy:SiteLabel>
                <asp:Literal ID="litPreferredGMTOffset" runat="server" />) <asp:Literal ID="litPreferredTime" runat="server" /> 
                <div style="clear:none;">
                <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="preferredtimezone-help"  /></div>
            </div>
             <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel6" runat="server" CssClass="settinglabel" ConfigKey="AdminMenuServerCurrentGMTTimeLabel"
                    UseLabelTag="false"> </cy:SiteLabel>
                <asp:Literal ID="litCurrentGMT" runat="server" />
            </div>
            <asp:Panel ID="pnlFeatureVersions" runat="server" CssClass="settingrow">
            <h2 class="heading">
                <asp:Literal ID="litFeaturesHeading" runat="server" /> <portal:CHelpLink ID="CynHelpLink2" runat="server" HelpKey="featureversion-help"  /></h2>
            <cy:CGridView ID="grdSchemaVersion" runat="server"
	             CssClass=""
	             AutoGenerateColumns="false"
                 DataKeyNames="ApplicationID"
                 EnableTheming="false"
	             >
                 <Columns>
		            <asp:TemplateField>
			            <ItemTemplate>
                            <%# Eval("ApplicationName") %>
                        </ItemTemplate>
		            </asp:TemplateField>
		            <asp:TemplateField>
			            <ItemTemplate>
                            <%# Eval("Major") %>.<%# Eval("Minor") %>.<%# Eval("Build") %>.<%# Eval("Revision") %>
                        </ItemTemplate>
		            </asp:TemplateField>
		            
            </Columns>
            </cy:CGridView>
            
            </asp:Panel>
        </div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
