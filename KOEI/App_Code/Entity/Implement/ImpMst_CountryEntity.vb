#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_CountryEntity
'	Class Discription	: Class of table mst_country
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
    Public Class ImpMst_CountryEntity
        Implements IMst_CountryEntity

        Private _id As Integer
        Private _name As String
        Private _delete_fg As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _update_by As Integer
        Private _update_date As String

        Private objCountry As New Dao.ImpMst_CountryDao

#Region "Function"
        Public Function GetListCountryName() As System.Collections.Generic.List(Of IMst_CountryEntity) Implements IMst_CountryEntity.GetListCountryName
            Dim objCountryDao As New Dao.ImpMst_CountryDao
            Return objCountryDao.DB_GetListCountryName
        End Function

        Public Function GetCountryList(ByVal strCountryName As String) As System.Collections.Generic.List(Of ImpMst_CountryDetailEntity) Implements IMst_CountryEntity.GetCountryList
            Return objCountry.GetCountryList(strCountryName)
        End Function

        Public Function DeleteCountry(ByVal intCountryID As Integer) As Integer Implements IMst_CountryEntity.DeleteCountry
            Return objCountry.DeleteCountry(intCountryID)
        End Function

        Public Function GetCountryByID(ByVal intCountryID As Integer) As IMst_CountryEntity Implements IMst_CountryEntity.GetCountryByID
            Return objCountry.GetCountryByID(intCountryID)
        End Function

        Public Function InsertCountry(ByVal objCountryEnt As IMst_CountryEntity) As Integer Implements IMst_CountryEntity.InsertCountry
            Return objCountry.InsertCountry(objCountryEnt)
        End Function

        Public Function UpdateCountry(ByVal objCountryEnt As IMst_CountryEntity) As Integer Implements IMst_CountryEntity.UpdateCountry
            Return objCountry.UpdateCountry(objCountryEnt)
        End Function

        Public Function CheckDupCountry( _
            ByVal strCountryName As String, _
            ByVal id As String _
        ) As Integer Implements IMst_CountryEntity.CheckDupCountry
            Return objCountry.CheckDupCountry(strCountryName, id)
        End Function

        Public Function CountUsedInVendor(ByVal intCountryID As Integer) As Integer Implements IMst_CountryEntity.CountUsedInVendor
            Return objCountry.CountUsedInVendor(intCountryID)
        End Function

#End Region

#Region "Property"
        Public Property created_by() As Integer Implements IMst_CountryEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IMst_CountryEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IMst_CountryEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property id() As Integer Implements IMst_CountryEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property name() As String Implements IMst_CountryEntity.name
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property update_by() As Integer Implements IMst_CountryEntity.update_by
            Get
                Return _update_by
            End Get
            Set(ByVal value As Integer)
                _update_by = value
            End Set
        End Property

        Public Property update_date() As String Implements IMst_CountryEntity.update_date
            Get
                Return _update_date
            End Get
            Set(ByVal value As String)
                _update_date = value
            End Set
        End Property
#End Region

    End Class
End Namespace

