<%@ Page Title="KOEI TOOL Management System" Language="VB" AutoEventWireup="false" CodeFile="KTAC02_Search.aspx.vb" Inherits="Account.KTAC02_Search" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/common.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/design.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/fix_table.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/font.css" type="text/css" rel="Stylesheet" />
</head>
<script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script>
<script language="javascript"  type="text/javascript" >
    
     // fn calculate Total in thai bath
    function IsValidDate_ClientValidate(sender, args) {
        var receiptDateFrom = $('#<% = txtReceiptDateStart.ClientID %>').val()
        var receiptDateTo = $('#<% = txtReceiptDateEnd.ClientID %>').val()
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02_Search.aspx/IsValidDate", // <-- Page Method
            data: "{strDateFrom: '" + receiptDateFrom + "', strDateTo: '" + receiptDateTo + "', name: 'en-AU'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async:false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });              
    }
     // fn calculate Total in thai bath
    function checkDateRange(sender, args) {    
        var receiptDateFrom = $('#<% = txtReceiptDateStart.ClientID %>').val()
        var receiptDateTo = $('#<% = txtReceiptDateEnd.ClientID %>').val()
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02_Search.aspx/IsValidDateRange", // <-- Page Method
            data: "{strDateFrom: '" + receiptDateFrom + "', strDateTo: '" + receiptDateTo + "'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async:false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });              
    }
     // fn calculate Total in thai bath
    function checkJobOrderRange(sender, args) {
        var jobOrderFrom = $('#<% = txtJobOrderStart.ClientID %>').val()
        var jobOrderTo = $('#<% = txtJobOrderEnd.ClientID %>').val()
        if (jobOrderFrom != "" && jobOrderTo != "") {
            if (jobOrderTo < jobOrderFrom) {
                args.IsValid = false;
            }else {
                args.IsValid = true;
            }
        }
    }
</script>
<body>
    <form id="form1" runat="server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
   
    <div class="text_center">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td class="font_header">
                    <asp:Label ID="lblHeader" runat="server" Text="SEARCH INCOME"></asp:Label>
                    <br />
                </td>
            </tr>
            <tr>
                <td class="text_left">                            
                    <table class="table_field">
                        <tr>
                            <td class="table_field_td tb_Fix100">
                                Job Order 
                            </td>
                            <td class="table_field_td tb_Fix300">
                                
                                <asp:TextBox ID="txtJobOrderStart" runat="server" MaxLength="6" Width="100px" />
                                &nbsp;&nbsp;-&nbsp;&nbsp;
                                <asp:TextBox ID="txtJobOrderEnd" runat="server" MaxLength="6" Width="100px" />
                                <br />
                                 <asp:CustomValidator ID="reqJobOrderRange" runat="server" 
                                    ErrorMessage="*Not exist Job Order." 
                                    ClientValidationFunction="checkJobOrderRange" />
                                    
                                <asp:FilteredTextBoxExtender ID="txtJobOrderStart_FilteredTextBoxExtender" 
                                 runat="server" Enabled="True" TargetControlID="txtJobOrderStart" 
                                 ValidChars="1234567890" />
                                <asp:FilteredTextBoxExtender ID="txtJobOrderEnd_FilteredTextBoxExtender" 
                                 runat="server" Enabled="True" TargetControlID="txtJobOrderEnd" 
                                 ValidChars="1234567890" />
                            </td>
                            <td class="table_field_td tb_Fix100">
                                AccountType<br />&nbsp;
                            </td>
                            <td class="table_field_td tb_fix300">&nbsp;
                                <asp:RadioButtonList ID="rbtAccountType" runat="server" 
                                    RepeatDirection="Horizontal" Height="23px" Width="280px" 
                                    RepeatLayout="Flow">
                                    <asp:ListItem Text="Current Account  " Value="1" />
                                    <asp:ListItem Text="Saving Account  " Value="2" />
                                    <asp:ListItem Text="Cash " Value="3" />
                                </asp:RadioButtonList><br />
                            </td>
                        </tr>
                        <tr>
                            <td class="table_field_td">
                                Vendor Name 
                            </td>
                            <td class="table_field_td">
                                <asp:TextBox MaxLength="50" ID="txtVendorName" Width="186px" runat="server" />
                                <br />
&nbsp;<br />
                            </td>
                            <td class="table_field_td">
                                Account Title&nbsp;
                            </td>
                            <td class="table_field_td">
                                <asp:DropDownList ID="ddlItemExpense" runat="server" Width="186px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="table_field_td">
                                Account Name
                            </td>
                            <td class="table_field_td">
                                <asp:TextBox MaxLength="50" ID="txtAccountName" Width="186px" runat="server" 
                                    SkinID="3" />
                                <br />
&nbsp;<br />
                            </td>
                            <td class="table_field_td">
                                Account No.
                            </td>
                            <td class="table_field_td">
                                <asp:TextBox MaxLength="100" ID="txtAccountNo" Width="186px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="table_field_td">
                                <asp:Label ID="lblDate" runat="server" Text="Receipt Date"></asp:Label>
                                <br />
                                (dd/mm/yyyy) </td>
                            <td class="table_field_td" colspan="3">
                                <asp:TextBox MaxLength="10" ID="txtReceiptDateStart" runat="server" Width="100px" />
                                &nbsp;&nbsp;-&nbsp;&nbsp;
                                <asp:TextBox ID="txtReceiptDateEnd" runat="server" MaxLength="10" 
                                    Width="100px" />
                                <asp:CalendarExtender ID="cldReceiptDateStart" runat="server" Format="dd/MM/yyyy" TargetControlID="txtReceiptDateStart" />
                                <asp:CalendarExtender ID="cldReceiptDateEnd" runat="server" Format="dd/MM/yyyy" TargetControlID="txtReceiptDateEnd" />
                                
                                &nbsp;<asp:CustomValidator ID="reqDateRange" runat="server" 
                                    ErrorMessage="*CustomValidator" ClientValidationFunction="checkDateRange"></asp:CustomValidator>                                 
                                <br />
                                <asp:CustomValidator ID="reqDateInvalid" runat="server" ErrorMessage="*Invalid format." 
                                    ClientValidationFunction="IsValidDate_ClientValidate" /> 
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" class="text_right">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" Width="100px" 
                                    CssClass="button_style" />
                                &nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" 
                                    CssClass="button_style" CausesValidation="False" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="table_inquiry">
                        <tr class="table_head">
                            <td class="tb_Fix50">
                                Select
                            </td>
                            <td class="tb_Fix80">
                                Job Order
                            </td>
                            <td class="tb_Fix150">
                                Account Type
                            </td>
                            <td class="tb_Fix150">
                                Vendor Name
                            </td>
                            <td class="tb_Fix200">
                                Account Title
                            </td>
                            <td class="tb_Fix150">
                                Account Name
                            </td>
                            <td class="tb_Fix100">
                                Account No.
                            </td>
                            <td class="tb_Fix120">
                                <asp:Label ID="lblDate2" runat="server" Text="Receipt Date"></asp:Label>
&nbsp;</td>
                            <td class="tb_Fix100">
                                Sub Total (Amount)
                            </td>
                        </tr>
                        <asp:Repeater ID="rptInquery" runat="server">
                            <itemtemplate>
                                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                        <td class="tb_Fix50"><div class="ctb_Fix50"><asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_edit1 icon_center15" runat="server" CausesValidation="false" ></asp:LinkButton></div></td>
                                        <td class="tb_Fix80"><div class="ctb_Fix80"><%#DataBinder.Eval(Container.DataItem, "Job_Order")%></div></td>
                                        <td class="tb_Fix150"><div class="ctb_Fix150"><%#DataBinder.Eval(Container.DataItem, "Account_Type")%></div></td>
                                        <td class="tb_Fix150"><div class="ctb_Fix150 text_left"><%#DataBinder.Eval(Container.DataItem, "Vendor_Name")%></div></td>
                                        <td class="tb_Fix200"><div class="ctb_Fix200 text_left"><%#DataBinder.Eval(Container.DataItem, "Account_Title")%></div></td>
                                        <td class="tb_Fix150"><div class="ctb_Fix150"><%#DataBinder.Eval(Container.DataItem, "Account_Name")%></div></td>
                                        <td class="tb_Fix100"><div class="ctb_Fix100"><%#DataBinder.Eval(Container.DataItem, "Account_No")%></div></td>
                                        <td class="tb_Fix120"><div class="ctb_Fix120"><%#DataBinder.Eval(Container.DataItem, "Receipt_Date")%></div></td>
                                        <td class="tb_Fix100"><div class="ctb_Fix100 text_right"><%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Sub_Total")).ToString("#,##0.00")%></div> </td>
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
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
