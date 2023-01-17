#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpAccountingService
'	Class Discription	: Implement Accounting Service
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 07-06-2013
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
Imports Utils

Namespace Service
    Public Class ImpAccountingService
        Implements IAccountingService

        Private objLog As New Common.Logs.Log
        Private objAccountingEnt As New Entity.ImpAccountingEntity
        Private objUtility As New Common.Utilities.Utility

#Region "Function"
        '/**************************************************************
        '	Function name	: GetAccountingList
        '	Discription	    : Get Accounting list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountingList( _
            ByVal objAccountingDto As Dto.AccountingDto, _
            ByVal dataType As String _
        ) As System.Data.DataTable Implements IAccountingService.GetAccountingList
            ' set default
            GetAccountingList = New DataTable
            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpMst_AccountingDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listAccountingEnt = objAccountingEnt.GetAccountingList(SetDtoToEntity(objAccountingDto))

                ' assign column header
                With GetAccountingList
                    .Columns.Add("id")
                    .Columns.Add("voucher_no")
                    .Columns.Add("account_date")
                    .Columns.Add("cheque_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("Ie_name")
                    .Columns.Add("job_order")
                    .Columns.Add("income")
                    .Columns.Add("Expense")
                    .Columns.Add("account_type")
                    .Columns.Add("month")
                    .Columns.Add("year")
                    .Columns.Add("vendor_id")

                    ' assign row from listAccountingEny
                    For Each values In listAccountingEnt
                        row = .NewRow
                        row("id") = values.id
                        row("voucher_no") = values.voucher_no

                        If dataType = "1" Then 'Output on gridview
                            row("account_date") = CDate(values.account_date.ToString()).ToString("dd/MMM/yyyy")
                        Else 'Output on Excel
                            row("account_date") = CDate(values.account_date.ToString()).ToString("dd/MM/yyyy")
                        End If
                        row("cheque_no") = values.cheque_no
                        row("vendor_name") = values.vendor_name
                        row("Ie_name") = values.Ie_name
                        row("job_order") = values.job_order

                        row("income") = Format(Convert.ToDouble(values.income.ToString.Trim), "#,##0.00")
                        row("Expense") = Format(Convert.ToDouble(values.Expense.ToString.Trim), "#,##0.00")

                        Select Case values.account_type
                            Case 1
                                row("account_type") = "CURRENT ACCOUNT"
                            Case 2
                                row("account_type") = "SAVING ACCOUNT"
                            Case 3
                                row("account_type") = "CASH"
                        End Select

                        row("month") = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(objAccountingDto.strAccountMonth)
                        row("year") = objAccountingDto.strAccountYear
                        row("vendor_id") = values.vendor_id

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccountingList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetWithholdingExcelList
        '	Discription	    : Get Withholding Excel List
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWithholdingList( _
            ByVal objAccountingDto As Dto.AccountingDto, _
            ByVal dataType As String _
        ) As System.Data.DataTable Implements IAccountingService.GetWithholdingList
            ' set default
            GetWithholdingList = New DataTable
            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpMst_AccountingDetailEntity)
                ' data row object
                Dim row As DataRow
                Dim culture As CultureInfo = CultureInfo.GetCultureInfo("th-TH")

                ' call function GetAccountingList from entity
                listAccountingEnt = objAccountingEnt.GetAccountingList(SetDtoToEntity(objAccountingDto))

                ' assign column header
                With GetWithholdingList
                    .Columns.Add("id")
                    .Columns.Add("voucher_no")
                    .Columns.Add("account_date")
                    .Columns.Add("cheque_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("Ie_name")
                    .Columns.Add("job_order")
                    .Columns.Add("income")
                    .Columns.Add("Expense")
                    .Columns.Add("account_type")
                    .Columns.Add("month")
                    .Columns.Add("year")
                    .Columns.Add("vendor_id")
                    .Columns.Add("po_no")
                    .Columns.Add("part_name")
                    .Columns.Add("sub_total")
                    .Columns.Add("wt_percentage")
                    .Columns.Add("wt_amount")
                    .Columns.Add("vendor_type2_no")
                    .Columns.Add("wt_type")
                    .Columns.Add("address")
                    .Columns.Add("vendor_type2")

                    ' assign row from listAccountingEny
                    For Each values In listAccountingEnt
                        'case person and company only
                        If values.vendor_type2 = "0" Or values.vendor_type2 = "1" Then
                            row = .NewRow
                            row("id") = values.id
                            row("voucher_no") = values.voucher_no
                            If dataType = "1" Then 'Output on gridview
                                row("account_date") = CDate(values.account_date.ToString()).ToString("dd/MMM/yyyy")
                            Else 'Output on Excel
                                row("account_date") = CDate(values.account_date.ToString()).ToString("dd/MM/yyyy")
                            End If
                            row("cheque_no") = values.cheque_no
                            row("vendor_name") = values.vendor_name
                            row("Ie_name") = values.Ie_name
                            row("job_order") = values.job_order

                            row("income") = Format(Convert.ToDouble(values.income.ToString.Trim), "#,##0.00")
                            row("Expense") = Format(Convert.ToDouble(values.Expense.ToString.Trim), "#,##0.00")

                            Select Case values.account_type
                                Case 1
                                    row("account_type") = "CURRENT ACCOUNT"
                                Case 2
                                    row("account_type") = "SAVING ACCOUNT"
                                Case 3
                                    row("account_type") = "CASH"
                            End Select

                            row("month") = culture.DateTimeFormat.GetMonthName(objAccountingDto.strAccountMonth)
                            'row("year") = objAccountingDto.strAccountYear

                            Dim dt As Date = "01/" & objAccountingDto.strAccountMonth & "/" & objAccountingDto.strAccountYear

                            row("year") = Year(dt.ToString("dd/MM/yyyy HH:mm:ss", culture))
                            row("vendor_id") = values.vendor_id
                            row("po_no") = values.po_no
                            row("part_name") = values.part_name

                            If dataType = "1" Then 'Output on gridview
                                row("sub_total") = Format(Convert.ToDouble(values.sub_total.ToString.Trim), "#,##0.00")
                                row("wt_percentage") = values.wt_percentage & "%"
                                row("wt_amount") = Format(Convert.ToDouble(values.wt_amount.ToString.Trim), "#,##0.00")
                            Else 'Output on Excel
                                row("sub_total") = values.sub_total
                                row("wt_percentage") = values.wt_percentage
                                row("wt_amount") = values.wt_amount
                            End If

                            row("vendor_type2_no") = values.vendor_type2_no
                            row("wt_type") = values.wt_type

                            'row("address") = values.address
                            row("address") = Regex.Replace(values.address, Chr(13) + Chr(10), " ")

                            row("vendor_type2") = values.vendor_type2

                            ' add data row to table
                            .Rows.Add(row)
                        End If
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWithholdingList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetAccountingList
        '	Discription	    : Get Accounting list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountingDetail( _
            ByVal objAccountingDto As Dto.AccountingDto _
        ) As System.Data.DataTable Implements IAccountingService.GetAccountingDetail
            ' set default
            GetAccountingDetail = New DataTable
            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpMst_AccountingDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listAccountingEnt = objAccountingEnt.GetAccountingDetail(SetDtoToEntity(objAccountingDto))

                ' assign column header
                With GetAccountingDetail
                    .Columns.Add("AccountType")
                    .Columns.Add("VendorName")
                    .Columns.Add("Bank")
                    .Columns.Add("AccountName")
                    .Columns.Add("AccountNo")
                    .Columns.Add("PaymentDate")
                    .Columns.Add("JobNo")
                    .Columns.Add("vat_amount")
                    .Columns.Add("vat_percentage")
                    .Columns.Add("wt_amount")
                    .Columns.Add("wt_percentage")
                    .Columns.Add("IE")
                    .Columns.Add("SubTotal")
                    .Columns.Add("Remarks")

                    ' assign row from listAccountingEny
                    For Each values In listAccountingEnt
                        row = .NewRow
                        row("AccountType") = values.account_type
                        row("VendorName") = values.vendor_name
                        row("Bank") = values.bank
                        row("AccountName") = values.account_name
                        row("AccountNo") = values.account_no
                        row("PaymentDate") = CDate(values.account_date.ToString()).ToString("dd/MMM/yyyy")
                        row("JobNo") = values.job_order
                        row("vat_amount") = Format(Convert.ToDouble(values.vat_amount.ToString.Trim), "#,##0.00")
                        row("vat_percentage") = values.vat_percentage
                        row("wt_amount") = Format(Convert.ToDouble(values.wt_amount.ToString.Trim), "#,##0.00")
                        row("wt_percentage") = values.wt_percentage
                        row("IE") = values.Ie_name
                        row("SubTotal") = Format(Convert.ToDouble(values.sub_total.ToString.Trim), "#,##0.00")
                        row("Remarks") = values.remark

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccountingDetail(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetAccountingList
        '	Discription	    : Get Accounting list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 13-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCostTableDetailList( _
            ByVal objAccountingDto As Dto.AccountingDto _
        ) As System.Data.DataTable Implements IAccountingService.GetCostTableDetailList
            ' set default
            GetCostTableDetailList = New DataTable
            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpMst_AccountingDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listAccountingEnt = objAccountingEnt.GetAccountingList(SetDtoToEntity(objAccountingDto))

                ' assign column header
                With GetCostTableDetailList
                    .Columns.Add("job_order")
                    .Columns.Add("Ie_name")
                    .Columns.Add("account_date")
                    .Columns.Add("account_type")
                    .Columns.Add("vendor_name")
                    .Columns.Add("income")
                    .Columns.Add("expense")
                    .Columns.Add("remark")

                    ' assign row from listAccountingEny
                    For Each values In listAccountingEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("job_order") = values.job_order
                        row("Ie_name") = values.Ie_name
                        row("account_date") = CDate(values.account_date.ToString()).ToString("dd/MMM/yyyy")
                        Select Case values.account_type
                            Case "1"
                                row("account_type") = "Current Account"
                            Case "2"
                                row("account_type") = "Saving Account"
                            Case "3"
                                row("account_type") = "Cash"
                            Case Else
                                row("account_type") = ""
                        End Select

                        row("vendor_name") = values.vendor_name
                        row("income") = values.income
                        row("expense") = Format(Convert.ToDouble(CDbl(values.income) + CDbl(values.Expense)), "#,##0.00")
                        row("remark") = values.remark
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccountingList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetCostTableDetailOverviewList
        '	Discription	    : Get Cost TableDetail Overview List
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCostTableOverviewList( _
            ByVal objAccountingDto As Dto.AccountingDto _
        ) As System.Data.DataTable Implements IAccountingService.GetCostTableOverviewList
            ' set default
            GetCostTableOverviewList = New DataTable
            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpMst_AccountingDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listAccountingEnt = objAccountingEnt.GetCostTableOverviewList(SetDtoToEntity(objAccountingDto))

                ' assign column header
                With GetCostTableOverviewList
                    .Columns.Add("job_order")
                    .Columns.Add("vendor_name")
                    .Columns.Add("job_type_name")
                    .Columns.Add("part_no")
                    .Columns.Add("part_name")

                    ' assign row from listAccountingEny
                    For Each values In listAccountingEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("job_order") = values.job_order
                        row("vendor_name") = values.vendor_name
                        row("job_type_name") = values.job_type_name
                        row("part_no") = values.part_no
                        row("part_name") = values.part_name
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCostTableOverviewList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetCostTableDetailOverviewReport
        '	Discription	    : Get Cost TableDetail Overview Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCostTableOverviewReport( _
            ByVal objAccountingDto As Dto.AccountingDto _
        ) As System.Data.DataTable Implements IAccountingService.GetCostTableOverviewReport
            ' set default
            GetCostTableOverviewReport = New DataTable
            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpMst_AccountingDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listAccountingEnt = objAccountingEnt.GetCostTableOverviewReport(SetDtoToEntity(objAccountingDto))

                ' assign column header
                With GetCostTableOverviewReport
                    .Columns.Add("job_order")
                    .Columns.Add("account_year")
                    .Columns.Add("account_month")
                    .Columns.Add("ie_type")
                    .Columns.Add("ie_code")
                    .Columns.Add("ie_desc")
                    .Columns.Add("sub_total")

                    ' assign row from listAccountingEny
                    For Each values In listAccountingEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("job_order") = values.job_order
                        row("account_year") = values.account_year
                        row("account_month") = values.account_month
                        row("ie_type") = values.ie_type
                        row("ie_code") = values.ie_code
                        row("ie_desc") = values.ie_desc
                        row("sub_total") = values.sub_total
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCostTableOverviewReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: chkCategoryAccountTitle
        '	Discription	    : chk Category AccountTitle
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function chkCategoryAccountTitle( _
            ByVal ieCode As String _
        ) As String Implements IAccountingService.chkCategoryAccountTitle
            ' set default
            chkCategoryAccountTitle = ""
            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpMst_AccountingDetailEntity)
                ' data row object
                Dim tempId As String = ""
                Dim i As Integer = 0

                ' call function GetAccountingList from entity
                listAccountingEnt = objAccountingEnt.chkCategoryAccountTitle(ieCode)

                ' assign row from listAccountingEny
                For Each values In listAccountingEnt
                    chkCategoryAccountTitle = values.category_id
                Next
            Catch ex As Exception
                chkCategoryAccountTitle = ""
                ' write error log
                objLog.ErrorLog("chkCategoryAccountTitle(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: chkExitIEMaster
        '	Discription	    : chk Exit IEMaster
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function chkExitIEMaster( _
            ByVal code As String _
        ) As Boolean Implements IAccountingService.chkExitIEMaster
            ' set default return value
            chkExitIEMaster = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objAccountingEnt.chkExitIEMaster(code)

                ' check count used
                If intCount > 0 Then
                    ' case not equal 0 then return True
                    chkExitIEMaster = True
                Else
                    ' case equal 0 then return False
                    chkExitIEMaster = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("chkExitIEMaster(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Accounting Entity object
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/

        Private Function SetDtoToEntity( _
            ByVal objAccountingDto As Dto.AccountingDto _
        ) As Entity.IAccountingEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpAccountingEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .strAccountMonth = objAccountingDto.strAccountMonth
                    .strAccountYear = objAccountingDto.strAccountYear
                    .strJob_order_text = objAccountingDto.strJob_order_text
                    .strAccount_id = objAccountingDto.strAccount_id
                    .strAccount_startdate = objAccountingDto.strAccount_startdate
                    .strAccount_enddate = objAccountingDto.strAccount_enddate
                    .strJoborder_start = objAccountingDto.strJoborder_start
                    .strJoborder_end = objAccountingDto.strJoborder_end
                    .strAccount_type = objAccountingDto.strAccount_type
                    .strIe_id = objAccountingDto.strIe_id
                    .strVendor_name = objAccountingDto.strVendor_name
                    .strPo_startno = objAccountingDto.strPo_startno
                    .strPo_endno = objAccountingDto.strPo_endno
                    .strIe_start_code = objAccountingDto.strIe_start_code
                    .strIe_end_code = objAccountingDto.strIe_end_code
                    .strStatus_id = objAccountingDto.strStatus_id
                    .accType = objAccountingDto.accType
                    .strIe_category_type = objAccountingDto.strIe_category_type
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertIncome
        '	Discription	    : Insert Income to Account
        '	Return Value	: Boolean
        '	Create User	    : Komsan L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertIncome( _
            ByVal dtValues As System.Data.DataTable _
        ) As Boolean Implements IAccountingService.InsertIncome
            ' set default return value
            InsertIncome = False
            Try
                ' variable Accounting object
                Dim objAccountingEnt As New Entity.ImpAccountingEntity
                ' variable effect row
                Dim intEff As Integer

                ' call function InsertIncome from AccountingEntity
                intEff = objAccountingEnt.InsertIncome(dtValues)

                ' check row effect
                If intEff > 0 Then
                    ' case row effect > 0 then return true
                    Return True
                Else
                    ' case row effect <=0 then return false
                    Return False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertIncome(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetTableReport
        '	Discription	    : Get table report
        '	Return Value	: Datatable
        '	Create User	    : Komsan L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTableReport( _
            ByVal dtValues As System.Data.DataTable _
        ) As System.Data.DataTable Implements IAccountingService.GetTableReport
            ' set default return value
            GetTableReport = New DataTable
            Try
                Dim dtReport As New DataTable
                Dim dr As DataRow

                ' set header columns
                With dtReport
                    .Columns.Add("Type")
                    .Columns.Add("AccountTypeName")
                    .Columns.Add("VendorName")
                    .Columns.Add("Bank")
                    .Columns.Add("JobOrder")
                    .Columns.Add("ReceiptDate")
                    .Columns.Add("WTAmount", System.Type.GetType("System.Decimal"))
                    .Columns.Add("WTText")
                    .Columns.Add("VatAmount", System.Type.GetType("System.Decimal"))
                    .Columns.Add("VatText")
                    .Columns.Add("IEName")
                    .Columns.Add("Remark")
                    .Columns.Add("SubTotal", System.Type.GetType("System.Decimal"))
                    .Columns.Add("UserName")
                    .Columns.Add("VoucherNo")
                End With

                ' loop set data to table report
                For Each values As DataRow In dtValues.Rows
                    dr = dtReport.NewRow
                    If values.Item("Type") = Convert.ToInt32(Enums.AccountRecordTypes.Income) Then
                        dr.Item("Type") = Enums.AccountRecordTypes.Income
                    ElseIf values.Item("Type") = Convert.ToInt32(Enums.AccountRecordTypes.Payment) Then
                        dr.Item("Type") = Enums.AccountRecordTypes.Payment
                    End If
                    dr.Item("AccountTypeName") = values.Item("AccountTypeName")
                    dr.Item("VendorName") = values.Item("VendorName")
                    dr.Item("Bank") = values.Item("Bank")
                    dr.Item("JobOrder") = values.Item("JobOrder")
                    dr.Item("ReceiptDate") = values.Item("ReceiptDateShow")
                    dr.Item("WTAmount") = values.Item("WTAmount")
                    dr.Item("WTText") = values.Item("WTText")
                    dr.Item("VatAmount") = values.Item("VatAmount")
                    dr.Item("VatText") = values.Item("VatText")
                    dr.Item("IEName") = values.Item("IEName")
                    dr.Item("Remark") = values.Item("Remark")
                    dr.Item("SubTotal") = values.Item("SubTotal")
                    dr.Item("UserName") = HttpContext.Current.Session("UserName")
                    dr.Item("VoucherNo") = values.Item("VoucherNo")
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
        '	Function name	: GetTableReport
        '	Discription	    : Get table report
        '	Return Value	: Datatable
        '	Create User	    : Wasan D.
        '	Create Date	    : 04-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTableReport( _
            ByVal dtValues As System.Data.DataTable, _
            ByVal sortColumn As String _
        ) As System.Data.DataTable Implements IAccountingService.GetTableReport
            ' set default return value
            GetTableReport = New DataTable
            Try
                Dim dtReport As New DataTable
                Dim dr As DataRow
                Dim colName As New List(Of String)
                colName.Add("Type")
                colName.Add("AccountTypeName")
                colName.Add("VendorName")
                colName.Add("Bank")
                colName.Add("JobOrder")
                colName.Add("ReceiptDate")
                colName.Add("WTAmount")
                colName.Add("WTText")
                colName.Add("VatAmount")
                colName.Add("VatText")
                colName.Add("IEName")
                colName.Add("Remark")
                colName.Add("SubTotal")
                colName.Add("UserName")
                colName.Add("VoucherNo")
                ' Sort column with ASC
                dtValues.DefaultView.Sort = sortColumn
                ' Set column name
                For Each colNameTmp As String In colName
                    GetTableReport.Columns.Add(colNameTmp)
                Next
                ' Set column type
                GetTableReport.Columns("WTAmount").DataType = System.Type.GetType("System.Decimal")
                GetTableReport.Columns("VatAmount").DataType = System.Type.GetType("System.Decimal")
                GetTableReport.Columns("SubTotal").DataType = System.Type.GetType("System.Decimal")
                ' Assign value to datatable
                For i = 0 To dtValues.DefaultView.Count - 1
                    dr = GetTableReport.NewRow
                    For Each colNameTmp As String In colName
                        If colNameTmp <> "UserName" Then
                            dr(colNameTmp) = dtValues.DefaultView(i).Item(colNameTmp)
                        Else
                            dr(colNameTmp) = HttpContext.Current.Session("UserName")
                        End If
                    Next
                    GetTableReport.Rows.Add(dr)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetTableReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsExistJobOrder
        '	Discription	    : Check Exist job order
        '	Return Value	: Boolean
        '	Create User	    : Komsan L.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsExistJobOrder( _
            ByVal strJobOrder As String _
        ) As Boolean Implements IAccountingService.IsExistJobOrder
            ' set default return value
            IsExistJobOrder = False
            Try
                Dim intJobOrder As Integer
                intJobOrder = objAccountingEnt.CountJobOrder(strJobOrder)

                ' check exist joborder
                If intJobOrder > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsExistJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetYearList
        '	Discription	    : Get Year for dropdownlist
        '	Return Value	: List
        '	Create User	    : Komsan L.
        '	Create Date	    : 03-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetYearList() As System.Data.DataTable Implements IAccountingService.GetYearList
            ' set default list
            GetYearList = New DataTable
            Try
                ' objVendorDto for keep value Dto 
                Dim listAccEnt As New List(Of Entity.IAccountingEntity)
                Dim startYear As Integer = 0
                Dim currentYear As Integer = 0
                Dim row As DataRow
                Dim j As Integer = 0

                ' call function GetVendorForList
                listAccEnt = objAccountingEnt.GetYearList()

                With GetYearList
                    .Columns.Add("id")
                    .Columns.Add("name")

                    'loop year assign value to Dto
                    For Each values In listAccEnt
                        startYear = values.min_year - 1
                        currentYear = DateTime.Now.Year
                        If values.latest_year > currentYear Then
                            currentYear = values.latest_year
                        End If
                    Next

                    For i As Integer = startYear To currentYear - 1
                        row = .NewRow
                        row("id") = j
                        row("name") = startYear + 1
                        ' add data row to table
                        .Rows.Add(row)
                        startYear = startYear + 1
                        j += 1
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetYearList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAccountApprove
        '	Discription	    : Get status approve from accounting
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 09-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountApprove(ByVal strAccId As String) As System.Data.DataTable Implements IAccountingService.GetAccountApprove
            ' set default list
            GetAccountApprove = New DataTable
            Try
                ' objVendorDto for keep value Dto 
                Dim listAccEnt As New List(Of Entity.IAccountingEntity)
                Dim row As DataRow

                ' call function GetVendorForList
                listAccEnt = objAccountingEnt.GetAccountApprove(strAccId)

                With GetAccountApprove
                    .Columns.Add("id")
                    .Columns.Add("voucher_no")
                    .Columns.Add("status_id")

                    ' assign row from listAccEnt
                    For Each values In listAccEnt
                        row = .NewRow
                        row("id") = values.id
                        row("voucher_no") = values.voucher_no
                        row("status_id") = values.status_id

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccountApprove", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAcountApproveList
        '	Discription	    : Get Account Approve list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 08-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAcountApproveList( _
            ByVal objAccountingDto As Dto.AccountingDto _
        ) As System.Data.DataTable Implements IAccountingService.GetAcountApproveList
            ' set default
            GetAcountApproveList = New DataTable
            Try
                ' variable for keep list
                Dim listAccount As New List(Of Entity.ImpAccountingEntity)
                ' data row object
                Dim row As DataRow
                Dim strIssueDate As String = ""

                ' call function GetAcountApproveList from entity
                listAccount = objAccountingEnt.GetAcountApproveList(objAccountingDto)

                ' assign column header
                With GetAcountApproveList
                    .Columns.Add("id")
                    .Columns.Add("type")
                    .Columns.Add("account_type")
                    .Columns.Add("voucher_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("status")
                    .Columns.Add("status_id")
                    .Columns.Add("input_cheque")
                    .Columns.Add("issue_date")
                    .Columns.Add("applied_by")

                    ' assign row from listAccount
                    For Each values In listAccount
                        row = .NewRow
                        row("id") = values.id
                        row("type") = values.type
                        row("account_type") = values.accType
                        row("voucher_no") = values.voucher_no
                        row("vendor_name") = values.strVendor_name
                        row("status") = values.approve_status
                        row("status_id") = values.status_id
                        row("input_cheque") = values.cheque_no
                        row("applied_by") = values.applied_by

                        If values.account_date <> Nothing Or values.account_date <> "" Then
                            strIssueDate = Left(values.account_date, 4) & "/" & Mid(values.account_date, 5, 2) & "/" & Right(values.account_date, 2)
                            row("issue_date") = CDate(strIssueDate).ToString("dd/MMM/yyyy")
                        Else
                            row("issue_date") = values.account_date
                        End If

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAcountApproveList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateAcountApprove
        '	Discription	    : Update Account Approve
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 08-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateAcountApprove( _
            ByVal strAcountId As String, _
            ByVal strAcountType As String, _
            ByVal intStatusId As Integer, _
            Optional ByVal objConn As Object = Nothing, _
            Optional ByVal flgTransaction As String = "", _
            Optional ByVal dtValues As System.Data.DataTable = Nothing, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IAccountingService.UpdateAcountApprove
            ' set default return value
            UpdateAcountApprove = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateAcountApprove from Item Entity
                intEff = objAccountingEnt.UpdateAcountApprove(strAcountId, _
                                                              strAcountType, _
                                                              intStatusId, _
                                                              objConn, _
                                                              flgTransaction, _
                                                              dtValues)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateAcountApprove = True
                Else
                    ' case row less than 1 then return False
                    UpdateAcountApprove = False
                End If

            Catch exSql As MySqlException
                strMsg = "KTAP_02_002"
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateAcountApprove(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetYearList
        '	Discription	    : Get Year for dropdownlist
        '	Return Value	: List
        '	Create User	    : Komsan L.
        '	Create Date	    : 03-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWIPYear() As System.Data.DataTable Implements IAccountingService.GetWIPYear
            ' set default list
            GetWIPYear = New DataTable
            Try
                ' objVendorDto for keep value Dto 
                Dim listAccEnt As New List(Of Entity.IAccountingEntity)
                Dim row As DataRow

                ' call function GetVendorForList
                listAccEnt = objAccountingEnt.GetWIPYear()

                With GetWIPYear
                    .Columns.Add("id")
                    .Columns.Add("year")

                    For Each values In listAccEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.min_year
                        row("year") = values.min_year
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWIPYear", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetCostTableDetailOverviewList
        '	Discription	    : Get Cost TableDetail Overview List
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAdvanceIncomeReport( _
            ByVal year As String _
        ) As System.Data.DataTable Implements IAccountingService.GetAdvanceIncomeReport
            ' set default
            GetAdvanceIncomeReport = New DataTable
            Try
                ' variable for keep list from Accounting entity
                Dim listAccountingEnt As New List(Of Entity.ImpMst_AccountingDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listAccountingEnt = objAccountingEnt.GetAdvanceIncomeReport(year)

                ' assign column header
                With GetAdvanceIncomeReport
                    .Columns.Add("id")
                    .Columns.Add("job_order")
                    .Columns.Add("customer")
                    .Columns.Add("jh_mold_amount")
                    .Columns.Add("jo_other_amount")
                    .Columns.Add("jho_total")
                    .Columns.Add("ji_mold_amount")
                    .Columns.Add("ratio")
                    .Columns.Add("ji_other_amount")
                    .Columns.Add("ji_total")
                    .Columns.Add("KTS_MTR_Currency")
                    .Columns.Add("KTS_MTR_THB")
                    .Columns.Add("KTS_INV")
                    .Columns.Add("KTC_MTR_Currency")
                    .Columns.Add("KTC_MTR_THB")
                    .Columns.Add("KTC_INV")
                    .Columns.Add("OTHER_MTR")
                    .Columns.Add("TotalMTR")
                    .Columns.Add("hh")

                    ' assign row from listAccountingEny
                    For Each values In listAccountingEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id
                        row("job_order") = values.job_order
                        row("customer") = values.customer
                        row("jh_mold_amount") = values.jh_mold_amount
                        row("jo_other_amount") = values.jo_other_amount
                        row("jho_total") = values.jho_total
                        row("ji_mold_amount") = values.ji_mold_amount
                        row("ratio") = values.ratio
                        row("ji_other_amount") = values.ji_other_amount
                        row("ji_total") = values.ji_total
                        row("KTS_MTR_Currency") = values.KTS_MTR_Currency
                        row("KTS_MTR_THB") = values.KTS_MTR_THB
                        row("KTS_INV") = values.KTS_INV
                        row("KTC_MTR_Currency") = values.KTC_MTR_Currency
                        row("KTC_MTR_THB") = values.KTC_MTR_THB
                        row("KTC_INV") = values.KTC_INV
                        row("OTHER_MTR") = values.OTHER_MTR
                        row("TotalMTR") = values.TotalMTR
                        row("hh") = values.hh

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAdvanceIncomeReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SearchIncomePayment
        '	Discription	    : Get Income Or Payment for search
        '	Return Value	: Datatable
        '	Create User	    : Wasan D.
        '	Create Date	    : 28-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SearchIncomePayment(ByVal intType As Integer, ByVal listPara As System.Collections.Generic.List(Of String)) As DataTable Implements IAccountingService.SearchIncomePayment
            SearchIncomePayment = New DataTable
            Try
                Dim objIncomeEnt As New List(Of Entity.ImpAccountingEntity)
                Dim sqlParameter() As String = {"id", "Job_Order", "Account_Type", "Vendor_Name", "Account_Title", "Account_Name", "Account_No", "Receipt_Date", "Sub_Total"}
                Dim dr As DataRow

                objIncomeEnt = objAccountingEnt.SearchIncomePayment(intType, listPara)
                If objIncomeEnt.Count <> 0 Then
                    ' Set datatable columns
                    For i = 0 To sqlParameter.Length - 1
                        SearchIncomePayment.Columns.Add(sqlParameter(i))
                    Next
                    ' Insert rows to datatable
                    For Each objIncomeTmp As Entity.IAccountingEntity In objIncomeEnt
                        dr = SearchIncomePayment.NewRow
                        With objIncomeTmp
                            dr(0) = .id
                            dr(1) = .job_order
                            dr(2) = .accType
                            dr(3) = .vendor_name
                            dr(4) = .ie_title
                            dr(5) = .account_name
                            dr(6) = .account_no
                            dr(7) = objUtility.String2Date(.account_date, "yyyyMMdd").ToString("dd/MMM/yyyy")
                            dr(8) = .sub_total
                        End With
                        SearchIncomePayment.Rows.Add(dr)
                    Next
                End If
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("SearchIncomePayment", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPoForDeleteList
        '	Discription	    : Get Account Approve list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 08-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPoForDeleteList( _
            ByVal strAcountId As String _
        ) As System.Data.DataTable Implements IAccountingService.GetPoForDeleteList
            ' set default
            GetPoForDeleteList = New DataTable
            Try
                ' variable for keep list
                Dim listAccount As New List(Of Entity.ImpAccountingEntity)
                ' data row object
                Dim row As DataRow
                Dim strIssueDate As String = ""

                ' call function GetPoForDeleteList from entity
                listAccount = objAccountingEnt.GetPoForDeleteList(strAcountId)

                ' assign column header
                With GetPoForDeleteList
                    .Columns.Add("job_order_id")
                    .Columns.Add("job_order_po_id")
                    .Columns.Add("hontai_type")
                    .Columns.Add("id")
                    .Columns.Add("type")
                    .Columns.Add("ref_id")
                    .Columns.Add("voucher_no")

                    ' assign row from listAccount
                    For Each values In listAccount
                        row = .NewRow
                        row("job_order_id") = values.job_order_id
                        row("job_order_po_id") = values.job_order_po_id
                        row("hontai_type") = values.hontai_type
                        row("id") = values.id
                        row("type") = values.type
                        row("ref_id") = values.ref_id
                        row("voucher_no") = values.voucher_no

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPoForDeleteList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAccountingWithID
        '	Discription	    : Get Income Or Payment for search
        '	Return Value	: Datatable
        '	Create User	    : Wasan D.
        '	Create Date	    : 29-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountingWithID(ByVal intAccID As Integer) As System.Data.DataTable Implements IAccountingService.GetAccountingWithID
            GetAccountingWithID = New DataTable
            Try
                Return objAccountingEnt.GetAccountingWithID(intAccID)
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("GetAccountingWithID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetListAccountTitle
        '	Discription	    : Get Income Or Payment for search
        '	Return Value	: Datatable
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 27-05-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetListAccountTitle(ByVal UserId As Integer) As List(Of Entity.ImpAccountingEntity) _
            Implements IAccountingService.GetListAccountTitle
            GetListAccountTitle = New List(Of Entity.ImpAccountingEntity)
            Try
                Return objAccountingEnt.GetListAccountTitle(UserId)
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("GetListAccountTitle(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region

#Region "Function ChequeApprove"
        '/**************************************************************
        '	Function name	: GetChequeApproveList
        '	Discription	    : Get Accounting list
        '	Return Value	: List
        '	Create User	    : Boonyarit
        '	Create Date	    : 08-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetChequeApproveList(ByVal objAccountingDto As Dto.AccountingDto) As System.Data.DataTable Implements IAccountingService.GetChequeApproveList
            ' set default
            GetChequeApproveList = New DataTable
            Try
                ' variable for keep list
                Dim listAccount As New List(Of Entity.ImpAccountingEntity)
                ' data row object
                Dim row As DataRow
                Dim strIssueDate As String = ""

                ' call function GetAcountApproveList from entity
                listAccount = objAccountingEnt.GetChequeApproveList(objAccountingDto)

                ' assign column header
                With GetChequeApproveList
                    .Columns.Add("newid")
                    .Columns.Add("account_type")
                    .Columns.Add("voucher_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("status")
                    .Columns.Add("status_id")
                    .Columns.Add("cheque_no")
                    .Columns.Add("applied_by")
                    .Columns.Add("account_date")
                    .Columns.Add("cheque_date")

                    ' assign row from listAccount
                    For Each values In listAccount
                        row = .NewRow
                        row("newid") = values.newid
                        row("account_type") = values.accType
                        row("voucher_no") = values.voucher_no
                        row("vendor_name") = values.vendor_name
                        row("status") = values.approve_status
                        row("status_id") = values.status_id
                        row("cheque_no") = values.cheque_no
                        row("applied_by") = values.applied_by

                        If values.account_date <> Nothing Or values.account_date <> "" Then
                            strIssueDate = Left(values.account_date, 4) & "/" & Mid(values.account_date, 5, 2) & "/" & Right(values.account_date, 2)
                            row("account_date") = CDate(strIssueDate).ToString("dd/MMM/yyyy")
                        Else
                            row("account_date") = values.account_date
                        End If
                        If values.cheque_date <> Nothing Or values.cheque_date <> "" Then
                            strIssueDate = Left(values.cheque_date, 4) & "/" & Mid(values.cheque_date, 5, 2) & "/" & Right(values.cheque_date, 2)
                            row("cheque_date") = CDate(strIssueDate).ToString("dd/MMM/yyyy")
                        Else
                            row("cheque_date") = values.cheque_date
                        End If

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetChequeApproveList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateChequeApprove
        '	Discription	    : Update Accounting list
        '	Return Value	: List
        '	Create User	    : Boonyarit
        '	Create Date	    : 08-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateChequeApprove(ByVal strNewId As String, ByVal strAcountType As String, ByVal intStatusId As Integer, Optional ByRef strMsg As String = "") As Boolean Implements IAccountingService.UpdateChequeApprove
            ' set default return value
            UpdateChequeApprove = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateAcountApprove from Item Entity
                intEff = objAccountingEnt.UpdateChequeApprove(strNewId, strAcountType, intStatusId)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateChequeApprove = True
                Else
                    ' case row less than 1 then return False
                    UpdateChequeApprove = False
                End If

            Catch exSql As MySqlException
                strMsg = "KTAP_02_002"
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateChequeApprove(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAccountApproveByVoucherNo
        '	Discription	    : GetAccountApproveByVoucherNo
        '	Return Value	: Datatable
        '	Create User	    : Wasan D.
        '	Create Date	    : 10-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountApproveByVoucherNo( _
        ByVal strVoucherNo As String, Optional ByVal strStatusID As String = Nothing) _
        As System.Data.DataTable Implements IAccountingService.GetAccountApproveByVoucherNo
            GetAccountApproveByVoucherNo = New DataTable
            Try
                Dim listAccAPEnt As New List(Of Entity.ImpAccountingEntity)
                Dim dr As DataRow
                Dim dtColumn() As String = {"account_type", "vendor_name", "bank", "accountname", "cheque_no", "account_date", _
                                            "job_order", "account_title", "sub_total", "vat_amount", "wt_amount", "remark"}
                listAccAPEnt = objAccountingEnt.GetAccountApproveByVoucherNo(strVoucherNo, strStatusID)
                If listAccAPEnt.Count > 0 Then
                    For Each strColumn As String In dtColumn
                        GetAccountApproveByVoucherNo.Columns.Add(strColumn)
                    Next
                    ' Set column data type
                    GetAccountApproveByVoucherNo.Columns("sub_total").DataType = System.Type.GetType("System.Decimal")
                    For Each accAPEnt As Entity.ImpAccountingEntity In listAccAPEnt
                        dr = GetAccountApproveByVoucherNo.NewRow
                        With accAPEnt
                            dr("account_type") = .account_type_text
                            dr("vendor_name") = .vendor_name
                            dr("bank") = .bank
                            dr("accountname") = .account_name
                            dr("cheque_no") = .cheque_no
                            dr("account_date") = .account_date
                            dr("job_order") = .job_order
                            dr("account_title") = .ie_title
                            dr("sub_total") = .sub_total
                            dr("vat_amount") = .vat_amount_text
                            dr("wt_amount") = .wt_amount_text
                            dr("remark") = .remark
                        End With
                        GetAccountApproveByVoucherNo.Rows.Add(dr)
                    Next
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccountApproveByVoucherNo(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region

        
    End Class
End Namespace

