
#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IStaffEntity
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


Namespace Entity
    Public Interface IStaffEntity
        Property id() As Integer
        Property first_name() As String
        Property last_name() As String
        Property work_category_id() As String
        Property delete_fg() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property update_by() As Integer
        Property update_date() As String

        Property name() As String

        Function GetStaffList(ByVal strID As String, ByVal strFirstName As String, ByVal strLastName As String, ByVal strWorkCategoryID As String) As List(Of Entity.ImpStaffDetailEntity)
        Function DeleteStaff(ByVal intStaffID As Integer) As Integer
        Function GetStaffByID(ByVal intStaffID As Integer) As Entity.IStaffEntity
        Function InsertStaff(ByVal objStaffEnt As Entity.IStaffEntity) As Integer
        Function UpdateStaff(ByVal objStaffEnt As Entity.IStaffEntity) As Integer
        Function CountUsedInPO(ByVal intStaffID As Integer) As Integer
        Function CheckDupInsert(ByVal strfirst_name As String, ByVal strlast_name As String) As Integer
        Function CheckDupUpdate(ByVal intStaffID As String, ByVal strfirst_name As String, ByVal strlast_name As String) As Integer

        Function GetWorkCategoryForList() As List(Of Entity.IStaffEntity)
        Function GetStaffForList() As List(Of Entity.IStaffEntity)
    End Interface
End Namespace

