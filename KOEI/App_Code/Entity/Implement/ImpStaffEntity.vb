
#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.

' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpStaffEntity
'	Class Discription	: Class of table mst_staff
'	Create User 		: Nisa S.
'	Create Date		    : 04-07-2013

' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:

'******************************************************************/
#End Region


#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Entity
    Public Class ImpStaffEntity
        Implements IStaffEntity

        Private _id As Integer
        Private _first_name As String
        Private _last_name As String
        Private _work_category_id As String
        Private _delete_fg As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _update_by As Integer
        Private _update_date As String

        Private _name As String

        Private objStaff As New Dao.ImpStaffDao

#Region "Function"

        Public Function GetStaffList(ByVal strID As String, ByVal strFirstName As String, ByVal strLastName As String, ByVal strWorkCategoryID As String) As System.Collections.Generic.List(Of ImpStaffDetailEntity) Implements IStaffEntity.GetStaffList
            Return objStaff.GetStaffList(strID, strFirstName, strLastName, strWorkCategoryID)
        End Function

        Public Function DeleteStaff(ByVal intStaffID As Integer) As Integer Implements IStaffEntity.DeleteStaff
            Return objStaff.DeleteStaff(intStaffID)
        End Function

        Public Function GetStaffByID(ByVal intStaffID As Integer) As IStaffEntity Implements IStaffEntity.GetStaffByID
            Return objStaff.GetStaffByID(intStaffID)
        End Function

        Public Function InsertStaff(ByVal objStaffEnt As IStaffEntity) As Integer Implements IStaffEntity.InsertStaff
            Return objStaff.InsertStaff(objStaffEnt)
        End Function

        Public Function UpdateStaff(ByVal objStaffEnt As IStaffEntity) As Integer Implements IStaffEntity.UpdateStaff
            Return objStaff.UpdateStaff(objStaffEnt)
        End Function

        Public Function CountUsedInPO(ByVal intStaffID As Integer) As Integer Implements IStaffEntity.CountUsedInPO
            Return objStaff.CountUsedInPO(intStaffID)
        End Function

        Public Function CheckDupInsert(ByVal strfirst_name As String, ByVal strlast_name As String) As Integer Implements IStaffEntity.CheckDupInsert
            Return objStaff.CheckDupInsert(strfirst_name, strlast_name)
        End Function

        Public Function CheckDupUpdate(ByVal intStaffID As String, ByVal strfirst_name As String, ByVal strlast_name As String) As Integer Implements IStaffEntity.CheckDupUpdate
            Return objStaff.CheckDupUpdate(intStaffID, strfirst_name, strlast_name)
        End Function

        Public Function GetWorkCategoryForList() As System.Collections.Generic.List(Of IStaffEntity) Implements IStaffEntity.GetWorkCategoryForList
            Return objStaff.GetWorkCategoryForList
        End Function

        Public Function GetStaffForList() As System.Collections.Generic.List(Of IStaffEntity) Implements IStaffEntity.GetStaffForList
            Return objStaff.GetStaffForList
        End Function
#End Region

#Region "Property"
        Public Property first_name() As String Implements IStaffEntity.first_name
            Get
                Return _first_name
            End Get
            Set(ByVal value As String)
                _first_name = value
            End Set
        End Property
        Public Property last_name() As String Implements IStaffEntity.last_name
            Get
                Return _last_name
            End Get
            Set(ByVal value As String)
                _last_name = value
            End Set
        End Property

        Public Property id() As Integer Implements IStaffEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property
        Public Property created_by() As Integer Implements IStaffEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IStaffEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IStaffEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property
        Public Property update_by() As Integer Implements IStaffEntity.update_by
            Get
                Return _update_by
            End Get
            Set(ByVal value As Integer)
                _update_by = value
            End Set
        End Property

        Public Property update_date() As String Implements IStaffEntity.update_date
            Get
                Return _update_date
            End Get
            Set(ByVal value As String)
                _update_date = value
            End Set
        End Property

        Public Property work_category_id() As String Implements IStaffEntity.work_category_id
            Get
                Return _work_category_id
            End Get
            Set(ByVal value As String)
                _work_category_id = value
            End Set
        End Property

        Public Property name() As String Implements IStaffEntity.name
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property
#End Region


    End Class
End Namespace

