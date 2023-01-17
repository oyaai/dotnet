Imports Microsoft.VisualBasic
Imports System.Data
Imports Service

Namespace Service

    Public Class ImpPurchase_HistoryService
        Implements IPurchase_HistoryService

        Private objLog As New Common.Logs.Log
        Private objPurchaseHistoryEnt As New Entity.ImpPurchase_HistoryEntity

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
            ByVal objPurchaseHistoryDto As Dto.Purchase_HistoryDto _
        ) As System.Data.DataTable Implements IPurchase_HistoryService.GetPurchaseHistoryReport
            ' set default
            GetPurchaseHistoryReport = New DataTable
            Try
                ' variable for keep list
                Dim listPurchaseHistoryEnt As New List(Of Entity.ImpPurchase_HistoryEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetPurchaseHistoryReport from entity
                listPurchaseHistoryEnt = objPurchaseHistoryEnt.GetPurchaseHistoryReport(SetDtoToEntity(objPurchaseHistoryDto))

                ' assign column header
                With GetPurchaseHistoryReport
                    .Columns.Add("job_order")
                    .Columns.Add("po_no")
                    .Columns.Add("invoice_no")
                    .Columns.Add("total_delivery_amount")
                    .Columns.Add("vendor_name")
                    .Columns.Add("ie_desc")
                    .Columns.Add("delivery_date")
                    .Columns.Add("delivery_qty")

                    ' assign row from listWorkingHourEnt
                    For Each values In listPurchaseHistoryEnt
                        row = .NewRow
                        row("job_order") = values.job_order
                        row("po_no") = values.po_no
                        row("invoice_no") = values.invoice_no
                        row("total_delivery_amount") = Format(Convert.ToDouble(values.delivery_amount.ToString.Trim), "#,##0.00")
                        row("vendor_name") = values.VendorName
                        row("ie_desc") = values.ItemName
                        row("delivery_date") = values.delivery_date
                        row("delivery_qty") = values.delivery_qty

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPurchaseHistoryReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Purchase History Entity object
        '	Create User	    : Nisa S.
        '	Create Date	    : 19-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objPurchaseHistoryDto As Dto.Purchase_HistoryDto _
        ) As Entity.IPurchase_HistoryEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpPurchase_HistoryEntity
            Try
                ' assign value to entity
                With SetDtoToEntity

                    'Receive data from report screen 
                    .job_order = objPurchaseHistoryDto.job_order
                    .po_no = objPurchaseHistoryDto.po_no
                    .ItemName = objPurchaseHistoryDto.ItemName
                    .invoice_no = objPurchaseHistoryDto.invoice_no
                    .VendorName = objPurchaseHistoryDto.VendorName
                    .delivery_date1 = objPurchaseHistoryDto.delivery_date1
                    .delivery_date2 = objPurchaseHistoryDto.delivery_date2

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

    End Class
End Namespace
