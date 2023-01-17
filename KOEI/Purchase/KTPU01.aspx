<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTPU01.aspx.vb" Inherits="Purchase_KTPU01" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<script language="javascript"  type="text/javascript">
function showpopup(url,frame)
{
    var width = parseInt(screen.availWidth*0.85);
    var height = parseInt(screen.availHeight*0.75);
    var left = parseInt((screen.availWidth/2) - (width/2));
    var top = parseInt((screen.availHeight/2) - (height/2));
    var windowFeatures = "width=" + width + ",height=" + height + ",left=" + left + ",top=" + top + ",resizable,scrollbars=1";
    newwin = window.open (url, frame, windowFeatures);
    if (window.focus) {newwin.focus()}
    return false;
}
</script>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" >
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnAdd" />
        </Triggers>
        <ContentTemplate>
            <div class="text_center">
                <br />
                <div class="font_size_12 text_left"><a class="fix_link" href="KTPU01.aspx">Purchase</a> > Purchase Order</div>
                <br />
                <div class="font_header">SEARCH PURCHASE ORDER</div> 
                <br />
                <table class="table_field" width="750">
                    <tr>
                        <td class="table_field_td tb_Fix100">Type of Purchase</td>
                        <td class="table_field_td tb_Fix250">
                            <asp:RadioButtonList ID="rblPoType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="All" Value="-1" Selected="True" />
                                <asp:ListItem Text="Purchase" Value="0" />
                                <asp:ListItem Text="Outsource" Value="1" />
                            </asp:RadioButtonList>
                        </td>
                        <td class="table_field_td tb_Fix120">PO No.</td>
                        <td class="table_field_td tb_Fix250">
                            <asp:TextBox ID="txtPONoFrom" runat="server" Width="95px" MaxLength="20"></asp:TextBox>
                            &nbsp;-
                            <asp:TextBox ID="txtPONoTo" runat="server" Width="95px" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_field_td tb_Fix100">Issue Date (dd/mm/yyyy)</td>
                        <td class="table_field_td tb_Fix250">
                            <asp:TextBox ID="txtIssueDateFrom" runat="server" Width="95px" MaxLength="10"></asp:TextBox>
                            <asp:CalendarExtender ID="txtIssueDateFrom_CalendarExtender" runat="server" 
                                Format="dd/MM/yyyy" TargetControlID="txtIssueDateFrom">
                            </asp:CalendarExtender>
                            &nbsp;-&nbsp;
                            <asp:TextBox ID="txtIssueDateTo" runat="server" Width="95px" MaxLength="10"></asp:TextBox>
                            <asp:CalendarExtender ID="txtIssueDateTo_CalendarExtender" runat="server" 
                                Format="dd/MM/yyyy" TargetControlID="txtIssueDateTo">
                            </asp:CalendarExtender>
                        </td>
                        <td class="table_field_td tb_Fix120">Delivery Plan (dd/mm/yyyy)</td>
                        <td class="table_field_td tb_Fix250">
                            <asp:TextBox ID="txtDeliveryDateFrom" runat="server" Width="95px" MaxLength="10"></asp:TextBox>
                            <asp:CalendarExtender ID="txtDeliveryDateFrom_CalendarExtender" runat="server"
                                Format="dd/MM/yyyy" TargetControlID="txtDeliveryDateFrom">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="txtDeliveryDateFrom_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="txtDeliveryDateFrom" 
                                ValidChars="1234567890/">
                            </asp:FilteredTextBoxExtender>
                            &nbsp;-
                            <asp:TextBox ID="txtDeliveryDateTo" runat="server" Width="95px" MaxLength="10"></asp:TextBox>
                            <asp:CalendarExtender ID="txtDeliveryDateTo_CalendarExtender" runat="server"
                                Format="dd/MM/yyyy" TargetControlID="txtDeliveryDateTo">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="txtDeliveryDateTo_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="txtDeliveryDateTo" 
                                ValidChars="1234567890/">
                            </asp:FilteredTextBoxExtender>
                            <asp:CustomValidator ID="custxttxtDeliveryDateFrom" runat="server" 
                                ControlToValidate="txtDeliveryDateFrom" Display="None" 
                                ErrorMessage="Delivery Date (From) is invalid format" ValidationGroup="Search">
                            </asp:CustomValidator>
                            <asp:CustomValidator ID="custxtDeliveryDateTo" runat="server" 
                                ControlToValidate="txtDeliveryDateTo" Display="None" 
                                ErrorMessage="Delivery Date (To) is invalid format" ValidationGroup="Search">
                            </asp:CustomValidator>
                        </td>
                    </tr>  
                    <tr>
                        <td class="table_field_td tb_Fix100">Vendor Name</td>
                        <td class="table_field_td tb_Fix250">
                            <asp:TextBox ID="txtVendor" runat="server" Width="250px" ></asp:TextBox>
                        </td>
                        <td colspan="2" class="table_field_td tb_Fix370"></td>
                    </tr>  
                    <tr>
                        <td colspan="4" class="text_right">
                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="button_style"/>&nbsp;
                            <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button_style"/>&nbsp;
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button_style"/>
                            
                        </td>
                    </tr>         
                </table> 
                <br /><br />
                <asp:Panel ID="Panel1" runat="server" Visible="True">
                <table class="table_inquiry" width="1050">
                    <tr class="table_head">
                        <td class="tb_Fix50">Edit</td>
                        <td class="tb_Fix50">Delete</td>
                        <td class="tb_Fix80">Type of Purchase</td>
                        <td class="tb_Fix150">PO No.</td>
                        <td style="width:250px;">Vendor Name</td>
                        <td class="tb_Fix100">Issue Date</td>
                        <td class="tb_Fix100">Delivery Plan</td>
                        <td width="80px">Currency</td>
                        <td class="tb_Fix100">Amount</td>
                        <td class="tb_Fix80">Status</td>
                        <td class="tb_Fix50">Details</td>
                    </tr>
                    <asp:Repeater ID="rptPurchase" OnItemCommand="rptPurchase_ItemCommand" runat="server">
                        <itemtemplate>
                            <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                <td class="tb_Fix50">
                                <asp:LinkButton ID="btnEdit" CausesValidation="false" CommandName="Edit" CommandArgument='<%#String.Format("{0},{1},{2}",Eval("id"),Eval("status_id"),Eval("po_type_text"))%>' CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton>
                                <asp:LinkButton ID="btnModify" CausesValidation="false" CommandName="Modify" CommandArgument='<%#String.Format("{0},{1},{2}",Eval("id"),Eval("status_id"),Eval("po_type_text"))%>' CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton>
                                </td>
                                <td class="tb_Fix50"><asp:LinkButton ID="btnDel" CausesValidation="false" CommandName="Del" CommandArgument='<%#String.Format("{0},{1},{2}",Eval("id"),Eval("status_id"),Eval("po_type_text"))%>' CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                                <td class="text_left tb_Fix80"><div><asp:Label ID="lblPo_type_text" runat="server"><%#DataBinder.Eval(Container.DataItem, "po_type_text")%> </asp:Label></div></td>
                                <td class="text_left tb_Fix150"><div><asp:Label ID="lblPo_no" runat="server"><%#DataBinder.Eval(Container.DataItem, "po_no")%></asp:Label></div></td>
                                <td style="width:250px;" class="text_left"><div><asp:Label ID="lblVendor_name" runat="server"><%#DataBinder.Eval(Container.DataItem, "vendor_name")%></asp:Label></div></td>
                                <td class="text_center tb_Fix100"><div><asp:Label ID="lblIssue_date" runat="server"><%#DataBinder.Eval(Container.DataItem, "issue_date")%></asp:Label></div></td>
                                <td class="text_center tb_Fix100"><div><asp:Label ID="lblDelivery_date" runat="server"><%#DataBinder.Eval(Container.DataItem, "delivery_date")%></asp:Label></div></td>
                                <td class="text_center tb_Fix50"><div><asp:Label ID="lblCurrency" runat="server"><%#DataBinder.Eval(Container.DataItem, "currency")%></asp:Label></div></td>
                                <td class="text_right tb_Fix100"><div><asp:Label ID="lblSub_total" runat="server"><%#DataBinder.Eval(Container.DataItem, "sub_total")%></asp:Label></div></td>
                                <td class="text_center tb_Fix80"><div><asp:Label ID="lblStatus" runat="server"><%#DataBinder.Eval(Container.DataItem, "status")%></asp:Label></div></td>
                                <td class="tb_Fix50"><asp:LinkButton ID="btnDetails" CausesValidation="false" CommandName="View" CommandArgument='<%#String.Format("{0},{1},{2}",Eval("id"),Eval("status_id"),Eval("po_type_text"))%>' CssClass="icon_detail1 icon_center15" runat="server"></asp:LinkButton></td>
                            </tr> 
                        </itemtemplate>
                    </asp:Repeater>            
                    
                    <tr class="table_head">
                        <td colspan = "11">
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



