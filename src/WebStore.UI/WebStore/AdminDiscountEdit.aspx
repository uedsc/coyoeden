<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminDiscountEdit.aspx.cs" Inherits="WebStore.UI.AdminDiscountEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">
<div class="settingrow">
<cy:SiteLabel id="lblDiscountCode" runat="server" ForControl="txtDiscountCode" CssClass="settinglabel" ConfigKey="DiscountCodeLabel" ResourceFile="WebStoreResources" />
<asp:TextBox ID="txtDiscountCode" runat="server" CssClass="forminput widetextbox" MaxLength="255" />	
</div>
<div class="settingrow">
<cy:SiteLabel id="lblDescription" runat="server" ForControl="txtDescription" CssClass="settinglabel" ConfigKey="DiscountDescriptionLabel" ResourceFile="WebStoreResources" />
<asp:TextBox ID="txtDescription"  runat="server" CssClass="forminput widetextbox" MaxLength="255" />	
</div>
<div class="settingrow">
<cy:SiteLabel id="lblValidityStartDate" runat="server" ForControl="dpBeginDate" CssClass="settinglabel" ConfigKey="DiscountValidityStartDateLabel" ResourceFile="WebStoreResources" />
<cy:DatePickerControl id="dpBeginDate" runat="server" ShowTime="True" CssClass="forminput" > </cy:DatePickerControl>
</div>
<div class="settingrow">
<cy:SiteLabel id="lblValidityEndDate" runat="server" ForControl="dpEndDate" CssClass="settinglabel" ConfigKey="DiscountValidityEndDateLabel" ResourceFile="WebStoreResources" />
<cy:DatePickerControl id="dpEndDate" runat="server" ShowTime="True" CssClass="forminput" > </cy:DatePickerControl>
<asp:Label ID="lblLeaveBlankForNoEndDate" runat="server" />
</div>
<div class="settingrow">
<cy:SiteLabel id="lblUseCountLabel" runat="server"  CssClass="settinglabel" ConfigKey="DiscountUseCountLabel" ResourceFile="WebStoreResources" />
<asp:Label ID="lblCountOfUse" runat="server" />
</div>
<div class="settingrow">
<cy:SiteLabel id="SiteLabel2" runat="server" ForControl="ckAllowOtherDiscounts" CssClass="settinglabel" ConfigKey="AllowOtherDiscountsLabel" ResourceFile="WebStoreResources" />
<asp:CheckBox ID="ckAllowOtherDiscounts" runat="server" CssClass="forminput" />
</div>
<div class="settingrow">
<cy:SiteLabel id="lblMaxCount" runat="server" ForControl="txtMaxCount" CssClass="settinglabel" ConfigKey="DiscountMaxCountLabel" ResourceFile="WebStoreResources" />
<asp:TextBox ID="txtMaxCount" runat="server" CssClass="forminput normaltextbox" MaxLength="50" Text="0" />	
<asp:Label ID="lblZeroIsUnlimitedUse" runat="server" />
</div>
<div class="settingrow">
<cy:SiteLabel id="lblMinOrderAmount" runat="server" ForControl="txtMinOrderAmount" CssClass="settinglabel" ConfigKey="DiscountMinOrderAmountLabel" ResourceFile="WebStoreResources" />
<asp:TextBox ID="txtMinOrderAmount" runat="server" CssClass="forminput normaltextbox" MaxLength="50" Text="0" />	
<asp:Label ID="lblZeroMeansNoMinimum" runat="server" />
</div>
<div class="settingrow">
<cy:SiteLabel id="lblAbsoluteDiscount" runat="server" ForControl="txtAbsoluteDiscount" CssClass="settinglabel" ConfigKey="DiscountAbsoluteDiscountLabel" ResourceFile="WebStoreResources" />
<asp:TextBox ID="txtAbsoluteDiscount" runat="server" CssClass="forminput normaltextbox" MaxLength="50" Text="0" />	
</div>
<div class="settingrow">
<cy:SiteLabel id="lblPercentageDiscount" runat="server" ForControl="txtPercentageDiscount" CssClass="settinglabel" ConfigKey="DiscountPercentageDiscountLabel" ResourceFile="WebStoreResources" />
<asp:TextBox ID="txtPercentageDiscount" runat="server" CssClass="forminput normaltextbox" MaxLength="50" Text="0" />	
</div>
<div>
<asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="Discount" />
<asp:RequiredFieldValidator ID="reqDiscountCode" runat="server" ControlToValidate="txtDiscountCode"
    Display="None" ValidationGroup="Discount" />
<asp:RequiredFieldValidator ID="reqDescription" runat="server" ControlToValidate="txtDescription"
    Display="None" ValidationGroup="Discount" />
<asp:RequiredFieldValidator ID="reqBeginDate" runat="server" ControlToValidate="dpBeginDate"
    Display="None" ValidationGroup="Discount" />
<asp:RequiredFieldValidator ID="reqMaxUseCount" runat="server" ControlToValidate="txtMaxCount"
    Display="None" ValidationGroup="Discount" />
<asp:RequiredFieldValidator ID="reqMinOrderAmount" runat="server" ControlToValidate="txtMinOrderAmount"
    Display="None" ValidationGroup="Discount" />
<asp:RequiredFieldValidator ID="reqDiscountAmount" runat="server" ControlToValidate="txtAbsoluteDiscount"
    Display="None" ValidationGroup="Discount" />
<asp:RequiredFieldValidator ID="reqPercentDiscount" runat="server" ControlToValidate="txtPercentageDiscount"
    Display="None" ValidationGroup="Discount" />
<asp:Label ID="lblError" runat="server" CssClass="txterror" />

</div>
<div class="settingrow">
<cy:SiteLabel id="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer"  />
<portal:CButton ID="btnSave" runat="server" ValidationGroup="Discount" />
<portal:CButton ID="btnDelete" runat="server" CausesValidation="false" />
</div>
</div>
</asp:Panel> 
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
