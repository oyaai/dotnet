#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpVendorBranchDao
'	Class Discription	: Interface of table mst_vendor_branch
'	Create User 		: Wasan D.
'	Create Date		    : 26-09-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports System.Data
Imports MySql.Data.MySqlClient
Imports Microsoft.VisualBasic

Namespace Dao
    Public Class ImpVendorBranchDao
        Implements IVendorBranchDao

        Private objLog As New Common.Logs.Log
        Private objConn As Common.DBConnection.MySQLAccess
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: GetBranchWithVendorID
        '	Discription	    : GetBranchWithVendorID
        '	Return Value	: list(Of Entity.ImpVendorBranch)
        '	Create User	    : Wasan D.
        '	Create Date	    : 07-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetBranchWithVendorID(ByVal intVendorID As Integer) As  _
        System.Collections.Generic.List(Of Entity.ImpVendorBranchEntity) _
        Implements IVendorBranchDao.GetBranchWithVendorID
            Dim strSql As New StringBuilder
            GetBranchWithVendorID = New List(Of Entity.ImpVendorBranchEntity)
            Try
                Dim dr As MySqlDataReader
                Dim objVendorBranchEnt As Entity.ImpVendorBranchEntity
                With strSql
                    .AppendLine("   SELECT b.id, b.name, b.vendor_id, b.address, c.id AS CountryID  ")
                    .AppendLine("   	, c.name AS CountryName, CONCAT(IF(ISNULL(b.address), ''    ")
                    .AppendLine("   	, b.address), ' ', IF(ISNULL(b.zipcode), '', b.zipcode)	    ")
                    .AppendLine("   	,' ',IF(ISNULL(c.name),'',c.name)) AS FullAddress		    ")
                    .AppendLine("   	, b.zipcode, b.tel, b.fax, b.email, b.contact, b.remarks	")
                    .AppendLine("   FROM mst_vendor_branch b LEFT JOIN mst_country c			    ")
                    .AppendLine("   	ON b.country_id = c.id AND c.delete_fg <> 1				    ")
                    .AppendLine("   WHERE b.vendor_id=?vendor_id AND b.delete_fg <> 1;	            ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' Assign parameter
                objConn.AddParameter("?vendor_id", intVendorID)
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)
                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new vendor entity
                        objVendorBranchEnt = New Entity.ImpVendorBranchEntity
                        With objVendorBranchEnt
                            ' assign data to object Vendor entity
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                            .vendorID = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                            .address = IIf(IsDBNull(dr.Item("address")), Nothing, dr.Item("address"))
                            .zipcode = IIf(IsDBNull(dr.Item("zipcode")), Nothing, dr.Item("zipcode"))
                            .countryID = IIf(IsDBNull(dr.Item("CountryID")), Nothing, dr.Item("CountryID"))
                            .countryName = IIf(IsDBNull(dr.Item("CountryName")), Nothing, dr.Item("CountryName"))
                            .fullAddress = IIf(IsDBNull(dr.Item("FullAddress")), Nothing, dr.Item("FullAddress"))
                            .telNo = IIf(IsDBNull(dr.Item("tel")), Nothing, dr.Item("tel"))
                            .faxNo = IIf(IsDBNull(dr.Item("fax")), Nothing, dr.Item("fax"))
                            .email = IIf(IsDBNull(dr.Item("email")), Nothing, dr.Item("email"))
                            .contact = IIf(IsDBNull(dr.Item("contact")), Nothing, dr.Item("contact"))
                            .remarks = IIf(IsDBNull(dr.Item("remarks")), Nothing, dr.Item("remarks"))
                        End With
                        ' add object Vendor entity to list
                        GetBranchWithVendorID.Add(objVendorBranchEnt)
                    End While
                End If
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("GetBranchWithVendorID", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not IsNothing(objConn) Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckBranchIsInUse
        '	Discription	    : CheckBranchIsInUse
        '	Return Value	: Integer
        '	Create User	    : Wasan D.
        '	Create Date	    : 07-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckBranchIsInUse(ByVal intBranchID As Integer) As Integer _
        Implements IVendorBranchDao.CheckBranchIsInUse
            Dim strSql As New StringBuilder
            CheckBranchIsInUse = -1
            Try
                With strSql
                    .AppendLine("   SELECT  (SELECT COUNT(*) AS cnt FROM po_header                                  ")
                    .AppendLine("   		    WHERE vendor_branch_id = ?vendor_branch_id AND status_id <> 6)+     ")
                    .AppendLine("           (SELECT COUNT(*) AS cnt FROM accounting                                 ")
                    .AppendLine("   		    WHERE vendor_branch_id = ?vendor_branch_id AND status_id <> 6)      ")
                    .AppendLine("   AS useCount;	                                                                ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' Assign parameter
                objConn.AddParameter("?vendor_branch_id", intBranchID)
                ' execute sql statement
                CheckBranchIsInUse = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("CheckBranchIsInUse(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckBranchIsInUse(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckBranchIsInUse(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not IsNothing(objConn) Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SaveVendorBranch
        '	Discription	    : SaveVendorBranch
        '	Return Value	: Integer
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SaveVendorBranch(ByVal listBranchEnt As  _
        System.Collections.Generic.List(Of Entity.ImpVendorBranchEntity)) As Integer _
        Implements IVendorBranchDao.SaveVendorBranch
            Dim strSql As New StringBuilder
            SaveVendorBranch = -1
            Try
                Dim doSuccess As Boolean = True
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)
                ' Loop for Insert or Update database
                For Each objBranchEnt As Entity.ImpVendorBranchEntity In listBranchEnt
                    If objBranchEnt.id <> 0 OrElse objBranchEnt.id <> Nothing Then
                        If UpdateVendorBranch(objBranchEnt) <= 0 Then
                            doSuccess = False
                            Exit For
                        End If
                    Else
                        If InsertVendorBranch(objBranchEnt) <= 0 Then
                            doSuccess = False
                            Exit For
                        End If
                    End If
                Next
                ' check row effect finally
                If doSuccess = True Then
                    ' case row effect > 0 then commit transaction
                    objConn.CommitTrans()
                    SaveVendorBranch = 1
                Else
                    ' case row effect <= 0 then rollback transaction
                    objConn.RollbackTrans()
                End If
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("SaveVendorBranch(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SaveVendorBranch(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("SaveVendorBranch(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not IsNothing(objConn) Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SaveVendorBranch
        '	Discription	    : SaveVendorBranch
        '	Return Value	: Integer
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function InsertVendorBranch(ByVal objBranchEnt As Entity.ImpVendorBranchEntity) As Integer
            Dim strSql As New StringBuilder
            InsertVendorBranch = -1
            Try
                With strSql
                    .AppendLine("   INSERT INTO mst_vendor_branch (name, vendor_id, address, zipcode      ")
                    .AppendLine("   	, country_id, tel, fax, email, contact, remarks, delete_fg        ")
                    .AppendLine("   	, created_by, created_date, updated_by, updated_date)             ")
                    .AppendLine("   VALUES (?name, ?vendor_id, ?address, ?zipcode, ?country_id            ")
                    .AppendLine("   	, ?tel, ?fax, ?email, ?contact, ?remarks, 0, ?user_id             ")
                    .AppendLine("   	, date_format(now(), '%Y%m%d%H%i%s'), ?user_id                    ")
                    .AppendLine("   	, date_format(now(),'%Y%m%d%H%i%s'))                              ")
                End With

                ' assign(Parameter)
                With objConn
                    .ClearParameter()
                    .AddParameter("?name", objBranchEnt.name)
                    .AddParameter("?vendor_id", objBranchEnt.vendorID)
                    .AddParameter("?address", objBranchEnt.address)
                    .AddParameter("?zipcode", objBranchEnt.zipcode)
                    .AddParameter("?country_id", objBranchEnt.countryID)
                    .AddParameter("?tel", objBranchEnt.telNo)
                    .AddParameter("?fax", objBranchEnt.faxNo)
                    .AddParameter("?email", objBranchEnt.email)
                    .AddParameter("?contact", objBranchEnt.contact)
                    .AddParameter("?remarks", objBranchEnt.remarks)
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    ' execute nonquery with sql command
                    InsertVendorBranch = objConn.ExecuteNonQuery(strSql.ToString)
                End With
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertVendorBranch(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertVendorBranch(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("InsertVendorBranch(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SaveVendorBranch
        '	Discription	    : SaveVendorBranch
        '	Return Value	: Integer
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function UpdateVendorBranch(ByVal objBranchEnt As Entity.ImpVendorBranchEntity) As Integer
            Dim strSql As New StringBuilder
            UpdateVendorBranch = -1
            Try
                With strSql
                    .AppendLine("   UPDATE mst_vendor_branch                                  ")
                    .AppendLine("   SET name=?name, vendor_id=?vendor_id                      ")
                    .AppendLine("   	, address=?address, zipcode=?zipcode                  ")
                    .AppendLine("   	, country_id=?country_id, tel=?tel, fax=?fax          ")
                    .AppendLine("   	, email=?email, contact=?contact, remarks=?remarks    ")
                    .AppendLine("   	, delete_fg=?delete_fg , updated_by=?user_id          ")
                    .AppendLine("   	, updated_date=date_format(now(),'%Y%m%d%H%i%s')      ")
                    .AppendLine("   WHERE id = ?id;                                           ")
                End With

                ' assign(Parameter)
                With objConn
                    .ClearParameter()
                    .AddParameter("?id", objBranchEnt.id)
                    .AddParameter("?name", objBranchEnt.name)
                    .AddParameter("?vendor_id", objBranchEnt.vendorID)
                    .AddParameter("?address", objBranchEnt.address)
                    .AddParameter("?zipcode", objBranchEnt.zipcode)
                    .AddParameter("?country_id", objBranchEnt.countryID)
                    .AddParameter("?tel", objBranchEnt.telNo)
                    .AddParameter("?fax", objBranchEnt.faxNo)
                    .AddParameter("?email", objBranchEnt.email)
                    .AddParameter("?contact", objBranchEnt.contact)
                    .AddParameter("?remarks", objBranchEnt.remarks)
                    .AddParameter("?delete_fg", objBranchEnt.delete_fg)
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    ' execute nonquery with sql command
                    UpdateVendorBranch = objConn.ExecuteNonQuery(strSql.ToString)
                End With
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateVendorBranch(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateVendorBranch(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("UpdateVendorBranch(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SaveVendorBranch
        '	Discription	    : SaveVendorBranch
        '	Return Value	: List(Of Entity.ImpVendorBranchEntity)
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVendorBranchForDDLList(ByVal intVendorID As Integer) _
        As System.Collections.Generic.List(Of Entity.ImpVendorBranchEntity) _
        Implements IVendorBranchDao.GetVendorBranchForDDLList
            Dim strSql As New StringBuilder
            GetVendorBranchForDDLList = New List(Of Entity.ImpVendorBranchEntity)
            Try
                Dim objVBEnt As Entity.ImpVendorBranchEntity
                Dim dr As MySqlDataReader
                With strSql
                    .AppendLine("   SELECT b.id, CONCAT(IF(ISNULL(b.name), '', b.name), ' ', IF(ISNULL(b.address)	")
                    .AppendLine("   	, '', b.address), ' ', IF(ISNULL(b.zipcode), '', b.zipcode), ' '			")
                    .AppendLine("   	, IF(ISNULL(c.name), '', c.name)) AS address								")
                    .AppendLine("   FROM mst_vendor_branch b														")
                    .AppendLine("   LEFT JOIN mst_country c ON b.country_id = c.id AND c.delete_fg <> 1				")
                    .AppendLine("   WHERE b.delete_fg <> 1 AND b.vendor_id = ?vendor_id 							")
                    .AppendLine("   UNION																			")
                    .AppendLine("   SELECT 0, CONCAT('Head Office ' ', ', IF(ISNULL(v.address), '', v.address)		")
                    .AppendLine("   	, ' ', IF(ISNULL(v.zipcode), '', v.zipcode), ' '							")
                    .AppendLine("   	, IF(ISNULL(c.name), '', c.name)) AS address								")
                    .AppendLine("   FROM mst_vendor v																")
                    .AppendLine("   LEFT JOIN mst_country c ON v.country_id = c.id AND c.delete_fg <> 1				")
                    .AppendLine("   WHERE v.delete_fg <> 1 AND v.id = ?vendor_id 									")
                    .AppendLine("   ORDER BY id;																	")
                End With
                objConn = New Common.DBConnection.MySQLAccess
                objConn.AddParameter("?vendor_id", intVendorID)
                dr = objConn.ExecuteReader(strSql.ToString)
                If dr.HasRows Then
                    While dr.Read
                        objVBEnt = New Entity.ImpVendorBranchEntity
                        objVBEnt.id = dr("id")
                        objVBEnt.fullAddress = dr("address")
                        GetVendorBranchForDDLList.Add(objVBEnt)
                    End While
                End If
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("GetVendorBranchForDDLList(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVendorBranchForDDLList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("GetVendorBranchForDDLList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not IsNothing(objConn) Then objConn.Close()
            End Try
        End Function

    End Class
End Namespace
