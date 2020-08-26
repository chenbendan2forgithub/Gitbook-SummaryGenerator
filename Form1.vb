Imports System.IO
Public Class Form1
    Public dirpath As String
    Public pro As New Process
    Public InitReturn As Boolean = False
    Public FinalFiles As String = ""


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        End
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Label2.Text = "Running."
        FolderBrowserDialog1.ShowDialog()
        If FolderBrowserDialog1.SelectedPath = Nothing Then
            Exit Sub
        End If
        dirpath = FolderBrowserDialog1.SelectedPath
        If Not dirpath.Substring(dirpath.Length - 1) = "\" Then
            dirpath += "\"
        End If
        FolderBrowserDialog1.SelectedPath = Nothing
        If Not File.Exists(dirpath & "SUMMARY.md") Then
            Select Case MsgBox("该文件夹非Gitbook库，是否新建？", vbOKCancel)
                Case vbOK
                    File.Create(dirpath & "SUMMARY.md").Close()
                    File.Create(dirpath & "README.md").Close()
                Case vbCancel
                    Exit Sub
            End Select
        End If
        Call CreateSUMMARY(dirpath)
    End Sub
    Public Sub CreateSUMMARY(dir As String)
        Dim ListF As New List(Of String)
        For Each d In Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly)
            If Not InStr(d, "_book") = 0 Then
                Directory.Delete(dir & "_book", True)
                Exit For
            End If
        Next
        For Each k In Directory.GetFiles(dir).ToList
            ListF.Add(k)
        Next
        Dim Dirlist As List(Of String) = Directory.GetDirectories(dir, "*", SearchOption.AllDirectories).ToList
        For Each d In Dirlist
            For Each s In Directory.GetFiles(d)
                ListF.Add(s)
            Next
        Next
        For j = 0 To ListF.Count - 1
            If Not InStr(ListF(j), "README.md") = 0 Or Not InStr(ListF(j), "SUMMARY.md") = 0 Then
                ListF(j) = Nothing
                Continue For
            End If
            If InStr(ListF(j), ".md") = 0 Then
                ListF(j) = Nothing
            End If
        Next
        Dim listfnew As New List(Of String)
        For Each o In ListF
            If o = Nothing Then
                Continue For
            End If
            listfnew.Add(o)
        Next
        Dim Titles As New List(Of String)
        'get tit
        For Each u In listfnew
            Dim d() As String = File.ReadAllLines(u)
            Titles.Add(d(0))
        Next

        For i = 0 To listfnew.Count - 1
            listfnew(i) = listfnew(i).Replace(dir, "")
        Next
        Dim FinalLines As New List(Of String)

        For i = 0 To listfnew.Count - 1
            Dim tmpLine As New SumLine(Titles(i), listfnew(i))
            Dim tmptoLine As String = tmpLine.ToLine()
            If tmptoLine = Nothing Then
                MsgBox("失败")
            End If
            FinalLines.Add(tmptoLine)
        Next
        FinalFiles += "# Summary" & vbCrLf & vbCrLf
        For Each q In FinalLines
            FinalFiles += q & vbCrLf
        Next
        RichTextBox1.Text = FinalFiles
        Label2.Text = "Waiting for Confirm."
        Button3.Enabled = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim sw As New StreamWriter(dirpath & "SUMMARY.md", False)
        sw.WriteLine(FinalFiles)
        sw.Close()
        Label2.Text = "Finished."
        Button3.Enabled = False
    End Sub
End Class
