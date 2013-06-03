Imports System.Text.RegularExpressions
Imports System.Security.Cryptography
Imports PokemonBattle.BattleNetwork
Public Module ModShare

    Public Delegate Sub WorkDelegate()
    Public Delegate Function MessageDelegate(ByVal form As Form, ByVal message As String, ByVal caption As String, _
        ByVal buttons As MessageBoxButtons, ByVal icon As MessageBoxIcon) As DialogResult

    Public TypeIndex As String() = New String() {"普通", "格斗", "飞行", "毒", "地面", "岩", "虫", "鬼", "钢", _
            "火", "水", "草", "电", "超能", "冰", "龙", "恶"}

    Public SeparateChar As String() = New String() {Chr(20), Chr(21), Chr(22)}

    Public Const TAIPath As String = "TAI.pgd"

    Public Function SplitString(ByVal str As String, ByVal ParamArray separator As String()) As String()
        Return str.Split(separator, StringSplitOptions.RemoveEmptyEntries)
    End Function

    Public Function ShowInformation(ByVal form As Form, ByVal message As String, ByVal caption As String, _
        ByVal buttons As MessageBoxButtons, ByVal icon As MessageBoxIcon) As DialogResult
        If form.InvokeRequired Then
            Return CType(form.Invoke(New MessageDelegate(AddressOf ShowInformation), form, message, caption, buttons, icon), DialogResult)
        Else
            Return MessageBox.Show(message, caption, buttons, icon)
        End If
    End Function

    Public Function GetSelection(ByVal list As String(), ByVal message As String, Optional ByVal title As String = Nothing) As Integer
        Dim dialog As New SelectionDialog(list, message, title)
        dialog.ShowDialog()
        Return dialog.SelectedIndex
    End Function
    Public Function GetSelection(ByVal list As Image(), ByVal message As String, ByVal imageSize As Size, _
        Optional ByVal title As String = Nothing) As Integer
        Dim dialog As New ImageSelectionDialog(list, message, imageSize, title)
        dialog.ShowDialog()
        Return dialog.SelectedIndex
    End Function
    Public Function GetInput(ByVal prompt As String, Optional ByVal title As String = Nothing, Optional ByVal textLength As Int32 = 0, _
        Optional ByVal muiltLine As Boolean = False, Optional ByVal defaultResponse As String = Nothing) As String
        Dim dialog As New InputDialog(prompt, title, textLength, muiltLine, defaultResponse)
        dialog.ShowDialog()
        Return dialog.InputText
    End Function

    Public Enum PokemonState As Integer
        No
        Poison
        Paralysis
        Burn
        Freeze
        Sleep
        Toxin
    End Enum

    Public Enum SelectedPage As Byte
        Index
        SelectingMove
        SelectingPM
        Waiting
    End Enum

    Public Function GetPMStateImg(ByVal index As Integer, ByVal pmVal As Pokemon, Optional ByVal showHP As Boolean = False) As Bitmap
        If pmVal.HP = 0 Then Return Nothing
        Return GetPMStateImg(index, pmVal.Nickname, pmVal.MAXHP, pmVal.HP, pmVal.Gender, pmVal.LV, pmVal.State, showHP)
    End Function
    Public Function GetPMStateImg(ByVal index As Integer, ByVal snapshot As PokemonBattle.BattleNetwork.PokemonSnapshot) As Bitmap
        If snapshot.Hp = 0 Then Return Nothing
        Return GetPMStateImg(index, snapshot.Nickname, snapshot.MaxHp, snapshot.Hp, _
                             CType(snapshot.Gender, PokemonGender), snapshot.Lv, CType(snapshot.State, PokemonState), False)
    End Function
    Private Function GetPMStateImg(ByVal index As Integer, ByVal nickname As String, ByVal maxHp As Integer, ByVal hp As Integer, _
                                   ByVal gender As PokemonGender, ByVal lv As Integer, ByVal state As PokemonState, ByVal showHP As Boolean) As Bitmap
        Dim newImg As Bitmap
        Dim font As New Font("宋体", 12, FontStyle.Regular, GraphicsUnit.Pixel)
        Select Case index
            Case 1
                If showHP Then
                    newImg = New Bitmap(My.Resources.PMState1)
                Else
                    newImg = New Bitmap(My.Resources.PMState3)
                End If
                Using graph As Graphics = Graphics.FromImage(newImg)
                    graph.DrawString(nickname, font, Brushes.Black, 12, 3)
                    graph.DrawString("Lv" & lv, font, Brushes.Black, 82, 5)
                    graph.DrawImageUnscaled(DrawHPLine(maxHp, hp), 47, 19)
                    Select Case gender
                        Case PokemonGender.Male
                            graph.DrawImageUnscaled(My.Resources.male, 76, 7)
                        Case PokemonGender.Female
                            graph.DrawImageUnscaled(My.Resources.female, 76, 7)
                    End Select

                    Dim pt As Point = New Point(14, 15)
                    If showHP Then pt = New Point(14, 22)
                    Select Case state
                        Case PokemonState.Freeze
                            graph.DrawImageUnscaled(My.Resources.冻结, pt)
                        Case PokemonState.Burn
                            graph.DrawImageUnscaled(My.Resources.烧伤, pt)
                        Case PokemonState.Sleep
                            graph.DrawImageUnscaled(My.Resources.睡眠, pt)
                        Case PokemonState.Toxin
                            graph.DrawImageUnscaled(My.Resources.中毒, pt)
                        Case PokemonState.Poison
                            graph.DrawImageUnscaled(My.Resources.中毒, pt)
                        Case PokemonState.Paralysis
                            graph.DrawImageUnscaled(My.Resources.麻痹, pt)
                    End Select

                    If showHP Then graph.DrawString(hp & "/" & maxHp, font, Brushes.Black, 70, 25)

                End Using
            Case Else
                newImg = New Bitmap(My.Resources.PMState2)
                Using graph As Graphics = Graphics.FromImage(newImg)
                    graph.DrawString(nickname, font, Brushes.Black, 7, 3)
                    graph.DrawString("Lv" & lv, font, Brushes.Black, 75, 5)
                    Select Case gender
                        Case PokemonGender.Male
                            graph.DrawImageUnscaled(My.Resources.male, 69, 7)
                        Case PokemonGender.Female
                            graph.DrawImageUnscaled(My.Resources.female, 69, 7)
                    End Select

                    graph.DrawImageUnscaled(DrawHPLine(maxHp, hp), 41, 19)
                    Select Case state
                        Case PokemonState.Freeze
                            graph.DrawImageUnscaled(My.Resources.冻结, 9, 15)
                        Case PokemonState.Burn
                            graph.DrawImageUnscaled(My.Resources.烧伤, 9, 15)
                        Case PokemonState.Sleep
                            graph.DrawImageUnscaled(My.Resources.睡眠, 9, 15)
                        Case PokemonState.Toxin
                            graph.DrawImageUnscaled(My.Resources.中毒, 9, 15)
                        Case PokemonState.Poison
                            graph.DrawImageUnscaled(My.Resources.中毒, 9, 15)
                        Case PokemonState.Paralysis
                            graph.DrawImageUnscaled(My.Resources.麻痹, 9, 15)
                    End Select

                End Using
        End Select
        Return newImg
    End Function
    Public Function GetPMImage(ByVal pmVal As Pokemon) As Bitmap
        Dim pmImg As New Bitmap(126, 46)
        Dim font1 As New Font("宋体", 12, FontStyle.Regular, GraphicsUnit.Pixel)
        Dim font2 As New Font("宋体", 10, FontStyle.Regular, GraphicsUnit.Pixel)

        Using graph As Graphics = Graphics.FromImage(pmImg)
            graph.DrawImageUnscaledAndClipped(pmVal.Icon, New Rectangle(8, 10, 32, 32))
            graph.DrawImageUnscaled(DrawHPLine(pmVal.MAXHP, pmVal.HP), 50, 22)

            Select Case pmVal.Gender
                Case PokemonGender.Male
                    graph.DrawImageUnscaled(My.Resources.male, 110, 10)
                Case PokemonGender.Female
                    graph.DrawImageUnscaled(My.Resources.female, 110, 10)
            End Select

            graph.DrawString(pmVal.Nickname, font1, Brushes.Black, 40, 9)
            graph.DrawString(pmVal.HP & "/" & pmVal.MAXHP, font2, Brushes.Black, 80, 30)
            If pmVal.State = PokemonState.No Then
                graph.DrawString("Lv:" & pmVal.LV, font2, Brushes.Black, 45, 30)
            Else
                Select Case pmVal.State
                    Case PokemonState.Paralysis
                        graph.DrawImageUnscaled(My.Resources.麻痹, 45, 30)
                    Case PokemonState.Freeze
                        graph.DrawImageUnscaled(My.Resources.冻结, 45, 30)
                    Case PokemonState.Burn
                        graph.DrawImageUnscaled(My.Resources.烧伤, 45, 30)
                    Case PokemonState.Sleep
                        graph.DrawImageUnscaled(My.Resources.睡眠, 45, 30)
                    Case PokemonState.Toxin
                        graph.DrawImageUnscaled(My.Resources.中毒, 45, 30)
                    Case PokemonState.Poison
                        graph.DrawImageUnscaled(My.Resources.中毒, 45, 30)
                End Select
            End If
        End Using
        Return pmImg
    End Function
    Public Function DrawHPLine(ByVal maxHP As Integer, ByVal hp As Integer) As Bitmap
        Dim line As New Bitmap(My.Resources.HPLine)
        Dim width As Integer = Convert.ToInt32(hp / maxHP * 50)
        If width = 0 AndAlso hp <> 0 Then width = 1
        If width > 0 Then
            Dim color As Color = My.Resources.HpColor.GetPixel(50 - width, 0)
            Dim color2 As Color = My.Resources.HpColor.GetPixel(50 - width, 1)

            Dim rect1 As Rectangle = New Rectangle(14, 1, width, 2)
            Dim rect2 As Rectangle = New Rectangle(14, 3, width, 2)
            Dim brush1 As LinearGradientBrush = New LinearGradientBrush(rect1, color2, color, LinearGradientMode.Vertical)
            Dim brush2 As LinearGradientBrush = New LinearGradientBrush(rect2, color, color2, LinearGradientMode.Vertical)
            Using painter As Graphics = Graphics.FromImage(line)
                painter.FillRectangle(brush1, rect1)
                painter.FillRectangle(brush2, rect2)
            End Using
        End If
        Return line
    End Function
    Public Function GetIndexImage(ByVal myteam As Team, ByVal opponentTeam As Team) As Bitmap
        Dim newImg As Bitmap
        newImg = New Bitmap(256, 25)
        Using graph As Graphics = Graphics.FromImage(newImg)
            For i As Integer = 0 To myteam.Pokemon.Count - 1
                If myteam.Pokemon(i) IsNot Nothing Then
                    If myteam.Pokemon(i).HP = 0 Then
                        If myteam.Pokemon.Count > 6 Then
                            graph.DrawImageUnscaled(My.Resources.pmball2c, My.Resources.pmball2c.Width * i, 8)
                        Else
                            graph.DrawImageUnscaled(My.Resources.pmballc, My.Resources.pmballc.Width * i, 3)
                        End If
                    ElseIf myteam.Pokemon(i).State <> PokemonState.No Then
                        If myteam.Pokemon.Count > 6 Then
                            graph.DrawImageUnscaled(My.Resources.pmball2b, My.Resources.pmball2b.Width * i, 8)
                        Else
                            graph.DrawImageUnscaled(My.Resources.pmballb, My.Resources.pmballb.Width * i, 3)
                        End If
                    Else
                        If myteam.Pokemon.Count > 6 Then
                            graph.DrawImageUnscaled(My.Resources.pmball2, My.Resources.pmball2.Width * i, 8)
                        Else
                            graph.DrawImageUnscaled(My.Resources.pmball, My.Resources.pmball.Width * i, 3)
                        End If
                    End If
                End If
                If opponentTeam.Pokemon(i) IsNot Nothing Then
                    If opponentTeam.Pokemon(i).HP = 0 Then
                        graph.DrawImageUnscaled(My.Resources.pmball2c, 256 - My.Resources.pmball2c.Width * (myteam.Pokemon.Count - i), 3)
                    ElseIf opponentTeam.Pokemon(i).State <> PokemonState.No Then
                        graph.DrawImageUnscaled(My.Resources.pmball2b, 256 - My.Resources.pmball2c.Width * (myteam.Pokemon.Count - i), 3)
                    Else
                        graph.DrawImageUnscaled(My.Resources.pmball2, 256 - My.Resources.pmball2c.Width * (myteam.Pokemon.Count - i), 3)
                    End If
                End If
            Next
        End Using
        Return newImg
    End Function
    Public Function GetTerrainImage(ByVal terrain As BattleTerrain) As Bitmap
        Select Case terrain
            Case BattleTerrain.Stadium
                Return My.Resources.建筑
            Case BattleTerrain.Grass
                Return My.Resources.草丛
            Case BattleTerrain.Flat
                Return My.Resources.平地
            Case BattleTerrain.Sand
                Return My.Resources.沙地
            Case BattleTerrain.Mountain
                Return My.Resources.岩山
            Case BattleTerrain.Cave
                Return My.Resources.洞窟
            Case BattleTerrain.Water
                Return My.Resources.水域
            Case Else
                Return My.Resources.雪原
        End Select
    End Function

    Public Function GetHiddenPower(ByVal pmVal As PokemonCustomInfo) As Byte
        Return GetHiddenPower(pmVal.HPIV, pmVal.AttackIV, pmVal.DefenceIV, pmVal.SpeedIV, pmVal.SpAttackIV, pmVal.SpDefenceIV)
    End Function
    Public Function GetHiddenPower(ByVal pmVal As Pokemon) As Byte
        Return GetHiddenPower(pmVal.HPIV, pmVal.AttackIV, pmVal.DefenceIV, pmVal.SpeedIV, pmVal.SpAttackIV, pmVal.SpDefenceIV)
    End Function
    Private Function GetHiddenPower(ByVal hp As Integer, ByVal atk As Integer, ByVal def As Integer, _
        ByVal speed As Integer, ByVal satk As Integer, ByVal sdef As Integer) As Byte
        Dim a, b, c, d, e, f, power As Byte
        If hp Mod 4 = 2 OrElse hp Mod 4 = 3 Then a = 1
        If atk Mod 4 = 2 OrElse atk Mod 4 = 3 Then b = 2
        If def Mod 4 = 2 OrElse def Mod 4 = 3 Then c = 4
        If speed Mod 4 = 2 OrElse speed Mod 4 = 3 Then d = 8
        If satk Mod 4 = 2 OrElse satk Mod 4 = 3 Then e = 16
        If sdef Mod 4 = 2 OrElse sdef Mod 4 = 3 Then f = 32
        power = Convert.ToByte(Math.Truncate((a + b + c + d + e + f) * 40 / 63 + 30))
        Return power
    End Function
    Public Function GetNaturalGiftPower(ByVal BerryIndex As Integer) As Byte
        If (1 <= BerryIndex AndAlso BerryIndex <= 16) OrElse (36 <= BerryIndex AndAlso BerryIndex <= 52) Then
            Return 60
        ElseIf 17 <= BerryIndex AndAlso BerryIndex <= 32 Then
            Return 70
        ElseIf 33 <= BerryIndex AndAlso BerryIndex <= 35 Then
            Return 80
        ElseIf 53 <= BerryIndex AndAlso BerryIndex <= 60 Then
            Return 80
        Else
            Return 80
        End If
    End Function
    Public Function GetNaturalGiftType(ByVal BerryIndex As Integer) As String
        Dim types As String() = _
            {"火", "水", "电", "草", "冰", "格斗", "毒", "地面", "飞行", "超能", "虫", "岩", "鬼", "龙", "恶", "钢", "普通"}
        If 1 <= BerryIndex AndAlso BerryIndex <= 16 Then
            Return types(BerryIndex - 1)
        ElseIf 17 <= BerryIndex AndAlso BerryIndex <= 32 Then
            Return types(BerryIndex - 17)
        ElseIf 33 <= BerryIndex AndAlso BerryIndex <= 35 Then
            Return types(BerryIndex - 33)
        ElseIf 36 <= BerryIndex AndAlso BerryIndex <= 52 Then
            Return types(BerryIndex - 36)
        ElseIf 53 <= BerryIndex AndAlso BerryIndex <= 60 Then
            Return types(BerryIndex - 50)
        Else
            Return types(16)
        End If
    End Function

    Public Function GetHiddenType(ByVal pmVal As PokemonCustomInfo) As String
        Dim a, b, c, d, e, f, index As Integer
        Dim type As String
        If pmVal.HPIV Mod 2 = 1 Then a = 1
        If pmVal.AttackIV Mod 2 = 1 Then b = 2
        If pmVal.DefenceIV Mod 2 = 1 Then c = 4
        If pmVal.SpeedIV Mod 2 = 1 Then d = 8
        If pmVal.SpAttackIV Mod 2 = 1 Then e = 16
        If pmVal.SpDefenceIV Mod 2 = 1 Then f = 32
        index = Convert.ToInt32(Math.Truncate((a + b + c + d + e + f) * 15 / 63))
        type = TypeIndex(index + 1)
        Return type
    End Function
    Public Function GetHiddenType(ByVal pmVal As Pokemon) As String
        Dim a, b, c, d, e, f, index As Integer
        Dim type As String
        If pmVal.HPIV Mod 2 = 1 Then a = 1
        If pmVal.AttackIV Mod 2 = 1 Then b = 2
        If pmVal.DefenceIV Mod 2 = 1 Then c = 4
        If pmVal.SpeedIV Mod 2 = 1 Then d = 8
        If pmVal.SpAttackIV Mod 2 = 1 Then e = 16
        If pmVal.SpDefenceIV Mod 2 = 1 Then f = 32
        index = Convert.ToInt32(Math.Truncate((a + b + c + d + e + f) * 15 / 63))
        type = TypeIndex(index + 1)
        Return type
    End Function

    Private key As String = "sJg/jtph3lzuhiy9EafZtw=="
    Private iv As String = "k/SHDmKCLIo="
    Public Sub Encrypt(ByVal ms As MemoryStream, ByVal outPath As String)
        Dim sa As SymmetricAlgorithm = SymmetricAlgorithm.Create("RC2")
        sa.IV = Convert.FromBase64String(iv)
        sa.Key = Convert.FromBase64String(key)
        Using fileOut As New FileStream(outPath, FileMode.Create, FileAccess.Write)
            Using cs As New CryptoStream(fileOut, sa.CreateEncryptor, CryptoStreamMode.Write)
                Dim data As Byte() = ms.GetBuffer
                cs.Write(data, 0, data.Length)
            End Using
        End Using
    End Sub
    Public Function Decrypt(ByVal fileIn As FileStream) As MemoryStream
        Dim sa As SymmetricAlgorithm = SymmetricAlgorithm.Create("RC2")
        sa.IV = Convert.FromBase64String(iv)
        sa.Key = Convert.FromBase64String(key)
        Dim buffer As Byte() = New Byte(2048) {}
        Dim length As Integer
        Dim ms As New MemoryStream()
        If fileIn.Length <> 0 Then
            Using cs As New CryptoStream(fileIn, sa.CreateDecryptor, CryptoStreamMode.Read)
                Do
                    length = cs.Read(buffer, 0, buffer.Length)
                    ms.Write(buffer, 0, length)
                Loop While length > 0
            End Using
        End If
        Return ms
    End Function

    Public Function ComputeHash(ByVal stream As Stream) As String
        Dim md5 As New MD5CryptoServiceProvider()
        Dim buffer As Byte() = md5.ComputeHash(stream)
        Return Convert.ToBase64String(buffer)
    End Function

    Public Function GetVersionInfo() As String
        Return Application.ProductVersion
    End Function

    Public Function ComputeAbility(ByVal base As Integer, ByVal ev As Integer, _
                                     ByVal iv As Integer, ByVal dv As Double, ByVal lv As Integer) As Integer
        Return Convert.ToInt32(Math.Truncate( _
            Math.Truncate((base * 2 + iv + ev \ 4) * lv * 0.01 + 5) * dv))
    End Function
End Module
