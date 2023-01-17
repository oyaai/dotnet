<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS20.aspx.vb" Inherits="Master_KTMS20" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS19.aspx?New=True">Master Management</a> > Working Category</div>
        <br />
        <div class="font_header">WORKING CATEGORY MANAGEMENT</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix120">ID </td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtWorkingCategoryID" runat="server" Width="100px" MaxLength="100" Enabled="false" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Working Category <span class="font_require">*</span></td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtWorkCat" runat="server" Width="135px" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="txtWorkCat_RequiredFieldValidator" runat="server" 
                        ControlToValidate="txtWorkCat" SetFocusOnError="True" ErrorMessage="*Require"></asp:RequiredFieldValidator>
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

