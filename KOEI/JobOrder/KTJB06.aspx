<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTJB06.aspx.vb" Inherits="JobOrder_KTJB06" MaintainScrollPositionOnPostback="true"  %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">


<link type="text/css" rel="Stylesheet" href="../Scripts/themes/base/jquery.ui.all.css" />

    <script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../Scripts/jsCommon.js"></script> 
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.core.js"></script>
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.button.js"></script>
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.menu.js"></script>
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.autocomplete.js"></script>
	<script type="text/javascript" src="../Scripts/ui/jquery.ui.tooltip.js"></script>
	<link type="text/css" rel="Stylesheet" href="../Scripts/demos/demos.css" />

  <style type="text/css">
	.custom-select {
		position: relative;
		display: inline-block;
	}
	.custom-select-toggle {
		position: absolute;
		top: 0;
		bottom: 0;
		margin-left: -1px;
		padding: 0;
		*height: 1.9em;
		*top: 0.1em;
	}
	.custom-select-input {
		margin: 0;
		padding: 0.3em;		
		*Width: 40.9em;
	}
	</style>
	
	<script type="text/javascript">
	(function( $ ) {
		$.widget( "custom.select", {
			_create: function() {
				this.wrapper = $( "<span>" )
					.addClass( "custom-select" )
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
					.addClass( "custom-select-input ui-widget ui-widget-content ui-state-default ui-corner-left" )
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
					.addClass( "custom-select-toggle ui-corner-right" )
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
		$( "#ctl00_MainContent_ddlCustomer" ).select();
        $( "#ctl00_MainContent_ddlAccountTitle" ).select();
	});
	
	</script>

 <script language="javascript"  type="text/javascript" >
        //Function input number only        
        function isNumberKey(evt) { 
         var charCode = (evt.which) ? evt.which : event.keyCode
         if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
             return false;
         else {
             var len = document.getElementById('<%=txtExchangeRate.ClientID %>').value.length;
             var index = document.getElementById('<%=txtExchangeRate.ClientID %>').value.indexOf('.'); 
             
             if (index > 0 && charCode == 46) {    
                       
                return false;                           
             }
         }
         return true;
      }    
        
    function setCurrencyValue(value) {
        alert("Z");
        alert(args.value);
    }
    
    function setVatSelected() {
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
    
    function checkRequireBtnSelect(source, args) {
        alert("OK");
        var arrr = document.getElementById('<%=reqValidatorExchangeRate.ClientID %>');
        alert(arrr.value);
        return false;      
    }
    </script>
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a href="KTJB05.aspx?New=True">Account</a> > Sale Invoice Management</div>
        <br /><div class="font_header">SALE INVOICE MANAGEMENT</div> 
        <br />         
        <table class="table_field table_field_td" style="width:1070px;" >
            <tr>
                <td class="table_field_td table_detail_head" colspan="2"><b>Header</b></td> 
            </tr>
            <tr>
                <td class="table_field_td">Invoice No.<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtInvoiceNo" runat="server" Width="118px" MaxLength="20"></asp:TextBox>                    
                    <asp:RequiredFieldValidator ID="reqValidatorInvoiceNo" runat="server" ValidationGroup="Add"
                        ControlToValidate="txtInvoiceNo" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Issue Date (dd/mm/yyyy)<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtIssueDate" runat="server" MaxLength="10" Width="118px"></asp:TextBox> 
                    <asp:CalendarExtender ID="txtIssueDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        TargetControlID="txtIssueDate">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="txtIssueDate_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtIssueDate" 
                        ValidChars="1234567890/" />   
                    <asp:RequiredFieldValidator ID="reqValidatorIssueDate" runat="server" ValidationGroup="Add"
                        ControlToValidate="txtIssueDate" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Receipt Date (dd/mm/yyyy)<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtReceiveDate" runat="server" Width="118px" MaxLength="10"></asp:TextBox>
                    <asp:CalendarExtender ID="txtReceiveDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        TargetControlID="txtReceiveDate">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="txtReceiveDate_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtReceiveDate" 
                        ValidChars="1234567890/" />   
                    <asp:RequiredFieldValidator ID="reqValidatorReceiveDate" runat="server" ValidationGroup="Add"
                        ControlToValidate="txtReceiveDate" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Account Title<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlAccountTitle" runat="server" Width="209px">
                    </asp:DropDownList>
                     <asp:RequiredFieldValidator ID="reqValidatorAccountTitle" runat="server" ValidationGroup="Add"
                        ControlToValidate="ddlAccountTitle" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Customer<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlCustomer" runat="server" Width="200px"></asp:DropDownList>
                    <button id="toggle"></button>
                    <asp:RequiredFieldValidator ID="reqValidatorCustomer" runat="server" ValidationGroup="Search"
                        ControlToValidate="ddlCustomer" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Account Type<span class="font_require">*</span></td>
                <td class="table_field_td">
                     <asp:RadioButtonList ID="rblAccountType" runat="server" 
                         RepeatDirection="Horizontal">
                         <asp:ListItem Value="1">Current Account</asp:ListItem>
                         <asp:ListItem Value="2">Saving Account</asp:ListItem>
                         <asp:ListItem Value="3">Cash</asp:ListItem>
                     </asp:RadioButtonList>
                     <asp:RequiredFieldValidator ID="reqValidatorAccountType" runat="server" ValidationGroup="Add"
                        ControlToValidate="rblAccountType" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Invoice Type<span class="font_require">*</span></td>
                <td class="table_field_td">
                     <asp:RadioButtonList ID="rblInvoiceType" runat="server" 
                         RepeatDirection="Horizontal" Height="16px">
                         <asp:ListItem Value="1">IN</asp:ListItem>
                         <asp:ListItem Value="2">IV</asp:ListItem>
                         <%--<asp:ListItem Value="3">IS</asp:ListItem>--%>
                     </asp:RadioButtonList>
                     <asp:RequiredFieldValidator ID="reqValidatorInvoiceType" runat="server" ValidationGroup="Add"
                        ControlToValidate="rblInvoiceType" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
           <%-- <tr>
                <td class="table_field_td"> Bank Fee</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtBankFee" runat="server" Width="118px" MaxLength="17" style="text-align:right"></asp:TextBox> 
                     <asp:FilteredTextBoxExtender ID="txtBankFee_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtBankFee" 
                         ValidChars="1234567890,.">
                     </asp:FilteredTextBoxExtender>
                     <asp:RangeValidator ID="rangBankFee" runat="server" 
                             ControlToValidate="txtBankFee" Display="Static" 
                             ErrorMessage="*Invalid Bank Fee => 0" MaximumValue="99999999999999.99" 
                             MinimumValue="0.00" Type="Double" ValidationGroup="Add">
                        </asp:RangeValidator>                    
                </td>
            </tr>--%>
            <tr>
                <td class="table_field_td">Sale Invoice Amount (THB)</td>
                <td class="table_field_td">
                    <b><asp:Label ID="lblInvoiceAmount" runat="server"></asp:Label>
                        <asp:Label ID="lblInvoiceHidAmount" runat="server" Text="******"></asp:Label>
                    </b>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="table_field_td table_detail_head" colspan="2">Detail</td> 
            </tr>
            <tr>
                <td class="table_field_td">Job Order<span class="font_require">*</span></td>
                <td class="table_field_td" >
                    <asp:TextBox ID="txtJobOrder" runat="server" Width="120px" MaxLength="6"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="reqValidatorJobOrder" runat="server" ValidationGroup="Search"
                        ControlToValidate="txtJobOrder" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator>                   
                    <asp:Button ID="btnSearch" runat="server" Text="Search"  CssClass="button_style" ValidationGroup="Search"/>
                    
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Currency</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtCurrency" runat="server" Width="120px" Enabled="false"></asp:TextBox> 
                    &nbsp; Schedule Rate (THB)&nbsp;
                    <asp:TextBox ID="txtScheduleRate" runat="server" style="text-align:right" Width="120px" Enabled="false"></asp:TextBox> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Actual Exchange Rate (THB)<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtExchangeRate" runat="server" Width="120px" MaxLength="17" style="text-align:right" onkeypress="return isNumberKey(event)"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="reqValidatorExchangeRate" runat="server" ValidationGroup="Add"
                        ControlToValidate="txtExchangeRate" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="sel"
                        ControlToValidate="txtExchangeRate" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator>  
                    <asp:RangeValidator ID="rngExchangeRate" runat="server" 
                             ControlToValidate="txtExchangeRate" Display="Static" 
                             ErrorMessage="*Invalid Actual Exchange Rate (THB) >= 0" MaximumValue="99999999999.99999" 
                             MinimumValue="0.01" Type="Double" ValidationGroup="Add"></asp:RangeValidator> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">
                    <table class="table_inquiry" width="650">
                        <tr class="table_head">
                            <td class="tb_Fix50">Select</td>
                            <td class="tb_Fix100">Sale PO No.</td>
                            <td class="tb_Fix100">PO Type</td>
                            <td class="tb_Fix100">Hontai Type</td>
                            <td class="tb_Fix100">Hontai Condition</td>
                            <td class="tb_Fix100">Amount</td>
                            <td class="tb_Fix100">PO Issue Date</td>                      
                        </tr>
                        <tbody>
                            <asp:Repeater ID="rptDetailFirst" runat="server">
                                <itemtemplate>
                                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                        <td class="text_center tb_Fix50">
                                            <label>
                                                <input type="checkbox" id="chkApprove" runat="server" />
                                            </label>
                                        </td>
                                        <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "po_no")%></div></td>
                                        <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "po_type_name")%></div></td>
                                        <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "hontai")%></div></td>
                                        <td class="text_right tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "hontai_cond")%></div></td>
                                        <td class="text_right tb_Fix100"><div><asp:Label ID="lblHeaderAmount" runat="server"></asp:Label></div></td>
                                        <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "po_date")%></div></td>
                                    </tr>
                                </itemtemplate>
                            </asp:Repeater> 
                        </tbody>
                        <tr class="table_head">
                            <td colspan = "7">
                                <div class="float_l">
                                    <asp:Label ID="lblDescription" runat="server"></asp:Label>
					            </div>
					            <div class="float_r">
                                    <asp:Label ID="lblPaging" runat="server" Text="&nbsp;"></asp:Label>
                                </div>
                            </td>
                        </tr> 
                        <tr>
                            <td class="text_right" colspan="7">
                                <asp:Button ID="btnSelect" runat="server" Text="Select" CssClass="button_style" ValidationGroup="sel" />
                            </td>
                        </tr>    
                    </table>
                    <table class="table_inquiry">
                        <tr class="table_head">
                            <td class="tb_Fix50">Delete</td>
                            <td class="tb_Fix100">Sale PO No.</td>
                            <td class="tb_Fix100">PO Type</td>
                            <td class="tb_Fix100">Hontai Type</td>
                            <td class="tb_Fix100">Hontai Condition</td>
                            <td class="tb_Fix100">Amount (THB)</td>
                            <td class="tb_Fix100">Job Order</td> 
                            <td class="tb_Fix100">Job Type</td>
                            <td class="tb_Fix100">Vat</td>
                            <td class="tb_Fix100">W/T</td>
                            <td class="tb_Fix100">Bank Fee (THB)</td>
                            <td class="tb_Fix120">Remarks</td>
                        </tr>
                        <tbody>
                            <asp:Repeater ID="rptDetailSecond" runat="server">
                                <itemtemplate>
                                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                                        <td class="td_delete tb_Fix50"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                                        <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "po_no")%></div></td>
                                        <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "po_type_name")%></div></td>
                                        <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "hontai")%></div></td>
                                        <td class="text_right tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "hontai_cond")%></div></td>
                                        <td class="text_right tb_Fix100"><div><asp:Label ID="lblDetailAmount" runat="server"></asp:Label></div></td>
                                        <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                                        <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "job_type")%></div></td>
                                        <td class="text_left tb_Fix100">
                                            <div> 
                                                <asp:DropDownList ID="ddlVat" runat="server" CssClass="tb_Fix80"></asp:DropDownList><span class="font_require">*</span>
                                                <asp:RequiredFieldValidator ID="reqValidatorJobOrder" runat="server" ValidationGroup="Add"
                                                    ControlToValidate="ddlVat" ErrorMessage="*Require."  class="font_error">
                                                </asp:RequiredFieldValidator> 
                                            </div>
                                        </td>
                                        <td class="text_left tb_Fix100">
                                            <div> 
                                                <asp:DropDownList ID="ddlWt" runat="server" CssClass="tb_Fix80"  ></asp:DropDownList><span class="font_require">*</span>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Add"
                                                    ControlToValidate="ddlWt" ErrorMessage="*Require."  class="font_error">
                                                </asp:RequiredFieldValidator> 
                                            </div>
                                        </td>
                                        <td class="text_left tb_Fix100">
                                            <div> 
                                                <asp:TextBox ID="txtBankfee" runat="server" Width="80" 
                                                    onblur="this.value=setCurrencyPoint(this.value);" MaxLength="16"  
                                                    Text='<%#DataBinder.Eval(Container.DataItem, "bank_fee")%>' ></asp:TextBox>
                                                <%--<asp:CompareValidator ID="reqBankfee" runat="server" ErrorMessage="*>=0" Type="Currency" 
                                                    ControlToValidate="txtBankfee" ValueToCompare="0.00" Operator="NotEqual"></asp:CompareValidator>
                                                --%>
                                                <asp:FilteredTextBoxExtender ID="filBankfee" 
                                                     runat="server" Enabled="True" TargetControlID="txtBankfee" 
                                                     ValidChars="1234567890.," />
                                            </div>
                                        </td>
                                        <td class="text_left tb_Fix120">
                                            <div> 
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="tb_Fix100" MaxLength="255"  Text='<%#DataBinder.Eval(Container.DataItem, "remark")%>' ></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    
                                </itemtemplate>
                            </asp:Repeater> 
                        </tbody>
                    </table>
                </td> 
            </tr>
        </table>
        <table width="865px;" >
            <tr>
                <td class="text_right">
                    <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button_style" ValidationGroup="Add" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear"  CssClass="button_style" CausesValidation="false" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back"  CssClass="button_style" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

