#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpIECategoryService
'	Class Discription	: Implement IE Service
'	Create User 		: Nisa S.
'	Create Date		    : 24-06-2013
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

Imports Dto
Imports Entity
Imports Dao
Imports Exceptions
Imports Interfaces

#End Region


Namespace Service

    Public Class ImpIECategoryService
        Implements IIECategoryService

#Region "Function"
        '/**************************************************************
        '	Function name	: GetAll
        '	Discription		: 
        '	Return Value	: IList IECategoryDto
        '	Create User		: Prasert S.
        '	Create Date		: 
        '	Update User		:
        '	Update Date		:
        '**************************************************************/
        Public Overridable Function GetAll() As List(Of IECategoryDto) Implements IIECategoryService.GetAll
            Dim dto As IECategoryDto
            Dim dtos As New List(Of IECategoryDto)
            Dim entities As List(Of IIECategoryEntity)
            Dim entity As IIECategoryEntity

            Try
                entity = New ImpIECategoryEntity()
                entities = entity.GetAll()

                For index As Integer = 0 To entities.Count - 1

                    dto = New IECategoryDto
                    dto.CreatedBy = entities(index).CreatedBy
                    dto.CreatedDate = entities(index).CreatedDate
                    dto.DeleteFlag = entities(index).DeleteFg
                    dto.ID = entities(index).ID
                    dto.Name = entities(index).Name
                    dto.NameJp = entities(index).NameJp
                    dto.UpdatedBy = entities(index).UpdatedBy
                    dto.UpdatedDate = entities(index).UpdatedDate

                    dtos.Add(Dto)
                Next

                Return dtos
            Catch ex As ApplicationException
                Throw New ApplicationException(ex)
            Catch ex As Exception
                'Logging

                'Throw
                Throw New ApplicationException(ex)
            End Try

        End Function
#End Region

    End Class
End Namespace