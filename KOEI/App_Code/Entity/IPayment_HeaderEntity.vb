#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IPayment_HeaderEntity
'	Class Discription	: Interface of table payment_header
'	Create User 		: Boonyarit
'	Create Date		    : 14-06-2013
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
    Public Interface IPayment_HeaderEntity

#Region "Property"
        Property id() As Integer
        Property po_header_id() As Integer
        Property delivery_date() As String
        Property payment_date() As String
        Property invoice_no() As String
        Property account_type() As Integer
        Property account_no() As String
        Property account_name() As String
        Property total_delivery_amount() As Decimal
        Property remark() As String
        Property user_id() As Integer
        Property status_id() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property update_by() As Integer
        Property update_date() As String
#End Region

#Region "Function"
        Function CheckPaymentByPurchase(ByVal intPurchase_id As Integer) As Boolean
#End Region

    End Interface
End Namespace

