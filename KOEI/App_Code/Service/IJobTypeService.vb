#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IJobTypeService
'	Class Discription	: Interface of Job Type
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
Imports System.Data

#End Region

Namespace Service
    Public Interface IJobTypeService
        Function GetJobTypeForList() As List(Of Dto.JobTypeDto)

        Function GetJobTypeList(ByVal strJobType As String) As DataTable
        Function IsUsedInPO(ByVal intJobTypeID As Integer) As Boolean
        Function DeleteJobType(ByVal intJobTypeID As Integer) As Boolean
        Function InsertJobType(ByVal objJobTypeDto As Dto.JobTypeDto, Optional ByRef strMsg As String = "") As Boolean
        Function GetJobTypeByID(ByVal intJobTypeID As Integer) As Dto.JobTypeDto
        Function CheckDupJobType(ByVal intJobTypeID As String, ByVal intJobType As String, ByVal mode As String, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateJobType(ByVal objJobTypeDto As Dto.JobTypeDto, Optional ByRef strMsg As String = "") As Boolean

    End Interface
End Namespace

