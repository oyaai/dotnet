#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpUserService
'	Class Discription	: Implement user Service
'	Create User 		: Komsan L.
'	Create Date		    : 20-05-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region
Imports Microsoft.VisualBasic

Namespace Service
    Public Class ImpUserService
        Implements Service.IUserService

        Private cLog As New Common.Logs.Log
        Private ceUser As New Entity.ImpUserEntity


#Region "Function"

        '/**************************************************************
        '	Function name	: GetUserLogin
        '	Discription	    : Get user login
        '	Return Value	: User data
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserLogin( _
            ByVal strUserName As String, _
            ByVal strPassword As String, _
          Optional ByVal strUserId As String = "" _
        ) As Dto.UserDto Implements Service.IUserService.GetUserLogin
            ' new user Dto
            GetUserLogin = New Dto.UserDto
            Try
                ' variable for keep value of user
                Dim eUser As New Entity.ImpUserEntity
                ' call GetUserLogin from user entity
                eUser = ceUser.GetUserLogin(strUserName, strPassword, strUserId)

                ' check exist data
                If eUser.user_name <> String.Empty Then
                    ' assign value to user Dto
                    With GetUserLogin
                        .UserID = eUser.id
                        .UserName = eUser.user_name
                        .DepartmentName = eUser.department_name
                        .ACNextApprove = eUser.account_next_approve
                        .PUNextApprove = eUser.purchase_next_approve
                        .OSNextApprove = eUser.outsource_next_approve
                    End With
                End If
            Catch ex As Exception
                ' Write error log
                cLog.ErrorLog("GetUserLogin(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateLastLogin
        '	Discription	    : Update last login
        '	Return Value	: True , False
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateLastLogin( _
            ByVal intUserID As Integer _
        ) As Boolean Implements IUserService.UpdateLastLogin
            Try
                ' variable keep row effect
                Dim intEff As Integer
                ' call fuction UpdateLastLogin
                intEff = ceUser.UpdateLastLogin(intUserID)

                ' check updated effect
                If intEff > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("UpdateLastLogin(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserPermission
        '	Discription	    : Get user permission
        '	Return Value	: List
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 21-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserPermission( _
            ByVal intUserID As Integer _
        ) As System.Collections.Generic.List(Of Common.UserPermissions.UserPermissionDto) Implements IUserService.GetUserPermission
            ' new user permission Dto
            GetUserPermission = New List(Of Common.UserPermissions.UserPermissionDto)
            Try
                ' variable for keep value list of user permission 
                Dim eUserPermission As New List(Of Entity.IUser_PermissionEntity)
                ' variable for keep value of user permission (dto)
                Dim cdUserPermision As Common.UserPermissions.UserPermissionDto
                Dim ceUserPermission As New Entity.ImpUser_PermissionEntity

                ' call GetUserLogin from user entity
                eUserPermission = ceUserPermission.GetUserPermission(intUserID)

                ' loop assign value to list dto
                For Each values In eUserPermission
                    cdUserPermision = New Common.UserPermissions.UserPermissionDto
                    With cdUserPermision
                        .UserID = values.user_id
                        .MenuID = values.menu_id
                        .Fn_Create = values.fn_create
                        .Fn_Update = values.fn_update
                        .Fn_Delete = values.fn_delete
                        .Fn_List = values.fn_list
                        .Fn_Confirm = values.fn_confirm
                        .Fn_Approve = values.fn_approve
                        .Fn_Amount = values.fn_amount
                    End With
                    ' add values to list dto
                    GetUserPermission.Add(cdUserPermision)
                Next
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetUserPermission(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserPermission
        '	Discription	    : Get user permission
        '	Return Value	: List
        '	Create User	    : Charoon
        '	Create Date	    : 30-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserPermission(ByVal intUserID As Integer, ByVal intMenuID As Integer) As System.Collections.Generic.List(Of Dto.UserPermissionDto) Implements IUserService.GetUserPermission
            ' new user permission Dto
            GetUserPermission = New List(Of Dto.UserPermissionDto)
            Try
                ' variable for keep value list of user permission 
                Dim eUserPermission As New List(Of Entity.IUser_PermissionEntity)
                ' variable for keep value of user permission (dto)
                Dim cdUserPermision As Dto.UserPermissionDto
                Dim ceUserPermission As New Entity.ImpUser_PermissionEntity

                ' call GetUserLogin from user entity
                eUserPermission = ceUserPermission.GetUserPermission(intUserID, intMenuID)

                ' loop assign value to list dto
                For Each values In eUserPermission
                    cdUserPermision = New Dto.UserPermissionDto
                    With cdUserPermision
                        .UserID = values.user_id
                        .MenuID = values.menu_id
                        .Fn_Create = values.fn_create
                        .Fn_Update = values.fn_update
                        .Fn_Delete = values.fn_delete
                        .Fn_List = values.fn_list
                        .Fn_Confirm = values.fn_confirm
                        .Fn_Approve = values.fn_approve
                        .Fn_Amount = values.fn_amount
                    End With
                    ' add values to list dto
                    GetUserPermission.Add(cdUserPermision)
                Next
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetUserPermission(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserForList
        '	Discription	    : Get user for dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserForList(Optional ByVal strDepartmentName As String = Nothing) As System.Collections.Generic.List(Of Dto.UserDto) Implements IUserService.GetUserForList
            ' set default list
            GetUserForList = New List(Of Dto.UserDto)
            Try
                ' objUserDto for keep value Dto 
                Dim objUserDto As Dto.UserDto
                ' listUserEnt for keep value from entity
                Dim listUserEnt As New List(Of Entity.IUserEntity)
                ' objUserEnt for call function
                Dim objUserEnt As New Entity.ImpUserEntity

                ' call function GetUserForList
                listUserEnt = objUserEnt.GetUserForList(strDepartmentName)

                ' loop listUserEnt for assign value to Dto
                For Each values In listUserEnt
                    ' new object
                    objUserDto = New Dto.UserDto
                    ' assign value to Dto
                    With objUserDto
                        .UserID = values.id
                        .UserName = values.user_name
                    End With
                    ' add object Dto to list Dto
                    GetUserForList.Add(objUserDto)
                Next
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetUserForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region

    End Class
End Namespace

