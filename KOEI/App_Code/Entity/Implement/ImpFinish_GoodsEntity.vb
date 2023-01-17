#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IFinish_GoodsEntity
'	Class Discription	: Class of table job order
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

#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Entity
    Public Class ImpFinish_GoodsEntity
        Implements IFinish_GoodsEntity

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
        Private _sum_amount As String
        Private _person_in_charge_id As Integer
        Private _person_in_charge_name As String
        Private _job_order_id As String
        Private _receive_header_id As String

        Private objFinishGoods As New Dao.ImpFinish_GoodsDao

#Region "Function"
        Public Function GetFinishGoodsList(ByVal objFinishGoodsEnt As IFinish_GoodsEntity) As System.Collections.Generic.List(Of ImpFinish_GoodsEntity) Implements IFinish_GoodsEntity.GetFinishGoodsList
            Return objFinishGoods.GetFinishGoodsList(objFinishGoodsEnt)
        End Function

        Public Function GetFinishGoodsReport(ByVal objFinishGoodsEnt As IFinish_GoodsEntity) As System.Collections.Generic.List(Of ImpFinish_GoodsEntity) Implements IFinish_GoodsEntity.GetFinishGoodsReport
            Return objFinishGoods.GetFinishGoodsReport(objFinishGoodsEnt)
        End Function

        Public Function GetSumFinishGoodsReport(ByVal objFinishGoodsEnt As IFinish_GoodsEntity) As System.Collections.Generic.List(Of ImpFinish_GoodsEntity) Implements IFinish_GoodsEntity.GetSumFinishGoodsReport
            Return objFinishGoods.GetSumFinishGoodsReport(objFinishGoodsEnt)
        End Function

        Public Function GetPersonInChangeForList() As System.Collections.Generic.List(Of IFinish_GoodsEntity) Implements IFinish_GoodsEntity.GetPersonInChangeForList
            Return objFinishGoods.GetPersonInChangeForList()
        End Function
#End Region

#Region "Property"

        Public Property id() As Integer Implements IFinish_GoodsEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property
        Public Property receive_header_id() As String Implements IFinish_GoodsEntity.receive_header_id
            Get
                Return _receive_header_id
            End Get
            Set(ByVal value As String)
                _receive_header_id = value
            End Set
        End Property
        Public Property job_order_id() As String Implements IFinish_GoodsEntity.job_order_id
            Get
                Return _job_order_id
            End Get
            Set(ByVal value As String)
                _job_order_id = value
            End Set
        End Property

        Public Property amount() As String Implements IFinish_GoodsEntity.amount
            Get
                Return _amount
            End Get
            Set(ByVal value As String)
                _amount = value
            End Set
        End Property

        Public Property sum_amount() As String Implements IFinish_GoodsEntity.sum_amount
            Get
                Return _sum_amount
            End Get
            Set(ByVal value As String)
                _sum_amount = value
            End Set
        End Property

        Public Property boi_search() As String Implements IFinish_GoodsEntity.boi_search
            Get
                Return _boi_search
            End Get
            Set(ByVal value As String)
                _boi_search = value
            End Set
        End Property

        Public Property customer_name() As String Implements IFinish_GoodsEntity.customer_name
            Get
                Return _customer_name
            End Get
            Set(ByVal value As String)
                _customer_name = value
            End Set
        End Property

        Public Property customer_search() As String Implements IFinish_GoodsEntity.customer_search
            Get
                Return _customer_search
            End Get
            Set(ByVal value As String)
                _customer_search = value
            End Set
        End Property

        Public Property finish_date() As String Implements IFinish_GoodsEntity.finish_date
            Get
                Return _finish_date
            End Get
            Set(ByVal value As String)
                _finish_date = value
            End Set
        End Property

        Public Property finish_date_from_search() As String Implements IFinish_GoodsEntity.finish_date_from_search
            Get
                Return _finish_date_from_search
            End Get
            Set(ByVal value As String)
                _finish_date_from_search = value
            End Set
        End Property

        Public Property finish_date_to_search() As String Implements IFinish_GoodsEntity.finish_date_to_search
            Get
                Return _finish_date_to_search
            End Get
            Set(ByVal value As String)
                _finish_date_to_search = value
            End Set
        End Property

        Public Property issue_date() As String Implements IFinish_GoodsEntity.issue_date
            Get
                Return _issue_date
            End Get
            Set(ByVal value As String)
                _issue_date = value
            End Set
        End Property

        Public Property issue_date_from_search() As String Implements IFinish_GoodsEntity.issue_date_from_search
            Get
                Return _issue_date_from_search
            End Get
            Set(ByVal value As String)
                _issue_date_from_search = value
            End Set
        End Property

        Public Property issue_date_to_search() As String Implements IFinish_GoodsEntity.issue_date_to_search
            Get
                Return _issue_date_to_search
            End Get
            Set(ByVal value As String)
                _issue_date_to_search = value
            End Set
        End Property

        Public Property job_order() As String Implements IFinish_GoodsEntity.job_order
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Public Property job_order_from_search() As String Implements IFinish_GoodsEntity.job_order_from_search
            Get
                Return _job_order_from_search
            End Get
            Set(ByVal value As String)
                _job_order_from_search = value
            End Set
        End Property

        Public Property job_order_to_search() As String Implements IFinish_GoodsEntity.job_order_to_search
            Get
                Return _job_order_to_search
            End Get
            Set(ByVal value As String)
                _job_order_to_search = value
            End Set
        End Property

        Public Property job_order_type_name() As String Implements IFinish_GoodsEntity.job_order_type_name
            Get
                Return _job_order_type_name
            End Get
            Set(ByVal value As String)
                _job_order_type_name = value
            End Set
        End Property

        Public Property job_type_search() As String Implements IFinish_GoodsEntity.job_type_search
            Get
                Return _job_type_search
            End Get
            Set(ByVal value As String)
                _job_type_search = value
            End Set
        End Property

        Public Property part_name() As String Implements IFinish_GoodsEntity.part_name
            Get
                Return _part_name
            End Get
            Set(ByVal value As String)
                _part_name = value
            End Set
        End Property

        Public Property part_name_search() As String Implements IFinish_GoodsEntity.part_name_search
            Get
                Return _part_name_search
            End Get
            Set(ByVal value As String)
                _part_name_search = value
            End Set
        End Property

        Public Property part_no() As String Implements IFinish_GoodsEntity.part_no
            Get
                Return _part_no
            End Get
            Set(ByVal value As String)
                _part_no = value
            End Set
        End Property

        Public Property part_no_search() As String Implements IFinish_GoodsEntity.part_no_search
            Get
                Return _part_no_search
            End Get
            Set(ByVal value As String)
                _part_no_search = value
            End Set
        End Property

        Public Property person_charge_search() As String Implements IFinish_GoodsEntity.person_charge_search
            Get
                Return _person_charge_search
            End Get
            Set(ByVal value As String)
                _person_charge_search = value
            End Set
        End Property

        Public Property receive_po_search() As String Implements IFinish_GoodsEntity.receive_po_search
            Get
                Return _receive_po_search
            End Get
            Set(ByVal value As String)
                _receive_po_search = value
            End Set
        End Property

        Public Property person_in_charge_id() As Integer Implements IFinish_GoodsEntity.person_in_charge_id
            Get
                Return _person_in_charge_id
            End Get
            Set(ByVal value As Integer)
                _person_in_charge_id = value
            End Set
        End Property

        Public Property person_in_charge_name() As String Implements IFinish_GoodsEntity.person_in_charge_name
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

