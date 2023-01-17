#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpIEDao
'	Class Discription	: Class of table mst_ie
'	Create User 		: Nisa S.
'	Create Date		    : 24-05-2013
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

Imports Interfaces
Imports Microsoft.VisualBasic
Imports Dto
Imports Entity
Imports Common.DBConnection
Imports System.Data
Imports Extensions
Imports MySql.Data.MySqlClient
Imports Exceptions

#End Region


Namespace Dao

    Public Class ImpIEDao
        Implements IIEDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty


#Region "Properties"

        Protected Overridable ReadOnly Property ClassName() As String Implements IIEDao.ClassName
            Get
                Return Convert.ToString(GetType(ImpIEDao))
            End Get
        End Property

#End Region

#Region "Functions"

        '/**************************************************************
        '	Function name	: DB_GetIEForList
        '	Discription	    : Get data ie for list
        '	Return Value	: Ilist ImpIEEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetIEForList(Optional ByVal showCode As Boolean = False) As System.Collections.Generic.List(Of Entity.ImpIEEntity) Implements Interfaces.IIEDao.DB_GetIEForList
            Dim strSQL As New Text.StringBuilder
            Dim objLog As New Common.Logs.Log
            Dim objConn As New MySQLAccess
            Try
                ' variable keep sql statement, datatable and ilist
                Dim strMsgErr As String
                Dim objDT As System.Data.DataTable
                Dim objIE As Entity.ImpIEEntity

                DB_GetIEForList = Nothing

                ' set new object
                strSQL = New Text.StringBuilder

                ' assign sql statement and parameter
                With strSQL
                    If showCode = True Then
                        .AppendLine(" SELECT id ")
                        .AppendLine(" 	,concat(code,'-',name) as name ")
                        .AppendLine(" FROM mst_ie ")
                        .AppendLine(" WHERE delete_fg <> 1 ")
                        .AppendLine(" ORDER BY code, name ")
                    Else
                        .AppendLine(" SELECT id ")
                        .AppendLine(" 	,name ")
                        .AppendLine(" FROM mst_ie ")
                        .AppendLine(" WHERE delete_fg <> 1 ")
                        .AppendLine(" ORDER BY id ")
                    End If
                    
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
                DB_GetIEForList = New List(Of Entity.ImpIEEntity)

                ' assign value to entity object
                For Each objItem As System.Data.DataRow In objDT.Rows
                    objIE = New Entity.ImpIEEntity
                    objIE.ID = objConn.CheckDBNull(objItem("id"), Common.DBConnection.DBType.DBDecimal)
                    objIE.Name = objConn.CheckDBNull(objItem("name"), Common.DBConnection.DBType.DBString)
                    DB_GetIEForList.Add(objIE)
                Next

            Catch ex As Exception
                ' write error log
                DB_GetIEForList = Nothing
                objLog.ErrorLog("DB_GetIEForList(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetIEForList(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_CheckIEByCategory
        '	Discription	    : Check IE by Category
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 24-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckIEByCategory(ByVal intCategory_id As Integer) As Boolean Implements IIEDao.DB_CheckIEByCategory
            Try
                Dim strSQL As Text.StringBuilder
                Dim intFlagCount As Integer = 0

                DB_CheckIEByCategory = False

                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder
                With strSQL
                    .AppendLine(" SELECT Count(*) As id ")
                    .AppendLine(" FROM mst_ie ")
                    .AppendLine(" WHERE category_id = ?category_id ")
                    .AppendLine(" 	AND delete_fg <> 1 ")
                    objConn.AddParameter("?category_id", intCategory_id)
                End With

                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                If intFlagCount > 0 Then
                    DB_CheckIEByCategory = True
                End If

            Catch ex As Exception
                DB_CheckIEByCategory = False
                objLog.ErrorLog("DB_CheckIEByCategory", ex.Message.Trim)
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetIEList
        '	Discription	    : Get IE list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetIEList( _
            ByVal strID As String, _
            ByVal strIECategory As String, _
            ByVal strIECode As String, _
            ByVal strIEName As String _
        ) As System.Collections.Generic.List(Of Entity.ImpMst_IEDetailEntity) Implements IIEDao.GetIEList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetIEList = New List(Of Entity.ImpMst_IEDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objIEDetail As Entity.ImpMst_IEDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine("		SELECT 					")
                    .AppendLine("			ie.id				")
                    .AppendLine("			, category.`name` AS category_name				")
                    .AppendLine("			, ie.code				")
                    .AppendLine("			, ie.`name`				")
                    .AppendLine("			, ie.`category_id`				")
                    .AppendLine("			, (CASE WHEN IFNULL(acc.acc_used, 0) <= 0 AND IFNULL(po_used, 0)<= 0 THEN 0 ELSE 1 END) AS in_used				")
                    .AppendLine("		FROM 					")
                    .AppendLine("			mst_ie ie 				")
                    .AppendLine("		INNER JOIN 					")
                    .AppendLine("			mst_ie_category category 				")
                    .AppendLine("		ON ie.`category_id` = category.`id`					")
                    .AppendLine("		LEFT JOIN 					")
                    .AppendLine("			(SELECT `ie_id`				")
                    .AppendLine("				, COUNT(`ie_id`) AS acc_used 			")
                    .AppendLine("				FROM accounting 			")
                    .AppendLine("				GROUP BY `ie_id`) acc 			")
                    .AppendLine("		ON ie.`id` = acc.`ie_id`					")
                    .AppendLine("		LEFT JOIN 					")
                    .AppendLine("			(SELECT `ie_id`				")
                    .AppendLine("				, COUNT(`ie_id`) AS po_used 			")
                    .AppendLine("				FROM po_detail 			")
                    .AppendLine("				GROUP BY `ie_id`) po 			")
                    .AppendLine("		ON po.`ie_id` = ie.`id`					")
                    .AppendLine("		WHERE (ie.`delete_fg` <> 1) 					")
                    .AppendLine("		AND (category.`delete_fg` <> 1) 					")
                    .AppendLine("		AND (ie.`id` = IFNULL(?id, ie.`id`)) 					")
                    .AppendLine("		AND (category.`id` = IFNULL(?category_id, category.`id`))					")
                    .AppendLine("		AND (ISNULL(?code) OR ie.code LIKE CONCAT('%', ?code, '%'))					")
                    .AppendLine("		AND (ISNULL(?name) OR ie.name LIKE CONCAT('%', ?name, '%'))					")
                    .AppendLine("		ORDER BY 					")
                    .AppendLine("		ie.`id`;					")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(strID), DBNull.Value, strID))
                objConn.AddParameter("?category_id", IIf(String.IsNullOrEmpty(strIECategory), DBNull.Value, strIECategory))
                objConn.AddParameter("?code", IIf(String.IsNullOrEmpty(strIECode), DBNull.Value, strIECode))
                objConn.AddParameter("?name", IIf(String.IsNullOrEmpty(strIEName), DBNull.Value, strIEName))


                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objIEDetail = New Entity.ImpMst_IEDetailEntity
                        ' assign data from db to entity object
                        With objIEDetail
                            .ID = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .Code = IIf(IsDBNull(dr.Item("code")), Nothing, dr.Item("code"))
                            .category_id = IIf(IsDBNull(dr.Item("category_id")), Nothing, dr.Item("category_id"))
                            .category_name = IIf(IsDBNull(dr.Item("category_name")), Nothing, dr.Item("category_name"))
                            .Name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                            .in_used = IIf(IsDBNull(dr.Item("in_used")), Nothing, dr.Item("in_used"))
                        End With
                        ' add IE to list
                        GetIEList.Add(objIEDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetIEList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetIEList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))

            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetIEByID
        '	Discription	    : Get IE by ID
        '	Return Value	: IMst_IEEntity Object
        '	Create User	    : Nisa S.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetIEByID( _
            ByVal intIEID As Integer _
        ) As IMst_IEEntity Implements IIEDao.GetIEByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetIEByID = New Entity.ImpIEEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	id	")
                    .AppendLine("		,category_id	")
                    .AppendLine("		,mid(code,4) as code	")
                    .AppendLine("		,name	")
                    .AppendLine("	FROM mst_ie		")
                    .AppendLine("	WHERE id = ?id		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intIEID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetIEByID
                            .ID = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .CategoryID = IIf(IsDBNull(dr.Item("category_id")), Nothing, dr.Item("category_id"))
                            .Code = IIf(IsDBNull(dr.Item("code")), Nothing, dr.Item("code"))
                            .Name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetIEByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetIEByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInPO
        '	Discription	    : Count IE in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInPO( _
            ByVal intIEID As Integer _
        ) As Integer Implements IIEDao.CountUsedInPO
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(ie_id) AS used_count 				")
                    .AppendLine("		FROM po_detail po 				")
                    .AppendLine("		WHERE po.ie_id = ?ie_id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?ie_id", intIEID)

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
        '	Discription	    : Count IE in used account
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 15-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInPO2( _
            ByVal intIEID As Integer _
        ) As Integer Implements IIEDao.CountUsedInPO2
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO2 = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(ie_id) AS used_count 				")
                    .AppendLine("		FROM accounting				")
                    .AppendLine("		WHERE ie_id = ?ie_id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?ie_id", intIEID)

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
        '	Function name	: DeleteIE
        '	Discription	    : Delete IE
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteIE( _
            ByVal intIEID As Integer _
        ) As Integer Implements IIEDao.DeleteIE
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteIE = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE mst_ie                             ")
                    .AppendLine("		SET delete_fg = 1,							")
                    .AppendLine("		    updated_by = ?update_by,							")
                    .AppendLine("		    updated_date = DATE_FORMAT(NOW(),'%Y%m%d%H%i%s')							")
                    .AppendLine("		WHERE id = ?id							")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?update_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intIEID)
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
                DeleteIE = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteIE(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteIE(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertIE
        '	Discription	    : Insert IE to mst_ie
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertIE( _
            ByVal objIEEnt As IMst_IEEntity _
        ) As Integer Implements IIEDao.InsertIE
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertIE = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		INSERT INTO mst_ie (category_id					")
                    .AppendLine("				,code				")
                    .AppendLine("				,name				")
                    .AppendLine("				,delete_fg				")
                    .AppendLine("				,created_by				")
                    .AppendLine("				,created_date				")
                    .AppendLine("				,updated_by				")
                    .AppendLine("				,updated_date)				")
                    .AppendLine("		VALUES (?category_id						")
                    .AppendLine("			,(select CONCAT(mid(name,1,3),?code) from mst_ie_category where id = ?category_id1) ")
                    .AppendLine("			,?name					")
                    .AppendLine("			,0					")
                    .AppendLine("			,?created_by					")
                    .AppendLine("			,DATE_FORMAT(NOW(),'%Y%m%d%H%i%s') ")
                    .AppendLine("	        ,?updated_by ")
                    .AppendLine("           ,DATE_FORMAT(NOW(),'%Y%m%d%H%i%s'));					")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?category_id", objIEEnt.CategoryID)
                    .AddParameter("?code", objIEEnt.Code)
                    .AddParameter("?category_id1", objIEEnt.CategoryID)
                    .AddParameter("?name", objIEEnt.Name)
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
                InsertIE = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateIE(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertIE(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertIE(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateIE
        '	Discription	    : Update IE to mst_ie
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateIE( _
            ByVal objIEEnt As IMst_IEEntity _
        ) As Integer Implements IIEDao.UpdateIE
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateIE = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE mst_ie							")
                    .AppendLine("		SET category_id = ?category_id							")
                    .AppendLine("		  ,code = (select CONCAT(mid(name,1,3),?code) from mst_ie_category where id = ?category_id1)		")
                    .AppendLine("		  ,name = ?name							")
                    .AppendLine("		  ,updated_by = ?updated_by							")
                    .AppendLine("		  ,updated_date = DATE_FORMAT(NOW(),'%Y%m%d%H%i%s')							")
                    .AppendLine("		WHERE id = ?id;							")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?category_id", objIEEnt.CategoryID)
                    .AddParameter("?category_id1", objIEEnt.CategoryID)
                    .AddParameter("?code", objIEEnt.Code)
                    .AddParameter("?name", objIEEnt.Name)
                    .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objIEEnt.ID)

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
                UpdateIE = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateIE(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateIE(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateIE(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupIE
        ' Discription     : Check duplication Account Title Master
        ' Return Value    : Integer
        ' Create User     : Nisa S.
        ' Create Date     : 15-07-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupIE( _
            ByVal intIEID As String, _
            ByVal strIECode As String, _
            ByVal strIECategory As String _
        ) As Integer Implements IIEDao.CheckDupIE
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckDupIE = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("  SELECT COUNT(id) AS used_count     ")
                    .AppendLine("  FROM mst_ie     ")
                    .AppendLine("  WHERE code = (select CONCAT(mid(name,1,3),?code) from mst_ie_category where id = ?category_id1)  ")
                    .AppendLine("   AND id <> ?id  ")
                    .AppendLine("   AND delete_fg <> 1   ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intIEID)
                objConn.AddParameter("?code", strIECode)
                objConn.AddParameter("?category_id1", strIECategory)

                ' execute sql command
                CheckDupIE = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("CheckDupIE(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupIE(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckDupIE(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        ' Function name   : GetListAccountTitleToDDL
        ' Discription     : Get Account Title to Dropdownlist
        ' Return Value    : List of Account Title
        ' Create User     : Wasan D.
        ' Create Date     : 15-07-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function GetListAccountTitleToDDL(ByVal intCategoryType As Integer) As System.Collections.Generic.List(Of Entity.ImpIEEntity) Implements Interfaces.IIEDao.GetListAccountTitleToDDL
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetListAccountTitleToDDL = New List(Of Entity.ImpIEEntity)
            Try
                ' Variable datareader object
                Dim dr As MySqlDataReader
                ' Variable object ie entity
                Dim objIEEnt As Entity.ImpIEEntity
                ' Assign SQL Command
                With strSql
                    .AppendLine("   SELECT ie.`id`, ie.`code`, ie.`name`                                            ")
                    .AppendLine("   FROM `mst_ie` ie LEFT JOIN `mst_ie_category` cat ON ie.`category_id` = cat.`id` ")
                    .AppendLine("   WHERE (ie.`delete_fg` <> 1) AND (cat.`category_type` = ?category_type);         ")
                End With
                ' New Connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' Assign parameter
                objConn.AddParameter("?category_type", intCategoryType)
                ' Execute SQL Command
                dr = objConn.ExecuteReader(strSql.ToString)
                ' Check rows
                If dr.HasRows Then
                    While dr.Read
                        ' Set new object entity
                        objIEEnt = New Entity.ImpIEEntity
                        ' assign value to IE entity
                        With objIEEnt
                            .ID = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .Code = IIf(IsDBNull(dr.Item("code")), Nothing, dr.Item("code"))
                            .Name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                        GetListAccountTitleToDDL.Add(objIEEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetListAccountTitleToDDL(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("GetListAccountTitleToDDL(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAccountTitleForList
        '	Discription	    : Get data ie for set dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountTitleForList() As System.Collections.Generic.List(Of Entity.ImpIEEntity) Implements IIEDao.GetAccountTitleForList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetAccountTitleForList = New List(Of Entity.ImpIEEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable ie entity
                Dim objIEEnt As Entity.ImpIEEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT id, 		")
                    .AppendLine("		 CONCAT(code,' - ',name) AS name  	")
                    .AppendLine("	FROM mst_ie  		")
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
                        ' new Ie entity
                        objIEEnt = New Entity.ImpIEEntity
                        With objIEEnt
                            ' assign data to object Vendor entity
                            .ID = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .Name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                        ' add object Vendor entity to list
                        GetAccountTitleForList.Add(objIEEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccountTitleForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetAccountTitleForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

#End Region
    End Class
End Namespace