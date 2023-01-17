<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTPU06.aspx.vb" Inherits="KTPU06"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

     <div class="text_center">
        <br />
        <br /><div class="font_size_12 text_left"><a href="KTPU05.aspx?New=True">Purchase</a> > Vendor Rating</div>
        <br /><div class="font_header">VENDOR RATING MANAGEMENT</div> 
        <br />
     
        <table class="table_field" >
            <tr>
                <td class="table_field_td" >Invoice No.</td>
                <td class="table_field_td" >
                        <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="100" ></asp:TextBox>
                </td>
                <td class="table_field_td" >Type of Purchase</td>
                <td class="table_field_td" >
                        <asp:RadioButtonList ID="rblSearchType" runat="server" 
                            RepeatDirection="Horizontal" Width="267px">
                            <asp:ListItem Text="All" Value="" Selected="True" />
                            <asp:ListItem Text="Purchase" Value="0" />
                            <asp:ListItem Text="Outsource" Value="1" />
                        </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">PO No.</td>
                <td class="table_field_td">
                        <asp:TextBox ID="txtPO" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                </td>
                <td class="table_field_td">Payment Date<br />(dd/mm/yyyy)</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtPaymentDateFrom" runat="server" Width="95px" 
                        MaxLength="10"></asp:TextBox><asp:CalendarExtender ID="txtPaymentDateFrom_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                        TargetControlID="txtPaymentDateFrom">
                    </asp:CalendarExtender>
                    &nbsp;-&nbsp;
                    <asp:TextBox ID="txtPaymentDateTo" runat="server" Width="95px" MaxLength="10"></asp:TextBox><asp:CalendarExtender ID="txtPaymentDateTo_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                        TargetControlID="txtPaymentDateTo">
                    </asp:CalendarExtender>
                </td>
            </tr>  
            <tr>
                <td class="table_field_td">Vendor Name</td>
                <td class="table_field_td">
                        <asp:TextBox ID="txtVendor_Name" runat="server" MaxLength="150"></asp:TextBox>
                </td>
                <td class="table_field_td">Delivery Date<br />(dd/mm/yyyy)</td>
                <td class="table_field_td">
                        <asp:TextBox ID="txtDeliveryDateFrom" runat="server" Width="95px" 
                            MaxLength="10" ></asp:TextBox>
                        <asp:CalendarExtender ID="txtDeliveryDateFrom_CalendarExtender" runat="server"
                            Format="dd/MM/yyyy" TargetControlID="txtDeliveryDateFrom">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="txtDeliveryDateFrom_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" TargetControlID="txtDeliveryDateFrom" 
                            ValidChars="1234567890/">
                        </asp:FilteredTextBoxExtender>
                        &nbsp;-&nbsp;
                        <asp:TextBox ID="txtDeliveryDateTo" runat="server" Width="95px" 
                            MaxLength="10"></asp:TextBox>
                        <asp:CalendarExtender ID="txtDeliveryDateTo_CalendarExtender" runat="server"
                            Format="dd/MM/yyyy" TargetControlID="txtDeliveryDateTo">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="txtDeliveryDateTo_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" TargetControlID="txtDeliveryDateTo" 
                            ValidChars="1234567890/">
                        </asp:FilteredTextBoxExtender>
                        <br />
                        <asp:CustomValidator ID="custxttxtDeliveryDateFrom" runat="server" 
                            ControlToValidate="txtDeliveryDateFrom" Display="None" 
                            ErrorMessage="Delivery Date (From) is invalid format" ValidationGroup="Search"></asp:CustomValidator>
                        <asp:CustomValidator ID="custxtDeliveryDateTo" runat="server" 
                            ControlToValidate="txtDeliveryDateTo" Display="None" 
                            ErrorMessage="Delivery Date (To) is invalid format" ValidationGroup="Search"></asp:CustomValidator>
                </td>
            </tr>  
            <tr>
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="button_style"/>
                </td>
            </tr>         
        </table> 
        <br /><br />
            <table class="table_inquiry" width="862px">
                <tr class="table_head">
                    <td style="width:40px;">Rate</td>
                    <td style="width:130px;">Invoice No</td>
                    <td style="width:130px;">PO No</td>
                    <td style="width:180px;">Vendor Name</td>
                    <td style="width:100px;">Payment Date</td>
                    <td style="width:100px;">Delivery Date</td>
                </tr>
                <tbody>
                <asp:Repeater ID="rptInquery" runat="server">
                    <itemtemplate>                        
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="td_edit">
                                <asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_rating1 icon_center15" runat="server"></asp:LinkButton>
                            </td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "invoice_no")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "po_no")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "vendor_name")%></div></td>
                            <td class="text_center"><div><%#DataBinder.Eval(Container.DataItem, "payment_date")%></div></td>
                            <td class="text_center"><div><%#DataBinder.Eval(Container.DataItem, "delivery_date")%></div></td>
                        </tr>
                    </itemtemplate>
                </asp:Repeater> 
                </tbody>           
                <tr class="table_head">
                    <td colspan = "12">
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
</asp:Content>

