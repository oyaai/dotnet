#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpMst_VendorDao
'	Class Discription	: Class of table mst_vendor
'	Create User 		: Boon
'	Create Date		    : 17-05-2013
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
Imports System.Data
Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
#End Region

Namespace Dao
    Public Class ImpMst_VendorDao
        Implements IMst_VendorDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: DB_CheckIsDupVendor
        '	Discription	    : Check duplicate name vendor
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckIsDupVendor(ByVal intType1 As Integer, ByVal intType2 As Integer, ByVal strName As String, Optional ByVal intId As Integer = 0) As Boolean Implements IMst_VendorDao.DB_CheckIsDupVendor
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlagCount As Integer = 0
                Dim objListParam As New List(Of MySqlParameter)

                DB_CheckIsDupVendor = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT Count(*) As vendor_count ")
                    .AppendLine(" FROM mst_vendor ")
                    .AppendLine(" WHERE delete_fg <> 1 ")
                    .AppendLine(" 	AND type1 = ?type1 ")
                    .AppendLine(" 	AND type2 = ?type2 ")
                    .AppendLine(" 	AND UPPER(name) = UPPER(?name) ")
                    ' assign parameter
                    objListParam.Add(New MySqlParameter("?type1", intType1))
                    objListParam.Add(New MySqlParameter("?type2", intType2))
                    objListParam.Add(New MySqlParameter("?name", strName.ToUpper))
                    ' check value vendor_id
                    If intId > 0 Then
                        .AppendLine(" 	AND id <> ?id ")
                        objListParam.Add(New MySqlParameter("?id", intId))
                    End If
                End With

                objConn.AddParameter(objListParam)

                ' execute by scalar
                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlagCount > 0 Then
                    DB_CheckIsDupVendor = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CheckIsDupVendor = False
                objLog.ErrorLog("DB_CheckIsDupVendor(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CheckIsDupVendor(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_InsertVendor
        '	Discription	    : Insert data vendor
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    : Wasan D.
        '	Update Date	    : 25-10-2013
        '*************************************************************/
        Public Function DB_InsertVendor(ByVal objVendor As Entity.IMst_VendorEntity, ByRef intVendorId As Integer) As Boolean Implements IMst_VendorDao.DB_InsertVendor
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim objListParam As New List(Of MySqlParameter)
                Dim intFlag As Integer = 0

                DB_InsertVendor = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder
                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)

                ' assign sql statement
                With strSQL
                    .AppendLine(" INSERT INTO mst_vendor ( ")
                    .AppendLine(" 	type1 ")
                    .AppendLine(" 	,type2 ")
                    .AppendLine(" 	,type2_no ")
                    .AppendLine(" 	,NAME ")
                    .AppendLine(" 	,abbr ")
                    .AppendLine(" 	,person_in_charge1 ")
                    .AppendLine(" 	,person_in_charge2 ")
                    '.AppendLine(" 	,payment_term_id ")
                    '.AppendLine(" 	,payment_cond1 ")
                    '.AppendLine(" 	,payment_cond2 ")
                    '.AppendLine(" 	,payment_cond3 ")
                    .AppendLine(" 	,country_id ")
                    .AppendLine(" 	,zipcode ")
                    .AppendLine(" 	,address ")
                    .AppendLine(" 	,tel ")
                    .AppendLine(" 	,fax ")
                    .AppendLine(" 	,email ")
                    '.AppendLine(" 	,type_of_goods ") ' add column purchase_fg,outsource_fg,other_fg
                    .AppendLine(" 	,purchase_fg ")
                    .AppendLine(" 	,outsource_fg ")
                    .AppendLine(" 	,other_fg ")
                    .AppendLine(" 	,remarks ")
                    .AppendLine(" 	,file ")
                    .AppendLine(" 	,delete_fg ")
                    .AppendLine(" 	,created_by ")
                    .AppendLine(" 	,created_date ")
                    .AppendLine(" 	,updated_by ")
                    .AppendLine(" 	,updated_date ")
                    .AppendLine(" 	) ")
                    .AppendLine(" VALUES ( ")
                    .AppendLine("  	?type1 ")
                    .AppendLine(" 	,?type2 ")
                    .AppendLine(" 	,?type2_no ")
                    .AppendLine(" 	,?name ")
                    .AppendLine(" 	,?short_name ")
                    .AppendLine(" 	,?person_in_charge1 ")
                    .AppendLine(" 	,?person_in_charge2 ")
                    '.AppendLine(" 	,?payment_term_id ")
                    '.AppendLine(" 	,?payment_cond1 ")
                    '.AppendLine(" 	,?payment_cond2 ")
                    '.AppendLine(" 	,?payment_cond3 ")
                    .AppendLine(" 	,?country_id ")
                    .AppendLine(" 	,?zipcode ")
                    .AppendLine(" 	,?address ")
                    .AppendLine(" 	,?tel ")
                    .AppendLine(" 	,?fax ")
                    .AppendLine(" 	,?email ")
                    '.AppendLine(" 	,?type_of_goods ")  ' add column purchase_fg,outsource_fg,other_fg
                    .AppendLine(" 	,?purchase_fg ")
                    .AppendLine(" 	,?outsource_fg ")
                    .AppendLine(" 	,?other_fg ")
                    .AppendLine(" 	,?remarks ")
                    .AppendLine(" 	,?file ")
                    .AppendLine(" 	,0 ")
                    .AppendLine(" 	,?user_id ")
                    .AppendLine(" 	,date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" 	,?user_id ")
                    .AppendLine(" 	,date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" 	); ")
                    .AppendLine("   SELECT Last_Insert_ID();    ")
                End With

                ' assign parameter
                With objVendor
                    objListParam.Add(New MySqlParameter("?type1", .type1))
                    objListParam.Add(New MySqlParameter("?type2", .type2))
                    objListParam.Add(New MySqlParameter("?type2_no", .type2_no.Trim))
                    objListParam.Add(New MySqlParameter("?name", .name.Trim))
                    objListParam.Add(New MySqlParameter("?short_name", .short_name.Trim))
                    objListParam.Add(New MySqlParameter("?person_in_charge1", .person_in_charge1.Trim))
                    objListParam.Add(New MySqlParameter("?person_in_charge2", .person_in_charge2.Trim))
                    'objListParam.Add(New MySqlParameter("?payment_term_id", .payment_term_id))
                    'objListParam.Add(New MySqlParameter("?payment_cond1", .payment_cond1))
                    'objListParam.Add(New MySqlParameter("?payment_cond2", .payment_cond2))
                    'objListParam.Add(New MySqlParameter("?payment_cond3", .payment_cond3))
                    objListParam.Add(New MySqlParameter("?country_id", .country_id))
                    objListParam.Add(New MySqlParameter("?zipcode", .zipcode.Trim))
                    objListParam.Add(New MySqlParameter("?address", .address.Trim))
                    objListParam.Add(New MySqlParameter("?tel", .tel.Trim))
                    objListParam.Add(New MySqlParameter("?fax", .fax.Trim))
                    objListParam.Add(New MySqlParameter("?email", .email.Trim))
                    'objListParam.Add(New MySqlParameter("?type_of_goods", .type_of_goods))' add column purchase_fg,outsource_fg,other_fg
                    objListParam.Add(New MySqlParameter("?purchase_fg", .purchase_fg))
                    objListParam.Add(New MySqlParameter("?outsource_fg", .outsource_fg))
                    objListParam.Add(New MySqlParameter("?other_fg", .other_fg))
                    objListParam.Add(New MySqlParameter("?remarks", .remarks.Trim))
                    objListParam.Add(New MySqlParameter("?file", .file.Trim))
                    objListParam.Add(New MySqlParameter("?user_id", HttpContext.Current.Session("UserID").ToString.Trim))
                End With
                objConn.AddParameter(objListParam)

                ' execute by nonquery
                intVendorId = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                'If intFlag > 0 Then
                '    intVendorId = GetVendorIdByDetails(objVendor)
                objConn.CommitTrans()
                DB_InsertVendor = True
                'End If

            Catch ex As Exception
                ' write error log
                objConn.RollbackTrans()
                objLog.ErrorLog("DB_InsertVendor(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_InsertVendor(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetVendorIdByDetails
        '	Discription	    : Get data vendor_id
        '	Return Value	: Integer (vendor_id)
        '	Create User	    : Boonyarit
        '	Create Date	    : 06-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function GetVendorIdByDetails(ByVal objValue As Entity.IMst_VendorEntity) As Integer
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable keep sql statement, datatable and ilist
                Dim objListParam As New List(Of MySqlParameter)

                GetVendorIdByDetails = 0

                ' set new object
                strSQL = New Text.StringBuilder

                ' assign sql statement and parameter
                With strSQL
                    .AppendLine(" SELECT v.id ")
                    .AppendLine(" FROM mst_vendor v ")
                    .AppendLine(" WHERE v.type1 = ?type1 ")
                    .AppendLine(" 	AND v.type2 = ?type2 ")
                    .AppendLine(" 	AND v.name = ?name ")
                    .AppendLine(" 	AND v.type_of_goods = ?type_of_goods ")

                    objListParam.Add(New MySqlParameter("?type1", objValue.type1))
                    objListParam.Add(New MySqlParameter("?type2", objValue.type2))
                    objListParam.Add(New MySqlParameter("?name", objValue.name))
                    objListParam.Add(New MySqlParameter("?type_of_goods", objValue.type_of_goods))

                End With

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                objConn.AddParameter(objListParam)

                ' execute by datatable
                GetVendorIdByDetails = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError

            Catch ex As Exception
                ' write error log
                GetVendorIdByDetails = 0
                objLog.ErrorLog("GetVendorIdByDetails(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetVendorIdByDetails(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_UpdateVendor
        '	Discription	    : Update data vendor
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_UpdateVendor(ByVal objVendor As Entity.IMst_VendorEntity) As Boolean Implements IMst_VendorDao.DB_UpdateVendor
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim objListParam As New List(Of MySqlParameter)
                Dim intFlag As Integer = 0

                DB_UpdateVendor = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" UPDATE mst_vendor ")
                    .AppendLine(" SET type1 = ?type1 ")
                    .AppendLine(" 	,type2 = ?type2 ")
                    .AppendLine(" 	,type2_no = ?type2_no ")
                    .AppendLine(" 	,name = ?name ")
                    .AppendLine(" 	,abbr = ?short_name ")
                    .AppendLine(" 	,person_in_charge1 = ?person_in_charge1 ")
                    .AppendLine(" 	,person_in_charge2 = ?person_in_charge2 ")
                    .AppendLine(" 	,country_id = ?country_id ")
                    .AppendLine(" 	,zipcode = ?zipcode ")
                    .AppendLine(" 	,address = ?address ")
                    .AppendLine(" 	,tel = ?tel ")
                    .AppendLine(" 	,fax = ?fax ")
                    .AppendLine(" 	,email = ?email ")
                    '.AppendLine(" 	,type_of_goods = ?type_of_goods ") ' add column purchase_fg,outsource_fg,other_fg
                    .AppendLine(" 	,purchase_fg = ?purchase_fg ")
                    .AppendLine(" 	,outsource_fg = ?outsource_fg ")
                    .AppendLine(" 	,other_fg = ?other_fg ")
                    .AppendLine(" 	,remarks = ?remarks ")
                    .AppendLine(" 	,file = ?file ")
                    .AppendLine(" 	,delete_fg = 0 ")
                    .AppendLine(" 	,updated_by = ?user_id ")
                    .AppendLine(" 	,updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" WHERE id = ?id ")
                End With

                ' assign parameter
                With objVendor
                    objListParam.Add(New MySqlParameter("?type1", .type1))
                    objListParam.Add(New MySqlParameter("?type2", .type2))
                    objListParam.Add(New MySqlParameter("?type2_no", .type2_no.Trim))
                    objListParam.Add(New MySqlParameter("?name", .name.Trim))
                    objListParam.Add(New MySqlParameter("?short_name", .short_name.Trim))
                    objListParam.Add(New MySqlParameter("?person_in_charge1", .person_in_charge1.Trim))
                    objListParam.Add(New MySqlParameter("?person_in_charge2", .person_in_charge2.Trim))
                    objListParam.Add(New MySqlParameter("?country_id", .country_id))
                    objListParam.Add(New MySqlParameter("?zipcode", .zipcode.Trim))
                    objListParam.Add(New MySqlParameter("?address", .address.Trim))
                    objListParam.Add(New MySqlParameter("?tel", .tel.Trim))
                    objListParam.Add(New MySqlParameter("?fax", .fax.Trim))
                    objListParam.Add(New MySqlParameter("?email", .email.Trim))
                    'objListParam.Add(New MySqlParameter("?type_of_goods", .type_of_goods))
                    objListParam.Add(New MySqlParameter("?purchase_fg", .purchase_fg))
                    objListParam.Add(New MySqlParameter("?outsource_fg", .outsource_fg))
                    objListParam.Add(New MySqlParameter("?other_fg", .other_fg))
                    objListParam.Add(New MySqlParameter("?remarks", .remarks.Trim))
                    objListParam.Add(New MySqlParameter("?file", .file.Trim))
                    objListParam.Add(New MySqlParameter("?user_id", HttpContext.Current.Session("UserID").ToString.Trim))
                    objListParam.Add(New MySqlParameter("?id", .id))
                End With
                objConn.AddParameter(objListParam)

                ' execute by nonquery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag > 0 Then
                    DB_UpdateVendor = True
                End If

            Catch ex As Exception
                ' write error log
                DB_UpdateVendor = False
                objLog.ErrorLog("DB_UpdateVendor(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_UpdateVendor(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteBranchAfterVendor
        '	Discription	    : Delete branch after delete vendor
        '	Return Value	: Boolean
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteBranchAfterVendor(ByVal intVendorID As Integer) As Integer
            DeleteBranchAfterVendor = -1
            Dim strSQL As New Text.StringBuilder
            Try
                ' assign sql statement
                With strSQL
                    .AppendLine("   UPDATE mst_vendor_branch 								")
                    .AppendLine("   SET delete_fg = 1 										")
                    .AppendLine(" 		,updated_by = ?user_id              				")
                    .AppendLine(" 		,updated_date = date_format(now(), '%Y%m%d%H%i%s')  ")
                    .AppendLine(" 	WHERE vendor_id = ?id;									")
                    ' assign parameter
                    objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    objConn.AddParameter("?id", intVendorID)
                End With
                ' execute by nonquery
                Return objConn.ExecuteNonQuery(strSQL.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteBranchAfterVendor(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DeleteBranchAfterVendor(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_CancelVendor
        '	Discription	    : Cancel data vendor
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 21-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CancelVendor(ByVal intVendor As Integer) As Boolean Implements IMst_VendorDao.DB_CancelVendor
            Dim strSQL As New Text.StringBuilder
            DB_CancelVendor = False
            Try
                ' variable
                Dim intFlag As Integer = 0
                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)
                ' assign sql statement
                With strSQL
                    .AppendLine(" UPDATE mst_vendor ")
                    .AppendLine(" SET delete_fg = 1 ")
                    .AppendLine(" 	,file = '' ")
                    .AppendLine(" 	,updated_by = ?user_id ")
                    .AppendLine(" 	,updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" WHERE id = ?id ")
                    ' assign parameter
                    objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    objConn.AddParameter("?id", intVendor)
                End With
                ' execute by nonquery and check data
                If objConn.ExecuteNonQuery(strSQL.ToString) > 0 Then
                    ' Delete branch
                    If DeleteBranchAfterVendor(intVendor) > 0 Then
                        objConn.CommitTrans()
                        DB_CancelVendor = True
                    Else
                        objConn.RollbackTrans()
                    End If
                Else
                    objConn.RollbackTrans()
                End If
            Catch ex As Exception
                ' write error log
                objConn.RollbackTrans()
                objLog.ErrorLog("DB_CancelVendor(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CancelVendor(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_GetFileNameById
        '	Discription	    : Get data file name vendor
        '	Return Value	: String
        '	Create User	    : Boonyarit
        '	Create Date	    : 30-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetFileNameById(ByVal intVendorId As Integer) As String Implements IMst_VendorDao.DB_GetFileNameById
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim strFileName As String

                DB_GetFileNameById = String.Empty

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT file As FileName   ")
                    .AppendLine(" FROM mst_vendor           ")
                    .AppendLine(" WHERE id = ?id            ")
                    ' assign parameter
                    objConn.AddParameter("?id", intVendorId)
                End With

                ' execute by scalar
                strFileName = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If (Not strFileName Is Nothing) And strFileName <> String.Empty Then
                    DB_GetFileNameById = strFileName
                End If

            Catch ex As Exception
                ' write error log
                DB_GetFileNameById = String.Empty
                objLog.ErrorLog("DB_GetFileNameById(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetFileNameById(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetVendorForList
        '	Discription	    : Get data vendor for set dropdownlist
        '	Return Value	: List
        '	Create User	    : Komsan L.
        '	Create Date	    : 03-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVendorForList(Optional ByVal intType1 As String = Nothing) As System.Collections.Generic.List(Of Entity.IMst_VendorEntity) Implements IMst_VendorDao.GetVendorForList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetVendorForList = New List(Of Entity.IMst_VendorEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable vendor entity
                Dim objVenderEnt As Entity.IMst_VendorEntity

                ' assign sql statement
                ' type1 0=vendor, 1=customer
                ' assign sql statement
                ' type1 0=vendor, 1=customer
                With strSql
                    .AppendLine("	SELECT id, name         ")
                    .AppendLine("	FROM mst_vendor 		")
                    .AppendLine("	WHERE delete_fg <> 1    ")
                    .AppendLine("	AND (type1=?type1       ")
                    .AppendLine("	OR ISNULL(?type1))      ")
                    'If intType1 > 0 Then
                    '    .AppendLine("	AND type1 = 1 		")
                    'End If
                    .AppendLine("	ORDER BY `name`		")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' Assign Parameter
                objConn.AddParameter("?type1", intType1)
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new vendor entity
                        objVenderEnt = New Entity.ImpMst_VendorEntity
                        With objVenderEnt
                            ' assign data to object Vendor entity
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                        ' add object Vendor entity to list
                        GetVendorForList.Add(objVenderEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVendorForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetVendorForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetVendorListForJobOrder
        '	Discription	    : Get data vendor for set dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVendorListForJobOrder() As System.Collections.Generic.List(Of Entity.IMst_VendorEntity) Implements IMst_VendorDao.GetVendorListForJobOrder
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetVendorListForJobOrder = New List(Of Entity.IMst_VendorEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable vendor entity
                Dim objVenderEnt As Entity.IMst_VendorEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT id, 		")
                    .AppendLine("		 name 	")
                    .AppendLine("	FROM mst_vendor 		")
                    .AppendLine("	WHERE delete_fg <> 1 		")
                    .AppendLine("	AND type1 = 1 		")
                    .AppendLine("	ORDER BY name		")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new vendor entity
                        objVenderEnt = New Entity.ImpMst_VendorEntity
                        With objVenderEnt
                            ' assign data to object Vendor entity
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                        ' add object Vendor entity to list
                        GetVendorListForJobOrder.Add(objVenderEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVendorListForJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetVendorListForJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_GetVendorForSearch
        '	Discription	    : Get data vendor for search
        '	Return Value	: IList
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetVendorForSearch(ByVal intType1 As Integer, ByVal intType2 As Integer, ByVal strName As String, ByVal intCountry_id As Integer) As System.Collections.Generic.List(Of Entity.ImpSubMst_VendorEntity) Implements IMst_VendorDao.DB_GetVendorForSearch
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable keep sql statement, datatable and ilist
                Dim objDT As System.Data.DataTable
                Dim objVendor As Entity.ImpSubMst_VendorEntity
                Dim objListParam As New List(Of MySqlParameter)

                DB_GetVendorForSearch = Nothing

                ' set new object
                strSQL = New Text.StringBuilder

                ' assign sql statement and parameter
                With strSQL
                    .AppendLine(" SELECT v.id                                       ")
                    .AppendLine(" 	,CASE v.type1                                   ")
                    .AppendLine(" 		WHEN 0                                      ")
                    .AppendLine(" 			THEN 'Vendor'                           ")
                    .AppendLine(" 		WHEN 1                                      ")
                    .AppendLine(" 			THEN 'Customer'                         ")
                    .AppendLine(" 		END AS type1_text                           ")
                    .AppendLine(" 	,CASE v.type2                                   ")
                    .AppendLine(" 		WHEN 0                                      ")
                    .AppendLine(" 			THEN 'Person'                           ")
                    .AppendLine(" 		WHEN 1                                      ")
                    .AppendLine(" 			THEN 'Company'                          ")
                    .AppendLine(" 		END AS type2_text                           ")
                    .AppendLine(" 	,v.NAME                                         ")
                    .AppendLine(" 	,c.NAME AS country                              ")
                    .AppendLine(" FROM mst_vendor v                                 ")
                    .AppendLine(" LEFT JOIN mst_country c ON v.country_id = c.id    ")
                    .AppendLine(" WHERE v.delete_fg <> 1                            ")
                    If intType1 > -1 Then
                        .AppendLine(" 	AND v.type1 = ?type1 ")
                        objListParam.Add(New MySqlParameter("?type1", intType1))
                    End If
                    If intType2 > -1 Then
                        .AppendLine(" 	AND v.type2 = ?type2 ")
                        objListParam.Add(New MySqlParameter("?type2", intType2))
                    End If
                    If (Not strName Is Nothing) AndAlso strName.Trim <> String.Empty Then
                        '.AppendLine(" 	AND v.NAME = ?name ")
                        'objListParam.Add(New MySqlParameter("?name", strName.Trim))
                        .AppendLine(" 	AND v.NAME Like '%" & strName.Trim & "%' ")
                    End If
                    If intCountry_id > 0 Then
                        .AppendLine(" 	AND v.country_id = ?country ")
                        objListParam.Add(New MySqlParameter("?country", intCountry_id))
                    End If
                    .AppendLine(" ORDER BY v.id ")
                End With

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                objConn.AddParameter(objListParam)
                objDT = New System.Data.DataTable

                ' execute by datatable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError

                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_GetVendorForSearch = New List(Of Entity.ImpSubMst_VendorEntity)

                ' assign value to entity object
                For Each objItem As System.Data.DataRow In objDT.Rows
                    objVendor = New Entity.ImpSubMst_VendorEntity
                    objVendor.id = objConn.CheckDBNull(objItem("id"), Common.DBConnection.DBType.DBDecimal)
                    objVendor.type1_text = objConn.CheckDBNull(objItem("type1_text"), Common.DBConnection.DBType.DBString)
                    objVendor.type2_text = objConn.CheckDBNull(objItem("type2_text"), Common.DBConnection.DBType.DBString)
                    objVendor.name = objConn.CheckDBNull(objItem("name"), Common.DBConnection.DBType.DBString)
                    objVendor.country_name = objConn.CheckDBNull(objItem("country"), Common.DBConnection.DBType.DBString)
                    DB_GetVendorForSearch.Add(objVendor)
                Next

            Catch ex As Exception
                ' write error log
                DB_GetVendorForSearch = Nothing
                objLog.ErrorLog("DB_GetVendorForSearch(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetVendorForSearch(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_GetVendorForReport
        '	Discription	    : Get data vendor for report
        '	Return Value	: IList
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetVendorForReport(ByVal intType1 As Integer, ByVal intType2 As Integer, ByVal strName As String, ByVal intCountry_id As Integer) As System.Collections.Generic.List(Of Entity.ImpVendorBranchEntity) Implements IMst_VendorDao.DB_GetVendorForReport
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable keep sql statement, datatable and ilist
                Dim objDT As New System.Data.DataTable
                Dim objVendor As Entity.ImpVendorBranchEntity

                DB_GetVendorForReport = Nothing

                ' assign sql statement and parameter
                With strSQL
                    .AppendLine("   SELECT v.id AS vendor_id, '' AS branch_id, v.name				            ")
                    .AppendLine("   	, CONCAT(v.address,' ',v.zipcode,', ',c.name) AS address		        ")
                    .AppendLine("       ,IF(v.person_in_charge2 = '' OR ISNULL(v.person_in_charge2)             ")
                    .AppendLine("       , v.person_in_charge1, CONCAT(v.person_in_charge1,', ',v.person_in_charge2)) as contact ")
                    '.AppendLine("   	, CONCAT(v.person_in_charge1,', ',v.person_in_charge2) AS contact		")
                    .AppendLine("   	, v.tel, v.fax, v.email, CONCAT(IF(v.purchase_fg= 1,'Purchase ','')		")
                    .AppendLine("   	, IF(v.outsource_fg= 1,'Outsource ','')									")
                    .AppendLine("   	, IF(v.other_fg= 1,'Other','')) AS type_of_goods, v.remarks				")
                    .AppendLine("   FROM mst_vendor v LEFT JOIN mst_country c ON v.country_id = c.id			")
                    .AppendLine("   WHERE v.delete_fg <> 1 														")
                    .AppendLine("   	AND (ISNULL(?type1) OR v.type1 = ?type1)								")
                    .AppendLine("   	AND (ISNULL(?type2) OR v.type2 = ?type2) 								")
                    .AppendLine("   	AND (ISNULL(?name) OR v.name LIKE CONCAT('%',?name,'%')) 				")
                    .AppendLine("   	AND (ISNULL(?country) OR v.country_id = ?country)						")
                    .AppendLine("   UNION																		")
                    .AppendLine("   SELECT b.vendor_id, b.id AS branch_id, b.name								")
                    .AppendLine("   	, CONCAT(b.address,' ',b.zipcode,', ',c.name) AS address				")
                    .AppendLine("   	, b.contact, b.tel, b.fax, b.email, '' AS type_of_goods, b.remarks		")
                    .AppendLine("   FROM mst_vendor_branch b													")
                    .AppendLine("   	LEFT JOIN mst_country c ON b.country_id = c.id AND c.delete_fg <> 1		")
                    .AppendLine("   	LEFT JOIN mst_vendor v ON b.vendor_id = v.id 							")
                    .AppendLine("   WHERE b.delete_fg <> 1 AND v.delete_fg <> 1 								")
                    .AppendLine("   	AND (ISNULL(?type1) OR v.type1 = ?type1)								")
                    .AppendLine("   	AND (ISNULL(?type2) OR v.type2 = ?type2) 								")
                    .AppendLine("   	AND (ISNULL(?name) OR v.name LIKE CONCAT('%',?name,'%')) 				")
                    .AppendLine("   	AND (ISNULL(?country) OR v.country_id = ?country)						")
                    .AppendLine("   ORDER BY vendor_id, branch_id;												")
                End With

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                With objConn
                    .AddParameter("?type1", IIf(intType1 = -1, DBNull.Value, intType1))
                    .AddParameter("?type2", IIf(intType2 = -1, DBNull.Value, intType2))
                    .AddParameter("?name", IIf(strName = Nothing, DBNull.Value, strName))
                    .AddParameter("?country", IIf(intCountry_id = 0, DBNull.Value, intCountry_id))
                End With

                ' execute by datatable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError

                ' check data
                If objDT Is Nothing OrElse objDT.Rows.Count = 0 Then Exit Function
                DB_GetVendorForReport = New List(Of Entity.ImpVendorBranchEntity)

                ' assign value to entity object
                For Each objItem As System.Data.DataRow In objDT.Rows
                    objVendor = New Entity.ImpVendorBranchEntity
                    With objVendor
                        .id = IIf(objItem("branch_id") = "" OrElse IsDBNull(objItem("branch_id")), 0, objItem("branch_id"))
                        .vendorID = IIf(CStr(objItem("vendor_id")) = "", 0, CInt(objItem("vendor_id")))
                        .name = IIf(IsDBNull(objItem("name")), String.Empty, objItem("name"))
                        .fullAddress = IIf(IsDBNull(objItem("address")), String.Empty, objItem("address"))
                        .contact = IIf(IsDBNull(objItem("contact")), String.Empty, objItem("contact"))
                        .telNo = IIf(IsDBNull(objItem("tel")), String.Empty, objItem("tel"))
                        .faxNo = IIf(IsDBNull(objItem("fax")), String.Empty, objItem("fax"))
                        .email = IIf(IsDBNull(objItem("email")), String.Empty, objItem("email"))
                        .typeOfGoods = IIf(IsDBNull(objItem("type_of_goods")), String.Empty, objItem("type_of_goods"))
                        .remarks = IIf(IsDBNull(objItem("remarks")), String.Empty, objItem("remarks"))
                    End With
                    DB_GetVendorForReport.Add(objVendor)
                Next

            Catch ex As Exception
                ' write error log
                DB_GetVendorForReport = Nothing
                objLog.ErrorLog("DB_GetVendorForReport(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetVendorForReport(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_GetVendorForDetail
        '	Discription	    : Get data vendor for detail
        '	Return Value	: Class entity
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetVendorForDetail(ByVal intID As Integer) As Entity.ImpSubMst_VendorEntity Implements IMst_VendorDao.DB_GetVendorForDetail
            Dim strSQL As New Text.StringBuilder

            Try
                ' variable keep sql statement, datatable and ilist
                Dim objDT As System.Data.DataTable
                Dim objDR As System.Data.DataRow

                DB_GetVendorForDetail = Nothing

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and parameter
                With strSQL
                    .AppendLine(" SELECT v.id ")
                    '***Start Boon add for Main
                    .AppendLine(" 	,v.type1 ")
                    .AppendLine(" 	,v.type2 ")
                    .AppendLine(" 	,v.payment_term_id ")
                    .AppendLine(" 	,v.payment_cond1 ")
                    .AppendLine(" 	,v.payment_cond2 ")
                    .AppendLine(" 	,v.payment_cond3 ")
                    .AppendLine(" 	,v.country_id    ")
                    .AppendLine(" 	,v.type_of_goods ")
                    .AppendLine(" 	,v.purchase_fg   ")
                    .AppendLine(" 	,v.outsource_fg  ")
                    .AppendLine(" 	,v.other_fg      ")
                    '***End Boon add for Main
                    .AppendLine(" 	,CASE v.type1 ")
                    .AppendLine(" 		WHEN 0 ")
                    .AppendLine(" 			THEN 'Vendor' ")
                    .AppendLine(" 		WHEN 1 ")
                    .AppendLine(" 			THEN 'Customer' ")
                    .AppendLine(" 		END AS type1_text ")
                    .AppendLine(" 	,CASE v.type2 ")
                    .AppendLine(" 		WHEN 0 ")
                    .AppendLine(" 			THEN 'Person' ")
                    .AppendLine(" 		WHEN 1 ")
                    .AppendLine(" 			THEN 'Company' ")
                    .AppendLine(" 		END AS type2_text ")
                    .AppendLine(" 	,v.type2_no ")
                    .AppendLine(" 	,v.name ")
                    .AppendLine(" 	,v.abbr ")
                    .AppendLine(" 	,v.person_in_charge1 ")
                    .AppendLine(" 	,v.person_in_charge2 ")
                    .AppendLine(" 	,CONCAT(p.term_day,' days') AS payment_term ")
                    .AppendLine(" 	,CONCAT(v.payment_cond1,'%',v.payment_cond2,'%',v.payment_cond3,'%') AS  payment_condition ")
                    .AppendLine(" 	,c.name AS country_name ")
                    .AppendLine(" 	,v.zipcode ")
                    .AppendLine(" 	,v.address ")
                    .AppendLine(" 	,v.tel ")
                    .AppendLine(" 	,v.fax ")
                    .AppendLine(" 	,v.email ")
                    .AppendLine(" 	,CASE v.type_of_goods ")
                    .AppendLine(" 		WHEN '1,0' ")
                    .AppendLine(" 			THEN 'Purchase' ")
                    .AppendLine(" 		WHEN '0,1' ")
                    .AppendLine(" 			THEN 'Outsource' ")
                    .AppendLine(" 		WHEN '1,1' ")
                    .AppendLine(" 			THEN 'Purchase, Outsource' ")
                    .AppendLine(" 		END AS type_of_goods_text ")
                    .AppendLine(" 	,v.remarks ")
                    .AppendLine(" 	,v.file ")
                    .AppendLine(" FROM mst_vendor v ")
                    .AppendLine(" LEFT JOIN mst_country c ON v.country_id = c.id ")
                    .AppendLine(" LEFT JOIN mst_payment_term p ON v.payment_term_id = p.id ")
                    .AppendLine(" WHERE v.id = ?id ")
                    objConn.AddParameter("?id", intID)
                End With

                ' set new object
                objDT = New System.Data.DataTable

                ' execute by datatable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError

                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                objDR = objDT.Rows(0)
                DB_GetVendorForDetail = New Entity.ImpSubMst_VendorEntity

                ' assign value to entity object
                With DB_GetVendorForDetail
                    .id = objConn.CheckDBNull(objDR("id"), Common.DBConnection.DBType.DBDecimal)
                    '***Start Boon add for Main
                    .type1 = objConn.CheckDBNull(objDR("type1"), Common.DBConnection.DBType.DBDecimal)
                    .type2 = objConn.CheckDBNull(objDR("type2"), Common.DBConnection.DBType.DBDecimal)
                    .payment_term_id = objConn.CheckDBNull(objDR("payment_term_id"), Common.DBConnection.DBType.DBDecimal)
                    .payment_cond1 = objConn.CheckDBNull(objDR("payment_cond1"), Common.DBConnection.DBType.DBDecimal)
                    .payment_cond2 = objConn.CheckDBNull(objDR("payment_cond2"), Common.DBConnection.DBType.DBDecimal)
                    .payment_cond3 = objConn.CheckDBNull(objDR("payment_cond3"), Common.DBConnection.DBType.DBDecimal)
                    .country_id = objConn.CheckDBNull(objDR("country_id"), Common.DBConnection.DBType.DBDecimal)
                    .type_of_goods = objConn.CheckDBNull(objDR("type_of_goods"), Common.DBConnection.DBType.DBString)
                    .purchase_fg = objConn.CheckDBNull(objDR("purchase_fg"), Common.DBConnection.DBType.DBString)
                    .outsource_fg = objConn.CheckDBNull(objDR("outsource_fg"), Common.DBConnection.DBType.DBString)
                    .other_fg = objConn.CheckDBNull(objDR("other_fg"), Common.DBConnection.DBType.DBString)
                    '***End Boon add for Main
                    .type1_text = objConn.CheckDBNull(objDR("type1_text"), Common.DBConnection.DBType.DBString)
                    .type2_text = objConn.CheckDBNull(objDR("type2_text"), Common.DBConnection.DBType.DBString)
                    .type2_no = objConn.CheckDBNull(objDR("type2_no"), Common.DBConnection.DBType.DBString)
                    .name = objConn.CheckDBNull(objDR("name"), Common.DBConnection.DBType.DBString)
                    .short_name = objConn.CheckDBNull(objDR("abbr"), Common.DBConnection.DBType.DBString)
                    .person_in_charge1 = objConn.CheckDBNull(objDR("person_in_charge1"), Common.DBConnection.DBType.DBString)
                    .person_in_charge2 = objConn.CheckDBNull(objDR("person_in_charge2"), Common.DBConnection.DBType.DBString)
                    .payment_term = objConn.CheckDBNull(objDR("payment_term"), Common.DBConnection.DBType.DBString)
                    .payment_condition = objConn.CheckDBNull(objDR("payment_condition"), Common.DBConnection.DBType.DBString)
                    .country_name = objConn.CheckDBNull(objDR("country_name"), Common.DBConnection.DBType.DBString)
                    .zipcode = objConn.CheckDBNull(objDR("zipcode"), Common.DBConnection.DBType.DBString)
                    .address = objConn.CheckDBNull(objDR("address"), Common.DBConnection.DBType.DBString)
                    .tel = objConn.CheckDBNull(objDR("tel"), Common.DBConnection.DBType.DBString)
                    .fax = objConn.CheckDBNull(objDR("fax"), Common.DBConnection.DBType.DBString)
                    .email = objConn.CheckDBNull(objDR("email"), Common.DBConnection.DBType.DBString)
                    .type_of_goods_text = objConn.CheckDBNull(objDR("type_of_goods_text"), Common.DBConnection.DBType.DBString)
                    .remarks = objConn.CheckDBNull(objDR("remarks"), Common.DBConnection.DBType.DBString)
                    .file = objConn.CheckDBNull(objDR("file"), Common.DBConnection.DBType.DBString)
                End With

            Catch ex As Exception
                ' write error log
                DB_GetVendorForDetail = Nothing
                objLog.ErrorLog("DB_GetVendorForDetail(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetVendorForDetail(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

    End Class
End Namespace

