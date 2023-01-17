<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTJB01_Detail.aspx.vb" Inherits="JobOrder_KTJB01_Detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="JobOrder_Detail" runat="server">
    <title></title>
    <link href="../App_Themes/common.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/design.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/font.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/fix_table.css" type="text/css" rel="Stylesheet" />   
</head>
<body>
    <form id="frmKTJB01_Detail" runat="server">
        <div class="text_center">
            <br />
            <table class="table_field">
                <tr>
                   <td class="table_field_td" style="width:140px;">Job Order No.</td>
                    <td class="table_field_td" style="width:630px;">
                        <asp:Label ID="lblJobOrder" runat="server">&nbsp;</asp:Label> 
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Issue Date</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblIssueDate" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr>   
                <tr>
                    <td class="table_field_td">Customer</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblCustomer" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr>   
                <tr>
                    <td class="table_field_td">End User</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblEndUser" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Receive PO</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblReceivePo" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr>  
                <tr>
                    <td class="table_field_td">Person in charge</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblPersonCharge" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td" colspan="2"><b>PO Information :</b></td>
                </tr> 
                <tr>
                    <td class="table_field_td text_center" colspan="2" >
                        <table class="table_field" >
                             <tr class="table_detail_head">
                                <td class="text_center tb_Fix150 table_detail_head">PO No.</td>
                                <td class="text_center tb_Fix200 table_detail_head">PO File</td>
                                <td class="text_center tb_Fix150 table_detail_head">PO Type</td>
                                <td class="text_center tb_Fix150 table_detail_head">PO Amount</td>
                            </tr>                            
                            <tbody>
                                <asp:Repeater ID="rptJobOrderPO" runat="server">
                                    <itemtemplate>
                                        <tr  >
                                            <td class="text_left tb_Fix150 table_field_td" ><div><%#DataBinder.Eval(Container.DataItem, "po_no")%></div></td> 
                                            <td class="text_left td_edit tb_Fix200 table_field_td">
                                                <div>
                                                    <asp:LinkButton ID="btnFilePO" CommandName="FilePO" runat="server">
                                                    <div><%#DataBinder.Eval(Container.DataItem, "po_file")%></div>
                                                    </asp:LinkButton>
                                                </div>
                                            </td>
                                            <td class="text_left tb_Fix150 table_field_td"><div><%#DataBinder.Eval(Container.DataItem, "po_type")%></div></td>
                                            <td class="text_right tb_Fix150 table_field_td"><div><asp:Label ID="lblAmountPO" runat ="server"><%#DataBinder.Eval(Container.DataItem, "po_amount")%></asp:Label></div></td>
                                           
                                         </tr>
                                    </itemtemplate>
                                </asp:Repeater> 
                            </tbody>
                            <tr class="table_detail_head">
                                <td colspan = "4" class="table_detail_head">
                                    <div class="float_l">
                                        <asp:Label ID="lblDescriptionPO" runat="server"></asp:Label>
					                </div>
					                <div class="float_r">
                                        <asp:Label ID="lblPagingPO" runat="server" Text="&nbsp;"></asp:Label>
                                    </div>
                                </td>
                            </tr>              
                        </table>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td" colspan="2"><b>Quotation Information :</b></td>
                </tr> 
                <tr>
                    <td class="table_field_td text_center" colspan="2">
                         <table class="table_field" >
                             <tr class="table_detail_head">
                                <td class="text_center tb_Fix150 table_detail_head">Quotation No.</td>
                                <td class="text_center tb_Fix200 table_detail_head ">Quotation File</td>
                                <td class="text_center tb_Fix150 table_detail_head">Quotation Type</td>
                            </tr>                            
                            <tbody>
                                <asp:Repeater ID="rptJobOrderQuo" runat="server">
                                    <itemtemplate>
                                        <tr  >
                                            <td class="text_left tb_Fix150 table_field_td" ><div><%#DataBinder.Eval(Container.DataItem, "quo_no")%></div></td> 
                                            <td class="text_left td_edit tb_Fix200 table_field_td">
                                                <div>
                                                    <asp:LinkButton ID="btnFile" CommandName="FileQuo" runat="server">
                                                    <div><%#DataBinder.Eval(Container.DataItem, "quo_file")%></div>
                                                    </asp:LinkButton>
                                                </div>
                                            </td>
                                            <td class="text_left tb_Fix150 table_field_td"><div><%#DataBinder.Eval(Container.DataItem, "quo_type")%></div></td>
                                            <%--<td class="text_right tb_Fix150 table_field_td"><div><asp:Label ID="lblAmountQuo" runat ="server"><%#DataBinder.Eval(Container.DataItem, "quo_amount")%></asp:Label></div></td>--%>
                                           
                                         </tr>
                                    </itemtemplate>
                                </asp:Repeater> 
                            </tbody>
                            <tr class="table_detail_head">
                                <td colspan = "4"  class="table_detail_head">
                                    <div class="float_l">
                                        <asp:Label ID="lblDescriptionQuo" runat="server"></asp:Label>
					                </div>
					                <div class="float_r">
                                        <asp:Label ID="lblPagingQuo" runat="server" Text="&nbsp;"></asp:Label>
                                    </div>
                                </td>
                            </tr>              
                        </table>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Job Order Type</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblJobOrderType" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">BOI</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblBoi" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Create At</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblCreateAt" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Part Name</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblPartname" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Part No</td>
                    <td class="table_field_td">
                       <asp:Label ID="lblPartNo" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Part Type</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblPartType" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Payment Condition</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblPaymentCondition" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Payment Term</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblPayment" runat="server"></asp:Label>&nbsp; Days
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Currency</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblCurrency" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Schedule Rate (THB)</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblScheduleRate" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td" colspan="2"><b>Hontai Information :</b></td>
                </tr>
                <tr>
                    <td class="table_field_td" colspan="2">
                         <table class="table_field">
                            <tr class="table_detail_head">
                                <td class="table_field_td text_center" rowspan="2" style="width:185px;" >1st</td>
                                <td class="text_center table_detail_head tb_Fix210">Date</td>
                                <td class="text_center table_detail_head tb_Fix210">Condition</td>
                                <td class="text_center table_detail_head tb_Fix210">Amount</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left"><asp:Label ID="lblDate1" runat="server" >&nbsp;</asp:Label></td>
                                <td class="table_field_td text_right"><asp:Label ID="lblCondition1" runat="server" >&nbsp;</asp:Label>%</td>
                                <td class="table_field_td text_right"><asp:Label ID="lblAmount1" runat="server" >&nbsp;</asp:Label></td>
                            </tr>
                            <tr class="table_detail_head">
                                <td class="table_field_td text_center" rowspan="2">2nd</td>
                                <td class="text_center table_detail_head">Date</td>
                                <td class="text_center">Condition</td>
                                <td class="text_center table_detail_head">Amount</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left"><asp:Label ID="lblDate2" runat="server" >&nbsp;</asp:Label></td>
                                <td class="table_field_td text_right"><asp:Label ID="lblCondition2" runat="server" >&nbsp;</asp:Label>%</td>
                                <td class="table_field_td text_right"><asp:Label ID="lblAmount2" runat="server" >&nbsp;</asp:Label></td>
                            </tr>
                            <tr class="table_detail_head">
                                <td class="table_field_td text_center" rowspan="2">3rd</td>
                                <td class="text_center table_detail_head">Date</td>
                                <td class="text_center">Condition</td>
                                <td class="text_center table_detail_head">Amount</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left"><asp:Label ID="lblDate3" runat="server" >&nbsp;</asp:Label></td>
                                <td class="table_field_td text_right"><asp:Label ID="lblCondition3" runat="server" >&nbsp;</asp:Label>%</td>
                                <td class="table_field_td text_right"><asp:Label ID="lblAmount3" runat="server" >&nbsp;</asp:Label></td>
                            </tr>
                        </table>
                     </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Hontai Amount</td>
                    <td class="table_field_td text_right">
                        <asp:Label ID="lblHontaiAmount" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Except Hontai Amount</td>
                    <td class="table_field_td text_right">
                        <asp:Label ID="lblSumOtherAmount" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Total Amount</td>
                    <td class="table_field_td text_right">
                        <asp:Label ID="lblTotalAmount" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Remarks</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblRemarks" runat="server">&nbsp;</asp:Label>
                    </td>
                </tr>
            </table>  
            <br />  
            <div style="position:absolute;left:7px;"  >
            <table class="table_field" style="width:870px;">                 
                <tr>
                    <td ><b>Invoice Information :</b></td>
                </tr> 
                <tr>
                    <td class="text_center table_field_td">  
                     <table class="table_field" >
                        <tr class="table_detail_head">
                            <td class="text_center tb_Fix100 table_detail_head">Status</td>
                            <td class="text_center tb_Fix120 table_detail_head">Invoice No.</td>
                            <td class="text_center tb_Fix100 table_detail_head">Receipt Date</td>
                            <td class="text_center tb_Fix120 table_detail_head">PO Type</td>
                            <td class="text_center tb_Fix120 table_detail_head">Account Title</td>
                            <td class="text_center tb_Fix100 table_detail_head">Issue Date</td>
                            <td class="text_center tb_Fix130 table_detail_head">Amount(THB)</td>
                        </tr>
                         <tbody>
                            <asp:Repeater ID="rptJobOrderInv" runat="server">
                                <itemtemplate>
                                    <tr >
                                        <td class="text_left tb_Fix100 table_field_td" ><div><%#DataBinder.Eval(Container.DataItem, "status")%></div></td> 
                                        <td class="text_left tb_Fix120 table_field_td" ><div><%#DataBinder.Eval(Container.DataItem, "invoice_no")%></div></td> 
                                        <td class="text_left tb_Fix100 table_field_td" ><div><%#DataBinder.Eval(Container.DataItem, "receipt_date")%></div></td> 
                                        <td class="text_left tb_Fix120 table_field_td" ><div><%#DataBinder.Eval(Container.DataItem, "po_type")%></div></td> 
                                        <td class="text_left tb_Fix120 table_field_td" ><div><%#DataBinder.Eval(Container.DataItem, "account_title")%></div></td> 
                                        <td class="text_left tb_Fix100 table_field_td"><div><%#DataBinder.Eval(Container.DataItem, "issue_date")%></div></td>
                                        <td class="text_right tb_Fix130 table_field_td"><div><asp:Label ID="lblAmountInv" runat ="server"> <%#DataBinder.Eval(Container.DataItem, "total_amount")%></asp:Label> </div></td>
                                        <%--<td class="text_right tb_Fix130 table_field_td"><div><asp:Label ID="lblAmountInv" runat ="server">Test</asp:Label></div></td>--%>
                                     </tr>
                                </itemtemplate>
                            </asp:Repeater> 
                        </tbody>
                        <tr class="table_detail_head">
                            <td colspan = "7" class="table_detail_head">
                                <div class="float_l">
                                    <asp:Label ID="lblDescriptionInv" runat="server"></asp:Label>
		                        </div>
		                        <div class="float_r">
                                    <asp:Label ID="lblPagingInv" runat="server" Text="&nbsp;"></asp:Label>
                                </div>
                            </td>
                        </tr>   
                    </table>
                </td>
                </tr>
            </table>
            </div>
        </div>
    </form>
</body>
</html>
