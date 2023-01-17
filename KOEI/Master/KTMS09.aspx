<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS09.aspx.vb" Inherits="Master_KTMS09" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />


    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTMS09.aspx?New=True">Master Management</a> > W/T</div>
        <br /><div class="font_header">SEARCH W/T</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix120">W/T (%)</td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtWT" runat="server" Width="290px" MaxLength="3" CssClass="text_field"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtWTFilteredTextBoxExtender" runat="server" Enabled="True"
                        TargetControlID="txtWT" FilterType="Numbers" />
                    <asp:RangeValidator ID="txtWTRangeValidator" runat="server" ControlToValidate="txtWT"
                        ErrorMessage="" MaximumValue="100" MinimumValue="1" Type="Integer" SetFocusOnError="True" />
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
    
        <table class="table_inquiry" width="550">
            <tr class="table_head">
                <%--<td colspan="2">Action</td>--%>
                <td class="tb_Fix50">Edit</td>
                <td class="tb_Fix50">Delete</td>
                <td class="tb_Fix50">ID</td>
                <td class="tb_Fix200">W/T (%)</td>
                <td class="tb_Fix200">Type</td>
            </tr>

            <asp:Repeater ID="rptInquery" runat="server">
                <itemtemplate>
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="tb_Fix50"><div class="ctb_Fix50"><asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></div></td>
                            <td class="tb_Fix50"><div class="ctb_Fix50"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></div> </td>
                            <td class="tb_Fix50"><div class="ctb_Fix50"><%#DataBinder.Eval(Container.DataItem, "id")%></div></td>
                            <td class="tb_Fix200"><div class="ctb_Fix200 text_left"><%#DataBinder.Eval(Container.DataItem, "percent")%></div></td>
                            <td class="tb_Fix200"><div class="ctb_Fix200 text_left"><%#DataBinder.Eval(Container.DataItem, "type")%></td>
                        </tr>
                </itemtemplate>
            </asp:Repeater>       
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

