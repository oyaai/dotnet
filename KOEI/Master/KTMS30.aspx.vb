Imports Enums

Partial Class Master_KTMS30
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objSRate As New Dto.ScheculeRateDto

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' Write start log
            objLog.StartLog("KTMS30: SCHEDULE RATE MANAGEMENT", Session("UserName"))
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
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                'Set data page default
                Call SetInit()
                Call CheckUserPer()
                Call CheckMode()
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Load", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	    : btnClear_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
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

    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	    : btnBack_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Try
            Call ClearSession()
            Response.Redirect("KTMS29.aspx?New=Back")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	    : btnSave_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            'Check data on form
            If CheckDataForm() = False Then Exit Sub
            'Set data to object Dto
            If SetDataToDto() = False Then Exit Sub
            'If CheckData() = False Then Exit Sub
            Call ConfirmMsg()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region
    
#Region "Function"
    '/**************************************************************
    '	Function name	: SetInit
    '	Discription	    : Set Init page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetInit()
        Try
            Dim objService As New Service.ImpCurrencyService
            Dim objListDto As New List(Of Dto.CurrencyDto)
            Dim objCom As New Common.Utilities.Utility
            Dim objMsg As New Common.Utilities.Message

            'reqCurrency.ErrorMessage = objMsg.GetXMLMessage("KTMS_29_004")
            rngRate.ErrorMessage = objMsg.GetXMLMessage("KTMS_30_009")
            lblMsgRate.Text = objMsg.GetXMLMessage("KTMS_30_009")

            'Get data list to DropDownList
            objListDto = objService.GetCurrencyForList
            If (Not objListDto Is Nothing) AndAlso (objListDto.Count > 0) Then
                Call objCom.LoadList(ddlCurrency, objListDto, "name", "id")
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
    '	Function name	: CheckUserPer
    '	Discription	    : Check user permission
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckUserPer()
        Try
            Dim objComUser As New Common.UserPermissions.UserPermission
            Dim objActUser As New Common.UserPermissions.ActionPermission

            'Check permission user
            objActUser = objComUser.CheckPermission(MenuId.ScheduleExchange)
            Session("ActUser") = objActUser

            btnSave.Enabled = objActUser.actCreate

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckUserPer", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckMode
    '	Discription	    : Check Mode page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckMode()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim strMode As String = objComm.GetQueryString("Mode")
            Dim strResConfirm As String = objComm.GetQueryString("ResConfirm")
            Dim strSRateId As String

            Call ClearForm()
            Select Case strMode.Trim
                Case "Add"
                    Session("Mode") = "Add"

                Case "Edit"
                    Session("Mode") = "Edit"
                    strSRateId = objComm.GetQueryString("ID")
                    Call GetDataSRateById(CInt(strSRateId))
                    Call ShowData()
            End Select
            If strResConfirm = "True" Then
                Call ShowData()
                Call SaveDataSRate()
            End If

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
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearForm()
        Try
            If Session("Mode") <> "Edit" Then txtSRateId.Text = String.Empty
            ddlCurrency.SelectedIndex = 0
            lblMsgCurrency.Visible = False
            txtEF_Date.Text = String.Empty
            lblMsgEF_date.Visible = False
            txtRate.Text = String.Empty
            lblMsgRate.Visible = False

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearForm", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToDto
    '	Discription	    : Set data to Dto
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function SetDataToDto() As Boolean
        Try
            SetDataToDto = False
            If (Not Session("SRateDto") Is Nothing) Then objSRate = Session("SRateDto")

            With objSRate
                If txtSRateId.Text <> String.Empty Then
                    .id = CInt(txtSRateId.Text)
                End If
                .currency_id = CInt(ddlCurrency.SelectedValue)
                .currency = ddlCurrency.Items(ddlCurrency.SelectedIndex).Text
                .ef_date = txtEF_Date.Text
                .rate = CDec(txtRate.Text)
            End With
            Session("SRateDto") = objSRate
            SetDataToDto = True

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataToDto", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: CheckDataForm
    '	Discription	    : Check data form Schecule Rate
    '	Return Value	: Boolean
    '	Create User	    : Boonyarit
    '	Create Date	    : 23-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckDataForm() As Boolean
        CheckDataForm = False
        Try
            Dim objCom As New Common.Validations.Validation

            'Check data Currency
            If ddlCurrency.SelectedIndex = 0 Then
                lblMsgCurrency.Visible = True
                Exit Function
            Else
                lblMsgCurrency.Visible = False
            End If

            'Check data Effective Date
            If objCom.IsDate(txtEF_Date.Text) = False Then
                lblMsgEF_date.Visible = True
                Exit Function
            Else
                lblMsgEF_date.Visible = False
            End If

            ''Check data Effective Date ห้ามมากกว่าวันที่ปัจจุบัน
            'If objCom.IsDateFromTo(txtEF_Date.Text, Now.ToString("dd/MM/yyyy")) = False Then
            '    lblMsgEF_date.Visible = True
            '    Exit Function
            'Else
            '    lblMsgEF_date.Visible = False
            'End If

            'Check data Rate
            If txtRate.Text <> String.Empty Then
                If Val(txtRate.Text) = 0 Then
                    lblMsgRate.Visible = True
                    Exit Function
                Else
                    lblMsgRate.Visible = False
                End If
            End If

            CheckDataForm = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckDataForm", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: CheckData
    '	Discription	    : Check data Schecule Rate
    '	Return Value	: Boolean
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckData() As Boolean
        Try
            Dim objServer As New Service.ImpScheculeRateService
            Dim objCom As New Common.Utilities.Message

            objSRate = Session("SRateDto")
            CheckData = False

            If objServer.CheckDataIsDup(objSRate) = True Then
                If Session("Mode") = "Add" Then
                    objCom.AlertMessage(String.Empty, "KTMS_30_004")
                ElseIf Session("Mode") = "Edit" Then
                    objCom.AlertMessage(String.Empty, "KTMS_30_007")
                End If
                Exit Function
            Else
                CheckData = True
            End If

        Catch ex As Exception
            ' Write error log
            CheckData = False
            objLog.ErrorLog("CheckData", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: ConfirmMsg
    '	Discription	    : Check confirm message
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub ConfirmMsg()
        Try
            Dim objComm As New Common.Utilities.Message

            If Session("Mode") = "Add" Then
                objComm.ConfirmMessage("KTMS30", "ResConfirm", String.Empty, "KTMS_30_001")
            ElseIf Session("Mode") = "Edit" Then
                objComm.ConfirmMessage("KTMS30", "ResConfirm", String.Empty, "KTMS_30_008")
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ConfirmMsg", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowData
    '	Discription	    : Show data no form
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowData()
        Try
            Dim objCom As New Common.Utilities.Utility

            If Session("SRateDto") Is Nothing Then Exit Sub
            objSRate = Session("SRateDto")

            With objSRate
                If Session("Mode") = "Edit" Then txtSRateId.Text = .id
                ddlCurrency.SelectedValue = .currency_id
                txtEF_Date.Text = .ef_date
                txtRate.Text = objCom.FormatNumeric(.rate, 5)
            End With

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ShowData", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SaveDataSRate
    '	Discription	    : Save data to DB
    '	Return Value	: Boolean
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SaveDataSRate()
        Try
            Dim objComm As New Common.Utilities.Message
            Dim objService As New Service.ImpScheculeRateService
            Dim strMode As String = Session("Mode")

            objSRate = Session("SRateDto")

            'Check data dup
            If CheckData() = False Then Exit Sub

            'Save data Schecule_Rate
            If objService.SaveScheculeRate(objSRate, strMode) Then
                'Add successful
                Call ClearSession()
                Select Case strMode
                    Case "Add"
                        objComm.AlertMessage(String.Empty, "KTMS_30_002", "KTMS29.aspx?New=Back")
                    Case "Edit"
                        objComm.AlertMessage(String.Empty, "KTMS_30_005", "KTMS29.aspx?New=Back")
                End Select
            Else
                'Add not successful
                Select Case strMode
                    Case "Add"
                        objComm.AlertMessage(String.Empty, "KTMS_30_003")
                    Case "Edit"
                        objComm.AlertMessage(String.Empty, "KTMS_30_006")
                End Select
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SaveDataSRate", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetDataSRateById
    '	Discription	    : Get data Schecule_Rate by id
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetDataSRateById(ByVal intSRateId As Integer)
        Try
            Dim objService As New Service.ImpScheculeRateService

            objSRate = objService.GetScheculeRateForDetail(intSRateId)
            Session("SRateDto") = objSRate

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("GetDataSRateById", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session all
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            Session.Remove("Mode")
            Session.Remove("ActUser")
            Session.Remove("SRateDto")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearSession", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region

End Class


