#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpSubMst_VendorEntity
'	Class Discription	: Class of sub table mst_vendor
'	Create User 		: Boon
'	Create Date		    : 14-05-2013
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
    Public Class ImpSubMst_VendorEntity
        Inherits ImpMst_VendorEntity

        'Add from table mst_country
        Private _country_name As String
        'Add from query
        Private _type1_text As String
        Private _type2_text As String
        Private _type_of_goods_text As String
        Private _payment_term As String
        Private _payment_condition As String

#Region "Property"
        Public Property payment_condition() As String
            Get
                Return _payment_condition
            End Get
            Set(ByVal value As String)
                _payment_condition = value
            End Set
        End Property

        Public Property payment_term() As String
            Get
                Return _payment_term
            End Get
            Set(ByVal value As String)
                _payment_term = value
            End Set
        End Property

        Public Property type_of_goods_text() As String
            Get
                Return _type_of_goods_text
            End Get
            Set(ByVal value As String)
                _type_of_goods_text = value
            End Set
        End Property

        Public Property type2_text() As String
            Get
                Return _type2_text
            End Get
            Set(ByVal value As String)
                _type2_text = value
            End Set
        End Property

        Public Property type1_text() As String
            Get
                Return _type1_text
            End Get
            Set(ByVal value As String)
                _type1_text = value
            End Set
        End Property

        Public Property country_name() As String
            Get
                Return _country_name
            End Get
            Set(ByVal value As String)
                _country_name = value
            End Set
        End Property
#End Region

    End Class
End Namespace

