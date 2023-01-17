#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpDepartmentService
'	Class Discription	: Class of Department
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
Imports System.Data
Imports MySql.Data.MySqlClient

#End Region

Namespace Service
    Public Class ImpDepartmentService
        Implements IDepartmentService

        Private objLog As New Common.Logs.Log
        Private objDepartmentEnt As New Entity.ImpMst_DepartmentEntity

        '/**************************************************************
        '	Function name	: GetDepartmentList
        '	Discription	    : Get department list
        '	Return Value	: List
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDepartmentList(ByVal strDepartmentName As String) As System.Data.DataTable Implements IDepartmentService.GetDepartmentList
            ' set default
            GetDepartmentList = New DataTable
            Try
                ' variable for keep list from department entity
                Dim listDepartmentEnt As New List(Of Entity.ImpMst_DepartmentDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetDepartmentList from entity
                listDepartmentEnt = objDepartmentEnt.GetDepartmentList(strDepartmentName)

                ' assign column header
                With GetDepartmentList
                    .Columns.Add("id")
                    .Columns.Add("name")
                    .Columns.Add("in_used")

                    ' assign row from listDepartmentEny
                    For Each values In listDepartmentEnt
                        row = .NewRow
                        row("id") = values.id
                        row("name") = values.name
                        row("in_used") = values.inuse
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDepartmentList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteDepartment
        '	Discription	    : Delete department
        '	Return Value	: Boolean
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteDepartment( _
            ByVal intDepartmentID As Integer _
        ) As Boolean Implements IDepartmentService.DeleteDepartment
            ' set default return value
            DeleteDepartment = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteDepartment from Department Entity
                intEff = objDepartmentEnt.DeleteDepartment(intDepartmentID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteDepartment = True
                Else
                    ' case row less than 1 then return False
                    DeleteDepartment = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteDepartment(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDepartmentByID
        '	Discription	    : Get department by ID
        '	Return Value	: Department dto object
        '	Create User	    : Komsan L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDepartmentByID( _
           ByVal intDepartmentID As Integer _
       ) As Dto.DepartmentDto Implements IDepartmentService.GetDepartmentByID
            ' set default return value
            GetDepartmentByID = New Dto.DepartmentDto
            Try
                ' object for return value from Entity
                Dim objDepartmentEntRet As New Entity.ImpMst_DepartmentEntity
                ' call function GetDepartmentByID from Entity
                objDepartmentEntRet = objDepartmentEnt.GetDepartmentByID(intDepartmentID)

                ' assign value from Entity to Dto
                With GetDepartmentByID
                    .id = objDepartmentEntRet.id
                    .name = objDepartmentEntRet.name
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDepartmentByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertDepartment
        '	Discription	    : Insert department
        '	Return Value	: Boolean
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertDepartment( _
            ByVal objDepartmentmDto As Dto.DepartmentDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IDepartmentService.InsertDepartment
            ' set default return value
            InsertDepartment = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertDepartment from Department Entity
                intEff = objDepartmentEnt.InsertDepartment(SetDtoToEntity(objDepartmentmDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertDepartment = True
                Else
                    ' case row less than 1 then return False
                    InsertDepartment = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_24_004"
                Else
                    ' other case
                    strMsg = "KTMS_24_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertDepartment(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateDepartment
        '	Discription	    : Update Department
        '	Return Value	: Boolean
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateDepartment( _
            ByVal objDepartmentDto As Dto.DepartmentDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IDepartmentService.UpdateDepartment
            ' set default return value
            UpdateDepartment = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateDepartment from Department Entity
                intEff = objDepartmentEnt.UpdateDepartment(SetDtoToEntity(objDepartmentDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateDepartment = True
                Else
                    ' case row less than 1 then return False
                    UpdateDepartment = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_24_007"
                Else
                    ' other case
                    strMsg = "KTMS_24_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateDepartment(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Department Entity object
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objDepartmentDto As Dto.DepartmentDto _
        ) As Entity.IMst_DepartmentEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpMst_DepartmentEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .id = objDepartmentDto.id
                    .name = objDepartmentDto.name
                    .delete_fg = objDepartmentDto.delete_fg
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
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
        Public Function CheckDupDepartment( _
            ByVal strDepartmentName As String, _
            Optional ByVal intDepartmentID As Integer = 0 _
        ) As Boolean Implements IDepartmentService.CheckDupDepartment
            ' set default return value
            CheckDupDepartment = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objDepartmentEnt.CheckDupDepartment(strDepartmentName, intDepartmentID)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    CheckDupDepartment = True
                Else
                    ' case equal 0 then return False
                    CheckDupDepartment = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupWorkCategory(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: CheckDepartmentForDel
        '	Discription	    : Check data Department for delete
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckDepartmentForDel(ByVal intDepartmentId As Integer) As Boolean Implements IDepartmentService.CheckDepartmentForDel
            Try
                ' variable
                Dim objUserEntity As New Entity.ImpUserEntity

                CheckDepartmentForDel = False
                ' check data will delete
                If intDepartmentId < 1 Then Exit Function

                'Step 1 Check TB user
                If objUserEntity.CheckUserByDepartment(intDepartmentId) = 0 Then Exit Function
                'Step 2 Check successful
                CheckDepartmentForDel = True

            Catch ex As Exception
                ' Write error log
                CheckDepartmentForDel = False
                objLog.ErrorLog("CheckDepartmenttForDel", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: SaveUnit
        '	Discription	    : Save data unit
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SaveDepartment(ByVal objDepartmentDto As Dto.DepartmentDto, ByVal strMode As String) As Boolean Implements IDepartmentService.SaveDepartment
            Try
                ' variable
                Dim objDepartmentEntity As New Entity.ImpMst_DepartmentEntity

                SaveDepartment = False

                ' assign object unit entity
                With objDepartmentDto
                    objDepartmentEntity.id = .id
                    objDepartmentEntity.name = .name
                    objDepartmentEntity.created_by = HttpContext.Current.Session("UserID")
                    objDepartmentEntity.update_by = HttpContext.Current.Session("UserID")
                End With

                ' check mode
                If strMode = "Add" Then
                    ' insert data vendor
                    If objDepartmentEntity.InsertDepartment(objDepartmentEntity) = False Then Exit Function
                ElseIf strMode = "Edit" Then
                    ' update data vendor
                    If objDepartmentEntity.UpdateDepartment(objDepartmentEntity) = False Then Exit Function
                End If
                SaveDepartment = True

            Catch ex As Exception
                SaveDepartment = False
                objLog.ErrorLog("SaveDepartment", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDepartmentForDDList
        '	Discription	    : Get data Department for dropdownlist
        '	Return Value	: list
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDepartmentForDDList() As System.Collections.Generic.List(Of Dto.DepartmentDto) Implements IDepartmentService.GetDepartmentForDDList
            ' set default list
            GetDepartmentForDDList = New List(Of Dto.DepartmentDto)
            Try
                ' objVendorDto for keep value Dto 
                Dim objDepDto As Dto.DepartmentDto
                ' listVendorEnt for keep value from entity
                Dim listDepEnt As New List(Of Entity.IMst_DepartmentEntity)
                ' objVendorEnt for call function
                Dim objDepEnt As New Entity.ImpMst_DepartmentEntity

                ' call function GetVendorForList
                listDepEnt = objDepEnt.GetDepartmentForDDList

                ' loop listVendorEnt for assign value to Dto
                For Each values In listDepEnt
                    ' new object
                    objDepDto = New Dto.DepartmentDto
                    ' assign value to Dto
                    With objDepDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetDepartmentForDDList.Add(objDepDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDepartmentForDDList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
    End Class
End Namespace

