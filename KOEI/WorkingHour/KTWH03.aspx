<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="KTWH03.aspx.vb" Inherits="WorkingHour_KTWH03" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTWH03.aspx?New=True">Working Hour Menagement </a> > Working Hour Report </div>
        <br /><div class="font_header">WORKING HOUR REPORT</div> 
        <br />
        <table class="table_field" style="width:500px">            
            <tr>
                <td class="table_field_td" style="width:110px;">
                    Period (dd/mm/yyyy)
                </td>
                <td class="table_field_td" style="width:300px;" >
                    <asp:TextBox ID="txtStartDate" runat="server" Width="95px" MaxLength="10"></asp:TextBox><asp:CalendarExtender
                    ID="CalendarExtenderStartDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDate" />
                    <asp:FilteredTextBoxExtender ID="txtStartDate_FilteredTextBoxExtender" 
                     runat="server" Enabled="True" TargetControlID="txtStartDate" 
                     ValidChars="1234567890/" >
                 </asp:FilteredTextBoxExtender>
                &nbsp;-&nbsp;<asp:TextBox ID="txtEndDate" runat="server" Width="95px" MaxLength="10"></asp:TextBox><asp:CalendarExtender
                    ID="CalendarExtenderEndDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndDate" />
                    <asp:FilteredTextBoxExtender ID="txtEndDate_FilteredTextBoxExtender" 
                     runat="server" Enabled="True" TargetControlID="txtEndDate" 
                     ValidChars="1234567890/" >
                 </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td class="table_field_td" style="width:110px;">
                    Job Status
                </td>
                <td class="table_field_td" style="width:300px;" >
                    <asp:RadioButtonList ID="radStatus" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="All" Value="" ></asp:ListItem>
                        <asp:ListItem Text="Finish" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Not Finish" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>                        
                </td>
            </tr>
            <tr>
                <td colspan="2" class="text_center">
                    <asp:Button ID="btnReport" runat="server" Text="PDF" CssClass="button_style" />
                </td>
            </tr>
            
        </table>
    </div>
</asp:Content>
