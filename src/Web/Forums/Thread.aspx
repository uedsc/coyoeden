<%@ Page Language="c#" CodeBehind="Thread.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="Cynthia.Web.GroupUI.GroupThreadView" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
    <asp:Panel ID="pnlThread" runat="server" CssClass="panelwrapper forumthreadview"
        EnableViewState="false">
        <div class="modulecontent">
            <div class="breadcrumbs">
                 <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink>
                &gt; <a href="" id="lnkGroup" runat="server"></a>&nbsp;&gt;
                <asp:Label ID="lblThreadDescription" runat="server"></asp:Label>
            </div>
            <h2 class="forumhead">
               <asp:Label ID="lblHeading" runat="server"></asp:Label>
            </h2>
            <div class="settingrow forumdesc">
                <asp:Literal ID="litGroupDescription" runat="server" />
            </div>
            <asp:Panel ID="pnlNotify" runat="server" Visible="false" CssClass="forumnotify">
                <asp:HyperLink ID="lnkNotify" runat="server"  />
                &nbsp;<asp:HyperLink ID="lnkNotify2" runat="server" />
            </asp:Panel>
            <div class="modulepager">
                <portal:CCutePager ID="pgrTop" runat="server" />
                <a href="" class="ModulePager" id="lnkNewThread" runat="server"></a>
                <asp:HyperLink ID="lnkLogin" runat="server" CssClass="ModulePager" />
            </div>
            <asp:Repeater ID="rptMessages" runat="server" EnableViewState="False">
                <ItemTemplate>
                    <div class="forumpostheader">
                        <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem),"PostDate", TimeOffset) %>
                    </div>
                    <div class="postwrapper">
                        <div class="postleft">
                            <div class="forumpostusername">
                                <asp:HyperLink Text="Edit" ID="Hyperlink2" ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/user_edit.png"  %>'
                                    NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx?userid=" + DataBinder.Eval(Container.DataItem,"UserID")   %>'
                                    Visible="<%# IsAdmin %>" runat="server" />
                                <%# SiteUtils.GetProfileLink(Page, DataBinder.Eval(Container.DataItem,"UserID"),DataBinder.Eval(Container.DataItem,"PostAuthor")) %>
                            </div>
                            <div class="forumpostuseravatar">
                                <%# GetAvatarUrl(DataBinder.Eval(Container.DataItem,"PostAuthorAvatar").ToString()) %>
                                <cy:Gravatar ID="grav1" runat="server" Email='<%# Eval("AuthorEmail") %>' MaxAllowedRating='<%# MaxAllowedGravatarRating %>'
                                    Visible='<%# allowGravatars %>' />
                            </div>
                            <div class="forumpostuserattribute">
                                <cy:SiteLabel ID="lblTotalPosts" runat="server" ConfigKey="ManageUsersTotalPostsLabel"
                                    UseLabelTag="false" />
                                <%# DataBinder.Eval(Container.DataItem,"PostAuthorTotalPosts") %>
                            </div>
                            <div class="forumpostuserattribute">
                                <portal:GroupUserThreadLink ID="lnkUserPosts" runat="server" UserId='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"UserID")) %>'
                                    TotalPosts='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"PostAuthorTotalPosts")) %>' />
                            </div>
                            <div id="divRevenue" runat="server" visible='<%# showUserRevenue %>' class="forumpostuserattribute">
                                <cy:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="UserSalesLabel" ResourceFile="GroupResources"
                                    UseLabelTag="false" />
                                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("UserRevenue"))) %>
                            </div>
                            <div class="forumpostuserattribute" id="divUntrustedSignature" runat="server" visible='<%# !Convert.ToBoolean(Eval("Trusted")) %>'>
                                <NeatHtml:UntrustedContent ID="UntrustedContent2" runat="server" TrustedImageUrlPattern='<%# allowedImageUrlRegexPattern %>'
                                    ClientScriptUrl="~/ClientScript/NeatHtml.js">
                                    <%# DataBinder.Eval(Container.DataItem, "PostAuthorSignature").ToString()%>
                                </NeatHtml:UntrustedContent>
                            </div>
                            <div class="forumpostuserattribute" id="divTrustedSignature" runat="server" visible='<%# Convert.ToBoolean(Eval("Trusted")) %>'>
                                <%# DataBinder.Eval(Container.DataItem, "PostAuthorSignature").ToString() %>
                            </div>
                            
                        </div>
                        <div class="postright">
                            <div class="posttopic">
                                <h3>
                                    <a id='post<%# DataBinder.Eval(Container.DataItem,"PostID") %>'></a>
                                    <asp:HyperLink Text="<%# Resources.GroupResources.GroupEditPostLink %>" ToolTip="<%# Resources.GroupResources.GroupEditPostLink %>"
                                        ID="editLink" ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
                                        NavigateUrl='<%# SiteRoot + "/Groups/EditPost.aspx?pageid=" + PageId.ToString() + "&amp;mid=" + moduleId.ToString() + "&amp;ItemID=" + ItemId.ToString() + "&amp;postid=" + DataBinder.Eval(Container.DataItem,"PostID")  + "&amp;thread=" + DataBinder.Eval(Container.DataItem,"TopicID") + "&amp;forumid=" + DataBinder.Eval(Container.DataItem,"GroupID") + "&amp;pagenumber=" + PageNumber.ToString()  %>'
                                        Visible='<%# GetPermission(DataBinder.Eval(Container.DataItem,"UserID"))%>' runat="server" />
                                    <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Subject").ToString())%></h3>
                            </div>
                            <div class="postbody">
                                <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# allowedImageUrlRegexPattern %>'
                                    ClientScriptUrl="~/ClientScript/NeatHtml.js">
                                    <%# DataBinder.Eval(Container.DataItem, "Post").ToString()%>
                                </NeatHtml:UntrustedContent>
                            </div>
                 
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div class="modulepager">
                <portal:CCutePager ID="pgrBottom" runat="server" />
                <a href="" class="ModulePager" id="lnkNewThreadBottom" runat="server"></a>
                <asp:HyperLink ID="lnkLoginBottom" runat="server" CssClass="ModulePager" />
            </div>
        </div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
