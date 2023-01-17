#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpVatService
'	Class Discription	: Implement Vat Service
'	Create User 		: Nisa S.
'	Create Date		    : 25-06-2013
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
Imports Exceptions
Imports Interfaces
Imports Entity
Imports Dao
Imports Microsoft.VisualBasic
Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Linq.Expressions
Imports Dto
#End Region

Namespace Service

    Public Class ImpVatService
        Implements IVatService

        Private objLog As New Common.Logs.Log
        Private objVatEnt As New Entity.ImpVatEntity

#Region "Functions"
        '/**************************************************************
        '	Function name	: GetVatList
        '	Discription	    : Get vat list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVatList(ByVal strID As String, ByVal strPercent As String) As System.Data.DataTable Implements IVatService.GetVatList
            ' set default
            GetVatList = New DataTable
            Try
                ' variable for keep list from item entity
                Dim listVatEnt As New List(Of ImpVatDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetItemList from entity
                listVatEnt = objVatEnt.GetVatList(strID, strPercent)

                ' assign column header
                With GetVatList
                    .Columns.Add("ID")
                    .Columns.Add("Percent")

                    ' assign row from listVatEnt
                    For Each values In listVatEnt
                        row = .NewRow
                        row("ID") = values.ID
                        row("Percent") = values.Percent

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVatList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInPO
        '	Discription	    : Check vat in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO( _
            ByVal intVatID As Integer _
        ) As Boolean Implements IVatService.IsUsedInPO
            ' set default return value
            IsUsedInPO = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objVatEnt.CountUsedInPO(intVatID)

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
        '	Function name	: DeleteVat
        '	Discription	    : Delete vat
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteVat( _
            ByVal intVatID As Integer _
        ) As Boolean Implements IVatService.DeleteVat
            ' set default return value
            DeleteVat = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteVat from vat Entity
                intEff = objVatEnt.DeleteVat(intVatID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteVat = True
                Else
                    ' case row less than 1 then return False
                    DeleteVat = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteVat(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Vat Entity object
        '	Create User	    : Nisa S.
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objVatDto As Dto.VatDto _
        ) As IVatEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpVatEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .ID = objVatDto.ID
                    .Percent = objVatDto.Percent
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertVat
        '	Discription	    : Insert Vat
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertVat( _
            ByVal objVatDto As Dto.VatDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IVatService.InsertVat
            ' set default return value
            InsertVat = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertItem from Item Entity
                intEff = objVatEnt.InsertVat(SetDtoToEntity(objVatDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertVat = True
                Else
                    ' case row less than 1 then return False
                    InsertVat = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_08_004"
                Else
                    ' other case
                    strMsg = "KTMS_08_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertVat(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateVat
        '	Discription	    : Update Vat
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateVat( _
            ByVal objVatDto As Dto.VatDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IVatService.UpdateVat
            ' set default return value
            UpdateVat = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdatePaymentTerm from Payment Term Entity
                intEff = objVatEnt.UpdateVat(SetDtoToEntity(objVatDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateVat = True
                Else
                    ' case row less than 1 then return False
                    UpdateVat = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_08_007"
                Else
                    ' other case
                    strMsg = "KTMS_08_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateVat(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupVat
        ' Discription     : Check duplication Vat Master
        ' Return Value    : Boolean
        ' Create User     : Nisa S.
        ' Create Date     : 26-06-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupVat( _
            ByVal intVatID As String, _
            ByVal intVat As String, _
            ByVal mode As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IVatService.CheckDupVat
            ' set default return value
            CheckDupVat = False

            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objVatEnt.CheckDupVat(intVatID, intVat)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    CheckDupVat = True
                Else
                    ' case equal 0 then return False
                    CheckDupVat = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_08_004"
                Else
                    If mode = "1" Then ' insert
                        ' other case
                        strMsg = "KTMS_08_003"
                    Else 'Moidfy
                        ' other case
                        strMsg = "KTMS_08_006"
                    End If

                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupVat(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetVatByID
        '	Discription	    : Get Vat by ID
        '	Return Value	: Vat dto object
        '	Create User	    : Nisa S.
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVatByID( _
           ByVal intVatID As Integer _
       ) As Dto.VatDto Implements IVatService.GetVatByID
            ' set default return value
            GetVatByID = New Dto.VatDto
            Try
                ' object for return value from Entity
                Dim objVatEntRet As New Entity.ImpVatEntity
                ' call function GetItemByID from Entity
                objVatEntRet = objVatEnt.GetVatByID(intVatID)

                ' assign value from Entity to Dto
                With GetVatByID
                    .ID = objVatEntRet.ID
                    .Percent = objVatEntRet.Percent

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVatByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetVatForList
        '	Discription		: Get Vat for list
        '	Return Value	: List
        '	Create User		: Komsan L.
        '	Create Date		: 17-06-2013
        '	Update User		:
        '	Update Date		:
        '**************************************************************/
        Public Function GetVatForList( _
        ) As System.Collections.Generic.List(Of Dto.VatDto) Implements Interfaces.IVatService.GetVatForList
            ' variable log object
            Dim objLog As New Common.Logs.Log
            ' set default return value
            GetVatForList = New List(Of Dto.VatDto)
            Try
                ' entity object for call function
                Dim objVatEntity As New ImpVatEntity
                ' list entity for container value
                Dim listVatEntity As New List(Of IVatEntity)
                Dim objVatDto As Dto.VatDto

                ' call function
                listVatEntity = objVatEntity.GetVatForList

                ' loop list transform entity to dto
                For Each value In listVatEntity
                    objVatDto = New Dto.VatDto
                    With objVatDto
                        .ID = value.ID
                        .Percent = value.Percent
                        .PercentString = value.Percent.ToString & "%"
                    End With
                    GetVatForList.Add(objVatDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVatForList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetListVat
        '	Discription	    : Set list vat to dropdownlist
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetListVat(ByRef objValue As System.Web.UI.WebControls.DropDownList) As Boolean Implements Interfaces.IVatService.SetListVat
            Dim objLog As New Common.Logs.Log
            Try
                ' variable
                Dim objVat As New Entity.ImpVatEntity
                Dim objListVat As List(Of Interfaces.IVatEntity)
                Dim objComm As New Common.Utilities.Utility

                SetListVat = False
                ' get data list Payment_day
                objListVat = objVat.GetVatForList
                If objListVat.Count < 1 Then Exit Function
                Call objComm.LoadList(objValue, objListVat, "Percent", "ID")
                If objValue.Items.Count > 0 Then SetListVat = True

            Catch ex As Exception
                ' Write error log
                SetListVat = False
                objLog.ErrorLog("SetListVat", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region

    End Class
End Namespace