<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS00.aspx.vb" Inherits="Master_KTMS00" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br /><br /><br />Master Management
        <asp:DropDownList ID="ddlMenu" runat="server">
            <asp:ListItem>-- Please select --</asp:ListItem>
            <asp:ListItem>Vendor</asp:ListItem>
            <asp:ListItem>Item</asp:ListItem>
            <asp:ListItem>I/E</asp:ListItem>
            <asp:ListItem>VAT</asp:ListItem>
            <asp:ListItem>W/T</asp:ListItem>
            <asp:ListItem>Unit</asp:ListItem>
            <asp:ListItem>Payment Term</asp:ListItem>
            <asp:ListItem>Job Order Type</asp:ListItem>
            <asp:ListItem>Currency</asp:ListItem>
            <asp:ListItem>Working Category</asp:ListItem>
            <asp:ListItem>Country</asp:ListItem>
            <asp:ListItem>Department</asp:ListItem>
        </asp:DropDownList>
        <br /><br /><br /
    </div>
</asp:Content>

