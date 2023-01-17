#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_VendorEntity
'	Class Discription	: Class of table mst_unit
'	Create User 		: Boon
'	Create Date		    : 04-06-2013
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
    Public Class ImpMst_UnitEntity
        Implements IMst_UnitEntity

        Private _id As Integer
        Private _name As String
        Private _delete_fg As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _update_by As Integer
        Private _update_date As String

        Private objUnitDao As New Dao.ImpMst_UnitDao

#Region "Function"
        Public Function GetUnitForList() As System.Collections.Generic.List(Of IMst_UnitEntity) Implements IMst_UnitEntity.GetUnitForList
            Return objUnitDao.DB_GetUnitForList
        End Function

        Public Function GetUnitForSearch(ByVal strName As String) As System.Collections.Generic.List(Of IMst_UnitEntity) Implements IMst_UnitEntity.GetUnitForSearch
            Return objUnitDao.DB_GetUnitForSearch(strName)
        End Function

        Public Function CancelUnit(ByVal intUnitId As Integer) As Boolean Implements IMst_UnitEntity.CancelUnit
            Return objUnitDao.DB_CancelUnit(intUnitId)
        End Function

        Public Function CheckIsDupUnit(ByVal strUnitName As String, Optional ByVal intUnitId As Integer = 0) As Boolean Implements IMst_UnitEntity.CheckIsDupUnit
            Return objUnitDao.DB_CheckIsDupUnit(strUnitName, intUnitId)
        End Function

        Public Function InsertUnit(ByVal objUnit As IMst_UnitEntity) As Boolean Implements IMst_UnitEntity.InsertUnit
            Return objUnitDao.DB_InsertUnit(objUnit)
        End Function

        Public Function UpdateUnit(ByVal objUnit As IMst_UnitEntity) As Boolean Implements IMst_UnitEntity.UpdateUnit
            Return objUnitDao.DB_UpdateUnit(objUnit)
        End Function
#End Region

#Region "Property"
        Public Property created_by() As Integer Implements IMst_UnitEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IMst_UnitEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IMst_UnitEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property id() As Integer Implements IMst_UnitEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property name() As String Implements IMst_UnitEntity.name
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property update_by() As Integer Implements IMst_UnitEntity.update_by
            Get
                Return _update_by
            End Get
            Set(ByVal value As Integer)
                _update_by = value
            End Set
        End Property

        Public Property update_date() As String Implements IMst_UnitEntity.update_date
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

