 <%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTAD01_Detail.aspx.vb" Inherits="Admin_KTAD01_Detail" %>

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
    <form id="form1" runat="server">
    <div class="text_center">
        <table class="table_field_nofix_width" width="500">
            <tr>
                <td class="table_field_td tb_Fix150" align="left">User Name</td>
                <td class="table_field_td tb_Fix350" align="left">
                    <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix150" align="left">Password</td>
                <td class="table_field_td tb_Fix350" align="left">
                    <asp:Label ID="lblPassword" runat="server" Text=""></asp:Label>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix150" align="left">First Name</td>
                <td class="table_field_td tb_Fix350" align="left">                    
                    <asp:Label ID="lblFirstName" runat="server" Text=""></asp:Label>&nbsp;                    
                </td> 
            </tr>
            <tr>
                <td class="table_field_td tb_Fix150" align="left">Last Name</td>
                <td class="table_field_td tb_Fix350" align="left">
                    <asp:Label ID="lblLastName" runat="server" Text=""></asp:Label>&nbsp; 
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix150" align="left">Department</td>
                <td class="table_field_td tb_Fix350" align="left">
                    <asp:Label ID="lblDepartment" runat="server" Text=""></asp:Label>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix150" align="left">Account Next Approve</td>
                <td class="table_field_td tb_Fix350" align="left">
                    <asp:Label ID="lblAccountNextApprove" runat="server" Text=""></asp:Label>&nbsp; 
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix150" align="left">Purchase Next Approve</td>
                <td class="table_field_td tb_Fix350" align="left">
                    <asp:Label ID="lblPurchaseNextApprove" runat="server" Text=""></asp:Label>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix150" align="left">Outsource Next Approve</td>
                <td class="table_field_td tb_Fix350" align="left">
                    <asp:Label ID="lblOutsourceNextApprove" runat="server" Text=""></asp:Label>&nbsp;
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
