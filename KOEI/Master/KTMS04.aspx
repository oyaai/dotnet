<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS04.aspx.vb" Inherits="Master_KTMS04" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTMS03.aspx?New=True">Master Management</a> > Item</div>
        <br /><div class="font_header">ITEM MANAGEMENT</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix200">ID </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtItemID" runat="server" Width="100px" MaxLength="100" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Name <span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtItemName" runat="server" Width="290px" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqValidatorItemName" runat="server" 
                        ErrorMessage="*Require." ControlToValidate="txtItemName" class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Vendor Name <span class="font_require">*</span></td>
                <td class="table_field_td">
                     <asp:DropDownList ID="ddlVendor" runat="server" Width="290px">
                        </asp:DropDownList>
                     <asp:RequiredFieldValidator ID="reqValidatorVendor" runat="server" 
                        ErrorMessage="*Require." ControlToValidate="ddlVendor" class="font_error">
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

