<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="FileManager.ascx.cs"
    Inherits="Cynthia.Web.AdminUI.FileManagerControl" %>
<portal:CLabel ID="lblDisabledMessage" runat="server" CssClass="txterror" />
<asp:Panel ID="pnlFile" runat="server" DefaultButton="btnUpload">
    <asp:PlaceHolder ID="myPlaceHolder" runat="server"></asp:PlaceHolder>
    <input id="hdnUploadID" type="hidden" name="hdnUploadID" />
    <asp:HiddenField ID="hdnCurDir" runat="server" />
    <table cellspacing="1" width="99%">
        <tr>
            <td class="">
                <asp:ImageButton ID="btnGoUp" runat="server" ImageUrl="/images/btnUp.jpg" AlternateText="" />
                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="/images/btnDelete.jpg" AlternateText="Delete" />
                &nbsp;&nbsp;<asp:Label ID="lblCurrentDirectory" runat="server" CssClass="foldername"></asp:Label>
                &nbsp;&nbsp;<portal:CLabel ID="lblError" runat="server" CssClass="txterror" />
            </td>
        </tr>
        <tr>
            <td>
                <cy:CGridView ID="dgFile" runat="server" DataKeyNames="type" EnableTheming="false"
                    SkinID="FileManager" AutoGenerateColumns="False" AllowSorting="True">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkChecked" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="fileName">
                            <ItemTemplate>
                                &nbsp;
                                <asp:PlaceHolder ID="plhImgEdit" runat="server"></asp:PlaceHolder>
                                <asp:Image ID="imgType" runat="server" AlternateText=" "></asp:Image>
                                <asp:Button ID="lnkName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"filename") %>'
                                    CommandName="ItemClicked" CommandArgument='<%# Eval("path").ToString() + "~" + Eval("type").ToString()  %>'
                                    CausesValidation="false" CssClass="buttonlink"></asp:Button>
                            </ItemTemplate>
                            <EditItemTemplate>
                                &nbsp;
                                <asp:PlaceHolder ID="Placeholder1" runat="server"></asp:PlaceHolder>
                                <asp:Image ID="imgEditType" runat="server"></asp:Image>
                                <asp:TextBox ID="txtEditName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"filename") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="fileName">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem,"size") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="fileName">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem,"modified") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="LinkButton1" runat="server" CssClass="buttonlink" CommandName="Edit"
                                    CausesValidation="false" Text="<%# Resources.Resource.FileManagerRename %>">
                                </asp:Button>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="LinkButton3" runat="server" CommandName="Update" Text='<%# Resources.Resource.FileManagerUpdateButton %>'>
                                </asp:Button>&nbsp;
                                <asp:Button ID="LinkButton2" runat="server" CommandName="Cancel" CausesValidation="false"
                                    Text="<%# Resources.Resource.FileManagerCancelButton %>"></asp:Button>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </cy:CGridView>
            </td>
        </tr>
        <tr>
            <td>
                <div class="moduletitle">
                    <asp:Label ID="lblCounter" runat="server"></asp:Label>
                </div>
            </td>
        </tr>
    </table>
    <table id="tblUpload" runat="server" class="fileupload" cellspacing="1" width="99%">
        <tr>
            <th colspan="2">
            </th>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="pnlNewFolder" runat="server" DefaultButton="btnNewFolder">
                    <asp:TextBox ID="txtNewDirectory" runat="server" CssClass="mediumtextbox"></asp:TextBox>
                    <asp:Button ID="btnNewFolder" runat="server" Text="" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="moduletitle">
                    <cy:SiteLabel ID="lblGalleryEditImageLabel" runat="server" ConfigKey="FileManagerUploadLabel">
                    </cy:SiteLabel>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="pnlUpload" runat="server" DefaultButton="btnUpload">
                    <NeatUpload:MultiFile ID="multiFile" runat="server" UseFlashIfAvailable="true">
                        <asp:Button ID="btnAddFile" Enabled="true" runat="server" />
                    </NeatUpload:MultiFile>
                    <GreyBoxUpload:GreyBoxProgressBar ID="gbProgressBar" runat="server" GreyBoxRoot="~/ClientScript/greybox" Visible="false">
                        <cy:SiteLabel ID="progresBarLabel" runat="server" ConfigKey="CheckProgressText">
                        </cy:SiteLabel>
                    </GreyBoxUpload:GreyBoxProgressBar>
                    <NeatUpload:ProgressBar ID="progressBar" runat="server">
                        <cy:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="CheckProgressText"> </cy:SiteLabel>
                    </NeatUpload:ProgressBar>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="fileupload" colspan="2">
                <asp:Button ID="btnUpload" runat="server" Text="Upload"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Panel>
