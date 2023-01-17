<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTPU04_Detail.aspx.vb" Inherits="KTPU04_Detail" %>

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
                    <td class="table_field_td_head" colspan="2">Purchase Information</td>
                </tr>
                <tr>
                    <td class="table_field_td" style="width:150px">Type of Purchase</td>
                    <td class="table_field_td" style="width:400px">
                        <asp:Label ID="lblType_Of_Purchase" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">PO No.</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblPO_No" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Devery Plan</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblDevery_Plan" runat="server"></asp:Label>
                    </td>
                </tr>  
                <tr>
                    <td class="table_field_td">Quotation No.</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblQuotation_No" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Vendor Name</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblVendor_Name" runat="server"></asp:Label>
                    </td>
                </tr>  
                <tr>
                    <td class="table_field_td">Payment Term</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblPayment_Term" runat="server"></asp:Label>
                    </td>
                </tr>  
                <tr>
                    <td class="table_field_td">VAT</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblVAT" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">W/T</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblWT" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Currency</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblCurrency" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Remarks</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">ATTN</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblATTN" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Delivery To</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblDevery_To" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Contact</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblContact" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td_head" colspan="2">Data for PO</td>
                </tr>
                <tr>
                    <td class="table_field_td">Sub Total</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblSub_Total" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Discount Total</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblDiscount_Total" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">VAT Total</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblVat_Total" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">W/T Total</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblWT_Total" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">Total Amount</td>
                    <td class="table_field_td">
                        <asp:Label ID="lblTotal_Amount" runat="server"></asp:Label>
                    </td>
                </tr> 

            </table> 
             <br /><br />
             <table class="table_inquiry" width="1300px">
                <tr class="table_head">
                    <td style="width:180px;">Item Name</td>
                    <td style="width:90px;">Job Order</td>
                    <td style="width:140px;">Accounting Title</td>
                    <td style="width:80px;">UnitPrice</td>
                    <td style="width:80px;">Qty</td>
                    <td style="width:40px;">Unit</td>
                    <td style="width:90px;">Discount</td>
                    <td style="width:80px;">Discount Type</td>
                    <td style="width:80px;">Vat</td>
                    <td style="width:80px;">W/T</td>
                    <td style="width:80px;">Amount</td>
                    <td >Remarks</td>
                </tr>
                <tbody>
                <asp:Repeater ID="rptInquery_Detail" runat="server">
                    <itemtemplate>                        
                            <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "item_name")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "ie_name")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "unit_price")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "quantity")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "unit_name")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "discount")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "discount_type")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "vat_amount")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "wt_amount")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "amount")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "remark")%></div></td>
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
