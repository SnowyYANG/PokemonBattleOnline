Imports System.Runtime.Serialization.Formatters.Binary
Imports PokemonBattle.BattleNetwork
Imports NetworkLib.Utilities
Public Class BattleReplay
    Private _teams As Dictionary(Of Byte, TeamData)
    Private _players As Dictionary(Of Byte, String)
    Private _randomSeed As Integer
    Private _battleMode As BattleMode
    Private _rules As List(Of BattleRule)

    Private _turns As List(Of List(Of PlayerMove))
    Private _turnCount As Integer = -1

    Public Sub New(ByVal teamValue As Dictionary(Of Byte, TeamData), ByVal seedValue As Integer, _
        ByVal playerName As Dictionary(Of Byte, String), ByVal mode As BattleMode, ByVal rules As List(Of BattleRule))
        _teams = teamValue
        _randomSeed = seedValue
        _players = playerName
        _battleMode = mode
        _rules = rules

        _turns = New List(Of List(Of PlayerMove))
    End Sub

    Public Sub AddMove(ByVal moves As List(Of PlayerMove))
        _turns.Add(New List(Of PlayerMove)(moves))
    End Sub

    Public Sub SaveReplay()
        If _turns.Count = 0 Then Return
        Dim savePath As String = ""
        If My.Settings.AutoSaveReplay OrElse MessageBox.Show("是否保存战斗录象?", "提示", _
            MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
            If My.Settings.RepTitle <> "" AndAlso My.Settings.RepPath <> "" Then
                Dim savePath2 As String = Path.Combine(My.Settings.RepPath, ImgTxt.GetFileName(My.Settings.LogTitle, _players) & ".pbr")
                Dim i As Integer = 1
                savePath = savePath2
                Do While File.Exists(savePath)
                    i += 1
                    savePath = savePath2.Insert(savePath2.Length - 4, "_" & i)
                Loop
            Else
                Dim dialog As New SaveFileDialog()
                dialog.Filter = "战斗录象(*.pbr)|*.pbr"
                Dim saveResult As DialogResult = dialog.ShowDialog()
                If (saveResult = Windows.Forms.DialogResult.OK Or saveResult = Windows.Forms.DialogResult.Yes) Then
                    savePath = dialog.FileName
                End If
            End If
        End If
        If savePath <> "" Then
            Dim stream As FileStream = New FileStream(savePath, FileMode.Create)
            Dim formatter As New BinaryFormatter

            Dim writer As New BinaryWriter(stream)

            Try
                writer.Write(_battleMode)
                writer.Write(_randomSeed)

                formatter.Serialize(stream, _players)
                formatter.Serialize(stream, _rules)

                writer.Write(_teams.Count)
                For Each pair As KeyValuePair(Of Byte, TeamData) In _teams
                    writer.Write(pair.Key)
                    pair.Value.Save(stream)
                Next

                writer.Write(_turns.Count)
                For Each moves As List(Of PlayerMove) In _turns
                    writer.Write(moves.Count)
                    For Each move As PlayerMove In moves
                        writer.Write(move.Pokemon)
                        writer.Write(move.Move)
                        writer.Write(move.MoveIndex)
                        writer.Write(move.Target)
                    Next
                Next
            Catch ex As Exception
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                writer.Close()
                stream.Close()
            End Try
        End If
    End Sub

    Public Shared Function FromFile(ByVal path As String) As BattleReplay
        Dim replay As BattleReplay

        Dim filename As FileStream = New FileStream(path, FileMode.Open)
        Try
            Dim reader As New BinaryReader(filename)

            Dim formatter As New BinaryFormatter

            Dim mode As BattleMode = CType(reader.ReadInt32, BattleMode)
            Dim seed As Integer = reader.ReadInt32

            Dim players As Dictionary(Of Byte, String) = CType(formatter.Deserialize(filename), Dictionary(Of Byte, String))
            Dim rules As List(Of BattleRule) = CType(formatter.Deserialize(filename), List(Of BattleRule))

            Dim teams As New Dictionary(Of Byte, TeamData)
            Dim teamCount As Integer = reader.ReadInt32()
            For i As Integer = 1 To teamCount
                Dim key As Byte = reader.ReadByte()
                Dim team As TeamData = TeamData.FormStream(filename)
                teams.Add(key, team)
            Next

            Dim turns As New List(Of List(Of PlayerMove))
            Dim turnCount As Integer = reader.ReadInt32()
            For i As Integer = 1 To turnCount
                Dim moves As New List(Of PlayerMove)
                Dim moveCount As Integer = reader.ReadInt32()
                For j As Integer = 1 To moveCount
                    Dim move As New PlayerMove
                    move.Pokemon = CType(reader.ReadInt32(), PokemonIndex)
                    move.Move = CType(reader.ReadInt32(), BattleMove)
                    move.MoveIndex = reader.ReadByte
                    move.Target = CType(reader.ReadInt32(), TargetIndex)
                    moves.Add(move)
                Next
                turns.Add(moves)
            Next

            replay = New BattleReplay(teams, seed, players, mode, rules)
            replay._turns = turns
            Return replay
        Catch ex As Exception
            MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            filename.Close()
        End Try
        Return Nothing
    End Function

    Public Function StartReplay(ByVal txt As ImgTxt) As Battle
        Dim newBattle As Battle = New Battle(txt, _battleMode)
        For Each key As Byte In _players.Keys
            newBattle.SetPlayer(key, _players(key))
        Next
        newBattle.SetTeams(_teams)
        newBattle.SetRules(_rules)
        newBattle.SetSeed(_randomSeed)
        newBattle.InitializeBattle()
        Reset()
        Return newBattle
    End Function

    Public Function NextTurn() As List(Of PlayerMove)
        _turnCount += 1
        If _turnCount <> _turns.Count Then
            Return _turns(_turnCount)
        End If
        Return Nothing
    End Function

    Public Sub Reset()
        _turnCount = -1
    End Sub
End Class
