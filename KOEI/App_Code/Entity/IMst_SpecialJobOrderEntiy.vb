#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_SpecialJobOrder
'	Class Discription	: Interface of table job order
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

Namespace Entity
    Public Interface IMst_SpecialJobOrderEntiy

        Property id() As Integer
        Property job_order() As String
        Property remark() As String
        Property created_by() As String
        Property created_date() As Integer
        Property updated_by() As String
        Property updated_date() As Integer
        Property delete_fg() As Integer

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

