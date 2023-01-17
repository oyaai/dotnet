<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS23.aspx.vb" Inherits="Master_KTMS23" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS23.aspx?New=True">Master Management</a> > Department</div>
        <br />
        <div class="font_header">SEARCH DEPARTMENT</div> 
        <br />
        <table class="table_field" width="450px">
            <tr>
                <td class="table_field_td tb_Fix150">Department</td>
                <td class="table_field_td tb_Fix300">
                    <asp:TextBox ID="txtDepartment" runat="server" CssClass="text_field" MaxLength="100"></asp:TextBox>
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
        <br /><br />
        <table class="table_inquiry" width="450px">
            <tr class="table_head">
                <td class="tb_Fix50 text_center">Edit</td>
                <td class="tb_Fix50 text_center">Delete</td>
                <td class="tb_Fix50 text_center">ID</td>
                <td class="tb_Fix250 text_center">Name</td>
            </tr>
            <asp:Repeater ID="rptDepartment" runat="server">
                <itemtemplate>
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="tb_Fix50"><asp:LinkButton ID="btnEdit" CausesValidation="false" CommandName="Edit" CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></td> <%-- Style="width:50px;background-position:center" --%>
                            <td class="tb_Fix50"><asp:LinkButton ID="btnDel" CausesValidation="false" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td> <%-- Style="width:50px;background-position:center" --%>
                            <td class="tb_Fix50 text_center"><div><%#DataBinder.Eval(Container.DataItem, "id")%></div></td>
                            <td class="tb_Fix250 text_left"><div class="ctb_Fix250"><%#DataBinder.Eval(Container.DataItem, "name")%></div></td>
                        </tr>
                </itemtemplate>
            </asp:Repeater>            
            <tr class="table_head">
                <td colspan = "4" class="text_right">
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

