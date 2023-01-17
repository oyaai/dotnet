#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_SpecialJobOrderEntity
'	Class Discription	: Class of table mst_SpecialJobOrder
'	Create User 		: Suwishaya L.
'	Create Date		    : 07-06-2013
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
    Public Class ImpMst_SpecialJobOrderEntity
        Implements IMst_SpecialJobOrderEntiy

        Private _id As Integer
        Private _job_order As String
        Private _remark As String
        Private _created_by As String
        Private _created_date As Integer
        Private _updated_by As String
        Private _updated_date As Integer
        Private _delete_fg As Integer

        Private objSpecialJobOrder As New Dao.ImpMst_SpecialJobOrderDao


#Region "Function"
        Public Function CountUsedInSpecialJobOrder(ByVal strJobOrderID As String) As Integer Implements IMst_SpecialJobOrderEntiy.CountUsedInSpecialJobOrder
            Return objSpecialJobOrder.CountUsedInSpecialJobOrder(strJobOrderID)
        End Function

        Public Function CountUsedInReceiveDetail(ByVal strJobOrderID As String) As Integer Implements IMst_SpecialJobOrderEntiy.CountUsedInReceiveDetail
            Return objSpecialJobOrder.CountUsedInReceiveDetail(strJobOrderID)
        End Function

        Public Function DeleteSpecialJobOrder(ByVal intJobOrderID As Integer) As Integer Implements IMst_SpecialJobOrderEntiy.DeleteSpecialJobOrder
            Return objSpecialJobOrder.DeleteSpecialJobOrder(intJobOrderID)
        End Function

        Public Function GetSpecialJobOrderByID(ByVal intJobOrderID As Integer) As IMst_SpecialJobOrderEntiy Implements IMst_SpecialJobOrderEntiy.GetSpecialJobOrderByID
            Return objSpecialJobOrder.GetSpecialJobOrderByID(intJobOrderID)
        End Function

        Public Function GetSpecialJobOrderList(ByVal strJobOrderFrom As String, ByVal strJobOrderTo As String) As System.Collections.Generic.List(Of ImpMst_SpecialJobOrderEntity) Implements IMst_SpecialJobOrderEntiy.GetSpecialJobOrderList
            Return objSpecialJobOrder.GetSpecialJobOrderList(strJobOrderFrom, strJobOrderTo)
        End Function

        Public Function InsertSpecialJobOrder(ByVal objSpecialJobOrderEnt As IMst_SpecialJobOrderEntiy) As Integer Implements IMst_SpecialJobOrderEntiy.InsertSpecialJobOrder
            Return objSpecialJobOrder.InsertSpecialJobOrder(objSpecialJobOrderEnt)
        End Function

        Public Function UpdateSpecialJobOrder(ByVal objSpecialJobOrderEnt As IMst_SpecialJobOrderEntiy) As Integer Implements IMst_SpecialJobOrderEntiy.UpdateSpecialJobOrder
            Return objSpecialJobOrder.UpdateSpecialJobOrder(objSpecialJobOrderEnt)
        End Function

        Public Function CheckDupSpecialJobOrder(ByVal intJobOrderID As Integer, ByVal strJobOrder As String) As Integer Implements IMst_SpecialJobOrderEntiy.CheckDupSpecialJobOrder
            Return objSpecialJobOrder.CheckDupSpecialJobOrder(intJobOrderID, strJobOrder)
        End Function

        Public Function CheckDupJobOrder(ByVal strJobOrder As String) As Integer Implements IMst_SpecialJobOrderEntiy.CheckDupJobOrder
            Return objSpecialJobOrder.CheckDupJobOrder(strJobOrder)
        End Function
#End Region

#Region "Property"

        Public Property created_by() As String Implements IMst_SpecialJobOrderEntiy.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As String)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As Integer Implements IMst_SpecialJobOrderEntiy.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As Integer)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IMst_SpecialJobOrderEntiy.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property id() As Integer Implements IMst_SpecialJobOrderEntiy.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property job_order() As String Implements IMst_SpecialJobOrderEntiy.job_order
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Public Property remark() As String Implements IMst_SpecialJobOrderEntiy.remark
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
            End Set
        End Property

        Public Property updated_by() As String Implements IMst_SpecialJobOrderEntiy.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As String)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As Integer Implements IMst_SpecialJobOrderEntiy.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As Integer)
                _updated_date = value
            End Set
        End Property

#End Region

    End Class
End Namespace
