<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTPU03_Detail.aspx.vb" Inherits="KTPU03_Detail" %>

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
        <div class="text_left">
            <br />
            <table class="table_field">
                <tr>
                    <td class="table_field_td" style="width:100px">Invoice No.</td>
                    <td class="table_field_td" style="width:200px">
                        <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Delivery Date</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblDelivery_date" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Payment Date</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblPayment_date" runat="server"></asp:Label>
                    </td>
                </tr>  
                <tr>
                    <td class="table_field_td">Account Type</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblAccountType" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Account No.</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
                    </td>
                </tr>  
                <tr>
                    <td class="table_field_td">Account Name</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblAccountName" runat="server"></asp:Label>
                    </td>
                </tr>  
                <tr>
                    <td class="table_field_td">Delivery Amount</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblDeliveryAmt" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">VAT</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblVatAmt" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Total</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblTotal" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Remarks</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblRemark" runat="server"></asp:Label>
                    </td>
                </tr> 

            </table> 
             <br /><br />
             <table class="table_inquiry" width="890px">
                <tr class="table_head">
                    <td>Item Name</td>
                    <td>Job Order</td>
                    <td>Accounting Title</td>
                    <td>Qty</td>
                    <td>Amount</td>
                    <td>Delivery Qty</td>
                    <td>Delivery Amount</td>
                </tr>
                <tbody>
                <asp:Repeater ID="rptInquery_Detail" runat="server">
                    <itemtemplate>                        
                            <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "ITName")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "IEName")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "quantity")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "amount")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "delivery_qty")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "delivery_amount")%></div></td>
                            </tr>
                    </itemtemplate>
                </asp:Repeater> 
                </tbody>           
                <tr class="table_head">
                    <td colspan = "12">
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

