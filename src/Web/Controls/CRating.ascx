<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CRating.ascx.cs"
    Inherits="Cynthia.Web.UI.CRating" %>
<div class="ratingcontainer">
    <asp:UpdatePanel ID="upRating" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <span id="spnPrompt" runat="server" class="ratingprompt">
                <asp:Literal ID="litPrompt" runat="server" />
            </span>
            <span class="rating" style="display: none;"><asp:Literal ID="litRating" runat="server" /></span>
                <portal:AjaxRating runat="server" ID="UserRating" EnableViewState="true" MaxRating="5"
                    CurrentRating="0" CssClass="ratingStar" StarCssClass="ratingItem" WaitingStarCssClass="Saved"
                    FilledStarCssClass="Filled" EmptyStarCssClass="Empty" />
                <asp:HiddenField ID="hdnContentGuid" runat="server" />
            <span id="spnTotal" runat="server" class="voteswrap">
                <asp:Label ID="lblRatingVotes" runat="server" CssClass="ratingvotes" />
                <portal:GreyBoxHyperlink ID="lnkRatingsReview" runat="server" Visible="false" ClientClick="return GB_showCenter(this.title, this.href, 600, 800)" />
            </span>
            <asp:Panel ID="pnlComments" runat="server" CssClass="ratingcomments">
                <asp:Label ID="lblComments" runat="server" AssociatedControlID="txtComments" CssClass="rcommentprompt" />
                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" CssClass="ratingcommentbox" />
                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="rcommentprompt rpemail" />
                <asp:TextBox ID="txtEmail" runat="server" CssClass="normaltextbox ratingemail" />
                <div class="ratingbuttons">
                    <asp:Button ID="btnComments" runat="server" CausesValidation="false" CommandArgument="Comment" />
                    <asp:Button ID="btnNoThanks" runat="server" CausesValidation="false" CommandArgument="NoThanks" />
                    <asp:HiddenField ID="hdnRating" runat="server" Value='' />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
