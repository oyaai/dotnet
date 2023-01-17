<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTJB04.aspx.vb" Inherits="JobOrder_KTJB04" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a href="KTJB03.aspx?New=True">Master Management</a> > Section Order</div>
        <br /><div class="font_header">SECTION ORDER MANAGEMENT</div> 
        <br />
        <table class="table_field"  style="width: 100%;">
            <tr>
                <td class="table_field_td">Section Order<span class="font_require">*</span></td>
                <td class="table_field_td"> 
                    <asp:TextBox ID="txtJobOrder" runat="server" Width="83px" MaxLength="6"  ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqValidatorJobOrder" runat="server" SetFocusOnError="True"
                        ControlToValidate="txtJobOrder" ErrorMessage="*Require." class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Remarks</td>
                <td class="table_field_td">
                   <asp:TextBox ID="txtRemark" runat="server" Width="350px" TextMode="MultiLine" 
                        CssClass="font" onkeypress="if (this.value.length > 255) { return false; }" >
                   </asp:TextBox>
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

