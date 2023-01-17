<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="KTMS02.aspx.vb" Inherits="Master_KTMS02" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 
<script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script> 
<script type="text/javascript" src="../Scripts/jsCommon.js"></script>
<script language="javascript" type="text/javascript">
       
    // Type2 Radiobutton select change
    function CheckType1IDNo(rbtGetValue){
        var txtType1Select = document.getElementById("<%= txtHideType1.ClientID %>");
        var TypeOfGoodsCBList = document.getElementById("<%= TypeOfGoodsCBList.ClientID %>");
        var lblErrShortName = document.getElementById("<%= lblErrShortName.ClientID %>");
        if (rbtGetValue.checked) {
            if(rbtGetValue.value == 0){
                txtType1Select.value = 0;
                TypeOfGoodsCBList.disabled = false;
                lblErrShortName.innerText = "";
            }else if (rbtGetValue.value == 1){
                txtType1Select.value = 1;
                TypeOfGoodsCBList.disabled = true;
                lblErrShortName.innerText = "*";
            } else {
                txtType1Select.value = -1;
            }            
        }
    }
    
     // Type2 Radiobutton select change
    function CheckType2IDNo(rbtGetValue){    
        var lblType2IDNo = document.getElementById("<%= lblType2IDNo.ClientID %>");
        var txtType2Select = document.getElementById("<%= txtHideType2.ClientID %>");
        if (rbtGetValue.checked) {
            if(rbtGetValue.value == 0){
                txtType2Select.value = 0;
                lblType2IDNo.innerText = "Identification Card No.";
            }else if (rbtGetValue.value == 1){
                txtType2Select.value = 1;
                lblType2IDNo.innerText = "Taxpayer Identification No.";
            }else {
                txtType2Select.value = -1;
                lblType2IDNo.innerText = "Type2 ID No.";
            }
            //CheckNotDup();
        }
    }
    
    // Validate Type Of Goods
    function CheckTypeOfGoods()
    {
      var chkListModules = document.getElementById ('<%= TypeOfGoodsCBList.ClientID %>');
      var chkListinputs = chkListModules.getElementsByTagName("input");
      var SelectState;
      if (chkListModules.disabled == false) {
          for (var i=0;i<chkListinputs .length;i++)
          {
            if (chkListinputs [i].checked)
            {
                SelectState = true;
                return SelectState;
            }
          }
          SelectState = false;
      }else {
          SelectState = true;
      }
        return SelectState;
    }
    
    // Validate Type Of Goods
    function ValidateModuleList(source, args)
    {
        args.IsValid = CheckTypeOfGoods();
    }
    
    //Get Confirm message
    function getConfirmMessage() {
        var confirmMsg;
        //Statement
		$.ajax({
		    type: "POST",
		    url: "./KTMS02.aspx/ConfirmMsgCheck", // <-- Page Method  txtVendorId
		    contentType: "application/json; charset=utf-8",
		    dataType: "json",
		    async: false,
		    success: function(msg) {
			    if (msg.d != "") {
			        confirmMsg = msg.d;
			    }else {
			        confirmMsg = "Cannot get message from XML file";
			    }      
		    }
		});
		return confirmMsg;
    }
    
    // Check duplicate data
    function nameDupValidate(sender, agrs) {
        var aspVendorIDValue = document.getElementById("<%= txtVendorId.ClientID %>"); 
        var aspType1Value = document.getElementById("<%= txtHideType1.ClientID %>"); 
        var aspType2Value = document.getElementById("<%= txtHideType2.ClientID %>"); 
        var aspNameValue = document.getElementById("<%= txtName.ClientID %>");
        var aspDupStatus = document.getElementById("<%= hideCheckDupPass.ClientID %>");
        var aspType2No = document.getElementById("<%= txtType2No.ClientID %>");
        if (aspType1Value.value != "" && aspType2Value.value != "" && aspNameValue.value != "" 
            && aspType2No.value != "" && CheckTypeOfGoods() == true) {
        //Statement
        $.ajax({
            type: "POST",
            url: "./KTMS02.aspx/CheckNotDupInBehind", // <-- Page Method  txtVendorId
            data: "{strVendorID: '" + aspVendorIDValue.value + "', strType1: '" + aspType1Value.value + "', strType2: '" + 
                    aspType2Value.value + "', strName: '" + aspNameValue.value + "'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function(msg) {
                if (confirm(getConfirmMessage())) {
                    if (msg.d != "Pass") {
                        alert(msg.d);
                        agrs.IsValid = false;
                    }else {
                        aspDupStatus.value = true;
                        agrs.IsValid = true;
                    }
                }else {
                    agrs.IsValid = false;
                }    
            }
        });
        }
    }
        
    // Show file upload
    function LinkFileAttBtn_Click() {
        var aspFileUpload = document.getElementById("<%= LinkFileAttBtn.ClientID %>"); 
       //Statement
		$.ajax({
		    type: "POST",
		    url: "./KTMS02.aspx/LinkFileAttBtn_Click", // <-- Page Method  txtVendorId divFileUpload
		    data: "{strFileName: '" + aspFileUpload.innerText + "'}", // <-- Method Parameter(s)
            contentType: "application/json; charset=utf-8",
		    dataType: "json",
		    async: false,
		    success: function(msg) {
			    if (msg.d != "") {	
			        // Show file upload in popup window
			        popup = window.open(msg.d,'_blank','width=800,height=600,toolbar=no,location=no' +
			                ', directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes');
			    }
			}
		});
		return false;
    }
     // Show file upload
    function branchAddress_Click() {  
        var aspVendorID = document.getElementById("<%= txtVendorId.ClientID %>"); 
        // Open Branch Popup window     	
        popup = window.open('KTMS02_Branch.aspx?New=True&VendorID=' + aspVendorID.value
                ,'VBranchPage','width=1200,height=600,toolbar=no,location=no' +
                ', directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes');		
		return false;
    }
</script>
    
    <div class="text_center">
        <br />
        <div class="font_size_12 text_left"><a class="fix_link" href="KTMS01.aspx?New=True">Master Management</a> > Vendor</div>
        <br />
        <div class="font_header">VENDOR MANAGEMENT</div> 
        <br />
        <table class="table_field">
            <tr>
                <td class="table_field_td tb_Fix120"> Vendor ID </td>
                <td class="table_field_td"><asp:TextBox ID="txtVendorId" Enabled="false" runat="server" CssClass="textbox_read_only text_field"></asp:TextBox> 
                    <asp:HiddenField ID="txtHideType1" runat="server" />
                    <asp:HiddenField ID="txtHideType2" runat="server" />
                    <asp:HiddenField ID="hideCheckDupPass" runat="server" />
                    <asp:CustomValidator ID="NameDupValidate" runat="server" 
                        ClientValidationFunction="nameDupValidate" 
                        ErrorMessage="" 
                        CssClass="font_error"></asp:CustomValidator>
                                    </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Type1<asp:Label ID="Label2" runat="server" CssClass="font_require" Text="*"></asp:Label></td>
                <td class="table_field_td">
                    <table style="width:100%;">
                        <tr>
                            <td class="tb_Fix160">
                                <asp:RadioButtonList ID="rbtnType1" runat="server"
                                    RepeatDirection="Horizontal" Width="160px" CellPadding="0" 
                                    CellSpacing="0">
                                    <asp:ListItem Text="Vendor" Value="0" OnClick="CheckType1IDNo(this);"></asp:ListItem> 
                                    <asp:ListItem Text="Customer" Value="1" OnClick="CheckType1IDNo(this);"></asp:ListItem>
                                </asp:RadioButtonList>        
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="rbtnType1" CssClass="font_error" ErrorMessage="*Required"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120" >Type2<asp:Label ID="Label3" runat="server" CssClass="font_require" Text="*"></asp:Label></td>
                <td class="table_field_td" >
                    <table style="width:100%;">
                        <tr>
                            <td class="tb_Fix160" >
                                <asp:RadioButtonList ID="rbtnType2" AutoPostBack="False" runat="server"                                   
                                    CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" Width="160px">
                                    <asp:ListItem Text="Person" Value="0" OnClick="CheckType2IDNo(this);"></asp:ListItem>
                                    <asp:ListItem Text="Company" Value="1" OnClick="CheckType2IDNo(this);"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td class="tb_Fix60" >
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                    ControlToValidate="rbtnType2" CssClass="font_error" ErrorMessage="*Required"></asp:RequiredFieldValidator>
                            </td>
                            <td class="tb_Fix180 text_right" >
                                <asp:Label ID="lblType2IDNo" runat="server" Text="Type2 ID No."></asp:Label>
                                &nbsp;<asp:Label ID="Label5" runat="server" CssClass="font_require" Text="*"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtType2No" runat="server" MaxLength="20" 
                                    CssClass="text_field"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ControlToValidate="txtType2No" CssClass="font_error" 
                                    ErrorMessage="*Required"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        
                    </table>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Name<asp:Label ID="Label4" runat="server" CssClass="font_require" Text="*"></asp:Label></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtName" runat="server" MaxLength="150" CssClass="text_field" 
                        onkeyup="javascript:this.value=this.value.toUpperCase();" Width="592px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                    ControlToValidate="txtName" CssClass="font_error" 
                        ErrorMessage="*Required"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Short Name<asp:Label ID="lblErrShortName" runat="server" CssClass="font_require" Text="*"></asp:Label></td>
                <td class="table_field_td">
                    <asp:TextBox ID="txtShortName" runat="server" MaxLength="150" CssClass="text_field"></asp:TextBox>
                    <asp:Label ID="lblRequiredShortName" runat="server" CssClass="font_require" Text="" Font-Italic="True"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Person in charge1</td>
                <td class="table_field_td tb_Fix600"><asp:TextBox ID="txtPerson1" runat="server" MaxLength="50" CssClass="text_field"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Person in charge2</td>
                <td class="table_field_td tb_Fix600"><asp:TextBox ID="txtPerson2" runat="server" MaxLength="50" CssClass="text_field"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Country</td>
                <td class="table_field_td tb_Fix600">
                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="dropdown_field">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Zip Code</td>
                <td class="table_field_td tb_Fix600"><asp:TextBox ID="txtZipCode" runat="server" MaxLength="20" CssClass="text_field"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Address</td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" 
                        CssClass="text_multiLine" Width="592px" Height="60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Tel No.</td>
                <td class="table_field_td tb_Fix600"><asp:TextBox ID="txtTelNo" runat="server" MaxLength="20" CssClass="text_field"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Fax No.</td>
                <td class="table_field_td tb_Fix600"><asp:TextBox ID="txtFaxNo" runat="server" MaxLength="20" CssClass="text_field"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">E-Mail </td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" CssClass="text_field"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        CssClass="font_error" ErrorMessage="*Please check e-Mail format." 
                        SetFocusOnError="True" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        ControlToValidate="txtEmail"></asp:RegularExpressionValidator>
                </td>                
            </tr> 
            <tr>
                <td class="table_field_td tb_Fix120" colspan="2">
                    <asp:LinkButton ID="LinkBrachAddress" runat="server" CausesValidation="false" OnClientClick="return branchAddress_Click();">Brach Address</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">
                    Type of Goods <asp:Label ID="Label8" runat="server" Text="*" CssClass="font_require" ></asp:Label>
                </td>
                <td class="table_field_td">
                    <table style="width: 100%">
                        <tr>
                            <td class="tb_Fix180">
                                <asp:CheckBoxList ID="TypeOfGoodsCBList" runat="server"  
                                    RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0">
                                    <asp:ListItem Value="0">Purchase</asp:ListItem>
                                    <asp:ListItem Value="1">Outsource</asp:ListItem>
                                    <asp:ListItem Value="2">Other</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                            <td class="tb_Fix60">
                                <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="font_error" 
                                    ErrorMessage="*Required" ClientValidationFunction="ValidateModuleList"></asp:CustomValidator>
                            </td>
                            <td>
                            &nbsp;<asp:TextBox ID="txtTypeGoodsHideField" runat="server" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Remarks</td>
                <td class="table_field_td tb_Fix600">
                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" 
                        CssClass="text_multiLine" Height="60px" Width="390px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="table_field_td tb_Fix120">Attached File</td>
                <td class="table_field_td tb_Fix600"> 
                    <asp:FileUpload ID="fileAttached" runat="server" Width="217px" Height="22px" />
                    <asp:CustomValidator ID="FileSizeValidator" runat="server" 
                        ErrorMessage="*CustomValidator" 
                        CssClass="font_error" SetFocusOnError="True" 
                        ClientValidationFunction="" ControlToValidate="fileAttached"></asp:CustomValidator>                    
                    <br />
                    Uploaded File : 
                    <asp:LinkButton ID="LinkFileAttBtn" runat="server" 
                        onclientclick="return LinkFileAttBtn_Click();" CausesValidation="False"></asp:LinkButton>
                    &nbsp;<asp:CheckBox ID="chkRemoveFile" runat="server" Text="Remove"/>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="text_right">
                    <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button_style"/>
                    &nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear"  
                        CssClass="button_style" ValidationGroup="CBBtn" CausesValidation="False" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Back"  
                        CssClass="button_style" ValidationGroup="CBBtn" CausesValidation="False" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>