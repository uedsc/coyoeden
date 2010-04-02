<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="PaymentAcceptanceMark.ascx.cs"
    Inherits="Cynthia.Web.UI.PaymentAcceptanceMark" %>
    <div class="floatpanel">
   <div class="floatpanel" style="padding-top:15px;">
    <asp:Image ID="imgMasterCard" runat="server" ImageUrl="~/Data/SiteImages/mc.gif"
    AlternateText="MasterCard" Visible="false" />
<asp:Image ID="imgVisaCard" runat="server" ImageUrl="~/Data/SiteImages/visa.gif"
    AlternateText="VISA" Visible="false" />
<asp:Image ID="imgAmexCard" runat="server" ImageUrl="~/Data/SiteImages/amex.gif"
    AlternateText="American Express" Visible="false" />
    <asp:Image ID="imgDiscover" runat="server" ImageUrl="~/Data/SiteImages/discover.gif"
    AlternateText="Discover Card" Visible="false" />
<asp:Image ID="imgPayPal" runat="server" ImageUrl="~/Data/SiteImages/paypal.gif"
    AlternateText="PayPal" Visible="false" />
    </div>
    <div class="floatpanel" id="divGoogle" runat="server" visible="false">
 <asp:Literal ID="litGCheckoutScript" runat="server"><script type="text/javascript" src="https://checkout.google.com/seller/accept/j.js"></script></asp:Literal>
 <asp:Literal ID="litGCheckoutSetup" runat="server" />
<noscript style="display:inline;"><asp:Image ID="imgGCheckout" runat="server" CssClass="floatPanel" ImageUrl="https://checkout.google.com/seller/accept/images/sc.gif"
    AlternateText="Google Checkout Acceptance Mark" /></noscript>
    </div>
</div>