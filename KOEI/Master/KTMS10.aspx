<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS10.aspx.vb" Inherits="Master_KTMS10" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"/>
    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTMS09.aspx?New=True">Master Management</a> > W/T</div>
        <br /><div class="font_header">W/T MANAGEMENT</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix120">ID </td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtID" runat="server" Width="100px" MaxLength="100" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">W/T (%) <span class="font_require">*</span></td>
                <td class="table_field_td tb_Fix600">
                <asp:TextBox ID="txtWT" runat="server" Width="290px" MaxLength="3"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtWTFilteredTextBoxExtender" runat="server" Enabled="True"
                        TargetControlID="txtWT" FilterType="Numbers" />
                <asp:RangeValidator ID="txtWTRangeValidator" runat="server" 
                        ControlToValidate="txtWT" MaximumValue="100"
                        MinimumValue="0" Display="Dynamic" Type="Integer" SetFocusOnError="True" />
                <asp:RequiredFieldValidator ID="reqtxtWT" runat="server" ControlToValidate="txtWT"
                        ErrorMessage="*Require" class="font_error" Display="Dynamic" SetFocusOnError="True" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Type</td>
                <td class="table_field_td tb_Fix600">
                     <asp:TextBox ID="txtType" runat="server" Width="290px" MaxLength="30"></asp:TextBox>
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
