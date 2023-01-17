#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Common.Utilities
'	Class Name		    : EncryptDecrypt, Files, Paging, Message
'	Class Discription	: Utilities Class
'	Create User 		: Boon  Cr.P'Lin, Kob
'	Create Date		    : 15 Mar 2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
' INDEX FUNCTIONS
'   ' Execute Method by String
'   Shared Sub InvokeMethod(String) Return Method's Results
'   
'
'******************************************************************/
#End Region

#Region "Imports"
Imports System.Web.UI
Imports System.Web.Configuration
Imports System.Text
Imports System.Data
#End Region

Namespace Utilities
    Public Class Paging

        Private Clogs As Common.Logs.Log

        Private _pagedData As WebControls.PagedDataSource
        Private _mPageNo As Integer
        Private _PageSize As Integer
        Private _setTabPage As Integer

        Sub New()
            Clogs = New Common.Logs.Log
            _pagedData = New WebControls.PagedDataSource
            _mPageNo = 1
            _PageSize = CInt(WebConfigurationManager.AppSettings("PageSize"))
            _setTabPage = CInt(WebConfigurationManager.AppSettings("PageFooter")) 'Must get form web.config (key = PageFooter)
        End Sub

        '/**************************************************************
        '	Function name	: PageSize
        '	Discription		: PageSize
        '	Return Value	: Integer
        '	Create User		: Boon
        '	Create Date		: 17-06-2013
        '	Update User		:
        '	Update Date		:
        '**************************************************************/        
        Public ReadOnly Property PageSize As Integer
            Get
                Return _PageSize
            End Get
        End Property

        '/**************************************************************
        '	Function name	: PageFooter
        '	Discription		: PageFooter
        '	Return Value	: Integer
        '	Create User		: Boon
        '	Create Date		: 17-06-2013
        '	Update User		:
        '	Update Date		:
        '**************************************************************/        
        Public ReadOnly Property PageFooter As Integer
            Get
                Return _setTabPage
            End Get
        End Property


        '/**************************************************************
        '	Function name	: DoPaging(Integer, DataTable)
        '	Discription		: DoPaging
        '	Return Value	: PagedDataSource
        '	Create User		: Boon  Cr.P'Lin
        '	Create Date		: 14-03-2013
        '	Update User		:
        '	Update Date		:
        '**************************************************************/
        Public Function DoPaging(ByVal PageNo As Integer, ByVal objDataTable As DataTable) As WebControls.PagedDataSource
            Dim DT As DataTable = objDataTable

            DoPaging = _pagedData
            Try
                If Not IsNothing(DT) AndAlso DT.Rows.Count > 0 Then
                    'use QueryString values for mPageNo
                    If PageNo > 0 Then _mPageNo = CInt(PageNo)

                    'set the pagedData Properties
                    With _pagedData
                        .CurrentPageIndex = _mPageNo - 1
                        .DataSource = DT.DefaultView 'or Dataset
                        .AllowPaging = True
                        .PageSize = _PageSize
                    End With
                End If
                Return _pagedData
            Catch ex As Exception
                Clogs.ErrorLog("DoPaging", ex.Message.ToString)
            End Try
        End Function


#Region "Old Version"
        ''/**************************************************************
        ''	Function name	: DrawPaging(Integer, Integer)
        ''	Discription		: DrawPaging
        ''	Return Value	: String
        ''	Create User		: Boon  Cr.Kob
        ''	Create Date		: 18-03-2013
        ''	Update User		: Boonyarit
        ''	Update Date		: 21-05-2013
        ''**************************************************************/
        'Public Function DrawPaging(ByVal pageNumber As Integer, ByVal pageCount As Integer, Optional totalRow As Integer = 0) As String
        '    DrawPaging = String.Empty
        '    Dim sb As New StringBuilder
        '    Dim x, y, pageEnd, pageStart As Integer

        '    Try
        '        If totalRow > 0 AndAlso _PageSize > 0 Then
        '            pageCount = Math.Ceiling(CDec(totalRow) / CDec(_PageSize))
        '        End If
        '        If _setTabPage = 0 Then _setTabPage = 3
        '        If pageNumber = 0 Then pageNumber = _mPageNo
        '        If pageCount >= 1 Then

        '            'handle 10 at a time
        '            'get the start and end
        '            If pageNumber < _setTabPage Then '10, 20, 30 appear with old set
        '                pageStart = 1
        '            Else
        '                y = Math.Floor(_setTabPage / 2)

        '                If (pageNumber + y) <= pageCount Then
        '                    pageStart = pageNumber - y
        '                Else
        '                    pageStart = (pageCount - _setTabPage) + 1
        '                End If
        '            End If
        '        End If

        '        If pageStart + (_setTabPage - 1) > pageCount Then
        '            pageEnd = pageCount
        '        Else
        '            pageEnd = pageStart + (_setTabPage - 1)
        '        End If

        '        'draw the page numbers (current page is not a hyperlink it is in bold in square brackets)
        '        For x = pageStart To pageEnd
        '            If x = pageNumber Then
        '                sb.Append("&nbsp;&nbsp;<a herf='#' class='on'>")
        '                sb.Append(x)
        '                sb.Append("</a>")
        '            Else
        '                sb.Append("&nbsp;&nbsp;<a href=")
        '                sb.Append(DrawLink(x))
        '                sb.Append(">")
        '                sb.Append(x)
        '                sb.Append("</a>")
        '            End If
        '        Next

        '        Return sb.ToString
        '    Catch ex As Exception
        '        Clogs.ErrorLog("DrawPaging", ex.Message.ToString)
        '    End Try
        'End Function
#End Region
        '/**************************************************************
        '	Function name	: DrawPaging(Integer, Integer)
        '	Discription		: DrawPaging
        '	Return Value	: String
        '	Create User		: Boonyarit
        '	Create Date		: 21-05-2013
        '	Update User		: Boonyarit copy Kob,wall
        '	Update Date		: 09-08-2013
        '**************************************************************/
        Public Function DrawPaging(ByVal pageNumber As Integer, ByVal pageCount As Integer, ByVal flagPage As Integer, Optional ByVal totalRow As Integer = 0) As String
            DrawPaging = String.Empty
            Dim sb As New StringBuilder
            Dim x, y, pageEnd, pageStart As Integer

            Try
                If totalRow > 0 AndAlso _PageSize > 0 Then
                    pageCount = Math.Ceiling(CDec(totalRow) / CDec(_PageSize))
                End If
                If _setTabPage = 0 Then _setTabPage = 3
                If pageNumber = 0 Then pageNumber = _mPageNo
                If pageCount >= 1 Then

                    y = Math.Floor(_setTabPage / 2)

                    'handle 10 at a time
                    'get the start and end
                    If pageNumber < _setTabPage AndAlso (pageNumber + y) <= _setTabPage Then '10, 20, 30 appear with old set
                        pageStart = 1
                    ElseIf pageNumber < _setTabPage Then
                        pageStart = 1
                    Else
                        If (pageNumber + y) <= pageCount Then
                            pageStart = pageNumber - y
                        Else
                            pageStart = (pageCount - _setTabPage) + 1
                        End If
                    End If
                End If

                If pageStart + (_setTabPage - 1) > pageCount Then
                    pageEnd = pageCount
                Else
                    pageEnd = pageStart + (_setTabPage - 1)
                End If

                'draw the page numbers (current page is not a hyperlink it is in bold in square brackets)
                If pageStart > 1 Then
                    sb.Append("&nbsp;&nbsp;<a class='paging' href=")
                    sb.Append("?PageNo=1")
                    sb.Append(">")
                    sb.Append("First")
                    sb.Append("</a>")
                End If

                For x = pageStart To pageEnd
                    If x = pageNumber Then
                        sb.Append("&nbsp;&nbsp;<a herf='#' class='pagingOn'>")
                        sb.Append(x)
                        sb.Append("</a>")
                    Else
                        sb.Append("&nbsp;&nbsp;<a class='paging' href=")
                        sb.Append(DrawLink(x, flagPage))
                        sb.Append(">")
                        sb.Append(x)
                        sb.Append("</a>")
                    End If
                Next

                If pageCount > pageEnd Then
                    sb.Append("&nbsp;&nbsp;<a class='paging' href=")
                    sb.Append(DrawLink(pageCount, flagPage))
                    sb.Append(">")
                    sb.Append("Last")
                    sb.Append("</a>")
                End If

                Return sb.ToString
            Catch ex As Exception
                Clogs.ErrorLog("DrawPaging", ex.Message.ToString)
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DrawLink(Integer)
        '	Discription		: DrawLink
        '	Return Value	: String
        '	Create User		: Boon Cr.P'Lin
        '	Create Date		: 14-03-2013
        '	Update User		: Wall
        '	Update Date		: 09-08-2013
        '**************************************************************/
        Private Function DrawLink(ByVal pageNumber As Integer, ByVal flagPage As Integer) As String
            DrawLink = String.Empty
            Try
                Return "?PageNo=" & pageNumber & "&FlagPage=" & flagPage
            Catch ex As Exception
                Clogs.ErrorLog("DrawLink", ex.Message.ToString)
            End Try
        End Function

        '/**************************************************************
        '	Function name	: WriteDescription(intPageNo, intPageCount, intAllRecs)
        '	Discription		: Write Description
        '	Return Value	: String
        '	Create User		: Kob
        '	Create Date		: 06-06-2013
        '	Update User		:
        '	Update Date		:
        '**************************************************************/
        Public Function WriteDescription( _
            ByVal intPageNo As Integer, _
            ByVal intPageCount As Integer, _
            ByVal intAllRecs As Integer _
        ) As String
            WriteDescription = "&nbsp;"
            Try
                ' variable page size get from web.config
                Dim intPageSize As Integer = _PageSize
                Dim intStart As Integer
                Dim intEnd As Integer

                If intAllRecs = 0 Then
                    Exit Function
                End If

                ' check page no
                If intPageNo = 0 Then
                    intPageNo = 1
                End If

                ' set record start
                intStart = ((intPageNo - 1) * intPageSize) + 1

                ' set record end
                If intPageNo = intPageCount Then
                    intEnd = intAllRecs
                Else
                    intEnd = intPageNo * intPageSize
                End If

                ' set wording 
                WriteDescription = "Showing " & intStart.ToString & " to " & intEnd.ToString & _
                " of " & intAllRecs.ToString & " entries"
            Catch ex As Exception
                Clogs.ErrorLog("WriteDescription", ex.Message.ToString)
            End Try
        End Function
    End Class
End Namespace


