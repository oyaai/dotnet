<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTAC05.aspx.vb" Inherits="KTAC05" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <div class="text_center">
        <br />
        <br /><div class="font_size_12 text_left"><a href="KTAC01.aspx?New=True">Accounting</a> > Cost Table Overview</div>
        <br /><div class="font_header">COST TABLE OVERVIEW</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td" style="width:150px;">
                    Year</td>
                <td class="table_field_td" style="width:300px;">
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropdown_field" Width="100px"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnPdf" runat="server" Text="PDF" CssClass="button_style" />                   
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" Visible=false >
        <br/>
            <table class="table_inquiry" border = "0" width="840">
                <tr class="table_head">
                    <td width="80px">Job Order</td>
                    <td width="230px">Vendor Name</td>
                    <td width="100px">Job Order Type</td>
                    <td width="110px">Part No.</td>
                    <td >Part Name</td>
                </tr>
                <tbody>
                <asp:Repeater ID="rptInquery" runat="server">
                    <itemtemplate>                        
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "vendor_name")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "job_type_name")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "part_no")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "part_name")%></div></td>
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



