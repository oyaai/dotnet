#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpJobTypeService
'	Class Discription	: Class of Job Type
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
Imports Dao
Imports Entity
Imports Dto
Imports Exceptions
Imports System.Data
Imports MySql.Data.MySqlClient

#End Region

Namespace Service
    Public Class ImpJobTypeService
        Implements IJobTypeService

        Private objLog As New Common.Logs.Log
        Private objJobTypeEntity As New Entity.ImpMst_JobTypeEntity


#Region "Function"

        '/**************************************************************
        '	Function name	: GetJobTypeForList
        '	Discription	    : Get job type for dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobTypeForList() As System.Collections.Generic.List(Of Dto.JobTypeDto) Implements IJobTypeService.GetJobTypeForList

            ' set default list
            GetJobTypeForList = New List(Of Dto.JobTypeDto)
            Try
                ' objJobTypeDto for keep value Dto 
                Dim objJobTypeDto As Dto.JobTypeDto
                ' listJobTypeEnt for keep value from entity
                Dim listJobTypeEnt As New List(Of Entity.IMst_JobTypeEntity)
                ' objVendorEnt for call function
                Dim objJobTypeEnt As New Entity.ImpMst_JobTypeEntity

                ' call function GetJobTypeForList
                listJobTypeEnt = objJobTypeEnt.GetJobTypeForList()

                ' loop listJobTypeEnt for assign value to Dto
                For Each values In listJobTypeEnt
                    ' new object
                    objJobTypeDto = New Dto.JobTypeDto
                    ' assign value to Dto
                    With objJobTypeDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetJobTypeForList.Add(objJobTypeDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobTypeForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobTypeList
        '	Discription	    : Get JobType list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobTypeList(ByVal strJobType As String) _
        As System.Data.DataTable Implements Service.IJobTypeService.GetJobTypeList
            ' set default
            GetJobTypeList = New DataTable
            Try
                ' variable for keep list from JobType entity
                Dim listJobTypeEntity As New List(Of Entity.IMst_JobTypeEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetJobTypeList from entity
                listJobTypeEntity = objJobTypeEntity.GetJobTypeList(strJobType)

                ' assign column header
                With GetJobTypeList
                    .Columns.Add("id")
                    .Columns.Add("job_type")


                    ' assign row from listJobTypeEntity
                    For Each values In listJobTypeEntity
                        row = .NewRow
                        row("id") = values.id
                        row("job_type") = values.name

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobTypeList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobTypeByID
        '	Discription	    : Get JobType by ID
        '	Return Value	: Job Type dto object
        '	Create User	    : Nisa S.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobTypeByID( _
           ByVal intJobTypeID As Integer _
       ) As Dto.JobTypeDto Implements IJobTypeService.GetJobTypeByID
            ' set default return value
            GetJobTypeByID = New Dto.JobTypeDto
            Try
                ' object for return value from Entity
                Dim objJobTypeEntRet As New Entity.ImpMst_JobTypeEntity
                ' call function GetJobTypeByID from Entity
                objJobTypeEntRet = objJobTypeEntity.GetJobTypeByID(intJobTypeID)

                ' assign value from Entity to Dto
                With GetJobTypeByID
                    .id = objJobTypeEntRet.id
                    .name = objJobTypeEntRet.name

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobTypeByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInPO
        '	Discription	    : Check JobType in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO( _
            ByVal intJobTypeID As Integer _
        ) As Boolean Implements Service.IJobTypeService.IsUsedInPO
            ' set default return value
            IsUsedInPO = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objJobTypeEntity.CountUsedInPO(intJobTypeID)


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
        '	Function name	: DeleteJobType
        '	Discription	    : Delete JobType
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobType( _
            ByVal intJobTypeID As Integer _
        ) As Boolean Implements IJobTypeService.DeleteJobType
            ' set default return value
            DeleteJobType = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteJobType from JobType Entity
                intEff = objJobTypeEntity.DeleteJobType(intJobTypeID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteJobType = True
                Else
                    ' case row less than 1 then return False
                    DeleteJobType = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobType(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertJobType
        '	Discription	    : Insert JobType
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertJobType( _
            ByVal objJobTypeDto As Dto.JobTypeDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IJobTypeService.InsertJobType
            ' set default return value
            InsertJobType = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertJobType from JobType Entity
                intEff = objJobTypeEntity.InsertJobType(SetDtoToEntity(objJobTypeDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertJobType = True
                Else
                    ' case row less than 1 then return False
                    InsertJobType = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_16_004"
                Else
                    ' other case
                    strMsg = "KTMS_16_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertJobType(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateJobType
        '	Discription	    : Update JobType
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateJobType( _
            ByVal objJobTypeDto As Dto.JobTypeDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IJobTypeService.UpdateJobType
            ' set default return value
            UpdateJobType = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateJobType from JobType Entity
                intEff = objJobTypeEntity.UpdateJobType(SetDtoToEntity(objJobTypeDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateJobType = True
                Else
                    ' case row less than 1 then return False
                    UpdateJobType = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_16_007"
                Else
                    ' other case
                    strMsg = "KTMS_16_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateJobType(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: JobType Entity object
        '	Create User	    : Nisa S.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objJobTypeDto As Dto.JobTypeDto _
        ) As Entity.IMst_JobTypeEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpMst_JobTypeEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .id = objJobTypeDto.id
                    .name = objJobTypeDto.name
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupJobType
        ' Discription     : Check duplication JobType Master
        ' Return Value    : Boolean
        ' Create User     : Nisa S.
        ' Create Date     : 02-07-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupJobType( _
            ByVal intJobTypeID As String, _
            ByVal intJobType As String, _
            ByVal mode As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IJobTypeService.CheckDupJobType
            ' set default return value
            CheckDupJobType = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objJobTypeEntity.CheckDupJobType(intJobTypeID, intJobType)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    CheckDupJobType = True
                Else
                    ' case equal 0 then return False
                    CheckDupJobType = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_16_004"
                Else
                    If mode = "1" Then ' insert
                        ' other case
                        strMsg = "KTMS_16_003"
                    Else 'Moidfy
                        ' other case
                        strMsg = "KTMS_16_006"
                    End If
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupJobType(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region



    End Class
End Namespace
