#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpInvoice_PurchaseService
'	Class Discription	: Implement Invoice Purchase Service
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

Namespace Service
    Public Class ImpInvoice_PurchaseService
        Implements IInvoice_PurchaseService

        Private objLog As New Common.Logs.Log
        Private objInvPurchaseEnt As New Entity.ImpInvoice_PurchaseEntity

#Region "Function"
        '/**************************************************************
        '	Function name	: GetInvoice_PurchaseList
        '	Discription	    : Get GetInvoice_PurchaseList
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetInvoice_PurchaseList( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto _
        ) As System.Data.DataTable Implements IInvoice_PurchaseService.GetInvoice_PurchaseList
            ' set default
            GetInvoice_PurchaseList = New DataTable
            Try
                ' variable for keep list from Invoice Purchase entity
                Dim listInvoice_PurchaseEnt As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listInvoice_PurchaseEnt = objInvPurchaseEnt.GetInvoice_PurchaseList(SetDtoToEntity(objInvPurchaseDto))

                ' assign column header
                With GetInvoice_PurchaseList
                    .Columns.Add("id")
                    .Columns.Add("po_type")
                    .Columns.Add("po_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("sub_total")
                    .Columns.Add("invoice_no")
                    .Columns.Add("delivery_date")
                    .Columns.Add("payment_date")
                    .Columns.Add("total_delivery_amount")
                    .Columns.Add("status_id")
                    .Columns.Add("status_name")
                    .Columns.Add("status")
                    .Columns.Add("canConfirm")
                    .Columns.Add("old_id")
                    .Columns.Add("po_header_id")

                    ' assign row from listAccountingEny
                    For Each values In listInvoice_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id
                        row("po_type") = values.po_type
                        row("po_no") = values.po_no
                        row("vendor_name") = values.vendor_name
                        row("sub_total") = Format(Convert.ToDouble(values.sub_total.ToString.Trim), "#,##0.00")
                        row("invoice_no") = values.invoice_no
                        row("delivery_date") = CDate(values.delivery_date.ToString()).ToString("dd/MMM/yyyy")
                        row("payment_date") = CDate(values.payment_date.ToString()).ToString("dd/MMM/yyyy")
                        row("total_delivery_amount") = Format(Convert.ToDouble(values.total_delivery_amount.ToString.Trim), "#,##0.00")
                        row("status_id") = values.status_id
                        row("status_name") = values.status_name
                        row("canConfirm") = values.canConfirm
                        row("old_id") = values.old_id
                        row("po_header_id") = values.po_header_id

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetInvoice_PurchaseList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetInvoice_Header
        '	Discription	    : Get Invoice Header
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetInvoice_Header( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto _
        ) As System.Data.DataTable Implements IInvoice_PurchaseService.GetInvoice_Header
            ' set default
            GetInvoice_Header = New DataTable
            Try
                ' variable for keep list from Invoice Purchase entity
                Dim listInvoice_PurchaseEnt As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listInvoice_PurchaseEnt = objInvPurchaseEnt.GetInvoice_Header(SetDtoToEntity(objInvPurchaseDto))

                ' assign column header
                With GetInvoice_Header
                    .Columns.Add("id")
                    .Columns.Add("po_header_id")
                    .Columns.Add("delivery_date")
                    .Columns.Add("payment_date")
                    .Columns.Add("invoice_no")
                    .Columns.Add("account_type")
                    .Columns.Add("account_no")
                    .Columns.Add("account_name")
                    .Columns.Add("total_delivery_amount")
                    .Columns.Add("remark")
                    .Columns.Add("user_id")
                    .Columns.Add("status_id")
                    .Columns.Add("created_by")
                    .Columns.Add("created_date")
                    .Columns.Add("updated_by")
                    .Columns.Add("updated_date")
                    .Columns.Add("vat_amount")

                    ' assign row from listAccountingEny
                    For Each values In listInvoice_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id
                        row("po_header_id") = values.po_header_id
                        row("delivery_date") = CDate(values.delivery_date).ToString("dd-MMM-yyyy")
                        row("payment_date") = CDate(values.payment_date).ToString("dd-MMM-yyyy")
                        row("invoice_no") = values.invoice_no
                        row("account_type") = values.account_type
                        row("account_no") = values.account_no
                        row("account_name") = values.account_name
                        row("total_delivery_amount") = Format(Convert.ToDouble(values.total_delivery_amount), "#,##0.00")
                        row("remark") = values.remark
                        row("user_id") = values.user_id
                        row("status_id") = values.status_id
                        row("created_by") = values.created_by
                        row("created_date") = values.created_date
                        row("updated_by") = values.updated_by
                        row("updated_date") = values.updated_date
                        row("vat_amount") = Format(Convert.ToDouble(values.vat_amount), "#,##0.00")
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetInvoice_Header(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetInvoice_Detail
        '	Discription	    : Get Invoice Detail
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetInvoice_Detail( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto _
        ) As System.Data.DataTable Implements IInvoice_PurchaseService.GetInvoice_Detail
            ' set default
            GetInvoice_Detail = New DataTable
            Try
                ' variable for keep list from Invoice Purchase entity
                Dim listInvoice_PurchaseEnt As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listInvoice_PurchaseEnt = objInvPurchaseEnt.GetInvoice_Detail(SetDtoToEntity(objInvPurchaseDto))

                ' assign column header
                With GetInvoice_Detail
                    .Columns.Add("id")
                    .Columns.Add("payment_header_id")
                    .Columns.Add("po_header_id")
                    .Columns.Add("po_detail_id")
                    .Columns.Add("delivery_qty")
                    .Columns.Add("delivery_amount")
                    .Columns.Add("stock_fg")
                    .Columns.Add("created_by")
                    .Columns.Add("created_date")
                    .Columns.Add("updated_by")
                    .Columns.Add("updated_date")
                    .Columns.Add("ITName")
                    .Columns.Add("job_order")
                    .Columns.Add("IEName")
                    .Columns.Add("quantity")
                    .Columns.Add("amount")

                    ' assign row from listAccountingEny
                    For Each values In listInvoice_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id
                        row("payment_header_id") = values.payment_header_id
                        row("po_header_id") = values.po_header_id
                        row("po_detail_id") = values.po_detail_id
                        row("delivery_qty") = values.delivery_qty
                        row("delivery_amount") = Format(Convert.ToDouble(values.delivery_amount), "#,##0.00")
                        row("stock_fg") = values.stock_fg
                        row("created_by") = values.created_by
                        row("created_date") = values.created_date
                        row("updated_by") = values.updated_by
                        row("updated_date") = values.updated_date
                        row("ITName") = values.ITName
                        row("job_order") = values.job_order
                        row("IEName") = values.IEName
                        row("quantity") = values.quantity
                        row("amount") = Format(Convert.ToDouble(values.amount), "#,##0.00")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetInvoice_Detail(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: DeletePayment
        '	Discription	    : Delete Payment Table
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeletePayment( _
            ByVal id As Integer _
        ) As Boolean Implements IInvoice_PurchaseService.DeletePayment
            ' set default return value
            DeletePayment = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                'call function DeletePayment from IInvoice_PurchaseService Entity
                intEff = objInvPurchaseEnt.DeletePayment(id)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeletePayment = True
                Else
                    ' case row less than 1 then return False
                    DeletePayment = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeletePayment(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: InsertAccounting
        '	Discription	    : Insert Accounting
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function ExecuteAccounting( _
            ByVal itemConfirm As String _
        ) As Boolean Implements IInvoice_PurchaseService.ExecuteAccounting
            ' set default return value
            ExecuteAccounting = False
            Try

                'call function DeletePayment from IInvoice_PurchaseService Entity
                ExecuteAccounting = objInvPurchaseEnt.ExecuteAccounting(itemConfirm)
                
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("ExecuteAccounting(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: IsUsedInPO
        '	Discription	    : Check item in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsed( _
            ByVal intItemID As Integer _
        ) As Boolean Implements IInvoice_PurchaseService.IsUsed
            ' set default return value
            IsUsed = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from payment_header_id 
                intCount = objInvPurchaseEnt.CountVendor_rating(intItemID)

                ' call function CountUsedInPO from CountVendor_rating 
                intCount = intCount + objInvPurchaseEnt.CountAccounting(intItemID)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUsed = True
                Else
                    ' case equal 0 then return False
                    IsUsed = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsed(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: checkConfirmPayment
        '	Discription	    : check Confirm Payment
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function checkConfirmPayment() As Boolean Implements IInvoice_PurchaseService.checkConfirmPayment
            ' set default return value
            checkConfirmPayment = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from payment_header_id 
                intCount = objInvPurchaseEnt.checkConfirmPayment()

                ' check count used
                If intCount > 0 Then
                    ' case not equal 0 then return True
                    checkConfirmPayment = True
                Else
                    ' case equal 0 then return False
                    checkConfirmPayment = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("checkConfirmPayment(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: checkInvoice
        '	Discription	    : check Invoice
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function checkInvoice(ByVal vendor_id As String, _
                                     ByVal invoice_no As String, _
                                     ByVal id As String _
        ) As Boolean Implements IInvoice_PurchaseService.checkInvoice
            ' set default return value
            checkInvoice = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from payment_header_id 
                intCount = objInvPurchaseEnt.checkInvoice(vendor_id, invoice_no, id)

                ' check count used
                If intCount > 0 Then
                    ' case not equal 0 then return True
                    checkInvoice = True
                Else
                    ' case equal 0 then return False
                    checkInvoice = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("checkInvoice(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Invoice Purchase Entity object
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto _
        ) As Entity.IInvoice_PurchaseEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpInvoice_PurchaseEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    'search
                    .strSearchType = objInvPurchaseDto.strSearchType
                    .strPO = objInvPurchaseDto.strPO
                    .strPOFrom = objInvPurchaseDto.strPOFrom
                    .strPOTo = objInvPurchaseDto.strPOTo
                    .strDeliveryDateFrom = objInvPurchaseDto.strDeliveryDateFrom
                    .strDeliveryDateTo = objInvPurchaseDto.strDeliveryDateTo
                    .strPaymentDateFrom = objInvPurchaseDto.strPaymentDateFrom
                    .strPaymentDateTo = objInvPurchaseDto.strPaymentDateTo
                    .strIssueDateFrom = objInvPurchaseDto.strIssueDateFrom
                    .strIssueDateTo = objInvPurchaseDto.strIssueDateTo
                    .strVendor_name = objInvPurchaseDto.strVendor_name
                    .strInvoice_start = objInvPurchaseDto.strInvoice_start
                    .strInvoice_end = objInvPurchaseDto.strInvoice_end

                    .strId = objInvPurchaseDto.strId
                    .taskID = objInvPurchaseDto.taskID

                    'insert payment header
                    .strPO_header_id = objInvPurchaseDto.strPO_header_id
                    .strDeliveryDateFrom = objInvPurchaseDto.strDeliveryDateFrom
                    .strPaymentDateFrom = objInvPurchaseDto.strPaymentDateFrom
                    .strInvoice_start = objInvPurchaseDto.strInvoice_start
                    .strSearchType = objInvPurchaseDto.strSearchType
                    .strAccountNo = objInvPurchaseDto.strAccountNo
                    .strAccountName = objInvPurchaseDto.strAccountName
                    .strTotal_Amount = objInvPurchaseDto.strTotal_Amount
                    .strVat_Amount = objInvPurchaseDto.strVAT_Amount
                    .strRemark = objInvPurchaseDto.strRemark
                    .strMode = objInvPurchaseDto.strMode
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetTableReport
        '	Discription	    : Get table report
        '	Return Value	: Datatable
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTableReport( _
            ByVal dtValues As System.Data.DataTable _
        ) As System.Data.DataTable Implements IInvoice_PurchaseService.GetTableReport
            ' set default return value
            GetTableReport = New DataTable
            Try
                Dim dtReport As New DataTable
                Dim dr As DataRow

                ' set header columns
                With dtReport
                    .Columns.Add("no")
                    .Columns.Add("invoice_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("delivery_date")
                    .Columns.Add("gross", System.Type.GetType("System.Decimal"))
                    .Columns.Add("vat", System.Type.GetType("System.Decimal"))
                    .Columns.Add("WT", System.Type.GetType("System.Decimal"))
                    .Columns.Add("amount", System.Type.GetType("System.Decimal"))
                    .Columns.Add("po_no")
                    .Columns.Add("paid_date")
                End With

                ' loop set data to table report
                For Each values As DataRow In dtValues.Rows
                    dr = dtReport.NewRow
                    With dr
                        .Item("no") = values.Item("no")
                        .Item("invoice_no") = values.Item("invoice_no")
                        .Item("vendor_name") = values.Item("vendor_name")
                        .Item("delivery_date") = values.Item("delivery_date")
                        .Item("gross") = values.Item("gross")
                        .Item("vat") = values.Item("vat")
                        .Item("WT") = values.Item("WT")
                        .Item("amount") = values.Item("amount")
                        .Item("po_no") = values.Item("po_no")
                        .Item("paid_date") = values.Item("paid_date")
                    End With
                    
                    dtReport.Rows.Add(dr)
                Next
                ' return new datatable
                Return dtReport
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetTableReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetCostTableDetailOverviewReport
        '	Discription	    : Get Cost TableDetail Overview Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPurchasePaidReport( _
            ByVal itemConfirm As String _
        ) As System.Data.DataTable Implements IInvoice_PurchaseService.GetPurchasePaidReport
            ' set default
            GetPurchasePaidReport = New DataTable
            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listAccountingEnt = objInvPurchaseEnt.GetPurchasePaidReport(itemConfirm)

                ' assign column header
                With GetPurchasePaidReport
                    .Columns.Add("no")
                    .Columns.Add("invoice_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("delivery_date")
                    .Columns.Add("gross")
                    .Columns.Add("vat")
                    .Columns.Add("WT")
                    .Columns.Add("amount")
                    .Columns.Add("po_no")
                    .Columns.Add("paid_date")
                    .Columns.Add("usercreate")
                    .Columns.Add("voucher_no")

                    ' assign row from listAccountingEny
                    For Each values In listAccountingEnt
                        row = .NewRow

                        row("no") = values.no
                        row("invoice_no") = values.invoice_no
                        row("vendor_name") = values.vendor_name
                        'row("delivery_date") = values.delivery_date
                        If IsNothing(values.delivery_date) = False Then
                            row("delivery_date") = CDate(values.delivery_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("delivery_date") = ""
                        End If
                        'row("gross") = values.gross
                        'row("vat") = values.vat
                        'row("WT") = values.WT
                        'row("amount") = values.amount
                        row("gross") = Format(Convert.ToDouble(values.gross.ToString.Trim), "#,##0.00")
                        row("vat") = Format(Convert.ToDouble(values.vat.ToString.Trim), "#,##0.00")
                        row("WT") = Format(Convert.ToDouble(values.WT.ToString.Trim), "#,##0.00")
                        row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")
                        row("po_no") = values.po_no
                        'row("paid_date") = values.paid_date
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
        '	Function name	: GetPO_List
        '	Discription	    : Get Get PO List
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPO_List( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto _
        ) As System.Data.DataTable Implements IInvoice_PurchaseService.GetPO_List
            ' set default
            GetPO_List = New DataTable
            Try
                ' variable for keep list from Invoice Purchase entity
                Dim listInvoice_PurchaseEnt As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listInvoice_PurchaseEnt = objInvPurchaseEnt.GetPO_List(SetDtoToEntity(objInvPurchaseDto))

                ' assign column header
                With GetPO_List
                    .Columns.Add("id")
                    .Columns.Add("po_type")
                    .Columns.Add("po_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("issue_date")
                    .Columns.Add("delivery_date")
                    .Columns.Add("sub_total")

                    ' assign row from listAccountingEny
                    For Each values In listInvoice_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id
                        row("po_type") = values.po_type
                        row("po_no") = values.po_no
                        row("vendor_name") = values.vendor_name
                        row("issue_date") = CDate(values.issue_date.ToString()).ToString("dd/MMM/yyyy")
                        row("delivery_date") = CDate(values.delivery_date.ToString()).ToString("dd/MMM/yyyy")
                        row("sub_total") = Format(Convert.ToDouble(values.sub_total.ToString.Trim), "#,##0.00")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPO_List(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPO_Header
        '	Discription	    : Get PO_Header
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPO_Header( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto _
        ) As System.Data.DataTable Implements IInvoice_PurchaseService.GetPO_Header
            ' set default
            GetPO_Header = New DataTable
            Try
                ' variable for keep list from Invoice Purchase entity
                Dim listInvoice_PurchaseEnt As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listInvoice_PurchaseEnt = objInvPurchaseEnt.GetPO_Header(SetDtoToEntity(objInvPurchaseDto))

                ' assign column header
                With GetPO_Header
                    .Columns.Add("po_type_name")
                    .Columns.Add("vendor_name")
                    .Columns.Add("payment_term")
                    .Columns.Add("VAT")
                    .Columns.Add("WT")
                    .Columns.Add("currency_Name")
                    .Columns.Add("id")
                    .Columns.Add("po_no")
                    .Columns.Add("po_type")
                    .Columns.Add("vendor_id")
                    .Columns.Add("quotation_no")
                    .Columns.Add("issue_date")
                    .Columns.Add("delivery_date")
                    .Columns.Add("payment_term_id")
                    .Columns.Add("vat_id")
                    .Columns.Add("wt_id")
                    .Columns.Add("currency_id")
                    .Columns.Add("remark")
                    .Columns.Add("discount_total")
                    .Columns.Add("sub_total")
                    .Columns.Add("vat_amount")
                    .Columns.Add("wt_amount")
                    .Columns.Add("total_amount")
                    .Columns.Add("attn")
                    .Columns.Add("deliver_to")
                    .Columns.Add("contact")
                    .Columns.Add("user_id")
                    .Columns.Add("status_id")
                    .Columns.Add("created_by")
                    .Columns.Add("created_date")
                    .Columns.Add("updated_by")
                    .Columns.Add("updated_date")

                    ' assign row from listAccountingEny
                    For Each values In listInvoice_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("po_type_name") = values.po_type_name
                        row("vendor_name") = values.vendor_name
                        row("payment_term") = values.payment_term
                        row("VAT") = values.vat & "%"
                        row("WT") = values.WT & "%"
                        row("currency_Name") = values.currency_Name
                        row("id") = values.id
                        row("po_no") = values.po_no
                        row("po_type") = values.po_type
                        row("vendor_id") = values.vendor_id
                        row("quotation_no") = values.quotation_no
                        row("issue_date") = CDate(values.issue_date.ToString()).ToString("dd/MMM/yyyy")
                        row("delivery_date") = CDate(values.delivery_date.ToString()).ToString("dd/MMM/yyyy")
                        row("payment_term_id") = values.payment_term_id
                        row("vat_id") = values.vat_id
                        row("wt_id") = values.wt_id
                        row("currency_id") = values.currency_id
                        row("remark") = values.remark
                        row("discount_total") = Format(Convert.ToDouble(values.discount_total.ToString.Trim), "#,##0.00")
                        row("sub_total") = Format(Convert.ToDouble(values.sub_total.ToString.Trim), "#,##0.00")
                        row("vat_amount") = Format(Convert.ToDouble(values.vat_amount.ToString.Trim), "#,##0.00")
                        row("wt_amount") = Format(Convert.ToDouble(values.wt_amount.ToString.Trim), "#,##0.00")
                        row("total_amount") = Format(Convert.ToDouble(values.total_amount.ToString.Trim), "#,##0.00")
                        row("attn") = values.attn
                        row("deliver_to") = values.deliver_to
                        row("contact") = values.contact
                        row("user_id") = values.user_id
                        row("status_id") = values.status_id
                        row("created_by") = values.created_by
                        row("created_date") = values.created_date
                        row("updated_by") = values.updated_by
                        row("updated_date") = values.updated_date

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPO_Header(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPO_Detail
        '	Discription	    : Get PO_Detail
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPO_Detail( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto _
        ) As System.Data.DataTable Implements IInvoice_PurchaseService.GetPO_Detail
            ' set default
            GetPO_Detail = New DataTable
            Try
                ' variable for keep list from Invoice Purchase entity
                Dim listInvoice_PurchaseEnt As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listInvoice_PurchaseEnt = objInvPurchaseEnt.GetPO_Detail(SetDtoToEntity(objInvPurchaseDto))

                ' assign column header
                With GetPO_Detail
                    .Columns.Add("id")
                    .Columns.Add("payment_header_id")
                    .Columns.Add("po_header_id")
                    .Columns.Add("po_detail_id")
                    .Columns.Add("delivery_qty")
                    .Columns.Add("delivery_amount")
                    .Columns.Add("stock_fg")
                    .Columns.Add("created_by")
                    .Columns.Add("created_date")
                    .Columns.Add("updated_by")
                    .Columns.Add("updated_date")
                    .Columns.Add("item_name")
                    .Columns.Add("job_order")
                    .Columns.Add("ie_name")
                    .Columns.Add("quantity")
                    .Columns.Add("amount")
                    .Columns.Add("unit_name")
                    .Columns.Add("discount")
                    .Columns.Add("discount_type")
                    .Columns.Add("remark")
                    .Columns.Add("unit_price")
                    .Columns.Add("vat_amount")
                    .Columns.Add("wt_amount")

                    ' assign row from listAccountingEny
                    For Each values In listInvoice_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id
                        row("payment_header_id") = values.payment_header_id
                        row("po_header_id") = values.po_header_id
                        row("po_detail_id") = values.po_detail_id
                        row("delivery_qty") = values.delivery_qty
                        row("delivery_amount") = values.delivery_amount
                        row("stock_fg") = values.stock_fg
                        row("created_by") = values.created_by
                        row("created_date") = values.created_date
                        row("updated_by") = values.updated_by
                        row("updated_date") = values.updated_date
                        row("item_name") = values.item_name
                        row("job_order") = values.job_order
                        row("ie_name") = values.ie_name
                        row("quantity") = values.quantity
                        row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")
                        row("unit_name") = values.unit_name
                        row("discount") = Format(Convert.ToDouble(values.discount.ToString.Trim), "#,##0.00")
                        row("discount_type") = values.discount_type
                        row("remark") = values.remark
                        row("unit_price") = Format(Convert.ToDouble(values.unit_price.ToString.Trim), "#,##0.00")
                        row("vat_amount") = Format(Convert.ToDouble(values.vat_amount.ToString.Trim), "#,##0.00")
                        row("wt_amount") = Format(Convert.ToDouble(values.wt_amount.ToString.Trim), "#,##0.00")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPO_Detail(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPO_Detail_Insert
        '	Discription	    : Get PO_Detail
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPO_Detail_Insert( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto _
        ) As System.Data.DataTable Implements IInvoice_PurchaseService.GetPO_Detail_Insert
            ' set default
            GetPO_Detail_Insert = New DataTable
            Try
                ' variable for keep list from Invoice Purchase entity
                Dim listInvoice_PurchaseEnt As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listInvoice_PurchaseEnt = objInvPurchaseEnt.GetPO_Detail_Insert(SetDtoToEntity(objInvPurchaseDto))

                ' assign column header
                With GetPO_Detail_Insert
                    .Columns.Add("id")
                    .Columns.Add("item_name")
                    .Columns.Add("job_order")
                    .Columns.Add("ie_name")
                    .Columns.Add("quantity")
                    .Columns.Add("amount")
                    .Columns.Add("unit_price")
                    .Columns.Add("remain_qty")
                    .Columns.Add("remain_amt")
                    .Columns.Add("delivery_qty")
                    .Columns.Add("delivery_amt")
                    .Columns.Add("po_no")
                    .Columns.Add("ie_id")
                    .Columns.Add("vendor_id")
                    .Columns.Add("item_id")
                    .Columns.Add("base")

                    ' assign row from listAccountingEny
                    For Each values In listInvoice_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id
                        row("item_name") = values.item_name.ToString.Trim
                        row("job_order") = values.job_order.ToString.Trim
                        row("ie_name") = values.ie_name.ToString.Trim
                        row("quantity") = values.quantity.ToString.Trim
                        row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")
                        row("unit_price") = Format(Convert.ToDouble(values.unit_price.ToString.Trim), "#,##0.00")
                        row("remain_qty") = values.remain_qty
                        row("remain_amt") = Format(Convert.ToDouble(values.remain_amt.ToString.Trim), "#,##0.00")
                        row("delivery_qty") = values.delivery_qty
                        row("delivery_amt") = Format(Convert.ToDouble(values.delivery_amt.ToString.Trim), "#,##0.00")
                        row("po_no") = values.po_no
                        row("ie_id") = values.ie_id
                        row("vendor_id") = values.vendor_id
                        row("item_id") = values.item_id
                        row("base") = values.base

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPO_Detail_Insert(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPO_Header_Insert
        '	Discription	    : Get PO_Detail
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPO_Header_Insert( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto _
        ) As System.Data.DataTable Implements IInvoice_PurchaseService.GetPO_Header_Insert
            ' set default
            GetPO_Header_Insert = New DataTable
            Try
                ' variable for keep list from Invoice Purchase entity
                Dim listInvoice_PurchaseEnt As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listInvoice_PurchaseEnt = objInvPurchaseEnt.GetPO_Header_Insert(SetDtoToEntity(objInvPurchaseDto))

                ' assign column header
                With GetPO_Header_Insert
                    '.Columns.Add("delivery_amount")
                    .Columns.Add("id")
                    .Columns.Add("delivery_date")
                    .Columns.Add("payment_date")
                    .Columns.Add("invoice_no")
                    .Columns.Add("account_type")
                    .Columns.Add("account_no")
                    .Columns.Add("account_name")
                    .Columns.Add("delivery_amount")
                    .Columns.Add("remark")
                    .Columns.Add("vendor_id")

                    ' assign row from listAccountingEny
                    For Each values In listInvoice_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        'row("delivery_amount") = Format(Convert.ToDouble(values.delivery_amount.ToString.Trim), "#,##0.00")
                        row("id") = values.id
                        row("delivery_date") = ""
                        row("payment_date") = ""
                        row("invoice_no") = ""
                        row("account_type") = ""
                        row("account_no") = ""
                        row("account_name") = ""
                        row("delivery_amount") = Format(Convert.ToDouble(values.delivery_amount.ToString.Trim), "#,##0.00")
                        row("remark") = ""
                        row("vendor_id") = values.vendor_id

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPO_Header_Insert(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: InsertPayment
        '	Discription	    : Insert Payment Header,payment detail
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertPayment( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto, _
            ByVal dtPaymentDetail As DataTable _
        ) As Boolean Implements IInvoice_PurchaseService.InsertPayment
            ' set default
            InsertPayment = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteWorkingCategory from WorkingCategory Entity
                intEff = objInvPurchaseEnt.InsertPayment(SetDtoToEntity(objInvPurchaseDto), dtPaymentDetail)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertPayment = True
                Else
                    ' case row less than 1 then return False
                    InsertPayment = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertPayment(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPaymentHeader
        '	Discription	    : Get Payment Header
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentHeader( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto _
        ) As System.Data.DataTable Implements IInvoice_PurchaseService.GetPaymentHeader
            ' set default
            GetPaymentHeader = New DataTable
            Try
                ' variable for keep list from Invoice Purchase entity
                Dim listInvoice_PurchaseEnt As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listInvoice_PurchaseEnt = objInvPurchaseEnt.GetPaymentHeader(SetDtoToEntity(objInvPurchaseDto))

                ' assign column header
                With GetPaymentHeader
                    .Columns.Add("id")
                    .Columns.Add("delivery_date")
                    .Columns.Add("payment_date")
                    .Columns.Add("invoice_no")
                    .Columns.Add("account_type")
                    .Columns.Add("account_no")
                    .Columns.Add("account_name")
                    .Columns.Add("delivery_amount")
                    .Columns.Add("remark")
                    .Columns.Add("vendor_id")

                    ' assign row from listAccountingEny
                    For Each values In listInvoice_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id
                        row("delivery_date") = CDate(values.delivery_date).ToString("dd/MM/yyyy")
                        row("payment_date") = CDate(values.payment_date).ToString("dd/MM/yyyy")
                        row("invoice_no") = values.invoice_no
                        row("account_type") = values.account_type
                        row("account_no") = values.account_no
                        row("account_name") = values.account_name
                        row("delivery_amount") = Format(Convert.ToDouble(values.delivery_amount.ToString.Trim), "#,##0.00")
                        row("remark") = values.remark
                        row("vendor_id") = values.vendor_id

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentHeader(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPaymentDetail
        '	Discription	    : Get Payment Detail
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentDetail( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto _
        ) As System.Data.DataTable Implements IInvoice_PurchaseService.GetPaymentDetail
            ' set default
            GetPaymentDetail = New DataTable
            Try
                ' variable for keep list from Invoice Purchase entity
                Dim listInvoice_PurchaseEnt As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listInvoice_PurchaseEnt = objInvPurchaseEnt.GetPaymentDetail(SetDtoToEntity(objInvPurchaseDto))

                ' assign column header
                With GetPaymentDetail
                    .Columns.Add("id")
                    .Columns.Add("item_name")
                    .Columns.Add("job_order")
                    .Columns.Add("ie_name")
                    .Columns.Add("quantity")
                    .Columns.Add("amount")
                    .Columns.Add("unit_price")
                    .Columns.Add("remain_qty")
                    .Columns.Add("remain_amt")
                    .Columns.Add("delivery_qty")
                    .Columns.Add("delivery_amt")
                    .Columns.Add("po_no")
                    .Columns.Add("ie_id")
                    .Columns.Add("vendor_id")
                    .Columns.Add("item_id")
                    .Columns.Add("base")
                    .Columns.Add("po_detail_id")

                    ' assign row from listAccountingEny
                    For Each values In listInvoice_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id
                        row("item_name") = values.item_name.ToString.Trim
                        row("job_order") = values.job_order.ToString.Trim
                        row("ie_name") = values.ie_name.ToString.Trim
                        row("quantity") = values.quantity.ToString.Trim
                        row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")
                        row("unit_price") = Format(Convert.ToDouble(values.unit_price.ToString.Trim), "#,##0.00")
                        row("remain_qty") = values.remain_qty
                        row("remain_amt") = Format(Convert.ToDouble(values.remain_amt.ToString.Trim), "#,##0.00")
                        row("delivery_qty") = values.delivery_qty
                        row("delivery_amt") = Format(Convert.ToDouble(values.delivery_amt.ToString.Trim), "#,##0.00")
                        row("po_no") = values.po_no
                        row("ie_id") = values.ie_id
                        row("vendor_id") = values.vendor_id
                        row("item_id") = values.item_id
                        row("base") = values.base
                        row("po_detail_id") = values.po_detail_id

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentDetail(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: UpdatePayment
        '	Discription	    : Update Payment Header,payment detail
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdatePayment( _
            ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto, _
            ByVal dtPaymentDetail As DataTable _
        ) As Boolean Implements IInvoice_PurchaseService.UpdatePayment
            ' set default
            UpdatePayment = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteWorkingCategory from WorkingCategory Entity
                intEff = objInvPurchaseEnt.UpdatePayment(SetDtoToEntity(objInvPurchaseDto), dtPaymentDetail)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdatePayment = True
                Else
                    ' case row less than 1 then return False
                    UpdatePayment = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdatePayment(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region

    End Class
End Namespace

