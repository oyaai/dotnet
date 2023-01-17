#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpIECategoryDao
'	Class Discription	: Class of table mst_ie_category
'	Create User 		: Nisa S.
'	Create Date		    : 24-05-2013
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

Imports Entity
Imports Exceptions
Imports Interfaces
Imports Microsoft.VisualBasic
Imports Common.DBConnection
Imports System.Data
Imports Extensions
Imports MySql.Data.MySqlClient

#End Region


Namespace Dao

    Public Class ImpIECategoryDao
        Implements IIECategoryDao

#Region "Constructors"

        Friend Sub New()
        End Sub

#End Region

#Region "Properties"

        Protected Overridable ReadOnly Property ClassName() As String Implements IIECategoryDao.ClassName
            Get
                Return Convert.ToString(GetType(ImpIECategoryDao))
            End Get
        End Property

#End Region

#Region "Functions"

        '/**************************************************************
        '	Function name	: GetAll
        '	Discription	    : 
        '	Return Value	: List of IIECategoryEntity
        '	Create User	    : Prasert S.
        '	Create Date	    : 
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Overridable Function GetAll() As List(Of IIECategoryEntity) Implements IIECategoryDao.GetAll
            Dim connection As MySQLAccess
            Dim dtCategory As DataTable
            Dim entities As New List(Of IIECategoryEntity)
            Dim entity As IIECategoryEntity

            Const commandText As String = "SELECT `id`, `name`, `name_jp`, `delete_fg`, `created_by`, `created_date`, `updated_by`, `updated_date` " & _
                                          "FROM `mst_ie_category` WHERE (delete_fg <> 1) " & _
                                          "ORDER BY `name`;"
            Try
                connection = New Common.DBConnection.MySQLAccess()

                dtCategory = connection.ExecuteDataTable(commandText)

                For Each dataRow As DataRow In dtCategory.Rows
                    entity = New ImpIECategoryEntity()
                    entity.ID = dataRow.GetByte("id")
                    entity.Name = dataRow.GetString("name")
                    entity.NameJp = dataRow.GetString("name_jp")
                    entity.DeleteFg = dataRow.GetByte("delete_fg")
                    entity.CreatedBy = dataRow.GetNullableInt32("created_by")
                    entity.CreatedDate = dataRow.GetString("created_date")
                    entity.UpdatedBy = dataRow.GetNullableInt32("updated_by")
                    entity.UpdatedDate = dataRow.GetString("updated_date")

                    entities.Add(entity)
                Next

                Return entities
            Catch ex As MySqlException
                'Log

                ' Throw
                Throw New ApplicationException(ex)
            Catch ex As Exception
                'Log

                ' Throw
                Throw New ApplicationException(ex)
            End Try
        End Function

#End Region

    End Class
End Namespace