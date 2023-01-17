#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IReceive_HeaderEntity
'	Class Discription	: Interface of table receive_header
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
    Public Interface IReceive_HeaderEntity
        Property id() As Integer
        Property invoice_no() As String
        Property receipt_date() As String
        Property ie_id() As Integer
        Property vendor_id() As Integer
        Property account_type() As Integer
        Property invoice_type() As Integer
        Property bank_fee() As Decimal
        Property total_amount() As Decimal
        Property user_id() As Integer
        Property status_id() As Integer
        Property customer() As Integer
        Property issue_date() As String
        Property created_by() As Integer
        Property created_date() As String
        Property updated_by() As Integer
        Property updated_date() As String

        Function CheckReceiveByVendor(ByVal intVendor_id As Integer) As Boolean

    End Interface
End Namespace

