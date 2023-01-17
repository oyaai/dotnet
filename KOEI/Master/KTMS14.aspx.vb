#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS14
'	Class Discription	: Webpage for maintenance Payment Term master
'	Create User 		: Nisa S.
'	Create Date		    : 20-06-2013
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
Imports Common.Utilities
Imports log4net.Repository.Hierarchy
Imports Exceptions
Imports Extensions
Imports Service

#End Region


Partial Class Master_KTMS14
    Inherits System.Web.UI.Page

    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objPaymentTermService As New ImpPaymentTermService
    Private strMsg As String = String.Empty
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission

#Region "Even"

    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	    : Event btnSave is click
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            ' call function set session dto
            SetValueToDto()

            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTMS14", strConfirmIns, objMessage.GetXMLMessage("KTMS_14_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTMS14", strConfirmUpd, objMessage.GetXMLMessage("KTMS_14_008"))
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
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS14 : Payment Term Master")
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
    '	Create Date	    : 19-06-2013
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
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            If IsNothing(Session("PageNo")) Then
                Response.Redirect("KTMS13.aspx")
            Else
                Response.Redirect("KTMS13.aspx?PageNo=" & Session("PageNo"))
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
    '	Create Date	    : 20-06-2013
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
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Item dto object
            Dim objPaymentTermDto As New Dto.IPayment_TermDto

            ' assign value to dto object
            With objPaymentTermDto
                If String.IsNullOrEmpty(txtID.Text.Trim) Then
                    .id = 0
                Else
                    .id = txtID.Text.Trim
                End If

                .term_day = txtPayment.Text.Trim
            End With

            ' set dto object to session
            Session("objPaymentTermDto") = objPaymentTermDto

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
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try

            ' check insert item
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                InsertPaymentTerm()
                Exit Sub
            End If

            ' check update item
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdatePaymentTerm()
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
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' Item Dto object for keep return value from service
            Dim objPaymentTermDto As New Dto.IPayment_TermDto
            Dim intPaymentTermID As Integer = 0

            ' check item id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intPaymentTermID = CInt(objUtility.GetQueryString("id"))
            End If

            ' call function GetItemByID from service
            objPaymentTermDto = objPaymentTermService.GetPaymentTermByID(intPaymentTermID)

            ' assign value to control
            With objPaymentTermDto
                txtID.Text = .id
                txtPayment.Text = .term_day
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertPaymentTerm
    '	Discription	    : Insert PaymentTerm
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertPaymentTerm()
        Try

            SetValueToControl()

            'check duplicate
            If objPaymentTermService.CheckDupInsert(txtPayment.Text, 1, strMsg) = False And strMsg = "" Then
                ' call function InsertPaymentTerm from service and alert message
                If objPaymentTermService.InsertPaymentTerm(Session("objPaymentTermDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_14_002"), Nothing, "KTMS13.aspx?New=Insert")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_14_004"))
            End If


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertPaymentTerm", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdatePaymentTerm
    '	Discription	    : Update PaymentTerm
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdatePaymentTerm()
        Try
            ' call function set value to control
            SetValueToControl()

            'check duplicate
            If objPaymentTermService.CheckDupUpdate(txtID.Text, txtPayment.Text, strMsg) = False And strMsg = "" Then
                ' call function UpdateItem from service and alert message
                If objPaymentTermService.UpdatePaymentTerm(Session("objPaymentTermDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_14_005"), Nothing, "KTMS13.aspx?New=Update")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_14_007"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdatePaymentTerm", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Item dto object
            Dim objPaymentTermDto As New Dto.IPayment_TermDto
            ' set value to dto object from session
            objPaymentTermDto = Session("objPaymentTermDto")

            ' set value to control
            With objPaymentTermDto
                If .id = 0 Then
                    txtID.Text = String.Empty
                Else
                    txtID.Text = .id
                End If

                txtPayment.Text = .term_day
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
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' clear control
            txtPayment.Text = String.Empty
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
            ' check permission of Payment Term menu
            objAction = objPermission.CheckPermission(31)
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
