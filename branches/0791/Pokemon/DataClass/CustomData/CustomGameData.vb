<Serializable()> Public Class CustomGameData

    Private _name As String
    Private _idBase As Integer = 1000

    Public Sub New(ByVal name As String)
        _name = name
        _addPokemonList = New List(Of CustomPokemonData)
        _updatePokemonList = New List(Of UpdatePokemonData)
        _removePokemonList = New List(Of String)
        _imageList = New List(Of Bitmap)
    End Sub

    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _addPokemonList As List(Of CustomPokemonData)
    Private _updatePokemonList As List(Of UpdatePokemonData)
    Private _removePokemonList As List(Of String)
    Public ReadOnly Property NewPokemon() As List(Of CustomPokemonData)
        Get
            Return _addPokemonList
        End Get
    End Property
    Public ReadOnly Property UpdatePokemon() As List(Of UpdatePokemonData)
        Get
            Return _updatePokemonList
        End Get
    End Property
    Public ReadOnly Property RemovePokemon() As List(Of String)
        Get
            Return _removePokemonList
        End Get
    End Property
    Public Function NewIdentity() As Integer
        _idBase += 1
        Return _idBase
    End Function

    Private _imageList As List(Of Bitmap)
    Public ReadOnly Property Images() As List(Of Bitmap)
        Get
            Return _imageList
        End Get
    End Property

    Public Function GetImage(ByVal index As Long) As Bitmap
        If index = -1 Then Return Nothing
        Return _imageList(Convert.ToInt32(index))
    End Function

    Public Sub Save(ByVal path As String)
        Using stream As New FileStream(path, FileMode.Create)
            Dim formatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
            Using writer As New BinaryWriter(stream)
                writer.Write(_name)
                formatter.Serialize(stream, _addPokemonList)
                formatter.Serialize(stream, _updatePokemonList)
                formatter.Serialize(stream, _removePokemonList)
                Dim positions As Long() = New Long(_imageList.Count - 1) {}
                Dim imgBuffer As Byte()
                Using ms As New MemoryStream()
                    For i As Integer = 0 To _imageList.Count - 1
                        positions(i) = ImageManager.SavaImage(_imageList(i), ms)
                    Next
                    imgBuffer = ms.GetBuffer
                End Using
                formatter.Serialize(stream, positions)
                writer.Write(imgBuffer)
            End Using
        End Using
    End Sub

    Public Shared Function FromFile(ByVal path As String, ByRef imageOffset As Long) As CustomGameData
        Using stream As New FileStream(path, FileMode.Open)
            Dim data As CustomGameData
            Using reader As New BinaryReader(stream)
                data = New CustomGameData(reader.ReadString())
                Dim formatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
                data._addPokemonList = CType(formatter.Deserialize(stream), List(Of CustomPokemonData))
                data._updatePokemonList = CType(formatter.Deserialize(stream), List(Of UpdatePokemonData))
                data._removePokemonList = CType(formatter.Deserialize(stream), List(Of String))
                Dim positions As Long() = CType(formatter.Deserialize(stream), Long())
                imageOffset = stream.Position
                For Each newPM As CustomPokemonData In data.NewPokemon
                    If newPM.FrontImage <> -1 Then newPM.FrontImage = positions(Convert.ToInt32(newPM.FrontImage))
                    If newPM.FrontImageF <> -1 Then newPM.FrontImageF = positions(Convert.ToInt32(newPM.FrontImageF))

                    If newPM.BackImage <> -1 Then newPM.BackImage = positions(Convert.ToInt32(newPM.BackImage))
                    If newPM.BackImageF <> -1 Then newPM.BackImageF = positions(Convert.ToInt32(newPM.BackImageF))

                    If newPM.Frame <> -1 Then newPM.Frame = positions(Convert.ToInt32(newPM.Frame))
                    If newPM.FrameF <> -1 Then newPM.FrameF = positions(Convert.ToInt32(newPM.FrameF))

                    If newPM.Icon <> -1 Then newPM.Icon = positions(Convert.ToInt32(newPM.Icon))
                Next
            End Using
            Return data
        End Using
        Return Nothing
    End Function

    Public Overloads Function Equals(ByVal data As CustomGameData) As Boolean
        If data._addPokemonList.Count <> _addPokemonList.Count Then Return False
        For i As Integer = 0 To _addPokemonList.Count - 1
            If Not data._addPokemonList(i).Equals(_addPokemonList(i)) Then Return False
        Next

        If data._updatePokemonList.Count <> _updatePokemonList.Count Then Return False
        For i As Integer = 0 To _updatePokemonList.Count - 1
            If Not data._updatePokemonList(i).Equals(_updatePokemonList(i)) Then Return False
        Next

        If data._removePokemonList.Count <> _removePokemonList.Count Then Return False
        For i As Integer = 0 To _removePokemonList.Count - 1
            If data._removePokemonList(i) <> _removePokemonList(i) Then Return False
        Next
        If data._imageList.Count <> _imageList.Count Then Return False
        For i As Integer = 0 To _imageList.Count - 1
            If Not data._imageList(i).Equals(_imageList(i)) Then Return False
        Next

        Return True
    End Function

    Public Function Clone() As CustomGameData
        Dim data As CustomGameData = New CustomGameData(_name)
        data.RemovePokemon.AddRange(_removePokemonList)
        For Each pm As CustomPokemonData In _addPokemonList
            data._addPokemonList.Add(pm.Clone())
        Next
        For Each pm As UpdatePokemonData In _updatePokemonList
            data._updatePokemonList.Add(pm.Clone())
        Next
        data._imageList.AddRange(_imageList)
        Return data
    End Function
End Class
