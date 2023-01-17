<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTJB02.aspx.vb" Inherits="JobOrder_KTJB02" MaintainScrollPositionOnPostback="true" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
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
		*Width: 30.9em;
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
						alert(this.selected);
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
			},
			
		});
	})( jQuery );
    
	$(function() {
		$( "#ctl00_MainContent_ddlCustomer" ).select({
		    select: function(event, ui ) {
                theForm.submit();     
		    }
		 });
		 
        $( "#ctl00_MainContent_ddlEndUser" ).select();
        $( "#ctl00_MainContent_ddlPersonCharge" ).select();

         $("#ctl00_MainContent_ddlCustomer option:selected").change(function () {
            
           // alert($('option:selected', $(this)).text());
        });
	});
	


	

	</script>


<script type="text/javascript" src="../Scripts/jsCommon.js"></script>
<script type="text/javascript">
    
    function onChange(val){
        var ddl = document.getElementById('<%= ddlCustomer.ClientID %>');
        var ddl2 = document.getElementById('<%= ddlEndUser.ClientID %>');
        if(val!= ""){
             ddl.value = ddl2.value;
        }
    }
    
    
    //Check Require of 1st Hontai checkbox
    function checkHontai1(source, args)
    {
        var elem = document.getElementById('<%= ChkHontai1.ClientID %>');
        if (elem.checked)
        {
            args.IsValid = true;
        }
        else
        {
            args.IsValid = false;
        }    
    }
    //Check Require of 2nd Hontai checkbox
    function checkHontai2(source, args)
    {
        var elem = document.getElementById('<%= ChkHontai2.ClientID %>');
        if (elem.checked)
        {
            args.IsValid = true;
        }
        else
        {
            args.IsValid = false;
        }    
    }
    //Check Require of 3rd Hontai checkbox
    function checkHontai3(source, args)
    {
        var elem = document.getElementById('<%= ChkHontai3.ClientID %>');
        if (elem.checked)
        {
            args.IsValid = true;
        }
        else
        {
            args.IsValid = false;
        }    
    }
        
    //Check data for check RequiredFieldValidator on CreateAt Item
    function GetDropDownValue()
    {
        //in val u get dropdown list selected value
        var IndexValue = document.getElementById('<%=ddlJobOrder.ClientID %>').selectedIndex;
        var SelectedVal = document.getElementById('<%=ddlJobOrder.ClientID %>').options[IndexValue].text;
        var lblReqCreateAt = document.getElementById('<%=lblReqCreateAt.ClientID %>');
        var RLCompareParameter = document.getElementById("<%=rbtCreateAt.ClientID %>");
        var rbtCreateAt = RLCompareParameter.getElementsByTagName('input');
        if (SelectedVal == "Modification" || SelectedVal == "Repair" )
        {   
            document.getElementById('<%=lblChkReq.ClientID%>').style.display = 'block';
            rbtCreateAt[0].disabled = false ; 
            rbtCreateAt[1].disabled = false;                  
        } 
        else{          
            document.getElementById('<%=lblChkReq.ClientID%>').style.display = 'none';
            if (SelectedVal == "New Mold" || SelectedVal == "Insert")
            {
                    rbtCreateAt[0].disabled = false; 
                    rbtCreateAt[1].disabled = false;
                }else {                
                    rbtCreateAt[0].disabled = true ; 
                    rbtCreateAt[1].disabled = true;
            }
        }           
    }
    
    
    //Check value of rbtReceivePo for show upload po item
    function GetReceivePo()
    { 
        var RLCompareParameter = document.getElementById("<%=rbtReceivePo.ClientID %>");
        var radioButtons = RLCompareParameter.getElementsByTagName('input');
        if (radioButtons[0].checked) { 
            document.getElementById('<%=linkUploadPO.ClientID %>').style.display = 'block';
            document.getElementById('<%=lblUploadPO.ClientID %>').style.display = 'none';
           
            
        }else if (radioButtons[1].checked) {
            document.getElementById('<%=linkUploadPO.ClientID %>').style.display = 'none'; 
            document.getElementById('<%=lblUploadPO.ClientID %>').style.display = 'block';
            
        }
    }
    
    
    function setHontaiAmount(HontaiAmount)
    {        
         //Item Amount
         var totalAmount = document.getElementById('<%=lblTotalAmount.ClientID %>');
         var hontaiAmount = document.getElementById('<%=lblHontaiAmount.ClientID %>');         
         var totalAmountThb = document.getElementById('<%=lblTotalAmountThb.ClientID %>');
         var hontaiAmountThb = document.getElementById('<%=lblHontaiAmountThb.ClientID %>');
         var hontai1AmountThb = document.getElementById('<%=lblHontai1AmountThb.ClientID %>');
         var hontai2AmountThb = document.getElementById('<%=lblHontai2AmountThb.ClientID %>');
         var hontai3AmountThb = document.getElementById('<%=lblHontai3AmountThb.ClientID %>');
         var scheduleRate = document.getElementById('<%=txtScheduleRate.ClientID %>');
         var txtCondition1 = document.getElementById('<%=txtCondition1.ClientID %>');
         var txtCondition2 = document.getElementById('<%=txtCondition2.ClientID %>');
         var txtCondition3 = document.getElementById('<%=txtCondition3.ClientID %>');
         var txtAmount1 = document.getElementById('<%=txtAmount1.ClientID %>');
         var txtAmount2 = document.getElementById('<%=txtAmount2.ClientID %>');
         var txtAmount3 = document.getElementById('<%=txtAmount3.ClientID %>');
         var hidHontaiAmount=document.getElementById('<%=hidHontaiAmount.ClientID%>');
        var hidTotalAmount=document.getElementById('<%=hidTotalAmount.ClientID%>');
        var	hidAmount1=document.getElementById('<%=hidAmount1.ClientID%>');
        var	hidAmount2=document.getElementById('<%=hidAmount2.ClientID%>');
        var	hidAmount3=document.getElementById('<%=hidAmount3.ClientID%>');
        var	hidTotalAmountThb=document.getElementById('<%=hidTotalAmountThb.ClientID%>');
        var	hidHontaiAmountThb=document.getElementById('<%=hidHontaiAmountThb.ClientID%>');
        var	hidHontai1AmountThb=document.getElementById('<%=hidHontai1AmountThb.ClientID%>');
        var	hidHontai2AmountThb=document.getElementById('<%=hidHontai2AmountThb.ClientID%>');
        var	hidHontai3AmountThb=document.getElementById('<%=hidHontai3AmountThb.ClientID%>');
        var	hidSumTotalAmount=document.getElementById('<%=hidSumTotalAmount.ClientID%>');
        var	hidPOTotalAmount=document.getElementById('<%=hidPOTotalAmount.ClientID%>');  
        var	hidHTAmount=document.getElementById('<%=hidHTAmount.ClientID%>');         
        
        var IndexValue = document.getElementById('<%=ddlPaymentCondition.ClientID %>').selectedIndex;
        var ddlPaymentCondition = document.getElementById('<%=ddlPaymentCondition.ClientID %>').options[IndexValue].text;
        
        if (ddlPaymentCondition == "" ){
            txtCondition1.value =0;
            txtCondition2.value =0;
            txtCondition3.value =0;
        }else{
         var strCondition = ddlPaymentCondition.split("%",3);         
            txtCondition1.value =strCondition[0];
            txtCondition2.value =strCondition[1];
            txtCondition3.value =strCondition[2];
        };
         //Convert String to decimal
         var valTotalAmount = totalAmount.innerHTML
         var dblTotalAmount = parseFloat(valTotalAmount.replace(/,/g, ''),10).toFixed(2);
         var dblHontaiAmount 
         if (HontaiAmount==''){
         dblHontaiAmount = 0;
         }else{
          dblHontaiAmount = parseFloat(HontaiAmount.replace(/,/g, ''),10).toFixed(2);         
        };
        //Check total amount
        var chkTotal = hidSumTotalAmount.value
        var dblSumTotalAmount       
//        if(chkTotal== 0) { 
//            dblSumTotalAmount =  parseFloat(dblHontaiAmount);
//        }else{
//        
//             dblSumTotalAmount = parseFloat(dblTotalAmount) ;
//        };
         //Set data to amount text
//         hontaiAmount.innerHTML = HontaiAmount; 
         if(HontaiAmount == "") {
            totalAmount.innerHTML = setCurrencyPoint(parseFloat((hidPOTotalAmount.value).replace(/,/g, '')));
            hidHTAmount.value = 0;
         } else {
            totalAmount.innerHTML = setCurrencyPoint(parseFloat((hidPOTotalAmount.value).replace(/,/g, '')) + parseFloat(HontaiAmount.replace(/,/g, '')));
            hidHTAmount.value = parseFloat(HontaiAmount.replace(/,/g, ''));
         }      
            dblSumTotalAmount = parseFloat(totalAmount.innerHTML.replace(/,/g, ''));            
         //Calculete Hontai Amount
         var dblHontaiAmount1 = dblHontaiAmount * (txtCondition1.value/100);
         var dblHontaiAmount2 = dblHontaiAmount * (txtCondition2.value/100);
         var dblHontaiAmount3 = dblHontaiAmount * (txtCondition3.value/100);
                   
         //Calculate Amount THB
         var amountTotal = setCurrencyPoint(dblSumTotalAmount * scheduleRate.value); 
         var amountHontai =setCurrencyPoint(dblHontaiAmount * scheduleRate.value); 
         var amountHontai1 =setCurrencyPoint(dblHontaiAmount1 * scheduleRate.value); 
         var amountHontai2 =setCurrencyPoint(dblHontaiAmount2 * scheduleRate.value); 
         var amountHontai3 =setCurrencyPoint(dblHontaiAmount3 * scheduleRate.value); 
//         alert('HontaiAmount = ' + HontaiAmount);
//         alert('hidPOTotalAmount = ' + hidPOTotalAmount.value);
//         alert('HontaiAmount = ' + HontaiAmount);
//         alert('HontaiAmount = ' + HontaiAmount);
            
//         sumAmount.innerHTML =SumAmount;
         //Set Hontai Amount 
         
         txtAmount1.value = setCurrencyPoint(dblHontaiAmount1);
         txtAmount2.value = setCurrencyPoint(dblHontaiAmount2);
         txtAmount3.value = setCurrencyPoint(dblHontaiAmount3);
          //Set data to amount thb
          var strTHB = "( THB = ";
          var strClose = " )";
         totalAmountThb.innerHTML = strTHB + amountTotal + strClose;
         hontaiAmountThb.innerHTML = strTHB + amountHontai + strClose;
         hontai1AmountThb.innerHTML = strTHB + setCurrencyPoint(amountHontai1) + strClose;
         hontai2AmountThb.innerHTML = strTHB + setCurrencyPoint(amountHontai2) + strClose;
         hontai3AmountThb.innerHTML = strTHB + setCurrencyPoint(amountHontai3) + strClose;
         
        hidHontaiAmount.value = HontaiAmount;
        hidTotalAmount.value =dblSumTotalAmount;
        hidAmount1.value =setCurrencyPoint(dblHontaiAmount1);
        hidAmount2.value =setCurrencyPoint(dblHontaiAmount2);
        hidAmount3.value =setCurrencyPoint(dblHontaiAmount3);
        hidTotalAmountThb.value = strTHB + amountTotal + strClose;
        //hidSumAmountThb.value = strTHB + amountSumOrther + strClose;
        hidHontaiAmountThb.value = strTHB + amountHontai + strClose;
        hidHontai1AmountThb.value = strTHB + setCurrencyPoint(amountHontai1) + strClose;
        hidHontai2AmountThb.value = strTHB + setCurrencyPoint(amountHontai2) + strClose;
        hidHontai3AmountThb.value = strTHB + setCurrencyPoint(amountHontai3) + strClose;
                
    }
    
     //funtion set quotation sumary when menagement on popup
    function addToParentQuoAmount_Conver()
    {                
         //Item Amount         
         var sumQuoAmount = document.getElementById('<%=lblSumOtherAmount.ClientID %>');  
         var sumAmountThb = document.getElementById('<%=lblSumOthersAmountThb.ClientID %>');   
         var hidSumAmount=document.getElementById('<%=hidSumAmount.ClientID%>');
         var hidSumAmountThb=document.getElementById('<%=hidSumAmountThb.ClientID%>');
         var scheduleRate = document.getElementById('<%=txtScheduleRate.ClientID %>');
         var hidPOTotalAmount = document.getElementById('<%=hidPOTotalAmount.ClientID%>');
        
        //Set data to amount text
         sumQuoAmount.innerHTML = hidPOTotalAmount.value;   
         
         //Convert String to decimal
         var dblSumAmount = parseFloat(valSumQuoAmount.replace(/,/g, ''),10).toFixed(2);  
         //Calculate Amount THB
         var amountSumOrther =setCurrencyPoint( dblSumAmount * scheduleRate.value); 
      
          //Set data to amount thb
          var strTHB = "( THB = ";
          var strClose = " )";
        // sumAmountThb.innerHTML = strTHB + amountSumOrther + strClose; 
        
                        
         //Set data to hidden
        hidSumAmount.value = hidPOTotalAmount;        
        hidSumAmountThb.value = strTHB + hidPOTotalAmount + strClose;
                
    }
    
    //funtion calculate total amount when menagement on popup
    function addToParentTotal_Conver(TotalAmount)
    {     //Item Amount
        var totalAmount = document.getElementById('<%=lblTotalAmount.ClientID %>');                 
        var totalAmountThb = document.getElementById('<%=lblTotalAmountThb.ClientID %>');         
        var scheduleRate = document.getElementById('<%=txtScheduleRate.ClientID %>');          
        var hidTotalAmount=document.getElementById('<%=hidTotalAmount.ClientID%>');         
        var	hidTotalAmountThb=document.getElementById('<%=hidTotalAmountThb.ClientID%>');
        var	hidSumTotalAmount=document.getElementById('<%=hidSumTotalAmount.ClientID%>'); 
        var	hidPOTotalAmount=document.getElementById('<%=hidPOTotalAmount.ClientID%>');  
        var	txtHontaiAmount=document.getElementById('<%=txtHontaiAmount.ClientID%>'); 
        var valHontaiAmount = setCurrencyPoint(txtHontaiAmount.value).replace(/,/g, '');
        var sumExceptHontaiAmount = document.getElementById('<%=lblSumOtherAmount.ClientID%>');
        var lblSumOthersAmountThb = document.getElementById('<%=lblSumOthersAmountThb.ClientID%>');
        var flagPO = document.getElementById('<%=hidflagPO.ClientID %>');
        //Check Total Amount            
        if (TotalAmount == 0) {
            hidSumTotalAmount.value = 0;
        }else{
            hidSumTotalAmount.value = 1;
        };

        sumExceptHontaiAmount.innerHTML = TotalAmount;
        //alert(hidSumTotalAmount.value);
        flagPO.value = 1; 
        
        
        //Set data to amount text              
        //totalAmount.innerHTML = TotalAmount;        
        var realTotalAmount;
        if(valHontaiAmount == "") {
           realTotalAmount = parseFloat(TotalAmount.replace(/,/g, ''));
        } else {
           realTotalAmount = parseFloat(TotalAmount.replace(/,/g, '')) + parseFloat(valHontaiAmount);         
        }      
           
        //Convert String to decimal
        //var dblTotalAmount = parseFloat(totalAmount.innerHTML.replace(/,/g, ''),10).toFixed(2);                             
        //Calculate Amount THB
        var amountTotal = setCurrencyPoint(parseFloat(realTotalAmount) * scheduleRate.value);   
                
        //Set data to amount thb
        var strTHB = "( THB = ";
        var strClose = " )";
        
        lblSumOthersAmountThb.innerHTML = strTHB + TotalAmount + strClose;
        
        totalAmount.innerHTML = setCurrencyPoint(realTotalAmount);
        totalAmountThb.innerHTML = strTHB + amountTotal + strClose;
        hidTotalAmount.value = TotalAmount;              
        hidPOTotalAmount.value = TotalAmount;       
        hidTotalAmountThb.value = strTHB + amountTotal + strClose; 
        
                    
    }
    
    //funtion calculate when menagement on popup
    function addToParentItemData_Conver(HontaiAmount,SumAmount,TotalAmount)
    {    
        //Item Amount
        var totalAmount = document.getElementById('<%=lblTotalAmount.ClientID %>');
        var sumAmount = document.getElementById('<%=lblSumOtherAmount.ClientID %>');
        var hontaiAmount = document.getElementById('<%=lblHontaiAmount.ClientID %>');         
        var totalAmountThb = document.getElementById('<%=lblTotalAmountThb.ClientID %>');
        var sumAmountThb = document.getElementById('<%=lblSumOthersAmountThb.ClientID %>');
        var hontaiAmountThb = document.getElementById('<%=lblHontaiAmountThb.ClientID %>');
        var hontai1AmountThb = document.getElementById('<%=lblHontai1AmountThb.ClientID %>');
        var hontai2AmountThb = document.getElementById('<%=lblHontai2AmountThb.ClientID %>');
        var hontai3AmountThb = document.getElementById('<%=lblHontai3AmountThb.ClientID %>');
        var scheduleRate = document.getElementById('<%=txtScheduleRate.ClientID %>');
        var txtCondition1 = document.getElementById('<%=txtCondition1.ClientID %>');
        var txtCondition2 = document.getElementById('<%=txtCondition2.ClientID %>');
        var txtCondition3 = document.getElementById('<%=txtCondition3.ClientID %>');
        var txtAmount1 = document.getElementById('<%=txtAmount1.ClientID %>');
        var txtAmount2 = document.getElementById('<%=txtAmount2.ClientID %>');
        var txtAmount3 = document.getElementById('<%=txtAmount3.ClientID %>');
        var hidHontaiAmount=document.getElementById('<%=hidHontaiAmount.ClientID%>');
        var hidTotalAmount=document.getElementById('<%=hidTotalAmount.ClientID%>');
        var	hidSumAmount=document.getElementById('<%=hidSumAmount.ClientID%>');
        var	hidAmount1=document.getElementById('<%=hidAmount1.ClientID%>');
        var	hidAmount2=document.getElementById('<%=hidAmount2.ClientID%>');
        var	hidAmount3=document.getElementById('<%=hidAmount3.ClientID%>');
        var	hidTotalAmountThb=document.getElementById('<%=hidTotalAmountThb.ClientID%>');
        var	hidSumAmountThb=document.getElementById('<%=hidSumAmountThb.ClientID%>');
        var	hidHontaiAmountThb=document.getElementById('<%=hidHontaiAmountThb.ClientID%>');
        var	hidHontai1AmountThb=document.getElementById('<%=hidHontai1AmountThb.ClientID%>');
        var	hidHontai2AmountThb=document.getElementById('<%=hidHontai2AmountThb.ClientID%>');
        var	hidHontai3AmountThb=document.getElementById('<%=hidHontai3AmountThb.ClientID%>');
        var IndexValue = document.getElementById('<%=ddlPaymentCondition.ClientID %>').selectedIndex;
        var ddlPaymentCondition = document.getElementById('<%=ddlPaymentCondition.ClientID %>').options[IndexValue].text;
        
        var strCondition = ddlPaymentCondition.split("%",3);         
           txtCondition1.value =strCondition[0];
           txtCondition2.value =strCondition[1];
           txtCondition3.value =strCondition[2];
                      
         //Convert String to decimal
        var dblTotalAmount = parseFloat(TotalAmount.replace(/,/g, ''),10).toFixed(2);
        var dblSumAmount = parseFloat(SumAmount.replace(/,/g, ''),10).toFixed(2);
        var dblHontaiAmount = parseFloat(HontaiAmount.replace(/,/g, ''),10).toFixed(2);
         
         //Calculete Hontai Amount
        var dblHontaiAmount1 = dblHontaiAmount * (txtCondition1.value/100);
        var dblHontaiAmount2 = dblHontaiAmount * (txtCondition2.value/100);
        var dblHontaiAmount3 = dblHontaiAmount * (txtCondition3.value/100);
                   
         //Calculate Amount THB
        var amountTotal = setCurrencyPoint( dblTotalAmount * scheduleRate.value); 
        var amountSumOrther =setCurrencyPoint( dblSumAmount * scheduleRate.value); 
        var amountHontai =setCurrencyPoint(dblHontaiAmount * scheduleRate.value); 
        var amountHontai1 =setCurrencyPoint(dblHontaiAmount1 * scheduleRate.value); 
        var amountHontai2 =setCurrencyPoint(dblHontaiAmount2 * scheduleRate.value); 
        var amountHontai3 =setCurrencyPoint(dblHontaiAmount3 * scheduleRate.value); 
        
         //Set data to amount text
         hontaiAmount.innerHTML = HontaiAmount;          
         totalAmount.innerHTML = TotalAmount; 
         sumAmount.innerHTML =SumAmount;
         //Set Hontai Amount 
         
         txtAmount1.value = setCurrencyPoint(dblHontaiAmount1);
         txtAmount2.value = setCurrencyPoint(dblHontaiAmount2);
         txtAmount3.value = setCurrencyPoint(dblHontaiAmount3);
          //Set data to amount thb
          var strTHB = "( THB = ";
          var strClose = " )";
         totalAmountThb.innerHTML = strTHB + amountTotal + strClose;
         sumAmountThb.innerHTML = strTHB + amountSumOrther + strClose;
         hontaiAmountThb.innerHTML = strTHB + amountHontai + strClose;
         hontai1AmountThb.innerHTML = strTHB + setCurrencyPoint(amountHontai1) + strClose;
         hontai2AmountThb.innerHTML = strTHB + setCurrencyPoint(amountHontai2) + strClose;
         hontai3AmountThb.innerHTML = strTHB + setCurrencyPoint(amountHontai3) + strClose;
         
        hidHontaiAmount.value =HontaiAmount;
        hidTotalAmount.value =TotalAmount;
        hidSumAmount.value =SumAmount;
        hidAmount1.value =setCurrencyPoint(dblHontaiAmount1);
        hidAmount2.value =setCurrencyPoint(dblHontaiAmount2);
        hidAmount3.value =setCurrencyPoint(dblHontaiAmount3);
        hidTotalAmountThb.value = strTHB + amountTotal + strClose;
        hidSumAmountThb.value = strTHB + amountSumOrther + strClose;
        hidHontaiAmountThb.value = strTHB + amountHontai + strClose;
        hidHontai1AmountThb.value = strTHB + setCurrencyPoint(amountHontai1) + strClose;
        hidHontai2AmountThb.value = strTHB + setCurrencyPoint(amountHontai2) + strClose;
        hidHontai3AmountThb.value = strTHB + setCurrencyPoint(amountHontai3) + strClose;
    }
    
    function calcDate2(status) {
        if (status) {
            var txtdate1 = document.getElementById('<%=txtDate1.ClientID%>').value;
            var set_txtdate1 = txtdate1.split('/');
            var dd = set_txtdate1[0];
            var mm = set_txtdate1[1] - 1;
            var yyyy = set_txtdate1[2];
            var date1 = new Date(yyyy, mm, dd);
            var txtcondition3 = document.getElementById('<%=txtCondition3.ClientID%>').value;
            var date2 = date1.setMonth(mm + (txtcondition3 == 0 ? 8 : 2));
            date2 = new Date(date2);
            
            var lastDay = new Date(date2.getFullYear(), date2.getMonth() + 1, 0);
            //alert(date2);
            dd = lastDay.getDate();
            mm = date2.getMonth() + 1;
            yyyy = date2.getFullYear();
                      
            var calcval = (((dd <= 9) ? "0" + dd : dd) + "/" + ((mm <= 9) ? "0" + mm : mm) + "/" + yyyy);
            //alert(calcval);
            
            document.getElementById('<%=txtDate2.ClientID%>').value = calcval;
            document.getElementById('<%=hidDate2.ClientID%>').value = calcval;
            
        } else {
            document.getElementById('<%=txtDate2.ClientID%>').value = "";
        }
    }
    
    function calcDate3(status) {
        if (status) {
            var txtdate2 = document.getElementById('<%=txtDate2.ClientID%>').value;
            var set_txtdate2 = txtdate2.split('/');
            var dd = set_txtdate2[0];
            var mm = set_txtdate2[1] - 1;
            var yyyy = set_txtdate2[2];
            var date2 = new Date(yyyy, mm, dd);
            var date3 = date2.setMonth(mm + 8);
            date3 = new Date(date3);
            
            var lastDay = new Date(date2.getFullYear(), date2.getMonth() + 1, 0);
            //alert(date3);
            dd = lastDay.getDate();
            mm = date3.getMonth() + 1;
            yyyy = date3.getFullYear();
            //alert("" + dd + "," + mm + "," + yyyy);
            var calcval = (((dd <= 9) ? "0" + dd : dd) + "/" + ((mm <= 9) ? "0" + mm : mm) + "/" + yyyy);
            //alert(calcval);
            document.getElementById('<%=txtDate3.ClientID%>').value = calcval;
            document.getElementById('<%=hidDate3.ClientID%>').value = calcval;
           
        } else {
            document.getElementById('<%=txtDate3.ClientID%>').value = "";
        }
    }
    
    function isNumberKey(evt) {
         var charCode = (evt.which) ? evt.which : event.keyCode
         if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
             return false;
         else {
             var len = document.getElementById("txtHontaiAmount").value.length;
             var index = document.getElementById("txtHontaiAmount").value.indexOf('.'); 
             
             if (index > 0 && charCode == 46) {    
                return false;                           
             }
         }
         return true;
      }
      
      
           
</script>


<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <div class="text_center" style ="width:1000px;">
        <br /><div class="font_size_12 text_left"><a href="KTJB01.aspx?New=True">Job Order</a> > Job Order</div>
        <br /><div class="font_header">JOB ORDER MANAGEMENT</div> 
        <br />       
        <table class="table_field" >
            <tr>
                <td class="table_field_td tb_Fix350 " colspan="2">&nbsp;&nbsp;Job Order</td>
                <td class="table_field_td tb_Fix650 ">
                    <asp:Label ID="lblJobOrder" runat="server" ForeColor="Blue" Font-Bold="True"></asp:Label>
                    <asp:Label ID="lblJobOrderHid" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2" >&nbsp;&nbsp;Issue Date (dd/mm/yyyy)<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtIssueDate" runat="server" MaxLength="10" AutoPostBack="true" ></asp:TextBox>
                    <asp:CalendarExtender ID="txtIssueDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        TargetControlID="txtIssueDate">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="txtIssueDate_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtIssueDate" 
                        ValidChars="1234567890/" />   
                    <asp:RequiredFieldValidator ID="reqValidatorJobOrder" runat="server" ValidationGroup="Add" SetFocusOnError="True"
                        ControlToValidate="txtIssueDate" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Customer<span class="font_require">*</span></td>
                <td class="table_field_td">
                     <asp:DropDownList ID="ddlCustomer" runat="server" Width="450px" AutoPostBack="true" /> 
                     <button id="toggle"></button>
                     <asp:RequiredFieldValidator ID="reqValidatorCustomer" runat="server" ValidationGroup="Add" SetFocusOnError="True"
                         ControlToValidate="ddlCustomer" ErrorMessage="*Require." class="font_error">
                     </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;End User<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlEndUser" runat="server" Width="450px" ></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqValidatorEndUser" runat="server" ValidationGroup="Add" SetFocusOnError="True"
                         ControlToValidate="ddlEndUser" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Receive PO<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rbtReceivePo" runat="server" AutoPostBack="true"   
                                     RepeatDirection="Horizontal" >
                                     <asp:ListItem Value="1">Yes</asp:ListItem>
                                     <asp:ListItem Value="0">No</asp:ListItem>
                                 </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="reqValidatorRecivePO" runat="server" ValidationGroup="rbtRecivePO" SetFocusOnError="True"
                                     ControlToValidate="rbtReceivePo" ErrorMessage="*Require."  class="font_error">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Person in charge<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlPersonCharge" runat="server" Width="155">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqValidatorPersonCharge" runat="server" ValidationGroup="Add" SetFocusOnError="True"
                        ControlToValidate="ddlPersonCharge" ErrorMessage="*Require." class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="3">&nbsp;&nbsp;
                    <asp:LinkButton id="linkUploadPO" runat="server"  text="Upload PO" /> 
                    <asp:Label ID="lblUploadPO" runat="server" Text="Upload PO" Visible="false"  />
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="3">&nbsp;&nbsp;
                    <asp:LinkButton id="linkUploadQuotation" runat="server"  text="Upload Quotation" /> 
                    <asp:Label ID="lblUploadQuo" runat="server" Text="Upload Quotation" Visible="false"  />
                 </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2" >&nbsp;&nbsp;Job Order Type<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlJobOrder" AutoPostBack="true"  runat="server" Width="155px"></asp:DropDownList>         
                    <asp:RequiredFieldValidator ID="reqValidatorJobOrderType" runat="server" ValidationGroup="Add" SetFocusOnError="True"
                        ControlToValidate="ddlJobOrder" ErrorMessage="*Require." class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;BOI</td>
                <td class="table_field_td">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rbtBOI" runat="server" RepeatDirection="Horizontal">
                                     <asp:ListItem Value="1">BOI</asp:ListItem>
                                     <asp:ListItem Value="2">Non-BOI</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>                     
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_left" colspan="2">                 
                    <table>
                        <tr>
                            <td>
                                Create At 
                            </td>
                            <td>
                                 <asp:Label ID="lblChkReq" runat="server" CssClass="font_error" text="*"/>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="table_field_td"> 
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rbtCreateAt" runat="server" AutoPostBack="true"
                                     RepeatDirection="Horizontal" >
                                     <asp:ListItem Value="2">Other Company</asp:ListItem>
                                     <asp:ListItem Value="1">Own Company</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                 <asp:TextBox ID="txtOwnCompany" runat="server" MaxLength="255" Width="249px" Enabled="false" onClick="this.value='';"  />      
                                                    
                            </td>
                            <td>
                                <asp:Label ID="lblReqCreateAt" runat="server" CssClass="font_error" text=""/>
                           <%--<asp:RequiredFieldValidator ID="reqValidatorRbtCreateAt" runat="server" ValidationGroup="Add" SetFocusOnError="True"
                                    ControlToValidate="rbtCreateAt" ErrorMessage="*Require." class="font_error">
                                </asp:RequiredFieldValidator> --%> 
                            </td>
                        </tr>
                    </table>    
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Part Name<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtPartName" runat="server" Width="500px" MaxLength="255"></asp:TextBox>                    
                    <asp:RequiredFieldValidator ID="reqValidatorPartName" runat="server" ValidationGroup="Add" SetFocusOnError="True"
                        ControlToValidate="txtPartName" ErrorMessage="*Require." class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Part No.</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtPartNo" runat="server" Width="500px" MaxLength="255"></asp:TextBox>                    
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Detail</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDetail" runat="server" Width="500px" MaxLength="255"></asp:TextBox>                    
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Part Type</td>
                <td class="table_field_td">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rbtPartType" runat="server" 
                                     RepeatDirection="Horizontal">
                                     <asp:ListItem Value="1">S/C</asp:ListItem>
                                     <asp:ListItem Value="2">D/C</asp:ListItem>
                                 </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>                     
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Payment Condition<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlPaymentCondition" runat="server" AutoPostBack="true" Width="142px">
                    </asp:DropDownList>        
                    <asp:RequiredFieldValidator ID="reqValidatorPaymentCondition" runat="server" ValidationGroup="Add" SetFocusOnError="True"
                        ControlToValidate="ddlPaymentCondition" ErrorMessage="*Require." class="font_error">
                    </asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtPaymentCondition" runat="server" Width="242px" MaxLength="255"></asp:TextBox> 
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Payment Term<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlPayment" runat="server" Width="142px">
                    </asp:DropDownList> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Days        
                    <asp:RequiredFieldValidator ID="reqValidatorPaymentTerm" runat="server" ValidationGroup="Add" SetFocusOnError="True"
                        ControlToValidate="ddlPayment" ErrorMessage="*Require." class="font_error">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Currency<span class="font_require">*</span></td>
                <td class="table_field_td">
                    <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="true"  Width="142px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqValidatorCurrency" runat="server" ValidationGroup="Add" SetFocusOnError="True"
                        ControlToValidate="ddlCurrency" ErrorMessage="*Require."  class="font_error">
                    </asp:RequiredFieldValidator>
                    &nbsp;<asp:Label ID="lblScheduleRate" runat="server" >Schedule Rate (THB)</asp:Label>
                    &nbsp;<asp:TextBox ID="txtScheduleRate" runat="server" Width="242px" MaxLength="255" Enabled="false" ></asp:TextBox> 
                </td>
            </tr>
            <%--<tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Quotation Amount</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtQuotationAmount" runat="server" Width="140px" 
                        MaxLength="255" CssClass="text_right" ></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtQuotationAmount_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtQuotationAmount" 
                         ValidChars="1234567890,.">
                     </asp:FilteredTextBoxExtender> 
                </td>
            </tr>--%>
            <tr>
                <td class="table_field_td text_left tb_Fix170">&nbsp;&nbsp;Hontai</td>
                <td class="table_field_td text_left tb_Fix180" >Hontai Amount</td>
                <td class="table_field_td text_left">
                    <asp:Label ID="lblHidHontaiAmount" runat="server" Font-Bold="True" Width="142px" Text="******"></asp:Label>
                    <asp:TextBox ID="txtHontaiAmount" runat="server" Width="150px"
                        onkeypress="return IsDecimals(event,'ctl00_MainContent_txtHontaiAmount',2);"
                        onBlur="this.value=setCurrencyPoint(this.value); setHontaiAmount(this.value);"
                        MaxLength="255" CssClass="text_right" ></asp:TextBox>
                    
                    <asp:RequiredFieldValidator ID="reqValidatorHontaiAmount" runat="server" ValidationGroup="second" SetFocusOnError="True"
                        ControlToValidate="txtHontaiAmount" ErrorMessage="*Require."  class="font_error" onkeyup="this.value=addCommas(this.value);"></asp:RequiredFieldValidator>
                        
                    <asp:RangeValidator ID="rngHontaiAmount" runat="server" SetFocusOnError="True"
                             ControlToValidate="txtHontaiAmount" Display="Static" 
                             ErrorMessage="Please insert number" MaximumValue="99999999999999.99" 
                             MinimumValue="0.00" Type="Currency" ValidationGroup="Add" ></asp:RangeValidator>
                        
                        <%-- <asp:RegularExpressionValidator ID="rglHontaiAmount" 
                                        ControlToValidate="txtHontaiAmount"
                                        ValidationExpression="([0-9]*)(\.*)([0-9]+)" 
                                        ErrorMessage="Section 4 Remove all commas"
                                        Width="3%" runat="server" Font-Bold="true" ValidationGroup="Add">
                                        <strong>*</strong>
                                    </asp:RegularExpressionValidator>   --%>

                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblHontaiAmountThb" runat="server" ></asp:Label><asp:Label ID="lblHidHontaiAmountThb" runat="server" >( THB = ****** )</asp:Label>
                    <asp:FilteredTextBoxExtender ID="txtHontaiAmount_FilteredTextBoxExtender" 
                         runat="server" Enabled="True" TargetControlID="txtHontaiAmount" 
                         ValidChars="1234567890,.">
                     </asp:FilteredTextBoxExtender> 
                    <asp:Label ID="lblHontaiAmount" runat="server" Font-Bold="True" Width="142px" Text="" Visible="false" ></asp:Label>
                   
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center" rowspan="3">
                    <asp:CheckBox ID="ChkHontai1" runat="server" Text="1st" AutoPostBack="True" />
                    <asp:Label ID="lblReqHontai1" runat="server" CssClass="font_error" text="*"/>
                    <br/>
                    <asp:Label ID="lblReq1" runat="server" CssClass="font_error" text="*Require." />
                                                 
                </td>
                <td class="table_field_td">
                    Date(dd/mm/yyyy)
                    <asp:Label ID="lblChkReqDate1" runat="server" CssClass="font_error" text="*"/>                                
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDate1" runat="server" MaxLength="10" ></asp:TextBox>    
                    <asp:CalendarExtender ID="txtDate1_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                        TargetControlID="txtDate1">
                    </asp:CalendarExtender>                                
                    <asp:FilteredTextBoxExtender ID="txtDate1_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtDate1" 
                    ValidChars="1234567890/" />                     
                    <asp:Label ID="lblDateReq1" runat="server" CssClass="font_error" text="*Require." />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Amount</td>
                <td class="table_field_td">
                    <asp:Label ID="lblHidAmount1" runat="server" Font-Bold="True" Text="******"></asp:Label>
                    <asp:TextBox ID="txtAmount1" runat="server" AutoPostBack="True" 
                    Enabled="false" ></asp:TextBox> 
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblHontai1AmountThb" runat="server" ></asp:Label><asp:Label ID="lblHidHontai1AmountThb" runat="server" >( THB = ****** )</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Condition</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtCondition1" runat="server" Width="32px" Enabled="false" ></asp:TextBox> &nbsp; %
                 </td>
            </tr>
            <tr>
                <td class="table_field_td text_center" rowspan="3">
                    <asp:CheckBox ID="ChkHontai2" runat="server" Text="2nd" onclick="calcDate2(this.checked);" AutoPostBack="True" />
                    <asp:Label ID="lblReqHontai2" runat="server" CssClass="font_error" text="*"/>
                    <br/>
                    <asp:Label ID="lblReq2" runat="server" CssClass="font_error" text="*Require." />                     
                </td>
                <td class="table_field_td">
                    Date (dd/mm/yyyy)
                     <asp:Label ID="lblChkReqDate2" runat="server" CssClass="font_error" text="*" disabled="true"/> 
                </td>
                <td class="table_field_td">
                     <asp:TextBox ID="txtDate2" runat="server" MaxLength="10" ></asp:TextBox>    
                    <asp:CalendarExtender ID="txtDate2_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                        TargetControlID="txtDate2">
                    </asp:CalendarExtender>                                
                    <asp:FilteredTextBoxExtender ID="txtDate2_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtDate2" 
                    ValidChars="1234567890/" />
                   <asp:Label ID="lblDateReq2" runat="server" CssClass="font_error" text="*Require." />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Amount</td>
                <td class="table_field_td">
                    <asp:Label ID="lblHidAmount2" runat="server" Font-Bold="True" Text="******"></asp:Label>
                    <asp:TextBox ID="txtAmount2" runat="server" AutoPostBack="True" 
                    MaxLength="19" Enabled="false"></asp:TextBox> 
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblHontai2AmountThb" runat="server" ></asp:Label>
                    <asp:Label ID="lblHidHontai2AmountThb" runat="server" >( THB = ****** )</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Condition</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtCondition2" runat="server" Width="32px" MaxLength="3" 
                    Enabled="false"></asp:TextBox> &nbsp; %
                </td>
            </tr>
            <tr>
                <td class="table_field_td text_center" rowspan="3">
                    <asp:CheckBox ID="ChkHontai3" runat="server" Text="3rd" onclick="calcDate3(this.checked);" AutoPostBack="True" />
                    <asp:Label ID="lblReqHontai3" runat="server" CssClass="font_error" text="*"/>  
                    <br/>
                    <asp:Label ID="lblReq3" runat="server" CssClass="font_error" text="*Require." />
                </td>
                <td class="table_field_td">
                    Date (dd/mm/yyyy)
                    <asp:Label ID="lblChkReqDate3" runat="server" CssClass="font_error" text="*"/>                     
                </td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtDate3" runat="server" MaxLength="10" ></asp:TextBox>    
                    <asp:CalendarExtender ID="txtDate3_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                        TargetControlID="txtDate3">
                    </asp:CalendarExtender>                                
                    <asp:FilteredTextBoxExtender ID="txtDate3_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" TargetControlID="txtDate3" 
                    ValidChars="1234567890/" />
                   <asp:Label ID="lblDateReq3" runat="server" CssClass="font_error" text="*Require." />
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Amount</td>
                <td class="table_field_td">
                    <asp:Label ID="lblHidAmount3" runat="server" Font-Bold="True" Text="******"></asp:Label>
                    <asp:TextBox ID="txtAmount3" runat="server" AutoPostBack="True" Enabled="false"></asp:TextBox> 
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblHontai3AmountThb" runat="server" ></asp:Label><asp:Label ID="lblHidHontai3AmountThb" runat="server" >( THB = ****** )</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td">Condition</td>
                <td class="table_field_td">
                     <asp:TextBox ID="txtCondition3" runat="server" Width="32px" MaxLength="3" 
                         AutoPostBack="True" Enabled="false"></asp:TextBox> &nbsp; %
                </td>
            </tr>
            <tr>
                <%--<td class="table_field_td" colspan="2">&nbsp;&nbsp;Except Quotation Amount</td>--%>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Except Hontai Amount</td>
                <td class="table_field_td">
                    <asp:Label ID="lblHidSumOtherAmount" runat="server" Font-Bold="True" Width="142px" Text="******"></asp:Label>                    
                    <asp:Label ID="lblSumOtherAmount" runat="server" Font-Bold="True" Width="142px" Text=""  ></asp:Label>  
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;                
                    <asp:Label ID="lblSumOthersAmountThb" runat="server" ></asp:Label><asp:Label ID="lblHidSumOthersAmountThb" runat="server" >( THB = ****** )</asp:Label>
                                       
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Total Amount</td>
                <td class="table_field_td">
                    <asp:Label ID="lblHidTotalAmount" runat="server" Font-Bold="True" Width="142px" Text="******"></asp:Label>
                    <asp:Label  ID="lblTotalAmount" runat="server" Font-Bold="True" Width="142px" Text="" ></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblTotalAmountThb" runat="server" ></asp:Label><asp:Label ID="lblHidTotalAmountThb" runat="server" >( THB = ****** )</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td" colspan="2">&nbsp;&nbsp;Remarks</td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="font" 
                        onkeypress="if (this.value.length > 255) { return false; }"  Width="434px">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="text_right">
                    <asp:HiddenField ID="hidIpAddress" runat="server" Value ="" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="Add" CssClass="button_style" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear"  CssClass="button_style" CausesValidation="false" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back"  CssClass="button_style" CausesValidation="false" />
                </td>
            </tr>
        </table> 
        <asp:HiddenField ID="hidHontaiAmount" runat="server" />
        <asp:HiddenField ID="hidHTAmount" runat="server" Value="0"/>
        <asp:HiddenField ID="hidTotalAmount" runat="server" />
        <asp:HiddenField ID="hidPOTotalAmount" runat="server" Value="0" />
        <asp:HiddenField ID="hidSumAmount" runat="server" />
        <asp:HiddenField ID="hidAmount1" runat="server" />
        <asp:HiddenField ID="hidAmount2" runat="server" />
        <asp:HiddenField ID="hidAmount3" runat="server" />
        <asp:HiddenField ID="hidTotalAmountThb" runat="server" />
        <asp:HiddenField ID="hidSumAmountThb" runat="server" />
        <asp:HiddenField ID="hidHontaiAmountThb" runat="server" />
        <asp:HiddenField ID="hidHontai1AmountThb" runat="server" />
        <asp:HiddenField ID="hidHontai2AmountThb" runat="server" />
        <asp:HiddenField ID="hidHontai3AmountThb" runat="server" />
        <asp:HiddenField ID="hidSumTotalAmount" runat="server" />
        <asp:HiddenField ID="hidSumHontaiAmout" runat="server" />
        <asp:HiddenField ID="hidflagPO" runat="server" />
        <asp:HiddenField ID="hidDate2" runat="server" />
        <asp:HiddenField ID="hidDate3" runat="server" />
        <asp:HiddenField ID="hidDate1" runat="server" />
    </div>
    
    
</asp:Content>
