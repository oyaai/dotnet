<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTAC04.aspx.vb" Inherits="KTAC04" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <div class="text_center">
        <br />
        <br /><div class="font_size_12 text_left"><a href="KTAC01.aspx?New=True">Accounting</a> > Withholding Tax</div>
        <br /><div class="font_header">WITHHOLDING TAX</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td">
                    Month and Year</td>
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
                    PO No.</td>
                <td class="table_field_td">
                    <asp:TextBox id="txtStartPONo" runat="server" MaxLength="20" Width="100px" />&nbsp; - &nbsp;<asp:TextBox id="txtEndPONo" runat="server" MaxLength="20" Width="100px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    Job Order</td>
                <td class="table_field_td">
                    <asp:TextBox id="txtStartJobOrder" runat="server" MaxLength="6" Width="100px" />&nbsp; - &nbsp;<asp:TextBox 
                        id="txtEndJobOrder" runat="server" MaxLength="6" Width="100px" />
                </td>
                <td class="table_field_td">
                    Vendor Name</td>
                <td class="table_field_td">
                    <asp:TextBox id="txtVendorName" runat="server" MaxLength="150" Width="225px" />
                </td>
            </tr>
            <tr>
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style" />&nbsp;&nbsp;
                    <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button_style" />                   
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" >
        <br/>
            <table class="table_inquiry" border = "0" width="992">
                <tr class="table_head">
                    <td width="140px">Vendor Name</td>
                    <td width="210px">Account Title</td>
                    <td width="70px">Date</td>
                    <td width="80px">Job Order</td>
                    <td width="90px">PO No.</td>
                    <td width="160px">Part Name</td>
                    <td width="80px">Amount</td>
                    <td width="60px">W/T</td>
                    <td width="100px">W/T (Baht)</td>
                </tr>
                <tbody>
                <asp:Repeater ID="rptInquery" runat="server">
                    <itemtemplate>                        
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="text_left" width="140px"><div><%#DataBinder.Eval(Container.DataItem, "vendor_name")%></div></td>
                            <td class="text_left" width="210px"><div><%#DataBinder.Eval(Container.DataItem, "Ie_name")%></div></td>
                            <td class="text_left" width="70px"><div><%#DataBinder.Eval(Container.DataItem, "account_date")%></div></td>
                            <td class="text_left" width="80px"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                            <td class="text_left" width="90px"><div><%#DataBinder.Eval(Container.DataItem, "po_no")%></div></td>
                            <td class="text_left" width="160px"><div><%#DataBinder.Eval(Container.DataItem, "part_name")%></div></td>
                            <td class="text_right" width="80px"><div><%#DataBinder.Eval(Container.DataItem, "sub_total")%></div></td>
                            <td class="text_right" width="60px"><div><%#DataBinder.Eval(Container.DataItem, "wt_percentage")%></div></td>
                            <td class="text_right" width="100px"><div><%#DataBinder.Eval(Container.DataItem, "wt_amount")%></div></td>
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
        </asp:Panel>
</div>
</asp:Content>

