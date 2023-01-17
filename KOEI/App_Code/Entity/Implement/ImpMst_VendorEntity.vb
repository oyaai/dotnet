#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_VendorEntity
'	Class Discription	: Class of table mst_vendor
'	Create User 		: Boon
'	Create Date		    : 13-05-2013
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
    Public Class ImpMst_VendorEntity
        Implements IMst_VendorEntity

        Private _id As Integer
        Private _type1 As Integer
        Private _type2 As Integer
        Private _type2_no As String
        Private _name As String
        Private _short_name As String
        Private _person_in_charge1 As String
        Private _person_in_charge2 As String
        Private _payment_term_id As Integer
        Private _payment_cond1 As Integer
        Private _payment_cond2 As Integer
        Private _payment_cond3 As Integer
        Private _country_id As Integer
        Private _zipcode As String
        Private _address As String
        Private _tel As String
        Private _fax As String
        Private _remarks As String
        Private _file As String
        Private _email As String
        Private _type_of_goods As String
        Private _delete_fg As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _update_by As Integer
        Private _update_date As String
        Private _purchase_fg As Integer
        Private _outsource_fg As Integer
        Private _other_fg As Integer

        Private objVendorDao As New Dao.ImpMst_VendorDao

#Region "Function"
        Public Function GetFileNameById(ByVal intVendorId As Integer) As String Implements IMst_VendorEntity.GetFileNameById
            Return objVendorDao.DB_GetFileNameById(intVendorId)
        End Function

        Public Function CancelVendor(ByVal intVendor As Integer) As Boolean Implements IMst_VendorEntity.CancelVendor
            Return objVendorDao.DB_CancelVendor(intVendor)
        End Function

        Public Function UpdateVendor(ByVal objVendor As IMst_VendorEntity) As Boolean Implements IMst_VendorEntity.UpdateVendor
            Return objVendorDao.DB_UpdateVendor(objVendor)
        End Function

        Public Function InsertVendor(ByVal objVendor As IMst_VendorEntity, ByRef intVendorId As Integer) As Boolean Implements IMst_VendorEntity.InsertVendor
            Dim objVendorDao As New Dao.ImpMst_VendorDao
            Return objVendorDao.DB_InsertVendor(objVendor, intVendorId)
        End Function

        Public Function CheckIsDupVendor(ByVal intType1 As Integer, ByVal intType2 As Integer, ByVal strName As String, Optional ByVal intId As Integer = 0) As Boolean Implements IMst_VendorEntity.CheckIsDupVendor
            Return objVendorDao.DB_CheckIsDupVendor(intType1, intType2, strName, intId)
        End Function

        Public Function GetVendorForSearch(ByVal intType1 As Integer, ByVal intType2 As Integer, _
                                           ByVal strName As String, ByVal intCountry_id As Integer) _
                                           As System.Collections.Generic.List(Of ImpSubMst_VendorEntity) Implements IMst_VendorEntity.GetVendorForSearch
            Return objVendorDao.DB_GetVendorForSearch(intType1, intType2, strName, intCountry_id)
        End Function

        Public Function GetVendorForReport(ByVal intType1 As Integer, ByVal intType2 As Integer, _
                                           ByVal strName As String, ByVal intCountry_id As Integer) _
                                           As System.Collections.Generic.List(Of ImpVendorBranchEntity) Implements IMst_VendorEntity.GetVendorForReport
            Return objVendorDao.DB_GetVendorForReport(intType1, intType2, strName, intCountry_id)
        End Function

        Public Function GetVendorForDetail(ByVal intID As Integer) As ImpSubMst_VendorEntity Implements IMst_VendorEntity.GetVendorForDetail
            Return objVendorDao.DB_GetVendorForDetail(intID)
        End Function

        Public Function GetVendorForList(Optional ByVal intType1 As String = Nothing) As System.Collections.Generic.List(Of IMst_VendorEntity) Implements IMst_VendorEntity.GetVendorForList
            Return objVendorDao.GetVendorForList(intType1)
        End Function

        ' Add by Suwishaya L. 2013-06-17 (for Job Order)
        Public Function GetVendorListForJobOrder() As System.Collections.Generic.List(Of IMst_VendorEntity) Implements IMst_VendorEntity.GetVendorListForJobOrder
            Return objVendorDao.GetVendorListForJobOrder
        End Function
#End Region

#Region "Properyt"
        Public Property address() As String Implements IMst_VendorEntity.address
            Get
                Return _address
            End Get
            Set(ByVal value As String)
                _address = value
            End Set
        End Property

        Public Property country_id() As Integer Implements IMst_VendorEntity.country_id
            Get
                Return _country_id
            End Get
            Set(ByVal value As Integer)
                _country_id = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IMst_VendorEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IMst_VendorEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IMst_VendorEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property email() As String Implements IMst_VendorEntity.email
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property

        Public Property fax() As String Implements IMst_VendorEntity.fax
            Get
                Return _fax
            End Get
            Set(ByVal value As String)
                _fax = value
            End Set
        End Property

        Public Property file() As String Implements IMst_VendorEntity.file
            Get
                Return _file
            End Get
            Set(ByVal value As String)
                _file = value
            End Set
        End Property

        Public Property id() As Integer Implements IMst_VendorEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property name() As String Implements IMst_VendorEntity.name
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property short_name() As String Implements IMst_VendorEntity.short_name
            Get
                Return _short_name
            End Get
            Set(ByVal value As String)
                _short_name = value
            End Set
        End Property

        Public Property payment_cond1() As Integer Implements IMst_VendorEntity.payment_cond1
            Get
                Return _payment_cond1
            End Get
            Set(ByVal value As Integer)
                _payment_cond1 = value
            End Set
        End Property

        Public Property payment_cond2() As Integer Implements IMst_VendorEntity.payment_cond2
            Get
                Return _payment_cond2
            End Get
            Set(ByVal value As Integer)
                _payment_cond2 = value
            End Set
        End Property

        Public Property payment_cond3() As Integer Implements IMst_VendorEntity.payment_cond3
            Get
                Return _payment_cond3
            End Get
            Set(ByVal value As Integer)
                _payment_cond3 = value
            End Set
        End Property

        Public Property payment_term_id() As Integer Implements IMst_VendorEntity.payment_term_id
            Get
                Return _payment_term_id
            End Get
            Set(ByVal value As Integer)
                _payment_term_id = value
            End Set
        End Property

        Public Property person_in_charge1() As String Implements IMst_VendorEntity.person_in_charge1
            Get
                Return _person_in_charge1
            End Get
            Set(ByVal value As String)
                _person_in_charge1 = value
            End Set
        End Property

        Public Property person_in_charge2() As String Implements IMst_VendorEntity.person_in_charge2
            Get
                Return _person_in_charge2
            End Get
            Set(ByVal value As String)
                _person_in_charge2 = value
            End Set
        End Property

        Public Property remarks() As String Implements IMst_VendorEntity.remarks
            Get
                Return _remarks
            End Get
            Set(ByVal value As String)
                _remarks = value
            End Set
        End Property

        Public Property tel() As String Implements IMst_VendorEntity.tel
            Get
                Return _tel
            End Get
            Set(ByVal value As String)
                _tel = value
            End Set
        End Property

        Public Property type_of_goods() As String Implements IMst_VendorEntity.type_of_goods
            Get
                Return _type_of_goods
            End Get
            Set(ByVal value As String)
                _type_of_goods = value
            End Set
        End Property

        Public Property type1() As Integer Implements IMst_VendorEntity.type1
            Get
                Return _type1
            End Get
            Set(ByVal value As Integer)
                _type1 = value
            End Set
        End Property

        Public Property type2() As Integer Implements IMst_VendorEntity.type2
            Get
                Return _type2
            End Get
            Set(ByVal value As Integer)
                _type2 = value
            End Set
        End Property

        Public Property type2_no() As String Implements IMst_VendorEntity.type2_no
            Get
                Return _type2_no
            End Get
            Set(ByVal value As String)
                _type2_no = value
            End Set
        End Property

        Public Property update_by() As Integer Implements IMst_VendorEntity.update_by
            Get
                Return _update_by
            End Get
            Set(ByVal value As Integer)
                _update_by = value
            End Set
        End Property

        Public Property update_date() As String Implements IMst_VendorEntity.update_date
            Get
                Return _update_date
            End Get
            Set(ByVal value As String)
                _update_date = value
            End Set
        End Property

        Public Property zipcode() As String Implements IMst_VendorEntity.zipcode
            Get
                Return _zipcode
            End Get
            Set(ByVal value As String)
                _zipcode = value
            End Set
        End Property

        Public Property purchase_fg() As Integer Implements IMst_VendorEntity.purchase_fg
            Get
                Return _purchase_fg
            End Get
            Set(ByVal value As Integer)
                _purchase_fg = value
            End Set
        End Property

        Public Property outsource_fg() As Integer Implements IMst_VendorEntity.outsource_fg
            Get
                Return _outsource_fg
            End Get
            Set(ByVal value As Integer)
                _outsource_fg = value
            End Set
        End Property

        Public Property other_fg() As Integer Implements IMst_VendorEntity.other_fg
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

