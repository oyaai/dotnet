
#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : CountryDto
'	Class Discription	: Dto class Country 
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

#Region "Import"
Imports Microsoft.VisualBasic
#End Region

Namespace Dto
    Public Class CountryDto
        Private _id As Integer
        Private _name As String
        Private _delete_fg As Integer

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

        Property delete_fg() As Integer
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property
#End Region

    End Class
End Namespace

