<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS01.aspx.vb" Inherits="Master_KTMS01" %>

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
		$( "#ctl00_MainContent_ddlSearchCountry" ).select();
//		$( "#toggle" ).click(function() {
//			$( "#ctl00_MainContent_ddlSearchCountry" ).toggle();
//		});
	});
	</script>
    
    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS01.aspx?New=True">Master Management</a> > Vendor</div>
        <br />
        <div class="font_header">VENDOR SEARCH</div> 
        <br />
        <table class="table_field" style="width:900px;">
            <tr>
                <td class="table_field_td tb_Fix100"> Type1</td>
                <td class="table_field_td tb_Fix350">
                    <asp:RadioButtonList ID="rbtnSearchType1" runat="server" 
                        RepeatDirection="Horizontal" BorderStyle="None">
                        <asp:ListItem Value="-1" Selected="True" Text="All"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Vendor"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Customer"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td class="table_field_td tb_Fix100">Type2</td>
                <td class="table_field_td tb_Fix350">
                    <asp:RadioButtonList ID="rbtnSearchType2" runat="server" 
                        RepeatDirection="Horizontal" BorderStyle="None">
                        <asp:ListItem Value="-1" Selected="True" Text="All"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Person"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Company"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix100">Name</td>
                <td class="table_field_td tb_Fix350">
                    <asp:TextBox ID="txtSearchName" runat="server" MaxLength="150"></asp:TextBox>
                </td>
                <td class="table_field_td tb_Fix100">Country</td>
                <td class="table_field_td tb_Fix350">
                    <asp:DropDownList ID="ddlSearchCountry" runat="server" Width="100">                        
                    </asp:DropDownList>
                </td>
            </tr>  
            <tr>
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>
                    &nbsp;
                    <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button_style"/>
                    &nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button_style"/>
                </td>
            </tr>         
        </table> 
        <br /><br />
        <table class="table_inquiry" width="900">
            <tr class="table_head">
                <%--<td colspan="2">Action</td>--%>
                <td class="tb_Fix50">Edit</td>
                <td class="tb_Fix50">Delete</td>
                <td class="tb_Fix50">ID</td>
                <td class="tb_Fix120">Type1</td>
                <td class="tb_Fix120">Type2</td>
                <td class="tb_Fix300">Name</td>
                <td class="tb_Fix120">Country</td>
                <td class="tb_Fix50">Details</td>
            </tr>
            <asp:Repeater ID="rptVendor" OnItemCommand="rptVendor_ItemCommand" runat="server">
                <itemtemplate>
                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                        <td class="tb_Fix50"><asp:LinkButton ID="btnEdit" CausesValidation="false" CommandName="Edit" CommandArgument='<%# Eval("id")%>' CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></td>
                        <td class="tb_Fix50"><asp:LinkButton ID="btnDel" CausesValidation="false" CommandName="Del" CommandArgument='<%# Eval("id")%>' CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                        <td class="text_center tb_Fix50"><div><%#DataBinder.Eval(Container.DataItem, "id")%></div></td>
                        <td class="text_left tb_Fix120"><div><%#DataBinder.Eval(Container.DataItem, "type1_text")%></div></td>
                        <td class="text_left tb_Fix120"><div><%#DataBinder.Eval(Container.DataItem, "type2_text")%></div></td>
                        <td class="text_left tb_Fix300"><div><%#DataBinder.Eval(Container.DataItem, "name")%></div></td>
                        <td class="text_left tb_Fix120"><div><%#DataBinder.Eval(Container.DataItem, "country_name")%></div></td>
                        <td class="tb_Fix50"><asp:LinkButton ID="btnDetails" CausesValidation="false" CommandName="View" CommandArgument='<%# Eval("id")%>' CssClass="icon_detail1 icon_center15" runat="server"></asp:LinkButton></td>
                    </tr> 
                </itemtemplate>
            </asp:Repeater>            
            <tr class="table_head">
                <td colspan = "8">
                    <div class="float_l"><asp:Label ID="lblFootTB1" runat="server" Text="0" ></asp:Label></div>
                    <div class="float_r"><asp:Label ID="lblFootTB2" runat="server" Text="0" ></asp:Label></div>
                </td>
            </tr>   
        </table>
    </div>
</asp:Content>

