<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTJB02_PO.aspx.vb" Inherits="JobOrder_KTJB02_PO" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
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
             var len = document.getElementById("txtPOAmount").value.length;
             var index = document.getElementById("txtPOAmount").value.indexOf('.'); 
             
             if (index > 0 && charCode == 46) {    
                       
                return false;                           
             }
         }
         return true;
      }
       //function send value to parent screen
        function addItemsToParent()
        { 
            var valTotal = document.getElementById("hidTotalAmount"); 
            var valSum = document.getElementById("hidSumAmount");
            var valHontai = document.getElementById("hidHontaiAmount");
            var PONO = document.getElementById("");
            //alert(valTotal.value);
           
            //Modify 2013/09/19
            window.opener.addToParentTotal_Conver(valTotal.value);
            //window.opener.addToParentItemData_Conver(valHontai.value,valSum.value,valTotal.value);
            window.close();             
        }
        
        function GetQueryStringParams(sParam)
        {
            var sPageURL = window.location.search.substring(1);
            var sURLVariables = sPageURL.split('&');

            for (var i = 0; i < sURLVariables.length; i++)
            {
                var sParameterName = sURLVariables[i].split('=');
                if (sParameterName[0] == sParam)
                {
                    return sParameterName[1];
                }
            }
        }
           
    </script>
    
</head>
<body>
    
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"/>
        <div class="text_center">
            <br />
            <div class="font_header">UPLOAD PO</div> 
            <br />
            <table class="table_field">
                <tr>
                    <td class="table_field_td tb_Fix250">&nbsp;PO Type<span class="font_require">*</span></td>
                    <td class="table_field_td tb_Fix600">
                        <table cellspacing="0">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rbtPoType" runat="server" RepeatColumns="5" 
                                        RepeatDirection="Horizontal" Width="403px" AutoPostBack="True">
                                        <asp:ListItem Value="0">Hontai</asp:ListItem>
                                        <asp:ListItem Value="1">Sample</asp:ListItem>
                                        <asp:ListItem Value="2">Material</asp:ListItem>
                                        <asp:ListItem Value="3">Delivery</asp:ListItem>
                                        <asp:ListItem Value="4">Other</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqValidatorPoType" runat="server" ValidationGroup="grpUpload"
                                        ControlToValidate="rbtPoType" ErrorMessage="*Require." class="font_error" SetFocusOnError="True" >
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">&nbsp;PO No.<span class="font_require">*</span></td>
                    <td class="table_field_td">
                        <asp:TextBox ID="txtPONo" runat="server" Width="210px" MaxLength="50"></asp:TextBox>    
                        <asp:RequiredFieldValidator ID="reqValidatorPoNo" runat="server" ValidationGroup="grpUpload" SetFocusOnError="True"
                            ControlToValidate="txtPONo" ErrorMessage="*Require."  class="font_error">
                        </asp:RequiredFieldValidator>                                        
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">&nbsp;PO Amount<asp:Label ID="lblReqPOAmount" runat="server" CssClass="font_error" text="*"></asp:Label> </td>
                    <td class="table_field_td">
                        <asp:TextBox ID="txtPOAmount" runat="server" Width="210px" MaxLength="18" CssClass="text_right" onBlur="this.value=setCurrencyPoint(this.value);" onkeypress="return isNumberKey(event)" ></asp:TextBox> 
                        <asp:RequiredFieldValidator ID="reqPOAmount" runat="server" ValidationGroup="grpUpload"
                            ControlToValidate="txtPOAmount" ErrorMessage="*Require." SetFocusOnError="True" class="font_error">
                        </asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="txtPOAmount_FilteredTextBoxExtender" runat="server" Enabled="true"
                            TargetControlID="txtPODate" ValidChars="1234567890/"></asp:FilteredTextBoxExtender>
                    </td>
                </tr>  
                <tr>
                    <td class="table_field_td">&nbsp;PO Date (dd/mm/yyyy)<span class="font_require">*</span></td>
                    <td class="table_field_td">
                        <asp:TextBox ID="txtPODate" runat="server" Width="210px" MaxLength="10"></asp:TextBox>
                        <asp:CalendarExtender ID="txtPODate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                TargetControlID="txtPODate">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="reqValidatorPODate" runat="server" ValidationGroup="grpUpload"
                            ControlToValidate="txtPODate" ErrorMessage="*Require." SetFocusOnError="True" class="font_error">
                        </asp:RequiredFieldValidator>                        
                         <asp:FilteredTextBoxExtender ID="txtPODate_FilteredTextBoxExtender"  
                            runat="server" Enabled="True" TargetControlID="txtPODate" 
                            ValidChars="1234567890/" >
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>   
                <tr>
                    <td class="table_field_td">&nbsp;Saled Date (dd/mm/yyyy)
                        <asp:Label ID="lblChkReq" runat="server" CssClass="font_error" text="*"/>
                    </td>
                    <td class="table_field_td">
                         <asp:TextBox ID="txtReceiptDate" runat="server" Width="210px" MaxLength="10"></asp:TextBox>
                        <asp:CalendarExtender ID="txtReceiptDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                TargetControlID="txtReceiptDate">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="reqValidatorReceiptDate" runat="server" ValidationGroup="grpUpload"
                            ControlToValidate="txtReceiptDate" ErrorMessage="*Require." SetFocusOnError="True" class="font_error">
                        </asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="txtReceiptDate_FilteredTextBoxExtender1" 
                            runat="server" Enabled="True" TargetControlID="txtReceiptDate" 
                            ValidChars="1234567890/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr> 
                <tr>
                    <td class="table_field_td">&nbsp;PO File<span class="font_require">*</span></td>
                    <td class="table_field_td">
                       <asp:FileUpload ID="FileAttach" runat="server" Width="410px" />
                       <asp:RequiredFieldValidator ID="reqValidatorFileAttach" runat="server" ValidationGroup="grpUpload"
                            ControlToValidate="FileAttach" ErrorMessage="*Require." SetFocusOnError="True" class="font_error">
                        </asp:RequiredFieldValidator>  
                        <asp:Label ID="lblFileAttach" runat="server" Width="410px"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td colspan="2" class="text_right">
                        <asp:HiddenField ID="hidIpAddress" runat="server" Value="" />
                        <asp:HiddenField ID="hidTotalAmount" runat="server" />
                        <asp:HiddenField ID="hidSumAmount" runat="server" />
                        <asp:HiddenField ID="hidHontaiAmount" runat="server" />
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="button_style" ValidationGroup="grpUpload" />&nbsp;&nbsp;
                         <asp:Button ID="btnUpload_2" runat="server" Text="Upload" CssClass="button_style" ValidationGroup="grpUpload" />&nbsp;&nbsp;
                        <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="button_style" OnClientClick="addItemsToParent();" /> 
                    </td>
                </tr>         
            </table> 
             <table class="table_inquiry" width="900">
                <tr class="table_head">
                    <td class="tb_Fix50">Edit</td>
                    <td class="tb_Fix50">Delete</td>
                    <td class="tb_Fix50">No.</td>
                    <td class="tb_Fix100">PO Type</td>
                    <td class="tb_Fix150">PO No.</td>
                    <td class="tb_Fix100">Amount</td>
                    <td class="tb_Fix100">PO Date</td>
                    <td class="tb_Fix100">Saled Date</td>
                    <td class="tb_Fix150">PO File</td>                     
                </tr>
                <tbody>
                    <asp:Repeater ID="rptJobOrderPO" runat="server">
                        <itemtemplate>
                            <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                <td class="td_edit tb_Fix50"><asp:LinkButton ID="btnEdit" CommandName="Edit_PO" CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></td>
                                <td class="td_delete tb_Fix50"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                                <td class="text_center tb_Fix50"><div><%#DataBinder.Eval(Container.DataItem, "no")%></div></td>
                                <td class="text_center tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "po_type")%></div></td>
                                <td class="text_center tb_Fix150"><div><%#DataBinder.Eval(Container.DataItem, "po_no")%></div></td>
                                <td class="text_right tb_Fix100"><div><asp:Label ID="lblAmount" runat ="server"><%#DataBinder.Eval(Container.DataItem, "po_amount")%></asp:Label></div></td>
                                <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "po_date")%></div></td>
                                <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "po_receipt_date")%></div></td>
                                <td class="text_left tb_Fix150"><div><%#DataBinder.Eval(Container.DataItem, "po_file")%></div></td>
                             </tr>
                        </itemtemplate>
                    </asp:Repeater> 
                </tbody>
                <tr class="table_head">
                    <td colspan = "9">
                        <div class="float_l">
                            <asp:Label ID="lblDescription" runat="server"></asp:Label>
					    </div>
					    <div class="float_r">
                            <asp:Label ID="lblPaging" runat="server" Text="&nbsp;"></asp:Label>
                        </div>
                    </td>
                </tr>                             
            </table>   
            <asp:HiddenField ID="hidJBHontaiAmount" runat="server" />
        </div>
    </form>
</body>
</html>
