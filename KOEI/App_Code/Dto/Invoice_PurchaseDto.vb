#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : Invoice_PurchaseDto
'	Class Discription	: Dto class Invoice Purchase 
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 20-06-2013
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

Namespace Dto
    Public Class Invoice_PurchaseDto
        'receive parameter from search screen 
        Private _strSearchType As String
        Private _strPO As String
        Private _strPOFrom As String
        Private _strPOTo As String
        Private _strDeliveryDateFrom As String
        Private _strDeliveryDateTo As String
        Private _strPaymentDateFrom As String
        Private _strPaymentDateTo As String
        Private _strVendor_name As String
        Private _strInvoice_start As String
        Private _strInvoice_end As String
        Private _strIssueDateFrom As String
        Private _strIssueDateTo As String

        'receive parameter from detail search screen
        Private _strId As String

        Private _strAccountNo As String
        Private _strAccountName As String
        Private _strTotal_Amount As String
        Private _strVat_Amount As String
        Private _strRemark As String
        Private _strPO_header_id As String
        Private _taskID As String
        Private _strMode As String

#Region "Property"
        Property strMode() As String
            Get
                Return _strMode
            End Get
            Set(ByVal value As String)
                _strMode = value
            End Set
        End Property
        Property taskID() As String
            Get
                Return _taskID
            End Get
            Set(ByVal value As String)
                _taskID = value
            End Set
        End Property
        Property strPO_header_id() As String
            Get
                Return _strPO_header_id
            End Get
            Set(ByVal value As String)
                _strPO_header_id = value
            End Set
        End Property
        Property strAccountNo() As String
            Get
                Return _strAccountNo
            End Get
            Set(ByVal value As String)
                _strAccountNo = value
            End Set
        End Property
        Property strAccountName() As String
            Get
                Return _strAccountName
            End Get
            Set(ByVal value As String)
                _strAccountName = value
            End Set
        End Property
        Property strTotal_Amount() As String
            Get
                Return _strTotal_Amount
            End Get
            Set(ByVal value As String)
                _strTotal_Amount = value
            End Set
        End Property
        Property strVAT_Amount() As String
            Get
                Return _strVAT_Amount
            End Get
            Set(ByVal value As String)
                _strVAT_Amount = value
            End Set
        End Property
        Property strRemark() As String
            Get
                Return _strRemark
            End Get
            Set(ByVal value As String)
                _strRemark = value
            End Set
        End Property
        Property strIssueDateFrom() As String
            Get
                Return _strIssueDateFrom
            End Get
            Set(ByVal value As String)
                _strIssueDateFrom = value
            End Set
        End Property
        Property strIssueDateTo() As String
            Get
                Return _strIssueDateTo
            End Get
            Set(ByVal value As String)
                _strIssueDateTo = value
            End Set
        End Property
        Property strPOFrom() As String
            Get
                Return _strPOFrom
            End Get
            Set(ByVal value As String)
                _strPOFrom = value
            End Set
        End Property
        Property strPOTo() As String
            Get
                Return _strPOTo
            End Get
            Set(ByVal value As String)
                _strPOTo = value
            End Set
        End Property
        Property strSearchType() As String
            Get
                Return _strSearchType
            End Get
            Set(ByVal value As String)
                _strSearchType = value
            End Set
        End Property
        Property strPO() As String
            Get
                Return _strPO
            End Get
            Set(ByVal value As String)
                _strPO = value
            End Set
        End Property
        Property strDeliveryDateFrom() As String
            Get
                Return _strDeliveryDateFrom
            End Get
            Set(ByVal value As String)
                _strDeliveryDateFrom = value
            End Set
        End Property
        Property strDeliveryDateTo() As String
            Get
                Return _strDeliveryDateTo
            End Get
            Set(ByVal value As String)
                _strDeliveryDateTo = value
            End Set
        End Property
        Property strPaymentDateFrom() As String
            Get
                Return _strPaymentDateFrom
            End Get
            Set(ByVal value As String)
                _strPaymentDateFrom = value
            End Set
        End Property
        Property strPaymentDateTo() As String
            Get
                Return _strPaymentDateTo
            End Get
            Set(ByVal value As String)
                _strPaymentDateTo = value
            End Set
        End Property
        Property strVendor_name() As String
            Get
                Return _strVendor_name
            End Get
            Set(ByVal value As String)
                _strVendor_name = value
            End Set
        End Property
        Property strInvoice_start() As String
            Get
                Return _strInvoice_start
            End Get
            Set(ByVal value As String)
                _strInvoice_start = value
            End Set
        End Property
        Property strInvoice_end() As String
            Get
                Return _strInvoice_end
            End Get
            Set(ByVal value As String)
                _strInvoice_end = value
            End Set
        End Property
        Property strId() As String
            Get
                Return _strId
            End Get
            Set(ByVal value As String)
                _strId = value
            End Set
        End Property
#End Region

    End Class
End Namespace

