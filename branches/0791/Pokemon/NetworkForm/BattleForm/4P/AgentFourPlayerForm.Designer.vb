<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgentFourPlayerForm
    Inherits PokemonBattleOnline.FourPlayerClientForm

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
        '
        'StartButton
        '
        Me.StartButton = New System.Windows.Forms.Button
        Me.StartButton.Enabled = False
        Me.StartButton.Location = New System.Drawing.Point(187, 148)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(75, 23)
        Me.StartButton.TabIndex = 4
        Me.StartButton.Text = "开始对战"
        Me.StartButton.UseVisualStyleBackColor = True

        Me.Controls.Add(Me.StartButton)
    End Sub

    Friend WithEvents StartButton As System.Windows.Forms.Button
End Class
