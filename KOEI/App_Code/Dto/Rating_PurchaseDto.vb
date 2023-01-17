#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : Rating_PurchaseDto
'	Class Discription	: Dto class Rating Purchase 
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
    Public Class Rating_PurchaseDto
        'receive parameter from search screen 
        Private _strSearchType As String
        Private _strPO As String
        Private _strDeliveryDateFrom As String
        Private _strDeliveryDateTo As String
        Private _strPaymentDateFrom As String
        Private _strPaymentDateTo As String
        Private _strVendor_name As String
        Private _strInvoce_no As String

        'receive parameter from detail search screen
        Private _strId As String


#Region "Property"
        Property strInvoce_no() As String
            Get
                Return _strInvoce_no
            End Get
            Set(ByVal value As String)
                _strInvoce_no = value
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

