<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="KTWH02.aspx.vb" Inherits="WorkingHour_KTWH02" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

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
		$( "#ctl00_MainContent_ddlWorkStaff" ).select();
		$( "#ctl00_MainContent_ddlWorkCategory" ).select();
//		$( "#toggle" ).click(function() {
//			$( "#ctl00_MainContent_ddlIECategory" ).toggle();
//		});
	});
	</script>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <div class="text_center">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTWH01.aspx?New=True">Working Hour Menagement </a> > Working Hour  </div>
        <br /><div class="font_header">WORKING HOUR MANAGEMENT</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td" align="left" style="width: 130px">
                    Date (dd/mm/yyyy)<asp:Label ID="lblError" runat="server" Text="*" CssClass="font_require" />
                </td>
                <td class="table_field_td" >
                    <asp:TextBox ID="txtWorkingDate" MaxLength="10" runat="server" />
                    <asp:CalendarExtender ID="txtWorkingDate_CalendarExtender" runat="server" TargetControlID="txtWorkingDate" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="txtWorkingDate_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtWorkingDate" 
                        ValidChars="1234567890/" >
                    </asp:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="reqValidatorWorkingDate" ControlToValidate="txtWorkingDate"
                       Display="Dynamic" runat="server" ErrorMessage="Require." SetFocusOnError="True"
                       ValidationGroup="WorkingHour"  />
                </td>
            </tr>
            <tr>
                <td align="left" class="table_field_td">
                    Staff <span class="font_require">*</span>
                </td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlWorkStaff" runat="server" Width="170px" >
                    </asp:DropDownList>
                    <button id="toggle"></button>
                    <asp:RequiredFieldValidator ID="reqValidatorStaff" ControlToValidate="ddlWorkStaff"
                        Display="Dynamic" runat="server" ErrorMessage="Require." SetFocusOnError="True"
                        ValidationGroup="WorkingHour" />
                </td>
            </tr>
            <tr>
                <td align="left" class="table_field_td">
                    Category <span class="font_require">*</span>
                </td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlWorkCategory" Width="170px" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqValidatorCategory" ControlToValidate="ddlWorkCategory"
                        Display="Dynamic" runat="server" ErrorMessage="Require" SetFocusOnError="True"
                        ValidationGroup="WorkingHour" />
                </td>
            </tr>
            <tr>
                <td colspan="2"  class="text_right" >
                    <asp:Button ID="btnAddWorkingHour" Text="Add Working Hour" CssClass="button_style submit_workinghour"
                        Width="130px" runat="server" ValidationGroup="WorkingHour" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnBack1" runat="server" Text="Back"  CssClass="button_style" CausesValidation="false" />
                </td>
            </tr>
        </table>
        <br />    
        <div id="pnlWorkingHourDetail" runat="server" >
            <table class="table_field">
                <tr>
                    <td class="table_field_td text_center " style="width: 130px">
                        Time
                    </td>
                    <td class="table_field_td">
                        &nbsp; Job Order
                    </td>
                    <td class="table_field_td">
                        Detail
                    </td>
                </tr>
                <tr>
                    <td class="table_field_td text_center"> 
                        08:30 - 09:00
                        <asp:HiddenField ID="hidDetailId1" runat="server" value=""/>
                    </td>
                    <td class="table_field_td">
                        <asp:TextBox ID="txtJobOrder1" runat="server" MaxLength="6" Width="83px" />
                    </td>
                    <td class="table_field_td">
                        <asp:TextBox ID="txtDetail1" runat="server" MaxLength="255" Width="360px" />
                    </td>
                </tr>
            <tr>
                <td class="table_field_td  text_center">
                    09:00 - 10:00
                    <asp:HiddenField ID="hidDetailId2" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder2" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail2" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td  text_center">
                    10:00 - 11:00
                    <asp:HiddenField ID="hidDetailId3" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder3" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail3" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td  text_center">
                    11:00 - 12:00
                    <asp:HiddenField ID="hidDetailId4" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder4" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail4" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td  text_center">
                    12:00 - 12:45
                </td>
                <td align="center" colspan="2" class="table_field_td">
                    Break Time
                </td>
            </tr>
            <tr>
                <td class="table_field_td  text_center">
                    12:45 - 13:00
                    <asp:HiddenField ID="hidDetailId5" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder5" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail5" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td  text_center">
                    13:00 - 14:00
                    <asp:HiddenField ID="hidDetailId6" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder6" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail6" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td  text_center">
                    14:00 - 15:00
                    <asp:HiddenField ID="hidDetailId7" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder7" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail7" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td  text_center">
                    15:00 - 15:15
                    <asp:HiddenField ID="hidDetailId8" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder8" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail8" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td  text_center">
                    15:15 - 15:30
                </td>
                <td align="center" colspan="2" class="table_field_td">
                    Break Time
                </td>
            </tr>
            <tr>
                <td class="table_field_td  text_center">
                    15:30 - 16:00
                    <asp:HiddenField ID="hidDetailId9" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder9" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail9" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td  text_center">
                    16:00 - 17:00
                    <asp:HiddenField ID="hidDetailId10" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder10" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail10" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td  text_center">
                    17:00 - 17:30
                    <asp:HiddenField ID="hidDetailId11" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder11" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail11" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td  class="table_field_td  text_center">
                    17:30 - 18:00
                </td>
                <td align="center" colspan="2" class="table_field_td">
                    Break Time
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    18:00 - 19:00
                    <asp:HiddenField ID="hidDetailId12" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder12" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail12" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    19:00 - 20:00
                    <asp:HiddenField ID="hidDetailId13" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder13" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail13" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    20:00 - 21:00
                    <asp:HiddenField ID="hidDetailId14" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder14" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail14" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    21:00 - 22:00
                    <asp:HiddenField ID="hidDetailId15" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder15" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail15" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    22:00 - 23:00
                    <asp:HiddenField ID="hidDetailId16" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder16" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail16" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    23:00 - 24:00
                    <asp:HiddenField ID="hidDetailId17" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder17" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail17" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td  class="table_field_td text_center">
                    00:00 - 00:45
                </td>
                <td align="center" colspan="2" class="table_field_td">
                    Break Time
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    00:45 - 01:00
                    <asp:HiddenField ID="hidDetailId18" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder18" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail18" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    01:00 - 02:00
                    <asp:HiddenField ID="hidDetailId19" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder19" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail19" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td  class="table_field_td text_center">
                    02:00 - 03:00
                    <asp:HiddenField ID="hidDetailId20" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder20" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail20" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    03:00 - 03:15
                    <asp:HiddenField ID="hidDetailId21" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder21" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail21" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    03:15 - 03:30
                </td>
                <td align="center" colspan="2" class="table_field_td">
                    Break Time
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    03:30 - 04:00
                    <asp:HiddenField ID="hidDetailId22" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder22" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail22" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td  class="table_field_td text_center">
                    04:00 - 05:00
                    <asp:HiddenField ID="hidDetailId23" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder23" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail23" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td  class="table_field_td text_center">
                    05:00 - 06:00
                    <asp:HiddenField ID="hidDetailId24" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder24" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail24" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    06:00 - 07:00
                    <asp:HiddenField ID="hidDetailId25" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder25" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail25" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    07:00 - 08:00
                    <asp:HiddenField ID="hidDetailId26" runat="server" value=""/>
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtJobOrder26" runat="server" MaxLength="6" Width="83px" />
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail26" runat="server" MaxLength="255" Width="360px" />
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center">
                    08:00 - 08:30
                </td>
                <td align="center" colspan="2" class="table_field_td">
                    Break Time
                </td>
            </tr>
            <tr>
                <td colspan="3" class="text_right">
                    <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button_style" ValidationGroup="WorkingHour" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear"  CssClass="button_style" CausesValidation="false" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back"  CssClass="button_style" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
    </div>
    </asp:Content>