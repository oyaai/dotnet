<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS30.aspx.vb" Inherits="Master_KTMS30" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">


<link type="text/css" rel="Stylesheet" href="../Scripts/themes/base/jquery.ui.all.css" />
	<script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script>
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
		$( "#ctl00_MainContent_ddlCurrency" ).select();
//		$( "#toggle" ).click(function() {
//			$( "#ctl00_MainContent_ddlCurrency" ).toggle();
//		});
	});
	</script>


<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS29.aspx?New=True">Master Management</a> > Schedule Rate</div>
        <br />
        <div class="font_header">SCHEDULE RATE MANAGEMENT</div> 
        <br />
        
        <table class="table_field" width="550">
            <tr>
                <td class="table_field_td tb_Fix150">ID</td>
                <td class="table_field_td tb_Fix400">
                    <asp:TextBox ID="txtSRateId" Enabled="false" runat="server" CssClass="textbox_read_only text_field"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix150">Currency <span class="font_require">*</span></td>
                <td class="table_field_td tb_Fix400">
                    <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="dropdown_field" />
                    <button id="toggle"></button>
                    <asp:RequiredFieldValidator ID="reqCurrency" runat="server" ErrorMessage="*Require" InitialValue="0" Display="Dynamic"
                        ControlToValidate="ddlCurrency" ></asp:RequiredFieldValidator>
                    <asp:Label ID="lblMsgCurrency" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix150">Effective Date <span class="font_require">*</span></td>
                <td class="table_field_td tb_Fix400">
                    <asp:TextBox ID="txtEF_Date" MaxLength="10" runat="server" CssClass="text_field"></asp:TextBox>
                    <asp:Label ID="lblMsgEF_date" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                    <asp:RequiredFieldValidator ID="reqEF_Date" Display="Dynamic"
                        runat="server" ControlToValidate="txtEF_Date" ErrorMessage="*Require" >
                    </asp:RequiredFieldValidator>
                    <asp:CalendarExtender ID="txtEF_Date_CalendarExtender"
                        runat="server" Format="dd/MM/yyyy" TargetControlID="txtEF_Date" >
                    </asp:CalendarExtender>                    
                    <asp:FilteredTextBoxExtender ID="txtEF_Date_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtEF_Date" ValidChars="1234567890/" >
                    </asp:FilteredTextBoxExtender>                    
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix150">Rate <span class="font_require">*</span></td>
                <td class="table_field_td tb_Fix400 text_left">
                    <asp:TextBox ID="txtRate" MaxLength="15" runat="server" CssClass="text_field"></asp:TextBox>
                    <asp:Label ID="lblMsgRate" runat="server" Text="" CssClass="font_require" Visible="false"></asp:Label>
                    <asp:RequiredFieldValidator ID="txtRate_RequiredFieldValidator" Display="Dynamic"
                        runat="server" ControlToValidate="txtRate" ErrorMessage="*Require" >
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rngRate" runat="server" ControlToValidate="txtRate"
                        Display="Dynamic" ErrorMessage="*Require" MaximumValue="999999999.99999"
                        MinimumValue="0.00001" >
                    </asp:RangeValidator> 
                    <asp:FilteredTextBoxExtender ID="txtRate_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtRate" 
                        ValidChars="1234567890.">
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button_style" CausesValidation="true" />
                    &nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear"  CssClass="button_style" CausesValidation="false" />
                    &nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back"  CssClass="button_style" CausesValidation="false" />
                </td>
            </tr>
        </table>
        
    </div>
</asp:Content>

