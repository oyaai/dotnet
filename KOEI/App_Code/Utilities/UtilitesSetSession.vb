#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Common.Utilities
'	Class Name		    : 
'	Class Discription	: Utilities Class
'	Create User 		: Suwishaya L.
'	Create Date		    : 13/08/2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
' INDEX FUNCTIONS
'   ' Execute Method by String
'   Shared Sub InvokeMethod(String) Return Method's Results
'   
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Utilities
    Public Class SetSession
        Private csUser As New Service.ImpUserService
        Private objLog As New Common.Logs.Log

#Region "Function"

        '/**************************************************************
        '	Function name	: SetSession
        '	Discription	    : Set session value
        '	Return Value	: Nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 20-05-2013
        '	Update User	    : Suwishaya L.
        '	Update Date	    : 13-08-2013
        '*************************************************************/
        Public Sub SetSession()
            Try
                ' Dto variable for keep user data
                Dim cdUser As New Dto.UserDto
                ' call GetUserLogin from user service
                cdUser = csUser.GetUserLogin("", "", HttpContext.Current.Session("UserID"))
                ' check exist value and assign
                If cdUser.UserID <> String.Empty Then
                    ' Description: Keep UserDto in Session
                    HttpContext.Current.Session("User") = cdUser
                    HttpContext.Current.Session("UserID") = cdUser.UserID
                    HttpContext.Current.Session("UserName") = cdUser.UserName
                    HttpContext.Current.Session("DepartmentName") = cdUser.DepartmentName
                    HttpContext.Current.Session("AccountNextApprove") = cdUser.ACNextApprove
                    HttpContext.Current.Session("PurchaseNextApprove") = cdUser.PUNextApprove
                    HttpContext.Current.Session("OutSourceNextApprove") = cdUser.OSNextApprove

                    csUser.UpdateLastLogin(cdUser.UserID)
                    ' get user permission
                    LoadPermission(cdUser.UserID)
                    ' get menu
                    LoadMenu(cdUser.UserID)
                End If
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("SetSession", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: LoadMenu
        '	Discription	    : Load menu
        '	Return Value	: Nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 21-05-2013
        '	Update User	    : Suwishaya L.
        '	Update Date	    : 13-08-2013
        '*************************************************************/
        Private Sub LoadMenu(ByVal intUserID As Integer)
            Try
                ' variable service menu object
                Dim csMenu As New Service.ImpMenuService
                ' variable list
                Dim listMenu As New List(Of Dto.MenuDto)

                ' list menu 
                listMenu = csMenu.GetMenuList(intUserID)

                ' assign list to session
                HttpContext.Current.Session("ListMenu") = listMenu
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("LoadMenu", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: LoadPermission
        '	Discription	    : Load permission
        '	Return Value	: Nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 21-05-2013
        '	Update User	    : Suwishaya L.
        '	Update Date	    : 13-08-2013
        '*************************************************************/
        Private Sub LoadPermission(ByVal intUserID As Integer)
            Try
                ' variable keep list of permission
                Dim listUserPermission As New List(Of Common.UserPermissions.UserPermissionDto)
                ' call service to get user permission
                listUserPermission = csUser.GetUserPermission(intUserID)
                ' keep to session
                HttpContext.Current.Session("ListPermission") = listUserPermission
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("LoadPermission", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Sub
#End Region
    End Class
End Namespace
