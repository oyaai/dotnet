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
    <Serializable()> _
    Public Class UserDto

        Private _UserID As String = Nothing
        Private _UserName As String = Nothing
        Private _DepartmentName As String = Nothing
        Private _ACNextApprove As Integer = Nothing
        Private _PUNextApprove As Integer = Nothing
        Private _OSNextApprove As Integer = Nothing

#Region "Property"
        Property UserID() As String
            Get
                Return _UserID
            End Get
            Set(ByVal value As String)
                _UserID = value
            End Set
        End Property

        Property UserName() As String
            Get
                Return _UserName
            End Get
            Set(ByVal value As String)
                _UserName = value
            End Set
        End Property

        Property DepartmentName() As String
            Get
                Return _DepartmentName
            End Get
            Set(ByVal value As String)
                _DepartmentName = value
            End Set
        End Property

        Property ACNextApprove() As Integer
            Get
                Return _ACNextApprove
            End Get
            Set(ByVal value As Integer)
                _ACNextApprove = value
            End Set
        End Property

        Property PUNextApprove() As Integer
            Get
                Return _PUNextApprove
            End Get
            Set(ByVal value As Integer)
                _PUNextApprove = value
            End Set
        End Property

        Property OSNextApprove() As Integer
            Get
                Return _OSNextApprove
            End Get
            Set(ByVal value As Integer)
                _OSNextApprove = value
            End Set
        End Property
#End Region

    End Class
End Namespace

