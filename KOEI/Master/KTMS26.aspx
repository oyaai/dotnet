<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS26.aspx.vb" Inherits="Master_KTMS26" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS25.aspx?New=True">Master Management</a> >Payment Condition</div>
        <br />
        <div class="font_header">PAYMENT CONDITION MANAGEMENT</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix100">&nbsp;&nbsp;ID </td>
                <td class="table_field_td tb_Fix500">
                    <asp:TextBox ID="txtPayID" runat="server" Width="150px" MaxLength="100" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">&nbsp;&nbsp;1st <span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtFirst" runat="server" Width="150px" MaxLength="3"></asp:TextBox>
                    &nbsp;%
                    <asp:RequiredFieldValidator ID="reqValidatorPayFirst" runat="server" 
                        ErrorMessage="*Require." ControlToValidate="txtFirst" class="font_error">
                    </asp:RequiredFieldValidator>               
                    <asp:RangeValidator ID="RangeValidator1st" runat="server" 
                        ControlToValidate="txtFirst" ErrorMessage="" MaximumValue="100" 
                        MinimumValue="0" Type="Integer"></asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">&nbsp;&nbsp;2nd <span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtSecond" runat="server" Width="150px" MaxLength="3"></asp:TextBox>
                    &nbsp;%
                    <asp:RequiredFieldValidator ID="reqValidatorPaySecond" runat="server" 
                        ErrorMessage="*Require." ControlToValidate="txtSecond" class="font_error">
                    </asp:RequiredFieldValidator>                 
                    <asp:RangeValidator ID="RangeValidator2nd" runat="server" 
                        ControlToValidate="txtSecond" ErrorMessage="" MaximumValue="100" 
                        MinimumValue="0" Type="Integer"></asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">&nbsp;&nbsp;3rd <span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtThird" runat="server" Width="150px" MaxLength="3"></asp:TextBox>
                    &nbsp;%
                    <asp:RequiredFieldValidator ID="reqValidatorPayThird" runat="server" 
                        ErrorMessage="*Require." ControlToValidate="txtThird" class="font_error">
                    </asp:RequiredFieldValidator>               
                    <asp:RangeValidator ID="RangeValidator3rd" runat="server" 
                        ControlToValidate="txtThird" ErrorMessage="" MaximumValue="100" 
                        MinimumValue="0" Type="Integer"></asp:RangeValidator>
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
