Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports Dao

#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpPurchase_HistoryDao
'	Class Discription	: Class of table Purchase_History
'	Create User 		: Nisa S.
'	Create Date		    : 19-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Public Class ImpPurchase_HistoryDao
    Implements IPurchase_HistoryDao

    Private objConn As Common.DBConnection.MySQLAccess
    Private objLog As New Common.Logs.Log

#Region "Function"

    '/**************************************************************
    '	Function name	: GetPurchaseHistoryReport
    '	Discription	    : Get Purchase History Report
    '	Return Value	: List
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Function GetPurchaseHistoryReport( _
        ByVal objPurchaseHistoryEnt As Entity.IPurchase_HistoryEntity _
    ) As System.Collections.Generic.List(Of Entity.ImpPurchase_HistoryEntity) Implements IPurchase_HistoryDao.GetPurchaseHistoryReport
        ' variable for keep sql command
        Dim strSql As New Text.StringBuilder
        ' set default list
        GetPurchaseHistoryReport = New List(Of Entity.ImpPurchase_HistoryEntity)
        Try
            ' data reader object
            Dim dr As MySqlDataReader
            Dim objPurchaseHistory As Entity.ImpPurchase_HistoryEntity

            ' assign sql command
            With strSql
                .AppendLine(" 		select A.job_order,E.po_no,F.invoice_no,													  ")
                .AppendLine(" 		I.delivery_amount*ifnull(case upper(H.name) when 'THB' then 1 when 'JPY' then G.rate/1 else G.rate end,1) delivery_amount,													  ")
                .AppendLine(" 		B.VendorName,D.ItemName,F.delivery_date,I.delivery_qty													  ")
                .AppendLine(" 		from po_detail A													  ")
                .AppendLine(" 		join po_header E on E.id=A.po_header_id													  ")
                .AppendLine(" 		join payment_header F on F.po_header_id=E.id													  ")
                .AppendLine(" 		join payment_detail I on I.payment_header_id=F.id and I.po_header_id=A.po_header_id and I.po_detail_id=A.id													  ")
                .AppendLine(" 		left join (select *,name as VendorName from mst_vendor) B on E.vendor_id=B.id													  ")
                .AppendLine(" 		left join mst_ie C on A.ie_id=C.id													  ")
                .AppendLine(" 		left join (select *,name as ItemName from mst_item) D on A.item_id=D.id													  ")
                .AppendLine(" 		left join mst_currency H on E.currency_id=H.id													  ")
                .AppendLine(" 		left join (select Z.currency_id,Z.issue_date,Y.ef_date,Y.rate													  ")
                .AppendLine(" 		  from (													  ")
                .AppendLine(" 		    select A.currency_id,A.issue_date,max(B.ef_date) max_ef_date													  ")
                .AppendLine(" 		    from po_header A													  ")
                .AppendLine(" 		    left join mst_schedule_rate B on B.currency_id=A.currency_id and B.ef_date<=A.issue_date													  ")
                .AppendLine(" 		    where B.delete_fg=0													  ")
                .AppendLine(" 		    group by A.currency_id,A.issue_date													  ")
                .AppendLine(" 		  ) Z left join mst_schedule_rate Y on Y.currency_id=Z.currency_id and Y.ef_date=Z.max_ef_date													  ")
                .AppendLine(" 		) G on G.currency_id=E.currency_id and G.issue_date=E.issue_date													  ")
                .AppendLine(" WHERE E.status_id<>6 ")
                .AppendLine(" AND C.code like 'B03%' ")
                .AppendLine(" AND (A.job_order=?job_order or isnull(?job_order)) ")
                .AppendLine(" AND (F.invoice_no=?invoice_no or isnull(?invoice_no))    ")
                .AppendLine(" AND (E.po_no=?po_no or isnull(?po_no))   ")
                .AppendLine(" AND (ISNULL(?VendorName) OR B.VendorName LIKE CONCAT('%', ?VendorName , '%'))  ")
                .AppendLine(" AND (ISNULL(?ItemName) OR D.ItemName LIKE CONCAT('%', ?ItemName , '%'))  ")
                .AppendLine(" AND (F.delivery_date>=?delivery_date1 or isnull(?delivery_date1))   ")
                .AppendLine(" AND (F.delivery_date<=?delivery_date2 or isnull(?delivery_date2))   ")
            End With

            ' new connection
            objConn = New Common.DBConnection.MySQLAccess
            ' assign parameter
            objConn.AddParameter("?job_order", IIf(String.IsNullOrEmpty(objPurchaseHistoryEnt.job_order), DBNull.Value, objPurchaseHistoryEnt.job_order))
            objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objPurchaseHistoryEnt.invoice_no), DBNull.Value, objPurchaseHistoryEnt.invoice_no))
            objConn.AddParameter("?po_no", IIf(String.IsNullOrEmpty(objPurchaseHistoryEnt.po_no), DBNull.Value, objPurchaseHistoryEnt.po_no))
            objConn.AddParameter("?VendorName", IIf(String.IsNullOrEmpty(objPurchaseHistoryEnt.VendorName), DBNull.Value, objPurchaseHistoryEnt.VendorName))
            objConn.AddParameter("?ItemName", IIf(String.IsNullOrEmpty(objPurchaseHistoryEnt.ItemName), DBNull.Value, objPurchaseHistoryEnt.ItemName))
            objConn.AddParameter("?delivery_date1", IIf(String.IsNullOrEmpty(objPurchaseHistoryEnt.delivery_date1), DBNull.Value, objPurchaseHistoryEnt.delivery_date1))
            objConn.AddParameter("?delivery_date2", IIf(String.IsNullOrEmpty(objPurchaseHistoryEnt.delivery_date2), DBNull.Value, objPurchaseHistoryEnt.delivery_date2))


            ' execute reader
            dr = objConn.ExecuteReader(strSql.ToString)

            ' check exist data
            If dr.HasRows Then
                While dr.Read
                    ' new object entity
                    objPurchaseHistory = New Entity.ImpPurchase_HistoryEntity
                    ' assign data from db to entity object
                    With objPurchaseHistory
                        .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                        .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                        .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                        .delivery_amount = IIf(IsDBNull(dr.Item("delivery_amount")), Nothing, dr.Item("delivery_amount"))
                        .VendorName = IIf(IsDBNull(dr.Item("VendorName")), Nothing, dr.Item("VendorName"))
                        .ItemName = IIf(IsDBNull(dr.Item("ItemName")), Nothing, dr.Item("ItemName"))
                        .delivery_date = IIf(IsDBNull(dr.Item("delivery_date")), Nothing, dr.Item("delivery_date"))
                        .delivery_qty = IIf(IsDBNull(dr.Item("delivery_qty")), Nothing, dr.Item("delivery_qty"))

                    End With
                    ' add Purchase History to list
                    GetPurchaseHistoryReport.Add(objPurchaseHistory)
                End While
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetPurchaseHistoryReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            ' write sql command
            objLog.InfoLog("GetPurchaseHistoryReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Function

#End Region


End Class
