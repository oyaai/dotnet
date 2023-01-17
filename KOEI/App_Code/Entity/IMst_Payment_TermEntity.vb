#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_Payment_TermEntity
'	Class Discription	: Interface of table mst_payment_term
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
    Public Interface IMst_Payment_TermEntity
        Property id() As Integer
        Property term_day() As String
        Property delete_fg() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property updated_by() As Integer
        Property updated_date() As String

        Function GetListPaymentDay() As List(Of IMst_Payment_TermEntity)
        Function GetPaymentList(ByVal strPayment As String) As List(Of Entity.IMst_Payment_TermEntity)
        Function CountUsedInPO(ByVal intPaymentTermID As Integer) As Integer
        Function CountUsedInPO2(ByVal intPaymentTermID As Integer) As Integer
        Function DeletePaymentTerm(ByVal intPaymentTermID As Integer) As Integer
        Function GetPaymentTermByID(ByVal intPaymentTermID As Integer) As Entity.IMst_Payment_TermEntity
        Function InsertPaymentTerm(ByVal objPaymentTermEntity As Entity.IMst_Payment_TermEntity) As Integer
        Function UpdatePaymentTerm(ByVal objPaymentTermEntity As Entity.IMst_Payment_TermEntity) As Integer
        Function CheckDupInsert(ByVal intPayment As String) As Integer
        Function CheckDupUpdate(ByVal intPaymentTermID As String, ByVal intPayment As String) As Integer
    End Interface
End Namespace

