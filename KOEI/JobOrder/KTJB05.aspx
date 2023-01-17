<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTJB05.aspx.vb" Inherits="JobOrder_KTJB05"  %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<script type="text/javascript">
    //Function input number only        
        function isNumberKey(evt) {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46){
                 return false;
                 } 
             return true;
        }
</script>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div class="text_left" style="width:1000px">
        <br /><div id="divJobOrder" runat="server" class="font_size_12 text_left"><a href="KTJB05.aspx?New=True&MenuId=3">Job Order </a> > Search Sale Invoice</div>
        <br /><div id="divAccount" runat="server"  class="font_size_12 text_left"><a href="KTJB05.aspx?New=True&MenuId=42">Account </a> > Search Sale Invoice</div>
        <br /><div class="font_header">SEARCH SALE INVOICE</div> 
        <br />
        <table class="table_field" width="1000px">
            <tr>
                <td class="table_field_td" style="width:110px;">Sale Invoice No.</td>
                <td class="table_field_td" style="width:300px;" >
                    <asp:TextBox ID="txtInvoiceNoSale" runat="server" Width="200" MaxLength="20" ></asp:TextBox>                    
                </td>
                <td class="table_field_td" style="width:150px;">&nbsp;Issue Date (dd/mm/yyyy)</td>
                <td class="table_field_td" style="width:300px;"  >
                     <asp:TextBox ID="txtIssueDateFrom" runat="server"  Width="100" MaxLength="10"></asp:TextBox>
                     <asp:CalendarExtender ID="txtIssueDateFrom_CalendarExtender" runat="server" TargetControlID="txtIssueDateFrom" Format="dd/MM/yyyy">
                     </asp:CalendarExtender>
                     <asp:FilteredTextBoxExtender ID="txtIssueDateFrom_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtIssueDateFrom" 
                         ValidChars="1234567890/" >
                     </asp:FilteredTextBoxExtender>  &nbsp; -  &nbsp;<asp:TextBox ID="txtIssueDateTo" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                     <asp:CalendarExtender ID="txtIssueDateTo_CalendarExtender" runat="server" TargetControlID="txtIssueDateTo" Format="dd/MM/yyyy">
                     </asp:CalendarExtender>
                     <asp:FilteredTextBoxExtender ID="txtIssueDateTo_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtIssueDateTo" 
                         ValidChars="1234567890/" >
                     </asp:FilteredTextBoxExtender>                 
                </td>
            </tr>
            <tr>
                <td class="table_field_td">&nbsp;Customer</td>
                <td class="table_field_td">
                     <asp:TextBox ID="txtCustomer" runat="server" Width="200" MaxLength="255"></asp:TextBox>
                </td>
                <td class="table_field_td">&nbsp;Receipt Date (dd/mm/yyyy)</td>
                <td class="table_field_td">
                     <asp:TextBox ID="txtReceiptDateFrom" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                     <asp:CalendarExtender ID="txtReceiptDateFrom_CalendarExtender" runat="server" TargetControlID="txtReceiptDateFrom" Format="dd/MM/yyyy">
                     </asp:CalendarExtender>
                     <asp:FilteredTextBoxExtender ID="txtReceiptDateFrom_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtReceiptDateFrom" 
                         ValidChars="1234567890/" >
                     </asp:FilteredTextBoxExtender>  &nbsp; - &nbsp;<asp:TextBox ID="txtReceiptDateTo" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                     <asp:CalendarExtender ID="txtReceiptDateTo_CalendarExtender" runat="server" TargetControlID="txtReceiptDateTo" Format="dd/MM/yyyy">
                     </asp:CalendarExtender>
                     <asp:FilteredTextBoxExtender ID="txtReceiptDateTo_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtReceiptDateTo" 
                         ValidChars="1234567890/" >
                     </asp:FilteredTextBoxExtender> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td">&nbsp;Sale Invoice Type</td>
                <td class="table_field_td">
                     <asp:RadioButtonList ID="rbtInvoiceType" runat="server" 
                         RepeatDirection="Horizontal">
                         <asp:ListItem Value="">All</asp:ListItem>
                         <asp:ListItem Value="1">IN</asp:ListItem>
                         <asp:ListItem Value="2">IV</asp:ListItem>
                         <%--<asp:ListItem Value="3">IS</asp:ListItem>--%>
                     </asp:RadioButtonList>
                </td>
                <td class="table_field_td">&nbsp;Job Order</td>
                <td class="table_field_td">
                     <asp:TextBox ID="txtJobOrderInvFrom" runat="server" Width="100" MaxLength="6"></asp:TextBox>
                     &nbsp; - &nbsp;<asp:TextBox ID="txtJobOrderInvTo" runat="server" Width="100" 
                         MaxLength="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>&nbsp;&nbsp;
                    <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button_style"/>&nbsp;&nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button_style"/>
                </td>
            </tr>         
        </table> 
        <%--Table show data on link from Accounting menu--%>
        <div id="divTableAccount" runat="server" class="text_left" >
            <table class="table_inquiry" style="width:1155px;">
                <tr class="table_head">
                    <td class="tb_Fix35">Edit</td>
                    <td class="tb_Fix45">Delete</td>
                    <td class="tb_Fix40">Select</td>
                    <td class="tb_Fix100">Sale Invoice No.</td>
                    <%--<td class="tb_Fix100">Sale Invoice Type</td>--%>
                    <td class="tb_Fix75">Issue Date</td>
                    <td class="tb_Fix75">Receipt Date</td>
                    <td class="tb_Fix400">Customer</td> 
                    <td class="tb_Fix100">Sale Invoice Amount(THB)</td> 
                    <td class="tb_Fix50">Status</td>
                    <td class="tb_Fix115">Bank Exchange Rate</td> 
                    <td class="tb_Fix115">Actual Amount</td> 
                    <td class="tb_Fix35">Save</td> 
                    <td class="tb_Fix45">Details</td>                       
                </tr>
                <tbody>
                    <asp:Repeater ID="rptInvoiceAccount" runat="server">
                        <itemtemplate>
                            <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                <td class="td_edit tb_Fix35"><asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_edit1 icon_left" runat="server"></asp:LinkButton></td>
                                <td class="td_delete tb_Fix45"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                                <td class="text_center tb_Fix40 ">
                                    <label class="tb_Fix40 icon_left">
                                        <input type="checkbox" id="chkApprove" runat="server"  />
                                    </label>
                                </td>
                                <td class="text_left tb_Fix100"><div><asp:Label ID="lblInvNo" runat="server"> <%#DataBinder.Eval(Container.DataItem, "invoice_no")%></asp:Label></div></td>
                                <%--<td class="text_left tb_Fix100"><div><asp:Label ID="lblInvType" runat="server"><%#DataBinder.Eval(Container.DataItem, "invoice_type")%></asp:Label></div></td>--%>                            
                                <td class="text_center tb_Fix75"><div><asp:Label ID="lblIssueDate" runat="server"><%#DataBinder.Eval(Container.DataItem, "issue_date")%></asp:Label></div></td>
                                <td class="text_center tb_Fix75"><div><asp:Label ID="lblRecDate" runat="server"><%#DataBinder.Eval(Container.DataItem, "receipt_date")%></asp:Label></div></td>
                                <td class="text_left tb_Fix400"><div><asp:Label ID="lblCustomer" runat="server"><%#DataBinder.Eval(Container.DataItem, "customer")%></asp:Label></div></td>
                                <td class="text_right tb_Fix100"><div><asp:Label ID="lblTotalamount" runat="server"><%#DataBinder.Eval(Container.DataItem, "total_amount")%></asp:Label></div></td>
                                <td class="text_center tb_Fix50"><div><asp:Label ID="lblStatus" runat="server"><%#DataBinder.Eval(Container.DataItem, "status")%></asp:Label></div></td>
                                
                                <td class="text_right tb_Fix115 ">
                                    <div> 
                                        <asp:TextBox ID="txtExchangeRate" runat="server" CssClass="tb_Fix100 text_right" MaxLength="21" onkeypress="return isNumberKey(event)" Text='<%#DataBinder.Eval(Container.DataItem, "bank_rate")%>'></asp:TextBox>
                                    </div>
                                </td>
                                
                                <td class="text_right tb_Fix115">
                                    <div> 
                                        <asp:TextBox ID="txtActualAmount" runat="server" CssClass="tb_Fix100 text_right" MaxLength="18" onkeypress="return isNumberKey(event)" Text='<%#DataBinder.Eval(Container.DataItem, "actual_amount")%>'></asp:TextBox>
                                    </div>
                                </td>
                                <td class="td_edit tb_Fix35">
                                    <div>
                                        <asp:LinkButton ID="btnSave" CommandName="Save"  CssClass="icon_save1 icon_center9" runat="server"></asp:LinkButton>
                                    </div>
                                </td>
                                <td class="td_edit tb_Fix45">
                                    <div>
                                        <asp:LinkButton ID="btnDetail" CommandName="Detail" CssClass="icon_detail1 icon_center15" runat="server"></asp:LinkButton>
                                    </div>
                                </td>
                             </tr>
                        </itemtemplate>
                    </asp:Repeater> 
                </tbody> 
                <tr>
                    <td class="text_right" colspan = "8">
                        <b><asp:Label ID="lblTotalAccount" runat="server" >Total Sale Invoice Amount(THB) : </asp:Label>
                        <asp:Label ID="lblTotalInvoiceAccount" runat="server" ></asp:Label> </b>
                    </td>
                    <td class="text_right" colspan = "5">
                        <asp:Button ID="btnConfirmReceive" runat="server" Text="Confirm Receive" CssClass="button_style_sp" />
                    </td>
                </tr>               
            </table> 
        </div>
        <%--Table show data on link from Job order menu--%>
        <div id="divTableJobOrder" runat="server" class="text_left" > 
            <table class="table_inquiry" width="1000">
                <tr class="table_head"> 
                    <td class="tb_Fix110">Sale Invoice No.</td>
                    <%--<td class="tb_Fix90">Sale Invoice Type</td>--%>
                    <td class="tb_Fix80">Issue Date</td>
                    <td class="tb_Fix80">Receipt Date</td>
                    <td class="tb_Fix400 ">Customer</td> 
                    <td class="tb_Fix150">Sale Invoice Amount(THB)</td> 
                    <td class="tb_Fix60">Status</td>
                    <td class="tb_Fix50">Details</td>                       
                </tr>
                <tbody>
                    <asp:Repeater ID="rptInvoiceJobOrder" runat="server">
                        <itemtemplate>
                            <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                 
                                <td class="text_left tb_Fix110"><div><asp:Label ID="lblInvNo1" runat="server"><%#DataBinder.Eval(Container.DataItem, "invoice_no")%> </asp:Label> </div></td>
                                <%--<td class="text_left tb_Fix90"><div><asp:Label ID="lblInvType1" runat="server"><%#DataBinder.Eval(Container.DataItem, "invoice_type")%></asp:Label></div></td> --%>                           
                                <td class="text_center tb_Fix80"><div><asp:Label ID="lblIssueDate1" runat="server"><%#DataBinder.Eval(Container.DataItem, "issue_date")%></asp:Label></div></td>
                                <td class="text_center tb_Fix80"><div><asp:Label ID="lblRecDate1" runat="server"><%#DataBinder.Eval(Container.DataItem, "receipt_date")%></asp:Label></div></td>
                                <td class="text_left tb_Fix400"><div><asp:Label ID="lblCustomer1" runat="server"><%#DataBinder.Eval(Container.DataItem, "customer")%></asp:Label></div></td>
                                <td class="text_right tb_Fix150"><div><asp:Label ID="lblAmount" runat ="server"><%#DataBinder.Eval(Container.DataItem, "total_amount")%></asp:Label></div></td>
                                <td class="text_center tb_Fix60"><div><asp:Label ID="lblStatus1" runat="server"><%#DataBinder.Eval(Container.DataItem, "status")%></asp:Label></div></td>
                                <td class="td_edit tb_Fix50">
                                    <div>
                                        <asp:LinkButton ID="btnDetail1" CommandName="Detail" CssClass="icon_detail1 icon_center15" runat="server"></asp:LinkButton>
                                    </div>
                                </td>
                             </tr>
                        </itemtemplate>
                    </asp:Repeater> 
                </tbody>
                <tr class="table_head">
                    <td colspan = "7">
                        <div class="float_l">
                            <asp:Label ID="lblDescription1" runat="server"></asp:Label>
					    </div>
					    <div class="float_r">
                            <asp:Label ID="lblPaging1" runat="server" Text="&nbsp;"></asp:Label>
                        </div>
                    </td>
                </tr>   
                <tr>
                
                    <td class="text_right" colspan = "5">
                        <b><asp:Label ID="lblTotal" runat="server" >Total Sale Invoice Amount(THB) : </asp:Label>
                        <asp:Label ID="lblTotalInvoice" runat="server" ></asp:Label> </b>
                    </td>
                    <td colspan = "2"></td>
                </tr>             
            </table>  
        </div>
    </div>
</asp:Content>

