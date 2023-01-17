<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTPU03.aspx.vb" Inherits="KTPU03"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
     <div class="text_center">
        <br />
        <br /><div class="font_size_12 text_left"><a href="KTPU03.aspx?New=True">Purchase</a> > Search Invoice</div>
        <br /><div class="font_header">SEARCH INVOICE</div> 
        <br />

        <table class="table_field" width="940px" >
            <tr>
                <td class="table_field_td tb_Fix170">Type of Purchase</td>
                <td class="table_field_td tb_Fix300">
                        <asp:RadioButtonList ID="rblSearchType" runat="server" 
                            RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Text="All&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Value="" Selected="True" />
                            <asp:ListItem Text="Purchase&nbsp;" Value="0" />
                            <asp:ListItem Text="Outsource" Value="1" />
                        </asp:RadioButtonList>
                </td>
                <td class="table_field_td tb_Fix170">PO No.</td>
                <td class="table_field_td tb_Fix300">
                        <asp:TextBox ID="txtPO" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                        &nbsp;</td>
            </tr>
            <tr>
                <td class="table_field_td">Delivery Date (dd/mm/yyyy)</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDeliveryDateFrom" runat="server" Width="95px" 
                        MaxLength="10"></asp:TextBox>
                    <asp:CalendarExtender ID="txtDeliveryDateFrom_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                        TargetControlID="txtDeliveryDateFrom">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="txtDeliveryDateFrom_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtDeliveryDateFrom" 
                         ValidChars="1234567890/" />
                         &nbsp;-&nbsp;
                         <asp:TextBox ID="txtDeliveryDateTo" runat="server" Width="95px" 
                        MaxLength="10"></asp:TextBox>
                    <asp:CalendarExtender ID="txtDeliveryDateTo_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                        TargetControlID="txtDeliveryDateTo">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="txtDeliveryDateTo_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtDeliveryDateTo" 
                         ValidChars="1234567890/" />
                </td>
                <td class="table_field_td">Payment Date (dd/mm/yyyy)</td>
                <td class="table_field_td">
                        <asp:TextBox ID="txtPaymentDateFrom" runat="server" Width="95px" 
                            MaxLength="10" ></asp:TextBox>
                        <asp:CalendarExtender ID="txtPaymentDateFrom_CalendarExtender" runat="server"
                            Format="dd/MM/yyyy" TargetControlID="txtPaymentDateFrom">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="txtPaymentDateFrom_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtPaymentDateFrom" 
                         ValidChars="1234567890/" />
                         &nbsp;-&nbsp;
                         <asp:TextBox ID="txtPaymentDateTo" runat="server" Width="95px" 
                            MaxLength="10"></asp:TextBox>
                        <asp:CalendarExtender ID="txtPaymentDateTo_CalendarExtender" runat="server"
                            Format="dd/MM/yyyy" TargetControlID="txtPaymentDateTo">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="txtPaymentDateTo_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtPaymentDateTo" 
                         ValidChars="1234567890/" />                     
                        
                </td>
            </tr>  
            <tr>
                <td class="table_field_td">Vendor Name</td>
                <td class="table_field_td">
                        <asp:TextBox ID="txtVendor_Name" runat="server" Width="290px"  MaxLength="150"></asp:TextBox>
                </td>
                <td class="table_field_td">Invoice No.</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtInvoice_Start" runat="server" Width="95px" MaxLength="100"></asp:TextBox>
                    &nbsp;-&nbsp;
                    <asp:TextBox ID="txtInvoice_End" runat="server" Width="95px" MaxLength="100"></asp:TextBox>
                </td>
            </tr>  
            <tr>
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>&nbsp;&nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button_style"/>&nbsp;&nbsp;
                </td>
            </tr>         
        </table> 
        <br /><br />
            <table class="table_inquiry" width="1055px">
                <tr class="table_head">
                    <td style="width:45px;">Edit</td>
                    <td style="width:40px;">Delete</td>
                    <td style="width:10px;">&nbsp;</td>
                    <td style="width:75px;">Type of Purchase</td>
                    <td style="width:180px;">PO No.</td>
                    <td style="width:300px;">Vendor Name</td>
                    <td style="width:120px;">Invoice No</td>
                    <td style="width:75px;">Delivery Date</td>
                    <td style="width:75px;">Pay Date</td>
                    <td style="width:80px;">Delivery Amount</td>
                    <td style="width:55px;">Detail</td>
                </tr>
                <tbody>
                <asp:Repeater ID="rptInquery" runat="server">
                    <itemtemplate>                        
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="td_edit" width="45px"><asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></td>
                            <td class="td_delete" width="40px"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                            <td class="text_left" width="10px">
                                <label>
                                    <%--<input type="checkbox" id="chkConfirm" runat="server" />--%>
                                    <asp:checkbox ID="chkConfirm" CommandName="Checkbox" runat="server"></asp:checkbox>
                                </label>
                            </td>
                            <td class="text_left" width="75px"><%#DataBinder.Eval(Container.DataItem, "po_type")%></td>
                            <td class="text_left" width="180px"><%#DataBinder.Eval(Container.DataItem, "po_no")%></td>
                            <td class="text_left" width="300px"><%#DataBinder.Eval(Container.DataItem, "vendor_name")%></td>
                            <td class="text_left" width="120px"><%#DataBinder.Eval(Container.DataItem, "invoice_no")%></td>
                            <td class="text_left" width="75px"><%#DataBinder.Eval(Container.DataItem, "delivery_date")%></td>
                            <td class="text_left" width="75px"><%#DataBinder.Eval(Container.DataItem, "payment_date")%></td>
                            <td class="text_right" width="80px"><%#DataBinder.Eval(Container.DataItem, "total_delivery_amount")%></td>
                            <td class="text_left" width="55px">
                                    <asp:LinkButton ID="btnDetail" CommandName="Detail" CssClass="icon_detail1 icon_center15" runat="server"></asp:LinkButton>
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
            <table class="table_field" >
	            <tr>
                    <td class="text_left">
                        <asp:Button ID="btnConfirmPayment" runat="server" Text="Confirm Payment" CssClass="button_style_sp"/>&nbsp;&nbsp;
                    </td>
                </tr>         
            </table>       
        </div>
</asp:Content>

