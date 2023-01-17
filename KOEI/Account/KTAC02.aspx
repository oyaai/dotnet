<%@ Page Title="KOEI TOOL Management System" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTAC02.aspx.vb" Inherits="Account.KTAC02" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <link rel="stylesheet" href="../Scripts/themes/base/jquery.ui.all.css"/>
    <script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script>
<script type="text/javascript" src="../Scripts/jsCommon.js"></script> 

	<script type="text/javascript" src="../Scripts/ui/jquery.ui.core.js"></script>
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.button.js"></script>
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.menu.js"></script>
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.autocomplete.js"></script>
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.tooltip.js"></script>
	<link rel="stylesheet" href="../Scripts/demos/demos.css"/>
<style>
	.custom-combobox {
		position: relative;
		display: inline-block;
	}
	.custom-combobox-toggle {
		position: absolute;
		top: 0;
		bottom: 0;
		margin-left: -1px;
		padding: 0;
		/* support: IE7 */
		*height: 1.7em;
		*top: 0.1em;
	}
	.custom-combobox-input {
		margin: 0;
		width: 250px;
		padding: 0.3em;
	}
	</style>

<script language="javascript"  type="text/javascript" >
    (function( $ ) {
		$.widget( "custom.combobox", {
			_create: function() {
				this.wrapper = $( "<span>" )
					.addClass( "custom-combobox" )
					.insertAfter( this.element );

				this.element.hide();
				this._createAutocomplete();
				this._createShowAllButton();
			},

			_createAutocomplete: function() {
				var selected = this.element.children( ":selected" ),
					value = selected.val() ? selected.text() : "";

				this.input = $( "<input>" )
					.appendTo( this.wrapper )
					.val( value )
					.attr( "title", "" )
					.addClass( "custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left" )
					.autocomplete({
						delay: 0,
						minLength: 0,
						source: $.proxy( this, "_source" )
					})
					.tooltip({
						tooltipClass: "ui-state-highlight"
					});

				this._on( this.input, {
					autocompleteselect: function( event, ui ) {
						ui.item.option.selected = true;
						this._trigger( "select", event, {
							item: ui.item.option
						});
					},

					autocompletechange: "_removeIfInvalid"
				});
			},

			_createShowAllButton: function() {
				var input = this.input,
					wasOpen = false;

				$( "<a>" )
					.attr( "tabIndex", -1 )
					.attr( "title", "Show All Items" )
					.tooltip()
					.appendTo( this.wrapper )
					.button({
						icons: {
							primary: "ui-icon-triangle-1-s"
						},
						text: false
					})
					.removeClass( "ui-corner-all" )
					.addClass( "custom-combobox-toggle ui-corner-right" )
					.mousedown(function() {
						wasOpen = input.autocomplete( "widget" ).is( ":visible" );
					})
					.click(function() {
						input.focus();

						// Close if already visible
						if ( wasOpen ) {
							return;
						}

						// Pass empty string as value to search for, displaying all results
						input.autocomplete( "search", "" );
					});
			},

			_source: function( request, response ) {
				var matcher = new RegExp( $.ui.autocomplete.escapeRegex(request.term), "i" );
				response( this.element.children( "option" ).map(function() {
					var text = $( this ).text();
					if ( this.value && ( !request.term || matcher.test(text) ) )
						return {
							label: text,
							value: text,
							option: this
						};
				}) );
			},

			_removeIfInvalid: function( event, ui ) {

				// Selected an item, nothing to do
				if ( ui.item ) {
					return;
				}

				// Search for a match (case-insensitive)
				var value = this.input.val(),
					valueLowerCase = value.toLowerCase(),
					valid = false;
				this.element.children( "option" ).each(function() {
					if ( $( this ).text().toLowerCase() === valueLowerCase ) {
						this.selected = valid = true;
						return false;
					}
				});

				// Found a match, nothing to do
				if ( valid ) {
					return;
				}

				// Remove invalid value
				this.input
					.val( "" )
					.attr( "title", value + " didn't match any item" )
					.tooltip( "open" );
				this.element.val( "" );
				this._delay(function() {
					this.input.tooltip( "close" ).attr( "title", "" );
				}, 2500 );
				this.input.data( "ui-autocomplete" ).term = "";
			},

			_destroy: function() {
				this.wrapper.remove();
				this.element.show();
			}
		});
	})( jQuery );
	
	$(function() {
	});
    $(function() {
		$( "#<% = ddlVendor.ClientID %>" ).combobox({
		    select: function(event, ui ) {
                theForm.submit();     
		    }
		});
		$( "#<% = ddlItemExpense.ClientID %>" ).combobox();
//		$( "#<% = ddlVendor.ClientID %>" ).select({
//		    select: function(event, ui ) {
//                theForm.submit();     
//		    }
//		});
	});

    // fn calculate Vat
    function CalVat(strVat, intSubTotal) {
        if (intSubTotal != "" && strVat != "") {
            var intVat = strVat.replace("%", "");
            $("#<% = txtVatAmount.ClientID %>").val(setCurrencyPoint((parseFloat(intSubTotal.replace(/,/g, "")) * intVat) / 100));
        }
    }

    // fn calculate WT
    function CalWT(strWT, intSubTotal) {
        if (intSubTotal != "" && strWT != "") {
            var intWT = strWT.replace("%", "");
            $("#<% = txtWTAmount.ClientID %>").val(setCurrencyPoint((parseFloat(intSubTotal.replace(/,/g, "")) * intWT) / 100));  
        }
    }
    
    // fn calculate Total in thai bath
    function CalTotalInTHB() {
        var inputTotal = ($('#<% = txtTotal.ClientID %>').val()).replace(/,/g, "");
        var inputCCRate = ($('#<% = txtCurrencyRate.ClientID %>').val()).replace(/,/g, "");
        if (isNaN(inputTotal) == false && isNaN(inputCCRate) == false){
            if (parseFloat(inputTotal) >= 0 && parseFloat(inputCCRate) >= 0) {
                $('#<% = txtTotal.ClientID %>').val(setCurrencyPoint(inputTotal));
                $('#<% = txtCurrencyRate.ClientID %>').val(setNumberPoint(inputCCRate));
                $('#<% = txtAmountTHB.ClientID %>').val(setCurrencyPoint(inputTotal * inputCCRate));
            }
        }
    }
    
    function IsValidDate_ClientValidate(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02.aspx/IsValidDate", // <-- Page Method
            data: "{strDate: '" + args.Value + "', name: 'en-AU'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async:false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });              
    }

    function IsExistJobOrder(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02.aspx/IsExistJobOrder", // <-- Page Method
            data: "{strJobOrder: '" + args.Value + "'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });
    }
    
    //Check same vendor selected
    function CheckSameVendor(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02.aspx/CheckSameVendor", // <-- Page Method
            data: "{strValueID: '" + args.Value + "', strColumnName: 'VendorID'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });
    }
    
    //Check same vendor selected
    function CheckSameVendorAddress(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02.aspx/CheckSameVendor", // <-- Page Method
            data: "{strValueID: '" + args.Value + "', strColumnName: 'VendorBranchID'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });
    }
    
    //Check same vendor selected
    function CheckSameBank(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02.aspx/CheckSameVendor", // <-- Page Method
            data: "{strValueID: '" + args.Value + "', strColumnName: 'Bank'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });
    }
    
    //Check same vendor selected
    function CheckSameAccountName(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02.aspx/CheckSameVendor", // <-- Page Method
            data: "{strValueID: '" + args.Value + "', strColumnName: 'AccountName'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });
    }
    
    //Check same vendor selected
    function CheckSameAccountNo(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02.aspx/CheckSameVendor", // <-- Page Method
            data: "{strValueID: '" + args.Value + "', strColumnName: 'AccountNo'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });
    }
    
    //Check same vendor selected
    function CheckSameAccountType(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02.aspx/CheckSameVendor", // <-- Page Method
            data: "{strValueID: '" + args.Value + "', strColumnName: 'AccountType'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });
    }
    
    //Check same vendor selected
    function CheckSameVat(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02.aspx/CheckSameVendor", // <-- Page Method
            data: "{strValueID: '" + args.Value + "', strColumnName: 'VatID'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });
    }
    
    //Check same vendor selected
    function CheckSameWT(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02.aspx/CheckSameVendor", // <-- Page Method
            data: "{strValueID: '" + args.Value + "', strColumnName: 'WTID'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });
    }
    
    //Check same vendor selected
    function CheckSameChequeNo(sender, args) {
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTAC02.aspx/CheckSameVendor", // <-- Page Method
            data: "{strValueID: '" + args.Value + "', strColumnName: 'ChequeNo'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                args.IsValid = msg.d; //true if valid, false if invalid        
            }
        });
    }
    
    $(document).ready(function() {

        // Change Event Handler ddlVat
        $("#ctl00_MainContent_ddlVat").change(function() {
            var strVatAmount = $('#<% = ddlVat.ClientID %> option:selected').text();            
            $('#<% = hideVatAmount.ClientID %>').val(strVatAmount.replace("%", ""));
            CalVat(strVatAmount, $('#<% = txtAmountTHB.ClientID %>').val());
        });

        // Change Event Handler ddlWT
        $("#ctl00_MainContent_ddlWT").change(function() {
            var strWTAmount = $('#<% = ddlWT.ClientID %> option:selected').text();            
            $('#<% = hideWTAmount.ClientID %>').val(strWTAmount.replace("%", ""));
            CalWT(strWTAmount, $('#<% = txtAmountTHB.ClientID %>').val());
        });

        // Change Event Handler ddlCurrency
        $("#<% = ddlCurrency.ClientID %>").change(function() {
            if ($('#<% = ddlCurrency.ClientID %> option:selected').text() == "THB") {
                $('#<% = txtCurrencyRate.ClientID %>').val("1.00");
            }
            CalTotalInTHB();
            CalVat($('#<% = ddlVat.ClientID %> option:selected').text(), $('#<% = txtAmountTHB.ClientID %>').val());
            CalWT($('#<% = ddlWT.ClientID %> option:selected').text(), $('#<% = txtAmountTHB.ClientID %>').val());
        });

        $("#ctl00_MainContent_txtTotal").focusout(function() {
            CalTotalInTHB();
            $('#<% = txtTotal.ClientID %>').val(setCurrencyPoint($('#<% = txtTotal.ClientID %>').val()));
            CalVat($('#<% = ddlVat.ClientID %> option:selected').text(), $('#<% = txtAmountTHB.ClientID %>').val());
            CalWT($('#<% = ddlWT.ClientID %> option:selected').text(), $('#<% = txtAmountTHB.ClientID %>').val());
        });
        
        $("#ctl00_MainContent_txtCurrencyRate").focusout(function() {
            CalTotalInTHB();
            $('#<% = txtCurrencyRate.ClientID %>').val(setNumberPoint($('#<% = txtCurrencyRate.ClientID %>').val()));
            CalVat($('#<% = ddlVat.ClientID %> option:selected').text(), $('#<% = txtAmountTHB.ClientID %>').val());
            CalWT($('#<% = ddlWT.ClientID %> option:selected').text(), $('#<% = txtAmountTHB.ClientID %>').val());
        });
        
    });
    
</script>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTAC02.aspx?NEW=True">Accounting</a> &gt; 
            Income
        </div>
        <br />
        <div class="font_header">
            INCOME
        </div>
        <div style="width: 240px"><asp:HiddenField ID="hideAccountingID" runat="server" /><asp:HiddenField ID="hideStatusID" runat="server" />
                    <asp:HiddenField ID="hideVatAmount" runat="server" />
                    <asp:HiddenField ID="hideWTAmount" runat="server" /></div>
        <br />
        <table class="table_field" style="width:1000px;">
            <tr>
                <td class="table_field_td tb_Fix100">
                    AccountType<span class="font_error">*</span><br />
                </td>
                <td class="table_field_td tb_Fix400" >&nbsp;
                    <asp:RadioButtonList ID="rbtAccountType" runat="server" 
                        RepeatDirection="Horizontal" Height="23px" Width="280px" 
                        RepeatLayout="Flow">
                        <asp:ListItem Text="Current Account  " Value="1" />
                        <asp:ListItem Text="Saving Account  " Value="2" />
                        <asp:ListItem Text="Cash " Value="3" />
                    </asp:RadioButtonList><br />
                    <asp:RequiredFieldValidator ID="reqVendor0" runat="server" ErrorMessage="*Required."
                        ControlToValidate="rbtAccountType" Display="Dynamic" 
                        SetFocusOnError="True" />
                    <asp:CustomValidator ID="reqSameAccountType" runat="server" 
                        ClientValidationFunction="CheckSameAccountType" 
                        ControlToValidate="rbtAccountType" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    &nbsp;
                </td>
                <td class="table_field_td tb_Fix100">
                    Job Order <span class="font_error">*</span>
                </td>
                <td class="table_field_td tb_Fix400">
                    
                    <asp:TextBox ID="txtJobOrder" runat="server" MaxLength="6" Width="186px" />
                    <br />
                    <asp:RequiredFieldValidator ID="reqJobOrder" runat="server" ErrorMessage="*Required."
                        ControlToValidate="txtJobOrder" Display="Dynamic" SetFocusOnError="True" />
                     <asp:CustomValidator ID="reqJobOrderExist" runat="server" 
                        ControlToValidate="txtJobOrder" ErrorMessage="*Not exist Job Order." 
                        ClientValidationFunction="IsExistJobOrder" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    Vendor Name <span class="font_error">*</span><br />&nbsp;
                </td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlVendor" runat="server" Width="268px" Height="22px" 
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <br />
                    <asp:RequiredFieldValidator ID="reqVendor" runat="server" ErrorMessage="*Required."
                        ControlToValidate="ddlVendor" Display="Dynamic" SetFocusOnError="True" />
                    <asp:CustomValidator ID="reqSameVendor" runat="server" 
                        ClientValidationFunction="CheckSameVendor" 
                        ControlToValidate="ddlVendor" ErrorMessage="CustomValidator"></asp:CustomValidator>
                </td>
                <td class="table_field_td">
                    Account Title <span class="font_error">*</span>
                </td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlItemExpense" runat="server" Width="186px">
                    </asp:DropDownList>
                    <%--<asp:DropDownList ID="ddlVendorAddress" runat="server" Width="805px" 
                        Height="22px" Enabled="False" Visible="False">
                    </asp:DropDownList>--%>
                    <br />
                    <asp:RequiredFieldValidator ID="reqAccountTitle" runat="server" ErrorMessage="*Required."
                        ControlToValidate="ddlItemExpense" Display="Dynamic" 
                        SetFocusOnError="True" />&nbsp;
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    Vendor <span class="font_error">*</span><br />Address &nbsp;
                </td>
                <td class="table_field_td" colspan="3">
                    <asp:DropDownList ID="ddlVendorAddress" runat="server" Width="800px">
                    </asp:DropDownList><br />
                    <asp:CustomValidator ID="reqVendorAddress" runat="server" 
                        ClientValidationFunction="CheckSameVendorAddress" 
                        ControlToValidate="ddlVendorAddress" ErrorMessage="CustomValidator"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    Bank<br />&nbsp;
                </td>
                <td class="table_field_td">
                    <asp:TextBox MaxLength="50" ID="txtBank" Width="261px" runat="server" />
                    <br />
                    <asp:CustomValidator ID="reqSameBank" runat="server" 
                        ClientValidationFunction="CheckSameBank" 
                        ControlToValidate="txtBank" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    <br />
                </td>
                <td class="table_field_td">
                    Cheque No.<br />&nbsp;
                </td>
                <td class="table_field_td">
                    <asp:TextBox MaxLength="50" ID="txtChequeNo" Width="261px" runat="server" />
                    <br />
                    <asp:CustomValidator ID="reqSameChequeNo" runat="server"
                        ClientValidationFunction="CheckSameChequeNo"  
                        ControlToValidate="txtChequeNo" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    <br />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    Account Name<br />&nbsp;
                </td>
                <td class="table_field_td">
                    <asp:TextBox MaxLength="50" ID="txtAccountName" Width="260px" runat="server" />
                    <br />
                    <asp:CustomValidator ID="reqAccountName" runat="server" 
                        ClientValidationFunction="CheckSameAccountName" 
                        ControlToValidate="txtAccountName" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    <br />
                </td>
                <td class="table_field_td">
                    Vat <span class="font_error">*</span>
                </td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlVat" runat="server" Width="50px">
                    </asp:DropDownList>
                    &nbsp;
                    <asp:TextBox ID="txtVatAmount" ReadOnly="true" CssClass="textbox_read_only" 
                        runat="server" Width="126px" Enabled="False" />
                    <br />
                    <asp:RequiredFieldValidator ID="reqInputVat" runat="server" ErrorMessage="*Required."
                        ControlToValidate="ddlVat" Display="Dynamic" SetFocusOnError="True" />
                  <%--  <asp:CustomValidator ID="reqVat" runat="server" 
                        ClientValidationFunction="CheckSameVat" 
                        ControlToValidate="ddlVat" ErrorMessage="CustomValidator"></asp:CustomValidator>--%>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    Account No.<br />&nbsp;
                </td>
                <td class="table_field_td">
                    <asp:TextBox MaxLength="100" ID="txtAccountNo" Width="260px" runat="server" />
                    <br />
                    <asp:CustomValidator ID="reqAccountNo" runat="server" 
                        ClientValidationFunction="CheckSameAccountNo" 
                        ControlToValidate="txtAccountNo" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    <br />
                </td>
                <td class="table_field_td">
                    W/T <span class="font_error">*</span>
                </td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlWT" runat="server" Width="50px">
                    </asp:DropDownList>
                    &nbsp;
                    <asp:TextBox ID="txtWTAmount" ReadOnly="True" CssClass="textbox_read_only" 
                        runat="server" Width="126px" Enabled="False" />
                    <br />
                    <asp:RequiredFieldValidator ID="reqInputWT" runat="server" ErrorMessage="*Required."
                        ControlToValidate="ddlWT" Display="Dynamic" SetFocusOnError="True" />
                    <%--<asp:CustomValidator ID="reqSameWT" runat="server" 
                        ClientValidationFunction="CheckSameWT" 
                        ControlToValidate="ddlWT" ErrorMessage="CustomValidator"></asp:CustomValidator>--%>
                        
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td class="table_field_td">
                    Receipt Date<br />
                    (dd/mm/yyyy) <span class="font_error">*</span></td>
                <td class="table_field_td">
                    <asp:TextBox MaxLength="10" ID="txtReceiptDate" runat="server" Width="186px" />
                    <asp:CalendarExtender ID="cldReceiptDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtReceiptDate" />
                    <br />
                    <asp:FilteredTextBoxExtender ID="txtReceiptDate_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtReceiptDate" 
                         ValidChars="1234567890/" />
                    <asp:RequiredFieldValidator ID="reqReceiptDate" runat="server" ErrorMessage="*Required."
                        ControlToValidate="txtReceiptDate" Display="Dynamic" 
                        SetFocusOnError="True" />
                    <asp:CustomValidator ID="reqDateInvalid" runat="server" 
                        ControlToValidate="txtReceiptDate" ErrorMessage="*Invalid format." 
                        ClientValidationFunction="IsValidDate_ClientValidate" />                     
                </td>
                <td class="table_field_td">
                    Sub Total<span class="font_error">*</span> 
                    <br />
                    (Tax Excluded) 
                </td>
                <td class="table_field_td tb_Fix400">
                    <asp:TextBox MaxLength="16" ID="txtTotal" Width="100px" 
                        runat="server"  CssClass="text_right"
                        onkeypress="return IsDecimals(event,'ctl00_MainContent_txtTotal',2);" />
                    <asp:DropDownList ID="ddlCurrency" runat="server" Width="70px">
                    </asp:DropDownList>
                    <asp:TextBox MaxLength="16" ID="txtCurrencyRate" Width="50px" 
                        runat="server" CssClass="text_right"                        
                        onkeypress="return IsDecimals(event,'ctl00_MainContent_txtCurrencyRate',2);" />
                    <asp:TextBox MaxLength="16" ID="txtAmountTHB" Width="100px" 
                        runat="server" ReadOnly="True" 
                        CssClass="ctb_Fix85 text_right textbox_read_only" Enabled="False" />
                    <br />
                    <asp:RequiredFieldValidator
                        ID="reqSubTotal" runat="server" ErrorMessage="*Sub Total required."
                        Display="Dynamic" ControlToValidate="txtTotal" Style="text-align: right" 
                        SetFocusOnError="True" />
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator4" runat="server" ErrorMessage="*Currency required."
                        Display="Dynamic" ControlToValidate="ddlCurrency" Style="text-align: right" 
                        SetFocusOnError="True" />
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*Sub Total must greater than 0."
                        ControlToValidate="txtTotal" Operator="NotEqual" ValueToCompare="0.00" Display="Dynamic"
                        SetFocusOnError="True" />
                    <asp:FilteredTextBoxExtender ID="filTotal" 
                         runat="server" Enabled="True" TargetControlID="txtTotal" 
                         ValidChars="1234567890.," />
                    <asp:FilteredTextBoxExtender ID="filCurrencyRate" 
                         runat="server" Enabled="True" TargetControlID="txtCurrencyRate" 
                         ValidChars="1234567890.," />
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator3" runat="server" ErrorMessage="*Currency rate required."
                        Display="Dynamic" ControlToValidate="txtCurrencyRate" Style="text-align: right" 
                        SetFocusOnError="True" />
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="*Currency rate must greater than 0."
                        ControlToValidate="txtCurrencyRate" Operator="NotEqual" 
                        ValueToCompare="0.00" Display="Dynamic"
                        SetFocusOnError="True" />&nbsp;
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    Remark
                </td>
                <td class="table_field_td tb_Fix400" colspan="3">
                    <asp:TextBox ID="txtRemark" runat="server" style="width: 800px;"></asp:TextBox><br />&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search Income" Width="120px" 
                        CssClass="button_style" CausesValidation="False" />
                    &nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="Save" Width="120px" 
                        CssClass="button_style" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" 
                        CssClass="button_style" CausesValidation="False"  />
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td align="left">
                    <asp:Button ID="btnApply" Text="Apply" runat ="server" Enabled="false" 
                        CausesValidation="false" Width="120px" />
                </td>
            </tr>
            <tr>
                <td>
                    <table class="table_inquiry" style="width:1000px;">
                        <tr class="table_head">
                            <td class="tb_Fix50">
                                Edit
                            </td>
                            <td class="tb_Fix50">
                                Delete
                            </td>
                            <td class="tb_Fix100">
                                Date
                            </td>
                            <td class="tb_Fix300">
                                Vendor Name
                            </td>
                            <td class="tb_Fix200">
                                Account Title
                            </td>
                            <td class="tb_Fix100">
                                Job Order
                            </td>
                            <td class="tb_Fix150">
                                Sub Total (Amount)
                            </td>
                            <td class="tb_Fix50">
                                Details
                            </td>
                        </tr>
                        <asp:Repeater ID="rptInquery" runat="server">
                            <itemtemplate>
                                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                        <td class="tb_Fix50"><div class="ctb_Fix50"><asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_edit1 icon_center15" runat="server" CausesValidation="false" ></asp:LinkButton></div></td>
                                        <td class="tb_Fix50"><div class="ctb_Fix50"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server" CausesValidation="false" ></asp:LinkButton></div> </td>
                                        <td class="tb_Fix100"><div class="ctb_Fix100"><%#DataBinder.Eval(Container.DataItem, "ReceiptDateShow")%></div></td>
                                        <td class="tb_Fix300"><div class="ctb_Fix300 text_left"><%#DataBinder.Eval(Container.DataItem, "VendorName")%></div></td>
                                        <td class="tb_Fix200"><div class="ctb_Fix200 text_left"><%#DataBinder.Eval(Container.DataItem, "IEName")%></div></td>
                                        <td class="tb_Fix100"><div class="ctb_Fix100"><%#DataBinder.Eval(Container.DataItem, "JobOrder")%></div></td>
                                        <td class="tb_Fix150"><div class="ctb_Fix150 text_right"><%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "SubTotal")).ToString("#,##0.00")%></div></td>
                                        <td class="tb_Fix50"><div class="ctb_Fix50"><asp:LinkButton ID="btnDetail" CommandName="View" CssClass="icon_detail1 icon_center15" runat="server" CausesValidation="false"></asp:LinkButton></div> </td>
                                    </tr>
                            </itemtemplate>
                        </asp:Repeater> 
                        
                        <tr class="table_head">
                            <td colspan = "8">
                                <div class="float_l">
                                    <asp:Label ID="lblDescription" runat="server" Text="&nbsp;"></asp:Label>
				                </div>
				                <div class="float_r">
                                    <asp:Label ID="lblPaging" runat="server" Text="&nbsp;"></asp:Label>
                                </div>
                            </td>
                        </tr> 
                        
                        <tr style="color: #000000; font-weight: bold; text-align: center; vertical-align: middle; border: 1px solid #76a2c7; border-collapse: collapse;">
                            <td class="text_right" colspan = "5">
                                Sum Sub Total (Amount)
                                :
                            </td>
                            <td class="text_right" colspan = "2">
                                <asp:Label ID="lblSumSubTotal" runat="server"></asp:Label>
                            </td>
                            <td>
                                
                            </td>
                        </tr>          
                    </table>
                </td>
            </tr>
        </table>
        <br /><br />
    </div>
</asp:Content> 

