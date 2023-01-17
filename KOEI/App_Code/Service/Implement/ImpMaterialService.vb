#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpMaterialService
'	Class Discription	: Implement Material Service
'	Create User 		: Suwishaya L.
'	Create Date		    : 03-07-2013
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
    Public Class ImpMaterialService
        Implements IMaterialService

        Private objLog As New Common.Logs.Log
        Private objMaterialEnt As New Entity.ImpMaterialEntity

#Region "Function"

        '/**************************************************************
        '	Function name	: GetMaterialList
        '	Discription	    : Get Material List
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetMaterialList( _
            ByVal objMaterialDto As Dto.MaterialDto _
        ) As System.Data.DataTable Implements IMaterialService.GetMaterialList
            ' set default
            GetMaterialList = New DataTable
            Try
                ' variable for keep list
                Dim listMaterialEnt As New List(Of Entity.ImpMaterialEntity)
                ' data row object
                Dim row As DataRow
                Dim strDateIn As String = ""
                Dim strDateOut As String = ""

                ' call function GetMaterialList from entity
                listMaterialEnt = objMaterialEnt.GetMaterialList(SetDtoToEntity(objMaterialDto))

                ' assign column header
                With GetMaterialList
                    .Columns.Add("id")
                    .Columns.Add("job_order")
                    .Columns.Add("po_no")
                    .Columns.Add("invoice_no")
                    .Columns.Add("amount")
                    .Columns.Add("vendor")
                    .Columns.Add("item_name")
                    .Columns.Add("delivery_date_in")
                    .Columns.Add("qty_in")
                    .Columns.Add("delivery_date_out")
                    .Columns.Add("qty_out")
                    .Columns.Add("qty_left")

                    ' assign row from listMaterialEnt
                    For Each values In listMaterialEnt
                        row = .NewRow
                        row("id") = values.id
                        row("job_order") = values.job_order
                        row("po_no") = values.po_no
                        row("invoice_no") = values.invoice_no
                        row("vendor") = values.vendor
                        row("item_name") = values.item_name

                        If values.delivery_date_in <> Nothing Or values.delivery_date_in <> "" Then
                            strDateIn = Left(values.delivery_date_in, 4) & "/" & Mid(values.delivery_date_in, 5, 2) & "/" & Right(values.delivery_date_in, 2)
                            row("delivery_date_in") = CDate(strDateIn).ToString("dd/MMM/yyyy")
                        Else
                            row("delivery_date_in") = values.delivery_date_in
                        End If

                        If values.delivery_date_out <> Nothing Or values.delivery_date_out <> "" Then
                            strDateOut = Left(values.delivery_date_out, 4) & "/" & Mid(values.delivery_date_out, 5, 2) & "/" & Right(values.delivery_date_out, 2)
                            row("delivery_date_out") = CDate(strDateOut).ToString("dd/MMM/yyyy")
                        Else
                            row("delivery_date_out") = values.delivery_date_out
                        End If

                        row("qty_in") = Format(Convert.ToDouble(values.qty_in.ToString.Trim), "#,##0.00")
                        row("qty_left") = Format(Convert.ToDouble(values.qty_left.ToString.Trim), "#,##0.00")
                        row("qty_out") = Format(Convert.ToDouble(values.qty_out.ToString.Trim), "#,##0.00")
                        row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetMaterialList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetMaterialListReport
        '	Discription	    : Get Material List Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetMaterialListReport( _
            ByVal objMaterialDto As Dto.MaterialDto _
        ) As System.Data.DataTable Implements IMaterialService.GetMaterialListReport
            ' set default
            GetMaterialListReport = New DataTable
            Try
                ' variable for keep list
                Dim listMaterialEnt As New List(Of Entity.ImpMaterialEntity)
                ' data row object
                Dim row As DataRow
                Dim strDateIn As String = ""
                Dim strDateOut As String = ""

                ' call function GetMaterialListReport from entity
                listMaterialEnt = objMaterialEnt.GetMaterialListReport(SetDtoToEntity(objMaterialDto))

                ' assign column header
                With GetMaterialListReport
                    .Columns.Add("id")
                    .Columns.Add("job_order")
                    .Columns.Add("po_no")
                    .Columns.Add("invoice_no")
                    .Columns.Add("amount")
                    .Columns.Add("vendor")
                    .Columns.Add("item_name")
                    .Columns.Add("delivery_date_in")
                    .Columns.Add("qty_in")
                    .Columns.Add("delivery_date_out")
                    .Columns.Add("qty_out")
                    .Columns.Add("qty_left")

                    ' assign row from listMaterialEnt
                    For Each values In listMaterialEnt
                        row = .NewRow
                        row("id") = values.id
                        row("job_order") = values.job_order
                        row("po_no") = values.po_no
                        row("invoice_no") = values.invoice_no
                        row("vendor") = values.vendor
                        row("item_name") = values.item_name

                        If values.delivery_date_in <> Nothing Or values.delivery_date_in <> "" Then
                            strDateIn = Left(values.delivery_date_in, 4) & "/" & Mid(values.delivery_date_in, 5, 2) & "/" & Right(values.delivery_date_in, 2)
                            row("delivery_date_in") = CDate(strDateIn).ToString("dd/MMM/yyyy")
                        Else
                            row("delivery_date_in") = values.delivery_date_in
                        End If

                        If values.delivery_date_out <> Nothing Or values.delivery_date_out <> "" Then
                            strDateOut = Left(values.delivery_date_out, 4) & "/" & Mid(values.delivery_date_out, 5, 2) & "/" & Right(values.delivery_date_out, 2)
                            row("delivery_date_out") = CDate(strDateOut).ToString("dd/MMM/yyyy")
                        Else
                            row("delivery_date_out") = values.delivery_date_out
                        End If

                        'Modify 2013/09/26 
                        'row("qty_in") = Format(Convert.ToDouble(values.qty_in.ToString.Trim), "#,##0.00")
                        'row("qty_out") = Format(Convert.ToDouble(values.qty_out.ToString.Trim), "#,##0.00")
                        'row("qty_left") = Format(Convert.ToDouble(values.qty_left.ToString.Trim), "#,##0.00")
                        'row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")

                        row("qty_in") = values.qty_in
                        row("qty_out") = values.qty_out
                        row("qty_left") = values.qty_left
                        row("amount") = values.amount

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetMaterialListReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumMaterialListReport
        '	Discription	    : Get Sum Material List Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 03-07-2013
        '	Update User	    : Suwishaya L.
        '	Update Date	    : 26-09-2013
        '*************************************************************/
        Public Function GetSumMaterialListReport( _
            ByVal objMaterialDto As Dto.MaterialDto _
        ) As System.Data.DataTable Implements IMaterialService.GetSumMaterialListReport
            ' set default
            GetSumMaterialListReport = New DataTable
            Try
                ' variable for keep list
                Dim listMaterialEnt As New List(Of Entity.ImpMaterialEntity)
                ' data row object
                Dim row As DataRow
                Dim strDateIn As String = ""
                Dim strDateOut As String = ""

                ' call function GetSumMaterialListReport from entity
                listMaterialEnt = objMaterialEnt.GetSumMaterialListReport(SetDtoToEntity(objMaterialDto))

                ' assign column header
                With GetSumMaterialListReport
                    .Columns.Add("sum_amount")

                    ' assign row from listMaterialEnt
                    For Each values In listMaterialEnt
                        row = .NewRow
                        'Modify 2013/09/26 
                        'row("sum_amount") = Format(Convert.ToDouble(values.sum_amount.ToString.Trim), "#,##0.00")
                        row("sum_amount") = values.sum_amount

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumMaterialListReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
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
            ByVal objMaterialDto As Dto.MaterialDto _
        ) As Entity.IMaterialEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpMaterialEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    'Receive data from search job order screen 
                    .job_order = objMaterialDto.job_order
                    .vendor = objMaterialDto.vendor
                    .invoice_no = objMaterialDto.invoice_no
                    .po_no = objMaterialDto.po_no
                    .item_name = objMaterialDto.item_name
                    .delivery_date_from = objMaterialDto.delivery_date_from
                    .delivery_date_to = objMaterialDto.delivery_date_to

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region
        
    End Class
End Namespace
