<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="FeedEdit.aspx.cs" Inherits="Cynthia.Web.FeedUI.FeedEditPage" %>

<%@ Register TagPrefix="mpf" TagName="FeedTypeSetting" Src="~/FeedManager/Controls/FeedTypeSetting.ascx" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="panelwrapper rssmodule">

<asp:Panel ID="pnlEdit" runat="server" CssClass="modulecontent" DefaultButton="btnUpdate">
<fieldset class="rssfeededit">
    <legend>	
		<cy:SiteLabel id="lblHeadingLabel" runat="server" ConfigKey="EditFeedLabel" ResourceFile="FeedResources" UseLabelTag="false"> </cy:SiteLabel>
	</legend>
	    
	        <div class="settingrow">
	            <cy:SiteLabel id="lblTitleLabel" runat="server" ForControl="txtAuthor" CssClass="settinglabel" ConfigKey="AuthorLabel" ResourceFile="FeedResources" > </cy:SiteLabel>
	            <asp:TextBox id="txtAuthor" runat="server" maxlength="100"  CssClass="forminput widetextbox"></asp:TextBox>
	        </div>
	        <div class="settingrow">
	            <cy:SiteLabel id="Sitelabel1" runat="server" ForControl="txtWebSite" CssClass="settinglabel" ConfigKey="WebSiteLabel" ResourceFile="FeedResources" > </cy:SiteLabel>
	            <asp:TextBox id="txtWebSite" runat="server" maxlength="255"  CssClass="forminput widetextbox"></asp:TextBox>
	        </div>
	        <div class="settingrow">
	            <cy:SiteLabel id="Sitelabel2" runat="server" ForControl="txtRssUrl" CssClass="settinglabel" ConfigKey="FeedUrlLabel" ResourceFile="FeedResources" > </cy:SiteLabel>
	            <asp:TextBox id="txtRssUrl" runat="server" maxlength="255"  CssClass="forminput widetextbox"></asp:TextBox>
	        </div>
	        <div class="settingrow">
	            <cy:SiteLabel id="Sitelabel4" runat="server" ForControl="txtSortRank" CssClass="settinglabel" ConfigKey="SortRankLabel" ResourceFile="FeedResources" > </cy:SiteLabel>
	            <asp:TextBox id="txtSortRank" runat="server" maxlength="10" Text="500"  CssClass="forminput smalltextbox"></asp:TextBox>
	        </div>
	        <div id="divImage" runat="server" visible="false" class="settingrow">
	            <cy:SiteLabel id="Sitelabel3" runat="server" ForControl="txtImageUrl" CssClass="settinglabel" ConfigKey="ImageUrlLabel" ResourceFile="FeedResources" > </cy:SiteLabel>
	            <asp:TextBox id="txtImageUrl" runat="server" maxlength="255"  CssClass="forminput widetextbox"></asp:TextBox>
	        </div>
	        <div id="divPublish" runat="server" class="settingrow">
	            <cy:SiteLabel id="Sitelabel5" runat="server" ForControl="txtRssUrl" CssClass="settinglabel" ConfigKey="PublishByDefaultLabel" ResourceFile="FeedResources" > </cy:SiteLabel>
	            <asp:CheckBox id="chkPublishByDefault" runat="server" CssClass="forminput" />
	        </div>
	        <div class="settingrow">
	            <asp:Label id="lblError" runat="server" ></asp:Label>
	        </div>
	        <div class="settingrow">
        <cy:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
            <div class="forminput">
	            <portal:CButton  id="btnUpdate" runat="server" Text="Update" ValidationGroup="feeds" />&nbsp;
			    <portal:CButton  id="btnDelete" runat="server" Text="" CausesValidation="false" />&nbsp;
			    <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;	
			    <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="rssfeededithelp" />
			    </div>
			    <asp:ValidationSummary id="vSummary" runat="server" ValidationGroup="feeds" />
			    <asp:RequiredFieldValidator id="reqTitle" runat="server" ControlToValidate="txtAuthor" Display="None" ValidationGroup="feeds" />
			    
			    <asp:RequiredFieldValidator id="reqFeedUrl" runat="server" ControlToValidate="txtRssUrl" Display="None" ValidationGroup="feeds" />
			    <asp:RegularExpressionValidator id="regexWebSiteUrl" runat="server" ControlToValidate="txtWebSite" Display="None" ValidationGroup="feeds" />
			    <asp:RegularExpressionValidator id="regexFeedUrl" runat="server" ControlToValidate="txtRssUrl" Display="None" ValidationGroup="feeds" />
			    
	        </div>
	    
</fieldset>
<asp:HiddenField ID="hdnReturnUrl" runat="server" />
</asp:Panel>
<asp:Panel ID="divNav" runat="server" CssClass="rssnavright" SkinID="plain">
            <asp:Label ID="lblFeedListName" Font-Bold="True" runat="server"></asp:Label>
            <br />
            <asp:Hyperlink id="lnkNewFeed" runat="server" />
            <portal:CDataList ID="dlstFeedList" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <asp:HyperLink ID="editLink" runat="server" Text="<%# Resources.FeedResources.EditImageAltText%>"
                        ToolTip="<%# Resources.FeedResources.EditImageAltText%>" ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
                        NavigateUrl='<%# this.SiteRoot + "/FeedManager/FeedEdit.aspx?pageid=" + PageId.ToString() + "&amp;ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&amp;mid=" + ModuleId.ToString()  %>'
                         />
                    <asp:HyperLink ID="Hyperlink2" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Url")%>'>
						<%# DataBinder.Eval(Container, "DataItem.Author")%>
                    </asp:HyperLink>
                    
                    <asp:HyperLink ID="Hyperlink3" runat="server" 
                        ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + RssImageFile %>' 
                        NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.RssUrl")%>'>
                    </asp:HyperLink>&nbsp;&nbsp;
                </ItemTemplate>
            </portal:CDataList>
        </asp:Panel>
        <portal:CButton  id="btnClearCache" runat="server"  CausesValidation="False" />
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

