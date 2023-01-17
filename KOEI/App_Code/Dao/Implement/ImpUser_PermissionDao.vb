#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpUser_PermissionDao
'	Class Discription	: Dao implement class user_permission table
'	Create User 		: Komsan Luecha
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
Imports MySql.Data.MySqlClient
Imports System.Data

Namespace Dao
    Public Class ImpUser_PermissionDao
        Implements Dao.IUser_PermissionDao

        Private Conn As Common.DBConnection.MySQLAccess
        Private cLog As New Common.Logs.Log
        Private SetDtoToEntityList As New List(Of Entity.ImpUser_PermissionEntity)

#Region "Funtion"

        '/**************************************************************
        '	Function name	: GetUserPermission
        '	Discription	    : Get user permission
        '	Return Value	: List of permission
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserPermission( _
            ByVal intUserID As Integer _
        ) As System.Collections.Generic.List(Of Entity.IUser_PermissionEntity) Implements IUser_PermissionDao.GetUserPermission
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetUserPermission = New List(Of Entity.IUser_PermissionEntity)
            Try
                ' object reader
                Dim dr As MySqlDataReader
                Dim eUserPermission As Entity.IUser_PermissionEntity

                ' assign sql statement
                With strSql
                    .AppendLine("		SELECT * FROM user_permission				")
                    .AppendLine("		WHERE user_id = ?user_id				")
                End With

                ' open new connection
                Conn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                Conn.AddParameter("?user_id", intUserID)

                ' execute datareader
                dr = Conn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    ' assign value to entity object
                    While dr.Read
                        eUserPermission = New Entity.ImpUser_PermissionEntity
                        With eUserPermission
                            .user_id = IIf(IsDBNull(dr.Item("user_id")), Nothing, dr.Item("user_id"))
                            .menu_id = IIf(IsDBNull(dr.Item("menu_id")), Nothing, dr.Item("menu_id"))
                            .fn_create = IIf(IsDBNull(dr.Item("fn_create")), Nothing, dr.Item("fn_create"))
                            .fn_update = IIf(IsDBNull(dr.Item("fn_update")), Nothing, dr.Item("fn_update"))
                            .fn_delete = IIf(IsDBNull(dr.Item("fn_delete")), Nothing, dr.Item("fn_delete"))
                            .fn_list = IIf(IsDBNull(dr.Item("fn_list")), Nothing, dr.Item("fn_list"))
                            .fn_amount = IIf(IsDBNull(dr.Item("fn_amount")), Nothing, dr.Item("fn_amount"))
                            .fn_approve = IIf(IsDBNull(dr.Item("fn_approve")), Nothing, dr.Item("fn_approve"))
                        End With
                        ' add value object to list
                        GetUserPermission.Add(eUserPermission)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetUserPermission(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                cLog.InfoLog("GetUserPermission(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserPermission
        '	Discription	    : Get user permission
        '	Return Value	: List of permission
        '	Create User	    : Charoon
        '	Create Date	    : 30-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserPermission(ByVal intUserID As Integer, ByVal intMenuID As Integer _
        ) As System.Collections.Generic.List(Of Entity.IUser_PermissionEntity) Implements IUser_PermissionDao.GetUserPermission
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetUserPermission = New List(Of Entity.IUser_PermissionEntity)

            Try
                ' object reader
                Dim dr As MySqlDataReader
                Dim eUserPermission As Entity.IUser_PermissionEntity

                ' assign sql statement
                With strSql
                    .AppendLine("SELECT * FROM user_permission")
                    .AppendLine("WHERE user_id = ?user_id and menu_id = ?menu_id")
                End With

                ' open new connection
                Conn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                Conn.AddParameter("?user_id", intUserID)
                Conn.AddParameter("?menu_id", intMenuID)

                ' execute datareader
                dr = Conn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    ' assign value to entity object
                    While dr.Read
                        eUserPermission = New Entity.ImpUser_PermissionEntity
                        With eUserPermission
                            .user_id = IIf(IsDBNull(dr.Item("user_id")), Nothing, dr.Item("user_id"))
                            .menu_id = IIf(IsDBNull(dr.Item("menu_id")), Nothing, dr.Item("menu_id"))
                            .fn_create = IIf(IsDBNull(dr.Item("fn_create")), Nothing, dr.Item("fn_create"))
                            .fn_update = IIf(IsDBNull(dr.Item("fn_update")), Nothing, dr.Item("fn_update"))
                            .fn_delete = IIf(IsDBNull(dr.Item("fn_delete")), Nothing, dr.Item("fn_delete"))
                            .fn_list = IIf(IsDBNull(dr.Item("fn_list")), Nothing, dr.Item("fn_list"))
                            .fn_amount = IIf(IsDBNull(dr.Item("fn_amount")), Nothing, dr.Item("fn_amount"))
                            .fn_approve = IIf(IsDBNull(dr.Item("fn_approve")), Nothing, dr.Item("fn_approve"))
                        End With
                        ' add value object to list
                        GetUserPermission.Add(eUserPermission)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetUserPermission(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                cLog.InfoLog("GetUserPermission(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserPermissionList
        '	Discription	    : Get user permission data list fo search
        '	Return Value	: Data list of user_permission
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserPermissionList(ByVal strFName As String, _
                                              ByVal strLName As String, _
                                              ByVal strUName As String, _
                                              ByVal intDepartment As String _
                                              ) As System.Collections.Generic.List(Of Entity.ImpUserEntity) _
                                              Implements IUser_PermissionDao.GetUserPermissionList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetUserPermissionList = New List(Of Entity.ImpUserEntity)

            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objDT As New System.Data.DataTable
                Dim objUSPDetail As Entity.ImpUserEntity

                'assign sql command
                With strSql
                    .AppendLine("   SELECT u.id, u.user_name, u.first_name, u.last_name, d.name AS department , u.last_login    ")
                    .AppendLine("   FROM user u  LEFT JOIN mst_department d ON u.department_id = d.id                           ")
                    .AppendLine("   WHERE u.delete_fg <> 1                                                                      ")
                    .AppendLine("   AND u.id IN (SELECT user_id FROM user_permission WHERE delete_fg <> 1)                      ")
                    .AppendLine("   AND (ISNULL(?user_name) OR (u.user_name LIKE CONCAT('%', ?user_name, '%')))                 ")
                    .AppendLine("   AND (ISNULL(?department_id) OR (u.department_id = ?department_id))                          ")
                    .AppendLine("   AND (ISNULL(?first_name) OR (u.first_name LIKE CONCAT('%', ?first_name, '%')))              ")
                    .AppendLine("   AND (ISNULL(?last_name) OR (u.last_name LIKE CONCAT('%', ?last_name, '%')))                 ")
                    .AppendLine("   ORDER BY u.user_name                                                                        ")
                End With

                ' new connection
                Conn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                With Conn
                    .AddParameter("?first_name", IIf(String.IsNullOrEmpty(strFName), DBNull.Value, strFName))
                    .AddParameter("?last_name", IIf(String.IsNullOrEmpty(strLName), DBNull.Value, strLName))
                    .AddParameter("?user_name", IIf(String.IsNullOrEmpty(strUName), DBNull.Value, strUName))

                    If String.IsNullOrEmpty(intDepartment) Then
                        Conn.AddParameter("?department_id", DBNull.Value)
                    Else
                        Conn.AddParameter("?department_id", CInt(intDepartment))
                    End If
                End With

                ' execute reader
                dr = Conn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objUSPDetail = New Entity.ImpUserEntity
                        ' assign data from db to entity object
                        With objUSPDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .first_name = IIf(IsDBNull(dr.Item("first_name")), Nothing, dr.Item("first_name"))
                            .last_name = IIf(IsDBNull(dr.Item("last_name")), Nothing, dr.Item("last_name"))
                            .user_name = IIf(IsDBNull(dr.Item("user_name")), Nothing, dr.Item("user_name"))
                            .department_name = IIf(IsDBNull(dr.Item("department")), Nothing, dr.Item("department"))
                            .last_login = IIf(IsDBNull(dr.Item("last_login")), Nothing, dr.Item("last_login"))
                        End With
                        ' add item to list
                        GetUserPermissionList.Add(objUSPDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetPaymentCondList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                cLog.InfoLog("GetPaymentCondList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserPermissionList
        '	Discription	    : Get user permission data list for search
        '	Return Value	: Data list of user_permission
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserSetPermissionList() As System.Collections.Generic.List(Of Entity.ImpUserEntity) Implements IUser_PermissionDao.GetUserSetPermissionList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetUserSetPermissionList = New List(Of Entity.ImpUserEntity)

            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objDT As New System.Data.DataTable
                Dim objUSPDetail As Entity.ImpUserEntity

                'assign sql command
                With strSql
                    .AppendLine("   SELECT id, user_name                                ")
                    .AppendLine("   FROM USER                                           ")
                    .AppendLine("   WHERE id IN (SELECT user_id FROM user_permission    ")
                    .AppendLine("   WHERE delete_fg <> 1)                               ")
                    .AppendLine("   AND (delete_fg <> 1)                                ")
                    .AppendLine("   ORDER BY user_name                                  ")
                End With

                ' new connection
                Conn = New Common.DBConnection.MySQLAccess

                ' execute reader
                dr = Conn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objUSPDetail = New Entity.ImpUserEntity
                        ' assign data from db to entity object
                        With objUSPDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .user_name = IIf(IsDBNull(dr.Item("user_name")), Nothing, dr.Item("user_name"))

                        End With
                        ' add item to list
                        GetUserSetPermissionList.Add(objUSPDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetUserSetPermissionList(dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetUserPermissionList
        '	Discription	    : Get user permission data list for search
        '	Return Value	: Data list of user_permission
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserNonSetPermissionList() As System.Collections.Generic.List(Of Entity.ImpUserEntity) Implements IUser_PermissionDao.GetUserNonSetPermissionList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetUserNonSetPermissionList = New List(Of Entity.ImpUserEntity)

            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objDT As New System.Data.DataTable
                Dim objUSPDetail As Entity.ImpUserEntity

                'assign sql command
                With strSql
                    .AppendLine("   SELECT id, user_name                                    ")
                    .AppendLine("   FROM USER                                               ")
                    .AppendLine("   WHERE id NOT IN (SELECT user_id FROM user_permission    ")
                    .AppendLine("   WHERE delete_fg <> 1 )                                  ")
                    .AppendLine("   AND (delete_fg <> 1)                                    ")
                    .AppendLine("   ORDER BY user_name                                      ")
                End With

                ' new connection
                Conn = New Common.DBConnection.MySQLAccess

                ' execute reader
                dr = Conn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objUSPDetail = New Entity.ImpUserEntity
                        ' assign data from db to entity object
                        With objUSPDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .user_name = IIf(IsDBNull(dr.Item("user_name")), Nothing, dr.Item("user_name"))
                        End With
                        ' add item to list
                        GetUserNonSetPermissionList.Add(objUSPDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetUserNonSetPermissionList(dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPermissionList
        '	Discription	    : Get user permission data list
        '	Return Value	: Data list of user_permission
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPermissionList() As System.Collections.Generic.List(Of Entity.ImpUser_PermissionEntity) Implements IUser_PermissionDao.GetPermissionList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPermissionList = New List(Of Entity.ImpUser_PermissionEntity)

            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objDT As New System.Data.DataTable
                Dim objUSPDetail As Entity.ImpUser_PermissionEntity

                'assign sql command
                With strSql
                    .AppendLine("   SELECT id, menu_text, fn_create, fn_update, fn_delete, fn_list, fn_confirm, fn_approve, fn_amount       ")
                    .AppendLine("   FROM Menu                               ")
                    .AppendLine("   ORDER BY category_id, priority          ")
                End With

                ' new connection
                Conn = New Common.DBConnection.MySQLAccess

                ' execute reader
                dr = Conn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objUSPDetail = New Entity.ImpUser_PermissionEntity
                        ' assign data from db to entity object
                        With objUSPDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .menu_id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .menu_text = IIf(IsDBNull(dr.Item("menu_text")), Nothing, dr.Item("menu_text"))
                            .fn_create = IIf(IsDBNull(dr.Item("fn_create")), Nothing, dr.Item("fn_create"))
                            .fn_update = IIf(IsDBNull(dr.Item("fn_update")), Nothing, dr.Item("fn_update"))
                            .fn_delete = IIf(IsDBNull(dr.Item("fn_delete")), Nothing, dr.Item("fn_delete"))
                            .fn_list = IIf(IsDBNull(dr.Item("fn_list")), Nothing, dr.Item("fn_list"))
                            .fn_confirm = IIf(IsDBNull(dr.Item("fn_confirm")), Nothing, dr.Item("fn_confirm"))
                            .fn_approve = IIf(IsDBNull(dr.Item("fn_approve")), Nothing, dr.Item("fn_approve"))
                            .fn_amount = IIf(IsDBNull(dr.Item("fn_amount")), Nothing, dr.Item("fn_amount"))

                        End With
                        ' add item to list
                        GetPermissionList.Add(objUSPDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetUserNonSetPermissionList(dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserDetail
        '	Discription	    : Get user data detail
        '	Return Value	: Data list of user
        '	Create User	    : Wasan D.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserDetail(ByVal strUserName As String) As System.Collections.Generic.List(Of Entity.ImpUserEntity) Implements IUser_PermissionDao.GetUserDetail
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetUserDetail = New List(Of Entity.ImpUserEntity)

            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objDT As New System.Data.DataTable
                Dim objUserDetail As Entity.ImpUserEntity

                'assign sql command
                With strSql
                    .AppendLine("   SELECT u.first_name, u.last_name, d.name AS department              ")
                    .AppendLine("   FROM user u LEFT JOIN mst_department d ON u.department_id = d.id    ")
                    .AppendLine("   WHERE(u.user_name = ?user_name) AND u.delete_fg <> 1                ")
                End With

                ' new connection
                Conn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                Conn.AddParameter("?user_name", IIf(String.IsNullOrEmpty(strUserName), DBNull.Value, strUserName))

                ' execute reader
                dr = Conn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objUserDetail = New Entity.ImpUserEntity
                        ' assign data from db to entity object
                        With objUserDetail
                            .first_name = IIf(IsDBNull(dr.Item("first_name")), Nothing, dr.Item("first_name"))
                            .last_name = IIf(IsDBNull(dr.Item("last_name")), Nothing, dr.Item("last_name"))
                            .department_name = IIf(IsDBNull(dr.Item("department")), Nothing, dr.Item("department"))
                        End With
                        ' add item to list
                        GetUserDetail.Add(objUserDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetUserDetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                cLog.InfoLog("GetUserDetail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPermissionList
        '	Discription	    : Get user permission data list
        '	Return Value	: Data list of user_permission
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserPermissionSettingList(ByVal strUserID As String) As System.Collections.Generic.List(Of Entity.ImpUser_PermissionEntity) Implements IUser_PermissionDao.GetUserPermissionSettingList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetUserPermissionSettingList = New List(Of Entity.ImpUser_PermissionEntity)

            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objDT As New System.Data.DataTable
                Dim objUSPDetail As Entity.ImpUser_PermissionEntity

                'assign sql command
                With strSql
                    .AppendLine("   SELECT up.id, m.id As menu_id, up.user_id, m.menu_text AS menu, ")
                    .AppendLine("   up.fn_create, up.fn_update, up.fn_delete,                       ")
                    .AppendLine("   up.fn_list, up.fn_amount, up.fn_confirm, up.fn_approve          ")
                    .AppendLine("   FROM user_permission up RIGHT JOIN menu m ON up.menu_id = m.id  ")
                    .AppendLine("   AND up.user_id = ?user_id AND up.delete_fg <> 1                 ")
                    .AppendLine("   ORDER BY m.category_id, m.priority                              ")
                End With

                ' new connection
                Conn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                Conn.AddParameter("?user_id", IIf(String.IsNullOrEmpty(strUserID), DBNull.Value, strUserID))

                ' execute reader
                dr = Conn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objUSPDetail = New Entity.ImpUser_PermissionEntity
                        ' assign data from db to entity object
                        With objUSPDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .menu_id = IIf(IsDBNull(dr.Item("menu_id")), Nothing, dr.Item("menu_id"))
                            .user_id = IIf(IsDBNull(dr.Item("user_id")), Nothing, dr.Item("user_id"))
                            .menu_text = IIf(IsDBNull(dr.Item("menu")), Nothing, dr.Item("menu"))
                            .fn_create = IIf(IsDBNull(dr.Item("fn_create")), Nothing, dr.Item("fn_create"))
                            .fn_update = IIf(IsDBNull(dr.Item("fn_update")), Nothing, dr.Item("fn_update"))
                            .fn_delete = IIf(IsDBNull(dr.Item("fn_delete")), Nothing, dr.Item("fn_delete"))
                            .fn_list = IIf(IsDBNull(dr.Item("fn_list")), Nothing, dr.Item("fn_list"))
                            .fn_confirm = IIf(IsDBNull(dr.Item("fn_confirm")), Nothing, dr.Item("fn_confirm"))
                            .fn_approve = IIf(IsDBNull(dr.Item("fn_approve")), Nothing, dr.Item("fn_approve"))
                            .fn_amount = IIf(IsDBNull(dr.Item("fn_amount")), Nothing, dr.Item("fn_amount"))

                        End With
                        ' add item to list
                        GetUserPermissionSettingList.Add(objUSPDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetUserPermissionSettingList(dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateUserPermission
        '	Discription	    : Get user permission data list
        '	Return Value	: Data list of user_permission
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateUserPermission(ByVal dtUserPM As System.Collections.Generic.List(Of Entity.ImpUser_PermissionEntity)) As Integer Implements IUser_PermissionDao.UpdateUserPermission
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' variable keep row effect
            Dim intEff As Integer
            ' Variable keep user_id
            Dim intUserID As Integer = 0

            'Set default return value
            UpdateUserPermission = False
            Try
                ' assign sql command
                With strSql
                    .AppendLine("	UPDATE user_permission          ")
                    .AppendLine("	SET fn_create=?fn_create,       ")
                    .AppendLine("	fn_update= ?fn_update,          ")
                    .AppendLine("	fn_delete=?fn_delete,           ")
                    .AppendLine("	fn_list=?fn_list,               ")
                    .AppendLine("	fn_confirm=?fn_confirm,         ")
                    .AppendLine("	fn_approve=?fn_approve,         ")
                    .AppendLine("	fn_amount=?fn_amount,           ")
                    .AppendLine("	updated_by=?update_by,          ")
                    .AppendLine("   updated_date = DATE_FORMAT(NOW(),'%Y%m%d%H%i%s')    ")
                    .AppendLine("	WHERE user_id = ?user_id        ")
                    .AppendLine("	AND menu_id = ?menu_id          ")
                End With

                ' new connection object
                Conn = New Common.DBConnection.MySQLAccess
                ' begin transaction
                Conn.BeginTrans()
                For Each objDto In dtUserPM
                    ' assign parameter
                    With Conn
                        .AddParameter("?fn_create", objDto.fn_create)
                        .AddParameter("?fn_update", objDto.fn_update)
                        .AddParameter("?fn_delete", objDto.fn_delete)
                        .AddParameter("?fn_list", objDto.fn_list)
                        .AddParameter("?fn_confirm", objDto.fn_confirm)
                        .AddParameter("?fn_approve", objDto.fn_approve)
                        .AddParameter("?fn_amount", objDto.fn_amount)
                        .AddParameter("?user_id", objDto.user_id)
                        .AddParameter("?menu_id", objDto.menu_id)
                        .AddParameter("?update_by", HttpContext.Current.Session("UserID"))


                        ' execute sql command and return row effect to intEff variable
                        intEff = .ExecuteNonQuery(strSql.ToString)

                        ' check effect
                        If intEff > 0 Then
                            ' case row effect more than 0 then commit transaction
                            .CommitTrans()
                        Else
                            ' Set Entity to List of Entity
                            SetDtoToEntityList.Add(objDto)
                            intUserID = objDto.user_id
                        End If
                    End With
                Next
                If intUserID <> 0 Then
                    InsertUserPermission(SetDtoToEntityList, intUserID)
                End If
                ' assign return value
                UpdateUserPermission = intEff
            Catch exSql As MySqlException
                ' write error log
                cLog.ErrorLog("UpdatePaymentCond(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("UpdateUserPermission(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                cLog.InfoLog("UpdateUserPermission(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertUserPermission
        '	Discription	    : Get user permission data list
        '	Return Value	: row effect
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertUserPermission(ByVal dtUserPM As System.Collections.Generic.List(Of Entity.ImpUser_PermissionEntity), ByVal intUserID As String) As Integer Implements IUser_PermissionDao.InsertUserPermission
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' variable keep row effect
            Dim intEff As Integer

            'Set default return value
            InsertUserPermission = False
            Try
                ' assign sql command
                With strSql
                    .AppendLine("	INSERT INTO user_permission (user_id, menu_id, fn_create, fn_update,    ")
                    .AppendLine("	fn_delete, fn_list, fn_confirm, fn_approve, fn_amount ,delete_fg,       ")
                    .AppendLine("	created_by, created_date, updated_by, updated_date)                     ")
                    .AppendLine("	VALUES(?user_id, ?menu_id, ?fn_create, ?fn_update, ?fn_delete,          ")
                    .AppendLine("	?fn_list, ?fn_confirm, ?fn_approve, ?fn_amount, 0, ?update_by,          ")
                    .AppendLine("	DATE_FORMAT(NOW(),'%Y%m%d%H%i%s'), ?update_by, DATE_FORMAT(NOW(),'%Y%m%d%H%i%s'))   ")
                End With

                ' new connection object
                Conn = New Common.DBConnection.MySQLAccess
                ' begin transaction
                Conn.BeginTrans()
                For Each objDto In dtUserPM
                    ' assign parameter
                    With Conn
                        .AddParameter("?fn_create", objDto.fn_create)
                        .AddParameter("?fn_update", objDto.fn_update)
                        .AddParameter("?fn_delete", objDto.fn_delete)
                        .AddParameter("?fn_list", objDto.fn_list)
                        .AddParameter("?fn_confirm", objDto.fn_confirm)
                        .AddParameter("?fn_approve", objDto.fn_approve)
                        .AddParameter("?fn_amount", objDto.fn_amount)
                        .AddParameter("?user_id", intUserID)
                        .AddParameter("?menu_id", objDto.menu_id)
                        .AddParameter("?update_by", HttpContext.Current.Session("UserID"))


                        ' execute sql command and return row effect to intEff variable
                        intEff = .ExecuteNonQuery(strSql.ToString)

                        ' check effect
                        If intEff > 0 Then
                            ' case row effect more than 0 then commit transaction
                            .CommitTrans()
                        Else
                            ' case row effect less than 1 then rollback transaction
                            .RollbackTrans()
                        End If
                    End With
                Next
                ' assign return value
                InsertUserPermission = intEff
            Catch exSql As MySqlException
                ' write error log
                cLog.ErrorLog("InsertUserPermission(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("InsertUserPermission(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                cLog.InfoLog("InsertUserPermission(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: DeleteUserPermission
        '	Discription	    : Delete user permission data list
        '	Return Value	: Row effect
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteUserPermission(ByVal intUserID As String) As Integer Implements IUser_PermissionDao.DeleteUserPermission
            Dim strSql As New Text.StringBuilder
            ' variable keep row effect
            Dim intEff As Integer

            'Set default return value
            DeleteUserPermission = False
            Try
                ' assign sql command
                strSql.AppendLine("	UPDATE user_permission SET delete_fg = 1 WHERE user_id = ?user_id   ")


                ' new connection object
                Conn = New Common.DBConnection.MySQLAccess
                ' begin transaction
                With Conn
                    .BeginTrans()
                    ' assign parameter
                    .AddParameter("?user_id", intUserID)


                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                    ' check effect
                    If intEff > 0 Then
                        ' case row effect more than 0 then commit transaction
                        .CommitTrans()
                    Else
                        ' case row effect less than 1 then rollback transaction
                        .RollbackTrans()
                    End If
                End With
                ' assign return value
                DeleteUserPermission = intEff
            Catch exSql As MySqlException
                ' write error log
                cLog.ErrorLog("DeleteUserPermission(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("DeleteUserPermission(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                cLog.InfoLog("DeleteUserPermission(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
       
#End Region

    End Class
End Namespace

