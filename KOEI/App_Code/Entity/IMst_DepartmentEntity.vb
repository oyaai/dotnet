#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_DepartmentEntity
'	Class Discription	: Interface of table mst_department
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
#End Region

Namespace Entity
    Public Interface IMst_DepartmentEntity
        Property id() As Integer
        Property name() As String
        Property delete_fg() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property update_by() As Integer
        Property update_date() As String

        Function GetDepartmentList(ByVal strDepartmentName As String) As List(Of Entity.ImpMst_DepartmentDetailEntity)
        Function DeleteDepartment(ByVal intDepartmentID As Integer) As Integer
        Function GetDepartmentByID(ByVal intDepartmentID As Integer) As Entity.IMst_DepartmentEntity
        Function InsertDepartment(ByVal objDepartmentEnt As Entity.IMst_DepartmentEntity) As Integer
        Function UpdateDepartment(ByVal objDepartmentEnt As Entity.IMst_DepartmentEntity) As Integer
        Function CheckDupDepartment(ByVal strDepartmentName As String, Optional ByVal intDepartmentID As Integer = 0) As Integer

        'add for Dropdownlist By Wasan D. On 08-07-2013
        Function GetDepartmentForDDList() As List(Of Entity.IMst_DepartmentEntity)

    End Interface
End Namespace

