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
'	Package Name	    : Working Hour 
'	Class Name		    : WorkingHour_KTWH01
'	Class Discription	: Webpage for Working Hour  
'	Create User 		: Suwishaya L.
'	Create Date		    : 10-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region


Partial Class WorkingHour_KTWH01
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objWorkingHourSer As New Service.ImpWorkingHourService
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const strResult As String = "Result"

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTWH01 : Working Hour")
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
    '	Create Date	    : 10-07-2013
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
    '	Function name	: btnAdd_Click
    '	Discription	    : Event btnAdd is clicked
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnAdd.Click
        Try
            ' redirect to KTWH02 with Add mode
            Response.Redirect("KTWH02.aspx?Mode=Add")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
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
            Dim dtWorkingHourReportSearch As New DataTable
            'check error
            If CheckCriteriaInput() = False Then
                Exit Sub
            End If

            'Get data
            SearchDataReport()

            ' get table object from session 
            dtWorkingHourReportSearch = Session("dtWorkingHourReportSearch")

            If Not IsNothing(dtWorkingHourReportSearch) AndAlso dtWorkingHourReportSearch.Rows.Count > 0 Then
                objMessage.ShowPagePopup("../Report/ReportViewer.aspx?ReportName=KTWH02", 1000, 990)
            Else
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnReport_Click", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: btnSearch_Click
    '	Discription	    : Event btnSearch is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSearch.Click

        Try
            'Check input Criteria data
            If CheckCriteriaInput() = False Then
                Exit Sub
            End If

            ' call function search data
            SearchData()

            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            SetDataToSession()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptWorkingHour_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptWorkingHour_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptWorkingHour.DataBinding
        Try
            ' clear hashtable data
            ClearHashtable()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptWorkingHour_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptWorkingHour_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptWorkingHour_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptWorkingHour.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intID As Integer = CInt(hashID(e.Item.ItemIndex).ToString())

            ' set ID to session
            Session("intID") = intID

            Select Case e.CommandName
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTWH01", strResult, objMessage.GetXMLMessage("KTWH_01_001"))

                Case "Edit"
                    ' redirect to KTJB02
                    Response.Redirect("KTWH02.aspx?Mode=Edit&id=" & intID)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptWorkingHour_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptWorkingHour_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptWorkingHour_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptWorkingHour.ItemDataBound
        Try
            ' object link button
            Dim btnDel As New LinkButton
            Dim btnEdit As New LinkButton

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)

            'set data and font on Repeater
            SetDataRepeater(e)

            ' set permission on button
            If Not Session("actUpdate") Then
                btnEdit.CssClass = "icon_edit2 icon_center15"
                btnEdit.Enabled = False
            End If

            If Not Session("actDelete") Then
                btnDel.CssClass = "icon_del2 icon_center15"
                btnDel.Enabled = False
            End If


            ' Set data to hashtable
            hashID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrder_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#Region "Property"

    ' Stores the id keys in ViewState
    ReadOnly Property hashID() As Hashtable
        Get
            If IsNothing(ViewState("hashID")) Then
                ViewState("hashID") = New Hashtable()
            End If
            Return CType(ViewState("hashID"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0830() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0830")) Then
                ViewState("hashLap0830") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0830"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0900() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0900")) Then
                ViewState("hashLap0900") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0900"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1000() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1000")) Then
                ViewState("hashLap1000") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1000"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1100() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1100")) Then
                ViewState("hashLap1100") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1100"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1200() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1200")) Then
                ViewState("hashLap1200") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1200"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1245() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1245")) Then
                ViewState("hashLap1245") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1245"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1300() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1300")) Then
                ViewState("hashLap1300") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1300"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1400() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1400")) Then
                ViewState("hashLap1400") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1400"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1500() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1500")) Then
                ViewState("hashLap1500") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1500"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1515() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1515")) Then
                ViewState("hashLap1515") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1515"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1530() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1530")) Then
                ViewState("hashLap1530") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1530"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1600() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1600")) Then
                ViewState("hashLap1600") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1600"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1700() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1700")) Then
                ViewState("hashLap1700") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1700"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1730() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1730")) Then
                ViewState("hashLap1730") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1730"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1800() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1800")) Then
                ViewState("hashLap1800") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1800"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap1900() As Hashtable
        Get
            If IsNothing(ViewState("hashLap1900")) Then
                ViewState("hashLap1900") = New Hashtable()
            End If
            Return CType(ViewState("hashLap1900"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap2000() As Hashtable
        Get
            If IsNothing(ViewState("hashLap2000")) Then
                ViewState("hashLap2000") = New Hashtable()
            End If
            Return CType(ViewState("hashLap2000"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap2100() As Hashtable
        Get
            If IsNothing(ViewState("hashLap2100")) Then
                ViewState("hashLap2100") = New Hashtable()
            End If
            Return CType(ViewState("hashLap2100"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap2200() As Hashtable
        Get
            If IsNothing(ViewState("hashLap2200")) Then
                ViewState("hashLap2200") = New Hashtable()
            End If
            Return CType(ViewState("hashLap2200"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap2300() As Hashtable
        Get
            If IsNothing(ViewState("hashLap2300")) Then
                ViewState("hashLap2300") = New Hashtable()
            End If
            Return CType(ViewState("hashLap2300"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0000() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0000")) Then
                ViewState("hashLap0000") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0000"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0045() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0045")) Then
                ViewState("hashLap0045") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0045"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0100() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0100")) Then
                ViewState("hashLap0100") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0100"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0200() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0200")) Then
                ViewState("hashLap0200") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0200"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0300() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0300")) Then
                ViewState("hashLap0300") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0300"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0315() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0315")) Then
                ViewState("hashLap0315") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0315"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0330() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0330")) Then
                ViewState("hashLap0330") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0330"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0400() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0400")) Then
                ViewState("hashLap0400") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0400"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0500() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0500")) Then
                ViewState("hashLap0500") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0500"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0600() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0600")) Then
                ViewState("hashLap0600") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0600"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0700() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0700")) Then
                ViewState("hashLap0700") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0700"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashLap0800() As Hashtable
        Get
            If IsNothing(ViewState("hashLap0800")) Then
                ViewState("hashLap0800") = New Hashtable()
            End If
            Return CType(ViewState("hashLap0800"), Hashtable)
        End Get
    End Property

#End Region

#End Region

#Region "Function"

    '/**************************************************************
    '	Function name	: ClearHashtable
    '	Discription	    : Clear Hashtable
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearHashtable()
        Try
            ' clear hashtable data
            hashID.Clear()
            hashLap0830.Clear()
            hashLap0900.Clear()
            hashLap1000.Clear()
            hashLap1100.Clear()
            hashLap1200.Clear()
            hashLap1245.Clear()
            hashLap1300.Clear()
            hashLap1400.Clear()
            hashLap1500.Clear()
            hashLap1515.Clear()
            hashLap1530.Clear()
            hashLap1600.Clear()
            hashLap1700.Clear()
            hashLap1730.Clear()
            hashLap1800.Clear()
            hashLap1900.Clear()
            hashLap2000.Clear()
            hashLap2100.Clear()
            hashLap2200.Clear()
            hashLap2300.Clear()
            hashLap0000.Clear()
            hashLap0045.Clear()
            hashLap0100.Clear()
            hashLap0200.Clear()
            hashLap0300.Clear()
            hashLap0315.Clear()
            hashLap0330.Clear()
            hashLap0400.Clear()
            hashLap0500.Clear()
            hashLap0600.Clear()
            hashLap0700.Clear()
            hashLap0800.Clear()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearHashtable", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataRepeater
    '	Discription	    : Set value and font color to Repeater
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataRepeater(ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Try
            ' object label
            Dim lblLap0830 As New Label
            Dim lblLap0900 As New Label
            Dim lblLap1000 As New Label
            Dim lblLap1100 As New Label
            Dim lblLap1200 As New Label
            Dim lblLap1245 As New Label
            Dim lblLap1300 As New Label
            Dim lblLap1400 As New Label
            Dim lblLap1500 As New Label
            Dim lblLap1515 As New Label
            Dim lblLap1530 As New Label
            Dim lblLap1600 As New Label
            Dim lblLap1700 As New Label
            Dim lblLap1730 As New Label
            Dim lblLap1800 As New Label
            Dim lblLap1900 As New Label
            Dim lblLap2000 As New Label
            Dim lblLap2100 As New Label
            Dim lblLap2200 As New Label
            Dim lblLap2300 As New Label
            Dim lblLap0000 As New Label
            Dim lblLap0045 As New Label
            Dim lblLap0100 As New Label
            Dim lblLap0200 As New Label
            Dim lblLap0300 As New Label
            Dim lblLap0315 As New Label
            Dim lblLap0330 As New Label
            Dim lblLap0400 As New Label
            Dim lblLap0500 As New Label
            Dim lblLap0600 As New Label
            Dim lblLap0700 As New Label
            Dim lblLap0800 As New Label

            ' find label and assign to variable
            lblLap0830 = DirectCast(e.Item.FindControl("lblLap0830"), Label)
            lblLap0900 = DirectCast(e.Item.FindControl("lblLap0900"), Label)
            lblLap1000 = DirectCast(e.Item.FindControl("lblLap1000"), Label)
            lblLap1100 = DirectCast(e.Item.FindControl("lblLap1100"), Label)
            lblLap1200 = DirectCast(e.Item.FindControl("lblLap1200"), Label)
            lblLap1245 = DirectCast(e.Item.FindControl("lblLap1245"), Label)
            lblLap1300 = DirectCast(e.Item.FindControl("lblLap1300"), Label)
            lblLap1400 = DirectCast(e.Item.FindControl("lblLap1400"), Label)
            lblLap1500 = DirectCast(e.Item.FindControl("lblLap1500"), Label)
            lblLap1515 = DirectCast(e.Item.FindControl("lblLap1515"), Label)
            lblLap1530 = DirectCast(e.Item.FindControl("lblLap1530"), Label)
            lblLap1600 = DirectCast(e.Item.FindControl("lblLap1600"), Label)
            lblLap1700 = DirectCast(e.Item.FindControl("lblLap1700"), Label)
            lblLap1730 = DirectCast(e.Item.FindControl("lblLap1730"), Label)
            lblLap1800 = DirectCast(e.Item.FindControl("lblLap1800"), Label)
            lblLap1900 = DirectCast(e.Item.FindControl("lblLap1900"), Label)
            lblLap2000 = DirectCast(e.Item.FindControl("lblLap2000"), Label)
            lblLap2100 = DirectCast(e.Item.FindControl("lblLap2100"), Label)
            lblLap2200 = DirectCast(e.Item.FindControl("lblLap2200"), Label)
            lblLap2300 = DirectCast(e.Item.FindControl("lblLap2300"), Label)
            lblLap0000 = DirectCast(e.Item.FindControl("lblLap0000"), Label)
            lblLap0045 = DirectCast(e.Item.FindControl("lblLap0045"), Label)
            lblLap0100 = DirectCast(e.Item.FindControl("lblLap0100"), Label)
            lblLap0200 = DirectCast(e.Item.FindControl("lblLap0200"), Label)
            lblLap0300 = DirectCast(e.Item.FindControl("lblLap0300"), Label)
            lblLap0315 = DirectCast(e.Item.FindControl("lblLap0315"), Label)
            lblLap0330 = DirectCast(e.Item.FindControl("lblLap0330"), Label)
            lblLap0400 = DirectCast(e.Item.FindControl("lblLap0400"), Label)
            lblLap0500 = DirectCast(e.Item.FindControl("lblLap0500"), Label)
            lblLap0600 = DirectCast(e.Item.FindControl("lblLap0600"), Label)
            lblLap0700 = DirectCast(e.Item.FindControl("lblLap0700"), Label)
            lblLap0800 = DirectCast(e.Item.FindControl("lblLap0800"), Label)

            ' Set data to hashtable
            hashLap0830.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0830"))
            hashLap0900.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0900"))
            hashLap1000.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1000"))
            hashLap1100.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1100"))
            hashLap1200.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1200"))
            hashLap1245.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1245"))
            hashLap1300.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1300"))
            hashLap1400.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1400"))
            hashLap1500.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1500"))
            hashLap1515.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1515"))
            hashLap1530.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1530"))
            hashLap1600.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1600"))
            hashLap1700.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1700"))
            hashLap1730.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1730"))
            hashLap1800.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1800"))
            hashLap1900.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap1900"))
            hashLap2000.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap2000"))
            hashLap2100.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap2100"))
            hashLap2200.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap2200"))
            hashLap2300.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap2300"))
            hashLap0000.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0000"))
            hashLap0045.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0045"))
            hashLap0100.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0100"))
            hashLap0200.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0200"))
            hashLap0300.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0300"))
            hashLap0315.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0315"))
            hashLap0330.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0330"))
            hashLap0400.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0400"))
            hashLap0500.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0500"))
            hashLap0600.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0600"))
            hashLap0700.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0700"))
            hashLap0800.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Lap0800"))

            'Set data hashtable to agument 
            Dim strLap0830 As String = hashLap0830(e.Item.ItemIndex).ToString()
            Dim strLap0900 As String = hashLap0900(e.Item.ItemIndex).ToString()
            Dim strLap1000 As String = hashLap1000(e.Item.ItemIndex).ToString()
            Dim strLap1100 As String = hashLap1100(e.Item.ItemIndex).ToString()
            Dim strLap1200 As String = hashLap1200(e.Item.ItemIndex).ToString()
            Dim strLap1245 As String = hashLap1245(e.Item.ItemIndex).ToString()
            Dim strLap1300 As String = hashLap1300(e.Item.ItemIndex).ToString()
            Dim strLap1400 As String = hashLap1400(e.Item.ItemIndex).ToString()
            Dim strLap1500 As String = hashLap1500(e.Item.ItemIndex).ToString()
            Dim strLap1515 As String = hashLap1515(e.Item.ItemIndex).ToString()
            Dim strLap1530 As String = hashLap1530(e.Item.ItemIndex).ToString()
            Dim strLap1600 As String = hashLap1600(e.Item.ItemIndex).ToString()
            Dim strLap1700 As String = hashLap1700(e.Item.ItemIndex).ToString()
            Dim strLap1730 As String = hashLap1730(e.Item.ItemIndex).ToString()
            Dim strLap1800 As String = hashLap1800(e.Item.ItemIndex).ToString()
            Dim strLap1900 As String = hashLap1900(e.Item.ItemIndex).ToString()
            Dim strLap2000 As String = hashLap2000(e.Item.ItemIndex).ToString()
            Dim strLap2100 As String = hashLap2100(e.Item.ItemIndex).ToString()
            Dim strLap2200 As String = hashLap2200(e.Item.ItemIndex).ToString()
            Dim strLap2300 As String = hashLap2300(e.Item.ItemIndex).ToString()
            Dim strLap0000 As String = hashLap0000(e.Item.ItemIndex).ToString()
            Dim strLap0045 As String = hashLap0045(e.Item.ItemIndex).ToString()
            Dim strLap0100 As String = hashLap0100(e.Item.ItemIndex).ToString()
            Dim strLap0200 As String = hashLap0200(e.Item.ItemIndex).ToString()
            Dim strLap0300 As String = hashLap0300(e.Item.ItemIndex).ToString()
            Dim strLap0315 As String = hashLap0315(e.Item.ItemIndex).ToString()
            Dim strLap0330 As String = hashLap0330(e.Item.ItemIndex).ToString()
            Dim strLap0400 As String = hashLap0400(e.Item.ItemIndex).ToString()
            Dim strLap0500 As String = hashLap0500(e.Item.ItemIndex).ToString()
            Dim strLap0600 As String = hashLap0600(e.Item.ItemIndex).ToString()
            Dim strLap0700 As String = hashLap0700(e.Item.ItemIndex).ToString()
            Dim strLap0800 As String = hashLap0800(e.Item.ItemIndex).ToString()

            'split data to array
            Dim arrLap0830() As String = Split(strLap0830.Trim(), ",")
            Dim arrLap0900() As String = Split(strLap0900.Trim(), ",")
            Dim arrLap1000() As String = Split(strLap1000.Trim(), ",")
            Dim arrLap1100() As String = Split(strLap1100.Trim(), ",")
            Dim arrLap1200() As String = Split(strLap1200.Trim(), ",")
            Dim arrLap1245() As String = Split(strLap1245.Trim(), ",")
            Dim arrLap1300() As String = Split(strLap1300.Trim(), ",")
            Dim arrLap1400() As String = Split(strLap1400.Trim(), ",")
            Dim arrLap1500() As String = Split(strLap1500.Trim(), ",")
            Dim arrLap1515() As String = Split(strLap1515.Trim(), ",")
            Dim arrLap1530() As String = Split(strLap1530.Trim(), ",")
            Dim arrLap1600() As String = Split(strLap1600.Trim(), ",")
            Dim arrLap1700() As String = Split(strLap1700.Trim(), ",")
            Dim arrLap1730() As String = Split(strLap1730.Trim(), ",")
            Dim arrLap1800() As String = Split(strLap1800.Trim(), ",")
            Dim arrLap1900() As String = Split(strLap1900.Trim(), ",")
            Dim arrLap2000() As String = Split(strLap2000.Trim(), ",")
            Dim arrLap2100() As String = Split(strLap2100.Trim(), ",")
            Dim arrLap2200() As String = Split(strLap2200.Trim(), ",")
            Dim arrLap2300() As String = Split(strLap2300.Trim(), ",")
            Dim arrLap0000() As String = Split(strLap0000.Trim(), ",")
            Dim arrLap0045() As String = Split(strLap0045.Trim(), ",")
            Dim arrLap0100() As String = Split(strLap0100.Trim(), ",")
            Dim arrLap0200() As String = Split(strLap0200.Trim(), ",")
            Dim arrLap0300() As String = Split(strLap0300.Trim(), ",")
            Dim arrLap0315() As String = Split(strLap0315.Trim(), ",")
            Dim arrLap0330() As String = Split(strLap0330.Trim(), ",")
            Dim arrLap0400() As String = Split(strLap0400.Trim(), ",")
            Dim arrLap0500() As String = Split(strLap0500.Trim(), ",")
            Dim arrLap0600() As String = Split(strLap0600.Trim(), ",")
            Dim arrLap0700() As String = Split(strLap0700.Trim(), ",")
            Dim arrLap0800() As String = Split(strLap0800.Trim(), ",")

            'set data to label
            lblLap0830.Text = arrLap0830(0)
            lblLap0900.Text = arrLap0900(0)
            lblLap1000.Text = arrLap1000(0)
            lblLap1100.Text = arrLap1100(0)
            lblLap1200.Text = arrLap1200(0)
            lblLap1245.Text = arrLap1245(0)
            lblLap1300.Text = arrLap1300(0)
            lblLap1400.Text = arrLap1400(0)
            lblLap1500.Text = arrLap1500(0)
            lblLap1515.Text = arrLap1515(0)
            lblLap1530.Text = arrLap1530(0)
            lblLap1600.Text = arrLap1600(0)
            lblLap1700.Text = arrLap1700(0)
            lblLap1730.Text = arrLap1730(0)
            lblLap1800.Text = arrLap1800(0)
            lblLap1900.Text = arrLap1900(0)
            lblLap2000.Text = arrLap2000(0)
            lblLap2100.Text = arrLap2100(0)
            lblLap2200.Text = arrLap2200(0)
            lblLap2300.Text = arrLap2300(0)
            lblLap0000.Text = arrLap0000(0)
            lblLap0045.Text = arrLap0045(0)
            lblLap0100.Text = arrLap0100(0)
            lblLap0200.Text = arrLap0200(0)
            lblLap0300.Text = arrLap0300(0)
            lblLap0315.Text = arrLap0315(0)
            lblLap0330.Text = arrLap0330(0)
            lblLap0400.Text = arrLap0400(0)
            lblLap0500.Text = arrLap0500(0)
            lblLap0600.Text = arrLap0600(0)
            lblLap0700.Text = arrLap0700(0)
            lblLap0800.Text = arrLap0800(0)

            'set font color 
            If arrLap0830(1) = "1" Then
                lblLap0830.CssClass = "font_green"
            End If
            If arrLap0900(1) = "1" Then
                lblLap0900.CssClass = "font_green"
            End If

            If arrLap1000(1) = "1" Then
                lblLap1000.CssClass = "font_green"
            End If
            If arrLap1100(1) = "1" Then
                lblLap1100.CssClass = "font_green"
            End If
            If arrLap1200(1) = "1" Then
                lblLap1200.CssClass = "font_green"
            End If
            If arrLap1245(1) = "1" Then
                lblLap1245.CssClass = "font_green"
            End If
            If arrLap1300(1) = "1" Then
                lblLap1300.CssClass = "font_green"
            End If
            If arrLap1400(1) = "1" Then
                lblLap1400.CssClass = "font_green"
            End If
            If arrLap1500(1) = "1" Then
                lblLap1500.CssClass = "font_green"
            End If
            If arrLap1515(1) = "1" Then
                lblLap1515.CssClass = "font_green"
            End If
            If arrLap1530(1) = "1" Then
                lblLap1530.CssClass = "font_green"
            End If
            If arrLap1600(1) = "1" Then
                lblLap1600.CssClass = "font_green"
            End If
            If arrLap1700(1) = "1" Then
                lblLap1700.CssClass = "font_green"
            End If
            If arrLap1730(1) = "1" Then
                lblLap1730.CssClass = "font_green"
            End If
            If arrLap1800(1) = "1" Then
                lblLap1800.CssClass = "font_green"
            End If
            If arrLap1900(1) = "1" Then
                lblLap1900.CssClass = "font_green"
            End If
            If arrLap2000(1) = "1" Then
                lblLap2000.CssClass = "font_green"
            End If
            If arrLap2100(1) = "1" Then
                lblLap2100.CssClass = "font_green"
            End If
            If arrLap2200(1) = "1" Then
                lblLap2200.CssClass = "font_green"
            End If
            If arrLap2300(1) = "1" Then
                lblLap2300.CssClass = "font_green"
            End If
            If arrLap0000(1) = "1" Then
                lblLap0000.CssClass = "font_green"
            End If
            If arrLap0045(1) = "1" Then
                lblLap0045.CssClass = "font_green"
            End If
            If arrLap0100(1) = "1" Then
                lblLap0100.CssClass = "font_green"
            End If
            If arrLap0200(1) = "1" Then
                lblLap0200.CssClass = "font_green"
            End If
            If arrLap0300(1) = "1" Then
                lblLap0300.CssClass = "font_green"
            End If
            If arrLap0315(1) = "1" Then
                lblLap0315.CssClass = "font_green"
            End If
            If arrLap0330(1) = "1" Then
                lblLap0330.CssClass = "font_green"
            End If
            If arrLap0400(1) = "1" Then
                lblLap0400.CssClass = "font_green"
            End If
            If arrLap0500(1) = "1" Then
                lblLap0500.CssClass = "font_green"
            End If
            If arrLap0600(1) = "1" Then
                lblLap0600.CssClass = "font_green"
            End If
            If arrLap0700(1) = "1" Then
                lblLap0700.CssClass = "font_green"
            End If
            If arrLap0800(1) = "1" Then
                lblLap0800.CssClass = "font_green"
            End If


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDataRepeater", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

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

            ' call function set WorkCategory dropdownlist
            LoadListWorkCategory()

            ' call function set Staff dropdownlist
            LoadListStaff()

            ' check case new enter
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
                ' set search text to session
                SetSessionToItem()
            Else
                If Session("flagAddMod") = "" Or Session("flagAddMod") = Nothing Then
                    ' case not new enter then display page with page no
                    DisplayPage(Request.QueryString("PageNo"))
                Else

                    SetSessionToItem()
                    ' call function search data
                    SearchData()
                    ' case not new enter then display page with page no
                    DisplayPage(Request.QueryString("PageNo"))
                End If
            End If

            ' call function check permission
            CheckPermission()

            ' check delete item
            If objUtility.GetQueryString(strResult) = "True" Then
                DeleteWorkingHour()
            End If
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
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckCriteriaInput() As Boolean
        Try
            CheckCriteriaInput = False
            Dim objIsDate As New Common.Validations.Validation

            'Check format date of field Issue Date From
            If txtWorkDate.Text.Trim <> "" Then
                If objIsDate.IsDate(txtWorkDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    txtWorkDate.Focus()
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
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
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtWorkingHour As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtWorkingHour = Session("dtWorkingHour")

            ' check record for display
            If Not IsNothing(dtWorkingHour) AndAlso dtWorkingHour.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtWorkingHour)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptWorkingHour.DataSource = pagedData
                rptWorkingHour.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtWorkingHour.Rows.Count)
            Else
                ' case not exist data
                ' show message box         
                If Session("flagAddMod") = "" Or Session("flagAddMod") = Nothing Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
                End If

                Session("flagAddMod") = Nothing
                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptWorkingHour.DataSource = Nothing
                rptWorkingHour.DataBind()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search job order data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtWorkingHour As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetWorkingHourList from WorkingHourService
            dtWorkingHour = objWorkingHourSer.GetWorkingHourList(Session("objWorkingHourDto"))
            ' set table object to session
            Session("dtWorkingHour") = dtWorkingHour
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchDataReport
    '	Discription	    : Search job order data for report
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchDataReport()
        Try
            ' table object keep value from item service
            Dim dtWorkingHourReportSearch As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetWorkingHourReport from JobOrderService
            dtWorkingHourReportSearch = objWorkingHourSer.GetWorkingHourReportSearch(Session("objWorkingHourDto"))
            ' set table object to session
            Session("dtWorkingHourReportSearch") = dtWorkingHourReportSearch
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
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(23)
            ' set permission Create
            btnAdd.Enabled = objAction.actCreate
            btnReport.Enabled = objAction.actList
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
            Session("dtWorkingHour") = Nothing
            Session("txtWorkDate") = Nothing
            Session("ddlStaff") = Nothing
            Session("ddlCategory") = Nothing
            Session("intID") = Nothing
            Session("actUpdate") = Nothing
            Session("actDelete") = Nothing

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
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            'Working Hour dto object
            Dim objWorkingHourDto As New Dto.WorkingHourDto
            Dim workDate As String = ""
            Dim arrWorkDate() As String = Split(txtWorkDate.Text.Trim(), "/")

            'set data from condition search into dto object
            With objWorkingHourDto
                'Set work date to format yyymmdd
                If UBound(arrWorkDate) > 0 Then
                    workDate = arrWorkDate(2) & arrWorkDate(1) & arrWorkDate(0)
                End If

                .work_date = workDate
                .staff_id_search = ddlStaff.SelectedValue
                .work_category_id_search = ddlCategory.SelectedValue

            End With

            ' set dto object to session
            Session("objWorkingHourDto") = objWorkingHourDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteWorkingHour
    '	Discription	    : Delete Working Hour data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteWorkingHour()
        Try

            ' check state of delete item
            If objWorkingHourSer.DeleteWorkingHour(Session("intID")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTWH_01_003"))
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTWH_01_002"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteWorkingHour", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListWorkCategory
    '	Discription	    : Load list WorkCategory function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListWorkCategory()
        Try
            ' object Staff service
            Dim objStaffSer As New Service.ImpStaffService
            ' listStaffDto for keep value from service
            Dim listStaffDto As New List(Of Dto.StaffDto)
            ' call function GetJobTypeForList from service
            listStaffDto = objStaffSer.GetWorkCategoryForList

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlCategory, listStaffDto, "name", "id", True)

            ' set select job type from session
            If Not IsNothing(Session("ddlCategory")) And ddlCategory.Items.Count > 0 Then
                ddlCategory.SelectedValue = Session("ddlCategory")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListWorkCategory", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListStaff
    '	Discription	    : Load list staff function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListStaff()
        Try
            ' object Staff service
            Dim objStaffSer As New Service.ImpStaffService
            ' listStaffDto for keep value from service
            Dim listStaffDto As New List(Of Dto.StaffDto)
            ' call function GetJobTypeForList from service
            listStaffDto = objStaffSer.GetStaffForList

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlStaff, listStaffDto, "name", "id", True)

            ' set select job type from session
            If Not IsNothing(Session("ddlStaff")) And ddlStaff.Items.Count > 0 Then
                ddlStaff.SelectedValue = Session("ddlStaff")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListStaff", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetSessionToItem
    '	Discription	    : Set data to item
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetSessionToItem()
        Try
            ' set search text to session
            txtWorkDate.Text = Session("txtWorkDate")
            ddlStaff.SelectedValue = Session("ddlStaff")
            ddlCategory.SelectedValue = Session("ddlCategory")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetSessionToItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToSession
    '	Discription	    : Set data to session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession()
        Try
            'set data from item to session
            Session("txtWorkDate") = txtWorkDate.Text.Trim
            Session("ddlStaff") = ddlStaff.SelectedValue
            Session("ddlCategory") = ddlCategory.SelectedValue

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDataToSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

End Class
