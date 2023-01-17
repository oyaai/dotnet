<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTJB07.aspx.vb" Inherits="JobOrder_KTJB07"   %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a href="KTJB07.aspx?New=True">Job Order</a> > Income</div>
        <br /><div class="font_header">INCOME</div> 
        <br />         
        <table class="table_field">
            <tr>
                <td class="table_field_td">Sale Invoice No.</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtInvoiceNo" runat="server" Width="210" MaxLength="20" ></asp:TextBox>                    
                </td>
                <td class="table_field_td">&nbsp;Customer</td>
                <td class="table_field_td">
                     <asp:TextBox ID="txtCustomer" runat="server" Width="210" MaxLength="150"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">&nbsp;Issue Date (dd/mm/yyyy)</td>
                <td class="table_field_td">
                     <asp:TextBox ID="txtIssueDateFrom" runat="server"  Width="90" MaxLength="10"></asp:TextBox>                    
                     <asp:CalendarExtender ID="txtIssueDateFrom_CalendarExtender" runat="server" 
                        Format="dd/MM/yyyy"  TargetControlID="txtIssueDateFrom">
                    </asp:CalendarExtender>   
                     <asp:FilteredTextBoxExtender ID="txtIssueDateFrom_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtIssueDateFrom" 
                        ValidChars="1234567890/" >  
                     </asp:FilteredTextBoxExtender>  &nbsp; -  &nbsp;<asp:TextBox ID="txtIssueDateTo" runat="server" Width="90" MaxLength="10"></asp:TextBox>
                     <asp:CalendarExtender ID="txtIssueDateTo_CalendarExtender" runat="server" 
                        Format="dd/MM/yyyy"  TargetControlID="txtIssueDateTo">
                    </asp:CalendarExtender> 
                     <asp:FilteredTextBoxExtender ID="txtIssueDateTo_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtIssueDateTo" 
                         ValidChars="1234567890/" >
                     </asp:FilteredTextBoxExtender>                
                 </td>
                <td class="table_field_td">&nbsp;Receipt Date (dd/mm/yyyy)</td>
                <td class="table_field_td">
                     <asp:TextBox ID="txtReceiptDateFrom" runat="server" Width="90" MaxLength="10"></asp:TextBox>
                     <asp:CalendarExtender ID="txtReceiptDateFrom_CalendarExtender" 
                        runat="server" TargetControlID="txtReceiptDateFrom" Format="dd/MM/yyyy">
                     </asp:CalendarExtender>
                     <asp:FilteredTextBoxExtender ID="txtReceiptDateFrom_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtReceiptDateFrom" 
                         ValidChars="1234567890/" >
                     </asp:FilteredTextBoxExtender>  &nbsp; - &nbsp;<asp:TextBox ID="txtReceiptDateTo" runat="server" Width="90" MaxLength="10"></asp:TextBox>
                     <asp:CalendarExtender ID="txtReceiptDateTo_CalendarExtender" 
                        runat="server" TargetControlID="txtReceiptDateTo" Format="dd/MM/yyyy">
                     </asp:CalendarExtender>
                     <asp:FilteredTextBoxExtender ID="txtReceiptDateTo_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtReceiptDateTo" 
                         ValidChars="1234567890/" >
                     </asp:FilteredTextBoxExtender> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td">&nbsp;Job Order</td>
                <td class="table_field_td" colspan= "3">
                     <asp:TextBox ID="txtJobOrderFrom" runat="server" Width="90" MaxLength="10"></asp:TextBox>
                     &nbsp; - &nbsp;<asp:TextBox ID="txtJobOrderTo" runat="server" Width="90" 
                         MaxLength="6"></asp:TextBox>
                </td> 
            </tr>
            <tr>
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>&nbsp;&nbsp;
                    <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button_style"/>
                </td>
            </tr>         
        </table> 
        <table class="table_inquiry" width="900">
            <tr class="table_head">
                <td class="tb_Fix100">Invoice No.</td>
                <td class="tb_Fix50">Invoice Type</td>
                <td class="tb_Fix100">Issue Date</td>
                <td class="tb_Fix100">Receipt Date</td>
                <td class="tb_Fix250">Customer</td>
                <td class="tb_Fix150">Amount (THB)</td>
                <td class="tb_Fix50">Details</td>                
            </tr>
            <tbody>
                <asp:Repeater ID="rptIncome" runat="server">
                    <itemtemplate>
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "invoice_no")%></div></td>
                            <td class="text_left tb_Fix50"><div><%#DataBinder.Eval(Container.DataItem, "invoice_type")%></div></td>
                            <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "issue_date")%></div></td>
                            <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "receipt_date")%></div></td>
                            <td class="text_left tb_Fix250"><div><%#DataBinder.Eval(Container.DataItem, "customer")%></div></td>
                            <td class="text_right tb_Fix150"><div><asp:Label ID="lblAmount" runat ="server"><%#DataBinder.Eval(Container.DataItem, "total_amount")%></asp:Label></div></td>
                            <td class="td_edit tb_Fix50">
                                <div>
                                    <asp:LinkButton ID="btnDetail" CommandName="Detail" CssClass="icon_detail1 icon_center15" runat="server"></asp:LinkButton>
                                </div>
                            </td>
                         </tr>
                    </itemtemplate>
                </asp:Repeater> 
            </tbody>
            <tr class="table_head">
                <td colspan = "7">
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

