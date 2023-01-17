Imports Enums

Partial Class Master_KTMS12
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private check_menu_id As MenuId = MenuId.Unit

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' Write start log
            objLog.StartLog("KTMS12: Unit Management", Session("UserName"))
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
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Session("UserName") Is Nothing Then Session("UserName") = "Boonyarit"
            If Not Page.IsPostBack Then
                'Set data page default
                Call CheckUserPer()
                Call CheckMode()
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
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Call SetDataToSession()
            Call ConfirmMsg()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	    : btnClear_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 04-06-2013
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
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Try
            Call ClearSession()
            Response.Redirect("KTMS11.aspx?New=Back")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: ClearForm
    '	Discription	    : Clear data on form
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearForm()
        Try
            txtUnitId.Text = String.Empty
            txtUnitName.Text = String.Empty

            If (Not Session("UnitId") Is Nothing) AndAlso (Not Session("Mode") Is Nothing) _
                AndAlso Session("Mode").ToString.Trim = "Edit" Then

                txtUnitId.Text = Session("UnitId").ToString
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearForm", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckMode
    '	Discription	    : Check Mode page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckMode()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim strMode As String = objComm.GetQueryString("Mode")
            Dim strResConfirm As String = objComm.GetQueryString("ResConfirm")
            Dim strUnitId As String
            Dim strUnitName As String

            Call ClearForm()
            Select Case strMode.Trim
                Case "Add"
                    Session("Mode") = "Add"
                Case "Edit"
                    Session("Mode") = "Edit"
                    strUnitId = objComm.GetQueryString("ID")
                    strUnitName = objComm.GetQueryString("Name")
                    strUnitName = Server.HtmlDecode(strUnitName)
                    Session("UnitId") = strUnitId
                    Session("UnitName") = strUnitName
                    Call ShowData()
            End Select
            If strResConfirm = "True" Then
                Call ShowData()
                Call SaveDataUnit()
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckMode", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckUserPer
    '	Discription	    : Check user permission
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckUserPer()
        Try
            Dim objComUser As New Common.UserPermissions.UserPermission
            Dim objActUser As New Common.UserPermissions.ActionPermission

            objActUser = objComUser.CheckPermission(check_menu_id)

            'Fix enabled by permission
            btnSave.Enabled = objActUser.actCreate
            btnClear.Enabled = True
            btnBack.Enabled = True

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckUserPer", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowData
    '	Discription	    : Show data no form
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowData()
        Try
            If (Not Session("UnitId") Is Nothing) AndAlso Val(Session("UnitId")) > 0 Then
                txtUnitId.Text = Session("UnitId").ToString.Trim
            Else
                txtUnitId.Text = String.Empty
            End If
            If (Not Session("UnitName") Is Nothing) Then
                txtUnitName.Text = Session("UnitName").ToString.Trim
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ShowData", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session all
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            Session.Remove("Mode")
            Session.Remove("UnitId")
            Session.Remove("UnitName")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearSession", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckData
    '	Discription	    : Check data unit
    '	Return Value	: Boolean
    '	Create User	    : Boonyarit
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckData() As Boolean
        Try
            Dim objService As New Service.ImpUnitService
            Dim objUnitDto As New Dto.UnitDto
            Dim objCom As New Common.Utilities.Message
            Dim strMode As String = Session("Mode")

            CheckData = False
            ' check data unit_name
            If txtUnitName.Text.Trim = String.Empty Then Exit Function

            objUnitDto.name = txtUnitName.Text.Trim
            If txtUnitId.Text.Trim <> String.Empty Then objUnitDto.id = CInt(txtUnitId.Text)

            ' check data duplicate
            If objService.CheckIsDupUnit(objUnitDto.name, objUnitDto.id) Then
                Select Case strMode
                    Case "Add"
                        objCom.AlertMessage(String.Empty, "KTMS_12_002")
                    Case "Edit"
                        objCom.AlertMessage(String.Empty, "KTMS_12_008")
                End Select
                Exit Function
            End If

            Session("UnitId") = objUnitDto.id
            Session("UnitName") = objUnitDto.name
            CheckData = True

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
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub ConfirmMsg()
        Try
            Dim objComm As New Common.Utilities.Message

            If Session("Mode") = "Add" Then
                objComm.ConfirmMessage("KTMS12", "ResConfirm", String.Empty, "KTMS_12_001")
            ElseIf Session("Mode") = "Edit" Then
                objComm.ConfirmMessage("KTMS12", "ResConfirm", String.Empty, "KTMS_12_007")
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ConfirmMsg", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToSession
    '	Discription	    : Save data to Session
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession()
        Try
            If Session("UnitId") Is Nothing Then Session("UnitId") = 0
            If Session("UnitName") Is Nothing Then Session("UnitName") = String.Empty

            If txtUnitId.Text.Trim <> String.Empty Then Session("UnitId") = txtUnitId.Text.Trim
            If txtUnitName.Text.Trim <> String.Empty Then Session("UnitName") = txtUnitName.Text.Trim

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataToSession", ex.Message.Trim, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: SaveDataUnit
    '	Discription	    : Save data to DB
    '	Return Value	: Boolean
    '	Create User	    : Boonyarit
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SaveDataUnit()
        Try
            Dim objComm As New Common.Utilities.Message
            Dim objService As New Service.ImpUnitService
            Dim strMode As String = Session("Mode")
            Dim objUnitDto As New Dto.UnitDto

            With objUnitDto
                .id = Session("UnitId")
                .name = Session("UnitName")
            End With

            ' Check data
            If CheckData() = False Then Exit Sub

            If objService.SaveUnit(objUnitDto, strMode) Then
                'Add successful
                Call ClearSession()
                Select Case strMode
                    Case "Add"
                        objComm.AlertMessage(String.Empty, "KTMS_12_003", "KTMS11.aspx?New=Back")
                    Case "Edit"
                        objComm.AlertMessage(String.Empty, "KTMS_12_005", "KTMS11.aspx?New=Back")
                End Select
            Else
                'Add not successful
                Select Case strMode
                    Case "Add"
                        objComm.AlertMessage(String.Empty, "KTMS_12_004")
                    Case "Edit"
                        objComm.AlertMessage(String.Empty, "KTMS_12_006")
                End Select
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SaveDataUnit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region

End Class

