#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpVendorBranchService
'	Class Discription	: Class of Branch Vendor
'	Create User 		: Wasan D.
'	Create Date		    : 07-10-2013
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

Namespace Service
    Public Class ImpVendorBranchService
        Implements IVendorBranchService

#Region "Property"
        Private objLog As New Common.Logs.Log
        Private objBranchEnt As New Entity.ImpVendorBranchEntity
#End Region

#Region "Function"
        '/**************************************************************
        '	Function name	: GetBranchWithVendorID
        '	Discription	    : Get Branch Vendor with VendorID for edit
        '	Return Value	: Datatable
        '	Create User	    : Wasan D.
        '	Create Date	    : 07-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetBranchWithVendorID(ByVal intVendorID As Integer) As System.Data.DataTable _
            Implements IVendorBranchService.GetBranchWithVendorID
            GetBranchWithVendorID = New DataTable
            Try
                Dim listVBEnt As New List(Of Entity.ImpVendorBranchEntity)
                Dim dr As DataRow
                Dim ItemIndex As Integer = 0
                ' Variable datatable column name
                Dim columnName() As String = {"index", "id", "name", "vendorID", "address", "zipcode", "countryID", "countryName", _
                                              "fullAddress", "telNo", "faxNo", "email", "contact", "remarks", "delete_fg"}
                listVBEnt = objBranchEnt.GetBranchWithVendorID(intVendorID)
                With GetBranchWithVendorID
                    For Each strTmp As String In columnName
                        GetBranchWithVendorID.Columns.Add(strTmp)
                    Next
                End With
                For Each item As Entity.ImpVendorBranchEntity In listVBEnt
                    dr = GetBranchWithVendorID.NewRow
                    dr("index") = ItemIndex
                    dr("id") = item.id
                    dr("name") = item.name
                    dr("vendorID") = item.vendorID
                    dr("address") = item.address
                    dr("zipcode") = item.zipcode
                    dr("countryID") = item.countryID
                    dr("countryName") = item.countryName
                    dr("fullAddress") = item.fullAddress
                    dr("telNo") = item.telNo
                    dr("faxNo") = item.faxNo
                    dr("email") = item.email
                    dr("contact") = item.contact
                    dr("remarks") = item.remarks
                    dr("delete_fg") = 0
                    GetBranchWithVendorID.Rows.Add(dr)
                    ItemIndex += 1
                Next

            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("GetBranchWithVendorID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckBranchIsInUse
        '	Discription	    : Get Branch Vendor already in use
        '	Return Value	: Boolean
        '	Create User	    : Wasan D.
        '	Create Date	    : 07-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckBranchIsInUse(ByVal intBranchID As Integer) As Boolean Implements IVendorBranchService.CheckBranchIsInUse
            Try
                If objBranchEnt.CheckBranchIsInUse(intBranchID) > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("CheckBranchIsInUse(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SaveVendorBranch
        '	Discription	    : SaveVendorBranch
        '	Return Value	: Boolean
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SaveVendorBranch(ByVal dtBranch As System.Data.DataTable, ByVal intVendorID As Integer) As Boolean _
        Implements IVendorBranchService.SaveVendorBranch
            SaveVendorBranch = False
            Try
                Dim listVBEnt As New List(Of Entity.ImpVendorBranchEntity)
                listVBEnt = SetDatatableToEntity(dtBranch, intVendorID)
                If objBranchEnt.SaveVendorBranch(listVBEnt) > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("SaveVendorBranch(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDatatableToEntity
        '	Discription	    : SetDatatableToEntity
        '	Return Value	: List(Of Entity.ImpVendorBranchEntity)
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDatatableToEntity(ByVal dtBranch As System.Data.DataTable, ByVal intVendorID As Integer) _
        As List(Of Entity.ImpVendorBranchEntity)
            SetDatatableToEntity = New List(Of Entity.ImpVendorBranchEntity)
            Try
                For Each row As DataRow In dtBranch.Rows
                    objBranchEnt = New Entity.ImpVendorBranchEntity
                    With objBranchEnt
                        .id = IIf(row("id") <> "", row("id"), 0)
                        .name = row("name")
                        .vendorID = IIf(row("vendorID") <> "", row("vendorID"), intVendorID)
                        .address = row("address")
                        .zipcode = row("zipcode")
                        .countryID = row("countryID")
                        .countryName = row("countryName")
                        .fullAddress = row("fullAddress")
                        .telNo = row("telNo")
                        .faxNo = row("faxNo")
                        .email = row("email")
                        .contact = row("contact")
                        .remarks = row("remarks")
                        .delete_fg = row("delete_fg")
                    End With
                    SetDatatableToEntity.Add(objBranchEnt)
                Next
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("SetDatatableToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetVendorBranchForDDLList
        '	Discription	    : GetVendorBranchForDDLList
        '	Return Value	: List(Of Dto.VendorBranchDto)
        '	Create User	    : Wasan D.
        '	Create Date	    : 14-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVendorBranchForDDLList(ByVal intVendorID As Integer) As System.Collections.Generic.List(Of Dto.VendorBranchDto) Implements IVendorBranchService.GetVendorBranchForDDLList
            GetVendorBranchForDDLList = New List(Of Dto.VendorBranchDto)
            Try
                ' objVendorDto for keep value Dto 
                Dim objVBDto As Dto.VendorBranchDto
                ' listVendorEnt for keep value from entity
                Dim listVBEnt As New List(Of Entity.ImpVendorBranchEntity)
                ' objVendorEnt for call function
                Dim objVBEnt As New Entity.ImpVendorBranchEntity

                ' call function GetVendorForList
                listVBEnt = objVBEnt.GetVendorBranchForDDLList(intVendorID)

                ' loop listVendorEnt for assign value to Dto
                For Each values In listVBEnt
                    ' new object
                    objVBDto = New Dto.VendorBranchDto
                    ' assign value to Dto
                    With objVBDto
                        .id = values.id
                        .fullAddress = values.fullAddress
                    End With
                    ' add object Dto to list Dto
                    GetVendorBranchForDDLList.Add(objVBDto)
                Next
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("GetVendorBranchForDDLList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region
    End Class
End Namespace