Imports PokemonBattle.BattleNetwork
Public Class ScreenUpPainter
    Implements IDisposable

    Private WithEvents myTeam As Team
    Private WithEvents opponentTeam As Team
    Private imgTxt As ImgTxt
    Private terrainImage As Bitmap

    Private myPokemonState As List(Of Image)
    Private opponentPokemonState As List(Of Image)

    Private Image As Bitmap
    Private imageSync As Object
    Private picScreen As PictureBox
    Private WithEvents picText As PictureBox

    Public Sub New(ByVal picturebox1 As PictureBox, ByVal picturebox2 As PictureBox, _
        ByVal terrain As BattleTerrain, ByVal imgTxtValue As ImgTxt, _
        ByVal teamValue As Team, ByVal targetTeamValue As Team, ByVal mode As BattleMode)
        picScreen = picturebox1
        picText = picturebox2
        terrainImage = GetTerrainImage(terrain)
        imgTxt = imgTxtValue
        myTeam = teamValue
        opponentTeam = targetTeamValue
        myPokemonState = New List(Of Image)
        opponentPokemonState = New List(Of Image)
        UpdateRects(mode)

        For i As Integer = 1 To myTeam.SelectedPokemon.Count
            myPokemonState.Add(Nothing)
            UpdatePokemonState(i)
            opponentPokemonState.Add(Nothing)
            UpdatePokemonState2(i)
        Next
        picScreen.Image = New Bitmap(256, 144)
        Image = CType(picScreen.Image, Bitmap)
        BindImgTxtImage()
        imageSync = New Object
    End Sub

    Public Sub Reset()
        picScreen.Image = New Bitmap(256, 144)
        Image = CType(picScreen.Image, Bitmap)
        Paint()
    End Sub
    Public Sub Paint()
        Draw(terrainImage, New Rectangle(0, 0, 256, 144))

        For i As Integer = 1 To myPokemonState.Count
            DrawMyPokemonState(i)
            DrawOpponentPokemonState(i)
            For i2 As Integer = 0 To myTeam.SelectedPokemon.Count - 1
                Dim pm As Pokemon = myTeam.SelectedPokemon(i2)
                If pm.HP <> 0 AndAlso Not pm.BattleState.Hide Then DrawAndClipped(pm.BackImage, MyPokemonRects(i2))
            Next

            For i3 As Integer = 0 To opponentTeam.SelectedPokemon.Count - 1
                Dim pm As Pokemon = opponentTeam.SelectedPokemon(i3)
                If pm.HP <> 0 AndAlso Not pm.BattleState.Hide Then
                    DrawAndClipped(pm.FrontImage, OpponentPokemonRects(i3))
                End If
            Next
        Next
        Update()
    End Sub
    Public Sub BindImgTxtImage()
        If picText.InvokeRequired Then
            picText.Invoke(New WorkDelegate(AddressOf BindImgTxtImage))
        Else
            picText.Image = imgTxt.Image
            picText.Refresh()
        End If
    End Sub
    Public Sub PaintImgTxt()
        If picText.InvokeRequired Then
            picText.Invoke(New WorkDelegate(AddressOf PaintImgTxt))
        Else
            picText.Refresh()
        End If
    End Sub

    Private Sub UpdatePokemonState(ByVal index As Integer)
        If MyPokemonStateRects.Count = 1 AndAlso index = 1 Then
            myPokemonState(index - 1) = GetPMStateImg(1, myTeam.SelectedPokemon(index - 1), True)
        Else
            myPokemonState(index - 1) = GetPMStateImg(1, myTeam.SelectedPokemon(index - 1))
        End If
    End Sub
    Private Sub RedrawMyPokemon(Optional ByVal drawDiedIndex As Integer = 0)
        Clear(MyPokemonRects)
        DrawMyPokemonWithoutClear(drawDiedIndex)
    End Sub
    Private Sub DrawMyPokemonWithoutClear(Optional ByVal drawDiedIndex As Integer = 0)
        For i As Integer = 0 To myTeam.SelectedPokemon.Count - 1
            Dim pm As Pokemon = myTeam.SelectedPokemon(i)
            If drawDiedIndex = i + 1 Then
                DrawAndClipped(pm.BackImage, MyPokemonRects(i))
            ElseIf pm.HP <> 0 AndAlso Not pm.BattleState.Hide Then
                DrawAndClipped(pm.BackImage, MyPokemonRects(i))
            End If
        Next
        Update()
    End Sub
    Private Sub DrawMyPokemonState(ByVal index As Integer)
        If myPokemonState(index - 1) IsNot Nothing Then _
            Draw(myPokemonState(index - 1), MyPokemonStateRects(index - 1))
    End Sub

    Private Sub UpdatePokemonState2(ByVal index As Integer)
        If OpponentPokemonStateRects.Count = 1 AndAlso index = 1 Then
            opponentPokemonState(index - 1) = GetPMStateImg(2, opponentTeam.SelectedPokemon(index - 1), True)
        Else
            opponentPokemonState(index - 1) = GetPMStateImg(2, opponentTeam.SelectedPokemon(index - 1))
        End If
    End Sub
    Private Sub RedrawOpponentPokemon(Optional ByVal drawDiedIndex As Integer = 0, Optional ByVal drawFrameIndex As Integer = 0)
        Clear(OpponentPokemonRects)
        DrawOpponentPokemonWithoutClear(drawDiedIndex, drawFrameIndex)
    End Sub
    Private Sub DrawOpponentPokemonWithoutClear(Optional ByVal drawDiedIndex As Integer = 0, Optional ByVal drawFrameIndex As Integer = 0)
        For i As Integer = 0 To opponentTeam.SelectedPokemon.Count - 1
            Dim pm As Pokemon = opponentTeam.SelectedPokemon(i)
            If drawDiedIndex = i + 1 Then
                DrawAndClipped(pm.FrontImage, OpponentPokemonRects(i))
            ElseIf drawFrameIndex = i + 1 Then
                DrawAndClipped(pm.Frame, OpponentPokemonRects(i))
            ElseIf pm.HP <> 0 AndAlso Not pm.BattleState.Hide Then
                DrawAndClipped(pm.FrontImage, OpponentPokemonRects(i))
            End If
        Next
        Update()
    End Sub
    Private Sub DrawOpponentPokemonState(ByVal index As Integer)
        If opponentPokemonState(index - 1) IsNot Nothing Then _
            Draw(opponentPokemonState(index - 1), OpponentPokemonStateRects(index - 1))
    End Sub

    Private Sub picturebox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) _
        Handles picText.MouseClick, picText.MouseDoubleClick
        If Not e.Button = MouseButtons.Left Then Return
        imgTxt.OnClick(e.Location)
    End Sub

    Private Sub myTeam_PokemonChanged(ByVal pokemon As Object, ByVal e As System.EventArgs) Handles myTeam.PokemonChanged
        Dim index As Integer = GetPokemonIndex(CType(pokemon, PokemonBattleOnline.Pokemon))
        UpdatePokemonState(index)
        DrawMyPokemonState(index)
        Dim pm As Pokemon = CType(pokemon, Pokemon)

        Clear(MyPokemonRects(index - 1))
        If Not pm.BattleState.substituted Then
            Dim tmpRect As Rectangle = MyPokemonRects(index - 1)
            For i As Integer = 1 To 4
                If i Mod 2 = 1 Then
                    MyPokemonRects(index - 1) = New Rectangle( _
                        New Point(tmpRect.X - 3, tmpRect.Y), New Size(80, 80))
                Else
                    MyPokemonRects(index - 1) = New Rectangle( _
                        New Point(tmpRect.X + 3, tmpRect.Y), New Size(80, 80))
                End If
                DrawMyPokemonWithoutClear()
                Threading.Thread.Sleep(150)
                Clear(MyPokemonRects(index - 1))
            Next
            MyPokemonRects(index - 1) = tmpRect
        End If
        RedrawMyPokemon()
    End Sub
    Private Sub myTeam_PokemonDied(ByVal Pokemon As Object, ByVal e As System.EventArgs) Handles myTeam.PokemonDied
        Dim index As Integer = GetPokemonIndex(CType(Pokemon, PokemonBattleOnline.Pokemon))
        Clear(MyPokemonStateRects(index - 1))
        If Not CType(Pokemon, PokemonBattleOnline.Pokemon).BattleState.Hide Then
            Dim tmpRect As Rectangle = MyPokemonRects(index - 1)
            For i As Integer = 1 To 10
                Clear(MyPokemonRects(index - 1))
                MyPokemonRects(index - 1) = New Rectangle( _
                    New Point(tmpRect.X, tmpRect.Y + 8 * i), New Size(80, 80 - 8 * i))
                DrawMyPokemonWithoutClear(index)
                Threading.Thread.Sleep(100)
            Next
            MyPokemonRects(index - 1) = tmpRect
        End If
        RedrawMyPokemon()
    End Sub
    Private Sub myTeam_PokemonStateChanged(ByVal pokemon As Object, ByVal e As System.EventArgs) _
        Handles myTeam.PokemonHPChanged, myTeam.PokemonStateChanged
        Dim index As Integer = GetPokemonIndex(CType(pokemon, PokemonBattleOnline.Pokemon))

        UpdatePokemonState(index)
        DrawMyPokemonState(index)
        Update()
    End Sub
    Private Sub myTeam_PokemonImageChanged(ByVal pokemon As Object, ByVal e As System.EventArgs) _
        Handles myTeam.PokemonImageChanged
        Dim index As Integer = GetPokemonIndex(CType(pokemon, PokemonBattleOnline.Pokemon))

        RedrawMyPokemon()
    End Sub

    Private Sub targetTeam_PokemonChanged(ByVal pokemon As Object, ByVal e As System.EventArgs) Handles opponentTeam.PokemonChanged
        Dim index As Integer = GetPokemonIndex(CType(pokemon, PokemonBattleOnline.Pokemon))
        UpdatePokemonState2(index)
        DrawOpponentPokemonState(index)
        Dim pm As Pokemon = CType(pokemon, PokemonBattleOnline.Pokemon)
        Clear(OpponentPokemonRects(index - 1))

        If Not pm.BattleState.substituted Then
            Dim tmpRect As Rectangle = OpponentPokemonRects(index - 1)
            For i As Integer = 1 To 4
                If i Mod 2 = 1 Then
                    OpponentPokemonRects(index - 1) = New Rectangle( _
                        New Point(tmpRect.X - 3, tmpRect.Y), New Size(80, 80))
                Else
                    OpponentPokemonRects(index - 1) = New Rectangle( _
                        New Point(tmpRect.X + 3, tmpRect.Y), New Size(80, 80))
                End If
                DrawOpponentPokemonWithoutClear(, index)
                Threading.Thread.Sleep(150)
                Clear(OpponentPokemonRects(index - 1))
            Next
            OpponentPokemonRects(index - 1) = tmpRect
        End If
        RedrawOpponentPokemon()
    End Sub
    Private Sub targetTeam_PokemonDied(ByVal pokemon As Object, ByVal e As System.EventArgs) Handles opponentTeam.PokemonDied
        Dim index As Integer = GetPokemonIndex(CType(pokemon, PokemonBattleOnline.Pokemon))
        Clear(OpponentPokemonStateRects(index - 1))
        If Not CType(pokemon, PokemonBattleOnline.Pokemon).BattleState.Hide Then
            Dim tmpRect As Rectangle = OpponentPokemonRects(index - 1)
            For i As Integer = 1 To 10
                Clear(OpponentPokemonRects(index - 1))
                OpponentPokemonRects(index - 1) = New Rectangle( _
                    New Point(tmpRect.X, tmpRect.Y + 8 * i), New Size(80, 80 - 8 * i))
                DrawOpponentPokemonWithoutClear(index)
                Threading.Thread.Sleep(100)
            Next
            OpponentPokemonRects(index - 1) = tmpRect
        End If
        RedrawOpponentPokemon()
    End Sub
    Private Sub targetTeam_PokemonStateChanged(ByVal pokemon As Object, ByVal e As System.EventArgs) _
        Handles opponentTeam.PokemonHPChanged, opponentTeam.PokemonStateChanged
        Dim index As Integer = GetPokemonIndex(CType(pokemon, PokemonBattleOnline.Pokemon))

        UpdatePokemonState2(index)
        DrawOpponentPokemonState(index)
        Update()
    End Sub
    Private Sub targetTeam_PokemonImageChanged(ByVal pokemon As Object, ByVal e As System.EventArgs) _
        Handles opponentTeam.PokemonImageChanged
        Dim index As Integer = GetPokemonIndex(CType(pokemon, PokemonBattleOnline.Pokemon))
        RedrawOpponentPokemon()
    End Sub

    Private Sub Clear(ByVal rects As List(Of Rectangle))
        rects.ForEach(AddressOf Clear)
    End Sub
    Private Sub Clear(ByVal rect As Rectangle)
        If rect.X < 0 Then rect.Width += rect.X : rect.X = 0
        If rect.X + rect.Width > 256 Then rect.Width = 256 - rect.X
        If rect.Y < 0 Then rect.Height += rect.Y : rect.Y = 0
        If rect.Y + rect.Height > 144 Then rect.Height = 144 - rect.Y
        Try
            Dim image As Image = CloneTerrain(rect)
            Draw(image, rect)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Draw(ByVal img As Image, ByVal rect As Rectangle)
        SyncLock imageSync
            Try
                Dim graph As Graphics = Graphics.FromImage(Image)
                Try
                    graph.DrawImageUnscaled(img, rect)
                Finally
                    graph.Dispose()
                End Try
            Catch ex As Exception
            End Try
        End SyncLock
    End Sub
    Private Sub DrawAndClipped(ByVal img As Image, ByVal rect As Rectangle)
        SyncLock imageSync
            Try
                Dim graph As Graphics = Graphics.FromImage(Image)
                Try
                    graph.DrawImageUnscaledAndClipped(img, rect)
                Catch ex As Exception
                Finally
                    graph.Dispose()
                End Try
            Catch ex As Exception
            End Try
        End SyncLock
    End Sub
    Private Sub Update()
        If picScreen.InvokeRequired Then
            picScreen.Invoke(New WorkDelegate(AddressOf Update))
        Else
            picScreen.Refresh()
        End If
    End Sub
    Private Function GetPokemonIndex(ByVal pm As Pokemon) As Integer
        For i As Integer = 1 To opponentTeam.SelectedPokemon.Count
            If pm Is myTeam.SelectedPokemon(i - 1) OrElse pm Is opponentTeam.SelectedPokemon(i - 1) Then Return i
        Next
    End Function

    Private shaking As Boolean
    Private shakeIndex As Integer
    Private tmpPoint As Point
    Private locker As Object = New Object
    Public Sub StartShake(ByVal pm As Pokemon)
        If shakeIndex <> 0 Then StopShake()
        ThreadPool.QueueUserWorkItem(AddressOf Shake, pm)
    End Sub
    Public Sub StopShake()
        If shakeIndex <> 0 Then
            shaking = False
        End If
    End Sub
    Private Sub Shake(ByVal pm As Object)
        SyncLock locker
            Dim pokemonIndex As Integer = myTeam.SelectedPokemon.IndexOf(CType(pm, Pokemon)) + 1
            shakeIndex = pokemonIndex
            tmpPoint = MyPokemonRects(shakeIndex - 1).Location
            shaking = True
            Do
                Clear(MyPokemonRects(shakeIndex - 1))
                If tmpPoint = MyPokemonRects(shakeIndex - 1).Location Then
                    MyPokemonRects(shakeIndex - 1) = New Rectangle(tmpPoint.X, tmpPoint.Y + 1, 80, 80)
                Else
                    MyPokemonRects(shakeIndex - 1) = New Rectangle(tmpPoint, New Size(80, 80))
                End If
                DrawMyPokemonWithoutClear()
                If Not shaking Then Exit Do
                Thread.Sleep(500)
            Loop While shaking
            MyPokemonRects(shakeIndex - 1) = New Rectangle(tmpPoint, New Size(80, 80))
            shakeIndex = 0
            RedrawMyPokemon()
        End SyncLock
    End Sub


    Private clearPool As Dictionary(Of Rectangle, Bitmap)
    Private Function CloneTerrain(ByVal rect As Rectangle) As Bitmap
        If clearPool Is Nothing Then clearPool = New Dictionary(Of Rectangle, Bitmap)
        If Not clearPool.ContainsKey(rect) Then
            Dim img As Bitmap = terrainImage.Clone(rect, terrainImage.PixelFormat)
            clearPool.Add(rect, img)
        End If
        Return clearPool(rect)
    End Function

    Private MyPokemonRects As List(Of Rectangle)
    Private MyPokemonStateRects As List(Of Rectangle)
    Private OpponentPokemonRects As List(Of Rectangle)
    Private OpponentPokemonStateRects As List(Of Rectangle)
    Private Sub UpdateRects(ByVal battleMode As BattleMode)
        MyPokemonRects = New List(Of Rectangle)
        MyPokemonStateRects = New List(Of Rectangle)
        OpponentPokemonRects = New List(Of Rectangle)
        OpponentPokemonStateRects = New List(Of Rectangle)
        If battleMode = battleMode.Single Then
            MyPokemonRects.Add(New Rectangle(20, 64, 80, 80))
            MyPokemonStateRects.Add(New Rectangle(136, 95, 120, 38))

            OpponentPokemonRects.Add(New Rectangle(155, 13, 80, 80))
            OpponentPokemonStateRects.Add(New Rectangle(-6, 27, 126, 30))
        Else
            MyPokemonRects.Add(New Rectangle(0, 64, 80, 80))
            MyPokemonRects.Add(New Rectangle(47, 64, 80, 80))
            MyPokemonStateRects.Add(New Rectangle(130, 87, 126, 30))
            MyPokemonStateRects.Add(New Rectangle(136, 116, 126, 30))

            OpponentPokemonRects.Add(New Rectangle(129, 3, 80, 80))
            OpponentPokemonRects.Add(New Rectangle(175, 7, 80, 80))
            OpponentPokemonStateRects.Add(New Rectangle(0, 0, 126, 30))
            OpponentPokemonStateRects.Add(New Rectangle(-6, 29, 126, 30))
        End If
    End Sub

    Private disposedValue As Boolean = False        ' 检测冗余的调用

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                StopShake()
                If clearPool IsNot Nothing Then clearPool.Clear()

            End If
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' Visual Basic 添加此代码是为了正确实现可处置模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean) 中。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
