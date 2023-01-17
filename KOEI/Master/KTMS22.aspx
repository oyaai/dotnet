<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS22.aspx.vb" Inherits="Master_KTMS22" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTMS21.aspx?New=True">Master Management</a> > Country</div>
        <br /><div class="font_header">COUNTRY MANAGEMENT</div> 
        <br />
        <table class="table_field"  style="width: 100%;">
            <tr>
                <td class="table_field_td">ID </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtCountryID" runat="server" Width="100px" MaxLength="100" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Country<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtCountryName" runat="server" Width="135px" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqValidatorCountryName" runat="server" SetFocusOnError="True"
                        ErrorMessage="*Require." ControlToValidate="txtCountryName" class="font_error">
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

