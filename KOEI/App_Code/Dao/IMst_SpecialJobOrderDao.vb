#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMst_SpecialJobOrderDao
'	Class Discription	: Interface of table job_order_special
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
#End Region

Namespace Dao
    Public Interface IMst_SpecialJobOrderDao

        Function GetSpecialJobOrderList(ByVal strJobOrderFrom As String, ByVal strJobOrderTo As String) As List(Of Entity.ImpMst_SpecialJobOrderEntity)
        Function DeleteSpecialJobOrder(ByVal intJobOrderID As Integer) As Integer
        Function GetSpecialJobOrderByID(ByVal intJobOrderID As Integer) As Entity.IMst_SpecialJobOrderEntiy
        Function InsertSpecialJobOrder(ByVal objSpecialJobOrderEnt As Entity.IMst_SpecialJobOrderEntiy) As Integer
        Function UpdateSpecialJobOrder(ByVal objSpecialJobOrderEnt As Entity.IMst_SpecialJobOrderEntiy) As Integer
        Function CountUsedInSpecialJobOrder(ByVal strJobOrderID As String) As Integer
        Function CountUsedInReceiveDetail(ByVal strJobOrderID As String) As Integer
        Function CheckDupSpecialJobOrder(ByVal intJobOrderID As Integer, ByVal strJobOrder As String) As Integer
        Function CheckDupJobOrder(ByVal strJobOrder As String) As Integer
    End Interface
End Namespace

