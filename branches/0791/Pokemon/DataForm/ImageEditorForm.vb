Imports System.IO
Imports PokemonBattle.PokemonData.Custom
Friend Class ImageEditorForm
    Private pm As CustomPokemonData
    Private customData As CustomGameData

    Public Sub New(ByVal pmValue As CustomPokemonData, ByVal data As CustomGameData)

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Icon = My.Resources.PokemonBall
        pm = pmValue
        customData = data
        picFront.Image = customData.GetImage(pm.FrontImage)
        picFront2.Image = customData.GetImage(pm.FrontImageF)

        picFrame.Image = customData.GetImage(pm.Frame)
        picFrame2.Image = customData.GetImage(pm.FrameF)

        picBack.Image = customData.GetImage(pm.BackImage)
        picBack2.Image = customData.GetImage(pm.BackImageF)

        picIcon.Image = customData.GetImage(pm.Icon)
    End Sub

    Private Sub btnBack2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack2.Click
        Dim position As Integer = GetImage()
        If position <> -1 Then
            pm.BackImageF = position
            picBack2.Image = customData.GetImage(pm.BackImageF)
        End If
    End Sub
    Private Sub btnFrame2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFrame2.Click
        Dim position As Integer = GetImage()
        If position <> -1 Then
            pm.FrameF = position
            picFrame2.Image = customData.GetImage(pm.FrameF)
        End If
    End Sub
    Private Sub btnFront2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFront2.Click
        Dim position As Integer = GetImage()
        If position <> -1 Then
            pm.FrontImageF = position
            picFront2.Image = customData.GetImage(pm.FrontImageF)
        End If
    End Sub
    Private Sub btnIcon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIcon.Click
        Dim position As Integer = GetImage()
        If position <> -1 Then
            pm.Icon = position
            picIcon.Image = customData.GetImage(pm.Icon)
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim position As Integer = GetImage()
        If position <> -1 Then
            pm.BackImage = position
            picBack.Image = customData.GetImage(pm.BackImage)
        End If
    End Sub
    Private Sub btnFrame_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFrame.Click
        Dim position As Integer = GetImage()
        If position <> -1 Then
            pm.Frame = position
            picFrame.Image = customData.GetImage(pm.Frame)
        End If
    End Sub
    Private Sub btnFront_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFront.Click
        Dim position As Integer = GetImage()
        If position <> -1 Then
            pm.FrontImage = position
            picFront.Image = customData.GetImage(pm.FrontImage)
        End If
    End Sub
    Private Function GetImage() As Integer
        Dim index As Integer = GetSelection(customData.Images.ToArray, "请选择图片(图标大小为32*32,其他图象大小为80*80)", _
            New Size(50, 50), "设置精灵图象")
        Return index
    End Function
End Class