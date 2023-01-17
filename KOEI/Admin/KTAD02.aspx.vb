#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Admin
'	Class Name		    : Admin_KTAD02
'	Class Discription	: Webpage for maintenance User Login Admin
'	Create User 		: Nisa S.
'	Create Date		    : 11-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class Admin_KTAD02
    Inherits System.Web.UI.Page

    Private objUtility As New Common.Utilities.Utility
    Private objLog As New Common.Logs.Log
    Private objUserLoginSer As New Service.ImpUserLoginService
    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private strMsg As String = String.Empty
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission



    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTAD02 : User Login Admin")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Load
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
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            'Set QueryString
            If Not String.IsNullOrEmpty(Request.QueryString("id")) And Request.QueryString("id") Is Nothing Then
                Session("intUserLoginID") = 0
            Else
                Session("intUserLoginID") = Request.QueryString("id")
            End If



            ' call function set WorkCategory dropdownlist
            LoadListDepartment()
            LoadListAccount_Next_Approve()
            LoadListPurchase_Next_Approve()
            'LoadListOutsource_Next_Approve()

            ' check insert staff
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                ' call function clear session
                'ClearControl()
                InsertUserLogin()
                Exit Sub
            End If


            ' check update staff
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateUserLogin()
                Exit Sub
            End If

            'SetValueToControl()

            
            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                'ClearControl()
                Session("Mode") = "Add"
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                Session("intUserLoginID") = Request.QueryString("user_name")
                LoadInitialUpdate()
            End If

            ' call function check permission
            CheckPermission()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListDepartment
    '	Discription	    : Load list Department function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListDepartment()
        Try
            ' object Department service
            Dim objDepartmentSer As New Service.ImpUserLoginService
            ' listDepartmentDto for keep value from service
            Dim listDepartmentDto As New List(Of Dto.UserLoginDto)
            ' call function GetDepartmentForList from service
            listDepartmentDto = objDepartmentSer.GetDepartmentForList

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlDepartment, listDepartmentDto, "name", "id", True)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListDepartment", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListAccount_Next_Approve 
    '	Discription	    : Load list Account_Next_Approve function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListAccount_Next_Approve()
        Try
            ' object Account_Next_Approve service
            Dim objAccount_Next_ApproveSer As New Service.ImpUserLoginService
            ' listAccount_Next_ApproveDto for keep value from service
            Dim listAccount_Next_ApproveDto As New List(Of Dto.UserLoginDto)
            ' call function GetAccount_Next_ApproveForList from service
            listAccount_Next_ApproveDto = objAccount_Next_ApproveSer.GetAccount_Next_ApproveForList(Session("intUserLoginID"))

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlAccount_Next_Approve, listAccount_Next_ApproveDto, "name", "id", True)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListAccount_Next_Approve", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListPurchase_Next_Approve
    '	Discription	    : Load list Purchase_Next_Approve function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListPurchase_Next_Approve()
        Try
            ' object Purchase_Next_Approve service
            Dim objPurchase_Next_ApproveSer As New Service.ImpUserLoginService
            ' listPurchase_Next_ApproveDto for keep value from service
            Dim listPurchase_Next_ApproveDto As New List(Of Dto.UserLoginDto)
            ' call function GetPurchase_Next_ApproveForList from service
            listPurchase_Next_ApproveDto = objPurchase_Next_ApproveSer.GetPurchase_Next_ApproveForList(Session("intUserLoginID"))

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlPurchase_Next_Approve, listPurchase_Next_ApproveDto, "name", "id", True)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListPurchase_Next_Approve", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListOutsource_Next_Approve
    '	Discription	    : Load list Outsource_Next_Approve function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListOutsource_Next_Approve()
        Try
            ' object Outsource_Next_Approve service
            Dim objOutsource_Next_ApproveSer As New Service.ImpUserLoginService
            ' listOutsource_Next_ApproveDto for keep value from service
            Dim listOutsource_Next_ApproveDto As New List(Of Dto.UserLoginDto)
            ' call function GetOutsource_Next_ApproveForList from service
            listOutsource_Next_ApproveDto = objOutsource_Next_ApproveSer.GetOutsource_Next_ApproveForList(Session("intUserLoginID"))

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlOutsource_Next_Approve, listOutsource_Next_ApproveDto, "name", "id", True)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListOutsource_Next_Approve", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' UserLogin Dto object for keep return value from service
            Dim objUserLoginDto As New Dto.UserLoginDto
            Dim intUserLoginID As String = 0

            ' check UserLogin id then convert to integer
            If IsNothing(Request.QueryString("id")) = False Then
                intUserLoginID = CInt(objUtility.GetQueryString("id"))
                Session("intUserLoginID") = intUserLoginID
            End If

            ' call function GetStaffByID from service
            objUserLoginDto = objUserLoginSer.GetUserLoginByID(intUserLoginID)

            ' assign value to control
            With objUserLoginDto
                txtUserName.Text = .user_name
                txtPassword.Text = .password
                txtFirstName.Text = .first_name
                txtLastName.Text = .last_name
                ddlDepartment.SelectedValue = .department_id
                ddlAccount_Next_Approve.SelectedValue = .account_next_approve
                ddlPurchase_Next_Approve.SelectedValue = .purchase_next_approve
                'ddlOutsource_Next_Approve.SelectedValue = .outsource_next_approve
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearControl
    '	Discription	    : Clear data each control
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' clear control
            txtUserName.Text = String.Empty
            txtPassword.Text = String.Empty
            txtFirstName.Text = String.Empty
            txtLastName.Text = String.Empty
            ddlDepartment.SelectedValue = String.Empty
            ddlAccount_Next_Approve.SelectedValue = String.Empty
            ddlPurchase_Next_Approve.SelectedValue = String.Empty
            'ddlOutsource_Next_Approve.SelectedValue = String.Empty

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            If IsNothing(Session("PageNo")) Then
                Response.Redirect("KTAD01.aspx")
            Else
                Response.Redirect("KTAD01.aspx?PageNo=" & Session("PageNo"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	    : Event btnClear is click
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClear_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnClear.Click
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
    '	Discription	    : Event btnSave is click
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSave.Click
        Try
            ' call function set session dto
            SetValueToDto()

            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTAD02", strConfirmIns, objMessage.GetXMLMessage("KTAD_02_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTAD02", strConfirmUpd, objMessage.GetXMLMessage("KTAD_02_004"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Staff dto object
            Dim objUserLoginDto As New Dto.UserLoginDto

            ' assign value to dto object
            With objUserLoginDto

                .id = CInt(Session("intUserLoginID"))
                .user_name = txtUserName.Text.Trim
                .password = txtPassword.Text.Trim
                .first_name = txtFirstName.Text.Trim
                .last_name = txtLastName.Text.Trim
                .department_id = ddlDepartment.SelectedValue
                .account_next_approve = ddlAccount_Next_Approve.SelectedValue
                .purchase_next_approve = ddlPurchase_Next_Approve.SelectedValue
                .outsource_next_approve = ddlPurchase_Next_Approve.SelectedValue
            End With

            ' set dto object to session
            Session("objUserLoginDto") = objUserLoginDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertUserLogin
    '	Discription	    : Insert UserLogin
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertUserLogin()
        Try
            ' call function set value to control
            SetValueToControl()

            'check duplicate
            If objUserLoginSer.CheckDupUserLogin(Session("intUserLoginID"), txtUserName.Text, 1, strMsg) = False And strMsg = "" Then
                ' call function InsertUserLogin from service and alert message
                If objUserLoginSer.InsertUserLogin(Session("objUserLoginDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTAD_02_002"), Nothing, "KTAD01.aspx?New=Insert")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If

            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAD_02_007"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertUserLogin", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateUserLogin
    '	Discription	    : Update UserLogin
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateUserLogin()
        Try
            ' call function set value to control
            SetValueToControl()

            'check duplicate
            If objUserLoginSer.CheckDupUserLogin(Session("intUserLoginID"), txtUserName.Text, strMsg) = False And strMsg = "" Then
                ' call function UpdateUserLogin from service and alert message
                If objUserLoginSer.UpdateUserLogin(Session("objUserLoginDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTAD_02_005"), Nothing, "KTAD01.aspx?New=Update")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If

            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAD_02_007"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateUserLogin", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Staff dto object
            Dim objUserLoginDto As New Dto.UserLoginDto
            ' set value to dto object from session
            If IsNothing(Session("objUserLoginDto")) = False Then
                objUserLoginDto = Session("objUserLoginDto")

                ' set value to control
                With objUserLoginDto
                    Session("intUserLoginID") = .id
                    txtUserName.Text = .user_name
                    txtPassword.Text = .password
                    txtFirstName.Text = .first_name
                    txtLastName.Text = .last_name
                    ddlDepartment.SelectedValue = .department_id
                    ddlAccount_Next_Approve.SelectedValue = .account_next_approve
                    ddlPurchase_Next_Approve.SelectedValue = .purchase_next_approve
                    'ddlOutsource_Next_Approve.SelectedValue = .outsource_next_approve
                End With
            End If

            

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Vat menu
            objAction = objPermission.CheckPermission(37)
            ' set permission Create
            btnSave.Enabled = objAction.actUpdate


            ' set action permission to session
            Session("actUpdate") = objAction.actUpdate
            Session("actDelete") = objAction.actDelete

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
End Class
