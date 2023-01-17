<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTJB03.aspx.vb" Inherits="JobOrder_KTJB03"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a href="KTJB03.aspx?New=True">Master Management</a> > Section Order</div>
        <br /><div class="font_header">SEARCH SECTION ORDER</div> 
        <br />
        <table class="table_field">
            <tr >
                <td class="table_field_td" style="width:100px;">Section Order</td>
                <td class="table_field_td" style="width:370px;">
                    <asp:TextBox ID="txtJobOrderFrom" runat="server" Width="100px" MaxLength="6"></asp:TextBox>   
                    &nbsp;-&nbsp;
                      <asp:TextBox ID="txtJobOrderTo" runat="server" Width="100px" MaxLength="6"></asp:TextBox>         
                </td>
            </tr>
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>&nbsp;&nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button_style"/>
                </td>
            </tr>         
        </table> 
        <table class="table_inquiry" width="500">
            <tr class="table_head">
                <td class="tb_Fix50">Edit</td>
                <td class="tb_Fix50">Delete</td>
                <td class="tb_Fix100">Section Order</td>
                <td class="tb_Fix250">Remarks</td>
            </tr>            
            <tbody>
            <asp:Repeater ID="rptSpJobOrder" runat="server" >
                <itemtemplate>
                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>' >
                        <td class="td_edit tb_Fix50"><asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></td>
                        <td class="td_delete tb_Fix50"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                        <td class="text_center tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                        <td class="text_left tb_Fix250"><div><%#DataBinder.Eval(Container.DataItem, "remark")%> </div></td>
                     </tr>
                </itemtemplate>
            </asp:Repeater> 
            </tbody>           
            <tr class="table_head">
                <td colspan = "4">
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

