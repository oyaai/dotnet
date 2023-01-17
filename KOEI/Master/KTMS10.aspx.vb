#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS10
'	Class Discription	: Webpage for maintenance WT master
'	Create User 		: Nisa S.
'	Create Date		    : 01-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Dto
Imports Extensions
Imports Service
Imports Exceptions
#End Region

Partial Class Master_KTMS10
    Inherits System.Web.UI.Page

    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objWTService As New ImpWTService
    Private strMsg As String = String.Empty
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission


#Region "Even"

    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	    : Event btnSave is click
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            ' call function set session dto
            SetValueToDto()

            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTMS10", strConfirmIns, objMessage.GetXMLMessage("KTMS_10_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTMS10", strConfirmUpd, objMessage.GetXMLMessage("KTMS_10_010"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS10 : W/T Master")
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
    '	Create Date	    : 01-07-2013
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
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            If IsNothing(Session("PageNo")) Then
                Response.Redirect("KTMS09.aspx")
            Else
                Response.Redirect("KTMS09.aspx?PageNo=" & Session("PageNo"))
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
    '	Create Date	    : 01-07-2013
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
            ElseIf objWTService.IsUsedInPO(CInt(txtID.Text.Trim)) = True Then
                txtType.Text = String.Empty
            Else
                ClearControl()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnClear_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region 'Even


#Region "Function"
    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Item dto object
            Dim objWTDto As New Dto.WTDto

            ' assign value to dto object
            With objWTDto
                If String.IsNullOrEmpty(txtID.Text.Trim) Then
                    .ID = 0
                Else
                    .ID = txtID.Text.Trim
                End If

                .Percent = txtWT.Text.Trim
                .Type = txtType.Text.Trim
            End With

            ' set dto object to session
            Session("objWTDto") = objWTDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try

            ' check insert WT
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                InsertWT()
                Exit Sub
            End If

            ' check update WT
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateWT()
                Exit Sub
            End If

            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                Session("Mode") = "Add"
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                LoadInitialUpdate()
            End If

            ' call function check permission
            CheckPermission()

            txtWTRangeValidator.ErrorMessage = objMessage.GetXMLMessage("KTMS_10_009")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
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
            objAction = objPermission.CheckPermission(29)
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

    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' WT Dto object for keep return value from service
            Dim objWTDto As New Dto.WTDto
            Dim intWTID As Integer = 0

            ' check WT id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intWTID = CInt(objUtility.GetQueryString("id"))
            End If

            ' call function GetWTByID from service
            objWTDto = objWTService.GetWTByID(intWTID)

            ' assign value to control
            With objWTDto
                txtID.Text = .ID
                txtWT.Text = .Percent
                txtType.Text = .Type
            End With

            'check accounting or po_detail
            If objWTService.IsUsedInPO(CInt(txtID.Text.Trim)) = True Then
                txtWT.Enabled = False
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertWT
    '	Discription	    : Insert WT
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertWT()
        Try

            SetValueToControl()

            'check duplicate
            If objWTService.CheckDupWT(txtID.Text, txtWT.Text, 1, strMsg) = False And strMsg = "" Then
                ' call function InsertWT from service and alert message
                If objWTService.InsertWT(Session("objWTDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_10_002"), Nothing, "KTMS09.aspx?New=Insert")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_10_004"))
            End If


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertWT", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateWT
    '	Discription	    : Update WT
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateWT()
        Try
            ' call function set value to control
            SetValueToControl()

            'check duplicate
            If objWTService.CheckDupWT(txtID.Text, txtWT.Text, strMsg) = False And strMsg = "" Then
                ' call function UpdateWT from service and alert message
                If objWTService.UpdateWT(Session("objWTDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_10_005"), Nothing, "KTMS09.aspx?New=Update")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_10_007"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateWT", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' WT dto object
            Dim objWTDto As New Dto.WTDto
            ' set value to dto object from session
            objWTDto = Session("objWTDto")

            ' set value to control
            With objWTDto
                If .ID = 0 Then
                    txtID.Text = String.Empty
                Else
                    txtID.Text = .ID
                End If

                txtWT.Text = .Percent
                txtType.Text = .Type
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearControl
    '	Discription	    : Clear data each control
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' clear control
            txtWT.Text = String.Empty
            txtType.Text = String.Empty
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region 'Function



End Class
