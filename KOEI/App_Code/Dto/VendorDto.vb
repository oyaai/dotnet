#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : VendorDto
'	Class Discription	: Class of VendorDto
'	Create User 		: Boon
'	Create Date		    : 21-05-2013
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

Namespace Dto
    Public Class VendorDto
        'Data Search
        Private _intSType1 As Integer
        Private _intSType2 As Integer
        Private _strSName As String
        Private _intSCountry As Integer

        'Data Show for table
        Private _id As Integer
        Private _type1_text As String
        Private _type2_text As String
        Private _name As String
        Private _short_name As String
        Private _country_name As String

        'Data Show for page detail
        Private _type2_no As String
        Private _person_in_charge1 As String
        Private _person_in_charge2 As String
        Private _payment_term As String
        Private _payment_condition As String
        Private _zipcode As String
        Private _address As String
        Private _tel As String
        Private _fax As String
        Private _email As String
        Private _type_of_goods_text As String
        Private _remarks As String
        Private _file As String
        Private _purchase_fg As Integer
        Private _outsource_fg As Integer
        Private _other_fg As Integer

        'Data Save and Edit for page Main
        Private _type1 As Integer
        Private _type2 As Integer
        Private _payment_term_id As Integer
        Private _payment_cond1 As Integer
        Private _payment_cond2 As Integer
        Private _payment_cond3 As Integer
        Private _country_id As Integer
        Private _type_of_goods As String

        'Check delete file for page Main
        Private _checkDelete As Boolean


#Region "Property"
        Public Property checkDelete() As Boolean
            Get
                Return _checkDelete
            End Get
            Set(ByVal value As Boolean)
                _checkDelete = value
            End Set
        End Property

        Public Property type_of_goods() As String
            Get
                Return _type_of_goods
            End Get
            Set(ByVal value As String)
                _type_of_goods = value
            End Set
        End Property

        Public Property country_id() As Integer
            Get
                Return _country_id
            End Get
            Set(ByVal value As Integer)
                _country_id = value
            End Set
        End Property

        Public Property payment_cond3() As Integer
            Get
                Return _payment_cond3
            End Get
            Set(ByVal value As Integer)
                _payment_cond3 = value
            End Set
        End Property

        Public Property payment_cond2() As Integer
            Get
                Return _payment_cond2
            End Get
            Set(ByVal value As Integer)
                _payment_cond2 = value
            End Set
        End Property

        Public Property payment_cond1() As Integer
            Get
                Return _payment_cond1
            End Get
            Set(ByVal value As Integer)
                _payment_cond1 = value
            End Set
        End Property

        Public Property payment_term_id() As Integer
            Get
                Return _payment_term_id
            End Get
            Set(ByVal value As Integer)
                _payment_term_id = value
            End Set
        End Property

        Public Property type2() As Integer
            Get
                Return _type2
            End Get
            Set(ByVal value As Integer)
                _type2 = value
            End Set
        End Property

        Public Property type1() As Integer
            Get
                Return _type1
            End Get
            Set(ByVal value As Integer)
                _type1 = value
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

        Public Property person_in_charge1() As String
            Get
                Return _person_in_charge1
            End Get
            Set(ByVal value As String)
                _person_in_charge1 = value
            End Set
        End Property

        Public Property person_in_charge2() As String
            Get
                Return _person_in_charge2
            End Get
            Set(ByVal value As String)
                _person_in_charge2 = value
            End Set
        End Property

        Public Property payment() As String
            Get
                Return _payment_term
            End Get
            Set(ByVal value As String)
                _payment_term = value
            End Set
        End Property

        Public Property payment_condition() As String
            Get
                Return _payment_condition
            End Get
            Set(ByVal value As String)
                _payment_condition = value
            End Set
        End Property

        Public Property zipcode() As String
            Get
                Return _zipcode
            End Get
            Set(ByVal value As String)
                _zipcode = value
            End Set
        End Property

        Public Property address() As String
            Get
                Return _address
            End Get
            Set(ByVal value As String)
                _address = value
            End Set
        End Property

        Public Property tel() As String
            Get
                Return _tel
            End Get
            Set(ByVal value As String)
                _tel = value
            End Set
        End Property

        Public Property fax() As String
            Get
                Return _fax
            End Get
            Set(ByVal value As String)
                _fax = value
            End Set
        End Property

        Public Property email() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
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

        Public Property remarks() As String
            Get
                Return _remarks
            End Get
            Set(ByVal value As String)
                _remarks = value
            End Set
        End Property

        Public Property file() As String
            Get
                Return _file
            End Get
            Set(ByVal value As String)
                _file = value
            End Set
        End Property

        Public Property type2_no() As String
            Get
                Return _type2_no
            End Get
            Set(ByVal value As String)
                _type2_no = value
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

        Public Property name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property short_name() As String
            Get
                Return _short_name
            End Get
            Set(ByVal value As String)
                _short_name = value
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

        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property intSCountry() As Integer
            Get
                Return _intSCountry
            End Get
            Set(ByVal value As Integer)
                _intSCountry = value
            End Set
        End Property

        Public Property strSName() As String
            Get
                Return _strSName
            End Get
            Set(ByVal value As String)
                _strSName = value
            End Set
        End Property

        Public Property intSType2() As Integer
            Get
                Return _intSType2
            End Get
            Set(ByVal value As Integer)
                _intSType2 = value
            End Set
        End Property

        Public Property intSType1() As Integer
            Get
                Return _intSType1
            End Get
            Set(ByVal value As Integer)
                _intSType1 = value
            End Set
        End Property

        Public Property purchase_fg() As Integer
            Get
                Return _purchase_fg
            End Get
            Set(ByVal value As Integer)
                _purchase_fg = value
            End Set
        End Property

        Public Property outsource_fg() As Integer
            Get
                Return _outsource_fg
            End Get
            Set(ByVal value As Integer)
                _outsource_fg = value
            End Set
        End Property

        Public Property other_fg() As Integer
            Get
                Return _other_fg
            End Get
            Set(ByVal value As Integer)
                _other_fg = value
            End Set
        End Property
#End Region

    End Class
End Namespace

