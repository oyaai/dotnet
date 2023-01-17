<%@ Page  Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTAC01.aspx.vb" Inherits="KTAC01" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <div class="text_center">
        <br />
        <br /><div class="font_size_12 text_left"><a href="KTAC01.aspx?New=True">Accounting</a> > Accounting</div>
        <br /><div class="font_header">ACCOUNTING</div> 
        <br />

        <table class="table_field">
            <tr>
                <td class="table_field_td">
                    Month/Year
                </td>
                <td class="table_field_td">
                    <asp:DropDownList ID="txtStartDate" runat="server" CssClass="dropdown_field" Width="100px">
                        <asp:ListItem Value="1">January</asp:ListItem>
                        <asp:ListItem Value="2">February</asp:ListItem>
                        <asp:ListItem Value="3">March</asp:ListItem>
                        <asp:ListItem Value="4">April</asp:ListItem>
                        <asp:ListItem Value="5">May</asp:ListItem>
                        <asp:ListItem Value="6">June</asp:ListItem>
                        <asp:ListItem Value="7">July</asp:ListItem>
                        <asp:ListItem Value="8">August</asp:ListItem>
                        <asp:ListItem Value="9">September</asp:ListItem>
                        <asp:ListItem Value="10">October</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList>&nbsp;-&nbsp;<asp:DropDownList ID="txtEndDate" runat="server" CssClass="dropdown_field" Width="100px"></asp:DropDownList>
                </td>
                <td class="table_field_td">
                    Account Title
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtStartIE" runat="server" Width="95px" MaxLength="20" />&nbsp; - &nbsp;<asp:TextBox ID="txtEndIE" runat="server" Width="95px" MaxLength="20" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    Job Order
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtStartJobOrder" runat="server" Width="95px" MaxLength="6"  />
                    &nbsp;-&nbsp;<asp:TextBox ID="txtEndJobOrder" runat="server" Width="95px" MaxLength="6" />
                </td>
                <td class="table_field_td">
                    Vendor Name
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtVendorName" runat="server" MaxLength="150" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    Type of Purchase
                </td>
                <td colspan="3" class="table_field_td">
                    <asp:RadioButtonList ID="rblSearchType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="All" Value="" Selected="True" />
                        <asp:ListItem Text="Current Account" Value="1" />
                        <asp:ListItem Text="Saving Account" Value="2" />
                        <asp:ListItem Text="Cash" Value="3" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style" />&nbsp;&nbsp;
                    <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button_style" />                   
                </td>
            </tr>
        </table>
        <br />
        <br />
        
        
            <table class="table_inquiry" width="950">
                <tr class="table_head">
                    <td>Voucher No.</td>
                    <td>Date</td>
                    <td>Cheque No.</td>
                    <td>Vendor Name</td>
                    <td>Account Title</td>
                    <td>Job Order</td>
                    <td>Income</td>
                    <td>Expense</td>
                    <td>Details</td>
                </tr>
                <tbody>
                <asp:Repeater ID="rptInquery" runat="server">
                    <itemtemplate>                        
                            <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "voucher_no")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "account_date")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "cheque_no")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "vendor_name")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "Ie_name")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "income")%></div></td>
                                <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "Expense")%></div></td>
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
       
    </div>
</asp:Content>
