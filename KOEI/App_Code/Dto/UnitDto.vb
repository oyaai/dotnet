#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : UnitDto
'	Class Discription	: Dto class Unit
'	Create User 		: Boonyarit
'	Create Date		    : 04-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Dto
    Public Class UnitDto
        Private _id As Integer
        Private _name As String

#Region "Property"
        Public Property name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property
#End Region

    End Class
End Namespace

