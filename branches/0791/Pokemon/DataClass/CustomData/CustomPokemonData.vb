<Serializable()> Public Class CustomPokemonData

    Private mFrontImage, mFrontImageF As Long
    Private mBackImage, mBackImageF As Long
    Private mIcon As Long
    Private mFrame, mFrameF As Long

    Private mNameBase As String
    Private mWeight As Double

    Private _identity As Integer
    Public ReadOnly Property Identity() As Integer
        Get
            Return _identity
        End Get
    End Property

    Public Sub New(ByVal _identity As Integer)
        _identity = Identity
        mType1 = ""
        mType2 = ""
        mNameBase = ""
        FrontImage = -1
        FrontImageF = -1
        BackImage = -1
        BackImageF = -1
        Icon = -1
        Frame = -1
        FrameF = -1
    End Sub

    Public Property FrontImage() As Long
        Get
            Return mFrontImage
        End Get
        Set(ByVal value As Long)
            mFrontImage = value
        End Set
    End Property
    Public Property FrontImageF() As Long
        Get
            Return mFrontImageF
        End Get
        Set(ByVal value As Long)
            mFrontImageF = value
        End Set
    End Property
    Public Property BackImage() As Long
        Get
            Return mBackImage
        End Get
        Set(ByVal value As Long)
            mBackImage = value
        End Set
    End Property
    Public Property BackImageF() As Long
        Get
            Return mBackImageF
        End Get
        Set(ByVal value As Long)
            mBackImageF = value
        End Set
    End Property
    Public Property Frame() As Long
        Get
            Return mFrame
        End Get
        Set(ByVal value As Long)
            mFrame = value
        End Set
    End Property
    Public Property FrameF() As Long
        Get
            Return mFrameF
        End Get
        Set(ByVal value As Long)
            mFrameF = value
        End Set
    End Property
    Public Property Icon() As Long
        Get
            Return mIcon
        End Get
        Set(ByVal value As Long)
            mIcon = value
        End Set
    End Property
    Public Property NameBase() As String
        Get
            Return mNameBase
        End Get
        Set(ByVal value As String)
            mNameBase = value
        End Set
    End Property
    Public Property Weight() As Double
        Get
            Return mWeight
        End Get
        Set(ByVal value As Double)
            mWeight = value
        End Set
    End Property
    Private mHPBase As Byte
    Private mAttackBase As Byte
    Private mDefenceBase As Byte
    Private mSpeedBase As Byte
    Private mSpAttackBase As Byte
    Private mSpDefenceBase As Byte
    Private mType1 As String
    Private mType2 As String
    Private mTrait1 As Integer
    Private mTrait2 As Integer
    Private mEggGroup1 As Integer
    Private mEggGroup2 As Integer
    Private mBeforeEvolution As Integer
    Private mAfterEvolution As List(Of Integer) = New List(Of Integer)
    Private mMoves As List(Of String) = New List(Of String)
    Private mGenderRestriction As Integer
    Private mNumber As Integer
    Public Property HPBase() As Byte
        Get
            Return mHPBase
        End Get
        Set(ByVal value As Byte)
            mHPBase = value
        End Set
    End Property
    Public Property AttackBase() As Byte
        Get
            Return mAttackBase
        End Get
        Set(ByVal value As Byte)
            mAttackBase = value
        End Set
    End Property
    Public Property DefenceBase() As Byte
        Get
            Return mDefenceBase
        End Get
        Set(ByVal value As Byte)
            mDefenceBase = value
        End Set
    End Property
    Public Property SpeedBase() As Byte
        Get
            Return mSpeedBase
        End Get
        Set(ByVal value As Byte)
            mSpeedBase = value
        End Set
    End Property
    Public Property SpAttackBase() As Byte
        Get
            Return mSpAttackBase
        End Get
        Set(ByVal value As Byte)
            mSpAttackBase = value
        End Set
    End Property
    Public Property SpDefenceBase() As Byte
        Get
            Return mSpDefenceBase
        End Get
        Set(ByVal value As Byte)
            mSpDefenceBase = value
        End Set
    End Property
    Public Property Type1() As String
        Get
            Return mType1
        End Get
        Set(ByVal value As String)
            mType1 = value
        End Set
    End Property
    Public Property Type2() As String
        Get
            Return mType2
        End Get
        Set(ByVal value As String)
            mType2 = value
        End Set
    End Property
    Public Property Trait1() As Integer
        Get
            Return mTrait1
        End Get
        Set(ByVal value As Integer)
            mTrait1 = value
        End Set
    End Property
    Public Property Trait2() As Integer
        Get
            Return mTrait2
        End Get
        Set(ByVal value As Integer)
            mTrait2 = value
        End Set
    End Property
    Public Property EggGroup1() As Integer
        Get
            Return mEggGroup1
        End Get
        Set(ByVal value As Integer)
            mEggGroup1 = value
        End Set
    End Property
    Public Property EggGroup2() As Integer
        Get
            Return mEggGroup2
        End Get
        Set(ByVal value As Integer)
            mEggGroup2 = value
        End Set
    End Property
    Public Property BeforeEvolution() As Integer
        Get
            Return mBeforeEvolution
        End Get
        Set(ByVal value As Integer)
            mBeforeEvolution = value
        End Set
    End Property
    Public ReadOnly Property AfterEvolution() As List(Of Integer)
        Get
            Return mAfterEvolution
        End Get
    End Property
    Public Property Moves() As List(Of String)
        Get
            Return mMoves
        End Get
        Set(ByVal value As List(Of String))
            mMoves = value
        End Set
    End Property
    Public Property GenderRestriction() As Integer
        Get
            Return mGenderRestriction
        End Get
        Set(ByVal value As Integer)
            mGenderRestriction = value
        End Set
    End Property
    Public Property Number() As Integer
        Get
            Return mNumber
        End Get
        Set(ByVal value As Integer)
            mNumber = value
        End Set
    End Property

    Public Function Clone() As CustomPokemonData
        Dim data As CustomPokemonData = CType(MemberwiseClone(), CustomPokemonData)
        data.mAfterEvolution = New List(Of Integer)
        data.mAfterEvolution.AddRange(mAfterEvolution)
        data.mMoves = New List(Of String)
        data.mMoves.AddRange(mMoves)
        Return data
    End Function
End Class
