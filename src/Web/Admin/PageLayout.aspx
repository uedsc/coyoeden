<%@ Page Language="c#" CodeBehind="PageLayout.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="Cynthia.Web.AdminUI.PageLayout" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlContent" runat="server" Visible="False" DefaultButton="btnCreateNewContent"
        CssClass="panelwrapper admin pagelayout">
        <div class="modulecontent">
            <fieldset class="pagelayout">
                <legend>
                    <cy:SiteLabel ID="Sitelabel1" runat="server" ConfigKey="PageLayoutContentLabel">
                    </cy:SiteLabel>
                    <asp:Label ID="lblPageName" runat="server"></asp:Label>
                </legend>
                <div class="breadcrumbs pageditlinks">
                    <asp:HyperLink ID="lnkEditSettings" EnableViewState="false" runat="server" />&nbsp;
                    <asp:HyperLink ID="lnkViewPage" runat="server" EnableViewState="false"></asp:HyperLink>&nbsp;
                    <asp:HyperLink ID="lnkPageTree" runat="server" />
                </div>
                <asp:UpdatePanel ID="upLayout" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="settings">
                            <strong>
                                <cy:SiteLabel ID="lblAddModule" runat="server" ConfigKey="PageLayoutAddModuleLabel"
                                    UseLabelTag="false"> </cy:SiteLabel>
                            </strong>
                            <div class="settingrow">
                                <cy:SiteLabel ID="lblModuleType" runat="server" ForControl="moduleType" CssClass="settinglabel"
                                    ConfigKey="PageLayoutModuleTypeLabel"> </cy:SiteLabel>
                                <asp:DropDownList ID="moduleType" runat="server" EnableTheming="false" CssClass="forminput"
                                    DataValueField="ModuleDefID" DataTextField="FeatureName">
                                </asp:DropDownList>
                                <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="pagelayoutmoduletypehelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="lblModuleName" runat="server" ForControl="moduleTitle" CssClass="settinglabel"
                                    ConfigKey="PageLayoutModuleNameLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="moduleTitle" runat="server" CssClass="widetextbox forminput" Text=""
                                    EnableViewState="false"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink2" runat="server" HelpKey="pagelayoutmodulenamehelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel2" runat="server" ForControl="ddPaneNames" CssClass="settinglabel"
                                    ConfigKey="PageLayoutLocationLabel"> </cy:SiteLabel>
                                <asp:DropDownList ID="ddPaneNames" runat="server" EnableTheming="false" CssClass="forminput"
                                    DataTextField="key" DataValueField="value">
                                </asp:DropDownList>
                                <portal:CHelpLink ID="CynHelpLink3" runat="server" HelpKey="pagelayoutmodulelocationhelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="lblOrganizeModules" runat="server" CssClass="settinglabel" ConfigKey="EmptyLabel"
                                    UseLabelTag="false"> </cy:SiteLabel>
                                <portal:CButton ID="btnCreateNewContent" runat="server" CssClass="forminput" />
                            </div>
                            <div class="settingrow pageditnotes">
                                <asp:Literal ID="litEditNotes" runat="server" />
                            </div>
                            <div class="settingrow">
                                &nbsp;</div>
                            <table border="0">
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td valign="top">
                                        <table cellspacing="0" cellpadding="2" border="0" class="layoutleft">
                                            <tr>
                                                <td align="center">
                                                    <h2>
                                                        <cy:SiteLabel ID="lblLeftPane" runat="server" ConfigKey="PageLayoutLeftPaneLabel"
                                                            UseLabelTag="false"> </cy:SiteLabel>
                                                    </h2>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td>
                                                    <table cellspacing="0" cellpadding="3" border="0">
                                                        <tr valign="top">
                                                            <td rowspan="6">
                                                                <asp:ListBox ID="leftPane" runat="server" DataValueField="ModuleID" DataTextField="ModuleTitle"
                                                                    Width="170" Rows="12"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="LeftUpBtn" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                    CommandName="up" CommandArgument="LeftPane"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="LeftRightBtn" runat="server" ImageUrl="~/Data/SiteImages/rt.gif"
                                                                    CommandName="right"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="LeftDownBtn" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                    CommandName="down" CommandArgument="LeftPane"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="LeftEditBtn" runat="server" I CommandName="edit" CommandArgument="LeftPane">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="LeftDeleteBtn" runat="server" CommandName="delete" CommandArgument="LeftPane">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table cellspacing="0" cellpadding="2" border="0" class="layoutcenter">
                                            <tr>
                                                <td align="center">
                                                    <h2>
                                                        <cy:SiteLabel ID="lblContentPane" runat="server" ConfigKey="PageLayoutContentPaneLabel"
                                                            UseLabelTag="false"> </cy:SiteLabel>
                                                    </h2>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table cellspacing="0" cellpadding="3" border="0">
                                                        <tr valign="top">
                                                            <td rowspan="8">
                                                                <asp:ListBox ID="contentPane" runat="server" DataValueField="ModuleID" DataTextField="ModuleTitle"
                                                                    Width="170" Rows="12" ></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="ContentUpBtn" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                    CommandName="up" CommandArgument="ContentPane"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="ContentLeftBtn" runat="server" ImageUrl="~/Data/SiteImages/lt.gif">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="ContentRightBtn" runat="server" ImageUrl="~/Data/SiteImages/rt.gif">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="ContentDownBtn" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                    CommandName="down" CommandArgument="ContentPane"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="ContentEditBtn" runat="server" 
                                                                    CommandName="edit" CommandArgument="ContentPane"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="ContentDeleteBtn" runat="server" 
                                                                    CommandName="delete" CommandArgument="ContentPane"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="bottom">
                                                                <asp:ImageButton ID="ContentDownToNextButton" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                    CommandName="downtoalt1" CommandArgument="ContentPane"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table cellspacing="0" cellpadding="2" border="0" class="layoutright">
                                            <tr>
                                                <td align="center">
                                                    <h2>
                                                        <cy:SiteLabel ID="lblRightPane" runat="server" ConfigKey="PageLayoutRightPaneLabel"
                                                            UseLabelTag="false"> </cy:SiteLabel>
                                                    </h2>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td>
                                                    <table cellspacing="0" cellpadding="3" border="0">
                                                        <tr valign="top">
                                                            <td rowspan="6">
                                                                <asp:ListBox ID="rightPane" runat="server" DataValueField="ModuleID" DataTextField="ModuleTitle"
                                                                    Width="170" Rows="12" ></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="RightUpBtn" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                    CommandName="up" CommandArgument="RightPane"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="RightLeftBtn" runat="server" ImageUrl="~/Data/SiteImages/lt.gif">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="RightDownBtn" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                    CommandName="down" CommandArgument="RightPane"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="RightEditBtn" runat="server" 
                                                                    CommandName="edit" CommandArgument="RightPane"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="RightDeleteBtn" runat="server" 
                                                                    CommandName="delete" CommandArgument="RightPane"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trAltInfo" runat="server">
                                    <td colspan="4">
                                        <cy:SiteLabel ID="SiteLabel5" runat="server" ConfigKey="PageLayoutAltPanelInfo" UseLabelTag="false">
                                        </cy:SiteLabel>
                                    </td>
                                </tr>
                                <tr id="trAltPanel1" runat="server">
                                    <td colspan="2">
                                    </td>
                                    <td colspan="2">
                                        <table cellspacing="0" cellpadding="2" border="0">
                                            <tr>
                                                <td align="center">
                                                    <h2>
                                                        <cy:SiteLabel ID="SiteLabel3" runat="server" ConfigKey="PageLayoutAltPanel1Label"
                                                            UseLabelTag="false"> </cy:SiteLabel>
                                                    </h2>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td>
                                                    <table cellspacing="0" cellpadding="3" border="0">
                                                        <tr valign="top">
                                                            <td rowspan="7">
                                                                <asp:ListBox ID="lbAltContent1" runat="server" DataValueField="ModuleID" DataTextField="ModuleTitle"
                                                                    Width="170" Rows="10"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="btnMoveAlt1ToCenter" runat="server" ImageUrl="~/Data/SiteImages/up.gif">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="btnAlt1MoveUp" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                    CommandName="up" CommandArgument="altcontent1"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="btnAlt1MoveDown" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                    CommandName="down" CommandArgument="altcontent1"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="btnEditAlt1" runat="server" 
                                                                    CommandName="edit" CommandArgument="lbAltContent1"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="btnDeleteAlt1" runat="server" 
                                                                    CommandName="delete" CommandArgument="lbAltContent1"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="btnMoveAlt1ToAlt2" runat="server" ImageUrl="~/Data/SiteImages/dn.gif">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trAltPanel2" runat="server">
                                    <td colspan="2">
                                    </td>
                                    <td colspan="2">
                                        <table cellspacing="0" cellpadding="2" border="0">
                                            <tr>
                                                <td align="center">
                                                    <h2>
                                                        <cy:SiteLabel ID="SiteLabel4" runat="server" ConfigKey="PageLayoutAltPanel2Label"
                                                            UseLabelTag="false"> </cy:SiteLabel>
                                                    </h2>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td>
                                                    <table cellspacing="0" cellpadding="3" border="0">
                                                        <tr valign="top">
                                                            <td rowspan="6">
                                                                <asp:ListBox ID="lbAltContent2" runat="server" DataValueField="ModuleID" DataTextField="ModuleTitle"
                                                                    Width="170" Rows="9"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="btnMoveAlt2ToAlt1" runat="server" ImageUrl="~/Data/SiteImages/up.gif">
                                                                </asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="btnAlt2MoveUp" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                    CommandName="up" CommandArgument="AltContent2"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="btnAlt2MoveDown" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                    CommandName="down" CommandArgument="AltContent2"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="btnEditAlt2" runat="server" 
                                                                    CommandName="edit" CommandArgument="lbAltContent2"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="btnDeleteAlt2" runat="server" 
                                                                    CommandName="delete" CommandArgument="lbAltContent2"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
