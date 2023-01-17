#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMst_PaymentConditionDao
'	Class Discription	: Interface of table mst_payment_condition
'	Create User 		: Suwishaya L.
'	Create Date		    : 17-06-2013
'
' UPDATE INFORMATION
'	Update User		: Wasan D.
'	Update Date		: 03-07-2013
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Dao
    Public Interface IMst_PaymentConditionDao
        Function GetPaymentConditionForList() As List(Of Entity.IMst_PaymentConditionEntity)

        'Update by Wasan D. On 03-07-2013
        Function GetPaymentCondList(ByVal strFirst As String, ByVal strSecond As String, ByVal strThird As String) As List(Of Entity.ImpMst_PaymentConditionEntity)
        Function DeletePaymentCond(ByVal intPayID As Integer) As Integer
        Function GetPaymentCondByID(ByVal intPayID As Integer) As Entity.IMst_PaymentConditionEntity
        Function InsertPaymentCond(ByVal objPayEnt As Entity.IMst_PaymentConditionEntity) As Integer
        Function UpdatePaymentCond(ByVal objPayEnt As Entity.IMst_PaymentConditionEntity) As Integer
        Function CountUsedInPO(ByVal intPayID As Integer) As Integer
        Function GetPayCondDupInsert(ByVal objPayEnt As Entity.IMst_PaymentConditionEntity) As Boolean
        Function GetPayCondDupUpdate(ByVal objPayEnt As Entity.IMst_PaymentConditionEntity) As Boolean
        'End Update by Wasan
    End Interface
End Namespace
