#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IVendorBranchEntity
'	Class Discription	: Interface of table mst_vendor_branch
'	Create User 		: Wasan D.
'	Create Date		    : 26-09-2013
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

Namespace Entity
    Public Interface IVendorBranchEntity

#Region "Property"
        Property id() As Integer
        Property name() As String
        Property vendorID() As Integer
        Property address() As String
        Property zipcode() As String
        Property countryID() As Integer
        Property countryName() As String
        Property fullAddress() As String
        Property telNo() As String
        Property faxNo() As String
        Property email() As String
        Property contact() As String
        Property remarks() As String
        Property typeOfGoods() As String
        Property delete_fg() As Integer

#End Region

#Region "Function"
        Function GetBranchWithVendorID(ByVal intVendorID As Integer) As List(Of Entity.ImpVendorBranchEntity)
        Function CheckBranchIsInUse(ByVal intBranchID As Integer) As Integer
        Function SaveVendorBranch(ByVal listBranchEnt As List(Of Entity.ImpVendorBranchEntity)) As Integer
        Function GetVendorBranchForDDLList(ByVal intVendorID As Integer) As List(Of Entity.ImpVendorBranchEntity)
#End Region

    End Interface
End Namespace
