#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS28
'	Class Discription	: Webpage for maintenance Staff master
'	Create User 		: Nisa S.
'	Create Date		    : 04-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class Master_KTMS28
    Inherits System.Web.UI.Page

    Private objUtility As New Common.Utilities.Utility
    Private objLog As New Common.Logs.Log
    Private objStaffSer As New Service.ImpStaffService
    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private strMsg As String = String.Empty
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS28 : Staff Master")
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
    '	Create Date	    : 04-07-2013
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
    '	Function name	: btnBack_Click
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            If IsNothing(Session("PageNo")) Then
                Response.Redirect("KTMS27.aspx")
            Else
                Response.Redirect("KTMS27.aspx?PageNo=" & Session("PageNo"))
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
    '	Create Date	    : 04-07-2013
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
    '	Create Date	    : 04-07-2013
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
                objMessage.ConfirmMessage("KTMS28", strConfirmIns, objMessage.GetXMLMessage("KTMS_28_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTMS28", strConfirmUpd, objMessage.GetXMLMessage("KTMS_28_008"))
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
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            ' call function set WorkCategory dropdownlist
            LoadListWorkCategory()

            ' check insert staff
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                InsertStaff()
                Exit Sub
            End If

            ' check update staff
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateStaff()
                Exit Sub
            End If

            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                Session("Mode") = "Add"
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                LoadInitialUpdate()
            End If

            CheckPermission()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListWorkCategory
    '	Discription	    : Load list WorkCategory function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListWorkCategory()
        Try
            ' object WorkCategory service
            Dim objWorkCategorySer As New Service.ImpStaffService
            ' listWorkCategoryDto for keep value from service
            Dim listWorkCategoryDto As New List(Of Dto.StaffDto)
            ' call function GetWorkCategoryForList from service
            listWorkCategoryDto = objWorkCategorySer.GetWorkCategoryForList

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlSection, listWorkCategoryDto, "name", "id", True)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListWorkCategory", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' Staff Dto object for keep return value from service
            Dim objStaffDto As New Dto.StaffDto
            Dim intStaffID As Integer = 0

            ' check Staff id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intStaffID = CInt(objUtility.GetQueryString("id"))
                Session("intStaffID") = intStaffID
            End If

            ' call function GetStaffByID from service
            objStaffDto = objStaffSer.GetStaffByID(intStaffID)

            ' assign value to control
            With objStaffDto
                txtFirstName.Text = .first_name
                txtLastName.Text = .last_name
                ddlSection.SelectedValue = .work_category_id
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
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' clear control
            txtFirstName.Text = String.Empty
            txtLastName.Text = String.Empty
            ddlSection.SelectedValue = String.Empty
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Staff dto object
            Dim objStaffDto As New Dto.StaffDto

            ' assign value to dto object
            With objStaffDto
               
                .id = CInt(Session("intStaffID"))
                .first_name = txtFirstName.Text.Trim
                .last_name = txtLastName.Text.Trim
                .work_category_id = CInt(ddlSection.SelectedValue)
            End With

            ' set dto object to session
            Session("objStaffDto") = objStaffDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Staff dto object
            Dim objStaffDto As New Dto.StaffDto
            ' set value to dto object from session
            objStaffDto = Session("objStaffDto")

            ' set value to control
            With objStaffDto
                Session("intStaffID") = .id
                txtFirstName.Text = .first_name
                txtLastName.Text = .last_name
                ddlSection.SelectedValue = .work_category_id
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertStaff
    '	Discription	    : Insert Staff
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertStaff()
        Try
            ' call function set value to control
            SetValueToControl()

            'check duplicate
            If objStaffSer.CheckDupInsert(txtFirstName.Text, txtLastName.Text, 1, strMsg) = False And strMsg = "" Then
                ' call function InsertStaff from service and alert message
                If objStaffSer.InsertStaff(Session("objStaffDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_28_002"), Nothing, "KTMS27.aspx?New=Insert")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If

            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_28_004"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertStaff", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateStaff
    '	Discription	    : Update Staff
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateStaff()
        Try
            ' call function set value to control
            SetValueToControl()

            'check duplicate
            If objStaffSer.CheckDupUpdate(Session("intStaffID"), txtFirstName.Text, txtLastName.Text, strMsg) = False And strMsg = "" Then
                ' call function UpdateStaff from service and alert message
                If objStaffSer.UpdateStaff(Session("objStaffDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_28_005"), Nothing, "KTMS27.aspx?New=Update")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If

            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_28_007"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateStaff", ex.Message.ToString, Session("UserName"))
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
            ' check permission of Staff menu
            objAction = objPermission.CheckPermission(40)
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
#End Region

End Class
