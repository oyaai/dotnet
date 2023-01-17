#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : StaffDto
'	Class Discription	: Dto class Staff 
'	Create User 		: Nisa S.
'	Create Date		    : 04-07-2013
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
    Public Class StaffDto

        Private _id As Int32
        Private _first_name As String
        Private _last_name As String
        Private _work_category_id As String
        Private _delete_fg As Integer
        Private _created_by As Int32?
        Private _created_date As String
        Private _updated_by As Int32?
        Private _updated_date As String

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

        Property first_name() As String
            Get
                Return _first_name
            End Get
            Set(ByVal value As String)
                _first_name = value
            End Set
        End Property

        Property last_name() As String
            Get
                Return _last_name
            End Get
            Set(ByVal value As String)
                _last_name = value
            End Set
        End Property

        Property work_category_id() As String
            Get
                Return _work_category_id
            End Get
            Set(ByVal value As String)
                _work_category_id = value
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


        Public Property created_by() As Int32?
            Get
                Return Me._created_by
            End Get
            Set(ByVal value As Int32?)
                Me._created_by = value
            End Set
        End Property

        Public Property created_date() As String
            Get
                Return Me._created_date
            End Get
            Set(ByVal value As String)
                Me._created_date = value
            End Set
        End Property

        Property updated_by() As Integer
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Property updated_date() As Integer
            Get
                Return _updated_date
            End Get
            Set(ByVal value As Integer)
                _updated_date = value
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
