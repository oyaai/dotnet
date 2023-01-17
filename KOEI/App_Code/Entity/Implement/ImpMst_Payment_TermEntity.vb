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

Namespace Entity
    Public Class ImpMst_Payment_TermEntity
        Implements IMst_Payment_TermEntity

        Private _id As Integer
        Private _term_day As String
        Private _delete_fg As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _updated_by As Integer
        Private _updated_date As String

        Private objPaymentTerm As New Dao.ImpMst_Payment_TermDao

#Region "Function"
        Public Function GetListPaymentDay() As System.Collections.Generic.List(Of IMst_Payment_TermEntity) Implements IMst_Payment_TermEntity.GetListPaymentDay
            Return objPaymentTerm.DB_GetListPaymentDay
        End Function

        Public Function GetPaymentList(ByVal strPayment As String) As System.Collections.Generic.List(Of IMst_Payment_TermEntity) Implements IMst_Payment_TermEntity.GetPaymentList
            Return objPaymentTerm.GetPaymentList(strPayment)
        End Function

        Public Function CountUsedInPO(ByVal intPaymentTermID As Integer) As Integer Implements IMst_Payment_TermEntity.CountUsedInPO
            Return objPaymentTerm.CountUsedInPO(intPaymentTermID)
        End Function

        Public Function CountUsedInPO2(ByVal intPaymentTermID As Integer) As Integer Implements IMst_Payment_TermEntity.CountUsedInPO2
            Return objPaymentTerm.CountUsedInPO2(intPaymentTermID)
        End Function

        Public Function DeletePaymentTerm(ByVal intPaymentTermID As Integer) As Integer Implements IMst_Payment_TermEntity.DeletePaymentTerm
            Return objPaymentTerm.DeletePaymentTerm(intPaymentTermID)
        End Function

        Public Function GetPaymentTermByID(ByVal intPaymentTermID As Integer) As IMst_Payment_TermEntity Implements IMst_Payment_TermEntity.GetPaymentTermByID
            Return objPaymentTerm.GetPaymentTermByID(intPaymentTermID)
        End Function

        Public Function InsertPaymentTerm(ByVal objPaymentTermEntity As IMst_Payment_TermEntity) As Integer Implements IMst_Payment_TermEntity.InsertPaymentTerm
            Return objPaymentTerm.InsertPaymentTerm(objPaymentTermEntity)
        End Function

        Public Function UpdatePaymentTerm(ByVal objPaymentTermEntity As IMst_Payment_TermEntity) As Integer Implements IMst_Payment_TermEntity.UpdatePaymentTerm
            Return objPaymentTerm.UpdatePaymentTerm(objPaymentTermEntity)
        End Function

        Public Function CheckDupInsert(ByVal intPayment As String) As Integer Implements IMst_Payment_TermEntity.CheckDupInsert
            Return objPaymentTerm.CheckDupInsert(intPayment)
        End Function

        Public Function CheckDupUpdate(ByVal intPaymentTermID As String, ByVal intPayment As String) As Integer Implements IMst_Payment_TermEntity.CheckDupUpdate
            Return objPaymentTerm.CheckDupUpdate(intPaymentTermID, intPayment)
        End Function

        

#End Region

#Region "Property"
        Public Property created_by() As Integer Implements IMst_Payment_TermEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IMst_Payment_TermEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IMst_Payment_TermEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property id() As Integer Implements IMst_Payment_TermEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property term_day() As String Implements IMst_Payment_TermEntity.term_day
            Get
                Return _term_day
            End Get
            Set(ByVal value As String)
                _term_day = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IMst_Payment_TermEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As String Implements IMst_Payment_TermEntity.updated_date
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

