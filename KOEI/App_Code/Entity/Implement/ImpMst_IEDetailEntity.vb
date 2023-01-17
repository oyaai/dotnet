'Imports Microsoft.VisualBasic

'Public Class ImpMst_IEDetailEntity

'End Class

#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_IEDetailEntity
'	Class Discription	: Class of IE Detail
'	Create User 		: Nisa S.
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

Namespace Entity
    Public Class ImpMst_IEDetailEntity
        Inherits Entity.ImpIEEntity

        Private _category_id As String = Nothing
        Private _category_name As String = Nothing
        Private _in_used As String = Nothing



#Region "Property"
        Property category_id() As String
            Get
                Return _category_id
            End Get
            Set(ByVal value As String)
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

        Property in_used() As String
            Get
                Return _in_used
            End Get
            Set(ByVal value As String)
                _in_used = value
            End Set
        End Property
        
#End Region

    End Class
End Namespace