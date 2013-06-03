Imports System.Text.RegularExpressions
Friend Class ListSorter
    Implements System.Collections.IComparer

    Private col As Integer
    Private order As SortOrder

    Public Sub New()
        Me.New(0)
    End Sub

    Public Sub New(ByVal column As Integer)
        col = column
        order = SortOrder.Ascending
    End Sub

    Public Sub SetColumn(ByVal column As Integer)
        If col = column Then
            If order = SortOrder.Ascending Then
                order = SortOrder.Descending
            Else
                order = SortOrder.Ascending
            End If
        Else
            col = column
            order = SortOrder.Ascending
        End If
    End Sub

    Public ReadOnly Property Column() As Integer
        Get
            Return col
        End Get
    End Property

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare
        Dim text1 As String = CType(x, ListViewItem).SubItems(col).Text
        Dim text2 As String = CType(y, ListViewItem).SubItems(col).Text
        Dim result As Integer
        Dim number1, number2 As Double

        If Double.TryParse(text1, number1) AndAlso Double.TryParse(text2, number2) Then
            result = number1.CompareTo(number2)
        Else
            result = String.Compare(text1, text2)
        End If
        If order = SortOrder.Descending Then result = -result
        Return result
    End Function

End Class
