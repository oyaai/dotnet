<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS11.aspx.vb" Inherits="Master_KTMS11" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS11.aspx?New=True">Master Management</a> > Unit</div>
        <br />
        <div class="font_header">SEARCH UNIT</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix100">Unit</td>
                <td class="table_field_td tb_Fix150">
                     <asp:TextBox ID="txtUnit" runat="server" CssClass="text_field" MaxLength="50"></asp:TextBox>
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
        <table class="table_inquiry" width="400">
            <tr class="table_head">
                <td class="tb_Fix50">Edit</td>
                <td class="tb_Fix50">Delete</td>
                <td class="tb_Fix80">ID</td>
                <td class="tb_Fix220">Unit</td>
            </tr>
            <asp:Repeater ID="rptUnit" OnItemCommand="rptUnit_ItemCommand" 
                runat="server">
                <itemtemplate>
                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                        <td class="tb_Fix50"><asp:LinkButton ID="btnEdit" CausesValidation="false" CommandName="Edit" CommandArgument='<%#String.Format("{0},{1}",Eval("id"),Eval("name"))%>' CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></td>
                        <td class="tb_Fix50"><asp:LinkButton ID="btnDel" CausesValidation="false" CommandName="Del" CommandArgument='<%#String.Format("{0},{1}",Eval("id"),Eval("name"))%>' CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                        <td class="text_center tb_Fix80"><div><%#DataBinder.Eval(Container.DataItem, "id")%></div></td>
                        <td class="text_left tb_Fix220"><div><%#DataBinder.Eval(Container.DataItem, "name")%><div></td>
                    </tr> 
                </itemtemplate>
            </asp:Repeater>            
            <tr class="table_head">
                <td colspan = "4">
                    <div class="float_l"><asp:Label ID="lblFootTB1" runat="server" Text="0" ></asp:Label></div>
                    <div class="float_r"><asp:Label ID="lblFootTB2" runat="server" Text="0" ></asp:Label></div>
                </td>
            </tr>   
        </table>
    </div>
</asp:Content>

