#Region "Imports"
Imports System.Web.Configuration
Imports System.Web.Services
Imports System.Data
Imports System.IO
Imports Enums
#End Region

Partial Class Master_KTMS02
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objVendor As New Dto.VendorDto
    Private objMessage As New Common.Utilities.Message
    Private objUtility As New Common.Utilities.Utility

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' Write start log
            objLog.StartLog("KTMS02 : ", Session("UserName"))
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Init", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Page_Load
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ' Set Validator message
            FileSizeValidator.ErrorMessage = " * " & objMessage.GetXMLMessage("Common_003")
            ' Set data to fileAttached
            'If fileSession.FileName <> Nothing Then
            '    fileAttached.ResolveClientUrl(fileSession.FileName)
            'End If
            If Not IsPostBack Then
                'Set data page default
                Call SetInit()
                Call CheckUserPer()
                Call CheckMode()
                Call CheckPageFirstLoad()   ' Clear Branch page session
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Load", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	    : btnSave_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            'If CheckNotDup() = False Then Exit Sub
            If CheckFileSize() = False Then Exit Sub
            If SetDataToDto() = False Then Exit Sub
            Call ConfirmMsg()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnCancel_Click
    '	Discription	    : btnCancel_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Call ClearSession()
            Response.Redirect("KTMS01.aspx?New=Back")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnCancel_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	    : btnClear_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 27-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Try
            Call ClearForm()
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnClear_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: CheckPageFirstLoad
    '	Discription	    : Check Page at First time Load 
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPageFirstLoad()
        Try
            ' Clear Branch page session
            If objUtility.GetQueryString("New") = "True" Then
                Session.Remove("dtBranchInquiry")
                Session.Remove("VendorBranchDto")
                Session.Remove("VendorBID")
                Session.Remove("ItemIndex")
                Session.Remove("objAction")
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckPageFirstLoad", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetInit
    '	Discription	    : Set Init page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetInit()
        Try
            Dim objPaymentService As New Service.ImpPaymentTermService
            Dim objCountrySer As New Service.ImpCountryService

            'Get data list to DropDownList
            If objCountrySer.SetListCountryName(ddlCountry, True, "Null") Then  'And objPaymentService.SetListPaymentDay(ddlPaymentTerm)
                'Set enable of button = True
                btnSave.Enabled = True
            Else
                'Set enable of button = False
                btnSave.Enabled = False
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetInit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckMode
    '	Discription	    : Check Mode page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckMode()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim strMode As String = objComm.GetQueryString("Mode")
            Dim strResConfirm As String = objComm.GetQueryString("ResConfirm")
            Dim strVendorId As String

            If strResConfirm = "True" Then
                objVendor = Session("toSaveVendorDto")
                Call ShowData()
                Call SaveDataVendor()
                Exit Sub
            End If

            Call ClearForm()
            Call ClearSession()
            Select Case strMode.Trim
                Case "Add"
                    Session("Mode") = "Add"
                    chkRemoveFile.Visible = False
                Case "Edit"
                    Session("Mode") = "Edit"
                    strVendorId = objComm.GetQueryString("ID")
                    Call GetDataVendorById(CInt(strVendorId))
                    Call ShowData()
            End Select

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckMode", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearForm
    '	Discription	    : Clear data on form
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearForm()
        Try
            rbtnType1.Items(0).Selected = False
            rbtnType1.Items(1).Selected = False
            rbtnType2.Items(0).Selected = False
            rbtnType2.Items(1).Selected = False
            txtType2No.Text = String.Empty
            txtName.Text = String.Empty
            txtShortName.Text = String.Empty
            txtPerson1.Text = String.Empty
            txtPerson2.Text = String.Empty
            ddlCountry.SelectedIndex = 0
            txtZipCode.Text = String.Empty
            txtAddress.Text = String.Empty
            txtTelNo.Text = String.Empty
            txtFaxNo.Text = String.Empty
            txtEmail.Text = String.Empty
            txtRemarks.Text = String.Empty
            LinkFileAttBtn.Text = String.Empty
            TypeOfGoodsCBList.Items(0).Selected = False
            TypeOfGoodsCBList.Items(1).Selected = False
            TypeOfGoodsCBList.Items(2).Selected = False
            Session.Remove("FileVendor")

            Call ClearMsgErr()

            If (Not Session("Mode") Is Nothing) AndAlso Session("Mode").ToString.Trim = "Edit" Then
                If Session("VendorDto") Is Nothing Then Exit Sub
                objVendor = Session("VendorDto")
                txtVendorId.Text = objVendor.id
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearForm", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearForm
    '	Discription	    : Clear data on form
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearMsgErr()
        Try
            'lblErrEmail.Visible = False
            'lblErrName.Visible = False
            'lblErrType1.Visible = False
            'lblErrType21.Visible = False
            'lblErrType22.Visible = False
            'lblErrTypeofGoods.Visible = False

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearMsgErr", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckData
    '	Discription	    : Check data vendor
    '	Return Value	: Boolean
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckData() As Boolean
        Dim bReturn As Boolean = True
        Try
            'Dim objComV As New Common.Validations.Validation
            'Dim objComM As New Common.Utilities.Message
            'Dim objComS As New Service.ImpCenterService
            'Dim iChkMsg As Boolean = False

            'Check short name if type customer
            If rbtnType1.Items(1).Selected AndAlso txtShortName.Text.Trim.Equals("") Then
                lblRequiredShortName.Text = "*Required"
                bReturn = False
            Else
                lblRequiredShortName.Text = ""
            End If
            'lblErrName.Visible = False
            'lblErrType1.Visible = False
            'lblErrType21.Visible = False
            'lblErrType22.Visible = False
            'lblErrTypeofGoods.Visible = False
            ''lblErrPayment.Visible = False
            'lblErrEmail.Visible = False

            'Check Type1
            'If rbtnType1.Items(0).Selected = False And rbtnType1.Items(1).Selected = False Then
            '    lblErrType1.Visible = True
            '    If iChkMsg = False Then iChkMsg = True
            'End If

            ''Check Type2
            'If rbtnType2.Items(0).Selected = False And rbtnType2.Items(1).Selected = False Then
            '    lblErrType21.Visible = True
            '    lblErrType22.Visible = True
            '    If iChkMsg = False Then iChkMsg = True
            'End If
            'If rbtnType2.Items(0).Selected = True Then
            '    If txtIDCardNo.Text.Trim = String.Empty Then
            '        lblErrType21.Visible = True
            '        If iChkMsg = False Then iChkMsg = True
            '    End If
            'End If
            'If rbtnType2.Items(1).Selected = True Then
            '    If txtTaxIDNo.Text.Trim = String.Empty Then
            '        lblErrType22.Visible = True
            '        If iChkMsg = False Then iChkMsg = True
            '    End If
            'End If

            ''Check Name
            'If txtName.Text.Trim = String.Empty Then
            '    lblErrName.Visible = True
            '    If iChkMsg = False Then iChkMsg = True
            'End If

            ''Check Type of Goods
            'If chkPurchase.Checked = False And chkOutsource.Checked = False Then
            '    lblErrTypeofGoods.Visible = True
            '    If iChkMsg = False Then iChkMsg = True
            'End If
            'If chkPurchase.Checked = True And chkOutsource.Checked = True Then
            '    lblErrTypeofGoods.Visible = True
            '    If iChkMsg = False Then iChkMsg = True
            'End If

            ''Check Payment Cond
            'If txtPaymentCond1.Text <> String.Empty Then
            '    If objComV.IsInteger(txtPaymentCond1.Text) = False Then
            '        lblErrPayment.Visible = True
            '        If iChkMsg = False Then iChkMsg = True
            '    End If
            'End If
            'If txtPaymentCond2.Text <> String.Empty Then
            '    If objComV.IsInteger(txtPaymentCond2.Text) = False Then
            '        lblErrPayment.Visible = True
            '        If iChkMsg = False Then iChkMsg = True
            '    End If
            'End If
            'If txtPaymentCond3.Text <> String.Empty Then
            '    If objComV.IsInteger(txtPaymentCond3.Text) = False Then
            '        lblErrPayment.Visible = True
            '        If iChkMsg = False Then iChkMsg = True
            '    End If
            'End If

            ''Check E-Mail
            'If txtEmail.Text <> String.Empty Then
            '    If objComV.IsEmail(txtEmail.Text) = False Then
            '        lblErrEmail.Visible = True
            '        If iChkMsg = False Then iChkMsg = True
            '    End If
            'End If

            'Check File Attached
            'If (Not fileAttached Is Nothing) Then
            '    'If objComS.CheckFile(fileAttached.FileBytes.Length) = False Then
            '    '    objComM.AlertMessage(String.Empty, "Common_003")
            '    '    If iChkMsg = False Then iChkMsg = True
            '    'End If
            '    'If fileAttached.FileName.Trim <> String.Empty AndAlso CheckLNameFile(fileAttached.FileName.Trim) = False Then

            '    '    If iChkMsg = False Then iChkMsg = True
            '    'End If
            'End If

            'If iChkMsg = False Then CheckData = True

        Catch ex As Exception
            ' Write error log
            'CheckData = False
            objLog.ErrorLog("CheckData", ex.Message.Trim, Session("UserName"))
        End Try
        CheckData = bReturn
    End Function

    '/**************************************************************
    '	Function name	: ConfirmMsg
    '	Discription	    : Check confirm message
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub ConfirmMsg()
        Try
            Dim objComm As New Common.Utilities.Message

            If hideCheckDupPass.Value <> True Then
                If Session("Mode") = "Add" Then
                    objComm.ConfirmMessage("KTMS02", "ResConfirm", String.Empty, "KTMS_02_001")
                ElseIf Session("Mode") = "Edit" Then
                    objComm.ConfirmMessage("KTMS02", "ResConfirm", String.Empty, "KTMS_02_007")
                End If
            Else
                Response.Redirect("KTMS02.aspx?ResConfirm=True")
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ConfirmMsg", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToDto
    '	Discription	    : Set data to Dto
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 23-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function SetDataToDto() As Boolean
        Try
            SetDataToDto = False
            'If (Not Session("VendorDto") Is Nothing) Then objVendor = Session("VendorDto")

            With objVendor
                .id = IIf(txtVendorId.Text.Trim <> "", txtVendorId.Text.Trim, 0)
                .type1 = rbtnType1.SelectedValue
                .type2 = rbtnType2.SelectedValue
                .type2_no = txtType2No.Text
                .name = txtName.Text.Trim
                .short_name = txtShortName.Text.Trim
                .person_in_charge1 = txtPerson1.Text
                .person_in_charge2 = txtPerson2.Text
                .country_id = GetValueDDL(ddlCountry.SelectedValue)
                .zipcode = txtZipCode.Text
                .address = txtAddress.Text
                .tel = txtTelNo.Text
                .fax = txtFaxNo.Text
                .email = txtEmail.Text
                .remarks = txtRemarks.Text
                .checkDelete = chkRemoveFile.Checked
                .purchase_fg = IIf(TypeOfGoodsCBList.Items(0).Selected, 1, 0)
                .outsource_fg = IIf(TypeOfGoodsCBList.Items(1).Selected, 1, 0)
                .other_fg = IIf(TypeOfGoodsCBList.Items(2).Selected, 1, 0)
                If fileAttached.FileName <> Nothing Then
                    .file = .name.ToUpper & "_" & DateTime.Now.ToString("yyyyMMddHHmmss") & "." & fileAttached.FileName.Split(".")(1).Trim
                    SaveFile(.file)
                Else
                    .file = LinkFileAttBtn.Text
                End If
            End With
            Session("FileVendor") = fileAttached
            Session("toSaveVendorDto") = objVendor
            SetDataToDto = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataToDto", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SaveFile
    '	Discription	    : SaveFile
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 06-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SaveFile(ByVal fileName As String)
        Try
            ' Path to save file
            Dim strPath As String = Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "Vendor/")
            If fileAttached.FileName <> "" Then
                ' Save file as
                fileAttached.SaveAs(strPath & fileName)
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataToDto", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetFileName
    '	Discription	    : Set FileName
    '	Return Value	: String
    '	Create User	    : Boonyarit
    '	Create Date	    : 06-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function SetNewFileName(ByVal fileUploadName As String) As String
        Try
            Dim objFileVendor As FileUpload = CType(Session("FileVendor"), FileUpload)
            Dim objAry() As String = objFileVendor.FileName.Split(".")
            Dim strNewFileName As String = String.Empty

            SetNewFileName = String.Empty
            If (Not Session("VendorDto") Is Nothing) Then objVendor = Session("VendorDto")

            'strNewFileName = Format(objVendor.type1, "00") & "-" & Format(objVendor.type2, "00") & _
            '                 "-" & objVendor.name.ToUpper & "." & objAry(1).Trim
            strNewFileName = objVendor.name.ToUpper & "_" & DateTime.Now.ToString("yyyyMMddHHmmss") & "." & objAry(1).Trim
            SetNewFileName = strNewFileName.Trim

        Catch ex As Exception
            ' Write error log
            SetNewFileName = String.Empty
            objLog.ErrorLog("SetFileName", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: CheckLNameFile
    '	Discription	    : Check Last name file
    '	Return Value	: Boolean
    '	Create User	    : Boonyarit
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckLNameFile(ByVal strFileName As String) As Boolean
        Try
            Dim objLName() As String = {"BMP", "WMF", "EMF", "ICO", "JPG", _
                                        "GIF", "PNG", "TIFF", "PCX", "PCC", _
                                        "DCX", "PBM", "PGM", "PPM", "TGA", _
                                        "VST", "AFI", "TXT", "LOG"}

            Dim objAry() As String = strFileName.Split(".")
            Dim intRes As Integer = Array.IndexOf(objLName, objAry(1).Trim)

            If intRes = -1 Then
                CheckLNameFile = False
            Else
                CheckLNameFile = True
            End If

        Catch ex As Exception
            ' Write error log
            CheckLNameFile = False
            objLog.ErrorLog("CheckLNameFile", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: ShowData
    '	Discription	    : Show data no form
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 23-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowData()
        Try
            With objVendor
                txtVendorId.Text = IIf(Session("Mode") = "Edit", .id, "")
                Select Case .type2
                    Case "0" : lblType2IDNo.Text = "Identification Card No."
                    Case "1" : lblType2IDNo.Text = "Taxpayer Identification No."
                    Case Else : lblType2IDNo.Text = "Type2 ID No."
                End Select
                rbtnType1.SelectedValue = .type1
                rbtnType2.SelectedValue = .type2
                txtHideType1.Value = .type1
                txtHideType2.Value = .type2
                txtType2No.Text = .type2_no
                txtName.Text = .name.ToUpper
                txtShortName.Text = .short_name
                txtPerson1.Text = .person_in_charge1
                txtPerson2.Text = .person_in_charge2
                ddlCountry.SelectedValue = .country_id
                txtZipCode.Text = .zipcode
                txtAddress.Text = .address
                txtTelNo.Text = .tel
                txtFaxNo.Text = .fax
                txtEmail.Text = .email
                txtRemarks.Text = .remarks

                If .type1 = 0 Then
                    TypeOfGoodsCBList.Items(0).Selected = .purchase_fg
                    TypeOfGoodsCBList.Items(1).Selected = .outsource_fg
                    TypeOfGoodsCBList.Items(2).Selected = .other_fg
                    lblErrShortName.Text = ""
                Else
                    TypeOfGoodsCBList.Attributes("disabled") = True
                    lblErrShortName.Text = "*"
                End If
                LinkFileAttBtn.Text = IIf(.file.Trim <> Nothing, .file.Trim, "")

            End With
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ShowData", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SaveDataVendor
    '	Discription	    : Save data to DB
    '	Return Value	: Boolean
    '	Create User	    : Boonyarit
    '	Create Date	    : 23-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SaveDataVendor()
        Try
            ' Check data
            If CheckData() = False Then Exit Sub

            Dim objComm As New Common.Utilities.Message
            Dim objService As New Service.ImpVendorService
            Dim strMode As String = Session("Mode")
            Dim fileUpSession As FileUpload = Session("FileVendor")

            objVendor = Session("toSaveVendorDto")

            If fileUpSession.FileName <> "" Or objVendor.checkDelete = True Or objVendor.file = Nothing Then
                Call SaveAndDelFileVendor(objVendor.file.Trim)
            End If

            'Save data vendor
            If objService.SaveVendor(objVendor, strMode) Then
                Dim objBranchSer As New Service.ImpVendorBranchService
                ' Case add Branch Address
                If Not IsNothing(Session("dtBranchInquiry")) Then
                    If CType(Session("dtBranchInquiry"), DataTable).Rows.Count > 0 Then
                        If Not objBranchSer.SaveVendorBranch(Session("dtBranchInquiry"), objVendor.id) Then
                            'Check mode is show message
                            Select Case strMode
                                Case "Add"
                                    objComm.AlertMessage(String.Empty, "KTMS_02_BRANCH_004", "KTMS01.aspx?New=Back")
                                Case "Edit"
                                    objComm.AlertMessage(String.Empty, "KTMS_02_BRANCH_006", "KTMS01.aspx?New=Back")
                            End Select
                            'Clear session all
                            Call ClearSession()
                            Exit Sub
                        End If
                    End If
                End If
                'Check mode is show message
                Select Case strMode
                    Case "Add"
                        objComm.AlertMessage(String.Empty, "KTMS_02_003", "KTMS01.aspx?New=Back")
                    Case "Edit"
                        objComm.AlertMessage(String.Empty, "KTMS_02_005", "KTMS01.aspx?New=Back")
                End Select
                'Clear session all
                Call ClearSession()
            Else
                'Add not successful
                Select Case strMode
                    Case "Add"
                        objComm.AlertMessage(String.Empty, "KTMS_02_004")
                    Case "Edit"
                        objComm.AlertMessage(String.Empty, "KTMS_02_006")
                End Select
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SaveDataVendor", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SaveAndDelFileVendor
    '	Discription	    : Save file vendor
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 06-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SaveAndDelFileVendor(ByVal strFileName As String)
        Try
            Dim objCenter As New Service.ImpCenterService
            Dim strPath As String = String.Empty
            Dim strMode As String = Session("Mode")
            Dim fileUpSession As FileUpload = Session("FileVendor")

            strPath = Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "Vendor/" & strFileName.Trim)
            ' Check Condition for delete file upload
            If (strMode = "Edit" And fileUpSession.FileName <> "") _
                                 Or (objVendor.checkDelete = True) _
                                 Or (objVendor.file = Nothing) Then
                'Delete file
                strPath = Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "Vendor/" & Session("OldFileName"))
                Call objCenter.DeleteFile(strPath)
            End If
            strPath = Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "Vendor/" & strFileName.Trim)
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SaveAndDelFileVendor", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetDataVendorById
    '	Discription	    : Get data vendor by id
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 23-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetDataVendorById(ByVal intVendorId As Integer)
        Try
            Dim objService As New Service.ImpVendorService

            objVendor = objService.GetVendorForDetail(intVendorId)
            Session("VendorDto") = objVendor
            If (Not objVendor.file Is Nothing) AndAlso objVendor.file <> String.Empty Then
                Session("OldFileName") = objVendor.file
            Else
                Session("OldFileName") = Nothing
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("GetDataVendorById", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session all
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 30-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            Session.Remove("Mode")
            Session.Remove("VendorDto")
            Session.Remove("toSaveVendorDto")
            Session.Remove("FileVendor")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearSession", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckFileUpload
    '	Discription	    : Check data file vendor
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 30-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckFileUpload()
        Try
            If (IsPostBack) Then
                If (Session("FileVendor") Is Nothing) AndAlso fileAttached.HasFile Then
                    Session("FileVendor") = fileAttached
                ElseIf (Not (Session("FileVendor") Is Nothing)) AndAlso (Not (fileAttached.HasFile)) Then
                    fileAttached = CType(Session("FileVendor"), FileUpload)
                ElseIf (fileAttached.HasFile) Then
                    Session("FileVendor") = fileAttached
                End If
                'Show file name of FileUpload
                If Session("Mode") = "Add" Then
                    LinkFileAttBtn.Text = fileAttached.FileName
                End If
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckFileUpload", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckUserPer
    '	Discription	    : Check user permission
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckUserPer()
        Try
            Dim objComUser As New Common.UserPermissions.UserPermission
            Dim objActUser As New Common.UserPermissions.ActionPermission

            'Check permission user
            objActUser = objComUser.CheckPermission(Enums.MenuId.Vendor)
            Session("ActUser") = objActUser

            btnSave.Enabled = objActUser.actCreate

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckUserPer", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetValueDDL
    '	Discription	    : Get value ddl(DropDownList)
    '	Return Value	: Integer
    '	Create User	    : Boonyarit
    '	Create Date	    : 15-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function GetValueDDL(ByVal strValue As String) As Integer
        Try
            If strValue = String.Empty Then
                GetValueDDL = 0
            Else
                GetValueDDL = CInt(strValue)
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("GetValueDDL", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: CheckFileSize
    '	Discription	    : Check file size
    '	Return Value	: Boolean
    '	Create User	    : Wasan D.
    '	Create Date	    : 25-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckFileSize() As Boolean
        Try
            If fileAttached.FileName.ToString <> Nothing Then
                Dim SelectedFileSize As Long = fileAttached.FileBytes.Length
                Dim LimitedFileSize As Long = WebConfigurationManager.AppSettings("FileUploadMaxLength")
                If SelectedFileSize > LimitedFileSize Then
                    FileSizeValidator.IsValid = False
                    Return False
                Else
                    FileSizeValidator.IsValid = True
                    Return True
                End If
            Else
                Return True
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckFileSize", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Function
#End Region

#Region "Web Method"

    '/**************************************************************
    '	Function name	: CheckNotDup
    '	Discription	    : Check data duplicate of vendor name
    '	Return Value	: Boolean
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    <WebMethod()> _
    Public Shared Function CheckNotDupInBehind( _
                ByVal strVendorID As String, _
                ByVal strType1 As String, _
                ByVal strType2 As String, _
                ByVal strName As String) As String
        Dim objLog As New Common.Logs.Log
        Try
            Dim objComm As New Common.Utilities.Message
            Dim objService As New Service.ImpVendorService
            Dim objVendorDupDto As New Dto.VendorDto
            Dim strMode As String = HttpContext.Current.Session("Mode")

            CheckNotDupInBehind = "Cannot check duplicate data entry."
            With objVendorDupDto
                'Set value to dto
                .type1 = strType1
                .type2 = strType2
                .name = strName
                'Set value id
                If strVendorID <> String.Empty Then
                    .id = CInt(strVendorID)
                End If
            End With
            'Check data vendor is duplicate
            If objService.CheckIsDupVendor(objVendorDupDto) Then
                If strMode = "Add" Then
                    CheckNotDupInBehind = objComm.GetXMLMessage("KTMS_02_002")
                ElseIf strMode = "Edit" Then
                    CheckNotDupInBehind = objComm.GetXMLMessage("KTMS_02_008")
                End If
            Else
                CheckNotDupInBehind = "Pass"
            End If

        Catch ex As Exception
            ' Write error log
            CheckNotDupInBehind = "Cannot check duplicate data entry."
            objLog.ErrorLog("CheckNotDupInBehind", ex.Message.Trim, HttpContext.Current.Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: ConfirmMsgCheck
    '	Discription	    : Check comfirm message mode
    '	Return Value	: string
    '	Create User	    : Wasan D.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    <WebMethod()> _
    Public Shared Function ConfirmMsgCheck() As String
        Dim objLog As New Common.Logs.Log
        Dim objComm As New Common.Utilities.Message
        ConfirmMsgCheck = ""
        Try

            If HttpContext.Current.Session("Mode") = "Add" Then
                ConfirmMsgCheck = objComm.GetXMLMessage("KTMS_02_001")
            ElseIf HttpContext.Current.Session("Mode") = "Edit" Then
                ConfirmMsgCheck = objComm.GetXMLMessage("KTMS_02_007")
            End If

        Catch ex As Exception
            ' Write error log
            ConfirmMsgCheck = ""
            objLog.ErrorLog("ConfirmMsgCheck", ex.Message.Trim, HttpContext.Current.Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: checkFileUploadSize
    '	Discription	    : Check file upload size
    '	Return Value	: Boolean
    '	Create User	    : Wasan D.
    '	Create Date	    : 06-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    <WebMethod()> _
    Public Shared Function checkFileUploadSize(ByVal strFileUploadPath As String) As Boolean
        Dim objLog As New Common.Logs.Log
        checkFileUploadSize = False
        Try
            ' Get file size limit from web.config
            Dim fileLimitedSize As Long = WebConfigurationManager.AppSettings("FileUploadMaxLength")
            Dim getFileInfo As New FileInfo(strFileUploadPath)
            If getFileInfo.Length > fileLimitedSize Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("checkFileUploadSize", ex.Message.Trim, HttpContext.Current.Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: LinkFileAttBtn_Click
    '	Discription	    : Event LinkFileAttBtn Click
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    <WebMethod()> _
    Public Shared Function LinkFileAttBtn_Click(ByVal strFileName As String) As String 'Handles LinkFileAttBtn.Click
        Dim objLog As New Common.Logs.Log
        Dim objMessage As New Common.Utilities.Message
        ' Set default return value
        LinkFileAttBtn_Click = ""
        Try
            Dim strPath As String = WebConfigurationManager.AppSettings("FilePaths") & "Vendor/" & strFileName
            Dim strFullPath As String = HttpContext.Current.Server.MapPath(strPath)
            Dim strNoFileFound As String = WebConfigurationManager.AppSettings("FilePaths") & "Vendor/FileNotFound.html"
            Dim objwriter As StreamWriter
            ' Check exists file
            If File.Exists(strFullPath) = True Then
                Return strPath
            Else
                ' Create File "FileNotFound.txt"
                objwriter = New StreamWriter(HttpContext.Current.Server.MapPath(strNoFileFound))
                ' Write text in File
                objwriter.Write("<html><html><head><title></title></head><body><h1>File not found.</h1></body></html>")
                objwriter.Close()
                Return strNoFileFound
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkFileAttBtn_Click", ex.Message.Trim, HttpContext.Current.Session("UserName"))
        End Try
    End Function
#End Region
End Class
