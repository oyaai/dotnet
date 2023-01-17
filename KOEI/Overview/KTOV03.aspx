<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTOV03.aspx.vb" Inherits="Overview_KTOV03" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="text_center">
                <br />
                <div class="font_size_12 text_left"><a class="fix_link" href="KTOV03.aspx">Task List</a> > Task</div>
                <br />
                <div class="font_header">SEARCH TASK</div> 
                <br />
                <table class="table_field" width="650">
                    <tr>
                        <td class="table_field_td tb_Fix100">Task</td>
                        <td class="table_field_td tb_Fix250">
                            <asp:DropDownList ID="ddlSearchTask" runat="server" CssClass="dropdown_field">                        
                            </asp:DropDownList>
                            &nbsp;
                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="button_style"/>
                        </td>
                    </tr>
                </table> 
                <br /><br />
                <asp:Panel ID="Panel1" runat="server" Visible="True">
                <table class="table_inquiry" width="650">
                    <tr class="table_head">
                        <td class="tb_Fix100">Schedule</td>
                        <td class="tb_Fix100">Task</td>
                        <td class="tb_Fix300">Note</td>
                        <td class="tb_Fix100">Reference</td>
                        <td class="tb_Fix50">To do</td>
                    </tr>
                    <asp:Repeater ID="rptTask" OnItemCommand="rptTask_ItemCommand" runat="server">
                        <itemtemplate>
                            <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "schedule")%></div></td>
                                <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "task")%></div></td>
                                <td class="text_left tb_Fix300"><div><%#DataBinder.Eval(Container.DataItem, "note")%></div></td>
                                <td class="tb_Fix100"><asp:LinkButton ID="btnRef" CausesValidation="false" CommandName="Ref" CommandArgument='<%#String.Format("{0}",Eval("refpage"))%>' CssClass="icon_detail1 icon_center35" runat="server"></asp:LinkButton></td>
                                <td class="tb_Fix50"><asp:LinkButton ID="btnTodo" CausesValidation="false" CommandName="Todo" CommandArgument='<%#String.Format("{0}",Eval("tskpage"))%>' CssClass="icon_delivery1 icon_center15" runat="server"></asp:LinkButton></td>
                            </tr> 
                        </itemtemplate>
                    </asp:Repeater>            
                    <tr class="table_head">
                        <td colspan = "5">
                            <div class="float_l"><asp:Label ID="lblFootTB1" runat="server" Text="&nbsp;" ></asp:Label></div>
                            <div class="float_r"><asp:Label ID="lblFootTB2" runat="server" Text="&nbsp;" ></asp:Label></div>
                        </td>
                    </tr>   
                </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

