<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTMS01_Detail.aspx.vb" Inherits="Master_KTMS01_Detail" %>

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
    <div>
        <table class="table_field_fix" style="width:780px;">
            <tr>
                <td class="table_field_td tb_Fix120">ID</td>
                <td class="table_field_td tb_Fix660">
                    <asp:Label ID="lblID" runat="server" Text=""></asp:Label> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Type1</td>
                <td class="table_field_td">
                    <asp:Label ID="lblType1" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Type2</td>
                <td class="table_field_td">                    
                    <asp:Label ID="lblType2_TextAll" runat="server" Text=""></asp:Label>                    
                </td> 
            </tr>
            <tr>
                <td class="table_field_td">Name</td>
                <td class="table_field_td">
                    <asp:Label ID="lblName" runat="server" Text=""></asp:Label> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Short Name</td>
                <td class="table_field_td">
                    <asp:Label ID="lblShortName" runat="server" Text=""></asp:Label> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Person in charge1</td>
                <td class="table_field_td">
                    <asp:Label ID="lblPerson1" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Person in charge2</td>
                <td class="table_field_td">
                    <asp:Label ID="lblPerson2" runat="server" Text=""></asp:Label> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Country</td>
                <td class="table_field_td">
                    <asp:Label ID="lblCountry" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Zip Code</td>
                <td class="table_field_td">
                    <asp:Label ID="lblZipCode" runat="server" Text=""></asp:Label> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Address</td>
                <td class="table_field_td">
                    <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Tel No.</td>
                <td class="table_field_td">
                    <asp:Label ID="lblTelNo" runat="server" Text=""></asp:Label> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Fax No.</td>
                <td class="table_field_td">
                    <asp:Label ID="lblFaxNo" runat="server" Text=""></asp:Label> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Remarks</td>
                <td class="table_field_td">
                    <asp:Label ID="lblRemarks" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Attached File</td>
                <td class="table_field_td">
                    <asp:LinkButton ID="btnFileAttached" runat="server"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <table class="table_field_fix" style="width:1040px;">
            <tr><td colspan="6"><b>Branch Information :</b></td></tr>
            <tr class="text_center" style="font-weight: bold;">
                <td class="table_field_td tb_Fix120">Name</td>
                <td class="table_field_td tb_Fix500">Address</td>
                <td class="table_field_td tb_Fix100">Telephone</td>
                <td class="table_field_td tb_Fix100">Fax</td>
                <td class="table_field_td tb_Fix120">E-Mail</td>
                <td class="table_field_td tb_Fix100">Contact</td>
            </tr>
            <asp:Repeater ID="rptVendor" runat="server">
                <itemtemplate>
                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                        <td class="table_field_td text_left"><div><%#DataBinder.Eval(Container.DataItem, "name")%></div></td>
                        <td class="table_field_td text_left"><div><%#DataBinder.Eval(Container.DataItem, "fullAddress")%></div></td>
                        <td class="table_field_td text_left"><div><%#DataBinder.Eval(Container.DataItem, "telNo")%></div></td>
                        <td class="table_field_td text_left"><div><%#DataBinder.Eval(Container.DataItem, "faxNo")%></div></td>
                        <td class="table_field_td text_left"><div><%#DataBinder.Eval(Container.DataItem, "email")%></div></td>
                        <td class="table_field_td text_left"><div><%#DataBinder.Eval(Container.DataItem, "contact")%></div></td>
                    </tr> 
                </itemtemplate>
            </asp:Repeater> 
        </table>
    </div>
    </form>
</body>
</html>
