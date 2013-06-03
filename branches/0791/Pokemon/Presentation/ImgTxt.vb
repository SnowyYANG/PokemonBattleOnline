Imports System.Text.RegularExpressions
Public Class ImgTxt
    Private Shared TextFont As New Font("宋体", 12, FontStyle.Regular, GraphicsUnit.Pixel)

    Private UpRect As New Rectangle(241, 7, 9, 5)
    Private DownRect As Rectangle

    Private battleLog As List(Of String)
    Private mCountOfLine As Integer
    Private mFrame As Image
    Private mImage As Bitmap

    Private mLines As List(Of String)
    Private mColorsOfLine As List(Of Color)

    Private selLine() As Integer

    Public Event UpdateScreen As EventHandler
    Public Event Roll As EventHandler

    Private wait As Boolean
    Public lastText As String
    Public lastColor As Color

    Private Const waitTime As Integer = 180
    Public Sub New(ByVal lineCount As Integer, Optional ByVal waitAfterAdd As Boolean = True)
        mCountOfLine = lineCount
        ReDim selLine(mCountOfLine - 1)
        mFrame = GetFrame(mCountOfLine)
        mImage = New Bitmap(mFrame)
        battleLog = New List(Of String)
        mLines = New List(Of String)
        mColorsOfLine = New List(Of Color)
        DownRect = New Rectangle(241, lineCount * 13 - 6, 9, 5)
        wait = waitAfterAdd
    End Sub

    Public ReadOnly Property Image() As Image
        Get
            Return mImage
        End Get
    End Property

    Public Sub AddText(ByVal text As String, ByVal color As Color, Optional ByVal cutText As Boolean = False)
        If Not cutText Then AddLog(text) : lastText = text : lastColor = color

        If GetTextLength(text) <= 18 Then
            mLines.Add(text)
            mColorsOfLine.Add(color)
        Else
            Dim strs As String() = CutString(text, 18)
            mLines.Add(strs(0))
            mColorsOfLine.Add(color)
            AddText(strs(1), color, True)
        End If
        For i As Integer = 1 To selLine.Length
            selLine(selLine.Length - i) = mLines.Count - i
        Next

        Update()
        If Not cutText Then
            RaiseEvent UpdateScreen(Me, EventArgs.Empty)
            If wait Then Threading.Thread.Sleep(waitTime)
        End If
    End Sub
    Public Sub AddText(ByVal text As String)
        AddText(text, Color.Black)
    End Sub

    Public Sub AddLog(ByVal text As String)
        battleLog.Add(text)
    End Sub

    Public Sub Reset()
        mImage = New Bitmap(mFrame)
        Update()
    End Sub
    Private Sub Update()
        Try
            Dim graph As Graphics = Graphics.FromImage(Image)
            Try
                graph.FillRectangle(Brushes.White, 10, 4, 223, mCountOfLine * 13 - 1)
                Dim index As Integer = 0
                For i As Integer = 0 To selLine.Length - 1
                    If selLine(i) >= 0 Then
                        graph.DrawString(mLines(selLine(i)), TextFont, New SolidBrush(mColorsOfLine(selLine(i))), _
                            10, 4 + index * 13)
                        index += 1
                    End If
                Next
            Finally
                graph.Dispose()
            End Try
        Catch ex As Exception
        End Try
    End Sub

    Public Sub Clear()
        battleLog.Clear()
        mLines.Clear()
        mColorsOfLine.Clear()
        Using graph As Graphics = Graphics.FromImage(mImage)
            graph.FillRectangle(Brushes.White, 10, 4, 223, mCountOfLine * 13 - 1)
        End Using
    End Sub

    Public Sub OnClick(ByVal pt As Point)
        If UpRect.Contains(pt) Then
            RollUp()
        ElseIf DownRect.Contains(pt) Then
            RollDown()
        End If
    End Sub
    Public Sub RollDown()
        If selLine(selLine.Length - 1) < mLines.Count - 1 Then
            For i As Integer = 0 To selLine.Length - 1
                selLine(i) += 1
            Next
            Update()
            RaiseEvent Roll(Me, EventArgs.Empty)
        End If
    End Sub
    Public Sub RollUp()
        If selLine(0) > 0 Then
            For i As Integer = 0 To selLine.Length - 1
                selLine(i) -= 1
            Next
            Update()
            RaiseEvent Roll(Me, EventArgs.Empty)
        End If
    End Sub

    Private Shared Function GetFrame(Optional ByVal line As Integer = 6) As Bitmap
        If line = 6 Then
            Return My.Resources.Frame
        Else
            Dim image As New Bitmap(256, 7 + line * 13)
            Using graph As Graphics = Graphics.FromImage(image)
                graph.DrawImageUnscaled(CloneFrame(New Rectangle(0, 0, 256, 40)), 0, 0)
                For i As Integer = 1 To line - 6
                    Dim lineImg As Bitmap = CloneFrame(New Rectangle(0, 40, 256, 13))
                    graph.DrawImageUnscaled(lineImg, 0, 27 + i * 13)
                Next
                graph.DrawImageUnscaled(CloneFrame(New Rectangle(0, 40, 256, 45)), 0, image.Height - 45)
            End Using
            Return image
        End If
    End Function
    Private Shared Function CloneFrame(ByVal rect As Rectangle) As Bitmap
        Return My.Resources.Frame.Clone(rect, My.Resources.Frame.PixelFormat)
    End Function

    Public Sub SaveLog(ByVal players As Dictionary(Of Byte, String))
        If battleLog.Count = 0 Then Return
        Dim savePath As String = ""
        If My.Settings.AutoSaveLog OrElse MessageBox.Show("是否保存战报?", "提示", _
            MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
            If My.Settings.LogTitle <> "" AndAlso My.Settings.LogPath <> "" Then
                Dim savePath2 As String = Path.Combine(My.Settings.LogPath, GetFileName(My.Settings.LogTitle, players) & ".txt")
                Dim i As Integer = 1
                savePath = savePath2
                Do While File.Exists(savePath)
                    i += 1
                    savePath = savePath2.Insert(savePath2.Length - 4, "_" & i)
                Loop
            Else
                Dim dialog As New SaveFileDialog()
                dialog.Filter = "文本文档(*.txt)|*.txt"
                Dim saveResult As DialogResult = dialog.ShowDialog()
                If saveResult = Windows.Forms.DialogResult.OK OrElse saveResult = Windows.Forms.DialogResult.Yes Then
                    savePath = dialog.FileName
                End If
            End If
        End If
        If savePath <> "" Then
            Dim stream As FileStream = New FileStream(savePath, FileMode.Create)
            Dim writer As New StreamWriter(stream, System.Text.Encoding.Unicode)
            Try
                writer.WriteLine("PokemonBattleOnline " & My.Application.Info.Version.ToString)
                writer.WriteLine("战报:")
                For Each line As String In battleLog
                    writer.WriteLine(line)
                Next
            Catch ex As Exception
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                writer.Close()
                stream.Close()
            End Try
        End If
    End Sub
    Public Shared Function GetFileName(ByVal expression As String, ByVal players As Dictionary(Of Byte, String)) As String
        Dim fileName As String = expression
        If players.Count = 2 Then
            fileName = fileName.Replace("\Player1", players(1))
            fileName = fileName.Replace("\Player2", players(2))
        Else
            fileName = fileName.Replace("\Player1", players(1) & " & " & players(2))
            fileName = fileName.Replace("\Player2", players(3) & " & " & players(4))
        End If
        fileName.Replace("\Time", TimeString)
        fileName.Replace("\Date", DateString)
        Return fileName
    End Function

    Private Function GetTextLength(ByVal text As String) As Integer
        Dim half As New Regex("[a-zA-Z0-9().]")
        Dim matches As MatchCollection = half.Matches(text)
        Return Convert.ToInt32(text.Length - Math.Truncate(matches.Count / 2))
    End Function
    Private Function CutString(ByVal str As String, ByVal length As Integer) As String()
        Dim array As String() = {Nothing, Nothing}
        Dim str1 As New System.Text.StringBuilder(str.Substring(0, length))
        Dim index As Integer = length
        Do
            Dim len As Integer = GetTextLength(str1.ToString)
            If len = length Then
                Exit Do
            Else
                Dim count As Integer = length - len
                str1.Append(str.Substring(index, count))
                index = index + count
            End If
        Loop
        array(0) = str1.ToString
        array(1) = str.Substring(index)
        Return array
    End Function
End Class
