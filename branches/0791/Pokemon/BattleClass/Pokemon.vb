Imports PokemonBattle.BattleNetwork
Imports PokemonBattle.PokemonData
Public Class Pokemon
    Private mNameBase As String
    Private mNumber As Integer
    Private mHPBase, mAttackBase, mDefenceBase, mSpeedBase, mSpAttackBase, mSpDefenceBase As Integer '种族
    Private mType1, mType2 As PokemonType
    Private mTrait1, mTrait2 As Trait
    Private mNickname As String
    Private mWeight As Double
    Private mLV As Byte
    Private mMaxHP, mHP, mAttack, mDefence, mSpeed, mSpAttack, mSpDefence As Integer
    Private mHPEV, mAttackEV, mDefenceEV, mSpeedEV, mSpAttackEV, mSpDefenceEV As Byte
    Private mHPIV, mAttackIV, mDefenceIV, mSpeedIV, mSpAttackIV, mSpDefenceIV As Byte '个体
    Private mAttackDV, mDefenceDV, mSpeedDV, mSpAttackDV, mSpDefenceDV As Double '性格修正

    Private _identity As Integer

    Private mGender As PokemonGender
    Private mNature As PokemonCharacter
    Private mState As PokemonState = PokemonState.No
    Private mSelTrait As Trait
    Private mSelMoves As Move()
    Private mImage1, mImage2, mImage1b, mImage2b, mIcon, mFrame1, mFrame2 As Long
    Private mItem As Item
    Private mUsedItem As Item

    Public Event HPChange As EventHandler
    Public Event StateChange As EventHandler
    Public Event Died As EventHandler
    Public Event ImageChange As EventHandler

    Public sleepCounter, sleepTurn As Integer
    Public lastMoveIndex As Byte
    Public attackSuccessed As Boolean

    Public turnHurt As New List(Of Integer)
    Public turnHurtMove As New List(Of Integer)
    Public turnHurtBy As New List(Of PokemonIndex)

    Public hypnosis As Boolean
    Public lastTarget As TargetIndex

    Public myTeam As Team
    Private mBattleState As PokemonBattleState

    Public Sub New(ByVal data As PokemonData, ByVal info As PokemonCustomInfo, ByVal myTeamValue As Team, _
        ByVal ppUp As Boolean)
        myTeam = myTeamValue
        mSelMoves = New Move(3) {}

        _identity = data.Identity

        mAttackBase = data.AttackBase
        mDefenceBase = data.DefenceBase
        mHPBase = data.HPBase
        mSpeedBase = data.SpeedBase
        mSpAttackBase = data.SpAttackBase
        mSpDefenceBase = data.SpDefenceBase

        mNameBase = data.Name
        mNumber = data.Number
        mWeight = data.Weight

        mType1 = BattleData.GetTypeData(data.Type1)
        mType2 = BattleData.GetTypeData(data.Type2)

        mTrait1 = data.Trait1
        mTrait2 = data.Trait2

        mImage1 = data.FrontImage
        mImage1b = data.FrontImageF
        mImage2 = data.BackImage
        mImage2b = data.BackImageF
        mIcon = data.Icon
        mFrame1 = data.Frame
        mFrame2 = data.FrameF

        mHPIV = info.HPIV
        mAttackIV = info.AttackIV
        mDefenceIV = info.DefenceIV
        mSpeedIV = info.SpeedIV
        mSpAttackIV = info.SpAttackIV
        mSpDefenceIV = info.SpDefenceIV

        mHPEV = info.HPEV
        mAttackEV = info.AttackEV
        mDefenceEV = info.DefenceEV
        mSpeedEV = info.SpeedEV
        mSpAttackEV = info.SpAttackEV
        mSpDefenceEV = info.SpDefenceEV

        mGender = info.Gender
        Nature = info.Character

        mLV = info.LV
        mNickname = info.Nickname
        mItem = info.Item

        Select Case info.SelectedTrait
            Case 1
                mSelTrait = data.Trait1
            Case 2
                mSelTrait = data.Trait2
        End Select
        For i As Integer = 0 To 3
            If info.SelectedMoves(i) <> MoveData.InvalidId Then
                SetMove(i + 1, New Move(BattleData.GetMove(info.SelectedMoves(i)), ppUp))
            End If
        Next

        If SelTrait = Trait.缓慢启动 OrElse SelTrait = Trait.多重属性 Then RaiseTrait()

        If mNameBase = "鬼蝉" Then
            mMaxHP = 1
        Else
            mMaxHP = Convert.ToInt32(Math.Truncate((mHPBase * 2 + mHPIV + mHPEV \ 4) * mLV * 0.01 + 10 + mLV))
        End If
        mHP = MAXHP
        mAttack = ComputeAbility(mAttackBase, mAttackEV, mAttackIV, mAttackDV, mLV)
        mDefence = ComputeAbility(mDefenceBase, mDefenceEV, mDefenceIV, mDefenceDV, mLV)
        mSpeed = ComputeAbility(mSpeedBase, mSpeedEV, mSpeedIV, mSpeedDV, mLV)
        mSpAttack = ComputeAbility(mSpAttackBase, mSpAttackEV, mSpAttackIV, mSpAttackDV, mLV)
        mSpDefence = ComputeAbility(mSpDefenceBase, mSpDefenceEV, mSpDefenceIV, mSpDefenceDV, mLV)

    End Sub

    Public ReadOnly Property Identity() As Integer
        Get
            Return _identity
        End Get
    End Property
    Public ReadOnly Property NO() As Integer
        Get
            Return mNumber
        End Get
    End Property
    Public ReadOnly Property NameBase() As String
        Get
            Return mNameBase
        End Get
    End Property
    Public Property Weight() As Double
        Get
            If BattleState.tempWeight <> 0 Then
                Return BattleState.tempWeight
            End If
            Return mWeight
        End Get
        Set(ByVal value As Double)
            BattleState.tempWeight = value
        End Set
    End Property

    Public Property FrontImage() As Bitmap
        Get
            With BattleState
                If .substituted Then Return My.Resources.Substitute
                If .transfrom OrElse .shapeShift Then
                    If Gender = PokemonGender.Female AndAlso .transTemp.FrontImageF <> -1 Then
                        Return GetImage(.transTemp.FrontImageF)
                    Else
                        Return GetImage(.transTemp.FrontImage)
                    End If
                End If
                If SelTrait = Trait.气象台 AndAlso .traitRaised Then Return .tempImg1
            End With
            If Gender = PokemonGender.Female Then Return FrontImageF
            Return GetImage(mImage1)
        End Get
        Set(ByVal value As Bitmap)
            BattleState.tempImg1 = value
            RaiseEvent ImageChange(Me, EventArgs.Empty)
        End Set
    End Property
    Public Property BackImage() As Bitmap
        Get
            With BattleState
                If .substituted Then Return My.Resources.SubBack
                If .transfrom OrElse .shapeShift Then
                    If Gender = PokemonGender.Female AndAlso .transTemp.FrontImageF <> -1 Then
                        Return GetImage(.transTemp.BackImageF)
                    Else
                        Return GetImage(.transTemp.BackImage)
                    End If
                End If
                If SelTrait = Trait.气象台 AndAlso .traitRaised Then Return .tempImg2
            End With
            If Gender = PokemonGender.Female Then
                Return BackImageF
            End If
            Return GetImage(mImage2)
        End Get
        Set(ByVal value As Bitmap)
            BattleState.tempImg2 = value
            RaiseEvent ImageChange(Me, EventArgs.Empty)
        End Set
    End Property
    Public ReadOnly Property FrontImageF() As Bitmap
        Get
            If mImage1b <> -1 Then Return GetImage(mImage1b)
            Return GetImage(mImage1)
        End Get
    End Property
    Public ReadOnly Property BackImageF() As Bitmap
        Get
            If mImage2b <> -1 Then Return GetImage(mImage2b)
            Return GetImage(mImage2)
        End Get
    End Property
    Public ReadOnly Property Icon() As Bitmap
        Get
            If BattleState.transfrom OrElse BattleState.shapeShift Then Return GetImage(BattleState.transTemp.Icon)
            Return GetImage(mIcon)
        End Get
    End Property
    Public ReadOnly Property Frame() As Bitmap
        Get
            If Gender = PokemonGender.Female Then Return FrameF
            If mFrame1 <> -1 Then Return GetImage(mFrame1)
            Return FrontImage
        End Get
    End Property
    Public ReadOnly Property FrameF() As Bitmap
        Get
            If mFrame2 <> -1 Then Return GetImage(mFrame2)
            If mFrame1 <> -1 Then Return GetImage(mFrame1)
            Return FrontImage
        End Get
    End Property
    Private Function GetImage(ByVal position As Long) As Bitmap
        Return BattleData.GetImage(_identity, position)
    End Function


    Public ReadOnly Property Nickname() As String
        Get
            Return mNickname
        End Get
    End Property
    Public ReadOnly Property Gender() As PokemonGender
        Get
            Return mGender
        End Get
    End Property
    Public ReadOnly Property LV() As Byte
        Get
            Return mLV
        End Get
    End Property

    Public ReadOnly Property CustomName() As String
        Get
            If My.Settings.CustomName = 0 Then
                Return Nickname
            Else
                Return NameBase
            End If
        End Get
    End Property
    Public Function GetNameString() As String
        Return String.Format("{0}的{1}", myTeam.GetPlayerName(Me), CustomName)
    End Function

    Public Property Nature() As PokemonCharacter
        Get
            Return mNature
        End Get
        Set(ByVal value As PokemonCharacter)
            mNature = value

            mAttackDV = 1
            mDefenceDV = 1
            mSpeedDV = 1
            mSpAttackDV = 1
            mSpDefenceDV = 1

            Select Case value
                Case PokemonCharacter.Lonely
                    mAttackDV = 1.1
                    mDefenceDV = 0.9
                Case PokemonCharacter.Brave
                    mAttackDV = 1.1
                    mSpeedDV = 0.9
                Case PokemonCharacter.Adamant
                    mAttackDV = 1.1
                    mSpAttackDV = 0.9
                Case PokemonCharacter.Naughty
                    mAttackDV = 1.1
                    mSpDefenceDV = 0.9

                Case PokemonCharacter.Bold
                    mDefenceDV = 1.1
                    mAttackDV = 0.9
                Case PokemonCharacter.Relaxed
                    mDefenceDV = 1.1
                    mSpeedDV = 0.9
                Case PokemonCharacter.Impish
                    mDefenceDV = 1.1
                    mSpAttackDV = 0.9
                Case PokemonCharacter.Lax
                    mDefenceDV = 1.1
                    mSpDefenceDV = 0.9

                Case PokemonCharacter.Timid
                    mSpeedDV = 1.1
                    mAttackDV = 0.9
                Case PokemonCharacter.Hasty
                    mSpeedDV = 1.1
                    mDefenceDV = 0.9
                Case PokemonCharacter.Jolly
                    mSpeedDV = 1.1
                    mSpAttackDV = 0.9
                Case PokemonCharacter.Naive
                    mSpeedDV = 1.1
                    mSpDefenceDV = 0.9

                Case PokemonCharacter.Modest
                    mSpAttackDV = 1.1
                    mAttackDV = 0.9
                Case PokemonCharacter.Mild
                    mSpAttackDV = 1.1
                    mDefenceDV = 0.9
                Case PokemonCharacter.Quiet
                    mSpAttackDV = 1.1
                    mSpeedDV = 0.9
                Case PokemonCharacter.Rash
                    mSpAttackDV = 1.1
                    mSpDefenceDV = 0.9

                Case PokemonCharacter.Calm
                    mSpDefenceDV = 1.1
                    mAttackDV = 0.9
                Case PokemonCharacter.Gentle
                    mSpDefenceDV = 1.1
                    mDefenceDV = 0.9
                Case PokemonCharacter.Sassy
                    mSpDefenceDV = 1.1
                    mSpeedDV = 0.9
                Case PokemonCharacter.Careful
                    mSpDefenceDV = 1.1
                    mSpAttackDV = 0.9

            End Select
        End Set
    End Property

    Public Property State() As PokemonState
        Get
            Return mState
        End Get
        Set(ByVal value As PokemonState)
            mState = value
            If value <> PokemonState.Sleep Then
                If hypnosis Then hypnosis = False
                If BattleState.nightmare Then BattleState.nightmare = False
            End If
            RaiseEvent StateChange(Me, EventArgs.Empty)
        End Set
    End Property
    Public Property BattleState() As PokemonBattleState
        Get
            If mBattleState Is Nothing Then Return PokemonBattleState.EmptyBattleState
            Return mBattleState
        End Get
        Set(ByVal value As PokemonBattleState)
            mBattleState = value
        End Set
    End Property

    Public ReadOnly Property [Friend]() As Pokemon
        Get
            For Each pm As Pokemon In myTeam.SelectedPokemon
                If pm IsNot Me Then Return pm
            Next
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property MAXHP() As Integer
        Get
            Return mMaxHP
        End Get
    End Property
    Public ReadOnly Property HP() As Integer
        Get
            Return mHP
        End Get
    End Property

    Public ReadOnly Property Attack(Optional ByVal ct As Boolean = False) As Integer
        Get
            Dim value As Integer
            Dim lv As Integer = AttackLV
            If SelTrait = Trait.单纯 Then lv *= 2
            If myTeam.ground.Unaware AndAlso SelTrait <> Trait.破格 Then lv = 0

            If ct AndAlso lv < 0 Then lv = 0
            value = CalculateAbility(AttackValue, lv)

            If State = PokemonState.Burn AndAlso Not SelTrait = Trait.上进 Then value = Convert.ToInt32(Math.Truncate(value * 0.5))
            If SelTrait = Trait.强有力 Then value *= 2
            If SelTrait = Trait.瑜伽威力 Then value *= 2
            If SelTrait = Trait.上进 AndAlso State <> ModShare.PokemonState.No Then value = Convert.ToInt32(Math.Truncate(value * 1.5))
            If SelTrait = Trait.紧张 Then value = Convert.ToInt32((Math.Truncate(value * 1.5)))
            If SelTrait = Trait.缓慢启动 AndAlso BattleState.slowStartCounter <> 0 Then value = Convert.ToInt32(Math.Truncate(value * 0.5))
            If Item = Item.电珠 AndAlso NO = 25 Then value *= 2
            If Item = Item.粗骨棒 AndAlso (NO = 105 OrElse NO = 104) Then value *= 2
            If Item = Item.专爱头巾 Then value = Convert.ToInt32(Math.Truncate(value * 1.5))
            Return value
        End Get
    End Property

    Public ReadOnly Property Defence(Optional ByVal target As Pokemon = Nothing, Optional ByVal ct As Boolean = False, _
                                     Optional ByVal miracleRoomRedirected As Boolean = False) As Integer
        Get
            If myTeam.ground.MiracleRoom AndAlso Not miracleRoomRedirected Then
                Return SpDefence(target, ct, True)
            End If
            Dim value As Integer
            Dim lv As Integer = DefenceLV
            If SelTrait = Trait.单纯 AndAlso target IsNot Nothing AndAlso target.SelTrait <> Trait.破格 Then lv *= 2
            If myTeam.ground.Unaware Then lv = 0

            If ct AndAlso lv > 0 Then lv = 0
            value = CalculateAbility(DefenceValue, lv)

            If target IsNot Nothing AndAlso target.SelTrait <> Trait.破格 AndAlso SelTrait = Trait.神奇鳞片 _
                AndAlso State <> ModShare.PokemonState.No Then _
                value = Convert.ToInt32(Math.Truncate(value * 1.5))
            If Item = Item.金属粉末 AndAlso mNameBase = "百变怪" Then value *= 2
            Return value
        End Get
    End Property

    Public ReadOnly Property Speed() As Integer
        Get
            Dim value As Double
            Dim lv As Integer = SpeedLV
            If SelTrait = Trait.单纯 Then lv *= 2
            If myTeam.ground.Unaware Then lv = 0

            value = CalculateAbility(SpeedValue, lv)

            If State = ModShare.PokemonState.Paralysis AndAlso Not SelTrait = Trait.快步走 Then value = value * 0.25
            If myTeam.WithWind Then value *= 2
            If SelTrait = Trait.轻快 AndAlso myTeam.ground.Weather = Weather.Rainy Then value *= 2
            If SelTrait = Trait.叶绿素 AndAlso myTeam.ground.Weather = Weather.Sunny Then value *= 2
            If SelTrait = Trait.走钢丝 AndAlso BattleState.traitRaised Then value *= 2
            If SelTrait = Trait.快步走 AndAlso State <> ModShare.PokemonState.No Then value = value * 1.5
            If SelTrait = Trait.缓慢启动 AndAlso BattleState.slowStartCounter <> 0 Then value = value * 0.5
            If Item = Item.速度珠 AndAlso mNameBase = "百变怪" Then value *= 2
            If Item = Item.专爱围巾 Then value = value * 1.5
            If Item = Item.黑铁球 OrElse Item = Item.竞争背心 Then value = value * 0.5
            If myTeam.moor Then value *= 0.25

            Return Convert.ToInt32(Math.Truncate(value))
        End Get
    End Property

    Public ReadOnly Property SpAttack(Optional ByVal ct As Boolean = False) As Integer
        Get
            Dim value As Double
            Dim lv As Integer = SpAttackLV
            If SelTrait = Trait.单纯 Then lv *= 2
            If myTeam.ground.Unaware AndAlso SelTrait <> Trait.破格 Then lv = 0

            If ct AndAlso lv < 0 Then lv = 0
            value = CalculateAbility(SpAttackValue, lv)

            If SelTrait = Trait.太阳能 AndAlso myTeam.ground.Weather = Weather.Sunny Then value *= 1.5
            If Item = Item.电珠 AndAlso NO = 25 Then value *= 2
            If Item = Item.深海之牙 AndAlso NO = 366 Then value *= 2
            If Item = Item.心之水滴 AndAlso (NO = 380 OrElse NO = 381) Then value *= 1.5
            If Item = Item.专爱眼镜 Then value *= 1.5
            If Me.Friend IsNot Nothing AndAlso Me.Friend.HP <> 0 Then
                If (SelTrait = Trait.正极 AndAlso Me.Friend.SelTrait = Trait.负极) _
                    OrElse (SelTrait = Trait.负极 AndAlso Me.Friend.SelTrait = Trait.正极) Then
                    value = Math.Truncate(value * 1.5)
                End If
            End If
            Return Convert.ToInt32(value)
        End Get
    End Property

    Public ReadOnly Property SpDefence(Optional ByVal target As Pokemon = Nothing, Optional ByVal ct As Boolean = False, _
                                      Optional ByVal miracleRoomRedirected As Boolean = False) As Integer
        Get
            If myTeam.ground.MiracleRoom AndAlso Not miracleRoomRedirected Then
                Return Defence(target, ct, True)
            End If
            Dim value As Integer
            Dim lv As Integer = SpDefenceLV
            If SelTrait = Trait.单纯 AndAlso target IsNot Nothing AndAlso target.SelTrait <> Trait.破格 Then lv *= 2
            If myTeam.ground.Unaware Then lv = 0

            If ct AndAlso lv > 0 Then lv = 0
            value = CalculateAbility(SpDefenceValue, lv)

            If Item = Item.深海之鳞 AndAlso NO = 366 Then value *= 2
            If Item = Item.心之水滴 AndAlso (NO = 380 OrElse NO = 381) Then value = Convert.ToInt32(Math.Truncate(value * 1.5))
            Return value
        End Get
    End Property

    Private Function CalculateAbility(ByVal value As Integer, ByVal lv As Integer) As Integer
        If lv >= 0 Then
            If lv > 6 Then lv = 6
            value = Convert.ToInt32(Math.Truncate(value * (2 + lv) * 0.5))
        Else
            If lv < -6 Then lv = -6
            value = Convert.ToInt32(Math.Truncate(value * 2 / (2 - lv)))
        End If
        Return value
    End Function


    Public Property AttackValue(Optional ByVal powerTrickRedirected As Boolean = False) As Integer
        Get
            If BattleState.powerTrick AndAlso Not powerTrickRedirected Then
                Return DefenceValue(True)
            End If
            If BattleState.tempAttack <> 0 Then Return BattleState.tempAttack
            Return mAttack
        End Get
        Set(ByVal value As Integer)
            If BattleState.powerTrick AndAlso Not powerTrickRedirected Then
                DefenceValue(True) = value
            End If
            BattleState.tempAttack = value
        End Set
    End Property
    Public Property DefenceValue(Optional ByVal powerTrickRedirected As Boolean = False) As Integer
        Get
            If BattleState.powerTrick AndAlso Not powerTrickRedirected Then
                Return AttackValue(True)
            End If
            If BattleState.tempDefence <> 0 Then Return BattleState.tempDefence
            Return mDefence
        End Get
        Set(ByVal value As Integer)
            If BattleState.powerTrick AndAlso Not powerTrickRedirected Then
                AttackValue(True) = value
            End If
            BattleState.tempDefence = value
        End Set
    End Property
    Public Property SpeedValue() As Integer
        Get
            If BattleState.tempSpeed <> 0 Then Return BattleState.tempSpeed
            Return mSpeed
        End Get
        Set(ByVal value As Integer)
            BattleState.tempSpeed = value
        End Set
    End Property
    Public Property SpAttackValue() As Integer
        Get
            If BattleState.tempSAttack <> 0 Then Return BattleState.tempSAttack
            Return mSpAttack
        End Get
        Set(ByVal value As Integer)
            BattleState.tempSAttack = value
        End Set
    End Property
    Public Property SpDefenceValue() As Integer
        Get
            If BattleState.tempSDefence <> 0 Then Return BattleState.tempSDefence
            Return mSpDefence
        End Get
        Set(ByVal value As Integer)
            BattleState.tempSDefence = value
        End Set
    End Property

    Public ReadOnly Property HPIV() As Byte
        Get
            Return mHPIV
        End Get
    End Property
    Public ReadOnly Property AttackIV() As Byte
        Get
            Return mAttackIV
        End Get
    End Property
    Public ReadOnly Property DefenceIV() As Byte
        Get
            Return mDefenceIV
        End Get
    End Property
    Public ReadOnly Property SpeedIV() As Byte
        Get
            Return mSpeedIV
        End Get
    End Property
    Public ReadOnly Property SpAttackIV() As Byte
        Get
            Return mSpAttackIV
        End Get
    End Property
    Public ReadOnly Property SpDefenceIV() As Byte
        Get
            Return mSpDefenceIV
        End Get
    End Property
    Public ReadOnly Property AttackLV() As Integer
        Get
            Return BattleState.AttackLV
        End Get
    End Property
    Public ReadOnly Property DefenceLV() As Integer
        Get
            Return BattleState.DefenceLV
        End Get
    End Property
    Public ReadOnly Property SpeedLV() As Integer
        Get
            Return BattleState.SpeedLV
        End Get
    End Property
    Public ReadOnly Property SpAttackLV() As Integer
        Get
            Return BattleState.SpAttackLV
        End Get
    End Property
    Public ReadOnly Property SpDefenceLV() As Integer
        Get
            Return BattleState.SpDefenceLV
        End Get
    End Property

    Public Function AtkLVUp(ByVal upCount As Integer, Optional ByVal showFailInfo As Boolean = True) As Boolean
        If BattleState.AttackLV < 6 Then
            BattleState.AttackLV += upCount
            If BattleState.AttackLV > 6 Then BattleState.AttackLV = 6
            If upCount > 1 Then
                myTeam.log.AddText(GetNameString() & "物攻疯狂提升！")
            Else
                myTeam.log.AddText(GetNameString() & "物攻提升！")
            End If
            Return True
        Else
            If showFailInfo Then
                myTeam.log.AddText(GetNameString() & "的物攻已经到达颠峰")
            End If
            Return False
        End If
    End Function
    Public Function DefLVUp(ByVal upCount As Integer, Optional ByVal showFailInfo As Boolean = True) As Boolean
        If BattleState.DefenceLV < 6 Then
            BattleState.DefenceLV += upCount
            If BattleState.DefenceLV > 6 Then BattleState.DefenceLV = 6
            If upCount > 1 Then
                myTeam.log.AddText(GetNameString() & "物防疯狂提升！")
            Else
                myTeam.log.AddText(GetNameString() & "物防提升！")
            End If
            Return True
        Else
            If showFailInfo Then
                myTeam.log.AddText(GetNameString() & "的物防已经到达颠峰")
            End If
            Return False
        End If
    End Function
    Public Function SpeedLVUp(ByVal upCount As Integer, Optional ByVal showFailInfo As Boolean = True) As Boolean
        If BattleState.SpeedLV < 6 Then
            BattleState.SpeedLV += upCount
            If BattleState.SpeedLV > 6 Then BattleState.SpeedLV = 6
            If upCount > 1 Then
                myTeam.log.AddText(GetNameString() & "速度疯狂提升！")
            Else
                myTeam.log.AddText(GetNameString() & "速度提升！")
            End If
            Return True
        Else
            If showFailInfo Then
                myTeam.log.AddText(GetNameString() & "的速度已经到达颠峰")
            End If
            Return False
        End If
    End Function
    Public Function SAtkLVUp(ByVal upCount As Integer, Optional ByVal showFailInfo As Boolean = True) As Boolean
        If BattleState.SpAttackLV < 6 Then
            BattleState.SpAttackLV += upCount
            If BattleState.SpAttackLV > 6 Then BattleState.SpAttackLV = 6
            If upCount > 1 Then
                myTeam.log.AddText(GetNameString() & "特攻疯狂提升！")
            Else
                myTeam.log.AddText(GetNameString() & "特攻提升！")
            End If
            Return True
        Else
            If showFailInfo Then
                myTeam.log.AddText(GetNameString() & "的特攻已经到达颠峰")
            End If
            Return False
        End If
    End Function
    Public Function SDefLVUp(ByVal upCount As Integer, Optional ByVal showFailInfo As Boolean = True) As Boolean
        If BattleState.SpDefenceLV < 6 Then
            BattleState.SpDefenceLV += upCount
            If BattleState.SpDefenceLV > 6 Then BattleState.SpDefenceLV = 6
            If upCount > 1 Then
                myTeam.log.AddText(GetNameString() & "特防疯狂提升！")
            Else
                myTeam.log.AddText(GetNameString() & "特防提升！")
            End If
            Return True
        Else
            If showFailInfo Then
                myTeam.log.AddText(GetNameString() & "的特防已经到达颠峰")
            End If
            Return False
        End If
    End Function
    Public Function EvasionLVUp(ByVal upCount As Integer, Optional ByVal showFailInfo As Boolean = True) As Boolean
        If BattleState.evasionLV < 6 Then
            BattleState.evasionLV += upCount
            If BattleState.evasionLV > 6 Then BattleState.evasionLV = 6
            myTeam.log.AddText(GetNameString() & "回避提升！")
            Return True
        Else
            If showFailInfo Then
                myTeam.log.AddText(GetNameString() & "的回避已经到达颠峰")
            End If
            Return False
        End If
    End Function
    Public Function AccuracyLVUp(ByVal upCount As Integer, Optional ByVal showFailInfo As Boolean = True) As Boolean
        If BattleState.AccLV < 6 Then
            BattleState.AccLV += upCount
            If BattleState.AccLV > 6 Then BattleState.AccLV = 6
            myTeam.log.AddText(GetNameString() & "命中提升！")
            Return True
        Else
            If showFailInfo Then
                myTeam.log.AddText(GetNameString() & "的命中已经到达颠峰")
            End If
            Return False
        End If
    End Function


    Public Sub AtkLVDown(ByVal DownCount As Integer, ByVal target As Pokemon, Optional ByVal showFailed As Boolean = False)
        If Not AbilityDown(target, showFailed) Then Return
        AttackLVDown(DownCount, showFailed, target)
    End Sub
    Public Sub AtkLVDown(ByVal DownCount As Integer, ByVal selfAffect As Boolean, Optional ByVal showFailed As Boolean = False)
        If Not AbilityDown(selfAffect, showFailed) Then Return
        If selfAffect Then
            AttackLVDown(DownCount, showFailed, Me)
        Else
            AttackLVDown(DownCount, showFailed)
        End If
    End Sub
    Public Sub DefLVDown(ByVal DownCount As Integer, ByVal target As Pokemon, Optional ByVal showFailed As Boolean = False)
        If Not AbilityDown(target, showFailed) Then Return
        DefenceLVDown(DownCount, showFailed)
    End Sub
    Public Sub DefLVDown(ByVal DownCount As Integer, ByVal selfAffect As Boolean, Optional ByVal showFailed As Boolean = False)
        If Not AbilityDown(selfAffect, showFailed) Then Return
        DefenceLVDown(DownCount, showFailed)
    End Sub
    Public Sub SpeedLVDown(ByVal DownCount As Integer, ByVal target As Pokemon, Optional ByVal showFailed As Boolean = False)
        If Not AbilityDown(target, showFailed) Then Return
        SpeedLVDown(DownCount, showFailed)
    End Sub
    Public Sub SpeedLVDown(ByVal DownCount As Integer, ByVal selfAffect As Boolean, ByVal showFailed As Boolean)
        If Not AbilityDown(selfAffect, showFailed) Then Return
        SpeedLVDown(DownCount, showFailed)
    End Sub
    Public Sub SAtkLVDown(ByVal DownCount As Integer, ByVal target As Pokemon, Optional ByVal showFailed As Boolean = False)
        If Not AbilityDown(target, showFailed) Then Return
        SAttackLVDown(DownCount, showFailed)
    End Sub
    Public Sub SAtkLVDown(ByVal DownCount As Integer, ByVal selfAffect As Boolean, Optional ByVal showFailed As Boolean = False)
        If Not AbilityDown(selfAffect, showFailed) Then Return
        SAttackLVDown(DownCount, showFailed)
    End Sub
    Public Sub SDefLVDown(ByVal DownCount As Integer, ByVal target As Pokemon, Optional ByVal showFailed As Boolean = False)
        If Not AbilityDown(target, showFailed) Then Return
        SDefenceLVDown(DownCount, showFailed)
    End Sub
    Public Sub SDefLVDown(ByVal DownCount As Integer, ByVal selfAffect As Boolean, Optional ByVal showFailed As Boolean = False)
        If Not AbilityDown(selfAffect, showFailed) Then Return
        SDefenceLVDown(DownCount, showFailed)
    End Sub

    Private Sub AttackLVDown(ByVal DownCount As Integer, ByVal showFailed As Boolean, Optional ByVal target As Pokemon = Nothing)
        If SelTrait = Trait.怪力钳 AndAlso target IsNot Me AndAlso (target Is Nothing OrElse target.SelTrait <> Trait.破格) Then
            If showFailed Then myTeam.log.AddText(GetNameString() & "的怪力钳特性防止了攻击下降")
            Return
        End If
        If myTeam.Mist Then Return
        If BattleState.AttackLV > -6 Then
            BattleState.AttackLV -= DownCount
            If BattleState.AttackLV < -6 Then BattleState.AttackLV = -6
            If DownCount > 1 Then
                myTeam.log.AddText(GetNameString() & "物攻疯狂下降！")
            Else
                myTeam.log.AddText(GetNameString() & "物攻下降！")
            End If
            If Item = Item.白色香草 AndAlso BattleState.AttackLV < 0 Then RaiseItem()
        Else
            If showFailed Then myTeam.log.AddText(GetNameString() & "的物攻已经无法下降")
        End If
    End Sub
    Private Sub DefenceLVDown(ByVal DownCount As Integer, ByVal showFailed As Boolean)
        If myTeam.Mist Then Return
        If BattleState.DefenceLV > -6 Then
            BattleState.DefenceLV -= DownCount
            If BattleState.DefenceLV < -6 Then BattleState.DefenceLV = -6
            If DownCount > 1 Then
                myTeam.log.AddText(GetNameString() & "物防疯狂下降！")
            Else
                myTeam.log.AddText(GetNameString() & "物防下降！")
            End If
            If Item = Item.白色香草 AndAlso BattleState.DefenceLV < 0 Then RaiseItem()
        Else
            If showFailed Then myTeam.log.AddText(GetNameString() & "的物防已经无法下降")
        End If
    End Sub
    Private Sub SpeedLVDown(ByVal DownCount As Integer, ByVal showFailed As Boolean)
        If myTeam.Mist Then Return
        If BattleState.SpeedLV > -6 Then
            BattleState.SpeedLV -= DownCount
            If BattleState.SpeedLV < -6 Then BattleState.SpeedLV = -6
            If DownCount > 1 Then
                myTeam.log.AddText(GetNameString() & "速度疯狂下降！")
            Else
                myTeam.log.AddText(GetNameString() & "速度下降！")
            End If
            If Item = Item.白色香草 AndAlso BattleState.SpeedLV < 0 Then RaiseItem()
        Else
            If showFailed Then myTeam.log.AddText(GetNameString() & "的速度已经无法下降")
        End If
    End Sub
    Private Sub SAttackLVDown(ByVal DownCount As Integer, ByVal showFailed As Boolean)
        If myTeam.Mist Then Return
        If BattleState.SpAttackLV > -6 Then
            BattleState.SpAttackLV -= DownCount
            If BattleState.SpAttackLV < -6 Then BattleState.SpAttackLV = -6
            If DownCount > 1 Then
                myTeam.log.AddText(GetNameString() & "特攻疯狂下降！")
            Else
                myTeam.log.AddText(GetNameString() & "特攻下降！")
            End If
            If Item = Item.白色香草 AndAlso BattleState.SpAttackLV < 0 Then RaiseItem()
        Else
            If showFailed Then myTeam.log.AddText(GetNameString() & "的特攻已经无法下降")
        End If
    End Sub
    Private Sub SDefenceLVDown(ByVal DownCount As Integer, ByVal showFailed As Boolean)
        If myTeam.Mist Then Return
        If BattleState.SpDefenceLV > -6 Then
            BattleState.SpDefenceLV -= DownCount
            If BattleState.SpDefenceLV < -6 Then BattleState.SpDefenceLV = -6
            If DownCount > 1 Then
                myTeam.log.AddText(GetNameString() & "特防疯狂下降！")
            Else
                myTeam.log.AddText(GetNameString() & "特防下降！")
            End If
            If Item = Item.白色香草 AndAlso BattleState.SpDefenceLV < 0 Then RaiseItem()
        Else
            If showFailed Then myTeam.log.AddText(GetNameString() & "的特防已经无法下降")
        End If
    End Sub

    Private Function AbilityDown(ByVal target As Pokemon, ByVal showFailed As Boolean) As Boolean
        If target.SelTrait = Trait.破格 OrElse (SelTrait <> Trait.白烟 AndAlso SelTrait <> Trait.净体) Then
            Return True
        ElseIf showFailed Then
            If SelTrait = Trait.白烟 Then
                myTeam.log.AddText(GetNameString() & "的白烟特性防止了能力下降")
            Else
                myTeam.log.AddText(GetNameString() & "的净体特性防止了能力下降")
            End If
        End If

        Return False
    End Function
    Private Function AbilityDown(ByVal selfAffect As Boolean, ByVal showFailed As Boolean) As Boolean
        If selfAffect OrElse (SelTrait <> Trait.白烟 AndAlso SelTrait <> Trait.净体) Then
            Return True
        ElseIf showFailed Then
            If SelTrait = Trait.白烟 Then
                myTeam.log.AddText(GetNameString() & "的白烟特性防止了能力下降")
            Else
                myTeam.log.AddText(GetNameString() & "的净体特性防止了能力下降")
            End If
        End If
        Return False
    End Function

    Public ReadOnly Property Evasion(ByVal target As Pokemon, ByVal ignoreEvasionLvUp As Boolean) As Double
        Get
            Dim evaLV As Integer = BattleState.evasionLV
            If SelTrait = Trait.单纯 AndAlso target.SelTrait <> Trait.破格 Then evaLV *= 2
            If evaLV > 6 Then evaLV = 6
            If evaLV < -6 Then evaLV = -6

            If ignoreEvasionLvUp AndAlso evaLV > 0 Then
                evaLV = 0
            End If

            Dim accLV As Integer = target.BattleState.AccLV
            If target.SelTrait = Trait.单纯 Then accLV *= 2
            If accLV > 6 Then accLV = 6
            If accLV < -6 Then accLV = -6

            Dim lv As Integer = evaLV - accLV
            If BattleState.miracleEye OrElse BattleState.foresight Then lv = -accLV

            If myTeam.ground.Unaware Then lv = 0
            Select Case lv
                Case Is > 0
                    Return 3 / (lv + 3)
                Case Is < 0
                    Return (3 - lv) / 3
                Case Else
                    Return 1
            End Select
        End Get
    End Property
    Public ReadOnly Property CTOdds(ByVal MoveEffect As Boolean) As Double
        Get
            Dim lv As Integer = BattleState.CTLV
            If SelTrait = Trait.幸运 Then lv += 1
            If (Item = Item.幸运拳套 AndAlso NO = 113) OrElse (Item = Item.长葱 AndAlso NO = 83) _
                Then
                lv += 2
            ElseIf Item = Item.聚焦镜 OrElse Item = Item.尖锐爪 Then
                lv += 1
            End If
            If MoveEffect Then lv += 1
            Select Case lv
                Case 1
                    Return 0.125
                Case 2
                    Return 0.25
                Case 3
                    Return 0.33333
                Case Is >= 4
                    Return 0.5
            End Select
            Return 0.0625
        End Get
    End Property
    Public Function CTLVUp(ByVal upCount As Integer) As Boolean
        If BattleState.CTLV < 4 Then
            BattleState.CTLV += upCount
            If BattleState.CTLV > 4 Then BattleState.CTLV = 4
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub EvasionLVDown(ByVal DownCount As Integer, ByVal target As Pokemon, ByVal showFailed As Boolean)
        If Not AbilityDown(target, showFailed) Then Return
        If BattleState.evasionLV > -6 Then
            BattleState.evasionLV -= DownCount
            If BattleState.evasionLV < -6 Then BattleState.evasionLV = -6
            myTeam.log.AddText(GetNameString() & "回避下降！")
            If Item = Item.白色香草 AndAlso BattleState.evasionLV < 0 Then RaiseItem()
        Else
            myTeam.log.AddText(GetNameString() & "的回避已经无法下降")
        End If
    End Sub
    Public Sub AccLVDown(ByVal DownCount As Integer, ByVal target As Pokemon, ByVal showFailed As Boolean)
        If Not AbilityDown(target, showFailed) Then Return
        If SelTrait = Trait.锐利眼光 AndAlso (target Is Nothing OrElse target.SelTrait <> Trait.破格) Then
            myTeam.log.AddText(GetNameString() & "的锐利眼光特性防止了命中下降")
            Return
        End If
        If BattleState.AccLV > -6 Then
            BattleState.AccLV -= DownCount
            If BattleState.AccLV < -6 Then BattleState.AccLV = -6
            myTeam.log.AddText(GetNameString() & "命中下降！")
            If Item = Item.白色香草 AndAlso BattleState.AccLV < 0 Then RaiseItem()
        Else
            myTeam.log.AddText(GetNameString() & "的命中已经无法下降")
        End If
    End Sub

    Public Property Item(Optional ByVal ignoreInvalid As Boolean = False) As Item
        Get
            If myTeam.ground.MagicRoom Then
                Return Item.无
            End If
            If (BattleState.detain OrElse SelTrait = Trait.不用武器) AndAlso Not ignoreInvalid Then Return Item.无
            Return mItem
        End Get
        Set(ByVal value As Item)
            mItem = value
            If SelTrait(True) = Trait.走钢丝 Then
                If value = Item.无 Then
                    BattleState.traitRaised = True
                Else
                    BattleState.traitRaised = False
                End If
            End If
        End Set
    End Property
    Public ReadOnly Property ChoiceItem() As Boolean
        Get
            Return Item = Item.专爱头巾 OrElse Item = Item.专爱围巾 OrElse Item = Item.专爱眼镜
        End Get
    End Property
    Public Property UsedItem() As Item
        Get
            Return mUsedItem
        End Get
        Set(ByVal value As Item)
            mUsedItem = value
        End Set
    End Property
    Public Property Type1() As PokemonType
        Get
            If BattleState.tempType IsNot Nothing Then
                If BattleState.roost AndAlso BattleState.tempType.Name = "飞行" Then Return BattleState.roostTemp
                Return BattleState.tempType
            End If
            If BattleState.roost AndAlso mType1.Name = "飞行" Then Return BattleState.roostTemp
            Return mType1
        End Get
        Set(ByVal value As PokemonType)
            BattleState.tempType = value
        End Set
    End Property
    Public Property Type2() As PokemonType
        Get
            If BattleState.tempType2 IsNot Nothing Then
                If BattleState.roost AndAlso BattleState.tempType2.Name = "飞行" Then Return BattleState.roostTemp
                Return BattleState.tempType2
            ElseIf BattleState.tempType IsNot Nothing Then
                Return Nothing
            End If
            If mType2 IsNot Nothing AndAlso BattleState.roost AndAlso mType2.Name = "飞行" Then Return BattleState.roostTemp
            Return mType2
        End Get
        Set(ByVal value As PokemonType)
            BattleState.tempType2 = value
        End Set
    End Property

    Public Property SelTrait(Optional ByVal ignoreGastricJuice As Boolean = False) As Trait
        Get
            If BattleState.gastricJuice AndAlso Not ignoreGastricJuice Then Return Trait.无
            If BattleState.worrySeed Then Return Trait.不眠
            If BattleState.tempTrait <> Trait.无 Then Return BattleState.tempTrait
            Return mSelTrait
        End Get
        Set(ByVal value As Trait)
            BattleState.tempTrait = value
            BattleState.traitRaised = False
        End Set
    End Property
    Public ReadOnly Property SelMove(ByVal index As Integer) As Move
        Get
            If BattleState.tempMoves(index - 1) IsNot Nothing Then
                Return BattleState.tempMoves(index - 1)
            End If
            If BattleState.transfrom Then Return Nothing
            Return mSelMoves(index - 1)
        End Get
    End Property
    Public ReadOnly Property SelMoveName(ByVal index As Integer) As String
        Get
            Dim move As Move = SelMove(index)
            If move IsNot Nothing Then
                Return move.Name
            End If
            Return String.Empty
        End Get
    End Property

    Public Sub GetHurt(ByVal damageValue As Double)
        If damageValue > 0 Then
            Dim damage As Integer = Convert.ToInt32(Math.Truncate(damageValue))
            If damage = 0 Then damage = 1
            mHP -= damage
            If mHP < 0 Then mHP = 0
            If mHP = 0 Then
                State = ModShare.PokemonState.No
                myTeam.log.AddText(GetNameString() & "倒下了！")
                RaiseEvent Died(Me, EventArgs.Empty)
            Else
                If Item = Item.果实7 OrElse (Item >= Item.果实10 AndAlso Item <= Item.果实15) Then
                    RaiseItem()
                ElseIf (Item >= Item.果实53 AndAlso Item <= Item.果实59) OrElse Item = Item.果实61 _
                    Then
                    RaiseItem()
                End If
                RaiseEvent HPChange(Me, EventArgs.Empty)
            End If
        End If
    End Sub

    Public Sub HPRecover(ByVal recovery As Double)
        Dim Recover As Integer = Convert.ToInt32(Math.Truncate(recovery))
        If Recover < 1 Then Recover = 1
        mHP += Recover
        If mHP > MAXHP Then mHP = MAXHP
        RaiseEvent HPChange(Me, EventArgs.Empty)
    End Sub

    Public Function CheckIfHPRecoveryIsEnabled() As Boolean
        If BattleState.healBlock Then
            myTeam.log.AddText(GetNameString() & "被禁止了回复")
            Return False
        End If
        Return True
    End Function

    Public Sub SetHP(ByVal hp As Integer)
        mHP = hp
        If mHP > MAXHP Then mHP = MAXHP
        RaiseEvent HPChange(Me, EventArgs.Empty)
    End Sub

    Public Function HaveType(ByVal typeName As String) As Boolean
        If Type1.Name = typeName Then Return True
        If Type2 IsNot Nothing AndAlso Type2.Name = typeName Then Return True
        Return False
    End Function
    Public Function HaveMove(ByVal moveName As String) As Boolean
        For i As Integer = 1 To 4
            If SelMoveName(i) = moveName Then Return True
        Next
        Return False
    End Function
    Public Sub SetMove(ByVal index As Integer, ByVal moveValue As Move)
        If 0 < index AndAlso index < 5 Then
            mSelMoves(index - 1) = moveValue
            UpdateMove(moveValue)
        End If
    End Sub
    Public Sub SetTempMove(ByVal index As Integer, ByVal moveValue As Move)
        If 0 < index AndAlso index < 5 Then
            BattleState.tempMoves(index - 1) = moveValue
            If moveValue IsNot Nothing Then UpdateMove(moveValue)
        End If
    End Sub
    Public Sub UpdateMove(ByVal moveValue As Move)
        If myTeam IsNot Nothing Then
            If moveValue.AddEff2 = MoveAdditionalEffect.觉醒力量 Then
                moveValue.SetType(BattleData.GetTypeData(GetHiddenType(Me)))
                moveValue.SetPower(GetHiddenPower(Me))
            ElseIf moveValue.AddEff2 = MoveAdditionalEffect.制裁飞石 Then
                If Item.格斗属性石板 <= Item(True) AndAlso Item(True) <= Item.恶属性石板 Then
                    moveValue.SetType(BattleData.GetTypeData(TypeIndex(Item - 64)))
                Else
                    moveValue.SetType(BattleData.GetTypeData("普通"))
                End If
            ElseIf moveValue.AddEff2 = MoveAdditionalEffect.自然恩惠 Then
                If Item.果实1 <= Item(True) AndAlso Item(True) <= Item.果实64 Then
                    moveValue.SetPower(GetNaturalGiftPower(Item(True)))
                    moveValue.SetType(BattleData.GetTypeData(GetNaturalGiftType(Item(True))))
                Else
                    moveValue.SetType(BattleData.GetTypeData("普通"))
                    moveValue.SetPower(0)
                End If
            ElseIf moveValue.AddEff2 = MoveAdditionalEffect.科技爆破 Then
                Select Case Item(True)
                    Case PokemonBattle.PokemonData.Item.海洋卡带
                        moveValue.SetType(BattleData.GetTypeData("水"))
                    Case PokemonBattle.PokemonData.Item.雷电卡带
                        moveValue.SetType(BattleData.GetTypeData("电"))
                    Case PokemonBattle.PokemonData.Item.火焰卡带
                        moveValue.SetType(BattleData.GetTypeData("火"))
                    Case PokemonBattle.PokemonData.Item.冰冻卡带
                        moveValue.SetType(BattleData.GetTypeData("冰"))
                    Case Else
                        moveValue.SetType(BattleData.GetTypeData("普通"))
                End Select
            ElseIf moveValue.Effect = MoveEffect.诅咒 Then
                moveValue.UpdateCourse(Me)
            End If
        End If
    End Sub
    Public Sub SetType(ByVal index As Integer, ByVal type As PokemonType)
        Select Case index
            Case 1
                mType1 = type
            Case 2
                mType2 = type
        End Select
    End Sub

    Public Sub Transfrom(ByVal pm As Pokemon)
        BattleState.transfrom = True
        Dim newData As PokemonData = BattleData.GetPokemon(pm._identity)
        BattleState.transTemp = newData

        AttackValue = pm.AttackValue
        DefenceValue = pm.DefenceValue
        SpAttackValue = pm.SpAttackValue
        SpDefenceValue = pm.SpDefenceValue
        SpeedValue = pm.SpeedValue
        For i As Integer = 1 To 4
            If pm.SelMove(i) Is Nothing Then
                SetTempMove(i, Nothing)
                Continue For
            End If
            Dim move As Move = New Move(BattleData.GetMove(pm.SelMove(i).Identity), False)
            move.SetPP(5)
            SetTempMove(i, move)
        Next
        BattleState.AttackLV = pm.AttackLV
        BattleState.DefenceLV = pm.DefenceLV
        BattleState.SpDefenceLV = pm.SpDefenceLV
        BattleState.SpDefenceLV = pm.SpDefenceLV
        BattleState.SpeedLV = pm.SpeedLV
        BattleState.evasionLV = pm.BattleState.evasionLV
        Type1 = pm.Type1
        Type2 = pm.Type2
        SetTrait(pm.SelTrait(True))
        If UsedItem = Item.专爱头巾 OrElse UsedItem = Item.专爱围巾 OrElse UsedItem = Item.专爱眼镜 Then UsedItem = Item.无
        RaiseEvent ImageChange(Me, EventArgs.Empty)
    End Sub

    Public Sub ShapeShift(ByVal pmData As PokemonData)
        BattleState.shapeShift = True
        BattleState.transTemp = pmData
        Type1 = BattleData.GetTypeData(pmData.Type1)
        Type2 = BattleData.GetTypeData(pmData.Type2)

        mAttack = ComputeAbility(pmData.AttackBase, mAttackEV, mAttackIV, mAttackDV, mLV)
        mDefence = ComputeAbility(pmData.DefenceBase, mDefenceEV, mDefenceIV, mDefenceDV, mLV)
        mSpeed = ComputeAbility(pmData.SpeedBase, mSpeedEV, mSpeedIV, mSpeedDV, mLV)
        mSpAttack = ComputeAbility(pmData.SpAttackBase, mSpAttackEV, mSpAttackIV, mSpAttackDV, mLV)
        mSpDefence = ComputeAbility(pmData.SpDefenceBase, mSpDefenceEV, mSpDefenceIV, mSpDefenceDV, mLV)
        RaiseEvent ImageChange(Me, EventArgs.Empty)
    End Sub

    Private Function CanSetState(ByVal stateValue As PokemonState, ByVal target As Pokemon, _
        ByVal showTrait As Boolean, ByRef traitShowed As Boolean) As Boolean
        If State <> ModShare.PokemonState.No Then Return False
        Select Case stateValue
            Case PokemonState.Poison
                If HaveType("毒") OrElse HaveType("钢") Then Return False
                If SelTrait = Trait.免疫 AndAlso (target Is Nothing OrElse target.SelTrait <> Trait.破格) Then
                    If showTrait Then
                        myTeam.log.AddText(GetNameString() & "的免疫特性防止了中毒")
                        traitShowed = True
                        Return True
                    Else
                        Return False
                    End If
                End If
            Case PokemonState.Toxin
                If HaveType("毒") OrElse HaveType("钢") Then Return False
                If SelTrait = Trait.免疫 AndAlso (target Is Nothing OrElse target.SelTrait <> Trait.破格) Then
                    If showTrait Then
                        myTeam.log.AddText(GetNameString() & "的免疫特性防止了中毒")
                        traitShowed = True
                        Return True
                    Else
                        Return False
                    End If
                End If
            Case PokemonState.Burn
                If HaveType("火") Then Return False
                If SelTrait = Trait.水幕 AndAlso (target Is Nothing OrElse target.SelTrait <> Trait.破格) Then
                    If showTrait Then
                        myTeam.log.AddText(GetNameString() & "的水幕特性防止了烧伤")
                        traitShowed = True
                        Return True
                    Else
                        Return False
                    End If
                End If
            Case PokemonState.Freeze
                If HaveType("冰") OrElse myTeam.ground.Weather = Weather.Sunny Then Return False
                For Each pm As Pokemon In myTeam.Pokemon
                    If pm IsNot Nothing AndAlso pm.State = PokemonState.Freeze Then
                        myTeam.log.AddText(GetNameString() & "的队伍已有精灵被冻结", Color.Orange)
                        Return False
                    End If
                Next
                If SelTrait = Trait.熔岩盔甲 AndAlso (target Is Nothing OrElse target.SelTrait <> Trait.破格) Then
                    If showTrait Then
                        myTeam.log.AddText(GetNameString() & "的熔岩盔甲特性防止了冻结")
                        traitShowed = True
                        Return True
                    Else
                        Return False
                    End If
                End If
            Case PokemonState.Sleep
                If myTeam.ground.Uproar Then Return False
                For Each pm As Pokemon In myTeam.Pokemon
                    If pm IsNot Nothing AndAlso pm.hypnosis Then
                        myTeam.log.AddText(GetNameString() & "的队伍已有精灵被催眠", Color.Orange)
                        Return False
                    End If
                Next
                If (SelTrait = Trait.不眠 OrElse SelTrait = Trait.活跃) _
                    AndAlso (target Is Nothing OrElse target.SelTrait <> Trait.破格) Then
                    If showTrait Then
                        If SelTrait = Trait.不眠 Then
                            myTeam.log.AddText(GetNameString() & "的不眠特性无法被催眠")
                        Else
                            myTeam.log.AddText(GetNameString() & "的活跃特性无法被催眠")
                        End If
                        traitShowed = True
                        Return True
                    Else
                        Return False
                    End If
                End If
            Case PokemonState.Paralysis
                If SelTrait = Trait.柔软 AndAlso (target Is Nothing OrElse target.SelTrait <> Trait.破格) Then
                    If showTrait Then
                        myTeam.log.AddText(GetNameString() & "的柔软特性防止了麻痹")
                        traitShowed = True
                        Return True
                    Else
                        Return False
                    End If
                End If
        End Select
        Return True
    End Function
    Public Function SetState(ByVal stateValue As PokemonState, Optional ByVal target As Pokemon = Nothing, _
        Optional ByVal addlog As Boolean = True, _
        Optional ByVal poisonSpike As Boolean = False, Optional ByVal itemEff As Boolean = False, _
        Optional ByVal synchronize As Boolean = False, Optional ByVal showTraitEff As Boolean = False) As Boolean

        If myTeam.Safeguard AndAlso Not itemEff Then Return False
        If SelTrait = Trait.叶子守护 AndAlso myTeam.ground.Weather = Weather.Sunny AndAlso Not itemEff Then Return False

        Dim traitShowed As Boolean
        If Not CanSetState(stateValue, target, showTraitEff, traitShowed) Then Return False
        If showTraitEff AndAlso traitShowed Then Return True

        If synchronize Then
            myTeam.log.AddText(GetNameString() & "被传染了！")
            If stateValue = PokemonState.Toxin Then stateValue = PokemonState.Poison
        ElseIf poisonSpike Then
            myTeam.log.AddText(GetNameString() & "因毒菱而中毒了")
        End If
        SetState(stateValue, addlog, target)
        If itemEff Then
            If stateValue = PokemonState.Toxin Then
                myTeam.log.AddText(GetNameString() & "的剧毒珠使它中毒了！")
                BattleState.toxinCounter = 0
            ElseIf stateValue = PokemonState.Burn Then
                myTeam.log.AddText(GetNameString() & "的火珠使它烧伤了！")
            End If
        End If
        Return True
    End Function
    Private Sub SetState(ByVal stateValue As PokemonState, ByVal addLog As Boolean, ByVal target As Pokemon)
        State = stateValue
        Select Case stateValue
            Case ModShare.PokemonState.Poison
                If addLog Then myTeam.log.AddText(GetNameString() & "中毒了！")
                If Item = Item.果实3 Then RaiseItem()
            Case ModShare.PokemonState.Toxin
                If addLog Then myTeam.log.AddText(GetNameString() & "中毒了！")
                If Item = Item.果实3 Then RaiseItem()
                BattleState.toxinCounter = 0
            Case ModShare.PokemonState.Burn
                If addLog Then myTeam.log.AddText(GetNameString() & "烧伤了！")
                If Item = Item.果实4 Then RaiseItem()
            Case ModShare.PokemonState.Paralysis
                If addLog Then myTeam.log.AddText(GetNameString() & "麻痹了！")
                If Item = Item.果实1 Then RaiseItem()
            Case ModShare.PokemonState.Freeze
                If addLog Then myTeam.log.AddText(GetNameString() & "冻结了！")
                If Item = Item.果实5 Then RaiseItem()
            Case ModShare.PokemonState.Sleep
                If addLog Then myTeam.log.AddText(GetNameString() & "睡着了！")
                Dim turn As Integer = myTeam.random.Next(Battle.MinSleepTurn, Battle.MaxSleepTurn + 1)
                sleepTurn = turn
                sleepCounter = 0
                hypnosis = True
                If SelTrait = Trait.早起 Then sleepTurn \= 2
                If Item = Item.果实2 Then RaiseItem()
        End Select
        If Item = Item.果实9 Then RaiseItem()
        If SelTrait = Trait.同步率 AndAlso target IsNot Nothing AndAlso stateValue <> PokemonState.Freeze AndAlso _
            stateValue <> PokemonState.Sleep Then
            target.SetState(stateValue, , , , , True)
        End If
    End Sub
    Public Sub SetItem(ByVal itemValue As Item)
        If mUsedItem = Item.专爱头巾 OrElse mUsedItem = Item.专爱眼镜 OrElse mUsedItem = Item.专爱围巾 Then
            mUsedItem = Item.无
            If itemValue = Item.专爱头巾 OrElse itemValue = Item.专爱眼镜 OrElse itemValue = Item.专爱围巾 Then
                mUsedItem = itemValue
            End If
        End If

        If itemValue <> Item.无 Then myTeam.log.AddText(GetNameString() & "得到了" & itemValue.ToString & "！")
        Item = itemValue
        If (Item.果实1 <= Item(True) AndAlso Item(True) <= Item.果实15) OrElse _
            (Item.果实53 <= Item(True) AndAlso Item(True) <= Item.果实59) OrElse _
            Item(True) = Item.果实61 OrElse Item(True) = Item.果实62 OrElse _
            Item(True) = Item.精神香草 OrElse Item(True) = Item.白色香草 Then
            RaiseItem()
        End If

        For i As Integer = 1 To 4
            If SelMove(i) Is Nothing Then Continue For
            If SelMove(i).AddEff2 = MoveAdditionalEffect.制裁飞石 Then
                UpdateMove(SelMove(i))
            ElseIf SelMove(i).AddEff2 = MoveAdditionalEffect.自然恩惠 Then
                UpdateMove(SelMove(i))
            ElseIf SelMove(i).AddEff2 = MoveAdditionalEffect.科技爆破 Then
                UpdateMove(SelMove(i))
            End If
        Next
        If SelTrait = Trait.多重属性 Then RaiseTrait()
    End Sub
    Public Function CanUseMove(ByVal index As Integer, Optional ByVal addLog As Boolean = False) As Boolean
        If Not addLog AndAlso BattleState.torment AndAlso lastMoveIndex = index AndAlso index <> Battle.StruggleIndex Then
            Return False
        End If
        If BattleState.taunt AndAlso SelMove(index).MoveType = MoveType.其他 Then
            If addLog Then myTeam.log.AddText(GetNameString() & "被挑拨不能使用" & SelMove(index).Name)
            Return False
        End If
        If BattleState.disableIndex = index Then
            If addLog Then myTeam.log.AddText(GetNameString() & "无法使用" & SelMove(index).Name)
            Return False
        End If
        For Each pm As Pokemon In myTeam.opponentTeam.SelectedPokemon
            With pm
                If .BattleState.imprison AndAlso .HaveMove(SelMoveName(index)) Then
                    If addLog Then myTeam.log.AddText(GetNameString() & "被封印无法使用" & SelMove(index).Name)
                    Return False
                End If
            End With
        Next
        Return True
    End Function
    Public Function NoMoveCanUse() As Boolean
        If (BattleState.encore OrElse (Item.专爱围巾 <= mUsedItem AndAlso mUsedItem <= Item.专爱眼镜)) _
            AndAlso lastMoveIndex = Battle.StruggleIndex Then Return True
        Dim canUse As Boolean() = New Boolean(3) {True, True, True, True}
        For i As Integer = 1 To 4
            If SelMove(i) Is Nothing Then canUse(i - 1) = False : Continue For
            If SelMove(i).PP = 0 Then canUse(i - 1) = False : Continue For
            If (BattleState.encore OrElse (Item.专爱围巾 <= mUsedItem AndAlso mUsedItem <= Item.专爱眼镜)) _
                AndAlso (lastMoveIndex <> i OrElse SelMove(i).PP = 0) Then canUse(i - 1) = False : Continue For
            If Not CanUseMove(i) Then canUse(i - 1) = False : Continue For
        Next
        For i As Integer = 0 To 3
            If canUse(i) Then Return False
        Next
        Return True
    End Function
    Public ReadOnly Property CanChooseMove() As Boolean
        Get
            If BattleState.Hide OrElse BattleState.bide OrElse BattleState.nextTurnCantMove OrElse BattleState.CrazyAttack _
                OrElse BattleState.prepare Then
                Return False
            End If
            Return True
        End Get
    End Property

    Public Sub RaiseTrait()
        If myTeam Is Nothing Then Return
        Select Case SelTrait
            Case Trait.威吓
                For Each target As Pokemon In myTeam.opponentTeam.SelectedPokemon
                    If Not (target.BattleState.substituted OrElse target.HP = 0) Then _
                        myTeam.log.AddText(GetNameString() & "的威吓特性发动了！") : BattleState.intimidated = True : target.AtkLVDown(1, Me)
                Next
            Case Trait.不眠
                If State = ModShare.PokemonState.Sleep Then _
                    myTeam.log.AddText(GetNameString() & "醒了过来！") : State = ModShare.PokemonState.No
            Case Trait.免疫
                If State = ModShare.PokemonState.Poison OrElse State = ModShare.PokemonState.Toxin Then _
                    myTeam.log.AddText(GetNameString() & "解除了中毒状态！") : State = ModShare.PokemonState.No
            Case Trait.水幕
                If State = ModShare.PokemonState.Burn Then _
                    myTeam.log.AddText(GetNameString() & "解除了烧伤状态！") : State = ModShare.PokemonState.No
            Case Trait.熔岩盔甲
                If State = ModShare.PokemonState.Freeze Then _
                    myTeam.log.AddText(GetNameString() & "解冻了！") : State = ModShare.PokemonState.No
            Case Trait.活跃
                If State = ModShare.PokemonState.Sleep Then _
                    myTeam.log.AddText(GetNameString() & "醒了过来！") : State = ModShare.PokemonState.No
            Case Trait.柔软
                If State = ModShare.PokemonState.Paralysis Then _
                    myTeam.log.AddText(GetNameString() & "解除了麻痹状态！") : State = ModShare.PokemonState.No
            Case Trait.自我中心
                If BattleState.confused Then _
                    myTeam.log.AddText(GetNameString() & "解除了混乱！") : BattleState.confused = False
            Case Trait.迟钝
                If BattleState.captivated Then _
                    myTeam.log.AddText(GetNameString() & "解除了着迷状态！") : BattleState.captivated = False : BattleState.captivateTarget = Nothing
            Case Trait.降雨
                myTeam.ground.NewWeather(Weather.Rainy, Me, myTeam.log, True)
            Case Trait.日照
                myTeam.ground.NewWeather(Weather.Sunny, Me, myTeam.log, True)
            Case Trait.沙尘暴
                myTeam.ground.NewWeather(Weather.SandStorm, Me, myTeam.log, True)
            Case Trait.暴雪
                myTeam.ground.NewWeather(Weather.HailStorm, Me, myTeam.log, True)
            Case Trait.偷懒
                BattleState.traitRaised = False
            Case Trait.压力
                myTeam.log.AddText(GetNameString() & "向对手施加了压力！")
            Case Trait.缓慢启动
                BattleState.slowStartCounter = 5
            Case Trait.润湿身体
                If State <> PokemonState.No AndAlso myTeam.ground.Weather = Weather.Rainy Then
                    State = ModShare.PokemonState.No
                    myTeam.log.AddText(GetNameString() & "的润湿身体特性使它恢复了状态")
                End If
            Case Trait.自然恢复
                State = ModShare.PokemonState.No
            Case Trait.多重属性
                If 64 < Item AndAlso Item < 81 Then
                    SetType(1, BattleData.GetTypeData(TypeIndex(Item - 64)))
                Else
                    SetType(1, BattleData.GetTypeData("普通"))
                End If
            Case Trait.蓄水
                myTeam.log.AddText(GetNameString() & "的蓄水特性吸收了攻击！")
                HPRecover(MAXHP * 0.25)
            Case Trait.吸水
                myTeam.log.AddText(GetNameString() & "的吸水特性吸收了攻击！")
                SAtkLVUp(1, False)
            Case Trait.干燥皮肤
                myTeam.log.AddText(GetNameString() & "的干燥皮肤特性吸收了攻击！")
                HPRecover(MAXHP * 0.25)
            Case Trait.蓄电
                myTeam.log.AddText(GetNameString() & "的蓄电特性吸收了攻击！")
                HPRecover(MAXHP * 0.25)
            Case Trait.避雷针
                myTeam.log.AddText(GetNameString() & "的避雷针特性吸收了攻击！")
                SAtkLVUp(1, False)

            Case Trait.引火
                myTeam.log.AddText(GetNameString() & "的引火特性吸收了攻击！")
                BattleState.traitRaised = True
            Case Trait.电力引擎
                myTeam.log.AddText(GetNameString() & "的电力引擎特性吸收了攻击！")
                SpeedLVUp(1, False)
            Case Trait.太阳能
                If myTeam.ground.Weather = Weather.Sunny Then
                    myTeam.log.AddText(GetNameString() & "吸收了阳光的能量！")
                    GetHurt(MAXHP * 0.125)
                End If
            Case Trait.冰之躯体
                If myTeam.ground.Weather = Weather.HailStorm AndAlso HP <> MAXHP Then
                    myTeam.log.AddText(GetNameString() & "的冰之躯体特性使它回复了HP！")
                    HPRecover(MAXHP * 0.0625)
                End If
            Case Trait.接雨盘
                If myTeam.ground.Weather = Weather.Rainy AndAlso HP <> MAXHP Then
                    myTeam.log.AddText(GetNameString() & "的接雨盘特性使它回复了HP！")
                    HPRecover(MAXHP * 0.0625)
                End If
            Case Trait.蜕皮
                If State <> PokemonState.No AndAlso myTeam.random.NextDouble < 0.3 Then
                    State = PokemonState.No
                    myTeam.log.AddText(GetNameString() & "的蜕皮特性使它恢复了状态")
                End If
            Case Trait.加速
                If SpeedLV < 6 AndAlso BattleState.stayOneTurn Then
                    SpeedLVUp(1, False)
                End If
            Case Trait.愤怒神经
                AtkLVUp(12, False)

            Case Trait.下载
                Dim targets As Pokemon() = myTeam.opponentTeam.AlivePokemon
                If targets.Length > 0 Then
                    Dim target As Pokemon = targets(myTeam.random.Next(0, targets.Length))
                    If target.Defence() < target.SpDefence Then
                        AtkLVUp(1, False)
                    ElseIf target.Defence() >= target.SpDefence Then
                        SAtkLVUp(1, False)
                    End If
                End If
            Case Trait.复制
                Dim targets As Pokemon() = myTeam.opponentTeam.AlivePokemon
                If targets.Length > 0 Then
                    Dim target As Pokemon = targets(myTeam.random.Next(0, targets.Length))
                    If Not target.SelTrait(True) = Trait.气象台 AndAlso Not target.SelTrait(True) = Trait.复制 Then
                        myTeam.log.AddText(GetNameString() & "复制了" & target.GetNameString & "的" & target.SelTrait(True).ToString & "特性！")
                        SetTrait(target.SelTrait(True))
                    End If
                End If
            Case Trait.洞悉心灵
                Dim targets As Pokemon() = myTeam.opponentTeam.AlivePokemon
                If targets.Length > 0 Then
                    Dim target As Pokemon = targets(myTeam.random.Next(0, targets.Length))
                    myTeam.log.AddText(GetNameString() & "洞悉到" & target.GetNameString & "持有" & target.Item.ToString & "！")
                End If
            Case Trait.预知危险
                Dim targets As Pokemon() = myTeam.opponentTeam.AlivePokemon
                If targets.Length > 0 Then
                    Dim target As Pokemon = targets(myTeam.random.Next(0, targets.Length))
                    For i As Integer = 1 To 4
                        If target.SelMove(i) IsNot Nothing Then
                            Dim typeEff As Double = Battle.CalculateAttackTypeEffect(target, Me, target.SelMove(i).Type)
                            If typeEff > 1 OrElse target.SelMove(i).AddEff2 = MoveAdditionalEffect.必杀 _
                                OrElse target.SelMove(i).AddEff1 = MoveAdditionalEffect.自爆 Then
                                myTeam.log.AddText(GetNameString() & "显得很害怕！") : Return
                            End If
                        End If
                    Next
                End If
            Case Trait.精神梦境
                Dim targets As Pokemon() = myTeam.opponentTeam.AlivePokemon
                If targets.Length > 0 Then
                    Dim target As Pokemon = targets(myTeam.random.Next(0, targets.Length))
                    Dim moveName As New List(Of String)
                    Dim power As Integer
                    For i As Integer = 1 To 4
                        If target.SelMove(i) IsNot Nothing Then
                            If target.SelMove(i).Power > power Then
                                moveName.Clear()
                                moveName.Add(target.SelMove(i).Name)
                            ElseIf target.SelMove(i).Power = power Then
                                moveName.Add(target.SelMove(i).Name)
                            End If
                        End If
                    Next
                    Dim count As Integer = myTeam.random.Next(0, moveName.Count)
                    myTeam.log.AddText(GetNameString() & "感知到" & target.GetNameString & "拥有" & moveName(count) & "技能！")
                End If
        End Select
    End Sub

    Public Sub RaiseItem()
        Select Case Item
            Case Item.果实1
                If State = PokemonState.Paralysis Then
                    State = ModShare.PokemonState.No
                    myTeam.log.AddText(GetNameString() & "的解麻果使它解除了麻痹状态")
                    UseItem()
                End If
            Case Item.果实2
                If State = PokemonState.Sleep Then
                    State = ModShare.PokemonState.No
                    myTeam.log.AddText(GetNameString() & "的睡眠果使它解除了睡眠状态")
                    UseItem()
                End If
            Case Item.果实3
                If State = PokemonState.Toxin OrElse State = PokemonState.Poison Then
                    State = ModShare.PokemonState.No
                    myTeam.log.AddText(GetNameString() & "的解毒果使它解除了中毒状态")
                    UseItem()
                End If
            Case Item.果实4
                If State = PokemonState.Burn Then
                    State = ModShare.PokemonState.No
                    myTeam.log.AddText(GetNameString() & "的烧伤果使它解除了烧伤状态")
                    UseItem()
                End If
            Case Item.果实5
                If State = PokemonState.Freeze Then
                    State = ModShare.PokemonState.No
                    myTeam.log.AddText(GetNameString() & "的解冻果使它解除了冻结状态")
                    UseItem()
                End If
            Case Item.果实6
                For i As Integer = 1 To 4
                    If SelMove(i) IsNot Nothing AndAlso SelMove(i).PP = 0 Then
                        SelMove(i).PPRecover(10)
                        myTeam.log.AddText(GetNameString() & "的6号果实使它的技能回复了PP")
                        UseItem()
                        Return
                    End If
                Next
            Case Item.果实7
                If HP <= MAXHP * 0.5 Then
                    myTeam.log.AddText(GetNameString() & "的7号果实使它回复了HP！")
                    HPRecover(10)
                    UseItem()
                End If
            Case Item.果实8
                If BattleState.confused Then
                    BattleState.confused = False
                    myTeam.log.AddText(GetNameString() & "的混乱果使它解除了混乱状态！")
                    UseItem()
                End If
            Case Item.果实9
                If BattleState.confused Then
                    BattleState.confused = False
                    myTeam.log.AddText(GetNameString() & "的奇迹果使它解除了混乱状态！")
                    UseItem()
                ElseIf State <> PokemonState.No Then
                    State = PokemonState.No
                    myTeam.log.AddText(GetNameString() & "的奇迹果使它恢复了状态！")
                    UseItem()
                End If
            Case Item.果实10
                If HP <= MAXHP * 0.5 Then
                    myTeam.log.AddText(GetNameString() & "的10号果实使它回复了HP！")
                    HPRecover(MAXHP * 0.25)
                    UseItem()
                End If
            Case Item.果实11
                If HP <= MAXHP * 0.5 Then
                    myTeam.log.AddText(GetNameString() & "的11号果实使它回复了HP！")
                    HPRecover(MAXHP * 0.125)
                    If (Nature + 1) Mod 5 = 1 AndAlso Nature <> 0 Then confuse(False)
                    UseItem()
                End If
            Case Item.果实12
                If HP <= MAXHP * 0.5 Then
                    myTeam.log.AddText(GetNameString() & "的12号果实使它回复了HP！")
                    HPRecover(MAXHP * 0.125)
                    If (Nature + 1) Mod 5 = 2 AndAlso Nature <> 6 Then confuse(False)
                    UseItem()
                End If
            Case Item.果实13
                If HP <= MAXHP * 0.5 Then
                    myTeam.log.AddText(GetNameString() & "的13号果实使它回复了HP！")
                    HPRecover(MAXHP * 0.125)
                    If (Nature + 1) Mod 5 = 3 AndAlso Nature <> 12 Then confuse(False)
                    UseItem()
                End If
            Case Item.果实14
                If HP <= MAXHP * 0.5 Then
                    myTeam.log.AddText(GetNameString() & "的14号果实使它回复了HP！")
                    HPRecover(MAXHP * 0.125)
                    If (Nature + 1) Mod 5 = 4 AndAlso Nature <> 18 Then confuse(False)
                    UseItem()
                End If
            Case Item.果实15
                If HP <= MAXHP * 0.5 Then
                    myTeam.log.AddText(GetNameString() & "的15号果实使它回复了HP！")
                    HPRecover(MAXHP * 0.125)
                    If (Nature + 1) Mod 5 = 5 AndAlso Nature <> 24 Then confuse(False)
                    UseItem()
                End If

            Case Item.果实53
                If HP <= MAXHP * 0.25 OrElse (SelTrait = Trait.早食 AndAlso HP <= MAXHP * 0.5) Then
                    myTeam.log.AddText(GetNameString() & "的物攻果发动了！")
                    AtkLVUp(1, False)
                    UseItem()
                End If
            Case Item.果实54
                If HP <= MAXHP * 0.25 OrElse (SelTrait = Trait.早食 AndAlso HP <= MAXHP * 0.5) Then
                    myTeam.log.AddText(GetNameString() & "的物防果发动了！")
                    DefLVUp(1, False)
                    UseItem()
                End If
            Case Item.果实55
                If HP <= MAXHP * 0.25 OrElse (SelTrait = Trait.早食 AndAlso HP <= MAXHP * 0.5) Then
                    myTeam.log.AddText(GetNameString() & "的速度果发动了！")
                    SpeedLVUp(1, False)
                    UseItem()
                End If
            Case Item.果实56
                If HP <= MAXHP * 0.25 OrElse (SelTrait = Trait.早食 AndAlso HP <= MAXHP * 0.5) Then
                    myTeam.log.AddText(GetNameString() & "的特攻果发动了！")
                    SAtkLVUp(1, False)
                    UseItem()
                End If
            Case Item.果实57
                If HP <= MAXHP * 0.25 OrElse (SelTrait = Trait.早食 AndAlso HP <= MAXHP * 0.5) Then
                    myTeam.log.AddText(GetNameString() & "的特防果发动了！")
                    SDefLVUp(1, False)
                    UseItem()
                End If
            Case Item.果实58
                If HP <= MAXHP * 0.25 OrElse (SelTrait = Trait.早食 AndAlso HP <= MAXHP * 0.5) Then
                    myTeam.log.AddText(GetNameString() & "的会心果发动了！")
                    If Not BattleState.focusEngery Then CTLVUp(1)
                    UseItem()
                End If
            Case Item.果实59
                If HP <= MAXHP * 0.25 OrElse (SelTrait = Trait.早食 AndAlso HP <= MAXHP * 0.5) Then
                    myTeam.log.AddText(GetNameString() & "的随机果发动了！")
                    Dim rnd As Integer = myTeam.random.Next(0, 5)
                    Select Case rnd
                        Case 0
                            AtkLVUp(2, False)
                        Case 1
                            DefLVUp(2, False)
                        Case 2
                            SpeedLVUp(2, False)
                        Case 3
                            SAtkLVUp(2, False)
                        Case 4
                            SDefLVUp(2, False)
                    End Select
                    UseItem()
                End If
            Case Item.果实61
                If HP <= MAXHP * 0.25 OrElse (SelTrait = Trait.早食 AndAlso HP <= MAXHP * 0.5) Then
                    myTeam.log.AddText(GetNameString() & "的命中果发动了！")
                    AccuracyLVUp(1, False)
                    UseItem()
                End If
            Case Item.果实62
                If HP <= MAXHP * 0.25 OrElse (SelTrait = Trait.早食 AndAlso HP <= MAXHP * 0.5) Then
                    myTeam.log.AddText(GetNameString() & "的先制果发动了！")
                    BattleState.quickBerry = True
                    UseItem()
                End If
            Case Item.精神香草
                If BattleState.captivated OrElse BattleState.encore OrElse BattleState.taunt Then
                    BattleState.captivated = False
                    BattleState.captivateTarget = Nothing
                    BattleState.encore = False
                    BattleState.taunt = False
                    myTeam.log.AddText(GetNameString() & "的精神香草使它回复了状态！")
                    UseItem()
                End If
            Case Item.白色香草
                If BattleState.AttackLV < 0 OrElse BattleState.SpAttackLV < 0 OrElse BattleState.DefenceLV < 0 _
                    OrElse BattleState.SpDefenceLV < 0 OrElse BattleState.SpeedLV < 0 _
                    OrElse BattleState.evasionLV < 0 OrElse BattleState.AccLV < 0 Then
                    If BattleState.AttackLV < 0 Then BattleState.AttackLV = 0
                    If BattleState.SpAttackLV < 0 Then BattleState.SpAttackLV = 0
                    If BattleState.DefenceLV < 0 Then BattleState.DefenceLV = 0
                    If BattleState.SpDefenceLV < 0 Then BattleState.SpDefenceLV = 0
                    If BattleState.SpeedLV < 0 Then BattleState.SpeedLV = 0
                    If BattleState.evasionLV < 0 Then BattleState.evasionLV = 0
                    If BattleState.AccLV < 0 Then BattleState.AccLV = 0

                    myTeam.log.AddText(GetNameString() & "的白色香草使它恢复了能力等级！")
                    UseItem()
                End If
            Case Item.力量草药
                myTeam.log.AddText(GetNameString() & "的力量草药发动了！")
                BattleState.prepare = False
                UseItem()
            Case Item.果实60
                HPRecover(MAXHP * 0.25)
                myTeam.log.AddText(GetNameString() & "的60号果实使它回复了HP！")
                UseItem()

            Case Item.果实36
                myTeam.log.AddText(GetNameString() & "的36号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实37
                myTeam.log.AddText(GetNameString() & "的37号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实38
                myTeam.log.AddText(GetNameString() & "的38号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实39
                myTeam.log.AddText(GetNameString() & "的39号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实40
                myTeam.log.AddText(GetNameString() & "的40号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实41
                myTeam.log.AddText(GetNameString() & "的41号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实42
                myTeam.log.AddText(GetNameString() & "的42号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实43
                myTeam.log.AddText(GetNameString() & "的43号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实44
                myTeam.log.AddText(GetNameString() & "的44号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实45
                myTeam.log.AddText(GetNameString() & "的45号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实46
                myTeam.log.AddText(GetNameString() & "的46号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实47
                myTeam.log.AddText(GetNameString() & "的47号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实48
                myTeam.log.AddText(GetNameString() & "的48号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实49
                myTeam.log.AddText(GetNameString() & "的49号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实50
                myTeam.log.AddText(GetNameString() & "的50号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实51
                myTeam.log.AddText(GetNameString() & "的51号果实使它受到的伤害减半！")
                UseItem()
            Case Item.果实52
                myTeam.log.AddText(GetNameString() & "的52号果实使它受到的伤害减半！")
                UseItem()
        End Select

    End Sub
    Public Sub UseItem()
        mUsedItem = Item : SetItem(Item.无)
    End Sub
    Public Sub CheckTrait()
        If SelTrait = Trait.不眠 OrElse SelTrait = Trait.免疫 OrElse SelTrait = Trait.水幕 _
            OrElse SelTrait = Trait.熔岩盔甲 OrElse SelTrait = Trait.活跃 OrElse SelTrait = Trait.柔软 _
            OrElse SelTrait = Trait.自我中心 OrElse SelTrait = Trait.迟钝 Then
            RaiseTrait()
        End If
    End Sub
    Public Sub SetTrait(ByVal traitValue As Trait)
        SelTrait = traitValue
        If SelTrait = Trait.威吓 OrElse SelTrait = Trait.缓慢启动 _
            OrElse SelTrait = Trait.洞悉心灵 _
            OrElse SelTrait = Trait.复制 OrElse SelTrait = Trait.下载 OrElse SelTrait = Trait.精神梦境 _
            OrElse SelTrait = Trait.预知危险 OrElse SelTrait = Trait.偷懒 _
            OrElse SelTrait = Trait.压力 OrElse SelTrait = Trait.多重属性 Then
            RaiseTrait()
        Else
            CheckTrait()
        End If
    End Sub

    Public Sub ChangePM(ByVal index As Integer, ByVal pass As Boolean)
        myTeam.ChangePokemon(Me, index, pass)
    End Sub
    Public Sub Showed()
        If SelTrait = Trait.威吓 OrElse SelTrait = Trait.降雨 OrElse SelTrait = Trait.日照 OrElse SelTrait = Trait.缓慢启动 _
            OrElse SelTrait = Trait.沙尘暴 OrElse SelTrait = Trait.暴雪 OrElse SelTrait = Trait.洞悉心灵 _
            OrElse SelTrait = Trait.复制 OrElse SelTrait = Trait.下载 OrElse SelTrait = Trait.精神梦境 _
            OrElse SelTrait = Trait.预知危险 OrElse SelTrait = Trait.偷懒 _
            OrElse SelTrait = Trait.压力 Then
            RaiseTrait()
        End If
        If Item = Item.白色香草 Then RaiseItem()

    End Sub
    Public Function Pass() As Boolean
        If Not myTeam.CanChange(Me) Then Return False
        BattleState.pass = True
        Return True
    End Function
    Public Function MakedToSwap(ByVal target As Pokemon) As Boolean
        If BattleState.ingrain OrElse (SelTrait = Trait.吸盘 AndAlso target.SelTrait <> Trait.破格) OrElse _
            Not myTeam.CanChange(Me) Then Return False
        Dim index As Integer
        Do
            index = myTeam.random.Next(0, 6)
            Dim pm As Pokemon = myTeam.Pokemon(index)
            If myTeam.Pokemon.Count > 6 AndAlso Me Is myTeam.SelectedPokemon(1) Then
                pm = myTeam.Pokemon(index + 6)
            End If
            If pm IsNot Nothing AndAlso pm.HP <> 0 _
                AndAlso Not myTeam.SelectedPokemon.Contains(pm) Then Exit Do
        Loop
        myTeam.log.AddText(GetNameString() & "强制下场！")
        myTeam.ChangePokemon(Me, index + 1, False)
        Return True
    End Function
    Public Function MakeSubstitute() As Boolean
        If HP > Math.Truncate(MAXHP * 0.25) AndAlso BattleState.substituted = False AndAlso HP > 1 Then
            GetHurt(MAXHP * 0.25)
            BattleState.substituted = True
            BattleState.subHP = Convert.ToInt32((Math.Truncate(MAXHP * 0.25)))
            myTeam.log.AddText(GetNameString() & "制造了一个替身！")
            If BattleState.constraint AndAlso BattleState.constraintTurn <> -1 Then
                myTeam.log.AddText(GetNameString() & "解除了束缚！")
                BattleState.constraint = False
                BattleState.constraintTurn = 0
            End If
            RaiseEvent ImageChange(Me, EventArgs.Empty)
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub SubGetHurt(ByVal damageValue As Integer)
        BattleState.subHP -= damageValue
        If BattleState.subHP <= 0 Then BattleState.substituted = False : RaiseEvent ImageChange(Me, EventArgs.Empty)
    End Sub

    Public Sub Haze()
        BattleState.evasionLV = 0
        BattleState.AccLV = 0
        BattleState.AttackLV = 0
        BattleState.DefenceLV = 0
        BattleState.SpeedLV = 0
        BattleState.SpAttackLV = 0
        BattleState.SpDefenceLV = 0
        BattleState.stockpileDefCounter = 0
        BattleState.stockpileSDefCounter = 0
    End Sub

    Public Sub PerishSong()
        If Not BattleState.perishSong Then
            BattleState.perishSong = True
            BattleState.perishSongCounter = 4
        End If
    End Sub

    Public Function confuse(ByVal moveCall As Boolean, _
        Optional ByVal target As Pokemon = Nothing) As Boolean
        If SelTrait = Trait.自我中心 Then Return False
        If myTeam.Safeguard Then Return False
        If BattleState.confused Then Return False
        BattleState.confused = True
        If moveCall Then
            BattleState.confusedTurn = 1
        Else
            BattleState.confusedTurn = myTeam.random.Next(1, 5)
        End If
        BattleState.confusedCounter = 0
        myTeam.log.AddText(GetNameString() & "混乱了！")
        If Item = Item.果实8 OrElse Item = Item.果实9 Then RaiseItem()
        Return True
    End Function

    Public Function terrify(ByVal target As Pokemon) As Boolean
        If SelTrait = Trait.精神力 AndAlso Not target.SelTrait = Trait.破格 Then Return False
        If BattleState.moved Then Return False
        BattleState.afraid = True
        myTeam.log.AddText(GetNameString() & "害怕了！")
        If SelTrait = Trait.不屈之心 Then SpeedLVUp(1, False)
        Return True
    End Function

    Public Function captivate(ByVal target As Pokemon, Optional ByVal traitEff As Boolean = False) As Boolean
        If SelTrait = Trait.迟钝 AndAlso target.SelTrait <> Trait.破格 Then Return False
        If BattleState.captivated Then Return False
        If myTeam.Safeguard Then Return False
        If Me.Gender <> PokemonGender.No AndAlso target.Gender <> PokemonGender.No AndAlso Me.Gender <> target.Gender Then
            BattleState.captivated = True
            BattleState.captivateTarget = target
            If traitEff Then
                myTeam.log.AddText(target.GetNameString & "的魅惑之身特性使" & CustomName() & "着迷了！")
            Else
                myTeam.log.AddText(GetNameString() & "着迷了！")
            End If
            If Item = Item.精神香草 OrElse Item = Item.果实9 Then
                RaiseItem()
            ElseIf Item = Item.红色线团 AndAlso target.captivate(Me) Then
                myTeam.log.AddText(CustomName & "的红色线团使" & target.GetNameString & "也着迷了！")
                UseItem()
            End If
            Return True
        End If
    End Function

    Public Function BeYawned() As Boolean
        If State = PokemonState.No AndAlso Not BattleState.yawn AndAlso Not myTeam.Safeguard Then
            BattleState.yawn = True
            BattleState.yawnCounter = 0
            myTeam.log.AddText(GetNameString() & "昏昏欲睡！")
            Return True
        End If
        Return False
    End Function

    Public Function BeSeeded(ByVal target As PokemonIndex) As Boolean
        If BattleState.seed Then
            myTeam.log.AddText(GetNameString() & "已经被种下了种子")
        ElseIf HaveType("草") Then
            myTeam.log.AddText(GetNameString() & "无法被种下了种子")
        Else
            BattleState.seed = True
            BattleState.seedTarget = target
            myTeam.log.AddText(GetNameString() & "被种下了种子！")
        End If
    End Function

    Public Function Encore() As Boolean
        If LastMoveIsValid AndAlso LastMove.PP <> 0 Then
            If BattleState.encore = False Then
                BattleState.encore = True
                BattleState.encoreTurn = Battle.EncoreTurn
                BattleState.encoreCounter = 0
                myTeam.log.AddText(GetNameString() & "被鼓掌了！")
                If Item = PokemonBattle.PokemonData.Item.精神香草 Then
                    RaiseItem()
                End If
            Else
                myTeam.log.AddText(GetNameString() & "已经处于鼓掌状态")
            End If
            Return True
        End If
        Return False
    End Function
    Public Function Taunt() As Boolean

        If BattleState.taunt = False Then
            BattleState.taunt = True
            BattleState.tauntTurn = Battle.TauntTurn
            BattleState.tauntCounter = 0
            myTeam.log.AddText(GetNameString() & "被挑拨了！")
            If Item = PokemonBattle.PokemonData.Item.精神香草 Then
                RaiseItem()
            End If
        Else
            myTeam.log.AddText(GetNameString() & "已经处于挑拨状态")
        End If
        Return True
    End Function

    Public Function Dive(ByVal prepareCount As Boolean, ByVal move As Move) As Boolean
        If prepareCount Then
            BattleState.dive = False
            BattleState.nextMove = Nothing
            RaiseEvent ImageChange(Me, EventArgs.Empty)
        ElseIf Item = Item.力量草药 Then
            RaiseItem()
        Else
            BattleState.dive = True
            BattleState.nextMove = move
            myTeam.log.AddText(GetNameString() & "潜入了水中！")
            RaiseEvent ImageChange(Me, EventArgs.Empty)
            Return False
        End If
        Return True
    End Function
    Public Function Dig(ByVal prepareCount As Boolean, ByVal move As Move) As Boolean
        If prepareCount Then
            BattleState.dig = False
            BattleState.nextMove = Nothing
            RaiseEvent ImageChange(Me, EventArgs.Empty)
        ElseIf Item = Item.力量草药 Then
            RaiseItem()
        Else
            BattleState.dig = True
            BattleState.nextMove = move
            myTeam.log.AddText(GetNameString() & "钻入了地下！")
            RaiseEvent ImageChange(Me, EventArgs.Empty)
            Return False
        End If
        Return True
    End Function
    Public Function Fly(ByVal prepareCount As Boolean, ByVal move As Move) As Boolean
        If Not prepareCount AndAlso myTeam.ground.Gravity Then
            myTeam.log.AddText(GetNameString() & "因为重力的作用无法飞起")
            Return False
        End If
        If prepareCount Then
            BattleState.fly = False
            BattleState.nextMove = Nothing
            RaiseEvent ImageChange(Me, EventArgs.Empty)
            If myTeam.ground.Gravity Then
                myTeam.log.AddText("但它失败了")
                Return False
            End If
        ElseIf Item = Item.力量草药 Then
            RaiseItem()
        Else
            BattleState.fly = True
            BattleState.nextMove = move
            myTeam.log.AddText(GetNameString() & "飞上了天空！")
            RaiseEvent ImageChange(Me, EventArgs.Empty)
            Return False
        End If
        Return True
    End Function
    Public Function Jump(ByVal prepareCount As Boolean, ByVal move As Move) As Boolean
        If Not prepareCount AndAlso myTeam.ground.Gravity Then
            myTeam.log.AddText(GetNameString() & "因为重力的作用无法跳起")
            Return False
        End If
        If prepareCount Then
            BattleState.jump = False
            BattleState.nextMove = Nothing
            RaiseEvent ImageChange(Me, EventArgs.Empty)
            If myTeam.ground.Gravity Then
                myTeam.log.AddText("但它失败了")
                Return False
            End If
        ElseIf Item = Item.力量草药 Then
            RaiseItem()
        Else
            BattleState.jump = True
            BattleState.nextMove = move
            myTeam.log.AddText(GetNameString() & "跳起了！")
            RaiseEvent ImageChange(Me, EventArgs.Empty)
            Return False
        End If
        Return True
    End Function
    Public Function Disappear(ByVal prepareCount As Boolean, ByVal move As Move) As Boolean
        If prepareCount Then
            BattleState.disappear = False
            BattleState.nextMove = Nothing
            RaiseEvent ImageChange(Me, EventArgs.Empty)
        ElseIf Item = Item.力量草药 Then
            RaiseItem()
        Else
            BattleState.disappear = True
            BattleState.nextMove = move
            myTeam.log.AddText(GetNameString() & "消失了！")
            RaiseEvent ImageChange(Me, EventArgs.Empty)
            Return False
        End If
        Return True
    End Function

    Public Function BeTelekinesis() As Boolean
        If BattleState.telekinesis Then
            Return False
        Else
            BattleState.telekinesis = True
            BattleState.telekinesisCounter = 0
        End If
    End Function

    Public Sub CountOfEndTurn(ByVal random As Random, ByVal battle As Battle)
        Select Case SelTrait
            Case Trait.太阳能
                RaiseTrait()
            Case Trait.冰之躯体
                RaiseTrait()
            Case Trait.接雨盘
                RaiseTrait()
            Case Trait.干燥皮肤
                If myTeam.ground.Weather = Weather.Sunny Then
                    myTeam.log.AddText(GetNameString() & "因为晴天而受到伤害")
                    GetHurt(MAXHP * 0.0625)
                ElseIf myTeam.ground.Weather = Weather.Rainy AndAlso HP <> MAXHP Then
                    myTeam.log.AddText(GetNameString() & "的干燥皮肤特性使它回复了HP")
                    HPRecover(MAXHP * 0.0625)
                End If
            Case Trait.蜕皮
                RaiseTrait()
            Case Trait.加速
                RaiseTrait()
            Case Trait.润湿身体
                RaiseTrait()
            Case Trait.缓慢启动
                If BattleState.slowStartCounter <> 0 Then
                    BattleState.slowStartCounter -= 1
                    If BattleState.slowStartCounter = 0 Then
                        myTeam.log.AddText(GetNameString() & "成功启动了！")
                    End If
                End If
            Case Trait.偷懒
                If BattleState.stayOneTurn Then BattleState.traitRaised = Not BattleState.traitRaised
        End Select
        If HP = 0 Then Return

        If HP <> MAXHP Then
            If BattleState.aquaRing Then
                myTeam.log.AddText(GetNameString() & "从液体中得到了营养！")
                If Item = Item.大树根 Then
                    HPRecover(MAXHP * 0.0625 * 1.3)
                Else
                    HPRecover(MAXHP * 0.0625)
                End If
            End If
            If BattleState.ingrain Then
                myTeam.log.AddText(GetNameString() & "的根从地面中得到了营养！")
                If Item = Item.大树根 Then
                    HPRecover(MAXHP * 0.0625 * 1.3)
                Else
                    HPRecover(MAXHP * 0.0625)
                End If
            End If
        End If

        If Item = Item.剩饭 AndAlso HP <> MAXHP Then
            myTeam.log.AddText(GetNameString() & "的剩饭使它回复了HP！")
            HPRecover(MAXHP * 0.0625)
        ElseIf Item = Item.黑色淤泥 AndAlso HaveType("毒") AndAlso HP <> MAXHP Then
            myTeam.log.AddText(GetNameString() & "的黑色淤泥使它回复了HP！")
            HPRecover(MAXHP * 0.0625)
        ElseIf Item = Item.果实6 Then
            RaiseItem()
        End If

        If SelTrait <> Trait.魔法守护 Then
            If BattleState.seed Then
                Dim target As Pokemon = battle.GetPokemonFromIndex(BattleState.seedTarget)
                If HP <> 0 Then
                    Dim damage As Double
                    If HP > Math.Truncate(MAXHP * 0.125) Then
                        damage = MAXHP * 0.125
                    Else
                        damage = HP
                    End If
                    myTeam.log.AddText(GetNameString() & "被种子吸取了HP！")
                    GetHurt(damage)
                    If SelTrait = Trait.淤泥 Then
                        myTeam.log.AddText(GetNameString() & "的淤泥特性使" & target.GetNameString & "受到了伤害！")
                        target.GetHurt(damage)
                    Else
                        If target.Item = Item.大树根 Then
                            target.HPRecover(damage * 1.3)
                        Else
                            target.HPRecover(damage)
                        End If
                        myTeam.log.AddText(target.GetNameString & "回复了HP！")
                    End If
                    If HP = 0 Then Return
                End If
            End If

            If Item = Item.附针 Then
                myTeam.log.AddText(GetNameString() & "因为附针而受到了伤害！")
                GetHurt(MAXHP * 0.125)
            ElseIf Item = Item.黑色淤泥 AndAlso Not HaveType("毒") Then
                myTeam.log.AddText(GetNameString() & "因为黑色淤泥而受到了伤害！")
                GetHurt(MAXHP * 0.125)
            End If
            If HP = 0 Then Return

            Select Case State
                Case PokemonState.Burn
                    myTeam.log.AddText(GetNameString() & "烧伤了！")
                    If SelTrait = Trait.耐热 Then
                        GetHurt(MAXHP * 0.0625)
                    Else
                        GetHurt(MAXHP * 0.125)
                    End If
                Case PokemonState.Poison
                    If SelTrait = Trait.毒疗 Then
                        If HP <> MAXHP Then
                            myTeam.log.AddText(GetNameString() & "的毒疗特性使它回复了HP！")
                            HPRecover(MAXHP * 0.125)
                        End If
                    Else
                        myTeam.log.AddText(GetNameString() & "受到了毒素的伤害！")
                        GetHurt(MAXHP * 0.125)
                    End If
                Case PokemonState.Toxin
                    If BattleState.toxinCounter > 0 Then
                        If SelTrait = Trait.毒疗 Then
                            If HP <> MAXHP Then
                                myTeam.log.AddText(GetNameString() & "的毒疗特性使它回复了HP！")
                                HPRecover(MAXHP * 0.125)
                            End If
                        Else
                            myTeam.log.AddText(GetNameString() & "受到了毒素的伤害！")
                            GetHurt(MAXHP * 0.0625 * BattleState.toxinCounter)
                        End If
                    End If
            End Select
            If HP = 0 Then Return

            If BattleState.nightmare AndAlso State <> PokemonState.Sleep Then BattleState.nightmare = False
            If BattleState.nightmare Then
                myTeam.log.AddText(GetNameString() & "在梦中受到伤害！")
                GetHurt(MAXHP * 0.25)
            End If
            If HP = 0 Then Return

            If State = PokemonState.Sleep Then
                For Each pm As Pokemon In myTeam.opponentTeam.SelectedPokemon
                    If pm.SelTrait = Trait.噩梦 AndAlso pm.HP <> 0 Then
                        myTeam.log.AddText(GetNameString() & "在梦中受到伤害！")
                        GetHurt(MAXHP * 0.125)
                        Exit For
                    End If
                Next
            End If
            If HP = 0 Then Return

            If BattleState.curse Then
                myTeam.log.AddText(GetNameString() & "被诅咒了！")
                GetHurt(MAXHP * 0.25)
            End If
            If HP = 0 Then Return

            If BattleState.constraint AndAlso BattleState.constraintTurn <> -1 Then
                Dim pm As Pokemon = battle.GetPokemonFromIndex(BattleState.constraintBy)
                If pm IsNot Nothing AndAlso pm.HP > 0 Then
                    myTeam.log.AddText(GetNameString() & "受到了束缚的伤害！")
                    GetHurt(MAXHP * 0.0625)
                End If
            End If
            If HP = 0 Then Return

        End If

        If Item = Item.剧毒珠 Then
            SetState(PokemonState.Toxin, , False, , True)
        ElseIf Item = Item.火珠 Then
            SetState(PokemonState.Burn, , False, , True)
        End If

        If BattleState.futureAtk AndAlso BattleState.futureCounter = 3 Then
            Dim defValue As Integer
            If BattleState.futureAtkMoveType = MoveType.物理 Then
                defValue = Defence
            Else
                defValue = SpDefence
            End If
            Dim damage As Double = (BattleState.futureAtkValue / defValue / 50 + 2) * random.Next(85, 101) / 100 * _
                battle.CalculateAttackTypeEffect(Nothing, Me, BattleState.futureAtkType) * BattleState.futureAtkDamageRevision
            myTeam.log.AddText(GetNameString() & "受到了来自过去的攻击！")
            If random.NextDouble < BattleState.futureAtkAcc Then
                GetHurt(damage)
            Else
                myTeam.log.AddText("但是没有命中...")
            End If
            BattleState.futureAtk = False
            BattleState.futureCounter = 0
        End If
        If HP = 0 Then Return

        If BattleState.perishSong Then
            myTeam.log.AddText(GetNameString() & "的灭亡歌倒数:" & BattleState.perishSongCounter)
            If BattleState.perishSongCounter = 0 Then
                GetHurt(MAXHP)
                BattleState.perishSong = False
            End If
        End If
        If HP = 0 Then Return
        If myTeam.flameSea Then
            myTeam.log.AddText(GetNameString() & "在火海中受到伤害！")
            GetHurt(MAXHP * 0.125)
        End If
    End Sub

    Public ReadOnly Property LastMoveIsValid As Boolean
        Get
            Return IsValidMoveIndex(lastMoveIndex)
        End Get
    End Property

    Public ReadOnly Property LastMove As Move
        Get
            If LastMoveIsValid Then
                Return SelMove(lastMoveIndex)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property IsAboveGroud As Boolean
        Get
            Return SelTrait = Trait.浮游 OrElse BattleState.suspension OrElse BattleState.telekinesis
        End Get
    End Property

    Public Shared Function IsValidMoveIndex(ByVal moveIndex As Integer) As Boolean
        If moveIndex = 0 OrElse moveIndex = Battle.StruggleIndex Then
            Return False
        Else
            Return True
        End If
    End Function

End Class
