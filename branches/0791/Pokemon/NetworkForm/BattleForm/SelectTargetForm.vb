Imports System.Windows.Forms
Imports PokemonBattle.BattleNetwork
Friend Class SelectTargetForm

    Private myTeam, opponentTeam As Team
    Private mTarget As TargetIndex

    Private opponent1 As New Rectangle(4, 12, 113, 65)
    Private opponent2 As New Rectangle(140, 12, 113, 65)
    Private teamFriend As Rectangle
    Private self As Rectangle

    Public ReadOnly Property Target() As TargetIndex
        Get
            Return mTarget
        End Get
    End Property

    Public Sub New(ByVal myTeamValue As Team, ByVal opponentTeamValue As Team, ByVal user As Pokemon, ByVal selfEff As Boolean)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        myTeam = myTeamValue
        opponentTeam = opponentTeamValue
        If user Is myTeamValue.SelectedPokemon(0) Then
            If selfEff Then self = New Rectangle(4, 93, 112, 47)
            If myTeamValue.SelectedPokemon(1).HP <> 0 Then teamFriend = New Rectangle(140, 93, 112, 47)
        Else
            If selfEff Then self = New Rectangle(140, 93, 112, 47)
            If myTeamValue.SelectedPokemon(0).HP <> 0 Then teamFriend = New Rectangle(4, 93, 112, 47)
        End If
    End Sub

    Private Sub FrmTarget_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim img As New Bitmap(My.Resources.SelectTarget)
        Using graph As Graphics = Graphics.FromImage(img)
            For i As Integer = 0 To 1
                If myTeam.SelectedPokemon(i).HP <> 0 Then
                    graph.DrawString(myTeam.SelectedPokemon(i).Nickname, NameFont, Brushes.Black, MyPoint(i, 0))
                    If myTeam.SelectedPokemon(i).Gender = PokemonGender.Male Then
                        graph.DrawImageUnscaled(My.Resources.male, MyPoint(i, 1))
                    ElseIf myTeam.SelectedPokemon(i).Gender = PokemonGender.Female Then
                        graph.DrawImageUnscaled(My.Resources.female, MyPoint(i, 1))
                    End If
                End If
                If opponentTeam.SelectedPokemon(i).HP <> 0 Then
                    graph.DrawImageUnscaled(opponentTeam.SelectedPokemon(i).Icon, OpponentPoint(i, 1))
                    graph.DrawString(opponentTeam.SelectedPokemon(i).Nickname, NameFont, Brushes.Black, OpponentPoint(i, 0))
                    If opponentTeam.SelectedPokemon(i).Gender = PokemonGender.Male Then
                        graph.DrawImageUnscaled(My.Resources.male, OpponentPoint(i, 2))
                    ElseIf opponentTeam.SelectedPokemon(i).Gender = PokemonGender.Female Then
                        graph.DrawImageUnscaled(My.Resources.female, OpponentPoint(i, 2))
                    End If
                End If
            Next
        End Using
        BackgroundImage = img
    End Sub

    Private Sub FrmTarget_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then

            Dim cancel As New Rectangle(10, 157, 236, 35)
            If cancel.Contains(e.Location) Then
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Close()
            ElseIf self.Contains(e.Location) Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                mTarget = TargetIndex.Self
            ElseIf opponent1.Contains(e.Location) Then
                If opponentTeam.SelectedPokemon(0).HP = 0 Then Return
                Me.DialogResult = Windows.Forms.DialogResult.OK
                mTarget = TargetIndex.Opponent1
            ElseIf opponent2.Contains(e.Location) Then
                If opponentTeam.SelectedPokemon(1).HP = 0 Then Return
                Me.DialogResult = Windows.Forms.DialogResult.OK
                mTarget = TargetIndex.Opponent2
            ElseIf teamFriend.Contains(e.Location) Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                mTarget = TargetIndex.TeamFriend
            End If
        End If
    End Sub

    Private Shared OpponentPoint As Point(,)
    Private Shared MyPoint As Point(,)
    Private Shared NameFont As New Font("宋体", 16, FontStyle.Regular, GraphicsUnit.Pixel)
    Shared Sub New()
        OpponentPoint = New Point(,) { _
            {New Point(13, 56), New Point(43, 21), New Point(98, 61)}, _
            {New Point(150, 56), New Point(180, 21), New Point(235, 61)}}
        MyPoint = New Point(,) { _
            {New Point(13, 113), New Point(98, 118)}, _
            {New Point(149, 113), New Point(234, 118)}}
    End Sub
End Class
