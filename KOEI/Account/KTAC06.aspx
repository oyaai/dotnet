<%@ Page  Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTAC06.aspx.vb" Inherits="KTAC06" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <link type="text/css" rel="Stylesheet" href="../Scripts/themes/base/jquery.ui.all.css" />
    <script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../Scripts/jsCommon.js"></script>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <div class="text_center">
        <br />
        <br /><div class="font_size_12 text_left"><a href="KTAC01.aspx?New=True">Accounting</a> > Cost Table Detailed</div>
        <br /><div class="font_header">COST TABLE DETAILED</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td">
                    Date (dd/mm/yyyy)</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtStartDate" runat="server" Width="100px" MaxLength="10"></asp:TextBox><asp:CalendarExtender
                        ID="CalendarExtenderStartDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDate" />&nbsp; - &nbsp;<asp:TextBox ID="txtEndDate" runat="server" Width="100px" MaxLength="10"></asp:TextBox><asp:CalendarExtender
                        ID="CalendarExtenderEndDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndDate" />
                        
                        <asp:FilteredTextBoxExtender ID="txtStartDate_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtStartDate" 
                         ValidChars="1234567890/" />
                         <asp:FilteredTextBoxExtender ID="txtEndDate_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtEndDate" 
                         ValidChars="1234567890/" />
                </td>
                <td class="table_field_td">
                    Account Title</td>
                <td class="table_field_td">
                    <asp:TextBox id="txtStartIE_Code" runat="server" MaxLength="20" Width="100px" AutoPostBack="true"  autocomplete="off" />
                    <ajaxToolkit:AutoCompleteExtender 
                            runat="server" 
                            ID="autoComplete1" 
                            BehaviorID="AutoComplete1"
                            TargetControlID="txtStartIE_Code"
                            ServiceMethod="GetStartIE"
                            MinimumPrefixLength="0" 
                            CompletionInterval="1000"
                            EnableCaching="true"
                            CompletionSetCount="10"
                            CompletionListCssClass="autocomplete_completionListElement" 
                            CompletionListItemCssClass="autocomplete_listItem" 
                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            DelimiterCharacters=""
                            ShowOnlyCurrentWordInCompletionListItem="true" >
                            <Animations>
                                <OnShow>
                                    <Sequence>
                                        <OpacityAction Opacity="0" />
                                        <HideAction Visible="true" />
                                        <ScriptAction Script="
                                                // Cache the size and setup the initial size
                                                var behavior = $find('autoComplete1');
                                                if (!behavior._height) {
                                                    var target = behavior.get_completionList();
                                                    behavior._height = target.offsetHeight - 2;
                                                    target.style.height = '0px';
                                                }" />            
                                            <%-- Expand from 0px to the appropriate size while fading in --%>
                                            <Parallel Duration=".4">
                                                <FadeIn />
                                                <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoComplete1')._height" />
                                            </Parallel>
                                    </Sequence>   
                                </OnShow>
                                <OnHide>
                                    <%-- Collapse down to 0px and fade out --%>
                                    <Parallel Duration=".4">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('AutoComplete1')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                            </Animations>
                    </ajaxToolkit:AutoCompleteExtender>
                    &nbsp; - &nbsp;
                    <asp:TextBox id="txtEndIE_Code" runat="server" MaxLength="20" Width="100px" AutoPostBack="true" />
                    <ajaxToolkit:AutoCompleteExtender 
                            runat="server" 
                            ID="autoComplete2" 
                            BehaviorID="AutoComplete2"
                            TargetControlID="txtEndIE_Code"
                            ServiceMethod="getEndIE"
                            MinimumPrefixLength="0" 
                            CompletionInterval="1000"
                            EnableCaching="true"
                            CompletionSetCount="10"
                            CompletionListCssClass="autocomplete_completionListElement" 
                            CompletionListItemCssClass="autocomplete_listItem" 
                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            DelimiterCharacters=""
                            ShowOnlyCurrentWordInCompletionListItem="true" >
                            <Animations>
                                <OnShow>
                                    <Sequence>
                                        <OpacityAction Opacity="0" />
                                        <HideAction Visible="true" />
                                        <ScriptAction Script="
                                                // Cache the size and setup the initial size
                                                var behavior = $find('autoComplete2');
                                                if (!behavior._height) {
                                                    var target = behavior.get_completionList();
                                                    behavior._height = target.offsetHeight - 2;
                                                    target.style.height = '0px';
                                                }" />            
                                            <%-- Expand from 0px to the appropriate size while fading in --%>
                                            <Parallel Duration=".4">
                                                <FadeIn />
                                                <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoComplete2')._height" />
                                            </Parallel>
                                    </Sequence>   
                                </OnShow>
                                <OnHide>
                                    <%-- Collapse down to 0px and fade out --%>
                                    <Parallel Duration=".4">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('AutoComplete2')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                            </Animations>
                    </ajaxToolkit:AutoCompleteExtender>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    Job Order</td>
                <td class="table_field_td">
                    <asp:TextBox id="txtStartJobOrder" runat="server" MaxLength="6" Width="100px" />&nbsp; - &nbsp;
                    <asp:TextBox id="txtEndJobOrder" runat="server" MaxLength="6" Width="100px" />
                </td>
                <td class="table_field_td">
                    Company Name</td>
                <td class="table_field_td">
                    <asp:TextBox id="txtVendor_name" runat="server" MaxLength="100" Width="180px" />
                </td>
            </tr>
            <tr>
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style" />&nbsp;&nbsp;
                    <asp:Button ID="btnPdf" runat="server" Text="PDF" CssClass="button_style" />                   
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" >
        <br/>
            <table class="table_inquiry" border = "0" width="960">
                <tr class="table_head">
                    <td width="80px">Job Order</td>
                    <td width="230px">Account Title</td>
                    <td width="70px">Date</td>
                    <td width="110px">Type</td>
                    <td width="150px">Company Name</td>
                    <td width="80px">Amount</td>
                    <td width="160px">Remarks</td> 
                </tr>
                <tbody>
                <asp:Repeater ID="rptInquery" runat="server">
                    <itemtemplate>                        
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "Ie_name")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "account_date")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "account_type")%></div></td>
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "vendor_name")%></div></td>
                            <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "expense")%></div></td>                            
                            <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "remark")%></div></td>
                         </tr>
                    </itemtemplate>
                </asp:Repeater> 
                </tbody>           
                <tr class="table_head">
                    <td colspan = "9">
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
</div>
</asp:Content>



