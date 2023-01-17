<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS12.aspx.vb" Inherits="Master_KTMS12" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS11.aspx?New=True">Master Management</a> > Unit</div>
        <br />
        <div class="font_header">UNIT MANAGEMENT</div> 
        <br />
        <table class="table_field">  <%--style="width: 100%;"--%>
            <tr>
                <td class="table_field_td tb_Fix80">ID</td>
                <td class="table_field_td tb_Fix250">
                    <asp:TextBox ID="txtUnitId" Enabled="false" runat="server" CssClass="textbox_read_only text_field" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix80">Unit <asp:Label ID="lblReq1" runat="server" CssClass="font_require" Text="*"></asp:Label></td>
                <td class="table_field_td tb_Fix250">
                    <asp:TextBox ID="txtUnitName" CssClass="text_field" MaxLength="50" runat="server" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="font_error" runat="server"
                         ErrorMessage="*Required" ControlToValidate="txtUnitName"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button_style" CausesValidation="true" />
                    &nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear"  CssClass="button_style" CausesValidation="false" />
                    &nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back"  CssClass="button_style" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

