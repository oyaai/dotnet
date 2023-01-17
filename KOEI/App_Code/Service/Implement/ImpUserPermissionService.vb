Imports Microsoft.VisualBasic
Imports System.Data
Imports MySql.Data.MySqlClient

Namespace Service
    Public Class ImpUserPermissionService
        Implements IUserPermissionService

        Private objLog As New Common.Logs.Log
        Private objUtility As New Common.Utilities.Utility
        Private objUserPermissionEnt As New Entity.ImpUser_PermissionEntity

        '/**************************************************************
        '	Function name	: GetUserPermissionList
        '	Discription	    : Get User Permission list
        '	Return Value	: List
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserPermissionList(ByVal strFName As String, ByVal strLName As String, ByVal strUName As String, ByVal intDepartment As String) As System.Data.DataTable Implements IUserPermissionService.GetUserPermissionList
            ' ser default
            GetUserPermissionList = New DataTable
            Try
                ' variable for keep list from UserPermission entity
                Dim listUserPerEnt As New List(Of Entity.ImpUserEntity)
                ' data row object
                Dim row As DataRow
                Dim tmp As New StringBuilder

                ' call function GetItemList from entity
                listUserPerEnt = objUserPermissionEnt.GetUserPermissionList(strFName, strLName, strUName, intDepartment)

                ' assign column header
                With GetUserPermissionList
                    .Columns.Add("id")
                    .Columns.Add("first_name")
                    .Columns.Add("last_name")
                    .Columns.Add("user_name")
                    .Columns.Add("department")
                    .Columns.Add("last_login")

                    ' assign row from listItemEny
                    For Each values In listUserPerEnt
                        tmp.Length = 0
                        row = .NewRow
                        row("id") = values.id
                        row("first_name") = values.first_name
                        row("last_name") = values.last_name
                        row("user_name") = values.user_name
                        row("department") = values.department_name
                        If IsNothing(values.last_login) Then
                            row("last_login") = values.last_login
                        Else
                            row("last_login") = objUtility.String2Date(values.last_login, "yyyyMMddHHmmss").ToString("dd/MMM/yyyy HH:mm:ss")
                        End If
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetUserPermissionList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserSetPermissionList
        '	Discription	    : Get User already set Permission list
        '	Return Value	: List
        '	Create User	    : Wasan D.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserSetPermissionList() As System.Collections.Generic.List(Of Dto.UserDto) Implements IUserPermissionService.GetUserSetPermissionList
            ' ser default
            GetUserSetPermissionList = New List(Of Dto.UserDto)
            Try

                ' objVendorDto for keep value Dto 
                Dim objUserDto As Dto.UserDto
                ' listVendorEnt for keep value from entity
                Dim listUserEnt As New List(Of Entity.ImpUserEntity)
                ' objVendorEnt for call function
                Dim objUserEnt As New Entity.ImpUser_PermissionEntity

                ' call function GetVendorForList
                listUserEnt = objUserEnt.GetUserSetPermissionList

                ' loop listVendorEnt for assign value to Dto
                For Each values In listUserEnt
                    ' new object
                    objUserDto = New Dto.UserDto
                    ' assign value to Dto
                    With objUserDto
                        .UserID = values.id
                        .UserName = values.user_name
                    End With
                    ' add object Dto to list Dto
                    GetUserSetPermissionList.Add(objUserDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetUserSetPermissionList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserNonSetPermissionList
        '	Discription	    : Get User not set Permission list
        '	Return Value	: List
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserNonSetPermissionList() As System.Collections.Generic.List(Of Dto.UserDto) Implements IUserPermissionService.GetUserNonSetPermissionList
            ' ser default
            GetUserNonSetPermissionList = New List(Of Dto.UserDto)
            Try

                ' objVendorDto for keep value Dto 
                Dim objUserDto As Dto.UserDto
                ' listVendorEnt for keep value from entity
                Dim listUserEnt As New List(Of Entity.ImpUserEntity)
                ' objVendorEnt for call function
                Dim objUserEnt As New Entity.ImpUser_PermissionEntity

                ' call function GetVendorForList
                listUserEnt = objUserEnt.GetUserNonSetPermissionList

                ' loop listVendorEnt for assign value to Dto
                For Each values In listUserEnt
                    ' new object
                    objUserDto = New Dto.UserDto
                    ' assign value to Dto
                    With objUserDto
                        .UserID = values.id
                        .UserName = values.user_name
                    End With
                    ' add object Dto to list Dto
                    GetUserNonSetPermissionList.Add(objUserDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetUserNonSetPermissionList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPermissionList
        '	Discription	    : Get Permission list
        '	Return Value	: List
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPermissionList() As System.Data.DataTable Implements IUserPermissionService.GetPermissionList
            ' ser default
            GetPermissionList = New DataTable
            Try

                ' variable for keep list from UserPermission entity
                Dim listPMEnt As New List(Of Entity.ImpUser_PermissionEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetItemList from entity
                listPMEnt = objUserPermissionEnt.GetPermissionList

                ' assign column header
                With GetPermissionList
                    .Columns.Add("id")
                    .Columns.Add("user_id")
                    .Columns.Add("menu_id")
                    .Columns.Add("menu_text")
                    .Columns.Add("Fn_Create")
                    .Columns.Add("Fn_Update")
                    .Columns.Add("Fn_Delete")
                    .Columns.Add("Fn_List")
                    .Columns.Add("Fn_Confirm")
                    .Columns.Add("Fn_Approve")
                    .Columns.Add("Fn_Amount")

                    ' assign row from listItemEny
                    For Each values In listPMEnt
                        row = .NewRow
                        row("id") = values.id
                        row("user_id") = values.user_id
                        row("menu_id") = values.menu_id
                        row("menu_text") = values.menu_text
                        row("Fn_Create") = values.fn_create
                        row("Fn_Update") = values.fn_update
                        row("Fn_Delete") = values.fn_delete
                        row("Fn_List") = values.fn_list
                        row("Fn_Confirm") = values.fn_confirm
                        row("Fn_Approve") = values.fn_approve
                        row("Fn_Amount") = values.fn_amount
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPermissionList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserDetail
        '	Discription	    : Get User datail list
        '	Return Value	: List
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserDetail(ByVal strUserName As String) As System.Data.DataTable Implements IUserPermissionService.GetUserDetail
            ' ser default
            GetUserDetail = New DataTable
            Try

                ' variable for keep list from UserPermission entity
                Dim listUserEnt As New List(Of Entity.ImpUserEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetItemList from entity
                listUserEnt = objUserPermissionEnt.GetUserDetail(strUserName)

                ' assign column header
                With GetUserDetail
                    .Columns.Add("first_name")
                    .Columns.Add("last_name")
                    .Columns.Add("department")

                    ' assign row from listItemEny
                    For Each values In listUserEnt
                        row = .NewRow
                        row("first_name") = values.first_name
                        row("last_name") = values.last_name
                        row("department") = values.department_name
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetUserDetail(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserPermissionSettingList
        '	Discription	    : Get User Permission datail list
        '	Return Value	: List
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserPermissionSettingList(ByVal strUserID As String) As System.Data.DataTable Implements IUserPermissionService.GetUserPermissionSettingList
            ' ser default
            GetUserPermissionSettingList = New DataTable
            Try
                ' variable for keep list from UserPermission entity
                Dim listUserEnt As New List(Of Entity.ImpUser_PermissionEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetItemList from entity
                listUserEnt = objUserPermissionEnt.GetUserPermissionSettingList(strUserID)

                ' assign column header
                With GetUserPermissionSettingList
                    .Columns.Add("id")
                    .Columns.Add("user_id")
                    .Columns.Add("menu_id")
                    .Columns.Add("menu_text")
                    .Columns.Add("Fn_Create")
                    .Columns.Add("Fn_Update")
                    .Columns.Add("Fn_Delete")
                    .Columns.Add("Fn_List")
                    .Columns.Add("Fn_Confirm")
                    .Columns.Add("Fn_Approve")
                    .Columns.Add("Fn_Amount")

                    ' assign row from listItemEny
                    For Each values In listUserEnt
                        row = .NewRow
                        row("id") = values.id
                        row("user_id") = IIf(values.user_id = 0, strUserID, values.user_id)
                        row("menu_id") = values.menu_id
                        row("menu_text") = values.menu_text
                        row("Fn_Create") = values.fn_create
                        row("Fn_Update") = values.fn_update
                        row("Fn_Delete") = values.fn_delete
                        row("Fn_List") = values.fn_list
                        row("Fn_Confirm") = values.fn_confirm
                        row("Fn_Approve") = values.fn_approve
                        row("Fn_Amount") = values.fn_amount
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetUserPermissionSettingList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertUserPermission
        '	Discription	    : Get User Permission datail list
        '	Return Value	: List
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/

        Public Function InsertUserPermission(ByVal listUserPDto As System.Collections.Generic.List(Of Dto.UserPermissionDto), ByVal IntUserID As String, Optional ByRef strMsg As String = "") As Boolean Implements IUserPermissionService.InsertUserPermission
            ' set default return value
            InsertUserPermission = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                If intEff = True Then
                    ' case Duplicate data
                    strMsg = "KTMS_26_007"
                    Exit Function
                End If

                ' call function UpdateItem from Payment Condition Entity
                intEff = objUserPermissionEnt.InsertUserPermission(SetDtoToEntityList(listUserPDto), IntUserID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertUserPermission = True
                Else
                    ' case row less than 1 then return False
                    InsertUserPermission = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_26_007"
                Else
                    ' other case
                    strMsg = "KTMS_26_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertUserPermission(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateUserPermission
        '	Discription	    : Update User Permission datail list
        '	Return Value	: List
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/

        Public Function UpdateUserPermission(ByVal listUserPDto As System.Collections.Generic.List(Of Dto.UserPermissionDto), Optional ByRef strMsg As String = "") As Boolean Implements IUserPermissionService.UpdateUserPermission
            ' set default return value
            UpdateUserPermission = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' call function UpdateUserPermission from User Permission Entity
                intEff = objUserPermissionEnt.UpdateUserPermission(SetDtoToEntityList(listUserPDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateUserPermission = True
                Else
                    ' case row less than 1 then return False
                    UpdateUserPermission = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                strMsg = "KTAD_04_006"
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateUserPermission(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntityList
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Wasan D.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntityList( _
            ByVal objUPDtoList As List(Of Dto.UserPermissionDto) _
        ) As List(Of Entity.ImpUser_PermissionEntity)
            ' set default return value
            SetDtoToEntityList = New List(Of Entity.ImpUser_PermissionEntity)
            Dim SetDtoToEntity As Entity.ImpUser_PermissionEntity

            Try
                For Each objUPDto In objUPDtoList
                    SetDtoToEntity = New Entity.ImpUser_PermissionEntity
                    ' assign value to entity
                    With SetDtoToEntity
                        .id = objUPDto.id
                        .fn_create = objUPDto.Fn_Create
                        .fn_update = objUPDto.Fn_Update
                        .fn_delete = objUPDto.Fn_Delete
                        .fn_list = objUPDto.Fn_List
                        .fn_confirm = objUPDto.Fn_Confirm
                        .fn_approve = objUPDto.Fn_Approve
                        .fn_amount = objUPDto.Fn_Amount
                        .user_id = objUPDto.UserID
                        .menu_id = objUPDto.MenuID
                    End With
                    ' Set Dto to List of Dto
                    SetDtoToEntityList.Add(SetDtoToEntity)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntityList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntityList
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Wasan D.
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteUserPermission(ByVal intUserID As String) As Boolean Implements IUserPermissionService.DeleteUserPermission
            ' set default return value
            DeleteUserPermission = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteItem from Payment Condition Entity
                intEff = objUserPermissionEnt.DeleteUserPermission(intUserID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteUserPermission = True
                Else
                    ' case row less than 1 then return False
                    DeleteUserPermission = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteUserPermission(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
    End Class
End Namespace
