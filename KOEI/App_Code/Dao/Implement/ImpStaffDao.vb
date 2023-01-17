
#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpStaffDao
'	Class Discription	: Class of table mst_staff
'	Create User 		: Nisa S.
'	Create Date		    : 04-07-2013
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
Imports System.Exception
Imports Exceptions

#End Region

Namespace Dao
    Public Class ImpStaffDao
        Implements IStaffDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

#Region "Function"
        '/**************************************************************
        '	Function name	: GetStaffList
        '	Discription	    : Get Staff list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetStaffList( _
            ByVal strID As String, _
            ByVal strFirstName As String, _
            ByVal strLastName As String, _
            ByVal strWorkCategoryID As String _
        ) As System.Collections.Generic.List(Of Entity.ImpStaffDetailEntity) Implements IStaffDao.GetStaffList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetStaffList = New List(Of Entity.ImpStaffDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objStaffDetail As Entity.ImpStaffDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT A.id									")
                    .AppendLine("			,first_name							")
                    .AppendLine("			,last_name						")
                    .AppendLine("			,B.name as section 						")
                    .AppendLine("	FROM mst_staff A  									")
                    .AppendLine("	LEFT JOIN mst_work_category B on A.work_category_id=B.id			")
                    .AppendLine("	WHERE A.delete_fg<>1 ")
                    .AppendLine("	AND (A.id = IFNULL(?id, A.id)) 					")
                    .AppendLine("   AND (ISNULL(?first_name) OR A.first_name like CONCAT('%', ?first_name, '%'))									")
                    .AppendLine("	AND (ISNULL(?last_name) OR A.last_name like CONCAT('%', ?last_name, '%')) 									")
                    .AppendLine("	AND (ISNULL(?section) OR A.work_category_id=?section)   									")
                    .AppendLine("	ORDER BY A.id;									")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(strID), DBNull.Value, strID))
                objConn.AddParameter("?first_name", IIf(String.IsNullOrEmpty(strFirstName), DBNull.Value, strFirstName))
                objConn.AddParameter("?last_name", IIf(String.IsNullOrEmpty(strLastName), DBNull.Value, strLastName))
                objConn.AddParameter("?section", IIf(String.IsNullOrEmpty(strWorkCategoryID), DBNull.Value, strWorkCategoryID))
                'If String.IsNullOrEmpty(strWorkCategoryID) Then
                '    objConn.AddParameter("?section", DBNull.Value)
                'Else
                '    objConn.AddParameter("?section", CInt(strWorkCategoryID))
                'End If


                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objStaffDetail = New Entity.ImpStaffDetailEntity
                        ' assign data from db to entity object
                        With objStaffDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .first_name = IIf(IsDBNull(dr.Item("first_name")), Nothing, dr.Item("first_name"))
                            .last_name = IIf(IsDBNull(dr.Item("last_name")), Nothing, dr.Item("last_name"))
                            .work_category_id = IIf(IsDBNull(dr.Item("section")), Nothing, dr.Item("section"))

                        End With
                        ' add Staff to list
                        GetStaffList.Add(objStaffDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetStaffList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetStaffList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteStaff
        '	Discription	    : Delete Staff
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteStaff( _
            ByVal intStaffID As Integer _
        ) As Integer Implements IStaffDao.DeleteStaff
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteStaff = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE mst_staff                             ")
                    .AppendLine("		SET delete_fg=1,							")
                    .AppendLine("		    updated_by=?loginid,							")
                    .AppendLine("		    updated_date=date_format(now(),'%Y%m%d%H%i%s')							")
                    .AppendLine("		WHERE id = ?id							")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?loginid", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intStaffID)
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
                DeleteStaff = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteStaff(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteStaff(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetStaffByID
        '	Discription	    : Get Staff by ID
        '	Return Value	: IStaffEntity Object
        '	Create User	    : Nisa S.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetStaffByID( _
            ByVal intStaffID As Integer _
        ) As Entity.IStaffEntity Implements IStaffDao.GetStaffByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetStaffByID = New Entity.ImpStaffEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	id	")
                    .AppendLine("		,first_name	")
                    .AppendLine("		,last_name	")
                    .AppendLine("		,work_category_id	")
                    .AppendLine("	FROM mst_staff		")
                    .AppendLine("	WHERE delete_fg <> 1		")
                    .AppendLine("	AND Id=?id		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intStaffID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetStaffByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .first_name = IIf(IsDBNull(dr.Item("first_name")), Nothing, dr.Item("first_name"))
                            .last_name = IIf(IsDBNull(dr.Item("last_name")), Nothing, dr.Item("last_name"))
                            .work_category_id = IIf(IsDBNull(dr.Item("work_category_id")), Nothing, dr.Item("work_category_id"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetStaffByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetStaffByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertStaff
        '	Discription	    : Insert Staff to mst_staff
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertStaff( _
            ByVal objStaffEnt As Entity.IStaffEntity _
        ) As Integer Implements IStaffDao.InsertStaff
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertStaff = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		INSERT INTO mst_staff (first_name						")
                    .AppendLine("				,last_name				")
                    .AppendLine("				,work_category_id				")
                    .AppendLine("				,delete_fg				")
                    .AppendLine("				,created_by				")
                    .AppendLine("				,created_date				")
                    .AppendLine("				,updated_by				")
                    .AppendLine("				,updated_date)				")
                    .AppendLine("		VALUES (?first_name						")
                    .AppendLine("			,?last_name					")
                    .AppendLine("			,?section					")
                    .AppendLine("			,0					")
                    .AppendLine("			,?loginid					")
                    .AppendLine("			,date_format(now(),'%Y%m%d%H%i%s')					")
                    .AppendLine("	        ,?updated_by ")
                    .AppendLine("           ,DATE_FORMAT(NOW(),'%Y%m%d%H%i%s'));					")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?first_name", objStaffEnt.first_name)
                    .AddParameter("?last_name", objStaffEnt.last_name)
                    .AddParameter("?section", objStaffEnt.work_category_id)
                    .AddParameter("?loginid", HttpContext.Current.Session("UserID"))
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
                InsertStaff = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateStaff(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertStaff(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertStaff(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateStaff
        '	Discription	    : Update Staff to mst_staff 
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateStaff( _
            ByVal objStaffEnt As Entity.IStaffEntity _
        ) As Integer Implements IStaffDao.UpdateStaff
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateStaff = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE mst_staff							")
                    .AppendLine("		SET first_name = ?first_name							")
                    .AppendLine("			,last_name = ?last_name 						")
                    .AppendLine("			,work_category_id = ?section						")
                    .AppendLine("		    ,updated_by = ?loginid						")
                    .AppendLine("		    ,updated_date = date_format(now(),'%Y%m%d%H%i%s')							")
                    .AppendLine("		WHERE id = ?id;							")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?first_name", objStaffEnt.first_name)
                    .AddParameter("?last_name", objStaffEnt.last_name)
                    .AddParameter("?section", objStaffEnt.work_category_id)
                    .AddParameter("?loginid", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objStaffEnt.id)

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
                UpdateStaff = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateStaff(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateStaff(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateStaff(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInPO
        '	Discription	    : Count Staff in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInPO( _
            ByVal intStaffID As Integer _
        ) As Integer Implements IStaffDao.CountUsedInPO
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(staff_id) AS used_count 				")
                    .AppendLine("		FROM wh_header 				")
                    .AppendLine("		WHERE staff_id = ?id			")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intStaffID)

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
        ' Function name   : CheckDupStaff
        ' Discription     : Check duplication Staff Master
        ' Return Value    : Integer
        ' Create User     : Nisa S.
        ' Create Date     : 04-07-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupInsert( _
            ByVal strfirst_name As String, _
            ByVal strlast_name As String _
        ) As Integer Implements IStaffDao.CheckDupInsert
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckDupInsert = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("  SELECT *     ")
                    .AppendLine("  FROM mst_staff     ")
                    .AppendLine("  WHERE delete_fg <> 1   ")
                    .AppendLine("   AND first_name=?first_name  ")
                    .AppendLine("   AND last_name=?last_name     ")
                    '.AppendLine("   AND work_category_id=?section     ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?first_name", strfirst_name)
                objConn.AddParameter("?last_name", strlast_name)
                'objConn.AddParameter("?section", strsection)

                ' execute sql command
                CheckDupInsert = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("CheckDupInsert(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupInsert(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckDupInsert(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupUpdate
        ' Discription     : Check duplication Staff Master
        ' Return Value    : Integer
        ' Create User     : Nisa S.
        ' Create Date     : 04-07-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupUpdate( _
            ByVal intStaffID As String, _
            ByVal strfirst_name As String, _
            ByVal strlast_name As String _
        ) As Integer Implements IStaffDao.CheckDupUpdate
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckDupUpdate = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("  SELECT *     ")
                    .AppendLine("  FROM mst_staff     ")
                    .AppendLine("  WHERE delete_fg <> 1   ")
                    .AppendLine("   AND id<>?id  ")
                    .AppendLine("   AND first_name=?first_name  ")
                    .AppendLine("   AND last_name=?last_name     ")
                    '.AppendLine("   AND work_category_id=?section     ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intStaffID)
                objConn.AddParameter("?first_name", strfirst_name)
                objConn.AddParameter("?last_name", strlast_name)
                'objConn.AddParameter("?section", strsection)

                ' execute sql command
                CheckDupUpdate = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("CheckDupUpdate(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupUpdate(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckDupUpdate(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetWorkCategoryForList
        '	Discription	    : Get data WorkCategory for set dropdownlist
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 05-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkCategoryForList() As System.Collections.Generic.List(Of Entity.IStaffEntity) Implements IStaffDao.GetWorkCategoryForList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetWorkCategoryForList = New List(Of Entity.IStaffEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable vendor entity
                Dim objWorkCategoryEnt As Entity.IStaffEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT id 		")
                    .AppendLine("		 ,name 	")
                    .AppendLine("	FROM mst_work_category 		")
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
                        ' new WorkCategory entity
                        objWorkCategoryEnt = New Entity.ImpStaffEntity
                        With objWorkCategoryEnt
                            ' assign data to object WorkCategory entity
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                        ' add object WorkCategory entity to list
                        GetWorkCategoryForList.Add(objWorkCategoryEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkCategoryForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetWorkCategoryForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetStaffForList
        '	Discription	    : Get data Staff for set dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetStaffForList() As System.Collections.Generic.List(Of Entity.IStaffEntity) Implements IStaffDao.GetStaffForList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetStaffForList = New List(Of Entity.IStaffEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable staff entity
                Dim objStaffEnt As Entity.IStaffEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT id 		")
                    .AppendLine("		 ,concat(first_name,' ',last_name) as name 	")
                    .AppendLine("	FROM mst_staff 		")
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
                        ' new WorkCategory entity
                        objStaffEnt = New Entity.ImpStaffEntity
                        With objStaffEnt
                            ' assign data to object WorkCategory entity
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                        ' add object WorkCategory entity to list
                        GetStaffForList.Add(objStaffEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetStaffForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetStaffForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function
#End Region

    End Class
End Namespace