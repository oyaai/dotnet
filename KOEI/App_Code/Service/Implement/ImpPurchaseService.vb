#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpPurchaseService
'	Class Discription	: Class of Purchase
'	Create User 		: Boonyarit
'	Create Date		    : 11-06-2013
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
Imports System.Globalization
Imports OfficeOpenXml
Imports System.IO
Imports OfficeOpenXml.Style
Imports System.Web.Configuration
Imports Extensions
Imports System.Data
Imports MySql.Data.MySqlClient
Imports Utils
#End Region

Namespace Service
    Public Class ImpPurchaseService
        Implements IPurchaseService

        Private objLog As New Common.Logs.Log

        '/**************************************************************
        '	Function name	: GetPurchaseForSearch(objValue, objDT)
        '	Discription	    : Get purchase for search
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 11-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPurchaseForSearch(ByVal objValue As Dto.PurchaseSearchDto, ByRef objDT As System.Data.DataTable) As Boolean Implements IPurchaseService.GetPurchaseForSearch
            Try
                ' variable
                Dim objPurchase As New Entity.ImpPurchaseEntity
                Dim objListPurchase As New List(Of Entity.ImpPurchaseEntity)

                GetPurchaseForSearch = False
                ' get data for search
                objListPurchase = objPurchase.SearchPurchase(objValue)
                ' check data result
                If objListPurchase Is Nothing Then Exit Function
                If objListPurchase.Count > 0 Then
                    objDT = SetPurchaseListToDT(objListPurchase)
                    GetPurchaseForSearch = True
                End If

            Catch ex As Exception
                ' Write error log
                GetPurchaseForSearch = False
                objLog.ErrorLog("GetPurchaseForSearch", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetPurchaseListToDT
        '	Discription	    : Set data purchase to datatable
        '	Return Value	: DataTable
        '	Create User	    : Boonyarit
        '	Create Date	    : 11-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetPurchaseListToDT(ByVal objValue As List(Of Entity.ImpPurchaseEntity)) As System.Data.DataTable
            Try
                ' variable
                Dim objDT As New System.Data.DataTable
                Dim objDR As System.Data.DataRow
                Dim objCom As New Common.Utilities.Utility

                SetPurchaseListToDT = Nothing

                With objDT.Columns
                    .Add("id")
                    .Add("po_type_text")
                    .Add("po_no")
                    .Add("vendor_name")
                    .Add("issue_date")
                    .Add("delivery_date")
                    .Add("sub_total")
                    .Add("status_id")
                    .Add("status")
                    '2013/09/26 Pranitda S. Start-Add
                    .Add("currency")
                    .Add("font_color")
                    .Add("delivery_fg")
                    '2013/09/26 Pranitda S. End-Add
                End With

                For Each objItem In objValue
                    With objItem
                        objDR = objDT.NewRow
                        objDR("id") = .id
                        objDR("po_type_text") = .po_type_text.Trim
                        objDR("po_no") = .po_no.Trim
                        objDR("vendor_name") = .vendor_name.Trim
                        objDR("issue_date") = .issue_date.ToDate.ToString("d-MMM-yyyy")
                        objDR("delivery_date") = .delivery_date.ToDate.ToString("d-MMM-yyyy")
                        objDR("sub_total") = objCom.FormatMoney(.sub_total)
                        objDR("status_id") = .status_id
                        objDR("status") = .status
                        '2013/09/26 Pranitda S. Start-Add
                        objDR("currency") = .currency
                        objDR("font_color") = .font_color
                        objDR("delivery_fg") = .delivery_fg
                        '2013/09/26 Pranitda S. End-Add
                        objDT.Rows.Add(objDR)
                    End With
                Next
                SetPurchaseListToDT = objDT

            Catch ex As Exception
                ' Write error log
                SetPurchaseListToDT = Nothing
                objLog.ErrorLog("SetPurchaseListToDT", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckPurchase
        '	Discription	    : Check data purchase for delete
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckPurchase(ByVal intPurchaseId As Integer) As Boolean Implements IPurchaseService.CheckPurchase
            Try
                ' variable
                Dim objAccount As New Entity.ImpAccountingEntity
                Dim objPayment As New Entity.ImpPayment_HeaderEntity

                CheckPurchase = False
                ' check data
                If intPurchaseId < 1 Then Exit Function
                'If objAccount.CheckAccountByPurchase(intPurchaseId) Then Exit Function
                If objPayment.CheckPaymentByPurchase(intPurchaseId) Then Exit Function
                CheckPurchase = True

            Catch ex As Exception
                ' Write error log
                CheckPurchase = False
                objLog.ErrorLog("CheckPurchase", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeletePurchase
        '	Discription	    : Delete data purchase
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeletePurchase(ByVal intPurchaseId As Integer) As Boolean Implements IPurchaseService.DeletePurchase
            Try
                ' variable
                Dim objPurchase As New Entity.ImpPurchaseEntity

                DeletePurchase = False
                ' check data will delete or cancel
                If intPurchaseId < 1 Then Exit Function
                If objPurchase.DeletePurchase(intPurchaseId) Then
                    ' check data result
                    DeletePurchase = True
                End If

            Catch ex As Exception
                ' Write error log
                DeletePurchase = False
                objLog.ErrorLog("DeletePurchase", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPurchaseForDetail
        '	Discription	    : Get data purchase and detail
        '	Return Value	: Dto.Purchase
        '	Create User	    : Boonyarit
        '	Create Date	    : 13-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPurchaseForDetail(ByVal intPurchaseId As Integer, ByRef objDT As System.Data.DataTable) As Dto.PurchaseDto Implements IPurchaseService.GetPurchaseForDetail
            Try
                ' variable
                Dim objPurchase As New Entity.ImpPurchaseEntity
                Dim objPurchaseDeatil As New Entity.ImpPurchaseEntity
                Dim objDetail As Dto.PurchaseDetailDto
                Dim objCom As New Common.Utilities.Utility

                GetPurchaseForDetail = Nothing

                If intPurchaseId < 1 Then Exit Function
                objPurchaseDeatil = objPurchase.SearchPurchaseDetail(intPurchaseId)

                GetPurchaseForDetail = New Dto.PurchaseDto
                With GetPurchaseForDetail
                    '.po_type
                    .po_type = objPurchaseDeatil.po_type
                    '.po_type_text
                    .po_type_text = CheckStrNull(objPurchaseDeatil.po_type_text).Trim
                    '.po_no
                    .po_no = CheckStrNull(objPurchaseDeatil.po_no).Trim
                    '.delivery_date
                    .delivery_date = objPurchaseDeatil.delivery_date.ToDate.ToString("d-MMM-yyyy")
                    '.quotation_no
                    .quotation_no = CheckStrNull(objPurchaseDeatil.quotation_no).Trim
                    '.vendor_id
                    .vendor_id = objPurchaseDeatil.vendor_id
                    '.vendor_name
                    .vendor_name = CheckStrNull(objPurchaseDeatil.vendor_name).Trim
                    '.payment_term_id
                    .payment_term_id = objPurchaseDeatil.payment_term_id
                    '.payment_term_text
                    .payment_term_name = CheckStrNull(objPurchaseDeatil.payment_term_text).Trim
                    '.vat_id
                    .vat_id = objPurchaseDeatil.vat_id
                    '.vat_text
                    .vat_name = CheckStrNull(objPurchaseDeatil.vat_text).Trim
                    '.wt_id
                    .wt_id = objPurchaseDeatil.wt_id
                    '.wt_text
                    .wt_name = CheckStrNull(objPurchaseDeatil.wt_text).Trim
                    '.currency_id
                    .currency_id = objPurchaseDeatil.currency_id
                    '.currency_name
                    .currency_name = CheckStrNull(objPurchaseDeatil.currency_name).Trim
                    '.remark
                    .remarks = CheckStrNull(objPurchaseDeatil.remark).Trim
                    '.attn
                    .attn = CheckStrNull(objPurchaseDeatil.attn).Trim
                    '.deliver_to
                    .deliver_to = CheckStrNull(objPurchaseDeatil.deliver_to).Trim
                    '.contact
                    .contact = CheckStrNull(objPurchaseDeatil.contact).Trim
                    '.sub_total
                    .sub_total = objPurchaseDeatil.sub_total
                    '.discount_total
                    .discount_total = objPurchaseDeatil.discount_total
                    '.vat_amount
                    .vat_amount = objPurchaseDeatil.vat_amount
                    '.wt_amount
                    .wt_amount = objPurchaseDeatil.wt_amount
                    '.total_amount
                    .total_amount = objPurchaseDeatil.total_amount
                End With

                If (Not objPurchaseDeatil.purchase_detail Is Nothing) AndAlso objPurchaseDeatil.purchase_detail.Count > 0 Then
                    objDT = SetPurchaseDetailListToDT(objPurchaseDeatil.purchase_detail)
                    GetPurchaseForDetail.purchase_detail = New List(Of Dto.PurchaseDetailDto)
                    For Each objItem In objPurchaseDeatil.purchase_detail
                        objDetail = New Dto.PurchaseDetailDto
                        With objDetail
                            '.item_id
                            .item_id = objItem.item_id
                            '.item_name
                            .item_name = CheckStrNull(objItem.item_name)
                            '.job_order
                            .job_order = CheckStrNull(objItem.job_order)
                            '.ie_id
                            .ie_id = objItem.ie_id
                            '.ie_name
                            .ie_name = CheckStrNull(objItem.ie_name)
                            '.unit_price
                            .unit_price = objItem.unit_price
                            '.quantity
                            .qty = objItem.quantity
                            '.unit_id
                            .unit_id = objItem.unit_id
                            '.unit_name
                            .unit_name = CheckStrNull(objItem.unit_name)
                            '.discount
                            .discount = objItem.discount
                            '.discount_type
                            .discount_type = objItem.discount_type
                            '.discount_type_text
                            .discount_type_text = CheckStrNull(objItem.discount_type_text)
                            '.vat_amount
                            .vat = objItem.vat_amount
                            '.wt_amount
                            .wt = objItem.wt_amount
                            '.amount
                            .amount = objItem.amount
                            '.remark
                            .remarks = CheckStrNull(objItem.remark)
                        End With
                        GetPurchaseForDetail.purchase_detail.Add(objDetail)
                    Next
                End If

            Catch ex As Exception
                ' Write error log
                GetPurchaseForDetail = Nothing
                objLog.ErrorLog("GetPurchaseForDetail", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetPurchaseDetailListToDT
        '	Discription	    : Set data purchase_detail to datatable
        '	Return Value	: DataTable
        '	Create User	    : Boonyarit
        '	Create Date	    : 13-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetPurchaseDetailListToDT(ByVal objValue As List(Of Entity.ImpPurchaseDetailEntity)) As System.Data.DataTable
            Try
                ' variable
                Dim objDT As New System.Data.DataTable
                Dim objDR As System.Data.DataRow
                Dim objCom As New Common.Utilities.Utility

                SetPurchaseDetailListToDT = Nothing

                With objDT.Columns
                    '.item_id
                    .Add("item_id")
                    '.item_name
                    .Add("item_name")
                    '.job_order
                    .Add("job_order")
                    '.ie_id
                    .Add("ie_id")
                    '.ie_name
                    .Add("ie_name")
                    '.unit_price
                    .Add("unit_price")
                    '.quantity
                    .Add("quantity")
                    '.unit_id
                    .Add("unit_id")
                    '.unit_name
                    .Add("unit_name")
                    '.discount
                    .Add("discount")
                    '.discount_type
                    .Add("discount_type")
                    '.discount_type_text
                    .Add("discount_type_text")
                    '.vat_amount
                    .Add("vat_amount")
                    '.wt_amount
                    .Add("wt_amount")
                    '.amount
                    .Add("amount")
                    '.remark
                    .Add("remark")
                End With

                For Each objItem In objValue
                    With objItem
                        objDR = objDT.NewRow
                        '.item_id
                        objDR("item_id") = .item_id
                        '.item_name
                        objDR("item_name") = .item_name
                        '.job_order
                        objDR("job_order") = .job_order
                        '.ie_id
                        objDR("ie_id") = .ie_id
                        '.ie_name
                        objDR("ie_name") = .ie_name
                        '.unit_price
                        objDR("unit_price") = objCom.FormatNumeric(.unit_price, 2, True)
                        '.quantity
                        objDR("quantity") = objCom.FormatNumeric(.quantity, 0, True)
                        '.unit_id
                        objDR("unit_id") = .unit_id
                        '.unit_name
                        objDR("unit_name") = .unit_name
                        '.discount
                        objDR("discount") = objCom.FormatNumeric(.discount, 2, True)
                        '.discount_type
                        objDR("discount_type") = .discount_type
                        '.discount_type_text
                        objDR("discount_type_text") = .discount_type_text
                        '.vat_amount
                        objDR("vat_amount") = objCom.FormatNumeric(.vat_amount, 2, True)
                        '.wt_amount
                        objDR("wt_amount") = objCom.FormatNumeric(.wt_amount, 2, True)
                        '.amount
                        objDR("amount") = objCom.FormatNumeric(.amount, 2, True)
                        '.remark
                        objDR("remark") = .remark
                        objDT.Rows.Add(objDR)
                    End With
                Next
                SetPurchaseDetailListToDT = objDT

            Catch ex As Exception
                ' Write error log
                SetPurchaseDetailListToDT = Nothing
                objLog.ErrorLog("SetPurchaseListToDT", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDiscountType
        '	Discription	    : Set list discount_type to dropdownlist
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetDiscountType(ByRef objValue As System.Web.UI.WebControls.DropDownList, Optional ByVal strCurrencyName As String = "Currency Name") As Boolean Implements IPurchaseService.SetDiscountType
            Try
                ' variable
                Dim objList As ListItem
                SetDiscountType = False

                '0=No Discount 
                '1=Currency Name
                '2=Percent(%)
                With objValue
                    objValue.Items.Clear()
                    objList = New ListItem
                    objList.Text = "No Discount"
                    objList.Value = "0"
                    objValue.Items.Add(objList)
                    objList = New ListItem
                    If strCurrencyName <> String.Empty Then
                        objList.Text = strCurrencyName.Trim
                    Else
                        objList.Text = "Currency Name"
                    End If
                    objList.Value = "1"
                    objValue.Items.Add(objList)
                    objList = New ListItem
                    objList.Text = "Percent(%)"
                    objList.Value = "2"
                    objValue.Items.Add(objList)
                End With
                SetDiscountType = True

            Catch ex As Exception
                ' Write error log
                SetDiscountType = False
                objLog.ErrorLog("SetDiscountType", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetItemList
        '	Discription	    : Set list item_name to dropdownlist
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 31-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetItemList(ByRef objValue As System.Web.UI.WebControls.DropDownList, Optional ByVal strVendorId As String = Nothing) As Boolean Implements IPurchaseService.SetItemList
            SetItemList = False
            Try
                Dim objComm As New Common.Utilities.Utility
                Dim objListItemEnt As New List(Of Entity.ImpMst_ItemEntity)
                Dim objItemEnt As New Entity.ImpMst_ItemEntity
                objListItemEnt = objItemEnt.GetListItem(strVendorId)
                If (Not objListItemEnt Is Nothing) AndAlso objListItemEnt.Count > 0 Then
                    Call objComm.LoadList(objValue, objListItemEnt, "name", "id", True, "")
                    objValue.SelectedIndex = 0
                    SetItemList = True
                Else
                    objValue.Items.Clear()
                    objValue.SelectedIndex = 0
                End If
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("SetItemList", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetAllDDL
        '	Discription	    : Set all object dropdownlist
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetAllDDL(ByRef objValue As System.Collections.Generic.List(Of System.Web.UI.WebControls.DropDownList), _
                                  ByRef thbIndex As String) As Boolean Implements IPurchaseService.SetAllDDL
            Try
                'With objValueDDL
                '0    .Add(ddlCurrency)
                '1    .Add(ddlDiscountType)
                '3/10/2013 Ping Start-Mod
                '    .Add(ddlIE)
                '3/10/2013 Ping End-Mod
                '2013/10/2013 Pranitda S. Start-Mod
                '    .Add(ddlItem)
                '2013/10/2013 Pranitda S. End-Mod
                '2    .Add(ddlPayTerm)
                '3    .Add(ddlUnit)
                '4    .Add(ddlVat)
                '2013/10/2013 Pranitda S. Start-Mod
                '    .Add(ddlVendor)
                '2013/10/2013 Pranitda S. End-Mod
                '5    .Add(ddlWT)
                'End With
                Dim boolCheck As Boolean = True
                Dim objCurrency As New ImpCurrencyService
                'Dim objIE As New ImpIEService
                'Dim objItem As New ImpItemService
                Dim objPaymentTerm As New ImpPaymentTermService
                Dim objUnit As New ImpUnitService
                Dim objVat As New ImpVatService
                'Dim objVendor As New ImpVendorService
                Dim objWT As New ImpWTService

                Dim objComm As New Common.Utilities.Utility

                'Currency
                'If objCurrency.SetListCurrency(objValue(0)) = False Then
                '    If boolCheck = True Then boolCheck = False
                'End If
                Dim objListCurrencyDto As New List(Of Dto.CurrencyDto)
                objListCurrencyDto = objCurrency.GetCurrencyForDropdownList

                '2013/09/27 Pranitda S. Start-Add
                If objListCurrencyDto.Item(0).name = "THB" Then
                    thbIndex = objListCurrencyDto.Item(0).id
                End If
                '2013/09/27 Pranitda S. End-Add

                If (Not objListCurrencyDto Is Nothing) AndAlso objListCurrencyDto.Count > 0 Then
                    Call objComm.LoadList(objValue(0), objListCurrencyDto, "name", "id", True, "")
                Else
                    If boolCheck = True Then boolCheck = False
                End If

                'Discount Type
                If SetDiscountType(objValue(1), objValue(0).Items(1).Text) = False Then
                    If boolCheck = True Then boolCheck = False
                End If

                

                'I/E
                'If objIE.SetListIE(objValue(2)) = False Then
                '    If boolCheck = True Then boolCheck = False
                'End If
                '3/09/27 Ping Start-Add
                'Dim objListIEDto As New List(Of Dto.IEDto)
                'objListIEDto = objIE.GetIEListForDropdownList(True)
                'If (Not objListIEDto Is Nothing) AndAlso objListIEDto.Count > 0 Then
                '    Call objComm.LoadList(objValue(2), objListIEDto, "Name", "ID", True, "")
                'Else
                '    If boolCheck = True Then boolCheck = False
                'End If
                '3/09/27 Ping End-Add

                'Item
                'If objItem.SetListItem(objValue(3)) = False Then
                '    If boolCheck = True Then boolCheck = False
                'End If
                '2013/10/2013 Pranitda S. Start-Mod
                'Dim objListItemDto As New List(Of Dto.ItemDto)
                'objListItemDto = objItem.GetItemListForDropdownList
                'If (Not objListItemDto Is Nothing) AndAlso objListItemDto.Count > 0 Then
                '    Call objComm.LoadList(objValue(3), objListItemDto, "name", "id", True, "")
                'Else
                '    If boolCheck = True Then boolCheck = False
                'End If
                '2013/10/2013 Pranitda S. End-Mod

                'PaymentTerm
                'If objPaymentTerm.SetListPaymentDay(objValue(4)) = False Then
                '    If boolCheck = True Then boolCheck = False
                'End If
                Dim objListPaymentTermDto As New List(Of Dto.IPayment_TermDto)
                objListPaymentTermDto = objPaymentTerm.GetPaymentDayForList
                If (Not objListPaymentTermDto Is Nothing) AndAlso objListPaymentTermDto.Count > 0 Then
                    Call objComm.LoadList(objValue(2), objListPaymentTermDto, "term_day", "id", True, "")
                Else
                    If boolCheck = True Then boolCheck = False
                End If

                'Unit
                'If objUnit.SetListUnit(objValue(5)) = False Then
                '    If boolCheck = True Then boolCheck = False
                'End If
                Dim objListUnitDto As New List(Of Dto.UnitDto)
                objListUnitDto = objUnit.GetUnitListForDropdownList
                If (Not objListUnitDto Is Nothing) AndAlso objListUnitDto.Count > 0 Then
                    Call objComm.LoadList(objValue(3), objListUnitDto, "name", "id", True, "")
                Else
                    If boolCheck = True Then boolCheck = False
                End If

                'Vat
                'If objVat.SetListVat(objValue(6)) = False Then
                '    If boolCheck = True Then boolCheck = False
                'End If
                Dim objListVatDto As New List(Of Dto.VatDto)
                objListVatDto = objVat.GetVatForList
                If (Not objListVatDto Is Nothing) AndAlso objListVatDto.Count > 0 Then
                    Call objComm.LoadList(objValue(4), objListVatDto, "Percent", "ID", True, "")
                Else
                    If boolCheck = True Then boolCheck = False
                End If

                'Vandor
                'If objVendor.SetListVendorName(objValue(7)) = False Then
                '    If boolCheck = True Then boolCheck = False
                'End If
                '2013/10/2013 Pranitda S. Start-Mod
                'Dim objListVendorDto As New List(Of Dto.VendorDto)
                'objListVendorDto = objVendor.GetVendorForList(0)
                'If (Not objListVendorDto Is Nothing) AndAlso objListVendorDto.Count > 0 Then
                '    Call objComm.LoadList(objValue(7), objListVendorDto, "name", "id", True, "")
                'Else
                '    If boolCheck = True Then boolCheck = False
                'End If
                '2013/10/2013 Pranitda S. End-Mod

                'W/T
                'If objWT.SetListWT(objValue(8)) = False Then
                '    If boolCheck = True Then boolCheck = False
                'End If
                Dim objListWTDto As New List(Of Dto.WTDto)
                objListWTDto = objWT.GetWTForList
                If (Not objListWTDto Is Nothing) AndAlso objListWTDto.Count > 0 Then
                    Call objComm.LoadList(objValue(5), objListWTDto, "Percent", "ID", True, "")
                Else
                    If boolCheck = True Then boolCheck = False
                End If

                SetAllDDL = boolCheck

            Catch ex As Exception
                ' Write error log
                SetAllDDL = False
                objLog.ErrorLog("SetAllDDL", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPurchaseForReport
        '	Discription	    : Get and print data for report
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPurchaseForReport(ByVal objValue As Dto.PurchaseSearchDto) As Boolean Implements IPurchaseService.GetPurchaseForReport
            Try
                ' variable
                Dim objPurchase As New Entity.ImpPurchaseEntity
                Dim objListPurchase As New List(Of Entity.ImpPurchaseReportEntity)

                GetPurchaseForReport = False
                ' assign object purchase search dto
                objListPurchase = objPurchase.SearchPurchaseReport(objValue)
                ' check data result
                If objListPurchase Is Nothing Then Exit Function
                If objListPurchase.Count > 0 Then
                    GetPurchaseForReport = ExcelPurchaseOrderList(objListPurchase)
                End If

            Catch ex As Exception
                ' Write error log
                GetPurchaseForReport = False
                objLog.ErrorLog("GetPurchaseForReport", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: ExcelPurchaseOrderList
        '	Discription	    : Export data purchase for report
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function ExcelPurchaseOrderList(ByVal objListPurchase As List(Of Entity.ImpPurchaseReportEntity)) As Boolean
            Try
                ' variable
                Dim pck As ExcelPackage = Nothing
                Dim wBook As OfficeOpenXml.ExcelWorksheet = Nothing
                Dim rowCount As Integer = 0
                Dim intIndexRow As Integer = 4
                Dim intMaxCol As Integer = 29
                Dim objComm As New Common.Utilities.Utility
                Dim strDate As String = String.Empty

                ExcelPurchaseOrderList = False

                Dim strPath As String = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("ReportPath") & "PurchaseOrderList.xlsx")

                If objListPurchase.Count < 1 Then Exit Function
                pck = New ExcelPackage(New MemoryStream(), New MemoryStream(File.ReadAllBytes(strPath)))
                wBook = pck.Workbook.Worksheets(1)

                With wBook
                    For i = 0 To objListPurchase.Count - 1
                        'Data Detail

                        If i > 0 Then
                            If (objListPurchase(i).issue_date.ToString <> objListPurchase(i - 1).issue_date.ToString) Or (objListPurchase(i).po_no <> objListPurchase(i - 1).po_no) _
                             Or (objListPurchase(i).vendor_name.Trim <> objListPurchase(i - 1).vendor_name.Trim) Or (objListPurchase(i).po_type_text <> objListPurchase(i - 1).po_type_text) Then
                                'issue_date
                                strDate = objListPurchase(i).issue_date.ToDate.ToString("d-MMM-yyyy")
                                .Cells((intIndexRow + i), 1).Value = strDate.Trim
                                'po_no
                                .Cells((intIndexRow + i), 2).Value = objListPurchase(i).po_no.Trim
                                'vendor_name
                                .Cells((intIndexRow + i), 3).Value = objListPurchase(i).vendor_name.Trim
                                'po_type_text
                                .Cells((intIndexRow + i), 4).Value = objListPurchase(i).po_type_text.Trim

                            End If

                        Else
                            'issue_date
                            strDate = objListPurchase(i).issue_date.ToDate.ToString("d-MMM-yyyy")
                            .Cells((intIndexRow + i), 1).Value = strDate.Trim
                            'po_no
                            .Cells((intIndexRow + i), 2).Value = objListPurchase(i).po_no.Trim
                            'vendor_name
                            .Cells((intIndexRow + i), 3).Value = objListPurchase(i).vendor_name.Trim
                            'po_type_text
                            .Cells((intIndexRow + i), 4).Value = objListPurchase(i).po_type_text.Trim

                        End If

                        'ie_name
                        .Cells((intIndexRow + i), 5).Value = objListPurchase(i).ie_name
                        'item_name
                        .Cells((intIndexRow + i), 6).Value = objListPurchase(i).item_name.Trim
                        ''quantity
                        '.Cells((intIndexRow + i), 7).Value = objComm.FormatNumeric(objListPurchase(i).quantity, 0, True)
                        ''unit_name
                        '.Cells((intIndexRow + i), 8).Value = objListPurchase(i).unit_name.Trim
                        ''unit_price
                        '.Cells((intIndexRow + i), 9).Value = objComm.FormatNumeric(objListPurchase(i).unit_price, 2, True)
                        ''discount_amt
                        '.Cells((intIndexRow + i), 10).Value = objComm.FormatNumeric(objListPurchase(i).discount_amt, 2, True)
                        ''amount
                        '.Cells((intIndexRow + i), 11).Value = objComm.FormatNumeric(objListPurchase(i).amount, 2, True)
                        ''vat_amount
                        '.Cells((intIndexRow + i), 12).Value = objComm.FormatNumeric(objListPurchase(i).vat_amount, 2, True)

                        'quantity
                        .Cells((intIndexRow + i), 7).Value = objListPurchase(i).quantity
                        'unit_name
                        .Cells((intIndexRow + i), 8).Value = objListPurchase(i).unit_name.Trim
                        'unit_price
                        .Cells((intIndexRow + i), 9).Value = objListPurchase(i).unit_price
                        'discount_amt
                        .Cells((intIndexRow + i), 10).Value = objListPurchase(i).discount_amt
                        'amount
                        .Cells((intIndexRow + i), 11).Value = objListPurchase(i).amount
                        'vat_amount
                        .Cells((intIndexRow + i), 12).Value = objListPurchase(i).vat_amount

                        'start add by Pranitda S. 2013/09/20
                        .Cells((intIndexRow + i), 13).Value = objListPurchase(i).job_order 'job_order
                        .Cells((intIndexRow + i), 14).Value = objListPurchase(i).name

                        strDate = objListPurchase(i).delivery_date.ToDate.ToString("d-MMM-yyyy")
                        .Cells((intIndexRow + i), 15).Value = strDate.Trim

                        strDate = CDate(objListPurchase(i).delivery_plan).ToString("d-MMM-yyyy")
                        .Cells((intIndexRow + i), 16).Value = objListPurchase(i).delivery_plan

                        .Cells((intIndexRow + i), 17).Value = objListPurchase(i).invoice_no
                        .Cells((intIndexRow + i), 18).Value = objListPurchase(i).remark
                        .Cells((intIndexRow + i), 19).Value = objListPurchase(i).quality50
                        .Cells((intIndexRow + i), 20).Value = objListPurchase(i).quality25
                        .Cells((intIndexRow + i), 21).Value = objListPurchase(i).quality0
                        .Cells((intIndexRow + i), 22).Value = objListPurchase(i).delivery30
                        .Cells((intIndexRow + i), 23).Value = objListPurchase(i).delivery15
                        .Cells((intIndexRow + i), 24).Value = objListPurchase(i).delivery0
                        .Cells((intIndexRow + i), 25).Value = objListPurchase(i).service20
                        .Cells((intIndexRow + i), 26).Value = objListPurchase(i).service10
                        .Cells((intIndexRow + i), 27).Value = objListPurchase(i).service0
                        .Cells((intIndexRow + i), 28).Value = objListPurchase(i).Score
                        .Cells((intIndexRow + i), 29).Value = objListPurchase(i).Grade
                        'end add by Pranitda S. 2013/09/20

                        'ทำการขีดเสีนให้ข้อมูล
                        For j As Integer = 1 To intMaxCol
                            .Cells((intIndexRow + i), j).Style.VerticalAlignment = ExcelVerticalAlignment.Top
                            .Cells((intIndexRow + i), j).Style.Border.Left.Style = ExcelBorderStyle.Thin
                            .Cells((intIndexRow + i), j).Style.Border.Top.Style = ExcelBorderStyle.Thin
                            .Cells((intIndexRow + i), j).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                        Next
                        .Cells((intIndexRow + i), intMaxCol).Style.Border.Right.Style = ExcelBorderStyle.Thin
                        If i = (objListPurchase.Count - 1) Then
                            For j As Integer = 1 To intMaxCol
                                .Cells((intIndexRow + i + 1), j).Style.Border.Top.Style = ExcelBorderStyle.Thin
                            Next
                        End If

                    Next
                    'Set data in Header and Footer
                    '.HeaderFooter.OddHeader.RightAlignedText = "Date: " & Now().ToString("d-MMM-yyyy hh:mm tt") & " Page: " & ExcelHeaderFooter.PageNumber
                    '.HeaderFooter.OddFooter.LeftAlignedText = "F-PC-002 (Approved Subcontractor and Supplier List : ASL)"
                    '.HeaderFooter.OddFooter.RightAlignedText = "Effective date : 01/06/09  Rev : 00"

                End With

                HttpContext.Current.Response.Clear()
                pck.SaveAs(HttpContext.Current.Response.OutputStream)
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=PurchaseOrderList.xlsx")
                HttpContext.Current.Response.End()

                ExcelPurchaseOrderList = True
            Catch ex As Exception
                ' Write error log
                ExcelPurchaseOrderList = False
                objLog.ErrorLog("ExcelPurchaseOrderList", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: SavePurchase
        '	Discription	    : Save data purchase
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SavePurchase(ByVal objPurchaseDto As Dto.PurchaseDto, ByVal strMode As String, Optional ByRef strPoNo_New As String = "", Optional ByRef intPoId_New As Integer = 0) As Boolean Implements IPurchaseService.SavePurchase
            Try
                Dim objPurchaseEntity As Entity.ImpPurchaseEntity

                If objPurchaseDto Is Nothing Then Exit Function

                objPurchaseEntity = ChangePurchaseDtoToEntity(objPurchaseDto)
                Select Case strMode
                    Case "Add"
                        If objPurchaseEntity.InsertPurchase(objPurchaseEntity, strPoNo_New, intPoId_New) > 0 Then
                            SavePurchase = True
                        Else
                            SavePurchase = False
                        End If

                    Case "Edit"
                        If objPurchaseEntity.UpdatePurchase(objPurchaseEntity) > 0 Then
                            strPoNo_New = objPurchaseEntity.po_no
                            intPoId_New = objPurchaseEntity.id
                            SavePurchase = True
                        Else
                            SavePurchase = False
                        End If

                    Case "Modify"
                        If objPurchaseEntity.ModifyPurchase(objPurchaseEntity) > 0 Then
                            strPoNo_New = objPurchaseEntity.po_no
                            intPoId_New = objPurchaseEntity.id
                            SavePurchase = True
                        Else
                            SavePurchase = False
                        End If
                End Select

            Catch ex As Exception
                ' Write error log
                SavePurchase = False
                objLog.ErrorLog("SavePurchase", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: ChangePurchaseDtoToEntity
        '	Discription	    : Change data purchase
        '	Return Value	: Entity.ImpPurchase
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function ChangePurchaseDtoToEntity(ByVal objPurchaseDto As Dto.PurchaseDto) As Entity.ImpPurchaseEntity
            Try
                Dim objCom As New Common.Utilities.Utility
                Dim objPurchaseEntity As New Entity.ImpPurchaseEntity
                Dim objDetailEntity As Entity.ImpPurchaseDetailEntity

                ChangePurchaseDtoToEntity = Nothing
                With objPurchaseEntity
                    '?id 
                    .id = objPurchaseDto.id
                    '?po_no 
                    .po_no = CheckStrNull(objPurchaseDto.po_no)
                    ',?po_type 
                    .po_type = objPurchaseDto.po_type
                    ',?vendor_id 
                    .vendor_id = objPurchaseDto.vendor_id
                    .vendor_branch_id = objPurchaseDto.vendor_branch_id
                    ',?quotation_no 
                    .quotation_no = CheckStrNull(objPurchaseDto.quotation_no)
                    ',?delivery_date 
                    .delivery_date = objCom.String2Date(objPurchaseDto.delivery_date).ToString("yyyyMMdd")
                    ',?payment_term_id 
                    .payment_term_id = objPurchaseDto.payment_term_id

                    ',?vat_id   '* ทำการปรับค่าเพื่อทำการบันทึกและแก้ไขข้อมูลในฐานข้อมูลได้
                    If objPurchaseDto.vat_id <> 0 Then
                        .vat_id = objPurchaseDto.vat_id
                    Else
                        .vat_id = 1
                    End If
                    ',?wt_id    '* ทำการปรับค่าเพื่อทำการบันทึกและแก้ไขข้อมูลในฐานข้อมูลได้
                    If objPurchaseDto.wt_id > 0 Then
                        .wt_id = objPurchaseDto.wt_id
                    Else
                        .wt_id = 1
                    End If

                    ',?currency_id 
                    .currency_id = objPurchaseDto.currency_id
                    ',?remark 
                    .remark = CheckStrNull(objPurchaseDto.remarks)
                    ',?attn 
                    .attn = CheckStrNull(objPurchaseDto.attn)
                    ',?deliver_to 
                    .deliver_to = CheckStrNull(objPurchaseDto.deliver_to)
                    ',?contact 
                    .contact = CheckStrNull(objPurchaseDto.contact)
                    ',?sub_total 
                    .sub_total = objPurchaseDto.sub_total
                    ',?discount_total 
                    .discount_total = objPurchaseDto.discount_total
                    ',?vat_amount 
                    .vat_amount = objPurchaseDto.vat_amount
                    ',?wt_amount 
                    .wt_amount = objPurchaseDto.wt_amount
                    ',?total_amount 
                    .total_amount = objPurchaseDto.total_amount
                    '.user_id = HttpContext.Current.Session("UserID")
                    If objPurchaseDto.approve_user = 0 Then
                        .user_id = HttpContext.Current.Session("UserID")
                    Else
                        .user_id = objPurchaseDto.approve_user
                    End If
                    .created_by = HttpContext.Current.Session("UserID")
                    .updated_by = HttpContext.Current.Session("UserID")

                End With

                If (Not objPurchaseDto.purchase_detail Is Nothing) AndAlso objPurchaseDto.purchase_detail.Count > 0 Then
                    objPurchaseEntity.purchase_detail = New List(Of Entity.ImpPurchaseDetailEntity)
                    For Each objDetail In objPurchaseDto.purchase_detail
                        objDetailEntity = New Entity.ImpPurchaseDetailEntity
                        With objDetailEntity
                            '?id '** ยกเลิกไม่ใส่ค่าให้ (Boon Change)
                            If objDetail.id.Substring(0, 1).Equals("A") Or objDetail.id.Substring(0, 1).Equals("E") Then
                                .id = objDetail.id.Substring(1)
                            Else
                                .id = objDetail.id
                            End If
                            '?po_header_id
                            .po_header_id = objPurchaseDto.id
                            ',?item_id
                            .item_id = objDetail.item_id
                            ',?job_order
                            .job_order = CheckStrNull(objDetail.job_order)
                            ',?ie_id
                            .ie_id = objDetail.ie_id
                            ',?unit_price
                            .unit_price = objDetail.unit_price
                            ',?quantity
                            .quantity = objDetail.qty
                            ',?unit_id
                            .unit_id = objDetail.unit_id
                            ',?discount
                            .discount = objDetail.discount
                            ',?discount_type
                            .discount_type = objDetail.discount_type
                            ',?remark
                            .remark = CheckStrNull(objDetail.remarks)
                            ',?amount
                            .amount = objDetail.amount
                            ',?amount * po_header.vat
                            .vat_amount = objDetail.vat
                            ',?amount * po_header.wt
                            .wt_amount = objDetail.wt
                            ',?user_id
                            .created_by = HttpContext.Current.Session("UserID")
                            .updated_by = HttpContext.Current.Session("UserID")

                        End With
                        objPurchaseEntity.purchase_detail.Add(objDetailEntity)
                    Next

                End If

                ChangePurchaseDtoToEntity = objPurchaseEntity

            Catch ex As Exception
                ' Write error log
                ChangePurchaseDtoToEntity = Nothing
                objLog.ErrorLog("ChangePurchaseDtoToEntity", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPurchaseById
        '	Discription	    : Get data purchase by id
        '	Return Value	: Dto.PurchaseDto
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPurchaseById(ByVal intPurchaseId As Integer) As Dto.PurchaseDto Implements IPurchaseService.GetPurchaseById
            Try
                Dim objPurchaseEntity As New Entity.ImpPurchaseEntity

                GetPurchaseById = Nothing
                If intPurchaseId = 0 Then Exit Function

                objPurchaseEntity = objPurchaseEntity.SearchPurchase(intPurchaseId)
                If objPurchaseEntity Is Nothing Then Exit Function

                GetPurchaseById = ChangePurchaseEntToDto(objPurchaseEntity)

            Catch ex As Exception
                ' Write error log
                GetPurchaseById = Nothing
                objLog.ErrorLog("GetPurchaseById", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckStrNull
        '	Discription	    : Chenge data string
        '	Return Value	: String
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function CheckStrNull(ByVal strValue As String) As String
            Try
                CheckStrNull = IIf(strValue Is Nothing, String.Empty, strValue)
            Catch ex As Exception
                ' Write error log
                CheckStrNull = String.Empty
                objLog.ErrorLog("CheckStrNull", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: ChangePurchaseEntToDto
        '	Discription	    : Chenge data purchase to Dto
        '	Return Value	: Dto.PurchaseDto
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function ChangePurchaseEntToDto(ByVal objPurchaseEnt As Entity.ImpPurchaseEntity) As Dto.PurchaseDto
            Try
                Dim objCom As New Common.Utilities.Utility
                Dim objPurchaeDto As New Dto.PurchaseDto
                Dim objDetailDto As Dto.PurchaseDetailDto

                ChangePurchaseEntToDto = Nothing
                With objPurchaeDto
                    'id As Integer
                    .id = objPurchaseEnt.id
                    'po_type As Integer
                    .po_type = objPurchaseEnt.po_type
                    'po_type_text As String
                    .po_type_text = CheckStrNull(objPurchaseEnt.po_type_text)
                    'po_no As String
                    .po_no = CheckStrNull(objPurchaseEnt.po_no)
                    'vendor_id As Integer
                    .vendor_id = objPurchaseEnt.vendor_id
                    .vendor_branch_id = objPurchaseEnt.vendor_branch_id
                    'vendor_name As String
                    .vendor_name = CheckStrNull(objPurchaseEnt.vendor_name)
                    .vendor_branch_name = CheckStrNull(objPurchaseEnt.vendor_branch_name)
                    'issue_date As String
                    .issue_date = objPurchaseEnt.issue_date.ToDate.ToString("dd/MM/yyyy")
                    'delivery_date As String
                    .delivery_date = objPurchaseEnt.delivery_date.ToDate.ToString("dd/MM/yyyy")
                    'sum_amount As Decimal
                    .sum_amount = objPurchaseEnt.sum_amount
                    'status_id As Integer
                    .status_id = objPurchaseEnt.status_id
                    'quotation_no As String
                    .quotation_no = CheckStrNull(objPurchaseEnt.quotation_no)
                    'payment_term_id As Integer
                    .payment_term_id = objPurchaseEnt.payment_term_id
                    'payment_term_name As String
                    .payment_term_name = CheckStrNull(objPurchaseEnt.payment_term_text)

                    'Fix Vat_id By Aey 2014/07/01
                    .vat_id = objPurchaseEnt.vat_id
                    .vat_name = CheckStrNull(objPurchaseEnt.vat_text)

                    'If objPurchaseEnt.vat_id = 1 Then
                    '    'vat_id As Integer
                    '    .vat_id = 0
                    '    'vat_name As String
                    '    .vat_name = String.Empty
                    'Else
                    '    'vat_id As Integer
                    '    .vat_id = objPurchaseEnt.vat_id
                    '    'vat_name As String
                    '    .vat_name = CheckStrNull(objPurchaseEnt.vat_text)
                    'End If

                    If objPurchaseEnt.wt_id = 1 Then
                        'wt_id As Integer
                        .wt_id = 0
                        'wt_name As String
                        .wt_name = String.Empty
                    Else
                        'wt_id As Integer
                        .wt_id = objPurchaseEnt.wt_id
                        'wt_name As String
                        .wt_name = CheckStrNull(objPurchaseEnt.wt_text)
                    End If

                    'currency_id As Integer
                    .currency_id = objPurchaseEnt.currency_id
                    'currency_name As String
                    .currency_name = CheckStrNull(objPurchaseEnt.currency_name)
                    'remarks As String
                    .remarks = CheckStrNull(objPurchaseEnt.remark)
                    'attn As String
                    .attn = CheckStrNull(objPurchaseEnt.attn)
                    'deliver_to As String
                    .deliver_to = CheckStrNull(objPurchaseEnt.deliver_to)
                    'contact As String
                    .contact = CheckStrNull(objPurchaseEnt.contact)
                    'sub_total As Decimal
                    .sub_total = objPurchaseEnt.sub_total
                    'discount_total As Decimal
                    .discount_total = objPurchaseEnt.discount_total
                    'vat_amount As Decimal
                    .vat_amount = objPurchaseEnt.vat_amount
                    'wt_amount As Decimal
                    .wt_amount = objPurchaseEnt.wt_amount
                    'total_amount As Decimal
                    .total_amount = objPurchaseEnt.total_amount
                    'user_id As Integer
                    .user_id = objPurchaseEnt.user_id

                    '*เพิ่มเติม ใช้ในการแสดงบนหน้าจอ Purchase
                    .schedule_rate = GetS_Rate(.currency_id, objPurchaseEnt.issue_date.ToString)
                    .thb_sub_total = (.sub_total * .schedule_rate)
                    .thb_discount_total = (.discount_total * .schedule_rate)
                    .thb_vat_amount = (.vat_amount * .schedule_rate)
                    .thb_wt_amount = (.wt_amount * .schedule_rate)
                    .thb_total_amount = (.total_amount * .schedule_rate)
                End With

                If (Not objPurchaseEnt.purchase_detail Is Nothing) AndAlso objPurchaseEnt.purchase_detail.Count > 0 Then
                    objPurchaeDto.purchase_detail = New List(Of Dto.PurchaseDetailDto)
                    For Each objDetail In objPurchaseEnt.purchase_detail
                        objDetailDto = New Dto.PurchaseDetailDto
                        With objDetailDto
                            'id As Integer
                            .id = objDetail.id
                            'po_header_id As Integer
                            .po_header_id = objDetail.po_header_id
                            'item_id As Integer
                            .item_id = objDetail.item_id
                            'item_name As String
                            .item_name = CheckStrNull(objDetail.item_name)
                            'job_order As String
                            .job_order = CheckStrNull(objDetail.job_order)
                            'ie_id As Integer
                            .ie_id = objDetail.ie_id
                            'ie_name As String
                            .ie_name = CheckStrNull(objDetail.ie_name)
                            'unit_price As Decimal
                            .unit_price = objDetail.unit_price
                            'qty As Integer
                            .qty = objDetail.quantity
                            'unit_id As Integer
                            .unit_id = objDetail.unit_id
                            'unit_name As String
                            .unit_name = CheckStrNull(objDetail.unit_name)
                            'discount As Decimal
                            .discount = objDetail.discount
                            'discount_type As Integer
                            .discount_type = objDetail.discount_type
                            'discount_type_text As String
                            .discount_type_text = CheckStrNull(objDetail.discount_type_text)
                            'vat As Decimal
                            .vat = objDetail.vat_amount
                            'wt As Decimal
                            .wt = objDetail.wt_amount
                            'amount As Decimal
                            .amount = objDetail.amount
                            'remarks As String
                            .remarks = CheckStrNull(objDetail.remark)

                            ',B.ITName As item_name
                            .item_name = CheckStrNull(objDetail.item_name)
                            ',C.IEName As ie_name
                            .ie_name = CheckStrNull(objDetail.ie_name)
                            ',D.UName As unit_name
                            .unit_name = CheckStrNull(objDetail.unit_name)
                            ',discount_type_text
                            .discount_type_text = CheckStrNull(objDetail.discount_type_text)

                        End With
                        objPurchaeDto.purchase_detail.Add(objDetailDto)
                    Next
                End If

                ChangePurchaseEntToDto = objPurchaeDto

            Catch ex As Exception
                ' Write error log
                ChangePurchaseEntToDto = Nothing
                objLog.ErrorLog("ChangePurchaseEntToDto", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPoNo
        '	Discription	    : Get data purchase po_no
        '	Return Value	: String
        '	Create User	    : Boonyarit
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPoNo() As String Implements IPurchaseService.GetPoNo
            Try
                Dim objPurchase As New Entity.ImpPurchaseEntity

                GetPoNo = objPurchase.GetPoNo

            Catch ex As Exception
                ' Write error log
                GetPoNo = String.Empty
                objLog.ErrorLog("GetPoNo", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDataReport
        '	Discription	    : Get data purchase for report_pdf
        '	Return Value	: DataSet
        '	Create User	    : Boonyarit
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDataReport(ByVal intPurchaseId As Integer) As System.Data.DataSet Implements IPurchaseService.GetDataReport
            Try
                Dim objPurchase As New Entity.ImpPurchaseEntity
                Dim objDataPurchase As New Entity.ImpPurchasePDFEntty


                GetDataReport = Nothing
                If intPurchaseId < 1 Then Exit Function

                objDataPurchase = objPurchase.SearchPurchasePDF(intPurchaseId)

                GetDataReport = ChangeDataToDataSet(objDataPurchase)

            Catch ex As Exception
                ' Write error log
                GetDataReport = Nothing
                objLog.ErrorLog("GetDataReport", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: ChangeDataToDataSet
        '	Discription	    : Change data entity to dataset
        '	Return Value	: DataSet
        '	Create User	    : Boonyarit
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function ChangeDataToDataSet(ByVal objValue As Entity.ImpPurchasePDFEntty) As System.Data.DataSet
            Try
                Dim objDataSet As New System.Data.DataSet("KTPU01DataSet")
                Dim objDataDT1 As New System.Data.DataTable
                Dim objDataDT2 As New System.Data.DataTable
                Dim objDR As System.Data.DataRow
                Dim intGRP As Integer
                Dim intPOHead As Integer

                Dim iTmp As Double
                Dim iTmpText As String

                ChangeDataToDataSet = Nothing
                Call SetHeadTable(objDataDT1, objDataDT2)
                objDataDT1.TableName = "Purchase"
                objDataDT2.TableName = "PurchaseDetail"

                With objValue
                    objDR = objDataDT1.NewRow
                    'id
                    objDR("id") = .id
                    'vendor_name()
                    objDR("vendor_name") = .vendor_name
                    'address()
                    objDR("address") = .address
                    'zipcode()
                    objDR("zipcode") = .zipcode
                    'tel()
                    objDR("tel") = .tel
                    'fax()
                    objDR("fax") = .fax
                    'attn()
                    objDR("attn") = .attn
                    'po_no()
                    objDR("po_no") = .po_no
                    '.currency
                    objDR("currency") = .currency
                    'payment_term_text()
                    objDR("payment_term_text") = .payment_term_text
                    'quotation_no()
                    objDR("quotation_no") = .quotation_no
                    'issue_date()
                    objDR("issue_date") = .issue_date
                    'discount_type_text
                    objDR("discount_type_text") = .discount_type_text
                    'discount_total()
                    objDR("discount_total") = .discount_total
                    'sub_total()
                    objDR("sub_total") = .sub_total
                    'vat_amount()
                    objDR("vat_amount") = .vat_amount
                    'total_amount()
                    objDR("total_amount") = .total_amount
                    'remark()
                    objDR("remark") = .remark


                    'amount_text()
                    'objDR("amount_text") = .amount_text
                    'iTmp = .total_amount
                    iTmp = .sub_total + .vat_amount
                    If .currency = "THB" Then
                        'amount_text()
                        iTmpText = Utils.BahtText(iTmp)
                        .amount_text = iTmpText
                        objDR("amount_text") = iTmpText
                    Else
                        'amount_text()
                        iTmpText = Utils.DollarText(iTmp)
                        .amount_text = iTmpText
                        objDR("amount_text") = iTmpText
                    End If

                    'deliver_to()
                    objDR("deliver_to") = .deliver_to
                    'delivery_date()
                    objDR("delivery_date") = .delivery_date
                    'contact()
                    objDR("contact") = .contact
                    objDataDT1.Rows.Add(objDR)
                End With

                If (Not objValue.purchase_detail Is Nothing) AndAlso objValue.purchase_detail.Count > 0 Then
                    For Each objItem In objValue.purchase_detail
                        With objItem
                            objDR = objDataDT2.NewRow
                            'grp
                            intGRP = .grp
                            objDR("grp") = .grp
                            'po_header_id
                            intPOHead = .po_header_id
                            objDR("po_header_id") = .po_header_id
                            'no()
                            objDR("no") = .no
                            'item_name()
                            objDR("item_name") = .item_name
                            'quantity()
                            objDR("quantity") = .quantity
                            'unit_name()
                            objDR("unit_name") = .unit_name
                            'unit_price()
                            objDR("unit_price") = .unit_price
                            'amount()
                            objDR("amount") = .amount
                            'discount_type_text()
                            objDR("discount_type_text") = .discount_type_text
                            objDataDT2.Rows.Add(objDR)
                        End With
                    Next

                    If (objValue.purchase_detail.Count Mod 8) <> 0 Then
                        Dim intNullRow As Integer = 8 - (objValue.purchase_detail.Count Mod 8)
                        For iRow As Integer = 1 To intNullRow
                            objDR = objDataDT2.NewRow
                            'grp
                            objDR("grp") = intGRP
                            'po_header_id
                            objDR("po_header_id") = intPOHead
                            ''no()
                            'objDR("no") = " "
                            ''item_name()
                            'objDR("item_name") = "..."
                            ''quantity()
                            'objDR("quantity") = " "
                            ''unit_name()
                            'objDR("unit_name") = "..."
                            ''unit_price()
                            'objDR("unit_price") = " "
                            ''amount()
                            'objDR("amount") = " "
                            ''discount_type_text()
                            'objDR("discount_type_text") = " "
                            objDataDT2.Rows.Add(objDR)
                        Next
                    End If
                End If

                objDataSet.Tables.Add(objDataDT1)
                objDataSet.Tables.Add(objDataDT2)

                ChangeDataToDataSet = objDataSet


            Catch ex As Exception
                ' Write error log
                ChangeDataToDataSet = Nothing
                objLog.ErrorLog("ChangeDataToDataSet", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetHeadTable
        '	Discription	    : Set Head Columns
        '	Return Value	: DataSet
        '	Create User	    : Boonyarit
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub SetHeadTable(ByRef objDT1 As System.Data.DataTable, ByRef objDT2 As System.Data.DataTable)
            Try
                With objDT1.Columns
                    'id
                    .Add("id")
                    'vendor_name()
                    .Add("vendor_name")
                    'address()
                    .Add("address")
                    'zipcode()
                    .Add("zipcode")
                    'tel()
                    .Add("tel")
                    'fax()
                    .Add("fax")
                    'attn()
                    .Add("attn")
                    'po_no()
                    .Add("po_no")
                    'currency
                    .Add("currency")
                    'payment_term_text()
                    .Add("payment_term_text")
                    'quotation_no()
                    .Add("quotation_no")
                    'issue_date()
                    .Add("issue_date")
                    'discount_type_text
                    .Add("discount_type_text")
                    'discount_total()
                    .Add("discount_total")
                    'sub_total()
                    .Add("sub_total")
                    'vat_amount()
                    .Add("vat_amount")
                    'total_amount()
                    .Add("total_amount")
                    'remark()
                    .Add("remark")
                    'amount_text()
                    .Add("amount_text")
                    'deliver_to()
                    .Add("deliver_to")
                    'delivery_date()
                    .Add("delivery_date")
                    'contact()
                    .Add("contact")

                End With

                With objDT2.Columns
                    .Add("grp")
                    'po_header_id
                    .Add("po_header_id")
                    'no()
                    .Add("no")
                    'item_name()
                    .Add("item_name")
                    'quantity()
                    .Add("quantity")
                    'unit_name()
                    .Add("unit_name")
                    'unit_price()
                    .Add("unit_price")
                    'amount()
                    .Add("amount")
                    'discount_type_text()
                    .Add("discount_type_text")

                End With

            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("SetHeadTable", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: GetPurchaseApproveList
        '	Discription	    : Get Purchase Approve list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPurchaseApproveList( _
            ByVal objPurchaseDto As Dto.PurchaseSearchDto _
        ) As System.Data.DataTable Implements IPurchaseService.GetPurchaseApproveList
            ' set default
            GetPurchaseApproveList = New DataTable
            Try
                Dim objPurchase As New Entity.ImpPurchaseEntity
                ' variable for keep list
                Dim listPurchase As New List(Of Entity.ImpPurchaseEntity)
                ' data row object
                Dim row As DataRow
                Dim strIssueDate As String = ""

                ' call function GetPurchaseApproveList from entity
                listPurchase = objPurchase.GetPurchaseApproveList(objPurchaseDto)

                ' assign column header
                With GetPurchaseApproveList
                    .Columns.Add("id")
                    .Columns.Add("po_type")
                    .Columns.Add("po_no")
                    .Columns.Add("status")
                    .Columns.Add("status_id")
                    .Columns.Add("issue_date")
                    .Columns.Add("applied_by")
                    .Columns.Add("vendor_name")

                    ' assign row from listPurchase
                    For Each values In listPurchase
                        row = .NewRow
                        row("id") = values.id
                        row("po_type") = values.po_type_text
                        row("po_no") = values.po_no
                        row("status") = values.status
                        row("status_id") = values.status_id
                        row("applied_by") = values.applied_by
                        row("vendor_name") = values.vendor_name

                        If values.issue_date_text <> Nothing Or values.issue_date_text <> "" Then
                            strIssueDate = Left(values.issue_date_text, 4) & "/" & Mid(values.issue_date_text, 5, 2) & "/" & Right(values.issue_date_text, 2)
                            row("issue_date") = CDate(strIssueDate).ToString("dd/MMM/yyyy")
                        Else
                            row("issue_date") = values.issue_date_text
                        End If

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPurchaseApproveList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdatePurchase
        '	Discription	    : Update Purchase
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 05-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdatePurchaseStatus( _
            ByVal strPurchaseId As String, _
            ByVal intStatus As Integer, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IPurchaseService.UpdatePurchaseStatus
            ' set default return value
            UpdatePurchaseStatus = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                Dim objPurchase As New Entity.ImpPurchaseEntity
                ' call function UpdatePurchase from Item Entity
                intEff = objPurchase.UpdatePurchaseStatus(strPurchaseId, intStatus)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdatePurchaseStatus = True
                Else
                    ' case row less than 1 then return False
                    UpdatePurchaseStatus = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTAP_01_002"
                Else
                    ' other case
                    strMsg = "KTAP_01_002"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdatePurchaseStatus(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckTypePurchaeApprove
        '	Discription	    : Check data purchase_next_approve, outsource_next_approve
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckTypePurchaeApprove(ByVal objType As Enums.PurchaseTypes) As Integer Implements IPurchaseService.CheckTypePurchaeApprove
            Try
                Dim objUser As New Entity.ImpUserEntity
                Dim intUserId As Integer = CInt(HttpContext.Current.Session("UserID"))

                CheckTypePurchaeApprove = objUser.CheckApproveUser(objType, intUserId)

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckTypePurchaeApprove(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetS_Rate
        '	Discription	    : Get data Schedule rate 
        '	Return Value	: Integer
        '	Create User	    : Boonyarit
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetS_Rate(ByVal intCurrencyId As Integer, Optional ByVal strDate As String = "") As Decimal Implements IPurchaseService.GetS_Rate
            Try
                Dim objSRate As New Service.ImpScheculeRateService

                GetS_Rate = objSRate.GetScheculeRate(intCurrencyId, strDate)

                'Dim objSRate As New Entity.ImpMst_ScheduleRateEntity
                'Dim intSRate As Decimal

                'intSRate = objSRate.GetScheduleRateByPurchase(intCurrencyId, strDate)
                ''ไม่พบข้อมูลให้คืนค่าเป็น 1 จาก PGS ของพี่ปิง
                'If intSRate = 0 Then
                '    GetS_Rate = 1
                'Else
                '    GetS_Rate = intSRate
                'End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetS_Rate(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: CheckPurchaseByJobOrder
        '	Discription	    : Check Exist job order
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 31-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckPurchaseByJobOrder(ByVal strJobOrder As String) As Boolean Implements IPurchaseService.CheckPurchaseByJobOrder
            ' set default return value
            CheckPurchaseByJobOrder = False
            Try
                Dim objJobOrderEnt As New Entity.ImpJob_OrderEntity
                CheckPurchaseByJobOrder = objJobOrderEnt.CheckJobOrderByPurchase(strJobOrder)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckPurchaseByJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPOApprove
        '	Discription	    : Get status approve from po header
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 09-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPOApprove( _
            ByVal strPoId As String _
        ) As System.Data.DataTable Implements IPurchaseService.GetPOApprove
            ' set default list
            GetPOApprove = New DataTable
            Try
                Dim objPOEnt As New Entity.ImpPo_HeaderEntity
                Dim listPOEnt As New List(Of Entity.IPo_HeaderEntity)
                Dim row As DataRow
                ' call function GetPOApprove
                listPOEnt = objPOEnt.GetPOApprove(strPoId)

                With GetPOApprove
                    .Columns.Add("id")
                    .Columns.Add("po_no")
                    .Columns.Add("status_id")

                    ' assign row from listPOEnt
                    For Each values In listPOEnt
                        row = .NewRow
                        row("id") = values.id
                        row("po_no") = values.po_no
                        row("status_id") = values.status_id

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPOApprove", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SaveTaskPurchase
        '	Discription	    : Save task purchase 
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 14-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SaveTaskPurchase(ByVal strMode As String, ByVal intPoId_New As Integer) As Boolean Implements IPurchaseService.SaveTaskPurchase
            Try
                Dim objPurchaseEnt As New Entity.ImpPo_HeaderEntity
                SaveTaskPurchase = False

                'If (intPoId_New Is Nothing) Then Exit Function
                If strMode.Equals("Add") Then
                    'เพิ่มรายการใน Task Purchase
                    If objPurchaseEnt.InsertTaskPurchase(intPoId_New) > 0 Then
                        SaveTaskPurchase = True
                    End If
                ElseIf strMode.Equals("Edit") Or strMode.Equals("Modify") Then
                    'แก้รายการใน Task Purchase
                    If objPurchaseEnt.UpdateTaskPurchase(intPoId_New) > 0 Then
                        SaveTaskPurchase = True
                    End If
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SaveTaskPurchase", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
    End Class
End Namespace




