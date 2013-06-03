Imports System.ComponentModel, System.IO
Imports System.Threading, System.Runtime.Serialization.Formatters.Binary
Imports PokemonBattle.BattleNetwork

Friend Class BattleReplayForm
    Private WithEvents imgtxt As New ImgTxt(Convert.ToInt32(My.Settings.LogLineCount))
    Private replay As BattleReplay
    Private myBattle As Battle
    Private Paused, Stoped As Boolean

    Private nextTurnMove As String

    Private gamedata As GameData

    Private Painter As ScreenUpPainter
    Public Sub New(ByVal data As GameData)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        gamedata = data
        Me.Icon = My.Resources.PokemonBall
        picScreen.Image = New Bitmap(256, 144)
        Dim logLine As Integer = Convert.ToInt32(My.Settings.LogLineCount)
        If logLine > 6 Then
            picText.Size = New Size(256, 7 + logLine * 13)
            Me.Size = New Size(291, 265 + logLine * 13)
        End If
    End Sub

    Private Sub cmdOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOpen.Click
        Dim file As New OpenFileDialog()
        file.Title = "打开录象"
        file.Multiselect = False
        file.Filter = "PBO战斗录象(*.pbr)|*.pbr"
        Dim result As DialogResult = file.ShowDialog()
        If result = Windows.Forms.DialogResult.OK AndAlso file.FileName <> "" Then
            replay = BattleReplay.FromFile(file.FileName)
            If replay IsNot Nothing Then
                cmdBegin.Enabled = True
            End If
        End If
    End Sub

    Private Sub cmdBegin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBegin.Click
        imgtxt.Clear()
        myBattle = replay.StartReplay(imgtxt)
        myBattle.StartBattle()
        Painter = New ScreenUpPainter(picScreen, picText, myBattle.ground.Terrain, imgtxt, myBattle.team1, myBattle.team2, myBattle.Mode)
        Painter.Paint()
        cmdBegin.Enabled = False
        cmdOpen.Enabled = False
        cmdPause.Enabled = True
        cmdStop.Enabled = True
        nextTurnMove = ""
        ThreadPool.QueueUserWorkItem(AddressOf Play)
    End Sub

    Private Sub Play(ByVal state As Object)
        Dim moves As List(Of PlayerMove) = replay.NextTurn()
        Do Until moves Is Nothing
            For Each move As PlayerMove In moves
                myBattle.SetMove(move)
            Next
            myBattle.NextResult()

            Thread.Sleep(300)
            If Stoped Then Exit Do
            If Paused Then Return
            moves = replay.NextTurn()
        Loop
        Paused = False
        Stoped = False
        myBattle = Nothing
        Stopped()
    End Sub

    Private Sub Pause()
        If InvokeRequired Then
            Invoke(New WorkDelegate(AddressOf Pause))
        Else
            cmdPause.Text = "继续(&C)"
            Focus()
        End If
    End Sub

    Private Sub Stopped()
        If InvokeRequired Then
            Invoke(New WorkDelegate(AddressOf Stopped))
        Else
            cmdPause.Enabled = False
            cmdStop.Enabled = False
            cmdBegin.Enabled = True
            cmdOpen.Enabled = True
        End If
    End Sub

    Private Sub cmdPause_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPause.Click
        If Paused Then
            Paused = False
            cmdPause.Text = "暂停(&P)"
            ThreadPool.QueueUserWorkItem(AddressOf Play)
        Else
            Pause()
            Paused = True
        End If
    End Sub

    Private Sub imgtxt_Roll(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgtxt.Roll
        If Painter IsNot Nothing Then Painter.PaintImgTxt()
    End Sub

    Private Sub imgtxt_UpdateScreen(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgtxt.UpdateScreen
        If Painter IsNot Nothing Then Painter.PaintImgTxt()
    End Sub

    Private Sub cmdStop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdStop.Click
        Stoped = True
        If Paused Then
            Stoped = False
            Paused = False
            cmdPause.Text = "暂停(&P)"
            Stopped()
            Me.Focus()
        End If
    End Sub

    Private Sub FrmBattleReplay_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Stoped = True
    End Sub

    Private Sub FrmBattleReplay_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.PageUp Then
            imgtxt.RollUp()
        ElseIf e.KeyCode = Keys.PageDown Then
            imgtxt.RollDown()
        End If
    End Sub

End Class