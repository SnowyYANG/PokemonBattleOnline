Imports PokemonBattle.BattleNetwork
Imports System.Text.RegularExpressions
Friend Class DirectBattleForm

    Private motherForm As MainForm
    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall
    End Sub

    Private Sub DirectBattleForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        motherForm = CType(MdiParent, MainForm)
        ModeCombo.SelectedIndex = 0
    End Sub

    Private Sub ServerRadio_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ServerRadio.CheckedChanged
        If ServerRadio.Checked Then
            AddressText.ReadOnly = True
            RuleGroup.Enabled = True
        Else
            AddressText.ReadOnly = False
            RuleGroup.Enabled = False
        End If
    End Sub

    Private Sub ObseverRadio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ObseverRadio.CheckedChanged
        If ObseverRadio.Checked Then
            NameText.ReadOnly = True
            ModeCombo.Enabled = False
        Else
            NameText.ReadOnly = False
            ModeCombo.Enabled = True
        End If
    End Sub

    Private Function IsEmptyString(ByVal input As String) As Boolean
        Return Regex.IsMatch(input, "^\s*$")
    End Function

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        If NameText.ReadOnly = False AndAlso IsEmptyString(NameText.Text) Then Return
        If AddressText.ReadOnly = False AndAlso IsEmptyString(AddressText.Text) Then Return

        If ModeCombo.SelectedIndex <> 0 AndAlso motherForm.Team.Pokemons(1).Identity = 0 AndAlso Not ObseverRadio.Checked Then
            MessageBox.Show("2V2至少要有两只PM")
            Return
        End If
        If ServerRadio.Checked Then
            If ModeCombo.SelectedIndex = 2 Then
                Dim form As Form = motherForm.Build4PServerForm(NameText.Text, Rules)
                form.MdiParent = Me.MdiParent
                form.Show()
            Else
                Dim battleForm As Form = motherForm.BuildBattleServerForm(NameText.Text, BattleMode, Rules)
                battleForm.Show()
            End If
        ElseIf ClientRadio.Checked Then
            If ModeCombo.SelectedIndex = 2 Then
                Dim form As Form = motherForm.Build4PClientForm(AddressText.Text, NameText.Text)
                form.Show()
            Else
                Dim battleForm As Form = motherForm.BuildBattleClientForm(AddressText.Text, 2, NameText.Text, BattleMode)
                battleForm.Show()
            End If
        Else
            Dim form As Form = motherForm.BuildBattleObserverForm(AddressText.Text, 1)
            form.Show()
        End If
    End Sub

    Private ReadOnly Property BattleMode() As BattleMode
        Get
            If ModeCombo.SelectedIndex = 0 Then
                Return PokemonBattle.BattleNetwork.BattleMode.Single
            ElseIf ModeCombo.SelectedIndex = 1 Then
                Return PokemonBattle.BattleNetwork.BattleMode.Double
            ElseIf ModeCombo.SelectedIndex = 2 Then
                Return PokemonBattle.BattleNetwork.BattleMode.Double_4P
            End If
        End Get
    End Property
    Private ReadOnly Property Rules() As List(Of BattleRule)
        Get
            Dim ruleList As New List(Of BattleRule)()
            If PPUpCheck.Checked Then ruleList.Add(BattleRule.PPUp)
            If RandomCheck.Checked Then ruleList.Add(BattleRule.Random)
            Return ruleList
        End Get
    End Property

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Close()
    End Sub
End Class