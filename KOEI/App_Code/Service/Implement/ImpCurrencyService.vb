#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpCurrencyService
'	Class Discription	: Class of Currency
'	Create User 		: Boonyarit
'	Create Date		    : 17-06-2013
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
    Public Class ImpCurrencyService
        Implements ICurrencyService

        Private objLog As New Common.Logs.Log
        Private objCurrencyEnt As New Entity.ImpMst_CurrencyEntity


#Region "Function"
        ''/**************************************************************
        ''	Function name	: SetListCurrency
        ''	Discription	    : Set list currency_name to dropdownlist
        ''	Return Value	: Boolean
        ''	Create User	    : Boonyarit
        ''	Create Date	    : 17-06-2013
        ''	Update User	    :
        ''	Update Date	    :
        ''*************************************************************/
        'Public Function SetListCurrency(ByRef objValue As System.Web.UI.WebControls.DropDownList, Optional ByVal optionAll As Boolean = False) As Boolean Implements ICurrencyService.SetListCurrency
        '    Try
        '        ' variable
        '        Dim objCurrency As New Entity.ImpMst_CurrencyEntity
        '        Dim objListCurrency As List(Of Entity.IMst_CurrencyEntity)
        '        Dim objComm As New Common.Utilities.Utility

        '        SetListCurrency = False
        '        ' get data list Payment_day
        '        objListCurrency = objCurrency.GetCurrencyForDropdownList
        '        If objListCurrency.Count < 1 Then Exit Function
        '        If optionAll = True Then
        '            Call objComm.LoadList(objValue, objListCurrency, "name", "id", True, "All")
        '        Else
        '            Call objComm.LoadList(objValue, objListCurrency, "name", "id")
        '        End If
        '        If objValue.Items.Count > 0 Then SetListCurrency = True

        '    Catch ex As Exception
        '        ' Write error log
        '        SetListCurrency = False
        '        objLog.ErrorLog("SetListCurrency", ex.Message.Trim, HttpContext.Current.Session("UserName"))
        '    End Try
        'End Function

        '/**************************************************************
        '	Function name	: GetCurrencyForList
        '	Discription	    : Get list currency_name to dropdownlist
        '	Return Value	: List of Dto.CurrencyDto
        '	Create User	    : Boonyarit
        '	Create Date	    : 23-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCurrencyForList() As System.Collections.Generic.List(Of Dto.CurrencyDto) Implements ICurrencyService.GetCurrencyForList
            ' set default list
            GetCurrencyForList = New List(Of Dto.CurrencyDto)
            Try
                ' objJobTypeDto for keep value Dto 
                Dim objCurrencyDto As Dto.CurrencyDto
                ' listCurrencyEnt for keep value from entity
                Dim listCurrencyEnt As New List(Of Entity.IMst_CurrencyEntity)
                ' objCurrencyEnt for call function
                Dim objJCurrencyEnt As New Entity.ImpMst_CurrencyEntity

                ' call function GetJCurrencyForList
                listCurrencyEnt = objJCurrencyEnt.GetCurrencyForList

                ' loop listCurrencyEnt for assign value to Dto
                For Each values In listCurrencyEnt
                    ' new object
                    objCurrencyDto = New Dto.CurrencyDto
                    ' assign value to Dto
                    With objCurrencyDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetCurrencyForList.Add(objCurrencyDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCurrencyForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetCurrencyForDropdownList
        '	Discription	    : Get Currency for dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCurrencyForDropdownList() As System.Collections.Generic.List(Of Dto.CurrencyDto) Implements ICurrencyService.GetCurrencyForDropdownList

            ' set default list
            GetCurrencyForDropdownList = New List(Of Dto.CurrencyDto)
            Try
                ' objJobTypeDto for keep value Dto 
                Dim objCurrencyDto As Dto.CurrencyDto
                ' listCurrencyEnt for keep value from entity
                Dim listCurrencyEnt As New List(Of Entity.IMst_CurrencyEntity)
                ' objCurrencyEnt for call function
                Dim objJCurrencyEnt As New Entity.ImpMst_CurrencyEntity

                ' call function GetJCurrencyForList
                listCurrencyEnt = objJCurrencyEnt.GetCurrencyForDropdownList()

                ' loop listCurrencyEnt for assign value to Dto
                For Each values In listCurrencyEnt
                    ' new object
                    objCurrencyDto = New Dto.CurrencyDto
                    ' assign value to Dto
                    With objCurrencyDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetCurrencyForDropdownList.Add(objCurrencyDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCurrencyForDropdownList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetCurrencyList
        '	Discription	    : Get Currency list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCurrencyList(ByVal strCurrency As String) _
        As System.Data.DataTable Implements Service.ICurrencyService.GetCurrencyList
            ' set default
            GetCurrencyList = New DataTable
            Try
                ' variable for keep list from Currency entity
                Dim listCurrencyEnt As New List(Of Entity.IMst_CurrencyEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetCurrencyList from entity
                listCurrencyEnt = objCurrencyEnt.GetCurrencyList(strCurrency)

                ' assign column header
                With GetCurrencyList
                    .Columns.Add("id")
                    .Columns.Add("Currency")


                    ' assign row from listCurrencyEnt
                    For Each values In listCurrencyEnt
                        row = .NewRow
                        row("id") = values.id
                        row("Currency") = values.name

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCurrencyList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInPO
        '	Discription	    : Check Currency in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO( _
            ByVal intCurrencyID As Integer _
        ) As Boolean Implements Service.ICurrencyService.IsUsedInPO
            ' set default return value
            IsUsedInPO = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objCurrencyEnt.CountUsedInPO(intCurrencyID)

                intCount = intCount + objCurrencyEnt.CountUsedInPO2(intCurrencyID)

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
        '	Discription	    : Check Currency in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO2( _
           ByVal intCurrencyID As Integer _
       ) As Boolean Implements Service.ICurrencyService.IsUsedInPO2
            ' set default return value
            IsUsedInPO2 = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objCurrencyEnt.CountUsedInPO(intCurrencyID)

                intCount = intCount + objCurrencyEnt.CountUsedInPO2(intCurrencyID)

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
        '	Function name	: DeleteCurrency
        '	Discription	    : Delete Currency
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteCurrency( _
            ByVal intCurrencyID As Integer _
        ) As Boolean Implements ICurrencyService.DeleteCurrency
            ' set default return value
            DeleteCurrency = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteCurrency from Currency Entity
                intEff = objCurrencyEnt.DeleteCurrency(intCurrencyID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteCurrency = True
                Else
                    ' case row less than 1 then return False
                    DeleteCurrency = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteCurrency(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetCurrencyByID
        '	Discription	    : Get Currency by ID
        '	Return Value	: Currency dto object
        '	Create User	    : Nisa S.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCurrencyByID( _
           ByVal intCurrencyID As Integer _
       ) As Dto.CurrencyDto Implements ICurrencyService.GetCurrencyByID
            ' set default return value
            GetCurrencyByID = New Dto.CurrencyDto
            Try
                ' object for return value from Entity
                Dim objCurrencyEntRet As New Entity.ImpMst_CurrencyEntity
                ' call function GetCurrencyByID from Entity
                objCurrencyEntRet = objCurrencyEnt.GetCurrencyByID(intCurrencyID)

                ' assign value from Entity to Dto
                With GetCurrencyByID
                    .id = objCurrencyEntRet.id
                    .name = objCurrencyEntRet.name

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCurrencyByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertCurrency
        '	Discription	    : Insert Currency
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertCurrency( _
            ByVal objCurrencyDto As Dto.CurrencyDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements ICurrencyService.InsertCurrency
            ' set default return value
            InsertCurrency = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertCurrency from Currency Entity
                intEff = objCurrencyEnt.InsertCurrency(SetDtoToEntity(objCurrencyDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertCurrency = True
                Else
                    ' case row less than 1 then return False
                    InsertCurrency = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_18_004"
                Else
                    ' other case
                    strMsg = "KTMS_18_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertCurrency(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateCurrency
        '	Discription	    : Update Currency
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateCurrency( _
            ByVal objCurrencyDto As Dto.CurrencyDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements ICurrencyService.UpdateCurrency
            ' set default return value
            UpdateCurrency = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateCurrency from Currency Entity
                intEff = objCurrencyEnt.UpdateCurrency(SetDtoToEntity(objCurrencyDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateCurrency = True
                Else
                    ' case row less than 1 then return False
                    UpdateCurrency = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_18_007"
                Else
                    ' other case
                    strMsg = "KTMS_18_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateCurrency(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Currency Entity object
        '	Create User	    : Nisa S.
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objCurrencyDto As Dto.CurrencyDto _
    ) As Entity.IMst_CurrencyEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpMst_CurrencyEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .id = objCurrencyDto.id
                    .name = objCurrencyDto.name
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupCurrency
        ' Discription     : Check duplication Currency Master
        ' Return Value    : Boolean
        ' Create User     : Nisa S.
        ' Create Date     : 28-06-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupCurrency( _
            ByVal intCurrencyID As String, _
            ByVal intCurrency As String, _
            ByVal mode As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements ICurrencyService.CheckDupCurrency
            ' set default return value
            CheckDupCurrency = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objCurrencyEnt.CheckDupCurrency(intCurrencyID, intCurrency)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    CheckDupCurrency = True
                Else
                    ' case equal 0 then return False
                    CheckDupCurrency = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_18_007"
                Else
                    If mode = "1" Then ' insert
                        ' other case
                        strMsg = "KTMS_18_003"
                    Else 'Moidfy
                        ' other case
                        strMsg = "KTMS_18_006"
                    End If

                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupCurrency(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region

    End Class
End Namespace

