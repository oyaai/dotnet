<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTPU01_Detail.aspx.vb" Inherits="Purchase_KTPU01_Detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/common.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/design.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/font.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/fix_table.css" type="text/css" rel="Stylesheet" />
<script language="javascript"  type="text/javascript">
function showpopup(url,frame)
{
    var width = parseInt(screen.availWidth*0.85);
    var height = parseInt(screen.availHeight*0.75);
    var left = parseInt((screen.availWidth/2) - (width/2));
    var top = parseInt((screen.availHeight/2) - (height/2));
    var windowFeatures = "width=" + width + ",height=" + height + ",left=" + left + ",top=" + top + ",resizable,scrollbars=1";
    newwin = window.open (url, frame, windowFeatures);
    if (window.focus) {newwin.focus()}
    return false;
}
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="text_left">
        <%--<br />--%>
        <table class="table_field_fixs" width="350">
            <tr>
                <td colspan="5"><asp:Button ID="btnPDF" runat="server" Text="PDF" CssClass="button_style"/></td>                
            </tr> 
            <tr>
                <td class="table_view_head" colspan="5">Purchase Information</td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Type of Purchase</td>
                <td class="table_field_td tb_Fix230"> <%--Purchase--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblType" runat="server"></asp:Label>
                    </div>
                </td>
                <td></td>
                <td class="table_field_td tb_Fix120">PO No.</td>
                <td class="table_field_td tb_Fix230"> <%--KTT-PO-1212-002_R001--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblPONo" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Delivery Date Plan</td>
                <td class="table_field_td tb_Fix230"> <%--12/Dec/2012--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblDeliveryDate" runat="server"></asp:Label>
                    </div>
                </td>
                <td></td>
                <td class="table_field_td tb_Fix120">Quotation No.</td>
                <td class="table_field_td tb_Fix230"> <%--QUO1--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblQuotation" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>  
            <tr>
                <td class="table_field_td tb_Fix120">Vendor Name</td>
                <td class="table_field_td tb_Fix230"> <%--Vendor1-update--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblVendor" runat="server"></asp:Label>
                    </div>
                </td>
                <td></td>
                <td class="table_field_td tb_Fix120">Payment Term</td>
                <td class="table_field_td tb_Fix230"> <%--10 days--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblPayTerm" runat="server"></asp:Label>
                    </div>
                </td>
            </tr> 
            <tr>
                <td class="table_field_td tb_Fix120">Vat</td>
                <td class="table_field_td tb_Fix230"> <%--7%--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblVat" runat="server"></asp:Label>
                    </div>
                </td>
                <td></td>
                <td class="table_field_td tb_Fix120">W/T</td>
                <td class="table_field_td tb_Fix230"> <%--0%--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblWT" runat="server"></asp:Label>
                    </div>
                </td>
            </tr> 
            <tr>
                <td class="table_field_td tb_Fix120">Currency</td>
                <td class="table_field_td tb_Fix230"> <%--Baht--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblCurrency" runat="server"></asp:Label>
                    </div>
                </td>
                <td></td>
                <td class="table_field_td tb_Fix120">Remarks</td>
                <td class="table_field_td tb_Fix230"> <%--Remark1--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblHeadRemark" runat="server"></asp:Label>
                    </div>
                </td>
            </tr> 
            <tr>
                <td class="table_field_td tb_Fix120">Attn</td>
                <td class="table_field_td tb_Fix230"> <%--Test Attn--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblAttn" runat="server"></asp:Label>
                    </div>
                </td>
                <td></td>
                <td class="table_field_td tb_Fix120">DeliverTo</td>
                <td class="table_field_td tb_Fix230"> <%--Test Deliver--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblDeliverTo" runat="server"></asp:Label>
                    </div>
                </td>
            </tr> 
            <tr>
                <td class="table_field_td tb_Fix120">Contact</td>
                <td class="table_field_td tb_Fix230"> <%--Test Contact--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblContact" runat="server"></asp:Label>
                    </div>
                </td>
                <td></td>
                <td colspan="2"></td>
            </tr> 
            <tr>
                <td class="table_view_head" colspan="5">Data for PO</td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Sub Total</td>
                <td class="table_field_td tb_Fix230 text_right"> <%--495.00--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblSubTotalAmt" runat="server"></asp:Label>
                    </div>
                </td>
                <td></td>
                <td class="table_field_td tb_Fix120">Discount Total</td>
                <td class="table_field_td tb_Fix230 text_right"> <%--0.00--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblDiscountAmt" runat="server"></asp:Label>
                    </div>
                </td>
            </tr> 
            <tr>
                <td class="table_field_td tb_Fix120">Vat Amount</td>
                <td class="table_field_td tb_Fix230 text_right"> <%--34.65--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblVatAmt" runat="server"></asp:Label>
                    </div>
                </td>
                <td></td>
                <td class="table_field_td tb_Fix120">W/T Amount</td>
                <td class="table_field_td tb_Fix230 text_right"> <%--0.00--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblWTAmt" runat="server"></asp:Label>
                    </div>
                </td>
            </tr> 
            <tr>
                <td class="table_field_td tb_Fix120">Total Amount</td>
                <td class="table_field_td tb_Fix230 text_right"> <%--529.65--%>
                    <div class="ctb_Fix230">
                    <asp:Label ID="lblTotalAmt" runat="server"></asp:Label>
                    </div>
                </td>
                <td></td>
                <td colspan="2"></td>
            </tr> 
        </table> 
         <br /><br />
         <table class="table_field_fix" width="1500">
            <tr class="table_view_head">
                <td class="table_field_td_head text_center">Item Name</td>
                <td class="table_field_td_head text_center">Job Order</td>
                <td class="table_field_td_head text_center">Account Title</td>
                <td class="table_field_td_head text_center">Unit Price</td>
                <td class="table_field_td_head text_center">Qty</td>
                <td class="table_field_td_head text_center">Unit</td>
                <td class="table_field_td_head text_center">Discount</td>
                <td class="table_field_td_head text_center">Discount Type</td>
                <td class="table_field_td_head text_center">Vat</td>
                <td class="table_field_td_head text_center">W/T</td>
                <td class="table_field_td_head text_center">Amount</td>
                <td class="table_field_td_head text_center">Remarks</td>
            </tr>
            <asp:Repeater ID="rptViewPurchase" runat="server">
                <itemtemplate>
                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                        <td class="table_field_td text_left "><%#DataBinder.Eval(Container.DataItem, "item_name")%></td>
                        <td class="table_field_td text_left "><%#DataBinder.Eval(Container.DataItem, "job_order")%></td>
                        <td class="table_field_td text_left "><%#DataBinder.Eval(Container.DataItem, "ie_name")%></td>
                        <td class="table_field_td text_right "><%#DataBinder.Eval(Container.DataItem, "unit_price")%></td>
                        <td class="table_field_td text_right "><%#DataBinder.Eval(Container.DataItem, "quantity")%></td>
                        <td class="table_field_td text_left "><%#DataBinder.Eval(Container.DataItem, "unit_name")%></td>
                        <td class="table_field_td text_right "><%#DataBinder.Eval(Container.DataItem, "discount")%></td>
                        <td class="table_field_td text_left "><%#DataBinder.Eval(Container.DataItem, "discount_type_text")%></td>
                        <td class="table_field_td text_right "><%#DataBinder.Eval(Container.DataItem, "vat_amount")%></td>
                        <td class="table_field_td text_right "><%#DataBinder.Eval(Container.DataItem, "wt_amount")%></td>
                        <td class="table_field_td text_right "><%#DataBinder.Eval(Container.DataItem, "amount")%></td>
                        <td class="table_field_td text_left "><%#DataBinder.Eval(Container.DataItem, "remark")%></td>
                    </tr> 
                </itemtemplate>
            </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>


