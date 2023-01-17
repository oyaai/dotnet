<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTML01.aspx.vb" Inherits="Material_KTML01" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <div class="text_left" style ="width:1050px;">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTML01.aspx?New=True">Material List</a> > Material List</div>
        <br /><div class="font_header">SEARCH MATERIAL LIST</div> 
        <br />  
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix140">
                    Job Order
                </td>
                <td class="table_field_td tb_Fix370">
                    <asp:TextBox ID="txtJobOrder" runat="server" MaxLength="6" Width="150px" />
                </td>
                <td class="table_field_td tb_Fix140">
                    Vendor
                </td>
                <td class="table_field_td tb_Fix370">
                    <asp:TextBox ID="txtVendorName" runat="server" Width="150px" MaxLength="150" />
                </td>                
            </tr>
            <tr>
                <td class="table_field_td">
                    Invoice No.
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtInvoiceNo" runat="server" Width="150px" MaxLength="100" />
                </td>
                <td class="table_field_td">
                    PO No.
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtPONo" runat="server" Width="150px" MaxLength="20"/>
                </td>
                
            </tr>
            <tr>
                <td class="table_field_td">
                    Item Name
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtItemName" runat="server" Width="150px" MaxLength="100" />
                </td>
                <td class="table_field_td">
                    Delivery Date (dd/mm/yyyy)
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDelivertyDateFrom" runat="server" Width="100px" MaxLength="10" />
                    <asp:CalendarExtender ID="txtDelivertyDateFrom_CalendarExtender" runat="server" 
                        Format="dd/MM/yyyy"  TargetControlID="txtDelivertyDateFrom">
                    </asp:CalendarExtender>   
                     <asp:FilteredTextBoxExtender ID="txtDelivertyDateFrom_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtDelivertyDateFrom" 
                        ValidChars="1234567890/" >  
                     </asp:FilteredTextBoxExtender> 
                    &nbsp;- &nbsp;<asp:TextBox ID="txtDelivertyDateTo" runat="server" Width="95px" MaxLength="10" />
                    <asp:CalendarExtender ID="txtDelivertyDateTo_CalendarExtender" runat="server" 
                        Format="dd/MM/yyyy"  TargetControlID="txtDelivertyDateTo">
                    </asp:CalendarExtender> 
                     <asp:FilteredTextBoxExtender ID="txtDelivertyDateTo_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtDelivertyDateTo" 
                         ValidChars="1234567890/" >
                     </asp:FilteredTextBoxExtender> 
                </td>
            </tr>
            <tr>
                <td class="text_right" colspan="4">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>&nbsp;&nbsp;
                    <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button_style"/>
                </td>
            </tr>
        </table>
        <table class="table_inquiry" width="1500">
            <tr class="table_head">
                <td class="tb_Fix70">Job Order</td>
                <td>PO No.</td>
                <td class="tb_Fix100">Invoice No</td>
                <td class="tb_Fix100">Amount(THB)</td>
                <td class="tb_Fix300">Vendor Name</td>
                <td class="tb_Fix300">Item Name</td> 
                <td class="tb_Fix80">Delivery <br />Date In</td> 
                <td class="tb_Fix90">Qty In</td> 
                <td class="tb_Fix80">Delivery <br />Date Out</td> 
                <td class="tb_Fix90">Qty Out</td> 
                <td class="tb_Fix50">Left</td>                
            </tr>
            <tbody>
                <asp:Repeater ID="rptMaterial" runat="server">
                    <itemtemplate>
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="text_left tb_Fix70"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                            <td><%#DataBinder.Eval(Container.DataItem, "po_no")%></td>
                            <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "invoice_no")%></div></td>
                            <td class="text_right tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "amount")%></div></td>
                            <td class="text_left tb_Fix300"><div><%#DataBinder.Eval(Container.DataItem, "vendor")%></div></td>
                            <td class="text_left tb_Fix300"><div><%#DataBinder.Eval(Container.DataItem, "item_name")%></div></td>
                            <td class="text_left tb_Fix80"><div><%#DataBinder.Eval(Container.DataItem, "delivery_date_in")%></div></td>
                            <td class="text_right tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "qty_in")%></div></td>
                            <td class="text_left tb_Fix80"><div><%#DataBinder.Eval(Container.DataItem, "delivery_date_out")%></div></td>
                            <td class="text_right tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "qty_out")%></div></td>
                            <td class="text_right tb_Fix50"><div><%#DataBinder.Eval(Container.DataItem, "qty_left")%></div></td>
                         </tr>
                    </itemtemplate>
                </asp:Repeater> 
            </tbody>
            <tr class="table_head">
                <td colspan = "11">
                    <div class="float_l">
                        <asp:Label ID="lblDescription" runat="server"></asp:Label>
					</div>
					<div class="float_r">
                        <asp:Label ID="lblPaging" runat="server" Text="&nbsp;"></asp:Label>
                    </div>
                </td>
            </tr>              
        </table>        
    </div>
</asp:Content>
