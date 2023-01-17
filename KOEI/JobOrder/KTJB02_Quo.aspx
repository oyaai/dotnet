<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTJB02_Quo.aspx.vb" Inherits="JobOrder_KTJB02_Quo" %> 
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/common.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/design.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/font.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/fix_table.css" type="text/css" rel="Stylesheet" />
    <script type="text/javascript" src="../Scripts/jsCommon.js"></script>
    <script language="javascript"  type="text/javascript" >
        //Function input number only        
        function isNumberKey(evt) {
         var charCode = (evt.which) ? evt.which : event.keyCode
         if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
             return false;
         else {
             var len = document.getElementById("txtQuoAmount").value.length;
             var index = document.getElementById("txtQuoAmount").value.indexOf('.'); 
             
             if (index > 0 && charCode == 46) {    
                 return false;                           
             }
         }
         return true;
      }
      
      //function send value to parent screen
        function addItemsToParent()
        { 
            // var valSumQuoAmount = document.getElementById("hidSumQuoAmount"); 
           // window.opener.addToParentQuoAmount_Conver(valSumQuoAmount.value);
            window.close();             
        }
               
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"/>
        <div class="text_center">
            <br />
            <div class="font_header">UPLOAD QUOTATION</div> 
            <br />
            <table class="table_field">
                <tr>
                    <td class="table_field_td tb_Fix250">&nbsp;Quotation Type<span class="font_require">*</span></td>
                    <td class="table_field_td tb_Fix600">
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rbtQuoType" runat="server" RepeatColumns="5" 
                                        RepeatDirection="Horizontal" Width="403px">
                                        <asp:ListItem Value="0">Hontai</asp:ListItem>
                                        <asp:ListItem Value="1">Sample</asp:ListItem>
                                        <asp:ListItem Value="2">Material</asp:ListItem>
                                        <asp:ListItem Value="3">Delivery</asp:ListItem>
                                        <asp:ListItem Value="4">Other</asp:ListItem>
                                    </asp:RadioButtonList>   
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqValidatorQtoType" runat="server" ValidationGroup="grpUpload"
                                        ControlToValidate="rbtQuoType" ErrorMessage="*Require." class="font_error" SetFocusOnError="True" >
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>  
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">&nbsp;Quotation No.<span class="font_require">*</span></td>
                    <td class="table_field_td">
                        <asp:TextBox ID="txtQuoNo" runat="server" Width="242px" MaxLength="50"></asp:TextBox> 
                        <asp:RequiredFieldValidator ID="reqValidatorQuoNo" runat="server" ValidationGroup="grpUpload"
                            ControlToValidate="txtQuoNo" ErrorMessage="*Require." SetFocusOnError="True" class="font_error">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr> 
                <%-- <tr>
                    <td class="table_field_td">&nbsp;Quotation Amount</td>
<!-- 
                    <span class="font_require">*</span>
-->
                    <td class="table_field_td">
                        <asp:TextBox ID="txtQuoAmount" runat="server" Width="131px" CssClass="text_right" onBlur="this.value=setCurrencyPoint(this.value);" onkeypress="return isNumberKey(event)"
                             MaxLength="18"></asp:TextBox>  
 
                          <asp:RequiredFieldValidator ID="reqValidatorQuoAmount" runat="server" ValidationGroup="QuoAmount"
                            ControlToValidate="txtQuoAmount" ErrorMessage="*Require." SetFocusOnError="True" class="font_error">
                        </asp:RequiredFieldValidator>

                         <asp:RangeValidator ID="rngQuoAmount" runat="server" SetFocusOnError="True"
                             ControlToValidate="txtQuoAmount" Display="Static" 
                             ErrorMessage="Please insert number" MaximumValue="99999999999999.99" 
                             MinimumValue="0.01" Type="Currency" ValidationGroup="grpUpload"></asp:RangeValidator>
                    </td>
                </tr>  --%>
                <tr>
                    <td class="table_field_td">&nbsp;Quotation Date (dd/mm/yyyy)<span class="font_require">*</span></td>
                    <td class="table_field_td">
                        <asp:TextBox ID="txtQuoDate" runat="server" Width="131px" MaxLength="10"></asp:TextBox>
                        <asp:CalendarExtender ID="txtQuoDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                TargetControlID="txtQuoDate">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="reqValidatorQuoDate" runat="server" ValidationGroup="grpUpload"
                            ControlToValidate="txtQuoDate" ErrorMessage="*Require."  class="font_error">
                        </asp:RequiredFieldValidator>                        
                         <asp:FilteredTextBoxExtender ID="txtQuoDate_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" TargetControlID="txtQuoDate" 
                            ValidChars="1234567890/" >
                        </asp:FilteredTextBoxExtender>                         
                    </td>
                </tr>   
                <tr>
                    <td class="table_field_td">&nbsp;Quotation File<span class="font_require">*</span></td>
                    <td class="table_field_td">
                        <asp:FileUpload ID="FileAttach" runat="server"  Width="410px" />                    
                          <asp:RequiredFieldValidator ID="reqValidatorFileAttach" runat="server" ValidationGroup="grpUpload"
                            ControlToValidate="FileAttach" ErrorMessage="*Require." SetFocusOnError="True" class="font_error">
                        </asp:RequiredFieldValidator>
                        <asp:Label ID="lblFileAttach" runat="server" Width="410px"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td colspan="2" class="text_right">
                        <asp:HiddenField ID="hidIpAddress" runat="server" Value="" />
                         <asp:HiddenField ID="hidSumQuoAmount" runat="server" />
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="button_style" ValidationGroup="grpUpload" />&nbsp;&nbsp;
                        <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="button_style" OnClientClick="addItemsToParent();" />
                         
                    </td>
                </tr>         
            </table> 
            <table class="table_inquiry" width="900">
                <tr class="table_head">
                    <td class="tb_Fix50">Edit</td>
                    <td class="tb_Fix50">Delete</td>
                    <td class="tb_Fix50">No.</td>
                    <td class="tb_Fix100">Quotation Type</td>
                    <td class="tb_Fix200">Quotation No</td>
                    <%--<td class="tb_Fix100">Amount</td>--%>
                    <td class="tb_Fix100">Quotation Date</td>
                    <td class="tb_Fix200">Quotation File</td>
                </tr>
                <tbody>
                    <asp:Repeater ID="rptJobOrderQuo" runat="server">
                        <itemtemplate>
                            <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="td_delete tb_Fix50"><asp:LinkButton ID="btnEdit" CommandName="EditQuo" CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></td>
                                <td class="td_delete tb_Fix50"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                                <td class="text_center tb_Fix50"><div><%#DataBinder.Eval(Container.DataItem, "no")%></div></td>
                                <td class="text_center tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "quo_type")%></div></td>
                                <td class="text_center tb_Fix150"><div><%#DataBinder.Eval(Container.DataItem, "quo_no")%></div></td>
                                <%--<td class="text_right tb_Fix100"><div><asp:Label ID="lblAmount" runat ="server"><%#DataBinder.Eval(Container.DataItem, "quo_amount")%></asp:Label></div></td>--%>
                                <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "quo_date")%></div></td> 
                                <td class="text_left tb_Fix150"><div><%#DataBinder.Eval(Container.DataItem, "quo_file")%></div></td>
                             </tr>
                        </itemtemplate>
                    </asp:Repeater> 
                </tbody>
                <tr class="table_head">
                    <td colspan = "8">
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
