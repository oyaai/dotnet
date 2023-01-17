<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTJB08.aspx.vb" Inherits="JobOrder_KTJB08" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

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
		$( "#ctl00_MainContent_ddlJobOrderType" ).select();
        $( "#ctl00_MainContent_ddlPersonInCharge" ).select();
	});
	
	</script>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div class="text_center"  style ="width:1000px;">
        <br /><div class="font_size_12 text_left"><a href="KTJB08.aspx?New=True">Job Order</a> > Finish Goods </div>
        <br /><div class="font_header">Finish Goods</div> 
        <br />
        <table class="table_field" style ="width:1000px;">
            <tr>
                <td class="table_field_td tb_Fix170">Job Order</td>
                <td class="table_field_td tb_Fix300">
                    <asp:TextBox ID="txtJobOrderFrom" runat="server" Width="120px" MaxLength="6"></asp:TextBox>&nbsp;-&nbsp;
                    <asp:TextBox ID="txtJobOrderTo" runat="server" Width="120px" MaxLength="6"></asp:TextBox>
                </td> 
                <td class="table_field_td tb_Fix170">&nbsp;Job Order Type</td>
                <td class="table_field_td tb_Fix300">
                    <asp:DropDownList ID="ddlJobOrderType" runat="server" Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">&nbsp;Part No.</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtPartNo" runat="server" Width="150px" MaxLength="255"></asp:TextBox>
                </td>
                <td class="table_field_td">&nbsp;Part Name</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtPartName" runat="server" Width="150px" MaxLength="255"></asp:TextBox>
                </td>
            </tr>  
            <tr>
                <td class="table_field_td">&nbsp;Customer</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtCustomer" runat="server" Width="260px"  MaxLength="255"></asp:TextBox>
                </td>
                <td class="table_field_td">&nbsp;Person in charge</td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlPersonInCharge" runat="server" Width="150px">
                    </asp:DropDownList>
                    <button id="toggle"></button>
                </td>
            </tr>  
            <tr>
                <td class="table_field_td">&nbsp;Issue Date (dd/mm/yyyy)</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtIssueDateFrom" runat="server" Width="120px" MaxLength="10"></asp:TextBox>
                    <asp:CalendarExtender ID="txtIssueDateFrom_CalendarExtender" runat="server" 
                        Format="dd/MM/yyyy"  TargetControlID="txtIssueDateFrom">
                    </asp:CalendarExtender>   
                     <asp:FilteredTextBoxExtender ID="txtIssueDateFrom_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtIssueDateFrom" 
                        ValidChars="1234567890/" >  
                     </asp:FilteredTextBoxExtender>&nbsp;-&nbsp;<asp:TextBox ID="txtIssueDateTo" runat="server" Width="120px" MaxLength="10"></asp:TextBox>
                     <asp:CalendarExtender ID="txtIssueDateTo_CalendarExtender" runat="server" 
                        Format="dd/MM/yyyy"  TargetControlID="txtIssueDateTo">
                    </asp:CalendarExtender> 
                     <asp:FilteredTextBoxExtender ID="txtIssueDateTo_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtIssueDateTo" 
                         ValidChars="1234567890/" >
                     </asp:FilteredTextBoxExtender>  
                </td>
                <td class="table_field_td">&nbsp;Finished Date (dd/mm/yyyy)</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtFinishDateFrom" runat="server" Width="120px" MaxLength="10"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFinishDateFrom_CalendarExtender" runat="server" 
                        Format="dd/MM/yyyy"  TargetControlID="txtFinishDateFrom">
                    </asp:CalendarExtender>   
                     <asp:FilteredTextBoxExtender ID="txtFinishDateFrom_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtFinishDateFrom" 
                        ValidChars="1234567890/" >  
                     </asp:FilteredTextBoxExtender> &nbsp;-&nbsp;<asp:TextBox ID="txtFinishDateTo" runat="server" Width="120px" MaxLength="10"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFinishDateTo_CalendarExtender" 
                        runat="server" TargetControlID="txtFinishDateTo" Format="dd/MM/yyyy">
                     </asp:CalendarExtender>
                     <asp:FilteredTextBoxExtender ID="txtFinishDateTo_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtFinishDateTo" 
                         ValidChars="1234567890/" >
                     </asp:FilteredTextBoxExtender>
                </td>
            </tr>  
            <tr>
                <td class="table_field_td">&nbsp;BOI</td>
                <td class="table_field_td">
                    <asp:radiobuttonlist id="rbtBoi" runat="server" RepeatDirection="Horizontal">
                        <asp:listitem id="rbtBoiAll" runat="server" value="" Text="All"  />
                        <asp:listitem id="rbtBoiYes" runat="server" value="1" Text="BOI" />
                        <asp:listitem id="rbtBoiNo" runat="server" value="2" Text="Non-BOI" />
                   </asp:radiobuttonlist>
                </td>
                <td class="table_field_td">&nbsp;Receive PO</td>
                <td class="table_field_td">
                    <asp:radiobuttonlist id="rbtReceivePO" runat="server" RepeatDirection="Horizontal">
                        <asp:listitem id="rbtReceivePoAll" runat="server" value="" Text="All" />
                        <asp:listitem id="rbtReceivePoYes" runat="server" value="1" Text="Yes" />
                        <asp:listitem id="rbtReceivePoNo" runat="server" value="0" Text="No" />
                   </asp:radiobuttonlist>
                </td>
            </tr> 
            <tr>
                <td colspan="4" class="text_right">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>&nbsp;&nbsp;
                    <asp:Button ID="btnExcel" runat="server" Text="Excel" CssClass="button_style"/>&nbsp;&nbsp;
                </td>
            </tr>         
        </table> 
        <table class="table_inquiry" width="1100">
            <tr class="table_head">
                <td class="tb_Fix100">Job Order</td>
                <td class="tb_Fix100">Finish Date</td>
                <td class="tb_Fix200">Customer</td>
                <td class="tb_Fix100">Job Order Type</td>
                <td class="tb_Fix150">Part Name</td>
                <td class="tb_Fix150">Part No.</td>
                <td class="tb_Fix150">Amount (THB)</td>
                <td class="tb_Fix50">Details</td> 
            </tr>
            <tbody>
                <asp:Repeater ID="rptFinishGoods" runat="server">
                    <itemtemplate>
                        <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                            <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "job_order")%></div></td>
                            <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "finish_date")%></div></td>
                            <td class="text_left tb_Fix200"><div><%#DataBinder.Eval(Container.DataItem, "customer")%></div></td>
                            <td class="text_left tb_Fix100"><div><%#DataBinder.Eval(Container.DataItem, "job_order_type")%></div></td>
                            <td class="text_left tb_Fix150"><div><%#DataBinder.Eval(Container.DataItem, "part_name")%></div></td>
                            <td class="text_left tb_Fix150"><div><%#DataBinder.Eval(Container.DataItem, "part_no")%></div></td>
                            <td class="text_right tb_Fix150"><div><asp:Label ID="lblAmount" runat ="server"><%#DataBinder.Eval(Container.DataItem, "total_amount")%></asp:Label></div></td>
                            <td class="td_edit tb_Fix50">
                                <div>
                                    <asp:LinkButton ID="btnDetail" CommandName="Detail" CssClass="icon_detail1 icon_center15" runat="server"></asp:LinkButton>
                                </div>
                            </td>
                         </tr>
                    </itemtemplate>
                </asp:Repeater> 
            </tbody>
            <tr class="table_head">
                <td colspan = "8">
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

