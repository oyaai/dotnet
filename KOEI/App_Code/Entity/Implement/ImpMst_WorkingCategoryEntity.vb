#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_WorkingCategoryEntity
'	Class Discription	: Class of table mst_WorkingCategory
'	Create User 		: Pranitda Sroengklang
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
Imports System.Exception
Imports MySql.Data.MySqlClient

#End Region

Namespace Entity
    Public Class ImpMst_WorkingCategoryEntity
        Implements IMst_WorkingCategoryEntity



        Private _id As Integer
        Private _name As String
        Private _delete_fg As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _update_by As Integer
        Private _update_date As String

        Private objWorkingCategory As New Dao.ImpMst_WorkingCategoryDao
#Region "Function"
        Public Function GetWorkingCategoryList( _
            ByVal strWorkingCategoryName As String _
        ) As System.Collections.Generic.List(Of ImpMst_WorkingCategoryDetailEntity) Implements IMst_WorkingCategoryEntity.GetWorkingCategoryList
            Return objWorkingCategory.GetWorkingCategoryList(strWorkingCategoryName)
        End Function
        Public Function DeleteWorkingCategory( _
            ByVal intWorkingCategoryID As Integer _
        ) As Integer Implements IMst_WorkingCategoryEntity.DeleteWorkingCategory
            Return objWorkingCategory.DeleteWorkingCategory(intWorkingCategoryID)
        End Function
        Public Function GetWorkingCategoryByID(ByVal intWorkingCategoryID As Integer) As IMst_WorkingCategoryEntity Implements IMst_WorkingCategoryEntity.GetWorkingCategoryByID
            Return objWorkingCategory.GetWorkingCategoryByID(intWorkingCategoryID)
        End Function

        Public Function InsertWorkingCategory(ByVal objWorkingCategoryEnt As IMst_WorkingCategoryEntity) As Integer Implements IMst_WorkingCategoryEntity.InsertWorkingCategory
            Try
                Return objWorkingCategory.InsertWorkingCategory(objWorkingCategoryEnt)
            Catch exSql As MySqlException
                Throw
            End Try

        End Function

        Public Function UpdateWorkingCategory(ByVal objWorkingCategoryEnt As IMst_WorkingCategoryEntity) As Integer Implements IMst_WorkingCategoryEntity.UpdateWorkingCategory
            Return objWorkingCategory.UpdateWorkingCategory(objWorkingCategoryEnt)
        End Function
        Public Function CountUsedInPO(ByVal intItemID As Integer) As Integer Implements IMst_WorkingCategoryEntity.CountUsedInPO
            Return objWorkingCategory.CountUsedInPO(intItemID)
        End Function
        Public Function CheckDupWorkCategory( _
            ByVal itemName_new As String, _
            ByVal id As String) As Integer Implements IMst_WorkingCategoryEntity.CheckDupWorkCategory
            Return objWorkingCategory.CheckDupWorkCategory(itemName_new, id)
        End Function
#End Region

#Region "Property"
        Public Property created_by() As Integer Implements IMst_WorkingCategoryEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IMst_WorkingCategoryEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IMst_WorkingCategoryEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property id() As Integer Implements IMst_WorkingCategoryEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property name() As String Implements IMst_WorkingCategoryEntity.name
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property update_by() As Integer Implements IMst_WorkingCategoryEntity.update_by
            Get
                Return _update_by
            End Get
            Set(ByVal value As Integer)
                _update_by = value
            End Set
        End Property

        Public Property update_date() As String Implements IMst_WorkingCategoryEntity.update_date
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

