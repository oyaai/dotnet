#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_JobTypeEntity
'	Class Discription	: Class of table mst_job_type
'	Create User 		: Suwishaya L.
'	Create Date		    : 11-06-2013
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
    Public Class ImpMst_JobTypeEntity
        Implements IMst_JobTypeEntity

        Private _id As Integer
        Private _name As String
        Private _delete_fg As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _updated_by As Integer
        Private _updated_date As String

        Private objJobTypeDao As New Dao.ImpMst_JobTypeDao

#Region "Function"
        Public Function GetJobTypeForList() As System.Collections.Generic.List(Of IMst_JobTypeEntity) Implements IMst_JobTypeEntity.GetJobTypeForList
            Return objJobTypeDao.GetJobTypeForList
        End Function

        Public Function GetJobTypeList(ByVal strJobType As String) As System.Collections.Generic.List(Of IMst_JobTypeEntity) Implements IMst_JobTypeEntity.GetJobTypeList
            Return objJobTypeDao.GetJobTypeList(strJobType)
        End Function

        Public Function CountUsedInPO(ByVal intJobTypeID As Integer) As Integer Implements IMst_JobTypeEntity.CountUsedInPO
            Return objJobTypeDao.CountUsedInPO(intJobTypeID)
        End Function

        Public Function DeleteJobType(ByVal intJobTypeID As Integer) As Integer Implements IMst_JobTypeEntity.DeleteJobType
            Return objJobTypeDao.DeleteJobType(intJobTypeID)
        End Function

        Public Function GetJobTypeByID(ByVal intJobTypeID As Integer) As IMst_JobTypeEntity Implements IMst_JobTypeEntity.GetJobTypeByID
            Return objJobTypeDao.GetJobTypeByID(intJobTypeID)
        End Function

        Public Function InsertJobType(ByVal objJobTypeEntity As IMst_JobTypeEntity) As Integer Implements IMst_JobTypeEntity.InsertJobType
            Return objJobTypeDao.InsertJobType(objJobTypeEntity)
        End Function

        Public Function UpdateJobType(ByVal objJobTypeEntity As IMst_JobTypeEntity) As Integer Implements IMst_JobTypeEntity.UpdateJobType
            Return objJobTypeDao.UpdateJobType(objJobTypeEntity)
        End Function

        Public Function CheckDupJobType(ByVal intJobTypeID As String, ByVal intJobType As String) As Integer Implements IMst_JobTypeEntity.CheckDupJobType
            Return objJobTypeDao.CheckDupJobType(intJobTypeID, intJobType)
        End Function
#End Region

#Region "Properyt"
        Public Property created_by() As Integer Implements IMst_JobTypeEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IMst_JobTypeEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IMst_JobTypeEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property id() As Integer Implements IMst_JobTypeEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property name() As String Implements IMst_JobTypeEntity.name
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IMst_JobTypeEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As String Implements IMst_JobTypeEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As String)
                _updated_date = value
            End Set
        End Property

#End Region

    End Class
End Namespace
