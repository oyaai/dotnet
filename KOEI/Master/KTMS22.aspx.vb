#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS22
'	Class Discription	: Webpage for maintenance Country master
'	Create User 		: Suwishaya L.
'	Create Date		    : 05-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region
Partial Class Master_KTMS22
    Inherits System.Web.UI.Page

    Private objUtility As New Common.Utilities.Utility
    Private objLog As New Common.Logs.Log
    Private objCountrySer As New Service.ImpCountryService
    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private strMsg As String = String.Empty

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS22 : Country Master")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Load
        Try
            ' check postback page
            If Not IsPostBack Then 
                'get data from search condition
                GetSession()
                ' case not post back then call function initialpage
                InitialPage()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            Response.Redirect("KTMS21.aspx?New=True")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	    : Event btnClear is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClear_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnClear.Click
        Try
            ' call function ClearControl
            ClearControl()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnClear_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	    : Event btnSave is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSave.Click
        Try
            ' call function set session dto
            SetValueToDto()

            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTMS22", strConfirmIns, objMessage.GetXMLMessage("KTMS_22_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTMS22", strConfirmUpd, objMessage.GetXMLMessage("KTMS_22_008"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            ' check insert item
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                InsertCountry()
                Exit Sub
            End If

            ' check update item
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateCountry()
                Exit Sub
            End If

            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                Session("Mode") = "Add"
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                LoadInitialUpdate()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' Item Dto object for keep return value from service
            Dim objCountryDto As New Dto.CountryDto
            Dim intCountryID As Integer = 0

            ' check item id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intCountryID = CInt(objUtility.GetQueryString("id"))
            End If

            ' call function GetItemByID from service
            objCountryDto = objCountrySer.GetCountryByID(intCountryID)

            ' assign value to control
            With objCountryDto
                txtCountryID.Text = .id
                txtCountryName.Text = .name
                Session("countryID") = .id
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearControl
    '	Discription	    : Clear data each control
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' clear control
            If Session("Mode") = "Add" Then
                txtCountryID.Text = String.Empty
            End If
            txtCountryName.Text = String.Empty

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Item dto object
            Dim objCountryDto As New Dto.CountryDto

            ' assign value to dto object
            With objCountryDto
                If String.IsNullOrEmpty(txtCountryID.Text.Trim) Then
                    .id = 0
                Else
                    .id = txtCountryID.Text.Trim
                End If
                .name = txtCountryName.Text.Trim 
            End With

            ' set dto object to session
            Session("objCountryDto") = objCountryDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Item dto object
            Dim objCountryDto As New Dto.CountryDto
            ' set value to dto object from session
            objCountryDto = Session("objCountryDto")

            ' set value to control
            With objCountryDto
                If .id = 0 Then
                    txtCountryID.Text = String.Empty
                Else
                    txtCountryID.Text = .id
                End If

                txtCountryName.Text = .name
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertCountry
    '	Discription	    : Insert Country
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertCountry()
        Try
            ' call function set value to control
            SetValueToControl()

            'call function CheckDupCountry from service and alert message
            If objCountrySer.CheckDupCountry(txtCountryName.Text, "") Then
                ' call function InsertCountry from service and alert message
                If objCountrySer.InsertCountry(Session("objCountryDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_22_002"), Nothing, "KTMS21.aspx?New=True")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_22_004"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertCountry", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateCountry
    '	Discription	    : Update Country
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateCountry()
        Try
            ' call function set value to control
            SetValueToControl()
            'call function CheckDupCountry from service and alert message
            If objCountrySer.CheckDupCountry(txtCountryName.Text, Session("countryID")) Then
                ' call function UpdateCountry from service and alert message
                If objCountrySer.UpdateCountry(Session("objCountryDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_22_005"), Nothing, "KTMS21.aspx?New=True")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_22_007"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateCountry", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetSession
    '	Discription	    : Get session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetSession()
        Try
            ' clear session before used in this page
            Session("flagAddMod") = Nothing

            'Set data into session
            Session("flagAddMod") = "1" 'Flag check screen : 1: Menagemant screen , Nothing : Search screen

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

End Class
