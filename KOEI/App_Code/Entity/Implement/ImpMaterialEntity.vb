#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMaterialEntity
'	Class Discription	: Class of table stock
'	Create User 		: Suwishaya L.
'	Create Date		    : 03-07-2013
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
    Public Class ImpMaterialEntity
        Implements IMaterialEntity

        'Receive data from screen (condition search)
        Private _job_order As String
        Private _vendor As String
        Private _invoice_no As String
        Private _po_no As String
        Private _item_name As String
        Private _delivery_date_from As String
        Private _delivery_date_to As String

        'Receive data for detail search and report       
        Private _id As Integer
        Private _amount As String
        Private _delivery_date_in As String
        Private _qty_in As String
        Private _delivery_date_out As String
        Private _qty_out As String
        Private _qty_left As String
        Private _sum_amount As String

        Private objMaterial As New Dao.ImpMaterialDao

#Region "Function"
        Public Function GetMaterialList(ByVal objMaterialEnt As IMaterialEntity) As System.Collections.Generic.List(Of ImpMaterialEntity) Implements IMaterialEntity.GetMaterialList
            Return objMaterial.GetMaterialList(objMaterialEnt)
        End Function

        Public Function GetMaterialListReport(ByVal objMaterialEnt As IMaterialEntity) As System.Collections.Generic.List(Of ImpMaterialEntity) Implements IMaterialEntity.GetMaterialListReport
            Return objMaterial.GetMaterialListReport(objMaterialEnt)
        End Function

        Public Function GetSumMaterialListReport(ByVal objMaterialEnt As IMaterialEntity) As System.Collections.Generic.List(Of ImpMaterialEntity) Implements IMaterialEntity.GetSumMaterialListReport
            Return objMaterial.GetSumMaterialListReport(objMaterialEnt)
        End Function

#End Region

#Region "Property"

        Public Property id() As Integer Implements IMaterialEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property sum_amount() As String Implements IMaterialEntity.sum_amount
            Get
                Return _sum_amount
            End Get
            Set(ByVal value As String)
                _sum_amount = value
            End Set
        End Property

        Public Property amount() As String Implements IMaterialEntity.amount
            Get
                Return _amount
            End Get
            Set(ByVal value As String)
                _amount = value
            End Set
        End Property

        Public Property delivery_date_from() As String Implements IMaterialEntity.delivery_date_from
            Get
                Return _delivery_date_from
            End Get
            Set(ByVal value As String)
                _delivery_date_from = value
            End Set
        End Property

        Public Property delivery_date_to() As String Implements IMaterialEntity.delivery_date_to
            Get
                Return _delivery_date_to
            End Get
            Set(ByVal value As String)
                _delivery_date_to = value
            End Set
        End Property

        Public Property delivery_date_in() As String Implements IMaterialEntity.delivery_date_in
            Get
                Return _delivery_date_in
            End Get
            Set(ByVal value As String)
                _delivery_date_in = value
            End Set
        End Property

        Public Property delivery_date_out() As String Implements IMaterialEntity.delivery_date_out
            Get
                Return _delivery_date_out
            End Get
            Set(ByVal value As String)
                _delivery_date_out = value
            End Set
        End Property

        Public Property invoice_no() As String Implements IMaterialEntity.invoice_no
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As String)
                _invoice_no = value
            End Set
        End Property

        Public Property item_name() As String Implements IMaterialEntity.item_name
            Get
                Return _item_name
            End Get
            Set(ByVal value As String)
                _item_name = value
            End Set
        End Property

        Public Property job_order() As String Implements IMaterialEntity.job_order
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Public Property po_no() As String Implements IMaterialEntity.po_no
            Get
                Return _po_no
            End Get
            Set(ByVal value As String)
                _po_no = value
            End Set
        End Property

        Public Property qty_in() As String Implements IMaterialEntity.qty_in
            Get
                Return _qty_in
            End Get
            Set(ByVal value As String)
                _qty_in = value
            End Set
        End Property

        Public Property qty_left() As String Implements IMaterialEntity.qty_left
            Get
                Return _qty_left
            End Get
            Set(ByVal value As String)
                _qty_left = value
            End Set
        End Property

        Public Property qty_out() As String Implements IMaterialEntity.qty_out
            Get
                Return _qty_out
            End Get
            Set(ByVal value As String)
                _qty_out = value
            End Set
        End Property

        Public Property vendor() As String Implements IMaterialEntity.vendor
            Get
                Return _vendor
            End Get
            Set(ByVal value As String)
                _vendor = value
            End Set
        End Property


#End Region

       
    End Class
End Namespace
