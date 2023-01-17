<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTMS02_Branch.aspx.vb" Inherits="Master_KTMS02_Branch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/common.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/design.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/fix_table.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/font.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-left: 20px; margin-top: 20px;">
        <span class="font_header">BRANCH</span><br /><br />
        <table class="table_field" style="width:800px;">
            <tr>
                <td class="table_field_td tb_Fix100">Name <span class="font_error">*</span></td>
                <td class="table_field_td tb_Fix700">
                    <asp:TextBox ID="txtName" runat="server" Width="600px" MaxLength="150" onkeyup="javascript:this.value=this.value.toUpperCase();" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="txtName" CssClass="font_error" ErrorMessage="*Required"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix100">Address</td>
                <td class="table_field_td tb_Fix700">
                    <asp:TextBox ID="txtAddress" CssClass="text_multiLine" runat="server" Width="600px" Height="60px" 
                        MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix100">Zip Code</td>
                <td class="table_field_td tb_Fix700">
                    <asp:TextBox ID="txtZipCode" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix100">Country</td>
                <td class="table_field_td tb_Fix700">
                    <asp:DropDownList ID="ddlCountry" runat="server" Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix100">Tel No.</td>
                <td class="table_field_td tb_Fix700">
                    <asp:TextBox ID="txtTelNo" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix100">Fax No.</td>
                <td class="table_field_td tb_Fix700">
                    <asp:TextBox ID="txtFaxNo" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix100">E-Mail</td>
                <td class="table_field_td tb_Fix700">
                    <asp:TextBox ID="txtEMail" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                                &nbsp;<asp:RegularExpressionValidator ID="reqEMail" runat="server" 
                        CssClass="font_error" ErrorMessage="*Please check e-Mail format." 
                        SetFocusOnError="True" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        ControlToValidate="txtEMail"></asp:RegularExpressionValidator>
                                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix100">Contact</td>
                <td class="table_field_td tb_Fix700">
                    <asp:TextBox ID="txtContact" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix100">Remark</td>
                <td class="table_field_td tb_Fix700">
                    <asp:TextBox ID="txtRemark" CssClass="text_multiLine" runat="server" Width="600px" Height="60px" 
                        MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                                </td>
            </tr>
            <tr>
                <td class="text_right" colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" />
                    &nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" CausesValidation="False" />
                    &nbsp;
                    <asp:Button ID="btnClose" runat="server" Text="Close" Width="100px" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <br />
        <table class="table_inquiry" style="width:1350px;">
            <tr class="table_head">
                <td class="tb_Fix50">Edit</td>
                <td class="tb_Fix50">Delete</td>
                <td class="tb_Fix200">Name</td>
                <td class="tb_Fix400">Address</td>
                <td class="tb_Fix100">Telephone</td>
                <td class="tb_Fix100">Fax</td>
                <td class="tb_Fix200">E-Mail</td>
                <td class="tb_Fix100">Contact</td>
                <td class="tb_Fix150">Remark</td>
            </tr>
            <asp:Repeater ID="rptBranch" runat="server">
                <itemtemplate>
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="tb_Fix50"><div><asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_edit1 icon_center15" runat="server" CausesValidation="false" ></asp:LinkButton></div></td>
                            <td class="tb_Fix50"><div><asp:LinkButton ID="btnDelete" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server" CausesValidation="false" ></asp:LinkButton></div></td>
                            <td class="tb_Fix200"><div><%#DataBinder.Eval(Container.DataItem, "name")%></div></td>
                            <td class="tb_Fix400"><div><%#DataBinder.Eval(Container.DataItem, "fullAddress")%></div></td>
                            <td class="tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "telNo")%></div></td>
                            <td class="tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "faxNo")%></div></td>
                            <td class="tb_Fix200"><div><%#DataBinder.Eval(Container.DataItem, "email")%></div></td>
                            <td class="tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "contact")%></div></td>
                            <td class="tb_Fix150"><div><%#DataBinder.Eval(Container.DataItem, "remarks")%></div> </td>
                        </tr>
                </itemtemplate>
            </asp:Repeater>        
                            
            <tr class="table_head">
                <td colspan = "9">
                    <div class="float_l">
                        <asp:Label ID="lblDescription" runat="server" Text="&nbsp;"></asp:Label>
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
