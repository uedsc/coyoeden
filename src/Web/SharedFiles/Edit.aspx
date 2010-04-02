<%@ Page language="c#" Codebehind="Edit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.SharedFilesUI.SharedFilesEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<div class="yui-skin-sam">
<asp:Panel id="pnlNotFound" runat="server" visible="True" CssClass="modulecontent">
	<cy:SiteLabel id="Sitelabel1" runat="server" ConfigKey="SharedFilesNotFoundMessage" ResourceFile="SharedFileResources" UseLabelTag="false"> </cy:SiteLabel>
</asp:Panel>

<asp:Panel id="pnlFolder" runat="server" CssClass="modulecontent" DefaultButton="btnUpdateFolder">
    <fieldset>
        <legend>	
            <cy:SiteLabel id="Sitelabel5" runat="server" ConfigKey="SharedFilesEditFolderLabel" ResourceFile="SharedFileResources" UseLabelTag="false"> </cy:SiteLabel>
        </legend>
        <div class="settingrow">
             <cy:SiteLabel id="Sitelabel10" runat="server" ForControl="ddFolderList" CssClass="settinglabel" ConfigKey="SharedFilesFolderParentLabel" ResourceFile="SharedFileResources"> </cy:SiteLabel>
            <asp:DropDownList id="ddFolderList" runat="server" EnableTheming="false" DataValueField="FolderID" DataTextField="FolderName" CssClass="forminput"></asp:DropDownList>
        </div>
        <div class="settingrow">
            <cy:SiteLabel id="Sitelabel8" runat="server" ForControl="txtFolderName" CssClass="settinglabel" ConfigKey="SharedFilesFolderNameLabel" ResourceFile="SharedFileResources"> </cy:SiteLabel>
            <asp:TextBox id="txtFolderName" runat="server"  Columns="45" maxlength="255" CssClass="forminput"></asp:TextBox>
        </div>
        <div class="settingrow">
        <cy:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
        <div class="forminput">
            <portal:CButton id="btnUpdateFolder" runat="server" />&nbsp;
		    <portal:CButton  id="btnDeleteFolder" runat="server" CausesValidation="false" />&nbsp;
		    <asp:HyperLink ID="lnkCancelFolder" runat="server" CssClass="cancellink" />&nbsp;	
		   </div>	
        </div>
    </fieldset>
</asp:Panel>
<asp:Panel id="pnlFile" runat="server" visible="False" CssClass="modulecontent" DefaultButton="btnUpdateFile">
   <fieldset>
        <legend>	
            <cy:SiteLabel id="lblEditLabel" runat="server" ConfigKey="SharedFilesEditLabel" ResourceFile="SharedFileResources" UseLabelTag="false"> </cy:SiteLabel>
        </legend>
        <div class="settingrow">
            <cy:SiteLabel id="lblUploadDateLabel" runat="server" CssClass="settinglabel" ConfigKey="SharedFilesUploadDateLabel" ResourceFile="SharedFileResources"> </cy:SiteLabel>
            <asp:Label id="lblUploadDate" runat="server" ></asp:Label>
        </div>
        <div class="settingrow">
            <cy:SiteLabel id="lblUploadByLabel" runat="server" CssClass="settinglabel" ConfigKey="SharedFilesUploadByLabel" ResourceFile="SharedFileResources"> </cy:SiteLabel>
            <asp:Label id="lblUploadBy" runat="server" ></asp:Label>
        </div>
        <div class="settingrow">
            <cy:SiteLabel id="lblFileNameLabel" runat="server" ForControl="txtFriendlyName" CssClass="settinglabel" ConfigKey="SharedFilesFileNameLabel" ResourceFile="SharedFileResources"> </cy:SiteLabel>
            <asp:TextBox id="txtFriendlyName" runat="server"  Columns="45" maxlength="255" CssClass="forminput widetextbox"></asp:TextBox>
        </div>
        <div class="settingrow">
            <cy:SiteLabel id="lblFileSizeLabel" runat="server" CssClass="settinglabel" ConfigKey="SharedFilesFileSizeLabel" ResourceFile="SharedFileResources"> </cy:SiteLabel>
            <asp:Label id="lblFileSize" runat="server" CssClass="Normal"></asp:Label>
        </div>
        <div class="settingrow">
            <cy:SiteLabel id="SiteLabel7" runat="server" CssClass="settinglabel" ConfigKey="FileDescription" ResourceFile="SharedFileResources" UseLabelTag="false"> </cy:SiteLabel>
        </div>
        <div class="settingrow">
            <cye:EditorControl ID="edDescription" runat="server"> </cye:EditorControl>
        </div>
        <div class="settingrow">
            <cy:SiteLabel id="Sitelabel2" runat="server" ForControl="ddFolders" CssClass="settinglabel" ConfigKey="SharedFilesFolderLabel" ResourceFile="SharedFileResources"> </cy:SiteLabel>
            <asp:DropDownList id="ddFolders" runat="server" EnableTheming="false" DataValueField="FolderID" DataTextField="FolderName" CssClass="forminput"></asp:DropDownList>
        </div>
        <div class="settingrow">
            <cy:SiteLabel id="Sitelabel3" runat="server" ForControl="file1" CssClass="settinglabel" ConfigKey="SharedFilesUploadLabel" ResourceFile="SharedFileResources"> </cy:SiteLabel>
            <div class="forminput">
            <NeatUpload:InputFile id="file1" size="28" runat="server" /> &nbsp;&nbsp;
		    <portal:CButton id="btnUpload" runat="server" Text="Upload" />
		    <NeatUpload:ProgressBar id="progressBar" runat="server"><cy:SiteLabel id="progresBarLabel" runat="server" ConfigKey="CheckProgressText" /></NeatUpload:ProgressBar>
		    <GreyBoxUpload:GreyBoxProgressBar id="gbProgressBar" runat="server" GreyBoxRoot="~/ClientScript/greybox">
				<cy:siteLabel id="SiteLabel6" runat="server" ConfigKey="CheckProgressText"> </cy:siteLabel>
			</GreyBoxUpload:GreyBoxProgressBar>
			</div>
        </div>
        <div class="settingrow">
             <portal:CButton  id="btnUpdateFile" runat="server" />&nbsp;
		    <portal:CButton id="btnDeleteFile" runat="server" CausesValidation="false" />&nbsp;
		    <asp:HyperLink ID="lnkCancelFile" runat="server" CssClass="cancellink" />&nbsp;	
        </div>
        <div class="settingrow">
            <portal:CLabel id="lblError" runat="server" cssclass="txterror" />
        </div>
        <div class="modulecontent">
            <cy:SiteLabel id="Sitelabel4" runat="server" ConfigKey="SharedFilesHistoryLabel" ResourceFile="SharedFileResources"> </cy:SiteLabel>
            <cy:CGridView ID="grdHistory" runat="server"
	             CssClass=""
	             AutoGenerateColumns="false"
                 DataKeyNames="ID"
                 EnableTheming="false"
	             >
                 <Columns>
		            <asp:TemplateField>
			            <ItemTemplate>
			                <asp:Button ID="lnkName" CssClass="FileManager buttonlink" runat="server" 
						        text='<%# DataBinder.Eval(Container.DataItem,"FriendlyName") %>' 
						        CommandName="download" 
						        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ServerFileName") %>' 
						        CausesValidation="false">
						    </asp:Button>
                        </ItemTemplate>
		            </asp:TemplateField>
		            <asp:TemplateField>
			            <ItemTemplate>
			               <%# DataBinder.Eval(Container.DataItem,"SizeInKB") %>
                        </ItemTemplate>
		            </asp:TemplateField>
		            <asp:TemplateField>
			            <ItemTemplate>
			             <%# Convert.ToDateTime(((System.Data.Common.DbDataRecord)Container.DataItem)["UploadDate"]).AddHours(TimeOffset).ToString()%>
			            </ItemTemplate>
		            </asp:TemplateField>
		            <asp:TemplateField>
			            <ItemTemplate>
			                <%# DataBinder.Eval(Container.DataItem,"ArchiveDate") %>
                        </ItemTemplate>
		            </asp:TemplateField>
		            <asp:TemplateField>
			            <ItemTemplate>
			                 <asp:Button ID="LinkButton1" runat="server" cssclass="buttonlink"
						        CommandName="restore" 
						        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>'
						        CausesValidation="false" 
						        text="<%# Resources.SharedFileResources.SharedFilesRestoreLabel %>">
						    </asp:Button>
                        </ItemTemplate>
		            </asp:TemplateField>
            </Columns>
            </cy:CGridView>     
        </div>
    </fieldset>
    <asp:HiddenField ID="hdnReturnUrl" runat="server" />
</asp:Panel>
</div>
<portal:SessionKeepAliveControl id="ka1" runat="server" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
