<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTAC02_Detail.aspx.vb" Inherits="Account.KTAC02_Detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/common.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/design.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/fix_table.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/font.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="text_left">
    <table class="table_field" style="width:500px;">
            <tr>
                <td class="table_field_td tb_Fix150 text_left">
                    Account Type</td>
                <td class="table_field_td tb_Fix350 text_left">
                    <asp:Label ID="lblAccountType" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>  
                <td class="table_field_td text_left">
                    Vendor Name</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblVendorName" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Vendor Address</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblVendorAddress" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Bank</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblBank" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Account Name</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblAccountName" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Account No.</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblAccountNo" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Cheque No.</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblChequeNo" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblDate" runat="server" Text=""></asp:Label></td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblReceiptDate" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Job Order</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblJobOrder" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Vat</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblVat" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    W/T</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblWT" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Account Title</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblAccountTitle" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Sub Total (Amount)</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblSubTotal" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Remarks</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblRemark" runat="server" ></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
