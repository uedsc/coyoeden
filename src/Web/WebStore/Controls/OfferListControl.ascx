<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfferListControl.ascx.cs" Inherits="WebStore.UI.OfferListControl" %>
<asp:Repeater ID="rptOffers" runat="server" >
        <ItemTemplate>
            <div class="hproduct productcontainer">
                <h4><a class="fn url productlink" href='<%# SiteRoot + Eval("Url") %>'><%# Eval("Name") %></a></h4>
                <span class="price"><%# string.Format(CurrencyCulture, "{0:c}",Convert.ToDecimal(Eval("Price"))) %></span>
                <a href='<%# SiteRoot + "/WebStore/CartAdd.aspx?offer=" + Eval("Guid") + "&pageid=" + PageId.ToString() + "&mid=" + ModuleId.ToString() %>'>
                    <%# Resources.WebStoreResources.AddToCartLink%></a>
                <div class="description"><%# Eval("Abstract") %></div>
                <asp:Repeater id="rptProducts" runat="server">
                <ItemTemplate>
                <div class="offercontainer">
                <asp:Literal ID="litName" runat="server" Text='<%# Eval("Name") %>' Visible='<%# (Eval("TeaserFile").ToString().Length == 0) %>' />
                <asp:HyperLink ID="lnkPreview" runat="server" Visible='<%# (Eval("TeaserFile").ToString().Length > 0) %>' Text='<%# Eval("Name") %>' NavigateUrl='<%# teaserFileBaseUrl + Eval("TeaserFile") %>' />
                <portal:CRating runat="server" ID="Rating" ContentGuid='<%# Eval("Guid") %>' Visible='<%# (EnableRatings && Convert.ToBoolean(Eval("EnableRating"))) %>' AllowFeedback='<%# EnableRatingComments %>' ShowPrompt="true" PromptText='<%# Resources.WebStoreResources.RatingPrompt %>'  />
                <a class="productdetaillink" href='<%# SiteRoot + Eval("Url") %>'><%# Resources.WebStoreResources.ProductDetailsLink%></a>
                </div>
                </ItemTemplate>
                </asp:Repeater>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="modulepager">
        <portal:CCutePager ID="pgr" runat="server" />
    </div>
    
    