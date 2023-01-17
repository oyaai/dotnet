<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTAP02_Detail.aspx.vb" Inherits="Approve_KTAP02_Detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/font.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/common.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/design.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/fix_table.css" type="text/css" rel="Stylesheet" />
</head>

<body>
    <form id="form1" runat="server">
    <div class="text_left">
        <%--<br />--%>
        <table class="table_field" style="width:800px;">
            <tr>
                <td class="table_field_td tb_Fix150">Account Type</td>
                <td class="table_field_td tb_Fix650"><asp:Label ID="lblAccountType" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="table_field_td">Vendor Name</td>
                <td class="table_field_td"><asp:Label ID="lblVendorName" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="table_field_td">Bank</td>
                <td class="table_field_td"><asp:Label ID="lblBank" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="table_field_td">Account No/Name</td>
                <td class="table_field_td"><asp:Label ID="lblAccountName" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="table_field_td">Cheque No.</td>
                <td class="table_field_td"><asp:Label ID="lblAccountNo" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="table_field_td">Receive/Payment Date</td>
                <td class="table_field_td"><asp:Label ID="lblPaymentDate" runat="server" Text=""></asp:Label></td>
            </tr>
        </table>
        <br />
        <span style="font-weight:bold;">Detail</span>
        <br />
        <table class="table_field" style="width:900px;">
            <tr class="text_center" style="font-weight:bold;">
                <td class="table_field_td tb_Fix100">Job Order</td>
                <td class="table_field_td tb_Fix250">Account Title</td>
                <td class="table_field_td tb_Fix150">Sub Total (Amount)</td>
                <td class="table_field_td tb_Fix100">Vat</td>
                <td class="table_field_td tb_Fix100">W/T</td>
                <td class="table_field_td tb_Fix200">Remarks</td>
            </tr>
            <asp:Repeater ID="rptInquery" runat="server">
                <itemtemplate>
                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                        <td class="table_field_td tb_Fix100 text_center"><%#DataBinder.Eval(Container.DataItem, "job_order")%></td>     
                        <td class="table_field_td tb_Fix250 text_left"><%#DataBinder.Eval(Container.DataItem, "account_title")%></td> 
                        <td class="table_field_td tb_Fix150 text_right"><%#CDec(DataBinder.Eval(Container.DataItem, "sub_total")).ToString("#,##0.00")%></td>    
                        <td class="table_field_td tb_Fix100 text_right"><%#DataBinder.Eval(Container.DataItem, "vat_amount")%></td>   
                        <td class="table_field_td tb_Fix100 text_right"><%#DataBinder.Eval(Container.DataItem, "wt_amount")%></td>    
                        <td class="table_field_td tb_Fix200 text_left"><%#DataBinder.Eval(Container.DataItem, "remark")%></td>        
                    </tr>
                </itemtemplate>
            </asp:Repeater> 
            
            <tr style="color: #000000; font-weight: bold; text-align: center; vertical-align: middle; border: 1px solid #76a2c7; border-collapse: collapse;">
                <td class="text_right" colspan = "2">
                    Sum Sub Total (Amount) :
                </td>
                <td class="text_right">
                    <asp:Label ID="lblSumSubTotal" runat="server"></asp:Label>
                </td>
            </tr>          
        </table>
        <br />
    </div>
    </form>
</body>
</html>
