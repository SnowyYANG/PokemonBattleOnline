Imports PokemonBattle.PokemonData
Imports PokemonBattle.PokemonData.Custom
Public Class GameData
    Implements IDataProvider

    Private _types As List(Of PokemonType)
    Private _moves As List(Of MoveData)
    Private _pokemons As List(Of PokemonData)

    Public FalseMove1 As String
    Public FalseMove2 As String

    Public Function ContainsPokemon(ByVal name As String) As Boolean
        Return _pokemons.Exists(Function(pm As PokemonData) pm.Name = name)
    End Function

    Public Function GetEggGroup(ByVal eggGroup As EggGroup, ByVal exceptList As List(Of EggGroup)) As List(Of PokemonData)
        Dim array As New List(Of PokemonData)
        For Each pm As PokemonData In _pokemons
            If (pm.EggGroup1 = eggGroup AndAlso Not exceptList.Contains(pm.EggGroup2)) OrElse _
                (pm.EggGroup2 = eggGroup AndAlso Not exceptList.Contains(pm.EggGroup1)) Then
                If pm.GenderRestriction <> PokemonGenderRestriction.FemaleOnly AndAlso pm.GenderRestriction <> PokemonGenderRestriction.NoGender Then
                    array.Add(pm)
                End If
            End If
        Next
        Return array
    End Function

    Private Function GetEggGroup(ByVal eggGroup As EggGroup, ByVal except As PokemonData) As List(Of PokemonData)
        Dim array As New List(Of PokemonData)
        If except.Number = 236 OrElse except.BeforeEvolution = 236 Then
            array.Add(GetPokemonData(106))
            array.Add(GetPokemonData(107))
            array.Add(GetPokemonData(236))
            array.Add(GetPokemonData(237))
            array.Remove(except)
        Else
            For Each pm As PokemonData In _pokemons
                If (pm.EggGroup1 = eggGroup OrElse pm.EggGroup2 = eggGroup) AndAlso pm IsNot except Then
                    If pm.GenderRestriction <> PokemonGenderRestriction.FemaleOnly AndAlso _
                        pm.GenderRestriction <> PokemonGenderRestriction.NoGender Then
                        array.Add(pm)
                    End If
                End If
            Next
        End If
        Return array
    End Function

    Public Function GetPMs(ByVal move1 As Integer, ByVal move2 As Integer, ByVal list As List(Of PokemonData)) As List(Of PokemonData)
        Dim array As New List(Of PokemonData)
        For Each pm As PokemonData In list
            If pm.GetMove(move1) IsNot Nothing AndAlso pm.GetMove(move2) IsNot Nothing Then
                array.Add(pm)
            End If
        Next
        Return array
    End Function

    Public Function GetPMs(ByVal moves As List(Of Integer), ByVal list As List(Of PokemonData)) As List(Of PokemonData)
        Dim array As New List(Of PokemonData)
        For Each pm As PokemonData In list
            Dim data As PokemonData = pm
            If moves.Exists(Function(move) data.GetMove(move) Is Nothing) Then
                Continue For
            Else
                array.Add(pm)
            End If
        Next
        Return array
    End Function

    Public Function CheckMoves(ByVal MoveDatas As List(Of Integer), ByVal pm As PokemonData, _
        Optional ByVal exceptList As List(Of EggGroup) = Nothing) As Boolean
        Dim inheritList As New List(Of Integer)
        For Each move As Integer In MoveDatas
            If pm.GetMove(move).LearnBy = "遗传" Then inheritList.Add(move)
        Next
        If inheritList.Count > 2 Then
            If Not CanInheritMoves(inheritList, pm, exceptList) Then
                Dim str As String = ""
                For i As Integer = 0 To inheritList.Count - 2
                    If i > 0 Then
                        str &= "," & BattleData.GetMove(inheritList(i)).Name
                    Else
                        str &= BattleData.GetMoveName(inheritList(i))
                    End If
                Next
                FalseMove2 = str
                FalseMove1 = BattleData.GetMoveName(inheritList(inheritList.Count - 1))
                Return False
            End If
        End If
        For i As Integer = 0 To MoveDatas.Count - 2
            For j As Integer = i + 1 To MoveDatas.Count - 1
                If Not CheckMoves(MoveDatas(i), MoveDatas(j), pm, exceptList) Then
                    FalseMove1 = BattleData.GetMoveName(MoveDatas(i))
                    FalseMove2 = BattleData.GetMoveName(MoveDatas(j))
                    Return False
                End If
            Next
        Next

        Return True
    End Function

    Private Function CheckMoves(ByVal move1 As Integer, ByVal move2 As Integer, ByVal pm As PokemonData, _
    Optional ByVal exceptList As List(Of EggGroup) = Nothing) As Boolean
        Dim moveData1, moveData2 As MoveLearnData
        moveData1 = pm.GetMove(move1)
        moveData2 = pm.GetMove(move2)

        Dim learnby1, learnby2 As String
        learnby1 = moveData1.LearnBy
        learnby2 = moveData2.LearnBy

        If learnby1 = "遗传" AndAlso learnby2 = "未进化" Then
            If moveData2.Info = "旧版" Then
                Return CheckMoves(move1, move2, GetPokemonData(pm.BeforeEvolution))
            End If
        ElseIf learnby2 = "遗传" AndAlso learnby1 = "未进化" Then
            If moveData1.Info = "旧版" Then
                Return CheckMoves(move1, move2, GetPokemonData(pm.BeforeEvolution))
            End If
        ElseIf learnby1 = "遗传" AndAlso learnby2 = "旧版" Then
            If moveData1.Info = "DSOnly" OrElse moveData2.Info = "GC" Then
                Return False
            End If
        ElseIf learnby2 = "遗传" AndAlso learnby1 = "旧版" Then
            If moveData2.Info = "DSOnly" OrElse moveData1.Info = "GC" Then
                Return False
            End If
        ElseIf learnby1 = "未进化" AndAlso learnby2 = "未进化" Then
            If pm.Name = "鬼蝉" Then
                If GetPokemonData(pm.BeforeEvolution).GetMove(move1) Is Nothing _
                    AndAlso GetPokemonData(pm.BeforeEvolution).GetMove(move2) Is Nothing Then
                    Return CheckNijiaMoveDatas(move1, move2, pm)
                End If
            Else
                Return CheckMoves(move1, move2, GetPokemonData(pm.BeforeEvolution))
            End If
        ElseIf learnby1 = "遗传" AndAlso learnby2 = "遗传" Then
            Return CanInheritMoves(move1, move2, pm, exceptList)
        ElseIf learnby1 = "NPC" AndAlso learnby2 = "遗传" Then
            Return False
        ElseIf learnby2 = "NPC" AndAlso learnby1 = "遗传" Then
            Return False
        End If
        Return True
    End Function

    Private Function CanInheritMoves(ByVal move1 As Integer, ByVal move2 As Integer, _
        ByVal pm As PokemonData, ByVal exceptList As List(Of EggGroup)) As Boolean

        If pm.EggGroup1 = EggGroup.陆上 OrElse pm.EggGroup2 = EggGroup.陆上 Then Return True
        If exceptList Is Nothing Then exceptList = New List(Of EggGroup)

        If exceptList.Contains(pm.EggGroup1) AndAlso (exceptList.Contains(pm.EggGroup2) OrElse pm.EggGroup2 = EggGroup.无) Then
            Return False
        ElseIf exceptList.Contains(pm.EggGroup1) OrElse exceptList.Contains(pm.EggGroup2) Then
            Dim eggGroup As EggGroup
            If exceptList.Contains(pm.EggGroup1) Then
                eggGroup = pm.EggGroup2
            Else
                eggGroup = pm.EggGroup1
            End If

            If eggGroup <> eggGroup.无 Then
                Dim list As List(Of PokemonData) = GetPMs(move1, move2, GetEggGroup(eggGroup, exceptList))
                exceptList.Add(eggGroup)
                For Each inheritPM As PokemonData In list
                    If CheckMoves(move1, move2, inheritPM, exceptList) Then
                        Return True
                    End If
                Next
            End If
        Else
            Dim list1 As List(Of PokemonData) = GetPMs(move1, move2, GetEggGroup(pm.EggGroup1, pm))
            exceptList.Add(pm.EggGroup1)
            If pm.EggGroup2 <> EggGroup.无 Then exceptList.Add(pm.EggGroup2)

            For Each inheritPM As PokemonData In list1
                If CheckMoves(move1, move2, inheritPM, exceptList) Then
                    Return True
                End If
            Next

            Dim list2 As List(Of PokemonData)
            If pm.EggGroup2 <> EggGroup.无 Then
                list2 = GetPMs(move1, move2, GetEggGroup(pm.EggGroup2, exceptList))
                For Each inheritPM As PokemonData In list2
                    If CheckMoves(move1, move2, inheritPM, exceptList) Then
                        Return True
                    End If
                Next
            End If

        End If
        Return False
    End Function

    Private Function CanInheritMoves(ByVal MoveDatas As List(Of Integer), _
        ByVal pm As PokemonData, ByVal exceptList As List(Of EggGroup)) As Boolean

        If pm.EggGroup1 = EggGroup.陆上 OrElse pm.EggGroup2 = EggGroup.陆上 Then Return True
        If exceptList Is Nothing Then exceptList = New List(Of EggGroup)

        If exceptList.Contains(pm.EggGroup1) AndAlso (exceptList.Contains(pm.EggGroup2) OrElse pm.EggGroup2 = EggGroup.无) Then
            Return False
        ElseIf exceptList.Contains(pm.EggGroup1) Then
            If pm.EggGroup2 <> EggGroup.无 Then
                Dim list As List(Of PokemonData) = GetPMs(MoveDatas, GetEggGroup(pm.EggGroup2, exceptList))
                exceptList.Add(pm.EggGroup2)
                For Each inheritPM As PokemonData In list
                    If CheckMoves(MoveDatas, inheritPM, exceptList) Then
                        Return True
                    End If
                Next
            End If
        ElseIf exceptList.Contains(pm.EggGroup2) Then
            Dim list As List(Of PokemonData) = GetPMs(MoveDatas, GetEggGroup(pm.EggGroup1, exceptList))
            exceptList.Add(pm.EggGroup1)
            For Each inheritPM As PokemonData In list
                If CheckMoves(MoveDatas, inheritPM, exceptList) Then
                    Return True
                End If
            Next
        Else
            Dim list1 As List(Of PokemonData) = GetPMs(MoveDatas, GetEggGroup(pm.EggGroup1, pm))
            exceptList.Add(pm.EggGroup1)
            If pm.EggGroup2 <> EggGroup.无 Then exceptList.Add(pm.EggGroup2)

            For Each inheritPM As PokemonData In list1
                If CheckMoves(MoveDatas, inheritPM, exceptList) Then
                    Return True
                End If
            Next

            Dim list2 As List(Of PokemonData)
            If pm.EggGroup2 <> EggGroup.无 Then
                list2 = GetPMs(MoveDatas, GetEggGroup(pm.EggGroup2, exceptList))
                For Each inheritPM As PokemonData In list2
                    If CheckMoves(MoveDatas, inheritPM, exceptList) Then
                        Return True
                    End If
                Next
            End If

        End If
        Return False
    End Function

    Private Function CheckNijiaMoveDatas(ByVal str1 As Integer, ByVal str2 As Integer, ByVal pm As PokemonData) As Boolean
        Dim move1, move2 As MoveLearnData
        Dim nijia As PokemonData = GetPokemonDataByNumber(291)(0)
        move1 = nijia.GetMove(str1)
        move2 = nijia.GetMove(str2)
        If move1.Info Is Nothing OrElse move2.Info Is Nothing Then Return True
        Dim lv1, lv2 As Integer
        lv1 = Convert.ToInt32(move1.Info)
        lv2 = Convert.ToInt32(move2.Info)
        If lv1 <> lv2 Then Return False
        Return True
    End Function

    Private Function RandomPokemon(ByVal random As Random) As PokemonCustomInfo
        Dim pm As New PokemonCustomInfo
        Dim data As PokemonData
        Dim pokemonList As New List(Of PokemonData)(_pokemons)
        If _customData IsNot Nothing Then
            pokemonList.RemoveAll(Function(pmdata As PokemonData) _customData.RemovePokemons.Contains(pm.Identity))
        End If

        data = pokemonList(random.Next(0, pokemonList.Count))

        pm.Identity = data.Identity
        pm.Nickname = data.Name
        pm.LV = 100
        If data.GenderRestriction = PokemonGenderRestriction.NoGender Then
            pm.Gender = PokemonGender.No
        ElseIf data.GenderRestriction = PokemonGenderRestriction.FemaleOnly Then
            pm.Gender = PokemonGender.Female
        ElseIf data.GenderRestriction = PokemonGenderRestriction.MaleOnly Then
            pm.Gender = PokemonGender.Male
        Else
            pm.Gender = New PokemonGender() {PokemonGender.Male, PokemonGender.Female}(random.Next(0, 2))
        End If
        pm.HPIV = Convert.ToByte(random.Next(0, 32))
        pm.SpeedIV = Convert.ToByte(random.Next(0, 32))
        pm.AttackIV = Convert.ToByte(random.Next(0, 32))
        pm.DefenceIV = Convert.ToByte(random.Next(0, 32))
        pm.SpAttackIV = Convert.ToByte(random.Next(0, 32))
        pm.SpDefenceIV = Convert.ToByte(random.Next(0, 32))

        Dim total As Integer = 510
        Dim ev As Byte() = New Byte(5) {}
        For i As Integer = 0 To 5
            If total >= 255 Then
                ev(i) = Convert.ToByte(random.Next(0, 256))
            Else
                ev(i) = Convert.ToByte(random.Next(0, total + 1))
            End If
            total -= ev(i)
            If total = 0 Then Exit For
        Next
        pm.HPEV = ev(0)
        pm.AttackEV = ev(1)
        pm.DefenceEV = ev(2)
        pm.SpAttackEV = ev(3)
        pm.SpDefenceEV = ev(4)
        pm.SpeedEV = ev(5)

        pm.Character = CType(random.Next(0, 25), PokemonCharacter)
        pm.Item = CType(random.Next(1, 150), Item)
        pm.SelectedTrait = 1
        If data.Trait2 <> Trait.无 Then pm.SelectedTrait = Convert.ToByte(random.Next(1, 3))
        If data.Moves.Count <= 4 Then
            For i As Integer = 0 To data.Moves.Count - 1
                pm.SelectedMoves(i) = data.Moves(i).MoveId
            Next
        Else
            Dim list As New List(Of MoveLearnData)(data.Moves)
            For i As Integer = 0 To 3
                Dim move As MoveLearnData = list(random.Next(0, list.Count))
                list.Remove(move)
                pm.SelectedMoves(i) = move.MoveId
            Next
        End If
        Return pm
    End Function

    Public Sub LoadData(ByVal types As List(Of PokemonType), ByVal moves As List(Of MoveData), ByVal pokemons As List(Of PokemonData))
        _types = types
        _moves = moves
        _pokemons = pokemons
    End Sub
    Public Sub LoadData(ByVal input As Stream)
        Dim reader As New BinaryReader(input)
        _types = New List(Of PokemonType)(reader.ReadInt32())
        For i As Integer = 1 To _types.Capacity
            _types.Add(PokemonType.FromStream(input))
        Next
        _moves = New List(Of MoveData)(reader.ReadInt32())
        For i As Integer = 1 To _moves.Capacity
            _moves.Add(MoveData.FromStream(input))
        Next
        _pokemons = New List(Of PokemonData)(reader.ReadInt32())
        For i As Integer = 1 To _pokemons.Capacity
            _pokemons.Add(PokemonData.FromStream(input))
        Next
    End Sub
    Public Sub Save(ByVal output As Stream)
        Dim writer As BinaryWriter = New BinaryWriter(output)
        writer.Write(_types.Count)
        For Each type As PokemonType In _types
            type.Save(output)
        Next
        writer.Write(_moves.Count)
        For Each move As MoveData In _moves
            move.Save(output)
        Next
        writer.Write(_pokemons.Count)
        For Each pm As PokemonData In _pokemons
            pm.Save(output)
        Next
    End Sub

    Private _customData As CustomGameData
    Private _customInfo As New CustomDataInfo
    Public Sub LoadCustomData(ByVal data As CustomGameData, ByVal hash As String)
        _customData = data
        _customInfo.DataName = data.Name
        _customInfo.DataHash = hash
        For Each pm As CustomPokemonData In data.CustomPokemons
            Dim newPM As New PokemonData
            newPM.Identity = pm.Identity
            newPM.Name = pm.NameBase
            newPM.FrontImage = pm.FrontImage
            newPM.FrontImageF = pm.FrontImageF
            newPM.BackImage = pm.BackImage
            newPM.BackImageF = pm.BackImageF
            newPM.Frame = pm.Frame
            newPM.FrameF = pm.FrameF
            newPM.Icon = pm.Icon
            newPM.Weight = pm.Weight

            newPM.HPBase = pm.HPBase
            newPM.AttackBase = pm.AttackBase
            newPM.DefenceBase = pm.DefenceBase
            newPM.SpeedBase = pm.SpeedBase
            newPM.SpAttackBase = pm.SpAttackBase
            newPM.SpDefenceBase = pm.SpDefenceBase
            newPM.Type1 = pm.Type1
            newPM.Type2 = pm.Type2
            newPM.Trait1 = pm.Trait1
            newPM.Trait2 = pm.Trait2
            newPM.EggGroup1 = pm.EggGroup1
            newPM.EggGroup2 = pm.EggGroup2
            newPM.BeforeEvolution = pm.BeforeEvolution
            newPM.AfterEvolution.AddRange(pm.AfterEvolution)
            newPM.GenderRestriction = pm.GenderRestriction
            newPM.Number = pm.Number
            For Each move As Integer In pm.Moves
                Dim moveData As New MoveLearnData
                moveData.MoveId = move
                moveData.LearnBy = "自定义"
                newPM.Moves.Add(moveData)
            Next
            _pokemons.Add(newPM)
        Next
        For Each update As UpdatePokemonData In data.UpdatePokemons
            UpdatePMData(GetPokemonData(update.Identity), update)
        Next
    End Sub
    Private Sub UpdatePMData(ByVal data As PokemonData, ByVal update As UpdatePokemonData)
        If update.HPBase <> 0 Then data.HPBase = update.HPBase
        If update.AttackBase <> 0 Then data.AttackBase = update.AttackBase
        If update.DefenceBase <> 0 Then data.DefenceBase = update.DefenceBase
        If update.SpeedBase <> 0 Then data.SpeedBase = update.SpeedBase
        If update.SpAttackBase <> 0 Then data.SpAttackBase = update.SpAttackBase
        If update.SpDefenceBase <> 0 Then data.SpDefenceBase = update.SpDefenceBase
        If update.Weight <> 0 Then data.Weight = update.Weight

        If update.Type1 <> PokemonType.InvalidId Then data.Type1 = update.Type1
        If update.Type2 <> PokemonType.InvalidId Then data.Type2 = update.Type2
        If update.Trait1 <> Trait.无 Then data.Trait1 = update.Trait1
        If update.Trait2 <> Trait.无 Then data.Trait2 = update.Trait2
        For Each moveId As Integer In update.AddMoves
            Dim move As New MoveLearnData
            move.MoveId = moveId
            move.LearnBy = "自定义"
            data.Moves.Add(move)
        Next
        For Each moveId As Integer In update.RemoveMoves
            data.Moves.Remove(data.GetMove(moveId))
        Next
    End Sub
    Private Function IsCustom(ByVal identity As Integer) As Boolean
        If _customData Is Nothing Then Return False

        Return _customData.CustomPokemons.Find(Function(pm As CustomPokemonData) pm.Identity = identity) IsNot Nothing
    End Function

    Public Function GetAllMoves() As PokemonBattle.PokemonData.MoveData() Implements IDataProvider.GetAllMoves
        Return _moves.ToArray()
    End Function

    Public Function GetAllTypes() As PokemonBattle.PokemonData.PokemonType() Implements IDataProvider.GetAllTypes
        Return _types.ToArray()
    End Function

    Public Function GetImage(ByVal identity As Integer, ByVal position As Long) As System.Drawing.Bitmap Implements IDataProvider.GetImage
        If IsCustom(identity) Then
            Return ImageManager.GetCustomImage(position)
        Else
            Return ImageManager.GetImage(position)
        End If
    End Function

    Public Function GetMoveData(ByVal name As String) As MoveData Implements IDataProvider.GetMoveData
        Return _moves.Find(Function(move As MoveData) move.Name = name)
    End Function

    Public Function GetMoveData(ByVal id As Integer) As MoveData Implements IDataProvider.GetMoveData
        Return _moves.Find(Function(m) m.Identity = id)
    End Function

    Public Function GetTypeData(ByVal id As Integer) As PokemonType Implements IDataProvider.GetTypeData
        Return _types.Find(Function(t) t.Identity = id)
    End Function

    Public Function GetPokemonData(ByVal identity As Integer) As PokemonData Implements IDataProvider.GetPokemonData
        Return _pokemons.Find(Function(pm As PokemonData) pm.Identity = identity)
    End Function

    Public Function GetPokemonDataByNumber(ByVal number As Integer) As List(Of PokemonData)
        Return _pokemons.FindAll(Function(pm As PokemonData) pm.Number = number)
    End Function

    Public Function GetTypeData(ByVal name As String) As PokemonType Implements PokemonBattle.PokemonData.IDataProvider.GetTypeData
        Return _types.Find(Function(type As PokemonType) type.Name = name)
    End Function

    Public Function GetAllPokemons() As PokemonData() Implements IDataProvider.GetAllPokemons
        Return _pokemons.ToArray()
    End Function

    Public Function GetRandomTeam(ByVal random As System.Random) As TeamData Implements IDataProvider.GetRandomTeam
        Dim newTeam As New TeamData()

        newTeam.CustomInfo.DataName = CustomData.DataName
        newTeam.CustomInfo.DataHash = CustomData.DataHash
        For i As Integer = 0 To 5
            newTeam.Pokemons(i) = RandomPokemon(random)
        Next
        Return newTeam
    End Function

    Public ReadOnly Property CustomData() As CustomDataInfo Implements IDataProvider.CustomData
        Get
            Return _customInfo
        End Get
    End Property

    Public Function CheckPokemon(ByVal pokemon As PokemonCustomInfo) As Boolean Implements IDataProvider.CheckPokemon
        Dim pokemonData As PokemonData = GetPokemonData(pokemon.Identity)
        If pokemonData Is Nothing Then Return False
        If PokemonIsRemoved(pokemon.Identity) Then Return False
        If pokemon.SelectedTrait = 2 AndAlso pokemonData.Trait2 = Trait.无 Then Return False

        Dim selTrait As Trait
        If pokemon.SelectedTrait = 1 Then
            selTrait = pokemonData.Trait1
        Else
            selTrait = pokemonData.Trait2
        End If
        Dim moves As New List(Of Integer)
        For Each move As Integer In pokemon.SelectedMoves
            If move <> MoveData.InvalidId Then
                If pokemonData.GetMove(move) Is Nothing Then
                    Return False
                ElseIf pokemonData.GetMove(move).WithoutTrait = selTrait Then
                    Return False
                End If
                moves.Add(move)
            End If
        Next

        If Not CheckMoves(moves, pokemonData) Then Return False
        Return True
    End Function

    Public Function PokemonIsRemoved(ByVal identity As Integer) As Boolean Implements IDataProvider.PokemonIsRemoved
        If _customData Is Nothing Then Return False
        Return _customData.RemovePokemons.Contains(identity)
    End Function
End Class
