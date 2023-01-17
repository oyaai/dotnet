<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTPU08.aspx.vb" Inherits="KTPU08"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
		height: 1.9em;
		top: 0.1em;
	}
	.custom-select-input {
		margin: 0;
		padding: 0.3em;
	}
	</style>
	
	<script type="text/javascript">
	    (function($) {
	        $.widget("custom.select", {
	            _create: function() {
	                this.wrapper = $("<span>")
					.addClass("custom-select")
					.insertAfter(this.element);

	                this.element.hide();
	                this._createAutocomplete();
	                this._createShowAllButton();
	            },

	            _createAutocomplete: function() {
	                var selected = this.element.children(":selected"),
					value = selected.val() ? selected.text() : "";

	                this.input = $("<input>")
					.appendTo(this.wrapper)
					.val(value)
					.attr("title", "")
					.addClass("custom-select-input ui-widget ui-widget-content ui-state-default ui-corner-left")
					.autocomplete({
					    delay: 0,
					    minLength: 0,
					    source: $.proxy(this, "_source")
					})
					.tooltip({
					    tooltipClass: "ui-state-highlight"
					});

	                this._on(this.input, {
	                    autocompleteselect: function(event, ui) {
	                        ui.item.option.selected = true;
	                        this._trigger("select", event, {
	                            item: ui.item.option
	                        });
	                    },

	                    autocompletechange: "_removeIfInvalid"
	                });
	            },

	            _createShowAllButton: function() {
	                var input = this.input,
					wasOpen = false;

	                $("<a>")
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.tooltip()
					.appendTo(this.wrapper)
					.button({
					    icons: {
					        primary: "ui-icon-triangle-1-s"
					    },
					    text: false
					})
					.removeClass("ui-corner-all")
					.addClass("custom-select-toggle ui-corner-right")
					.mousedown(function() {
					    wasOpen = input.autocomplete("widget").is(":visible");
					})
					.click(function() {
					    input.focus();

					    // Close if already visible
					    if (wasOpen) {
					        return;
					    }

					    // Pass empty string as value to search for, displaying all results
					    input.autocomplete("search", "");
					});
	            },

	            _source: function(request, response) {
	                var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
	                response(this.element.children("option").map(function() {
	                    var text = $(this).text();
	                    if (this.value && (!request.term || matcher.test(text)))
	                        return {
	                            label: text,
	                            value: text,
	                            option: this
	                        };
	                }));
	            },

	            _removeIfInvalid: function(event, ui) {

	                // Selected an item, nothing to do
	                if (ui.item) {
	                    return;
	                }

	                // Search for a match (case-insensitive)
	                var value = this.input.val(),
					valueLowerCase = value.toLowerCase(),
					valid = false;
	                this.element.children("option").each(function() {
	                    if ($(this).text().toLowerCase() === valueLowerCase) {
	                        this.selected = valid = true;
	                        return false;
	                    }
	                });

	                // Found a match, nothing to do
	                if (valid) {
	                    return;
	                }

	                // Remove invalid value
	                this.input
					.val("")
					.attr("title", value + " didn't match any item")
					.tooltip("open");
	                this.element.val("");
	                this._delay(function() {
	                    this.input.tooltip("close").attr("title", "");
	                }, 2500);
	                this.input.data("ui-autocomplete").term = "";
	            },

	            _destroy: function() {
	                this.wrapper.remove();
	                this.element.show();
	            }
	        });
	    })(jQuery);

	    $(function() {
	        $("#ctl00_MainContent_ddlVendor").select();
	        //		$( "#toggle" ).click(function() {
	        //			$( "#ctl00_MainContent_ddlVendor" ).toggle();
	        //		});
	    });
	</script>

<script type="text/javascript">

    function waitProcess(txt) {
        DivWait();
    }

    function DivWait() {
        setTimeout('disableAfterTimeout()', 0);
        processing = true;
        return false;
    }

    function disableAfterTimeout() {
        document.getElementById('coverit').style.visibility = 'visible';
        document.getElementById('coverit').focus();
    }

    function closeDiv() {
        document.getElementById('coverit').style.visibility = 'hidden';
        document.aspnetForm.submit();
    }  
    
</script>

<script type="text/javascript">
    function CheckCurrentAc() {
        document.getElementById("<%= txtBankName.ClientID %>").value = "";
        document.getElementById("<%= txtAccountNo.ClientID %>").value = "";
        document.getElementById("<%= txtAccountName.ClientID %>").value = "";
    }
    function CheckSavingAc() {
        document.getElementById("<%= txtChequeNo.ClientID %>").value = "";
    }
    function CheckCash() {
        document.getElementById("<%= txtBankName.ClientID %>").value = "";
        document.getElementById("<%= txtAccountNo.ClientID %>").value = "";
        document.getElementById("<%= txtAccountName.ClientID %>").value = "";
        document.getElementById("<%= txtChequeNo.ClientID %>").value = "";
    }
</script>

<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div id="coverit" onkeydown="if(event.keyCode == 13 || event.keyCode == 9) return false;" style="cursor:wait;position:absolute;background-color:#FAFCF5;filter: Alpha(Opacity=90);z-index:1000;visibility:hidden;width:100%;height:100%;left:0px;top:120px;" >
            <div id="splashscreen" style="position:absolute;z-index:5;top:20%;left:35%;">
                
            </div>
        </div> 
       
        <br />
        <br /><div class="font_size_12 text_left"><a href="KTPU07.aspx?New=True">Accounting</a> > Purchase Payment</div>
        <br /><div class="font_header">PURCHASE PAYMENT MANAGEMENT</div> 
        <br />

         <asp:Panel ID="panelSearch" runat="server" >
            <div class="text_left">
            <table class="table_common" width="1000px" >
                <tr>
                    <td class="table_field_td">Vendor Name</td>
                    <td class="table_field_td">
                            <asp:DropDownList ID="ddlVendor" runat="server" CssClass="dropdown_field"></asp:DropDownList>
                            <button id="toggle" ></button>
                    </td>
                    <td class="table_field_td" >Payment Date</td>
                    <td class="table_field_td" >
                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="dropdown_field" Width="100px">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="01">January</asp:ListItem>
                        <asp:ListItem Value="02">February</asp:ListItem>
                        <asp:ListItem Value="03">March</asp:ListItem>
                        <asp:ListItem Value="04">April</asp:ListItem>
                        <asp:ListItem Value="05">May</asp:ListItem>
                        <asp:ListItem Value="06">June</asp:ListItem>
                        <asp:ListItem Value="07">July</asp:ListItem>
                        <asp:ListItem Value="08">August</asp:ListItem>
                        <asp:ListItem Value="09">September</asp:ListItem>
                        <asp:ListItem Value="10">October</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList>&nbsp;-&nbsp;<asp:DropDownList ID="ddlYear" runat="server" CssClass="dropdown_field" Width="100px"></asp:DropDownList>
<!-- 
 -->
                       <asp:TextBox ID="txtPaymentDate" runat="server" Width="95px" MaxLength="10" Visible="False"></asp:TextBox>
                       <asp:CalendarExtender ID="txtPaymentDate_CalendarExtender" TargetControlID="txtPaymentDate" Format="dd/MM/yyyy" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="text_right">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button_style"/>&nbsp;&nbsp;
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="button_style"/>
                    </td>
                </tr>         
            </table>
            </div>  
        </asp:Panel> 
        
        <asp:Panel ID="Panel1" runat="server" >
        <table class="table_common" width="1000px">
            <tr>
                <td class="table_field_td_head" colspan="4">Header</td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">Vendor Name&nbsp;&nbsp;&nbsp;<asp:Label ID="lblVendorName" runat="server"></asp:Label></td>
                <td class="table_field_td" colspan="2">Vendor Type&nbsp;&nbsp;&nbsp;<asp:Label ID="lblVendorType" runat="server"></asp:Label></td>
            </tr> 
            <tr>
                 <td class="table_field_td" >
                    Pay Method : <span class="font_require">*</span><br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoCurrentAc" runat="server" GroupName="account_type" Text="Current Account" onclick="javascript:CheckCurrentAc();"></asp:RadioButton>
                 </td> 
                 <td class="table_field_td" colspan="3" >
                    <br />
                    Cheque No. &nbsp;<span class="font_require">*</span>
                    <asp:TextBox ID="txtChequeNo" runat="server" MaxLength="100" ></asp:TextBox>
                 </td>
      
            </tr>
            <tr>
                 <td class="table_field_td" >
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoSavingAc" runat="server" GroupName="account_type" Text="Saving Account" onclick="javascript:CheckSavingAc();"></asp:RadioButton>
                 </td> 
                 <td class="table_field_td" >
                    Bank Name &nbsp;<span class="font_require">*</span>
                    <asp:TextBox ID="txtBankName" runat="server" MaxLength="50" ></asp:TextBox>
                 </td>  
                 <td class="table_field_td" >
                    Account No. &nbsp;<span class="font_require">*</span>
                    <asp:TextBox ID="txtAccountNo" runat="server" MaxLength="100" ></asp:TextBox>
                 </td> 
                 <td class="table_field_td" >
                    Account Name &nbsp;<span class="font_require">*</span>
                    <asp:TextBox ID="txtAccountName" runat="server" MaxLength="100" ></asp:TextBox>
                 </td> 
                 
            </tr>
            <tr>
                 <td class="table_field_td" colspan="4" >
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoCash" runat="server" GroupName="account_type" Text="Cash" onclick="javascript:CheckCash();"></asp:RadioButton>
                 </td> 
            </tr>
           <tr>
                <td class="table_field_td" colspan="2" >
                    Pay Date (dd/mm/yyyy) <span class="font_require">*</span>&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtChequeDate" runat="server" Width="95px" MaxLength="10"></asp:TextBox>
                    <asp:CalendarExtender ID="txtChequeDate_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtChequeDate" runat="server" />
                    <asp:RequiredFieldValidator ID="txtChequeDate_RequiredFieldValidator" runat="server" ControlToValidate="txtChequeDate" ValidationGroup="btnCreate" SetFocusOnError="True" ErrorMessage="*Require" />
                 </td>
                 <td class="table_field_td" colspan="2">Bank Rate : &nbsp;&nbsp;&nbsp; 
                    <asp:TextBox ID="txtBankRate" runat="server" Width="95px" AutoPostBack="true" OnTextChanged="CalBankRate" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
        </table> 
        <br /><br />
        
        <table class="table_inquiry" width="1040px" id="tbInquiry">
            <tr>
            <!-- ping -->            
                <td class="table_field_td_head" colspan="11">Detail</td>
            </tr>
            <tr class="table_head">
                <td style="width:35px;">Select</td>
                <td style="width:85px;">Voucher No.</td>
                <td style="width:140px;">Invoice No.</td>
                <td style="width:110px;">Amount<br />(Schedule rate)</td>
                <td style="width:100px;">Amount<br />(Bank rate)</td>
                <td style="width:40px;">Vat</td>
                <td style="width:70px;">Vat(Baht)</td>
                <td style="width:40px;">W/T</td>
                <td style="width:70px;">W/T(Baht)</td>
                <td style="width:65px;">Payment Date</td>
                <td style="width:65px;">Payment From</td>
            </tr>
            <tbody>
            <asp:Repeater ID="rptInquery" runat="server">
                <itemtemplate>                        
                    <tr class='<%# IIf(Container.ItemIndex Mod 2 = 0, "table_item", "table_alter") %>'>
                        <td class="text_center" >
                            <label>
                                <asp:checkbox ID="chkCheque" CommandName="Checkbox" runat="server"></asp:checkbox>
                            </label>
                        </td>
                        <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "voucher_no")%></div></td>
                        <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "invoice_no")%></div></td>
                        <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "sub_total")%></div></td>
                        <td class="text_right">
                            <div>
                                <asp:TextBox ID="txtBank_rate" Width="75" MaxLength="16" runat="server" 
                                OnTextChanged="txtBank_rate_TextChanged" onchange = "waitProcess(this);" AutoPostback="true"
                                Text='<%#DataBinder.Eval(Container.DataItem, "amount_bank")%>' style="text-align:right">
                                </asp:TextBox>
                            </div>
                        </td>
                        <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "vat_name")%></div></td>
                        <td class="text_right">
                            <div>
                                <asp:TextBox ID="txtVat_amount" Width="75"  MaxLength="16" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "vat_amount")%>' style="text-align:right"></asp:TextBox>
                            </div>
                        </td>
                       
                        <td class="text_right"><div><%#DataBinder.Eval(Container.DataItem, "wt_name")%></div></td>
                        <td class="text_right">
                            <div>
                                <asp:TextBox ID="txtWt_amount" Width="75" MaxLength="16" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "wt_amount")%>' style="text-align:right"></asp:TextBox>
                            </div>
                        </td>
                        <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "payment_date")%></div></td>
                        <td class="text_left"><div><%#DataBinder.Eval(Container.DataItem, "payment_from")%></div></td>
                    </tr>
                </itemtemplate>
            </asp:Repeater> 
            </tbody> 
            </table>
            
	        <table class="table_inquiry" width="1040px" id="Table1">
                <tr>
                    <td class="table_field_td_head">&nbsp;
                        <asp:Label ID="Label1" runat="server" Width="345px"></asp:Label>
                        <asp:Label ID="lblSumScheduleRate" runat="server" Width="110px" text="0.00" style="color:white;font-weight:bold;text-align:right"></asp:Label>
                        <asp:Label ID="lblSumBankRate" runat="server" Width="100px" text="0.00" style="color:white;font-weight:bold;text-align:right"></asp:Label>
                        <asp:Label ID="Label2" runat="server" Width="45px"></asp:Label>
                        <asp:Label ID="lblSumVat" runat="server" Width="90px" text="0.00" style="color:white;font-weight:bold;text-align:right"></asp:Label>
                        <asp:Label ID="Label3" runat="server" Width="44px"></asp:Label>
                        <asp:Label ID="lblSumWT" runat="server" Width="87px" text="0.00" style="color:white;font-weight:bold;text-align:right"></asp:Label>                
                    </td>
                </tr>
	        </table>        

        <table class="table_inquiry">
            <tr>
                <td class="text_right">
                    <asp:Button ID="btnCreate" runat="server" Text="Create" ValidationGroup="btnCreate" CssClass="button_style" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear"  CssClass="button_style" CausesValidation="false" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnBackEdit" runat="server" Text="Back" CssClass="button_style"/>
                </td>
            </tr>  
        </table>

</asp:Panel> 
    
</asp:Content>

