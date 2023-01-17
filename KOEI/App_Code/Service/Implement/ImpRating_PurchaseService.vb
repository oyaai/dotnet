#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpRating_PurchaseService
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

Namespace Service
    Public Class ImpRating_PurchaseService
        Implements IRating_PurchaseService

        Private objLog As New Common.Logs.Log
        Private objRatingPurchaseEnt As New Entity.ImpRating_PurchaseEntity

#Region "Function"
        '/**************************************************************
        '	Function name	: GetRating_PurchaseList
        '	Discription	    : Get GetRating_PurchaseList
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetRating_PurchaseList( _
            ByVal objRatingDto As Dto.Rating_PurchaseDto _
        ) As System.Data.DataTable Implements IRating_PurchaseService.GetRating_PurchaseList
            ' set default
            GetRating_PurchaseList = New DataTable
            Try
                ' variable for keep list from Rating Purchase entity
                Dim listRating_PurchaseEnt As New List(Of Entity.ImpRating_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listRating_PurchaseEnt = objRatingPurchaseEnt.GetRating_PurchaseList(SetDtoToEntity(objRatingDto))

                ' assign column header
                With GetRating_PurchaseList
                    .Columns.Add("id")
                    .Columns.Add("invoice_no")
                    .Columns.Add("po_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("payment_date")
                    .Columns.Add("delivery_date")
                    .Columns.Add("score")

                    ' assign row from listAccountingEny
                    For Each values In listRating_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id
                        row("invoice_no") = values.invoice_no
                        row("po_no") = values.po_no
                        row("vendor_name") = values.vendor_name
                        If IsNothing(values.payment_date) = False Then
                            row("payment_date") = CDate(values.payment_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("payment_date") = ""
                        End If
                        If IsNothing(values.delivery_date) = False Then
                            row("delivery_date") = CDate(values.delivery_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("delivery_date") = ""
                        End If

                        row("score") = values.score.ToString.Trim

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetRating_PurchaseList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetVendorRatingReport
        '	Discription	    : Get Vendor Rating Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVendorRatingReport( _
            ByVal objRatingDto As Dto.Rating_PurchaseDto _
        ) As System.Data.DataTable Implements IRating_PurchaseService.GetVendorRatingReport
            ' set default
            GetVendorRatingReport = New DataTable
            Try
                ' variable for keep list from Rating Purchase entity
                Dim listRating_PurchaseEnt As New List(Of Entity.ImpRating_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listRating_PurchaseEnt = objRatingPurchaseEnt.GetVendorRatingReport(SetDtoToEntity(objRatingDto))

                ' assign column header
                With GetVendorRatingReport
                    .Columns.Add("invoice_no")
                    .Columns.Add("po_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("delivery_date_po")
                    .Columns.Add("delivery_date")
                    .Columns.Add("payment_date")
                    .Columns.Add("quality")
                    .Columns.Add("delivery")
                    .Columns.Add("service")

                    ' assign row from listAccountingEny
                    For Each values In listRating_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("invoice_no") = values.invoice_no
                        row("po_no") = values.po_no
                        row("vendor_name") = values.vendor_name
                        If IsNothing(values.delivery_date_po) = False Then
                            row("delivery_date_po") = CDate(values.delivery_date_po.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("delivery_date_po") = ""
                        End If
                        If IsNothing(values.delivery_date) = False Then
                            row("delivery_date") = CDate(values.delivery_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("delivery_date") = ""
                        End If
                        If IsNothing(values.payment_date) = False Then
                            row("payment_date") = CDate(values.payment_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("payment_date") = ""
                        End If

                        row("quality") = values.quality.ToString.Trim
                        row("delivery") = values.delivery.ToString.Trim
                        row("service") = values.service.ToString.Trim

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVendorRatingReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetYearVendorRatingReport
        '	Discription	    : Get Year Vendor Rating Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetYearVendorRatingReport( _
            ByVal objRatingDto As Dto.Rating_PurchaseDto _
        ) As System.Data.DataTable Implements IRating_PurchaseService.GetYearVendorRatingReport
            ' set default
            GetYearVendorRatingReport = New DataTable
            Try
                ' variable for keep list from Rating Purchase entity
                Dim listRating_PurchaseEnt As New List(Of Entity.ImpRating_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listRating_PurchaseEnt = objRatingPurchaseEnt.GetYearVendorRatingReport(SetDtoToEntity(objRatingDto))

                ' assign column header
                With GetYearVendorRatingReport
                    .Columns.Add("grp")
                    .Columns.Add("vendor_id")
                    .Columns.Add("po_type")
                    .Columns.Add("vendor_name")
                    .Columns.Add("delivery_year")
                    .Columns.Add("quality")
                    .Columns.Add("delivery")
                    .Columns.Add("service")

                    ' assign row from listAccountingEny
                    For Each values In listRating_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("grp") = values.grp
                        row("vendor_id") = values.id
                        row("po_type") = values.po_type
                        row("vendor_name") = values.vendor_name
                        row("delivery_year") = values.delivery_year.ToString()
                        row("quality") = values.quality.ToString.Trim
                        row("delivery") = values.delivery.ToString.Trim
                        row("service") = values.service.ToString.Trim
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetYearVendorRatingReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
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
            ByVal objRatingPurchaseDto As Dto.Rating_PurchaseDto _
        ) As Entity.IRating_PurchaseEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpRating_PurchaseEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    'search
                    .strInvoce_no = objRatingPurchaseDto.strInvoce_no
                    .strSearchType = objRatingPurchaseDto.strSearchType
                    .strPO = objRatingPurchaseDto.strPO
                    .strPaymentDateFrom = objRatingPurchaseDto.strPaymentDateFrom
                    .strPaymentDateTo = objRatingPurchaseDto.strPaymentDateTo
                    .strDeliveryDateFrom = objRatingPurchaseDto.strDeliveryDateFrom
                    .strDeliveryDateTo = objRatingPurchaseDto.strDeliveryDateTo
                    .strVendor_name = objRatingPurchaseDto.strVendor_name
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: DeleteWorkingCategory
        '	Discription	    : Delete WorkingCategory
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteRatingInvoice( _
            ByVal intRatingId As Integer _
        ) As Boolean Implements IRating_PurchaseService.DeleteRatingInvoice
            ' set default return value
            DeleteRatingInvoice = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteWorkingCategory from WorkingCategory Entity
                intEff = objRatingPurchaseEnt.DeleteRatingInvoice(intRatingId)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteRatingInvoice = True
                Else
                    ' case row less than 1 then return False
                    DeleteRatingInvoice = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteRatingInvoice(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPurchaseList
        '	Discription	    : Get Purchase List
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPurchaseList( _
            ByVal objRatingDto As Dto.Rating_PurchaseDto _
        ) As System.Data.DataTable Implements IRating_PurchaseService.GetPurchaseList
            ' set default
            GetPurchaseList = New DataTable
            Try
                ' variable for keep list from Rating Purchase entity
                Dim listRating_PurchaseEnt As New List(Of Entity.ImpRating_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listRating_PurchaseEnt = objRatingPurchaseEnt.GetPurchaseList(SetDtoToEntity(objRatingDto))

                ' assign column header
                With GetPurchaseList
                    .Columns.Add("id")
                    .Columns.Add("invoice_no")
                    .Columns.Add("po_no")
                    .Columns.Add("vendor_name")
                    .Columns.Add("payment_date")
                    .Columns.Add("delivery_date")

                    ' assign row from listAccountingEny
                    For Each values In listRating_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("id") = values.id
                        row("invoice_no") = values.invoice_no
                        row("po_no") = values.po_no
                        row("vendor_name") = values.vendor_name
                        If IsNothing(values.payment_date) = False Then
                            row("payment_date") = CDate(values.payment_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("payment_date") = ""
                        End If
                        If IsNothing(values.delivery_date) = False Then
                            row("delivery_date") = CDate(values.delivery_date.ToString()).ToString("dd/MMM/yyyy")
                        Else
                            row("delivery_date") = ""
                        End If

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPurchaseList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetRating_PurchaseList
        '	Discription	    : Get GetRating_PurchaseList
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetRatingVendor( _
            ByVal ratingId As String, _
            ByVal payment_header_id As String _
        ) As System.Data.DataTable Implements IRating_PurchaseService.GetRatingVendor
            ' set default
            GetRatingVendor = New DataTable
            Try
                ' variable for keep list from Rating Purchase entity
                Dim listRating_PurchaseEnt As New List(Of Entity.ImpRating_PurchaseDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetAccountingList from entity
                listRating_PurchaseEnt = objRatingPurchaseEnt.GetRatingVendor(ratingId, payment_header_id)

                ' assign column header
                With GetRatingVendor
                    .Columns.Add("quality")
                    .Columns.Add("delivery")
                    .Columns.Add("service")

                    ' assign row from listAccountingEny
                    For Each values In listRating_PurchaseEnt
                        'set in list only account_type = 1 or 3 
                        row = .NewRow
                        row("quality") = values.quality
                        row("delivery") = values.delivery
                        row("service") = values.service

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetRatingVendor(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: CheckDupVendor_Rating
        '	Discription	    : Check duplication Vendor_Rating Table
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 15-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckDupVendor_Rating( _
            ByVal id As String, _
            ByVal payment_header_id As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IRating_PurchaseService.CheckDupVendor_Rating
            ' set default return value
            CheckDupVendor_Rating = False
            Try
                ' intEff keep row effect
                Dim listRating_PurchaseEnt As New List(Of Entity.ImpRating_PurchaseDetailEntity)

                ' call function CountUsedInPO from entity
                listRating_PurchaseEnt = objRatingPurchaseEnt.GetRatingVendor(id, payment_header_id)

                ' check count used
                If listRating_PurchaseEnt.Count > 0 Then
                    ' case not equal 0 then return True
                    CheckDupVendor_Rating = True
                Else
                    ' case equal 0 then return False
                    CheckDupVendor_Rating = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTPU_06_008"
                Else
                    ' other case
                    strMsg = "KTPU_06_007"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupVendor_Rating(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: InsUpdVendor_Rating
        '	Discription	    : Insert/Update Vendor_Rating
        '	Return Value	: Boolean
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
            ByVal strService As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IRating_PurchaseService.InsUpdVendor_Rating
            ' set default return value
            InsUpdVendor_Rating = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateWorkingCategory from WorkingCategory Entity
                intEff = objRatingPurchaseEnt.InsUpdVendor_Rating( _
                                                                    mode, _
                                                                    strId, _
                                                                    strPayment_header_id, _
                                                                    strQuality, _
                                                                    strDelivery, _
                                                                    strService)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsUpdVendor_Rating = True
                Else
                    ' case row less than 1 then return False
                    InsUpdVendor_Rating = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTPU_06_001"
                Else
                    ' other case
                    strMsg = "KTPU_06_007"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsUpdVendor_Rating(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region
    End Class
End Namespace

