Imports PokemonBattle.BattleNetwork
Imports PokemonBattle.PokemonData
Public Class PokemonBattleState
    Public CTLV As Integer
    Public focusEngery As Boolean
    Public evasionLV, AccLV As Integer

    Public confused As Boolean
    Public confusedCounter, confusedTurn As Integer

    Public toxinCounter As Integer
    Public encoreCounter, encoreTurn As Integer
    Public tauntCounter, tauntTurn As Integer
    Public disableIndex, disableCounter, disableTurn As Integer
    Public perishSongCounter As Integer
    Public protectCounter As Integer
    Public stockpileCounter, stockpileDefCounter, stockpileSDefCounter As Integer

    Public firstTurnAtk As Boolean = True
    Public stayOneTurn As Boolean

    Public wished As Boolean
    Public wishTurn As Integer = -1
    Public wishRecovery As Integer
    Public healwish, lunarDance As Boolean

    Public futureAtk As Boolean
    Public futureCounter As Integer
    Public futureAtkValue As Double
    Public futureAtkMoveType As MoveType
    Public futureAtkType As PokemonType
    Public futureAtkAcc As Double
    Public futureAtkDamageRevision As Double

    Public moved As Boolean
    Public swaped As Boolean
    Public pass As Boolean
    Public uTurn As Boolean
    Public quickBerry As Boolean

    Public afraid, curse, nightmare, perishSong, encore, taunt, destinyBond, protect, _
           endure, protectCancel, intimidated As Boolean

    Public seed, captivated As Boolean
    Public seedTarget As PokemonIndex
    Public captivateTarget As Pokemon
    Public constraint As Boolean
    Public constraintCounter, constraintTurn As Integer
    Public constraintBy As PokemonIndex

    Public torment, aquaRing, miracleEye, foresight, ingrain, _
        imprison, magicCoat, grudge, defenseCurl, minimize, rage As Boolean
    '挑衅,液体圈,奇迹之眼,识破,扎根,封印,魔装反射,变小,愤怒

    Public snatched, meFirst As Boolean
    Public snatchBy As Pokemon

    Public pursuit As Boolean

    Public worrySeed, gastricJuice As Boolean

    Public suspension As Boolean
    Public suspensionCounter As Integer

    Public charge As Boolean
    Public chargeCounter As Integer

    Public lockOn As Boolean
    Public lockOnCounter As Integer

    Public healBlock As Boolean
    Public healBlockCounter As Integer

    Public yawn As Boolean
    Public yawnCounter As Integer

    Public detain As Boolean
    Public detainCounter As Integer

    Public transfrom As Boolean
    Public transTemp As PokemonData
    Public roost As Boolean
    Public roostTemp As PokemonType

    Public substituted As Boolean
    Public subHP As Integer

    Public powerTrick As Boolean
    Public slowStartCounter As Integer
    Public assisted As Boolean
    Public choiceSleepTalked As Boolean

    Public AttackLV, DefenceLV, SpeedLV, SpAttackLV, SpDefenceLV As Integer '能力等级 
    Public tempType, tempType2 As PokemonType
    Public tempAttack, tempDefence, tempSpeed, tempSAttack, tempSDefence As Integer
    Public tempTrait As Trait
    Public traitRaised As Boolean
    Public tempMoves As Move()
    Public tempImg1, tempImg2 As Bitmap
    Public tempWeight As Double

    Public successContinAttackCounter As Integer
    Public previousMove As Integer

    Public sleepTalking As Boolean

    '念动力
    Public telekinesis As Boolean
    Public telekinesisCounter As Integer

    Public shapeShift As Boolean

    Public crossFire As Boolean
    Public crossThunder As Boolean

    Public comboMoor As Boolean
    Public comboRainbow As Boolean
    Public comboFlameSea As Boolean

    Public Sub New()
        tempMoves = New Move(3) {}
    End Sub

    Public Sub CountOfEndTurn(ByVal pm As Pokemon, ByVal ground As BattleGround, ByVal log As ImgTxt)
        afraid = False
        magicCoat = False
        snatched = False
        snatchBy = Nothing
        meFirst = False
        assisted = False
        sleepTalking = False
        crossFire = False
        crossThunder = False
        comboMoor = False
        comboRainbow = False
        comboFlameSea = False

        If pm.State = PokemonState.Toxin Then toxinCounter += 1
        If perishSong Then perishSongCounter -= 1
        If constraint AndAlso constraintTurn <> -1 Then constraintCounter += 1
        If suspension Then suspensionCounter -= 1
        If healBlock Then healBlockCounter -= 1
        If disableIndex <> 0 Then disableCounter += 1
        If yawn Then yawnCounter += 1
        If detain Then detainCounter -= 1
        If charge Then chargeCounter -= 1
        If lockOn Then lockOnCounter -= 1
        If roost Then
            roost = False
            roostTemp = Nothing
        End If
        If protect Then
            protect = False
        ElseIf protectCancel Then
            protectCancel = False
        ElseIf endure Then
            endure = False
        Else
            protectCounter = 0
        End If
        If charge AndAlso chargeCounter = 0 Then
            charge = False
        End If
        If lockOn AndAlso lockOnCounter = 0 Then
            lockOn = False
        End If
        If healBlock AndAlso healBlockCounter = 0 Then
            healBlock = False
            log.AddText(pm.GetNameString & "的回复封印解除了！")
        End If
        If detain AndAlso detainCounter = 0 Then
            detain = False
            log.AddText(pm.GetNameString & "的道具恢复了效果！")
        End If

        If cannotControlAttack Then cannotControlCounter += 1
        If cannotControlAttack Then
            If cannotControlCounter = cannotControlTurn Then
                log.AddText(pm.GetNameString & "停止了攻击！")
                If pm.BattleState.mUproar Then
                    pm.BattleState.mUproar = False
                ElseIf mRollCounter < 2 Then
                    pm.confuse(False)
                End If
                cannotControlAttack = False
                rollSuccessed = False
                mRollCounter = 1
                nextMove = Nothing
            ElseIf mRollCounter > 1 Then
                If rollSuccessed Then
                    rollSuccessed = False
                Else
                    mRollCounter = 1
                    cannotControlAttack = False
                End If
            End If
        End If
        If bide Then bideCounter += 1
        If bideCounter = 3 Then pm.BattleState.bide = False

        If taunt Then
            tauntCounter += 1
            If tauntCounter = tauntTurn Then _
                 taunt = False : log.AddText(pm.GetNameString & "的挑拨状态结束了")
        End If
        If suspension AndAlso suspensionCounter = 0 Then
            suspension = False
            log.AddText(pm.GetNameString & "的电磁悬浮状态消失了！")
        End If
        If disableIndex <> 0 AndAlso disableCounter = disableTurn Then
            log.AddText(pm.GetNameString & "的" & pm.SelMove(disableIndex).Name & "可以使用了！")
            disableIndex = 0
        End If
        If constraint AndAlso constraintTurn = constraintCounter Then
            constraint = False
            log.AddText(pm.GetNameString & "的束缚解除了！")
        End If
        If wished AndAlso wishTurn = 1 Then
            wished = False
            log.AddText(pm.myTeam.GetPlayerName(pm) & "队伍的愿望实现了！")
            If pm.HP < pm.MAXHP Then
                pm.HPRecover(wishRecovery)
                log.AddText(pm.GetNameString & "恢复了HP！")
            Else
                log.AddText(pm.GetNameString & "的HP已经满了！")
            End If
        End If
        If encore Then
            encoreCounter += 1
            If encoreTurn = encoreCounter OrElse (pm.LastMoveIsValid AndAlso pm.LastMove.PP = 0) Then
                log.AddText(pm.GetNameString & "的鼓掌结束了！")
                encore = False
            End If
        End If
        If yawn AndAlso yawnCounter = 2 Then
            pm.SetState(PokemonState.Sleep)
            yawn = False
        End If

        If telekinesis Then telekinesisCounter += 1
        If telekinesisCounter = 3 Then
            telekinesis = False
            log.AddText(pm.GetNameString & "回到了地面！")
        End If
    End Sub

    Public Sub PassCopy(ByVal state As PokemonBattleState)
        With state
            CTLV = .CTLV
            focusEngery = .focusEngery
            AccLV = .AccLV
            evasionLV = .evasionLV
            substituted = .substituted
            subHP = .subHP
            lockOn = .lockOn

            constraint = .constraint
            constraintTurn = .constraintTurn
            constraintCounter = .constraintCounter
            constraintBy = .constraintBy

            confused = .confused
            confusedTurn = .confusedTurn
            confusedCounter = .confusedCounter

            perishSong = .perishSong
            perishSongCounter = .perishSongCounter
            suspension = .suspension
            suspensionCounter = .suspensionCounter
            detain = .detain
            detainCounter = .detainCounter

            curse = .curse
            ingrain = .ingrain
            aquaRing = .aquaRing
            gastricJuice = .gastricJuice
            powerTrick = .powerTrick

            seed = .seed
            seedTarget = .seedTarget

            AttackLV = .AttackLV
            DefenceLV = .DefenceLV
            SpeedLV = .SpeedLV
            SpAttackLV = .SpAttackLV
            SpDefenceLV = .SpDefenceLV


        End With
    End Sub

    Public nextMove As Move

    Public nextTurnCantMove As Boolean

    Public prepare As Boolean
    Public dive, dig, fly, jump, disappear As Boolean

    Private cannotControlAttack As Boolean
    Private cannotControlTurn, cannotControlCounter As Integer

    Private mUproar As Boolean
    Private rollSuccessed As Boolean
    Private mRollCounter As Integer = 1

    Public bide As Boolean
    Public bideCounter As Integer
    Public bideHurt As Integer
    Public bideTarget As PokemonIndex

    Public ReadOnly Property Hide() As Boolean
        Get
            Return dive OrElse dig OrElse jump OrElse fly OrElse disappear
        End Get
    End Property

    Public Sub BeginCrazyAttack(ByVal random As Random, ByVal move As Move)
        '梦话不锁招
        If sleepTalking Then
            Return
        End If

        cannotControlAttack = True
        cannotControlTurn = random.Next(2, 4)
        cannotControlCounter = 0
        nextMove = move
    End Sub

    Public Sub BeginUproar(ByVal random As Random, ByVal move As Move)
        cannotControlAttack = True
        cannotControlTurn = Battle.UproarTurn
        cannotControlCounter = 0
        mUproar = True
        nextMove = move
    End Sub

    Public ReadOnly Property Uproar() As Boolean
        Get
            Return mUproar
        End Get
    End Property

    Public ReadOnly Property CrazyAttack() As Boolean
        Get
            Return cannotControlAttack
        End Get
    End Property

    Public Sub Roll(ByVal move As Move)
        If mRollCounter > 1 Then
            NextRoll()
        Else
            BeginRoll(move)
        End If
    End Sub

    Private Sub BeginRoll(ByVal move As Move)
        '梦话不锁招
        If sleepTalking Then
            Return
        End If
        cannotControlAttack = True
        cannotControlTurn = 5
        cannotControlCounter = 0
        mRollCounter = 2
        rollSuccessed = True
        nextMove = move
    End Sub

    Private Sub NextRoll()
        rollSuccessed = True
        mRollCounter += 1
    End Sub

    Public ReadOnly Property RollCounter() As Integer
        Get
            Return mRollCounter
        End Get
    End Property

    Private Shared emptyState As PokemonBattleState
    Shared Sub New()
        emptyState = New PokemonBattleState
    End Sub
    Public Shared ReadOnly Property EmptyBattleState() As PokemonBattleState
        Get
            Return emptyState
        End Get
    End Property
End Class
