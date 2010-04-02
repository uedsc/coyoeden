<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ProductListControl.ascx.cs" Inherits="WebStore.UI.ProductListControl" %>

<asp:Repeater ID="rptProducts" runat="server" >
        <ItemTemplate>
            <div class="hproduct hreview productcontainer">
                <h4><a class="fn url productlink" href='<%# SiteRoot + Eval("Url") %>'><%# Eval("Name") %></a></h4>
                <portal:CRating runat="server" ID="Rating" ContentGuid='<%# Eval("Guid") %>' Visible='<%# (EnableRatings && Convert.ToBoolean(Eval("EnableRating"))) %>' AllowFeedback='<%# EnableRatingComments %>' ShowPrompt="true" PromptText='<%# Resources.WebStoreResources.RatingPrompt %>'  />
                <div class="description"><%# Eval("Abstract") %></div>
                <a class="productdetaillink" href='<%# SiteRoot + Eval("Url") %>'><%# Resources.WebStoreResources.ProductDetailsLink%></a>
                <asp:HyperLink ID="lnkPreview" runat="server" Visible='<%# (Eval("TeaserFile").ToString().Length > 0) %>' Text='<%# Eval("TeaserFileLink") %>' NavigateUrl='<%# teaserFileBaseUrl + Eval("TeaserFile") %>' />
                <asp:Repeater id="rptOffers" runat="server">
                <ItemTemplate>
                <div class="offercontainer">
                <%# Eval("ProductListName") %>
                <asp:HyperLink ID="lnkOfferDetail" runat="server" Visible='<%# Convert.ToBoolean(Eval("ShowDetailLink")) %>' NavigateUrl='<%# SiteRoot + Eval("Url") %>' Text='<%# Resources.WebStoreResources.OfferDetailLink %>' EnableViewState="false" />
                <span class="price"><%# string.Format(CurrencyCulture, "{0:c}",Convert.ToDecimal(Eval("Price"))) %></span>
                <a href='<%# SiteRoot + "/WebStore/CartAdd.aspx?offer=" + Eval("Guid") + "&pageid=" + PageId.ToString() + "&mid=" + ModuleId.ToString() %>'>
                    <%# Resources.WebStoreResources.AddToCartLink%></a>
                </div>
                </ItemTemplate>
                </asp:Repeater>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="modulepager">
        <portal:CCutePager ID="pgr" runat="server" />
    </div>
    