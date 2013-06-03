Imports PokemonBattle.PokemonData
Public Class Move

    Private _Type As PokemonType

    Private _maxPP As Integer
    Private _pp As Integer
    Private _power As Integer
    Private _target As MoveTarget

    Private _protectable As Boolean
    Private _substitute As Boolean

    Private _image As Bitmap

    Private moveData As MoveData

    Public Sub New(ByVal data As MoveData, ByVal ppUp As Boolean)
        moveData = data

        _power = moveData.Power
        _target = moveData.Target
        _protectable = moveData.Protectable
        _substitute = moveData.Substitute

        If ppUp Then
            _maxPP = Convert.ToInt32(data.PP * 1.6)
        Else
            _maxPP = data.PP
        End If
        _pp = _maxPP
        _Type = BattleData.GetTypeData(data.Type)
    End Sub

    Public ReadOnly Property Identity() As Integer
        Get
            Return moveData.Identity
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return moveData.Name
        End Get
    End Property

    Public ReadOnly Property Type() As PokemonType
        Get
            Return _Type
        End Get
    End Property

    Public ReadOnly Property MoveType() As MoveType
        Get
            Return moveData.MoveType
        End Get
    End Property

    Public ReadOnly Property MAXPP() As Integer
        Get
            Return _maxPP
        End Get
    End Property

    Public ReadOnly Property PP() As Integer
        Get
            Return _pp
        End Get
    End Property

    Public ReadOnly Property Acc As Double
        Get
            Return moveData.Accuracy
        End Get
    End Property

    Public ReadOnly Property Power As Integer
        Get
            Return _power
        End Get
    End Property

    Public ReadOnly Property Priority As Integer
        Get
            Return moveData.Priority
        End Get
    End Property

    Public ReadOnly Property Effect As MoveEffect
        Get
            Return moveData.Effect
        End Get
    End Property

    Public ReadOnly Property AddEff1 As MoveAdditionalEffect
        Get
            Return moveData.AddEffect1
        End Get
    End Property

    Public ReadOnly Property AddEffOdds As Double
        Get
            Return moveData.AddEffectOdds
        End Get
    End Property

    Public ReadOnly Property AddEff2 As MoveAdditionalEffect
        Get
            Return moveData.AddEffect2
        End Get
    End Property

    Public ReadOnly Property Target As MoveTarget
        Get
            Return _target
        End Get
    End Property

    Public ReadOnly Property Contact As Boolean
        Get
            Return moveData.Contact
        End Get
    End Property

    Public ReadOnly Property Sound As Boolean
        Get
            Return moveData.Sound
        End Get
    End Property

    Public ReadOnly Property KingRock As Boolean
        Get
            Return moveData.KingRock
        End Get
    End Property

    Public ReadOnly Property Snatchable As Boolean
        Get
            Return moveData.Snatchable
        End Get
    End Property

    Public ReadOnly Property AttackAtTarget As Boolean
        Get
            Return moveData.AttackAtTarget
        End Get
    End Property

    Public ReadOnly Property Substitute As Boolean
        Get
            Return _substitute
        End Get
    End Property

    Public ReadOnly Property Protectable As Boolean
        Get
            Return _protectable
        End Get
    End Property

    Public ReadOnly Property Punch() As Boolean
        Get
            Return moveData.Punch
        End Get
    End Property

    Public Sub PPRecover(ByVal count As Integer)
        SetPP(_pp + count)
    End Sub

    Public Sub Used()
        If _pp > 0 Then SetPP(_pp - 1) : _everUsed = True
    End Sub

    Public Sub UpdateCourse(ByVal pm As Pokemon)
        If pm.HaveType("鬼") Then
            _target = MoveTarget.选一
            _substitute = True
            _protectable = True
        Else
            _target = MoveTarget.自身
            _substitute = False
            _protectable = False
        End If
    End Sub

    Private ppChanged As Boolean
    Public Sub SetPP(ByVal PPValue As Integer)
        _pp = PPValue
        If _pp > MAXPP Then _pp = MAXPP
        If _image IsNot Nothing Then ppChanged = True
    End Sub
    Private typeChanged As Boolean
    Public Sub SetType(ByVal typeValue As PokemonType)
        _Type = typeValue
        If _image IsNot Nothing Then typeChanged = True
    End Sub
    Public Sub SetPower(ByVal powerValue As Integer)
        _power = powerValue
    End Sub

    Public Property EverUsed As Boolean

    Public ReadOnly Property Image As Bitmap
        Get
            If _image Is Nothing Then _image = GetMoveImage(Me)
            If ppChanged Then
                Using graph As Graphics = Graphics.FromImage(_image)
                    Dim str As String = "PP " & PP & "/" & MAXPP
                    graph.FillRectangle(New SolidBrush(MoveImage.GetPixel(52, 33)), _
                        New Rectangle(52, 33, 56, 12))
                    graph.DrawString(str, PPFont, Brushes.Black, 52, 33)
                End Using
                ppChanged = False
            End If
            If typeChanged Then
                Using graph As Graphics = Graphics.FromImage(_image)
                    DrawMoveFrame(graph, CType(_Type.Image, Bitmap))
                End Using
                typeChanged = False
            End If
            Return _image
        End Get
    End Property

    Private Shared MoveImage As Bitmap = My.Resources.Move
    Private Shared PPFont As Font = New Font("宋体", 12, FontStyle.Bold, GraphicsUnit.Pixel)
    Private Shared NameFont As Font = New Font("宋体", 16, FontStyle.Regular, GraphicsUnit.Pixel)
    Private Shared TypePath As GraphicsPath()
    Shared Sub New()
        TypePath = New GraphicsPath() {New GraphicsPath(), New GraphicsPath()}
        TypePath(0).AddRectangle(New Rectangle(4, 50, 116, 1))
        TypePath(0).AddRectangle(New Rectangle(3, 49, 118, 1))
        TypePath(0).AddRectangle(New Rectangle(3, 48, 5, 1))
        TypePath(0).AddRectangle(New Rectangle(116, 48, 5, 1))

        TypePath(1).AddRectangle(New Rectangle(5, 4, 114, 1))
        TypePath(1).AddRectangle(New Rectangle(4, 5, 116, 5))
        TypePath(1).AddRectangle(New Rectangle(7, 10, 1, 1))
        TypePath(1).AddRectangle(New Rectangle(116, 10, 1, 1))
    End Sub

    Private Shared Function GetMoveImage(ByVal moveVal As Move) As Bitmap
        Dim newImg As Bitmap = New Bitmap(My.Resources.Move)
        Using graph As Graphics = Graphics.FromImage(newImg)
            If moveVal.Type.Name <> "无" Then DrawMoveFrame(graph, CType(moveVal.Type.Image, Bitmap))
            graph.DrawString(moveVal.Name, NameFont, Brushes.Black, 16, 14)
            graph.DrawString("PP " & moveVal.PP & "/" & moveVal.MAXPP, PPFont, Brushes.Black, 52, 33)
        End Using
        Return newImg
    End Function

    Private Shared Sub DrawMoveFrame(ByVal graph As Graphics, ByVal typeImg As Bitmap)
        Dim color1 As Color = typeImg.GetPixel(5, 0)
        Dim color2 As Color = typeImg.GetPixel(5, 3)
        Dim color3 As Color = typeImg.GetPixel(5, typeImg.Height - 1)

        Dim rect1 As New Rectangle(117, 7, 4, 43)
        Dim rect2 As New Rectangle(3, 7, 4, 43)


        Dim brush As LinearGradientBrush
        brush = New LinearGradientBrush(New Rectangle(0, 0, 118, 3), color2, color3, LinearGradientMode.Vertical)
        graph.FillPath(brush, TypePath(0))
        brush = New LinearGradientBrush(New Rectangle(0, 0, 116, 7), color2, color1, LinearGradientMode.Vertical)
        graph.FillPath(brush, TypePath(1))

        brush = New LinearGradientBrush(rect1, color3, color2, LinearGradientMode.Horizontal)
        graph.FillRectangle(brush, rect1)

        brush = New LinearGradientBrush(rect2, color2, color3, LinearGradientMode.Horizontal)
        graph.FillRectangle(brush, rect2)

        graph.DrawImageUnscaled(typeImg, 14, 32)
    End Sub

End Class
