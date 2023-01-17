<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTPU07.aspx.vb" Inherits="KTPU07"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
         <div class="text_center">
            <br />
            <br /><div class="font_size_12 text_left"><a href="KTPU07.aspx?New=True">Accounting</a> > Purchase Payment</div>
            <br /><div class="font_header">SEARCH PURCHASE PAYMENT</div> 
            <br />
         
            <table class="table_field" >
                <tr>
                    <td class="table_field_td" >Cheque No./Bank Transfer</td>
                    <td class="table_field_td" >
                            <asp:TextBox ID="txtChequeNoSrch" runat="server" MaxLength="100"></asp:TextBox>
                    </td>
                    <td class="table_field_td" >Pay Date<br />(dd/mm/yyyy)</td>
                    <td class="table_field_td" >
                           <asp:TextBox ID="txtChequeDateFrom" runat="server" Width="95px" 
                            MaxLength="10"></asp:TextBox><asp:CalendarExtender ID="txtChequeDateFrom_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                            TargetControlID="txtChequeDateFrom">
                        </asp:CalendarExtender>
                        &nbsp;-&nbsp;
                        <asp:TextBox ID="txtChequeDateTo" runat="server" Width="95px" MaxLength="10"></asp:TextBox><asp:CalendarExtender ID="txtChequeDateTo_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                            TargetControlID="txtChequeDateTo">
                        </asp:CalendarExtender> 
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td">Vendor Name</td>
                    <td class="table_field_td">
                            <asp:TextBox ID="txtVendor_NameLst" Width="250px" runat="server" MaxLength="150"></asp:TextBox>
                    </td>
                    <td class="table_field_td">Vendor Type</td>
                    <td class="table_field_td">
                        
                        <asp:RadioButtonList ID="rblSearchType" runat="server" 
                                RepeatDirection="Horizontal" Width="267px">
                                <asp:ListItem Text="All" Value="" Selected="True" />
                                <asp:ListItem Text="Person" Value="0" />
                                <asp:ListItem Text="Company" Value="1" />
                            </asp:RadioButtonList>
                    </td>
                </tr>  
                <tr>
                    <td colspan="4" class="text_right">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>&nbsp;&nbsp;
                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button_style"/>
                    </td>
                </tr>         
            </table> 
            <br /><br />
                <table class="table_inquiry" width="890px">
                    <tr class="table_head">
                        <td style="width:50px;">Edit</td>
                        <td style="width:50px;">Delete</td>
                        <td style="width:100px;">Voucher No</td>
                        <td style="width:150px;">Payment Type</td>
                        <td style="width:300px;">Vendor Name</td>
                        <td style="width:100px;">Vendor Type</td>
                        <td style="width:90px;">Pay Date</td>
                        <td style="width:50px;">Detail</td>
                    </tr>
                    <tbody>
                    <asp:Repeater ID="rptInquery" runat="server">
                        <itemtemplate>                        
                            <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                <td class="td_edit"><asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></td>
                                <td class="td_delete"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "voucher_no")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "cheque_no")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "vendor_name")%></div></td>
                                <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "vendor_type")%></div></td>
                                <td class="text_center"><div><%#DataBinder.Eval(Container.DataItem, "cheque_date")%></div></td>
                                <td class="text_right">
                                    <asp:LinkButton ID="btnDetail" CommandName="Detail" CssClass="icon_detail1 icon_center15" runat="server"></asp:LinkButton>
                                </td>
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


