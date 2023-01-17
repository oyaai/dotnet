#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMst_JobTypeDao
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

Namespace Dao
    Public Interface IMst_JobTypeDao
        Function GetJobTypeForList() As List(Of Entity.IMst_JobTypeEntity)

        Function GetJobTypeList(ByVal strJobType As String) As List(Of Entity.IMst_JobTypeEntity)
        Function CountUsedInPO(ByVal intJobTypeID As Integer) As Integer
        Function DeleteJobType(ByVal intJobTypeID As Integer) As Integer
        Function GetJobTypeByID(ByVal intJobTypeID As Integer) As Entity.IMst_JobTypeEntity
        Function InsertJobType(ByVal objJobTypeEntity As Entity.IMst_JobTypeEntity) As Integer
        Function UpdateJobType(ByVal objJobTypeEntity As Entity.IMst_JobTypeEntity) As Integer
        Function CheckDupJobType(ByVal intJobTypeID As String, ByVal intJobType As String) As Integer
    End Interface
End Namespace
