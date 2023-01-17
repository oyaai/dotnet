<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTAP01.aspx.vb" Inherits="Approve_KTAP01" %>

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
            url: "./KTAP01.aspx/IsCheckPermission", // <-- Page Method check permission button
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
<script language="javascript"  type="text/javascript">
    function showpopup(url, frame) {
        var width = parseInt(screen.availWidth * 0.85);
        var height = parseInt(screen.availHeight * 0.75);
        var left = parseInt((screen.availWidth / 2) - (width / 2));
        var top = parseInt((screen.availHeight / 2) - (height / 2));
        var windowFeatures = "width=" + width + ",height=" + height + ",left=" + left + ",top=" + top + ",resizable,scrollbars=1";
        newwin = window.open(url, frame, windowFeatures);
        if (window.focus) { newwin.focus() }
        return false;
    }
</script>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
            <div class="text_center" style="width:900px">
                <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTAP01.aspx?New=True">Approve Management</a> > Purchase</div>
                <br /><div class="font_header">SEARCH PURCHASE APPROVE</div> 
                <br />
                <table class="table_field" >
                    <tr>
                        <td class="table_field_td" style="width:140px;" >
                            &nbsp; Type of Purchase
                        </td>
                        <td class="table_field_td " style="width:300px;" >
                            <asp:RadioButtonList ID="rbtPurchaseType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="All" Value="" />
                                <asp:ListItem Text="Purchase" Value="0" />
                                <asp:ListItem Text="Outsource" Value="1" />
                            </asp:RadioButtonList>
                        </td>
                        <td class="table_field_td" style="width:140px;" >
                            &nbsp; PO No.
                        </td>
                        <td class="table_field_td " style="width:300px;">
                            <asp:TextBox ID="txtStartPONo" runat="server" MaxLength="20" Width="100px" />&nbsp;
                            - &nbsp;<asp:TextBox ID="txtEndPONo" runat="server" MaxLength="20" 
                                Width="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="table_field_td">
                            &nbsp; Job Order
                        </td>
                        <td class="table_field_td">
                            <asp:TextBox ID="txtStartJobOrder" runat="server" MaxLength="6" Width="100px" />&nbsp;
                            - &nbsp;<asp:TextBox ID="txtEndJobOrder" runat="server" MaxLength="6" Width="100px" />
                        </td>
                        <td class="table_field_td">
                            &nbsp; Issue Date (dd/mm/yyyy)
                        </td>
                        <td class="table_field_td">
                            <asp:TextBox ID="txtStartIssueDate" runat="server" Width="100px" MaxLength="10"></asp:TextBox><asp:CalendarExtender
                                ID="CalendarExtenderStartDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartIssueDate" />
                            &nbsp; - &nbsp;<asp:TextBox ID="txtEndIssueDate" runat="server" Width="100px" MaxLength="10"></asp:TextBox><asp:CalendarExtender
                                ID="CalendarExtenderEndDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndIssueDate" />
                        </td>
                    </tr>
                    <tr>
                        <td class="table_field_td">
                            &nbsp; Vendor Name
                        </td>
                        <td class="table_field_td">
                            <asp:TextBox ID="txtVendorName" runat="server" Width="150px" MaxLength="150" />
                        </td>
                        <td class="table_field_td" colspan="2"></td>
                        <%-- Del 2013/08/09 
                        <td class="table_field_td">
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
                <table class="table_inquiry" width="900">
                    <tr class="table_head">
                        <td style="width:50px;">Select</td>
                        <td style="width:80px;">Type of Purchase</td>
                        <td style="width:130px;">Po No.</td>
                        <td style="width:80px;">Status</td>
                        <td style="width:100px;">Issue Date</td>
                        <td style="width:100px;">Applied By</td> 
                        <td style="width:150px;">Vendor Name</td> 
                        <td style="width:50px;">Details</td>           
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
                                    <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "po_type")%></div></td>
                                    <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "po_no")%></div></td>
                                    <td class="text_center"><div><asp:Label ID="lblStatus" runat="server"><%#DataBinder.Eval(Container.DataItem, "status")%></asp:Label></div></td>
                                    <td class="text_center"><div><%#DataBinder.Eval(Container.DataItem, "issue_date")%></div></td>
                                    <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "applied_by")%></div></td>
                                    <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "vendor_name")%></div></td>
                                    <td class="td_edit">
                                        <div>
                                            <asp:LinkButton ID="btnDetail" CommandName="Detail" CssClass="icon_detail1 icon_center15" runat="server"></asp:LinkButton>
                                        </div>
                                    </td>
                                 </tr>
                            </itemtemplate>
                        </asp:Repeater> 
                    </tbody>
                    <tr class="table_head">
                        <td colspan = "11">
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
