#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpMst_DepartmentDao
'	Class Discription	: Class of table mst_department
'	Create User 		: Charoon
'	Create Date		    : 30-05-2013
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
    Public Class ImpMst_DepartmentDao
        Implements IMst_DepartmentDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: GetDepartmentList
        '	Discription	    : Get Department list
        '	Return Value	: List
        '	Create User	    : Komsan L.
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDepartmentList( _
            ByVal strDepartmentName As String) As System.Collections.Generic.List(Of Entity.ImpMst_DepartmentDetailEntity) Implements IMst_DepartmentDao.GetDepartmentList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetDepartmentList = New List(Of Entity.ImpMst_DepartmentDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objDepartmentDetail As Entity.ImpMst_DepartmentDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine("SELECT id,name,case when ifnull(B.usecount,0)<1 then 0 else 1 end as inuse FROM mst_department A")
                    .AppendLine("left join (select department_id,count(*) as usecount from user group by department_id) B on A.id=B.department_id")
                    .AppendLine("WHERE delete_fg <> 1")
                    If (Not strDepartmentName Is Nothing) AndAlso strDepartmentName.Trim <> String.Empty Then
                        '.AppendLine("AND name = ?name")
                        'objListParam.Add(New MySqlParameter("?name", strName.Trim))
                        .AppendLine("AND name like '%" & strDepartmentName.Trim & "%'")
                    End If
                    .AppendLine("ORDER BY id")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objDepartmentDetail = New Entity.ImpMst_DepartmentDetailEntity
                        ' assign data from db to entity object
                        With objDepartmentDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                            .inuse = IIf(IsDBNull(dr.Item("inuse")), Nothing, dr.Item("inuse"))
                        End With
                        ' add Department to list
                        GetDepartmentList.Add(objDepartmentDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDepartmentList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetDepartmentList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteDepartment
        '	Discription	    : Delete Department
        '	Return Value	: Integer
        '	Create User	    : Komsan L.
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteDepartment( _
            ByVal intDepartmentID As Integer _
        ) As Integer Implements IMst_DepartmentDao.DeleteDepartment
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteDepartment = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("UPDATE mst_department SET delete_fg=1,updated_by = ?user_id,updated_date = date_format(now(),'%Y%m%d%H%i%s')")
                    .AppendLine("WHERE id = ?id")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intDepartmentID)
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
                DeleteDepartment = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteDepartment(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteDepartment(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDepartmentByID
        '	Discription	    : Get Department by ID
        '	Return Value	: IMst_DepartmentEntity Object
        '	Create User	    : Komsan L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDepartmentByID( _
            ByVal intDepartmentID As Integer _
        ) As Entity.IMst_DepartmentEntity Implements IMst_DepartmentDao.GetDepartmentByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetDepartmentByID = New Entity.ImpMst_DepartmentEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("SELECT id,name FROM mst_department")
                    .AppendLine("WHERE delete_fg <> 1")
                    .AppendLine("AND id = ?id")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intDepartmentID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetDepartmentByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDepartmentByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetDepartmentByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertDepartment
        '	Discription	    : Insert Department to mst_Department
        '	Return Value	: Integer
        '	Create User	    : Komsan L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertDepartment( _
            ByVal objDepartmentEnt As Entity.IMst_DepartmentEntity _
        ) As Integer Implements IMst_DepartmentDao.InsertDepartment
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertDepartment = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("INSERT INTO mst_department(name,delete_fg,created_by,created_date)")
                    .AppendLine("VALUES (?name,0,?user_id,date_format(now(),'%Y%m%d%H%i%s'))")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?name", objDepartmentEnt.name)
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))

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
                InsertDepartment = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateDepartment(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertDepartment(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertDepartment(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateDepartment
        '	Discription	    : Update Department to mst_Department
        '	Return Value	: Integer
        '	Create User	    : Komsan L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateDepartment( _
            ByVal objDepartmentEnt As Entity.IMst_DepartmentEntity _
        ) As Integer Implements IMst_DepartmentDao.UpdateDepartment
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateDepartment = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("UPDATE mst_department")
                    .AppendLine("SET name = ?name,updated_by = ?user_id,updated_date = date_format(now(),'%Y%m%d%H%i%s')")
                    .AppendLine("WHERE id = ?id")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?name", objDepartmentEnt.name)
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objDepartmentEnt.id)

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
                UpdateDepartment = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateDepartment(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateDepartment(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateDepartment(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInPO
        '	Discription	    : Count item in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    : Boonyarit
        '	Update Date	    : 14-08-2013
        '*************************************************************/
        Public Function CheckDupDepartment( _
            ByVal strDepartmentName As String, _
            Optional ByVal intDepartmentID As Integer = 0 _
        ) As Integer Implements IMst_DepartmentDao.CheckDupDepartment
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckDupDepartment = -1
            Try

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign sql command
                With strSql
                    If intDepartmentID > 0 Then
                        .AppendLine("SELECT COUNT(id) AS used_count")
                        .AppendLine("FROM mst_department")
                        .AppendLine("WHERE name = ?name and id <> ?id and delete_fg = 0;")

                        ' assign parameter
                        objConn.AddParameter("?name", strDepartmentName)
                        objConn.AddParameter("?id", intDepartmentID)
                    Else
                        .AppendLine("SELECT COUNT(id) AS used_count")
                        .AppendLine("FROM mst_department")
                        .AppendLine("WHERE name = ?name and delete_fg = 0;")

                        ' assign parameter
                        objConn.AddParameter("?name", strDepartmentName)
                    End If
                    
                End With

                ' execute sql command
                CheckDupDepartment = objConn.ExecuteScalar(strSql.ToString)

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupDepartment(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckDupDepartment(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDepartmentForDDList
        '	Discription	    : Get data Department for dropdownlist
        '	Return Value	: list
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDepartmentForDDList() As System.Collections.Generic.List(Of Entity.IMst_DepartmentEntity) Implements IMst_DepartmentDao.GetDepartmentForDDList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            ' object variable data reader
            Dim dr As MySqlDataReader
            ' object variable department entity
            Dim objDepEnt As Entity.IMst_DepartmentEntity

            'Set default return value
            GetDepartmentForDDList = New List(Of Entity.IMst_DepartmentEntity)
            Try
                'assign SQL Command
                strSql.AppendLine("SELECT id, name from mst_department WHERE delete_fg <> 1 ORDER By name")

                'new connection object
                objConn = New Common.DBConnection.MySQLAccess

                'execute sql command
                dr = objConn.ExecuteReader(strSql.ToString)


                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new job type entity
                        objDepEnt = New Entity.ImpMst_DepartmentEntity
                        With objDepEnt
                            ' assign data to object job type entity
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                        ' add object job type entity to list
                        GetDepartmentForDDList.Add(objDepEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDepartmentForDDList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetDepartmentForDDList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try

        End Function
    End Class
End Namespace

