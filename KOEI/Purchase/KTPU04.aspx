<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTPU04.aspx.vb" Inherits="KTPU04"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<script language="javascript"  type="text/javascript">
function showpopup(url,frame)
{
    var width = parseInt(screen.availWidth*0.70);
    var height = parseInt(screen.availHeight*0.60);
    var left = parseInt((screen.availWidth/2) - (width/2));
    var top = parseInt((screen.availHeight/2) - (height/2));
    var windowFeatures = "width=" + width + ",height=" + height + ",left=" + left + ",top=" + top + ",resizable,scrollbars=1";
    newwin = window.open (url, frame, windowFeatures);
    if (window.focus) {newwin.focus()}
    return false;
}
</script>
        <br />
        <br /><div class="font_size_12 text_left"><a href="KTPU03.aspx">Purchase</a> > Invoice Management</div>
        <br /> 
        <br />
        <div class="font_header">INVOICE MANAGEMENT</div> 
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <table class="table_field" style="width:940px;">
            <tr>
                <td class="table_field_td tb_Fix170">Type of Purchase</td>
                <td class="table_field_td tb_Fix300">
                        <asp:RadioButtonList ID="rblSearchType" runat="server" 
                            RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Text="All&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Value="" Selected="True" />
                            <asp:ListItem Text="Purchase&nbsp;" Value="0" />
                            <asp:ListItem Text="Outsource" Value="1" />
                        </asp:RadioButtonList>
                </td>
                <td class="table_field_td tb_Fix170">PO No.</td>
                <td class="table_field_td tb_Fix300">
                        <asp:TextBox ID="txtPOFrom" runat="server" Width="95px" MaxLength="20" ></asp:TextBox>
                        &nbsp;-&nbsp;
                        <asp:TextBox ID="txtPOTo" runat="server" Width="95px" MaxLength="20" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Issue&nbsp; Date (dd/mm/yyyy)</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtIssueDateFrom" runat="server" Width="95px" 
                        MaxLength="10"></asp:TextBox>
                    <asp:CalendarExtender ID="txtIssueDateFrom_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                        TargetControlID="txtIssueDateFrom">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="txtIssueDateFrom_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtIssueDateFrom" 
                         ValidChars="1234567890/" />
                         &nbsp;-&nbsp;
                         <asp:TextBox ID="txtIssueDateTo" runat="server" Width="95px" 
                        MaxLength="10"></asp:TextBox>
                    <asp:CalendarExtender ID="txtIssueDateTo_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                        TargetControlID="txtIssueDateTo">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="txtIssueDateTo_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtIssueDateTo" 
                         ValidChars="1234567890/" />
                </td>
                <td class="table_field_td">Delivery Plan (dd/mm/yyyy)</td>
                <td class="table_field_td">
                        <asp:TextBox ID="txtDeliveryDateFrom" runat="server" Width="95px" 
                            MaxLength="10" ></asp:TextBox>
                        <asp:CalendarExtender ID="txtDeliveryDateFrom_CalendarExtender" runat="server"
                            Format="dd/MM/yyyy" TargetControlID="txtDeliveryDateFrom">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="txtDeliveryDateFrom_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtDeliveryDateFrom" 
                         ValidChars="1234567890/" />
                         &nbsp;-&nbsp;
                         <asp:TextBox ID="txtDeliveryDateTo" runat="server" Width="95px" 
                            MaxLength="10"></asp:TextBox>
                        <asp:CalendarExtender ID="txtDeliveryDateTo_CalendarExtender" runat="server"
                            Format="dd/MM/yyyy" TargetControlID="txtDeliveryDateTo">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="txtDeliveryDateTo_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtDeliveryDateTo" 
                         ValidChars="1234567890/" />                     
                        
                </td>
            </tr>  
            <tr>
                <td class="table_field_td">Vendor Name</td>
                <td class="table_field_td">
                        <asp:TextBox ID="txtVendor_Name" runat="server" Width="290px"  MaxLength="150"></asp:TextBox>
                </td>
                <td class="table_field_td">&nbsp;</td>
                <td class="table_field_td">
                    &nbsp;</td>
            </tr>  
            <tr>
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>&nbsp;&nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="button_style"/>&nbsp;&nbsp;
                </td>
            </tr>         
        </table> 

        <br /><br />
        <asp:Panel ID="Panel2" runat="server" >
            <table class="table_inquiry" width="1000px">
                <tr class="table_head">
                    <td style="width:40px;">Delivery</td>
                    <td style="width:100px;">Type of Purchase</td>
                    <td style="width:180px;">PO No.</td>
                    <td style="width:300px;">Vendor Name</td>
                    <td style="width:100px;">Issue Date</td>
                    <td style="width:100px;">Delivery Plan</td>
                    <td style="width:100px;">Amount</td>
                    <td style="width:60px;">Detail</td>
                </tr>
                <tbody>
                <asp:Repeater ID="rptInquery" runat="server">
                    <itemtemplate>                        
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="td_edit">
                                <asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_delivery1 icon_center15" runat="server"></asp:LinkButton>
                            </td>
                            <td class="text_center"><div><%#DataBinder.Eval(Container.DataItem, "po_type")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "po_no")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "vendor_name")%></div></td>
                            <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "issue_date")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "delivery_date")%></div></td>
                            <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "sub_total")%></div></td>
                            <td class="text_center">
                                <asp:LinkButton ID="btnDetail" CommandName="Detail" CssClass="icon_detail1 icon_center15" runat="server"></asp:LinkButton>
                            </td>
                        </tr>
                    </itemtemplate>
                </asp:Repeater> 
                </tbody>           
                <tr class="table_head">
                    <td colspan = "12">
                        <div class="float_l">
                            <asp:Label ID="lblDescription" runat="server"></asp:Label>
					    </div>
					    <div class="float_r">
                            <asp:Label ID="lblPaging" runat="server" Text="&nbsp;"></asp:Label>
                        </div>
                    </td>
                </tr> 
                  
            </table>        
        </asp:Panel>
</asp:Content>

