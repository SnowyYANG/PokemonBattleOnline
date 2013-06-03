Imports PokemonBattle.BattleNetwork
Public Class TurnResult

    Private _turnComplete As Boolean
    Private _passPokemon As Pokemon
    Private _deadPokemons As List(Of Pokemon) = New List(Of Pokemon)

    Public Sub New(ByVal turnEnded As Boolean)
        _turnComplete = turnEnded
    End Sub

    Public ReadOnly Property TurnComplete() As Boolean
        Get
            Return _turnComplete
        End Get
    End Property
    Public Property PassPokemon() As Pokemon
        Get
            Return _passPokemon
        End Get
        Set(ByVal value As Pokemon)
            _passPokemon = value
        End Set
    End Property
    Public ReadOnly Property DeadPokemons() As List(Of Pokemon)
        Get
            Return _deadPokemons
        End Get
    End Property
End Class
