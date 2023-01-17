<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTAD04.aspx.vb" Inherits="Admin_KTAD04" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script> 
<script type="text/javascript" src="../Scripts/jsCommon.js"></script>


<link type="text/css" rel="Stylesheet" href="../Scripts/themes/base/jquery.ui.all.css" />
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
		$( "#ctl00_MainContent_ddlUserName" ).select({
		    select: function(event, ui ) {
                theForm.submit();     
		    }
		});
	});
	</script>

<script language="javascript" type="text/javascript">



   
</script>
    
    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTAD03.aspx?New=True">Admin Management</a> > User Permission Management</div>
        <br />
        <div class="font_header">USER PERMISSION MANAGEMENT</div> 
        <br />
        <table>
        <tr><td class="text_left" style="width: 750px;">
        <table class="table_field text_left"  style="width: 400px;">
            <tr>
                <td class="table_field_td tb_Fix80"> User Name</td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlUserName" runat="server" Width="150" 
                        AutoPostBack="True">                        
                    </asp:DropDownList>
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="* Required" SetFocusOnError="True" 
                        style="font-weight: 700; font-style: italic" 
                        ControlToValidate="ddlUserName"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix80">First Name</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="150" Width="150" 
                        ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix80">Last Name</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="150" Width="150" 
                        ReadOnly="True"></asp:TextBox>
                </td>
            </tr> 
            <tr>
                <td class="table_field_td tb_Fix80">Department</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDepartment" runat="server" MaxLength="150" Width="150" 
                        ReadOnly="True"></asp:TextBox>
                </td>
            </tr>        
        </table> 
        </td></tr>
        </table>
        <br />
        
        <table class="table_inquiry" width="750">
            <tr><td colspan="8" class="text_right"><asp:CheckBox ID="checkAll" runat="server" 
                    Text="Select All" AutoPostBack="True" /></td></tr>
            <tr class="table_head">
                <%--<td colspan="2">Action</td>--%>
                <td class="text_center tb_Fix300">Menu Name</td>
                <td class="text_center tb_Fix60">Create</td>
                <td class="text_center tb_Fix60">Update</td>
                <td class="text_center tb_Fix60">Delete</td>
                <td class="text_center tb_Fix60">List</td>
                <td class="text_center tb_Fix60">Amount</td>
                <td class="text_center tb_Fix60">Comfirm</td>
                <td class="text_center tb_Fix60">Approve</td>
            </tr>
            <asp:Repeater ID="rptUserPermission" runat="server">
                <itemtemplate>
                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                        <td class="text_left tb_Fix300"><div><%#DataBinder.Eval(Container.DataItem, "menu_text")%></div></td>
                        <td class="text_center tb_Fix60"><div><asp:CheckBox ID="cbFn_Create" runat="server" Visible='<%#IIf(DataBinder.Eval(Container.DataItem, "Fn_Create") = 1, True, False)%>'></asp:CheckBox></div></td>
                        <td class="text_center tb_Fix60"><div><asp:CheckBox ID="cbFn_Update" runat="server" Visible='<%#IIf(DataBinder.Eval(Container.DataItem, "Fn_Update") = 1, True, False)%>'></asp:CheckBox></div></td>
                        <td class="text_center tb_Fix60"><div><asp:CheckBox ID="cbFn_Delete" runat="server" Visible='<%#IIf(DataBinder.Eval(Container.DataItem, "Fn_Delete") = 1, True, False)%>'></asp:CheckBox></div></td>
                        <td class="text_center tb_Fix60"><div><asp:CheckBox ID="cbFn_List" runat="server" Visible='<%#IIf(DataBinder.Eval(Container.DataItem, "Fn_List") = 1, True, False)%>'></asp:CheckBox></div></td>
                        <td class="text_center tb_Fix60"><div><asp:CheckBox ID="cbFn_Amount" runat="server" Visible='<%#IIf(DataBinder.Eval(Container.DataItem, "Fn_Amount") = 1, True, False)%>'></asp:CheckBox></div></td>
                        <td class="text_center tb_Fix60"><div><asp:CheckBox ID="cbFn_Confirm" runat="server" Visible='<%#IIf(DataBinder.Eval(Container.DataItem, "Fn_Confirm") = 1, True, False)%>'></asp:CheckBox></div></td>
                        <td class="text_center tb_Fix60"><div><asp:CheckBox ID="cbFn_Approve" runat="server" Visible='<%#IIf(DataBinder.Eval(Container.DataItem, "Fn_Approve") = 1, True, False)%>'></asp:CheckBox></div></td>
                    </tr> 
                </itemtemplate>
            </asp:Repeater>   
            <tr class="table_head">
                <td class="text_right" colspan="8">&nbsp;</td>
            </tr>   
            <tr>
                <td class="text_right" colspan="8">
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

