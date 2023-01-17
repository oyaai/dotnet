#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpWorkingCategoryService
'	Class Discription	: Implement WorkingCategory Service
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 04-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports Microsoft.VisualBasic
Imports System.Data
Imports MySql.Data.MySqlClient

Namespace Service
    Public Class ImpWorkingCategoryService
        Implements IWorkingCategoryService

        Private objLog As New Common.Logs.Log
        Private objWorkingCategoryEnt As New Entity.ImpMst_WorkingCategoryEntity

#Region "Function"
        '/**************************************************************
        '	Function name	: GetWorkingCategoryList
        '	Discription	    : Get WorkingCategory list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkingCategoryList( _
            ByVal strWorkingCategoryName As String _
        ) As System.Data.DataTable Implements IWorkingCategoryService.GetWorkingCategoryList
            ' set default
            GetWorkingCategoryList = New DataTable
            Try
                ' variable for keep list from WorkingCategory entity
                Dim listWorkingCategoryEnt As New List(Of Entity.ImpMst_WorkingCategoryDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetWorkingCategoryList from entity
                listWorkingCategoryEnt = objWorkingCategoryEnt.GetWorkingCategoryList(strWorkingCategoryName)

                ' assign column header
                With GetWorkingCategoryList
                    .Columns.Add("item_id")
                    .Columns.Add("item_name")
                    .Columns.Add("in_used")

                    ' assign row from listWorkingCategoryEny
                    For Each values In listWorkingCategoryEnt
                        row = .NewRow
                        row("item_id") = values.id
                        row("item_name") = values.name
                        row("in_used") = values.inuse
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingCategoryList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteWorkingCategory
        '	Discription	    : Delete WorkingCategory
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteWorkingCategory( _
            ByVal intWorkingCategoryID As Integer _
        ) As Boolean Implements IWorkingCategoryService.DeleteWorkingCategory
            ' set default return value
            DeleteWorkingCategory = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteWorkingCategory from WorkingCategory Entity
                intEff = objWorkingCategoryEnt.DeleteWorkingCategory(intWorkingCategoryID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteWorkingCategory = True
                Else
                    ' case row less than 1 then return False
                    DeleteWorkingCategory = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteWorkingCategory(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetWorkingCategoryByID
        '	Discription	    : Get Working Category by ID
        '	Return Value	: WorkingCategory dto object
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkingCategoryByID( _
           ByVal intWorkingCategoryID As Integer _
       ) As Dto.WorkingCategoryDto Implements IWorkingCategoryService.GetWorkingCategoryByID
            ' set default return value
            GetWorkingCategoryByID = New Dto.WorkingCategoryDto
            Try
                ' object for return value from Entity
                Dim objWorkingCategoryEntRet As New Entity.ImpMst_WorkingCategoryEntity
                ' call function GetWorkingCategoryByID from Entity
                objWorkingCategoryEntRet = objWorkingCategoryEnt.GetWorkingCategoryByID(intWorkingCategoryID)

                ' assign value from Entity to Dto
                With GetWorkingCategoryByID
                    .id = objWorkingCategoryEntRet.id
                    .name = objWorkingCategoryEntRet.name
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingCategoryByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertWorkingCategory
        '	Discription	    : Insert WorkingCategory
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertWorkingCategory( _
            ByVal objWorkingCategoryDto As Dto.WorkingCategoryDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IWorkingCategoryService.InsertWorkingCategory
            ' set default return value
            InsertWorkingCategory = False

            Try

                Dim intChkDup As Integer = 0
                Dim intEff As Integer = 0 ' intEff keep row effect

                ' call function InsertWorkingCategory from WorkingCategory Entity
                intEff = objWorkingCategoryEnt.InsertWorkingCategory(SetDtoToEntity(objWorkingCategoryDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertWorkingCategory = True
                Else
                    ' case row less than 1 then return False
                    InsertWorkingCategory = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_20_008"
                Else
                    ' other case
                    strMsg = "KTMS_20_009"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertWorkingCategory(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateWorkingCategory
        '	Discription	    : Update WorkingCategory
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateWorkingCategory( _
            ByVal objWorkingCategoryDto As Dto.WorkingCategoryDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IWorkingCategoryService.UpdateWorkingCategory
            ' set default return value
            UpdateWorkingCategory = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateWorkingCategory from WorkingCategory Entity
                intEff = objWorkingCategoryEnt.UpdateWorkingCategory(SetDtoToEntity(objWorkingCategoryDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateWorkingCategory = True
                Else
                    ' case row less than 1 then return False
                    UpdateWorkingCategory = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_20_007"
                Else
                    ' other case
                    strMsg = "KTMS_20_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateWorkingCategory(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: WorkingCategory Entity object
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objWorkingCategoryDto As Dto.WorkingCategoryDto _
        ) As Entity.IMst_WorkingCategoryEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpMst_WorkingCategoryEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .id = objWorkingCategoryDto.id
                    .name = objWorkingCategoryDto.name
                    .delete_fg = objWorkingCategoryDto.delete_fg
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: IsUsedInPO
        '	Discription	    : Check item in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO( _
            ByVal intItemID As Integer _
        ) As Boolean Implements IWorkingCategoryService.IsUsedInPO
            ' set default return value
            IsUsedInPO = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objWorkingCategoryEnt.CountUsedInPO(intItemID)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUsedInPO = True
                Else
                    ' case equal 0 then return False
                    IsUsedInPO = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInPO(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: CheckDupWorkCategory
        '	Discription	    : Check duplication Working Category Master
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckDupWorkCategory( _
            ByVal itemName_new As String, _
            ByVal id As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IWorkingCategoryService.CheckDupWorkCategory
            ' set default return value
            CheckDupWorkCategory = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objWorkingCategoryEnt.CheckDupWorkCategory(itemName_new, id)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    CheckDupWorkCategory = True
                Else
                    ' case equal 0 then return False
                    CheckDupWorkCategory = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_20_007"
                Else
                    ' other case
                    strMsg = "KTMS_20_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupWorkCategory(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region

    End Class
End Namespace

