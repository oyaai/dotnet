#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : Cheque_PurchaseDto
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
    Public Class Cheque_PurchaseDto
        'receive parameter from search screen 
        Private _strSearchType As String
        Private _strChequeNo As String
        Private _strChequeDateFrom As String
        Private _strChequeDateTo As String
        Private _strVendor_name As String

        'receive parameter from detail search screen
        Private _strId As String


#Region "Property"
        Property strSearchType() As String
            Get
                Return _strSearchType
            End Get
            Set(ByVal value As String)
                _strSearchType = value
            End Set
        End Property
        Property strChequeNo() As String
            Get
                Return _strChequeNo
            End Get
            Set(ByVal value As String)
                _strChequeNo = value
            End Set
        End Property
        Property strChequeDateFrom() As String
            Get
                Return _strChequeDateFrom
            End Get
            Set(ByVal value As String)
                _strChequeDateFrom = value
            End Set
        End Property
        Property strChequeDateTo() As String
            Get
                Return _strChequeDateTo
            End Get
            Set(ByVal value As String)
                _strChequeDateTo = value
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

