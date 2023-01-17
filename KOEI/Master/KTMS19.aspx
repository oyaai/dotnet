<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS19.aspx.vb" Inherits="Master_KTMS19" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS19.aspx?New=True">Master Management</a> > Working Category</div>
        <br />
        <div class="font_header">SEARCH WORKING CATEGORY</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix120">Working Category</td>
                 <td class="table_field_td tb_Fix600">
                     <asp:TextBox ID="txtWorkCategoty" runat="server" Width="116px" MaxLength="50" ></asp:TextBox>
                    
                </td>
            </tr>            
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>&nbsp;&nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button_style"/>
                </td>
            </tr>         

        </table> 
        <br /><br />
        
        <table class="table_inquiry" border="0" width="400">
            <tr class="table_head">
                <td class="tb_Fix50">Edit</td>
                <td class="tb_Fix50">Delete</td>
                <td class="tb_Fix80">ID</td>
                <td class="tb_Fix220">Working Category</td>
            </tr>
            <tbody>
            <asp:Repeater ID="rptInquery" runat="server">
                <itemtemplate>                    
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="tb_Fix50"><asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></td>
                            <td class="tb_Fix50"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                            <td class="text_center tb_Fix80"><div><%#DataBinder.Eval(Container.DataItem, "item_id")%></div></td>
                            <td class="text_left tb_Fix220"><div><%#DataBinder.Eval(Container.DataItem, "item_name")%></div></td>
                        </tr>
                    
                </itemtemplate>
            </asp:Repeater> 
            </tbody>           
            <tr class="table_head">
                <td colspan = "5">
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

