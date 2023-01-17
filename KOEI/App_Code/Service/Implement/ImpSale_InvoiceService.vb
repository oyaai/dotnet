#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpSale_InvoiceService
'	Class Discription	: Implement Sale Invoice Service
'	Create User 		: Suwishaya L.
'	Create Date		    : 02-07-2013
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
#End Region

Namespace Service
    Public Class ImpSale_InvoiceService
        Implements ISale_InvoiceService

        Private objLog As New Common.Logs.Log
        Private objSaleInvoiceEnt As New Entity.ImpSale_InvoiceEntity

#Region "Function"

        '/**************************************************************
        '	Function name	: DeleteSaleInvoice
        '	Discription	    : Delete Sale invoice
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteSaleInvoice( _
            ByVal intRefID As Integer, _
            ByVal dtValues As System.Data.DataTable _
        ) As Boolean Implements ISale_InvoiceService.DeleteSaleInvoice
            ' set default return value
            DeleteSaleInvoice = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteSaleInvoice from Entity
                intEff = objSaleInvoiceEnt.DeleteSaleInvoice(intRefID, dtValues)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteSaleInvoice = True
                Else
                    ' case row less than 1 then return False
                    DeleteSaleInvoice = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteSaleInvoice(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SaveSaleInvoice
        '	Discription	    : Save Sale invoice
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 25-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SaveSaleInvoice( _
            ByVal intRefID As Integer, _
            ByVal decExchangeRate As Decimal, _
            ByVal decActualAmount As Decimal, _
            ByVal strJobOrder As String _
        ) As Boolean Implements ISale_InvoiceService.SaveSaleInvoice
            ' set default return value
            SaveSaleInvoice = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function SaveSaleInvoice from Entity
                intEff = objSaleInvoiceEnt.SaveSaleInvoice(intRefID, decExchangeRate, decActualAmount, strJobOrder)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    SaveSaleInvoice = True
                Else
                    ' case row less than 1 then return False
                    SaveSaleInvoice = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SaveSaleInvoice(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceHeaderByID
        '	Discription	    : Get Sale Invoice header by ID
        '	Return Value	: Sale Invoice dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceHeaderByID( _
            ByVal intRefID As Integer _
        ) As Dto.SaleInvoiceDto Implements ISale_InvoiceService.GetSaleInvoiceHeaderByID
            ' set default return value
            GetSaleInvoiceHeaderByID = New Dto.SaleInvoiceDto
            Try
                ' object for return value from Entity
                Dim objSaleInvEntRet As New Entity.ImpSale_InvoiceEntity
                ' call function GetSaleInvoiceByID from Entity
                objSaleInvEntRet = objSaleInvoiceEnt.GetSaleInvoiceHeaderByID(intRefID)

                ' assign value from Entity to Dto
                With GetSaleInvoiceHeaderByID
                    .id = objSaleInvEntRet.id
                    .invoice_no = objSaleInvEntRet.invoice_no
                    .receipt_date = objSaleInvEntRet.receipt_date
                    .ie_id = objSaleInvEntRet.ie_id
                    .vendor_id = objSaleInvEntRet.vendor_id
                    .account_type = objSaleInvEntRet.account_type
                    .invoice_type = objSaleInvEntRet.invoice_type
                    .bank_fee = objSaleInvEntRet.bank_fee
                    .total_amount = objSaleInvEntRet.total_amount
                    .issue_date = objSaleInvEntRet.issue_date
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceHeaderByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceDetailByID
        '	Discription	    : Get Sale Invoice Detail by ID
        '	Return Value	: Sale Invoice dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceDetailByID( _
            ByVal intRefID As Integer _
        ) As Dto.SaleInvoiceDto Implements ISale_InvoiceService.GetSaleInvoiceDetailByID
            ' set default return value
            GetSaleInvoiceDetailByID = New Dto.SaleInvoiceDto
            Try
                ' object for return value from Entity
                Dim objSaleInvEntRet As New Entity.ImpSale_InvoiceEntity
                ' call function GetSaleInvoiceByID from Entity
                objSaleInvEntRet = objSaleInvoiceEnt.GetSaleInvoiceDetailByID(intRefID)

                ' assign value from Entity to Dto
                With GetSaleInvoiceDetailByID
                    .id = objSaleInvEntRet.id
                    .job_order = objSaleInvEntRet.job_order
                    .job_order_id = objSaleInvEntRet.job_order_id
                    .issue_date = objSaleInvEntRet.issue_date
                    .currency = objSaleInvEntRet.currency
                    .actual_rate = objSaleInvEntRet.actual_rate
                    .currency_id = objSaleInvEntRet.currency_id

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceDetailByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceList
        '	Discription	    : Get Sale Invoice list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceList( _
            ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto _
        ) As System.Data.DataTable Implements ISale_InvoiceService.GetSaleInvoiceList
            ' set default
            GetSaleInvoiceList = New DataTable
            Try
                ' variable for keep list
                Dim listSaleInvoiceEnt As New List(Of Entity.ImpSale_InvoiceEntity)
                ' data row object
                Dim row As DataRow
                Dim strIssueDate As String = ""
                Dim strReceiptDate As String = ""

                ' call function GetSaleInvoiceList from entity
                listSaleInvoiceEnt = objSaleInvoiceEnt.GetSaleInvoiceList(SetDtoToEntity(objSaleInvoiceDto))

                ' assign column header
                With GetSaleInvoiceList
                    .Columns.Add("id")
                    .Columns.Add("invoice_no")
                    .Columns.Add("invoice_type")
                    .Columns.Add("issue_date")
                    .Columns.Add("receipt_date")
                    .Columns.Add("customer")
                    .Columns.Add("total_amount")
                    .Columns.Add("bank_rate")
                    .Columns.Add("actual_amount")
                    .Columns.Add("job_order")
                    .Columns.Add("currency")
                    .Columns.Add("status_id")
                    .Columns.Add("status")

                    ' assign row from listSaleInvoiceEnt
                    For Each values In listSaleInvoiceEnt
                        row = .NewRow
                        row("id") = values.id
                        row("invoice_no") = values.invoice_no
                        row("invoice_type") = values.invoice_type_name
                        row("customer") = values.customer_name

                        If values.issue_date <> Nothing Or values.issue_date <> "" Then
                            strIssueDate = Left(values.issue_date, 4) & "/" & Mid(values.issue_date, 5, 2) & "/" & Right(values.issue_date, 2)
                            row("issue_date") = CDate(strIssueDate).ToString("dd/MMM/yyyy")
                        Else
                            row("issue_date") = values.issue_date
                        End If

                        If values.receipt_date <> Nothing Or values.receipt_date <> "" Then
                            strReceiptDate = Left(values.receipt_date, 4) & "/" & Mid(values.receipt_date, 5, 2) & "/" & Right(values.receipt_date, 2)
                            row("receipt_date") = CDate(strReceiptDate).ToString("dd/MMM/yyyy")
                        Else
                            row("receipt_date") = values.receipt_date
                        End If


                        If values.bank_rate <> Nothing Or values.bank_rate <> "" Then
                            row("bank_rate") = Format(Convert.ToDouble(values.bank_rate.ToString.Trim), "#,##0.00000")
                        Else
                            row("bank_rate") = values.bank_rate
                        End If

                        If values.actual_amount <> Nothing Or values.actual_amount <> "" Then
                            row("actual_amount") = Format(Convert.ToDouble(values.actual_amount.ToString.Trim), "#,##0.00")
                        Else
                            row("actual_amount") = values.actual_amount
                        End If

                        row("total_amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")
                        row("job_order") = values.job_order
                        row("currency") = values.currency
                        row("status_id") = values.status_id
                        row("status") = values.status

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInAccounting
        '	Discription	    : Check sale invoice in used accounting
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInAccounting( _
            ByVal intRefID As Integer _
        ) As Boolean Implements ISale_InvoiceService.IsUsedInAccounting
            ' set default return value
            IsUsedInAccounting = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInAccounting from entity
                intCount = objSaleInvoiceEnt.CountUsedInAccounting(intRefID)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUsedInAccounting = True
                Else
                    ' case equal 0 then return False
                    IsUsedInAccounting = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInAccounting(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto _
        ) As Entity.ISale_InvoiceEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpSale_InvoiceEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    'Receive data from search job order screen 
                    .job_order_from_search = objSaleInvoiceDto.job_order_from_search
                    .job_order_to_search = objSaleInvoiceDto.job_order_to_search
                    .customer_search = objSaleInvoiceDto.customer_search
                    .issue_date_from_search = objSaleInvoiceDto.issue_date_from_search
                    .issue_date_to_search = objSaleInvoiceDto.issue_date_to_search
                    .invoice_no_search = objSaleInvoiceDto.invoice_no_search
                    .invoice_type_search = objSaleInvoiceDto.invoice_type_search
                    .receipt_date_from_search = objSaleInvoiceDto.receipt_date_from_search
                    .receipt_date_to_search = objSaleInvoiceDto.receipt_date_to_search

                    .id = objSaleInvoiceDto.id
                    .invoice_no = objSaleInvoiceDto.invoice_no
                    .receipt_date = objSaleInvoiceDto.receipt_date
                    .issue_date = objSaleInvoiceDto.issue_date
                    .account_title = objSaleInvoiceDto.account_title
                    .customer = objSaleInvoiceDto.customer
                    .account_type = objSaleInvoiceDto.account_type
                    .invoice_type = objSaleInvoiceDto.invoice_type
                    .bank_fee = objSaleInvoiceDto.bank_fee
                    .total_amount = objSaleInvoiceDto.total_amount
                    .account_next_approve = objSaleInvoiceDto.account_next_approve

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceDetailList
        '	Discription	    : Get Sale Invoice detail
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 27-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceDetailList( _
            ByVal intRefID As Integer _
        ) As System.Data.DataTable Implements ISale_InvoiceService.GetSaleInvoiceDetailList
            ' set default
            GetSaleInvoiceDetailList = New DataTable
            Try
                ' variable for keep list
                Dim listSaleInvoiceEnt As New List(Of Entity.ImpSale_InvoiceEntity)
                ' data row object
                Dim row As DataRow
                Dim strIssueDate As String = ""
                Dim strReceiptDate As String = ""

                ' call function GetSaleInvoiceDetailList from entity
                listSaleInvoiceEnt = objSaleInvoiceEnt.GetSaleInvoiceDetailList(intRefID)

                ' assign column header
                With GetSaleInvoiceDetailList
                    .Columns.Add("job_order")
                    .Columns.Add("po_type")
                    .Columns.Add("hontai")
                    .Columns.Add("po_no")
                    .Columns.Add("amount")
                    .Columns.Add("vat")
                    .Columns.Add("wt")
                    .Columns.Add("remark")
                    .Columns.Add("bank_fee")

                    ' assign row from listSaleInvoiceEnt
                    For Each values In listSaleInvoiceEnt
                        row = .NewRow
                        row("job_order") = values.job_order
                        row("po_type") = values.po_type_name
                        row("hontai") = values.hontai
                        row("po_no") = values.po_no
                        row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")
                        row("vat") = values.vat
                        row("wt") = values.wt
                        row("remark") = values.remark
                        row("bank_fee") = values.bank_fee

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceDetailList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceHeaderList
        '	Discription	    : Get Sale Invoice header
        '	Return Value	: Sale Invoice dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceHeaderList( _
            ByVal intRefID As Integer _
        ) As Dto.SaleInvoiceDto Implements ISale_InvoiceService.GetSaleInvoiceHeaderList
            ' set default return value
            GetSaleInvoiceHeaderList = New Dto.SaleInvoiceDto
            Try
                Dim strReceiptDate As String = ""
                Dim strIssueDate As String = ""
                ' object for return value from Entity
                Dim objSaleInvEntRet As New Entity.ImpSale_InvoiceEntity
                ' call function GetSaleInvoiceByID from Entity
                objSaleInvEntRet = objSaleInvoiceEnt.GetSaleInvoiceHeaderList(intRefID)

                ' assign value from Entity to Dto
                With GetSaleInvoiceHeaderList
                    .invoice_no = objSaleInvEntRet.invoice_no

                    If objSaleInvEntRet.receipt_date <> Nothing Or objSaleInvEntRet.receipt_date <> "" Then
                        strReceiptDate = Left(objSaleInvEntRet.receipt_date, 4) & "/" & Mid(objSaleInvEntRet.receipt_date, 5, 2) & "/" & Right(objSaleInvEntRet.receipt_date, 2)
                        .receipt_date = CDate(strReceiptDate).ToString("dd/MMM/yyyy")
                    Else
                        .receipt_date = objSaleInvEntRet.receipt_date
                    End If

                    If objSaleInvEntRet.issue_date <> Nothing Or objSaleInvEntRet.issue_date <> "" Then
                        strIssueDate = Left(objSaleInvEntRet.issue_date, 4) & "/" & Mid(objSaleInvEntRet.issue_date, 5, 2) & "/" & Right(objSaleInvEntRet.issue_date, 2)
                        .issue_date = CDate(strIssueDate).ToString("dd/MMM/yyyy")
                    Else
                        .issue_date = objSaleInvEntRet.issue_date
                    End If

                    .account_title = objSaleInvEntRet.account_title
                    .customer_name = objSaleInvEntRet.customer_name
                    .account_type_name = objSaleInvEntRet.account_type_name
                    .invoice_type_name = objSaleInvEntRet.invoice_type_name
                    .bank_fee_detail = Format(Convert.ToDouble(objSaleInvEntRet.bank_fee.ToString.Trim), "#,##0.00")
                    .amount = Format(Convert.ToDouble(objSaleInvEntRet.amount.ToString.Trim), "#,##0.00")

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceHeaderList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceReportList
        '	Discription	    : Get Sale Invoice report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 27-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceReportList( _
            ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto _
        ) As System.Data.DataTable Implements ISale_InvoiceService.GetSaleInvoiceReportList
            ' set default
            GetSaleInvoiceReportList = New DataTable
            Try
                ' variable for keep list
                Dim listSaleInvoiceEnt As New List(Of Entity.ImpSale_InvoiceEntity)
                ' data row object
                Dim row As DataRow
                Dim strIssueDate As String = ""

                ' call function GetSaleInvoiceReportList from entity
                listSaleInvoiceEnt = objSaleInvoiceEnt.GetSaleInvoiceReportList(SetDtoToEntity(objSaleInvoiceDto))

                ' assign column header
                With GetSaleInvoiceReportList
                    .Columns.Add("invoice_no")
                    .Columns.Add("issue_date")
                    .Columns.Add("job_order")
                    .Columns.Add("customer")
                    .Columns.Add("po_no")
                    .Columns.Add("stage")
                    .Columns.Add("percent")
                    .Columns.Add("actual_rate")
                    .Columns.Add("price")
                    .Columns.Add("vat")
                    .Columns.Add("amount")
                    .Columns.Add("remark")

                    ' assign row from listSaleInvoiceEnt
                    For Each values In listSaleInvoiceEnt
                        row = .NewRow

                        row("invoice_no") = values.invoice_no
                        If values.issue_date <> Nothing Or values.issue_date <> "" Then
                            strIssueDate = Left(values.issue_date, 4) & "/" & Mid(values.issue_date, 5, 2) & "/" & Right(values.issue_date, 2)
                            row("issue_date") = CDate(strIssueDate).ToString("dd/MMM/yyyy")
                        Else
                            row("issue_date") = values.issue_date
                        End If

                        row("job_order") = values.job_order
                        row("customer") = values.customer_name
                        row("po_no") = values.po_no
                        row("stage") = values.stage
                        row("percent") = values.percent
                        row("price") = Format(Convert.ToDouble(values.price.ToString.Trim), "#,##0.00")
                        row("vat") = Format(Convert.ToDouble(values.vat.ToString.Trim), "#,##0.00")
                        row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")
                        If values.amount = "0" Then
                            row("actual_rate") = Format(Convert.ToDouble("0"), "#,##0.00000")
                        Else
                            row("actual_rate") = Format(Convert.ToDouble(values.actual_rate.ToString.Trim), "#,##0.00000")
                        End If
                        row("remark") = values.remark

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceReportList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumSaleInvoiceReportList
        '	Discription	    : Get sum Sale Invoice report
        '	Return Value	: Sale Invoice dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumSaleInvoiceReportList( _
            ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto _
        ) As Dto.SaleInvoiceDto Implements ISale_InvoiceService.GetSumSaleInvoiceReportList
            ' set default return value
            GetSumSaleInvoiceReportList = New Dto.SaleInvoiceDto
            Try
                Dim strReceiptDate As String = ""
                Dim strIssueDate As String = ""
                ' object for return value from Entity
                Dim objSaleInvEntRet As New Entity.ImpSale_InvoiceEntity
                ' call function GetSumSaleInvoiceReportList from Entity
                objSaleInvEntRet = objSaleInvoiceEnt.GetSumSaleInvoiceReportList(SetDtoToEntity(objSaleInvoiceDto))

                ' assign value from Entity to Dto
                With GetSumSaleInvoiceReportList
                    .sum_amount = Format(Convert.ToDouble(objSaleInvEntRet.sum_amount.ToString.Trim), "#,##0.00")
                    .sum_price = Format(Convert.ToDouble(objSaleInvEntRet.sum_price.ToString.Trim), "#,##0.00")
                    .sum_vat = Format(Convert.ToDouble(objSaleInvEntRet.sum_vat.ToString.Trim), "#,##0.00")
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumSaleInvoiceReportList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetTotalSaleInvoiceAmount
        '	Discription	    : Get Total Sale Invoice Amount
        '	Return Value	: Sale Invoice dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTotalSaleInvoiceAmount( _
            ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto _
        ) As Dto.SaleInvoiceDto Implements ISale_InvoiceService.GetTotalSaleInvoiceAmount
            ' set default return value
            GetTotalSaleInvoiceAmount = New Dto.SaleInvoiceDto
            Try
                Dim strReceiptDate As String = ""
                Dim strIssueDate As String = ""
                ' object for return value from Entity
                Dim objSaleInvEntRet As New Entity.ImpSale_InvoiceEntity
                ' call function GetTotalSaleInvoiceAmount from Entity
                objSaleInvEntRet = objSaleInvoiceEnt.GetTotalSaleInvoiceAmount(SetDtoToEntity(objSaleInvoiceDto))

                ' assign value from Entity to Dto
                With GetTotalSaleInvoiceAmount
                    .total_invoice_amount = Format(Convert.ToDouble(objSaleInvEntRet.total_invoice_amount.ToString.Trim), "#,##0.00")

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetTotalSaleInvoiceAmount(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: ConfirmReceive
        '	Discription	    : Confirm Receive
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 26-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function ConfirmReceive( _
            ByVal invoiceHeaderId As String, _
            ByVal dtValues As System.Data.DataTable, _
            Optional ByVal dtBankFree As System.Data.DataTable = Nothing _
        ) As Boolean Implements ISale_InvoiceService.ConfirmReceive
            ' set default return value
            ConfirmReceive = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function ConfirmReceive from Entity
                intEff = objSaleInvoiceEnt.ConfirmReceive(invoiceHeaderId, dtValues, dtBankFree)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    ConfirmReceive = True
                Else
                    ' case row less than 1 then return False
                    ConfirmReceive = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("ConfirmReceive(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrerSaleInvoiceDetail
        '	Discription	    : Get Sale Invoice detail
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrerSaleInvoiceDetail( _
            ByVal strJobOrder As String _
        ) As System.Data.DataTable Implements ISale_InvoiceService.GetJobOrerSaleInvoiceDetail
            ' set default
            GetJobOrerSaleInvoiceDetail = New DataTable
            Try
                ' variable for keep list
                Dim listSaleInvoiceEnt As New List(Of Entity.ImpSale_InvoiceEntity)
                ' data row object
                Dim row As DataRow
                Dim strPODate As String = ""

                ' call function GetJobOrerSaleInvoiceDetail from entity
                listSaleInvoiceEnt = objSaleInvoiceEnt.GetJobOrerSaleInvoiceDetail(strJobOrder)


                ' assign column header
                With GetJobOrerSaleInvoiceDetail
                    .Columns.Add("id")
                    .Columns.Add("job_order_id")
                    .Columns.Add("job_order")
                    .Columns.Add("customer")
                    .Columns.Add("currency_id")
                    .Columns.Add("po_no")
                    .Columns.Add("po_type")
                    .Columns.Add("po_type_name")
                    .Columns.Add("hontai")
                    .Columns.Add("amount")
                    .Columns.Add("po_date")
                    .Columns.Add("hontai_type")
                    .Columns.Add("hontai_fg")
                    .Columns.Add("hontai_cond")
                    .Columns.Add("job_type")

                    ' assign row from listSaleInvoiceEnt
                    For Each values In listSaleInvoiceEnt
                        row = .NewRow
                        row("id") = values.id
                        row("job_order_id") = values.job_order_id
                        row("job_order") = values.job_order
                        row("customer") = values.customer
                        row("currency_id") = values.currency_id
                        row("po_no") = values.po_no
                        row("po_type") = values.po_type
                        row("po_type_name") = values.po_type_name
                        row("hontai") = values.hontai
                        row("hontai_type") = values.hontai_type
                        row("hontai_fg") = values.hontai_fg
                        row("hontai_cond") = values.hontai_cond
                        row("job_type") = values.job_type
                        row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")

                        If values.po_date <> Nothing Or values.po_date <> "" Then
                            strPODate = Left(values.po_date, 4) & "/" & Mid(values.po_date, 5, 2) & "/" & Right(values.po_date, 2)
                            row("po_date") = CDate(strPODate).ToString("dd/MMM/yyyy")
                        Else
                            row("po_date") = values.po_date
                        End If

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrerSaleInvoiceDetail(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetJobOrerSaleInvoiceDetailEdit
        '	Discription	    : Get Sale Invoice detail
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrerSaleInvoiceDetailEdit( _
            ByVal intRefID As Integer _
        ) As System.Data.DataTable Implements ISale_InvoiceService.GetJobOrerSaleInvoiceDetailEdit
            ' set default
            GetJobOrerSaleInvoiceDetailEdit = New DataTable
            Try
                ' variable for keep list
                Dim listSaleInvoiceEnt As New List(Of Entity.ImpSale_InvoiceEntity)
                ' data row object
                Dim row As DataRow
                Dim strPODate As String = ""
                ' call function GetJobOrerSaleInvoiceDetail from entity
                listSaleInvoiceEnt = objSaleInvoiceEnt.GetJobOrerSaleInvoiceDetailEdit(intRefID)

                ' assign column header
                With GetJobOrerSaleInvoiceDetailEdit
                    .Columns.Add("id")
                    .Columns.Add("receive_header_id")
                    .Columns.Add("job_order_po_id")
                    .Columns.Add("job_order_id")
                    .Columns.Add("po_no")
                    .Columns.Add("po_type_name")
                    .Columns.Add("hontai")
                    .Columns.Add("amount_thb")
                    .Columns.Add("actual_rate")
                    .Columns.Add("po_date")
                    .Columns.Add("vat_id")
                    .Columns.Add("wt_id")
                    .Columns.Add("remark")
                    .Columns.Add("po_type")
                    .Columns.Add("hontai_type")
                    .Columns.Add("amount")
                    .Columns.Add("hontai_fg1")
                    .Columns.Add("hontai_fg2")
                    .Columns.Add("hontai_fg3")
                    .Columns.Add("po_fg")
                    .Columns.Add("job_order")
                    .Columns.Add("bank_fee")
                    .Columns.Add("hontai_cond")
                    .Columns.Add("job_type")
                    ' assign row from listSaleInvoiceEnt
                    For Each values In listSaleInvoiceEnt
                        row = .NewRow
                        row("id") = values.id
                        row("job_order_po_id") = values.job_order_po_id
                        row("receive_header_id") = values.receive_header_id
                        row("job_order_id") = values.job_order_id
                        row("po_no") = values.po_no
                        row("po_type_name") = values.po_type_name
                        row("hontai") = values.hontai
                        row("amount") = values.total_amount
                        row("amount_thb") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")
                        row("actual_rate") = values.actual_rate
                        row("vat_id") = values.vat_id
                        row("wt_id") = values.wt_id
                        row("remark") = values.remark
                        row("po_type") = values.po_type
                        row("hontai_type") = values.hontai_type

                        If values.po_date <> Nothing Or values.po_date <> "" Then
                            strPODate = Left(values.po_date, 4) & "/" & Mid(values.po_date, 5, 2) & "/" & Right(values.po_date, 2)
                            row("po_date") = CDate(strPODate).ToString("dd/MMM/yyyy")
                        Else
                            row("po_date") = values.po_date
                        End If

                        row("hontai_fg1") = values.hontai_fg1
                        row("hontai_fg2") = values.hontai_fg2
                        row("hontai_fg3") = values.hontai_fg3
                        row("po_fg") = values.po_fg
                        row("job_order") = values.job_order
                        row("bank_fee") = values.bank_fee
                        row("hontai_cond") = values.hontai_cond
                        row("job_type") = values.job_type

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrerSaleInvoiceDetailEdit(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInReceiveHeader
        '	Discription	    : Check Sale Invoice No. in used receive_header 
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 31-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInReceiveHeader( _
            ByVal strInvoice_no As String _
        ) As Boolean Implements ISale_InvoiceService.IsUsedInReceiveHeader
            ' set default return value
            IsUsedInReceiveHeader = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInReceiveHeader from entity
                intCount = objSaleInvoiceEnt.CountUsedInReceiveHeader(strInvoice_no)

                ' check count used
                If intCount > 0 Then
                    ' case not equal 0 then return True
                    IsUsedInReceiveHeader = True
                Else
                    ' case equal 0 then return False
                    IsUsedInReceiveHeader = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInReceiveHeader(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInJobOrder
        '	Discription	    : Check  Job Order in used Job_order 
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 31-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInJobOrder( _
            ByVal strJobOrder As String _
        ) As Boolean Implements ISale_InvoiceService.IsUsedInJobOrder
            ' set default return value
            IsUsedInJobOrder = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInJoborder from entity
                intCount = objSaleInvoiceEnt.CountUsedInJobOrder(strJobOrder)

                ' check count used
                If intCount > 0 Then
                    ' case not equal 0 then return True
                    IsUsedInJobOrder = True
                Else
                    ' case equal 0 then return False
                    IsUsedInJobOrder = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsCustomerUsedInJobOrder
        '	Discription	    : Check customer in used Job_order 
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 31-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsCustomerUsedInJobOrder( _
            ByVal strJobOrder As String, _
            ByVal intCustomer As Integer _
        ) As Boolean Implements ISale_InvoiceService.IsCustomerUsedInJobOrder
            ' set default return value
            IsCustomerUsedInJobOrder = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountCustomerUsedInJobOrder from entity
                intCount = objSaleInvoiceEnt.CountCustomerUsedInJobOrder(strJobOrder, intCustomer)

                ' check count used
                If intCount > 0 Then
                    ' case not equal 0 then return True
                    IsCustomerUsedInJobOrder = True
                Else
                    ' case equal 0 then return False
                    IsCustomerUsedInJobOrder = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsCustomerUsedInJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceByJobOrder
        '	Discription	    : Get Sale Invoice Detail by Job order
        '	Return Value	: Sale Invoice dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceByJobOrder( _
            ByVal strJobOrder As String _
        ) As Dto.SaleInvoiceDto Implements ISale_InvoiceService.GetSaleInvoiceByJobOrder
            ' set default return value
            GetSaleInvoiceByJobOrder = New Dto.SaleInvoiceDto
            Try
                ' object for return value from Entity
                Dim objSaleInvEntRet As New Entity.ImpSale_InvoiceEntity
                ' call function GetSaleInvoiceByJobOrder from Entity
                objSaleInvEntRet = objSaleInvoiceEnt.GetSaleInvoiceByJobOrder(strJobOrder)

                ' assign value from Entity to Dto
                With GetSaleInvoiceByJobOrder
                    .id = objSaleInvEntRet.id
                    .job_order = objSaleInvEntRet.job_order
                    .job_order_id = objSaleInvEntRet.job_order_id
                    .issue_date = objSaleInvEntRet.issue_date
                    .currency = objSaleInvEntRet.currency
                    .actual_rate = objSaleInvEntRet.actual_rate
                    .currency_id = objSaleInvEntRet.currency_id

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceByJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertSaleInvoice
        '	Discription	    : Insert Sale Invoice
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 01-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertSaleInvoice( _
            ByVal strJobOrder1 As String, ByVal strJobOrder2 As String, ByVal strJobOrder3 As String, _
            ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto, _
            ByVal dtValues As System.Data.DataTable _
        ) As Boolean Implements ISale_InvoiceService.InsertSaleInvoice
            ' set default return value
            InsertSaleInvoice = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertSaleInvoice from Entity
                intEff = objSaleInvoiceEnt.InsertSaleInvoice(strJobOrder1, strJobOrder2, strJobOrder3, SetDtoToEntity(objSaleInvoiceDto), dtValues)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertSaleInvoice = True
                Else
                    ' case row less than 1 then return False
                    InsertSaleInvoice = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertSaleInvoice(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateSaleInvoice
        '	Discription	    : Update Sale Invoice
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 01-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateSaleInvoice( _
            ByVal strIdDelete As String, _
            ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto, _
            ByVal dtValues As System.Data.DataTable _
        ) As Boolean Implements ISale_InvoiceService.UpdateSaleInvoice
            ' set default return value
            UpdateSaleInvoice = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateSaleInvoice from Entity
                intEff = objSaleInvoiceEnt.UpdateSaleInvoice(strIdDelete, SetDtoToEntity(objSaleInvoiceDto), dtValues)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateSaleInvoice = True
                Else
                    ' case row less than 1 then return False
                    UpdateSaleInvoice = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateSaleInvoice(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderHontai
        '	Discription	    : Get Job Order for update hontai flag
        '	Return Value	: Sale Invoice dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderHontai( _
            ByVal dtValues As System.Data.DataTable _
        ) As Dto.SaleInvoiceDto Implements ISale_InvoiceService.GetJobOrderHontai
            ' set default return value
            GetJobOrderHontai = New Dto.SaleInvoiceDto
            Try
                ' object for return value from Entity
                Dim objSaleInvEntRet As New Entity.ImpSale_InvoiceEntity
                ' call function GetJobOrderHontai from Entity
                objSaleInvEntRet = objSaleInvoiceEnt.GetJobOrderHontai(dtValues)

                ' assign value from Entity to Dto
                With GetJobOrderHontai
                    .strJobOrder1 = objSaleInvEntRet.strJobOrder1
                    .strJobOrder2 = objSaleInvEntRet.strJobOrder2
                    .strJobOrder3 = objSaleInvEntRet.strJobOrder3
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderHontai(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetHontaiFinish
        '	Discription	    : Get hontai for update finish flag
        '	Return Value	: Sale Invoice dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 17-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetHontaiFinish( _
           ByVal intRefID As Integer _
        ) As Dto.SaleInvoiceDto Implements ISale_InvoiceService.GetHontaiFinish
            ' set default return value
            GetHontaiFinish = New Dto.SaleInvoiceDto
            Try
                ' object for return value from Entity
                Dim objSaleInvEntRet As New Entity.ImpSale_InvoiceEntity
                ' call function GetJobOrderHontai from Entity
                objSaleInvEntRet = objSaleInvoiceEnt.GetHontaiFinish(intRefID)

                ' assign value from Entity to Dto
                With GetHontaiFinish
                    .strJobOrder1 = objSaleInvEntRet.strJobOrder1
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetHontaiFinish(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInHontai
        '	Discription	    : Check hontai flag in Job_order_po 
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 31-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInHontai( _
            ByVal intJobOrder As Integer, _
            ByVal intHontaiFlg As Integer _
        ) As Boolean Implements ISale_InvoiceService.IsUsedInHontai
            ' set default return value
            IsUsedInHontai = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInHontai from entity
                intCount = objSaleInvoiceEnt.CountUsedInHontai(intJobOrder, intHontaiFlg)

                ' check count used
                If intCount > 0 Then
                    ' case not equal 0 then return True
                    IsUsedInHontai = True
                Else
                    ' case equal 0 then return False
                    IsUsedInHontai = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInHontai(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAccountTitleForList
        '	Discription	    : Get Account title for dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 08-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountTitleForList() As System.Collections.Generic.List(Of Dto.SaleInvoiceDto) Implements ISale_InvoiceService.GetAccountTitleForList

            ' set default list
            GetAccountTitleForList = New List(Of Dto.SaleInvoiceDto)
            Try
                ' objAccountTitleDto for keep value Dto 
                Dim objAccountTitleDto As Dto.SaleInvoiceDto
                ' listAccountTitleEnt for keep value from entity
                Dim listAccountTitleEnt As New List(Of Entity.ISale_InvoiceEntity)
                ' objAccountTileEnt for call function
                Dim objAccountTileEnt As New Entity.ImpSale_InvoiceEntity

                ' call function GetAccountTitleForList
                listAccountTitleEnt = objAccountTileEnt.GetAccountTitleForList()

                ' loop listAccountTitleEnt for assign value to Dto
                For Each values In listAccountTitleEnt
                    ' new object
                    objAccountTitleDto = New Dto.SaleInvoiceDto
                    ' assign value to Dto
                    With objAccountTitleDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetAccountTitleForList.Add(objAccountTitleDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccountTitleForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateJobOrderPOFlag
        '	Discription	    : Update flag on job_order_po
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 22-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateJobOrderPOFlag( _
            ByVal dtValues As System.Data.DataTable _
        ) As Boolean Implements ISale_InvoiceService.UpdateJobOrderPOFlag
            ' set default return value
            UpdateJobOrderPOFlag = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateJobOrderPOFlag from Entity
                intEff = objSaleInvoiceEnt.UpdateJobOrderPOFlag(dtValues)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateJobOrderPOFlag = True
                Else
                    ' case row less than 1 then return False
                    UpdateJobOrderPOFlag = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateJobOrderPOFlag(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteJobOrderPOFlag
        '	Discription	    : Update flag on job_order_po
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 22-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobOrderPOFlag( _
            ByVal dtValues As System.Data.DataTable _
        ) As Boolean Implements ISale_InvoiceService.DeleteJobOrderPOFlag
            ' set default return value
            DeleteJobOrderPOFlag = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' call function DeleteJobOrderPOFlag from Entity
                intEff = objSaleInvoiceEnt.DeleteJobOrderPOFlag(dtValues)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteJobOrderPOFlag = True
                Else
                    ' case row less than 1 then return False
                    DeleteJobOrderPOFlag = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobOrderPOFlag(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDataBankFreeList
        '	Discription	    : Get data for update bank free
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 26-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDataBankFreeList( _
            ByVal strReceive_header_id As String _
        ) As System.Data.DataTable Implements ISale_InvoiceService.GetDataBankFreeList
            ' set default
            GetDataBankFreeList = New DataTable
            Try
                ' variable for keep list
                Dim listSaleInvoiceEnt As New List(Of Entity.ImpSale_InvoiceEntity)
                ' data row object
                Dim row As DataRow
                Dim strIssueDate As String = ""
                Dim strReceiptDate As String = ""

                ' call function GetDataBankFreeList from entity
                listSaleInvoiceEnt = objSaleInvoiceEnt.GetDataBankFreeList(strReceive_header_id)

                ' assign column header
                With GetDataBankFreeList
                    .Columns.Add("receive_header_id")
                    .Columns.Add("vendor_id")
                    .Columns.Add("receipt_date")
                    .Columns.Add("job_order")
                    .Columns.Add("vat_id")
                    .Columns.Add("wt_id")
                    .Columns.Add("ie_id")
                    .Columns.Add("vat_amount")
                    .Columns.Add("wt_amount")
                    .Columns.Add("sub_total")
                    .Columns.Add("actual_rate")
                    .Columns.Add("remark")
                    ' assign row from listSaleInvoiceEnt
                    For Each values In listSaleInvoiceEnt
                        row = .NewRow
                        row("receive_header_id") = values.receive_header_id
                        row("vendor_id") = values.vendor_id
                        row("receipt_date") = values.receipt_date
                        row("job_order") = values.job_order
                        row("vat_id") = values.vat_id
                        row("wt_id") = values.wt_id
                        row("ie_id") = values.ie_id
                        row("vat_amount") = values.vat_amount
                        row("wt_amount") = values.wt_amount
                        row("sub_total") = values.sub_total
                        row("actual_rate") = values.actual_rate
                        row("remark") = values.remark
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDataBankFreeList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDataReceiveDetail
        '	Discription	    : Get data for update job_order_po
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDataReceiveDetail( _
            ByVal strReceive_header_id As String _
        ) As System.Data.DataTable Implements ISale_InvoiceService.GetDataReceiveDetail
            ' set default
            GetDataReceiveDetail = New DataTable
            Try
                ' variable for keep list
                Dim listSaleInvoiceEnt As New List(Of Entity.ImpSale_InvoiceEntity)
                ' data row object
                Dim row As DataRow
                Dim strIssueDate As String = ""
                Dim strReceiptDate As String = ""

                ' call function GetDataBankFreeList from entity
                listSaleInvoiceEnt = objSaleInvoiceEnt.GetDataReceiveDetail(strReceive_header_id)

                ' assign column header
                With GetDataReceiveDetail
                    .Columns.Add("id")
                    .Columns.Add("job_order_id")
                    .Columns.Add("job_order_po_id")
                    .Columns.Add("hontai_type")

                    ' assign row from listSaleInvoiceEnt
                    For Each values In listSaleInvoiceEnt
                        row = .NewRow
                        row("id") = values.id
                        row("job_order_id") = values.job_order_id
                        row("job_order_po_id") = values.job_order_po_id
                        row("hontai_type") = values.hontai_type

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDataReceiveDetail(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetConfirmReceiveForReport
        '	Discription	    : Get data from job order income report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetConfirmReceiveForReport( _
            ByVal strReceiceHeaderId As String _
        ) As System.Data.DataTable Implements ISale_InvoiceService.GetConfirmReceiveForReport
            ' set default
            GetConfirmReceiveForReport = New DataTable
            Try
                ' variable for keep list
                Dim listSaleInvoiceEnt As New List(Of Entity.ImpSale_InvoiceEntity)
                ' data row object
                Dim row As DataRow
                Dim strReceiveDate As String = ""

                ' call function GetConfirmReceiveForReport from entity
                listSaleInvoiceEnt = objSaleInvoiceEnt.GetConfirmReceiveForReport(strReceiceHeaderId)

                ' assign column header
                With GetConfirmReceiveForReport
                    .Columns.Add("receive_header_id")
                    .Columns.Add("receive_detail_id")
                    .Columns.Add("job_order_id")
                    .Columns.Add("job_order")
                    .Columns.Add("customer_name")
                    .Columns.Add("vat_amount_thb")
                    .Columns.Add("wt_amount_thb")
                    .Columns.Add("amount_thb")
                    .Columns.Add("invoice_no")
                    .Columns.Add("receipt_date")
                    .Columns.Add("user_login")
                    .Columns.Add("bank_fee")

                    ' assign row from listSaleInvoiceEnt
                    For Each values In listSaleInvoiceEnt
                        row = .NewRow

                        row("receive_header_id") = values.receive_header_id
                        row("receive_detail_id") = values.receive_detail_id
                        row("job_order_id") = values.job_order_id
                        row("job_order") = values.job_order
                        row("customer_name") = values.customer_name
                        row("vat_amount_thb") = values.vat_amount
                        row("wt_amount_thb") = values.wt_amount
                        row("amount_thb") = values.amount
                        row("invoice_no") = values.invoice_no
                        If values.receipt_date <> Nothing Or values.receipt_date <> "" Then
                            strReceiveDate = Left(values.receipt_date, 4) & "/" & Mid(values.receipt_date, 5, 2) & "/" & Right(values.receipt_date, 2)
                            row("receipt_date") = CDate(strReceiveDate).ToString("dd/MMM/yyyy")
                        Else
                            row("receipt_date") = values.receipt_date
                        End If

                        row("user_login") = values.name
                        If values.bank_fee <> Nothing Or values.bank_fee <> "" Then
                            row("bank_fee") = "0.00"
                        Else
                            row("bank_fee") = values.bank_fee
                        End If
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetConfirmReceiveForReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumConfirmReport
        '	Discription	    : Get sum vat /sum w/t for job order income report
        '	Return Value	: Sale Invoice dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumConfirmReport( _
            ByVal strReceiceHeaderId As String _
        ) As Dto.SaleInvoiceDto Implements ISale_InvoiceService.GetSumConfirmReport
            ' set default return value
            GetSumConfirmReport = New Dto.SaleInvoiceDto
            Try
                Dim strReceiptDate As String = ""
                Dim strIssueDate As String = ""
                ' object for return value from Entity
                Dim objSaleInvEntRet As New Entity.ImpSale_InvoiceEntity
                ' call function GetSumConfirmReport from Entity
                objSaleInvEntRet = objSaleInvoiceEnt.GetSumConfirmReport(strReceiceHeaderId)

                ' assign value from Entity to Dto
                With GetSumConfirmReport
                    .sum_wt = objSaleInvEntRet.sum_wt.ToString.Trim
                    .sum_vat = objSaleInvEntRet.sum_vat.ToString.Trim
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumConfirmReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetTableConfirmReport
        '	Discription	    : Get table report
        '	Return Value	: Datatable
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTableConfirmReport( _
            ByVal dtValues As System.Data.DataTable, _
            ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto, _
            ByVal strVoucherNo As String, _
            ByVal dtValuesBankFree As Dto.SaleInvoiceDto _
        ) As System.Data.DataTable Implements ISale_InvoiceService.GetTableConfirmReport
            ' set default return value
            GetTableConfirmReport = New DataTable
            Try
                Dim dtReport As New DataTable
                Dim dr As DataRow

                ' set header columns
                With dtReport
                    .Columns.Add("receive_header_id")
                    .Columns.Add("receive_detail_id")
                    .Columns.Add("job_order_id")
                    .Columns.Add("job_order")
                    .Columns.Add("customer_name")
                    .Columns.Add("amount_thb")
                    .Columns.Add("invoice_no")
                    .Columns.Add("receipt_date")
                    .Columns.Add("voucher_no")
                    .Columns.Add("user_login")
                    .Columns.Add("sum_vat")
                    .Columns.Add("sum_wt")
                    .Columns.Add("bank_fee")
                End With

                ' loop set data to table report
                For Each values As DataRow In dtValues.Rows
                    dr = dtReport.NewRow

                    dr.Item("receive_header_id") = values.Item("receive_header_id")
                    dr.Item("receive_detail_id") = values.Item("receive_detail_id")
                    dr.Item("job_order_id") = values.Item("job_order_id")
                    dr.Item("job_order") = values.Item("job_order")
                    dr.Item("customer_name") = values.Item("customer_name")
                    dr.Item("amount_thb") = values.Item("amount_thb")
                    dr.Item("invoice_no") = values.Item("invoice_no")
                    dr.Item("receipt_date") = values.Item("receipt_date")
                    dr.Item("voucher_no") = strVoucherNo
                    dr.Item("user_login") = values.Item("user_login")
                    dr.Item("sum_vat") = CDec(values.Item("vat_amount_thb")).ToString("#,##0.00")
                    dr.Item("sum_wt") = CDec(values.Item("wt_amount_thb")).ToString("#,##0.00")
                    dr.Item("bank_fee") = CDec(dtValuesBankFree.bank_fee).ToString("#,##0.00")
                    dtReport.Rows.Add(dr)
                Next
                ' return new datatable
                Return dtReport
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetTableConfirmReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateSaleInvoice
        '	Discription	    : Get table report
        '	Return Value	: Datatable
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateReciveDetail(ByVal tbIntIdJBPO As DataTable _
             , ByVal objIDSaleInv As Dto.SaleInvoiceDto _
             , ByVal intJobOrderID As Integer) _
        As Boolean Implements ISale_InvoiceService.UpdateReciveDetail
            Try
                UpdateReciveDetail = False

                'Dim objJobOrder As DataTable
                UpdateReciveDetail = objSaleInvoiceEnt.UpdateReciveDetail(tbIntIdJBPO, objIDSaleInv, intJobOrderID)
                If UpdateReciveDetail = True Then

                End If


            Catch ex As Exception
                objLog.ErrorLog("UpdateReciveDetail(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SumBankfeeConfirmReport
        '	Discription	    : Get Bank Fee 
        '	Return Value	: Sale Invoice dto object
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 07-05-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SumBankfeeConfirmReport( _
            ByVal strReceiceHeaderId As String _
        ) As Dto.SaleInvoiceDto Implements ISale_InvoiceService.SumBankfeeConfirmReport
            ' set default return value
            SumBankfeeConfirmReport = New Dto.SaleInvoiceDto
            Try
                Dim strReceiptDate As String = ""
                Dim strIssueDate As String = ""
                ' object for return value from Entity
                Dim objSaleInvEntRet As New Entity.ImpSale_InvoiceEntity
                ' call function GetSumConfirmReport from Entity
                objSaleInvEntRet = objSaleInvoiceEnt.SumBankfeeConfirmReport(strReceiceHeaderId)

                ' assign value from Entity to Dto
                With SumBankfeeConfirmReport
                    .bank_fee = objSaleInvEntRet.bank_fee.ToString.Trim
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SumBankfeeConfirmReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceforUpdate
        '	Discription	    : Get Sale Invoice Detail for support Update Sale Invoice When Edit Job Order
        '	Return Value	: Sale Invoice dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceforUpdate( _
            ByVal strJobOrder As String _
        ) As Dto.SaleInvoiceDto Implements ISale_InvoiceService.GetSaleInvoiceforUpdate
            ' set default return value
            GetSaleInvoiceforUpdate = New Dto.SaleInvoiceDto
            Try
                ' object for return value from Entity
                Dim objSaleInvEntRet As New Entity.ImpSale_InvoiceEntity
                ' call function GetSaleInvoiceByJobOrder from Entity
                objSaleInvEntRet = objSaleInvoiceEnt.GetSaleInvoiceforUpdate(strJobOrder)

                ' assign value from Entity to Dto
                With GetSaleInvoiceforUpdate
                    .id = objSaleInvEntRet.id
                    .invoice_no = objSaleInvEntRet.invoice_no
                    .hontai_type = objSaleInvEntRet.hontai_type

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceforUpdate(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: NewExChangeRate
        '	Discription	    : 
        '	Return Value	: ExChangeRate
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 11-08-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function NewExChangeRate(ByVal strReceiveHeaderId As String, ByVal ExchangeRate As Integer) As Boolean Implements ISale_InvoiceService.NewExChangeRate
            NewExChangeRate = False
            Try
                ' object for return value from Entity
                Dim objSaleInvEntRet As Boolean
                ' call function GetSaleInvoiceByJobOrder from Entity
                objSaleInvEntRet = objSaleInvoiceEnt.UpExChangeRate(strReceiveHeaderId, ExchangeRate)

                ' assign value from Entity to Dto
                If Not IsNothing(objSaleInvEntRet) Then
                    NewExChangeRate = True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("NewExChangeRate(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetExChangeRate
        '	Discription	    : 
        '	Return Value	: ExChangeRate
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 12-09-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Function GetExChangeRate(ByVal intID As Integer) As Integer Implements ISale_InvoiceService.GetExChangeRate
            GetExChangeRate = 0
            Try
                Dim intExChangeRate As Integer
                intExChangeRate = objSaleInvoiceEnt.GetActualRate(intID)


                ' assign value from Entity to Dto
                'With GetExChangeRate
                '    '.id = intExChangeRate.id
                '    .actual_rate = intExChangeRate.actual_rate
                'End With

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetExChangeRate(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: UpdateExChangeRate
        '	Discription	    : 
        '	Return Value	: ExChangeRate
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 12-09-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateExChangeRate(ByVal strReceiveHeaderId As String, _
                                           ByVal objExChangeRate As System.Data.DataTable _
                                           ) As Boolean Implements ISale_InvoiceService.UpdateExChangeRate

            UpdateExChangeRate = False
            Try
                Dim intExChangeRate As Integer
                intExChangeRate = objSaleInvoiceEnt.UpdateExChangeRate(strReceiveHeaderId, objExChangeRate)


                ' assign value from Entity to Dto
                'With GetExChangeRate
                '    '.id = intExChangeRate.id
                '    .actual_rate = intExChangeRate.actual_rate
                'End With

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetExChangeRate(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function


#End Region

    End Class
End Namespace
