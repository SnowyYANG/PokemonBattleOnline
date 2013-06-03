Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Public Class PokemonCounter
    Private Shared formatter As BinaryFormatter

    Shared Sub New()
        formatter = New BinaryFormatter()
    End Sub

    Private Shared pokemonDict As Dictionary(Of Integer, Integer)

    Public Shared Sub CreateDict(ByVal dataPath As String, ByVal dictLength As Integer)
        pokemonDict = New Dictionary(Of Integer, Integer)()
        For i As Integer = 1 To dictLength
            pokemonDict.Add(i, 0)
        Next
        Using stream As New FileStream(dataPath, FileMode.Create, FileAccess.Write)
            formatter.Serialize(stream, pokemonDict)
        End Using
    End Sub

    Public Shared Sub LoadDict(ByVal dataPath As String)
        Using stream As New FileStream(dataPath, FileMode.Open, FileAccess.Read)
            pokemonDict = CType(formatter.Deserialize(stream), Dictionary(Of Integer, Integer))
        End Using
    End Sub

    Public Shared Sub SaveDict(ByVal dataPath As String)
        Using stream As New FileStream(dataPath, FileMode.OpenOrCreate, FileAccess.Write)
            formatter.Serialize(stream, pokemonDict)
        End Using
    End Sub

    Public Shared Sub Count(ByVal id As Integer)
        If pokemonDict IsNot Nothing AndAlso pokemonDict.ContainsKey(id) Then pokemonDict(id) += 1
    End Sub

    Public Shared Function GetCount(ByVal id As Integer) As Integer
        If pokemonDict Is Nothing OrElse Not pokemonDict.ContainsKey(id) Then Return 0
        Return pokemonDict(id)
    End Function
End Class
