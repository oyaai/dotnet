Imports System.Data
Imports System.Web.Configuration
Imports OfficeOpenXml.Style
Imports OfficeOpenXml
Imports System.IO
Imports System.Globalization
Imports System.Web.Services
Imports System.Web.Services.WebMethodAttribute

#Region "History"
'******************************************************************
' Copyright KOEI TOOL (Thailand) co., ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Sale Invoice ExChange Rate
'	Class Name		    : JobOrder_KTJB05_Exchange
'	Class Discription	: Webpage for ExChange Rate
'	Create User 		: Rawikarn K.
'	Create Date		    : 12-09-2014
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region


Partial Class JobOrder_KTJB05_Exchange
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objSaleInvoiceSer As New Service.ImpSale_InvoiceService
    Private objSaleInvoiceVal As New Validations.CommonValidation
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 12-09-2014
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTJB05_ExChangeRate : Sale Invoice")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 12-09-2014
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
                
                ' case not post back then call function initialpage
                InitialPage()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: btnSubmit_Click
    '	Discription	    : Event btnSubmit is Clicked
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 25-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSubmit_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSubmit.Click
        Try
            Dim objExChangeRateDto As New Dto.SaleInvoiceDto
            Dim ExChangeRate As Boolean

            ExChangeRate = objSaleInvoiceSer.NewExChangeRate(Session("ReceiveHeaderId"), txtExChangeRate.Text)

            'objExChangeRateDto = objSaleInvoiceSer.NewExChangeRate(Session("ReceiveHeaderId"), txtExChangeRate.Text)

            If Not IsNothing(ExChangeRate) = True Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_Exchange_001"))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_Exchange_002"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSubmit_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 12-09-2014
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Dim intID As Integer
        Try
            intID = Request.QueryString("ID")
            GetExChangeRate(intID)

            Session("ReceiveHeaderId") = intID

            If Request.QueryString("ID") Then
                Session("ID") = Request.QueryString("ID")
            End If

            ' check delete item
            If objUtility.GetQueryString("mode") = "Add" Then
                UpdateExChangeRate(Session("ID"), Session("objExChangeRate"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: GetExChangeRate
    '	Discription	    : Get ExChange value
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 12-09-2014
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetExChangeRate(ByVal intID As Integer)
        Try
            Dim objActualRate As Integer

            objActualRate = objSaleInvoiceSer.GetExChangeRate(intID)

            Session("objActualRate") = objActualRate

        Catch ex As Exception
            objLog.ErrorLog("GetExChangeRate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateExChangeRate
    '	Discription	    : Get ExChange value
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 12-09-2014
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateExChangeRate(ByVal intID As String, ByVal objExChangeRate As System.Data.DataTable)
        Try
            Dim objNewExChangeRate As Integer

            objNewExChangeRate = objSaleInvoiceSer.UpdateExChangeRate(intID, objExChangeRate)

            Session("ID") = intID
            Session("objExChangeRate") = objExChangeRate

        Catch ex As Exception
            objLog.ErrorLog("GetExChangeRate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region



End Class
