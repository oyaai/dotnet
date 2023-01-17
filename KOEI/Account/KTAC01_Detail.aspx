<%@  Language="VB" AutoEventWireup="false" CodeFile="KTAC01_Detail.aspx.vb"
    Inherits="KTAC01_Detail" %>

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
    <div class="text_left">
        <%--<br />--%>
        <table class="table_field" width="550">
            <tr>
                <td class="table_field_td text_left" width="150">
                    Account Type
                </td>
                <td class="table_field_td text_left" width="400">
                    <span id="spanAccountType">
                        <asp:Label ID="lblAccountType" runat="server" Text="&nbsp;"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Vendor Name
                </td>
                <td class="table_field_td text_left">
                    <span id="spanVendorName">
                        <asp:Label ID="lblVendorName" runat="server" Text="&nbsp;"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Bank
                </td>
                <td class="table_field_td text_left">
                    <span id="spanBank">
                        <asp:Label ID="lblBank" runat="server" Text="&nbsp;"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Account Name
                </td>
                <td class="table_field_td text_left">
                    <span id="spanAccountName">
                        <asp:Label ID="lblAccountName" runat="server" Text="&nbsp;"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Account No.
                </td>
                <td class="table_field_td text_left">
                    <span id="spanAccountNo">
                        <asp:Label ID="lblAccountNo" runat="server" Text="&nbsp;"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Payment Date
                </td>
                <td class="table_field_td text_left">
                    <span id="spanPaymentDate">
                        <asp:Label ID="lblPaymentDate" runat="server" Text="&nbsp;"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Job Order
                </td>
                <td class="table_field_td text_left">
                    <span id="spanJobNo">
                        <asp:Label ID="lblJobNo" runat="server" Text="&nbsp;"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Vat
                </td>
                <td class="table_field_td text_left">
                    <span id="spanVat">
                        <asp:Label ID="lblVat" runat="server" Text="&nbsp;"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    W/T
                </td>
                <td class="table_field_td text_left">
                    <span id="spanWT">
                        <asp:Label ID="lblWT" runat="server" Text="&nbsp;"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Account Title
                </td>
                <td class="table_field_td text_left">
                    <span id="spanIE">
                        <asp:Label ID="lblIE" runat="server" Text="&nbsp;"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Sub Total (Amount)
                </td>
                <td class="table_field_td text_left">
                    <span id="spanSubTotal">
                        <asp:Label ID="lblSubTotal" runat="server" Text="&nbsp;"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left">
                    Remarks
                </td>
                <td class="table_field_td text_left">
                    <span id="spanRemarks">
                        <asp:Label ID="lblRemarks" runat="server" Text="&nbsp;"></asp:Label>
                    </span>
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
</body>
</html>