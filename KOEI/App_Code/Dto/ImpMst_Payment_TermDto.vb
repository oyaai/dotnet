#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_Payment_TermEntity
'	Class Discription	: Class of table mst_payment_term
'	Create User 		: Boon
'	Create Date		    : 13-05-2013
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

    <Serializable()> _
    Public Class ImpMst_Payment_TermDto

        Private _id As Integer
        Private _term_day As Integer
        Private _delete_fg As Integer
        Private _created_by As Integer?
        Private _created_date As String
        Private _updated_by As Integer?
        Private _updated_date As String



#Region "Property"
        Public Property created_by() As Integer?
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer?)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
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

        Public Property term_day() As Integer
            Get
                Return _term_day
            End Get
            Set(ByVal value As Integer)
                _term_day = value
            End Set
        End Property

        Public Property updated_by() As Integer?
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer?)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As String
            Get
                Return _updated_date
            End Get
            Set(ByVal value As String)
                _updated_date = value
            End Set
        End Property
#End Region

    End Class
End Namespace

