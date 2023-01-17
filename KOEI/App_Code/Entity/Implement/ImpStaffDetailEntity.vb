
#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpStaffDetailEntity
'	Class Discription	: Class of Staff detail
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

Namespace Entity
    Public Class ImpStaffDetailEntity
        Inherits Entity.ImpStaffEntity


        Private _section As String = Nothing
        Private _work_category_name As String = Nothing
        Private _inuse As Integer = Nothing

#Region "Property"
        

        Property work_category_name() As String
            Get
                Return _work_category_name
            End Get
            Set(ByVal value As String)
                _work_category_name = value
            End Set
        End Property

        Property inuse() As Integer
            Get
                Return _inuse
            End Get
            Set(ByVal value As Integer)
                _inuse = value
            End Set
        End Property

        Property section() As String
            Get
                Return _section
            End Get
            Set(ByVal value As String)
                _section = value
            End Set
        End Property
#End Region

    End Class
End Namespace
