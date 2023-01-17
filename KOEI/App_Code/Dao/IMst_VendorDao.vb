#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMst_VendorDao
'	Class Discription	: Interface of table mst_vendor
'	Create User 		: Boon
'	Create Date		    : 17-05-2013
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

Namespace Dao
    Public Interface IMst_VendorDao
        Function DB_CheckIsDupVendor(ByVal intType1 As Integer, ByVal intType2 As Integer, ByVal strName As String, Optional ByVal intId As Integer = 0) As Boolean
        Function DB_InsertVendor(ByVal objVendor As Entity.IMst_VendorEntity, ByRef intVendorId As Integer) As Boolean
        Function DB_UpdateVendor(ByVal objVendor As Entity.IMst_VendorEntity) As Boolean
        Function DB_CancelVendor(ByVal intVendor As Integer) As Boolean
        Function DB_GetFileNameById(ByVal intVendorId As Integer) As String

        Function GetVendorForList(Optional ByVal intType1 As String = Nothing) As List(Of Entity.IMst_VendorEntity)

        'Class sub vendor
        Function DB_GetVendorForSearch(ByVal intType1 As Integer, ByVal intType2 As Integer, _
                                       ByVal strName As String, ByVal intCountry_id As Integer) _
                                       As List(Of Entity.ImpSubMst_VendorEntity)
        Function DB_GetVendorForReport(ByVal intType1 As Integer, ByVal intType2 As Integer, _
                                       ByVal strName As String, ByVal intCountry_id As Integer) _
                                       As List(Of Entity.ImpVendorBranchEntity)
        Function DB_GetVendorForDetail(ByVal intID As Integer) As Entity.ImpSubMst_VendorEntity
        ' Add by Suwishaya L. 2013-06-17 (for Job Order)
        Function GetVendorListForJobOrder() As List(Of Entity.IMst_VendorEntity)
    End Interface
End Namespace

