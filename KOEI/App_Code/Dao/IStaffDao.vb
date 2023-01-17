
#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IStaffDao
'	Class Discription	: Interface of table mst_staff
'	Create User 		: Nisa S.
'	Create Date		    : 04-07-2013
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
    Public Interface IStaffDao

        Function GetStaffList(ByVal strID As String, ByVal strFirstName As String, ByVal strLastName As String, ByVal strWorkCategoryID As String) As List(Of Entity.ImpStaffDetailEntity)
        Function DeleteStaff(ByVal intStaffID As Integer) As Integer
        Function GetStaffByID(ByVal intStaffID As Integer) As Entity.IStaffEntity
        Function InsertStaff(ByVal objStaffEnt As Entity.IStaffEntity) As Integer
        Function UpdateStaff(ByVal objStaffEnt As Entity.IStaffEntity) As Integer
        Function CountUsedInPO(ByVal intStaffID As Integer) As Integer
        Function CheckDupUpdate(ByVal intStaffID As String, ByVal strfirst_name As String, ByVal strlast_name As String) As Integer
        Function CheckDupInsert(ByVal strfirst_name As String, ByVal strlast_name As String) As Integer
        Function GetWorkCategoryForList() As System.Collections.Generic.List(Of Entity.IStaffEntity)
        Function GetStaffForList() As System.Collections.Generic.List(Of Entity.IStaffEntity)
    End Interface
End Namespace
