Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Friend Class ImageManager 
    Private Shared formatter As BinaryFormatter
    Shared Sub New()
        formatter = New BinaryFormatter
        images = New Dictionary(Of Long, WeakReference)
        imageLocker = New Object
    End Sub
    Private Sub New()
    End Sub

    Private Shared data As FileStream
    Private Shared images As Dictionary(Of Long, WeakReference)
    Private Shared imageLocker As Object
    Public Shared Sub LoadData(ByVal filePath As String)
        data = New FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite)
    End Sub
    Public Shared Sub Close()
        data.Close()
    End Sub
    Public Shared Function GetImage(ByVal position As Long) As Bitmap
        If position < 0 Then Return Nothing
        Try
            SyncLock imageLocker
                If images.ContainsKey(position) Then
                    If Not images(position).IsAlive Then images(position) = New WeakReference(ReadData(position, data))
                Else
                    Dim reference As New WeakReference(ReadData(position, data))
                    images.Add(position, reference)
                End If
                Return CType(images(position).Target, Bitmap)
            End SyncLock
        Catch ex As Exception
        End Try
        Return Nothing
    End Function

    Private Shared custom As FileStream
    Private Shared customImages As Dictionary(Of Long, WeakReference)
    Private Shared customLocker As Object
    Private Shared customOffset As Long
    Public Shared Sub LoadCustomData(ByVal path As String, ByVal offset As Long)
        If customImages Is Nothing Then
            customImages = New Dictionary(Of Long, WeakReference)
            customLocker = New Object
        Else
            If custom IsNot Nothing Then CloseCustom()
        End If
        custom = New FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite)
        customOffset = offset
    End Sub
    Public Shared Sub CloseCustom()
        If custom IsNot Nothing Then custom.Close() : custom = Nothing
        If customImages IsNot Nothing Then customImages.Clear()
        customOffset = 0
    End Sub
    Public Shared Function GetCustomImage(ByVal position As Long) As Bitmap
        If position < 0 Then Return Nothing
        Try
            SyncLock customLocker
                If customImages.ContainsKey(position) Then
                    If Not customImages(position).IsAlive Then
                        customImages(position) = New WeakReference(ReadData(position + customOffset, custom))
                    End If
                Else
                    Dim reference As New WeakReference(ReadData(position + customOffset, custom))
                    customImages.Add(position, reference)
                End If
                Return CType(customImages(position).Target, Bitmap)
            End SyncLock
        Catch ex As Exception
        End Try
        Return Nothing
    End Function


    Public Shared Function SavaImage(ByVal image As Bitmap, ByVal data As Stream) As Long
        If image Is Nothing Then Return -1
        If data Is Nothing Then Return -1
        Dim newPosition As Long = -1
        Try
            data.Seek(0, SeekOrigin.End)
            newPosition = data.Position
            formatter.Serialize(data, image)
        Catch ex As Exception
            newPosition = -1
        End Try
        Return newPosition
    End Function
    Private Shared Function ReadData(ByVal position As Long, ByVal stream As FileStream) As Object
        Try
            stream.Seek(position, SeekOrigin.Begin)
            Return formatter.Deserialize(stream)
        Catch ex As Exception
        End Try
        Return Nothing
    End Function
End Class
