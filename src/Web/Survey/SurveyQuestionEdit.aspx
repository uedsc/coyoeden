<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SurveyQuestionEdit.aspx.cs" Inherits="SurveyFeature.UI.QuestionEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkSurveys" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkPages" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkQuestions" CssClass="unselectedcrumb"></asp:HyperLink> 
   
</div>
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlQuestionEdit" runat="server" CssClass="panelwrapper survey">  
<div class="modulecontent">
 <fieldset class="htmledit">
    <legend>	
		<asp:Literal ID="litHeading" runat="server" />
	</legend>
    <div class="settingrow">
        <cye:EditorControl id="edMessage" runat="server"> </cye:EditorControl>
    </div>
    <div class="settingrow">
        <cy:SiteLabel ID="lblQuestionRequiredLabel" runat="server" ForControl="chkAnswerRequired" ConfigKey="QuestionRequiredLabel" ResourceFile="SurveyResources" CssClass="settinglabel" />
        <asp:CheckBox runat="server" ID="chkAnswerRequired" />
    </div>
    <div class="settingrow">
        <cy:SiteLabel ID="lblQuestionTypeLabel" runat="server" ConfigKey="QuestionTypeLabel" ResourceFile="SurveyResources" CssClass="settinglabel" />
        <asp:Label ID="lblQuestionType" runat="server" />
    </div>
    <div class="settingrow" runat="server">
        <cy:SiteLabel ID="lblValidationMessage" runat="server" ForControl="txtValidationMessage" ConfigKey="QuestionValidationMessageLabel" ResourceFile="SurveyResources" CssClass="settinglabel" />
        <asp:TextBox ID="txtValidationMessage" runat="server" Columns="39" MaxLength="100"></asp:TextBox>
    </div>
    <div class="settingrow" runat="server" id="itemsRow">
            <div id="questionItems" class="floatpanel">
                <asp:ListBox ID="lbOptions" SkinID="PageTree" DataTextField="Answer" DataValueField="OptionGuid" Rows="10"  runat="server" />
            </div>
            <div id="questionItemsMove" class="floatpanel">
            	    <asp:ImageButton ID="btnUp" CommandName="up"  runat="server" CausesValidation="False" />
		            <br />
		            <asp:ImageButton ID="btnDown" CommandName="down" runat="server" CausesValidation="False" />
	                <br />
			        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" />
		            <br />
			        <asp:ImageButton ID="btnDeleteOption" runat="server" CausesValidation="False" />	
		            <br /><br />
		            <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="addeditsurveypageshelp" />	
            </div>
    </div>
    <div class="settingrow" runat="server" id="addOptionRow">
        <asp:TextBox ID="txtNewOption" runat="server" Columns="39" MaxLength="100"></asp:TextBox>
        <portal:CButton ID="btnAddOption" runat="server" CausesValidation="false" /><br />
    </div>
    <div class="settingrow">
        <br />
        <portal:CButton ID="btnSave" runat="server" />
        <portal:CButton ID="btnCancel" runat="server" />
    </div>
    </fieldset>
    </div>
    </asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
<portal:SessionKeepAliveControl id="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
