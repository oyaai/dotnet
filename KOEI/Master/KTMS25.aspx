<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS25.aspx.vb" Inherits="Master_KTMS25" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS25.aspx?New=True">Master Management</a> >Payment Condition</div>
        <br />
        <div class="font_header">SEARCH PAYMENT CONDITION</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix100">&nbsp;&nbsp;1st</td>
                <td class="table_field_td tb_Fix500">
                    <asp:TextBox ID="txtFirst" runat="server" Width="150px" MaxLength="3" ></asp:TextBox>&nbsp;%                
                    <asp:RangeValidator ID="RangeValidator1st" runat="server" 
                        ControlToValidate="txtFirst" ErrorMessage="" MaximumValue="100" 
                        MinimumValue="0" Type="Integer"></asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix100">&nbsp;&nbsp;2nd</td>
                <td class="table_field_td tb_Fix500">
                    <asp:TextBox ID="txtSecond" runat="server" Width="150px" MaxLength="3"></asp:TextBox>&nbsp;%                  
                    <asp:RangeValidator ID="RangeValidator2nd" runat="server" 
                        ControlToValidate="txtSecond" ErrorMessage="" MaximumValue="100" 
                        MinimumValue="0" Type="Integer"></asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix100">&nbsp;&nbsp;3rd</td>
                <td class="table_field_td tb_Fix500">
                    <asp:TextBox ID="txtThird" runat="server" Width="150px" MaxLength="3"></asp:TextBox>&nbsp;%                
                    <asp:RangeValidator ID="RangeValidator3rd" runat="server" 
                        ControlToValidate="txtThird" ErrorMessage="" MaximumValue="100" 
                        MinimumValue="0" Type="Integer"></asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>
                    &nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button_style"/>
                </td>
            </tr>         
        </table> <br />
        <table class="table_inquiry">
            <tr class="table_head">
                <td class="tb_Fix50 text_center">Edit</td>
                <td class="tb_Fix50 text_center">Delete</td>
                <td class="tb_Fix50 text_center">ID</td>
                <td class="tb_Fix300 text_center">Payment Condition</td>
            </tr>
            <asp:Repeater ID="rptPayCondition" runat="server">
                <itemtemplate>
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="tb_Fix50"><div class="ctb_Fix50"><asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></div></td>
                            <td class="tb_Fix50"><div class="ctb_Fix50"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></div> </td>
                            <td class="tb_Fix50"><div class="ctb_Fix50"><%#DataBinder.Eval(Container.DataItem, "PayID")%></div></td>
                            <td class="tb_Fix300"><div class="ctb_Fix300 text_left"><%#DataBinder.Eval(Container.DataItem, "First") & "%" & DataBinder.Eval(Container.DataItem, "Second") & "%" & DataBinder.Eval(Container.DataItem, "Third") & "%"%></div></td>
                        </tr>
                </itemtemplate>
            </asp:Repeater>            
            <tr class="table_head">
                <td colspan = "5" class="text_right">
                    <div class="float_l"><asp:Label ID="lblDescription" runat="server"></asp:Label></div>
					<div class="float_r"><asp:Label ID="lblPaging" runat="server" Text="&nbsp;"></asp:Label></div>
                </td>
            </tr>   
        </table>
    </div>
</asp:Content>

