#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : PaymentConditionDto
'	Class Discription	: Dto class Payment Condition
'	Create User 		: Suwishaya L.
'	Create Date		    : 17-06-2013
'
' UPDATE INFORMATION
'	Update User		: Wasan D.
'	Update Date		: 02/07/2013
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Dto
    Public Class PaymentConditionDto
        'for get data to dropdrown list
        Private _codition_id As Integer
        Private _codition_name As String

        Private _codition_1st As Integer
        Private _codition_2nd As Integer
        Private _codition_3rd As Integer


#Region "Property"
        Property codition_id() As Integer
            Get
                Return _codition_id
            End Get
            Set(ByVal value As Integer)
                _codition_id = value
            End Set
        End Property

        Property codition_name() As String
            Get
                Return _codition_name
            End Get
            Set(ByVal value As String)
                _codition_name = value
            End Set
        End Property

        Property codition_1st() As Integer
            Get
                Return _codition_1st
            End Get
            Set(ByVal value As Integer)
                _codition_1st = value
            End Set
        End Property

        Property codition_2nd() As Integer
            Get
                Return _codition_2nd
            End Get
            Set(ByVal value As Integer)
                _codition_2nd = value
            End Set
        End Property

        Property codition_3rd() As Integer
            Get
                Return _codition_3rd
            End Get
            Set(ByVal value As Integer)
                _codition_3rd = value
            End Set
        End Property

#End Region

    End Class
End Namespace

