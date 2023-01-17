<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTAC07.aspx.vb" Inherits="KTAC07" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <div class="text_center">
        <br />
        <br /><div class="font_size_12 text_left"><a href="KTAC01.aspx?New=True">Accounting</a> > Advance Income and W.I.P</div>
        <br /><div class="font_header">SEARCH ADVANCE INCOME AND W.I.P</div> 
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
                    <asp:Button ID="btnExcel" runat="server" Text="EXCEL" CssClass="button_style" />                   
                </td>
            </tr>
        </table>
        </div>
</asp:Content>



