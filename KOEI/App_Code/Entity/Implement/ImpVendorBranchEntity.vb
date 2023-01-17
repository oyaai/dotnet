#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IVendorBranchEntity
'	Class Discription	: Interface of table mst_vendor_branch
'	Create User 		: Wasan D.
'	Create Date		    : 26-09-2013
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
    Public Class ImpVendorBranchEntity
        Implements IVendorBranchEntity

        Private _id As Integer = Nothing
        Private _name As String = Nothing
        Private _vendorID As Integer = Nothing
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


#Region "Function"

        Public Function GetBranchWithVendorID(ByVal intVendorID As Integer) As System.Collections.Generic.List(Of ImpVendorBranchEntity) Implements IVendorBranchEntity.GetBranchWithVendorID
            Return objDao.GetBranchWithVendorID(intVendorID)
        End Function

        Public Function CheckBranchIsInUse(ByVal intBranchID As Integer) As Integer Implements IVendorBranchEntity.CheckBranchIsInUse
            Return objDao.CheckBranchIsInUse(intBranchID)
        End Function

        Public Function SaveVendorBranch(ByVal listBranchEnt As System.Collections.Generic.List(Of ImpVendorBranchEntity)) As Integer Implements IVendorBranchEntity.SaveVendorBranch
            Return objDao.SaveVendorBranch(listBranchEnt)
        End Function

        Public Function GetVendorBranchForDDLList(ByVal intVendorID As Integer) As System.Collections.Generic.List(Of ImpVendorBranchEntity) Implements IVendorBranchEntity.GetVendorBranchForDDLList
            Return objDao.GetVendorBranchForDDLList(intVendorID)
        End Function

#End Region

#Region "Property"

        Public Property address() As String Implements IVendorBranchEntity.address
            Get
                Return _address
            End Get
            Set(ByVal value As String)
                _address = value
            End Set
        End Property

        Public Property contact() As String Implements IVendorBranchEntity.contact
            Get
                Return _contact
            End Get
            Set(ByVal value As String)
                _contact = value
            End Set
        End Property

        Public Property countryID() As Integer Implements IVendorBranchEntity.countryID
            Get
                Return _countryID
            End Get
            Set(ByVal value As Integer)
                _countryID = value
            End Set
        End Property

        Public Property countryName() As String Implements IVendorBranchEntity.countryName
            Get
                Return _countryName
            End Get
            Set(ByVal value As String)
                _countryName = value
            End Set
        End Property

        Public Property email() As String Implements IVendorBranchEntity.email
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property

        Public Property faxNo() As String Implements IVendorBranchEntity.faxNo
            Get
                Return _faxNo
            End Get
            Set(ByVal value As String)
                _faxNo = value
            End Set
        End Property

        Public Property fullAddress() As String Implements IVendorBranchEntity.fullAddress
            Get
                Return _fullAddress
            End Get
            Set(ByVal value As String)
                _fullAddress = value
            End Set
        End Property

        Public Property id() As Integer Implements IVendorBranchEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property name() As String Implements IVendorBranchEntity.name
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property remarks() As String Implements IVendorBranchEntity.remarks
            Get
                Return _remarks
            End Get
            Set(ByVal value As String)
                _remarks = value
            End Set
        End Property

        Public Property telNo() As String Implements IVendorBranchEntity.telNo
            Get
                Return _telNo
            End Get
            Set(ByVal value As String)
                _telNo = value
            End Set
        End Property

        Public Property vendorID() As Integer Implements IVendorBranchEntity.vendorID
            Get
                Return _vendorID
            End Get
            Set(ByVal value As Integer)
                _vendorID = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IVendorBranchEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property zipcode() As String Implements IVendorBranchEntity.zipcode
            Get
                Return _zipcode
            End Get
            Set(ByVal value As String)
                _zipcode = value
            End Set
        End Property

        Public Property typeOfGoods() As String Implements IVendorBranchEntity.typeOfGoods
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
