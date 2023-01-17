<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS18.aspx.vb" Inherits="Master_KTMS18" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTMS17.aspx?New=True">Master Management</a> > Currency</div>
        <br /><div class="font_header">CURRENCY MANAGEMENT</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix120">ID </td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtID" runat="server" Width="100px" MaxLength="100" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Currency <span class="font_require">*</span></td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtCurrency" runat="server" Width="290px" MaxLength="10"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="txtCurrency_RequiredFieldValidator" runat="server" 
                        ErrorMessage="*Require." ControlToValidate="txtCurrency" class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
                       
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button_style" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear"  CssClass="button_style" CausesValidation="false" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back"  CssClass="button_style" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

