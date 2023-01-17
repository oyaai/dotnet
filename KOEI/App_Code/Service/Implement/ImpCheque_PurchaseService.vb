#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpCheque_PurchaseService
'	Class Discription	: Implement Rating Purchase Service
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

Imports Microsoft.VisualBasic
Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Globalization
Imports OfficeOpenXml.Style
Imports OfficeOpenXml
Imports System.IO

Namespace Service
    Public Class ImpCheque_PurchaseService
        Implements ICheque_PurchaseService

        Private objLog As New Common.Logs.Log
        Private objChequePurchaseEnt As New Entity.ImpCheque_PurchaseEntity

#Region "Function"
        '/**************************************************************
        '	Function name	: GetCheque_PurchaseList
        '	Discription	    : Get GetCheque_PurchaseList
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCheque_PurchaseList( _
            ByVal objRatingDto As Dto.Cheque_PurchaseDto _
        ) As System.Data.DataTable Implements ICheque_PurchaseService.GetCheque_PurchaseList
            ' set default
            GetCheque_PurchaseList = New DataTable
            Try
                ' variable for keep list from Rating Purchase entity
                Dim listCheque_PurchaseEnt As New List(Of Entity.ImpCheque_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listCheque_PurchaseEnt = objChequePurchaseEnt.GetCheque_PurchaseList(SetDtoToEntity(objRatingDto))

                ' assign column header
                With GetCheque_PurchaseList
                    .Columns.Add("id")
                    .Columns.Add("voucher_no")
                    .Columns.Add("cheque_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("vendor_type")
                    .Columns.Add("cheque_date")

                    ' assign row from listAccountingEny
                    For Each values In listCheque_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id
                        row("voucher_no") = values.voucher_no
                        row("cheque_no") = values.cheque_no
                        row("vendor_name") = values.vendor_name
                        row("vendor_type") = values.vendor_type
                        If IsNothing(values.cheque_date) = False Then
                            row("cheque_date") = CDate(values.cheque_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("cheque_date") = ""
                        End If

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCheque_PurchaseList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetCheque_Head
        '	Discription	    : Get Cheque Head
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCheque_Head( _
            ByVal strChequeNo As String, _
            ByVal strChequeDate As String _
        ) As System.Data.DataTable Implements ICheque_PurchaseService.GetCheque_Head
            ' set default
            GetCheque_Head = New DataTable
            Try
                ' variable for keep list from Rating Purchase entity
                Dim listCheque_PurchaseEnt As New List(Of Entity.ImpCheque_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listCheque_PurchaseEnt = objChequePurchaseEnt.GetCheque_Head(strChequeNo, strChequeDate)

                ' assign column header
                With GetCheque_Head
                    .Columns.Add("cheque_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("vendor_type")
                    .Columns.Add("cheque_date")

                    ' assign row from listAccountingEny
                    For Each values In listCheque_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("cheque_no") = values.cheque_no
                        row("vendor_name") = values.vendor_name
                        row("vendor_type") = values.vendor_type
                        If IsNothing(values.cheque_date) = False Then
                            row("cheque_date") = CDate(values.cheque_date.ToString()).ToString("dd-MMM-yyyy")
                        Else
                            row("cheque_date") = ""
                        End If

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCheque_Head(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetCheque_Head
        '	Discription	    : Get Cheque Head
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCheque_Detail( _
            ByVal strChequeNo As String, _
            ByVal strChequeDate As String _
        ) As System.Data.DataTable Implements ICheque_PurchaseService.GetCheque_Detail
            ' set default
            GetCheque_Detail = New DataTable
            Try
                ' variable for keep list from Rating Purchase entity
                Dim listCheque_PurchaseEnt As New List(Of Entity.ImpCheque_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow
                Dim i As Long = 0
                Dim sumSub_total As Double = 0
                Dim sumVat_amount As Double = 0
                Dim sumWt_amount As Double = 0

                ' call function GetAccountingList from entity
                listCheque_PurchaseEnt = objChequePurchaseEnt.GetCheque_Detail(strChequeNo, strChequeDate)

                ' assign column header
                With GetCheque_Detail
                    .Columns.Add("voucher_no")
                    .Columns.Add("ie_name")
                    .Columns.Add("job_order")
                    .Columns.Add("sub_total")
                    .Columns.Add("vat_name")
                    .Columns.Add("vat_amount")
                    .Columns.Add("wt_name")
                    .Columns.Add("wt_amount")
                    .Columns.Add("payment_date")
                    .Columns.Add("payment_from")

                    ' assign row from listAccountingEny
                    For Each values In listCheque_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("voucher_no") = values.voucher_no
                        row("ie_name") = values.ie_name
                        row("job_order") = values.job_order

                        row("sub_total") = Format(Convert.ToDouble(values.sub_total.ToString.Trim), "#,##0.00")
                        row("vat_name") = values.vat_name
                        row("vat_amount") = Format(Convert.ToDouble(values.vat_amount.ToString.Trim), "#,##0.00")
                        row("wt_name") = values.wt_name
                        row("wt_amount") = Format(Convert.ToDouble(values.wt_amount.ToString.Trim), "#,##0.00")
                        If IsNothing(values.payment_date) = False Then
                            row("payment_date") = CDate(values.payment_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("payment_date") = ""
                        End If
                        row("payment_from") = values.payment_from

                        ' add data row to table
                        .Rows.Add(row)

                        'sum value
                        sumSub_total = sumSub_total + values.sub_total
                        sumVat_amount = sumVat_amount + values.vat_amount
                        sumWt_amount = sumWt_amount + values.wt_amount

                        If i = listCheque_PurchaseEnt.Count - 1 Then
                            'set in list only account_type = 1 or 3 
                            row = .NewRow
                            row("voucher_no") = ""
                            row("ie_name") = ""
                            row("job_order") = ""
                            row("sub_total") = Format(sumSub_total, "#,##0.00")
                            row("vat_name") = ""
                            row("vat_amount") = Format(sumVat_amount, "#,##0.00")
                            row("wt_name") = ""
                            row("wt_amount") = Format(sumWt_amount, "#,##0.00")
                            row("payment_date") = ""
                            row("payment_from") = ""

                            ' add data row to table
                            .Rows.Add(row)
                        End If

                        i += 1
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCheque_Detail(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetAccounting
        '	Discription	    : Get Accounting
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccounting_Detail( _
            ByVal id As String, _
            ByVal dtDate As String, _
            ByVal mode As String, _
            ByRef sumScheduleRate As Double, _
            ByRef sumBankRate As Double, _
            ByRef sumVat As Double, _
            ByRef sumWT As Double _
        ) As System.Data.DataTable Implements ICheque_PurchaseService.GetAccounting_Detail
            ' set default
            GetAccounting_Detail = New DataTable
            Try
                ' variable for keep list from Rating Purchase entity
                Dim listCheque_PurchaseEnt As New List(Of Entity.ImpCheque_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listCheque_PurchaseEnt = objChequePurchaseEnt.GetAccounting_Detail(id, dtDate, mode)

                ' assign column header
                With GetAccounting_Detail
                    .Columns.Add("id")
                    .Columns.Add("chkCheque")
                    '********************
                    .Columns.Add("vendor_name")
                    .Columns.Add("vendor_type")
                    .Columns.Add("cheque_no")
                    .Columns.Add("cheque_date")
                    .Columns.Add("vat_percent")
                    .Columns.Add("wt_percent")
                    '********************
                    .Columns.Add("voucher_no")
                    .Columns.Add("invoice_no")
                    .Columns.Add("sub_total")
                    .Columns.Add("amount_bank")
                    .Columns.Add("vat_name")
                    .Columns.Add("vat_amount")
                    .Columns.Add("wt_name")
                    .Columns.Add("wt_amount")
                    .Columns.Add("payment_date")
                    .Columns.Add("payment_from")
                    'start add new 2013/09/19------
                    .Columns.Add("hsub_total")
                    .Columns.Add("hvat_amount")
                    .Columns.Add("hwt_amount")
                    .Columns.Add("account_type")
                    .Columns.Add("bank")
                    .Columns.Add("account_no")
                    .Columns.Add("account_name")
                    'end add new 2013/09/19------

                    ' assign row from listAccountingEny
                    For Each values In listCheque_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id

                        row("chkCheque") = values.chkCheque
                        '********************
                        row("vendor_name") = values.vendor_name
                        row("vendor_type") = values.vendor_type
                        row("cheque_no") = values.cheque_no
                        row("account_type") = values.account_type
                        If IsNothing(values.cheque_date) = False And IsDBNull(values.cheque_date) = False Then
                            row("cheque_date") = CDate(values.cheque_date.ToString()).ToString("dd/MM/yyyy")
                        Else
                            row("cheque_date") = ""
                        End If
                        row("bank") = values.bank
                        row("account_no") = values.account_no
                        row("account_name") = values.account_name
                        row("vat_percent") = values.vat_percent
                        row("wt_percent") = values.wt_percent
                        '********************
                        row("voucher_no") = values.voucher_no
                        row("invoice_no") = values.invoice_no

                        row("sub_total") = Format(Convert.ToDouble(values.sub_total.ToString.Trim), "#,##0.00")
                        row("amount_bank") = Format(Convert.ToDouble(values.amount_bank.ToString.Trim), "#,##0.00")
                        row("vat_name") = values.vat_name
                        row("vat_amount") = Format(Convert.ToDouble(values.vat_amount.ToString.Trim), "#,##0.00")
                        row("wt_name") = values.wt_name
                        row("wt_amount") = Format(Convert.ToDouble(values.wt_amount.ToString.Trim), "#,##0.00")
                        If IsNothing(values.payment_date) = False And IsDBNull(values.payment_date) = False Then
                            row("payment_date") = CDate(values.payment_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("payment_date") = ""
                        End If
                        row("payment_from") = values.payment_from

                        'start add new 2013/09/19------
                        row("hsub_total") = values.hsub_total
                        row("hvat_amount") = values.hvat_amount
                        row("hwt_amount") = values.hwt_amount
                        'end add new 2013/09/19------

                        sumScheduleRate = Format(sumScheduleRate + CDbl(values.sub_total.ToString.Trim), "#,##0.00")
                        sumBankRate = Format(sumBankRate + CDbl(values.amount_bank.ToString.Trim), "#,##0.00")
                        sumVat = Format(sumVat + CDbl(values.vat_amount.ToString.Trim), "#,##0.00")
                        sumWT = Format(sumWT + CDbl(values.wt_amount.ToString.Trim), "#,##0.00")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccounting_Detail(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: DeleteCheque
        '	Discription	    : Delete Cheque
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteCheque( ByVal strId As String) As Boolean Implements ICheque_PurchaseService.DeleteCheque
            ' set default return value
            DeleteCheque = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteWorkingCategory from WorkingCategory Entity
                intEff = objChequePurchaseEnt.DeleteCheque(strId)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteCheque = True
                Else
                    ' case row less than 1 then return False
                    DeleteCheque = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteCheque(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: UpdateAccounting
        '	Discription	    : Update Accounting
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateAccounting( _
            ByVal strApprover As String, _
            ByVal dtInsAcc As DataTable, _
            ByRef errorType As String _
        ) As Boolean Implements ICheque_PurchaseService.UpdateAccounting
            ' set default
            UpdateAccounting = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteWorkingCategory from WorkingCategory Entity
                intEff = objChequePurchaseEnt.UpdateAccounting(strApprover, dtInsAcc, errorType)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateAccounting = True
                Else
                    ' case row less than 1 then return False
                    UpdateAccounting = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateAccounting(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: CheckDupAccounting
        '	Discription	    : Check duplication Accounting Table
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 15-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckDupAccounting( _
            ByVal cheque_no As String, _
            ByVal cheque_date As String _
        ) As Boolean Implements ICheque_PurchaseService.CheckDupAccounting
            ' set default return value
            CheckDupAccounting = False
            Try
                ' intEff keep row effect
                Dim cntRec As Integer

                ' call function CountUsedInPO from entity
                cntRec = objChequePurchaseEnt.CheckDupAccounting(cheque_no, cheque_date)

                ' check count used
                If cntRec > 0 Then
                    ' case not equal 0 then return True
                    CheckDupAccounting = True
                Else
                    ' case equal 0 then return False
                    CheckDupAccounting = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupAccounting(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPurchasePaidReport
        '	Discription	    : Get Purchase Paid Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 23-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPurchasePaidReport( _
            ByVal itemConfirm As String _
        ) As System.Data.DataTable Implements ICheque_PurchaseService.GetPurchasePaidReport
            ' set default
            GetPurchasePaidReport = New DataTable

            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpCheque_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' Modify of 2014/09/26 By Rawikarn Katekeaw
                Dim objService As New Service.ImpPurchaseService
                Dim Currency As String


                ' call function GetAccountingList from entity
                listAccountingEnt = objChequePurchaseEnt.GetPurchasePaidReport(itemConfirm)

                ' assign column header
                With GetPurchasePaidReport
                    .Columns.Add("no")
                    .Columns.Add("invoice_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("delivery_date")
                    .Columns.Add("gross")
                    .Columns.Add("currency_id")
                    .Columns.Add("vat")
                    .Columns.Add("WT")
                    .Columns.Add("amount")
                    .Columns.Add("po_no")
                    .Columns.Add("paid_date")
                    .Columns.Add("usercreate")
                    .Columns.Add("voucher_no")
                    .Columns.Add("currency_name")

                    ' assign row from listAccountingEny
                    For Each values In listAccountingEnt
                        row = .NewRow

                        row("no") = values.no
                        row("invoice_no") = values.invoice_no
                        row("vendor_name") = values.vendor_name
                        If IsNothing(values.delivery_date) = False Then
                            row("delivery_date") = CDate(values.delivery_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("delivery_date") = ""
                        End If
                        row("gross") = Format(Convert.ToDouble(values.gross.ToString.Trim), "#,##0.00")

                        
                        If String.IsNullOrEmpty(values.currency_id) = False Then
                            Currency = values.gross.ToString.Trim / objService.GetS_Rate(values.currency_id)
                            row("currency_id") = values.currency_name & "  " & Format(Convert.ToDouble(Currency), "#,##0.00")
                        End If

                        row("vat") = Format(Convert.ToDouble(values.vat.ToString.Trim), "#,##0.00")
                        row("WT") = Format(Convert.ToDouble(values.wt.ToString.Trim), "#,##0.00")
                        row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")
                        row("po_no") = values.po_no
                        If IsNothing(values.paid_date) = False Then
                            row("paid_date") = CDate(values.paid_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("paid_date") = ""
                        End If
                        row("usercreate") = values.usercreate
                        row("voucher_no") = values.voucher_no

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPurchasePaidReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPaymentVoucher
        '	Discription	    : Get Payment Voucher
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 23-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentVoucher( _
            ByVal itemConfirm As String _
        ) As System.Data.DataTable Implements ICheque_PurchaseService.GetPaymentVoucher
            ' set default
            GetPaymentVoucher = New DataTable
            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpCheque_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listAccountingEnt = objChequePurchaseEnt.GetPaymentVoucher(itemConfirm)

                ' assign column header
                With GetPaymentVoucher
                    .Columns.Add("voucher_no")
                    .Columns.Add("print_date")
                    .Columns.Add("vendor_name")
                    .Columns.Add("cheque_no")
                    .Columns.Add("bank")
                    '2013/09/25 Pranitda S. Start-Add
                    .Columns.Add("account_type")
                    .Columns.Add("account_no")
                    .Columns.Add("account_name")
                    '2013/09/25 Pranitda S. End-Add
                    .Columns.Add("cheque_date")
                    .Columns.Add("sub_total")
                    .Columns.Add("vat_percent")
                    .Columns.Add("vat")
                    .Columns.Add("wt_percent")
                    .Columns.Add("wt")
                    .Columns.Add("total")
                    .Columns.Add("thai_bath")

                    ' assign row from listAccountingEny
                    For Each values In listAccountingEnt
                        row = .NewRow

                        row("voucher_no") = values.voucher_no
                        'row("print_date") = values.print_date
                        If IsNothing(values.print_date) = False And IsDBNull(values.print_date) = False Then
                            row("print_date") = CDate(values.print_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("print_date") = ""
                        End If
                        row("vendor_name") = values.vendor_name
                        row("cheque_no") = values.cheque_no
                        row("bank") = values.bank
                        '2013/09/25 Pranitda S. Start-Add
                        row("account_type") = values.account_type
                        row("account_no") = values.account_no
                        row("account_name") = values.account_name
                        '2013/09/25 Pranitda S. End-Add
                        If IsNothing(values.cheque_date) = False And IsDBNull(values.cheque_date) = False Then
                            row("cheque_date") = CDate(values.cheque_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("cheque_date") = ""
                        End If
                        row("sub_total") = Format(Convert.ToDouble(values.sub_total.ToString.Trim), "#,##0.00")
                        row("vat_percent") = values.vat_percent
                        row("vat") = Format(Convert.ToDouble(values.vat.ToString.Trim), "#,##0.00")
                        row("wt_percent") = values.wt_percent
                        If IsNothing(values.wt) = True Then
                            row("wt") = ""
                        Else
                            row("wt") = Format(Convert.ToDouble(values.wt.ToString.Trim), "#,##0.00")
                        End If

                        row("total") = Format(Convert.ToDouble(values.total.ToString.Trim), "#,##0.00")
                        row("thai_bath") = Utils.BahtText(CDbl(values.total))

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentVoucher(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetTaxReport
        '	Discription	    : Get Tax Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 23-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTaxReport( _
            ByVal itemConfirm As String _
        ) As System.Data.DataTable Implements ICheque_PurchaseService.GetTaxReport
            ' set default
            GetTaxReport = New DataTable
            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpCheque_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listAccountingEnt = objChequePurchaseEnt.GetTaxReport(itemConfirm)

                ' assign column header
                With GetTaxReport
                    .Columns.Add("name")
                    .Columns.Add("address")
                    .Columns.Add("type2_no")
                    .Columns.Add("type2")
                    .Columns.Add("cheque_date")
                    .Columns.Add("amount")
                    .Columns.Add("wt_amount")

                    ' assign row from listAccountingEny
                    For Each values In listAccountingEnt
                        row = .NewRow

                        row("name") = values.name
                        row("address") = values.address
                        row("type2_no") = values.type2_no
                        row("type2") = values.type2
                        If IsNothing(values.cheque_date) = False And IsDBNull(values.cheque_date) = False Then
                            row("cheque_date") = CDate(values.cheque_date.ToString()).ToString("dd-MMM-yyyy")
                        Else
                            row("cheque_date") = ""
                        End If
                        row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")
                        row("wt_amount") = Format(Convert.ToDouble(values.wt_amount.ToString.Trim), "#,##0.00")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetTaxReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetAccountReport
        '	Discription	    : Get Account Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 23-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountReport( _
            ByVal itemConfirm As String _
        ) As System.Data.DataTable Implements ICheque_PurchaseService.GetAccountReport
            ' set default
            GetAccountReport = New DataTable
            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpCheque_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listAccountingEnt = objChequePurchaseEnt.GetAccountReport(itemConfirm)

                ' assign column header
                With GetAccountReport
                    .Columns.Add("account_type")
                    .Columns.Add("account_date")
                    .Columns.Add("vendor_id")
                    .Columns.Add("cheque_no")
                    .Columns.Add("voucher_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("Expense")
                    .Columns.Add("income")
                    .Columns.Add("month")
                    .Columns.Add("year")

                    ' assign row from listAccountingEny
                    For Each values In listAccountingEnt
                        row = .NewRow

                        Select Case values.account_type
                            Case 1
                                row("account_type") = "CURRENT ACCOUNT"
                            Case 2
                                row("account_type") = "SAVING ACCOUNT"
                            Case 3
                                row("account_type") = "CASH"
                        End Select

                        'row("account_date") = values.account_date
                        If IsNothing(values.account_date) = False And IsDBNull(values.account_date) = False Then
                            row("account_date") = CDate(values.account_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("account_date") = ""
                        End If
                        row("vendor_id") = values.vendor_id
                        row("cheque_no") = values.cheque_no
                        row("voucher_no") = values.voucher_no
                        row("vendor_name") = values.vendor_name
                        row("Expense") = Format(Convert.ToDouble(values.Expense.ToString.Trim), "#,##0.00")
                        If IsNothing(values.income) Then
                            row("income") = ""
                        Else
                            row("income") = values.income.ToString.Trim
                        End If
                        row("month") = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(CDate(values.account_date).Month)
                        row("year") = CDate(values.account_date).Year

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccountReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Rating Purchase Entity object
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objChequePurchaseDto As Dto.Cheque_PurchaseDto _
        ) As Entity.ICheque_PurchaseEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpCheque_PurchaseEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    'search
                    .strChequeNo = objChequePurchaseDto.strChequeNo
                    .strChequeDateFrom = objChequePurchaseDto.strChequeDateFrom
                    .strChequeDateTo = objChequePurchaseDto.strChequeDateTo
                    .strVendor_name = objChequePurchaseDto.strVendor_name
                    .strSearchType = objChequePurchaseDto.strSearchType
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region
    End Class
End Namespace

