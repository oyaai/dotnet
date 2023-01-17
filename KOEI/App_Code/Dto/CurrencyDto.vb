
#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : CurrencyDto
'	Class Discription	: Dto class Currency
'	Create User 		: Suwishaya L.
'	Create Date		    : 18-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Import"
Imports Microsoft.VisualBasic
#End Region

Namespace Dto
    Public Class CurrencyDto
        Private _id As Integer
        Private _name As String

#Region "Property"
        Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Property name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property
#End Region

    End Class
End Namespace
