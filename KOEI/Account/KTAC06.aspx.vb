Imports System.Data
Imports System.Web.Configuration
Imports OfficeOpenXml.Style
Imports OfficeOpenXml
Imports System.IO
Imports System.Globalization
Imports MySql.Data.MySqlClient

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Accounting
'	Class Name		    : KTAC01
'	Class Discription	: Searching data from [Accounting] table
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 07-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region
Partial Class KTAC06
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private objValidate As New Common.Validations.Validation
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message

    'connect with service
    Private objAccountingService As New Service.ImpAccountingService
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : ini load
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            objLog.StartLog("KTAC06", Session("UserName"))
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 13-06-2013
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
    '	Function name	: btnSearch_Click
    '	Discription	    : Event btnSearch is click
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 13-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
            ByVal sender As Object, _
            ByVal e As System.EventArgs _
        ) Handles btnSearch.Click
        Try
            'check error
            If CheckError() = False Then
                Exit Sub
            End If

            ' call function search data
            SearchData()
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            setScreenToSession()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSearch_Click
    '	Discription	    : call export pdf
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 13-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnPdf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdf.Click
        Try
            Dim dtCostTableDetail As New DataTable

            'check error
            If CheckError() = False Then
                Exit Sub
            End If

            'Get data
            SearchData()

            ' get table object from session 
            dtCostTableDetail = Session("dtCostTableDetail")

            If Not IsNothing(dtCostTableDetail) AndAlso dtCostTableDetail.Rows.Count > 0 Then
                objMessage.ShowPagePopup("../Report/ReportViewer.aspx?ReportName=KTAC06", 1000, 990)
            Else
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnPdf_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#Region "Function"
    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 13-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Accounting dto object
            Dim objAccountingDto As New Dto.AccountingDto
            Dim startDate As String = ""
            Dim endDate As String = ""
            Dim arrStartDate() As String = Split(txtStartDate.Text.Trim(), "/")
            Dim arrEndDate() As String = Split(txtEndDate.Text.Trim(), "/")

            'set data from condition search into dto object
            With objAccountingDto
                If UBound(arrStartDate) > 0 Then
                    startDate = arrStartDate(2) & arrStartDate(1) & arrStartDate(0)
                End If
                If UBound(arrEndDate) > 0 Then
                    endDate = arrEndDate(2) & arrEndDate(1) & arrEndDate(0)
                End If

                .strAccount_startdate = startDate
                .strAccount_enddate = endDate
                .strJoborder_start = txtStartJobOrder.Text.Trim
                .strJoborder_end = txtEndJobOrder.Text.Trim
                .strIe_start_code = txtStartIE_Code.Text.Trim
                .strVendor_name = txtVendor_name.Text.Trim
                .strIe_end_code = txtEndIE_Code.Text.Trim
                '.strIe_category_type = Enums.AccountRecordTypes.Payment
                .strStatus_id = Enums.RecordStatus.Completed & "," & Enums.RecordStatus.Approved
                .accType = ""
            End With

            ' set dto object to session
            Session("objAccountingDto") = objAccountingDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Item data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 13-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtCostTableDetail As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetItemList from ItemService
            dtCostTableDetail = objAccountingService.GetCostTableDetailList(Session("objAccountingDto"))
            ' set table object to session
            Session("dtCostTableDetail") = dtCostTableDetail
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 13-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtCostTableDetail As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtCostTableDetail = Session("dtCostTableDetail")

            ' check record for display
            If Not IsNothing(dtCostTableDetail) AndAlso dtCostTableDetail.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtCostTableDetail)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptInquery.DataSource = pagedData
                rptInquery.DataBind()

                ' call fucntion set description
                ShowDescription(intPageNo, pagedData.PageCount, dtCostTableDetail.Rows.Count)
            Else
                ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))

                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptInquery.DataSource = Nothing
                rptInquery.DataBind()

            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: ShowDescription
    '	Discription	    : Show description
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 13-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowDescription( _
        ByVal intPageNo As Integer, _
        ByVal intPageCount As Integer, _
        ByVal intAllRecs As Integer)
        Try
            ' variable page size get from web.config
            Dim intPageSize As Integer = CInt(WebConfigurationManager.AppSettings("PageSize"))
            Dim intStart As Integer
            Dim intEnd As Integer

            ' check page no
            If intPageNo = 0 Then
                intPageNo = 1
            End If

            ' set record start
            intStart = ((intPageNo - 1) * intPageSize) + 1

            ' set record end
            If intPageNo = intPageCount Then
                intEnd = intAllRecs
            Else
                intEnd = intPageNo * intPageSize
            End If

            ' set wording 
            lblDescription.Text = "Showing " & intStart.ToString & " to " & intEnd.ToString & _
            " of " & intAllRecs.ToString & " entries"
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ShowDescription", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 13-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try

            ' check case new enter
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"))
            End If


            setSessionToScreen()

            'SetDropDownlist()

            ' call function check permission
            CheckPermission()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: setScreenToSession
    '	Discription	    : set Screen To Session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 13-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub setScreenToSession()
        Try
            'ser value into session
            Session("txtStartDate") = txtStartDate.Text
            Session("txtEndDate") = txtEndDate.Text
            Session("txtStartIE_Code") = txtStartIE_Code.Text
            Session("txtEndIE_Code") = txtEndIE_Code.Text
            Session("txtStartJobOrder") = txtStartJobOrder.Text
            Session("txtEndJobOrder") = txtEndJobOrder.Text
            Session("txtVendor_name") = txtVendor_name.Text
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("setScreenToSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: setSessionToScreen
    '	Discription	    : set Session To Screen
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 13-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub setSessionToScreen()
        Try
            'set value to from session to textbox 
            txtStartDate.Text = Session("txtStartDate")
            txtEndDate.Text = Session("txtEndDate")
            txtStartIE_Code.Text = Session("txtStartIE_Code")
            txtEndIE_Code.Text = Session("txtEndIE_Code")
            txtStartJobOrder.Text = Session("txtStartJobOrder")
            txtEndJobOrder.Text = Session("txtEndJobOrder")
            txtVendor_name.Text = Session("txtVendor_name")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("setSessionToScreen", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 13-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtCostTableDetail") = Nothing
            Session("actList") = Nothing
            Session("txtStartDate") = Nothing
            Session("txtEndDate") = Nothing
            Session("txtStartIE_Code") = Nothing
            Session("txtEndIE_Code") = Nothing
            Session("txtStartJobOrder") = Nothing
            Session("txtEndJobOrder") = Nothing
            Session("txtVendor_name") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 13-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(12)

            ' set permission 
            btnPdf.Enabled = objAction.actList
            btnSearch.Enabled = objAction.actList

            ' set action permission to session
            Session("actList") = objAction.actList
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    ' Function name : IsDate
    ' Discription     : Check Is date format
    ' Return Value    : True , False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 13-06-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function CheckError() As Boolean
        CheckError = False

        Try
            'check start date
            If txtStartDate.Text.Trim <> "" Then
                If objValidate.IsDate(txtStartDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check end date
            If txtEndDate.Text.Trim <> "" Then
                If objValidate.IsDate(txtEndDate.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check date startDate >  endDate display "Please verify Date from must <= Date to"
            If txtEndDate.Text.Trim <> "" And txtStartDate.Text.Trim <> "" Then
                If objValidate.IsDate(txtStartDate.Text.Trim) > objValidate.IsDate(txtEndDate.Text.Trim) Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_005"))
                    Exit Function
                End If
            End If

            'check input account title from and to
            If txtStartIE_Code.Text.Trim <> "" And txtEndIE_Code.Text.Trim = "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAC_06_002"))
                Exit Function
            End If

            'check input account title from and to
            If txtStartIE_Code.Text.Trim = "" And txtEndIE_Code.Text.Trim <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAC_06_002"))
                Exit Function
            End If

            'check range of account title
            If String.Compare(txtStartIE_Code.Text.Trim, txtEndIE_Code.Text.Trim) > 0 Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAC_06_003"))
                Exit Function
            End If

            'check category of account title
            If txtStartIE_Code.Text.Trim <> "" And txtEndIE_Code.Text.Trim <> "" Then
                If objAccountingService.chkCategoryAccountTitle(txtStartIE_Code.Text.Trim) <> objAccountingService.chkCategoryAccountTitle(txtEndIE_Code.Text.Trim) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTAC_06_004"))
                    Exit Function
                End If
            End If

            'Check exit account Title from
            If txtStartIE_Code.Text.Trim <> "" Then
                If objAccountingService.chkExitIEMaster(txtStartIE_Code.Text.Trim) = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTAC_06_001"))
                    Exit Function
                End If
            End If

            'Check exit account Title to
            If txtEndIE_Code.Text.Trim <> "" Then
                If objAccountingService.chkExitIEMaster(txtEndIE_Code.Text.Trim) = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTAC_06_001"))
                    Exit Function
                End If
            End If

            CheckError = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckError", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    ' Function name   : GetStartIE
    ' Discription     : Set Value To DropDownlist
    ' Return Value    : List
    ' Create User     : Rawikarn Katekeaw
    ' Create Date     : 27-05-2014
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    <System.Web.Script.Services.ScriptMethod(), _
    System.Web.Services.WebMethod()> _
    Public Shared Function GetStartIE(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim conn As Common.DBConnection.MySQLAccess = New Common.DBConnection.MySQLAccess
        ' Dim cmdText As String = "SELECT cast(concat(code,'-',name,' # ',id) as char) as iename FROM mst_ie where delete_fg <> 1"
        Dim cmdText As String = "SELECT code as iename FROM mst_ie where delete_fg <> 1"
        If prefixText.Trim <> "" Then
            cmdText = cmdText & " and (name like '%" & prefixText & "%' or code like '%" & prefixText & "%')"
        End If
        cmdText = cmdText & " limit " & count.ToString()

        Dim IEs As List(Of String) = New List(Of String)
        Try
            If conn.ConnectionClose Then
                conn.Open()
            End If
            Dim sdr As MySqlDataReader = conn.ExecuteReader(cmdText)
            While sdr.Read
                IEs.Add(sdr("iename").ToString)
            End While
        Catch ex As Exception
        Finally
            If conn.ConnectionOpen Then
                conn.Close()
            End If
        End Try
        If IEs.Count > 0 Then
            Return IEs.ToArray()
        Else
            Return Nothing
        End If
    End Function

    '/**************************************************************
    ' Function name   : getEndIE
    ' Discription     : Set Value To DropDownlist
    ' Return Value    : List
    ' Create User     : Rawikarn Katekeaw
    ' Create Date     : 27-05-2014
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    <System.Web.Script.Services.ScriptMethod(), _
    System.Web.Services.WebMethod()> _
    Private Function getEndIE(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim conn As Common.DBConnection.MySQLAccess = New Common.DBConnection.MySQLAccess

        Dim cmdText As String = "SELECT code as iename FROM mst_ie where delete_fg <> 1"
        If prefixText.Trim <> "" Then
            cmdText = cmdText & " and (name like '%" & prefixText & "%' or code like '%" & prefixText & "%')"
        End If
        cmdText = cmdText & " limit " & count.ToString()

        Dim IEs As List(Of String) = New List(Of String)
        Try
            If conn.ConnectionClose Then
                conn.Open()
            End If
            Dim sdr As MySqlDataReader = conn.ExecuteReader(cmdText)
            While sdr.Read
                IEs.Add(sdr("iename").ToString)
            End While
        Catch ex As Exception
        Finally
            If conn.ConnectionOpen Then
                conn.Close()
            End If
        End Try
        If IEs.Count > 0 Then
            Return IEs.ToArray()
        Else
            Return Nothing
        End If
    End Function

#End Region
End Class
