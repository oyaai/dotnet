#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : UserDto
'	Class Discription	: Dto class user
'	Create User 		: Komsan Luecha
'	Create Date		    : 20-05-2013
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
    Public Class MenuDto
        Private _id As Integer
        Private _category_id As Integer
        Private _category_name As String
        Private _menu_text As String
        Private _value As String
        Private _priority As Integer
        Private _navigate_url As String

#Region "Property"
        Property category_id() As Integer
            Get
                Return _category_id
            End Get
            Set(ByVal value As Integer)
                _category_id = value
            End Set
        End Property

        Property category_name() As String
            Get
                Return _category_name
            End Get
            Set(ByVal value As String)
                _category_name = value
            End Set
        End Property

        Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Property menu_text() As String
            Get
                Return _menu_text
            End Get
            Set(ByVal value As String)
                _menu_text = value
            End Set
        End Property

        Property navigate_url() As String
            Get
                Return _navigate_url
            End Get
            Set(ByVal value As String)
                _navigate_url = value
            End Set
        End Property

        Property priority() As Integer
            Get
                Return _priority
            End Get
            Set(ByVal value As Integer)
                _priority = value
            End Set
        End Property

        Property value() As String
            Get
                Return _value
            End Get
            Set(ByVal value As String)
                _value = value
            End Set
        End Property
#End Region
    End Class
End Namespace

