<Serializable()> Public Class UpdatePokemonData

    Private _nameBase As String

    Private HPBase, AttackBase, DefenceBase, SpeedBase, SpAttackBase, SpDefenceBase As Byte
    Private weight As Double
    Private type1, type2 As String

    Private trait1, trait2 As Integer

    Private addMoveList As List(Of String)
    Private removeMoveList As List(Of String)
    Public Sub New(ByVal identity As Integer, ByVal pokemonName As String)
        _nameBase = pokemonName
        _identity = identity
        type1 = ""
        type2 = ""
        addMoveList = New List(Of String)
        removeMoveList = New List(Of String)
    End Sub

    Public ReadOnly Property Name() As String
        Get
            Return _nameBase
        End Get
    End Property
    Public Property NewHPBase() As Byte
        Get
            Return HPBase
        End Get
        Set(ByVal value As Byte)
            HPBase = value
        End Set
    End Property
    Public Property NewAttackBase() As Byte
        Get
            Return AttackBase
        End Get
        Set(ByVal value As Byte)
            AttackBase = value
        End Set
    End Property
    Public Property NewDefenceBase() As Byte
        Get
            Return DefenceBase
        End Get
        Set(ByVal value As Byte)
            DefenceBase = value
        End Set
    End Property
    Public Property NewSpeedBase() As Byte
        Get
            Return SpeedBase
        End Get
        Set(ByVal value As Byte)
            SpeedBase = value
        End Set
    End Property
    Public Property NewSpAttackBase() As Byte
        Get
            Return SpAttackBase
        End Get
        Set(ByVal value As Byte)
            SpAttackBase = value
        End Set
    End Property
    Public Property NewSpDefenceBase() As Byte
        Get
            Return SpDefenceBase
        End Get
        Set(ByVal value As Byte)
            SpDefenceBase = value
        End Set
    End Property
    Public Property NewWeight() As Double
        Get
            Return weight
        End Get
        Set(ByVal value As Double)
            weight = value
        End Set
    End Property

    Public Property NewType1() As String
        Get
            Return type1
        End Get
        Set(ByVal value As String)
            type1 = value
        End Set
    End Property
    Public Property NewType2() As String
        Get
            Return type2
        End Get
        Set(ByVal value As String)
            type2 = value
        End Set
    End Property
    Public Property NewTrait1() As Integer
        Get
            Return trait1
        End Get
        Set(ByVal value As Integer)
            trait1 = value
        End Set
    End Property
    Public Property NewTrait2() As Integer
        Get
            Return trait2
        End Get
        Set(ByVal value As Integer)
            trait2 = value
        End Set
    End Property

    Private _identity As Integer
    Public ReadOnly Property Identity() As Integer
        Get
            Return _identity
        End Get
    End Property

    Public ReadOnly Property AddMove() As List(Of String)
        Get
            Return addMoveList
        End Get
    End Property
    Public ReadOnly Property RemoveMove() As List(Of String)
        Get
            Return removeMoveList
        End Get
    End Property

    Public Function Clone() As UpdatePokemonData
        Dim obj As UpdatePokemonData = CType(Me.MemberwiseClone, UpdatePokemonData)
        obj.addMoveList = New List(Of String)
        obj.addMoveList.AddRange(addMoveList)
        obj.removeMoveList.AddRange(removeMoveList)
        Return obj
    End Function
End Class
