<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTAP02.aspx.vb" Inherits="Approve_KTAP02" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
<script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script>
<script type="text/javascript" src="../Scripts/jsCommon.js"></script>
<script language="javascript"  type="text/javascript" >

    function IsCheckPermission(args) {       
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAP02.aspx/IsCheckPermission", // <-- Page Method check permission button
            data: "{blnItem: '" + args.checked + "'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async:false,
            success: function(msg) {  
                //Check checkbox is checked
                var sum = 0;
                $('input[id*="chkApprove"]:checked').each(function() // mathing all checked imputs
                  {                      
                    sum++;
                   }
                  )
                  //if all checkbox is not check,button is disable.
                  if (sum.toString() == "0") {
                    msg.d = true ;
                  }   
                                                                    
                   $("#ctl00_MainContent_btnApprove").prop('disabled', msg.d);  //true if enable, false if disable
                   $("#ctl00_MainContent_btnDecline").prop('disabled', msg.d); 
                   $("#ctl00_MainContent_btnDelete").prop('disabled', msg.d);                   
            }
        });  
    } 
     
</script>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <div style="width:990px">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTAP02.aspx?New=True">Approve Management</a> > Account</div>
        <br /><div class="font_header">SEARCH ACCOUNTING APPROVE</div> 
        <br /> 
        <table class="table_field" style="width:840px">
            <tr>
                <td class="table_field_td tb_Fix120">
                    &nbsp; Type
                </td>
                <td class="table_field_td tb_Fix300">
                    <asp:RadioButtonList ID="rbtType" runat="server" RepeatDirection="Horizontal" 
                        Height="16px">
                        <asp:ListItem Text="All&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Value="" />
                        <asp:ListItem Text="Income&nbsp;" Value="2" />
                        <asp:ListItem Text="Payment" Value="1" />
                    </asp:RadioButtonList>
                </td>
                <td class="table_field_td tb_Fix120">
                    &nbsp; Account Title
                </td>
                <td class="table_field_td tb_Fix300">
                    <asp:TextBox ID="txtStartIE" runat="server" MaxLength="20" Width="120px" />
                    &nbsp;-&nbsp; <asp:TextBox ID="txtEndIE" runat="server" MaxLength="20" Width="120px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    &nbsp; Job Order
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtStartJobOrder" runat="server" MaxLength="6" Width="120px" />
                    &nbsp;-&nbsp; <asp:TextBox ID="txtEndJobOrder" runat="server" MaxLength="6" Width="120px" />
                </td>
                <td class="table_field_td">
                    &nbsp; Issue Date 
                    <br />
                    &nbsp; (dd/mm/yyyy)
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtStartIssueDate" runat="server" Width="120px" MaxLength="10"></asp:TextBox><asp:CalendarExtender
                        ID="CalendarExtenderStartDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartIssueDate" />
                    &nbsp;-&nbsp; <asp:TextBox ID="txtEndIssueDate" runat="server" Width="120px" MaxLength="10"></asp:TextBox><asp:CalendarExtender
                        ID="CalendarExtenderEndDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndIssueDate" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    &nbsp; Vendor Name
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtVendorName" runat="server" Width="265" MaxLength="150" />
                </td>
                <td class="table_field_td" colspan="2"></td> 
                <%--<td class="table_field_td">
                    &nbsp; Approve Status
                </td>
                <td class="table_field_td">
                    <asp:CheckBoxList ID="chkApprovalStatus" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Waiting" Value="3" />
                        <asp:ListItem Text="Approve" Value="4" />
                        <asp:ListItem Text="Decline" Value="5" />
                    </asp:CheckBoxList>
                </td>--%>
            </tr>
            <tr> 
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>
                </td>
            </tr>
        </table>
        <table class="table_inquiry" width="840">
            <tr class="table_head">
                <td class="tb_Fix50">Select</td>
                <td class="tb_Fix110">Type</td>
                <td class="tb_Fix100">Voucher No.</td>
                <%--<td class="tb_Fix150">Vendor Name</td>--%>
                <td class="tb_Fix110">Status</td>
                <td class="tb_Fix110">Input Cheque</td>
                <td class="tb_Fix110">Pay Date</td>
                <td class="tb_Fix200">Applied By</td> 
                <td class="tb_Fix50">Details</td>           
            </tr>
            <tbody>
                <asp:Repeater ID="rptApprove" runat="server">
                    <itemtemplate>
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="text_center">
                                <label>
                                    <input type="checkbox" id="chkApprove" runat="server" onclick="IsCheckPermission(this);"  />
                                </label>
                            </td>
                            <td class="text_left tb_Fix110"><div><%#DataBinder.Eval(Container.DataItem, "account_type")%></div></td>
                            <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "voucher_no")%></div></td>
                            <%--<td class="text_left tb_Fix150"><div><%#DataBinder.Eval(Container.DataItem, "vendor_name")%></div></td>--%>
                            <td class="text_center tb_Fix110"><div><asp:Label ID="lblStatus" runat="server"><%#DataBinder.Eval(Container.DataItem, "status")%></asp:Label></div></td>
                            <td class="text_center tb_Fix110"><div><%#DataBinder.Eval(Container.DataItem, "input_cheque")%></div></td>
                            <td class="text_center tb_Fix110"><div><%#DataBinder.Eval(Container.DataItem, "issue_date")%></div></td>
                            <td class="text_left tb_Fix200"><div><%#DataBinder.Eval(Container.DataItem, "applied_by")%></div></td> 
                            <td class="td_edit tb_Fix50">
                                <div>
                                    <asp:LinkButton ID="btnDetail" CommandName="Detail" CssClass="icon_detail1 icon_center15" runat="server"></asp:LinkButton>
                                </div>
                            </td>
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
        <table width="900">
            <tr>
                <td>&nbsp;</td>
                <td class="text_left" >
                    <asp:Button ID="btnApprove" Text="Approve" CssClass="button_style action" runat="server" />
                    &nbsp;&nbsp;<asp:Button ID="btnDecline" Text="Decline" CssClass="button_style action" runat="server" />
                    &nbsp;&nbsp;<asp:Button ID="btnDelete" Text="Delete" CssClass="button_style action" runat="server" />  
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
