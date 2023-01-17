#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_VendorEntity
'	Class Discription	: Interface of table mst_vendor
'	Create User 		: Boon
'	Create Date		    : 13-05-2013
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

Namespace Entity
    Public Interface IMst_VendorEntity

#Region "Property"
        Property id() As Integer
        Property type1() As Integer
        Property type2() As Integer
        Property type2_no() As String
        Property name() As String
        Property short_name() As String
        Property person_in_charge1() As String
        Property person_in_charge2() As String
        Property payment_term_id() As Integer
        Property payment_cond1() As Integer
        Property payment_cond2() As Integer
        Property payment_cond3() As Integer
        Property country_id() As Integer
        Property zipcode() As String
        Property address() As String
        Property tel() As String
        Property fax() As String
        Property remarks() As String
        Property file() As String
        Property email() As String
        Property type_of_goods() As String
        Property delete_fg() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property update_by() As Integer
        Property update_date() As String
        Property purchase_fg() As Integer
        Property outsource_fg() As Integer
        Property other_fg() As Integer
#End Region
       
#Region "Function"
        Function GetVendorForSearch(ByVal intType1 As Integer, ByVal intType2 As Integer, _
                                    ByVal strName As String, ByVal intCountry_id As Integer) _
                                    As List(Of Entity.ImpSubMst_VendorEntity)
        Function GetVendorForReport(ByVal intType1 As Integer, ByVal intType2 As Integer, _
                                       ByVal strName As String, ByVal intCountry_id As Integer) _
                                       As List(Of Entity.ImpVendorBranchEntity)
        Function GetVendorForDetail(ByVal intID As Integer) As Entity.ImpSubMst_VendorEntity
        Function CheckIsDupVendor(ByVal intType1 As Integer, ByVal intType2 As Integer, ByVal strName As String, Optional ByVal intId As Integer = 0) As Boolean
        Function InsertVendor(ByVal objVendor As Entity.IMst_VendorEntity, ByRef intVendorId As Integer) As Boolean
        Function UpdateVendor(ByVal objVendor As Entity.IMst_VendorEntity) As Boolean
        Function CancelVendor(ByVal intVendor As Integer) As Boolean
        Function GetFileNameById(ByVal intVendorId As Integer) As String
        Function GetVendorForList(Optional ByVal intType1 As String = Nothing) As List(Of Entity.IMst_VendorEntity)
        ' Add by Suwishaya L. 2013-06-17 (for Job Order)
        Function GetVendorListForJobOrder() As List(Of Entity.IMst_VendorEntity)
#End Region
     
    End Interface
End Namespace

