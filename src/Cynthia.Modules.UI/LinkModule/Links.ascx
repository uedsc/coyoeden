<%@ Control Language="c#" Inherits="Cynthia.Web.LinksUI.LinksModule" CodeBehind="Links.ascx.cs"
    AutoEventWireup="false" %>
    
<portal:ModulePanel ID="pnlContainer" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
    <asp:Panel ID="pnlWrapper" runat="server" CssClass="art-Post-inner panelwrapper linksmodule">
        <portal:ModuleTitleControl ID="Title1" runat="server" EnableViewState="false" />
        <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <asp:UpdatePanel ID="updPnl" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlLinks" runat="server" CssClass="modulecontent">
                    <asp:Repeater ID="rptLinks" runat="server">
                        <HeaderTemplate>
                            <ul class="linkitem">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li class="linkitem">
                                <%# CreateLink(DataBinder.Eval(Container.DataItem, "Title").ToString(), DataBinder.Eval(Container.DataItem, "Url").ToString(), DataBinder.Eval(Container.DataItem, "Description").ToString(), DataBinder.Eval(Container.DataItem, "Target").ToString())%>
                                <asp:HyperLink ID="editLink2" runat="server" EnableViewState="false" Text="<%# Resources.LinkResources.LinksEditLink %>"
                                    ToolTip="<%# Resources.LinkResources.LinksEditLink %>" NavigateUrl='<%# this.SiteRoot + "/LinkModule/EditLink.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&amp;mid=" + ModuleId + "&amp;pageid=" + PageId %>'
                                    Visible="<%# IsEditable%>" ImageUrl="<%# LinkImage %>"></asp:HyperLink>
                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="<%# DeleteLinkImage %>"
                                    Visible="<%# ShowDeleteIcon%>" CommandName="delete" ToolTip='<%# Resources.LinkResources.LinksDeleteLink %>'
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemId", "{0}") %>'>
                                </asp:ImageButton>
                                <asp:Literal ID="lblDescription2" runat="server" Visible="<%# UseDescription%>" Text='<%#  "&nbsp;" +DataBinder.Eval(Container.DataItem,"Description").ToString()%>'
                                    EnableViewState="false" /></li>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <li class="linkaltitem">
                                <%# CreateLink(DataBinder.Eval(Container.DataItem, "Title").ToString(), DataBinder.Eval(Container.DataItem, "Url").ToString(), DataBinder.Eval(Container.DataItem, "Description").ToString(), DataBinder.Eval(Container.DataItem, "Target").ToString())%>
                                <asp:HyperLink ID="editLink2" runat="server" EnableViewState="false" Text="<%# Resources.LinkResources.LinksEditLink %>"
                                    ToolTip="<%# Resources.LinkResources.LinksEditLink %>" NavigateUrl='<%# this.SiteRoot + "/LinkModule/EditLink.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&amp;mid=" + ModuleId + "&amp;pageid=" + PageId %>'
                                    Visible="<%# IsEditable%>" ImageUrl="<%# LinkImage %>"></asp:HyperLink>
                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="<%# DeleteLinkImage %>"
                                    Visible="<%# ShowDeleteIcon%>" CommandName="delete" ToolTip='<%# Resources.LinkResources.LinksDeleteLink %>'
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemId", "{0}") %>'>
                                </asp:ImageButton>
                                <asp:Literal ID="lblDescription2" runat="server" Visible="<%# UseDescription%>" Text='<%#  "&nbsp;" +DataBinder.Eval(Container.DataItem,"Description").ToString()%>'
                                    EnableViewState="false" /></li>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </ul></FooterTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="rptDescription" runat="server" Visible="false">
                        <ItemTemplate>
                            <div class="linkdesc">
                                <asp:HyperLink ID="editLink2" runat="server" EnableViewState="false" Text="<%# Resources.LinkResources.LinksEditLink %>"
                                    ToolTip="<%# Resources.LinkResources.LinksEditLink %>" NavigateUrl='<%# this.SiteRoot + "/LinkModule/EditLink.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&amp;mid=" + ModuleId + "&amp;pageid=" + PageId %>'
                                    Visible="<%# IsEditable%>" ImageUrl="<%# LinkImage %>"></asp:HyperLink>
                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="<%# DeleteLinkImage %>"
                                    Visible="<%# ShowDeleteIcon%>" CommandName="delete" ToolTip='<%# Resources.LinkResources.LinksDeleteLink %>'
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemId", "{0}") %>'>
                                </asp:ImageButton>
                                <asp:Literal ID="lblDescription2" runat="server" Visible="<%# UseDescription%>" Text='<%# DataBinder.Eval(Container.DataItem,"Description").ToString()%>'
                                    EnableViewState="false" />
                            </div>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <div class="linkdesc linkdescalt">
                                <asp:HyperLink ID="editLink2" runat="server" EnableViewState="false" Text="<%# Resources.LinkResources.LinksEditLink %>"
                                    ToolTip="<%# Resources.LinkResources.LinksEditLink %>" NavigateUrl='<%# this.SiteRoot + "/LinkModule/EditLink.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&amp;mid=" + ModuleId + "&amp;pageid=" + PageId %>'
                                    Visible="<%# IsEditable%>" ImageUrl="<%# LinkImage %>"></asp:HyperLink>
                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="<%# DeleteLinkImage %>"
                                    Visible="<%# ShowDeleteIcon%>" CommandName="delete" ToolTip='<%# Resources.LinkResources.LinksDeleteLink %>'
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemId", "{0}") %>'>
                                </asp:ImageButton>
                                <asp:Literal ID="lblDescription2" runat="server" Visible="<%# UseDescription%>" Text='<%# DataBinder.Eval(Container.DataItem,"Description").ToString()%>'
                                    EnableViewState="false" />
                            </div>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                    <portal:CCutePager ID="pgr" runat="server" Visible="false" />
                </asp:Panel>
                <div class="modulefooter">
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        </portal:CPanel>
        <div class="cleared"></div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
    </portal:CPanel>
</portal:ModulePanel>
