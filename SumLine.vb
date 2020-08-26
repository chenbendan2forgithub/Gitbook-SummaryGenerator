Public Class SumLine
    Public ReadOnly SumHeadLine As String = "# Summary" & vbCrLf & vbCrLf
    Private tit As String
    Private pah As String
    Public Sub New(Title As String, Path As String)
        tit = Title
        pah = Path

    End Sub

    Public Function ToLine() As String
        Dim blk As Integer = pah.Split("\").Length - 1
        If tit = "" Or pah = "" Then
            Return Nothing
        End If
        Dim Line As String = ""
        Select Case blk
            Case 0
                Line = "* "
            Case 1
                Line = "    * "
            Case 2
                Line = "        * "
            Case 3
                Line = "            * "
            Case 4
                Line = "                * "
        End Select
        Line += "[" & tit & "]" & "(" & pah & ")"
        Return Line
    End Function
End Class
