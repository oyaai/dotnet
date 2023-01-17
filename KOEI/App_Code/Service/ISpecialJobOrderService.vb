#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ISpecialJobOrderService
'	Class Discription	: Interface class Special Job Order service
'	Create User 		: Suwishaya L.
'	Create Date		    : 07-06-2013
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
#End Region

Namespace Service
    Public Interface ISpecialJobOrderService
        Function GetSpecialJobOrderList(ByVal strJobOrderFrom As String, ByVal strJobOrderTo As String) As DataTable
        Function DeleteSpecialJobOrder(ByVal intJobOrderID As Integer) As Boolean
        Function GetSpecialJobOrderByID(ByVal intJobOrderID As Integer) As Dto.SpecialJobOrderDto
        Function InsertSpecialJobOrder(ByVal objSpecialJobOrderDto As Dto.SpecialJobOrderDto, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateSpecialJobOrder(ByVal objSpecialJobOrderDto As Dto.SpecialJobOrderDto, Optional ByRef strMsg As String = "") As Boolean
        Function IsUsedInAccounting(ByVal strJobOrder As String) As Boolean
        Function CheckDupSpecialJobOrder(ByVal intJobOrderID As Integer, ByVal strJobOrder As String) As Boolean
    End Interface
End Namespace
