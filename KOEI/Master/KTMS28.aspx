<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS28.aspx.vb" Inherits="Master_KTMS28" %>

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
		$( "#ctl00_MainContent_ddlSection" ).select();
//		$( "#toggle" ).click(function() {
//			$( "#ctl00_MainContent_ddlSection" ).toggle();
//		});
	});
	</script>


    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTMS27.aspx?New=True">Master Management</a> > Staff</div>
        <br /><div class="font_header">STAFF MANAGEMENT</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix120">First Name <span class="font_require">*</span></td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtFirstName" runat="server" Width="290px" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="*Require." ControlToValidate="txtFirstName" class="font_error" SetFocusOnError="True">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Last Name <span class="font_require">*</span></td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtLastName" runat="server" Width="290px" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqValidatorLastName" runat="server" 
                        ErrorMessage="*Require." ControlToValidate="txtLastName" class="font_error" SetFocusOnError="True">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Section <span class="font_require">*</span></td>
                <td class="table_field_td tb_Fix600">
                     <asp:DropDownList ID="ddlSection" runat="server" Width="290px" MaxLength="50">
                        </asp:DropDownList>
                        <button id="toggle"></button>
                     <asp:RequiredFieldValidator ID="reqValidatorSection" runat="server" 
                        ErrorMessage="*Require." ControlToValidate="ddlSection" class="font_error" SetFocusOnError="True">
                     </asp:RequiredFieldValidator>
                </td>
            </tr>
            
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button_style" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear"  CssClass="button_style" CausesValidation="false" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back"  CssClass="button_style" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

