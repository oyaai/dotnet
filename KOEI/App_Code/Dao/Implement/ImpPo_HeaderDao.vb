#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpPo_HeaderDao
'	Class Discription	: Class of table po_header
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
Imports Microsoft.VisualBasic
Imports Exceptions
'Imports Extensions
Imports Enums
Imports MySql.Data.MySqlClient
#End Region

Namespace Dao
    Public Class ImpPo_HeaderDao
        Implements IPo_HeaderDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: DB_CheckPoByVendor
        '	Discription	    : Check Po_Header by vendor
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckPoByVendor(ByVal intVendor_id As Integer) As Boolean Implements IPo_HeaderDao.DB_CheckPoByVendor
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlagCount As Integer = 0

                DB_CheckPoByVendor = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT Count(*) As po_count ")
                    .AppendLine(" FROM po_header ")
                    .AppendLine(" WHERE vendor_id = ?VendorId ")
                    ' assign parameter
                    objConn.AddParameter("?VendorId", intVendor_id)
                End With

                ' execute by scalar
                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlagCount > 0 Then
                    DB_CheckPoByVendor = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CheckPoByVendor = False
                objLog.ErrorLog("DB_CheckPoByVendor(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CheckPoByVendor(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_SearchPurchase
        '	Discription	    : Search purchase 
        '	Return Value	: ImpPurchaseEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_SearchPurchase(ByVal intPurchaseId As Integer) As Entity.ImpPurchaseEntity Implements IPo_HeaderDao.DB_SearchPurchase
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim objDS As System.Data.DataSet
                Dim objDT As System.Data.DataTable
                Dim objItem As System.Data.DataRow
                Dim objPurchaseDetail As Entity.ImpPurchaseDetailEntity

                DB_SearchPurchase = Nothing

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and assign parameter
                With strSQL
                    .AppendLine(" SELECT A.*,B.name as vendor_name,IF(ifnull(A.vendor_branch_id,0)=0,'Head Office',C.name) vendor_branch_name,if(ISNULL(D.id),0,1) as delivery_fg ")
                    .AppendLine(" FROM po_header A join mst_vendor B on A.vendor_id=B.id                                                                                            ")
                    .AppendLine(" left join payment_header D on D.po_header_id=A.id                                                                                                 ")
                    .AppendLine(" left join mst_vendor_branch C on C.vendor_id=A.vendor_id and C.id=A.vendor_branch_id                                                               ")
                    .AppendLine(" WHERE A.id = ?purchase_id; ")

                    '.AppendLine(" SELECT * ")
                    '.AppendLine(" FROM po_detail ")
                    '.AppendLine(" WHERE po_header_id = ?purchase_id; ")
                    .AppendLine(" SELECT A.id ")
                    .AppendLine(" 	,B.ITName As item_name ")
                    .AppendLine(" 	,A.job_order ")
                    .AppendLine(" 	,C.IEName As ie_name ")
                    .AppendLine(" 	,A.unit_price ")
                    .AppendLine(" 	,A.quantity ")
                    .AppendLine(" 	,D.UName As unit_name ")
                    .AppendLine(" 	,A.discount ")
                    '.AppendLine("   ,E.vat_id   ")
                    .AppendLine(" 	,CASE discount_type ")
                    .AppendLine(" 		WHEN 0 ")
                    .AppendLine(" 			THEN 'No Discount' ")
                    .AppendLine(" 		WHEN 1 ")
                    .AppendLine(" 			THEN F.CName ")
                    .AppendLine(" 		WHEN 2 ")
                    .AppendLine(" 			THEN 'Percent(%)' ")
                    .AppendLine(" 		END As discount_type_text ")
                    .AppendLine(" 	,A.vat_amount ")
                    .AppendLine(" 	,A.wt_amount ")
                    .AppendLine(" 	,A.amount ")
                    .AppendLine(" 	,A.remark ")
                    .AppendLine(" 	,A.* ")
                    .AppendLine(" FROM po_detail A ")
                    .AppendLine(" JOIN po_header E ON A.po_header_id = E.id ")
                    .AppendLine(" JOIN ( ")
                    .AppendLine(" 	SELECT * ")
                    .AppendLine(" 		,NAME AS ITName ")
                    .AppendLine(" 	FROM mst_item ")
                    .AppendLine(" 	WHERE delete_fg = 0 ")
                    .AppendLine(" 	) B ON B.id = A.item_id ")
                    .AppendLine(" JOIN ( ")
                    .AppendLine(" 	SELECT * ")
                    .AppendLine(" 		,NAME AS IEName ")
                    .AppendLine(" 	FROM mst_ie ")
                    .AppendLine(" 	WHERE delete_fg = 0 ")
                    .AppendLine(" 	) C ON C.id = A.ie_id ")
                    .AppendLine(" JOIN ( ")
                    .AppendLine(" 	SELECT * ")
                    .AppendLine(" 		,NAME AS UName ")
                    .AppendLine(" 	FROM mst_unit ")
                    .AppendLine(" 	WHERE delete_fg = 0 ")
                    .AppendLine(" 	) D ON D.id = A.unit_id ")
                    .AppendLine(" JOIN ( ")
                    .AppendLine(" 	SELECT * ")
                    .AppendLine(" 		,NAME AS CName ")
                    .AppendLine(" 	FROM mst_currency ")
                    .AppendLine(" 	WHERE delete_fg = 0 ")
                    .AppendLine(" 	) F ON E.currency_id = F.id ")
                    .AppendLine(" WHERE A.po_header_id = ?purchase_id ")
                    .AppendLine("   AND E.status_id <> 6 ")
                    .AppendLine(" ORDER BY A.id; ")

                    ' assign parameter
                    objConn.AddParameter("?purchase_id", intPurchaseId)

                End With

                ' execute by datatable
                objDS = objConn.ExecuteDataSet(strSQL.ToString)
                strMsgErr = objConn.MessageError
                If objDS.Tables.Count <> 2 Then Exit Function
                objDT = New System.Data.DataTable
                objDT = objDS.Tables(0)
                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                objItem = objDT.Rows(0)
                DB_SearchPurchase = New Entity.ImpPurchaseEntity
                With DB_SearchPurchase
                    'id() As Integer
                    .id = objConn.CheckDBNull(objItem("id"), Common.DBConnection.DBType.DBDecimal)
                    'po_no() As String
                    .po_no = objConn.CheckDBNull(objItem("po_no"), Common.DBConnection.DBType.DBString)
                    'po_type() As Integer
                    .po_type = objConn.CheckDBNull(objItem("po_type"), Common.DBConnection.DBType.DBDecimal)
                    'vendor_id() As Integer
                    .vendor_id = objConn.CheckDBNull(objItem("vendor_id"), Common.DBConnection.DBType.DBDecimal)
                    .vendor_name = objConn.CheckDBNull(objItem("vendor_name"), Common.DBConnection.DBType.DBString)
                    .vendor_branch_id = objConn.CheckDBNull(objItem("vendor_branch_id"), Common.DBConnection.DBType.DBDecimal)
                    .vendor_branch_name = objConn.CheckDBNull(objItem("vendor_branch_name"), Common.DBConnection.DBType.DBString)
                    .delivery_fg = objConn.CheckDBNull(objItem("delivery_fg"), Common.DBConnection.DBType.DBDecimal)
                    'quotation_no() As String
                    .quotation_no = objConn.CheckDBNull(objItem("quotation_no"), Common.DBConnection.DBType.DBString)
                    'issue_date() As DateString
                    .issue_date = objConn.CheckDBNull(objItem("issue_date"), Common.DBConnection.DBType.DBString).ToString
                    'delivery_date() As DateString
                    .delivery_date = objConn.CheckDBNull(objItem("delivery_date"), Common.DBConnection.DBType.DBString).ToString
                    'payment_term_id() As Integer
                    .payment_term_id = objConn.CheckDBNull(objItem("payment_term_id"), Common.DBConnection.DBType.DBDecimal)
                    'vat_id() As Integer
                    .vat_id = objConn.CheckDBNull(objItem("vat_id"), Common.DBConnection.DBType.DBDecimal)
                    'wt_id() As Integer
                    .wt_id = objConn.CheckDBNull(objItem("wt_id"), Common.DBConnection.DBType.DBDecimal)
                    'currency_id() As Integer
                    .currency_id = objConn.CheckDBNull(objItem("currency_id"), Common.DBConnection.DBType.DBDecimal)
                    'remark() As String
                    .remark = objConn.CheckDBNull(objItem("remark"), Common.DBConnection.DBType.DBString)
                    'discount_total() As Decimal
                    .discount_total = objConn.CheckDBNull(objItem("discount_total"), Common.DBConnection.DBType.DBDecimal)
                    'sub_total() As Decimal
                    .sub_total = objConn.CheckDBNull(objItem("sub_total"), Common.DBConnection.DBType.DBDecimal)
                    'vat_amount() As Decimal
                    .vat_amount = objConn.CheckDBNull(objItem("vat_amount"), Common.DBConnection.DBType.DBDecimal)
                    'wt_amount() As Decimal
                    .wt_amount = objConn.CheckDBNull(objItem("wt_amount"), Common.DBConnection.DBType.DBDecimal)
                    'total_amount() As Decimal
                    .total_amount = objConn.CheckDBNull(objItem("total_amount"), Common.DBConnection.DBType.DBDecimal)
                    'attn() As String
                    .attn = objConn.CheckDBNull(objItem("attn"), Common.DBConnection.DBType.DBString)
                    'deliver_to() As String
                    .deliver_to = objConn.CheckDBNull(objItem("deliver_to"), Common.DBConnection.DBType.DBString)
                    'contact() As String
                    .contact = objConn.CheckDBNull(objItem("contact"), Common.DBConnection.DBType.DBString)
                    'user_id() As Integer
                    .user_id = objConn.CheckDBNull(objItem("user_id"), Common.DBConnection.DBType.DBDecimal)
                    'status_id() As Integer
                    .status_id = objConn.CheckDBNull(objItem("status_id"), Common.DBConnection.DBType.DBDecimal)
                    'created_by() As Integer
                    .created_by = objConn.CheckDBNull(objItem("created_by"), Common.DBConnection.DBType.DBDecimal)
                    'created_date() As DateString
                    .created_date = objConn.CheckDBNull(objItem("created_date"), Common.DBConnection.DBType.DBString).ToString
                    'update_by() As Integer
                    .updated_by = objConn.CheckDBNull(objItem("updated_by"), Common.DBConnection.DBType.DBDecimal)
                    'update_date() As DateString
                    .updated_date = objConn.CheckDBNull(objItem("updated_date"), Common.DBConnection.DBType.DBString).ToString

                End With

                objDT = New System.Data.DataTable
                objDT = objDS.Tables(1)
                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_SearchPurchase.purchase_detail = New List(Of Entity.ImpPurchaseDetailEntity)
                For Each objItem In objDT.Rows
                    objPurchaseDetail = New Entity.ImpPurchaseDetailEntity
                    With objPurchaseDetail
                        'id() As Integer
                        .id = objConn.CheckDBNull(objItem("id"), Common.DBConnection.DBType.DBDecimal)
                        'po_header_id() As Integer
                        .po_header_id = objConn.CheckDBNull(objItem("po_header_id"), Common.DBConnection.DBType.DBDecimal)
                        'item_id() As Integer
                        .item_id = objConn.CheckDBNull(objItem("item_id"), Common.DBConnection.DBType.DBDecimal)
                        ',B.ITName As item_name
                        .item_name = objConn.CheckDBNull(objItem("item_name"), Common.DBConnection.DBType.DBString)
                        'job_order() As String
                        .job_order = objConn.CheckDBNull(objItem("job_order"), Common.DBConnection.DBType.DBString)
                        'ie_id() As Integer
                        .ie_id = objConn.CheckDBNull(objItem("ie_id"), Common.DBConnection.DBType.DBDecimal)
                        ',C.IEName As ie_name
                        .ie_name = objConn.CheckDBNull(objItem("ie_name"), Common.DBConnection.DBType.DBString)
                        'unit_price() As Decimal
                        .unit_price = objConn.CheckDBNull(objItem("unit_price"), Common.DBConnection.DBType.DBDecimal)
                        'quantity() As Integer
                        .quantity = objConn.CheckDBNull(objItem("quantity"), Common.DBConnection.DBType.DBDecimal)
                        'unit_id() As Integer
                        .unit_id = objConn.CheckDBNull(objItem("unit_id"), Common.DBConnection.DBType.DBDecimal)
                        ',D.UName As unit_name
                        .unit_name = objConn.CheckDBNull(objItem("unit_name"), Common.DBConnection.DBType.DBString)
                        'discount() As Decimal
                        .discount = objConn.CheckDBNull(objItem("discount"), Common.DBConnection.DBType.DBDecimal)
                        'discount_type() As Integer
                        .discount_type = objConn.CheckDBNull(objItem("discount_type"), Common.DBConnection.DBType.DBDecimal)
                        ',discount_type_text
                        .discount_type_text = objConn.CheckDBNull(objItem("discount_type_text"), Common.DBConnection.DBType.DBString)
                        'amount() As Decimal
                        .amount = objConn.CheckDBNull(objItem("amount"), Common.DBConnection.DBType.DBDecimal)
                        'vat_amount() As Decimal
                        .vat_amount = objConn.CheckDBNull(objItem("vat_amount"), Common.DBConnection.DBType.DBDecimal)
                        'wt_amount() As Decimal
                        .wt_amount = objConn.CheckDBNull(objItem("wt_amount"), Common.DBConnection.DBType.DBDecimal)
                        'remark() As String
                        .remark = objConn.CheckDBNull(objItem("remark"), Common.DBConnection.DBType.DBString)
                        'created_by() As Integer
                        .created_by = objConn.CheckDBNull(objItem("created_by"), Common.DBConnection.DBType.DBDecimal)
                        'created_date() As DateString
                        .created_date = objConn.CheckDBNull(objItem("created_date"), Common.DBConnection.DBType.DBString).ToString
                        'update_by() As Integer
                        .updated_by = objConn.CheckDBNull(objItem("updated_by"), Common.DBConnection.DBType.DBDecimal)
                        'update_date() As DateString
                        .updated_date = objConn.CheckDBNull(objItem("updated_date"), Common.DBConnection.DBType.DBString).ToString

                    End With
                    DB_SearchPurchase.purchase_detail.Add(objPurchaseDetail)
                Next

            Catch ex As Exception
                ' write error log
                DB_SearchPurchase = Nothing
                objLog.ErrorLog("DB_SearchPurchase(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_SearchPurchase(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_SearchPurchase
        '	Discription	    : Search purchase 
        '	Return Value	: Ilist of ImpPurchaseEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 11-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_SearchPurchase(ByVal objSearchPurchase As Dto.PurchaseSearchDto) As List(Of Entity.ImpPurchaseEntity) Implements IPo_HeaderDao.DB_SearchPurchase
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim objDT As System.Data.DataTable
                Dim objPurchase As Entity.ImpPurchaseEntity
                Dim objCom As New Common.Utilities.Utility
                Dim dateTemp As String = String.Empty

                DB_SearchPurchase = Nothing

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and assign parameter
                With strSQL
                    .AppendLine(" SELECT distinct A.id ")
                    .AppendLine(" 	,CASE ")
                    .AppendLine(" 		WHEN A.po_type = 0 ")
                    .AppendLine(" 			THEN 'Purchase' ")
                    .AppendLine(" 		WHEN A.po_type = 1 ")
                    .AppendLine(" 			THEN 'Outsource' ")
                    .AppendLine(" 		ELSE '' ")
                    .AppendLine(" 		END As po_type_text ")
                    .AppendLine(" 	,A.po_no ")
                    .AppendLine(" 	,B.name As vendor_name ")
                    .AppendLine(" 	,A.issue_date ")
                    .AppendLine(" 	,A.delivery_date ")
                    '2013/09/26 Pranitda S. Start-Add
                    .AppendLine(" 	,C.name as currency ")
                    .AppendLine(" 	,if(SYSDATE()>cast(A.delivery_date as date) and A.status_id not in (4,7),'Red','Black') as font_color ")
                    .AppendLine("   ,if(ISNULL(D.id),0,1) as delivery_fg")
                    '2013/09/26 Pranitda S. End-Add
                    .AppendLine(" 	,A.sub_total ")
                    .AppendLine(" 	,A.status_id ")
                    .AppendLine(" 	,CASE A.status_id ")
                    .AppendLine(" 		WHEN 1 THEN 'Normal' ")
                    .AppendLine(" 		WHEN 2 THEN 'Delivery' ")
                    .AppendLine(" 		WHEN 3 THEN 'Waiting' ")
                    .AppendLine(" 		WHEN 4 THEN 'Approve' ")
                    .AppendLine(" 		WHEN 5 THEN 'Decline' ")
                    .AppendLine(" 		WHEN 6 THEN 'Delete' ")
                    .AppendLine(" 		WHEN 7 THEN 'Complete' ")
                    .AppendLine(" 		ELSE '' ")
                    .AppendLine("     END AS status ")
                    .AppendLine(" FROM po_header A ")
                    .AppendLine(" JOIN mst_vendor B ON A.vendor_id = B.id ")

                    '2013/09/26 Pranitda S. Start-Add
                    .AppendLine(" join mst_currency C on A.currency_id = C.id ")
                    .AppendLine(" left join payment_header D on D.po_header_id=A.id ")
                    '2013/09/26 Pranitda S. End-Add

                    .AppendLine(" WHERE A.status_id <> " & RecordStatus.Deleted & " ")
                    .AppendLine(" 	AND ( B.type1 = 0 ) ")
                    .AppendLine(" 	AND ( B.delete_fg = 0 ) ")

                    If objSearchPurchase.type_purchase > -1 Then
                        '.AppendLine(" 	AND ( A.po_type = ?po_type OR ?po_type = - 1 ) ")
                        .AppendLine(" 	AND ( A.po_type = ?po_type ) ")
                        objConn.AddParameter("?po_type", objSearchPurchase.type_purchase)
                    End If
                    If (Not objSearchPurchase.po_no_from Is Nothing) AndAlso objSearchPurchase.po_no_from.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( A.po_no >= ?po_no_from OR ?po_no_from = '' ) ")
                        .AppendLine(" 	AND ( Left(A.po_no,15) >= ?po_no_from ) ")
                        objConn.AddParameter("?po_no_from", objSearchPurchase.po_no_from.Trim)
                    End If
                    If (Not objSearchPurchase.po_no_to Is Nothing) AndAlso objSearchPurchase.po_no_to.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( A.po_no <= ?po_no_to OR ?po_no_to = '' ) ")
                        .AppendLine(" 	AND ( Left(A.po_no,15) <= ?po_no_to ) ")
                        objConn.AddParameter("?po_no_to", objSearchPurchase.po_no_to.Trim)
                    End If
                    If (Not objSearchPurchase.issue_date_from Is Nothing) AndAlso objSearchPurchase.issue_date_from.ToString.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( A.issue_date >= ?issue_date_from OR ?issue_date_from = '' ) ")
                        .AppendLine(" 	AND ( A.issue_date >= ?issue_date_from ) ")
                        dateTemp = objCom.String2Date(objSearchPurchase.issue_date_from.ToString).ToString("yyyyMMdd")
                        objConn.AddParameter("?issue_date_from", dateTemp)
                    End If
                    If (Not objSearchPurchase.issue_date_to Is Nothing) AndAlso objSearchPurchase.issue_date_to.ToString.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( A.issue_date <= ?issue_date_to OR ?issue_date_to = '' ) ")
                        .AppendLine(" 	AND ( A.issue_date <= ?issue_date_to ) ")
                        dateTemp = objCom.String2Date(objSearchPurchase.issue_date_to.ToString).ToString("yyyyMMdd")
                        objConn.AddParameter("?issue_date_to", dateTemp)
                    End If
                    If (Not objSearchPurchase.delivery_date_from Is Nothing) AndAlso objSearchPurchase.delivery_date_from.ToString.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( A.delivery_date >= ?delivery_date_from OR ?delivery_date_from = '' ) ")
                        .AppendLine(" 	AND ( A.delivery_date >= ?delivery_date_from ) ")
                        dateTemp = objCom.String2Date(objSearchPurchase.delivery_date_from.ToString).ToString("yyyyMMdd")
                        objConn.AddParameter("?delivery_date_from", dateTemp)
                    End If
                    If (Not objSearchPurchase.delivery_date_to Is Nothing) AndAlso objSearchPurchase.delivery_date_to.ToString.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( A.delivery_date <= ?delivery_date_to OR ?delivery_date_to = '' ) ")
                        .AppendLine(" 	AND ( A.delivery_date <= ?delivery_date_to ) ")
                        dateTemp = objCom.String2Date(objSearchPurchase.delivery_date_to.ToString).ToString("yyyyMMdd")
                        objConn.AddParameter("?delivery_date_to", dateTemp)
                    End If
                    If (Not objSearchPurchase.vendor_name Is Nothing) AndAlso objSearchPurchase.vendor_name.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( B.name LIKE '%?name%' OR ?name = '' ) ")
                        'LIKE CONCAT('%', @vendor_name, '%')
                        '.AppendLine(" 	AND ( B.name LIKE '%" & objSearchPurchase.vendor_name & "%' ) ")
                        .AppendLine(" 	AND ( B.name LIKE CONCAT('%', ?vendor_name, '%') ) ")
                        objConn.AddParameter("?vendor_name", objSearchPurchase.vendor_name)
                    End If
                    .AppendLine(" ORDER BY id DESC; ")
                End With

                ' execute by datatable
                objDT = New System.Data.DataTable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_SearchPurchase = New List(Of Entity.ImpPurchaseEntity)
                For Each objItem In objDT.Rows
                    objPurchase = New Entity.ImpPurchaseEntity

                    With objPurchase
                        .id = objConn.CheckDBNull(objItem("id"), Common.DBConnection.DBType.DBDecimal)
                        .po_type_text = objConn.CheckDBNull(objItem("po_type_text"), Common.DBConnection.DBType.DBString)
                        .po_no = objConn.CheckDBNull(objItem("po_no"), Common.DBConnection.DBType.DBString)
                        .vendor_name = objConn.CheckDBNull(objItem("vendor_name"), Common.DBConnection.DBType.DBString)
                        .issue_date = objConn.CheckDBNull(objItem("issue_date"), Common.DBConnection.DBType.DBString).ToString
                        .delivery_date = objConn.CheckDBNull(objItem("delivery_date"), Common.DBConnection.DBType.DBString).ToString
                        .sub_total = objConn.CheckDBNull(objItem("sub_total"), Common.DBConnection.DBType.DBDecimal)
                        .status_id = objConn.CheckDBNull(objItem("status_id"), Common.DBConnection.DBType.DBDecimal)
                        .status = objConn.CheckDBNull(objItem("status"), Common.DBConnection.DBType.DBString)
                        '2013/09/26 Pranitda S. Start-Add
                        .currency = objConn.CheckDBNull(objItem("currency"), Common.DBConnection.DBType.DBString)
                        .font_color = objConn.CheckDBNull(objItem("font_color"), Common.DBConnection.DBType.DBString)
                        .delivery_fg = objConn.CheckDBNull(objItem("delivery_fg"), Common.DBConnection.DBType.DBDecimal)
                        '2013/09/26 Pranitda S. End-Add
                    End With
                    DB_SearchPurchase.Add(objPurchase)
                Next

            Catch ex As Exception
                ' write error log
                DB_SearchPurchase = Nothing
                objLog.ErrorLog("DB_SearchPurchase(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_SearchPurchase(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_DeletePurchase
        '	Discription	    : Delete data purchase 
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 11-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_DeletePurchase(ByVal intPurchaseId As Integer) As Boolean Implements IPo_HeaderDao.DB_DeletePurchase
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlag As Integer = 0

                DB_DeletePurchase = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess

                ' open begin transaction
                objConn.BeginTrans()

                '' assign sql statement
                'strSQL = New Text.StringBuilder
                'With strSQL
                '    .AppendLine(" DELETE ")
                '    .AppendLine(" FROM po_detail ")
                '    .AppendLine(" WHERE po_header_id = ?purchase_id; ")
                '    ' assign parameter
                '    objConn.AddParameter("?purchase_id", intPurchaseId)
                'End With
                '' execute by nonquery
                'intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                'strMsgErr = objConn.MessageError
                '' check data
                'If intFlag = -1 Then
                '    objConn.RollbackTrans()
                '    Exit Function
                'End If

                ' assign sql statement
                strSQL = New Text.StringBuilder
                With strSQL
                    '.AppendLine(" DELETE ")
                    '.AppendLine(" FROM po_header ")
                    '.AppendLine(" WHERE id = ?purchase_id; ")

                    .AppendLine("UPDATE po_header")
                    .AppendLine("SET status_id = " & RecordStatus.Deleted)
                    .AppendLine(",updated_by=?loginid ")
                    .AppendLine(",updated_date=date_format(now(),'%Y%m%d%H%i%s') ")
                    .AppendLine("WHERE id = ?purchase_id;")
                    ' assign parameter
                    objConn.AddParameter("?loginid", HttpContext.Current.Session("UserID"))
                    objConn.AddParameter("?purchase_id", intPurchaseId)
                End With
                ' execute by nonquery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag = -1 Then
                    objConn.RollbackTrans()
                Else
                    objConn.CommitTrans()
                    DB_DeletePurchase = True
                End If

            Catch ex As Exception
                ' write error log
                DB_DeletePurchase = False
                objLog.ErrorLog("DB_DeletePurchase(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_DeletePurchase(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_SearchPurchaseDetail
        '	Discription	    : Search data purchase and detail
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 13-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_SearchPurchaseDetail(ByVal intPurchaseId As Integer) As Entity.ImpPurchaseEntity Implements IPo_HeaderDao.DB_SearchPurchaseDetail
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim objDT As System.Data.DataTable
                Dim objItem As System.Data.DataRow
                Dim objPurchaseDetail As Entity.ImpPurchaseDetailEntity

                DB_SearchPurchaseDetail = Nothing

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and assign parameter
                With strSQL
                    .AppendLine(" SELECT CASE po_type ")
                    .AppendLine(" 		WHEN 0 ")
                    .AppendLine(" 			THEN 'Purchase' ")
                    .AppendLine(" 		WHEN 1 ")
                    .AppendLine(" 			THEN 'Outsource' ")
                    .AppendLine(" 		ELSE '' ")
                    .AppendLine(" 		END AS po_type_text ")
                    .AppendLine(" 	,A.po_type ")
                    .AppendLine(" 	,A.po_no ")
                    .AppendLine(" 	,A.delivery_date ")
                    .AppendLine(" 	,A.quotation_no ")
                    .AppendLine(" 	,A.vendor_id ")
                    .AppendLine(" 	,B.VendorName As vendor_name ")
                    .AppendLine(" 	,A.payment_term_id ")
                    .AppendLine(" 	,CONCAT(C.term_day,' day(s)' ")
                    .AppendLine(" 		) AS payment_term_text ")
                    .AppendLine(" 	,A.vat_id ")
                    .AppendLine(" 	,CONCAT(D.PERCENT,'%' ")
                    .AppendLine(" 		) AS vat_text ")
                    .AppendLine(" 	,A.wt_id ")
                    .AppendLine(" 	,CONCAT(E.PERCENT,'%' ")
                    .AppendLine(" 		) AS wt_text ")
                    .AppendLine(" 	,A.currency_id ")
                    .AppendLine(" 	,F.Currency AS currency_name ")
                    .AppendLine(" 	,A.remark ")
                    .AppendLine(" 	,A.attn ")
                    .AppendLine(" 	,A.deliver_to ")
                    .AppendLine(" 	,A.contact ")
                    .AppendLine(" 	,A.sub_total ")
                    .AppendLine(" 	,A.discount_total ")
                    .AppendLine(" 	,A.vat_amount ")
                    .AppendLine(" 	,A.wt_amount ")
                    .AppendLine(" 	,A.total_amount ")
                    .AppendLine(" FROM po_header A ")
                    .AppendLine(" LEFT JOIN ( ")
                    .AppendLine(" 	SELECT * ")
                    .AppendLine(" 		,NAME AS VendorName ")
                    .AppendLine(" 	FROM mst_vendor ")
                    .AppendLine(" 	WHERE ( type1 = 0 ) ")
                    .AppendLine(" 	AND ( delete_fg = 0 ) ")
                    .AppendLine(" 	) B ON A.vendor_id = B.id ")
                    .AppendLine(" LEFT JOIN mst_payment_term C ON A.payment_term_id = C.id ")
                    .AppendLine(" LEFT JOIN mst_vat D ON A.vat_id = D.id ")
                    .AppendLine(" LEFT JOIN mst_wt E ON A.wt_id = E.id ")
                    .AppendLine(" LEFT JOIN ( ")
                    .AppendLine(" 	SELECT * ")
                    .AppendLine(" 		,NAME AS Currency ")
                    .AppendLine(" 	FROM mst_currency ")
                    .AppendLine(" 	WHERE ( delete_fg = 0 ) ")
                    .AppendLine(" 	) F ON A.currency_id = F.id ")
                    .AppendLine(" WHERE A.id = ?purchase_id ")
                    .AppendLine("   AND C.delete_fg = 0 ")
                    .AppendLine("   AND (D.delete_fg = 0 or D.id=1) ")
                    .AppendLine("   AND (E.delete_fg = 0 or E.id=1) ")
                    .AppendLine("   AND A.status_id <> 6 ")
                    ' assign parameter
                    objConn.AddParameter("?purchase_id", intPurchaseId)

                End With

                ' execute by datatable
                objDT = New System.Data.DataTable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_SearchPurchaseDetail = New Entity.ImpPurchaseEntity
                ' assign data ot object entity
                objItem = objDT.Rows(0)
                With DB_SearchPurchaseDetail
                    'po_type
                    .po_type = objConn.CheckDBNull(objItem("po_type"), Common.DBConnection.DBType.DBDecimal)
                    .po_type_text = objConn.CheckDBNull(objItem("po_type_text"), Common.DBConnection.DBType.DBString)
                    'po_no
                    .po_no = objConn.CheckDBNull(objItem("po_no"), Common.DBConnection.DBType.DBString)
                    'delivery_date
                    .delivery_date = objConn.CheckDBNull(objItem("delivery_date"), Common.DBConnection.DBType.DBString).ToString
                    'quotation_no
                    .quotation_no = objConn.CheckDBNull(objItem("quotation_no"), Common.DBConnection.DBType.DBString)
                    'vendor_id
                    .vendor_id = objConn.CheckDBNull(objItem("vendor_id"), Common.DBConnection.DBType.DBDecimal)
                    .vendor_name = objConn.CheckDBNull(objItem("vendor_name"), Common.DBConnection.DBType.DBString)
                    'payment_term_id
                    .payment_term_id = objConn.CheckDBNull(objItem("payment_term_id"), Common.DBConnection.DBType.DBDecimal)
                    .payment_term_text = objConn.CheckDBNull(objItem("payment_term_text"), Common.DBConnection.DBType.DBString)
                    'vat_id
                    .vat_id = objConn.CheckDBNull(objItem("vat_id"), Common.DBConnection.DBType.DBDecimal)
                    .vat_text = objConn.CheckDBNull(objItem("vat_text"), Common.DBConnection.DBType.DBString)
                    'wt_id
                    .wt_id = objConn.CheckDBNull(objItem("wt_id"), Common.DBConnection.DBType.DBDecimal)
                    .wt_text = objConn.CheckDBNull(objItem("wt_text"), Common.DBConnection.DBType.DBString)
                    'currency_id
                    .currency_id = objConn.CheckDBNull(objItem("currency_id"), Common.DBConnection.DBType.DBDecimal)
                    .currency_name = objConn.CheckDBNull(objItem("currency_name"), Common.DBConnection.DBType.DBString)
                    'remark
                    .remark = objConn.CheckDBNull(objItem("remark"), Common.DBConnection.DBType.DBString)
                    'attn
                    .attn = objConn.CheckDBNull(objItem("attn"), Common.DBConnection.DBType.DBString)
                    'deliver_to
                    .deliver_to = objConn.CheckDBNull(objItem("deliver_to"), Common.DBConnection.DBType.DBString)
                    'contact
                    .contact = objConn.CheckDBNull(objItem("contact"), Common.DBConnection.DBType.DBString)

                    'sub_total
                    .sub_total = objConn.CheckDBNull(objItem("sub_total"), Common.DBConnection.DBType.DBDecimal)
                    'discount_total
                    .discount_total = objConn.CheckDBNull(objItem("discount_total"), Common.DBConnection.DBType.DBDecimal)
                    'vat_amount
                    .vat_amount = objConn.CheckDBNull(objItem("vat_amount"), Common.DBConnection.DBType.DBDecimal)
                    'wt_amount
                    .wt_amount = objConn.CheckDBNull(objItem("wt_amount"), Common.DBConnection.DBType.DBDecimal)
                    'total_amount
                    .total_amount = objConn.CheckDBNull(objItem("total_amount"), Common.DBConnection.DBType.DBDecimal)
                End With

                ' set new object (for get purchase_detail)
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and assign parameter
                With strSQL
                    .AppendLine(" SELECT B.iTemName As item_name ")
                    .AppendLine(" 	,A.item_id ")
                    .AppendLine(" 	,A.job_order ")
                    .AppendLine(" 	,A.ie_id ")
                    .AppendLine(" 	,C.AccountTitle As ie_name ")
                    .AppendLine(" 	,A.unit_price ")
                    .AppendLine(" 	,A.quantity ")
                    .AppendLine(" 	,A.unit_id ")
                    .AppendLine(" 	,D.UName As unit_name ")
                    .AppendLine(" 	,A.discount ")
                    .AppendLine(" 	,A.discount_type ")
                    .AppendLine(" 	,CASE discount_type ")
                    .AppendLine(" 		WHEN 0 ")
                    .AppendLine(" 			THEN 'No Discount' ")
                    .AppendLine(" 		WHEN 1 ")
                    .AppendLine(" 			THEN F.Currency ")
                    .AppendLine(" 		WHEN 2 ")
                    .AppendLine(" 			THEN 'Percent(%)' ")
                    .AppendLine(" 		ELSE '' ")
                    .AppendLine(" 		END AS discount_type_text ")
                    .AppendLine(" 	,A.vat_amount ")
                    .AppendLine(" 	,A.wt_amount ")
                    .AppendLine(" 	,A.amount ")
                    .AppendLine(" 	,A.remark ")
                    .AppendLine(" FROM po_detail A ")
                    .AppendLine(" JOIN po_header E ON A.po_header_id = E.id ")
                    .AppendLine(" LEFT JOIN ( ")
                    .AppendLine(" 	SELECT * ")
                    .AppendLine(" 		,NAME AS iTemName ")
                    .AppendLine(" 	FROM mst_item ")
                    .AppendLine(" 	WHERE delete_fg = 0 ")
                    .AppendLine(" 	) B ON A.item_id = B.id ")
                    .AppendLine(" LEFT JOIN ( ")
                    .AppendLine(" 	SELECT * ")
                    .AppendLine(" 		,NAME AS AccountTitle ")
                    .AppendLine(" 	FROM mst_ie ")
                    .AppendLine(" 	WHERE delete_fg = 0 ")
                    .AppendLine(" 	) C ON A.ie_id = C.id ")
                    .AppendLine(" LEFT JOIN ( ")
                    .AppendLine(" 	SELECT * ")
                    .AppendLine(" 		,NAME AS UName ")
                    .AppendLine(" 	FROM mst_unit ")
                    .AppendLine(" 	WHERE delete_fg = 0 ")
                    .AppendLine(" 	) D ON A.unit_id = D.id ")
                    .AppendLine(" LEFT JOIN ( ")
                    .AppendLine(" 	SELECT * ")
                    .AppendLine(" 		,NAME AS Currency ")
                    .AppendLine(" 	FROM mst_currency ")
                    .AppendLine(" 	WHERE delete_fg = 0 ")
                    .AppendLine(" 	) F ON E.currency_id = F.id ")
                    .AppendLine(" WHERE A.po_header_id = ?purchase_id ")
                    .AppendLine(" 	AND E.status_id <> 6 ")
                    .AppendLine(" ORDER BY A.id; ")

                    ' assign parameter
                    objConn.AddParameter("?purchase_id", intPurchaseId)

                End With

                ' execute by datatable
                objDT = New System.Data.DataTable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_SearchPurchaseDetail.purchase_detail = New List(Of Entity.ImpPurchaseDetailEntity)
                ' assign data ot object entity
                For Each objItem In objDT.Rows
                    objPurchaseDetail = New Entity.ImpPurchaseDetailEntity
                    With objPurchaseDetail
                        'item_id
                        .item_id = objConn.CheckDBNull(objItem("item_id"), Common.DBConnection.DBType.DBDecimal)
                        .item_name = objConn.CheckDBNull(objItem("item_name"), Common.DBConnection.DBType.DBString)
                        'job_order
                        .job_order = objConn.CheckDBNull(objItem("job_order"), Common.DBConnection.DBType.DBString)
                        'ie_id
                        .ie_id = objConn.CheckDBNull(objItem("ie_id"), Common.DBConnection.DBType.DBDecimal)
                        .ie_name = objConn.CheckDBNull(objItem("ie_name"), Common.DBConnection.DBType.DBString)
                        'unit_price
                        .unit_price = objConn.CheckDBNull(objItem("unit_price"), Common.DBConnection.DBType.DBDecimal)
                        'quantity
                        .quantity = objConn.CheckDBNull(objItem("quantity"), Common.DBConnection.DBType.DBDecimal)
                        'unit_id
                        .unit_id = objConn.CheckDBNull(objItem("unit_id"), Common.DBConnection.DBType.DBDecimal)
                        .unit_name = objConn.CheckDBNull(objItem("unit_name"), Common.DBConnection.DBType.DBString)
                        'discount
                        .discount = objConn.CheckDBNull(objItem("discount"), Common.DBConnection.DBType.DBDecimal)
                        'discount_type
                        .discount_type = objConn.CheckDBNull(objItem("discount_type"), Common.DBConnection.DBType.DBDecimal)
                        .discount_type_text = objConn.CheckDBNull(objItem("discount_type_text"), Common.DBConnection.DBType.DBString)
                        'vat_amount
                        .vat_amount = objConn.CheckDBNull(objItem("vat_amount"), Common.DBConnection.DBType.DBDecimal)
                        'wt_amount
                        .wt_amount = objConn.CheckDBNull(objItem("wt_amount"), Common.DBConnection.DBType.DBDecimal)
                        'amount
                        .amount = objConn.CheckDBNull(objItem("amount"), Common.DBConnection.DBType.DBDecimal)
                        'remark
                        .remark = objConn.CheckDBNull(objItem("remark"), Common.DBConnection.DBType.DBString)
                    End With
                    DB_SearchPurchaseDetail.purchase_detail.Add(objPurchaseDetail)
                Next

            Catch ex As Exception
                ' write error log
                DB_SearchPurchaseDetail = Nothing
                objLog.ErrorLog("DB_SearchPurchaseDetail(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_SearchPurchaseDetail(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_SearchPurchaseReport
        '	Discription	    : Search data purchase and detail to report excel
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_SearchPurchaseReport(ByVal objSearchPurchase As Dto.PurchaseSearchDto) As System.Collections.Generic.List(Of Entity.ImpPurchaseReportEntity) Implements IPo_HeaderDao.DB_SearchPurchaseReport
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim objDT As System.Data.DataTable
                Dim objPurchase As Entity.ImpPurchaseReportEntity
                Dim objCom As New Common.Utilities.Utility
                Dim dateTemp As String = String.Empty

                DB_SearchPurchaseReport = Nothing

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and assign parameter
                With strSQL
                    .AppendLine(" SELECT E.issue_date ")
                    .AppendLine(" 	,E.po_no ")
                    .AppendLine(" 	,B.name As vendor_name ")
                    .AppendLine(" 	,CASE po_type ")
                    .AppendLine(" 		WHEN 0 ")
                    .AppendLine(" 			THEN 'Purchase' ")
                    .AppendLine(" 		WHEN 1 ")
                    .AppendLine(" 			THEN 'Outsource' ")
                    .AppendLine(" 		ELSE '' ")
                    .AppendLine(" 		END AS po_type_text ")
                    .AppendLine(" 	,C.name As ie_name ")
                    .AppendLine(" 	,D.name As item_name ")
                    .AppendLine(" 	,A.quantity ")
                    .AppendLine(" 	,F.name As unit_name ")
                    .AppendLine(" 	,A.unit_price * ifnull(CASE upper(H.NAME) ")
                    .AppendLine(" 			WHEN 'THB' ")
                    .AppendLine(" 				THEN 1 ")
                    .AppendLine(" 			WHEN 'JPY' ")
                    .AppendLine(" 				THEN G.rate / 1 ")
                    .AppendLine(" 			ELSE G.rate ")
                    .AppendLine(" 			END, 1) unit_price ")
                    '.AppendLine(" 	,E.discount_total * ifnull(CASE upper(H.NAME) ")
                    .AppendLine(" 	,(case when discount_type=2 then A.quantity*A.unit_price*A.discount/100 else ifnull(A.discount,0) end) * ifnull(CASE upper(H.NAME) ")
                    .AppendLine(" 			WHEN 'THB' ")
                    .AppendLine(" 				THEN 1 ")
                    .AppendLine(" 			WHEN 'JPY' ")
                    .AppendLine("  				THEN G.rate / 1 ")
                    .AppendLine(" 			ELSE G.rate ")
                    .AppendLine(" 			END, 1) discount_amt ")
                    .AppendLine(" 	,A.amount * ifnull(CASE upper(H.NAME) ")
                    .AppendLine(" 			WHEN 'THB' ")
                    .AppendLine(" 				THEN 1 ")
                    .AppendLine(" 			WHEN 'JPY' ")
                    .AppendLine(" 				THEN G.rate / 1 ")
                    .AppendLine(" 			ELSE G.rate ")
                    .AppendLine(" 			END, 1) amount ")
                    .AppendLine(" 	,A.vat_amount * ifnull(CASE upper(H.NAME) ")
                    .AppendLine(" 			WHEN 'THB' ")
                    .AppendLine(" 				THEN 1 ")
                    .AppendLine(" 			WHEN 'JPY' ")
                    .AppendLine(" 				THEN G.rate / 1 ")
                    .AppendLine(" 			ELSE G.rate ")
                    .AppendLine(" 			END, 1) vat_amount ")

                    'start add by Pranitda S. 2013/09/20
                    .AppendLine(" 			,A.job_order ")
                    .AppendLine(" 			,J.name ")
                    .AppendLine(" 			,E.delivery_date ")
                    .AppendLine(" 			,CAST(L.delivery_date AS DATE) AS delivery_plan ")
                    .AppendLine(" 			,L.invoice_no ")
                    .AppendLine(" 			,L.remark ")
                    .AppendLine(" 			,if(M.quality=50,M.quality,null) quality50 ")
                    .AppendLine(" 			,IF(M.quality=25,M.quality,null) quality25 ")
                    .AppendLine(" 			,IF(M.quality=0,M.quality,null) quality0 ")
                    .AppendLine(" 			,if(M.delivery=30,M.delivery,null) delivery30 ")
                    .AppendLine(" 			,if(M.delivery=15,M.delivery,null) delivery15 ")
                    .AppendLine(" 			,if(M.delivery=0,M.delivery,null) delivery0 ")
                    .AppendLine(" 			,if(M.service=20,M.service,null) service20 ")
                    .AppendLine(" 			,if(M.service=10,M.service,null) service10 ")
                    .AppendLine(" 			,if(M.service=0,M.service,null) service0 ")
                    .AppendLine(" 			,N.Score ")
                    .AppendLine(" 			,N.Grade ")
                    'end add by Pranitda S. 2013/09/20

                    .AppendLine(" FROM po_detail A ")
                    .AppendLine(" JOIN po_header E ON E.id = A.po_header_id ")
                    .AppendLine(" LEFT JOIN mst_vendor B ON E.vendor_id = B.id ")
                    .AppendLine(" LEFT JOIN mst_ie C ON A.ie_id = C.id ")
                    .AppendLine(" LEFT JOIN mst_item D ON A.item_id = D.id ")
                    .AppendLine(" LEFT JOIN mst_unit F ON A.unit_id = F.id ")

                    .AppendLine(" LEFT JOIN mst_currency H ON E.currency_id = H.id ")
                    .AppendLine(" LEFT JOIN ( ")

                    '.AppendLine(" 	SELECT currency_id ")
                    '.AppendLine(" 		,ef_date ")
                    '.AppendLine(" 		,rate ")
                    '.AppendLine(" 	FROM mst_schedule_rate ")
                    '.AppendLine(" 	WHERE delete_fg = 0 ")
                    '.AppendLine(" 	ORDER BY ef_date DESC limit 1 ")
                    '.AppendLine(" 	) G ON G.currency_id = E.currency_id ")
                    '.AppendLine(" 	AND G.ef_date <= E.issue_date ")

                    .AppendLine(" 	SELECT Z.currency_id ")
                    .AppendLine(" 		,Z.issue_date ")
                    .AppendLine(" 		,Y.ef_date ")
                    .AppendLine(" 		,Y.rate ")
                    .AppendLine(" 	FROM ( ")
                    .AppendLine(" 		SELECT A.currency_id ")
                    .AppendLine(" 			,A.issue_date ")
                    .AppendLine(" 			,max(B.ef_date) max_ef_date ")
                    .AppendLine(" 		FROM po_header A ")
                    .AppendLine(" 		LEFT JOIN mst_schedule_rate B ON B.currency_id = A.currency_id ")
                    .AppendLine(" 			AND B.ef_date <= A.issue_date ")
                    .AppendLine(" 		WHERE B.delete_fg = 0 ")
                    .AppendLine(" 			AND A.status_id <> 6 ")
                    .AppendLine(" 		GROUP BY A.currency_id ")
                    .AppendLine(" 			,A.issue_date ")
                    .AppendLine(" 		) Z ")
                    .AppendLine(" 	LEFT JOIN mst_schedule_rate Y ON Y.currency_id = Z.currency_id ")
                    .AppendLine(" 		AND Y.ef_date = Z.max_ef_date ")
                    .AppendLine(" 		) G ON G.currency_id = E.currency_id ")
                    .AppendLine(" 	AND G.issue_date = E.issue_date ")

                    'start add by Pranitda S. 2013/09/20
                    .AppendLine(" 	LEFT JOIN job_order I on I.job_order=A.job_order ")
                    .AppendLine(" 	LEFT Join mst_vendor J on J.id=I.customer ")
                    .AppendLine(" 	join payment_detail K on K.po_header_id=A.po_header_id and K.po_detail_id=A.id ")
                    .AppendLine(" 	join payment_header L on K.payment_header_id=L.id ")
                    .AppendLine(" 	left join vendor_rating M on M.payment_header_id=L.id ")
                    .AppendLine(" 	left join ( ")
                    .AppendLine(" 	select B.po_header_id,avg(quality)+avg(delivery)+avg(service) Score,case when avg(quality)+avg(delivery)+avg(service)>79 then 'A' when avg(quality)+avg(delivery)+avg(service)>69 then 'B' when avg(quality)+avg(delivery)+avg(service)>59 then 'C' else 'D' end Grade ")
                    .AppendLine(" 	from vendor_rating A join payment_header B on A.payment_header_id=B.id ")
                    .AppendLine(" 	group by B.po_header_id ")
                    .AppendLine(" 	) N on N.po_header_id=E.id ")
                    'end add by Pranitda S. 2013/09/20

                    .AppendLine(" WHERE E.status_id <> " & RecordStatus.Deleted)
                    .AppendLine(" 	AND H.delete_fg = 0 and L.status_id<>6 ")

                    If objSearchPurchase.type_purchase > -1 Then
                        '.AppendLine(" 	AND ( po_type = ?po_type OR ?po_type IS NULL ) ")
                        .AppendLine(" 	AND ( E.po_type = ?po_type ) ")
                        objConn.AddParameter("?po_type", objSearchPurchase.type_purchase)
                    End If
                    If (Not objSearchPurchase.po_no_from Is Nothing) AndAlso objSearchPurchase.po_no_from.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( po_no >= ?po_no OR ?po_no = '' ) ")
                        .AppendLine(" 	AND ( Left(E.po_no,15) >= ?po_no_from ) ")
                        objConn.AddParameter("?po_no_from", objSearchPurchase.po_no_from.Trim)
                    End If
                    If (Not objSearchPurchase.po_no_to Is Nothing) AndAlso objSearchPurchase.po_no_to.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( po_no <= ?po_no OR ?po_no = '' ) ")
                        .AppendLine(" 	AND ( Left(E.po_no,15) <= ?po_no_to ) ")
                        objConn.AddParameter("?po_no_to", objSearchPurchase.po_no_to.Trim)
                    End If
                    If (Not objSearchPurchase.issue_date_from Is Nothing) AndAlso objSearchPurchase.issue_date_from.ToString.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( issue_date >= ?issue_date OR ?issue_date = '' ) ")
                        .AppendLine(" 	AND ( E.issue_date >= ?issue_date_from ) ")
                        dateTemp = objCom.String2Date(objSearchPurchase.issue_date_from.ToString).ToString("yyyyMMdd")
                        objConn.AddParameter("?issue_date_from", dateTemp)
                    End If
                    If (Not objSearchPurchase.issue_date_to Is Nothing) AndAlso objSearchPurchase.issue_date_to.ToString.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( issue_date <= ?issue_date OR ?issue_date = '' ) ")
                        .AppendLine(" 	AND ( E.issue_date <= ?issue_date_to ) ")
                        dateTemp = objCom.String2Date(objSearchPurchase.issue_date_to.ToString).ToString("yyyyMMdd")
                        objConn.AddParameter("?issue_date_to", dateTemp)
                    End If
                    If (Not objSearchPurchase.delivery_date_from Is Nothing) AndAlso objSearchPurchase.delivery_date_from.ToString.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( delivery_date >= ?delivery_date OR ?delivery_date = '' ) ")
                        .AppendLine(" 	AND ( E.delivery_date >= ?delivery_date_from ) ")
                        dateTemp = objCom.String2Date(objSearchPurchase.delivery_date_from.ToString).ToString("yyyyMMdd")
                        objConn.AddParameter("?delivery_date_from", dateTemp)
                    End If
                    If (Not objSearchPurchase.delivery_date_to Is Nothing) AndAlso objSearchPurchase.delivery_date_to.ToString.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( delivery_date <= ?delivery_date OR ?delivery_date = '' ) ")
                        .AppendLine(" 	AND ( E.delivery_date <= ?delivery_date_to ) ")
                        dateTemp = objCom.String2Date(objSearchPurchase.delivery_date_to.ToString).ToString("yyyyMMdd")
                        objConn.AddParameter("?delivery_date_to", dateTemp)
                    End If
                    If (Not objSearchPurchase.vendor_name Is Nothing) AndAlso objSearchPurchase.vendor_name.Trim <> String.Empty Then
                        '.AppendLine(" 	AND ( B.NAME LIKE '%?B.name%' OR ?B.name = '' ) ")
                        .AppendLine(" 	AND ( B.name LIKE '%" & objSearchPurchase.vendor_name & "%' ) ")
                    End If
                    .AppendLine(" ORDER BY E.id desc, A.id ")
                End With

                ' execute by datatable
                objDT = New System.Data.DataTable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_SearchPurchaseReport = New List(Of Entity.ImpPurchaseReportEntity)
                For Each objItem In objDT.Rows
                    objPurchase = New Entity.ImpPurchaseReportEntity
                    With objPurchase
                        'issue_date     'PO Date
                        'po_no          'PO No.
                        'vendor_name    'Supplier Name
                        'po_type_text   'Group
                        'ie_name        'Type
                        'item_name      'Description
                        'quantity       'Quantity
                        'unit_name      'Unit
                        'unit_price     'Unit Price
                        'discount_amt   'Discount
                        'amount         'Amount
                        'vat_amount     'VAT

                        .issue_date = objConn.CheckDBNull(objItem("issue_date"), Common.DBConnection.DBType.DBString).ToString
                        .po_no = objConn.CheckDBNull(objItem("po_no"), Common.DBConnection.DBType.DBString)
                        .vendor_name = objConn.CheckDBNull(objItem("vendor_name"), Common.DBConnection.DBType.DBString)
                        .po_type_text = objConn.CheckDBNull(objItem("po_type_text"), Common.DBConnection.DBType.DBString)
                        .ie_name = objConn.CheckDBNull(objItem("ie_name"), Common.DBConnection.DBType.DBString)
                        .item_name = objConn.CheckDBNull(objItem("item_name"), Common.DBConnection.DBType.DBString)
                        .quantity = objConn.CheckDBNull(objItem("quantity"), Common.DBConnection.DBType.DBDecimal)
                        .unit_name = objConn.CheckDBNull(objItem("unit_name"), Common.DBConnection.DBType.DBString)
                        .unit_price = objConn.CheckDBNull(objItem("unit_price"), Common.DBConnection.DBType.DBDecimal)
                        .discount_amt = objConn.CheckDBNull(objItem("discount_amt"), Common.DBConnection.DBType.DBDecimal)
                        .amount = objConn.CheckDBNull(objItem("amount"), Common.DBConnection.DBType.DBDecimal)
                        .vat_amount = objConn.CheckDBNull(objItem("vat_amount"), Common.DBConnection.DBType.DBDecimal)

                        'start add by Pranitda S. 2013/09/20
                        .job_order = objConn.CheckDBNull(objItem("job_order"), Common.DBConnection.DBType.DBString)
                        .name = objConn.CheckDBNull(objItem("name"), Common.DBConnection.DBType.DBString)
                        .delivery_date = objConn.CheckDBNull(objItem("delivery_date"), Common.DBConnection.DBType.DBString).ToString
                        .delivery_plan = objConn.CheckDBNull(objItem("delivery_plan"), Common.DBConnection.DBType.DBString).ToString
                        .invoice_no = objConn.CheckDBNull(objItem("invoice_no"), Common.DBConnection.DBType.DBString)
                        .remark = objConn.CheckDBNull(objItem("remark"), Common.DBConnection.DBType.DBString)
                        .quality50 = objConn.CheckDBNull(objItem("quality50"), Common.DBConnection.DBType.DBString)
                        .quality25 = objConn.CheckDBNull(objItem("quality25"), Common.DBConnection.DBType.DBString)
                        .quality0 = objConn.CheckDBNull(objItem("quality0"), Common.DBConnection.DBType.DBString)
                        .delivery30 = objConn.CheckDBNull(objItem("delivery30"), Common.DBConnection.DBType.DBString)
                        .delivery15 = objConn.CheckDBNull(objItem("delivery15"), Common.DBConnection.DBType.DBString)
                        .delivery0 = objConn.CheckDBNull(objItem("delivery0"), Common.DBConnection.DBType.DBString)
                        .service20 = objConn.CheckDBNull(objItem("service20"), Common.DBConnection.DBType.DBString)
                        .service10 = objConn.CheckDBNull(objItem("service10"), Common.DBConnection.DBType.DBString)
                        .service0 = objConn.CheckDBNull(objItem("service0"), Common.DBConnection.DBType.DBString)
                        .Score = objConn.CheckDBNull(objItem("Score"), Common.DBConnection.DBType.DBString)
                        .Grade = objConn.CheckDBNull(objItem("Grade"), Common.DBConnection.DBType.DBString)
                        'end add by Pranitda S. 2013/09/20
                    End With
                    DB_SearchPurchaseReport.Add(objPurchase)
                Next

            Catch ex As Exception
                ' write error log
                DB_SearchPurchaseReport = Nothing
                objLog.ErrorLog("DB_SearchPurchaseReport(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_SearchPurchaseReport(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_SearchPurchasePDF
        '	Discription	    : Search data purchase and detail to report pdf
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_SearchPurchasePDF(ByVal intPurchaseId As Integer) As Entity.ImpPurchasePDFEntty Implements IPo_HeaderDao.DB_SearchPurchasePDF
            Dim strSQL As New Text.StringBuilder
            Dim objCom As New Common.Utilities.Utility
            Try
                ' variable
                Dim objDT As System.Data.DataTable
                Dim objItem As System.Data.DataRow
                Dim objPurchaseDetail As Entity.ImpPurchaseDetailEntity
                Dim intNO As Integer = 0

                DB_SearchPurchasePDF = Nothing

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and assign parameter
                With strSQL
                    .AppendLine(" SELECT @ROWNUM div 8 as grp, @ROWNUM:=@ROWNUM+1 AS ID ")
                    .AppendLine(" 	,if(IFNULL(A.vendor_branch_id,0)=0,concat(B.name,' (',if(left(hex(left(B.name,1)),2)='E0','สำนักงานใหญ่','Head office'),')'),concat(B.name,' (',I.name,')')) vendor_name ")
                    .AppendLine(" 	,if(IFNULL(A.vendor_branch_id,0)=0,B.address,I.address) address ")
                    .AppendLine(" 	,if(IFNULL(A.vendor_branch_id,0)=0,B.zipcode,I.zipcode) zipcode ")
                    .AppendLine(" 	,CONCAT('Tel : ',if(IFNULL(A.vendor_branch_id,0)=0,B.tel,I.tel)) tel ")
                    .AppendLine(" 	,CONCAT('Fax : ',if(IFNULL(A.vendor_branch_id,0)=0,B.fax,I.fax)) fax ")
                    .AppendLine(" 	,CONCAT('Attn : ',A.attn) attn ")
                    .AppendLine(" 	,A.po_no,case A.payment_term_id when 1 then 'Cash' when 2 then 'Cheque' when 3 then 'By (                    )' else concat(term_day,' day(s)') end AS payment_term_text,A.quotation_no,A.issue_date ")
                    '.AppendLine(" 	,concat(E.name,' ',D.remark,' : ',D.job_order) As item_name,D.quantity,F.name As unit_name,G.name as currency,D.unit_price,D.amount ")
                    ' begin modify case apply 1 qty to base 100 for installment
                    .AppendLine(" 	,concat(E.name,' ',substring_index(D.remark,'#',-1),' : ',D.job_order) As item_name")
                    .AppendLine("	,if(INSTR(D.remark,'#')>0,D.quantity / REPLACE(substring_index(D.remark,'#',2),'#',''),D.quantity) quantity,F.name As unit_name,G.name as currency")
                    .AppendLine("	,if(INSTR(D.remark,'#')>0,D.unit_price * REPLACE(substring_index(D.remark,'#',2),'#',''),D.unit_price) unit_price,D.amount")
                    ' end modify
                    .AppendLine("	,CONCAT('Discount ',CASE D.discount_type WHEN 2 THEN CONCAT('(',discount,'%)') ELSE '' END) AS discount_type_text ")
                    .AppendLine("	,A.discount_total,A.sub_total,A.vat_amount,A.total_amount ")
                    .AppendLine("	,concat('Note : ',H.ie_name,'\r\n',IF(ifnull(A.remark,'')='','',concat(A.remark,'\r\n')),'Please confirm purchase order by fax or phone\r\nThankyou') remark ")
                    .AppendLine("	,A.deliver_to,A.delivery_date,CASE ifnull(A.contact, '') WHEN '' THEN '' ELSE CONCAT('Contact : ',A.contact) END AS contact ")
                    .AppendLine(" FROM po_header A ")
                    .AppendLine(" LEFT JOIN mst_vendor B ON A.vendor_id = B.id ")
                    .AppendLine(" LEFT JOIN mst_payment_term C ON A.payment_term_id = C.id ")
                    .AppendLine(" JOIN po_detail D ON D.po_header_id = A.id ")
                    .AppendLine(" LEFT JOIN mst_item E ON D.item_id = E.id ")
                    .AppendLine(" LEFT JOIN mst_unit F ON D.unit_id = F.id ")
                    .AppendLine(" LEFT JOIN mst_currency G ON A.currency_id = G.id ")
                    .AppendLine(" LEFT JOIN ( ")
                    .AppendLine("	select D.po_header_id,GROUP_CONCAT(distinct H.name SEPARATOR ' /') ie_name ")
                    .AppendLine("	from po_detail D ")
                    .AppendLine("	left join mst_ie H on D.ie_id = H.id ")
                    .AppendLine("	where D.po_header_id = ?purchase_id1 ")
                    .AppendLine("	group by D.po_header_id ")
                    .AppendLine(") H on A.id = H.po_header_id ")
                    .AppendLine(" LEFT JOIN mst_vendor_branch I ON A.vendor_branch_id = I.id ")
                    .AppendLine(" ,(select @ROWNUM:=0) Z ")
                    .AppendLine(" WHERE A.id = ?purchase_id2 ")
                    .AppendLine(" 	AND A.status_id <> 6 ")
                    ' assign parameter
                    objConn.AddParameter("?purchase_id1", intPurchaseId)
                    objConn.AddParameter("?purchase_id2", intPurchaseId)

                End With

                ' execute by datatable
                objDT = New System.Data.DataTable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_SearchPurchasePDF = New Entity.ImpPurchasePDFEntty
                ' assign data ot object entity
                objItem = objDT.Rows(0)
                With DB_SearchPurchasePDF
                    .id = intPurchaseId
                    'vendor_name()
                    .vendor_name = objConn.CheckDBNull(objItem("vendor_name"), Common.DBConnection.DBType.DBString)
                    'address()
                    .address = objConn.CheckDBNull(objItem("address"), Common.DBConnection.DBType.DBString)
                    'zipcode()
                    .zipcode = objConn.CheckDBNull(objItem("zipcode"), Common.DBConnection.DBType.DBString)
                    'tel()
                    .tel = objConn.CheckDBNull(objItem("tel"), Common.DBConnection.DBType.DBString)
                    'fax()
                    .fax = objConn.CheckDBNull(objItem("fax"), Common.DBConnection.DBType.DBString)
                    'attn()
                    .attn = objConn.CheckDBNull(objItem("attn"), Common.DBConnection.DBType.DBString)
                    'po_no()
                    .po_no = objConn.CheckDBNull(objItem("po_no"), Common.DBConnection.DBType.DBString)
                    'payment_term_text()
                    .payment_term_text = objConn.CheckDBNull(objItem("payment_term_text"), Common.DBConnection.DBType.DBString)
                    'currency
                    .currency = objConn.CheckDBNull(objItem("currency"), Common.DBConnection.DBType.DBString)
                    'quotation_no()
                    .quotation_no = objConn.CheckDBNull(objItem("quotation_no"), Common.DBConnection.DBType.DBString)
                    'issue_date()
                    .issue_date = objConn.CheckDBNull(objItem("issue_date"), Common.DBConnection.DBType.DBString).ToString
                    'discount_type_text
                    .discount_type_text = objConn.CheckDBNull(objItem("discount_type_text"), Common.DBConnection.DBType.DBString)
                    'discount_total()
                    .discount_total = objConn.CheckDBNull(objItem("discount_total"), Common.DBConnection.DBType.DBDecimal)
                    'sub_total()
                    .sub_total = objConn.CheckDBNull(objItem("sub_total"), Common.DBConnection.DBType.DBDecimal)
                    'vat_amount()
                    .vat_amount = objConn.CheckDBNull(objItem("vat_amount"), Common.DBConnection.DBType.DBDecimal)
                    'total_amount()
                    .total_amount = objConn.CheckDBNull(objItem("total_amount"), Common.DBConnection.DBType.DBDecimal)
                    'remark()
                    .remark = objConn.CheckDBNull(objItem("remark"), Common.DBConnection.DBType.DBString)
                    'amount_text()
                    .amount_text = objCom.ConvertNum2Word(.total_amount)
                    'deliver_to()
                    .deliver_to = objConn.CheckDBNull(objItem("deliver_to"), Common.DBConnection.DBType.DBString)
                    'delivery_date()
                    .delivery_date = objConn.CheckDBNull(objItem("delivery_date"), Common.DBConnection.DBType.DBString).ToString
                    'contact()
                    .contact = objConn.CheckDBNull(objItem("contact"), Common.DBConnection.DBType.DBString).ToString
                End With

                intNO = 0
                DB_SearchPurchasePDF.purchase_detail = New List(Of Entity.ImpPurchaseDetailEntity)
                ' assign data ot object entity
                For Each objItem In objDT.Rows
                    objPurchaseDetail = New Entity.ImpPurchaseDetailEntity
                    With objPurchaseDetail
                        .grp = objConn.CheckDBNull(objItem("grp"), Common.DBConnection.DBType.DBDecimal)
                        .po_header_id = intPurchaseId
                        'no() ID
                        '.no = intNO + 1
                        .no = objConn.CheckDBNull(objItem("ID"), Common.DBConnection.DBType.DBDecimal)
                        'item_name()
                        .item_name = objConn.CheckDBNull(objItem("item_name"), Common.DBConnection.DBType.DBString)
                        'quantity()
                        .quantity = objConn.CheckDBNull(objItem("quantity"), Common.DBConnection.DBType.DBDecimal)
                        'unit_name()
                        .unit_name = objConn.CheckDBNull(objItem("unit_name"), Common.DBConnection.DBType.DBString)
                        'unit_price()
                        .unit_price = objConn.CheckDBNull(objItem("unit_price"), Common.DBConnection.DBType.DBDecimal)
                        'amount()
                        .amount = objConn.CheckDBNull(objItem("amount"), Common.DBConnection.DBType.DBDecimal)
                        'discount_type_text()
                        .discount_type_text = objConn.CheckDBNull(objItem("discount_type_text"), Common.DBConnection.DBType.DBString)

                    End With
                    DB_SearchPurchasePDF.purchase_detail.Add(objPurchaseDetail)
                Next

            Catch ex As Exception
                ' write error log
                DB_SearchPurchasePDF = Nothing
                objLog.ErrorLog("DB_SearchPurchasePDF(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_SearchPurchasePDF(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_InsertPurchase
        '	Discription	    : Insert data purchase
        '	Return Value	: integer
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_InsertPurchase(ByVal objPurchase As Entity.ImpPurchaseEntity, Optional ByRef strPoNo_New As String = "", Optional ByRef intPoId_New As Integer = 0) As Integer Implements IPo_HeaderDao.DB_InsertPurchase
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlag As Integer = 0
                '*Format	@po no = KTT-PO-YYMM-xxx
                Dim strPo_no As String = "KTT-PO-"
                Dim intPo_no As Integer = 0
                Dim intPurchaseId As Integer = 0
                Dim intCheckAdd As Integer = 0

                DB_InsertPurchase = 0

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess

                ' open begin transaction
                objConn.BeginTrans()

                'Get po_no (new)
                strPo_no = DB_GetPoNo(intPo_no)

                ' assign sql statement
                strSQL = New Text.StringBuilder
                With strSQL
                    .AppendLine(" INSERT INTO po_header ( ")
                    .AppendLine(" 	po_no ")
                    .AppendLine(" 	,po_type ")
                    .AppendLine(" 	,vendor_id ")
                    .AppendLine(" 	,vendor_branch_id ")
                    .AppendLine(" 	,quotation_no ")
                    .AppendLine(" 	,delivery_date ")
                    .AppendLine(" 	,payment_term_id ")
                    .AppendLine(" 	,vat_id ")
                    .AppendLine(" 	,wt_id ")
                    .AppendLine(" 	,currency_id ")
                    .AppendLine(" 	,remark ")
                    .AppendLine(" 	,attn ")
                    .AppendLine(" 	,deliver_to ")
                    .AppendLine(" 	,contact ")
                    .AppendLine(" 	,sub_total ")
                    .AppendLine(" 	,discount_total ")
                    .AppendLine(" 	,vat_amount ")
                    .AppendLine(" 	,wt_amount ")
                    .AppendLine(" 	,total_amount ")
                    .AppendLine(" 	,issue_date ")
                    .AppendLine(" 	,user_id ")
                    .AppendLine(" 	,status_id ")
                    .AppendLine(" 	,created_by ")
                    .AppendLine(" 	,created_date ")
                    .AppendLine(" 	,updated_by ")
                    .AppendLine(" 	,updated_date ")
                    .AppendLine(" 	) ")
                    .AppendLine(" VALUES ( ")
                    .AppendLine(" 	?po_no ")
                    .AppendLine(" 	,?po_type ")
                    .AppendLine(" 	,?vendor_id ")
                    .AppendLine(" 	,?vendor_branch_id ")
                    .AppendLine(" 	,?quotation_no ")
                    .AppendLine(" 	,?delivery_date ")
                    .AppendLine(" 	,?payment_term_id ")
                    .AppendLine(" 	,?vat_id ")
                    .AppendLine(" 	,?wt_id ")
                    .AppendLine(" 	,?currency_id ")
                    .AppendLine(" 	,?remark ")
                    .AppendLine(" 	,?attn ")
                    .AppendLine(" 	,?deliver_to ")
                    .AppendLine(" 	,?contact ")
                    .AppendLine(" 	,?sub_total ")
                    .AppendLine(" 	,?discount_total ")
                    .AppendLine(" 	,?vat_amount ")
                    .AppendLine(" 	,?wt_amount ")
                    .AppendLine(" 	,?total_amount ")
                    .AppendLine(" 	,date_format(now(), '%Y%m%d') ")
                    .AppendLine(" 	,?user_id1 ")
                    .AppendLine(" 	,?status_id ")
                    .AppendLine(" 	,?user_id2 ")
                    .AppendLine(" 	,date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" 	,?user_id2 ")
                    .AppendLine(" 	,date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" 	); ")
                End With

                With objPurchase
                    ' assign parameter
                    '?po_no
                    objConn.AddParameter("?po_no", strPo_no)
                    ',?po_type
                    objConn.AddParameter("?po_type", .po_type)
                    ',?vendor_id
                    objConn.AddParameter("?vendor_id", .vendor_id)
                    objConn.AddParameter("?vendor_branch_id", .vendor_branch_id)
                    ',?quotation_no
                    objConn.AddParameter("?quotation_no", .quotation_no.Trim)
                    ',?delivery_date
                    objConn.AddParameter("?delivery_date", .delivery_date.ToString)
                    ',?payment_term_id
                    objConn.AddParameter("?payment_term_id", .payment_term_id)
                    ',?vat_id
                    objConn.AddParameter("?vat_id", .vat_id)
                    ',?wt_id
                    objConn.AddParameter("?wt_id", .wt_id)
                    ',?currency_id
                    objConn.AddParameter("?currency_id", .currency_id)
                    ',?remark
                    objConn.AddParameter("?remark", .remark)
                    ',?attn
                    objConn.AddParameter("?attn", .attn)
                    ',?deliver_to
                    objConn.AddParameter("?deliver_to", .deliver_to)
                    ',?contact
                    objConn.AddParameter("?contact", .contact)
                    ',?sub_total
                    objConn.AddParameter("?sub_total", .sub_total)
                    ',?discount_total
                    objConn.AddParameter("?discount_total", .discount_total)
                    ',?vat_amount
                    objConn.AddParameter("?vat_amount", .vat_amount)
                    ',?wt_amount
                    objConn.AddParameter("?wt_amount", .wt_amount)
                    ',?total_amount
                    objConn.AddParameter("?total_amount", .total_amount)
                    ',date_format(now(), '%Y%m%d')
                    ',?user_id1
                    objConn.AddParameter("?user_id1", .user_id)
                    ',?status_id = 3
                    objConn.AddParameter("?status_id", RecordStatus.Waiting)
                    ',?user_id2
                    objConn.AddParameter("?user_id2", .created_by)
                End With
                ' execute by ExecuteNonQuery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag = -1 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If

                ' assign sql statement
                strSQL = New Text.StringBuilder
                With strSQL
                    .AppendLine(" SELECT id ")
                    .AppendLine(" FROM po_header ")
                    .AppendLine(" WHERE po_no = ?po_no; ")
                    ' assign parameter
                    objConn.AddParameter("?po_no", strPo_no.Trim)
                End With
                ' execute by ExecuteScalar
                intPurchaseId = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError

                'Check data purchasetr_detail
                If (Not objPurchase.purchase_detail Is Nothing) AndAlso objPurchase.purchase_detail.Count > 0 Then
                    intCheckAdd = 0
                    For Each objDetail In objPurchase.purchase_detail
                        ' assign sql statement
                        strSQL = New Text.StringBuilder
                        With strSQL
                            .AppendLine(" INSERT INTO po_detail ( ")
                            .AppendLine(" 	po_header_id ")
                            .AppendLine(" 	,item_id ")
                            .AppendLine(" 	,job_order ")
                            .AppendLine(" 	,ie_id ")
                            .AppendLine(" 	,unit_price ")
                            .AppendLine(" 	,quantity ")
                            .AppendLine(" 	,unit_id ")
                            .AppendLine(" 	,discount ")
                            .AppendLine(" 	,discount_type ")
                            .AppendLine(" 	,remark ")
                            .AppendLine(" 	,amount ")
                            .AppendLine(" 	,vat_amount ")
                            .AppendLine(" 	,wt_amount ")
                            .AppendLine(" 	,created_by ")
                            .AppendLine(" 	,created_date ")
                            .AppendLine(" 	,updated_by ")
                            .AppendLine(" 	,updated_date ")
                            .AppendLine(" 	) ")
                            .AppendLine(" VALUES ( ")
                            .AppendLine(" 	?purchase_id ")
                            .AppendLine(" 	,?item_id ")
                            .AppendLine(" 	,?job_order ")
                            .AppendLine(" 	,?ie_id ")
                            .AppendLine(" 	,?unit_price ")
                            .AppendLine(" 	,?quantity ")
                            .AppendLine(" 	,?unit_id ")
                            .AppendLine(" 	,?discount ")
                            .AppendLine(" 	,?discount_type ")
                            .AppendLine(" 	,?remark ")
                            .AppendLine("  	,?amount ")
                            .AppendLine(" 	,?vat_amount ")
                            .AppendLine(" 	,?wt_amount ")
                            .AppendLine(" 	,?user_id ")
                            .AppendLine(" 	,date_format(now(), '%Y%m%d%H%i%s') ")
                            .AppendLine(" 	,?user_id ")
                            .AppendLine(" 	,date_format(now(), '%Y%m%d%H%i%s') ")
                            .AppendLine(" 	) ")
                        End With
                        With objDetail
                            ' assign parameter
                            '?po_header.id
                            objConn.AddParameter("?purchase_id", intPurchaseId)
                            ',?item_id
                            objConn.AddParameter("?item_id", .item_id)
                            ',?job_order
                            objConn.AddParameter("?job_order", .job_order.Trim)
                            ',?ie_id
                            objConn.AddParameter("?ie_id", .ie_id)
                            ',?unit_price
                            objConn.AddParameter("?unit_price", .unit_price)
                            ',?quantity
                            objConn.AddParameter("?quantity", .quantity)
                            ',?unit_id
                            objConn.AddParameter("?unit_id", .unit_id)
                            ',?discount
                            objConn.AddParameter("?discount", .discount)
                            ',?discount_type
                            objConn.AddParameter("?discount_type", .discount_type)
                            ',?remark
                            objConn.AddParameter("?remark", .remark)
                            ',?amount
                            objConn.AddParameter("?amount", .amount)
                            ',?vat_amount  'amount * po_header.vat
                            objConn.AddParameter("?vat_amount", .vat_amount)
                            ',?wt_amount   'amount * po_header.wt
                            objConn.AddParameter("?wt_amount", .wt_amount)
                            ',?user_id
                            objConn.AddParameter("?user_id", .created_by)
                            ',date_format(now(), '%Y%m%d%H%i%s')
                            'objConn.AddParameter("?purchase_id", .item_id)
                        End With
                        ' execute by ExecuteNonQuery
                        intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                        strMsgErr = objConn.MessageError
                        If intFlag = 1 Then intCheckAdd += 1
                    Next

                    If objPurchase.purchase_detail.Count <> intCheckAdd Then
                        objConn.RollbackTrans()
                        Exit Function
                    End If
                End If

                ' assign sql statement
                strSQL = New Text.StringBuilder
                With strSQL
                    If intPo_no = 1 Then
                        .AppendLine(" INSERT INTO po_running ( ")
                        .AppendLine(" 	po_year ")
                        .AppendLine(" 	,po_month ")
                        .AppendLine(" 	,po_last ")
                        .AppendLine(" 	,created_by ")
                        .AppendLine(" 	,created_date ")
                        .AppendLine(" 	,updated_by ")
                        .AppendLine(" 	,updated_date ")
                        .AppendLine(" 	) ")
                        .AppendLine(" VALUES ( ")
                        .AppendLine(" 	year(now()) ")
                        .AppendLine(" 	,month(now()) ")
                        .AppendLine(" 	,1 ")
                        .AppendLine(" 	,?user_id ")
                        .AppendLine(" 	,date_format(now(), '%Y%m%d%H%i%s') ")
                        .AppendLine(" 	,?user_id ")
                        .AppendLine(" 	,date_format(now(), '%Y%m%d%H%i%s') ")
                        .AppendLine(" 	); ")
                        ' assign parameter
                        objConn.AddParameter("?user_id", objPurchase.created_by)

                    Else
                        .AppendLine(" UPDATE po_running ")
                        .AppendLine(" SET po_last = ?po_no ")
                        .AppendLine(" 	,updated_by = ?user_id ")
                        .AppendLine(" 	,updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                        .AppendLine(" WHERE po_year = year(now()) ")
                        .AppendLine(" 	AND po_month = month(now()); ")
                        '' assign parameter
                        objConn.AddParameter("?po_no", intPo_no)
                        objConn.AddParameter("?user_id", objPurchase.created_by)

                    End If

                End With
                ' execute by ExecuteNonQuery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError

                ' check data
                If intFlag = -1 Or intFlag = 0 Then
                    objConn.RollbackTrans()
                Else
                    objConn.CommitTrans()
                    intPoId_New = intPurchaseId
                    strPoNo_New = strPo_no
                    DB_InsertPurchase = intFlag
                End If

            Catch ex As Exception
                ' write error log
                DB_InsertPurchase = 0
                objLog.ErrorLog("DB_InsertPurchase(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_InsertPurchase(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        Private Function GetPurchaseIdByDetail(ByVal strPoNo As String, ByVal intPoType As Integer, ByVal intVendorId As Integer) As Integer
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intPurchaseId As Integer

                GetPurchaseIdByDetail = 0

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and assign parameter
                With strSQL
                    .AppendLine(" SELECT po.id ")
                    .AppendLine(" FROM po_header AS po ")
                    .AppendLine(" WHERE po.po_no = ?po_no ")
                    .AppendLine(" 	AND po.po_type = ?po_type ")
                    .AppendLine(" 	AND po.vendor_id = ?vendor_id ")

                    objConn.AddParameter("?po_no", strPoNo.Trim)
                    objConn.AddParameter("?po_type", intPoType)
                    objConn.AddParameter("?vendor_id", intVendorId)
                End With

                ' execute by ExecuteScalar
                intPurchaseId = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                GetPurchaseIdByDetail = intPurchaseId

            Catch ex As Exception
                ' write error log
                GetPurchaseIdByDetail = 0
                objLog.ErrorLog("GetPurchaseIdByDetail(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetPurchaseIdByDetail(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_UpdatePurchase
        '	Discription	    : Update data purchase
        '	Return Value	: integer
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_UpdatePurchase(ByRef objPurchase As Entity.ImpPurchaseEntity) As Integer Implements IPo_HeaderDao.DB_UpdatePurchase
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlag As Integer = 0
                '•	@po no = KTT-PO-YYMM-xxx-Rx
                Dim strPo_no As String = String.Empty
                Dim intCheck As Integer = 0

                DB_UpdatePurchase = 0

                ' check data po_no
                If objPurchase.po_no.Trim.Length = 15 Or objPurchase.po_no.Trim.Length < 15 Then
                    '* ผิด Format Po_No กรณีที่น้อยกว่า 15 ตัว
                    strPo_no = objPurchase.po_no.Trim & "-R1"
                Else
                    strPo_no = objPurchase.po_no.Substring(0, 15)
                    If IsNumeric(objPurchase.po_no.Substring(17)) Then
                        strPo_no &= ("-R" & (CInt(objPurchase.po_no.Substring(17)) + 1))
                    Else
                        '* ผิด Format Po_No กรณีที่ไม่ใช่ค่าตัวเลข
                        strPo_no &= "-R1"
                    End If
                End If

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                ' open begin transaction
                objConn.BeginTrans()

                ' assign sql statement
                strSQL = New Text.StringBuilder
                With strSQL
                    .AppendLine(" UPDATE po_header ")
                    .AppendLine(" SET po_no = ?po_no ")
                    .AppendLine(" 	,po_type = ?po_type ")
                    .AppendLine(" 	,vendor_id = ?vendor_id ")
                    .AppendLine(" 	,vendor_branch_id = ?vendor_branch_id ")
                    .AppendLine(" 	,quotation_no = ?quotation_no ")
                    .AppendLine(" 	,delivery_date = ?delivery_date ")
                    .AppendLine(" 	,payment_term_id = ?payment_term_id ")
                    .AppendLine(" 	,vat_id = ?vat_id ")
                    .AppendLine(" 	,wt_id = ?wt_id ")
                    .AppendLine(" 	,currency_id = ?currency_id ")
                    .AppendLine(" 	,remark = ?remark ")
                    .AppendLine(" 	,attn = ?attn ")
                    .AppendLine(" 	,deliver_to = ?deliver_to ")
                    .AppendLine(" 	,contact = ?contact ")
                    .AppendLine(" 	,sub_total = ?sub_total ")
                    .AppendLine(" 	,discount_total = ?discount_total ")
                    .AppendLine(" 	,vat_amount = ?vat_amount ")
                    .AppendLine(" 	,wt_amount = ?wt_amount ")
                    .AppendLine(" 	,total_amount = ?total_amount ")
                    .AppendLine(" 	,issue_date = date_format(now(), '%Y%m%d') ")
                    .AppendLine(" 	,user_id = ?user_id1 ")
                    .AppendLine(" 	,status_id = ?status_id ")
                    .AppendLine(" 	,updated_by = ?user_id2 ")
                    .AppendLine(" 	,updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                    .Append(" WHERE id = ?id;")
                End With

                With objPurchase
                    ' assign parameter
                    '?po_no
                    objConn.AddParameter("?po_no", strPo_no)
                    ',?po_type
                    objConn.AddParameter("?po_type", .po_type)
                    ',?vendor_id
                    objConn.AddParameter("?vendor_id", .vendor_id)
                    objConn.AddParameter("?vendor_branch_id", .vendor_branch_id)
                    ',?quotation_no
                    objConn.AddParameter("?quotation_no", .quotation_no.Trim)
                    ',?delivery_date
                    objConn.AddParameter("?delivery_date", .delivery_date.ToString)
                    ',?payment_term_id
                    objConn.AddParameter("?payment_term_id", .payment_term_id)
                    ',?vat_id
                    objConn.AddParameter("?vat_id", .vat_id)
                    ',?wt_id
                    objConn.AddParameter("?wt_id", .wt_id)
                    ',?currency_id
                    objConn.AddParameter("?currency_id", .currency_id)
                    ',?remark
                    objConn.AddParameter("?remark", .remark)
                    ',?attn
                    objConn.AddParameter("?attn", .attn)
                    ',?deliver_to
                    objConn.AddParameter("?deliver_to", .deliver_to)
                    ',?contact
                    objConn.AddParameter("?contact", .contact)
                    ',?sub_total
                    objConn.AddParameter("?sub_total", .sub_total)
                    ',?discount_total
                    objConn.AddParameter("?discount_total", .discount_total)
                    ',?vat_amount
                    objConn.AddParameter("?vat_amount", .vat_amount)
                    ',?wt_amount
                    objConn.AddParameter("?wt_amount", .wt_amount)
                    ',?total_amount
                    objConn.AddParameter("?total_amount", .total_amount)
                    ',?user_id1
                    objConn.AddParameter("?user_id1", .user_id)
                    ',?status_id
                    objConn.AddParameter("?status_id", RecordStatus.Waiting)
                    ',?user_id2
                    objConn.AddParameter("?user_id2", .updated_by)
                    'WHERE id = ?id
                    objConn.AddParameter("?id", .id)
                End With
                ' execute by ExecuteNonQuery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag = -1 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If
                objConn.ClearParameter()

                If (Not objPurchase.purchase_detail Is Nothing) AndAlso objPurchase.purchase_detail.Count > 0 Then
                    Dim strIds As String = String.Empty
                    For Each objDetail In objPurchase.purchase_detail
                        strIds = strIds + "," + objDetail.id.ToString()
                    Next
                    strSQL.Length = 0
                    strSQL.AppendLine(" Delete from po_detail where po_header_id=?purchase_id and FIND_IN_SET(CAST(id AS CHAR), ?ids) = 0; ")
                    objConn.AddParameter("?purchase_id", objPurchase.id)
                    objConn.AddParameter("?ids", strIds)
                    ' execute by ExecuteNonQuery
                    intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                    strMsgErr = objConn.MessageError
                    objConn.ClearParameter()
                End If
                'Check data purchasetr_detail
                If (Not objPurchase.purchase_detail Is Nothing) AndAlso objPurchase.purchase_detail.Count > 0 Then

                    intCheck = 0
                    For Each objDetail In objPurchase.purchase_detail
                        ' assign sql statement
                        strSQL.Length = 0
                        With strSQL
                            .AppendLine(" Update po_detail set ")
                            .AppendLine(" 	item_id = ?item_id")
                            .AppendLine(" 	,job_order = ?job_order ")
                            .AppendLine(" 	,ie_id = ?ie_id ")
                            .AppendLine(" 	,unit_price = ?unit_price ")
                            .AppendLine(" 	,quantity = ?quantity ")
                            .AppendLine(" 	,unit_id = ?unit_id ")
                            .AppendLine(" 	,discount = ?discount ")
                            .AppendLine(" 	,discount_type = ?discount_type ")
                            .AppendLine(" 	,remark = ?remark ")
                            .AppendLine(" 	,amount = ?amount ")
                            .AppendLine(" 	,vat_amount = ?vat_amount ")
                            .AppendLine(" 	,wt_amount = ?wt_amount ")
                            .AppendLine(" 	,updated_by = ?user_id ")
                            .AppendLine(" 	,updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                            .AppendLine(" Where po_header_id = ?purchase_id and id = ?id;")
                        End With

                        With objDetail
                            ' assign parameter
                            ',?id
                            objConn.AddParameter("?purchase_id", objPurchase.id)
                            objConn.AddParameter("?id", .id)
                            ',?item_id
                            objConn.AddParameter("?item_id", .item_id)
                            ',?job_order
                            objConn.AddParameter("?job_order", .job_order.Trim)
                            ',?ie_id
                            objConn.AddParameter("?ie_id", .ie_id)
                            ',?unit_price
                            objConn.AddParameter("?unit_price", .unit_price)
                            ',?quantity
                            objConn.AddParameter("?quantity", .quantity)
                            ',?unit_id
                            objConn.AddParameter("?unit_id", .unit_id)
                            ',?discount
                            objConn.AddParameter("?discount", .discount)
                            ',?discount_type
                            objConn.AddParameter("?discount_type", .discount_type)
                            ',?remark
                            objConn.AddParameter("?remark", .remark)
                            ',?amount
                            objConn.AddParameter("?amount", .amount)
                            ',?vat_amount  'amount * po_header.vat
                            objConn.AddParameter("?vat_amount", .vat_amount)
                            ',?wt_amount   'amount * po_header.wt
                            objConn.AddParameter("?wt_amount", .wt_amount)
                            ',?user_id
                            objConn.AddParameter("?user_id", .updated_by)
                        End With
                        ' execute by ExecuteNonQuery
                        intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                        strMsgErr = objConn.MessageError
                        objConn.ClearParameter()

                        If intFlag < 1 Then
                            strSQL.Length = 0
                            With strSQL
                                .AppendLine(" INSERT INTO po_detail (po_header_id,item_id,job_order,ie_id,unit_price,quantity,unit_id,discount ")
                                .AppendLine(" 	,discount_type,remark,amount,vat_amount,wt_amount,created_by,created_date,updated_by,updated_date) ")
                                .AppendLine(" VALUES ( ")
                                .AppendLine(" 	?purchase_id,?item_id,?job_order,?ie_id,?unit_price,?quantity,?unit_id,?discount ")
                                .AppendLine(" 	,?discount_type,?remark,?amount,?vat_amount,?wt_amount,?user_id,date_format(now(), '%Y%m%d%H%i%s') ")
                                .AppendLine(" 	,?user_id,date_format(now(), '%Y%m%d%H%i%s') ")
                                .AppendLine(" 	); ")
                            End With

                            With objDetail
                                ' assign parameter
                                ',?id
                                objConn.AddParameter("?purchase_id", objPurchase.id)
                                objConn.AddParameter("?id", .id)
                                ',?item_id
                                objConn.AddParameter("?item_id", .item_id)
                                ',?job_order
                                objConn.AddParameter("?job_order", .job_order.Trim)
                                ',?ie_id
                                objConn.AddParameter("?ie_id", .ie_id)
                                ',?unit_price
                                objConn.AddParameter("?unit_price", .unit_price)
                                ',?quantity
                                objConn.AddParameter("?quantity", .quantity)
                                ',?unit_id
                                objConn.AddParameter("?unit_id", .unit_id)
                                ',?discount
                                objConn.AddParameter("?discount", .discount)
                                ',?discount_type
                                objConn.AddParameter("?discount_type", .discount_type)
                                ',?remark
                                objConn.AddParameter("?remark", .remark)
                                ',?amount
                                objConn.AddParameter("?amount", .amount)
                                ',?vat_amount  'amount * po_header.vat
                                objConn.AddParameter("?vat_amount", .vat_amount)
                                ',?wt_amount   'amount * po_header.wt
                                objConn.AddParameter("?wt_amount", .wt_amount)
                                ',?user_id
                                objConn.AddParameter("?user_id", .updated_by)
                            End With
                            ' execute by ExecuteNonQuery
                            intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                            strMsgErr = objConn.MessageError
                            objConn.ClearParameter()

                            If intFlag = 1 Then intCheck += 1
                        Else
                            intCheck += 1
                        End If
                    Next

                    If objPurchase.purchase_detail.Count <> intCheck Then
                        objConn.RollbackTrans()
                        Exit Function
                    End If
                End If

                ' check data
                If intFlag = -1 Or intFlag = 0 Then
                    objConn.RollbackTrans()
                Else
                    objPurchase.po_no = strPo_no
                    objConn.CommitTrans()
                    DB_UpdatePurchase = intFlag
                End If

            Catch ex As Exception
                ' write error log
                DB_UpdatePurchase = 0
                objLog.ErrorLog("DB_UpdatePurchase(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_UpdatePurchase(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        Public Function DB_ModifyPurchase(ByRef objPurchase As Entity.ImpPurchaseEntity) As Integer Implements IPo_HeaderDao.DB_ModifyPurchase
            Dim strSQL As New Text.StringBuilder
            Try
                Dim intFlag As Integer = 0
                'Dim strPo_no As String = String.Empty
                Dim intCheck As Integer = 0
                DB_ModifyPurchase = 0

                ' case modify no need change p/o number
                ' check data po_no
                'If objPurchase.po_no.Trim.Length = 15 Or objPurchase.po_no.Trim.Length < 15 Then
                '    '* ผิด Format Po_No กรณีที่น้อยกว่า 15 ตัว
                '    strPo_no = objPurchase.po_no.Trim & "-R1"
                'Else
                '    strPo_no = objPurchase.po_no.Substring(0, 15)
                '    If IsNumeric(objPurchase.po_no.Substring(17)) Then
                '        strPo_no &= ("-R" & (CInt(objPurchase.po_no.Substring(17)) + 1))
                '    Else
                '        '* ผิด Format Po_No กรณีที่ไม่ใช่ค่าตัวเลข
                '        strPo_no &= "-R1"
                '    End If
                'End If

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                ' open begin transaction
                objConn.BeginTrans()

                ' assign sql statement
                strSQL = New Text.StringBuilder
                With strSQL
                    .AppendLine(" UPDATE po_header ")
                    '.AppendLine(" SET po_no = ?po_no ")
                    .AppendLine(" SET vendor_branch_id = ?vendor_branch_id ")
                    .AppendLine(" 	,quotation_no = ?quotation_no ")
                    .AppendLine(" 	,delivery_date = ?delivery_date ")
                    .AppendLine(" 	,payment_term_id = ?payment_term_id ")
                    .AppendLine(" 	,remark = ?remark ")
                    .AppendLine(" 	,attn = ?attn ")
                    .AppendLine(" 	,deliver_to = ?deliver_to ")
                    .AppendLine(" 	,contact = ?contact ")
                    .AppendLine(" 	,updated_by = ?user_id ")
                    .AppendLine(" 	,updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                    .Append(" WHERE id = ?id;")
                End With

                With objPurchase
                    ' assign parameter
                    'objConn.AddParameter("?po_no", strPo_no)
                    objConn.AddParameter("?vendor_branch_id", .vendor_branch_id)
                    objConn.AddParameter("?quotation_no", .quotation_no.Trim)
                    objConn.AddParameter("?delivery_date", .delivery_date.ToString)
                    objConn.AddParameter("?payment_term_id", .payment_term_id)
                    objConn.AddParameter("?remark", .remark)
                    objConn.AddParameter("?attn", .attn)
                    objConn.AddParameter("?deliver_to", .deliver_to)
                    objConn.AddParameter("?contact", .contact)
                    objConn.AddParameter("?user_id", .updated_by)
                    objConn.AddParameter("?id", .id)
                End With
                ' execute by ExecuteNonQuery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag = -1 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If

                'Check data purchasetr_detail
                If (Not objPurchase.purchase_detail Is Nothing) AndAlso objPurchase.purchase_detail.Count > 0 Then

                    intCheck = 0
                    For Each objDetail In objPurchase.purchase_detail
                        ' assign sql statement
                        strSQL.Length = 0
                        objConn.ClearParameter()
                        With strSQL
                            .AppendLine(" Update po_detail set ")
                            .AppendLine(" 	ie_id = ?ie_id ")
                            .AppendLine(" 	,unit_id = ?unit_id ")
                            .AppendLine(" 	,updated_by = ?user_id ")
                            .AppendLine(" 	,updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                            .AppendLine(" Where po_header_id = ?purchase_id and id = ?id;")
                        End With

                        With objDetail
                            ' assign parameter
                            ',?id
                            objConn.AddParameter("?purchase_id", objPurchase.id)
                            objConn.AddParameter("?id", .id)
                            objConn.AddParameter("?ie_id", .ie_id)
                            objConn.AddParameter("?unit_id", .unit_id)
                            objConn.AddParameter("?user_id", .updated_by)
                        End With
                        ' execute by ExecuteNonQuery
                        intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                        strMsgErr = objConn.MessageError
                        If intFlag = 1 Then intCheck += 1

                        strSQL.Length = 0
                        objConn.ClearParameter()
                        With strSQL
                            .AppendLine("update stock A join payment_detail B on A.payment_detail_id=B.id")
                            .AppendLine("set A.ie_id = ?ie_id ")
                            .AppendLine(" 	,A.updated_by = ?user_id ")
                            .AppendLine(" 	,A.updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                            .AppendLine("Where B.po_detail_id = ?id;")
                        End With

                        With objDetail
                            ' assign parameter
                            objConn.AddParameter("?ie_id", .ie_id)
                            objConn.AddParameter("?user_id", .updated_by)
                            objConn.AddParameter("?id", .id)
                        End With
                        ' execute by ExecuteNonQuery
                        objConn.ExecuteNonQuery(strSQL.ToString)
                        strMsgErr = objConn.MessageError

                        strSQL.Length = 0
                        objConn.ClearParameter()
                        With strSQL
                            .AppendLine("update accounting A")
                            .AppendLine("join payment_detail B on A.ref_id=B.payment_header_id")
                            .AppendLine("join po_detail C on B.po_detail_id=C.id and A.item_id=C.item_id and A.job_order=C.job_order")
                            .AppendLine("set A.ie_id = ?ie_id, A.vendor_branch_id = ?vendor_branch_id")
                            .AppendLine(" 	,A.updated_by = ?user_id ")
                            .AppendLine(" 	,A.updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                            .AppendLine("Where A.type=3 and B.po_header_id = ?po_header_id and A.item_id = ?item_id and A.job_order = ?job_order;")
                        End With

                        With objDetail
                            ' assign parameter
                            objConn.AddParameter("?ie_id", .ie_id)
                            objConn.AddParameter("?vendor_branch_id", objPurchase.vendor_branch_id)
                            objConn.AddParameter("?user_id", .updated_by)
                            objConn.AddParameter("?po_header_id", objPurchase.id)
                            objConn.AddParameter("?item_id", .item_id)
                            objConn.AddParameter("?job_order", .job_order)
                        End With
                        ' execute by ExecuteNonQuery
                        objConn.ExecuteNonQuery(strSQL.ToString)
                        strMsgErr = objConn.MessageError

                    Next

                    If objPurchase.purchase_detail.Count <> intCheck Then
                        objConn.RollbackTrans()
                        Exit Function
                    End If
                End If

                ' check data
                If intFlag = -1 Or intFlag = 0 Then
                    objConn.RollbackTrans()
                Else
                    'objPurchase.po_no = strPo_no
                    objConn.CommitTrans()
                    DB_ModifyPurchase = intFlag
                End If

            Catch ex As Exception
                ' write error log
                DB_ModifyPurchase = 0
                objLog.ErrorLog("DB_ModifyPurchase(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_ModifyPurchase(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: DB_GetPoNo
        '	Discription	    : Get data po_no by po_running
        '	Return Value	: String
        '	Create User	    : Boonyarit
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetPoNo(Optional ByRef intPoNo As Integer = 0) As String Implements IPo_HeaderDao.DB_GetPoNo
            Dim objConns As New Common.DBConnection.MySQLAccess
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim strPurchasePoNo As String = "KTT-PO-"

                DB_GetPoNo = String.Empty

                ' set new object
                objConns = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and assign parameter
                With strSQL
                    .AppendLine(" SELECT ifnull(max(po_last), 0) + 1 As po_no ")
                    .AppendLine(" FROM po_running ")
                    .AppendLine(" WHERE po_year = year(now()) ")
                    .AppendLine(" 	AND po_month = month(now()); ")
                End With

                ' execute by datatable
                intPoNo = objConns.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConns.MessageError

                strPurchasePoNo &= (Now().Year.ToString.Substring(2, 2) & Format(Now().Month, "00") & "-" & Format(intPoNo, "000"))
                DB_GetPoNo = strPurchasePoNo

            Catch ex As Exception
                ' write error log
                DB_GetPoNo = String.Empty
                objLog.ErrorLog("DB_GetPoNo(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetPoNo(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConns Is Nothing Then objConns.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPurchaseApproveList
        '	Discription	    : Get Purchase Approve List
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPurchaseApproveList( _
            ByVal objSearchPurchase As Dto.PurchaseSearchDto _
        ) As System.Collections.Generic.List(Of Entity.ImpPurchaseEntity) Implements IPo_HeaderDao.GetPurchaseApproveList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPurchaseApproveList = New List(Of Entity.ImpPurchaseEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objPurchase As Entity.ImpPurchaseEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT distinct po.id                                                                 ")
                    .AppendLine("  ,(CASE po.po_type WHEN 0 THEN 'Purchase' WHEN 1 THEN 'Outsource' END) AS po_type_name ")
                    .AppendLine("  ,po.po_no,status.name AS status_name,po.status_id,po.issue_date                      ")
                    .AppendLine("  ,CONCAT(requester.first_name, ' ', requester.last_name) AS applied_by                ")
                    .AppendLine("  ,vendor.name AS vendor_name                                                          ")
                    .AppendLine(" FROM po_header po                                                                     ")
                    .AppendLine(" inner join po_detail pod on pod.po_header_id=po.id                                    ")
                    .AppendLine(" INNER JOIN status ON po.status_id = status.id                                         ")
                    .AppendLine(" LEFT JOIN mst_vendor vendor ON po.vendor_id = vendor.id                               ")
                    .AppendLine(" LEFT JOIN user requester ON po.created_by = requester.id                              ")
                    .AppendLine(" WHERE 1=1                                                                             ")
                    If Not String.IsNullOrEmpty(objSearchPurchase.status_ids) AndAlso objSearchPurchase.status_ids.Equals("3") Then
                        .AppendLine(" and po.status_id=3                                                                ")
                    Else
                        .AppendLine(" and po.status_id>=2                                                               ")
                    End If
                    Dim strSuperUser As String = System.Web.Configuration.WebConfigurationManager.AppSettings("AutoApprovePurchase")
                    If strSuperUser.ToUpper.IndexOf(HttpContext.Current.Session("UserName").ToString().ToUpper) = -1 Then
                        .AppendLine(" AND (requester.purchase_next_approve = ?approver_id)                              ")
                    End If
                    .AppendLine(" AND ( (ISNULL(?joborder_start) AND ISNULL(?joborder_end))                             ")
                    .AppendLine("   OR ( ((NOT ISNULL(?joborder_start)) AND (NOT ISNULL(?joborder_end))) AND (pod.job_order BETWEEN ?joborder_start AND ?joborder_end) ) ")
                    .AppendLine("   OR ( (((NOT ISNULL(?joborder_start)) AND ISNULL(?joborder_end) )) AND (pod.job_order >= ?joborder_start) ) ")
                    .AppendLine("   OR ( ((ISNULL(?joborder_start) AND (NOT ISNULL(?joborder_end)) )) AND (pod.job_order <= ?joborder_end) ) ")
                    .AppendLine(" ) ")
                    .AppendLine(" AND ( po.po_type = IFNULL(?po_type, po.po_type) ) ")
                    '.AppendLine(" AND ( ISNULL(?status_ids) OR (FIND_IN_SET(CAST(po.status_id AS CHAR), ?status_ids) > 0) ) ")
                    .AppendLine(" AND ( (ISNULL(?po_startno) AND ISNULL(?po_endno))  ")
                    .AppendLine("       OR ( ((NOT ISNULL(?po_startno)) AND (NOT ISNULL(?po_endno))) AND (left(po.po_no,15) BETWEEN ?po_startno AND ?po_endno) ) ")
                    .AppendLine("       OR ( (((NOT ISNULL(?po_startno)) AND ISNULL(?po_endno) )) AND (left(po.po_no,15) >= ?po_startno) ) ")
                    .AppendLine("       OR ( ((ISNULL(?po_startno) AND (NOT ISNULL(?po_endno)) )) AND (left(po.po_no,15) <= ?po_endno) ) ")
                    .AppendLine("      )")
                    .AppendLine(" AND ( ISNULL(?vendor_name) OR (vendor.name LIKE CONCAT('%', ?vendor_name, '%')) ) ")
                    .AppendLine(" AND ( (ISNULL(?issuedate_start) AND ISNULL(?issuedate_end)) ")
                    .AppendLine("       OR ( ((NOT ISNULL(?issuedate_start)) AND (NOT ISNULL(?issuedate_end))) AND (CAST(po.issue_date AS DATE) BETWEEN CAST(?issuedate_start AS DATE) AND CAST(?issuedate_end AS DATE)) )       ")
                    .AppendLine("       OR ( (((NOT ISNULL(?issuedate_start)) AND ISNULL(?issuedate_end) )) AND CAST(po.issue_date AS DATE) >= CAST(?issuedate_start AS DATE)) ")
                    .AppendLine("       OR ( ((ISNULL(?issuedate_start) AND (NOT ISNULL(?issuedate_end)) )) AND CAST(po.issue_date AS DATE) <= CAST(?issuedate_end AS DATE)) ")
                    .AppendLine("      )")
                    If Not String.IsNullOrEmpty(objSearchPurchase.status_ids) AndAlso objSearchPurchase.status_ids.Equals("3") Then
                        .AppendLine(" ORDER BY po.po_no asc;")
                    Else
                        .AppendLine(" ORDER BY po.po_no desc;")
                    End If
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?approver_id", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?joborder_start", IIf(String.IsNullOrEmpty(objSearchPurchase.job_order_from), DBNull.Value, objSearchPurchase.job_order_from))
                objConn.AddParameter("?joborder_end", IIf(String.IsNullOrEmpty(objSearchPurchase.job_order_to), DBNull.Value, objSearchPurchase.job_order_to))
                objConn.AddParameter("?po_type", IIf(String.IsNullOrEmpty(objSearchPurchase.po_type), DBNull.Value, objSearchPurchase.po_type))
                objConn.AddParameter("?po_startno", IIf(String.IsNullOrEmpty(objSearchPurchase.po_no_from), DBNull.Value, objSearchPurchase.po_no_from))
                objConn.AddParameter("?po_endno", IIf(String.IsNullOrEmpty(objSearchPurchase.po_no_to), DBNull.Value, objSearchPurchase.po_no_to))
                objConn.AddParameter("?vendor_name", IIf(String.IsNullOrEmpty(objSearchPurchase.vendor_name), DBNull.Value, objSearchPurchase.vendor_name))
                objConn.AddParameter("?issuedate_start", IIf(String.IsNullOrEmpty(objSearchPurchase.issue_date_start), DBNull.Value, objSearchPurchase.issue_date_start))
                objConn.AddParameter("?issuedate_end", IIf(String.IsNullOrEmpty(objSearchPurchase.issue_date_end), DBNull.Value, objSearchPurchase.issue_date_end))
                'objConn.AddParameter("?status_ids", IIf(String.IsNullOrEmpty(objSearchPurchase.status_ids), DBNull.Value, objSearchPurchase.status_ids))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objPurchase = New Entity.ImpPurchaseEntity
                        ' assign data from db to entity object
                        With objPurchase
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .po_type_text = IIf(IsDBNull(dr.Item("po_type_name")), Nothing, dr.Item("po_type_name"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .status = IIf(IsDBNull(dr.Item("status_name")), Nothing, dr.Item("status_name"))
                            .status_id = IIf(IsDBNull(dr.Item("status_id")), Nothing, dr.Item("status_id"))
                            .issue_date_text = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .applied_by = IIf(IsDBNull(dr.Item("applied_by")), Nothing, dr.Item("applied_by"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                        End With
                        ' add Material to list
                        GetPurchaseApproveList.Add(objPurchase)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPurchaseApproveList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPurchaseApproveList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdatePurchaseStatus
        '	Discription	    : Update Status or Purchase
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 05-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdatePurchaseStatus( _
            ByVal strPurchaseId As String, _
            ByVal intStatus As Integer _
        ) As Integer Implements IPo_HeaderDao.UpdatePurchaseStatus
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdatePurchaseStatus = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE po_header po						                  ")
                    .AppendLine("		SET po.status_id = ?status_id							  ")
                    .AppendLine("		  ,po.updated_by = ?updated_by							  ")
                    .AppendLine("		  ,po.updated_date = date_format(now(), '%Y%m%d%H%i%s')	  ")
                    .AppendLine("		WHERE FIND_IN_SET(CAST(po.id AS CHAR), ?selected_ids) > 0;")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?status_id", intStatus)
                    .AddParameter("?selected_ids", strPurchaseId)
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
                UpdatePurchaseStatus = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdatePurchaseStatus(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdatePurchaseStatus(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdatePurchaseStatus(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPOApprove
        '	Discription	    : Get data approve on po_header
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 09-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPOApprove(ByVal strPoId As String) As System.Collections.Generic.List(Of Entity.IPo_HeaderEntity) Implements IPo_HeaderDao.GetPOApprove
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetPOApprove = New List(Of Entity.IPo_HeaderEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable vendor entity
                Dim objPOEnt As Entity.IPo_HeaderEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT   ")
                    .AppendLine("		 id, po_no, status_id 	")
                    .AppendLine("	FROM po_header  	")
                    .AppendLine("	WHERE (FIND_IN_SET(CAST(id AS CHAR), ?id) > 0)  ; ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", strPoId)
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new vendor entity
                        objPOEnt = New Entity.ImpPo_HeaderEntity
                        With objPOEnt
                            ' assign data to object account entity
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .status_id = IIf(IsDBNull(dr.Item("status_id")), Nothing, dr.Item("status_id"))
                        End With
                        ' add object po header entity to list
                        GetPOApprove.Add(objPOEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPOApprove(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetPOApprove(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_InsertTaskPurchase
        '	Discription	    : Insert data task on TB: Task
        '	Return Value	: Integer
        '	Create User	    : Boonyarit
        '	Create Date	    : 14-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_InsertTaskPurchase(ByVal intPoId_New As Integer) As Integer Implements IPo_HeaderDao.DB_InsertTaskPurchase
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlag As Integer = 0
                Dim intUserId As Integer = HttpContext.Current.Session("UserID")
                DB_InsertTaskPurchase = 0

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess

                ' open begin transaction
                'objConn.BeginTrans()

                ' assign sql statement
                strSQL = New Text.StringBuilder
                With strSQL
                    .AppendLine(" INSERT INTO task(schedule, task, note, refpage, tskpage, user_id, refkey) ")
                    .AppendLine(" SELECT replace(convert(DATE_ADD(convert(delivery_date, DATE), INTERVAL 1 Day),char),'-','') ")
                    '.AppendLine(" SELECT DATE_FORMAT(DATE_ADD(convert(delivery_date, DATE), INTERVAL 1 Month), '%Y%m%d') ")
                    .AppendLine(" 	,'Create Invoice' ")
                    .AppendLine(" 	,CONCAT('Vendor[',b.NAME,'] PO[',po_no,'] DeliveryPlan[',delivery_date,']') ")
                    '.AppendLine(" 	,CONCAT('Vendor[',b.NAME,'] PO[',po_no,'] DeliveryPlan[',DATE_FORMAT(convert(delivery_date, DATE), '%d %b %Y'),']') ")
                    .AppendLine(" 	,CONCAT('../purchase/KTPU01_Detail.aspx?ID=',a.id) ")
                    .AppendLine(" 	,CONCAT('../purchase/KTPU04_Delivery.aspx?Mode=Add&ID=',a.id,'&TskID=@id') ")
                    .AppendLine(" 	,?user_id ")
                    .AppendLine(" 	,a.id ")
                    .AppendLine(" FROM po_header a ")
                    .AppendLine(" JOIN mst_vendor b ON a.vendor_id = b.id ")
                    .AppendLine(" WHERE a.id = ?po_id; ")

                    ' assign parameter
                    '?user_id
                    objConn.AddParameter("?user_id", intUserId)
                    '?po_no
                    objConn.AddParameter("?po_id", intPoId_New)
                End With

                ' execute by ExecuteNonQuery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag = -1 Then
                    'objConn.RollbackTrans()
                    Exit Function
                Else
                    'objConn.CommitTrans()
                    DB_InsertTaskPurchase = intFlag
                End If

            Catch ex As Exception
                ' write error log
                DB_InsertTaskPurchase = 0
                objLog.ErrorLog("DB_InsertTaskPurchase(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_InsertTaskPurchase(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_UpdateTaskPurchase
        '	Discription	    : Update data task on TB: Task
        '	Return Value	: Integer
        '	Create User	    : Charoon
        '	Create Date	    : 03-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_UpdateTaskPurchase(ByVal intPoId_New As Integer) As Integer Implements IPo_HeaderDao.DB_UpdateTaskPurchase
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlag As Integer = 0
                Dim intUserId As Integer = HttpContext.Current.Session("UserID")
                DB_UpdateTaskPurchase = 0

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess

                ' open begin transaction
                'objConn.BeginTrans()

                ' assign sql statement
                strSQL = New Text.StringBuilder
                With strSQL
                    .AppendLine("update task join po_header po on task.refkey=po.id JOIN mst_vendor b ON po.vendor_id = b.id")
                    .AppendLine("set task.schedule=DATE_FORMAT(DATE_ADD(convert(po.delivery_date, DATE), INTERVAL 1 Day), '%Y%m%d'), ")
                    .AppendLine("note=CONCAT('Vendor[',b.NAME,'] PO[',po.po_no,'] DeliveryPlan[',po.delivery_date,']') ")
                    .AppendLine("where task.user_id=?user_id and task.refkey=?po_id;")

                    ' assign parameter
                    '?user_id
                    objConn.AddParameter("?user_id", intUserId)
                    '?po_no
                    objConn.AddParameter("?po_id", intPoId_New)
                End With

                ' execute by ExecuteNonQuery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag = -1 Then
                    'objConn.RollbackTrans()
                    Exit Function
                Else
                    'objConn.CommitTrans()
                    DB_UpdateTaskPurchase = intFlag
                End If

            Catch ex As Exception
                ' write error log
                DB_UpdateTaskPurchase = 0
                objLog.ErrorLog("DB_UpdateTaskPurchase(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_UpdateTaskPurchase(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

    End Class

End Namespace



