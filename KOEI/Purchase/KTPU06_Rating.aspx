<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTPU06_Rating.aspx.vb" Inherits="KTPU06_Rating"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

     <div class="text_left">
        <br />
        <br /><div class="font_size_12 text_left"><a href="KTPU05.aspx?New=True">Vendor Rating Management</a> > Vendor Rating</div>
        <br /><div class="font_header">VENDOR RATING MANAGEMENT</div> 
        <br />
     
        <table class="table_rating" width="400px" >
            <tr>
                <td class="table_head_gray" style="width:100px;" >Category</td>
                <td class="table_head_gray" style="width:250px;" >Score</td>
            </tr>
            <tr>
                <td class="table_field_td1" >Quality <span class="font_require">*</span></td>
                <td class="table_field_td" >
                    <asp:RadioButtonList ID="rblQuality" runat="server" Width="225px">
                        <asp:ListItem Text="Pass 50%" Value="50" />
                        <asp:ListItem Text="Special Accept 25%" Value="25" />
                        <asp:ListItem Text="NC 0%" Value="0" />
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rblQuality_RequiredFieldValidator" runat="server" 
                    ControlToValidate="rblQuality" ValidationGroup="btnCreate" SetFocusOnError="True" ErrorMessage="*Require"></asp:RequiredFieldValidator>
                </td>
            </tr>  
            <tr>
                <td class="table_field_td1" >Delivery <span class="font_require">*</span></td>
                <td class="table_field_td" >
                    <asp:RadioButtonList ID="rblDelivery" runat="server" Width="225px">
                        <asp:ListItem Text="OnTime 30%" Value="30"  />
                        <asp:ListItem Text="Delay but no effect PD 15%" Value="15" />
                        <asp:ListItem Text="Delay but effect PD 0%" Value="0" />
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rblDelivery_RequiredFieldValidator" runat="server" 
                    ControlToValidate="rblDelivery" ValidationGroup="btnCreate" SetFocusOnError="True" ErrorMessage="*Require"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td1" >Service <span class="font_require">*</span></td>
                <td class="table_field_td" >
                    <asp:RadioButtonList ID="rblService" runat="server" Width="225px">
                        <asp:ListItem Text="1-2 days response 20%" Value="20" />
                        <asp:ListItem Text="3-7 days response 10%" Value="10" />
                        <asp:ListItem Text="more than 7 days 0%" Value="0" />
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rblService_RequiredFieldValidator" runat="server" 
                    ControlToValidate="rblService" ValidationGroup="btnCreate" SetFocusOnError="True" ErrorMessage="*Require"></asp:RequiredFieldValidator>
                </td>
            </tr>  
            <tr>
                <td colspan="2" class="text_center">
                    <asp:Button ID="btnCreate" runat="server" Text="Create" ValidationGroup="btnCreate" CssClass="button_style"/>&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button_style"/>
                </td>
            </tr>         
        </table> 
              
    </div>
</asp:Content>

