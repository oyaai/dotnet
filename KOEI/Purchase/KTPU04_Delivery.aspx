<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTPU04_Delivery.aspx.vb" Inherits="KTPU04_Delivery"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script>
<script type="text/javascript" src="../Scripts/jsCommon.js"></script>
<script type="text/javascript">
    function calDate(textbox){
        var arr=textbox.value.split("/");
        var stDate = arr[0];
        var stMonth = arr[1];
        var stYear = arr[2];
        var intMonth = parseInt(stMonth, 10) + 2;
        intMonth = intMonth + "";
        
        if (intMonth.length < 2){
            var newMonth = "0" + intMonth + "";
        }else{
            var newMonth = intMonth + "";
        }
        
        if(newMonth == 13)
        {
            stYear = parseInt(stYear) + 1;
            newMonth = "01";
        }else if (newMonth == 14){
            stYear = parseInt(stYear) + 1;
            newMonth = "02";
        }
        
        var newDate = "05" + "/" + newMonth.substr(0,2) + "/" + stYear;
        document.getElementById('<%=txtPaymentDate.ClientID%>').value = newDate;
    }

    function check2Month(textbox){
    
        var dtFrom = document.getElementById('<%=txtDeliveryDate.ClientID%>').value;
        var arrFrom=dtFrom.split("/");
        var stMonthFrom = arrFrom[1];
        var intMonthFrom = parseInt(stMonthFrom, 10);
        
        var arrTo=textbox.value.split("/");
        var stMonthTo = arrTo[1];
        var intMonthTo = parseInt(stMonthTo, 10);

        if((intMonthTo - intMonthFrom) > 2){
            alert("Payment date over 2 months.");
        }
    }

   function waitProcess(txt)
   {
	    DivWait();
   }
   
   function DivWait(){     
        setTimeout('disableAfterTimeout()',0);     
	    processing = true;     
	    return false; 
    } 
    
    function disableAfterTimeout(){
         document.getElementById('coverit').style.visibility = 'visible';     
         document.getElementById('coverit').focus();
    } 
  
      function closeDiv(){
         document.getElementById('coverit').style.visibility = 'hidden';     
         document.aspnetForm.submit();
    } 
    
   
    
    
    
        
</script>
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div id="coverit" onkeydown="if(event.keyCode == 13 || event.keyCode == 9) return false;" style="cursor:wait;position:absolute;background-color:#FAFCF5;filter: Alpha(Opacity=90);z-index:1000;visibility:hidden;width:100%;height:100%;left:0px;top:120px;" >
            <div id="splashscreen" style="position:absolute;z-index:5;top:20%;left:35%;">
                
            </div>
        </div> 
       
        <br />
        <br /><div class="font_size_12 text_left"><a href="KTPU01.aspx?New=True">Invoice Management</a> > Invoice</div>
        <br /><div class="font_header">INVOICE INFORMATION</div> 
        <br />

         <asp:Panel ID="Panel3" runat="server" >
            <div class="text_left">
            <table class="table_field">
                <tr>
                    <td class="table_view_head" colspan="2">Invoice Information</td>
                </tr>
                <tr>
                    <td class="table_field_td">Delivery Date (dd/mm/yyyy)<span class="font_require">*</span></td>
                    <td class="table_field_td">
                            <asp:TextBox ID="txtDeliveryDate" runat="server" MaxLength="10"  
                                onchange="calDate(this)" ></asp:TextBox>
                            <asp:CalendarExtender ID="txtDeliveryDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                TargetControlID="txtDeliveryDate">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="reqDeliveryDate" runat="server" ControlToValidate="txtDeliveryDate"
                                ErrorMessage="Delivery Date is required." SetFocusOnError="True" ></asp:RequiredFieldValidator>
                            <asp:FilteredTextBoxExtender ID="txtDeliveryDate_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="txtDeliveryDate" 
                                ValidChars="1234567890/">
                            </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Payment Date (dd/mm/yyyy)<span class="font_require">*</span></td>
                    <td class="table_field_td">
                            <asp:TextBox ID="txtPaymentDate" onchange="check2Month(this)" runat="server" MaxLength="10" ></asp:TextBox>
                            <asp:CalendarExtender ID="txtPaymentDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                TargetControlID="txtPaymentDate">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="reqPaymentDate" runat="server" ControlToValidate="txtPaymentDate"
                                ErrorMessage="Payment Date is required." SetFocusOnError="True" ></asp:RequiredFieldValidator>
                            <asp:FilteredTextBoxExtender ID="txtPaymentDate_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="txtPaymentDate" 
                                ValidChars="1234567890/">
                            </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Invoice No.<span class="font_require">*</span></td>
                    <td class="table_field_td">
                            <asp:TextBox ID="txtInvoiceNo" runat="server"  MaxLength="100" Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqInvoiceNo" runat="server" ControlToValidate="txtInvoiceNo"
                                ErrorMessage="Invoice No. is required." SetFocusOnError="True" ></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Account Type<span class="font_require">*</span></td>
                    <td class="table_field_td">
                            <asp:RadioButtonList ID="rblAccountType" runat="server" RepeatDirection="Horizontal" >
                                <asp:ListItem Selected="True" Text="Current Account" Value="1" />
                                <asp:ListItem Text="Saving Account" Value="2" />
                                <asp:ListItem Text="Cash" Value="3" />
                            </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Account No.</td>
                    <td class="table_field_td">
                            <asp:TextBox ID="txtAccountNo" runat="server" MaxLength="100" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Account Name</td>
                    <td class="table_field_td">
                            <asp:TextBox ID="txtAccountName" runat="server" MaxLength="100" Width="200px" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Delivery Amount</td>
                    <td class="table_field_td">
                            <%--<asp:Label ID="lblTotalAmount" runat="server" Font-Bold="True" Text="0.00" onkeypress="return IsDecimals(event,'txtTotalAmount',2);"></asp:Label>--%>
                            <asp:TextBox ID="txtTotalAmount" runat="server" Font-Bold="true" 
                            onkeypress="return IsDecimals(event,'ctl00_MainContent_txtTotalAmount',2);" 
                            onBlur="this.value=setCurrencyPoint(this.value);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqValidatorTxtTotalAmount" runat="server" ValidationGroup="Add" SetFocusOnError="True"
                        ControlToValidate="txtTotalAmount" ErrorMessage="*Require."  class="font_error" ></asp:RequiredFieldValidator>
                        
                    <asp:RangeValidator ID="rngTxtTotalAmount" runat="server" SetFocusOnError="True"
                             ControlToValidate="txtTotalAmount" Display="Static" 
                             ErrorMessage="Please insert number" MaximumValue="99999999999999.99" 
                             MinimumValue="0.00" Type="Currency" ValidationGroup="Add" ></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Remarks</td>
                    <td class="table_field_td">
                            <asp:TextBox ID="txtRemark" runat="server" MaxLength="300" Width="200px" ></asp:TextBox>
                    </td>
                </tr>
            </table> 
            <br /><br />
            
            <table class="table_inquiry" width="960px" id="tbInquiry">
                <tr class="table_head">
                    <td style="width:130px;">Item Name</td>
                    <td style="width:70px;">Job Order</td>
                    <td style="width:80px;">Account Title</td>
                    <td style="width:40px;">Qty</td>
                    <td style="width:80px;">Amount</td>
                    <td style="width:60px;">Unit Price</td>
                    <td style="width:80px;">Remain Qty</td>
                    <td style="width:80px;">Remain Amount</td>
                    <td style="width:65px;">Delivery Qty</td>
                    <td style="width:65px;">Delivery Amount</td>
                </tr>
                <tbody>
                <asp:Repeater ID="rptInquery" runat="server">
                    <itemtemplate>                        
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "item_name")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "ie_name")%></div></td>
                            <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "quantity")%></div></td>
                            <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "amount")%></div></td>
                            <td class="text_right">
                                <div>
                                    <asp:Label ID="lblUnitPrice" Width="75" MaxLength="10" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "unit_price")%>'></asp:Label>
                                </div>
                            </td>
                            <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "remain_qty")%></div></td>
                            <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "remain_amt")%></div></td>
                            <td class="text_left">
                                <div>
                                    <asp:TextBox ID="txtDelivery_qty" Width="75" OnTextChanged="txtDelivery_qty_TextChanged" onchange = "waitProcess(this);" AutoPostback="true" MaxLength="10" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "delivery_qty")%>'></asp:TextBox>
                                </div>
                            </td>
                            <td class="text_left">
                                <div>
                                    <asp:TextBox ID="txtDelivery_amt" Width="75" OnTextChanged="txtDelivery_amt_TextChanged" onchange = "waitProcess(this);" AutoPostback="true" MaxLength="16" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "delivery_amt")%>'></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </itemtemplate>
                </asp:Repeater> 
                </tbody>           
                <tr class="table_head">
                    <td colspan = "10">
                        <div class="float_l">
                            <asp:Label ID="lblDescription" runat="server"></asp:Label>
					    </div>
					    <div class="float_r">
                        </div>
                    </td>
                </tr> 
                  
            </table>        

            <table class="table_inquiry">
                <tr>
                    <td colspan="9" class="text_right">
                    <asp:Button ID="btnCreate" runat="server" Text="Create"  CssClass="button_style" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel"  CssClass="button_style" CausesValidation="false" />
                    &nbsp;&nbsp;
                    </td>
                </tr>  
            </table>
            </div>
        </asp:Panel> 
        <asp:HiddenField ID="hidTotalAmount" runat="server" />
    

</asp:Content>

