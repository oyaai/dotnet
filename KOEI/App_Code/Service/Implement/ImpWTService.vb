#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpWTService
'	Class Discription	: Implement WT Service
'	Create User 		: Nisa S.
'	Create Date		    : 01-07-2013
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
Imports System.Linq.Expressions
Imports Interfaces
Imports Dao
Imports Entity
Imports Exceptions
Imports System.Data
Imports Service
Imports MySql.Data.MySqlClient
#End Region

Namespace Service

    Public Class ImpWTService
        Implements IWTService

        Private objWTEntity As New Entity.ImpWTEntity
        Private objLog As New Common.Logs.Log

#Region "Functions"

        '/**************************************************************
        '	Function name	: GetWTList
        '	Discription	    : Get W/T list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWTList(ByVal strID As String, ByVal strWT As String) _
        As System.Data.DataTable Implements IWTService.GetWTList
            ' set default
            GetWTList = New DataTable
            Try
                ' variable for keep list from WT entity
                Dim listWTEntity As New List(Of Entity.IWTEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetWTList from entity
                listWTEntity = objWTEntity.GetWTList(strID, strWT)

                ' assign column header
                With GetWTList
                    .Columns.Add("id")
                    .Columns.Add("percent")
                    .Columns.Add("type")


                    ' assign row from listWTEntity
                    For Each values In listWTEntity
                        row = .NewRow
                        row("id") = values.ID
                        row("percent") = values.Percent
                        row("type") = values.Type

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWTList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetWTByID
        '	Discription	    : Get W/T by ID
        '	Return Value	: Payment Term dto object
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWTByID( _
           ByVal intWTID As Integer _
       ) As Dto.WTDto Implements IWTService.GetWTByID
            ' set default return value
            GetWTByID = New Dto.WTDto
            Try
                ' object for return value from Entity
                Dim objWTEntRet As New Entity.ImpWTEntity
                ' call function GetWTByID from Entity
                objWTEntRet = objWTEntity.GetWTByID(intWTID)

                ' assign value from Entity to Dto
                With GetWTByID
                    .ID = objWTEntRet.ID
                    .Percent = objWTEntRet.Percent
                    .Type = objWTEntRet.Type

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWTByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: IsUsedInPO
        '	Discription	    : Check W/T in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO( _
            ByVal intWTID As Integer _
        ) As Boolean Implements IWTService.IsUsedInPO
            ' set default return value
            IsUsedInPO = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objWTEntity.CountUsedInPO(intWTID)

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
        '	Function name	: DeleteWT
        '	Discription	    : Delete WT
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteWT( _
            ByVal intWTID As Integer _
        ) As Boolean Implements IWTService.DeleteWT
            ' set default return value
            DeleteWT = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteWT from WT Entity
                intEff = objWTEntity.DeleteWT(intWTID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteWT = True
                Else
                    ' case row less than 1 then return False
                    DeleteWT = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteWT(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertWT
        '	Discription	    : Insert WT
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertWT( _
            ByVal objWTDto As Dto.WTDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IWTService.InsertWT
            ' set default return value
            InsertWT = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertWT from WT Entity
                intEff = objWTEntity.InsertWT(SetDtoToEntity(objWTDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertWT = True
                Else
                    ' case row less than 1 then return False
                    InsertWT = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_10_004"
                Else
                    ' other case
                    strMsg = "KTMS_10_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertWT(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateWT
        '	Discription	    : Update WT
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateWT( _
            ByVal objWTDto As Dto.WTDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IWTService.UpdateWT
            ' set default return value
            UpdateWT = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateWT from WT Entity
                intEff = objWTEntity.UpdateWT(SetDtoToEntity(objWTDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateWT = True
                Else
                    ' case row less than 1 then return False
                    UpdateWT = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_10_007"
                Else
                    ' other case
                    strMsg = "KTMS_10_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateWT(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objWTDto As Dto.WTDto _
        ) As IWTEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpWTEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .ID = objWTDto.ID
                    .Percent = objWTDto.Percent
                    .Type = objWTDto.Type
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupWT
        ' Discription     : Check duplication WT Master
        ' Return Value    : Boolean
        ' Create User     : Nisa S.
        ' Create Date     : 01-07-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupWT( _
            ByVal intWTID As String, _
            ByVal intWT As String, _
            ByVal mode As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IWTService.CheckDupWT
            ' set default return value
            CheckDupWT = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objWTEntity.CheckDupWT(intWTID, intWT)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    CheckDupWT = True
                Else
                    ' case equal 0 then return False
                    CheckDupWT = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_10_004"
                Else
                    If mode = "1" Then ' insert
                        ' other case
                        strMsg = "KTMS_10_003"
                    Else 'Moidfy
                        ' other case
                        strMsg = "KTMS_10_006"
                    End If
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupWT(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        Public Function GetWTForList() As System.Collections.Generic.List(Of Dto.WTDto) Implements Interfaces.IWTService.GetWTForList
            Dim objLog As New Common.Logs.Log
            GetWTForList = New List(Of Dto.WTDto)
            Try
                ' entity object for call function
                Dim objWtEntity As New ImpWTEntity
                ' list entity for container value
                Dim listWTEntity As New List(Of IWTEntity)
                Dim objWTDto As Dto.WTDto

                ' call function
                listWTEntity = objWtEntity.GetWTForList

                ' loop list transform entity to dto
                For Each value In listWTEntity
                    objWTDto = New Dto.WTDto
                    With objWTDto
                        .ID = value.ID
                        .Percent = value.Percent
                        .PercentString = value.Percent.ToString & "%"
                    End With
                    GetWTForList.Add(objWTDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWTForList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetListWT
        '	Discription	    : Set list wt to dropdownlist
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetListWT(ByRef objValue As System.Web.UI.WebControls.DropDownList) As Boolean Implements Interfaces.IWTService.SetListWT
            Try
                '' variable
                Dim objWT As New Entity.ImpWTEntity
                Dim objListWT As List(Of Entity.IWTEntity)
                Dim objComm As New Common.Utilities.Utility

                SetListWT = False
                ' get data list wt
                objListWT = objWT.GetWTForList
                If objListWT.Count < 1 Then Exit Function
                Call objComm.LoadList(objValue, objListWT, "Percent", "ID")
                If objValue.Items.Count > 0 Then SetListWT = True

            Catch ex As Exception
                ' Write error log
                SetListWT = False
                'objLog.ErrorLog("SetListWT", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region

    End Class
End Namespace