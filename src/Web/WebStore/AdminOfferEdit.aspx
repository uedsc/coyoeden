<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminOfferEdit.aspx.cs" Inherits="WebStore.UI.AdminOfferEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" />

    <cy:YUIPanel ID="pnlProduct" runat="server" CssClass="art-Post-inner panelwrapper webstore webstoreadminoffer tundra"
        DefaultButton="btnSave">
        <h2 class="moduletitle heading">
            <asp:Literal ID="litHeading" runat="server" /></h2>
            <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent">
            <div id="divtabs" class="yui-navset">
                <ul class="yui-nav">
                    <li class="selected"><a href="#tabSettings"><em><asp:Literal ID="litSettingsTab" runat="server" /></em></a></li>
                    <li><a href="#tabDescription" ><em><asp:Literal ID="litDescriptionTab" runat="server" /></em></a></li>
                    <li id="liProducts" runat="server"><a href="#tabProducts" id="lnkProducts" runat="server">
                        <em><asp:Literal ID="litProductsTab" runat="server" /></em></a></li>
                   <li><a href="#tabMeta"><em><asp:Literal ID="litMetaTab" runat="server" /></em></a></li>
                    
                </ul>
                <div class="yui-content">
                    <div id="tabSettings">
                        <div class="settingrow">
                            <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="OfferNameLabel"
                                ResourceFile="WebStoreResources" ForControl="txtName" />
                            <asp:TextBox ID="txtName"  runat="server" MaxLength="255" CssClass="verywidetextbox forminput" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="SiteLabel6" runat="server" CssClass="settinglabel" ConfigKey="OfferNameOnProductListLabel"
                                ResourceFile="WebStoreResources" ForControl="txtProductList" />
                            <asp:TextBox ID="txtProductListName" runat="server" MaxLength="255" CssClass="verywidetextbox forminput" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="PrimarySortRankLabel"
                                ResourceFile="WebStoreResources" ForControl="txtSortRank1" />
                            <asp:TextBox ID="txtSortRank1" runat="server" Text="5000" MaxLength="20" CssClass="forminput smalltextbox" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="SiteLabel9" runat="server" CssClass="settinglabel" ConfigKey="SecondarySortRankLabel"
                                ResourceFile="WebStoreResources" ForControl="txtSortRank2" />
                            <asp:TextBox ID="txtSortRank2" runat="server" Text="5000" MaxLength="20" CssClass="forminput smalltextbox" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="SiteLabel4" runat="server" CssClass="settinglabel" ConfigKey="OfferPriceLabel"
                                ResourceFile="WebStoreResources" ForControl="txtPrice" />
                            <asp:TextBox ID="txtPrice" runat="server" MaxLength="255" CssClass="forminput mediumtextbox" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="lblTaxClassGuid" runat="server" CssClass="settinglabel" ConfigKey="OfferTaxClassLabel"
                                ResourceFile="WebStoreResources" ForControl="ddTaxClassGuid" />
                            <asp:DropDownList ID="ddTaxClassGuid" runat="server" EnableTheming="false" DataValueField="Guid" DataTextField="Title" CssClass="forminput" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="lblIsVisible" runat="server" CssClass="settinglabel" ConfigKey="OfferIsVisibleLabel"
                                ResourceFile="WebStoreResources" ForControl="chkIsVisible"  />
                            <asp:CheckBox ID="chkIsVisible" runat="server" CssClass="forminput" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="lblIsSpecial" runat="server" CssClass="settinglabel" ConfigKey="OfferIsSpecialLabel"
                                ResourceFile="WebStoreResources" ForControl="chkIsSpecial"  />
                            <asp:CheckBox ID="chkIsSpecial" runat="server" CssClass="forminput" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="OfferIsDonationLabel"
                                ResourceFile="WebStoreResources" ForControl="chkIsDonation"  />
                            <asp:CheckBox ID="chkIsDonation" runat="server" CssClass="forminput" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="SiteLabel7" runat="server" CssClass="settinglabel" ConfigKey="OfferShowDetailLink"
                                ResourceFile="WebStoreResources" ForControl="chkShowDetailLink" />
                            <asp:CheckBox ID="chkShowDetailLink" runat="server" CssClass="forminput" />
                        </div>
                        <div class="settingrow">&nbsp;</div>
                    </div>
                    <div id="tabDescription">
                    <div class="settingrow">
                            <cye:EditorControl ID="edDescription" runat="server">
                            </cye:EditorControl>
                        </div>
                    </div>
                    <div id="tabProducts" runat="server">
                        <div class="minheightpanel">
                            <asp:UpdatePanel ID="upProducts" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <cy:CGridView ID="grdOfferProduct" runat="server" AllowPaging="false" AllowSorting="false"
                                        AutoGenerateColumns="false" CssClass="editgrid" DataKeyNames="Guid" SkinID="plain">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="editgridcell" HeaderStyle-CssClass="editgridheader">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.WebStoreResources.OfferProductGridEditButton %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Name" ItemStyle-CssClass="editgridcell" HeaderStyle-CssClass="editgridheader">
                                                <ItemTemplate>
                                                    <%# Eval("Name") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div class="settingrow">
                                                        <cy:SiteLabel ID="lblProductGuid" runat="server" CssClass="settinglabel" ConfigKey="OfferProductGridNameHeader"
                                                            ResourceFile="WebStoreResources" />
                                                        <asp:Label ID="lblProduct" runat="server" Text='<%# Eval("Name") %>' CssClass="productname" />
                                                        <asp:HiddenField ID="hdnFType" runat="server" Value='<%# Eval("FullfillType") %>' />
                                                        <asp:HiddenField ID="hdnFTerms" runat="server" Value='<%# Eval("FullFillTermsGuid") %>' />
                                                    </div>
                                                    <asp:Panel id="divFulfillment" runat="server" cssclass="settingrow">
                                                        <cy:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="OfferProductGridFullfillmentTermsHeader"
                                                            ResourceFile="WebStoreResources" />
                                                        <asp:DropDownList ID="ddFullFillTerms" runat="server" DataValueField="Guid" DataTextField="Name" CssClass="forminput" />
                                                    </asp:Panel>
                                                    <div class="settingrow">
                                                        <cy:SiteLabel ID="lblQuantity" runat="server" CssClass="settinglabel" ConfigKey="OfferProductGridQuantityHeader"
                                                            ResourceFile="WebStoreResources" />
                                                        <asp:TextBox ID="txtQuantity"  Text='<%# Eval("Quantity") %>' runat="server"
                                                            MaxLength="8" CssClass="forminput" />
                                                    </div>
                                                    <div class="settingrow">
                                                        <cy:SiteLabel ID="lblSortOrder" runat="server" CssClass="settinglabel" ConfigKey="OfferProductGridSortHeader"
                                                            ResourceFile="WebStoreResources" />
                                                        <asp:TextBox ID="txtSortOrder"  Text='<%# Eval("SortOrder") %>' runat="server"
                                                            MaxLength="8" CssClass="forminput" />
                                                    </div>
                                                    <asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.WebStoreResources.OfferProductGridUpdateButton %>'
                                                        CommandName="Update" />
                                                    <asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.WebStoreResources.OfferProductGridDeleteButton %>'
                                                        CommandName="Delete" />
                                                    <asp:Button ID="btnGridCancel" runat="server" Text='<%# Resources.WebStoreResources.OfferProductGridCancelButton %>'
                                                        CommandName="Cancel" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="FullfillType" ItemStyle-CssClass="editgridcell"
                                                HeaderStyle-CssClass="editgridheader">
                                                <ItemTemplate>
                                                    <%# WebStore.Helpers.StoreHelper.GetFulfillmentTypeLabel(Eval("FullfillType").ToString())%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Quantity" ItemStyle-CssClass="editgridcell" HeaderStyle-CssClass="editgridheader">
                                                <ItemTemplate>
                                                    <%# Eval("Quantity") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </cy:CGridView>
                                    <div class="settingrow">
                                        <portal:GreyBoxHyperlink ID="lnkAddProducts" runat="server" ClientClick="return GB_showCenter(this.title, this.href)" />
                                        <asp:HiddenField ID="hdnProductGuid" runat="server" />
                                        <asp:HiddenField ID="hdnFulfillmentType" runat="server" />
                                        <asp:HiddenField ID="hdnFulfillmentTermsGuid" runat="server" />
                                        <asp:ImageButton ID="btnAddFromGreyBox" runat="server" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div id="tabAvailability" runat="server" visible="false">
                        <div class="minheightpanel">
                            <asp:UpdatePanel ID="upAvailability" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <cy:CGridView ID="grdOfferAvailability" runat="server" AllowPaging="false" AllowSorting="false"
                                        AutoGenerateColumns="false" CssClass="editgrid" DataKeyNames="Guid" SkinID="plain">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="editgridcell" HeaderStyle-CssClass="editgridheader">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit" ToolTip='<%# Resources.WebStoreResources.OfferAvailabilityGridEditButton %>'
                                                        AlternateText='<%# Resources.WebStoreResources.OfferAvailabilityGridEditButton %>'
                                                        ImageUrl="~/Data/SiteImages/edit.gif" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.WebStoreResources.OfferAvailabilityGridUpdateButton %>'
                                                        CommandName="Update" />
                                                    <asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.WebStoreResources.OfferAvailabilityGridDeleteButton %>'
                                                        CommandName="Delete" />
                                                    <asp:Button ID="btnGridCancel" runat="server" Text='<%# Resources.WebStoreResources.OfferAvailabilityGridCancelButton %>'
                                                        CommandName="Cancel" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="BeginUTC" ItemStyle-CssClass="editgridcell" HeaderStyle-CssClass="editgridheader">
                                                <ItemTemplate>
                                                    <%# Eval("BeginUTC") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <cy:DatePickerControl ID="dpBeginDate" runat="server" Text='<%# GetBeginDate(Eval("BeginUTC")) %>'
                                                        Columns="15" Required="True" MaxLength="50" ShowTime="True"> </cy:DatePickerControl>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="EndUTC" ItemStyle-CssClass="editgridcell" HeaderStyle-CssClass="editgridheader">
                                                <ItemTemplate>
                                                    <%# Eval("EndUTC") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <cy:DatePickerControl ID="dpEndDate" runat="server" Text='<%# Bind("EndUTC") %>'
                                                        Columns="15" Required="False" MaxLength="50" ShowTime="True"> </cy:DatePickerControl>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="RequiresOfferCode" ItemStyle-CssClass="editgridcell"
                                                HeaderStyle-CssClass="editgridheader">
                                                <ItemTemplate>
                                                    <%# Eval("RequiresOfferCode") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkRequiresOfferCode" Checked='<%# Eval("RequiresOfferCode") %>'
                                                        runat="server" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="OfferCode" ItemStyle-CssClass="editgridcell" HeaderStyle-CssClass="editgridheader">
                                                <ItemTemplate>
                                                    <%# Eval("OfferCode") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtOfferCode" Text='<%# Eval("OfferCode") %>' runat="server"
                                                        MaxLength="50" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="MaxAllowedPerCustomer" ItemStyle-CssClass="editgridcell"
                                                HeaderStyle-CssClass="editgridheader">
                                                <ItemTemplate>
                                                    <%# Eval("MaxAllowedPerCustomer") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtMaxAllowedPerCustomer" Columns="20" Text='<%# Eval("MaxAllowedPerCustomer") %>'
                                                        runat="server" MaxLength="4" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </cy:CGridView>
                                    <div class="settingrow">
                                        <asp:Button ID="btnAddNewAvailability" runat="server" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div id="tabMeta">
		                <div class="settingrow">
		                    <cy:SiteLabel id="SiteLabel2" runat="server" ForControl="txtMetaDescription" CssClass="settinglabel" ConfigKey="MetaDescriptionLabel" ResourceFile="WebStoreResources" > </cy:SiteLabel>
		                    <asp:TextBox id="txtMetaDescription" runat="server"  Columns="50" maxlength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
		                </div>
		                <div class="settingrow">
		                    <cy:SiteLabel id="SiteLabel10" runat="server" ForControl="txtMetaKeywords" CssClass="settinglabel" ConfigKey="MetaKeywordsLabel" ResourceFile="WebStoreResources" > </cy:SiteLabel>
		                    <asp:TextBox id="txtMetaKeywords" runat="server"  Columns="50" maxlength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
		                </div>
		                <div class="settingrow">
                                            <cy:SiteLabel ID="lblAdditionalMetaTags" runat="server" CssClass="settinglabel" ConfigKey="MetaAdditionalLabel" ResourceFile="WebStoreResources"> </cy:SiteLabel>
                                            <portal:CHelpLink ID="CynHelpLink25" runat="server" HelpKey="pagesettingsadditionalmetahelp" />
                                        </div>
                                        <div class="settingrow">
                                            <asp:UpdatePanel ID="updMetaLinks" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <cy:CGridView ID="grdMetaLinks" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
                                                        DataKeyNames="Guid" EnableTheming="false">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEditMetaLink" runat="server" CommandName="Edit" Text='<%# Resources.WebStoreResources.ContentMetaGridEditButton %>' />
                                                                    <asp:ImageButton ID="btnMoveUpMetaLink" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                        CommandName="MoveUp" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.WebStoreResources.ContentMetaGridMoveUpButton %>'
                                                                        Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
                                                                    <asp:ImageButton ID="btnMoveDownMetaLink" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                        CommandName="MoveDown" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.WebStoreResources.ContentMetaGridMoveDownButton %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%# Eval("Rel") %>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblNameMetaRel" runat="server" ForControl="txtRel" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaRelLabel" ResourceFile="WebStoreResources" />
                                                                        <asp:TextBox ID="txtRel" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Rel") %>' />
                                                                        <asp:RequiredFieldValidator ID="reqMetaName" runat="server" ControlToValidate="txtRel"
                                                                            ErrorMessage='<%# Resources.WebStoreResources.ContentMetaLinkRelRequired %>' ValidationGroup="metalink" />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblMetaHref" runat="server" ForControl="txtHref" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaMetaHrefLabel" ResourceFile="WebStoreResources" />
                                                                        <asp:TextBox ID="txtHref" CssClass="verywidetextbox forminput" runat="server" Text='<%# Eval("Href") %>' />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtHref"
                                                                            ErrorMessage='<%# Resources.WebStoreResources.ContentMetaLinkHrefRequired %>' ValidationGroup="metalink" />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblScheme" runat="server" ForControl="txtScheme" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetHrefLangLabel" ResourceFile="Resource" />
                                                                        <asp:TextBox ID="txtHrefLang" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("HrefLang") %>' />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <asp:Button ID="btnUpdateMetaLink" runat="server" Text='<%# Resources.WebStoreResources.ContentMetaGridUpdateButton %>'
                                                                            CommandName="Update" ValidationGroup="metalink" CausesValidation="true" />
                                                                        <asp:Button ID="btnDeleteMetaLink" runat="server" Text='<%# Resources.WebStoreResources.ContentMetaGridDeleteButton %>'
                                                                            CommandName="Delete" CausesValidation="false" />
                                                                        <asp:Button ID="btnCancelMetaLink" runat="server" Text='<%# Resources.WebStoreResources.ContentMetaGridCancelButton %>'
                                                                            CommandName="Cancel" CausesValidation="false" />
                                                                    </div>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%# Eval("Href") %>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </cy:CGridView>
                                                    <div class="settingrow">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddMetaLink" runat="server" />&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:UpdateProgress ID="prgMetaLinks" runat="server" AssociatedUpdatePanelID="updMetaLinks">
                                                                        <ProgressTemplate>
                                                                            <img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicators/indicator1.gif") %>' alt=' ' />
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="settingrow">
                                            <asp:UpdatePanel ID="upMeta" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <cy:CGridView ID="grdContentMeta" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
                                                        DataKeyNames="Guid" EnableTheming="false">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEditMeta" runat="server" CommandName="Edit" Text='<%# Resources.WebStoreResources.ContentMetaGridEditButton %>' />
                                                                    <asp:ImageButton ID="btnMoveUpMeta" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                        CommandName="MoveUp" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.WebStoreResources.ContentMetaGridMoveUpButton %>'
                                                                        Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
                                                                    <asp:ImageButton ID="btnMoveDownMeta" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                        CommandName="MoveDown" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.WebStoreResources.ContentMetaGridMoveDownButton %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%# Eval("Name") %>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblName" runat="server" ForControl="txtName" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaNameLabel" ResourceFile="WebStoreResources" />
                                                                        <asp:TextBox ID="txtName" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Name") %>' />
                                                                        <asp:RequiredFieldValidator ID="reqMetaName" runat="server" ControlToValidate="txtName"
                                                                            ErrorMessage='<%# Resources.WebStoreResources.ContentMetaNameRequired %>' ValidationGroup="meta" />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblMetaContent" runat="server" ForControl="txtMetaContent" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaMetaContentLabel" ResourceFile="WebStoreResources" />
                                                                        <asp:TextBox ID="txtMetaContent" CssClass="verywidetextbox forminput" runat="server"
                                                                            Text='<%# Eval("MetaContent") %>' />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                                                            ErrorMessage='<%# Resources.WebStoreResources.ContentMetaContentRequired %>' ValidationGroup="meta" />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblScheme" runat="server" ForControl="txtScheme" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaSchemeLabel" ResourceFile="WebStoreResources" />
                                                                        <asp:TextBox ID="txtScheme" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Scheme") %>' />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblLangCode" runat="server" ForControl="txtLangCode" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaLangCodeLabel" ResourceFile="WebStoreResources" />
                                                                        <asp:TextBox ID="txtLangCode" CssClass="smalltextbox forminput" runat="server" Text='<%# Eval("LangCode") %>' />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblDir" runat="server" ForControl="ddDirection" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaDirLabel" ResourceFile="WebStoreResources" />
                                                                        <asp:DropDownList ID="ddDirection" runat="server" CssClass="forminput">
                                                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                                                            <asp:ListItem Text="ltr" Value="ltr"></asp:ListItem>
                                                                            <asp:ListItem Text="rtl" Value="rtl"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <asp:Button ID="btnUpdateMeta" runat="server" Text='<%# Resources.WebStoreResources.ContentMetaGridUpdateButton %>'
                                                                            CommandName="Update" ValidationGroup="meta" CausesValidation="true" />
                                                                        <asp:Button ID="btnDeleteMeta" runat="server" Text='<%# Resources.WebStoreResources.ContentMetaGridDeleteButton %>'
                                                                            CommandName="Delete" CausesValidation="false" />
                                                                        <asp:Button ID="btnCancelMeta" runat="server" Text='<%# Resources.WebStoreResources.ContentMetaGridCancelButton %>'
                                                                            CommandName="Cancel" CausesValidation="false" />
                                                                    </div>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%# Eval("MetaContent") %>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </cy:CGridView>
                                                    <div class="settingrow">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddMeta" runat="server" />&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:UpdateProgress ID="prgMeta" runat="server" AssociatedUpdatePanelID="upMeta">
                                                                        <ProgressTemplate>
                                                                            <img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicators/indicator1.gif") %>' alt=' ' />
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
		                <div class="settingrow">&nbsp;</div>
		            </div>
                </div>
                <div>&nbsp;</div>
                 <div>
                    
                   
                    <portal:CButton ID="btnSave" runat="server" ValidationGroup="Product" />
                    <portal:CButton ID="btnDelete" runat="server" CausesValidation="false" />
                </div>
                <div>
                    <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName" Display="None" ValidationGroup="Product" />
                    <asp:RequiredFieldValidator ID="reqPrice" runat="server" ControlToValidate="txtPrice" Display="None" ValidationGroup="Product" />
                    <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="Product" />
                    <portal:CLabel ID="lblError" runat="server" CssClass="txterror" />
                </div>
            </div>
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </cy:YUIPanel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
