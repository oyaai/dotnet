#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Working Hour
'	Class Name		    : WorkingHour_KTWH02
'	Class Discription	: Webpage for maintenance Working Hour
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

Imports System.Data

Partial Class WorkingHour_KTWH02
    Inherits System.Web.UI.Page

    Private objUtility As New Common.Utilities.Utility
    Private objLog As New Common.Logs.Log
    Private objAction As New Common.UserPermissions.ActionPermission
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objWorkingHourSer As New Service.ImpWorkingHourService
    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private strMsg As String = String.Empty

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
            objLog.StartLog("KTWH02 : Working Hour")
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
    '	Function name	: btnBack_Click
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            'get data from search condition
            GetSession()
            Response.Redirect("KTWH01.aspx?New=False")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack1_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnBack1.Click
        Try
            'get data from search condition
            GetSession()
            Response.Redirect("KTWH01.aspx?New=False")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	    : Event btnClear is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
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
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSave.Click
        Try
            'get data from search condition
            GetSession()

            ' call function set session dto
            SetValueToDto()

            'call function set data to datatable
            SetDataToDataTable()

            'call funtion set job order to session
            GetJobOrder()

            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTWH02", strConfirmIns, objMessage.GetXMLMessage("KTWH_02_003"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTWH02", strConfirmUpd, objMessage.GetXMLMessage("KTWH_02_006"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnAddWorkingHour_Click
    '	Discription	    : Event btnAddWorkingHour is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAddWorkingHour_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnAddWorkingHour.Click
        Try
            'Check error
            If CheckCriteriaInput() = False Then
                Exit Sub
            End If

            'show detail part
            pnlWorkingHourDetail.Visible = True

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnAddWorkingHour_Click", ex.Message.ToString, Session("UserName"))
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
            ' call function check permission
            CheckPermission()

            ' call function set WorkCategory dropdownlist
            LoadListWorkCategory()

            ' call function set Staff dropdownlist
            LoadListStaff()

            ' check insert item
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                ' call function clear session
                ClearSession()
                'Insert Working Hour
                InsertWorkingHour()
                Exit Sub
            End If

            ' check update Working Hour
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateWorkingHour()
                Exit Sub
            End If

            'Set QueryString
            If Not String.IsNullOrEmpty(Request.QueryString("id")) And Request.QueryString("id") Is Nothing Then
                Session("working_hour_id") = 0
            Else
                Session("working_hour_id") = Request.QueryString("id")
            End If

            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                Session("Mode") = "Add"
                SetEnableItem()
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                LoadInitialUpdate()
                SetDisableItem()
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
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' Item Dto object for keep return value from service
            Dim objWorkingHourDto As New Dto.WorkingHourDto
            Dim intID As Integer = 0

            ' check item id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intID = CInt(objUtility.GetQueryString("id"))
                Session("working_hour_id") = intID
            Else
                intID = Session("working_hour_id")
            End If

            ' call function GetWorkingHourByID from service
            objWorkingHourDto = objWorkingHourSer.GetWorkingHourByID(intID)

            If objWorkingHourDto.work_date = Nothing And objWorkingHourDto.staff_id = Nothing And objWorkingHourDto.work_category_id = Nothing Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
                Exit Sub
            End If
            ' assign value to control
            With objWorkingHourDto
                'header 
                txtWorkingDate.Text = .work_date
                ddlWorkStaff.SelectedValue = .staff_id
                ddlWorkCategory.SelectedValue = .work_category_id
                'job order
                txtJobOrder1.Text = .Lap0830
                txtJobOrder2.Text = .Lap0900
                txtJobOrder3.Text = .Lap1000
                txtJobOrder4.Text = .Lap1100
                txtJobOrder5.Text = .Lap1245
                txtJobOrder6.Text = .Lap1300
                txtJobOrder7.Text = .Lap1400
                txtJobOrder8.Text = .Lap1500
                txtJobOrder9.Text = .Lap1530
                txtJobOrder10.Text = .Lap1600
                txtJobOrder11.Text = .Lap1700
                txtJobOrder12.Text = .Lap1800
                txtJobOrder13.Text = .Lap1900
                txtJobOrder14.Text = .Lap2000
                txtJobOrder15.Text = .Lap2100
                txtJobOrder16.Text = .Lap2200
                txtJobOrder17.Text = .Lap2300
                txtJobOrder18.Text = .Lap0045
                txtJobOrder19.Text = .Lap0100
                txtJobOrder20.Text = .Lap0200
                txtJobOrder21.Text = .Lap0300
                txtJobOrder22.Text = .Lap0330
                txtJobOrder23.Text = .Lap0400
                txtJobOrder24.Text = .Lap0500
                txtJobOrder25.Text = .Lap0600
                txtJobOrder26.Text = .Lap0700
                'detail
                txtDetail1.Text = .detail_1
                txtDetail2.Text = .detail_2
                txtDetail3.Text = .detail_3
                txtDetail4.Text = .detail_4
                txtDetail5.Text = .detail_5
                txtDetail6.Text = .detail_6
                txtDetail7.Text = .detail_7
                txtDetail8.Text = .detail_8
                txtDetail9.Text = .detail_9
                txtDetail10.Text = .detail_10
                txtDetail11.Text = .detail_11
                txtDetail12.Text = .detail_12
                txtDetail13.Text = .detail_13
                txtDetail14.Text = .detail_14
                txtDetail15.Text = .detail_15
                txtDetail16.Text = .detail_16
                txtDetail17.Text = .detail_17
                txtDetail18.Text = .detail_18
                txtDetail19.Text = .detail_19
                txtDetail20.Text = .detail_20
                txtDetail21.Text = .detail_21
                txtDetail22.Text = .detail_22
                txtDetail23.Text = .detail_23
                txtDetail24.Text = .detail_24
                txtDetail25.Text = .detail_25
                txtDetail26.Text = .detail_26
                'detail id 
                hidDetailId1.Value = .detail_id_1 
                hidDetailId2.Value = .detail_id_2
                hidDetailId3.Value = .detail_id_3
                hidDetailId4.Value = .detail_id_4
                hidDetailId5.Value = .detail_id_5
                hidDetailId6.Value = .detail_id_6
                hidDetailId7.Value = .detail_id_7
                hidDetailId8.Value = .detail_id_8
                hidDetailId9.Value = .detail_id_9
                hidDetailId10.Value = .detail_id_10
                hidDetailId11.Value = .detail_id_11
                hidDetailId12.Value = .detail_id_12
                hidDetailId13.Value = .detail_id_13
                hidDetailId14.Value = .detail_id_14
                hidDetailId15.Value = .detail_id_15
                hidDetailId16.Value = .detail_id_16
                hidDetailId17.Value = .detail_id_17
                hidDetailId18.Value = .detail_id_18
                hidDetailId19.Value = .detail_id_19
                hidDetailId20.Value = .detail_id_20
                hidDetailId21.Value = .detail_id_21
                hidDetailId22.Value = .detail_id_22
                hidDetailId23.Value = .detail_id_23
                hidDetailId24.Value = .detail_id_24
                hidDetailId25.Value = .detail_id_25
                hidDetailId26.Value = .detail_id_26
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
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            If Session("Mode") = "Add" Then

                txtWorkingDate.Text = String.Empty
                ddlWorkStaff.SelectedValue = String.Empty
                ddlWorkCategory.SelectedValue = String.Empty

                txtJobOrder1.Text = String.Empty
                txtJobOrder2.Text = String.Empty
                txtJobOrder3.Text = String.Empty
                txtJobOrder4.Text = String.Empty
                txtJobOrder5.Text = String.Empty
                txtJobOrder6.Text = String.Empty
                txtJobOrder7.Text = String.Empty
                txtJobOrder8.Text = String.Empty
                txtJobOrder9.Text = String.Empty
                txtJobOrder10.Text = String.Empty
                txtJobOrder11.Text = String.Empty
                txtJobOrder12.Text = String.Empty
                txtJobOrder13.Text = String.Empty
                txtJobOrder14.Text = String.Empty
                txtJobOrder15.Text = String.Empty
                txtJobOrder16.Text = String.Empty
                txtJobOrder17.Text = String.Empty
                txtJobOrder18.Text = String.Empty
                txtJobOrder19.Text = String.Empty
                txtJobOrder20.Text = String.Empty
                txtJobOrder21.Text = String.Empty
                txtJobOrder22.Text = String.Empty
                txtJobOrder23.Text = String.Empty
                txtJobOrder24.Text = String.Empty
                txtJobOrder25.Text = String.Empty
                txtJobOrder26.Text = String.Empty
                txtDetail1.Text = String.Empty
                txtDetail2.Text = String.Empty
                txtDetail3.Text = String.Empty
                txtDetail4.Text = String.Empty
                txtDetail5.Text = String.Empty
                txtDetail6.Text = String.Empty
                txtDetail7.Text = String.Empty
                txtDetail8.Text = String.Empty
                txtDetail9.Text = String.Empty
                txtDetail10.Text = String.Empty
                txtDetail11.Text = String.Empty
                txtDetail12.Text = String.Empty
                txtDetail13.Text = String.Empty
                txtDetail14.Text = String.Empty
                txtDetail15.Text = String.Empty
                txtDetail16.Text = String.Empty
                txtDetail17.Text = String.Empty
                txtDetail18.Text = String.Empty
                txtDetail19.Text = String.Empty
                txtDetail20.Text = String.Empty
                txtDetail21.Text = String.Empty
                txtDetail22.Text = String.Empty
                txtDetail23.Text = String.Empty
                txtDetail24.Text = String.Empty
                txtDetail25.Text = String.Empty
                txtDetail26.Text = String.Empty

                pnlWorkingHourDetail.Visible = False

            ElseIf Session("Mode") = "Edit" Then
                LoadInitialUpdate()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("working_hour_id") = Nothing
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
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' WorkingHour dto object
            Dim objWorkingHourDto As New Dto.WorkingHourDto
            Dim workDate As String = ""
            Dim arrWorkDate() As String = Split(txtWorkingDate.Text.Trim(), "/")

            ' assign value to dto object
            With objWorkingHourDto
                .id = Session("working_hour_id")
                .staff_id = ddlWorkStaff.SelectedValue
                .work_category_id = ddlWorkCategory.SelectedValue

                'Set work date to format yyymmdd
                If UBound(arrWorkDate) > 0 Then
                    workDate = arrWorkDate(2) & arrWorkDate(1) & arrWorkDate(0)
                End If

                .work_date = workDate

                .Lap0830 = txtJobOrder1.Text
                .Lap0900 = txtJobOrder2.Text
                .Lap1000 = txtJobOrder3.Text
                .Lap1100 = txtJobOrder4.Text
                .Lap1245 = txtJobOrder5.Text
                .Lap1300 = txtJobOrder6.Text
                .Lap1400 = txtJobOrder7.Text
                .Lap1500 = txtJobOrder8.Text
                .Lap1530 = txtJobOrder9.Text
                .Lap1600 = txtJobOrder10.Text
                .Lap1700 = txtJobOrder11.Text
                .Lap1800 = txtJobOrder12.Text
                .Lap1900 = txtJobOrder13.Text
                .Lap2000 = txtJobOrder14.Text
                .Lap2100 = txtJobOrder15.Text
                .Lap2200 = txtJobOrder16.Text
                .Lap2300 = txtJobOrder17.Text
                .Lap0045 = txtJobOrder18.Text
                .Lap0100 = txtJobOrder19.Text
                .Lap0200 = txtJobOrder20.Text
                .Lap0300 = txtJobOrder21.Text
                .Lap0330 = txtJobOrder22.Text
                .Lap0400 = txtJobOrder23.Text
                .Lap0500 = txtJobOrder24.Text
                .Lap0600 = txtJobOrder25.Text
                .Lap0700 = txtJobOrder26.Text
                .detail_1 = txtDetail1.Text
                .detail_2 = txtDetail2.Text
                .detail_3 = txtDetail3.Text
                .detail_4 = txtDetail4.Text
                .detail_5 = txtDetail5.Text
                .detail_6 = txtDetail6.Text
                .detail_7 = txtDetail7.Text
                .detail_8 = txtDetail8.Text
                .detail_9 = txtDetail9.Text
                .detail_10 = txtDetail10.Text
                .detail_11 = txtDetail11.Text
                .detail_12 = txtDetail12.Text
                .detail_13 = txtDetail13.Text
                .detail_14 = txtDetail14.Text
                .detail_15 = txtDetail15.Text
                .detail_16 = txtDetail16.Text
                .detail_17 = txtDetail17.Text
                .detail_18 = txtDetail18.Text
                .detail_19 = txtDetail19.Text
                .detail_20 = txtDetail20.Text
                .detail_21 = txtDetail21.Text
                .detail_22 = txtDetail22.Text
                .detail_23 = txtDetail23.Text
                .detail_24 = txtDetail24.Text
                .detail_25 = txtDetail25.Text
                .detail_26 = txtDetail26.Text

                .detail_id_1 = hidDetailId1.Value
                .detail_id_2 = hidDetailId2.Value
                .detail_id_3 = hidDetailId3.Value
                .detail_id_4 = hidDetailId4.Value
                .detail_id_5 = hidDetailId5.Value
                .detail_id_6 = hidDetailId6.Value
                .detail_id_7 = hidDetailId7.Value
                .detail_id_8 = hidDetailId8.Value
                .detail_id_9 = hidDetailId9.Value
                .detail_id_10 = hidDetailId10.Value
                .detail_id_11 = hidDetailId11.Value
                .detail_id_12 = hidDetailId12.Value
                .detail_id_13 = hidDetailId13.Value
                .detail_id_14 = hidDetailId14.Value
                .detail_id_15 = hidDetailId15.Value
                .detail_id_16 = hidDetailId16.Value
                .detail_id_17 = hidDetailId17.Value
                .detail_id_18 = hidDetailId18.Value
                .detail_id_19 = hidDetailId19.Value
                .detail_id_20 = hidDetailId20.Value
                .detail_id_21 = hidDetailId21.Value
                .detail_id_22 = hidDetailId22.Value
                .detail_id_23 = hidDetailId23.Value
                .detail_id_24 = hidDetailId24.Value
                .detail_id_25 = hidDetailId25.Value
                .detail_id_26 = hidDetailId26.Value

            End With

            ' set dto object to session
            Session("objWorkingHourDto") = objWorkingHourDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Item dto object
            Dim objWorkingHourDto As New Dto.WorkingHourDto
            Dim strWorkDate As String
            ' set value to dto object from session
            objWorkingHourDto = Session("objWorkingHourDto")

            ' set value to control
            With objWorkingHourDto

                If .work_date.ToString.Trim.Length > 0 Then
                    strWorkDate = Right(.work_date, 2) & "/" & Mid(.work_date, 5, 2) & "/" & Left(.work_date, 4)
                    txtWorkingDate.Text = strWorkDate
                Else
                    txtWorkingDate.Text = .work_date
                End If

                ddlWorkStaff.SelectedValue = .staff_id
                ddlWorkCategory.SelectedValue = .work_category_id

                txtJobOrder1.Text = .Lap0830
                txtJobOrder2.Text = .Lap0900
                txtJobOrder3.Text = .Lap1000
                txtJobOrder4.Text = .Lap1100
                txtJobOrder5.Text = .Lap1245
                txtJobOrder6.Text = .Lap1300
                txtJobOrder7.Text = .Lap1400
                txtJobOrder8.Text = .Lap1500
                txtJobOrder9.Text = .Lap1530
                txtJobOrder10.Text = .Lap1600
                txtJobOrder11.Text = .Lap1700
                txtJobOrder12.Text = .Lap1800
                txtJobOrder13.Text = .Lap1900
                txtJobOrder14.Text = .Lap2000
                txtJobOrder15.Text = .Lap2100
                txtJobOrder16.Text = .Lap2200
                txtJobOrder17.Text = .Lap2300
                txtJobOrder18.Text = .Lap0045
                txtJobOrder19.Text = .Lap0100
                txtJobOrder20.Text = .Lap0200
                txtJobOrder21.Text = .Lap0300
                txtJobOrder22.Text = .Lap0330
                txtJobOrder23.Text = .Lap0400
                txtJobOrder24.Text = .Lap0500
                txtJobOrder25.Text = .Lap0600
                txtJobOrder26.Text = .Lap0700
                txtDetail1.Text = .detail_1
                txtDetail2.Text = .detail_2
                txtDetail3.Text = .detail_3
                txtDetail4.Text = .detail_4
                txtDetail5.Text = .detail_5
                txtDetail6.Text = .detail_6
                txtDetail7.Text = .detail_7
                txtDetail8.Text = .detail_8
                txtDetail9.Text = .detail_9
                txtDetail10.Text = .detail_10
                txtDetail11.Text = .detail_11
                txtDetail12.Text = .detail_12
                txtDetail13.Text = .detail_13
                txtDetail14.Text = .detail_14
                txtDetail15.Text = .detail_15
                txtDetail16.Text = .detail_16
                txtDetail17.Text = .detail_17
                txtDetail18.Text = .detail_18
                txtDetail19.Text = .detail_19
                txtDetail20.Text = .detail_20
                txtDetail21.Text = .detail_21
                txtDetail22.Text = .detail_22
                txtDetail23.Text = .detail_23
                txtDetail24.Text = .detail_24
                txtDetail25.Text = .detail_25
                txtDetail26.Text = .detail_26

                'detail id 
                hidDetailId1.Value = .detail_id_1
                hidDetailId2.Value = .detail_id_2
                hidDetailId3.Value = .detail_id_3
                hidDetailId4.Value = .detail_id_4
                hidDetailId5.Value = .detail_id_5
                hidDetailId6.Value = .detail_id_6
                hidDetailId7.Value = .detail_id_7
                hidDetailId8.Value = .detail_id_8
                hidDetailId9.Value = .detail_id_9
                hidDetailId10.Value = .detail_id_10
                hidDetailId11.Value = .detail_id_11
                hidDetailId12.Value = .detail_id_12
                hidDetailId13.Value = .detail_id_13
                hidDetailId14.Value = .detail_id_14
                hidDetailId15.Value = .detail_id_15
                hidDetailId16.Value = .detail_id_16
                hidDetailId17.Value = .detail_id_17
                hidDetailId18.Value = .detail_id_18
                hidDetailId19.Value = .detail_id_19
                hidDetailId20.Value = .detail_id_20
                hidDetailId21.Value = .detail_id_21
                hidDetailId22.Value = .detail_id_22
                hidDetailId23.Value = .detail_id_23
                hidDetailId24.Value = .detail_id_24
                hidDetailId25.Value = .detail_id_25
                hidDetailId26.Value = .detail_id_26
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertWorkingHour
    '	Discription	    : Insert Working Hour
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertWorkingHour()
        Try
            ' call function set value to control
            SetValueToControl()

            'Check duplicate of working hour
            If CheckDuplicate() = False Then
                Exit Sub
            End If

            'call function from service and alert message
            If Session("job_order") <> "" Then
                If objWorkingHourSer.IsUsedInJobOrder(Session("job_order")) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTWH_02_001"))
                    Exit Sub
                End If

                'call function from service and alert message
                'Modify 2014-02-25
                'If objWorkingHourSer.IsFinishJobOrder(Session("job_order")) Then
                '    objMessage.AlertMessage(objMessage.GetXMLMessage("KTWH_02_002"))
                '    Exit Sub
                'End If
            End If

            ' call function InsertSpecialJobOrder from service and alert message
            If objWorkingHourSer.InsertWorkingHour(Session("objWorkingHourDto"), Session("dtWorkingHourDetail"), strMsg) Then
                'show success message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTWH_02_004"), Nothing, "KTWH01.aspx?New=False")
            Else
                'show error message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTWH_02_005"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertWorkingHour", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateWorkingHour
    '	Discription	    : Update Working Hour
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateWorkingHour()
        Try
            ' call function set value to control
            SetValueToControl()

            'Check duplicate of working hour
            If CheckDuplicate() = False Then
                'set disable item
                SetDisableItem()
                Exit Sub
            End If

            If Session("job_order") <> "" Then
                'call function from service and alert message
                If objWorkingHourSer.IsUsedInJobOrder(Session("job_order")) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTWH_02_001"))
                    'set disable item
                    SetDisableItem()
                    Exit Sub
                End If
                'Delete by wall
                ''call function from service and alert message
                'If objWorkingHourSer.IsFinishJobOrder(Session("job_order")) Then
                '    objMessage.AlertMessage(objMessage.GetXMLMessage("KTWH_02_002"))
                '    Exit Sub
                'End If
            End If

            ' call function UpdateWorkingHour from service and alert message
            If objWorkingHourSer.UpdateWorkingHour(Session("working_hour_id"), Session("dtWorkingHourDetail"), strMsg) Then
                'set disable item
                SetDisableItem()
                'show message 
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTWH_02_007"), Nothing, "KTWH01.aspx?New=False")
            Else
                'set disable item
                SetDisableItem()
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTWH_02_008"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateWorkingHour", ex.Message.ToString, Session("UserName"))
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
            If txtWorkingDate.Text.Trim <> "" Then
                If objIsDate.IsDate(txtWorkingDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    txtWorkingDate.Focus()
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
            ' check permission of Special Job Order menu
            objAction = objPermission.CheckPermission(23)
            ' set permission Create
            btnSave.Enabled = objAction.actCreate
            btnAddWorkingHour.Enabled = objAction.actCreate
            btnClear.Enabled = objAction.actList
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetSession
    '	Discription	    : Get session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetSession()
        Try
            ' clear session before used in this page
            Session("flagAddMod") = Nothing
            Session("flagAddMod") = "1" 'Flag check screen : 1: Menagemant screen , Nothing : Search screen

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetEnableItem
    '	Discription	    : Get enable Item
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetEnableItem()
        Try
            txtWorkingDate.Enabled = True
            ddlWorkCategory.Enabled = True
            ddlWorkStaff.Enabled = True
            pnlWorkingHourDetail.Visible = False
            btnAddWorkingHour.Visible = True

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetEnableItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDisableItem
    '	Discription	    : Get disable Item
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDisableItem()
        Try
            txtWorkingDate.Enabled = False
            ddlWorkCategory.Enabled = False
            ddlWorkStaff.Enabled = False
            btnAddWorkingHour.Visible = False

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDisableItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetJobOrder
    '	Discription	    : Keep data of job order
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetJobOrder()
        Try
            'Keep data of each record that is already checked 
            Dim strJobOrder As String = ""

            If txtJobOrder1.Text.Trim <> "" Then
                strJobOrder = txtJobOrder1.Text
            End If

            If txtJobOrder2.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder2.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder2.Text
                End If
            End If

            If txtJobOrder3.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder3.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder3.Text
                End If
            End If

            If txtJobOrder4.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder4.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder4.Text
                End If
            End If

            If txtJobOrder5.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder5.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder5.Text
                End If
            End If

            If txtJobOrder6.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder6.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder6.Text
                End If
            End If

            If txtJobOrder7.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder7.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder7.Text
                End If
            End If

            If txtJobOrder8.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder8.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder8.Text
                End If
            End If

            If txtJobOrder9.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder9.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder9.Text
                End If
            End If

            If txtJobOrder10.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder10.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder10.Text
                End If
            End If

            If txtJobOrder11.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder11.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder11.Text
                End If
            End If

            If txtJobOrder12.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder12.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder12.Text
                End If
            End If

            If txtJobOrder13.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder13.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder13.Text
                End If
            End If

            If txtJobOrder14.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder14.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder14.Text
                End If
            End If

            If txtJobOrder15.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder15.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder15.Text
                End If
            End If

            If txtJobOrder16.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder16.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder16.Text
                End If
            End If

            If txtJobOrder17.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder17.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder17.Text
                End If
            End If

            If txtJobOrder18.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder18.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder18.Text
                End If
            End If

            If txtJobOrder19.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder19.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder19.Text
                End If
            End If

            If txtJobOrder20.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder20.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder20.Text
                End If
            End If

            If txtJobOrder21.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder21.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder21.Text
                End If
            End If

            If txtJobOrder22.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder22.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder22.Text
                End If
            End If

            If txtJobOrder23.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder23.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder23.Text
                End If
            End If

            If txtJobOrder24.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder24.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder24.Text
                End If
            End If

            If txtJobOrder25.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder25.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder25.Text
                End If
            End If

            If txtJobOrder26.Text.Trim <> "" Then
                If strJobOrder.Trim = "" Then
                    strJobOrder = txtJobOrder26.Text
                Else
                    strJobOrder = strJobOrder & "," & txtJobOrder26.Text
                End If
            End If

            'Set itemConfirm into session
            Session("job_order") = strJobOrder
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetJobOrder", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: LoadListWorkCategory
    '	Discription	    : Load list WorkCategory function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-07-2013
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
            objUtility.LoadList(ddlWorkCategory, listStaffDto, "name", "id", True)

            ' set select job type from session
            If Not IsNothing(Session("ddlWorkCategory")) And ddlWorkCategory.Items.Count > 0 Then
                ddlWorkCategory.SelectedValue = Session("ddlWorkCategory")
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
    '	Create Date	    : 12-07-2013
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
            objUtility.LoadList(ddlWorkStaff, listStaffDto, "name", "id", True)

            ' set select job type from session
            If Not IsNothing(Session("ddlWorkStaff")) And ddlWorkStaff.Items.Count > 0 Then
                ddlWorkStaff.SelectedValue = Session("ddlWorkStaff")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListStaff", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToDataTable
    '	Discription	    : Keep data to datatable 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToDataTable()
        Try
            ' variable table object
            Dim dtWorkingHourDetail As New DataTable
            ' variable row object
            Dim row As DataRow

            ' set header columns
            With dtWorkingHourDetail
                .Columns.Add("job_order")
                .Columns.Add("start_time")
                .Columns.Add("end_time")
                .Columns.Add("detail")
                .Columns.Add("detail_id")
            End With

            ' loop set data to table report

            For i As Integer = 1 To 26
                row = dtWorkingHourDetail.NewRow

                Select Case i
                    Case 1
                        row.Item("job_order") = txtJobOrder1.Text
                        row.Item("start_time") = "0830"
                        row.Item("end_time") = "0900"
                        row.Item("detail") = txtDetail1.Text
                        row.Item("detail_id") = hidDetailId1.Value
                    Case 2
                        row.Item("job_order") = txtJobOrder2.Text
                        row.Item("start_time") = "0900"
                        row.Item("end_time") = "1000"
                        row.Item("detail") = txtDetail2.Text
                        row.Item("detail_id") = hidDetailId2.Value
                    Case 3
                        row.Item("job_order") = txtJobOrder3.Text
                        row.Item("start_time") = "1000"
                        row.Item("end_time") = "1100"
                        row.Item("detail") = txtDetail3.Text
                        row.Item("detail_id") = hidDetailId3.Value
                    Case 4
                        row.Item("job_order") = txtJobOrder4.Text
                        row.Item("start_time") = "1100"
                        row.Item("end_time") = "1200"
                        row.Item("detail") = txtDetail4.Text
                        row.Item("detail_id") = hidDetailId4.Value
                    Case 5
                        row.Item("job_order") = txtJobOrder5.Text
                        row.Item("start_time") = "1245"
                        row.Item("end_time") = "1300"
                        row.Item("detail") = txtDetail5.Text
                        row.Item("detail_id") = hidDetailId5.Value
                    Case 6
                        row.Item("job_order") = txtJobOrder6.Text
                        row.Item("start_time") = "1300"
                        row.Item("end_time") = "1400"
                        row.Item("detail") = txtDetail6.Text
                        row.Item("detail_id") = hidDetailId6.Value
                    Case 7
                        row.Item("job_order") = txtJobOrder7.Text
                        row.Item("start_time") = "1400"
                        row.Item("end_time") = "1500"
                        row.Item("detail") = txtDetail7.Text
                        row.Item("detail_id") = hidDetailId7.Value
                    Case 8
                        row.Item("job_order") = txtJobOrder8.Text
                        row.Item("start_time") = "1500"
                        row.Item("end_time") = "1515"
                        row.Item("detail") = txtDetail8.Text
                        row.Item("detail_id") = hidDetailId8.Value
                    Case 9
                        row.Item("job_order") = txtJobOrder9.Text
                        row.Item("start_time") = "1530"
                        row.Item("end_time") = "1600"
                        row.Item("detail") = txtDetail9.Text
                        row.Item("detail_id") = hidDetailId9.Value
                    Case 10
                        row.Item("job_order") = txtJobOrder10.Text
                        row.Item("start_time") = "1600"
                        row.Item("end_time") = "1700"
                        row.Item("detail") = txtDetail10.Text
                        row.Item("detail_id") = hidDetailId10.Value
                    Case 11
                        row.Item("job_order") = txtJobOrder11.Text
                        row.Item("start_time") = "1700"
                        row.Item("end_time") = "1730"
                        row.Item("detail") = txtDetail11.Text
                        row.Item("detail_id") = hidDetailId11.Value
                    Case 12
                        row.Item("job_order") = txtJobOrder12.Text
                        row.Item("start_time") = "1800"
                        row.Item("end_time") = "1900"
                        row.Item("detail") = txtDetail12.Text
                        row.Item("detail_id") = hidDetailId12.Value
                    Case 13
                        row.Item("job_order") = txtJobOrder13.Text
                        row.Item("start_time") = "1900"
                        row.Item("end_time") = "2000"
                        row.Item("detail") = txtDetail13.Text
                        row.Item("detail_id") = hidDetailId13.Value
                    Case 14
                        row.Item("job_order") = txtJobOrder14.Text
                        row.Item("start_time") = "2000"
                        row.Item("end_time") = "2100"
                        row.Item("detail") = txtDetail14.Text
                        row.Item("detail_id") = hidDetailId14.Value
                    Case 15
                        row.Item("job_order") = txtJobOrder15.Text
                        row.Item("start_time") = "2100"
                        row.Item("end_time") = "2200"
                        row.Item("detail") = txtDetail15.Text
                        row.Item("detail_id") = hidDetailId15.Value
                    Case 16
                        row.Item("job_order") = txtJobOrder16.Text
                        row.Item("start_time") = "2200"
                        row.Item("end_time") = "2300"
                        row.Item("detail") = txtDetail16.Text
                        row.Item("detail_id") = hidDetailId16.Value
                    Case 17
                        row.Item("job_order") = txtJobOrder17.Text
                        row.Item("start_time") = "2300"
                        row.Item("end_time") = "2400"
                        row.Item("detail") = txtDetail17.Text
                        row.Item("detail_id") = hidDetailId17.Value
                    Case 18
                        row.Item("job_order") = txtJobOrder18.Text
                        row.Item("start_time") = "0045"
                        row.Item("end_time") = "0100"
                        row.Item("detail") = txtDetail18.Text
                        row.Item("detail_id") = hidDetailId18.Value
                    Case 19
                        row.Item("job_order") = txtJobOrder19.Text
                        row.Item("start_time") = "0100"
                        row.Item("end_time") = "0200"
                        row.Item("detail") = txtDetail19.Text
                        row.Item("detail_id") = hidDetailId19.Value
                    Case 20
                        row.Item("job_order") = txtJobOrder20.Text
                        row.Item("start_time") = "0200"
                        row.Item("end_time") = "0300"
                        row.Item("detail") = txtDetail20.Text
                        row.Item("detail_id") = hidDetailId20.Value
                    Case 21
                        row.Item("job_order") = txtJobOrder21.Text
                        row.Item("start_time") = "0300"
                        row.Item("end_time") = "0315"
                        row.Item("detail") = txtDetail21.Text
                        row.Item("detail_id") = hidDetailId21.Value
                    Case 22
                        row.Item("job_order") = txtJobOrder22.Text
                        row.Item("start_time") = "0330"
                        row.Item("end_time") = "0400"
                        row.Item("detail") = txtDetail22.Text
                        row.Item("detail_id") = hidDetailId22.Value
                    Case 23
                        row.Item("job_order") = txtJobOrder23.Text
                        row.Item("start_time") = "0400"
                        row.Item("end_time") = "0500"
                        row.Item("detail") = txtDetail23.Text
                        row.Item("detail_id") = hidDetailId23.Value
                    Case 24
                        row.Item("job_order") = txtJobOrder24.Text
                        row.Item("start_time") = "0500"
                        row.Item("end_time") = "0600"
                        row.Item("detail") = txtDetail24.Text
                        row.Item("detail_id") = hidDetailId24.Value
                    Case 25
                        row.Item("job_order") = txtJobOrder25.Text
                        row.Item("start_time") = "0600"
                        row.Item("end_time") = "0700"
                        row.Item("detail") = txtDetail25.Text
                        row.Item("detail_id") = hidDetailId25.Value
                    Case 26
                        row.Item("job_order") = txtJobOrder26.Text
                        row.Item("start_time") = "0700"
                        row.Item("end_time") = "0800"
                        row.Item("detail") = txtDetail26.Text
                        row.Item("detail_id") = hidDetailId26.Value
                End Select

                dtWorkingHourDetail.Rows.Add(row)
            Next

            ' set data to session
            Session("dtWorkingHourDetail") = dtWorkingHourDetail

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDataToDataTable", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: CheckDuplicate
    '	Discription	    : Check duplicate of working hour
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 16-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckDuplicate() As Boolean
        Try
            CheckDuplicate = False
            Dim dt As DataTable
            Dim workDate As String = ""
            Dim strStartDate As String
            Dim strEndDate As String
            Dim strErrMsg As String = objMessage.GetXMLMessage("KTWH_02_009")
            dt = Session("dtWorkingHourDetail")
            Dim arrWorkDate() As String = Split(txtWorkingDate.Text.Trim(), "/")
            'Set work date to format yyymmdd
            If UBound(arrWorkDate) > 0 Then
                workDate = arrWorkDate(2) & arrWorkDate(1) & arrWorkDate(0)
            End If

            For i As Integer = 0 To dt.Rows.Count - 1
                If Not String.IsNullOrEmpty(dt.Rows(i)("job_order").ToString()) Then
                    'Check duplicate working hour
                    If Session("Mode") = "Add" Then
                        If objWorkingHourSer.IsUseWorkingHour(workDate, _
                                                            ddlWorkStaff.SelectedValue, _
                                                            dt.Rows(i)("start_time").ToString(), _
                                                            dt.Rows(i)("end_time").ToString()) = False Then
                            'show error msg 
                            strStartDate = Left(dt.Rows(i)("start_time").ToString(), 2) & ":" & Right(dt.Rows(i)("start_time").ToString(), 2)
                            strEndDate = Left(dt.Rows(i)("end_time").ToString(), 2) & ":" & Right(dt.Rows(i)("end_time").ToString(), 2)

                            strErrMsg = strErrMsg.Replace("%STARTTIME%", strStartDate)
                            strErrMsg = strErrMsg.Replace("%ENDTIME%", strEndDate)
                            strErrMsg = strErrMsg.Replace("%JOBORDER%", dt.Rows(i)("job_order").ToString())
                            objMessage.AlertMessage(strErrMsg)
                            Exit Function
                        End If
                    Else
                        If objWorkingHourSer.IsUseWorkingHour(workDate, _
                                                            ddlWorkStaff.SelectedValue, _
                                                            dt.Rows(i)("start_time").ToString(), _
                                                            dt.Rows(i)("end_time").ToString(), _
                                                            dt.Rows(i)("detail_id").ToString()) = False Then
                            'show error msg 
                            strStartDate = Left(dt.Rows(i)("start_time").ToString(), 2) & ":" & Right(dt.Rows(i)("start_time").ToString(), 2)
                            strEndDate = Left(dt.Rows(i)("end_time").ToString(), 2) & ":" & Right(dt.Rows(i)("end_time").ToString(), 2)

                            strErrMsg = strErrMsg.Replace("%STARTTIME%", strStartDate)
                            strErrMsg = strErrMsg.Replace("%ENDTIME%", strEndDate)
                            strErrMsg = strErrMsg.Replace("%JOBORDER%", dt.Rows(i)("job_order").ToString())
                            objMessage.AlertMessage(strErrMsg)
                            Exit Function
                        End If
                    End If
                End If
            Next
 
            CheckDuplicate = True

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckDuplicate", ex.Message.ToString, Session("UserName"))
        End Try
    End Function
 
#End Region

End Class