#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMst_DepartmentDao
'	Class Discription	: Interface of table mst_department
'	Create User 		: Charoon
'	Create Date		    : 30-05-2013
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
    Public Interface IMst_DepartmentDao
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

