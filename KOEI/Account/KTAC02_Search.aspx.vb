Imports System.Data
Imports CrystalDecisions
Imports System.Web.Services
Imports System.Globalization
Imports System.Web.UI

#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Account
'	Class Name		    : KTAC02_Search
'	Class Discription	: Webpage popup window for search income or payment
'	Create User 		: Wasan D.
'	Create Date		    : 27-08-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Namespace Account
    Partial Class KTAC02_Search
        Inherits System.Web.UI.Page

        Private objLog As New Common.Logs.Log
        Private objMessage As New Common.Utilities.Message
        Private objUtility As New Common.Utilities.Utility
        Private objPermission As New Common.UserPermissions.UserPermission
        Private objAction As New Common.UserPermissions.ActionPermission
        Private pagedData As New PagedDataSource

#Region "Event"
        '/**************************************************************
        '	Function name	: Page_Init
        '	Discription	    : Event page initial
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 28-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub Page_Init( _
            ByVal sender As Object, _
            ByVal e As System.EventArgs _
        ) Handles Me.Init
            Try
                ' write start log
                objLog.StartLog("KTAC02_Search : Accounting Income/Payment")
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
        '	Create Date	    : 28-08-2013
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
        '	Function name	: rptInquery_DataBinding
        '	Discription	    : Event repeater binding data
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 27-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub rptInquery_DataBinding( _
            ByVal sender As Object, _
            ByVal e As System.EventArgs _
        ) Handles rptInquery.DataBinding
            Try
                ' clear hashtable data
                hashIndex.Clear()
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("rptInquery_DataBinding", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: rptInquery_ItemCommand
        '	Discription	    : Event repeater item command
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 28-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub rptInquery_ItemCommand( _
            ByVal source As Object, _
            ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
        ) Handles rptInquery.ItemCommand
            Try
                ' variable for keep data from hashtable
                Dim intIndex As Integer = CInt(hashIndex(e.Item.ItemIndex).ToString())
                Dim sb As New StringBuilder
                With sb
                    .AppendLine("<script type = 'text/javascript'>")
                    .AppendLine("   window.opener.focus();")
                    .AppendLine("   window.opener.open('" & IIf(Session("IEType2") = 1, "KTAC03", "KTAC02") & ".aspx?Mode=Edit&accID=" & intIndex & "','_top');")
                    .AppendLine("   window.close();")
                    .AppendLine("</script>")
                End With
                ClientScript.RegisterStartupScript(Page.GetType(), "test", sb.ToString)
                ' set data to control for edit
                'UpdateInitial(intIndex)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("rptInquery_ItemCommand", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: rptInquery_ItemDataBound
        '	Discription	    : Event repeater item data bound
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 28-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub rptInquery_ItemDataBound( _
            ByVal sender As Object, _
            ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
        ) Handles rptInquery.ItemDataBound
            Try
                ' Set ItemID to hashtable
                hashIndex.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("rptInquery_ItemDataBound", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        ' Stores the Index keys in ViewState
        ReadOnly Property hashIndex() As Hashtable
            Get
                If IsNothing(ViewState("hashIndex")) Then
                    ViewState("hashIndex") = New Hashtable()
                End If
                Return CType(ViewState("hashIndex"), Hashtable)
            End Get
        End Property

        '/**************************************************************
        '	Function name	: btnSearch_Click
        '	Discription	    : btnSearch click
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 28-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            Try
                Dim objAccountSer As New Service.ImpAccountingService
                Session("dtSearchInquiry") = objAccountSer.SearchIncomePayment(Session("IEType2"), SetListOfSting())
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"))
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

#End Region

#Region "Function"

        '/**************************************************************
        '	Function name	: InitialPage
        '	Discription	    : Initial page function
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 27-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub InitialPage()
            Try
                SetValidateErrorMessage()
                ' check case new enter
                If objUtility.GetQueryString("New") = "True" Then
                    ' call function clear session
                    ClearSession()
                    Select Case objUtility.GetQueryString("Type2")
                        Case "Income" : Session("IEType2") = Enums.AccountRecordTypes.Income
                        Case "Payment" : Session("IEType2") = Enums.AccountRecordTypes.Payment
                        Case Else : Session("IEType2") = Nothing
                    End Select
                Else
                    ' Set value to control
                    SetDataToControl()
                End If
                LoadListAccountTitle(Session("IEType2"))
                lblHeader.Text = IIf(Session("IEType2") = 1, "SEARCH PAYMENT", "SEARCH INCOME")
                lblDate.Text = IIf(Session("IEType2") = 1, "Payment Date", "Receipt Date")
                lblDate2.Text = IIf(Session("IEType2") = 1, "Payment Date", "Receipt Date")
                ' call function check permission
                CheckPermission(Session("IEType2"))
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"), True)

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: ClearSession
        '	Discription	    : Clear session
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 28-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub ClearSession()
            Try
                ' clase all session used in this page
                Session("listControlValue") = Nothing
                Session("dtSearchInquiry") = Nothing
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: CheckPermission
        '	Discription	    : Check permission
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 28-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub CheckPermission(ByVal intMenuID As Integer)
            Try
                ' check permission of Item menu
                Select Case intMenuID
                    Case 1 : objAction = objPermission.CheckPermission(Enums.MenuId.Payment)
                    Case 2 : objAction = objPermission.CheckPermission(Enums.MenuId.Income_KTAC)
                    Case Else : objAction = objPermission.CheckPermission(0)
                End Select
                ' set permission Create
                btnSearch.Enabled = objAction.actList
                btnCancel.Enabled = objAction.actList

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: SetListOfSting
        '	Discription	    : Set data to list of string
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 28-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetListOfSting() As List(Of String)
            SetListOfSting = New List(Of String)
            Try
                ' set data from control to dto object
                With SetListOfSting
                    .Add(txtJobOrderStart.Text.Trim)
                    .Add(txtJobOrderEnd.Text.Trim)
                    .Add(rbtAccountType.SelectedValue)
                    .Add(txtVendorName.Text.Trim)
                    .Add(ddlItemExpense.SelectedValue)
                    .Add(txtAccountName.Text.Trim)
                    .Add(txtAccountNo.Text.Trim)
                    If String.IsNullOrEmpty(txtReceiptDateStart.Text.Trim) Then
                        .Add(txtReceiptDateStart.Text.Trim)
                    Else
                        .Add(objUtility.String2Date(txtReceiptDateStart.Text.Trim, "dd/MM/yyyy").ToString("yyyyMMdd"))
                    End If
                    If String.IsNullOrEmpty(txtReceiptDateEnd.Text.Trim) Then
                        .Add(txtReceiptDateEnd.Text.Trim)
                    Else
                        .Add(objUtility.String2Date(txtReceiptDateEnd.Text.Trim, "dd/MM/yyyy").ToString("yyyyMMdd"))
                    End If
                End With
                ' set object to session
                Session("listControlValue") = SetListOfSting
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetListOfSting", ex.Message.ToString, Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DisplayPage
        '	Discription	    : Display page
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 28-08-2013
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
                dtInquiry = Session("dtSearchInquiry")
                Session("PageNo") = intPageNo

                ' check record for display
                If Not IsNothing(dtInquiry) AndAlso dtInquiry.Rows.Count > 0 Then
                    ' get page source for repeater
                    pagedData = objPage.DoPaging(intPageNo, dtInquiry)
                    ' write paging
                    lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                    ' bound data between pageDate with repeater
                    rptInquery.DataSource = pagedData
                    rptInquery.DataBind()
                    ' call fucntion set description
                    lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtInquiry.Rows.Count)
                    'btnApply.Enabled = True
                Else
                    ' case not exist data
                    ' show message box
                    If Not (boolNotAlertMsg) Then
                        objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
                    End If

                    ' clear binding data and clear description
                    lblPaging.Text = Nothing
                    lblDescription.Text = "&nbsp;"
                    rptInquery.DataSource = Nothing
                    rptInquery.DataBind()
                    'btnApply.Enabled = False
                End If
                'Session("Action") = Action.Insert
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
            Finally
                objUtility.RemQueryString("PageNo")
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: SetDataToControl
        '	Discription	    : Set data to control
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 01-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub SetDataToControl()
            Try
                Dim lsOfCtrlValue As List(Of String) = Session("listControlValue")
                With CType(Session("listControlValue"), List(Of String))
                    txtJobOrderStart.Text = .Item(0)
                    txtJobOrderEnd.Text = .Item(1)
                    rbtAccountType.SelectedValue = CInt(.Item(2))
                    txtVendorName.Text = .Item(3)
                    ddlItemExpense.SelectedValue = CInt(.Item(4))
                    txtAccountName.Text = .Item(5)
                    txtAccountNo.Text = .Item(6)
                    txtReceiptDateStart.Text = .Item(7)
                    txtReceiptDateEnd.Text = .Item(8)
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDataToControl", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: LoadListAccountTitle
        '	Discription	    : Load list Account Title function
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 27-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub LoadListAccountTitle(ByVal intIEType As Integer)
            Try
                ' object Vendor service
                Dim objAccTitleSer As New Service.ImpIEService
                ' listVendorDto for keep value from service
                Dim listAccTitleDto As New List(Of Dto.IEDto)
                ' call function GetVendorForList from service
                listAccTitleDto = objAccTitleSer.GetListAccountTitleToDDL(intIEType)

                ' call function for bound data with dropdownlist
                objUtility.LoadList(ddlItemExpense, listAccTitleDto, "Name", "ID", True)

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: SetValidateErrorMessage
        '	Discription	    : Set Validate Error Message
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 30-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub SetValidateErrorMessage()
            Try
                reqJobOrderRange.ErrorMessage = objMessage.GetXMLMessage("KTJB_01_005")
                reqDateInvalid.ErrorMessage = objMessage.GetXMLMessage("Common_004")
                reqDateRange.ErrorMessage = objMessage.GetXMLMessage("Common_005")
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetValidateErrorMessage", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub


#End Region

#Region "WebMethod"
        '/**************************************************************
        '	Function name	: IsValidDateRange
        '	Discription	    : Check valid date range
        '	Return Value	: Boolean
        '	Create User	    : Wasan D.
        '	Create Date	    : 30-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        <WebMethod()> _
        Public Shared Function IsValidDateRange( _
            ByVal strDateFrom As String, _
            ByVal strDateTo As String _
        ) As Boolean
            Dim objUtility As New Common.Utilities.Utility
            If Not (String.IsNullOrEmpty(strDateFrom)) AndAlso Not (String.IsNullOrEmpty(strDateTo)) Then
                If objUtility.String2Date(strDateFrom, "dd/MM/yyyy") > objUtility.String2Date(strDateTo, "dd/MM/yyyy") Then Return False
            End If
            Return True
        End Function

        '/**************************************************************
        '	Function name	: IsValidDate
        '	Discription	    : Check valid date format
        '	Return Value	: Boolean
        '	Create User	    : Wasan D.
        '	Create Date	    : 30-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        <WebMethod()> _
        Public Shared Function IsValidDate( _
            ByVal strDateFrom As String, _
            ByVal strDateTo As String, _
            ByVal name As String _
        ) As Boolean

            Dim specificCulture As System.Globalization.CultureInfo = CultureInfo.CreateSpecificCulture(name)
            Dim validDateFrom As Boolean = True
            Dim validDateTo As Boolean = True
            Dim parsedDate As Date

            If Not (String.IsNullOrEmpty(strDateFrom)) Then validDateFrom = Date.TryParse(strDateFrom, specificCulture, DateTimeStyles.None, parsedDate)
           
            If Not (String.IsNullOrEmpty(strDateTo)) Then validDateTo = Date.TryParse(strDateTo, specificCulture, DateTimeStyles.None, parsedDate)

            If validDateFrom = True AndAlso validDateTo = True Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region
    End Class
End Namespace