#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_PaymentConditionEntity
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

Namespace Entity
    Public Interface IMst_PaymentConditionEntity

#Region "Property"
        Property id() As Integer
        Property first() As Integer
        Property second() As Integer
        Property third() As Integer
        Property delete_fg() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property updated_by() As Integer
        Property updated_date() As String

        'property for dropdrown List
        Property condition_id() As Integer
        Property condition_name() As String
        Property payment_condition() As String

#End Region

#Region "Function"
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

#End Region

    End Interface
End Namespace
