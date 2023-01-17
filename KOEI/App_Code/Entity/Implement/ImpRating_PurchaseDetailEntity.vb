'Imports Microsoft.VisualBasic

'Public Class ImpRating_PurchaseDetailEntity

'End Class
#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpRating_PurchaseDetailEntity
'	Class Discription	: Class of Accounting detail
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 07-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports Microsoft.VisualBasic

Namespace Entity
    Public Class ImpRating_PurchaseDetailEntity
        Inherits Entity.ImpRating_PurchaseEntity

        Private _po_type As String
        Private _vendor_name As String
        Private _delivery_plan As String
        Private _delivery_year As String
        Private _delivery_date_po As String
        Private _grp As String
#Region "Property"
        Property grp() As String
            Get
                Return _grp
            End Get
            Set(ByVal value As String)
                _grp = value
            End Set
        End Property
        Property delivery_date_po() As String
            Get
                Return _delivery_date_po
            End Get
            Set(ByVal value As String)
                _delivery_date_po = value
            End Set
        End Property
        Property po_type() As String
            Get
                Return _po_type
            End Get
            Set(ByVal value As String)
                _po_type = value
            End Set
        End Property
        Property delivery_year() As String
            Get
                Return _delivery_year
            End Get
            Set(ByVal value As String)
                _delivery_year = value
            End Set
        End Property
        Property delivery_plan() As String
            Get
                Return _delivery_plan
            End Get
            Set(ByVal value As String)
                _delivery_plan = value
            End Set
        End Property
        Property vendor_name() As String
            Get
                Return _vendor_name
            End Get
            Set(ByVal value As String)
                _vendor_name = value
            End Set
        End Property
#End Region

    End Class
End Namespace

