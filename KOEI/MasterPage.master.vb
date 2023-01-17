Imports System.Web.Services

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : 
'	Class Name		    : MasterPage
'	Class Discription	: Webpage for master page
'	Create User 		: Komsan Luecha
'	Create Date		    : 22-05-2013
'
' UPDATE INFORMATION
'	Update User		: Lin
'	Update Date		: 24-06-2013
'   Description     : Add event log out
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    Private cLog As New Common.Logs.Log
    Private objMessage As New Common.Utilities.Message
    Private objComm As New Common.Utilities.Utility

    Protected htmlMenu As String = ""
    Private Const strResult As String = "Result"

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Load
        Try
            ' check session time out
            If IsNothing(Session("UserName")) AndAlso Session("UserName") = String.Empty Then
                ' case time out or nothing user
                Response.Redirect("../Overview/KTOV01.aspx")
            End If

            ' check case first visit
            If IsNothing(Session("HtmlMenu")) Then
                ' call function write html menu
                WriteHtmlMenu()
            Else
                ' case not case first visit ⇒ assign html from session
                htmlMenu = Session("HtmlMenu")
            End If


            ' Write user login information
            lblUsername.Text = Session("UserName") & ""
            lblDepartment.Text = Session("DepartmentName") & ""
            MainContent.Focus()
        Catch ex As Exception
            ' write error log
            cLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnLogout_Click
    '	Discription	    : Event log out click
    '	Return Value	: nothing
    '	Create User	    : Lin
    '	Create Date	    : 18-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        Try
            'Go to log in page
            objMessage.ConfirmMessage("../Overview/KTOV01", strResult, objMessage.GetXMLMessage("KTOV_01_004"))
        Catch ex As Exception
            ' write error log
            cLog.ErrorLog("Logout_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: WriteHtmlMenu
    '	Discription	    : Write html menu
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub WriteHtmlMenu()
        Try
            ' variable keep list of menu
            Dim listMenu As New List(Of Dto.MenuDto)
            ' variable check menu id
            Dim intOldID As Integer
            ' variable check index
            Dim intIndex As Integer = 1

            ' assign menu list from session
            listMenu = Session("ListMenu")

            ' loop for write html
            For Each values In listMenu
                ' case same menu id
                If intOldID = values.category_id Then
                    ' write sub menu
                    htmlMenu &= "       <li><a href='" & values.navigate_url & "'>" & values.menu_text & "</a></li>"
                    'htmlMenu &= "       <li><asp:LinkButton runat='server' id='btn" & Mid(values.navigate_url, values.navigate_url.Length - 10, 6) & "'>" & values.menu_text & "</asp:LinkButton></li>"

                Else
                    ' case new menu id
                    If intIndex <> 1 Then
                        ' case not first row
                        ' write close tag
                        htmlMenu &= "</ul></li>"
                    End If

                    ' case new menu id and not first row
                    ' write main menu and sub menu
                    htmlMenu &= "<li><a href='#'>" & values.category_name & "</a>"
                    htmlMenu &= "   <ul>"
                    '' check category
                    'If values.category_id = 2 Then
                    '    ' case category = 8 set class master
                    '    htmlMenu &= "   <ul class='master'>"
                    'ElseIf values.category_id = 9 Then
                    '    ' case category = 9 set class admin
                    '    htmlMenu &= "   <ul class='admin'>"
                    'Else
                    'End If

                    htmlMenu &= "       <li><a href='" & values.navigate_url & "'>" & values.menu_text & "</a></li>"
                End If
                ' increase index
                intIndex += 1
                ' assign menu id
                intOldID = values.category_id
            Next
            ' assign html to session
            Session("HtmlMenu") = htmlMenu

        Catch ex As Exception
            ' write error log
            cLog.ErrorLog("WriteHtmlMenu", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    ' Test check in
#End Region

End Class

