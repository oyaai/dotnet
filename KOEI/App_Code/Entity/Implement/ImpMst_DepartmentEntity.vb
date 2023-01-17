#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_DepartmentEntity
'	Class Discription	: Class of table mst_department
'	Create User 		: Charoon
'	Create Date		    : 30-05-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
Imports System.Exception
Imports MySql.Data.MySqlClient
#End Region

Namespace Entity
    Public Class ImpMst_DepartmentEntity
        Implements IMst_DepartmentEntity

        Private _id As Integer
        Private _name As String
        Private _delete_fg As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _update_by As Integer
        Private _update_date As String
        Private objDepartment As New Dao.ImpMst_DepartmentDao

#Region "Function"
        Public Function GetDepartmentList(ByVal strDepartmentName As String) As System.Collections.Generic.List(Of ImpMst_DepartmentDetailEntity) Implements IMst_DepartmentEntity.GetDepartmentList
            Return objDepartment.GetDepartmentList(strDepartmentName)
        End Function

        Public Function DeleteDepartment(ByVal intDepartmentID As Integer) As Integer Implements IMst_DepartmentEntity.DeleteDepartment
            Return objDepartment.DeleteDepartment(intDepartmentID)
        End Function

        Public Function GetDepartmentByID(ByVal intDepartmentID As Integer) As IMst_DepartmentEntity Implements IMst_DepartmentEntity.GetDepartmentByID
            Return objDepartment.GetDepartmentByID(intDepartmentID)
        End Function

        Public Function InsertDepartment(ByVal objDepartmentEnt As IMst_DepartmentEntity) As Integer Implements IMst_DepartmentEntity.InsertDepartment
            Try
                Return objDepartment.InsertDepartment(objDepartmentEnt)
            Catch exSql As MySqlException
                Throw
            End Try

        End Function

        Public Function UpdateDepartment(ByVal objDepartmentEnt As IMst_DepartmentEntity) As Integer Implements IMst_DepartmentEntity.UpdateDepartment
            Return objDepartment.UpdateDepartment(objDepartmentEnt)
        End Function

        Public Function CheckDupDepartment(ByVal strDepartmentName As String, Optional ByVal intDepartmentID As Integer = 0) As Integer Implements IMst_DepartmentEntity.CheckDupDepartment
            Return objDepartment.CheckDupDepartment(strDepartmentName, intDepartmentID)
        End Function

        Public Function GetDepartmentForDDList() As System.Collections.Generic.List(Of IMst_DepartmentEntity) Implements IMst_DepartmentEntity.GetDepartmentForDDList
            Return objDepartment.GetDepartmentForDDList
        End Function

#End Region

#Region "Property"

        Public Property created_by() As Integer Implements IMst_DepartmentEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IMst_DepartmentEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IMst_DepartmentEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property


        Public Property id() As Integer Implements IMst_DepartmentEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property name() As String Implements IMst_DepartmentEntity.name
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property update_by() As Integer Implements IMst_DepartmentEntity.update_by
            Get
                Return _update_by
            End Get
            Set(ByVal value As Integer)
                _update_by = value
            End Set
        End Property

        Public Property update_date() As String Implements IMst_DepartmentEntity.update_date
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

