Imports System.Data
Imports System.Web.Configuration
Imports OfficeOpenXml.Style
Imports OfficeOpenXml
Imports System.IO
Imports System.Globalization

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Working Hour Report
'	Class Name		    : WorkingHour_KTWH03
'	Class Discription	: Webpage for Working Hour  Report
'	Create User 		: Suwishaya L.
'	Create Date		    : 11-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class WorkingHour_KTWH03
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objWorkingHourSer As New Service.ImpWorkingHourService
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTWH03 : Working Hour Report")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
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
    '	Function name	: btnReport_Click
    '	Discription	    : export data to pdf file
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnReport_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnReport.Click
        Try
            Dim dtWorkingHourReport As New DataTable
            'check error
            If CheckCriteriaInput() = False Then
                Exit Sub
            End If

            'Get data
            SearchDataReport()

            'set status to session
            Select Case radStatus.SelectedValue
                Case ""
                    Session("status") = "All"
                Case "1"
                    Session("status") = "Finish"
                Case "0"
                    Session("status") = "Not Finish"
            End Select

            ' get table object from session 
            dtWorkingHourReport = Session("dtWorkingHourReport")

            If Not IsNothing(dtWorkingHourReport) AndAlso dtWorkingHourReport.Rows.Count > 0 Then
                objMessage.ShowPagePopup("../Report/ReportViewer.aspx?ReportName=KTWH03", 1000, 990)
            Else
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnReport_Click", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try

            ' check case new enter
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
            End If

            ' set search text to session
            txtStartDate.Text = Session("txtStartDate")
            txtEndDate.Text = Session("txtEndDate") 
            radStatus.SelectedValue = Session("radStatus")

            ' call function check permission
            CheckPermission()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckCriteriaInput
    '	Discription	    : Check Criteria input data 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckCriteriaInput() As Boolean
        Try
            CheckCriteriaInput = False
            Dim objIsDate As New Common.Validations.Validation

            'Check format date of field Start Date 
            If txtStartDate.Text.Trim <> "" Then
                If objIsDate.IsDate(txtStartDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    txtStartDate.Focus()
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check format date of field end Date 
            If txtEndDate.Text.Trim <> "" Then
                If objIsDate.IsDate(txtEndDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    txtEndDate.Focus()
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check txtStartDate > txtEndDate
            If txtStartDate.Text.Trim <> "" And txtEndDate.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtStartDate.Text.Trim, txtEndDate.Text.Trim) = False Then
                    txtStartDate.Focus()
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_005"))
                    Exit Function
                End If
            End If

            'Check year from <> year To
            If txtStartDate.Text.Trim <> "" And txtEndDate.Text.Trim <> "" Then
                If CDate(txtStartDate.Text.Trim).Year.ToString.Trim <> CDate(txtEndDate.Text.Trim).Year.ToString.Trim Then
                    txtStartDate.Focus()
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTWH_03_001"))
                    Exit Function
                End If
            End If

            CheckCriteriaInput = True

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckCriteriaInput", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SearchDataReport
    '	Discription	    : Search job order data for report
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchDataReport()
        Try
            ' table object keep value from item service
            Dim dtWorkingHourReport As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetWorkingHourReport from WorkingHourService
            dtWorkingHourReport = objWorkingHourSer.GetWorkingHourReport(Session("objWorkingHourDto"))
            ' set table object to session
            Session("dtWorkingHourReport") = dtWorkingHourReport
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDataReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(24)
            ' set permission Create 
            btnReport.Enabled = objAction.actList

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtWorkingHourReport") = Nothing
            Session("txtStartDate") = Nothing
            Session("txtEndDate") = Nothing
            Session("radStatus") = Nothing

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            'Working Hour dto object
            Dim objWorkingHourDto As New Dto.WorkingHourDto
            Dim startDate As String = ""
            Dim endDate As String = ""
            Dim arrStartDate() As String = Split(txtStartDate.Text.Trim(), "/")
            Dim arrEndDate() As String = Split(txtEndDate.Text.Trim(), "/")

            'set data from condition search into dto object
            With objWorkingHourDto
                'Set start date to format yyymmdd
                If UBound(arrStartDate) > 0 Then
                    startDate = arrStartDate(2) & arrStartDate(1) & arrStartDate(0)
                End If
                'Set end date to format yyymmdd
                If UBound(arrEndDate) > 0 Then
                    endDate = arrEndDate(2) & arrEndDate(1) & arrEndDate(0)
                End If

                .start_work_date = startDate
                .end_work_date = endDate
                .job_status = radStatus.SelectedValue

            End With

            ' set dto object to session
            Session("objWorkingHourDto") = objWorkingHourDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToSession
    '	Discription	    : Set data to session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession()
        Try
            'set data from item to session
            Session("txtStartDate") = txtStartDate.Text.Trim
            Session("txtEndDate") = txtEndDate.Text
            Session("radStatus") = radStatus.SelectedValue

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDataToSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region
End Class
