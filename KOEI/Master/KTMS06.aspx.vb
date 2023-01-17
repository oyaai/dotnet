Imports Dto
Imports Exceptions
Imports Service
Imports System.Data


#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS06
'	Class Discription	: Webpage for maintenance Account title master
'	Create User 		: Nisa S.
'	Create Date		    : 24-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class Master_KTMS06
    Inherits System.Web.UI.Page

    Private objUtility As New Common.Utilities.Utility
    Private objLog As New Common.Logs.Log
    Private objIESer As New Service.ImpIEService
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
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS06 : Account title Master")
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
    '	Create Date	    : 21-06-2013
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
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            If IsNothing(Session("PageNo")) Then
                Response.Redirect("KTMS05.aspx")
            Else
                Response.Redirect("KTMS05.aspx?PageNo=" & Session("PageNo"))
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
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClear_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnClear.Click
        Try
            If txtID.Text = "" Then
                ClearControl()
            ElseIf objIESer.IsUsedInPO(CInt(txtID.Text.Trim)) = True Then
                txtIEName.Text = String.Empty
            Else
                ClearControl()
            End If

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
    '	Create Date	    : 21-06-2013
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
                objMessage.ConfirmMessage("KTMS06", strConfirmIns, objMessage.GetXMLMessage("KTMS_06_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTMS06", strConfirmUpd, objMessage.GetXMLMessage("KTMS_06_008"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region 'Event


#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            ' call function set Category dropdownlist
            LoadListCategory()

            ' check insert IE
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                InsertIE()
                Exit Sub
            End If

            ' check update IE
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateIE()
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
    '	Function name	: LoadListCategory
    '	Discription	    : Load list Category function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListCategory()
        Try
            ' object Category service
            Dim objCategorySer As New Service.ImpIECategoryService
            ' listCategoryDto for keep value from service
            Dim listCategoryDto As New List(Of Dto.IECategoryDto)
            ' call function GetAll from service
            listCategoryDto = objCategorySer.GetAll

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlSearchIECategory, listCategoryDto, "name", "id", True)



        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListCategory", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Nisa
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' IE Dto object for keep return value from service
            Dim objIEDto As New Dto.IEDto
            Dim intIEID As Integer = 0


            ' check IE id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intIEID = CInt(objUtility.GetQueryString("id"))
            End If

            ' call function GetIEByID from service
            objIEDto = objIESer.GetIEByID(intIEID)

            ' assign value to control
            With objIEDto
                txtID.Text = .ID
                txtIEName.Text = .Name
                txtIECode.Text = .Code
                ddlSearchIECategory.SelectedValue = .CategoryID
            End With

            'check accounting or po_detail
            If objIESer.IsUsedInPO(CInt(txtID.Text.Trim)) = True Then
                ddlSearchIECategory.Enabled = False
                txtIECode.Enabled = False
            End If

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
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' clear control
            ddlSearchIECategory.SelectedValue = String.Empty
            txtIECode.Text = String.Empty
            txtIEName.Text = String.Empty

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
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' IE dto object
            Dim objIEDto As New Dto.IEDto


            ' assign value to dto object
            With objIEDto
                If String.IsNullOrEmpty(txtID.Text.Trim) Then
                    .ID = 0
                Else
                    .ID = txtID.Text.Trim
                End If

                .CategoryID = ddlSearchIECategory.SelectedValue
                .Code = txtIECode.Text.Trim
                .Name = txtIEName.Text.Trim

            End With

            ' set dto object to session
            Session("objIEDto") = objIEDto

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
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' IE dto object
            Dim objIEDto As New Dto.IEDto
            ' set value to dto object from session
            objIEDto = Session("objIEDto")

            ' set value to control
            With objIEDto
                If .ID = 0 Then
                    txtID.Text = String.Empty
                Else
                    txtID.Text = .ID
                End If

                txtIECode.Text = .Code
                txtIEName.Text = .Name
                ddlSearchIECategory.SelectedValue = .CategoryID
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertIE
    '	Discription	    : Insert IE
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertIE()
        Try
            ' call function set value to control
            SetValueToControl()

            If objIESer.CheckDupIE(txtID.Text, txtIECode.Text, ddlSearchIECategory.SelectedValue, 1, strMsg) = False And strMsg = "" Then
                ' call function InsertIE from service and alert message
                If objIESer.InsertIE(Session("objIEDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_06_002"), Nothing, "KTMS05.aspx?New=Insert")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_06_004"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertIE", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateIE
    '	Discription	    : Update IE
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateIE()
        Try
            ' call function set value to control
            SetValueToControl()

            If objIESer.CheckDupIE(txtID.Text, txtIECode.Text, ddlSearchIECategory.SelectedValue, strMsg) = False And strMsg = "" Then
                ' call function UpdateIE from service and alert message
                If objIESer.UpdateIE(Session("objIEDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_06_005"), Nothing, "KTMS05.aspx?New=Edit")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If

            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_06_007"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateIE", ex.Message.ToString, Session("UserName"))
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
            objAction = objPermission.CheckPermission(28)
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
