<%@ Control Language="c#" AutoEventWireup="true" Codebehind="SharedFilesModule.ascx.cs" Inherits="Cynthia.Web.SharedFilesUI.SharedFilesModule" %>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="art-Post-inner panelwrapper sharedfiles">
<portal:ModuleTitleControl id="Title1" runat="server" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
<asp:UpdatePanel ID="upFiles" UpdateMode="Conditional" runat="server">
<ContentTemplate>

<asp:Panel ID="pnlFile" runat="server" DefaultButton="btnUpload" >
<table class="FileManager_table" cellspacing="0" cellpadding="0" width="99%" border="0">
	<tr >
		<td><asp:ImageButton id="btnGoUp" runat="server" OnClick="btnGoUp_Click"
                AlternateText="" ImageUrl="~/Data/SiteImages/btnUp.jpg" />                               
		    <asp:ImageButton id="btnDelete" runat="server" OnClick="btnDelete_Click"
		        AlternateText="Delete" ImageUrl="~/Data/SiteImages/btnDelete.jpg" 
		                                       ToolTip="<%# Resources.SharedFileResources.SharedFilesDeleteButton %>" />
		                                        &nbsp;&nbsp;
		   <asp:label id="lblCurrentDirectory" Runat="server" CssClass="foldername"></asp:label>
			&nbsp;&nbsp;<asp:Label id="lblError" runat="server" CssClass="txterror"></asp:Label>
		</td>
	</tr>
	<tr>
		<td>
		
		    <cy:CGridView     id="dgFile" 
		          runat="server" 
		          DataKeyNames="ID"
		          EnableTheming="false"
		          SkinID="FileManager"
		          Allowsorting="True"  
		          OnRowCancelingEdit="dgFile_RowCancelingEdit"
		          OnRowCommand="dgFile_RowCommand"
		          OnRowDataBound="dgFile_RowDataBound"
		          OnRowEditing="dgFile_RowEditing"
		          OnRowUpdating="dgFile_RowUpdating"
		          OnSorting="dgFile_Sorting"
		          Autogeneratecolumns="False">
				<Columns>
					<asp:TemplateField>
						<ItemTemplate>
							<asp:CheckBox ID="chkChecked" runat="server" Visible='<%# IsEditable%>' />
							<asp:HyperLink runat="server" Text="<%# Resources.SharedFileResources.SharedFilesEditLink %>" 
							    ToolTip="<%# Resources.SharedFileResources.SharedFilesEditLink %>"
							                       id="editLink" 
							                       ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
							                       NavigateUrl='<%# this.SiteRoot + "/SharedFiles/Edit.aspx?pageid=" + PageId.ToString() + "&ItemID=" + DataBinder.Eval(Container.DataItem,"ID") + "&mid=" + ModuleId.ToString()  %>' 
							                       Visible="<%# IsEditable %>" 
							                        />
							                        
						<asp:HyperLink runat="server" Text="<%# Resources.SharedFileResources.SharedFilesDownloadLink %>" 
					                                ToolTip="<%# Resources.SharedFileResources.SharedFilesDownloadLink %>"
							                       id="downloadLink" 
							                       ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/Download.gif" %>' 
							                       NavigateUrl='<%# this.SiteRoot + "/SharedFiles/Download.aspx?pageid=" + PageId.ToString() + "&fileid=" + DataBinder.Eval(Container.DataItem,"ID").ToString().Replace("~file", String.Empty) + "&mid=" + ModuleId.ToString()  %>' 
							                       Visible='<%# (DataBinder.Eval(Container.DataItem,"type").ToString().ToLower() == "1") %>' 
							                        />                      
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField SortExpression="filename">
						<ItemTemplate>
							<asp:PlaceHolder id="plhImgEdit" runat="server"></asp:PlaceHolder>
							<asp:Image ID="imgType" runat="server" AlternateText=" " ImageUrl="~/Data/SiteImages/Icons/unknown.gif"  />
							<asp:Button id="lnkName" runat="server" 
							                        cssclass ="buttonlink" 
							                        text='<%# DataBinder.Eval(Container.DataItem,"filename") %>' 
							                        commandname="ItemClicked" 
							                        CommandArgument='<%# Eval("ID") %>'
							                        causesvalidation="false" 
							                        visible='<%# (DataBinder.Eval(Container.DataItem,"type").ToString().ToLower() != "1") %>'
							                        />
							<asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem,"filename") %>' 
					                                ToolTip='<%# DataBinder.Eval(Container.DataItem,"filename") %>'
							                       id="fileLink" 
							                       NavigateUrl='<%# this.SiteRoot + "/SharedFiles/Download.aspx?pageid=" + PageId.ToString() + "&fileid=" + DataBinder.Eval(Container.DataItem,"ID").ToString().Replace("~file", String.Empty) + "&mid=" + ModuleId.ToString()  %>' 
							                       Visible='<%# (DataBinder.Eval(Container.DataItem,"type").ToString().ToLower() == "1") %>' 
							                       runat="server" />
							        
						</ItemTemplate>
						<EditItemTemplate>
						    <asp:Panel ID="PnlRename" runat="server" DefaultButton="btnRename">
							<asp:PlaceHolder ID="Placeholder1" runat="server"></asp:PlaceHolder>
							<asp:Image ID="imgEditType" runat="server" ImageUrl="~/Data/SiteImages/Icons/unknown.gif" />
							<asp:TextBox ID="txtEditName" runat="server" columns="50"  
							                      Text='<%# DataBinder.Eval(Container.DataItem,"filename") %>'></asp:TextBox>
							</asp:Panel>
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField SortExpression="Description">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem,"Description") %>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField SortExpression="size">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem,"size") %>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField SortExpression="DownloadCount">
						<ItemTemplate>
							<%# DataBinder.Eval(Container.DataItem, "DownloadCount")%>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField SortExpression="modified">
						<ItemTemplate>
						    <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.DataRowView)Container.DataItem),"modified", TimeOffset)%>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField SortExpression="username">
						<ItemTemplate>
							&nbsp;&nbsp;&nbsp;<%# DataBinder.Eval(Container.DataItem, "username")%>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemTemplate>
							<asp:Button id="LinkButton1"  runat="server" Visible='<%# IsEditable%>' CssClass="buttonlink"
							     CommandName="Edit" CommandArgument='<%# Eval("ID") %>' CausesValidation="false" Text="<%# Resources.SharedFileResources.FileManagerRename %>" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:Button id="btnRename" runat="server" 
							    CommandName="Update" 
							    CommandArgument='<%# Eval("ID") %>'
							    Text="<%# Resources.SharedFileResources.SharedFilesUpdateButton %>" />&nbsp;
							<asp:Button ID="LinkButton2" runat="server" 
							    CommandName="Cancel" 
							    CausesValidation="false" 
							    Text="<%# Resources.SharedFileResources.SharedFilesCancelButton %>" />
						</EditItemTemplate>
					</asp:TemplateField>
				</Columns>
			 </cy:CGridView>
		
			<asp:HiddenField ID="hdnCurrentFolderId" runat="server" Value="-1" />
		</td>
	</tr>
	<tr id="trObjectCount" runat="server" class="moduletitle">
		<td>
		    <asp:Label id="lblCounter" Runat="server"></asp:Label>
		 </td>
	</tr>
	<tr>
		<td></td>
	</tr>
</table>
</asp:Panel>
<table cellspacing="0" cellpadding="2" width="99%" border="0" id="tblNewFolder" runat="server">
    <tr>
		<td colspan="2">&nbsp;</td>
	</tr>
	<tr>
		<td colspan="2">
		<asp:Panel ID="pnlNewFolder" runat="server" DefaultButton="btnNewFolder">
		    <asp:TextBox id="txtNewDirectory" runat="server" Width="224px"></asp:TextBox>
		    <portal:CButton id="btnNewFolder" runat="server" Text="" OnClick="btnNewFolder_Click" />
		</asp:Panel>
		</td>
	</tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>

<asp:Panel ID="pnlUpload" runat="server" DefaultButton="btnUpload" CssClass="settingrow uploadpanel">
<NeatUpload:MultiFile ID="multiFile" runat="server" UseFlashIfAvailable="true">
       <portal:CButton ID="btnAddFile"  Enabled="true" runat="server" />
</NeatUpload:MultiFile>
<br />
<portal:CButton id="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"  />

</asp:Panel>
	   
</div>
</portal:CPanel>
<div class="cleared"></div>
<NeatUpload:ProgressBar id="progressBar" runat="server" ><cy:siteLabel id="progresBarLabel" runat="server" ConfigKey="CheckProgressText" /></NeatUpload:ProgressBar>
<GreyBoxUpload:GreyBoxProgressBar id="gbProgressBar" runat="server" GreyBoxRoot="~/ClientScript/greybox">
				<cy:siteLabel id="SiteLabel1" runat="server" ConfigKey="CheckProgressText"> </cy:siteLabel>
			</GreyBoxUpload:GreyBoxProgressBar>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
