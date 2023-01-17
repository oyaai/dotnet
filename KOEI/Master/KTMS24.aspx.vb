
Partial Class Master_KTMS24
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private objDepartmentSer As New Service.ImpDepartmentService
    Private objMessage As New Common.Utilities.Message
    Private strMsg As String = String.Empty

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	: Event page initial
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS24 : Department Master")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	: Event page load
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ' check postback page
            If Not IsPostBack Then
                ' case not post back then call function initialpage
                InitialPage()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	: Event btnBack is click
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 05-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Try
            Call ClearSession()
            Response.Redirect("KTMS23.aspx?New=Back")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	: Event btnClear is click
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 05-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Try
            ' call function ClearControl
            ClearControl()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnClear_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	: Event btnSave is click
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 05-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            ' call function set session dto
            SetValueToDto()

            ' check data Department is null
            lblErrName.Visible = False
            If txtName.Text.Trim = String.Empty Then
                lblErrName.Visible = True
                Exit Sub
            End If

            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTMS24", strConfirmIns, objMessage.GetXMLMessage("KTMS_24_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTMS24", strConfirmUpd, objMessage.GetXMLMessage("KTMS_24_008"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	: Initial page function
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 07-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub InitialPage()
        Try
            ' check insert Department
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                InsertDepartment()
                Exit Sub
            End If

            ' check update Department
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateDepartment()
                Exit Sub
            End If

            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                Session("Mode") = "Add"
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                LoadInitialUpdate()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: InsertDepartment
    '	Discription	: Insert Department
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 05-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub InsertDepartment()
        Try
            ' call function set value to control
            SetValueToControl()

            'check duplicate
            If objDepartmentSer.CheckDupDepartment(txtName.Text) = False Then
                ' call function InsertDepartment from service and alert message
                If objDepartmentSer.InsertDepartment(Session("objDepartmentDto"), strMsg) Then
                    Call ClearSession()
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_24_002"), Nothing, "KTMS23.aspx?New=Back")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_24_004"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertDepartment", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	: Set value to control
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 05-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Item dto object
            Dim objDepartmentDto As New Dto.DepartmentDto
            ' set value to dto object from session
            objDepartmentDto = Session("objDepartmentDto")

            ' set value to control
            With objDepartmentDto
                If .id = 0 Then
                    txtId.Text = String.Empty
                Else
                    txtId.Text = .id
                End If

                txtName.Text = .name
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: UpdateDepartment
    '	Discription	: Update Department
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 05-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub UpdateDepartment()
        Try
            Dim intID As Integer = 0

            ' call function set value to control
            SetValueToControl()

            If txtId.Text.Trim <> String.Empty Then intID = Val(txtId.Text)

            'check duplicate
            If objDepartmentSer.CheckDupDepartment(txtName.Text, intID) = False Then
                ' call function UpdateDepartment from service and alert message
                If objDepartmentSer.UpdateDepartment(Session("objDepartmentDto"), strMsg) Then
                    Call ClearSession()
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_24_005"), Nothing, "KTMS23.aspx?New=Back")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_24_007"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateDepartment", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	: Load initial for update data
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 05-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' Item Dto object for keep return value from service
            Dim objDepartmentDto As New Dto.DepartmentDto
            Dim intDepartmentID As Integer = 0

            ' check Department id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intDepartmentID = CInt(objUtility.GetQueryString("id"))
            End If

            ' call function GetItemByID from service
            objDepartmentDto = objDepartmentSer.GetDepartmentByID(intDepartmentID)

            ' assign value to control
            With objDepartmentDto
                txtId.Text = .id
                txtName.Text = .name
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: ClearControl
    '	Discription	: Clear data each control
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 05-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' clear control
            If Session("Mode") <> "Edit" Then txtId.Text = String.Empty
            txtName.Text = String.Empty
            lblErrName.Visible = False

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	: Set value to Dto
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 05-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Department dto object
            Dim objDepartmentDto As New Dto.DepartmentDto

            ' assign value to dto object
            With objDepartmentDto
                If String.IsNullOrEmpty(txtId.Text.Trim) Then
                    .id = 0
                Else
                    .id = txtId.Text.Trim
                End If

                .name = txtName.Text.Trim
            End With

            ' set dto object to session
            Session("objDepartmentDto") = objDepartmentDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	: Clear value in Session
    '	Return Value	: nothing
    '	Create User	: Boonyarit
    '	Create Date	: 15-07-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub ClearSession()
        Try
            Session.Remove("Mode")
            Session.Remove("objDepartmentDto")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region


End Class

