Imports System.Data
Imports System.Web.Configuration

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : KTMS21
'	Class Discription	: Webpage for Country master
'	Create User 		: Suwishaya L.
'	Create Date		    : 04-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class Master_KTMS21
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objCountrySer As New Service.ImpCountryService
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
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS21:Country Master")
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
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ' check postback page
            If Not IsPostBack Then
                ' case not post back then call function initialpage
                InitialPage()
                'set initialpage when back from magement screen
                InitialData()
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Load", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	    : Even on btnAdd_Click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            'set data search into Session
            Session("txtCountryNameSearch") = txtCountryName.Text.Trim
            ' redirect to KTMS22 with Add mode
            Response.Redirect("KTMS22.aspx?Mode=Add")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSearch_Click
    '	Discription	    : Even on btnSearch_Click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            ' call function search data
            SearchData()
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            Session("txtCountryName") = txtCountryName.Text.Trim
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptCountry_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptCountry_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptCountry.DataBinding
        Try
            ' clear hashtable data
            hashCountryID.Clear()
            'hashInUsed.Clear()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptCountry_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptCountry_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptCountry_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptCountry.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intCountryID As Integer = CInt(hashCountryID(e.Item.ItemIndex).ToString())
            Dim boolInuse As Boolean = objCountrySer.IsUsedInVendor(intCountryID)

            ' set ItemID to session
            Session("intCountryID") = intCountryID
            Session("boolInuse") = boolInuse

            Select Case e.CommandName
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTMS21", strResult, objMessage.GetXMLMessage("KTMS_21_001"))

                Case "Edit"
                    'set data search into Session
                    Session("txtCountryNameSearch") = txtCountryName.Text.Trim
                    ' redirect to KTMS04
                    Response.Redirect("KTMS21.aspx?strEdit=True")
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptCountry_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptCountry_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptCountry_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptCountry.ItemDataBound
        Try
            ' object link button
            Dim btnDel As New LinkButton
            Dim btnEdit As New LinkButton

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)

            ' set permission on button
            If Not Session("actUpdate") Then
                btnEdit.CssClass = "icon_edit2 icon_center15"
                btnEdit.Enabled = False
            End If

            If Not Session("actDelete") Then
                btnDel.CssClass = "icon_del2 icon_center15"
                btnDel.Enabled = False
            End If 

            ' Set ItemID to hashtable
            hashCountryID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Country_id"))
           
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptCountry_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    ' Stores the Item_ID keys in ViewState
    ReadOnly Property hashCountryID() As Hashtable
        Get
            If IsNothing(ViewState("hashCountryID")) Then
                ViewState("hashCountryID") = New Hashtable()
            End If
            Return CType(ViewState("hashCountryID"), Hashtable)
        End Get
    End Property

#End Region

#Region "Function"

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
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

            ' set value to txtCountryName from session
            txtCountryName.Text = Session("txtCountryName")

            ' call function check permission
            CheckPermission()

            ' check delete item
            If objUtility.GetQueryString(strResult) = "True" Then
                DeleteCountry()
            End If

            ' check Edit item
            If Request.QueryString("strEdit") = "True" Then
                EditCountry()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Country menu
            objAction = objPermission.CheckPermission(35)
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
    '	Discription	    : Search Item data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtCountry As New DataTable

            ' call function GetCountryList from CountryService
            dtCountry = objCountrySer.GetCountryList(txtCountryName.Text.Trim)
            ' set table object to session
            Session("dtCountry") = dtCountry
            'set data search into Session
            Session("txtCountryNameSearch") = txtCountryName.Text.Trim

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtCountry As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtCountry = Session("dtCountry")

            ' check record for display
            If Not IsNothing(dtCountry) AndAlso dtCountry.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtCountry)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptCountry.DataSource = pagedData
                rptCountry.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtCountry.Rows.Count)
            Else
                ' case not exist data
                ' show message box
                If Session("flagAddMod") = "" Or Session("flagAddMod") = Nothing Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
                End If

                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptCountry.DataSource = Nothing
                rptCountry.DataBind()
                End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteCountry
    '	Discription	    : Delete Country data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteCountry()
        Try
            ' check flag in_used
            If Session("boolInuse") Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_21_002"))
                Exit Sub
            End If

            ' check state of delete Country
            If objCountrySer.DeleteCountry(Session("intCountryID")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_21_003"))
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_21_004"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteCountry", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: EditCountry
    '	Discription	    : Edit Country data
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 05-09-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub EditCountry()
        Try
            ' check flag in_used
            If Session("boolInuse") Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_21_007"))
            Else
                ' case else redirect manage page
                Response.Redirect("KTMS22.aspx?Mode=Edit&id=" & Session("intCountryID"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteCountry", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtCountry") = Nothing
            Session("txtCountryName") = Nothing
            Session("intCountryID") = Nothing
            Session("boolInuse") = Nothing
            Session("actUpdate") = Nothing
            Session("actDelete") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InitialData
    '	Discription	    : Initial data function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialData()
        Try

            If Session("flagAddMod") = "1" Then
                ' set value to txtCountryName from session
                txtCountryName.Text = Session("txtCountryNameSearch")
                ' call function search data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))

                ' set search text to session
                Session("txtCountryName") = txtCountryName.Text.Trim
                ' call function check permission
                CheckPermission()

                'Clear session
                Session("flagAddMod") = Nothing
                Session("txtCountryNameSearch") = Nothing
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

End Class
