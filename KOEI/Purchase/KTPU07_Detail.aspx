<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTPU07_Detail.aspx.vb" Inherits="KTPU07_Detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/common.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/design.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/font.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <table class="table_rating" width = "500px">
                <tr>
                    <td class="table_field_td">Vendor Name</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblVendorName" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Vendor Type</td>
                        <td class="table_field_td">
                    <asp:Label ID="lblVendorType" runat="server"></asp:Label>
                    </td>
                </tr>  
                <tr>
                    <td class="table_field_td" style="width:200px">Payment Type</td>
                    <td class="table_field_td" style="width:300px">
                        <asp:Label ID="lblChequeNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Pay Date</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblChequeDate" runat="server"></asp:Label>
                    </td>
                </tr>   
            </table> 
             <br /><br />
             <table class="table_inquiry" width="950px">
                <tr class="table_head">
                    <td>Voucher No.</td>
                    <td>Item</td>
                    <td>Job Order</td>
                    <td>Amount</td>
                    <td>Vat</td>
                    <td>Vat(Baht)</td>
                    <td>W/T</td>
                    <td>W/T(Baht)</td>
                    <td>Payment Date</td>
                    <td>Payment From</td>
                </tr>
                <tbody>
                <asp:Repeater ID="rptInquery_Detail" runat="server">
                    <itemtemplate>                        
                            <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "voucher_no")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "ie_name")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "sub_total")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "vat_name")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "vat_amount")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "wt_name")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "wt_amount")%></div></td>
                                <td class="text_center"><div><%#DataBinder.Eval(Container.DataItem, "payment_date")%></div></td>
                                <td class="text_center"><div><%#DataBinder.Eval(Container.DataItem, "payment_from")%></div></td>
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
<!--  
comment
-->