#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : WTDto
'	Class Discription	: Dto class wt 
'	Create User 		: Nisa S.
'	Create Date		    : 01-07-2013
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
    Public Class WTDto

#Region "Fields"

        Private _createdBy As Long
        Private _createdDate As String
        Private _deleteFlag As Byte
        Private _id As Byte
        Private _percent As Byte
        Private _type As String
        Private _updatedBy As Long?
        Private _updatedDate As String

        Private _isInUsed As Boolean

        Private _user As UserDto
        Private _percent_string As String
#End Region

#Region "Properties"

        Public Property CreatedBy() As Long
            Get
                Return Me._createdBy
            End Get
            Set(ByVal value As Long)
                Me._createdBy = value
            End Set
        End Property


        '''<summary>
        '''There are no comments for Property created_date in the schema.
        '''</summary>
        Public Property CreatedDate() As String
            Get
                Return Me._createdDate
            End Get
            Set(ByVal value As String)

                Me._createdDate = value

            End Set
        End Property

        '''<summary>
        '''There are no comments for Property delete_fg in the schema.
        '''</summary>
        Public Property DeleteFlag() As Byte
            Get
                Return Me._deleteFlag
            End Get
            Set(ByVal value As Byte)
                Me._deleteFlag = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property id in the schema.
        '''</summary>
        Public Property ID() As Byte
            Get
                Return Me._id
            End Get
            Set(ByVal value As Byte)
                Me._id = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property percent in the schema.
        '''</summary>
        Public Property Percent() As Byte
            Get
                Return Me._percent
            End Get
            Set(ByVal value As Byte)
                Me._percent = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property type in the schema.
        '''</summary>

        Public Property Type() As String
            Get
                Return Me._type
            End Get
            Set(ByVal value As String)
                Me._type = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property updated_by in the schema.
        '''</summary>
        Public Property UpdatedBy() As Long?
            Get
                Return Me._updatedBy
            End Get
            Set(ByVal value As Long?)
                Me._updatedBy = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property updated_date in the schema.
        '''</summary>
        Public Property UpdatedDate() As String
            Get
                Return Me._updatedDate
            End Get
            Set(ByVal value As String)
                Me._updatedDate = value
            End Set
        End Property

        Public Property User() As UserDto
            Get
                Return _user
            End Get
            Set(ByVal value As UserDto)
                _user = value
            End Set
        End Property

        Public Property PercentString() As String
            Get
                Return _percent_string
            End Get
            Set(ByVal value As String)
                _percent_string = value
            End Set
        End Property

        Public Property IsInUsed() As Boolean
            Get
                Return _isInUsed
            End Get
            Friend Set(ByVal value As Boolean)
                _isInUsed = value
            End Set
        End Property

#End Region

    End Class
End Namespace