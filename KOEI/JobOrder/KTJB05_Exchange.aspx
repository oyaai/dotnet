<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTJB05_Exchange.aspx.vb" Inherits="JobOrder_KTJB05_Exchange" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ExChange Rate</title>
    <%--<head id="ExChange Rate" runat="server">--%>
    <link href="../App_Themes/common.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/design.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/font.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/fix_table.css" type="text/css" rel="Stylesheet" />
    <script language="javascript"  type="text/javascript" >
    //function send value to parent screen
        function addItemsToParent()
        { 
            //window.opener.addToParentItemData_Conver(valHontai.value,valSum.value,valTotal.value);
            window.close();             
        }
        function isNumberKey(evt) {
         var charCode = (evt.which) ? evt.which : event.keyCode
         if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
             return false;
         else {
             var len = document.getElementById("txtHontaiAmount").value.length;
             var index = document.getElementById("txtHontaiAmount").value.indexOf('.'); 
             
             if (index > 0 && charCode == 46) {    
                return false;                           
             }
         }
         return true;
      }
    </script>
    <script type="text/javascript" src="../Scripts/jsCommon.js"></script>   
</head>
</head>
<body>
    <form id="form1" runat="server">
    <div style="position:fixed;top:50px;right:700px;">
        <label>ExChangeRate</label>
        <asp:TextBox ID="txtExChangeRate" runat="server" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqValidatorExchangeRate" runat="server" ValidationGroup="second" SetFocusOnError="True"
                        ControlToValidate="txtExChangeRate" ErrorMessage="*Require."  class="font_error" onkeyup="this.value=addCommas(this.value);"></asp:RequiredFieldValidator>  
                    <asp:RangeValidator ID="rngExchangeRate" runat="server" 
                             ControlToValidate="txtExchangeRate" Display="Static" 
                             ErrorMessage="*Invalid Actual Exchange Rate (THB) >= 0" MaximumValue="99999999999.99999" 
                             MinimumValue="0.00" Type="Double" ValidationGroup="Add"></asp:RangeValidator> 
    </div>
    <div  style="position:fixed;top:80px;right:700px;">
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button_style_sp" />
        <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="button_style" OnClientClick="addItemsToParent();" /> 
    </div>
    </form>
</body>
</html>
