<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="AdminDownloadHistory.aspx.cs" Inherits="WebStore.UI.AdminDownloadHistoryPage" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">

<div  style="padding: 5px 5px 5px 5px;" class="downloadhx">

<cy:CGridView ID="grdDownloadHistory" runat="server" 
    AllowPaging="false" 
    AllowSorting="false"
    CssClass="" AutoGenerateColumns="false"
    DataKeyNames="Guid" EnableTheming="false" SkinID="plain" >
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("UTCTimestamp")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("IPAddress")%>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
 </cy:CGridView>

</div>
</asp:Content>
