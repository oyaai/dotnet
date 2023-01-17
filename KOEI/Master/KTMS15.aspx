<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" 
CodeFile="KTMS15.aspx.vb" Inherits="Master_KTMS15" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTMS15.aspx?New=True">Master Management</a> > Job Order Type</div>
        <br /><div class="font_header">SEARCH JOB ORDER TYPE</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix120">Job Order Type</td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtJobType" runat="server" MaxLength="50" CssClass="text_field"></asp:TextBox>
                </td>
            </tr>
            
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>
                    &nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button_style"/>
                </td>
            </tr>         
        </table> 
    
        <table class="table_inquiry" width="400">
            <tr class="table_head">
                <%--<td colspan="2">Action</td>--%>
                <td class="tb_Fix50">Edit</td>
                <td class="tb_Fix50">Delete</td>
                <td class="tb_Fix50">ID</td>
                <td class="tb_Fix250">Job Order Type</td>
                
            </tr>

            <asp:Repeater ID="rptInquery" runat="server">
                <itemtemplate>
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="tb_Fix50"><div class="ctb_Fix50"><asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></div></td>
                            <td class="tb_Fix50"><div class="ctb_Fix50"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></div> </td>
                            <td class="tb_Fix50"><div class="ctb_Fix50"><%#DataBinder.Eval(Container.DataItem, "id")%></div></td>
                            <td class="tb_Fix250"><div class="ctb_Fix250 text_left"><%#DataBinder.Eval(Container.DataItem, "job_type")%></div></td>
  
                        </tr>
                </itemtemplate>
            </asp:Repeater>       
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

