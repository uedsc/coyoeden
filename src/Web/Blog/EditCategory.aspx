<%@ Page language="c#" Codebehind="EditCategory.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.BlogUI.BlogCategoryEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
		<cy:CornerRounderTop id="ctop1" runat="server" />
		<asp:Panel id="pnlBlog" runat="server" DefaultButton="btnAddCategory" cssclass="art-Post-inner panelwrapper blogcategoryedit">
		    <h2 class="moduletitle"><cy:SiteLabel id="lbl1" runat="server" ConfigKey="BlogCategoriesLabel" ResourceFile="BlogResources" UseLabelTag="false"> </cy:SiteLabel></h2>
		    <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
		    <div class="modulecontent">
		    <div class="settingrow">
		        <asp:Label ID="lblError" Runat="server" CssClass="txterror"></asp:Label>
		    </div>
		    <div class="settingrow">
		        <asp:Button  runat="server" id="btnAddCategory"></asp:Button>
				<asp:TextBox ID="txtNewCategoryName" Runat="server" MaxLength="50" Columns="50"></asp:TextBox>
		    </div>
		    <div class="settingrow">
		        <portal:CDataList id="dlCategories" DataKeyField="CategoryID" runat="server">
					<ItemTemplate>
						<asp:ImageButton runat="server" ID="btnEdit"
						    ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
						    CommandName="edit" ToolTip="<%# Resources.BlogResources.EditImageAltText%>"
						    AlternateText="<%# Resources.BlogResources.EditImageAltText%>" 
						        />
						<asp:ImageButton runat="server" ID="btnDelete"
						    ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + DeleteLinkImage %>'
						    CommandName="delete" ToolTip="<%# Resources.BlogResources.BlogEditDeleteButton%>"
						    AlternateText="<%# Resources.BlogResources.BlogEditDeleteButton%>" 
						 />
						&nbsp;&nbsp;
						<%# DataBinder.Eval(Container.DataItem, "Category") %>
					</ItemTemplate>
					<EditItemTemplate>
					    <div >
						<asp:Textbox id="CategoryName" runat="server"
						    MaxLength="50" 
						    Columns="50" CssClass="widetextbox"
						    Text='<%# DataBinder.Eval(Container.DataItem, "Category") %>'  />&nbsp;
						<asp:Button Text="<%# Resources.BlogResources.BlogEditUpdateButton%>" ToolTip="<%# Resources.BlogResources.BlogEditUpdateButton%>"
						    CommandName="apply" runat="server" ID="Button1" />
						</div>
					</EditItemTemplate>
				</portal:CDataList>
		    </div>
		    </div>
		    </portal:CPanel>
		    <div class="cleared"></div>
		</asp:Panel>
		<cy:CornerRounderBottom id="cbottom1" runat="server" />	
		</portal:CPanel>
	</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
