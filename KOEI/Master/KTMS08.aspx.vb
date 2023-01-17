#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS08
'	Class Discription	: Webpage for maintenance vat master
'	Create User 		: Nisa S.
'	Create Date		    : 26-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports Dto
Imports Service
Imports Exceptions

Partial Class Master_KTMS08
    Inherits System.Web.UI.Page

    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objVatService As New ImpVatService
    Private strMsg As String = String.Empty
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission





#Region "Properties"

    'Protected Overrides ReadOnly Property ClassName() As String
    '    Get
    '        Return Convert.ToString(GetType(Master_KTMS08))
    '    End Get
    'End Property

    'Protected Overrides ReadOnly Property PagedDataSource() As PagedDataSource
    '    Get
    '        Throw New NotImplementedException()
    '    End Get
    'End Property

#End Region

#Region "Even"
    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	    : Event btnSave is click
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            ' call function set session dto
            SetValueToDto()

            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTMS08", strConfirmIns, objMessage.GetXMLMessage("KTMS_08_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTMS08", strConfirmUpd, objMessage.GetXMLMessage("KTMS_08_010"))
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
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS08 : Vat Master")
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
    '	Create Date	    : 26-06-2013
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
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            If IsNothing(Session("PageNo")) Then
                Response.Redirect("KTMS07.aspx")
            Else
                Response.Redirect("KTMS07.aspx?PageNo=" & Session("PageNo"))
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
    '	Create Date	    : 26-06-2013
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
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Item dto object
            Dim objVatDto As New Dto.VatDto

            ' assign value to dto object
            With objVatDto
                If String.IsNullOrEmpty(txtID.Text.Trim) Then
                    .ID = 0
                Else
                    .ID = txtID.Text.Trim
                End If

                .Percent = txtVat.Text.Trim
            End With

            ' set dto object to session
            Session("objVatDto") = objVatDto

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
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try

            ' check insert Vat
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                InsertVat()
                Exit Sub
            End If

            ' check update Vat
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateVat()
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


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertVat
    '	Discription	    : Insert Vat
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertVat()
        Try

            SetValueToControl()

            'check duplicate
            If objVatService.CheckDupVat(txtID.Text, txtVat.Text, 1, strMsg) = False And strMsg = "" Then
                ' call function InsertVat from service and alert message
                If objVatService.InsertVat(Session("objVatDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_08_002"), Nothing, "KTMS07.aspx?New=Insert")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_08_004"))
            End If


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertVat", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateVat
    '	Discription	    : Update Vat
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateVat()
        Try
            ' call function set value to control
            SetValueToControl()

            'check duplicate
            If objVatService.CheckDupVat(txtID.Text, txtVat.Text, strMsg) = False And strMsg = "" Then
                ' call function UpdateItem from service and alert message
                If objVatService.UpdateVat(Session("objVatDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_08_005"), Nothing, "KTMS07.aspx?New=Update")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_08_007"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateVat", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' Vat Dto object for keep return value from service
            Dim objVatDto As New Dto.VatDto
            Dim intVatID As Integer = 0

            ' check Vat id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intVatID = CInt(objUtility.GetQueryString("id"))
            End If

            ' call function GetVatByID from service
            objVatDto = objVatService.GetVatByID(intVatID)

            ' assign value to control
            With objVatDto
                txtID.Text = .ID
                txtVat.Text = .Percent
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Item dto object
            Dim objVatDto As New Dto.VatDto
            ' set value to dto object from session
            objVatDto = Session("objVatDto")

            ' set value to control
            With objVatDto
                If .ID = 0 Then
                    txtID.Text = String.Empty
                Else
                    txtID.Text = .ID
                End If

                txtVat.Text = .Percent
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
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' clear control
            txtVat.Text = String.Empty
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
