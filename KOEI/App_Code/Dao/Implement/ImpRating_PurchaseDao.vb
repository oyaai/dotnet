#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpRating_Purchase
'	Class Discription	: Execute process Rating Purchase
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 20-06-2013
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
Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Exception
#End Region

Namespace Dao
    Public Class ImpRating_PurchaseDao
        Implements IRating_PurchaseDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private objUtility As New Common.Utilities.Utility
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: GetRating_PurchaseList
        '	Discription	    : Get Rating Purchase list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetRating_PurchaseList( _
            ByVal objRatingEnt As Entity.IRating_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpRating_PurchaseDetailEntity) Implements IRating_PurchaseDao.GetRating_PurchaseList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetRating_PurchaseList = New List(Of Entity.ImpRating_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpRating_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 					")
                    .AppendLine("			rate.id				")
                    .AppendLine("			,pay_head.invoice_no				")
                    .AppendLine("			,po_head.po_no				")
                    .AppendLine("			,mst.name AS vendor_name				")
                    .AppendLine("			,CAST(pay_head.payment_date AS DATE) AS payment_date				")
                    .AppendLine("			,CAST(pay_head.delivery_date AS DATE) AS delivery_date				")
                    .AppendLine("			,concat(rate.quality,'/',rate.delivery,'/',rate.service) AS score 				")
                    .AppendLine("		from vendor_rating rate					")
                    .AppendLine("		join payment_header pay_head 					")
                    .AppendLine("		on rate.payment_header_id=pay_head.id					")
                    .AppendLine("		join po_header po_head 					")
                    .AppendLine("		on pay_head.po_header_id=po_head.id					")
                    .AppendLine("		left join mst_vendor mst					")
                    .AppendLine("		on po_head.vendor_id=mst.id					")
                    .AppendLine("		where 						")
                    'invoice no
                    .AppendLine("		pay_head.invoice_no = IFNULL(?invoice_no, pay_head.invoice_no)	")
                    'po type
                    .AppendLine("		AND (po_type = IFNULL(?po_type, po_type))					")
                    'po no
                    .AppendLine("		AND (po_no = IFNULL(?po_no, po_no))						")

                    'payment date
                    .AppendLine("		AND ( (ISNULL(?payment_start_date) AND ISNULL(?payment_end_date))						")
                    .AppendLine("				OR ( ((NOT ISNULL(?payment_start_date)) AND (NOT ISNULL(?payment_end_date))) AND (CAST(pay_head.payment_date AS DATE) BETWEEN CAST(?payment_start_date AS DATE) AND CAST(?payment_end_date AS DATE)) )      				")
                    .AppendLine("				OR ( (((NOT ISNULL(?payment_start_date)) AND ISNULL(?payment_end_date) )) AND CAST(pay_head.payment_date AS DATE) >= CAST(?payment_start_date AS DATE))				")
                    .AppendLine("				OR ( ((ISNULL(?payment_start_date) AND (NOT ISNULL(?payment_end_date)) )) AND CAST(pay_head.payment_date AS DATE) <= CAST(?payment_end_date AS DATE))				")
                    .AppendLine("			)					")

                    'delivery date
                    .AppendLine("		AND ( (ISNULL(?delivery_start_date) AND ISNULL(?delivery_end_date))						")
                    .AppendLine("		      OR ( ((NOT ISNULL(?delivery_start_date)) AND (NOT ISNULL(?delivery_end_date))) AND (CAST(pay_head.delivery_date AS DATE) BETWEEN CAST(?delivery_start_date AS DATE) AND CAST(?delivery_end_date AS DATE)) )      						")
                    .AppendLine("		      OR ( (((NOT ISNULL(?delivery_start_date)) AND ISNULL(?delivery_end_date) )) AND CAST(pay_head.delivery_date AS DATE) >= CAST(?delivery_start_date AS DATE))						")
                    .AppendLine("		      OR ( ((ISNULL(?delivery_start_date) AND (NOT ISNULL(?delivery_end_date)) )) AND CAST(pay_head.delivery_date AS DATE) <= CAST(?delivery_end_date AS DATE))						")
                    .AppendLine("		    )						")

                    'vendor name
                    .AppendLine("		AND (ISNULL(?vendor_name) OR (mst.name LIKE CONCAT('%', ?vendor_name, '%')))						")
                    .AppendLine("		order by id desc					")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objRatingEnt.strInvoce_no), DBNull.Value, objRatingEnt.strInvoce_no))
                objConn.AddParameter("?po_type", IIf(String.IsNullOrEmpty(objRatingEnt.strSearchType), DBNull.Value, objRatingEnt.strSearchType))
                objConn.AddParameter("?po_no", IIf(String.IsNullOrEmpty(objRatingEnt.strPO), DBNull.Value, objRatingEnt.strPO))
                objConn.AddParameter("?payment_start_date", IIf(String.IsNullOrEmpty(objRatingEnt.strPaymentDateFrom), DBNull.Value, objRatingEnt.strPaymentDateFrom))
                objConn.AddParameter("?payment_end_date", IIf(String.IsNullOrEmpty(objRatingEnt.strPaymentDateTo), DBNull.Value, objRatingEnt.strPaymentDateTo))
                objConn.AddParameter("?delivery_start_date", IIf(String.IsNullOrEmpty(objRatingEnt.strDeliveryDateFrom), DBNull.Value, objRatingEnt.strDeliveryDateFrom))
                objConn.AddParameter("?delivery_end_date", IIf(String.IsNullOrEmpty(objRatingEnt.strDeliveryDateTo), DBNull.Value, objRatingEnt.strDeliveryDateTo))
                objConn.AddParameter("?vendor_name", IIf(String.IsNullOrEmpty(objRatingEnt.strVendor_name), DBNull.Value, objRatingEnt.strVendor_name))
                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpRating_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .payment_date = IIf(IsDBNull(dr.Item("payment_date")), Nothing, dr.Item("payment_date"))
                            .delivery_date = IIf(IsDBNull(dr.Item("delivery_date")), Nothing, dr.Item("delivery_date"))
                            .score = IIf(IsDBNull(dr.Item("score")), Nothing, dr.Item("score"))
                        End With
                        ' add Accounting to list
                        GetRating_PurchaseList.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetRating_PurchaseList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetRating_PurchaseList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetVendorRatingReport
        '	Discription	    : Get Vendor Rating Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVendorRatingReport( _
            ByVal objRatingEnt As Entity.IRating_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpRating_PurchaseDetailEntity) Implements IRating_PurchaseDao.GetVendorRatingReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetVendorRatingReport = New List(Of Entity.ImpRating_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpRating_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 					")
                    .AppendLine("			pay_head.invoice_no				")
                    .AppendLine("			,po_head.po_no				")
                    .AppendLine("			,mst.name as vendor_name				")
                    .AppendLine("			,ifnull(CAST(po_head.delivery_date AS DATE),'') delivery_date_po	")
                    .AppendLine("			,CAST(pay_head.delivery_date AS DATE) AS delivery_date				")
                    .AppendLine("			,CAST(pay_head.payment_date AS DATE) AS payment_date				")
                    .AppendLine("			,rate.quality				")
                    .AppendLine("			,rate.delivery				")
                    .AppendLine("			,rate.service				")
                    .AppendLine("		from vendor_rating rate					")
                    .AppendLine("		join payment_header pay_head 					")
                    .AppendLine("		on rate.payment_header_id=pay_head.id					")
                    .AppendLine("		join po_header po_head 					")
                    .AppendLine("		on pay_head.po_header_id=po_head.id					")
                    .AppendLine("		left join mst_vendor mst					")
                    .AppendLine("		on po_head.vendor_id=mst.id					")
                    .AppendLine("		where 						")
                    'invoice no
                    .AppendLine("		pay_head.invoice_no = IFNULL(?invoice_no, pay_head.invoice_no)	")
                    'po type
                    .AppendLine("		AND (po_type = IFNULL(?po_type, po_type))					")
                    'po no
                    .AppendLine("		AND (po_no = IFNULL(?po_no, po_no))						")

                    'payment date
                    .AppendLine("		AND ( (ISNULL(?payment_start_date) AND ISNULL(?payment_end_date))						")
                    .AppendLine("				OR ( ((NOT ISNULL(?payment_start_date)) AND (NOT ISNULL(?payment_end_date))) AND (CAST(pay_head.payment_date AS DATE) BETWEEN CAST(?payment_start_date AS DATE) AND CAST(?payment_end_date AS DATE)) )      				")
                    .AppendLine("				OR ( (((NOT ISNULL(?payment_start_date)) AND ISNULL(?payment_end_date) )) AND CAST(pay_head.payment_date AS DATE) >= CAST(?payment_start_date AS DATE))				")
                    .AppendLine("				OR ( ((ISNULL(?payment_start_date) AND (NOT ISNULL(?payment_end_date)) )) AND CAST(pay_head.payment_date AS DATE) <= CAST(?payment_end_date AS DATE))				")
                    .AppendLine("			)					")

                    'delivery date
                    .AppendLine("		AND ( (ISNULL(?delivery_start_date) AND ISNULL(?delivery_end_date))						")
                    .AppendLine("		      OR ( ((NOT ISNULL(?delivery_start_date)) AND (NOT ISNULL(?delivery_end_date))) AND (CAST(pay_head.delivery_date AS DATE) BETWEEN CAST(?delivery_start_date AS DATE) AND CAST(?delivery_end_date AS DATE)) )      						")
                    .AppendLine("		      OR ( (((NOT ISNULL(?delivery_start_date)) AND ISNULL(?delivery_end_date) )) AND CAST(pay_head.delivery_date AS DATE) >= CAST(?delivery_start_date AS DATE))						")
                    .AppendLine("		      OR ( ((ISNULL(?delivery_start_date) AND (NOT ISNULL(?delivery_end_date)) )) AND CAST(pay_head.delivery_date AS DATE) <= CAST(?delivery_end_date AS DATE))						")
                    .AppendLine("		    )						")

                    'vendor name
                    .AppendLine("		AND (ISNULL(?vendor_name) OR (mst.name LIKE CONCAT('%', ?vendor_name, '%')))						")
                    '.AppendLine("		order by id desc					")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objRatingEnt.strInvoce_no), DBNull.Value, objRatingEnt.strInvoce_no))
                objConn.AddParameter("?po_type", IIf(String.IsNullOrEmpty(objRatingEnt.strSearchType), DBNull.Value, objRatingEnt.strSearchType))
                objConn.AddParameter("?po_no", IIf(String.IsNullOrEmpty(objRatingEnt.strPO), DBNull.Value, objRatingEnt.strPO))
                objConn.AddParameter("?payment_start_date", IIf(String.IsNullOrEmpty(objRatingEnt.strPaymentDateFrom), DBNull.Value, objRatingEnt.strPaymentDateFrom))
                objConn.AddParameter("?payment_end_date", IIf(String.IsNullOrEmpty(objRatingEnt.strPaymentDateTo), DBNull.Value, objRatingEnt.strPaymentDateTo))
                objConn.AddParameter("?delivery_start_date", IIf(String.IsNullOrEmpty(objRatingEnt.strDeliveryDateFrom), DBNull.Value, objRatingEnt.strDeliveryDateFrom))
                objConn.AddParameter("?delivery_end_date", IIf(String.IsNullOrEmpty(objRatingEnt.strDeliveryDateTo), DBNull.Value, objRatingEnt.strDeliveryDateTo))
                objConn.AddParameter("?vendor_name", IIf(String.IsNullOrEmpty(objRatingEnt.strVendor_name), DBNull.Value, objRatingEnt.strVendor_name))
                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpRating_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .delivery_date_po = IIf(IsDBNull(dr.Item("delivery_date_po")), Nothing, dr.Item("delivery_date_po"))
                            .delivery_date = IIf(IsDBNull(dr.Item("delivery_date")), Nothing, dr.Item("delivery_date"))
                            .payment_date = IIf(IsDBNull(dr.Item("payment_date")), Nothing, dr.Item("payment_date"))
                            .quality = IIf(IsDBNull(dr.Item("quality")), Nothing, dr.Item("quality"))
                            .delivery = IIf(IsDBNull(dr.Item("delivery")), Nothing, dr.Item("delivery"))
                            .service = IIf(IsDBNull(dr.Item("service")), Nothing, dr.Item("service"))
                        End With
                        ' add Accounting to list
                        GetVendorRatingReport.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVendorRatingReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetVendorRatingReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetVendorRatingReport
        '	Discription	    : Get Vendor Rating Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetYearVendorRatingReport( _
            ByVal objRatingEnt As Entity.IRating_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpRating_PurchaseDetailEntity) Implements IRating_PurchaseDao.GetYearVendorRatingReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetYearVendorRatingReport = New List(Of Entity.ImpRating_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpRating_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 					")
                    .AppendLine("           concat(year(pay_head.delivery_date),mst.id,po_head.po_type) grp ")
                    .AppendLine("			,po_head.po_type				")
                    .AppendLine("			,mst.id				")
                    .AppendLine("			,mst.name as vendor_name				")
                    .AppendLine("			,year(pay_head.delivery_date) delivery_year				")
                    .AppendLine("			,avg(rate.quality) as quality				")
                    .AppendLine("			,avg(rate.delivery) as delivery				")
                    .AppendLine("			,avg(rate.service) as service				")
                    .AppendLine("		from vendor_rating rate					")
                    .AppendLine("		join payment_header pay_head					")
                    .AppendLine("		on rate.payment_header_id=pay_head.id					")
                    .AppendLine("		join po_header po_head					")
                    .AppendLine("		on pay_head.po_header_id=po_head.id					")
                    .AppendLine("		left join mst_vendor mst					")
                    .AppendLine("		on po_head.vendor_id=mst.id					")
                    .AppendLine("		where 						")
                    'invoice no
                    .AppendLine("		pay_head.invoice_no = IFNULL(?invoice_no, pay_head.invoice_no)	")
                    'po type
                    .AppendLine("		AND (po_type = IFNULL(?po_type, po_type))					")
                    'po no
                    .AppendLine("		AND (po_no = IFNULL(?po_no, po_no))						")

                    'payment date
                    .AppendLine("		AND ( (ISNULL(?payment_start_date) AND ISNULL(?payment_end_date))						")
                    .AppendLine("				OR ( ((NOT ISNULL(?payment_start_date)) AND (NOT ISNULL(?payment_end_date))) AND (CAST(pay_head.payment_date AS DATE) BETWEEN CAST(?payment_start_date AS DATE) AND CAST(?payment_end_date AS DATE)) )      				")
                    .AppendLine("				OR ( (((NOT ISNULL(?payment_start_date)) AND ISNULL(?payment_end_date) )) AND CAST(pay_head.payment_date AS DATE) >= CAST(?payment_start_date AS DATE))				")
                    .AppendLine("				OR ( ((ISNULL(?payment_start_date) AND (NOT ISNULL(?payment_end_date)) )) AND CAST(pay_head.payment_date AS DATE) <= CAST(?payment_end_date AS DATE))				")
                    .AppendLine("			)					")

                    'delivery date
                    .AppendLine("		AND ( (ISNULL(?delivery_start_date) AND ISNULL(?delivery_end_date))						")
                    .AppendLine("		      OR ( ((NOT ISNULL(?delivery_start_date)) AND (NOT ISNULL(?delivery_end_date))) AND (CAST(pay_head.delivery_date AS DATE) BETWEEN CAST(?delivery_start_date AS DATE) AND CAST(?delivery_end_date AS DATE)) )      						")
                    .AppendLine("		      OR ( (((NOT ISNULL(?delivery_start_date)) AND ISNULL(?delivery_end_date) )) AND CAST(pay_head.delivery_date AS DATE) >= CAST(?delivery_start_date AS DATE))						")
                    .AppendLine("		      OR ( ((ISNULL(?delivery_start_date) AND (NOT ISNULL(?delivery_end_date)) )) AND CAST(pay_head.delivery_date AS DATE) <= CAST(?delivery_end_date AS DATE))						")
                    .AppendLine("		    )						")

                    'vendor name
                    .AppendLine("		AND (ISNULL(?vendor_name) OR (mst.name LIKE CONCAT('%', ?vendor_name, '%')))						")
                    .AppendLine("		group by					")
                    .AppendLine("		    po_type			")
                    .AppendLine("			,id		")
                    .AppendLine("			,year(pay_head.delivery_date)		")
                    .AppendLine("		order by id desc					")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objRatingEnt.strInvoce_no), DBNull.Value, objRatingEnt.strInvoce_no))
                objConn.AddParameter("?po_type", IIf(String.IsNullOrEmpty(objRatingEnt.strSearchType), DBNull.Value, objRatingEnt.strSearchType))
                objConn.AddParameter("?po_no", IIf(String.IsNullOrEmpty(objRatingEnt.strPO), DBNull.Value, objRatingEnt.strPO))
                objConn.AddParameter("?payment_start_date", IIf(String.IsNullOrEmpty(objRatingEnt.strPaymentDateFrom), DBNull.Value, objRatingEnt.strPaymentDateFrom))
                objConn.AddParameter("?payment_end_date", IIf(String.IsNullOrEmpty(objRatingEnt.strPaymentDateTo), DBNull.Value, objRatingEnt.strPaymentDateTo))
                objConn.AddParameter("?delivery_start_date", IIf(String.IsNullOrEmpty(objRatingEnt.strDeliveryDateFrom), DBNull.Value, objRatingEnt.strDeliveryDateFrom))
                objConn.AddParameter("?delivery_end_date", IIf(String.IsNullOrEmpty(objRatingEnt.strDeliveryDateTo), DBNull.Value, objRatingEnt.strDeliveryDateTo))
                objConn.AddParameter("?vendor_name", IIf(String.IsNullOrEmpty(objRatingEnt.strVendor_name), DBNull.Value, objRatingEnt.strVendor_name))
                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpRating_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .grp = IIf(IsDBNull(dr.Item("grp")), Nothing, dr.Item("grp"))
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .po_type = IIf(IsDBNull(dr.Item("po_type")), Nothing, dr.Item("po_type"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .delivery_year = IIf(IsDBNull(dr.Item("delivery_year")), Nothing, dr.Item("delivery_year"))
                            .quality = IIf(IsDBNull(dr.Item("quality")), Nothing, dr.Item("quality"))
                            .delivery = IIf(IsDBNull(dr.Item("delivery")), Nothing, dr.Item("delivery"))
                            .service = IIf(IsDBNull(dr.Item("service")), Nothing, dr.Item("service"))
                        End With
                        ' add Accounting to list
                        GetYearVendorRatingReport.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetYearVendorRatingReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetYearVendorRatingReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
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
        Public Function DeleteRatingInvoice( _
            ByVal intRatingId As Integer _
        ) As Integer Implements IRating_PurchaseDao.DeleteRatingInvoice
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteRatingInvoice = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       DELETE from vendor_rating                               ")
                    .AppendLine("		WHERE id = ?id							")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?id", intRatingId)

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
                DeleteRatingInvoice = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteRatingInvoice(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteRatingInvoice(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPurchaseList
        '	Discription	    : Get Purchase list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPurchaseList( _
            ByVal objRatingEnt As Entity.IRating_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpRating_PurchaseDetailEntity) Implements IRating_PurchaseDao.GetPurchaseList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPurchaseList = New List(Of Entity.ImpRating_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpRating_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 				")
                    .AppendLine("			pay_head.id			")
                    .AppendLine("			,pay_head.invoice_no			")
                    .AppendLine("			,po_head.po_no			")
                    .AppendLine("			,mst.name as vendor_name			")
                    .AppendLine("			,CAST(pay_head.payment_date AS DATE) AS payment_date			")
                    .AppendLine("			,CAST(pay_head.delivery_date AS DATE) AS delivery_date 			")
                    .AppendLine("		from payment_header pay_head 				")
                    .AppendLine("		join po_header po_head 				")
                    .AppendLine("		on pay_head.po_header_id=po_head.id				")
                    .AppendLine("		join  mst_vendor mst 				")
                    .AppendLine("		on po_head.vendor_id=mst.id				")
                    .AppendLine("		left join vendor_rating rate 				")
                    .AppendLine("		on pay_head.id=rate.payment_header_id				")
                    .AppendLine("		where pay_head.status_id<>6 				")
                    .AppendLine("		and rate.id is null 				")
                    'invoice no
                    .AppendLine("		AND pay_head.invoice_no = IFNULL(?invoice_no, pay_head.invoice_no)	")
                    'po type
                    .AppendLine("		AND (po_type = IFNULL(?po_type, po_type))					")
                    'po no
                    .AppendLine("		AND (po_no = IFNULL(?po_no, po_no))						")

                    'payment date
                    .AppendLine("		AND ( (ISNULL(?payment_start_date) AND ISNULL(?payment_end_date))						")
                    .AppendLine("				OR ( ((NOT ISNULL(?payment_start_date)) AND (NOT ISNULL(?payment_end_date))) AND (CAST(pay_head.payment_date AS DATE) BETWEEN CAST(?payment_start_date AS DATE) AND CAST(?payment_end_date AS DATE)) )      				")
                    .AppendLine("				OR ( (((NOT ISNULL(?payment_start_date)) AND ISNULL(?payment_end_date) )) AND CAST(pay_head.payment_date AS DATE) >= CAST(?payment_start_date AS DATE))				")
                    .AppendLine("				OR ( ((ISNULL(?payment_start_date) AND (NOT ISNULL(?payment_end_date)) )) AND CAST(pay_head.payment_date AS DATE) <= CAST(?payment_end_date AS DATE))				")
                    .AppendLine("			)					")

                    'delivery date
                    .AppendLine("		AND ( (ISNULL(?delivery_start_date) AND ISNULL(?delivery_end_date))						")
                    .AppendLine("		      OR ( ((NOT ISNULL(?delivery_start_date)) AND (NOT ISNULL(?delivery_end_date))) AND (CAST(pay_head.delivery_date AS DATE) BETWEEN CAST(?delivery_start_date AS DATE) AND CAST(?delivery_end_date AS DATE)) )      						")
                    .AppendLine("		      OR ( (((NOT ISNULL(?delivery_start_date)) AND ISNULL(?delivery_end_date) )) AND CAST(pay_head.delivery_date AS DATE) >= CAST(?delivery_start_date AS DATE))						")
                    .AppendLine("		      OR ( ((ISNULL(?delivery_start_date) AND (NOT ISNULL(?delivery_end_date)) )) AND CAST(pay_head.delivery_date AS DATE) <= CAST(?delivery_end_date AS DATE))						")
                    .AppendLine("		    )						")

                    'vendor name
                    .AppendLine("		AND (ISNULL(?vendor_name) OR (mst.name LIKE CONCAT('%', ?vendor_name, '%')))						")
                    .AppendLine("		order by pay_head.id desc				")

                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objRatingEnt.strInvoce_no), DBNull.Value, objRatingEnt.strInvoce_no))
                objConn.AddParameter("?po_type", IIf(String.IsNullOrEmpty(objRatingEnt.strSearchType), DBNull.Value, objRatingEnt.strSearchType))
                objConn.AddParameter("?po_no", IIf(String.IsNullOrEmpty(objRatingEnt.strPO), DBNull.Value, objRatingEnt.strPO))
                objConn.AddParameter("?payment_start_date", IIf(String.IsNullOrEmpty(objRatingEnt.strPaymentDateFrom), DBNull.Value, objRatingEnt.strPaymentDateFrom))
                objConn.AddParameter("?payment_end_date", IIf(String.IsNullOrEmpty(objRatingEnt.strPaymentDateTo), DBNull.Value, objRatingEnt.strPaymentDateTo))
                objConn.AddParameter("?delivery_start_date", IIf(String.IsNullOrEmpty(objRatingEnt.strDeliveryDateFrom), DBNull.Value, objRatingEnt.strDeliveryDateFrom))
                objConn.AddParameter("?delivery_end_date", IIf(String.IsNullOrEmpty(objRatingEnt.strDeliveryDateTo), DBNull.Value, objRatingEnt.strDeliveryDateTo))
                objConn.AddParameter("?vendor_name", IIf(String.IsNullOrEmpty(objRatingEnt.strVendor_name), DBNull.Value, objRatingEnt.strVendor_name))
                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpRating_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .payment_date = IIf(IsDBNull(dr.Item("payment_date")), Nothing, dr.Item("payment_date"))
                            .delivery_date = IIf(IsDBNull(dr.Item("delivery_date")), Nothing, dr.Item("delivery_date"))
                        End With
                        ' add Accounting to list
                        GetPurchaseList.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPurchaseList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPurchaseList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetRatingVendor
        '	Discription	    : Get Rating Vendor
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetRatingVendor( _
            ByVal ratingId As String, _
            ByVal payment_header_id As String _
        ) As System.Collections.Generic.List(Of Entity.ImpRating_PurchaseDetailEntity) Implements IRating_PurchaseDao.GetRatingVendor
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetRatingVendor = New List(Of Entity.ImpRating_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpRating_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 				                ")
                    .AppendLine("			quality		                    ")
                    .AppendLine("			,delivery		                ")
                    .AppendLine("			,service 		                ")
                    .AppendLine("		from vendor_rating  				")
                    .AppendLine("		where 1 =1               			")
                    If ratingId.Trim <> "" Then
                        .AppendLine("		and id = ?ratingId 				")
                    End If
                    If payment_header_id.Trim <> "" Then
                        .AppendLine("		and payment_header_id = ?payment_header_id 				")
                    End If
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                If ratingId.Trim <> "" Then
                    objConn.AddParameter("?ratingId", IIf(String.IsNullOrEmpty(ratingId), DBNull.Value, ratingId))
                End If
                If payment_header_id.Trim <> "" Then
                    objConn.AddParameter("?payment_header_id", IIf(String.IsNullOrEmpty(payment_header_id), DBNull.Value, payment_header_id))
                End If

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpRating_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .quality = IIf(IsDBNull(dr.Item("quality")), Nothing, dr.Item("quality"))
                            .delivery = IIf(IsDBNull(dr.Item("delivery")), Nothing, dr.Item("delivery"))
                            .service = IIf(IsDBNull(dr.Item("service")), Nothing, dr.Item("service"))
                        End With
                        ' add Accounting to list
                        GetRatingVendor.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetRatingVendor(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetRatingVendor(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: InsUpdVendor_Rating
        '	Discription	    : Insert/Update Vendor_Rating
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 15-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsUpdVendor_Rating( _
            ByVal mode As String, _
            ByVal strId As String, _
            ByVal strPayment_header_id As String, _
            ByVal strQuality As String, _
            ByVal strDelivery As String, _
            ByVal strService As String _
        ) As Integer Implements IRating_PurchaseDao.InsUpdVendor_Rating
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsUpdVendor_Rating = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    If mode = "Add" Then
                        .AppendLine("		INSERT vendor_rating (							")
                        .AppendLine("		  payment_header_id							")
                        .AppendLine("		  ,quality 							")
                        .AppendLine("		  ,delivery 							")
                        .AppendLine("		  ,service 							")
                        .AppendLine("		  ,created_by 						")
                        .AppendLine("		  ,created_date 						")
                        .AppendLine("		  ,updated_by 						")
                        .AppendLine("		  ,updated_date 							")
                        .AppendLine("		)							")
                        .AppendLine("		VALUES (							")
                        .AppendLine("		  ?payment_header_id							")
                        .AppendLine("		  ,?quality 							")
                        .AppendLine("		  ,?delivery 							")
                        .AppendLine("		  ,?service 							")
                        .AppendLine("		  ,?created_by 						")
                        .AppendLine("		  ,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') 						")
                        .AppendLine("		  ,?updated_by 						")
                        .AppendLine("		  ,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') 							")
                        .AppendLine("		)							")
                    ElseIf mode = "Edit" Then
                        .AppendLine("		UPDATE vendor_rating 							")
                        .AppendLine("		SET quality = ?quality							")
                        .AppendLine("		  ,delivery = ?delivery							")
                        .AppendLine("		  ,service = ?service							")
                        .AppendLine("		  ,updated_by = ?updated_by							")
                        .AppendLine("		  ,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                        .AppendLine("		WHERE (id = ?id);							")
                    End If
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    If mode = "Add" Then 'Add
                        .AddParameter("?payment_header_id", strPayment_header_id)
                        .AddParameter("?quality", strQuality)
                        .AddParameter("?delivery", strDelivery)
                        .AddParameter("?service", strService)
                        .AddParameter("?created_by", HttpContext.Current.Session("UserID"))
                        .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                    ElseIf mode = "Edit" Then 'Modify
                        .AddParameter("?quality", strQuality)
                        .AddParameter("?delivery", strDelivery)
                        .AddParameter("?service", strService)
                        .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                        .AddParameter("?id", strId)
                    End If
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
                InsUpdVendor_Rating = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsUpdVendor_Rating(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsUpdVendor_Rating(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsUpdVendor_Rating(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
    End Class
End Namespace


