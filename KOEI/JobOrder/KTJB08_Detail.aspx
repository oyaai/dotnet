<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTJB08_Detail.aspx.vb" Inherits="JobOrder_KTJB08_Detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/common.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/design.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/font.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="text_center">
            <br />
            <table class="table_field">
                <tr>
                    <td class="table_field_td">Job Order No.</td>
                    <td class="table_field_td">
                        130125 <asp:Label ID="lblJobOrder" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Issue Date</td>
                    <td class="table_field_td">
                        29/Jan/2013 <asp:Label ID="lblIssueDate" runat="server"></asp:Label>
                    </td>
                </tr>   
                <tr>
                    <td class="table_field_td">Customer</td>
                    <td class="table_field_td">
                        C-SNA-00001 <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                    </td>
                </tr>   
                <tr>
                    <td class="table_field_td">End User</td>
                    <td class="table_field_td">
                        C-SNA-00002 <asp:Label ID="lblEndUser" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Receive PO</td>
                    <td class="table_field_td">
                        YES <asp:Label ID="lblReceivePo" runat="server"></asp:Label>
                    </td>
                </tr>  
                <tr>
                    <td class="table_field_td">Person in charge</td>
                    <td class="table_field_td">
                        Test<asp:Label ID="lblPersonCharge" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td" colspan="2">PO Information :</td>
                </tr> 
                <tr>
                    <td class="table_field_td" colspan="2">
                         <table class="table_field">
                            <tr class="table_view_head">
                                <td class="text_center">PO No</td>
                                <td class="text_center">File</td>
                                <td class="text_center">Type</td>
                                <td class="text_center">Amount</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">PO1</td>
                                <td class="table_field_td text_left"><a href="#">File1.pdf</a></td>
                                <td class="table_field_td text_left">Sample</td>
                                <td class="table_field_td text_right">22,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">PO2</td>
                                <td class="table_field_td text_left"><a href="#">File2.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">22,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">PO3</td>
                                <td class="table_field_td text_left"><a href="#">File3.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">22,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">PO4</td>
                                <td class="table_field_td text_left"><a href="#">File4.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">22,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">PO5</td>
                                <td class="table_field_td text_left"><a href="#">File5.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">22,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">PO6</td>
                                <td class="table_field_td text_left"><a href="#">File6.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">22,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">PO7</td>
                                <td class="table_field_td text_left"><a href="#">File7.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">22,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">PO8</td>
                                <td class="table_field_td text_left"><a href="#">File8.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">22,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">PO9</td>
                                <td class="table_field_td text_left"><a href="#">File9.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">22,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">PO10</td>
                                <td class="table_field_td text_left"><a href="#">File10.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">22,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_view_head text_right" colspan="4">1 2 3 4 5</td>
                            </tr>
                        </table>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td" colspan="2">Quotation Information :</td>
                </tr> 
                <tr>
                    <td class="table_field_td" colspan="2">
                         <table class="table_field">
                            <tr class="table_view_head">
                                <td class="text_center">Quotation No</td>
                                <td class="text_center">File</td>
                                <td class="text_center">PO Type</td>
                                <td class="text_center">Amount</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">QUO1</td>
                                <td class="table_field_td text_left"><a href="#">File1.pdf</a></td>
                                <td class="table_field_td text_left">Sample</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">QUO2</td>
                                <td class="table_field_td text_left"><a href="#">File2.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">2,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">QUO3</td>
                                <td class="table_field_td text_left"><a href="#">File3.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">2,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">QUO4</td>
                                <td class="table_field_td text_left"><a href="#">File4.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">20,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">QUO5</td>
                                <td class="table_field_td text_left"><a href="#">File5.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">20,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">QUO6</td>
                                <td class="table_field_td text_left"><a href="#">File6.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">2,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">QUO7</td>
                                <td class="table_field_td text_left"><a href="#">File7.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">QUO8</td>
                                <td class="table_field_td text_left"><a href="#">File8.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">QUO9</td>
                                <td class="table_field_td text_left"><a href="#">File9.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">2,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">QUO10</td>
                                <td class="table_field_td text_left"><a href="#">File10.pdf</a></td>
                                <td class="table_field_td text_left">Hontai</td>
                                <td class="table_field_td text_right">5,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_view_head text_right" colspan="4">1 2 3 4 5</td>
                            </tr>
                        </table>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Job Order Type</td>
                    <td class="table_field_td">
                        Type10<asp:Label ID="lblJobOrderType" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">BOI</td>
                    <td class="table_field_td">
                        Yes<asp:Label ID="lblBoi" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Create At</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblCreateAt" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Part Name</td>
                    <td class="table_field_td">
                        test 20130129_1<asp:Label ID="lblPartname" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Part No</td>
                    <td class="table_field_td">
                       No2 <asp:Label ID="lblPartNo" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Part Type</td>
                    <td class="table_field_td">
                        Type1<asp:Label ID="lblPartType" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Payment Term</td>
                    <td class="table_field_td">
                        0  Days<asp:Label ID="lblPayment" runat="server"></asp:Label>&nbsp; Days
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Currency</td>
                    <td class="table_field_td">
                        Baht<asp:Label ID="lblCurrency" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td" colspan="2">Hontai Information :</td>
                </tr>
                <tr>
                    <td class="table_field_td" colspan="2">
                         <table class="table_field">
                            <tr class="table_view_head">
                                <td class="table_field_td text_center" rowspan="2">1st</td>
                                <td class="text_center">Receive PO Date</td>
                                <td class="text_center">Condition</td>
                                <td class="text_center">Amount</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">29/Jan/2013<asp:Label ID="lblReceivePoDate" runat="server" ></asp:Label></td>
                                <td class="table_field_td text_left">9%<asp:Label ID="lblCondition1" runat="server" ></asp:Label></td>
                                <td class="table_field_td text_right">3.00<asp:Label ID="lblAmount1" runat="server" ></asp:Label></td>
                            </tr>
                            <tr class="table_view_head">
                                <td class="table_field_td text_center" rowspan="2">2nd</td>
                                <td class="text_center">Try1 Date</td>
                                <td class="text_center">Condition</td>
                                <td class="text_center">Amount</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">29/Jan/2013<asp:Label ID="lblTryDate" runat="server" ></asp:Label></td>
                                <td class="table_field_td text_left">85%<asp:Label ID="lblCondition2" runat="server" ></asp:Label></td>
                                <td class="table_field_td text_right">28.33<asp:Label ID="lblAmount2" runat="server" ></asp:Label></td>
                            </tr>
                            <tr class="table_view_head">
                                <td class="table_field_td text_center" rowspan="2">3rd</td>
                                <td class="text_center">Final Approve Date</td>
                                <td class="text_center">Condition</td>
                                <td class="text_center">Amount</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">29/Jan/2013<asp:Label ID="lblFinalDate" runat="server" ></asp:Label></td>
                                <td class="table_field_td text_left">6%<asp:Label ID="lblCondition3" runat="server" ></asp:Label></td>
                                <td class="table_field_td text_right">2.00<asp:Label ID="lblAmount3" runat="server" ></asp:Label></td>
                            </tr>
                        </table>
                     </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Hontai Amount</td>
                    <td class="table_field_td">
                        34.00 <asp:Label ID="lblHontaiAmount" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Total Amount</td>
                    <td class="table_field_td">
                        601.00 <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Remarks</td>
                    <td class="table_field_td">
                        Remark1<asp:Label ID="lblRemarks" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td" colspan="2">
                        <br />
                         <table class="table_field">
                            <tr class="table_view_head">
                                <td class="text_center">Invoice No</td>
                                <td class="text_center">Invoice Type</td>
                                <td class="text_center">Issue Date</td>
                                <td class="text_center">"Receipt Date</td>
                                <td class="text_center">Customer</td>
                                <td class="text_center">Amount</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">inv1</td>
                                <td class="table_field_td text_left">IN</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">C-SNA-00001</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">inv2</td>
                                <td class="table_field_td text_left">IN</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">C-SNA-00001</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">inv3</td>
                                <td class="table_field_td text_left">IN</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">C-SNA-00001</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">inv4</td>
                                <td class="table_field_td text_left">IN</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">C-SNA-00001</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">inv5</td>
                                <td class="table_field_td text_left">IN</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">C-SNA-00001</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">inv6</td>
                                <td class="table_field_td text_left">IN</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">C-SNA-00001</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">inv7</td>
                                <td class="table_field_td text_left">IN</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">C-SNA-00001</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">inv8</td>
                                <td class="table_field_td text_left">IN</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">C-SNA-00001</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">inv9</td>
                                <td class="table_field_td text_left">IN</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">C-SNA-00001</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_field_td text_left">inv10</td>
                                <td class="table_field_td text_left">IN</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">20/May/2013</td>
                                <td class="table_field_td text_left">C-SNA-00001</td>
                                <td class="table_field_td text_right">12,000.00</td>
                            </tr>
                            <tr>
                                <td class="table_view_head text_right" colspan="6">1 2 3 4 5</td>
                            </tr>
                        </table>
                    </td>
                </tr> 
            </table> 

        </div>
    </form>
</body>
</html>
