Imports PokemonBattle.PokemonData
Imports PokemonBattle.BattleNetwork
Public Class BattleGround
    Private _weather As Weather
    Private weatherCount As Integer
    Private _terrain As BattleTerrain

    Private _trickRoom As Boolean
    Private trickRoomCounter As Integer

    Private _magicRoom As Boolean
    Private magicRoomCounter As Integer

    Private _miracleRoom As Boolean
    Private miracleRoomCounter As Integer

    Private _gravity As Boolean
    Private gravityCounter As Integer

    Public waterSport, mudSport As Boolean
    Public followPokemon As Pokemon

    Private weatherType As List(Of PokemonType)
    Private team1, team2 As Team

    Public Sub New(ByVal randomValue As Random, ByVal team1Value As Team, ByVal team2Value As Team)
        _terrain = New BattleTerrain() {BattleTerrain.Cave, BattleTerrain.Flat, BattleTerrain.Grass, BattleTerrain.Mountain, _
                                       BattleTerrain.Sand, BattleTerrain.SnowField, BattleTerrain.Stadium, BattleTerrain.Water} _
                                       (randomValue.Next(0, 8))
        team1 = team1Value
        team2 = team2Value
        weatherType = New List(Of PokemonType)
        weatherType.Add(BattleData.GetTypeData("火"))
        weatherType.Add(BattleData.GetTypeData("水"))
        weatherType.Add(BattleData.GetTypeData("冰"))
    End Sub

    Public ReadOnly Property Terrain() As BattleTerrain
        Get
            Return _terrain
        End Get
    End Property

    Public ReadOnly Property Weather() As Weather
        Get
            If AirLock Then Return Weather.No
            Return _weather
        End Get
    End Property
    Public Function NewWeather(ByVal weatherValue As Weather, ByVal maker As Pokemon, _
        ByVal log As ImgTxt, Optional ByVal traitEff As Boolean = False) As Boolean
        If _weather = weatherValue Then
            If traitEff Then weatherCount = -1
            Return False
        Else
            weatherCount = 5
            _weather = weatherValue
            Select Case _weather
                Case Weather.Rainy
                    log.AddText("场上下起雨了！")
                    If maker.Item = Item.湿石 Then weatherCount = 8
                Case Weather.Sunny
                    log.AddText("太阳出来了！")
                    If maker.Item = Item.热石 Then weatherCount = 8
                Case Weather.SandStorm
                    log.AddText("场上刮起了沙尘暴！")
                    If maker.Item = Item.沙石 Then weatherCount = 8
                Case Weather.HailStorm
                    log.AddText("场上下起了冰雹！")
                    If maker.Item = Item.冰石 Then weatherCount = 8
            End Select
            If traitEff Then weatherCount = -1
            If Not AirLock Then ChangeWeather()
            Return True
        End If
    End Function

    Public ReadOnly Property TrickRoom() As Boolean
        Get
            Return _trickRoom
        End Get
    End Property
    Public Sub TrickRoomChange(ByVal log As ImgTxt)
        If TrickRoom Then
            _trickRoom = False
        Else
            _trickRoom = True
            trickRoomCounter = 5
            log.AddText("5回合内速度慢的先手！")
        End If
    End Sub

    Public ReadOnly Property MagicRoom As Boolean
        Get
            Return _magicRoom
        End Get
    End Property
    Public Sub MagicRoomChange(ByVal log As ImgTxt)
        If Not _magicRoom Then
            _magicRoom = True
            magicRoomCounter = 5
            log.AddText("5回合内道具失效！")
        End If
    End Sub

    Public ReadOnly Property MiracleRoom As Boolean
        Get
            Return _miracleRoom
        End Get
    End Property
    Public Sub MiracleRoomChange(ByVal log As ImgTxt)
        If Not _miracleRoom Then
            _miracleRoom = True
            miracleRoomCounter = 5
            log.AddText("5回合内物防与特防交换！")
        Else
            _miracleRoom = False
        End If
    End Sub


    Public ReadOnly Property Gravity() As Boolean
        Get
            Return _gravity
        End Get
    End Property
    Public Sub Gravitate(ByVal log As ImgTxt)
        _gravity = True
        gravityCounter = 5
        log.AddText("全场都受到了重力的作用！")
        For Each pm As Pokemon In team1.SelectedPokemon
            GravitatePM(log, pm)
        Next
        For Each pm As Pokemon In team2.SelectedPokemon
            GravitatePM(log, pm)
        Next
    End Sub
    Private Sub GravitatePM(ByVal log As ImgTxt, ByVal pm As Pokemon)
        With pm
            If .BattleState.fly OrElse .BattleState.jump Then
                .BattleState.jump = False
                .BattleState.fly = False
                log.AddText(.myTeam.GetPlayerName(pm) & "的" & .CustomName & "回到了地面！")
            End If
        End With
    End Sub

    Public ReadOnly Property AirLock() As Boolean
        Get
            For Each pm As Pokemon In team1.SelectedPokemon
                If (pm.SelTrait = Trait.无天气 OrElse pm.SelTrait = Trait.气象锁) AndAlso pm.HP > 0 Then Return True
            Next
            For Each pm As Pokemon In team2.SelectedPokemon
                If (pm.SelTrait = Trait.无天气 OrElse pm.SelTrait = Trait.气象锁) AndAlso pm.HP > 0 Then Return True
            Next
            Return False
        End Get
    End Property
    Public ReadOnly Property Unaware() As Boolean
        Get
            For Each pm As Pokemon In team1.SelectedPokemon
                If pm.SelTrait = Trait.天然 AndAlso pm.HP > 0 Then Return True
            Next
            For Each pm As Pokemon In team2.SelectedPokemon
                If pm.SelTrait = Trait.天然 AndAlso pm.HP > 0 Then Return True
            Next
            Return False
        End Get
    End Property
    Public ReadOnly Property Uproar() As Boolean
        Get
            For Each pm As Pokemon In team1.SelectedPokemon
                If pm.BattleState.Uproar AndAlso pm.HP > 0 Then Return True
            Next
            For Each pm As Pokemon In team2.SelectedPokemon
                If pm.BattleState.Uproar AndAlso pm.HP > 0 Then Return True
            Next
            Return False
        End Get
    End Property
    Public ReadOnly Property 湿气() As Boolean
        Get
            For Each pm As Pokemon In team1.SelectedPokemon
                If pm.SelTrait = Trait.湿气 AndAlso pm.HP > 0 Then Return True
            Next
            For Each pm As Pokemon In team2.SelectedPokemon
                If pm.SelTrait = Trait.湿气 AndAlso pm.HP > 0 Then Return True
            Next
            Return False
        End Get
    End Property

    Public Sub CountOfTurnEnd(ByVal log As ImgTxt)
        If TrickRoom Then
            trickRoomCounter -= 1
            If trickRoomCounter = 0 Then
                _trickRoom = False
                log.AddText("欺骗空间消失了！")
            End If
        End If
        If Gravity Then
            gravityCounter -= 1
            If gravityCounter = 0 Then
                _gravity = False
                log.AddText("重力的作用消失了！")
            End If
        End If
        If MagicRoom Then
            magicRoomCounter -= 1
            If magicRoomCounter = 0 Then
                _magicRoom = False
            End If
        End If
        If MiracleRoom Then
            miracleRoomCounter -= 1
            If miracleRoomCounter = 0 Then
                _miracleRoom = False
            End If
        End If
        If Not Echo Then
            EchoCounter = 0
        Else
            Echo = False
            EchoCounter += 1
        End If
        SingARound = False
        CountWeather(log)
    End Sub
    Private Sub CountWeather(ByVal log As ImgTxt)
        If _weather <> Weather.No Then
            weatherCount -= 1
            If weatherCount <> 0 Then
                Select Case Weather
                    Case Weather.HailStorm
                        log.AddText("冰雹继续下着")
                        WeatherHurt(team1, log)
                        WeatherHurt(team2, log)
                    Case Weather.Rainy
                        log.AddText("雨一直下")
                    Case Weather.SandStorm
                        log.AddText("沙尘暴继续刮着")
                        WeatherHurt(team1, log)
                        WeatherHurt(team2, log)
                    Case Weather.Sunny
                        log.AddText("天气十分晴朗")
                End Select
            Else
                _weather = Weather.No
                log.AddText("天气恢复了正常")
                ChangeWeather()
            End If
        End If
    End Sub
    Private Sub WeatherHurt(ByVal team As Team, ByVal log As ImgTxt)
        For Each pm As Pokemon In team.SelectedPokemon
            WeatherHurt(pm, log)
        Next
    End Sub
    Private Sub WeatherHurt(ByVal pm As Pokemon, ByVal log As ImgTxt)
        With pm
            If .SelTrait <> Trait.魔法守护 AndAlso Not ( _
                .HP = 0 OrElse .BattleState.dig OrElse .BattleState.dive) Then
                Select Case Weather
                    Case Weather.SandStorm
                        If Not (.HaveType("地面") OrElse .HaveType("岩") OrElse .HaveType("钢") OrElse .SelTrait = Trait.沙隐术) Then
                            log.AddText(.myTeam.GetPlayerName(pm) & "的" & .CustomName & "受到了沙尘暴的伤害！")
                            .GetHurt(.MAXHP * 0.0625)
                        End If
                    Case Weather.HailStorm
                        If Not (.HaveType("冰") OrElse .SelTrait = Trait.雪遁) Then
                            log.AddText(.myTeam.GetPlayerName(pm) & "的" & .CustomName & "受到了冰雹的伤害！")
                            .GetHurt(.MAXHP * 0.0625)
                        End If
                End Select
            End If
        End With
    End Sub

    Private Sub ChangeWeather()
        For Each pm As Pokemon In team1.SelectedPokemon
            ChangeWeather(pm)
        Next
        For Each pm As Pokemon In team2.SelectedPokemon
            ChangeWeather(pm)
        Next
    End Sub
    Public Sub ChangeWeather(ByVal pm As Pokemon)
        Select Case Weather
            Case Weather.No
                Select Case pm.SelTrait
                    Case Trait.气象台
                        pm.BattleState.traitRaised = False
                        pm.Type1 = Nothing
                        pm.Type2 = Nothing
                        pm.FrontImage = Nothing
                        pm.BackImage = Nothing
                End Select
                For i As Integer = 1 To 4
                    If pm.SelMove(i) IsNot Nothing AndAlso pm.SelMove(i).AddEff2 = MoveAdditionalEffect.天气球 Then
                        pm.SelMove(i).SetType(BattleData.GetTypeData("普通"))
                    End If
                Next
            Case Weather.HailStorm
                Select Case pm.SelTrait
                    Case Trait.气象台
                        pm.BattleState.traitRaised = True
                        pm.FrontImage = My.Resources.Hailstrom
                        pm.BackImage = My.Resources.HailstromBack
                        pm.Type1 = weatherType(2)
                        pm.Type2 = Nothing
                End Select
                For i As Integer = 1 To 4
                    If pm.SelMove(i) IsNot Nothing AndAlso pm.SelMove(i).AddEff2 = MoveAdditionalEffect.天气球 Then
                        pm.SelMove(i).SetType(BattleData.GetTypeData("冰"))
                    End If
                Next
            Case Weather.Sunny
                Select Case pm.SelTrait
                    Case Trait.气象台
                        pm.BattleState.traitRaised = True
                        pm.FrontImage = My.Resources.Sunny
                        pm.BackImage = My.Resources.SunnyBack
                        pm.Type1 = weatherType(0)
                        pm.Type2 = Nothing
                End Select
                For i As Integer = 1 To 4
                    If pm.SelMove(i) IsNot Nothing AndAlso pm.SelMove(i).AddEff2 = MoveAdditionalEffect.天气球 Then
                        pm.SelMove(i).SetType(BattleData.GetTypeData("火"))
                    End If
                Next
            Case Weather.SandStorm
                Select Case pm.SelTrait
                    Case Trait.气象台
                        pm.BattleState.traitRaised = False
                        pm.Type1 = Nothing
                        pm.Type2 = Nothing
                        pm.FrontImage = Nothing
                        pm.BackImage = Nothing
                End Select
                For i As Integer = 1 To 4
                    If pm.SelMove(i) IsNot Nothing AndAlso pm.SelMove(i).AddEff2 = MoveAdditionalEffect.天气球 Then
                        pm.SelMove(i).SetType(BattleData.GetTypeData("岩"))
                    End If
                Next
            Case Weather.Rainy
                Select Case pm.SelTrait
                    Case Trait.气象台
                        pm.BattleState.traitRaised = True
                        pm.FrontImage = My.Resources.Rainy
                        pm.BackImage = My.Resources.RainyBack
                        pm.Type1 = weatherType(1)
                        pm.Type2 = Nothing
                End Select
                For i As Integer = 1 To 4
                    If pm.SelMove(i) IsNot Nothing AndAlso pm.SelMove(i).AddEff2 = MoveAdditionalEffect.天气球 Then
                        pm.SelMove(i).SetType(BattleData.GetTypeData("水"))
                    End If
                Next
        End Select
    End Sub

    Public Property Echo As Boolean
    Public Property EchoCounter As Integer

    Public Property SingARound As Boolean
End Class
