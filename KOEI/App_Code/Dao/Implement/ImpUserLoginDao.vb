#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpUserLoginDao
'	Class Discription	: Class of table user
'	Create User 		: Nisa S.
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


Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Exception
Imports Exceptions
Imports Dao

Public Class ImpUserLoginDao
    Implements IUserLoginDao

    Private objConn As Common.DBConnection.MySQLAccess
    Private objLog As New Common.Logs.Log
    Private strMsgErr As String = String.Empty

#Region "Function"


    '/**************************************************************
    '	Function name	: GetUserLoginList
    '	Discription	    : Get UserLogin list
    '	Return Value	: List
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Function GetUserLoginList( _
            ByVal strID As String, _
            ByVal strUserName As String, _
            ByVal strFirstName As String, _
            ByVal strLastName As String, _
            ByVal strDepartment As String _
    ) As System.Collections.Generic.List(Of Entity.ImpUserLoginDetailEntity) Implements IUserLoginDao.GetUserLoginList
        ' variable for keep sql command
        Dim strSql As New Text.StringBuilder
        ' set default list
        GetUserLoginList = New List(Of Entity.ImpUserLoginDetailEntity)
        Try
            ' data reader object
            Dim dr As MySqlDataReader
            Dim objUserLoginDetail As Entity.ImpUserLoginDetailEntity

            ' assign sql command
            With strSql
                .AppendLine(" SELECT u.id ")
                .AppendLine(" 		, u.user_name ")
                .AppendLine(" 		, u.password ")
                .AppendLine(" 		, u.first_name ")
                .AppendLine(" 		, u.last_name ")
                .AppendLine(" 		, d.name AS department ")
                .AppendLine(" 		, date_format(u.last_login,'%Y/%m/%d %h:%i:%s') as last_login ")
                .AppendLine(" FROM user u  LEFT JOIN mst_department d ON u.department_id = d.id ")
                .AppendLine(" WHERE u.delete_fg <> 1 ")
                .AppendLine("   AND (ISNULL(?user_name) OR u.user_name like CONCAT('%', ?user_name, '%')) ")
                .AppendLine("   AND (ISNULL(?department_id) OR u.department_id=?department_id) ")
                .AppendLine("   AND (ISNULL(?first_name) OR u.first_name like CONCAT('%', ?first_name, '%')) ")
                .AppendLine("   AND (ISNULL(?last_name) OR u.last_name like CONCAT('%', ?last_name, '%')) ")
                .AppendLine(" ORDER BY u.user_name ")
            End With

            ' new connection
            objConn = New Common.DBConnection.MySQLAccess
            ' assign parameter
            objConn.AddParameter("?user_name", IIf(String.IsNullOrEmpty(strUserName), DBNull.Value, strUserName))
            objConn.AddParameter("?department_id", IIf(String.IsNullOrEmpty(strDepartment), DBNull.Value, strDepartment))
            objConn.AddParameter("?first_name", IIf(String.IsNullOrEmpty(strFirstName), DBNull.Value, strFirstName))
            objConn.AddParameter("?last_name", IIf(String.IsNullOrEmpty(strLastName), DBNull.Value, strLastName))


            ' execute reader
            dr = objConn.ExecuteReader(strSql.ToString)

            ' check exist data
            If dr.HasRows Then
                While dr.Read
                    ' new object entity
                    objUserLoginDetail = New Entity.ImpUserLoginDetailEntity
                    ' assign data from db to entity object
                    With objUserLoginDetail
                        .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                        .user_name = IIf(IsDBNull(dr.Item("user_name")), Nothing, dr.Item("user_name"))
                        .password = IIf(IsDBNull(dr.Item("password")), Nothing, dr.Item("password"))
                        .first_name = IIf(IsDBNull(dr.Item("first_name")), Nothing, dr.Item("first_name"))
                        .last_name = IIf(IsDBNull(dr.Item("last_name")), Nothing, dr.Item("last_name"))
                        .name = IIf(IsDBNull(dr.Item("department")), Nothing, dr.Item("department"))
                        .last_login = IIf(IsDBNull(dr.Item("last_login")), Nothing, dr.Item("last_login"))
                    End With
                    ' add Staff to list
                    GetUserLoginList.Add(objUserLoginDetail)
                End While
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetUserLoginList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            ' write sql command
            objLog.InfoLog("GetUserLoginList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        Finally
            If Not objConn Is Nothing Then objConn.Close()
        End Try
    End Function

    '/**************************************************************
    '	Function name	: GetDepartmentForList
    '	Discription	    : Get data Department for set dropdownlist
    '	Return Value	: List
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Function GetDepartmentForList() As System.Collections.Generic.List(Of Entity.IUserLoginEntity) Implements IUserLoginDao.GetDepartmentForList
        ' variable keep sql statement
        Dim strSql As New Text.StringBuilder
        GetDepartmentForList = New List(Of Entity.IUserLoginEntity)
        Try
            ' object variable data reader
            Dim dr As MySqlDataReader
            ' object variable vendor entity
            Dim objDepartmentEnt As Entity.IUserLoginEntity

            ' assign sql statement
            With strSql
                .AppendLine("	SELECT id 		")
                .AppendLine("		 ,name 	")
                .AppendLine("	FROM mst_department 		")
                .AppendLine("	WHERE delete_fg <> 1 		")
                .AppendLine("	ORDER BY name		")
            End With
            ' new connection
            objConn = New Common.DBConnection.MySQLAccess
            ' execute sql statement
            dr = objConn.ExecuteReader(strSql.ToString)

            ' check exist data
            If dr.HasRows Then
                While dr.Read
                    ' new WorkCategory entity
                    objDepartmentEnt = New Entity.ImpUserLoginEntity
                    With objDepartmentEnt
                        ' assign data to object WorkCategory entity
                        .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                        .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                    End With
                    ' add object WorkCategory entity to list
                    GetDepartmentForList.Add(objDepartmentEnt)
                End While
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetDepartmentForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            ' write sql statement
            objLog.InfoLog("GetDepartmentForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        Finally
            ' Dispose object connection
            If Not IsNothing(objConn) Then objConn = Nothing
        End Try
    End Function

    '/**************************************************************
    '	Function name	: GetAccount_Next_ApproveForList
    '	Discription	    : Get data Account_Next_Approve for set dropdownlist
    '	Return Value	: List
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Function GetAccount_Next_ApproveForList(ByVal id As String) As System.Collections.Generic.List(Of Entity.IUserLoginEntity) Implements IUserLoginDao.GetAccount_Next_ApproveForList
        ' variable keep sql statement
        Dim strSql As New Text.StringBuilder
        GetAccount_Next_ApproveForList = New List(Of Entity.IUserLoginEntity)
        Try
            ' object variable data reader
            Dim dr As MySqlDataReader
            ' object variable Account_Next_Approve entity
            Dim objAccount_Next_ApproveEnt As Entity.IUserLoginEntity

            ' assign sql statement
            With strSql
                .AppendLine("	SELECT id 		")
                .AppendLine("		 ,concat(first_name,' ',last_name) name 	")
                .AppendLine("	FROM user 		")
                .AppendLine("	WHERE delete_fg <> 1 		")
                If IsNothing(id) = False Then
                    .AppendLine("	AND id<>?Session		")
                End If
                .AppendLine("	ORDER BY first_name,last_name		")
            End With
            ' new connection
            objConn = New Common.DBConnection.MySQLAccess
            ' assign parameter
            If IsNothing(id) = False Then
                objConn.AddParameter("?Session", id)
            End If
            ' execute sql statement
            dr = objConn.ExecuteReader(strSql.ToString)

            ' check exist data
            If dr.HasRows Then
                While dr.Read
                    ' new Account_Next_Approve entity
                    objAccount_Next_ApproveEnt = New Entity.ImpUserLoginEntity
                    With objAccount_Next_ApproveEnt
                        ' assign data to object Account_Next_Approve entity
                        .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                        .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                    End With
                    ' add object Account_Next_Approve entity to list
                    GetAccount_Next_ApproveForList.Add(objAccount_Next_ApproveEnt)
                End While
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetAccount_Next_ApproveForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            ' write sql statement
            objLog.InfoLog("GetAccount_Next_ApproveForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        Finally
            ' Dispose object connection
            If Not IsNothing(objConn) Then objConn = Nothing
        End Try
    End Function

    '/**************************************************************
    '	Function name	: GetPurchase_Next_ApproveForList
    '	Discription	    : Get data Purchase_Next_Approve for set dropdownlist
    '	Return Value	: List
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Function GetPurchase_Next_ApproveForList(ByVal id As String) As System.Collections.Generic.List(Of Entity.IUserLoginEntity) Implements IUserLoginDao.GetPurchase_Next_ApproveForList
        ' variable keep sql statement
        Dim strSql As New Text.StringBuilder
        GetPurchase_Next_ApproveForList = New List(Of Entity.IUserLoginEntity)
        Try
            ' object variable data reader
            Dim dr As MySqlDataReader
            ' object variable Account_Next_Approve entity
            Dim objPurchase_Next_ApproveEnt As Entity.IUserLoginEntity

            ' assign sql statement
            With strSql
                .AppendLine("	SELECT id 		")
                .AppendLine("		 ,concat(first_name,' ',last_name) name 	")
                .AppendLine("	FROM user 		")
                .AppendLine("	WHERE delete_fg <> 1 		")
                If IsNothing(id) = False Then
                    .AppendLine("	AND id<>?Session		")
                End If
                .AppendLine("	ORDER BY first_name,last_name		")
            End With
            ' new connection
            objConn = New Common.DBConnection.MySQLAccess

            ' assign parameter
            If IsNothing(id) = False Then
                objConn.AddParameter("?Session", id)
            End If

            ' execute sql statement
            dr = objConn.ExecuteReader(strSql.ToString)

            ' check exist data
            If dr.HasRows Then
                While dr.Read
                    ' new Account_Next_Approve entity
                    objPurchase_Next_ApproveEnt = New Entity.ImpUserLoginEntity
                    With objPurchase_Next_ApproveEnt
                        ' assign data to object Account_Next_Approve entity
                        .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                        .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                    End With
                    ' add object Account_Next_Approve entity to list
                    GetPurchase_Next_ApproveForList.Add(objPurchase_Next_ApproveEnt)
                End While
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetPurchase_Next_ApproveForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            ' write sql statement
            objLog.InfoLog("GetPurchase_Next_ApproveForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        Finally
            ' Dispose object connection
            If Not IsNothing(objConn) Then objConn = Nothing
        End Try
    End Function

    '/**************************************************************
    '	Function name	: GetOutsource_Next_ApproveForList
    '	Discription	    : Get data Outsource_Next_Approve for set dropdownlist
    '	Return Value	: List
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Function GetOutsource_Next_ApproveForList(ByVal id As String) As System.Collections.Generic.List(Of Entity.IUserLoginEntity) Implements IUserLoginDao.GetOutsource_Next_ApproveForList
        ' variable keep sql statement
        Dim strSql As New Text.StringBuilder
        GetOutsource_Next_ApproveForList = New List(Of Entity.IUserLoginEntity)
        Try
            ' object variable data reader
            Dim dr As MySqlDataReader
            ' object variable Account_Next_Approve entity
            Dim objOutsource_Next_ApproveEnt As Entity.IUserLoginEntity

            ' assign sql statement
            With strSql
                .AppendLine("	SELECT id 		")
                .AppendLine("		 ,concat(first_name,' ',last_name) name 	")
                .AppendLine("	FROM user 		")
                .AppendLine("	WHERE delete_fg <> 1 		")
                If IsNothing(id) = False Then
                    .AppendLine("	AND id<>?Session		")
                End If
                .AppendLine("	ORDER BY first_name,last_name		")
            End With
            ' new connection
            objConn = New Common.DBConnection.MySQLAccess

            ' assign parameter
            If IsNothing(id) = False Then
                objConn.AddParameter("?Session", id)
            End If

            ' execute sql statement
            dr = objConn.ExecuteReader(strSql.ToString)

            ' check exist data
            If dr.HasRows Then
                While dr.Read
                    ' new Account_Next_Approve entity
                    objOutsource_Next_ApproveEnt = New Entity.ImpUserLoginEntity
                    With objOutsource_Next_ApproveEnt
                        ' assign data to object Account_Next_Approve entity
                        .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                        .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                    End With
                    ' add object Account_Next_Approve entity to list
                    GetOutsource_Next_ApproveForList.Add(objOutsource_Next_ApproveEnt)
                End While
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetOutsource_Next_ApproveForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            ' write sql statement
            objLog.InfoLog("GetOutsource_Next_ApproveForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        Finally
            ' Dispose object connection
            If Not IsNothing(objConn) Then objConn = Nothing
        End Try
    End Function

    '/**************************************************************
    '	Function name	: CountUsedInPO
    '	Discription	    : Count UserLogin in used PO_Detail
    '	Return Value	: Integer
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Function CountUsedInPO( _
        ByVal intUserLoginID As Integer _
    ) As Integer Implements IUserLoginDao.CountUsedInPO
        ' variable keep sql command
        Dim strSql As New Text.StringBuilder
        ' set default return value
        CountUsedInPO = -1
        Try
            ' assign sql command
            With strSql
                .AppendLine("		SELECT COUNT(user_id) AS used_count 				")
                .AppendLine("		FROM user_permission 				")
                .AppendLine("		WHERE user_id = ?id			")
                .AppendLine("       AND delete_fg <> 1 ")
            End With

            ' new connection object
            objConn = New Common.DBConnection.MySQLAccess
            ' assign parameter
            objConn.AddParameter("?id", intUserLoginID)

            ' execute sql command
            CountUsedInPO = objConn.ExecuteScalar(strSql.ToString)
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CountUsedInPO(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            ' write sql command
            objLog.ErrorLog("CountUsedInPO(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        Finally
            If Not objConn Is Nothing Then objConn.Close()
        End Try
    End Function

    '/**************************************************************
    '	Function name	: DeleteUserLogin
    '	Discription	    : Delete UserLogin
    '	Return Value	: Integer
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Function DeleteUserLogin( _
        ByVal intUserLoginID As Integer _
    ) As Integer Implements IUserLoginDao.DeleteUserLogin
        ' strSql for keep sql command
        Dim strSql As New Text.StringBuilder
        ' set default return value
        DeleteUserLogin = 0
        Try
            ' intEff keep row effect
            Dim intEff As Integer
            ' assign sql command
            With strSql
                .AppendLine("       UPDATE user                             ")
                .AppendLine("		SET delete_fg=1,							")
                .AppendLine("		    updated_by = ?update_by,							")
                .AppendLine("		    updated_date = DATE_FORMAT(NOW(),'%Y%m%d%H%i%s')							")
                .AppendLine("		WHERE id = ?id							")
            End With
            ' new object connection
            objConn = New Common.DBConnection.MySQLAccess

            ' assign parameter
            objConn.AddParameter("?update_by", HttpContext.Current.Session("UserID"))
            objConn.AddParameter("?id", intUserLoginID)

            ' begin transaction
            objConn.BeginTrans()

            ' execute non query and keep row effect
            intEff = objConn.ExecuteNonQuery(strSql.ToString)

            ' check row effect
            If intEff > 0 Then
                ' case row effect more than 0 then commit transaction
                objConn.CommitTrans()
            Else
                ' case row effect less than 1 then rollback transaction
                objConn.RollbackTrans()
            End If
            ' set value to return variable
            DeleteUserLogin = intEff
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteUserLogin(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            ' write sql command
            objLog.ErrorLog("DeleteUserLogin(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        Finally
            If Not objConn Is Nothing Then objConn.Close()
        End Try
    End Function


    '/**************************************************************
    ' Function name   : CheckDupUserLogin
    ' Discription     : Check duplication UserLogin Admin
    ' Return Value    : Integer
    ' Create User     : Nisa S.
    ' Create Date     : 11-07-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Public Function CheckDupUserLogin( _
        ByVal intUserLoginID As String, _
        ByVal strUserName As String _
    ) As Integer Implements IUserLoginDao.CheckDupUserLogin
        ' variable keep sql command
        Dim strSql As New Text.StringBuilder
        ' set default return value
        CheckDupUserLogin = -1
        Try
            ' assign sql command
            With strSql
                .AppendLine("  SELECT *     ")
                .AppendLine("  FROM user     ")
                .AppendLine("  WHERE user_name=?user_name   ")
                .AppendLine("   AND delete_fg<>1 ")
                .AppendLine("   AND id<>?id  ")
            End With

            ' new connection object
            objConn = New Common.DBConnection.MySQLAccess
            ' assign parameter
            objConn.AddParameter("?id", intUserLoginID)
            objConn.AddParameter("?user_name", strUserName)


            ' execute sql command
            CheckDupUserLogin = objConn.ExecuteScalar(strSql.ToString)
        Catch exSql As MySqlException
            ' write error log
            objLog.ErrorLog("CheckDupUserLogin(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
            ' throw exception
            Throw
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckDupUserLogin(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            ' write sql command
            objLog.ErrorLog("CheckDupUserLogin(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        Finally
            If Not objConn Is Nothing Then objConn.Close()
        End Try
    End Function

    '/**************************************************************
    '	Function name	: InsertUserLogin
    '	Discription	    : Insert UserLogin to user
    '	Return Value	: Integer
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Function InsertUserLogin( _
        ByVal objUserLoginEnt As Entity.IUserLoginEntity _
    ) As Integer Implements IUserLoginDao.InsertUserLogin
        ' variable for keep sql command
        Dim strSql As New Text.StringBuilder
        ' set default return value
        InsertUserLogin = 0
        Try
            ' variable keep row effect
            Dim intEff As Integer

            ' assign sql command
            With strSql
                .AppendLine("		INSERT INTO user (user_name						")
                .AppendLine("				,password				")
                .AppendLine("				,first_name				")
                .AppendLine("				,last_name				")
                .AppendLine("				,account_next_approve				")
                .AppendLine("				,purchase_next_approve				")
                .AppendLine("				,outsource_next_approve				")
                .AppendLine("				,delete_fg				")
                .AppendLine("				,department_id				")
                .AppendLine("				,created_by				")
                .AppendLine("				,created_date				")
                .AppendLine("				,updated_by				")
                .AppendLine("				,updated_date)				")
                .AppendLine("		VALUES (?user_name						")
                .AppendLine("			,?password					")
                .AppendLine("			,?first_name					")
                .AppendLine("			,?last_name					")
                .AppendLine("			,case when ?account_next_approve=0 then null else ?account_next_approve end				")
                .AppendLine("			,case when ?purchase_next_approve=0 then null else ?purchase_next_approve end				")
                .AppendLine("			,case when ?outsource_next_approve=0 then null else ?outsource_next_approve end					")
                .AppendLine("			,0					")
                .AppendLine("			,case when ?department_id=0 then null else ?department_id end					")
                .AppendLine("			,?created_by				")
                .AppendLine("			,DATE_FORMAT(NOW(),'%Y%m%d%H%i%s')					")
                .AppendLine("	        ,?updated_by        ")
                .AppendLine("           ,DATE_FORMAT(NOW(),'%Y%m%d%H%i%s'))				")
            End With

            ' new connection object
            objConn = New Common.DBConnection.MySQLAccess

            With objConn
                ' assign parameter
                .AddParameter("?user_name", objUserLoginEnt.user_name)
                .AddParameter("?password", objUserLoginEnt.password)
                .AddParameter("?first_name", objUserLoginEnt.first_name)
                .AddParameter("?last_name", objUserLoginEnt.last_name)
                .AddParameter("?account_next_approve", objUserLoginEnt.account_next_approve)
                .AddParameter("?purchase_next_approve", objUserLoginEnt.purchase_next_approve)
                .AddParameter("?outsource_next_approve", objUserLoginEnt.outsource_next_approve)
                .AddParameter("?department_id", objUserLoginEnt.department_id)
                .AddParameter("?created_by", HttpContext.Current.Session("UserID"))
                .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))

                ' begin transaction
                .BeginTrans()

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
            InsertUserLogin = intEff
        Catch exSql As MySqlException
            ' write error log
            objLog.ErrorLog("UpdateUserLogin(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
            ' throw exception
            Throw
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertUserLogin(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            ' write sql command
            objLog.InfoLog("InsertUserLogin(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        Finally
            If Not objConn Is Nothing Then objConn.Close()
        End Try
    End Function

    '/**************************************************************
    '	Function name	: UpdateUserLogin
    '	Discription	    : Update UserLogin to user 
    '	Return Value	: Integer
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Function UpdateUserLogin( _
        ByVal objUserLoginEnt As Entity.IUserLoginEntity _
    ) As Integer Implements IUserLoginDao.UpdateUserLogin
        ' variable for keep sql command
        Dim strSql As New Text.StringBuilder
        ' set default return value
        UpdateUserLogin = 0
        Try
            ' variable keep row effect
            Dim intEff As Integer

            ' assign sql command
            With strSql
                .AppendLine("		UPDATE user							")
                .AppendLine("		SET user_name=?user_name    ")
                .AppendLine("           ,password=?password							")
                .AppendLine("           ,first_name = ?first_name							")
                .AppendLine("			,last_name = ?last_name 						")
                .AppendLine("			,account_next_approve=case when ?account_next_approve=0 then null else ?account_next_approve end						")
                .AppendLine("			,purchase_next_approve=case when ?purchase_next_approve=0 then null else ?purchase_next_approve end					")
                .AppendLine("			,outsource_next_approve=case when ?outsource_next_approve=0 then null else ?outsource_next_approve end					")
                .AppendLine("			,department_id=case when ?department_id=0 then null else ?department_id end				")
                .AppendLine("		    ,updated_by=?Session					")
                .AppendLine("		    ,updated_date=date_format(now(),'%Y%m%d%H%i%s')							")
                .AppendLine("		WHERE id = ?id							")
            End With

            ' new connection object
            objConn = New Common.DBConnection.MySQLAccess

            With objConn
                ' assign parameter
                .AddParameter("?user_name", objUserLoginEnt.user_name)
                .AddParameter("?password", objUserLoginEnt.password)
                .AddParameter("?first_name", objUserLoginEnt.first_name)
                .AddParameter("?last_name", objUserLoginEnt.last_name)
                .AddParameter("?account_next_approve", objUserLoginEnt.account_next_approve)
                .AddParameter("?purchase_next_approve", objUserLoginEnt.purchase_next_approve)
                .AddParameter("?outsource_next_approve", objUserLoginEnt.outsource_next_approve)
                .AddParameter("?department_id", objUserLoginEnt.department_id)
                .AddParameter("?Session", HttpContext.Current.Session("UserID"))
                .AddParameter("?id", objUserLoginEnt.id)

                ' begin transaction
                .BeginTrans()

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
            UpdateUserLogin = intEff
        Catch exSql As MySqlException
            ' write error log
            objLog.ErrorLog("UpdateUserLogin(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
            ' throw exception
            Throw
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateUserLogin(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            ' write sql command
            objLog.InfoLog("UpdateUserLogin(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        Finally
            If Not objConn Is Nothing Then objConn.Close()
        End Try
    End Function

    '/**************************************************************
    '	Function name	: GetUserLoginByID
    '	Discription	    : Get UserLogin by ID
    '	Return Value	: IUserLoginEntity Object
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '    Update(User)
    '	Update Date	    :
    '*************************************************************/
    Public Function GetUserLoginByID( _
        ByVal intUserLoginID As String _
    ) As Entity.IUserLoginEntity Implements IUserLoginDao.GetUserLoginByID
        'variable for keep sql command
        Dim strSql As New Text.StringBuilder
        'set default return value
        GetUserLoginByID = New Entity.ImpUserLoginEntity
        Try
            'variable datareader object
            Dim dr As MySqlDataReader

            'assign sql command
            With strSql
                .AppendLine("	SELECT id ")
                .AppendLine("	    ,user_name ")
                .AppendLine("	    ,password ")
                .AppendLine("	    ,first_name")
                .AppendLine("	    ,last_name ")
                .AppendLine("	    ,department_id ")
                .AppendLine("	    ,account_next_approve ")
                .AppendLine("	    ,purchase_next_approve")
                .AppendLine("	    ,outsource_next_approve ")
                .AppendLine("	FROM user		")
                .AppendLine("	WHERE id = ?id		")
            End With

            'new connection object
            objConn = New Common.DBConnection.MySQLAccess
            'assign(Parameter)
            objConn.AddParameter("?id", intUserLoginID)

            'execute sql command with data reader object
            dr = objConn.ExecuteReader(strSql.ToString)

            'check exist data
            If dr.HasRows Then
                While dr.Read
                    'assign data from db to entity object
                    With GetUserLoginByID
                        .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                        .user_name = IIf(IsDBNull(dr.Item("user_name")), Nothing, dr.Item("user_name"))
                        .password = IIf(IsDBNull(dr.Item("password")), Nothing, dr.Item("password"))
                        .first_name = IIf(IsDBNull(dr.Item("first_name")), Nothing, dr.Item("first_name"))
                        .last_name = IIf(IsDBNull(dr.Item("last_name")), Nothing, dr.Item("last_name"))
                        .department_id = IIf(IsDBNull(dr.Item("department_id")), Nothing, dr.Item("department_id"))
                        .account_next_approve = IIf(IsDBNull(dr.Item("account_next_approve")), Nothing, dr.Item("account_next_approve"))
                        .purchase_next_approve = IIf(IsDBNull(dr.Item("purchase_next_approve")), Nothing, dr.Item("purchase_next_approve"))
                        .outsource_next_approve = IIf(IsDBNull(dr.Item("outsource_next_approve")), Nothing, dr.Item("outsource_next_approve"))
                    End With
                End While
            End If

        Catch ex As Exception
            'write error log
            objLog.ErrorLog("GetUserLoginByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            'write sql command
            objLog.InfoLog("GetUserLoginByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        Finally
            If Not objConn Is Nothing Then objConn.Close()
        End Try
    End Function

    '/**************************************************************
    '	Function name	: GetUserLoginForDetail
    '	Discription	    : Get data UserLogin for detail
    '	Return Value	: Class entity
    '	Create User	    : Nisa S.
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Function GetUserLoginForDetail(ByVal intUserLoginID As Integer) As Entity.ImpUserLoginEntity Implements IUserLoginDao.GetUserLoginForDetail
        Dim strSQL As New Text.StringBuilder

        Try
            ' variable keep sql statement, datatable and ilist
            Dim objDT As System.Data.DataTable
            Dim objDR As System.Data.DataRow

            GetUserLoginForDetail = Nothing

            ' set new object
            objConn = New Common.DBConnection.MySQLAccess
            strSQL = New Text.StringBuilder

            ' assign sql statement and parameter
            With strSQL
                .AppendLine(" SELECT A.user_name")
                .AppendLine(" 	,A.password ")
                .AppendLine(" 	,A.first_name ")
                .AppendLine(" 	,A.last_name ")
                .AppendLine(" 	,B.name ")
                .AppendLine(" 	,concat(C.first_name,' ',C.last_name) account_next_approve ")
                .AppendLine(" 	,concat(D.first_name,' ',D.last_name) purchase_next_approve ")
                .AppendLine(" 	,concat(E.first_name,' ',E.last_name) outsource_next_approve ")
                .AppendLine(" FROM user A left join mst_department B on A.department_id=B.id ")
                .AppendLine(" LEFT JOIN user C on A.account_next_approve=C.id ")
                .AppendLine(" LEFT JOIN user D on A.purchase_next_approve=D.id ")
                .AppendLine(" LEFT JOIN user E on A.outsource_next_approve=E.id ")
                .AppendLine(" WHERE A.id=?id")

            End With

            objConn.AddParameter("?id", intUserLoginID)
            ' set new object
            objDT = New System.Data.DataTable

            ' execute by datatable
            objDT = objConn.ExecuteDataTable(strSQL.ToString)
            strMsgErr = objConn.MessageError

            ' check data
            If objDT Is Nothing Then Exit Function
            If objDT.Rows.Count = 0 Then Exit Function
            objDR = objDT.Rows(0)
            GetUserLoginForDetail = New Entity.ImpUserLoginEntity

            ' assign value to entity object
            With GetUserLoginForDetail
                .user_name = objConn.CheckDBNull(objDR("user_name"), Common.DBConnection.DBType.DBString)
                .password = objConn.CheckDBNull(objDR("password"), Common.DBConnection.DBType.DBString)
                .first_name = objConn.CheckDBNull(objDR("first_name"), Common.DBConnection.DBType.DBString)
                .last_name = objConn.CheckDBNull(objDR("last_name"), Common.DBConnection.DBType.DBString)
                .name = objConn.CheckDBNull(objDR("name"), Common.DBConnection.DBType.DBString)
                .account_next_approve = objConn.CheckDBNull(objDR("account_next_approve"), Common.DBConnection.DBType.DBString)
                .purchase_next_approve = objConn.CheckDBNull(objDR("purchase_next_approve"), Common.DBConnection.DBType.DBString)
                .outsource_next_approve = objConn.CheckDBNull(objDR("outsource_next_approve"), Common.DBConnection.DBType.DBString)
            End With

        Catch ex As Exception
            ' write error log
            GetUserLoginForDetail = Nothing
            objLog.ErrorLog("GetUserLoginForDetail(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            ' write sql statement
            objLog.InfoLog("GetUserLoginForDetail(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
        Finally
            If Not objConn Is Nothing Then objConn.Close()
        End Try
    End Function

#End Region

End Class
