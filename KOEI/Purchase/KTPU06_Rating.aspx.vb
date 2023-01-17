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
'	Package Name	    : Rating Purchase
'	Class Name		    : KTPU06_Rating
'	Class Discription	: Searching data of Rating Purchase
'	Create User 		: Pranitda Sroengklang
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
Partial Class KTPU06_Rating
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private itemConfirm As String = ""
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private strMsg As String = String.Empty

    'connect with service
    Private objRatingPurchaseService As New Service.ImpRating_PurchaseService

#Region "Event"
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
            objLog.StartLog("KTPU06_Rating", Session("UserName"))
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
    '	Create Date	    : 12-07-2013
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
    '	Function name	: btnCancel_Click
    '	Discription	    : back to previous screen
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 15-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnCancel_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnCancel.Click
        Try
            If Session("Mode") = "Add" Then
                Response.Redirect("KTPU06.aspx?New=''")
            ElseIf Session("Mode") = "Edit" Then
                Response.Redirect("KTPU05.aspx?New=''")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnCancel_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnCreate_Click
    '	Discription	    : insert/update data into vendor_rating table
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnCreate_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnCreate.Click
        Try
            Session("rblQuality") = rblQuality.SelectedValue
            Session("rblDelivery") = rblDelivery.SelectedValue
            Session("rblService") = rblService.SelectedValue

            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTPU06_Rating", strConfirmIns, objMessage.GetXMLMessage("KTPU_06_002"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTPU06_Rating", strConfirmUpd, objMessage.GetXMLMessage("KTPU_06_005"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnCreate_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            ' check insert Working Category
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                InsUpdVendor_Rating() 'insert
                Exit Sub
            End If

            ' check update Working Category
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                InsUpdVendor_Rating() 'update
                Exit Sub
            End If

            ' check case new enter
            If objUtility.GetQueryString("Mode") = "Add" Then
                Session("payment_header_id") = objUtility.GetQueryString("id")
                btnCreate.Text = "Create"
                Session("Mode") = "Add"
            Else
                Session("editID") = objUtility.GetQueryString("id")
                'Get rating vendor
                GetRatingVendor(Session("editID"))
                SetRadioButton()
                btnCreate.Text = "Update"
                Session("Mode") = "Edit"
            End If

            ' call function check permission
            CheckPermission()


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsUpdVendor_Rating
    '	Discription	    : Insert/Update Vendor_Rating
    '	Parameter   	: 1:insert ,2:update
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 15-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsUpdVendor_Rating()
        Try
            'check duplicate
            If Session("Mode") = "Add" Then 'Add
                If objRatingPurchaseService.CheckDupVendor_Rating("", Session("payment_header_id"), strMsg) = True And strMsg = "" Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_06_001"), Nothing, "KTPU06.aspx?New=Insert")
                    Exit Sub
                ElseIf strMsg <> "" Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                    Exit Sub
                End If
            Else 'Modify
                If objRatingPurchaseService.CheckDupVendor_Rating(Session("editID"), "", strMsg) = False And strMsg = "" Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_06_008"), Nothing, "KTPU05.aspx?New=")
                    Exit Sub
                ElseIf strMsg <> "" Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                    Exit Sub
                End If
            End If

            ' call function InsertWorkingCategory from service and alert message
            If objRatingPurchaseService.InsUpdVendor_Rating(Session("Mode"), _
                                                            Session("editID"), _
                                                            Session("payment_header_id"), _
                                                            Session("rblQuality"), _
                                                            Session("rblDelivery"), _
                                                            Session("rblService"), _
                                                            strMsg) = True Then

                If Session("Mode") = "Add" Then 'Add
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_06_003"), Nothing, "KTPU06.aspx?New=Insert")
                Else 'Modify
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_06_006"), Nothing, "KTPU05.aspx?New=Update")
                End If
            Else
                If Session("Mode") = "Add" Then 'Add
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_06_004"), Nothing)
                Else 'Modify
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_06_007"), Nothing)
                End If

            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertWorkingCategory", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: GetRatingVendor
    '	Discription	    : Get Rating Vendor
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetRatingVendor(ByVal ratingId As String)
        Try
            ' table object keep value from item service
            Dim dtGetRatingVendor As New DataTable

            ' call function GetItemList from ItemService
            dtGetRatingVendor = objRatingPurchaseService.GetRatingVendor(ratingId, "")
            ' set table object to session
            Session("dtGetRatingVendor") = dtGetRatingVendor
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SetRadioButton
    '	Discription	    : set data into radio button
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetRadioButton()
        Try
            ' table object keep value from item service
            Dim dtGetRatingVendor As New DataTable

            dtGetRatingVendor = Session("dtGetRatingVendor")

            If Not IsNothing(dtGetRatingVendor) AndAlso dtGetRatingVendor.Rows.Count > 0 Then
                'set default of quality
                Select Case dtGetRatingVendor.Rows(0)("quality").ToString()
                    Case "50"
                        rblQuality.SelectedIndex = 0
                    Case "25"
                        rblQuality.SelectedIndex = 1
                    Case "0"
                        rblQuality.SelectedIndex = 2
                    Case Else
                End Select

                'set default of delivery
                Select Case dtGetRatingVendor.Rows(0)("delivery").ToString()
                    Case "30"
                        rblDelivery.SelectedIndex = 0
                    Case "15"
                        rblDelivery.SelectedIndex = 1
                    Case "0"
                        rblDelivery.SelectedIndex = 2
                    Case Else
                End Select

                'set default of Service
                Select Case dtGetRatingVendor.Rows(0)("service").ToString()
                    Case "20"
                        rblService.SelectedIndex = 0
                    Case "10"
                        rblService.SelectedIndex = 1
                    Case "0"
                        rblService.SelectedIndex = 2
                    Case Else
                End Select

            Else ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(9)

            ' set permission 
            btnCreate.Enabled = objAction.actCreate

            ' set action permission to session
            Session("actList") = objAction.actList

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

End Class
