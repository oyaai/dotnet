#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IPo_DetailEntity
'	Class Discription	: Interface of table po_detail
'	Create User 		: Boon
'	Create Date		    : 04-06-2013
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
    Public Interface IPo_DetailEntity
        Property id() As Integer
        Property po_header_id() As Integer
        Property item_id() As Integer
        Property job_order() As String
        Property ie_id() As Integer
        Property unit_price() As Decimal
        Property quantity() As Integer
        Property unit_id() As Integer
        Property discount() As Decimal
        Property discount_type() As Integer
        Property amount() As Decimal
        Property vat_amount() As Decimal
        Property wt_amount() As Decimal
        Property remark() As String
        Property created_by() As Integer
        Property created_date() As DateString
        Property updated_by() As Integer
        Property updated_date() As DateString

        Function CheckPoByUnit(ByVal intUnit_id As Integer) As Boolean

    End Interface
End Namespace

