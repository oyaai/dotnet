<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTPU09.aspx.vb" Inherits="Purchase_KTPU09"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>
    <div class="text_center">
        <br />
        <div class="font_header">PURCHASE HISTORY REPORT</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td">Job Order</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder" runat="server" MaxLength="6"></asp:TextBox>
                </td>
                <td class="table_field_td">Invoice No.</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">PO No.</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtPoNo" runat="server" MaxLength="20"></asp:TextBox>
                </td>
                <td class="table_field_td">Vendor Name</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtVendorName" Width="300px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Item Name</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtItemName" runat="server"></asp:TextBox>
                </td>
                <td class="table_field_td">Delivery Date (dd/mm/yyyy)</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDeliveryDateFrom" runat="server" MaxLength="8"></asp:TextBox>
                    <asp:CalendarExtender ID="txtSearchDeliveryDateTo_CalendarExtender" runat="server"
                        Format="dd/MM/yyyy" TargetControlID="txtDeliveryDateFrom">
                    </asp:CalendarExtender>
                    &nbsp;-
                    <asp:TextBox ID="txtDeliveryDateTo" runat="server" MaxLength="8"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                        Format="dd/MM/yyyy" TargetControlID="txtDeliveryDateTo">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnPDF" runat="server" Text="PDF" CssClass="button_style"/>&nbsp;&nbsp;
                </td>
            </tr>         
        </table> 
                            
       
    </div>

</asp:Content>

