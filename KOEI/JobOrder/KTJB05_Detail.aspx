<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTJB05_Detail.aspx.vb" Inherits="JobOrder_KTJB05_Detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/common.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/design.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/font.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/fix_table.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="frmKTJB05_Detail" runat="server">
        <div class="text_left">
            <br />
            <table class="table_field" style="width:860px;">
                <tr>
                    <td class="table_field_td tb_Fix100">Sale Invoice No.</td>
                    <td class="table_field_td tb_Fix650">
                        <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Issue Date</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblIssueDate" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Receipt Date</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblReceiptDate" runat="server"></asp:Label>
                    </td>
                </tr>  
                <tr>
                    <td class="table_field_td">Account Title</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblAccountTitle" runat="server"></asp:Label>
                    </td>
                </tr>   
                <tr>
                    <td class="table_field_td">Customer</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Account Type</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblAccountType" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Invoice Type</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblInvoiceType" runat="server"></asp:Label>
                    </td>
                </tr> <%--
                <tr>
                    <td class="table_field_td">Bank Fee</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblBankFee" runat="server"></asp:Label>
                    </td>
                </tr> --%>
                <tr>
                    <td class="table_field_td">Sale Invoice Amount(THB)</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblInvoiceAmount" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr> 
            </table> 
            <br /><br />
            <table class="table_field" >
                <tr class="table_detail_head">
                    <td class="text_center tb_Fix100 table_detail_head">Job Order</td>
                    <td class="text_center tb_Fix100 table_detail_head">PO Type</td>
                    <td class="text_center tb_Fix100 table_detail_head">Hontai Type</td>
                    <td class="text_center tb_Fix100 table_detail_head">PO No</td>
                    <td class="text_center tb_Fix100 table_detail_head">Amount(THB)</td>
                    <td class="text_center tb_Fix100 table_detail_head">Vat</td>
                    <td class="text_center tb_Fix100 table_detail_head">W/T</td>
                    <td class="text_center tb_Fix100 table_detail_head">Bank Fee</td>
                    <td class="text_center tb_Fix100 table_detail_head">Remarks</td>
                </tr>
                 <tbody>
                    <asp:Repeater ID="rptSaleInvDetail" runat="server">
                        <itemtemplate>
                            <tr >
                                <td class="text_center tb_Fix100 table_field_td" ><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td> 
                                <td class="text_left tb_Fix100 table_field_td" ><div><%#DataBinder.Eval(Container.DataItem, "po_type")%></div></td> 
                                <td class="text_left tb_Fix100 table_field_td" ><div><%#DataBinder.Eval(Container.DataItem, "hontai")%></div></td> 
                                <td class="text_left tb_Fix100 table_field_td" ><div><%#DataBinder.Eval(Container.DataItem, "po_no")%></div></td> 
                                <td class="text_right tb_Fix100 table_field_td"><div><asp:Label ID="lblAmount" runat ="server"> <%#DataBinder.Eval(Container.DataItem, "amount")%></asp:Label> </div></td>
                                <td class="text_right tb_Fix100 table_field_td" ><div><%#DataBinder.Eval(Container.DataItem, "vat")%></div></td> 
                                <td class="text_right tb_Fix100 table_field_td"><div><%#DataBinder.Eval(Container.DataItem, "wt")%></div></td>
                                <td class="text_right tb_Fix100 table_field_td"><div><%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "bank_fee")).ToString("#,##0.00")%></div></td>
                                <td class="text_left tb_Fix100 table_field_td"><div><%#DataBinder.Eval(Container.DataItem, "remark")%></div></td>
                             </tr>
                        </itemtemplate>
                    </asp:Repeater> 
                </tbody>
                <tr class="table_detail_head">
                    <td colspan = "9" class="table_detail_head">
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
    </form>
</body>

</html>
