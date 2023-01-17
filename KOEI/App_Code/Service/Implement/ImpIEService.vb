#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpIEService
'	Class Discription	: Implement Account title Service
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

Imports Dao
Imports Interfaces
Imports Microsoft.VisualBasic
Imports Dto
Imports Entity
Imports Exceptions
Imports System.Data
Imports MySql.Data.MySqlClient

#End Region


Namespace Service

    Public Class ImpIEService
        Implements IIEService

        Private objIEEnt As New Entity.ImpIEEntity
        Private objLog As New Common.Logs.Log

#Region "Function"
        '/**************************************************************
        '	Function name	: GetIEList
        '	Discription	    : Get IE list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetIEList( _
        ByVal strID As String, _
        ByVal strIECategory As String, _
        ByVal strIECode As String, _
        ByVal strIEName As String) As System.Data.DataTable Implements IIEService.GetIEList
            ' set default
            GetIEList = New DataTable
            Try
                ' variable for keep list from IE entity
                Dim listIEEnt As New List(Of Entity.ImpMst_IEDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetIEList from entity
                listIEEnt = objIEEnt.GetIEList(strID, strIECategory, strIECode, strIEName)

                ' assign column header
                With GetIEList
                    .Columns.Add("id")
                    .Columns.Add("category_name")
                    .Columns.Add("code")
                    .Columns.Add("name")
                    .Columns.Add("category_id")
                    ' assign row from listIEEnt
                    For Each values In listIEEnt
                        row = .NewRow
                        row("id") = values.ID
                        row("category_name") = values.category_name
                        row("code") = values.Code
                        row("name") = values.Name
                        row("category_id") = values.category_id
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("listIEEnt(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInPO
        '	Discription	    : Check IE in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO( _
            ByVal intIEID As Integer _
        ) As Boolean Implements IIEService.IsUsedInPO
            ' set default return value
            IsUsedInPO = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objIEEnt.CountUsedInPO(intIEID)
                intCount = intCount + objIEEnt.CountUsedInPO2(intIEID)

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
        '	Function name	: IsUsedInPO2
        '	Discription	    : Check IE in used account
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 15-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO2( _
           ByVal intIEID As Integer _
       ) As Boolean Implements IIEService.IsUsedInPO2
            ' set default return value
            IsUsedInPO2 = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objIEEnt.CountUsedInPO(intIEID)

                intCount = intCount + objIEEnt.CountUsedInPO2(intIEID)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUsedInPO2 = True
                Else
                    ' case equal 0 then return False
                    IsUsedInPO2 = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInPO2(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteIE
        '	Discription	    : Delete IE
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteIE( _
            ByVal intIEID As Integer _
        ) As Boolean Implements IIEService.DeleteIE
            ' set default return value
            DeleteIE = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteIE from IE Entity
                intEff = objIEEnt.DeleteIE(intIEID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteIE = True
                Else
                    ' case row less than 1 then return False
                    DeleteIE = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteIE(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetIEByID
        '	Discription	    : Get IE by ID
        '	Return Value	: IE dto object
        '	Create User	    : Nisa S.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetIEByID( _
           ByVal intIEID As Integer _
       ) As Dto.IEDto Implements IIEService.GetIEByID
            ' set default return value
            GetIEByID = New Dto.IEDto
            Try
                ' object for return value from Entity
                Dim objIEEntRet As New Entity.ImpIEEntity
                ' call function GetIEByID from Entity
                objIEEntRet = objIEEnt.GetIEByID(intIEID)

                ' assign value from Entity to Dto
                With GetIEByID
                    .ID = objIEEntRet.ID
                    .Name = objIEEntRet.Name
                    .Code = objIEEntRet.Code
                    .CategoryID = objIEEntRet.CategoryID
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetIEByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertIE
        '	Discription	    : Insert IE
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertIE( _
            ByVal objIEDto As Dto.IEDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IIEService.InsertIE
            ' set default return value
            InsertIE = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertIE from IE Entity
                intEff = objIEEnt.InsertIE(SetDtoToEntity(objIEDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertIE = True
                Else
                    ' case row less than 1 then return False
                    InsertIE = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_06_004"
                Else
                    ' other case
                    strMsg = "KTMS_06_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertIE(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateIE
        '	Discription	    : Update IE
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateIE( _
            ByVal objIEDto As Dto.IEDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IIEService.UpdateIE
            ' set default return value
            UpdateIE = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateIE from IE Entity
                intEff = objIEEnt.UpdateIE(SetDtoToEntity(objIEDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateIE = True
                Else
                    ' case row less than 1 then return False
                    UpdateIE = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_06_007"
                Else
                    ' other case
                    strMsg = "KTMS_06_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateIE(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: I Entity object
        '	Create User	    : Nisa S.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objIEDto As Dto.IEDto _
        ) As IMst_IEEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpIEEntity

            Try
                ' assign value to entity
                With SetDtoToEntity
                    .ID = objIEDto.ID
                    .Name = objIEDto.Name
                    .CategoryID = objIEDto.CategoryID
                    .Code = objIEDto.Code
                    .UpdatedBy = objIEDto.UpdatedBy
                    .UpdatedDate = objIEDto.UpdatedDate
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: SetListIE
        '	Discription	    : Set list ie to dropdownlist
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetListIE(ByRef objValue As DropDownList) As Boolean Implements IIEService.SetListIE
            Dim objLog As New Common.Logs.Log
            Try
                ' variable
                Dim objIE As New Entity.ImpIEEntity
                Dim objListIE As List(Of Entity.ImpIEEntity)
                Dim objComm As New Common.Utilities.Utility

                SetListIE = False
                ' get data list IE
                objListIE = objIE.GetIEForList
                If objListIE.Count < 1 Then Exit Function
                Call objComm.LoadList(objValue, objListIE, "Name", "ID")
                If objValue.Items.Count > 0 Then SetListIE = True

            Catch ex As Exception
                ' Write error log
                SetListIE = False
                objLog.ErrorLog("SetListIE", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetIEList
        '	Discription	    : Get data I/E List
        '	Return Value	: List of Dto.IEDto
        '	Create User	    : Boonyarit
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetIEListForDropdownList(Optional ByVal showCode As Boolean = False) As System.Collections.Generic.List(Of Dto.IEDto) Implements Interfaces.IIEService.GetIEListForDropdownList
            Try
                Dim objIE As New Entity.ImpIEEntity
                Dim objListIE As List(Of Entity.ImpIEEntity)
                Dim objIEDto As Dto.IEDto

                GetIEListForDropdownList = Nothing
                ' get data list IE
                If showCode = True Then
                    objListIE = objIE.GetIEForList(True)
                Else
                    objListIE = objIE.GetIEForList
                End If
                ' get data list IE
                If objListIE.Count < 1 Then Exit Function
                GetIEListForDropdownList = New List(Of Dto.IEDto)
                For Each objItem In objListIE
                    objIEDto = New Dto.IEDto
                    objIEDto.ID = objItem.ID
                    objIEDto.Name = objItem.Name
                    GetIEListForDropdownList.Add(objIEDto)
                Next

            Catch ex As Exception
                ' Write error log
                GetIEListForDropdownList = Nothing
                objLog.ErrorLog("GetIEListForDropdownList", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        ' Function name : CheckDupIE
        ' Discription     : Check duplication Account Title Master
        ' Return Value : Boolean
        ' Create User     : Nisa S.
        ' Create Date     : 15-07-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupIE( _
            ByVal intIEID As String, _
            ByVal strIECode As String, _
            ByVal strIECategory As String, _
            ByVal mode As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IIEService.CheckDupIE
            ' set default return value
            CheckDupIE = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objIEEnt.CheckDupIE(intIEID, strIECode, strIECategory)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    CheckDupIE = True
                Else
                    ' case equal 0 then return False
                    CheckDupIE = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_06_004"
                Else
                    If mode = "1" Then ' insert
                        ' other case
                        strMsg = "KTMS_06_003"
                    Else 'Moidfy
                        ' other case
                        strMsg = "KTMS_06_006"
                    End If
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupIE(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        ' Function name : GetListAccountTitleToDDL
        ' Discription   : Get list of account title to dropdownlist
        ' Return Value  : List of Dto
        ' Create User   : Wasan D.
        ' Create Date   : 15-07-2013
        ' Update User   :
        ' Update Date   :
        '*************************************************************/
        Public Function GetListAccountTitleToDDL(ByVal intCategoryType As Integer) As System.Collections.Generic.List(Of Dto.IEDto) Implements Interfaces.IIEService.GetListAccountTitleToDDL
            ' Set default value
            GetListAccountTitleToDDL = Nothing
            Try
                ' Return value as list of Dto
                GetListAccountTitleToDDL = SetEntityToDto(objIEEnt.GetListAccountTitleToDDL(intCategoryType))
            Catch ex As Exception
                'Write error log
                objLog.ErrorLog("GetListAccountTitleToDDL(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: I Entity object
        '	Create User	    : Nisa S.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetEntityToDto( _
            ByVal listIEEnt As List(Of Entity.ImpIEEntity) _
        ) As List(Of Dto.IEDto)
            ' set default return value
            SetEntityToDto = New List(Of Dto.IEDto)
            Try
                ' Variable IE Entity Object
                Dim objIEDto As Dto.IEDto

                For Each objIE In listIEEnt
                    ' Set new entity object
                    objIEDto = New Dto.IEDto
                    objIEDto.ID = objIE.ID
                    objIEDto.Code = objIE.Code
                    objIEDto.Name = objIE.Code & "-" & objIE.Name
                    ' Add Dto to list of Dto
                    SetEntityToDto.Add(objIEDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetEntityToDto(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAccountTitleForList
        '	Discription	    : Get ie for dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountTitleForList() As System.Collections.Generic.List(Of Dto.IEDto) Implements Interfaces.IIEService.GetAccountTitleForList
            ' set default list
            GetAccountTitleForList = New List(Of Dto.IEDto)
            Try
                ' objIEDto for keep value Dto 
                Dim objIEDto As Dto.IEDto
                ' listIEEnt for keep value from entity
                Dim listIEEnt As New List(Of Entity.ImpIEEntity)
                ' objIEEnt for call function
                Dim objIEEnt As New Entity.ImpIEEntity

                ' call function GetJobTypeForList
                listIEEnt = objIEEnt.GetAccountTitleForList()

                ' loop listJobTypeEnt for assign value to Dto
                For Each values In listIEEnt
                    ' new object
                    objIEDto = New Dto.IEDto
                    ' assign value to Dto
                    With objIEDto
                        .ID = values.ID
                        .Name = values.Name
                    End With
                    ' add object Dto to list Dto
                    GetAccountTitleForList.Add(objIEDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccountTitleForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region

     
    End Class
End Namespace