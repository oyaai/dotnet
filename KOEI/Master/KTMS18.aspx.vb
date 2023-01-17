Imports Service

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS18
'	Class Discription	: Webpage for maintenance Currency master
'	Create User 		: Nisa S.
'	Create Date		    : 28-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region


Partial Class Master_KTMS18
    Inherits System.Web.UI.Page


    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objCurrencyService As New ImpCurrencyService
    Private strMsg As String = String.Empty
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission


#Region "Even"

    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	    : Event btnSave is click
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 28-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            ' call function set session dto
            SetValueToDto()

            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTMS18", strConfirmIns, objMessage.GetXMLMessage("KTMS_18_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTMS18", strConfirmUpd, objMessage.GetXMLMessage("KTMS_18_008"))
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
    '	Create Date	    : 28-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS18 : Currency Master")
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
    '	Create Date	    : 28-06-2013
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
    '	Create Date	    : 28-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            If IsNothing(Session("PageNo")) Then
                Response.Redirect("KTMS17.aspx")
            Else
                Response.Redirect("KTMS17.aspx?PageNo=" & Session("PageNo"))
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
    '	Create Date	    : 28-06-2013
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
#End Region 'Even


#Region "Function"
    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 28-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Currency dto object
            Dim objCurrencyDto As New Dto.CurrencyDto

            ' assign value to dto object
            With objCurrencyDto
                If String.IsNullOrEmpty(txtID.Text.Trim) Then
                    .id = 0
                Else
                    .id = txtID.Text.Trim
                End If

                .name = txtCurrency.Text.Trim
            End With

            ' set dto object to session
            Session("objCurrencyDto") = objCurrencyDto

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
    '	Create Date	    : 28-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try

            ' check insert Currency
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                InsertCurrency()
                Exit Sub
            End If

            ' check update Currency
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateCurrency()
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
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 28-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' Currency Dto object for keep return value from service
            Dim objCurrencyDto As New Dto.CurrencyDto
            Dim intCurrencyID As Integer = 0

            ' check Currency id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intCurrencyID = CInt(objUtility.GetQueryString("id"))
            End If

            ' call function GetCurrencyByID from Currencyservice
            objCurrencyDto = objCurrencyService.GetCurrencyByID(intCurrencyID)

            ' assign value to control
            With objCurrencyDto
                txtID.Text = .id
                txtCurrency.Text = .name
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertCurrency
    '	Discription	    : Insert Currency
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 28-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertCurrency()
        Try

            SetValueToControl()

            'check duplicate
            If objCurrencyService.CheckDupCurrency(txtID.Text, txtCurrency.Text, 1, strMsg) = False And strMsg = "" Then
                ' call function InsertCurrency from service and alert message
                If objCurrencyService.InsertCurrency(Session("objCurrencyDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_18_002"), Nothing, "KTMS17.aspx?New=Insert")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_18_004"))
            End If


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertCurrency", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateCurrency
    '	Discription	    : Update Currency
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 28-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateCurrency()
        Try
            ' call function set value to control
            SetValueToControl()

            'check duplicate
            If objCurrencyService.CheckDupCurrency(txtID.Text, txtCurrency.Text, strMsg) = False And strMsg = "" Then
                ' call function UpdateCurrency from service and alert message
                If objCurrencyService.UpdateCurrency(Session("objCurrencyDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_18_005"), Nothing, "KTMS17.aspx?New=Update")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_18_007"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateCurrency", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 28-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Item dto object
            Dim objCurrencyDto As New Dto.CurrencyDto
            ' set value to dto object from session
            objCurrencyDto = Session("objCurrencyDto")

            ' set value to control
            With objCurrencyDto
                If .id = 0 Then
                    txtID.Text = String.Empty
                Else
                    txtID.Text = .id
                End If

                txtCurrency.Text = .name
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
    '	Create Date	    : 28-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' clear control
            txtCurrency.Text = String.Empty
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
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
            objAction = objPermission.CheckPermission(33)
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
#End Region 'Function

End Class
