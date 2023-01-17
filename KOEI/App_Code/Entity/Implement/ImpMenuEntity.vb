#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMenuEntity
'	Class Discription	: Implement Menu entity
'	Create User 		: Komsan L.
'	Create Date		    : 21-05-2013
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

Namespace Entity
    Public Class ImpMenuEntity
        Implements Entity.IMenuEntity

        Private _id As Integer
        Private _category_id As Integer
        Private _category_name As String
        Private _menu_text As String
        Private _value As String
        Private _priority As Integer
        Private _navigate_url As String

        Private daoMenu As New Dao.ImpMenuDao
#Region "Property"
        Public Property category_id() As Integer Implements IMenuEntity.category_id
            Get
                Return _category_id
            End Get
            Set(ByVal value As Integer)
                _category_id = value
            End Set
        End Property

        Public Property category_name() As String Implements IMenuEntity.category_name
            Get
                Return _category_name
            End Get
            Set(ByVal value As String)
                _category_name = value
            End Set
        End Property

        Public Property id() As Integer Implements IMenuEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property menu_text() As String Implements IMenuEntity.menu_text
            Get
                Return _menu_text
            End Get
            Set(ByVal value As String)
                _menu_text = value
            End Set
        End Property

        Public Property navigate_url() As String Implements IMenuEntity.navigate_url
            Get
                Return _navigate_url
            End Get
            Set(ByVal value As String)
                _navigate_url = value
            End Set
        End Property

        Public Property priority() As Integer Implements IMenuEntity.priority
            Get
                Return _priority
            End Get
            Set(ByVal value As Integer)
                _priority = value
            End Set
        End Property

        Public Property value() As String Implements IMenuEntity.value
            Get
                Return _value
            End Get
            Set(ByVal value As String)
                _value = value
            End Set
        End Property
#End Region

#Region "Funtion"
        Public Function GetMenuList(ByVal intUserID As Integer) As System.Collections.Generic.List(Of IMenuEntity) Implements IMenuEntity.GetMenuList
            Return daoMenu.GetMenuList(intUserID)
        End Function
#End Region

       
    End Class
End Namespace

