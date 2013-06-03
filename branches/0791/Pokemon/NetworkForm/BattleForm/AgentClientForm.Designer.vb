<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgentClientForm
    Inherits PokemonBattleOnline.BattleClientForm

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
        Me.SuspendLayout()
        Me.Name = "FrmBattleAgentClient"
        Me.Text = "FrmBattleAgentClient"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
End Class
