#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IVendorService
'	Class Discription	: Interface of Vendor
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
#End Region

Namespace Service
    Public Interface IVendorService
        Function SetListVendorName(ByRef objValue As DropDownList) As Boolean
        Function GetVendorForSearch(ByVal objValue As Dto.VendorDto, ByRef objDT As System.Data.DataTable) As Boolean
        Function CheckVendorForDel(ByVal intVendorId As Integer) As Boolean
        Function CancelVendor(ByVal intVendorID As Integer) As Boolean
        Function GetVendorForDetail(ByVal intVendorId As Integer) As Dto.VendorDto
        'Function CheckNotDupVendor(ByVal intType1 As Integer, ByVal intType2 As Integer, ByVal strVandorName As String) As Boolean
        Function CheckIsDupVendor(ByVal objVendorDto As Dto.VendorDto) As Boolean
        Function SaveVendor(ByRef objVendorDto As Dto.VendorDto, ByVal strMode As String) As Boolean
        Function GetVendorForReport(ByVal objValue As Dto.VendorDto) As Boolean
        Function GetVendorForList(Optional ByVal intType1 As String = Nothing) As List(Of Dto.VendorDto)
        ' Add by Suwishaya L. 2013-06-17 (for Job Order)
        Function GetVendorListForJobOrder() As List(Of Dto.VendorDto)
    End Interface
End Namespace

