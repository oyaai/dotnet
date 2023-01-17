<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS24.aspx.vb" Inherits="Master_KTMS24" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS23.aspx?New=True">Master Management</a> > Department</div>
        <br />
        <div class="font_header">DEPARTMENT MANAGEMENT</div> 
        <br />
        <table class="table_field"  style="width: 100%;">
            <tr>
                <td class="table_field_td">ID </td>
                <td class="table_field_td"><asp:TextBox ID="txtId" Enabled="false" runat="server" 
                        CssClass="textbox_read_only text_field"></asp:TextBox> </td>
            </tr>
            <tr>
                <td class="table_field_td">Department <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtName" runat="server" MaxLength="100" CssClass="text_field"></asp:TextBox>
                    <asp:Label ID="lblErrName" runat="server" CssClass="font_error" Visible="false" Text="*Require"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button_style" CausesValidation="false" />
                    &nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear"  CssClass="button_style" CausesValidation="false" />
                    &nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back"  CssClass="button_style" 
                        CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

