#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpMst_SpecialJobOrderDao
'	Class Discription	: Class of table job_order_special
'	Create User 		: Suwishaya L.
'	Create Date		    : 07-06-2013
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
    Public Class ImpMst_SpecialJobOrderDao
        Implements IMst_SpecialJobOrderDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

#Region "Function"

        '/**************************************************************
        '	Function name	: CountUsedInSpecialJobOrder
        '	Discription	    : Count item in used accounting
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInSpecialJobOrder( _
            ByVal strJobOrderID As String _
        ) As Integer Implements IMst_SpecialJobOrderDao.CountUsedInSpecialJobOrder
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInSpecialJobOrder = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(job_order) AS used_count 				")
                    .AppendLine("		FROM accounting  				")
                    .AppendLine("		WHERE job_order = ?job_order				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrderID)

                ' execute sql command
                CountUsedInSpecialJobOrder = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInSpecialJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInSpecialJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInReceiveDetail 
        '	Discription	    : Count item in used receive_detail 
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInReceiveDetail( _
            ByVal strJobOrderID As String _
        ) As Integer Implements IMst_SpecialJobOrderDao.CountUsedInReceiveDetail
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInReceiveDetail = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(rd.id) AS cnt  				")
                    .AppendLine("		FROM receive_detail rd 				")
                    .AppendLine("       INNER JOIN job_order j  ")
                    .AppendLine("       ON rd.job_order_id = j.id ")
                    .AppendLine("		WHERE j.job_order = ?job_order				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrderID)

                ' execute sql command
                CountUsedInReceiveDetail = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInReceiveDetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInReceiveDetail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteSpecialJobOrder
        '	Discription	    : Delete Special Job Order 
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteSpecialJobOrder( _
            ByVal intJobOrderID As Integer _
        ) As Integer Implements IMst_SpecialJobOrderDao.DeleteSpecialJobOrder
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteSpecialJobOrder = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE job_order_special                             ")
                    .AppendLine("		SET delete_fg = 1,							")
                    .AppendLine("		    updated_by = ?update_by,							")
                    .AppendLine("		    updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE id = ?id							")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?update_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intJobOrderID)
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
                DeleteSpecialJobOrder = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteSpecialJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteSpecialJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSpecialJobOrderByID
        '	Discription	    : Get Special Job Order  by ID
        '	Return Value	: IMst_SpecialJobOrderEntiy Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSpecialJobOrderByID( _
            ByVal intJobOrderID As Integer _
        ) As Entity.IMst_SpecialJobOrderEntiy Implements IMst_SpecialJobOrderDao.GetSpecialJobOrderByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetSpecialJobOrderByID = New Entity.ImpMst_SpecialJobOrderEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	id	")
                    .AppendLine("		,job_order	")
                    .AppendLine("		,remark	")
                    .AppendLine("	FROM job_order_special		")
                    .AppendLine("	WHERE id = ?id		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intJobOrderID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetSpecialJobOrderByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSpecialJobOrderByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSpecialJobOrderByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSpecialJobOrderList
        '	Discription	    : Get Special Job Order  list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSpecialJobOrderList( _
            ByVal strJobOrderFrom As String, _
            ByVal strJobOrderTo As String _
        ) As System.Collections.Generic.List(Of Entity.ImpMst_SpecialJobOrderEntity) Implements IMst_SpecialJobOrderDao.GetSpecialJobOrderList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSpecialJobOrderList = New List(Of Entity.ImpMst_SpecialJobOrderEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderSpecial As Entity.ImpMst_SpecialJobOrderEntity

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT id ")
                    .AppendLine("			,job_order ")
                    .AppendLine("			,remark ")
                    .AppendLine("	FROM job_order_special  									")
                    .AppendLine("	WHERE delete_fg <> 1 								")
                    If strJobOrderFrom.Trim.Length > 0 And strJobOrderTo.Trim.Length = 0 Then
                        .AppendLine("	AND (ISNULL(?job_order_from) OR job_order >=  ?job_order_from ) 	")
                    ElseIf strJobOrderFrom.Trim.Length = 0 And strJobOrderTo.Trim.Length > 0 Then
                        .AppendLine("	AND (ISNULL(?job_order_to) OR job_order <=  ?job_order_to )    ")
                    Else
                        .AppendLine("	AND (ISNULL(?job_order_from) OR job_order >=  ?job_order_from ) 	")
                        .AppendLine("	AND (ISNULL(?job_order_to) OR job_order <=  ?job_order_to )    ")
                    End If
                    .AppendLine("	ORDER BY job_order;									")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order_from", IIf(String.IsNullOrEmpty(strJobOrderFrom), DBNull.Value, strJobOrderFrom))
                objConn.AddParameter("?job_order_to", IIf(String.IsNullOrEmpty(strJobOrderTo), DBNull.Value, strJobOrderTo))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderSpecial = New Entity.ImpMst_SpecialJobOrderEntity
                        ' assign data from db to entity object
                        With objJobOrderSpecial
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                        End With
                        ' add item to list
                        GetSpecialJobOrderList.Add(objJobOrderSpecial)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSpecialJobOrderList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSpecialJobOrderList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertSpecialJobOrder
        '	Discription	    : Insert Special Job Order to job_order_special 
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertSpecialJobOrder( _
            ByVal objSpecialJobOrderEnt As Entity.IMst_SpecialJobOrderEntiy _
        ) As Integer Implements IMst_SpecialJobOrderDao.InsertSpecialJobOrder
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertSpecialJobOrder = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		INSERT INTO job_order_special  (  ")
                    .AppendLine("		        job_order ")
                    .AppendLine("				,remark				")
                    .AppendLine("				,delete_fg				")
                    .AppendLine("				,created_by				")
                    .AppendLine("				,created_date				")
                    .AppendLine("				,updated_by				")
                    .AppendLine("				,updated_date)				")
                    .AppendLine("		VALUES ( ")
                    .AppendLine("		    ?job_order						")
                    .AppendLine("			,?remark					")
                    .AppendLine("			,0					")
                    .AppendLine("			,?created_by					")
                    .AppendLine("			,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')					")
                    .AppendLine("			,?created_by					")
                    .AppendLine("			,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')					")
                    .AppendLine("			);					")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?job_order", objSpecialJobOrderEnt.job_order)
                    .AddParameter("?remark", objSpecialJobOrderEnt.remark)
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
                InsertSpecialJobOrder = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertSpecialJobOrder(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertSpecialJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertSpecialJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateSpecialJobOrder
        '	Discription	    : Update item to job_order_special
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateSpecialJobOrder( _
            ByVal objSpecialJobOrderEnt As Entity.IMst_SpecialJobOrderEntiy _
        ) As Integer Implements IMst_SpecialJobOrderDao.UpdateSpecialJobOrder
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateSpecialJobOrder = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE job_order_special 				")
                    .AppendLine("		SET job_order = ?job_order							")
                    .AppendLine("			,remark = ?remark						")
                    .AppendLine("		  ,updated_by = ?updated_by							")
                    .AppendLine("		  ,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE (id = ?id);							")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?job_order", objSpecialJobOrderEnt.job_order)
                    .AddParameter("?remark", objSpecialJobOrderEnt.remark)
                    .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objSpecialJobOrderEnt.id)

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
                UpdateSpecialJobOrder = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateSpecialJobOrder(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateSpecialJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateSpecialJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckDupSpecialJobOrder
        '	Discription	    : Check data Special Job Order duplicate
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckDupSpecialJobOrder( _
          ByVal intJobOrderID As Integer, _
          ByVal strJobOrder As String _
        ) As Integer Implements IMst_SpecialJobOrderDao.CheckDupSpecialJobOrder

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder

            Try
                ' set default return value
                CheckDupSpecialJobOrder = 0

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	Count(job_order) AS jobOrder_count	")
                    .AppendLine("	FROM job_order_special 		")
                    .AppendLine("   WHERE id <> ?id ")
                    .AppendLine("	AND UPPER(job_order) = ?job_order ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intJobOrderID)
                objConn.AddParameter("?job_order", strJobOrder.ToUpper)

                ' execute sql command with data reader object
                CheckDupSpecialJobOrder = objConn.ExecuteScalar(strSql.ToString)

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupSpecialJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("CheckDupSpecialJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckDupJobOrder
        '	Discription	    : Check data Job Order duplicate
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckDupJobOrder( _
          ByVal strJobOrder As String _
        ) As Integer Implements IMst_SpecialJobOrderDao.CheckDupJobOrder

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder

            Try
                ' set default return value
                CheckDupJobOrder = 0

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	Count(id) AS cnt	")
                    .AppendLine("	FROM job_order 		")
                    .AppendLine("	WHERE job_order = ?job_order ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter 
                objConn.AddParameter("?job_order", strJobOrder)

                ' execute sql command with data reader object
                CheckDupJobOrder = objConn.ExecuteScalar(strSql.ToString)

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("CheckDupJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region

    End Class
End Namespace

