#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpMst_WorkingCategoryDao
'	Class Discription	: Class of table mst_WorkingCategory
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 04-06-2013
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

#End Region

Namespace Dao
    Public Class ImpMst_WorkingCategoryDao
        Implements IMst_WorkingCategoryDao


        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: GetWorkingCategoryList
        '	Discription	    : Get WorkingCategory list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkingCategoryList( _
            ByVal strWorkingCategoryName As String _
        ) As System.Collections.Generic.List(Of Entity.ImpMst_WorkingCategoryDetailEntity) Implements IMst_WorkingCategoryDao.GetWorkingCategoryList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetWorkingCategoryList = New List(Of Entity.ImpMst_WorkingCategoryDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objWorkingCategoryDetail As Entity.ImpMst_WorkingCategoryDetailEntity

                With strSql
                    .AppendLine("	SELECT 					")
                    .AppendLine("			mst.id AS item_id							")
                    .AppendLine("			,mst.name AS item_name							")
                    .AppendLine("			,(CASE WHEN IFNULL(po.used_count, 0) <= 0 THEN 0 ELSE 1 END) AS in_used	")
                    .AppendLine("	FROM mst_work_category mst 						")
                    .AppendLine("	LEFT JOIN (SELECT work_category_id									")
                    .AppendLine("		,COUNT(work_category_id) AS used_count 								")
                    .AppendLine("	 FROM wh_header po 									")
                    .AppendLine("	 GROUP BY work_category_id) po ON mst.id = po.work_category_id									")
                    .AppendLine("	WHERE (ISNULL(?category_name) OR mst.name LIKE CONCAT('%', ?category_name , '%'))									")
                    .AppendLine("	AND mst.delete_fg <> 1								")
                    .AppendLine("	ORDER BY mst.id ;									")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?category_name", IIf(String.IsNullOrEmpty(strWorkingCategoryName), DBNull.Value, strWorkingCategoryName))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objWorkingCategoryDetail = New Entity.ImpMst_WorkingCategoryDetailEntity
                        ' assign data from db to entity object
                        With objWorkingCategoryDetail
                            .id = IIf(IsDBNull(dr.Item("item_id")), Nothing, dr.Item("item_id"))
                            .name = IIf(IsDBNull(dr.Item("item_name")), Nothing, dr.Item("item_name"))
                            .inuse = IIf(IsDBNull(dr.Item("in_used")), Nothing, dr.Item("in_used"))
                        End With
                        ' add WorkingCategory to list
                        GetWorkingCategoryList.Add(objWorkingCategoryDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingCategoryList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetWorkingCategoryList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteWorkingCategory
        '	Discription	    : Delete WorkingCategory
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteWorkingCategory( _
            ByVal intWorkingCategoryID As Integer _
        ) As Integer Implements IMst_WorkingCategoryDao.DeleteWorkingCategory
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteWorkingCategory = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE mst_work_category                              ")
                    .AppendLine("		SET delete_fg = 1,							")
                    .AppendLine("		    updated_by = ?update_by,							")
                    .AppendLine("		    updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE id = ?id							")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?update_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intWorkingCategoryID)
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
                DeleteWorkingCategory = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteWorkingCategory(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteWorkingCategory(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetWorkingCategoryByID
        '	Discription	    : Get WorkingCategory by ID
        '	Return Value	: IMst_WorkingCategoryEntity Object
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkingCategoryByID( _
            ByVal intWorkingCategoryID As Integer _
        ) As Entity.IMst_WorkingCategoryEntity Implements IMst_WorkingCategoryDao.GetWorkingCategoryByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetWorkingCategoryByID = New Entity.ImpMst_WorkingCategoryEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	id	")
                    .AppendLine("		,name	")
                    .AppendLine("	FROM mst_work_category		")
                    .AppendLine("	WHERE delete_fg<>1 		")
                    .AppendLine("	AND id = ?item_id		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?item_id", intWorkingCategoryID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetWorkingCategoryByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingCategoryByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetWorkingCategoryByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertWorkingCategory
        '	Discription	    : Insert WorkingCategory to mst_Work_Category
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertWorkingCategory( _
            ByVal objWorkingCategoryEnt As Entity.IMst_WorkingCategoryEntity _
        ) As Integer Implements IMst_WorkingCategoryDao.InsertWorkingCategory
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertWorkingCategory = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		INSERT INTO mst_work_category   ")
                    .AppendLine("		(					            ")
                    .AppendLine("		    name						")
                    .AppendLine("			,delete_fg				    ")
                    .AppendLine("			,created_by				    ")
                    .AppendLine("			,created_date				")
                    .AppendLine("			,updated_by				    ")
                    .AppendLine("			,updated_date				")
                    .AppendLine("		)					            ")
                    .AppendLine("		VALUES                          ")
                    .AppendLine("		(                               ")
                    .AppendLine("		    ?name						")
                    .AppendLine("			,0					        ")
                    .AppendLine("			,?created_by				")
                    .AppendLine("			,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')					")
                    .AppendLine("			,NULL					    ")
                    .AppendLine("			,NULL 		                ")
                    .AppendLine("		);					            ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?name", objWorkingCategoryEnt.name)
                    .AddParameter("?created_by", HttpContext.Current.Session("UserID"))

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
                InsertWorkingCategory = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertWorkingCategory(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertWorkingCategory(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertWorkingCategory(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateWorkingCategory
        '	Discription	    : Update WorkingCategory to mst_Work_Category
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateWorkingCategory( _
            ByVal objWorkingCategoryEnt As Entity.IMst_WorkingCategoryEntity _
        ) As Integer Implements IMst_WorkingCategoryDao.UpdateWorkingCategory
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateWorkingCategory = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE mst_work_category							")
                    .AppendLine("		SET name = ?name							")
                    .AppendLine("		  ,updated_by = ?updated_by							")
                    .AppendLine("		  ,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE (id = ?id);							")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?name", objWorkingCategoryEnt.name)
                    .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objWorkingCategoryEnt.id)

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
                UpdateWorkingCategory = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateWorkingCategory(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateWorkingCategory(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateWorkingCategory(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: CountUsedInPO
        '	Discription	    : Count item in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInPO( _
            ByVal intItemID As Integer _
        ) As Integer Implements IMst_WorkingCategoryDao.CountUsedInPO
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(work_category_id) AS used_count 				")
                    .AppendLine("		FROM wh_header po 				")
                    .AppendLine("		WHERE po.work_category_id = ?item_id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?item_id", intItemID)

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
        '	Function name	: CountUsedInPO
        '	Discription	    : Count item in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckDupWorkCategory( _
            ByVal itemName_new As String, _
            ByVal id As String _
        ) As Integer Implements IMst_WorkingCategoryDao.CheckDupWorkCategory
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckDupWorkCategory = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(id) AS used_count 				")
                    .AppendLine("		FROM mst_work_category  				")
                    .AppendLine("		WHERE  delete_fg <> 1				")
                    .AppendLine("		AND UPPER(name) = ?item_new	")
                    If id <> "" Then
                        .AppendLine("		AND id <> ?id				")
                    End If
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?item_new", itemName_new.ToUpper)
                If id <> "" Then
                    objConn.AddParameter("?id", id)
                End If

                ' execute sql command
                CheckDupWorkCategory = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("CheckDupWorkCategory(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupWorkCategory(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckDupWorkCategory(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        
    End Class
End Namespace

