<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTPU02.aspx.vb" Inherits="Purchase_KTPU02"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">


<script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script>
	
<script type="text/javascript" src="../Scripts/jsCommon.js"></script>
<script language="javascript"  type="text/javascript" >
    function IsExistJobOrder(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            //url: "../Account/KTAC02.aspx/IsExistJobOrder", // <-- Page Method
            url: "KTPU02.aspx/IsExistJobOrder", // <-- Page Method
            data: "{strJobOrder: '" + args.Value + "'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });
    }

    //    $(function() {
    //        ToggleTHB();
    //    });

    function ToggleTHB() {
        //$("tr.THB").hide();
        alert("In Function: " + $("select[id$='ddlCurrency'] option:selected").text());
        if ($("select[id$='ddlCurrency'] option:selected").text() != "THB") {
            alert("In Process: " + $("select[id$='ddlCurrency'] option:selected").text());
            $("tr.THB").show(); // show when true
        } else {
            alert("In Process: (Hide) " + $("select[id$='ddlCurrency'] option:selected").text());
            $("tr.THB").hide();
        }
        //alert("In Methods");
        
    }

    function CurrencyCheckedChanged(selectedItem) {
        //alert(selectedItem.value);
        var txtDiscount = document.getElementById("<%=txtDiscount.ClientID%>");

        //alert(txtDiscount);
        txtDiscount.setAttribute("value", "");
        txtDiscount.setAttribute("disabled", false);

        switch (parseInt(selectedItem.value)) {
            case 0:
                txtDiscount.setAttribute("disabled", true);
                break;
            case 1:
                txtDiscount.setAttribute("disabled", true);
                break;
            case 2:
                txtDiscount.setAttribute("disabled", false);
                break;
                
        }

    }

    function TypeCheckedChanged(selectedItem) {
        //alert(selectedItem.value);
        var ddlWT = document.getElementById("<%=ddlWT.ClientID%>");
        //alert(ddlWT.value);
        ddlWT.setAttribute("disabled", true);

        switch (parseInt(selectedItem.value)) {
            case 0:
                ddlWT.setAttribute("value", "");
                ddlWT.setAttribute("disabled", true);
                break;
            case 1:
                ddlWT.setAttribute("disabled", false);
                break;
        }
    }

    /*
    $(function() {
    // Handle change event
    $("select[id$='ddlCurrency']").change(function() {
    var thbTableRow = $("tr.THB");
    //thbTableRow.hide(); // hide by default
    if ($("option:selected", $(this)).text() != "Baht") {
    thbTableRow.show(); // show when true
    }
    });

        // And now fire change event when the DOM is ready
    $("select[id$='ddlCountry']").trigger('change');
    });
    */    

    //2013/10/2013 Pranitda S. Start-Add
    function IsExistVendorClient(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            url: "KTPU02.aspx/IsExistVendorClient", // <-- Page Method
            data: "{strVendor: '" + args.Value + "'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });
    }
    //2013/10/2013 Pranitda S. End-Add
</script>
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <div class="text_center">
                <br />
                <div class="font_size_12 text_left"><a class="fix_link" href="KTPU01.aspx">Purchase</a> > Purchase Order</div>
                <br />
                <div class="font_header">PURCHASE ORDER MANAGEMENT</div> 
                <br />

                <table width="1000" class="table_common">
                    <tr>
                        <td class="table_view_head" colspan="6">Header</td>
                    </tr>
                    <tr>
                        <td class="table_field_td">
                            <asp:Label ID="lblHeaderPo" runat="server" Text="PO No."></asp:Label>
                        </td>
                        <td class="table_field_td" colspan="5">
                            <asp:Label ID="lblPoNo" runat="server" Font-Bold="True" ForeColor="Blue" ></asp:Label>
                        </td> 
                    </tr>
                    <tr>
                        <td class="table_field_td">Type of Purchase<span class="font_require">*</span></td>
                        <td class="table_field_td" style="width:500px;">
                            <asp:RadioButtonList ID="rblPoType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" >
                                <asp:ListItem Text="Purchase" Value="0" Selected="True" />  <%--onclick="javascript:TypeCheckedChanged(this);"--%>
                                <asp:ListItem Text="Outsource" Value="1" />                 <%--onclick="javascript:TypeCheckedChanged(this);"--%>
                            </asp:RadioButtonList>
                            <asp:Label ID="lblMsgPoType" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                        </td>
                        <td class="table_field_td" style="width:150px;">Vendor Name<span class="font_require">*</span><br />/ Branch</td>
                        <td class="table_field_td" style="width:800px;">
                            <asp:TextBox ID="ddlVendor" runat="server" autocomplete="off" Width="200px" AutoPostBack="True"></asp:TextBox>
<%--
                            <asp:Label ID="lblMsgVendorName" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
--%>                            
                            <asp:CustomValidator ID="validatorVendor" runat="server" ControlToValidate="ddlVendor" ErrorMessage="* Invalid Vendor *" 
                                    ClientValidationFunction="IsExistVendorClient" />
                            <ajaxToolkit:AutoCompleteExtender
                                runat="server" 
                                ID="autoComplete1" 
                                BehaviorID="AutoComplete1"
                                TargetControlID="ddlVendor"
                                ServiceMethod="GetVendorList"
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
                                        <%-- Make the completion list transparent and then show it --%>
                                        <OpacityAction Opacity="0" />
                                        <HideAction Visible="true" />            
                                        <%--Cache the original size of the completion list the first time
                                            the animation is played and then set it to zero --%>
                                        <ScriptAction Script="
                                            // Cache the size and setup the initial size
                                            var behavior = $find('AutoComplete1');
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
                            <asp:TextBox ID="ddlVendorBranch" runat="server" autocomplete="off" Width="200px" ></asp:TextBox>
                            <ajaxToolkit:AutoCompleteExtender
                                runat="server" 
                                ID="autoComplete4" 
                                BehaviorID="AutoComplete4"
                                TargetControlID="ddlVendorBranch"
                                ServiceMethod="GetVendorBranchList"
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
                                        <%-- Make the completion list transparent and then show it --%>
                                        <OpacityAction Opacity="0" />
                                        <HideAction Visible="true" />            
                                        <%--Cache the original size of the completion list the first time
                                            the animation is played and then set it to zero --%>
                                        <ScriptAction Script="
                                            // Cache the size and setup the initial size
                                            var behavior = $find('AutoComplete4');
                                            if (!behavior._height) {
                                                var target = behavior.get_completionList();
                                                behavior._height = target.offsetHeight - 2;
                                                target.style.height = '0px';
                                            }" />            
                                        <%-- Expand from 0px to the appropriate size while fading in --%>
                                        <Parallel Duration=".4">
                                            <FadeIn />
                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoComplete4')._height" />
                                        </Parallel>
                                    </Sequence>
                                </OnShow>
                                <OnHide>
                                    <%-- Collapse down to 0px and fade out --%>
                                    <Parallel Duration=".4">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('AutoComplete4')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                                </Animations>
                            </ajaxToolkit:AutoCompleteExtender>
                        </td>
                        <td class="table_field_td">Quotation No<span class="font_require">*</span></td>
                        <td class="table_field_td" style="width:500px;">
                            <asp:TextBox ID="txtQuotationNo" MaxLength="20" runat="server" CssClass="text_field"></asp:TextBox>
                            <asp:Label ID="lblMsgQuotationNo" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_field_td">Delivery Plan<span class="font_require">*</span> (dd/mm/yyyy)</td>
                        <td class="table_field_td">
                            <asp:TextBox ID="txtDeliveryDate" MaxLength="10" runat="server" CssClass="text_field"></asp:TextBox>
                            <asp:Label ID="lblMsgDeliveryDate" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                            <asp:Label ID="lblMsgDeliveryDate1" runat="server" Text="" CssClass="font_require" Visible="false"></asp:Label>
                            <asp:CalendarExtender ID="txtDeliveryDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                TargetControlID="txtDeliveryDate">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="txtDeliveryDate_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="txtDeliveryDate" 
                                ValidChars="1234567890/">
                            </asp:FilteredTextBoxExtender>
                            <asp:CustomValidator ID="cusDeliveryDate" runat="server" 
                                ControlToValidate="txtDeliveryDate" Display="None" 
                                ErrorMessage="Delivery Date is invalid format" ValidationGroup="Purchase">
                            </asp:CustomValidator>
                        </td>
                        <td class="table_field_td">Payment Term<span class="font_require">*</span></td>
                        <td class="table_field_td">
                            <asp:DropDownList ID="ddlPayTerm" runat="server" CssClass="dropdown_mini_field" /><%-- &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; day(s) --%>
                            <asp:Label ID="lblMsgPayTerm" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                        </td>
                        <td class="table_field_td">VAT (%)</td>
                        <td class="table_field_td">
                            <asp:DropDownList ID="ddlVat" runat="server" CssClass="dropdown_mini_field" AutoPostBack="True" />
                        </td>
                    </tr>  
                    <tr>
                        <td rowspan="2" class="table_field_td">W/T (%)</td>
                        <td rowspan="2" class="table_field_td">
                            <asp:DropDownList ID="ddlWT" runat="server" CssClass="dropdown_mini_field" AutoPostBack="True" /> <%--Enabled="false"--%>
                        </td>
                        <td class="table_field_td">Currency<span class="font_require">*</span></td>
                        <td class="table_field_td">
                            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="dropdown_mini_field" OnTextChanged="CheckTypeCurrency" AutoPostBack="True" /> 
                            <asp:Label ID="lblMsgCurrency" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                        </td>
                        <td rowspan="2" class="table_field_td">Remarks</td>
                        <td rowspan="2" class="table_field_td">
                            <asp:TextBox ID="txtHeadRemark" MaxLength="255" runat="server" CssClass="text_field"></asp:TextBox>
                        </td>
                    </tr>  
                    <tr> 
                        <td class="table_field_td">Schedule Rate (THB)</td>
                        <td class="table_field_td">
                            <asp:Label ID="lblScheduleRate" runat="server" Text="0.00" Visible="true" Width="100px" ></asp:Label><br />
                            <asp:Label ID="lblMsgScheduleRate" runat="server" Text="" CssClass="font_require" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_view_head" colspan="6">Detail</td>
                    </tr>
                    <tr>
                        <td class="table_field_td">Item Name<span class="font_require">*</span></td>
                        <td class="table_field_td">
                            <%--<asp:DropDownList ID="ddlItem" runat="server" CssClass="dropdown_mini_field" />--%>
                            <asp:TextBox ID="ddlItem" runat="server" autocomplete="off" Width="250px" ></asp:TextBox>
                            <asp:Label ID="lblMsgItem" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                            <ajaxToolkit:AutoCompleteExtender
                                runat="server" 
                                ID="autoComplete2" 
                                BehaviorID="AutoComplete2"
                                TargetControlID="ddlItem"
                                ServiceMethod="GetItemList"
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
                                        <%-- Make the completion list transparent and then show it --%>
                                        <OpacityAction Opacity="0" />
                                        <HideAction Visible="true" />            
                                        <%--Cache the original size of the completion list the first time
                                            the animation is played and then set it to zero --%>
                                        <ScriptAction Script="
                                            // Cache the size and setup the initial size
                                            var behavior = $find('AutoComplete2');
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
                        <td class="table_field_td">Job Order<span class="font_require">*</span></td>
                        <td class="table_field_td">
                            <asp:TextBox ID="txtJobOrder" runat="server" MaxLength="6" CssClass="text_field"></asp:TextBox>
                            <asp:Label ID="lblMsgJobOrder" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                        
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtJobOrder" ErrorMessage="*Not exist Job Order." ClientValidationFunction="IsExistJobOrder" ValidationGroup="Detail" />
                        </td>
                        <td class="table_field_td">Account Title<span class="font_require">*</span></td>
                        <td class="table_field_td">
                            <%--<asp:DropDownList ID="ddlIE" runat="server" CssClass="dropdown_mini_field" />--%>
                            <asp:TextBox ID="ddlIE" runat="server" autocomplete="off" Width="250px" ></asp:TextBox>
                            <asp:Label ID="lblMsgIE" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                            <ajaxToolkit:AutoCompleteExtender
                                runat="server" 
                                ID="autoComplete3" 
                                BehaviorID="AutoComplete3"
                                TargetControlID="ddlIE"
                                ServiceMethod="GetIEList"
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
                                        <%-- Make the completion list transparent and then show it --%>
                                        <OpacityAction Opacity="0" />
                                        <HideAction Visible="true" />            
                                        <%--Cache the original size of the completion list the first time
                                            the animation is played and then set it to zero --%>
                                        <ScriptAction Script="
                                            // Cache the size and setup the initial size
                                            var behavior = $find('AutoComplete3');
                                            if (!behavior._height) {
                                                var target = behavior.get_completionList();
                                                behavior._height = target.offsetHeight - 2;
                                                target.style.height = '0px';
                                            }" />            
                                        <%-- Expand from 0px to the appropriate size while fading in --%>
                                        <Parallel Duration=".4">
                                            <FadeIn />
                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoComplete3')._height" />
                                        </Parallel>
                                    </Sequence>
                                </OnShow>
                                <OnHide>
                                    <%-- Collapse down to 0px and fade out --%>
                                    <Parallel Duration=".4">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('AutoComplete3')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                                </Animations>
                            </ajaxToolkit:AutoCompleteExtender>
                        </td>
                    </tr>  
                    <tr>
                        <td class="table_field_td">Unit Price<span class="font_require">*</span></td>
                        <td class="table_field_td">
                            <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="text_field" MaxLength="17"></asp:TextBox>
                            <asp:Label ID="lblMsgUnitPrice" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                            <asp:RangeValidator ID="rngUnitPrice" runat="server" ControlToValidate="txtUnitPrice"
                                Display="Dynamic" ErrorMessage="Unit Price must &gt; 0 and < 99,999,999,999,999.99" MaximumValue="99999999999999.99" 
                                Type="Double" MinimumValue="0.01" ValidationGroup="Detail">
                            </asp:RangeValidator>
                            <asp:FilteredTextBoxExtender ID="txtUnitPrice_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="txtUnitPrice" 
                                ValidChars="1234567890.">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td class="table_field_td">Qty<span class="font_require">*</span></td>
                        <td class="table_field_td">
                            <asp:TextBox ID="txtQty" runat="server" CssClass="text_field" MaxLength="10"></asp:TextBox>
                            <asp:Label ID="lblMsgQty" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                            <asp:RangeValidator ID="rngQty" runat="server" ControlToValidate="txtQty" Display="Dynamic"
                                ErrorMessage="Qty must &gt; 0 and < 9,999,999,999" MaximumValue="9999999999" MinimumValue="0"
                                Type="Double" ValidationGroup="Detail">
                            </asp:RangeValidator>
                            <asp:FilteredTextBoxExtender ID="txtQty_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="txtQty" 
                                ValidChars="1234567890">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td class="table_field_td">Unit<span class="font_require">*</span></td>
                        <td class="table_field_td">
                            <asp:DropDownList ID="ddlUnit" runat="server" CssClass="dropdown_mini_field" />
                            <asp:Label ID="lblMsgUnit" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                        </td>
                    </tr>  
                    <tr>
                        <td class="table_field_td">Discount</td>
                        <td class="table_field_td" colspan="3">
                            <asp:TextBox ID="txtDiscount" MaxLength="11" runat="server" CssClass="text_field"></asp:TextBox>
                            <asp:DropDownList ID="ddlDiscountType" runat="server" OnChange="javascript:CurrencyCheckedChanged(this);" CssClass="dropdown_mini_field" AutoPostBack="true" />  <%--AutoPostBack="true"--%>
                            <asp:Label ID="lblMsgDiscount" runat="server" Text="*Value more max length" CssClass="font_require" Visible="false"></asp:Label>
                            
                            <asp:RangeValidator ID="rngDiscount" runat="server" ControlToValidate="txtDiscount"
                                Display="None" ErrorMessage="Discount must &gt; 0." MaximumValue="99999999.99"
                                MinimumValue="0.01" ValidationGroup="Detail">
                            </asp:RangeValidator>
                            <asp:FilteredTextBoxExtender ID="txtDiscount_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="txtDiscount" 
                                ValidChars="1234567890.">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td class="table_field_td">Remarks</td>
                        <td class="table_field_td">
                            <asp:TextBox ID="txtDetailRemark" MaxLength="255" runat="server" CssClass="text_field"></asp:TextBox>
                        </td>
                    </tr> 
                    <tr>
                        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>--%>
                                <td colspan="6" class="table_field_td text_right">
                                    <asp:Button ID="btnAddDetail" OnClick="btnAddDetail_Click" runat="server" Text="Add Purchase Detail" CausesValidation="true"
                                        CssClass="button_style" Width="140px" ValidationGroup="Detail"/>&nbsp;&nbsp;
                                    <asp:Button ID="btnEditDetail" OnClick="btnEditDetail_Click" runat="server" Text="Edit Purchase Data" CausesValidation="true" Visible="false"
                                        CssClass="button_style" Width="140px" ValidationGroup="Detail"/>&nbsp;&nbsp;
                                    <asp:Button ID="btnCancelDetail" OnClick="btnCancelDetail_Click" runat="server" Text="Clear Purchase Data" Visible="true"
                                        CssClass="button_style" Width="145px"/>&nbsp;&nbsp;
                                </td>
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>                                
                    </tr> 
                    <tr>
                        <td colspan="6" class="table_field_td text_center">
                            <asp:Panel ID="Panel1" runat="server" Visible="true">
                                <table class="table_inquiry">
                                    <tr class="table_head">
                                        <td class="text_center tb_Fix50">Edit</td>
                                        <td class="text_center tb_Fix50">Delete</td>
                                        <td class="text_center tb_Fix90">Item Name</td>
                                        <td class="text_center tb_Fix90">Job Order</td>
                                        <td class="text_center tb_Fix90">I/E</td>
                                        <td class="text_center tb_Fix90">Unit Price</td>
                                        <td class="text_center tb_Fix90">Qty</td>
                                        <td class="text_center tb_Fix90">Unit</td>
                                        <td class="text_center tb_Fix90">Discount</td>
                                        <td class="text_center tb_Fix90">Discount Type</td>
                                        <td class="text_center tb_Fix90">Vat</td>
                                        <td class="text_center tb_Fix90">W/T</td>
                                        <td class="text_center tb_Fix90">Amount</td>
                                        <%--<td class="text_center">Remarks</td>--%>
                                    </tr>
                                    <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>--%>
                                            <asp:Repeater ID="rptPurchase" OnItemCommand="rptPurchase_ItemCommand" runat="server">
                                                <itemtemplate>
                                                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                                        <td class="tb_Fix50"><asp:LinkButton ID="btnEdit" CausesValidation="false" CommandName="Edit" CommandArgument='<%# Eval("id")%>' CssClass="icon_edit1 icon_left" runat="server"></asp:LinkButton></td>
                                                        <td class="tb_Fix50"><asp:LinkButton ID="btnDel" CausesValidation="false" CommandName="Del" CommandArgument='<%# Eval("id")%>' CssClass="icon_del1 icon_center9" runat="server"></asp:LinkButton></td>
                                                        <td class="text_left tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "item_name")%></div></td>
                                                        <td class="text_left tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                                                        <td class="text_left tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "ie_name")%></div></td>
                                                        <td class="text_right tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "unit_price")%></div></td>
                                                        <td class="text_right tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "quantity")%></div></td>
                                                        <td class="text_left tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "unit_name")%></div></td>
                                                        <td class="text_right tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "discount")%></div></td>
                                                        <td class="text_left tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "discount_type_text")%></div></td>
                                                        <td class="text_right tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "vat_amount")%></div></td>
                                                        <td class="text_right tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "wt_amount")%></div></td>
                                                        <td class="text_right tb_Fix90"><div><%#DataBinder.Eval(Container.DataItem, "amount")%></div></td>
                                                        <%--<td class="text_left tb_Fix"><div><%#DataBinder.Eval(Container.DataItem, "remark")%></div></td>--%>
                                                    </tr> 
                                                </itemtemplate>
                                            </asp:Repeater>                            
                                        <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>                                            
                                    <tr class="table_head">
                                        <td colspan = "13">
                                        <div class="float_l"><asp:Label ID="lblFootTB1" runat="server" Text="&nbsp;" ></asp:Label></div>
                                        <div class="float_r"><asp:Label ID="lblFootTB2" runat="server" Text="&nbsp;" ></asp:Label></div>
                                        </td>
                                    </tr>   
                                </table>
                            </asp:Panel> 
                        </td>
                    </tr> 
                    <tr>
                        <td class="table_field_td">Attn</td>
                        <td class="table_field_td"><asp:TextBox ID="txtAttn" runat="server" CssClass="text_field" MaxLength="100"></asp:TextBox></td>
                        <td class="table_field_td">Deliver to</td>
                        <td class="table_field_td"><asp:TextBox ID="txtDeliverTo" runat="server" CssClass="text_field" MaxLength="100"></asp:TextBox></td>
                        <td class="table_field_td">Contact</td>
                        <td class="table_field_td"><asp:TextBox ID="txtContact" runat="server" CssClass="text_field" MaxLength="100"></asp:TextBox></td>
                    </tr>  
                </table>
                <br /> 
                <table class="table_field" width="1500">
                    <tr>
                        <td class="table_field_td tb_Fix80">Sub Total</td>
                        <td class="table_field_td tb_Fix140 text_right"><asp:Label ID="lblSubTotal" runat="server" Font-Bold="True"></asp:Label></td>
                        <td class="table_field_td tb_Fix80">Discount Total</td>
                        <td class="table_field_td tb_Fix140 text_right"><asp:Label ID="lblDiscountTotal" runat="server" Font-Bold="True"></asp:Label></td>
                        <td class="table_field_td tb_Fix80">Vat Amount</td>
                        <td class="table_field_td tb_Fix140 text_right"><asp:Label ID="lblVatAmount" runat="server" Font-Bold="True"></asp:Label></td>
                        <td class="table_field_td tb_Fix80">W/T Amount</td>
                        <td class="table_field_td tb_Fix140 text_right"><asp:Label ID="lblWTAmount" runat="server" Font-Bold="True"></asp:Label></td>
                        <td class="table_field_td tb_Fix80">Total Amount</td>
                        <td class="table_field_td tb_Fix160 text_right"><asp:Label ID="lblTotalAmount" runat="server" Font-Bold="True"></asp:Label></td>
                    </tr>         
                    <asp:Panel ID="panelTHB" runat="server" Visible="false">
                        <tr class="THB" > <%--style="display:none"--%>
                            <td class="table_field_td tb_Fix80">Sub Total (THB)</td>
                            <td class="table_field_td tb_Fix140 text_right"><asp:Label ID="lblThbSubTotal" runat="server" Font-Bold="True"></asp:Label></td>
                            <td class="table_field_td tb_Fix80">Discount Total (THB)</td>
                            <td class="table_field_td tb_Fix140 text_right"><asp:Label ID="lblThbDiscountTotal" runat="server" Font-Bold="True"></asp:Label></td>
                            <td class="table_field_td tb_Fix80">Vat Amount (THB)</td>
                            <td class="table_field_td tb_Fix140 text_right"><asp:Label ID="lblThbVatAmount" runat="server" Font-Bold="True"></asp:Label></td>
                            <td class="table_field_td tb_Fix80">W/T Amount (THB)</td>
                            <td class="table_field_td tb_Fix140 text_right"><asp:Label ID="lblThbWTAmount" runat="server" Font-Bold="True"></asp:Label></td>
                            <td class="table_field_td tb_Fix80">Total Amount (THB)</td>
                            <td class="table_field_td tb_Fix160 text_right"><asp:Label ID="lblThbTotalAmount" runat="server" Font-Bold="True"></asp:Label></td>
                        </tr> 
                    </asp:Panel>
                    <tr>
                        <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>--%>                                        
                                <td colspan="10" class="text_right">
                                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="button_style" CausesValidation="true" ValidationGroup="Purchase" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" CssClass="button_style" CausesValidation="false" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="button_style" CausesValidation="false" />
                                </td>        
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </tr>
                </table>
               
            </div>
            <asp:HiddenField id="hiddenWT" runat="server"></asp:HiddenField>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>



