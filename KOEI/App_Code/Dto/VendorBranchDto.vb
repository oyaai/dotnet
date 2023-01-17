#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : VendorBranchDto
'	Class Discription	: Interface of table mst_vendor_branch
'	Create User 		: Wasan D.
'	Create Date		    : 02-10-2013
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

Namespace Dto
    Public Class VendorBranchDto

        Private _id As String = Nothing
        Private _name As String = Nothing
        Private _vendorID As String = Nothing
        Private _address As String = Nothing
        Private _zipcode As String = Nothing
        Private _countryID As Integer = Nothing
        Private _countryName As String = Nothing
        Private _fullAddress As String = Nothing
        Private _telNo As String = Nothing
        Private _faxNo As String = Nothing
        Private _email As String = Nothing
        Private _contact As String = Nothing
        Private _remarks As String = Nothing
        Private _delete_fg As Integer = Nothing
        Private _typeOfGoods As String = Nothing

        Private objDao As New Dao.ImpVendorBranchDao

#Region "Property"

        Public Property address() As String
            Get
                Return _address
            End Get
            Set(ByVal value As String)
                _address = value
            End Set
        End Property

        Public Property contact() As String
            Get
                Return _contact
            End Get
            Set(ByVal value As String)
                _contact = value
            End Set
        End Property

        Public Property countryID() As Integer
            Get
                Return _countryID
            End Get
            Set(ByVal value As Integer)
                _countryID = value
            End Set
        End Property

        Public Property countryName() As String
            Get
                Return _countryName
            End Get
            Set(ByVal value As String)
                _countryName = value
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

        Public Property faxNo() As String
            Get
                Return _faxNo
            End Get
            Set(ByVal value As String)
                _faxNo = value
            End Set
        End Property

        Public Property fullAddress() As String
            Get
                Return _fullAddress
            End Get
            Set(ByVal value As String)
                _fullAddress = value
            End Set
        End Property

        Public Property id() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
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

        Public Property remarks() As String
            Get
                Return _remarks
            End Get
            Set(ByVal value As String)
                _remarks = value
            End Set
        End Property

        Public Property telNo() As String
            Get
                Return _telNo
            End Get
            Set(ByVal value As String)
                _telNo = value
            End Set
        End Property

        Public Property vendorID() As String
            Get
                Return _vendorID
            End Get
            Set(ByVal value As String)
                _vendorID = value
            End Set
        End Property

        Public Property delete_fg() As Integer
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
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

        Public Property typeOfGoods() As String
            Get
                Return _typeOfGoods
            End Get
            Set(ByVal value As String)
                _typeOfGoods = value
            End Set
        End Property

#End Region

    End Class
End Namespace