#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpVendorService
'	Class Discription	: Class of Vendor
'	Create User 		: Boon
'	Create Date		    : 21-05-2013
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
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports System.IO
Imports System.Web.Configuration

#End Region

Namespace Service
    Public Class ImpVendorService
        Implements IVendorService

        Private objLog As New Common.Logs.Log

#Region "Function"
        '/**************************************************************
        '	Function name	: GetVenderForSearch
        '	Discription	    : Get data vender for search
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVendorForSearch(ByVal objValue As Dto.VendorDto, ByRef objDT As System.Data.DataTable) As Boolean Implements IVendorService.GetVendorForSearch
            Try
                ' variable
                Dim objVendor As New Entity.ImpMst_VendorEntity
                Dim objListVendor As New List(Of Entity.ImpSubMst_VendorEntity)

                GetVendorForSearch = False
                ' assign object vendor dto
                With objValue
                    If .intSType1 = -1 And .intSType2 = -1 And .strSName = String.Empty And .intSCountry = -1 Then Exit Function
                    ' get data vendor
                    objListVendor = objVendor.GetVendorForSearch(.intSType1, .intSType2, .strSName, .intSCountry)
                End With
                ' check data result
                If objListVendor Is Nothing Then Exit Function
                If objListVendor.Count > 0 Then
                    objDT = SetVendorListToDT(objListVendor)
                    GetVendorForSearch = True
                End If

            Catch ex As Exception
                ' Write error log
                GetVendorForSearch = False
                objLog.ErrorLog("GetVenderForSearch", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetVendorForReport
        '	Discription	    : Get data vender for report
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 27-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVendorForReport(ByVal objValue As Dto.VendorDto) As Boolean Implements IVendorService.GetVendorForReport
            Try
                ' variable
                Dim objVendor As New Entity.ImpMst_VendorEntity
                Dim objListVendor As New List(Of Entity.ImpVendorBranchEntity)

                GetVendorForReport = False
                ' assign object vendor dto
                With objValue
                    If .intSType1 = -1 And .intSType2 = -1 And .strSName = String.Empty And .intSCountry = -1 Then Exit Function
                    ' get data vendor
                    objListVendor = objVendor.GetVendorForReport(.intSType1, .intSType2, .strSName, .intSCountry)
                End With
                ' check data result
                If objListVendor Is Nothing Then Exit Function
                If objListVendor.Count > 0 Then
                    'objDT = SetVendorListToDT(objListVendor)
                    'GetVendorForReport = True
                    GetVendorForReport = ExcelVendorList(objListVendor)
                End If

            Catch ex As Exception
                ' Write error log
                GetVendorForReport = False
                objLog.ErrorLog("GetVendorForReport", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: ExcelVendorList
        '	Discription	    : Export data vendor for report
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 27-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function ExcelVendorList(ByVal objListVendor As List(Of Entity.ImpVendorBranchEntity)) As Boolean
            Try
                ' variable
                Dim pck As ExcelPackage = Nothing
                Dim wBook As OfficeOpenXml.ExcelWorksheet = Nothing
                'Dim DT As DataTable = Nothing
                Dim rowCount As Integer = 0
                Dim intIndexRow As Integer = 4
                'Dim sRange As String
                Dim objComm As New Common.Utilities.Utility
                Dim intVendorID As Integer = 0

                ExcelVendorList = False

                'Dim strPath As String = HttpContext.Current.Server.MapPath("~/App_Data/report/VendorListReport.xlsx")
                Dim strPath As String = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("ReportPath") & "VendorListReport.xlsx")

                If objListVendor.Count < 1 Then Exit Function
                pck = New ExcelPackage(New MemoryStream(), New MemoryStream(File.ReadAllBytes(strPath)))
                wBook = pck.Workbook.Worksheets(1)
                wBook.InsertRow(intIndexRow + 1, 1, 4)
                wBook.DeleteRow(intIndexRow, 1)

                With wBook
                    For i = 0 To objListVendor.Count - 1
                        wBook.InsertRow(intIndexRow + i, 1, 4)
                        If objListVendor(i).vendorID <> intVendorID Then
                            intVendorID = objListVendor(i).vendorID
                            'No.
                            .Cells((intIndexRow + i), 1).Value = i + 1
                        End If
                        If objListVendor(i).id = 0 Then
                            'Supplier Name
                            .Cells((intIndexRow + i), 2).Value = objListVendor(i).name.Trim
                            'Type of Goods
                            .Cells((intIndexRow + i), 9).Value = Replace(objListVendor(i).typeOfGoods.Trim, " ", ", ")
                        Else
                            'Branch Name
                            .Cells((intIndexRow + i), 3).Value = objListVendor(i).name.Trim
                        End If
                        'Address
                        .Cells((intIndexRow + i), 4).Value = objListVendor(i).fullAddress.Trim
                        'Contact Name
                        .Cells((intIndexRow + i), 5).Value = objListVendor(i).contact.Trim
                        'Telephone
                        .Cells((intIndexRow + i), 6).Value = objListVendor(i).telNo.Trim
                        'Fax
                        .Cells((intIndexRow + i), 7).Value = objListVendor(i).faxNo.Trim
                        'E-mail Adds.
                        .Cells((intIndexRow + i), 8).Value = objListVendor(i).email.Trim
                        'Remarks
                        .Cells((intIndexRow + i), 10).Value = objListVendor(i).remarks.Trim

                    Next
                    'Set data in Header and Footer
                    .HeaderFooter.OddHeader.RightAlignedText = "Date: " & Now() & " Page: " & ExcelHeaderFooter.PageNumber
                    .HeaderFooter.OddFooter.LeftAlignedText = "F-PC-002 (Approved Subcontractor and Supplier List : ASL)"
                    .HeaderFooter.OddFooter.RightAlignedText = "Effective date : 01/06/09  Rev : 00"

                End With

                HttpContext.Current.Response.Clear()
                pck.SaveAs(HttpContext.Current.Response.OutputStream)
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=VendorListReport.xlsx")
                HttpContext.Current.Response.End()

                ExcelVendorList = True
            Catch ex As Exception
                ' Write error log
                ExcelVendorList = False
                objLog.ErrorLog("ExcelVendorList", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        'Sub MergeCells(ByVal sStartCellAddress As String, ByVal sEndCellAddress As String, ByVal iRowOffset As Integer, ByVal iColOffset As Integer)

        '    Dim sNewStartAddress, sNewEndAddress As String

        '    sNewStartAddress = CalcCellAddress(sStartCellAddress, iRowOffset, iColOffset)
        '    sNewEndAddress = CalcCellAddress(sEndCellAddress, iRowOffset, iColOffset)
        '    Dim sRange As String = sNewStartAddress & ":" & sNewEndAddress
        '    xlWorksheet.Cells(sRange).Merge = True
        '    xlWorksheet.Cells(sRange).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left
        '    xlWorksheet.Cells(sRange).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top
        '    xlWorksheet.Cells(sRange).Style.WrapText = True
        'End Sub


        '/**************************************************************
        '	Function name	: CheckStrNull
        '	Discription	    : Check string is null return String.Empty
        '	Return Value	: String
        '	Create User	    : Boonyarit
        '	Create Date	    : 27-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function CheckStrNull(ByVal strValue1 As String, ByVal strValue2 As String) As String
            Try
                CheckStrNull = String.Empty

                If strValue1 = String.Empty And strValue2 = String.Empty Then
                    Exit Function
                ElseIf strValue1 <> String.Empty And strValue2 = String.Empty Then
                    CheckStrNull = strValue1
                ElseIf strValue1 = String.Empty And strValue2 <> String.Empty Then
                    CheckStrNull = strValue2
                ElseIf strValue1 <> String.Empty And strValue2 <> String.Empty Then
                    CheckStrNull = strValue1 & ", " & strValue2
                Else
                    Exit Function
                End If

            Catch ex As Exception
                CheckStrNull = String.Empty
                objLog.ErrorLog("CheckStrNull", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckStrNull
        '	Discription	    : Chenge data string
        '	Return Value	: String
        '	Create User	    : Boonyarit
        '	Create Date	    : 15-07-2013
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
        '	Function name	: SetVenderListToDT
        '	Discription	    : Set data vender to datatable
        '	Return Value	: DataTable
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetVendorListToDT(ByVal objValue As List(Of Entity.ImpSubMst_VendorEntity)) As System.Data.DataTable
            Try
                ' variable
                Dim objDT As New System.Data.DataTable
                Dim objDR As System.Data.DataRow

                SetVendorListToDT = Nothing

                With objDT.Columns
                    .Add("id")
                    .Add("type1_text")
                    .Add("type2_text")
                    .Add("name")
                    .Add("country_name")
                End With

                For Each objItem In objValue
                    With objItem
                        objDR = objDT.NewRow
                        objDR("id") = .id
                        objDR("type1_text") = .type1_text.Trim
                        objDR("type2_text") = .type2_text.Trim
                        objDR("name") = .name.Trim
                        objDR("country_name") = .country_name.Trim
                        objDT.Rows.Add(objDR)
                    End With
                Next
                SetVendorListToDT = objDT

            Catch ex As Exception
                ' Write error log
                SetVendorListToDT = Nothing
                objLog.ErrorLog("SetVenderListToDT", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckVenderForDel
        '	Discription	    : Check data vender for delete
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 21-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckVendorForDel(ByVal intVendorId As Integer) As Boolean Implements IVendorService.CheckVendorForDel
            Try
                ' variable
                Dim objItemEntity As New Entity.ImpMst_ItemEntity
                Dim objPojHEntity As New Entity.ImpPo_HeaderEntity
                Dim objJobEntity As New Entity.ImpJob_OrderEntity
                Dim objReceiveEntity As New Entity.ImpReceive_HeaderEntity
                Dim objAccountEntity As New Entity.ImpAccountingEntity

                CheckVendorForDel = False
                If intVendorId < 1 Then Exit Function

                'Step 1 Check TB mst_item
                If objItemEntity.CheckItemByVendor(intVendorId) Then Exit Function
                'Step 2 Check TB po_header
                If objPojHEntity.CheckPoByVendor(intVendorId) Then Exit Function
                'Step 3 Check TB job_order
                If objJobEntity.CheckJobOrderByVendor(intVendorId) Then Exit Function
                'Step 4 Check TB receive_header
                If objReceiveEntity.CheckReceiveByVendor(intVendorId) Then Exit Function
                'Step 5 Check TB accounting
                If objAccountEntity.CheckAccountByVendor(intVendorId) Then Exit Function
                'Step 6 Check successful
                CheckVendorForDel = True

            Catch ex As Exception
                ' Write error log
                CheckVendorForDel = False
                objLog.ErrorLog("CancelVender", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CancelVender
        '	Discription	    : Cancel data vender
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 21-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CancelVendor(ByVal intVendorID As Integer) As Boolean Implements IVendorService.CancelVendor
            Try
                ' variable
                Dim objVendorEntity As New Entity.ImpMst_VendorEntity
                Dim strFileName As String = String.Empty

                CancelVendor = False
                ' check data will delete or cancel
                If intVendorID < 1 Then Exit Function
                ' check data file in vendor
                strFileName = objVendorEntity.GetFileNameById(intVendorID)
                If objVendorEntity.CancelVendor(intVendorID) Then
                    ' delete file vendor
                    Call DeleteFileVendor(strFileName)
                    CancelVendor = True
                End If

            Catch ex As Exception
                ' Write error log
                CancelVendor = False
                objLog.ErrorLog("CancelVender", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteFileVendor
        '	Discription	    : Delete file data vender
        '	Return Value	: 
        '	Create User	    : Boonyarit
        '	Create Date	    : 30-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub DeleteFileVendor(ByVal strFileName As String)
            Try
                ' variable
                Dim strPath = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "Vendor/" & strFileName.Trim)
                Dim fileVendor As New FileInfo(strPath)
                ' check and delete file of vendor
                If fileVendor.Exists = True Then
                    fileVendor.Delete()
                End If

            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("DeleteFileVendor", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: CancelVender
        '	Discription	    : Cancel data vendor
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 21-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVendorForDetail(ByVal intVendorId As Integer) As Dto.VendorDto Implements IVendorService.GetVendorForDetail
            Try
                ' variable
                Dim objVendor As New Entity.ImpMst_VendorEntity
                Dim objDataVendor As New Entity.ImpSubMst_VendorEntity

                GetVendorForDetail = Nothing
                'Get data vendor
                objDataVendor = objVendor.GetVendorForDetail(intVendorId)
                If objDataVendor Is Nothing Then Exit Function

                ' assign object vendor dto
                GetVendorForDetail = New Dto.VendorDto
                With GetVendorForDetail
                    .id = objDataVendor.id
                    '***Start Boon add
                    .type1 = objDataVendor.type1
                    .type2 = objDataVendor.type2
                    .payment_term_id = objDataVendor.payment_term_id
                    .payment_cond1 = objDataVendor.payment_cond1
                    .payment_cond2 = objDataVendor.payment_cond2
                    .payment_cond3 = objDataVendor.payment_cond3
                    If objDataVendor.country_id = 1 Then
                        .country_id = 0
                    Else
                        .country_id = objDataVendor.country_id
                    End If
                    '.type_of_goods = objDataVendor.type_of_goods
                    .purchase_fg = objDataVendor.purchase_fg
                    .outsource_fg = objDataVendor.outsource_fg
                    .other_fg = objDataVendor.other_fg
                    '***End Boon add
                    .type1_text = objDataVendor.type1_text.Trim
                    .type2_text = objDataVendor.type2_text.Trim
                    .type2_no = objDataVendor.type2_no.Trim
                    .name = objDataVendor.name.Trim
                    .short_name = objDataVendor.short_name.Trim
                    .person_in_charge1 = objDataVendor.person_in_charge1.Trim
                    .person_in_charge2 = objDataVendor.person_in_charge2.Trim
                    .payment_term = objDataVendor.payment_term.Trim
                    .payment_condition = objDataVendor.payment_condition.Trim
                    .country_name = objDataVendor.country_name.Trim
                    .zipcode = objDataVendor.zipcode.Trim
                    .address = objDataVendor.address.Trim
                    .tel = objDataVendor.tel.Trim
                    .fax = objDataVendor.fax.Trim
                    .email = objDataVendor.email.Trim
                    '.type_of_goods_text = objDataVendor.type_of_goods_text.Trim
                    .remarks = objDataVendor.remarks.Trim
                    .file = objDataVendor.file.Trim
                End With

            Catch ex As Exception
                ' Write error log
                GetVendorForDetail = Nothing
                objLog.ErrorLog("GetVendorForDetail", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        ''/**************************************************************
        ''	Function name	: CheckNotDupVendor
        ''	Discription	    : Check data vendor duplicate
        ''	Return Value	: Boolean
        ''	Create User	    : Boonyarit
        ''	Create Date	    : 22-05-2013
        ''	Update User	    :
        ''	Update Date	    :
        ''*************************************************************/
        'Public Function CheckNotDupVendor(ByVal intType1 As Integer, ByVal intType2 As Integer, ByVal strVandorName As String) As Boolean Implements IVendorService.CheckNotDupVendor
        '    Try
        '        ' variable
        '        Dim objVendor As New Entity.ImpMst_VendorEntity

        '        ' check data vendor duplicate
        '        CheckNotDupVendor = Not objVendor.CheckIsDupVendor(intType1, intType2, strVandorName)

        '    Catch ex As Exception
        '        CheckNotDupVendor = False
        '        objLog.ErrorLog("CheckNotDupVendor", ex.Message.Trim, HttpContext.Current.Session("UserName"))
        '    End Try
        'End Function

        '/**************************************************************
        '	Function name	: CheckIsDupVendor
        '	Discription	    : Check data vendor duplicate
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 06-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckIsDupVendor(ByVal objVendorDto As Dto.VendorDto) As Boolean Implements IVendorService.CheckIsDupVendor
            Try
                ' variable
                Dim objVendor As New Entity.ImpMst_VendorEntity
                ' check data vendor duplicate
                With objVendorDto
                    CheckIsDupVendor = objVendor.CheckIsDupVendor(.type1, .type2, .name, .id)
                End With

            Catch ex As Exception
                CheckIsDupVendor = True
                objLog.ErrorLog("CheckIsDupVendor", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SaveVendor
        '	Discription	    : Save data vendor
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 23-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SaveVendor(ByRef objVendorDto As Dto.VendorDto, ByVal strMode As String) As Boolean Implements IVendorService.SaveVendor
            Try
                ' variable
                Dim objVendorEntity As New Entity.ImpMst_VendorEntity
                Dim intVendorId As Integer = 0

                SaveVendor = False

                ' assign object vendor entity
                With objVendorDto
                    objVendorEntity.id = .id
                    objVendorEntity.type1 = .type1
                    objVendorEntity.type2 = .type2
                    objVendorEntity.type2_no = .type2_no
                    objVendorEntity.name = .name
                    objVendorEntity.short_name = .short_name
                    objVendorEntity.person_in_charge1 = .person_in_charge1
                    objVendorEntity.person_in_charge2 = .person_in_charge2
                    If .country_id = 0 Then
                        objVendorEntity.country_id = 1
                    Else
                        objVendorEntity.country_id = .country_id
                    End If
                    objVendorEntity.zipcode = CheckStrNull(.zipcode)
                    objVendorEntity.address = CheckStrNull(.address)
                    objVendorEntity.tel = CheckStrNull(.tel)
                    objVendorEntity.fax = CheckStrNull(.fax)
                    objVendorEntity.email = CheckStrNull(.email)
                    objVendorEntity.purchase_fg = CheckStrNull(.purchase_fg)
                    objVendorEntity.outsource_fg = CheckStrNull(.outsource_fg)
                    objVendorEntity.other_fg = CheckStrNull(.other_fg)
                    objVendorEntity.remarks = CheckStrNull(.remarks)
                    If .checkDelete = True Then
                        objVendorEntity.file = String.Empty
                    Else
                        objVendorEntity.file = CheckStrNull(.file)
                    End If

                End With

                ' check mode
                If strMode = "Add" Then
                    ' insert data vendor
                    If objVendorEntity.InsertVendor(objVendorEntity, intVendorId) = False Then Exit Function
                    objVendorDto.id = intVendorId
                ElseIf strMode = "Edit" Then
                    ' update data vendor
                    If objVendorEntity.UpdateVendor(objVendorEntity) = False Then Exit Function
                End If

                SaveVendor = True

            Catch ex As Exception
                SaveVendor = False
                objLog.ErrorLog("SaveVendor", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetVendorForList
        '	Discription	    : Get Vendor for dropdownlist
        '	Return Value	: List
        '	Create User	    : Komsan L.
        '	Create Date	    : 03-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVendorForList(Optional ByVal intType1 As String = Nothing) As System.Collections.Generic.List(Of Dto.VendorDto) Implements IVendorService.GetVendorForList
            ' set default list
            GetVendorForList = New List(Of Dto.VendorDto)
            Try
                ' objVendorDto for keep value Dto 
                Dim objVendorDto As Dto.VendorDto
                ' listVendorEnt for keep value from entity
                Dim listVendorEnt As New List(Of Entity.IMst_VendorEntity)
                ' objVendorEnt for call function
                Dim objVendorEnt As New Entity.ImpMst_VendorEntity

                ' call function GetVendorForList
                listVendorEnt = objVendorEnt.GetVendorForList(intType1)

                ' loop listVendorEnt for assign value to Dto
                For Each values In listVendorEnt
                    ' new object
                    objVendorDto = New Dto.VendorDto
                    ' assign value to Dto
                    With objVendorDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetVendorForList.Add(objVendorDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVendorForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetVendorListForJobOrder
        '	Discription	    : Get Vendor for dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVendorListForJobOrder() As System.Collections.Generic.List(Of Dto.VendorDto) Implements IVendorService.GetVendorListForJobOrder
            ' set default list
            GetVendorListForJobOrder = New List(Of Dto.VendorDto)
            Try
                ' objVendorDto for keep value Dto 
                Dim objVendorDto As Dto.VendorDto
                ' listVendorEnt for keep value from entity
                Dim listVendorEnt As New List(Of Entity.IMst_VendorEntity)
                ' objVendorEnt for call function
                Dim objVendorEnt As New Entity.ImpMst_VendorEntity

                ' call function GetVendorForList
                listVendorEnt = objVendorEnt.GetVendorListForJobOrder()

                ' loop listVendorEnt for assign value to Dto
                For Each values In listVendorEnt
                    ' new object
                    objVendorDto = New Dto.VendorDto
                    ' assign value to Dto
                    With objVendorDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetVendorListForJobOrder.Add(objVendorDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVendorListForJobOrder", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetListVendorName
        '	Discription	    : Set list vandor_name to dropdownlist
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetListVendorName(ByRef objValue As System.Web.UI.WebControls.DropDownList) As Boolean Implements IVendorService.SetListVendorName
            Try
                ' variable
                Dim objVendor As New Entity.ImpMst_VendorEntity
                Dim objListVendor As List(Of Entity.IMst_VendorEntity)
                Dim objComm As New Common.Utilities.Utility

                SetListVendorName = False
                ' get data list Payment_day
                objListVendor = objVendor.GetVendorForList
                If objListVendor.Count < 1 Then Exit Function
                Call objComm.LoadList(objValue, objListVendor, "name", "id")
                If objValue.Items.Count > 0 Then SetListVendorName = True

            Catch ex As Exception
                ' Write error log
                SetListVendorName = False
                objLog.ErrorLog("SetListVendorName", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region

    End Class
End Namespace

