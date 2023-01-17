#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_PaymentConditionEntity
'	Class Discription	: Class of table mst_payment_condition
'	Create User 		: Suwishaya L.
'	Create Date		    : 17-06-2013
'
' UPDATE INFORMATION
'	Update User		: Wasan D.
'	Update Date		: 03-07-2013	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Entity
    Public Class ImpMst_PaymentConditionEntity
        Implements IMst_PaymentConditionEntity

        Private _id As Integer
        Private _first As Integer
        Private _second As Integer
        Private _third As Integer
        Private _delete_fg As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _updated_by As Integer
        Private _updated_date As String

        'property for dropdrown List
        Private _condition_id As Integer
        Private _condition_name As String
        Private _payment_condition As String

        Private objPaymentConDao As New Dao.ImpMst_PaymentConditionDao

#Region "Function"
        Public Function GetPaymentConditionForList() As System.Collections.Generic.List(Of IMst_PaymentConditionEntity) Implements IMst_PaymentConditionEntity.GetPaymentConditionForList
            Return objPaymentConDao.GetPaymentConditionForList
        End Function

        Public Function CountUsedInPO(ByVal intPayID As Integer) As Integer Implements IMst_PaymentConditionEntity.CountUsedInPO
            Return objPaymentConDao.CountUsedInPO(intPayID)
        End Function

        Public Function DeletePaymentCond(ByVal intPayID As Integer) As Integer Implements IMst_PaymentConditionEntity.DeletePaymentCond
            Return objPaymentConDao.DeletePaymentCond(intPayID)
        End Function

        Public Function GetPaymentCondByID(ByVal intPayID As Integer) As IMst_PaymentConditionEntity Implements IMst_PaymentConditionEntity.GetPaymentCondByID
            Return objPaymentConDao.GetPaymentCondByID(intPayID)
        End Function

        Public Function GetPaymentCondList(ByVal strFirst As String, ByVal strSecond As String, ByVal strThird As String) As System.Collections.Generic.List(Of ImpMst_PaymentConditionEntity) Implements IMst_PaymentConditionEntity.GetPaymentCondList
            Return objPaymentConDao.GetPaymentCondList(strFirst, strSecond, strThird)
        End Function

        Public Function InsertPaymentCond(ByVal objPayEnt As IMst_PaymentConditionEntity) As Integer Implements IMst_PaymentConditionEntity.InsertPaymentCond
            Return objPaymentConDao.InsertPaymentCond(objPayEnt)
        End Function

        Public Function UpdatePaymentCond(ByVal objPayEnt As IMst_PaymentConditionEntity) As Integer Implements IMst_PaymentConditionEntity.UpdatePaymentCond
            Return objPaymentConDao.UpdatePaymentCond(objPayEnt)
        End Function

        Public Function GetPayCondDupInsert(ByVal objPayEnt As IMst_PaymentConditionEntity) As Boolean Implements IMst_PaymentConditionEntity.GetPayCondDupInsert
            Return objPaymentConDao.GetPayCondDupInsert(objPayEnt)
        End Function

        Public Function GetPayCondDupUpdate(ByVal objPayEnt As IMst_PaymentConditionEntity) As Boolean Implements IMst_PaymentConditionEntity.GetPayCondDupUpdate
            Return objPaymentConDao.GetPayCondDupUpdate(objPayEnt)
        End Function
#End Region

#Region "Properyt"
        Public Property condition_id() As Integer Implements IMst_PaymentConditionEntity.condition_id
            Get
                Return _condition_id
            End Get
            Set(ByVal value As Integer)
                _condition_id = value
            End Set
        End Property

        Public Property condition_name() As String Implements IMst_PaymentConditionEntity.condition_name
            Get
                Return _condition_name
            End Get
            Set(ByVal value As String)
                _condition_name = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IMst_PaymentConditionEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IMst_PaymentConditionEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IMst_PaymentConditionEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property first() As Integer Implements IMst_PaymentConditionEntity.first
            Get
                Return _first
            End Get
            Set(ByVal value As Integer)
                _first = value
            End Set
        End Property

        Public Property id() As Integer Implements IMst_PaymentConditionEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property second() As Integer Implements IMst_PaymentConditionEntity.second
            Get
                Return _second
            End Get
            Set(ByVal value As Integer)
                _second = value
            End Set
        End Property

        Public Property third() As Integer Implements IMst_PaymentConditionEntity.third
            Get
                Return _third
            End Get
            Set(ByVal value As Integer)
                _third = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IMst_PaymentConditionEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As String Implements IMst_PaymentConditionEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As String)
                _updated_date = value
            End Set
        End Property

        Public Property payment_condition() As String Implements IMst_PaymentConditionEntity.payment_condition
            Get
                Return _payment_condition
            End Get
            Set(ByVal value As String)
                _payment_condition = value
            End Set
        End Property

#End Region
    End Class
End Namespace

