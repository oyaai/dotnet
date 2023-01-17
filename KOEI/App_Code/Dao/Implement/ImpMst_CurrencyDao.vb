#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMst_CountryDao
'	Class Discription	: Class of table IMst_CurrencyDao
'	Create User 		: Boon
'	Create Date		    : 17-06-2013
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
    Public Class ImpMst_CurrencyDao
        Implements IMst_CurrencyDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

#Region "Function"

        '/**************************************************************
        '	Function name	: DB_GetCurrencyForList
        '	Discription	    : Get data currency for list
        '	Return Value	: Ilist IMst_CurrencyEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetCurrencyForList() As System.Collections.Generic.List(Of Entity.IMst_CurrencyEntity) Implements IMst_CurrencyDao.DB_GetCurrencyForList
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable keep sql statement, datatable and ilist
                Dim objDT As System.Data.DataTable
                Dim objCurrency As Entity.ImpMst_CurrencyEntity

                DB_GetCurrencyForList = Nothing

                ' set new object
                strSQL = New Text.StringBuilder

                ' assign sql statement and parameter
                With strSQL
                    .AppendLine(" SELECT id ")
                    .AppendLine(" 	,name ")
                    .AppendLine(" FROM ( ")
                    .AppendLine(" 	SELECT 1 grp ")
                    .AppendLine(" 		,id ")
                    .AppendLine(" 		,name ")
                    .AppendLine(" 	FROM mst_currency ")
                    .AppendLine(" 	WHERE delete_fg = 0 ")
                    .AppendLine(" 	  AND name <> 'THB' ")
                    .AppendLine(" 	UNION ")
                    .AppendLine(" 	SELECT 0 ")
                    .AppendLine(" 		,0 ")
                    .AppendLine(" 		,'--Please select--' ")
                    .AppendLine(" 	) A ")
                    .AppendLine(" ORDER BY grp ")
                    .AppendLine(" 	,id; ")
                End With

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                objDT = New System.Data.DataTable

                ' execute by datatable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError

                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_GetCurrencyForList = New List(Of Entity.IMst_CurrencyEntity)

                ' assign value to entity object
                For Each objItem As System.Data.DataRow In objDT.Rows
                    objCurrency = New Entity.ImpMst_CurrencyEntity
                    objCurrency.id = objConn.CheckDBNull(objItem("id"), Common.DBConnection.DBType.DBDecimal)
                    objCurrency.name = objConn.CheckDBNull(objItem("name"), Common.DBConnection.DBType.DBString)
                    DB_GetCurrencyForList.Add(objCurrency)
                Next

            Catch ex As Exception
                ' write error log
                DB_GetCurrencyForList = Nothing
                objLog.ErrorLog("DB_GetCurrencyForList(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetCurrencyForList(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetCurrencyForDropdownList
        '	Discription	    : Get data Currency for set dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 03-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCurrencyForDropdownList() As System.Collections.Generic.List(Of Entity.IMst_CurrencyEntity) Implements IMst_CurrencyDao.GetCurrencyForDropdownList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetCurrencyForDropdownList = New List(Of Entity.IMst_CurrencyEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable Currency entity
                Dim objCurrencyEnt As Entity.IMst_CurrencyEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT id, 		")
                    .AppendLine("		 name 	")
                    .AppendLine("	FROM mst_currency 		")
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
                        ' new vendor entity
                        objCurrencyEnt = New Entity.ImpMst_CurrencyEntity
                        With objCurrencyEnt
                            ' assign data to object Vendor entity
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                        ' add object Currency entity to list
                        GetCurrencyForDropdownList.Add(objCurrencyEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCurrencyForDropdownList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetCurrencyForDropdownList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetCurrencyList
        '	Discription	    : Get Currency list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCurrencyList(ByVal strCurrency As String) _
        As System.Collections.Generic.List(Of Entity.IMst_CurrencyEntity) Implements IMst_CurrencyDao.GetCurrencyList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetCurrencyList = New List(Of Entity.IMst_CurrencyEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objCurrencyDetail As Entity.ImpMst_CurrencyEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT id  ")
                    .AppendLine(" 	,name ")
                    .AppendLine(" FROM mst_currency  ")
                    .AppendLine(" WHERE delete_fg<>1  ")
                    .AppendLine("	AND (ISNULL(?Currency) OR name LIKE CONCAT('%', ?Currency , '%')) 	")
                    .AppendLine(" ORDER BY id ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?Currency", IIf(String.IsNullOrEmpty(strCurrency), DBNull.Value, strCurrency))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objCurrencyDetail = New Entity.ImpMst_CurrencyEntity
                        ' assign data from db to entity object
                        With objCurrencyDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))

                        End With
                        ' add item to list
                        GetCurrencyList.Add(objCurrencyDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCurrencyList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetCurrencyList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function


        '/**************************************************************
        '	Function name	: CountUsedInPO
        '	Discription	    : Count Currency in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInPO( _
            ByVal intCurrencyID As Integer _
        ) As Integer Implements IMst_CurrencyDao.CountUsedInPO
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(job.currency_id) AS used_count 				")
                    .AppendLine("		FROM job_order  job 				")
                    .AppendLine("		WHERE job.currency_id = ?id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intCurrencyID)

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
        '	Function name	: CountUsedInPO2
        '	Discription	    : Count Currency in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInPO2( _
            ByVal intCurrencyID As Integer _
        ) As Integer Implements IMst_CurrencyDao.CountUsedInPO2
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO2 = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(job.currency_id) AS used_count 				")
                    .AppendLine("		FROM po_header  job 				")
                    .AppendLine("		WHERE job.currency_id = ?id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intCurrencyID)

                ' execute sql command
                CountUsedInPO2 = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInPO2(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInPO2(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteCurrency
        '	Discription	    : Delete Currency
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteCurrency( _
            ByVal intCurrencyID As Integer _
        ) As Integer Implements IMst_CurrencyDao.DeleteCurrency
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteCurrency = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE mst_currency                             ")
                    .AppendLine("		SET delete_fg = 1,							")
                    .AppendLine("		    updated_by=?user_id,							")
                    .AppendLine("		    updated_date=date_format(now(),'%Y%m%d%H%i%s') 							")
                    .AppendLine("		WHERE id = ?id						")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intCurrencyID)
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
                DeleteCurrency = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteCurrency(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteCurrency(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetCurrencyByID
        '	Discription	    : Get Currency by ID
        '	Return Value	: IMst_Payment_TermEntity Object
        '	Create User	    : Nisa S.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCurrencyByID( _
            ByVal intCurrencyID As Integer _
        ) As Entity.IMst_CurrencyEntity Implements IMst_CurrencyDao.GetCurrencyByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetCurrencyByID = New Entity.ImpMst_CurrencyEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	id	")
                    .AppendLine("		,name	")
                    .AppendLine("	FROM mst_currency		")
                    .AppendLine("	WHERE id = ?id		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intCurrencyID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetCurrencyByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCurrencyByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetCurrencyByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertCurrency
        '	Discription	    : Insert Currencyto mst_currency
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertCurrency( _
            ByVal objCurrencyEnt As Entity.IMst_CurrencyEntity _
        ) As Integer Implements IMst_CurrencyDao.InsertCurrency
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertCurrency = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		INSERT INTO mst_currency  (name					")
                    .AppendLine("				,delete_fg				")
                    .AppendLine("				,created_by				")
                    .AppendLine("				,created_date				")
                    .AppendLine("				,updated_by				")
                    .AppendLine("				,updated_date)				")
                    .AppendLine("		VALUES (?Currency						")
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
                    .AddParameter("?Currency", objCurrencyEnt.name)
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
                InsertCurrency = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateCurrency(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertCurrency(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertCurrency(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateCurrency
        '	Discription	    : Update Currency to mst_currency
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateCurrency( _
            ByVal objCurrencyEnt As Entity.IMst_CurrencyEntity _
        ) As Integer Implements IMst_CurrencyDao.UpdateCurrency
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateCurrency = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE mst_currency 							")
                    .AppendLine("		SET name = ?Currency							")
                    .AppendLine("		  ,updated_by = ?user_id							")
                    .AppendLine("		  ,updated_date = date_format(now(),'%Y%m%d%H%i%s')							")
                    .AppendLine("		WHERE (id = ?id);							")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?Currency", objCurrencyEnt.name)
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objCurrencyEnt.id)

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
                UpdateCurrency = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateCurrency(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateCurrency(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateCurrency(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupCurrency
        ' Discription     : Check duplication Currency Master
        ' Return Value    : Integer
        ' Create User     : Nisa S.
        ' Create Date     : 28-06-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupCurrency( _
            ByVal intCurrencyID As String, _
            ByVal intCurrency As String _
        ) As Integer Implements IMst_CurrencyDao.CheckDupCurrency
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckDupCurrency = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("  SELECT COUNT(id) AS used_count     ")
                    .AppendLine("  FROM mst_currency     ")
                    .AppendLine("  WHERE id<>?id     ")
                    .AppendLine("   AND delete_fg <> 1   ")
                    .AppendLine("   AND UPPER(name)=UPPER(?Currency)   ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intCurrencyID)
                objConn.AddParameter("?Currency", intCurrency)

                ' execute sql command
                CheckDupCurrency = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("CheckDupCurrency(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupCurrency(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckDupCurrency(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

#End Region

    End Class
End Namespace

