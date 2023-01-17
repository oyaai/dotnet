<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS08.aspx.vb" Inherits="Master_KTMS08" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />

    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS07.aspx?New=True">Master Management</a> &gt; Vat</div>
        <br />
        <div class="font_header">VAT MANAGEMENT</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix120">ID</td>
                <td class="table_field_td tb_Fix600">
                <asp:TextBox ID="txtID" runat="server" Width="100px" MaxLength="100" Enabled="false" /></td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Vat (%) <span class="font_require">*</span></td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtVat" runat="server" Width="290px" MaxLength="3"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtVat_FilteredTextBoxExtender" runat="server" 
                        Enabled="True" FilterType="Numbers" TargetControlID="txtVat">
                    </asp:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="txtVatRequiredFieldValidator" runat="server" 
                        ErrorMessage="*Require." ControlToValidate="txtVat" class="font_error" Display="Dynamic" SetFocusOnError="True">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button_style" CommandName="Save"/>
                    &nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button_style" CausesValidation="false" CommandName="Clear" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="button_style" CausesValidation="false" CommandName="Back" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>