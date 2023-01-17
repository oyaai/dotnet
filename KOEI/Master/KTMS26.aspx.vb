#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS26
'	Class Discription	: Webpage for maintenance item master
'	Create User 		: Wasan D.
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

Partial Class Master_KTMS26
    Inherits System.Web.UI.Page

    Private objUtility As New Common.Utilities.Utility
    Private objLog As New Common.Logs.Log
    Private objPaymentCondSer As New Service.ImpPaymentConditionService
    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private strMsg As String = String.Empty

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
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
            objLog.StartLog("KTMS26 : Payment Condition Master")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
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
    '	Create User	    : Wasan D.
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
                Response.Redirect("KTMS25.aspx")
            Else
                Response.Redirect("KTMS25.aspx?PageNo=" & Session("PageNo"))
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
    '	Create User	    : Wasan D.
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
    '	Create User	    : Wasan D.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSave.Click
        Try
            If Not (CheckTotal100(txtFirst.Text.Trim, txtSecond.Text.Trim, txtThird.Text.Trim)) Then
                ' Case Sum percent not 100
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_26_010"))
            Else
                ' call function set session dto
                SetValueToDto()

                ' check mode then show confirm message box
                If Session("Mode") = "Add" Then
                    objMessage.ConfirmMessage("KTMS26", strConfirmIns, objMessage.GetXMLMessage("KTMS_26_001"))
                ElseIf Session("Mode") = "Edit" Then
                    objMessage.ConfirmMessage("KTMS26", strConfirmUpd, objMessage.GetXMLMessage("KTMS_26_008"))
                End If
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: CheckTotal100
    '	Discription	    : Check value total will be 100
    '	Return Value	: boolean
    '	Create User	    : Wasan D.
    '	Create Date	    : 15-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckTotal100( _
            ByVal intFirst As Integer, _
            ByVal intSecond As Integer, _
            ByVal intThird As Integer) As Boolean
        ' Set default value
        CheckTotal100 = False
        Try
            Dim intPlus As Integer
            intPlus = intFirst + intSecond + intThird
            If intPlus = 100 Then
                CheckTotal100 = True
            Else
                CheckTotal100 = False
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckTotal100", ex.Message.ToString, Session("UserName"))
        End Try
    End Function
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            ' set validator Errormessage
            RangeValidator1st.ErrorMessage = " * " & objMessage.GetXMLMessage("KTMS_26_009")
            RangeValidator2nd.ErrorMessage = " * " & objMessage.GetXMLMessage("KTMS_26_009")
            RangeValidator3rd.ErrorMessage = " * " & objMessage.GetXMLMessage("KTMS_26_009")


            ' check insert item
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                InsertItem()
                Exit Sub
            End If

            ' check update item
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateItem()
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
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' Payment Condition Dto object for keep return value from service
            Dim objPaymentCondDto As New Dto.PaymentConditionDto
            Dim intPayID As Integer = 0

            ' check PayID then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("ID")) Then
                intPayID = CInt(objUtility.GetQueryString("ID"))
            End If

            ' call function GetPaymentCondByID from service
            objPaymentCondDto = objPaymentCondSer.GetPaymentCondByID(intPayID)

            ' assign value to control
            With objPaymentCondDto
                txtPayID.Text = .codition_id
                txtFirst.Text = .codition_1st
                txtSecond.Text = .codition_2nd
                txtThird.Text = .codition_3rd
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
    '	Create User	    : Wasan D.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' clear control
            txtFirst.Text = String.Empty
            txtSecond.Text = String.Empty
            txtThird.Text = String.Empty
            'ddlVendor.SelectedValue = String.Empty
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Item dto object
            Dim objPaymentCondDto As New Dto.PaymentConditionDto

            ' assign value to dto object
            With objPaymentCondDto
                If String.IsNullOrEmpty(txtPayID.Text.Trim) Then
                    .codition_id = 0
                Else
                    .codition_id = txtPayID.Text.Trim
                End If

                .codition_1st = txtFirst.Text.Trim
                .codition_2nd = txtSecond.Text.Trim
                .codition_3rd = txtThird.Text.Trim
            End With
            ' set dto object to session
            Session("objPaymentCondDto") = objPaymentCondDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Payment Condition dto object
            Dim objPaymentCondDto As New Dto.PaymentConditionDto
            ' set value to dto object from session
            objPaymentCondDto = Session("objPaymentCondDto")

            ' set value to control
            With objPaymentCondDto
                If .codition_id = 0 Then
                    txtPayID.Text = String.Empty
                Else
                    txtPayID.Text = .codition_id
                End If

                txtFirst.Text = .codition_1st
                txtSecond.Text = .codition_2nd
                txtThird.Text = .codition_3rd
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertItem
    '	Discription	    : Insert Item
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertItem()
        Try
            ' call function set value to control
            SetValueToControl()

            ' call function InsertItem from service and alert message
            If objPaymentCondSer.InsertPaymentCond(Session("objPaymentCondDto"), strMsg) Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_26_002"), Nothing, "KTMS25.aspx?New=Insert")
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertPayment_Condition", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateItem
    '	Discription	    : Update Item
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateItem()
        Try
            ' call function set value to control
            SetValueToControl()

            ' call function UpdateItem from service and alert message
            If objPaymentCondSer.UpdatePaymentCond(Session("objPaymentCondDto"), strMsg) Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_26_005"), Nothing, "KTMS25.aspx?New=Update")
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdatePaymentCond", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

End Class
