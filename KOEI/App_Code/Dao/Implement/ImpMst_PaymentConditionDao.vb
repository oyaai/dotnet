#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpMst_PaymentConditionDao
'	Class Discription	: Class of table mst_payment_condition
'	Create User 		: Suwishaya L.
'	Create Date		    : 17-06-2013
'
' UPDATE INFORMATION
'	Update User		: Wasan D.
'	Update Date		: 03-07-2013
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
    Public Class ImpMst_PaymentConditionDao
        Implements IMst_PaymentConditionDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

#Region "Function"

        '/**************************************************************
        '	Function name	: GetPaymentConditionForList
        '	Discription	    : Get data Payment Condition for set dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentConditionForList() As System.Collections.Generic.List(Of Entity.IMst_PaymentConditionEntity) Implements IMst_PaymentConditionDao.GetPaymentConditionForList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetPaymentConditionForList = New List(Of Entity.IMst_PaymentConditionEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable job type entity
                Dim objPaymentConEnt As Entity.IMst_PaymentConditionEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT id, 		")
                    .AppendLine("		 CONCAT(1st,'%',2nd,'%',3rd,'%') as codition_name 	")
                    .AppendLine("	FROM mst_payment_condition 		")
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
                        objPaymentConEnt = New Entity.ImpMst_PaymentConditionEntity
                        With objPaymentConEnt
                            ' assign data to object job type entity
                            .condition_id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .condition_name = IIf(IsDBNull(dr.Item("codition_name")), Nothing, dr.Item("codition_name"))
                        End With
                        ' add object job type entity to list
                        GetPaymentConditionForList.Add(objPaymentConEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentConditionForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetPaymentConditionForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

        Public Function CountUsedInPO(ByVal intPayID As Integer) As Integer Implements IMst_PaymentConditionDao.CountUsedInPO
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO = -1
            Try
                ' assign sql command
                strSql.AppendLine("		SELECT * FROM job_order WHERE payment_condition_id=?payID        ")

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?payID", intPayID)

                ' execute sql command
                CountUsedInPO = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertPaymentCond(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                'Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInPO(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInPO(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        Public Function DeletePaymentCond(ByVal intPayID As Integer) As Integer Implements IMst_PaymentConditionDao.DeletePaymentCond

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeletePaymentCond = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql commandUPDATE mst_payment_condition 
                With strSql
                    .AppendLine("       UPDATE(mst_payment_condition)                   ")
                    .AppendLine("		SET delete_fg = 1,                              ")
                    .AppendLine("		updated_by=?user_id,                            ")
                    .AppendLine("		updated_date=date_format(now(),'%Y%m%d%H%i%s')  ")
                    .AppendLine("		WHERE id = ?id	                                ")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intPayID)
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
                DeletePaymentCond = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeletePaymentCond(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeletePaymentCond(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        Public Function GetPaymentCondByID(ByVal intPayID As Integer) As Entity.IMst_PaymentConditionEntity Implements IMst_PaymentConditionDao.GetPaymentCondByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetPaymentCondByID = New Entity.ImpMst_PaymentConditionEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	id AS PayID         ")
                    .AppendLine("		,1st AS	First           ")
                    .AppendLine("		,2nd AS Second          ")
                    .AppendLine("		,3rd AS Third           ")
                    .AppendLine("	FROM mst_payment_condition  ")
                    .AppendLine("	WHERE id = ?PayCondID		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?PayCondID", intPayID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetPaymentCondByID
                            .id = IIf(IsDBNull(dr.Item("PayID")), Nothing, dr.Item("PayID"))
                            .first = IIf(IsDBNull(dr.Item("First")), Nothing, dr.Item("First"))
                            .second = IIf(IsDBNull(dr.Item("Second")), Nothing, dr.Item("Second"))
                            .third = IIf(IsDBNull(dr.Item("Third")), Nothing, dr.Item("Third"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentCondByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPaymentCondByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        Public Function GetPaymentCondList(ByVal strFirst As String, ByVal strSecond As String, ByVal strThird As String) As System.Collections.Generic.List(Of Entity.ImpMst_PaymentConditionEntity) Implements IMst_PaymentConditionDao.GetPaymentCondList
            '/**************************************************************
            '	Function name	: GetPaymentCondList
            '	Discription	    : Get data Payment Condition for search
            '	Return Value	: List
            '	Create User	    : Wasan D.
            '	Create Date	    : 03-07-2013
            '*************************************************************/

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPaymentCondList = New List(Of Entity.ImpMst_PaymentConditionEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objDT As New System.Data.DataTable
                Dim objPayDetail As Entity.ImpMst_PaymentConditionEntity

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT id AS    PayID			    ")
                    .AppendLine("		    ,1st AS First           ")
                    .AppendLine("		    ,2nd AS Second          ")
                    .AppendLine("		    ,3rd AS Third           ")
                    .AppendLine("	FROM mst_payment_condition      ")
                    .AppendLine("	WHERE delete_fg <> 1			")
                    .AppendLine("       AND (1st=?1st or ?1st='')   ")
                    .AppendLine("       AND (2nd=?2nd or ?2nd='')   ")
                    .AppendLine("       AND (3rd=?3rd or ?3rd='')   ")
                    .AppendLine("	ORDER BY id;					")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?1st", strFirst.ToString.Trim)
                objConn.AddParameter("?2nd", strSecond.ToString.Trim)
                objConn.AddParameter("?3rd", strThird.ToString.Trim)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objPayDetail = New Entity.ImpMst_PaymentConditionEntity


                        ' assign data from db to entity object
                        With objPayDetail
                            .id = IIf(IsDBNull(dr.Item("PayID")), Nothing, dr.Item("PayID"))
                            .first = IIf(IsDBNull(dr.Item("First")), Nothing, dr.Item("First"))
                            .second = IIf(IsDBNull(dr.Item("Second")), Nothing, dr.Item("Second"))
                            .third = IIf(IsDBNull(dr.Item("Third")), Nothing, dr.Item("Third"))
                            .payment_condition = IIf(IsDBNull(dr.Item("First")), Nothing, dr.Item("First")) & "%" _
                                                & IIf(IsDBNull(dr.Item("Second")), Nothing, dr.Item("Second")) & "%" _
                                                & IIf(IsDBNull(dr.Item("Third")), Nothing, dr.Item("Third")) & "%"
                        End With
                        ' add item to list
                        GetPaymentCondList.Add(objPayDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentCondList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPaymentCondList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        Public Function InsertPaymentCond(ByVal objPayEnt As Entity.IMst_PaymentConditionEntity) As Integer Implements IMst_PaymentConditionDao.InsertPaymentCond
            '***************************************************************************
            '	Function name	: GetPayCondDupInsert
            '	Discription	    : Get Duplicate data for Payment Condition Insert
            '	Return Value	: Boolean
            '	Create User	    : Wasan D.
            '	Create Date	    : 04-07-2013
            '***************************************************************************

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder

            ' variable keep row effect
            Dim intEff As Integer

            'Set default return value
            InsertPaymentCond = False
            Try
                ' assign sql command
                With strSql
                    .AppendLine("	INSERT INTO mst_payment_condition                   ")
                    .AppendLine("	(1st,2nd,3rd,delete_fg,created_by,created_date,updated_by,updated_date)  ")
                    .AppendLine("	VALUES (?1st,?2nd,?3rd, 0,                           ")
                    .AppendLine("   ?user_id, date_format(now(),'%Y%m%d%H%i%s'),        ")
                    .AppendLine("	?user_id, date_format(now(),'%Y%m%d%H%i%s'))        ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                With objConn
                    .AddParameter("?1st", objPayEnt.first)
                    .AddParameter("?2nd", objPayEnt.second)
                    .AddParameter("?3rd", objPayEnt.third)
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
                InsertPaymentCond = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertPaymentCond(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertPaymentCond(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertPaymentCond(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        Public Function UpdatePaymentCond(ByVal objPayEnt As Entity.IMst_PaymentConditionEntity) As Integer Implements IMst_PaymentConditionDao.UpdatePaymentCond
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdatePaymentCond = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("	UPDATE mst_payment_condition                        ")
                    .AppendLine("	SET 1st=?First,                                     ")
                    .AppendLine("	    2nd=?Second,                                    ")
                    .AppendLine("	    3rd=?Third,                                     ")
                    .AppendLine("	    updated_by=?user_id, 						    ")
                    .AppendLine("	    updated_date=date_format(now(),'%Y%m%d%H%i%s')  ")
                    .AppendLine("	WHERE id = ?PayID                                   ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?PayID", objPayEnt.id)
                    .AddParameter("?First", objPayEnt.first)
                    .AddParameter("?Second", objPayEnt.second)
                    .AddParameter("?Third", objPayEnt.third)
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
                UpdatePaymentCond = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdatePaymentCond(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdatePaymentCond(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdatePaymentCond(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        Public Function GetPayCondDupUpdate(ByVal objPayEnt As Entity.IMst_PaymentConditionEntity) As Boolean Implements IMst_PaymentConditionDao.GetPayCondDupUpdate
            '***************************************************************************
            '	Function name	: GetPayCondDupUpdate
            '	Discription	    : Get Duplicate data for Payment Condition Update
            '	Return Value	: Boolean
            '	Create User	    : Wasan D.
            '	Create Date	    : 04-07-2013
            '***************************************************************************

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            'Set default return value
            GetPayCondDupUpdate = False
            Try
                ' assign sql command
                With strSql
                    .AppendLine("	SELECT *                    ")
                    .AppendLine("	FROM mst_payment_condition  ")
                    .AppendLine("	WHERE id<>?id               ")
                    .AppendLine("	AND delete_fg <> 1          ")
                    .AppendLine("	AND 1st= ?1st               ")
                    .AppendLine("	AND 2nd=?2nd                ")
                    .AppendLine("	AND 3rd=?3rd                ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", objPayEnt.id)
                objConn.AddParameter("?1st", objPayEnt.first)
                objConn.AddParameter("?2nd", objPayEnt.second)
                objConn.AddParameter("?3rd", objPayEnt.third)

                ' execute sql command to return value
                If objConn.ExecuteScalar(strSql.ToString) <> 0 Then
                    GetPayCondDupUpdate = True
                Else
                    GetPayCondDupUpdate = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPayCondDupUpdate(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPayCondDupUpdate(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        Public Function GetPayCondDupInsert(ByVal objPayEnt As Entity.IMst_PaymentConditionEntity) As Boolean Implements IMst_PaymentConditionDao.GetPayCondDupInsert
            '***************************************************************************
            '	Function name	: GetPayCondDupInsert
            '	Discription	    : Get Duplicate data for Payment Condition Insert
            '	Return Value	: Boolean
            '	Create User	    : Wasan D.
            '	Create Date	    : 04-07-2013
            '***************************************************************************

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            'Set default return value
            GetPayCondDupInsert = False
            Try
                ' assign sql command
                With strSql
                    .AppendLine("	SELECT *                    ")
                    .AppendLine("	FROM mst_payment_condition  ")
                    .AppendLine("	WHERE delete_fg <> 1        ")
                    .AppendLine("	AND 1st= ?1st               ")
                    .AppendLine("	AND 2nd=?2nd                ")
                    .AppendLine("	AND 3rd=?3rd                ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?1st", objPayEnt.first)
                objConn.AddParameter("?2nd", objPayEnt.second)
                objConn.AddParameter("?3rd", objPayEnt.third)

                ' execute sql command to return value
                If objConn.ExecuteScalar(strSql.ToString) <> 0 Then
                    GetPayCondDupInsert = True
                Else
                    GetPayCondDupInsert = False
                End If
            Catch ex As Exception

            End Try
        End Function
#End Region

    End Class
End Namespace
