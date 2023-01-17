#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpCountryService
'	Class Discription	: Class of Country
'	Create User 		: Suwishaya L.
'	Create Date		    : 04-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'
'******************************************************************/
#End Region

#Region "Imports"

Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Web.Configuration
Imports System.Data
Imports MySql.Data.MySqlClient

#End Region

Namespace Service
    Public Class ImpCountryService 
        Implements ICountryService

        Private objLog As New Common.Logs.Log
        Private objCountryEnt As New Entity.ImpMst_CountryEntity

#Region "Function"
        '/**************************************************************
        '	Function name	: GetCountryList
        '	Discription	    : Get Country list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCountryList( _
            ByVal strCountryName As String _
        ) As System.Data.DataTable Implements ICountryService.GetCountryList
            ' set default
            GetCountryList = New DataTable
            Try
                ' variable for keep list from Country entity
                Dim listCountryEnt As New List(Of Entity.ImpMst_CountryDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetCountryList from entity
                listCountryEnt = objCountryEnt.GetCountryList(strCountryName)

                ' assign column header
                With GetCountryList
                    .Columns.Add("Country_id")
                    .Columns.Add("Country_name")

                    ' assign row from listCountryEny
                    For Each values In listCountryEnt
                        row = .NewRow
                        row("Country_id") = values.id
                        row("Country_name") = values.name
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCountryList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteCountry
        '	Discription	    : Delete Country
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteCountry( _
            ByVal intCountryID As Integer _
        ) As Boolean Implements ICountryService.DeleteCountry
            ' set default return value
            DeleteCountry = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteCountry from Country Entity
                intEff = objCountryEnt.DeleteCountry(intCountryID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteCountry = True
                Else
                    ' case row less than 1 then return False
                    DeleteCountry = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteCountry(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetCountryID
        '	Discription	    : Get Country by ID
        '	Return Value	: Country dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCountryByID( _
           ByVal intCountryID As Integer _
       ) As Dto.CountryDto Implements ICountryService.GetCountryByID
            ' set default return value
            GetCountryByID = New Dto.CountryDto
            Try
                ' object for return value from Entity
                Dim objCountryEntRet As New Entity.ImpMst_CountryEntity
                ' call function GetCountryByID from Entity
                objCountryEntRet = objCountryEnt.GetCountryByID(intCountryID)

                ' assign value from Entity to Dto
                With GetCountryByID
                    .id = objCountryEntRet.id
                    .name = objCountryEntRet.name
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCountryByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertCountry
        '	Discription	    : Insert Country
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertCountry( _
            ByVal objCountryDto As Dto.CountryDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements ICountryService.InsertCountry
            ' set default return value
            InsertCountry = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' call function InsertCountry from Country Entity
                intEff = objCountryEnt.InsertCountry(SetDtoToEntity(objCountryDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertCountry = True
                Else
                    ' case row less than 1 then return False
                    InsertCountry = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_22_004"
                Else
                    ' other case
                    strMsg = "KTMS_22_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertCountry(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateCountry
        '	Discription	    : Update Country
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateCountry( _
            ByVal objCountryDto As Dto.CountryDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements ICountryService.UpdateCountry
            ' set default return value
            UpdateCountry = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateCountry from Country Entity
                intEff = objCountryEnt.UpdateCountry(SetDtoToEntity(objCountryDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateCountry = True
                Else
                    ' case row less than 1 then return False
                    UpdateCountry = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_22_007"
                Else
                    ' other case
                    strMsg = "KTMS_22_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateCountry(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objCountryDto As Dto.CountryDto _
        ) As Entity.IMst_CountryEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpMst_CountryEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .id = objCountryDto.id
                    .name = objCountryDto.name
                    .delete_fg = objCountryDto.delete_fg
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckDupCountry
        '	Discription	    : Check data country is duplicate
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 06-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckDupCountry( _
            ByVal strCountryName As String, _
            ByVal id As String _
        ) As Boolean Implements ICountryService.CheckDupCountry
            ' set default return value
            CheckDupCountry = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function CheckDupCountry from Country Entity
                intEff = objCountryEnt.CheckDupCountry(strCountryName, id)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    CheckDupCountry = False
                Else
                    ' case row less than 1 then return False
                    CheckDupCountry = True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupCountry(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInVendor
        '	Discription	    : Check country in used mst_vendor
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 06-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInVendor( _
            ByVal intCountryID As Integer _
        ) As Boolean Implements ICountryService.IsUsedInVendor
            ' set default return value
            IsUsedInVendor = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInVendor from entity
                intCount = objCountryEnt.CountUsedInVendor(intCountryID)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUsedInVendor = True
                Else
                    ' case equal 0 then return False
                    IsUsedInVendor = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInVendor(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetListCountryName
        '	Discription	    : Set list country to dropdownlist
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetListCountryName(ByRef objValue As System.Web.UI.WebControls.DropDownList, Optional ByVal OptionAll As Boolean = False, Optional ByVal OptionText As String = "") As Boolean Implements ICountryService.SetListCountryName
            Try
                ' variable
                Dim objCountry As New Entity.ImpMst_CountryEntity
                Dim objListCountry As List(Of Entity.IMst_CountryEntity)
                Dim objComm As New Common.Utilities.Utility

                SetListCountryName = False
                ' get data list country_name
                objListCountry = objCountry.GetListCountryName
                If objListCountry.Count < 1 Then Exit Function
                If OptionAll = True Then
                    If OptionText = String.Empty Then
                        Call objComm.LoadList(objValue, objListCountry, "name", "id", True, "ALL")
                    Else
                        Call objComm.LoadList(objValue, objListCountry, "name", "id", True, " ")
                    End If
                Else
                    Call objComm.LoadList(objValue, objListCountry, "name", "id")
                End If
                If objValue.Items.Count > 0 Then SetListCountryName = True

            Catch ex As Exception
                ' Write error log
                SetListCountryName = False
                objLog.ErrorLog("SetListCountryName", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region

    End Class
End Namespace
