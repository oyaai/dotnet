#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpMst_JobTypeDao
'	Class Discription	: Class of table mst_job_type
'	Create User 		: Suwishaya L.
'	Create Date		    : 11-06-2013
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
    Public Class ImpMst_JobTypeDao
        Implements IMst_JobTypeDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log

#Region "Function"

        '/**************************************************************
        '	Function name	: GetJobTypeForList
        '	Discription	    : Get data job type for set dropdownlist
        '	Return Value	: List 
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobTypeForList() As System.Collections.Generic.List(Of Entity.IMst_JobTypeEntity) Implements IMst_JobTypeDao.GetJobTypeForList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetJobTypeForList = New List(Of Entity.IMst_JobTypeEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable job type entity
                Dim objJobtypeEnt As Entity.IMst_JobTypeEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT id, 		")
                    .AppendLine("		 name 	")
                    .AppendLine("	FROM mst_job_type 		")
                    .AppendLine("	WHERE delete_fg <> 1 		")
                    .AppendLine("	ORDER BY id		")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new job type entity
                        objjobtypeEnt = New Entity.ImpMst_JobTypeEntity
                        With objjobtypeEnt
                            ' assign data to object job type entity
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                        ' add object job type entity to list
                        GetJobTypeForList.Add(objjobtypeEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobTypeForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetJobTypeForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobTypeList
        '	Discription	    : Get JobType list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobTypeList(ByVal strJobType As String) _
        As System.Collections.Generic.List(Of Entity.IMst_JobTypeEntity) Implements IMst_JobTypeDao.GetJobTypeList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetJobTypeList = New List(Of Entity.IMst_JobTypeEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobTypeDetail As Entity.ImpMst_JobTypeEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT id ")
                    .AppendLine(" 	,name ")
                    .AppendLine(" FROM mst_job_type ")
                    .AppendLine(" WHERE delete_fg <> 1 ")
                    .AppendLine("	AND (ISNULL(?job_type) OR name LIKE CONCAT('%', ?job_type , '%'))	")
                    .AppendLine(" ORDER BY id ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_type", IIf(String.IsNullOrEmpty(strJobType), DBNull.Value, strJobType))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobTypeDetail = New Entity.ImpMst_JobTypeEntity
                        ' assign data from db to entity object
                        With objJobTypeDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))

                        End With
                        ' add JobType to list
                        GetJobTypeList.Add(objJobTypeDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobTypeList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobTypeList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInPO
        '	Discription	    : Count Job Type in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInPO( _
            ByVal intJobTypeID As Integer _
        ) As Integer Implements IMst_JobTypeDao.CountUsedInPO
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(job.job_type_id) AS used_count 				")
                    .AppendLine("		FROM job_order job 				")
                    .AppendLine("		WHERE job.job_type_id = ?job_type_id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_type_id", intJobTypeID)

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
        '	Function name	: DeleteJobType
        '	Discription	    : Delete JobType
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobType( _
            ByVal intJobTypeID As Integer _
        ) As Integer Implements IMst_JobTypeDao.DeleteJobType
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteJobType = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE mst_job_type                             ")
                    .AppendLine("		SET delete_fg = 1,							")
                    .AppendLine("		    updated_by=?user_id,							")
                    .AppendLine("		    updated_date=date_format(now(),'%Y%m%d%H%i%s')  							")
                    .AppendLine("		WHERE id = ?id						")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intJobTypeID)
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
                DeleteJobType = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobType(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteJobType(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobTypeByID
        '	Discription	    : Get JobType by ID
        '	Return Value	: IMst_JobTypeEntity Object
        '	Create User	    : Nisa S.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobTypeByID( _
            ByVal intJobTypeID As Integer _
        ) As Entity.IMst_JobTypeEntity Implements IMst_JobTypeDao.GetJobTypeByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetJobTypeByID = New Entity.ImpMst_JobTypeEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT id	")
                    .AppendLine("		,name	")
                    .AppendLine("	FROM mst_job_type		")
                    .AppendLine("	WHERE delete_fg <> 1 AND Id=?id	")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intJobTypeID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetJobTypeByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobTypeByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobTypeByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertJobType
        '	Discription	    : Insert JobType to mst_job_type
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertJobType( _
            ByVal objJobTypeEntity As Entity.IMst_JobTypeEntity _
        ) As Integer Implements IMst_JobTypeDao.InsertJobType
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertJobType = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		INSERT INTO mst_job_type (name					")
                    .AppendLine("				,delete_fg				")
                    .AppendLine("				,created_by				")
                    .AppendLine("				,created_date				")
                    .AppendLine("				,updated_by				")
                    .AppendLine("				,updated_date)				")
                    .AppendLine("		VALUES (?job_type						")
                    .AppendLine("			,0					")
                    .AppendLine("			,?user_id					")
                    .AppendLine("			,date_format(now(),'%Y%m%d%H%i%s')					")
                    .AppendLine("	        ,?updated_by ")
                    .AppendLine("           ,DATE_FORMAT(NOW(),'%Y%m%d%H%i%s'));					")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?job_type", objJobTypeEntity.name)
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
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
                InsertJobType = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateJobType(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertJobType(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertJobType(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateJobType
        '	Discription	    : Update JobType to mst_job_type
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateJobType( _
            ByVal objJobTypeEntity As Entity.IMst_JobTypeEntity _
        ) As Integer Implements IMst_JobTypeDao.UpdateJobType
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateJobType = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE mst_job_type 							")
                    .AppendLine("		SET name = ?job_type							")
                    .AppendLine("		  ,updated_by=?user_id							")
                    .AppendLine("		  ,updated_date= date_format(now(),'%Y%m%d%H%i%s')							")
                    .AppendLine("		WHERE id = ?id;							")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?job_type", objJobTypeEntity.name)
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objJobTypeEntity.id)

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
                UpdateJobType = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateJobType(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateJobType(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateJobType(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupJobType
        ' Discription     : Check duplication JobType Master
        ' Return Value    : Integer
        ' Create User     : Nisa S.
        ' Create Date     : 03-07-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupJobType( _
            ByVal intJobTypeID As String, _
            ByVal intJobType As String _
        ) As Integer Implements IMst_JobTypeDao.CheckDupJobType
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckDupJobType = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("  SELECT COUNT(id) AS used_count     ")
                    .AppendLine("  FROM mst_job_type     ")
                    .AppendLine("  WHERE id<>?id   ")
                    .AppendLine("   AND delete_fg <> 1    ")
                    .AppendLine("   AND UPPER(name)=UPPER(?job_type)    ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intJobTypeID)
                objConn.AddParameter("?job_type", intJobType)

                ' execute sql command
                CheckDupJobType = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("CheckDupJobType(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupJobType(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckDupJobType(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

#End Region

    End Class
End Namespace
