#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_JobTypeEntity
'	Class Discription	: Interface of table mst_job_type
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
    Public Interface IMst_JobTypeEntity

#Region "Property"
        Property id() As Integer
        Property name() As String
        Property delete_fg() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property updated_by() As Integer
        Property updated_date() As String
#End Region

#Region "Function"
        Function GetJobTypeForList() As List(Of Entity.IMst_JobTypeEntity)

        Function GetJobTypeList(ByVal strJobType As String) As List(Of Entity.IMst_JobTypeEntity)
        Function CountUsedInPO(ByVal intJobTypeID As Integer) As Integer
        Function DeleteJobType(ByVal intJobTypeID As Integer) As Integer
        Function GetJobTypeByID(ByVal intJobTypeID As Integer) As Entity.IMst_JobTypeEntity
        Function InsertJobType(ByVal objJobTypeEntity As Entity.IMst_JobTypeEntity) As Integer
        Function UpdateJobType(ByVal objJobTypeEntity As Entity.IMst_JobTypeEntity) As Integer
        Function CheckDupJobType(ByVal intJobTypeID As String, ByVal intJobType As String) As Integer
#End Region

    End Interface
End Namespace
