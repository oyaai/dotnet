#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : FinishGoodsDto
'	Class Discription	: Dto class Finish Goods
'	Create User 		: Suwishaya L.
'	Create Date		    : 02-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Import"
Imports Microsoft.VisualBasic
#End Region

Namespace Dto
    Public Class FinishGoodsDto

#Region "Fields"
        'Receive data from screen (condition search)
        Private _job_order_from_search As String
        Private _job_order_to_search As String
        Private _job_type_search As String
        Private _part_no_search As String
        Private _part_name_search As String
        Private _customer_search As String
        Private _person_charge_search As String
        Private _issue_date_from_search As String
        Private _issue_date_to_search As String
        Private _finish_date_from_search As String
        Private _finish_date_to_search As String
        Private _boi_search As String
        Private _receive_po_search As String

        'Receive data detail 
        Private _id As Integer
        Private _job_order As String
        Private _issue_date As String
        Private _finish_date As String
        Private _customer_name As String
        Private _job_order_type_name As String
        Private _part_name As String
        Private _part_no As String
        Private _amount As String
        Private _person_in_charge_id As Integer
        Private _person_in_charge_name As String

#End Region

#Region "Property"

        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property amount() As String
            Get
                Return _amount
            End Get
            Set(ByVal value As String)
                _amount = value
            End Set
        End Property

        Public Property boi_search() As String
            Get
                Return _boi_search
            End Get
            Set(ByVal value As String)
                _boi_search = value
            End Set
        End Property

        Public Property customer_name() As String
            Get
                Return _customer_name
            End Get
            Set(ByVal value As String)
                _customer_name = value
            End Set
        End Property

        Public Property customer_search() As String
            Get
                Return _customer_search
            End Get
            Set(ByVal value As String)
                _customer_search = value
            End Set
        End Property

        Public Property finish_date() As String
            Get
                Return _finish_date
            End Get
            Set(ByVal value As String)
                _finish_date = value
            End Set
        End Property

        Public Property finish_date_from_search() As String
            Get
                Return _finish_date_from_search
            End Get
            Set(ByVal value As String)
                _finish_date_from_search = value
            End Set
        End Property

        Public Property finish_date_to_search() As String
            Get
                Return _finish_date_to_search
            End Get
            Set(ByVal value As String)
                _finish_date_to_search = value
            End Set
        End Property

        Public Property issue_date() As String
            Get
                Return _issue_date
            End Get
            Set(ByVal value As String)
                _issue_date = value
            End Set
        End Property

        Public Property issue_date_from_search() As String
            Get
                Return _issue_date_from_search
            End Get
            Set(ByVal value As String)
                _issue_date_from_search = value
            End Set
        End Property

        Public Property issue_date_to_search() As String
            Get
                Return _issue_date_to_search
            End Get
            Set(ByVal value As String)
                _issue_date_to_search = value
            End Set
        End Property

        Public Property job_order() As String
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Public Property job_order_from_search() As String
            Get
                Return _job_order_from_search
            End Get
            Set(ByVal value As String)
                _job_order_from_search = value
            End Set
        End Property

        Public Property job_order_to_search() As String
            Get
                Return _job_order_to_search
            End Get
            Set(ByVal value As String)
                _job_order_to_search = value
            End Set
        End Property

        Public Property job_order_type_name() As String
            Get
                Return _job_order_type_name
            End Get
            Set(ByVal value As String)
                _job_order_type_name = value
            End Set
        End Property

        Public Property job_type_search() As String
            Get
                Return _job_type_search
            End Get
            Set(ByVal value As String)
                _job_type_search = value
            End Set
        End Property

        Public Property part_name() As String
            Get
                Return _part_name
            End Get
            Set(ByVal value As String)
                _part_name = value
            End Set
        End Property

        Public Property part_name_search() As String
            Get
                Return _part_name_search
            End Get
            Set(ByVal value As String)
                _part_name_search = value
            End Set
        End Property

        Public Property part_no() As String
            Get
                Return _part_no
            End Get
            Set(ByVal value As String)
                _part_no = value
            End Set
        End Property

        Public Property part_no_search() As String
            Get
                Return _part_no_search
            End Get
            Set(ByVal value As String)
                _part_no_search = value
            End Set
        End Property

        Public Property person_charge_search() As String
            Get
                Return _person_charge_search
            End Get
            Set(ByVal value As String)
                _person_charge_search = value
            End Set
        End Property

        Public Property receive_po_search() As String
            Get
                Return _receive_po_search
            End Get
            Set(ByVal value As String)
                _receive_po_search = value
            End Set
        End Property

        Public Property person_in_charge_id() As Integer
            Get
                Return _person_in_charge_id
            End Get
            Set(ByVal value As Integer)
                _person_in_charge_id = value
            End Set
        End Property

        Public Property person_in_charge_name() As String
            Get
                Return _person_in_charge_name
            End Get
            Set(ByVal value As String)
                _person_in_charge_name = value
            End Set
        End Property
#End Region

    End Class
End Namespace
