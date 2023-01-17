#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpMst_CountryDao
'	Class Discription	: Class of table mst_country
'	Create User 		: Boon
'	Create Date		    : 14-05-2013
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
    Public Class ImpMst_CountryDao
        Implements IMst_CountryDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: DB_GetListCountryName
        '	Discription	    : Get List country name
        '	Return Value	: IList
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetListCountryName() As System.Collections.Generic.List(Of Entity.IMst_CountryEntity) Implements IMst_CountryDao.DB_GetListCountryName
            Try
                Dim strSQL As Text.StringBuilder
                Dim objDT As System.Data.DataTable
                Dim objCountry As Entity.IMst_CountryEntity

                DB_GetListCountryName = Nothing

                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder
                With strSQL
                    .AppendLine(" SELECT ID ")
                    .AppendLine(" 	,NAME ")
                    .AppendLine(" FROM mst_country ")
                    .AppendLine(" WHERE delete_fg <> 1 ")
                    .AppendLine(" ORDER BY id ")
                End With

                objDT = New System.Data.DataTable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_GetListCountryName = New List(Of Entity.IMst_CountryEntity)
                For Each objItem As System.Data.DataRow In objDT.Rows
                    objCountry = New Entity.ImpMst_CountryEntity
                    objCountry.id = objConn.CheckDBNull(objItem("ID"), Common.DBConnection.DBType.DBDecimal)
                    objCountry.name = objConn.CheckDBNull(objItem("NAME"), Common.DBConnection.DBType.DBString)
                    DB_GetListCountryName.Add(objCountry)
                Next

            Catch ex As Exception
                DB_GetListCountryName = Nothing
                objLog.ErrorLog("DB_GetListCountryName", ex.Message.Trim)
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetCountryList
        '	Discription	    : Get Country list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCountryList( _
            ByVal strCountryName As String _
        ) As System.Collections.Generic.List(Of Entity.ImpMst_CountryDetailEntity) Implements IMst_CountryDao.GetCountryList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetCountryList = New List(Of Entity.ImpMst_CountryDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objCountryDetail As Entity.ImpMst_CountryDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine("SELECT   ")
                    .AppendLine("		id AS country_id	 ")
                    .AppendLine("		,name AS country_name	 ")
                    .AppendLine("FROM mst_Country 		 ")
                    .AppendLine("	WHERE (ISNULL(?Country_name) OR name LIKE CONCAT('%', ?Country_name , '%'))									")
                    .AppendLine("	AND  delete_fg <> 1 ")
                    .AppendLine("	ORDER BY id; ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?Country_name", IIf(String.IsNullOrEmpty(strCountryName), DBNull.Value, strCountryName))
                 
                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objCountryDetail = New Entity.ImpMst_CountryDetailEntity
                        ' assign data from db to entity object
                        With objCountryDetail
                            .id = IIf(IsDBNull(dr.Item("Country_id")), Nothing, dr.Item("Country_id"))
                            .name = IIf(IsDBNull(dr.Item("Country_name")), Nothing, dr.Item("Country_name"))
                            '.inuse = IIf(IsDBNull(dr.Item("in_used")), Nothing, dr.Item("in_used"))
                        End With
                        ' add Country to list
                        GetCountryList.Add(objCountryDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCountryList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetCountryList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteCountry
        '	Discription	    : Delete Country
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteCountry( _
            ByVal intCountryID As Integer _
        ) As Integer Implements IMst_CountryDao.DeleteCountry
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteCountry = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE mst_Country                             ")
                    .AppendLine("		SET delete_fg = 1,							")
                    .AppendLine("		    updated_by = ?update_by,							")
                    .AppendLine("		    updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE id = ?id							")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?update_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intCountryID)
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
                DeleteCountry = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteCountry(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteCountry(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetCountryByID
        '	Discription	    : Get Country by ID
        '	Return Value	: IMst_CountryEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCountryByID( _
            ByVal intCountryID As Integer _
        ) As Entity.IMst_CountryEntity Implements IMst_CountryDao.GetCountryByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetCountryByID = New Entity.ImpMst_CountryEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	id	")
                    .AppendLine("		,name	")
                    .AppendLine("	FROM mst_country		")
                    .AppendLine("	WHERE id = ?country_id		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?country_id", intCountryID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetCountryByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCountryByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetCountryByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertCountry
        '	Discription	    : Insert Country to mst_Country
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertCountry( _
            ByVal objCountryEnt As Entity.IMst_CountryEntity _
        ) As Integer Implements IMst_CountryDao.InsertCountry
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertCountry = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		INSERT INTO mst_country  ( ")
                    .AppendLine("				name				")
                    .AppendLine("				,delete_fg				")
                    .AppendLine("				,created_by				")
                    .AppendLine("				,created_date				")
                    .AppendLine("				,updated_by				")
                    .AppendLine("				,updated_date)				")
                    .AppendLine("		VALUES (						")
                    .AppendLine("			?name					")
                    .AppendLine("			,0					")
                    .AppendLine("			,?created_by					")
                    .AppendLine("			,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')					")
                    .AppendLine("			,?created_by					")
                    .AppendLine("			,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', ''));					")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?name", objCountryEnt.name)
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
                InsertCountry = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertCountry(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertCountry(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertCountry(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateCountry
        '	Discription	    : Update Country to mst_Country
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateCountry( _
           ByVal objCountryEnt As Entity.IMst_CountryEntity _
       ) As Integer Implements IMst_CountryDao.UpdateCountry
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateCountry = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE mst_country							")
                    .AppendLine("		SET name = ?name							")
                    .AppendLine("		  ,updated_by = ?updated_by							")
                    .AppendLine("		  ,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE (id = ?id);							")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?name", objCountryEnt.name)
                    .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objCountryEnt.id)

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
                UpdateCountry = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateCountry(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateCountry(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateCountry(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: CheckDupCountry
        '	Discription	    : Check data country duplicate
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckDupCountry( _
           ByVal strCountryName As String, _
           ByVal id As String _
        ) As Integer Implements IMst_CountryDao.CheckDupCountry

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder

            Try
                ' set default return value
                CheckDupCountry = 0

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	Count(*) AS country_count	")
                    .AppendLine("	FROM mst_country		")
                    .AppendLine("   WHERE delete_fg <> 1")
                    .AppendLine("	AND UPPER(name) = ?country_name		")
                    If id <> "" Then
                        .AppendLine("	AND id <> ?id		")
                    End If
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?country_name", strCountryName.ToUpper)
                If id <> "" Then
                    objConn.AddParameter("?id", id)
                End If

                ' execute sql command with data reader object
                CheckDupCountry = objConn.ExecuteScalar(strSql.ToString)

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupCountry(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("CheckDupCountry(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInVendor
        '	Discription	    : Count country in used mst_vendor
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 06-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInVendor( _
             ByVal intCountryID As Integer _
        ) As Integer Implements IMst_CountryDao.CountUsedInVendor
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInVendor = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(country_id) AS used_count 				")
                    .AppendLine("		FROM mst_vendor 				")
                    .AppendLine("		WHERE country_id = ?country_id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?country_id", intCountryID)

                ' execute sql command
                CountUsedInVendor = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInVendor(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInVendor(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

    End Class

End Namespace

