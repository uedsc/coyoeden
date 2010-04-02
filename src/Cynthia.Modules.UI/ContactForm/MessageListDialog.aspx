<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master"
    CodeBehind="MessageListDialog.aspx.cs" Inherits="Cynthia.Web.ContactUI.MessageListDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
    <portal:ScriptLoader ID="ScriptInclude" runat="server" IncludejQueryLayout="true" />
    
<script type="text/javascript">

function GetMessage(messageGuid, context)
 {
    
    <%= sCallBackFunctionInvocation %>
 }
 
 function ShowMessage(message, context) 
 {
    document.getElementById('<%= pnlMessage.ClientID %>').innerHTML = message;
 }
 
 function OnError(message, context) {
    //alert('An unhandled exception has occurred:\n' + message);
 }
 
</script>
    <asp:Panel ID="pnlContainer" runat="server" CssClass="ui-layout-container">
        <asp:Panel ID="pnlLeft" runat="server" CssClass="ui-layout-west">
            <cy:CGridView ID="grdContactFormMessage" runat="server" AllowPaging="false" AllowSorting="false"
                 CssClass="" AutoGenerateColumns="false" DataKeyNames="RowGuid" EnableTheming="false"
                SkinID="plain">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Url") %><br />
                            <a href='mailto:<%# Eval("Email") %>'>
                                <%# Eval("Email") %></a><br />
                            <%# Eval("Subject") %><br />
                            <%# Eval("CreatedUtc") %><br />
                            <asp:Button ID="btnView" runat="server" Text='<%# Resources.ContactFormResources.ContactFormViewButton %>'
                                CommandArgument='<%# Eval("RowGuid") %>' CommandName="view" OnClientClick='<%# GetViewOnClick(Eval("RowGuid").ToString()) %>' />
                            <asp:Button ID="btnDelete" runat="server" Text='<%# Resources.ContactFormResources.ContactFormDeleteButton %>'
                                CommandArgument='<%# Eval("RowGuid") %>' CommandName="remove" OnClientClick='<%# GetDeleteOnClick(Eval("RowGuid").ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </cy:CGridView>
            <div class="modulepager">
                <portal:CCutePager ID="pgrContactFormMessage" runat="server" />
            </div>
            <div class="modulepager">
                <asp:HyperLink ID="lnkRefresh" runat="server" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlCenter" runat="server" CssClass="ui-layout-center">
            <asp:Literal ID="litMessage" runat="server" />
            <asp:Panel ID="pnlMessage" runat="server" CssClass="contactmessage">
            </asp:Panel>
            <br class="clear" />
        </asp:Panel>
    </asp:Panel>
</asp:Content>
