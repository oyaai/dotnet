#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_CurrencyEntity
'	Class Discription	: Class of table mst_currency
'	Create User 		: Boon
'	Create Date		    : 17-06-2013
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
    Public Class ImpMst_CurrencyEntity
        Implements IMst_CurrencyEntity

        Private _id As Integer
        Private _name As String
        Private _delete_fg As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _updated_by As Integer
        Private _updated_date As String

        Private objCurrencyDao As New Dao.ImpMst_CurrencyDao
        Private objCurrency As New Dao.ImpMst_CurrencyDao

#Region "Function"
        Public Function GetCurrencyForList() As System.Collections.Generic.List(Of IMst_CurrencyEntity) Implements IMst_CurrencyEntity.GetCurrencyForList
            Dim objCurrency As New Dao.ImpMst_CurrencyDao
            Return objCurrency.DB_GetCurrencyForList
        End Function

        Public Function GetCurrencyForDropdownList() As System.Collections.Generic.List(Of IMst_CurrencyEntity) Implements IMst_CurrencyEntity.GetCurrencyForDropdownList
            Return objCurrencyDao.GetCurrencyForDropdownList
        End Function

        Public Function GetCurrencyList(ByVal strCurrency As String) As System.Collections.Generic.List(Of IMst_CurrencyEntity) Implements IMst_CurrencyEntity.GetCurrencyList
            Return objCurrency.GetCurrencyList(strCurrency)
        End Function

        Public Function CountUsedInPO(ByVal intCurrencyID As Integer) As Integer Implements IMst_CurrencyEntity.CountUsedInPO
            Return objCurrency.CountUsedInPO(intCurrencyID)
        End Function

        Public Function CountUsedInPO2(ByVal intCurrencyID As Integer) As Integer Implements IMst_CurrencyEntity.CountUsedInPO2
            Return objCurrency.CountUsedInPO2(intCurrencyID)
        End Function

        Public Function DeleteCurrency(ByVal intCurrencyID As Integer) As Integer Implements IMst_CurrencyEntity.DeleteCurrency
            Return objCurrency.DeleteCurrency(intCurrencyID)
        End Function

        Public Function GetCurrencyByID(ByVal intCurrencyID As Integer) As IMst_CurrencyEntity Implements IMst_CurrencyEntity.GetCurrencyByID
            Return objCurrency.GetCurrencyByID(intCurrencyID)
        End Function

        Public Function InsertCurrency(ByVal objCurrencyEnt As IMst_CurrencyEntity) As Integer Implements IMst_CurrencyEntity.InsertCurrency
            Return objCurrency.InsertCurrency(objCurrencyEnt)
        End Function

        Public Function UpdateCurrency(ByVal objPaymentTermEntity As IMst_CurrencyEntity) As Integer Implements IMst_CurrencyEntity.UpdateCurrency
            Return objCurrency.UpdateCurrency(objPaymentTermEntity)
        End Function

        Public Function CheckDupCurrency(ByVal intCurrencyID As String, ByVal intCurrency As String) As Integer Implements IMst_CurrencyEntity.CheckDupCurrency
            Return objCurrency.CheckDupCurrency(intCurrencyID, intCurrency)
        End Function
#End Region
        

#Region "Property"
        Public Property created_by() As Integer Implements IMst_CurrencyEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IMst_CurrencyEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IMst_CurrencyEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property id() As Integer Implements IMst_CurrencyEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property name() As String Implements IMst_CurrencyEntity.name
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IMst_CurrencyEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As String Implements IMst_CurrencyEntity.updated_date
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

