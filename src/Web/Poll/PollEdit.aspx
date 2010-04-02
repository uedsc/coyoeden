<%@ Page ValidateRequest="false" Language="c#" MaintainScrollPositionOnPostback="true"
    Codebehind="PollEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="PollFeature.UI.PollEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkPolls" CssClass="unselectedcrumb"></asp:HyperLink>
</div>

<cy:CornerRounderTop id="ctop1" runat="server" />
    <asp:Panel ID="pnlPoll" runat="server" DefaultButton="btnSave" CssClass="panelwrapper poll">
    <div class="modulecontent">
        <fieldset>
            <legend>
                <cy:SiteLabel ID="lblEditPoll" runat="server" ConfigKey="PollEditLabel" ResourceFile="PollResources"
                    UseLabelTag="false"> </cy:SiteLabel>
            </legend>
            <div class="modulecontent">
                <asp:Button ID="btnAddNewPoll" runat="server" CssClass="buttonlink" CausesValidation="false" />
                <asp:Button ID="btnViewPolls" runat="server" CssClass="buttonlink" CausesValidation="false" />
                <hr />
                <div class="settingrow">
                    <cy:SiteLabel ID="lblQuestion" runat="server" ForControl="txtQuestion" CssClass="settinglabel"
                        ConfigKey="PollEditQuestionLabel" ResourceFile="PollResources"> </cy:SiteLabel>
                    <asp:TextBox ID="txtQuestion" runat="server" CssClass="forminput verywidetextbox"  MaxLength="255"></asp:TextBox>
                </div>
                <div class="settingrow">
                    <cy:SiteLabel ID="lblAnonymousVoting" runat="server" ForControl="chkAnonymousVoting"
                        CssClass="settinglabel" ConfigKey="PollEditAnonymousVotingLabel" ResourceFile="PollResources">
                    </cy:SiteLabel>
                    <asp:CheckBox ID="chkAnonymousVoting" runat="server"></asp:CheckBox>
                </div>
                <div class="settingrow">
                    <cy:SiteLabel ID="lblAllowViewingResultsBeforeVoting" runat="server" ForControl="chkAllowViewingResultsBeforeVoting"
                        CssClass="settinglabel" ConfigKey="PollEditAllowViewingResultsBeforeVotingLabel"
                        ResourceFile="PollResources"> </cy:SiteLabel>
                    <asp:CheckBox ID="chkAllowViewingResultsBeforeVoting" runat="server"></asp:CheckBox>
                </div>
                <div class="settingrow">
                    <cy:SiteLabel ID="lblShowOrderNumbers" runat="server" ForControl="chkShowOrderNumbers"
                        CssClass="settinglabel" ConfigKey="PollEditShowOrderNumbersLabel" ResourceFile="PollResources">
                    </cy:SiteLabel>
                    <asp:CheckBox ID="chkShowOrderNumbers" runat="server"></asp:CheckBox>
                </div>
                <div class="settingrow">
                    <cy:SiteLabel ID="lblShowResultsWhenDeactivated" runat="server" ForControl="chkShowResultsWhenDeactivated"
                        CssClass="settinglabel" ConfigKey="PollEditShowResultsWhenDeactivatedLabel" ResourceFile="PollResources">
                    </cy:SiteLabel>
                    <asp:CheckBox ID="chkShowResultsWhenDeactivated" runat="server"></asp:CheckBox>
                </div>
                <div class="settingrow">
                    <cy:SiteLabel ID="lblPollAddOptions" runat="server" ForControl="tblOptions" CssClass="settinglabel"
                        ConfigKey="PollEditOptionsLabel" ResourceFile="PollResources"> </cy:SiteLabel>
                    <table id="tblOptions" cellpadding="0" cellspacing="0" border="0">
                        <tr valign="top">
                            <td>
                                <asp:ListBox ID="lbOptions" SkinID="PageTree" DataTextField="Answer" DataValueField="OptionGuid"
                                    Rows="10" runat="server" />
                            </td>
                            <td>
                                <asp:ImageButton ID="btnUp" CommandName="up" runat="server" CausesValidation="False" />
                                <br />
                                <asp:ImageButton ID="btnDown" CommandName="down" runat="server" CausesValidation="False" />
                                <br />
                                <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" />
                                <br />
                                <asp:ImageButton ID="btnDeleteOption" runat="server" CausesValidation="False" />
                                <br />
                                <br />
                                <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="addeditpolloptionshelp" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtNewOption" runat="server" Columns="39" MaxLength="100"></asp:TextBox>
                                <portal:CButton ID="btnAddOption" runat="server" CausesValidation="False" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="settingrow">
                    <cy:SiteLabel ID="lblActiveFromTo" runat="server" CssClass="settinglabel" ConfigKey="PollEditActiveFromToLabel"
                        ResourceFile="PollResources"> </cy:SiteLabel>
                    <cy:DatePickerControl ID="dpActiveFrom" runat="server" />
                    <cy:SiteLabel ID="lblTo" runat="server" ResourceFile="PollResources" ConfigKey="PollEditToLabel" />
                    <cy:DatePickerControl ID="dpActiveTo" runat="server" />
                </div>
                <div class="settingrow" id="divStartDeactivated" runat="server" visible="false">
                    <cy:SiteLabel ID="lblStartDeactivated" runat="server" ForControl="chkStartDeactivated"
                        CssClass="settinglabel" ConfigKey="PollEditStartDeactivatedLabel" ResourceFile="PollResources">
                    </cy:SiteLabel>
                    <asp:CheckBox ID="chkStartDeactivated" runat="server" />
                </div>
                <div class="settingrow">
                    <asp:RequiredFieldValidator ID="reqQuestion" runat="server" ControlToValidate="txtQuestion"
                        Display="None" CssClass="txterror"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvOptionsLessThanTwo" runat="server" Display="None" CssClass="txterror"></asp:CustomValidator>
                    <asp:ValidationSummary ID="vSummary" runat="server" CssClass="txterror"></asp:ValidationSummary>
                </div>
                <div class="settingrow">
                    <cy:SiteLabel ID="lblspacer" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                    <portal:CButton ID="btnSave" runat="server" />&nbsp;
                    <portal:CButton ID="btnDelete" runat="server" CausesValidation="false" />&nbsp;
                    <portal:CButton ID="btnActivateDeactivate" runat="server" CausesValidation="False" Visible="false" />
                </div>
            </div>
        </fieldset>
        </div>
    </asp:Panel>
    <cy:CornerRounderBottom id="cbottom1" runat="server" />
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
