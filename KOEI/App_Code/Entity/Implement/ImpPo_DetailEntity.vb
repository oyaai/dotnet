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
    Public Class ImpPo_DetailEntity
        Implements IPo_DetailEntity


        Private _id As Integer
        Private _po_header_id As Integer
        Private _item_id As Integer
        Private _job_order As String
        Private _ie_id As Integer
        Private _unit_price As Decimal
        Private _quantity As Integer
        Private _unit_id As Integer
        Private _discount As Decimal
        Private _discount_type As Integer
        Private _amount As Decimal
        Private _vat_amount As Decimal
        Private _wt_amount As Decimal
        Private _remark As String
        Private _created_by As Integer
        Private _created_date As DateString
        Private _updated_by As Integer
        Private _updated_date As DateString

        Private objPoDetailDao As New Dao.ImpPo_DetailDao

#Region "Function"
        Public Function CheckPoByUnit(ByVal intUnit_id As Integer) As Boolean Implements IPo_DetailEntity.CheckPoByUnit
            Return objPoDetailDao.DB_CheckPoByUnit(intUnit_id)
        End Function
#End Region

#Region "Property"
        Public Property amount() As Decimal Implements IPo_DetailEntity.amount
            Get
                Return _amount
            End Get
            Set(ByVal value As Decimal)
                _amount = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IPo_DetailEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As DateString Implements IPo_DetailEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As DateString)
                _created_date = value
            End Set
        End Property

        Public Property discount() As Decimal Implements IPo_DetailEntity.discount
            Get
                Return _discount
            End Get
            Set(ByVal value As Decimal)
                _discount = value
            End Set
        End Property

        Public Property discount_type() As Integer Implements IPo_DetailEntity.discount_type
            Get
                Return _discount_type
            End Get
            Set(ByVal value As Integer)
                _discount_type = value
            End Set
        End Property

        Public Property id() As Integer Implements IPo_DetailEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property ie_id() As Integer Implements IPo_DetailEntity.ie_id
            Get
                Return _ie_id
            End Get
            Set(ByVal value As Integer)
                _ie_id = value
            End Set
        End Property

        Public Property item_id() As Integer Implements IPo_DetailEntity.item_id
            Get
                Return _item_id
            End Get
            Set(ByVal value As Integer)
                _item_id = value
            End Set
        End Property

        Public Property job_order() As String Implements IPo_DetailEntity.job_order
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Public Property po_header_id() As Integer Implements IPo_DetailEntity.po_header_id
            Get
                Return _po_header_id
            End Get
            Set(ByVal value As Integer)
                _po_header_id = value
            End Set
        End Property

        Public Property quantity() As Integer Implements IPo_DetailEntity.quantity
            Get
                Return _quantity
            End Get
            Set(ByVal value As Integer)
                _quantity = value
            End Set
        End Property

        Public Property remark() As String Implements IPo_DetailEntity.remark
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
            End Set
        End Property

        Public Property unit_id() As Integer Implements IPo_DetailEntity.unit_id
            Get
                Return _unit_id
            End Get
            Set(ByVal value As Integer)
                _unit_id = value
            End Set
        End Property

        Public Property unit_price() As Decimal Implements IPo_DetailEntity.unit_price
            Get
                Return _unit_price
            End Get
            Set(ByVal value As Decimal)
                _unit_price = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IPo_DetailEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As DateString Implements IPo_DetailEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As DateString)
                _updated_date = value
            End Set
        End Property

        Public Property vat_amount() As Decimal Implements IPo_DetailEntity.vat_amount
            Get
                Return _vat_amount
            End Get
            Set(ByVal value As Decimal)
                _vat_amount = value
            End Set
        End Property

        Public Property wt_amount() As Decimal Implements IPo_DetailEntity.wt_amount
            Get
                Return _wt_amount
            End Get
            Set(ByVal value As Decimal)
                _wt_amount = value
            End Set
        End Property
#End Region
        
    End Class
End Namespace

