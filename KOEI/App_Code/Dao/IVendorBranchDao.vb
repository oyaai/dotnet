#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IVendorBranchDao
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

Namespace Dao
    Public Interface IVendorBranchDao
        Function GetBranchWithVendorID(ByVal intVendorID As Integer) As List(Of Entity.ImpVendorBranchEntity)
        Function CheckBranchIsInUse(ByVal intBranchID As Integer) As Integer
        Function SaveVendorBranch(ByVal listBranchEnt As List(Of Entity.ImpVendorBranchEntity)) As Integer
        Function GetVendorBranchForDDLList(ByVal intVendorID As Integer) As List(Of Entity.ImpVendorBranchEntity)
    End Interface
End Namespace
