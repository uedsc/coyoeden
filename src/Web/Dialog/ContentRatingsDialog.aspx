<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master"
    CodeBehind="ContentRatingsDialog.aspx.cs" Inherits="Cynthia.Web.AdminUI.ContentRatingsDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
    <portal:ScriptLoader ID="ScriptInclude" runat="server" IncludeYuiDataTable="true" />
    <div style="padding: 5px 5px 5px 5px;" class="yui-skin-sam">
        <cy:CGridView ID="grdContentRating" runat="server" CssClass="" AutoGenerateColumns="false"
            DataKeyNames="RowGuid" EnableTheming="false">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("EmailAddress") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("IpAddress") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("CreatedUtc"), timeOffset)%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("Rating") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" EnableViewState="false"
                            ClientScriptUrl="~/ClientScript/NeatHtml.js">
                            <%# Eval("Comments") %>
                        </NeatHtml:UntrustedContent>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnDeleteRating" runat="server" Text='<%# Resources.Resource.DeleteButton %>'
                            CssClass="buttonlink" CommandArgument='<%# Eval("RowGuid") %>' CommandName='DeleteRating' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cy:CGridView>
        <div class="modulepager">
            <portal:CCutePager ID="pgr" runat="server" />
        </div>
    </div>
</asp:Content>
