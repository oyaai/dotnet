<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS29.aspx.vb" Inherits="Master_KTMS29" %>
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
		$( "#ctl00_MainContent_ddlSearchCurrency" ).select();
//		$( "#toggle" ).click(function() {
//			$( "#ctl00_MainContent_ddlSearchCurrency" ).toggle();
//		});
	});
	</script>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS29.aspx?New=True">Master Management</a> > Schedule Rate</div>
        <br />
        <div class="font_header">SEARCH SCHEDULE RATE</div> 
        <br />
        
        <table class="table_field" width="800">
            <tr>
                <td class="table_field_td tb_Fix100">Currency</td>
                <td class="table_field_td tb_Fix300">
                    <asp:DropDownList ID="ddlSearchCurrency" runat="server" CssClass="dropdown_field">                        
                    </asp:DropDownList>
                    <button id="toggle"></button>
                    <asp:Label ID="lblMsgCurrency" runat="server" Text="*Require" CssClass="font_require" Visible="false" ></asp:Label>
                    <%--<asp:RequiredFieldValidator ID="reqCurrency" runat="server" ErrorMessage="*Require" InitialValue="0" Display="Dynamic"
                        ControlToValidate="ddlSearchCurrency" >
                    </asp:RequiredFieldValidator>--%>
                </td>
                <%--เพิ่มข้อมูลใหม่ตามสเป็กพี่ปิง--%>
                <td class="table_field_td tb_Fix100">Effective Date</td>
                <td class="table_field_td tb_Fix300">
                    <asp:TextBox ID="txtEF_Date" MaxLength="10" runat="server" CssClass="text_field"></asp:TextBox>
                    <asp:Label ID="lblMsgEF_date" runat="server" Text="*Require" CssClass="font_require" Visible="false"></asp:Label>
                    <%--<asp:RequiredFieldValidator ID="reqEF_Date" Display="Dynamic"
                        runat="server" ControlToValidate="txtEF_Date" ErrorMessage="*Require" >
                    </asp:RequiredFieldValidator>--%>
                    <asp:CalendarExtender ID="txtEF_Date_CalendarExtender"
                        runat="server" Format="dd/MM/yyyy" TargetControlID="txtEF_Date" >
                    </asp:CalendarExtender>                    
                    <asp:FilteredTextBoxExtender ID="txtEF_Date_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtEF_Date" ValidChars="1234567890/" >
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>
                    &nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button_style" CausesValidation="false" />
                </td>
            </tr>
        </table>
        <br /><br />
        
        <table class="table_inquiry" width="550">
            <tr class="table_head">
                <td class="tb_Fix50 text_center">Edit</td>
                <td class="tb_Fix50 text_center">Delete</td>
                <td class="tb_Fix50 text_center">ID</td>
                <td class="tb_Fix150 text_center">Currency</td>
                <td class="tb_Fix150 text_center">Effective Date</td>
                <td class="tb_Fix100 text_center">Rate</td>
            </tr>
            <asp:Repeater ID="rptScheculeRate" OnItemCommand="rptScheculeRate_ItemCommand" runat="server">
                <itemtemplate>
                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                        <td class="tb_Fix50"><asp:LinkButton ID="btnEdit" CausesValidation="false" CommandName="Edit" CommandArgument='<%# Eval("id")%>' CssClass="icon_edit1 icon_center15" runat="server"></asp:LinkButton></td>
                        <td class="tb_Fix50"><asp:LinkButton ID="btnDel" CausesValidation="false" CommandName="Del" CommandArgument='<%# Eval("id")%>' CssClass="icon_del1 icon_center15" runat="server"></asp:LinkButton></td>
                        <td class="text_center tb_Fix50"><div><%#DataBinder.Eval(Container.DataItem, "id")%></div></td>
                        <td class="text_center tb_Fix150"><div><%#DataBinder.Eval(Container.DataItem, "currency")%></div></td>
                        <td class="text_center tb_Fix150"><div><%#DataBinder.Eval(Container.DataItem, "ef_date")%></div></td>
                        <td class="text_right tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "rate")%></div></td>
                    </tr> 
                </itemtemplate>
            </asp:Repeater>            
            <tr class="table_head">
                <td colspan = "6">
                    <div class="float_l"><asp:Label ID="lblFootTB1" runat="server" Text="&nbsp;" ></asp:Label></div>
                    <div class="float_r"><asp:Label ID="lblFootTB2" runat="server" Text="&nbsp;" ></asp:Label></div>
                </td>
            </tr>   
        </table>
    </div>
</asp:Content>

