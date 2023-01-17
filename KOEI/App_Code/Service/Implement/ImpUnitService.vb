#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpUnitService
'	Class Discription	: Class of Unit
'	Create User 		: Boon
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

#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Service
    Public Class ImpUnitService
        Implements IUnitService

        Private objLog As New Common.Logs.Log

#Region "Function"
        '/**************************************************************
        '	Function name	: CancelUnit(intUnitId)
        '	Discription	    : Cancel data unit
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CancelUnit(ByVal intUnitId As Integer) As Boolean Implements IUnitService.CancelUnit
            Try
                ' variable
                Dim objUnitEntity As New Entity.ImpMst_UnitEntity

                CancelUnit = False
                ' check data will delete or cancel
                If intUnitId < 1 Then Exit Function
                If objUnitEntity.CancelUnit(intUnitId) Then
                    ' check data result
                    CancelUnit = True
                End If

            Catch ex As Exception
                ' Write error log
                CancelUnit = False
                objLog.ErrorLog("CancelVender", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckUnitForDel
        '	Discription	    : Check data unit for delete
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckUnitForDel(ByVal intUnitId As Integer) As Boolean Implements IUnitService.CheckUnitForDel
            Try
                ' variable
                Dim objPoEntity As New Entity.ImpPo_DetailEntity

                CheckUnitForDel = False
                ' check data will delete
                If intUnitId < 1 Then Exit Function

                'Step 1 Check TB po_detail
                If objPoEntity.CheckPoByUnit(intUnitId) Then Exit Function
                'Step 2 Check successful
                CheckUnitForDel = True

            Catch ex As Exception
                ' Write error log
                CheckUnitForDel = False
                objLog.ErrorLog("CheckUnitForDel", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUnitForSearch
        '	Discription	    : Get data unit for search
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUnitForSearch(ByVal strName As String, ByRef objDT As System.Data.DataTable) As Boolean Implements IUnitService.GetUnitForSearch
            Try
                ' variable
                Dim objUnit As New Entity.ImpMst_UnitEntity
                Dim objListUnit As New List(Of Entity.IMst_UnitEntity)

                GetUnitForSearch = False
                ' check data will delete
                If strName Is Nothing AndAlso strName.Trim = String.Empty Then Exit Function
                ' get data for search
                objListUnit = objUnit.GetUnitForSearch(strName)
                ' check data result
                If objListUnit Is Nothing Then Exit Function
                If objListUnit.Count > 0 Then
                    objDT = SetUnitListToDT(objListUnit)
                    GetUnitForSearch = True
                End If

            Catch ex As Exception
                ' Write error log
                GetUnitForSearch = False
                objLog.ErrorLog("GetUnitForSearch", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetVenderListToDT
        '	Discription	    : Set data vender to datatable
        '	Return Value	: DataTable
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetUnitListToDT(ByVal objValue As List(Of Entity.IMst_UnitEntity)) As System.Data.DataTable
            Try
                ' variable
                Dim objDT As New System.Data.DataTable
                Dim objDR As System.Data.DataRow

                SetUnitListToDT = Nothing

                With objDT.Columns
                    .Add("id")
                    .Add("name")
                End With

                For Each objItem In objValue
                    With objItem
                        objDR = objDT.NewRow
                        objDR("id") = .id
                        objDR("name") = .name.Trim
                        objDT.Rows.Add(objDR)
                    End With
                Next
                SetUnitListToDT = objDT

            Catch ex As Exception
                ' Write error log
                SetUnitListToDT = Nothing
                objLog.ErrorLog("SetUnitListToDT", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckIsDupUnit
        '	Discription	    : Check data unit duplicate
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckIsDupUnit(ByVal strUnitName As String, Optional ByVal intUnitId As Integer = 0) As Boolean Implements IUnitService.CheckIsDupUnit
            Try
                ' variable
                Dim objUnitEntity As New Entity.ImpMst_UnitEntity

                ' check data Unit duplicate
                CheckIsDupUnit = objUnitEntity.CheckIsDupUnit(strUnitName, intUnitId)

            Catch ex As Exception
                CheckIsDupUnit = True
                objLog.ErrorLog("CheckIsDupUnit", ex.Message.Trim, HttpContext.Current.Session("UserName"))
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
        Public Function SaveUnit(ByVal objUnitDto As Dto.UnitDto, ByVal strMode As String) As Boolean Implements IUnitService.SaveUnit
            Try
                ' variable
                Dim objUnitEntity As New Entity.ImpMst_UnitEntity

                SaveUnit = False

                ' assign object unit entity
                With objUnitDto
                    objUnitEntity.id = .id
                    objUnitEntity.name = .name
                    objUnitEntity.created_by = HttpContext.Current.Session("UserID")
                    objUnitEntity.update_by = HttpContext.Current.Session("UserID")
                End With

                ' check mode
                If strMode = "Add" Then
                    ' insert data vendor
                    If objUnitEntity.InsertUnit(objUnitEntity) = False Then Exit Function
                ElseIf strMode = "Edit" Then
                    ' update data vendor
                    If objUnitEntity.UpdateUnit(objUnitEntity) = False Then Exit Function
                End If
                SaveUnit = True

            Catch ex As Exception
                SaveUnit = False
                objLog.ErrorLog("SaveUnit", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetListUnit
        '	Discription	    : Set list unit to dropdownlist
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetListUnit(ByRef objValue As System.Web.UI.WebControls.DropDownList) As Boolean Implements IUnitService.SetListUnit
            Try
                ' variable
                Dim objUnit As New Entity.ImpMst_UnitEntity
                Dim objListUnit As List(Of Entity.IMst_UnitEntity)
                Dim objComm As New Common.Utilities.Utility

                SetListUnit = False
                ' get data list Payment_day
                objListUnit = objUnit.GetUnitForList
                If objListUnit.Count < 1 Then Exit Function
                Call objComm.LoadList(objValue, objListUnit, "name", "id")
                If objValue.Items.Count > 0 Then SetListUnit = True

            Catch ex As Exception
                ' Write error log
                SetListUnit = False
                objLog.ErrorLog("SetListUnit", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUnitListForDropdownList
        '	Discription	    : Get data list unit to dropdownlist
        '	Return Value	: List of Dto.UnitDto
        '	Create User	    : Boonyarit
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUnitListForDropdownList() As System.Collections.Generic.List(Of Dto.UnitDto) Implements IUnitService.GetUnitListForDropdownList
            Try
                Dim objUnit As New Entity.ImpMst_UnitEntity
                Dim objListUnit As List(Of Entity.IMst_UnitEntity)
                Dim objUnitDto As Dto.UnitDto

                GetUnitListForDropdownList = Nothing
                ' get data list Item
                objListUnit = objUnit.GetUnitForList
                If objListUnit.Count < 1 Then Exit Function
                GetUnitListForDropdownList = New List(Of Dto.UnitDto)
                For Each objItem In objListUnit
                    objUnitDto = New Dto.UnitDto
                    objUnitDto.id = objItem.id
                    objUnitDto.name = objItem.name
                    GetUnitListForDropdownList.Add(objUnitDto)
                Next

            Catch ex As Exception
                ' Write error log
                GetUnitListForDropdownList = Nothing
                objLog.ErrorLog("GetUnitListForDropdownList", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region

        
    End Class
End Namespace

