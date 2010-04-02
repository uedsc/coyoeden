<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="FeedManager.aspx.cs" Inherits="Cynthia.Web.FeedUI.FeedManagerPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<div class="breadcrumbs">
<asp:HyperLink ID="lnkBackToPage" runat="server" />&nbsp;&nbsp;
<asp:HyperLink ID="lnkEditFeeds" runat="server" />
</div>
<cy:CornerRounderTop id="ctop1" runat="server"  />
<asp:Panel id="pnl1" runat="server" CssClass="art-Post-inner panelwrapper ">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
<asp:UpdatePanel ID="updPnlRSSA" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Panel ID="divFeedEntries" runat="server" CssClass="rsscenter-rightnav" SkinID="plain">
            <asp:Literal ID="lblFeedHeading" runat="server" Visible="false" />
            <asp:Repeater ID="rptEntries" runat="server" 
                onitemcommand="rptEntries_ItemCommand">
                
                <ItemTemplate>
                <asp:ImageButton CommandName="Confirm" CommandArgument=<%#DataBinder.Eval(Container, "DataItem.EntryHash") + "_" + Convert.ToString(DataBinder.Eval(Container, "DataItem.Confirmed")) %> 
                ID="ConfirmBtn" runat="server" ImageUrl='<%# ConfirmImage + DataBinder.Eval(Container, "DataItem.Confirmed") + ".png"%>'
                 AlternateText=<%# Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.Confirmed"))?Resources.FeedResources.EntryPublishTrueAlternateText:Resources.FeedResources.EntryPublishFalseAlternateText %> 
                />
                    <div class=<%# "rssfeedentry" + DataBinder.Eval(Container, "DataItem.Confirmed") %> id="divFeedEntry" runat="server" >
                    <div class="rsstitle">
                        <h3>
                            <asp:HyperLink ID="Hyperlink4" runat="server" SkinID="BlogTitle" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Link")%>'>
												<%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.Title").ToString())%>
                            </asp:HyperLink></h3>
                    </div>
                    <div class="rssdate">
                        <%# GetDateHeader((DateTime)DataBinder.Eval(Container, "DataItem.PubDate"))%>
                    </div>
                    <div class="rsstext" id="divFeedBody" runat="server" >
                        <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# allowedImageUrlRegexPattern %>'
                            ClientScriptUrl="~/ClientScript/NeatHtml.js">
                            <%# DataBinder.Eval(Container, "DataItem.Description").ToString()%>
                            
                        </NeatHtml:UntrustedContent>
                    </div>
                    <div class="rssauthor" id="divAuthor" runat="server" >
                        <asp:HyperLink ID="Hyperlink1" runat="server" SkinID="BlogTitle" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.BlogUrl")%>'>
												<%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.Author").ToString())%>
                        </asp:HyperLink>
                    </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>
        <portal:CCutePager ID="pgrRptEntries" runat="server" /> 
</ContentTemplate>
</asp:UpdatePanel>
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel> 
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
