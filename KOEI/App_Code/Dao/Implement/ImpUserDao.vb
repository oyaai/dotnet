#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpUserDao
'	Class Discription	: Class of table user
'	Create User 		: Boon
'	Create Date		    : 15-05-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

#End Region

Namespace Dao
    Public Class ImpUserDao
        Implements IUserDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: DB_CheckApproveUser
        '	Discription	    : Check approve user
        '	Return Value	: Integer
        '	Create User	    : Boonyarit
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckApproveUser(ByVal objType As Enums.PurchaseTypes, ByVal intUser_id As Integer) As Integer Implements IUserDao.DB_CheckApproveUser
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim objDT As System.Data.DataTable
                Dim objDR As System.Data.DataRow

                DB_CheckApproveUser = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder
                objDT = New System.Data.DataTable

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT purchase_next_approve As P_Approve ")
                    .AppendLine(" 	,outsource_next_approve As O_Approve ")
                    .AppendLine(" FROM user ")
                    .AppendLine(" WHERE id = ?user_id; ")

                    ' assign parameter
                    objConn.AddParameter("?user_id", intUser_id)
                End With

                ' execute by datatable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError

                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                objDR = objDT.Rows(0)

                ' check value
                Select Case objType
                    Case Enums.PurchaseTypes.Purchase
                        DB_CheckApproveUser = IIf(IsDBNull(objDR("P_Approve")), 0, CInt(objDR("P_Approve")))
                    Case Enums.PurchaseTypes.OutSource
                        DB_CheckApproveUser = IIf(IsDBNull(objDR("O_Approve")), 0, CInt(objDR("O_Approve")))
                    Case Else

                End Select

            Catch ex As Exception
                ' write error log
                DB_CheckApproveUser = False
                objLog.ErrorLog("DB_CheckApproveUser(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CheckApproveUser(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_AddUser
        '	Discription	    : Add user
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 03-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_AddUser(ByVal objUser As Entity.IUserEntity, ByRef intUser_id As Integer) As Boolean Implements IUserDao.DB_AddUser
            Return False
        End Function

        '/**************************************************************
        '	Function name	: DB_DeleteUser
        '	Discription	    : Delete user
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 03-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_DeleteUser(ByVal intUser_id As Integer) As Boolean Implements IUserDao.DB_DeleteUser
            Return False
        End Function

        '/**************************************************************
        '	Function name	: DB_SearchUser
        '	Discription	    : Search user
        '	Return Value	: IUserEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 03-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_SearchUser(ByVal intUser_id As Integer) As Entity.IUserEntity Implements IUserDao.DB_SearchUser
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable keep sql statement, datatable and datarow
                Dim objDT As System.Data.DataTable
                Dim objDR As System.Data.DataRow

                DB_SearchUser = Nothing

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT * ")
                    .AppendLine(" FROM user AS U ")
                    .AppendLine(" WHERE U.id = ?user_id ")
                    ' assign parameter
                    objConn.AddParameter("?user_id", intUser_id)
                End With

                objDT = New System.Data.DataTable
                ' execute by datatable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError

                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_SearchUser = New Entity.ImpUserEntity
                objDR = objDT.Rows(0)

                ' assign value to entity object
                With DB_SearchUser
                    .user_name = objConn.CheckDBNull(objDR("user_name"), Common.DBConnection.DBType.DBString)
                    .password = objConn.CheckDBNull(objDR("password"), Common.DBConnection.DBType.DBString)
                    .first_name = objConn.CheckDBNull(objDR("first_name"), Common.DBConnection.DBType.DBString)
                    .last_name = objConn.CheckDBNull(objDR("last_name"), Common.DBConnection.DBType.DBString)
                    .delete_fg = objConn.CheckDBNull(objDR("delete_fg"), Common.DBConnection.DBType.DBDecimal)
                End With

            Catch ex As Exception
                ' write error log
                DB_SearchUser = Nothing
                objLog.ErrorLog("DB_SearchUser(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_SearchUser(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_UpdateUser
        '	Discription	    : Update user
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 03-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_UpdateUser(ByVal objUser As Entity.IUserEntity) As Boolean Implements IUserDao.DB_UpdateUser
            Return False
        End Function

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
        ) As Entity.IUserEntity Implements IUserDao.GetUserLogin
            ' variable for keep sql statement
            Dim strSql As New Text.StringBuilder
            ' new user entity
            GetUserLogin = New Entity.ImpUserEntity
            Try
                ' variable for read data from mysql
                Dim dr As MySqlDataReader
                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT u.*				")
                    .AppendLine("		,d.name AS department_name			")
                    .AppendLine("	FROM user u				")
                    .AppendLine("	LEFT JOIN  mst_department d 				")
                    .AppendLine("	ON u.department_id = d.id				")
                    .AppendLine("	WHERE u.delete_fg <> 1 				")
                    .AppendLine("	AND ((ISNULL(?user_name) OR u.user_name = ?user_name )				")
                    .AppendLine("	AND (ISNULL(?password) OR u.password = ?password)	)			")
                    .AppendLine("	OR ISNULL(?id) OR u.id = ?id				")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' set parameter
                objConn.AddParameter("?user_name", strUserName)
                objConn.AddParameter("?password", strPassword)
                objConn.AddParameter("?id", strUserId)

                ' read data
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign value to user entity
                        With GetUserLogin
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .user_name = IIf(IsDBNull(dr.Item("user_name")), Nothing, dr.Item("user_name"))
                            .department_name = IIf(IsDBNull(dr.Item("department_name")), Nothing, dr.Item("department_name"))
                            .account_next_approve = IIf(IsDBNull(dr.Item("account_next_approve")), Nothing, dr.Item("account_next_approve"))
                            .purchase_next_approve = IIf(IsDBNull(dr.Item("purchase_next_approve")), Nothing, dr.Item("purchase_next_approve"))
                            .outsource_next_approve = IIf(IsDBNull(dr.Item("outsource_next_approve")), Nothing, dr.Item("outsource_next_approve"))
                        End With
                    End While
                End If
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("GetUserLogin(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' Write sql statement
                objLog.ErrorLog("GetUserLogin(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' close connection
                If Not IsNothing(objConn) Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateLastLogin
        '	Discription	    : Update last login
        '	Return Value	: Integer
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateLastLogin(ByVal intUserID As Integer) As Integer Implements IUserDao.UpdateLastLogin
            ' variable for keep sql statement
            Dim strSql As New Text.StringBuilder
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql statement
                With strSql
                    .AppendLine("		UPDATE user					")
                    .AppendLine("		SET last_login=?last_login					")
                    .AppendLine("			 ,updated_by=?UserID				")
                    .AppendLine("			 ,updated_date= ?updated_date				")
                    .AppendLine("		where id=?UserID				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' begin transaction
                objConn.BeginTrans()
                ' assign parameter list
                objConn.AddParameter("?last_login", Now.ToString("yyyyMMddHHmmss"))
                objConn.AddParameter("?UserID", intUserID)
                objConn.AddParameter("?updated_date", Now.ToString("yyyyMMddHHmmss"))

                ' execute sql statement
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' check row effect and commit transaction
                If intEff > 0 Then
                    objConn.CommitTrans()
                Else
                    objConn.RollbackTrans()
                End If

                ' return row effect
                Return intEff
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("UpdateLastLogin(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("UpdateLastLogin(Dao)", strSql.ToString)
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckUserByDepartment
        '	Discription	    : Check use of department
        '	Return Value	: Integer
        '	Create User	    : Charoon
        '	Create Date	    : 30-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckUserByDepartment(ByVal intDepartment_id As Integer) As Integer Implements IUserDao.CheckUserByDepartment
            Try
                Dim strSQL As Text.StringBuilder
                Dim intFlagCount As Integer = 0
                Dim objListParam As New List(Of MySqlParameter)

                CheckUserByDepartment = 0

                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder
                With strSQL
                    .AppendLine("SELECT Count(*) As department_count")
                    .AppendLine("FROM user")
                    .AppendLine("WHERE delete_fg <> 1")
                    .AppendLine("AND department_id = ?id")
                    objListParam.Add(New MySqlParameter("?id", intDepartment_id))
                End With
                objConn.AddParameter(objListParam)
                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError

                CheckUserByDepartment = intFlagCount

            Catch ex As Exception
                objLog.ErrorLog("CheckUserByDepartment", ex.Message.Trim)
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetUserForList
        '	Discription	    : Get data user for set dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 17-06-2013
        '	Update User	    : Wasan D.
        '	Update Date	    : 25-10-2013
        '*************************************************************/
        Public Function GetUserForList(Optional ByVal strDepartmentName As String = Nothing) _
        As System.Collections.Generic.List(Of Entity.IUserEntity) Implements IUserDao.GetUserForList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetUserForList = New List(Of Entity.IUserEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable user entity
                Dim objUserEnt As Entity.IUserEntity

                ' assign sql statement
                With strSql
                    .AppendLine("   SELECT A.id, CONCAT(first_name, ' ', last_name) AS user_name				")
                    .AppendLine("	FROM user A join mst_department B ON A.department_id = B.id					")
                    .AppendLine("	WHERE A.delete_fg <> 1 AND (ISNULL(?department) OR B.name = ?department)	")
                    .AppendLine("	ORDER BY A.first_name;														")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                objConn.AddParameter("?department", IIf(strDepartmentName = Nothing, DBNull.Value, strDepartmentName))
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new user entity
                        objUserEnt = New Entity.ImpUserEntity
                        With objUserEnt
                            ' assign data to object user entity
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .user_name = IIf(IsDBNull(dr.Item("user_name")), Nothing, dr.Item("user_name"))
                        End With
                        ' add object user entity to list
                        GetUserForList.Add(objUserEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetUserForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetUserForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

    End Class
End Namespace

