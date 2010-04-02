<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminProductEdit.aspx.cs" Inherits="WebStore.UI.AdminProductEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    
    <cy:YUIPanel ID="pnlProduct" runat="server" CssClass="art-Post-inner panelwrapper webstore webstoreproductedit"
        DefaultButton="btnSave">
        <h2 class="moduletitle heading">
            <asp:Literal ID="litHeading" runat="server" /></h2>
            <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent">
            <div id="divtabs" class="yui-navset">
                <ul class="yui-nav">
                    <li class="selected"><a href="#tabSettings"><em>
                        <asp:Literal ID="litSettingsTab" runat="server" /></em></a></li>
                    <li><a href="#tabAbstract"><em><asp:Literal ID="litAbstactTab" runat="server" /></em></a></li>
                    <li><a href="#tabDescription"><em><asp:Literal ID="litDescriptionTab" runat="server" /></em></a></li>
                    <li id="liFullfillment" runat="server"><a href="#tabFullfillment" id="lnkFullfillment"
                        runat="server"><em><asp:Literal ID="litFullfillmentTab" runat="server" /></em></a></li>
                    <li><a href="#tabMeta"><em><asp:Literal ID="litMetaTab" runat="server" /></em></a></li>
                </ul>
                <div class="yui-content">
                    <div id="tabSettings">
                        <div class="settingrow">
                            <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="ProductNameLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtName" Columns="70" runat="server" MaxLength="255" CssClass="forminput" />
                        </div>
                       
                        <div class="settingrow" id="divTaxClass" runat="server" visible="false">
                            <cy:SiteLabel ID="lblTaxClassGuid" runat="server" CssClass="settinglabel" ConfigKey="ProductTaxClassLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddTaxClassGuid" runat="server" DataValueField="Guid" DataTextField="Title" CssClass="forminput" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="lblModelNumber" runat="server" CssClass="settinglabel" ConfigKey="ProductModelNumberLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtModelNumber" Columns="20" runat="server" MaxLength="255" CssClass="forminput" />
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
                            <cy:SiteLabel ID="lblStatus" runat="server" CssClass="settinglabel" ConfigKey="ProductStatusLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddStatus" runat="server" EnableTheming="false" CssClass="forminput" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="lblWeight" runat="server" CssClass="settinglabel" ConfigKey="ProductWeightLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtWeight" Columns="10" runat="server" MaxLength="20" Text="0" CssClass="forminput" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="lblQuantityOnHand" runat="server" CssClass="settinglabel" ConfigKey="ProductQuantityOnHandLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtQuantityOnHand" Columns="10" runat="server" MaxLength="20" Text="0" CssClass="forminput" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="lblIsVisible" runat="server" CssClass="settinglabel" ConfigKey="ProductShowInListLabel"
                                ResourceFile="WebStoreResources" ForControl="chkShowInProductList"  />
                            <asp:CheckBox ID="chkShowInProductList" runat="server" CssClass="forminput" />
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="ProductEnableRatingLabel"
                                ResourceFile="WebStoreResources" ForControl="chkEnableRating"  />
                            <asp:CheckBox ID="chkEnableRating" runat="server" CssClass="forminput" />
                        </div>
                        <div class="settingrow">&nbsp;</div>
                    </div>
                    <div id="tabAbstract">
                     <cye:EditorControl ID="edAbstract" runat="server">
                            </cye:EditorControl>
                    </div>
                    <div id="tabDescription">
                     <cye:EditorControl ID="edDescription" runat="server">
                            </cye:EditorControl>
                    </div>
                    <div id="tabFullfillment" runat="server">
                        <div class="settingrow">
                            <cy:SiteLabel ID="lblFullfillmentType" runat="server" CssClass="settinglabel" ConfigKey="ProductFullfillmentTypeLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddFullfillmentType" EnableTheming="false" runat="server" CssClass="forminput" />
                            <asp:HyperLink ID="lnkDownload" runat="server" />
                        </div>
                        <asp:Panel ID="pnlUpload" runat="server" Visible="false">
                        <asp:Panel ID="pnlFileUpload" runat="server" CssClass="settingrow"  DefaultButton="btnUpload">
                            <cy:SiteLabel ID="Sitelabel3" runat="server" CssClass="settinglabel" ConfigKey="FileUploadLabel"
                                ResourceFile="WebStoreResources"> </cy:SiteLabel>
                            <NeatUpload:InputFile ID="fileInput" Size="28" runat="server" />
                            &nbsp;&nbsp;
                            <portal:CButton ID="btnUpload" runat="server" Text="Upload" ValidationGroup="Upload" />
                            
                        </asp:Panel>
                        <div class="settingrow">
                        <cy:SiteLabel ID="SiteLabel10" runat="server" CssClass="settinglabel" ForControl="txtTeaserFileLinkText" ConfigKey="TeaserFileLinkTextLabel"
                                ResourceFile="WebStoreResources" />
                               <asp:TextBox ID="txtTeaserFileLinkText" runat="server" CssClass="mediumtextbox forminput" />
                        </div>
                        <asp:Panel ID="pnlTeaserUpload" runat="server" CssClass="settingrow"  DefaultButton="btnUploadTeaser">
                            <cy:SiteLabel ID="Sitelabel5" runat="server" CssClass="settinglabel" ConfigKey="TeaserFileUploadLabel"
                                ResourceFile="WebStoreResources"> </cy:SiteLabel>
                                <asp:HyperLink ID="lnkTeaserDownload" runat="server" Visible="false" />
                            <NeatUpload:InputFile ID="teaserFileInput" Size="28" runat="server" />
                            &nbsp;&nbsp;
                            <portal:CButton ID="btnUploadTeaser" runat="server" Text="Upload" ValidationGroup="Upload" />
                            
                            <div>
                            <cy:SiteLabel ID="Sitelabel11" runat="server" UseLabelTag="false" ConfigKey="TeaserFileInfo"
                                ResourceFile="WebStoreResources"> </cy:SiteLabel>
                            </div>
                            <NeatUpload:ProgressBar ID="progressBar" runat="server">
                                <cy:SiteLabel ID="progresBarLabel" runat="server" ConfigKey="CheckProgressText" />
                            </NeatUpload:ProgressBar>
                            <GreyBoxUpload:GreyBoxProgressBar id="gbProgressBar" runat="server" GreyBoxRoot="~/ClientScript/greybox">
				                <cy:siteLabel id="SiteLabel4" runat="server" ConfigKey="CheckProgressText"> </cy:siteLabel>
			                </GreyBoxUpload:GreyBoxProgressBar>
                        </asp:Panel>
                        </asp:Panel>
                        <div class="settingrow">&nbsp;</div>
                    </div>
                    <div id="tabMeta">
		                <div class="settingrow">
		                    <cy:SiteLabel id="SiteLabel6" runat="server" ForControl="txtMetaDescription" CssClass="settinglabel" ConfigKey="MetaDescriptionLabel" ResourceFile="WebStoreResources" > </cy:SiteLabel>
		                    <asp:TextBox id="txtMetaDescription" runat="server"  Columns="50" maxlength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
		                </div>
		                <div class="settingrow">
		                    <cy:SiteLabel id="SiteLabel7" runat="server" ForControl="txtMetaKeywords" CssClass="settinglabel" ConfigKey="MetaKeywordsLabel" ResourceFile="WebStoreResources" > </cy:SiteLabel>
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
            </div>
            <div>
                <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="Product" />
                <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName"
                    Display="None" ValidationGroup="Product" />
                <portal:CButton ID="btnSave" runat="server" ValidationGroup="Product" />
                <portal:CButton ID="btnDelete" runat="server" CausesValidation="false" />
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
