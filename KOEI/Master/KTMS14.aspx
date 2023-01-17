<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS14.aspx.vb" Inherits="Master_KTMS14" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <div class="text_center">
    <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTMS13.aspx?New=True">Master Management</a> > Payment Term</div>
        <br />
        <div class="font_header">PAYMENT TERM MANAGEMENT</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix150"> ID</td>
                <td class="table_field_td tb_Fix550">
                <asp:TextBox ID="txtID" runat="server" Width="100px" MaxLength="100" Enabled="false" />
                </td>
            </tr>
            
            <tr>
                <td class="table_field_td tb_Fix150"> Payment Term (days)<span class="font_require">*</span></td>
                <td class="table_field_td tb_Fix550">
                    <asp:TextBox ID="txtPayment" runat="server" Width="290px" MaxLength="100"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtPayment_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtPayment">
                    </asp:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="txtPayment_RequiredFieldValidator" runat="server" 
                    ControlToValidate="txtPayment" ErrorMessage="*Require" class="font_error" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>
            
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button_style" 
                        CommandName="Create"/>
                    &nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear"  CssClass="button_style" CausesValidation="false" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back"  CssClass="button_style" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

