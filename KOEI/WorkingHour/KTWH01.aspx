<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="KTWH01.aspx.vb" Inherits="WorkingHour_KTWH01" %>

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
		$( "#ctl00_MainContent_ddlStaff" ).select();
		$( "#ctl00_MainContent_ddlCategory" ).select();
	});
	</script>
   
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <div class="text_left">
        <br /><div class="font_size_12 text_left"><a class="fix_link" href="KTWH01.aspx?New=True">Working Hour Menagement </a> > Working Hour  </div>
        <br /><div class="font_header">SEARCH WORKING HOUR</div> 
        <br />
        <table class="table_field" style="width:400px">
            <tr>
                <td class="table_field_td" style="width:110px;">
                    &nbsp;Date (dd/mm/yyyy)
                </td>
                <td class="table_field_td" style="width:200px;" >
                    <asp:TextBox ID="txtWorkDate" runat="server"  Width="100" MaxLength="10"></asp:TextBox>
                     <asp:CalendarExtender ID="txtWorkDate_CalendarExtender" runat="server" TargetControlID="txtWorkDate" Format="dd/MM/yyyy">
                     </asp:CalendarExtender>
                     <asp:FilteredTextBoxExtender ID="txtWorkDate_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtWorkDate" 
                         ValidChars="1234567890/" >
                     </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">
                    &nbsp;Staff
                </td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlStaff" runat="server" CssClass="dropdown_field"></asp:DropDownList>
                    <button id="toggle"></button>
                </td>
            </tr>
            <tr>
                <td align="left" class="table_field_td">
                    &nbsp;Category
                </td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="dropdown_field"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>&nbsp;&nbsp;
                    <asp:Button ID="btnReport" runat="server" Text="PDF" CssClass="button_style" />&nbsp;&nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button_style"/>
                </td>
            </tr>
        </table>
        <table class="table_inquiry">
            <tr class="table_head">
                <td class="tb_Fix50"><asp:Label ID="lblHeadEdit" runat="server" Text="Edit" Width="50px" /></td>
                <td class="tb_Fix50"><asp:Label ID="lblHeadDel" runat="server" Text="Delete" Width="50px" /></td>
                <td class="tb_Fix100"><asp:Label ID="lblHeadDate" runat="server" Text="Date" Width="100px" /></td>
                <td class="tb_Fix150"><asp:Label ID="lblHeadStaff" runat="server" Text="Staff" Width="100px" /></td>
                <td class="tb_Fix100"><asp:Label ID="lblHeadCategory" runat="server" Text="Category" Width="100px"/></td>
                <td class="tb_Fix100">08:30</br>- </br>09:00</td>
                <td class="tb_Fix100">09:00</br>-</br> 10:00</td>
                <td class="tb_Fix100">10:00 </br>-</br> 11:00</td>
                <td class="tb_Fix100">11:00 </br>-</br> 12:00</td>
                <td class="tb_Fix100">12:00 </br>-</br> 12:45</td>
                <td class="tb_Fix100">12:45 </br>-</br> 13:00</td>
                <td class="tb_Fix100">13:00 </br>-</br> 14:00</td>
                <td class="tb_Fix100">14:00 </br>-</br> 15:00</td>
                <td class="tb_Fix100">15:00 </br>-</br> 15:15</td>
                <td class="tb_Fix100">15:15 </br>-</br> 15:30</td>
                <td class="tb_Fix100">15:30 </br>-</br> 16:00</td>
                <td class="tb_Fix100">16:00 </br>-</br> 17:00</td>
                <td class="tb_Fix100">17:00 </br>-</br> 17:30</td>
                <td class="tb_Fix100">17:30 </br>-</br> 18:00</td>
                <td class="tb_Fix100">18:00 </br>-</br> 19:00</td>
                <td class="tb_Fix100">19:00 </br>-</br> 20:00</td>
                <td class="tb_Fix100">20:00 </br>- 21:00</td>
                <td class="tb_Fix100">21:00 </br>-</br> 22:00</td>
                <td class="tb_Fix100">22:00 </br>-</br> 23:00</td>
                <td class="tb_Fix100">23:00 </br>-</br> 24:00</td>
                <td class="tb_Fix100">24:00 </br>-</br> 00:45</td>
                <td class="tb_Fix100">00:45 </br>-</br> 01:00</td>
                <td class="tb_Fix100">01:00 </br>-</br> 02:00</td>
                <td class="tb_Fix100">02:00 </br>-</br> 03:00</td>
                <td class="tb_Fix100">03:00 </br>-</br> 03:15</td>
                <td class="tb_Fix100">03:15 </br>-</br> 03:30</td>
                <td class="tb_Fix100">03:30 </br>-</br> 04:00</td>
                <td class="tb_Fix100">04:00 </br>-</br> 05:00</td>
                <td class="tb_Fix100">05:00 </br>-</br> 06:00</td>
                <td class="tb_Fix100">06:00 </br>-</br> 07:00</td>
                <td class="tb_Fix100">07:00 </br>-</br> 08:00</td>
                <td class="tb_Fix100">08:00 </br>-</br> 08:30</td>
            </tr>
            <tbody>
                <asp:Repeater ID="rptWorkingHour" runat="server">
                    <itemtemplate>
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="td_edit tb_Fix50"><asp:LinkButton ID="btnEdit" CommandName="Edit" CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></td>
                            <td class="td_delete tb_Fix50"><asp:LinkButton ID="btnDel" CommandName="Delete" CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                            <td class="text_center tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "work_date")%></div></td>
                            <td class="text_left tb_Fix150"><div><%#DataBinder.Eval(Container.DataItem, "staff_name")%></div></td>
                            <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "category")%></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0830" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0900" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1000" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1100" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1200" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1245" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1300" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1400" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1500" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1515" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1530" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1600" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1700" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1730" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1800" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap1900" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap2000" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap2100" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap2200" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap2300" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0000" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0045" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0100" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0200" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0300" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0315" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0330" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0400" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0500" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0600" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0700" runat ="server"></asp:Label></div></td>
                            <td class="text_left tb_Fix100"><div><asp:Label ID="lblLap0800" runat ="server"></asp:Label></div></td>
                          
                         </tr>
                    </itemtemplate>
                </asp:Repeater> 
            </tbody>
            <tr class="table_head">
                <td colspan = "37">
                    <div class="float_l">
                        <asp:Label ID="lblDescription" runat="server"></asp:Label>
					</div>
					<div class="float_r">
                        <asp:Label ID="lblPaging" runat="server" Text="&nbsp;"></asp:Label>
                    </div>
                </td>
            </tr>              
        </table> 
    </div>
   </asp:Content>
