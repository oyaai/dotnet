
#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpStaffService
'	Class Discription	: Implement Staff Service
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

Imports Microsoft.VisualBasic
Imports System.Data
Imports MySql.Data.MySqlClient

Namespace Service
    Public Class ImpStaffService
        Implements IStaffService

        Private objLog As New Common.Logs.Log
        Private objStaffEnt As New Entity.ImpStaffEntity

#Region "Function"
        '/**************************************************************
        '	Function name	: GetStaffList
        '	Discription	    : Get Staff list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetStaffList( _
            ByVal strID As String, _
            ByVal strFirstName As String, _
            ByVal strLastName As String, _
            ByVal strWorkCategoryID As String _
        ) As System.Data.DataTable Implements IStaffService.GetStaffList
            ' set default
            GetStaffList = New DataTable
            Try
                ' variable for keep list from Staff entity
                Dim listStaffEnt As New List(Of Entity.ImpStaffDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetStaffList from entity
                listStaffEnt = objStaffEnt.GetStaffList(strID, strFirstName, strLastName, strWorkCategoryID)

                ' assign column header
                With GetStaffList
                    .Columns.Add("id")
                    .Columns.Add("first_name")
                    .Columns.Add("last_name")
                    .Columns.Add("section")

                    ' assign row from listStaffEny
                    For Each values In listStaffEnt
                        row = .NewRow
                        row("id") = values.id
                        row("first_name") = values.first_name
                        row("last_name") = values.last_name
                        row("section") = values.work_category_id
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetStaffList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteStaff
        '	Discription	    : Delete Staff
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteStaff( _
            ByVal intStaffID As Integer _
        ) As Boolean Implements IStaffService.DeleteStaff
            ' set default return value
            DeleteStaff = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteStaff from Staff Entity
                intEff = objStaffEnt.DeleteStaff(intStaffID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteStaff = True
                Else
                    ' case row less than 1 then return False
                    DeleteStaff = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteStaff(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetStaffByID
        '	Discription	    : Get Staff by ID
        '	Return Value	: Staff dto object
        '	Create User	    : Nisa S.
        '	Create Date	    : 05-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetStaffByID( _
           ByVal intStaffID As Integer _
       ) As Dto.StaffDto Implements IStaffService.GetStaffByID
            ' set default return value
            GetStaffByID = New Dto.StaffDto
            Try
                ' object for return value from Entity
                Dim objStaffEntRet As New Entity.ImpStaffEntity
                ' call function GetStaffByID from Entity
                objStaffEntRet = objStaffEnt.GetStaffByID(intStaffID)

                ' assign value from Entity to Dto
                With GetStaffByID
                    .id = objStaffEntRet.id
                    .first_name = objStaffEntRet.first_name
                    .last_name = objStaffEntRet.last_name
                    .work_category_id = objStaffEntRet.work_category_id
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetStaffByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertStaff
        '	Discription	    : Insert Staff
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertStaff( _
            ByVal objStaffDto As Dto.StaffDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IStaffService.InsertStaff
            ' set default return value
            InsertStaff = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertStaff from Staff Entity
                intEff = objStaffEnt.InsertStaff(SetDtoToEntity(objStaffDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertStaff = True
                Else
                    ' case row less than 1 then return False
                    InsertStaff = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_28_004"
                Else
                    ' other case
                    strMsg = "KTMS_28_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertStaff(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateStaff
        '	Discription	    : Update Staff
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateStaff( _
            ByVal objStaffDto As Dto.StaffDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IStaffService.UpdateStaff
            ' set default return value
            UpdateStaff = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateStaff from Staff Entity
                intEff = objStaffEnt.UpdateStaff(SetDtoToEntity(objStaffDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateStaff = True
                Else
                    ' case row less than 1 then return False
                    UpdateStaff = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_28_007"
                Else
                    ' other case
                    strMsg = "KTMS_28_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateStaff(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Staff Entity object
        '	Create User	    : Nisa S.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objStaffDto As Dto.StaffDto _
        ) As Entity.IStaffEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpStaffEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .id = objStaffDto.id
                    .first_name = objStaffDto.first_name
                    .last_name = objStaffDto.last_name
                    .work_category_id = objStaffDto.work_category_id
                    .delete_fg = objStaffDto.delete_fg
                    .update_by = objStaffDto.updated_by
                    .update_date = objStaffDto.updated_date
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInPO
        '	Discription	    : Check Staff in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO( _
            ByVal intStaffID As Integer _
        ) As Boolean Implements IStaffService.IsUsedInPO
            ' set default return value
            IsUsedInPO = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objStaffEnt.CountUsedInPO(intStaffID)

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
        ' Function name   : CheckDupInsert
        ' Discription     : Check duplication Staff Master
        ' Return Value    : Boolean
        ' Create User     : Nisa S.
        ' Create Date     : 04-07-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupInsert( _
            ByVal strfirst_name As String, _
            ByVal strlast_name As String, _
            ByVal mode As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IStaffService.CheckDupInsert
            ' set default return value
            CheckDupInsert = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objStaffEnt.CheckDupInsert(strfirst_name, strlast_name)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    CheckDupInsert = True
                Else
                    ' case equal 0 then return False
                    CheckDupInsert = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_14_004"
                Else
                    If mode = "1" Then ' insert
                        ' other case
                        strMsg = "KTMS_28_003"
                    Else 'Moidfy
                        ' other case
                        strMsg = "KTMS_28_006"
                    End If
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupInsert(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupUpdate
        ' Discription     : Check duplication Staff Master
        ' Return Value    : Boolean
        ' Create User     : Nisa S.
        ' Create Date     : 04-07-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupUpdate( _
            ByVal intStaffID As String, _
            ByVal strfirst_name As String, _
            ByVal strlast_name As String, _
            ByVal mode As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IStaffService.CheckDupUpdate
            ' set default return value
            CheckDupUpdate = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objStaffEnt.CheckDupUpdate(intStaffID, strfirst_name, strlast_name)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    CheckDupUpdate = True
                Else
                    ' case equal 0 then return False
                    CheckDupUpdate = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_14_004"
                Else
                    If mode = "1" Then ' insert
                        ' other case
                        strMsg = "KTMS_28_003"
                    Else 'Moidfy
                        ' other case
                        strMsg = "KTMS_28_006"
                    End If
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupUpdate(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetWorkCategoryForList
        '	Discription	    : Get WorkCategory for dropdownlist
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 05-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkCategoryForList() As System.Collections.Generic.List(Of Dto.StaffDto) Implements IStaffService.GetWorkCategoryForList
            ' set default list
            GetWorkCategoryForList = New List(Of Dto.StaffDto)
            Try
                ' objWorkCategoryDto for keep value Dto 
                Dim objWorkCategoryDto As Dto.StaffDto
                ' listWorkCategoryEnt for keep value from entity
                Dim listWorkCategoryEnt As New List(Of Entity.IStaffEntity)
                ' objWorkCategoryEnt for call function
                Dim objWorkCategoryEnt As New Entity.ImpStaffEntity

                ' call function GetVendorForList
                listWorkCategoryEnt = objWorkCategoryEnt.GetWorkCategoryForList()

                ' loop listVendorEnt for assign value to Dto
                For Each values In listWorkCategoryEnt
                    ' new object
                    objWorkCategoryDto = New Dto.StaffDto
                    ' assign value to Dto
                    With objWorkCategoryDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetWorkCategoryForList.Add(objWorkCategoryDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkCategoryForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetStaffForList
        '	Discription	    : Get Staff for dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishjaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetStaffForList() As System.Collections.Generic.List(Of Dto.StaffDto) Implements IStaffService.GetStaffForList
            ' set default list
            GetStaffForList = New List(Of Dto.StaffDto)
            Try
                ' objStaffDto for keep value Dto 
                Dim objStaffDto As Dto.StaffDto
                ' listStaffgoryEnt for keep value from entity
                Dim listStaffEnt As New List(Of Entity.IStaffEntity)
                ' objStaffEnt for call function
                Dim objStaffEnt As New Entity.ImpStaffEntity

                ' call function GetStaffForList
                listStaffEnt = objStaffEnt.GetStaffForList()

                ' loop listStaffEnt for assign value to Dto
                For Each values In listStaffEnt
                    ' new object
                    objStaffDto = New Dto.StaffDto
                    ' assign value to Dto
                    With objStaffDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetStaffForList.Add(objStaffDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetStaffForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region

    End Class
End Namespace
