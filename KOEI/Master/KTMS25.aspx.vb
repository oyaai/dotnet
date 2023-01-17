#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS25
'	Class Discription	: Webpage for maintenance Payment Condition master
'	Create User 		: Wasan D.
'	Create Date		    : 03-07-2013
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
Imports System.Data
Imports System.Web.Configuration
#End Region

Partial Class Master_KTMS25
    Inherits System.Web.UI.Page

#Region "Fields"
    Private _isNew As Boolean
    Private objLog As New Common.Logs.Log
    Private objPaymentConSer As New Service.ImpPaymentConditionService
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const strResult As String = "Result"
#End Region

#Region "Events"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '/**************************************************************
        '	Function name	: Page_Init
        '	Discription	    : Event page initial
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Try
            ' write start log
            objLog.StartLog("KTMS25 : Payment Condition Master")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '/**************************************************************
        '	Function name	: Page_Load
        '	Discription	    : Event page load
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Try
            ' set validator Errormessage
            RangeValidator1st.ErrorMessage = " * " & objMessage.GetXMLMessage("KTMS_26_009")
            RangeValidator2nd.ErrorMessage = " * " & objMessage.GetXMLMessage("KTMS_26_009")
            RangeValidator3rd.ErrorMessage = " * " & objMessage.GetXMLMessage("KTMS_26_009")

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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        '/**************************************************************
        '	Function name	: btnSearch_Click
        '	Discription	    : Event btnSearch is click
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Try
            ' call function search data
            SearchData(txtFirst.Text, txtSecond.Text, txtThird.Text)
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            Session("txtFirst") = txtFirst.Text.Trim
            Session("txtSecond") = txtSecond.Text.Trim
            Session("txtThird") = txtThird.Text.Trim
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    Protected Sub btnAdd_Click( _
           ByVal sender As Object, _
           ByVal e As System.EventArgs _
       ) Handles btnAdd.Click
        '/**************************************************************
        '	Function name	: btnAdd_Click
        '	Discription	    : Event btnAdd is clicked
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Try
            ' Set textbox value to Session
            SetTXTSession()
            ' redirect to KTMS26 with Add mode
            Response.Redirect("KTMS26.aspx?Mode=Add")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#Region "rptResult"

    Protected Sub rptPayCondition_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles rptPayCondition.DataBinding
        '/**************************************************************
        '	Function name	: rptInquery_DataBinding
        '	Discription	    : Event repeater binding data
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Try
            ' clear hashtable data
            hashPayID.Clear()
            hashPayFirst.Clear()
            hashPaySecond.Clear()
            hashPayThird.Clear()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptPayCondition_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    Protected Sub rptPayCondition_Command(ByVal source As Object, ByVal e As  _
                                              System.Web.UI.WebControls.RepeaterCommandEventArgs _
                                              ) Handles rptPayCondition.ItemCommand
        '/**************************************************************
        '	Function name	: rptPayCondition_Command
        '	Discription	    : Event repeater Payment Condition command
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Try
            ' variable for keep data from hashtable
            Dim intPayID As Integer = CInt(hashPayID(e.Item.ItemIndex).ToString())
            Dim intPayFirst As Integer = CInt(hashPayFirst(e.Item.ItemIndex).ToString())
            Dim intPaySecond As Integer = CInt(hashPaySecond(e.Item.ItemIndex).ToString())
            Dim intPayThird As Integer = CInt(hashPayThird(e.Item.ItemIndex).ToString())

            ' set Payment_Condition to session
            Session("intPayID") = intPayID
            Session("intPayFirst") = intPayFirst
            Session("intPaySecond") = intPaySecond
            Session("intPayThird") = intPayThird

            Select Case e.CommandName
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTMS25", strResult, objMessage.GetXMLMessage("KTMS_25_001"))
                Case "Edit"
                    ' redirect to KTMS25 (Edit)
                    Response.Redirect("KTMS25.aspx?" & strResult & "=Edit&PageNo=" & Session("PageNo"))
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptPayCondition_Command", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    Protected Sub rptPayCondition_DataBound( _
          ByVal sender As Object, _
          ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
      ) Handles rptPayCondition.ItemDataBound
        '/**************************************************************
        '	Function name	: rptPayCondition_DataBound
        '	Discription	    : Event repeater bound data
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Try
            ' object link button
            Dim btnDel As LinkButton
            Dim btnEdit As LinkButton

            ' find linkbutton and assign to variable
            btnDel = CType(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = CType(e.Item.FindControl("btnEdit"), LinkButton)

            ' set permission on button
            If Not Session("actUpdate") Then
                ' Set textbox value to session
                btnEdit.CssClass = "icon_edit2 icon_center15"
                btnEdit.Enabled = False
            End If

            If Not Session("actDelete") Then
                btnDel.CssClass = "icon_del2 icon_center15"
                btnDel.Enabled = False
            End If

            ' Set Payment Condition to hashtable
            hashPayID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "PayID"))
            hashPayFirst.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "First"))
            hashPaySecond.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Second"))
            hashPayThird.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Third"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptPayCondition_DataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    ' Stores the PayID keys in ViewState
    ReadOnly Property hashPayID() As Hashtable
        Get
            If IsNothing(ViewState("hashPayID")) Then
                ViewState("hashPayID") = New Hashtable()
            End If
            Return CType(ViewState("hashPayID"), Hashtable)
        End Get
    End Property

    ' Stores the Payment_Cond1 keys in ViewState
    ReadOnly Property hashPayFirst() As Hashtable
        Get
            If IsNothing(ViewState("hashPayFirst")) Then
                ViewState("hashPayFirst") = New Hashtable()
            End If
            Return CType(ViewState("hashPayFirst"), Hashtable)
        End Get
    End Property

    ' Stores the Payment_Cond2 keys in ViewState
    ReadOnly Property hashPaySecond() As Hashtable
        Get
            If IsNothing(ViewState("hashPaySecond")) Then
                ViewState("hashPaySecond") = New Hashtable()
            End If
            Return CType(ViewState("hashPaySecond"), Hashtable)
        End Get
    End Property

    ' Stores the Payment_Cond3 keys in ViewState
    ReadOnly Property hashPayThird() As Hashtable
        Get
            If IsNothing(ViewState("hashPayThird")) Then
                ViewState("hashPayThird") = New Hashtable()
            End If
            Return CType(ViewState("hashPayThird"), Hashtable)
        End Get
    End Property
#End Region
#End Region

#Region "Function"

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            Dim strPageEnter As String = objUtility.GetQueryString("New")
            Dim strResultValue As String = objUtility.GetQueryString(strResult)

            ' check case new enter
            If strPageEnter = "True" Then
                ' call function clear session
                ClearSession()
            ElseIf strPageEnter = "Update" Or strPageEnter = "Insert" Then
                ' call function search new data
                SearchData(Session("txtFirst"), _
                           Session("txtSecond"), _
                           Session("txtThird"))
                ' call function display page
                DisplayPage(Session("PageNo"), True)
            ElseIf strResultValue = "True" Or strResultValue = "Edit" Then
                ' call function display page
                DisplayPage(Session("PageNo"), True)
            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"), True)
            End If

            ' set value to txtPayment_Condition from session
            txtFirst.Text = Session("txtFirst")
            txtSecond.Text = Session("txtSecond")
            txtthird.Text = Session("txtThird")
            ' call function check permission
            CheckPermission()

            ' check delete & Edit item
            If strResultValue = "True" Then
                DeletePaymentCond()
            ElseIf strResultValue = "Edit" Then
                UpdateCheckUseInPO()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeletePaymentCond
    '	Discription	    : Delete Payment Condition data
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateCheckUseInPO()
        Try
            ' check state of delete item
            Dim boolInuse As Integer = objPaymentConSer.IsUsedInPO(Session("intPayID"))
            ' check flag in_used
            If boolInuse > 0 Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_26_011"))
            ElseIf boolInuse < 0 Then
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_26_006"))
            Else ' Case is not in use
                Response.Redirect("KTMS26.aspx?Mode=Edit&ID=" & Session("intPayID"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateCheckUseInPO", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeletePaymentCond
    '	Discription	    : Delete Payment Condition data
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeletePaymentCond()
        Try
            ' check state of delete item
            Dim boolInuse As Integer = objPaymentConSer.IsUsedInPO(Session("intPayID"))
            ' check flag in_used
            If boolInuse > 0 Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_25_002"))
            ElseIf boolInuse < 0 Then
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_25_004"))
            Else ' Case is not in use
                If objPaymentConSer.DeletePaymentCond(Session("intPayID")) Then
                    ' case delete success show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_25_003"))
                    ' call function search new data
                    SearchData(Session("txtFirst"), _
                               Session("txtSecond"), _
                               Session("txtThird"))
                    ' call function display page
                    DisplayPage(Session("PageNo"), True)
                Else
                    ' case delete not success show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_25_004"))
                End If
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeletePaymentCondition", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Payment Condition menu
            objAction = objPermission.CheckPermission(Enums.MenuId.PaymentCondition)
            ' set permission Create
            btnAdd.Enabled = objAction.actCreate
            btnSearch.Enabled = objAction.actList

            ' set action permission to session
            Session("actUpdate") = objAction.actUpdate
            Session("actDelete") = objAction.actDelete

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Payment Condition data
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData(ByVal strFirst As String, ByVal strSecond As String, ByVal strThird As String)
        Try
            ' table object keep value from Payment Condition service
            Dim dtInquiry As New DataTable

            ' call function GetPaymentCondList from ImpPaymentConditionService
            dtInquiry = objPaymentConSer.GetPaymentCondList(strFirst.ToString.Trim, _
                                                            strSecond.ToString.Trim, _
                                                            strThird.ToString.Trim)
            ' set table object to session
            Session("dtInquiry") = dtInquiry
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage( _
        ByVal intPageNo As Integer, _
        Optional ByVal boolNotAlertMsg As Boolean = False)
        Try
            Dim dtInquiry As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtInquiry = Session("dtInquiry")

            ' check record for display
            If Not IsNothing(dtInquiry) AndAlso dtInquiry.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtInquiry)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptPayCondition.DataSource = pagedData
                rptPayCondition.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtInquiry.Rows.Count)
                Session("PageNo") = intPageNo
            Else
                ' case not exist data
                ' show message box
                If Not (boolNotAlertMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
                End If

                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptPayCondition.DataSource = Nothing
                rptPayCondition.DataBind()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("txtFirst") = Nothing
            Session("txtSecond") = Nothing
            Session("txtThird") = Nothing
            Session("actUpdate") = Nothing
            Session("actDelete") = Nothing
            Session("PageNo") = Nothing
            Session("dtInquiry") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: SetTXTSession
    '	Discription	    : set Textbox session
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetTXTSession()
        Try
            ' set search text to session
            Session("txtFirst") = txtFirst.Text.Trim
            Session("txtSecond") = txtSecond.Text.Trim
            Session("txtThird") = txtThird.Text.Trim
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetTXTSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


#End Region
End Class
