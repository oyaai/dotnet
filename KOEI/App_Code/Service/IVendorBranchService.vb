#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IVendorBranchService
'	Class Discription	: Interface of Branch Vendor
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

Imports System.Data
Imports Microsoft.VisualBasic


Namespace Service
    Public Interface IVendorBranchService
        Function GetBranchWithVendorID(ByVal intVendorID As Integer) As DataTable
        Function CheckBranchIsInUse(ByVal intBranchID As Integer) As Boolean
        Function SaveVendorBranch(ByVal dtBranch As DataTable, ByVal intVendorID As Integer) As Boolean
        Function GetVendorBranchForDDLList(ByVal intVendorID As Integer) As List(Of Dto.VendorBranchDto)
    End Interface
End Namespace
