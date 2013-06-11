Friend Class FrmBattleClient_4P
 
    Public Sub New(ByVal Address As String, ByVal name As String, ByVal index As Integer, ByVal myTeamValue As TeamInfo, _
        ByVal data As GameData)
        MyBase.New(Address, name, myTeamValue, data, battleMode.Double_4P, index)
        ' �˵����� Windows ���������������ġ�
        InitializeComponent()

        ' �� InitializeComponent() ����֮�������κγ�ʼ����
    End Sub
    Protected Overrides Sub GetTeam(ByVal client As NetworkClient)
        SetProgress(20)

        SetProgressText("���ڷ�������")
        client.Send(myIndex)
        client.Send(myName)

        Dim rules As BattleRules = CType(client.ReadString, BattleRules)
        Dim seed As Integer = Convert.ToInt32(client.ReadString)

        If Convert.ToBoolean(rules And BattleRules.Random) = False Then client.Serialize(myTeamInfo)

        SetProgress(50)
        SetProgressText("���ڽ�������")
        Dim players As String()
        Dim teams As TeamInfo()
        players = New String(3) {}
        teams = New TeamInfo(3) {}

        For i As Integer = 0 To 3
            If i = myIndex - 1 Then
                players(i) = myName 
            Else
                players(i) = client.ReadString 
            End If
        Next
        If Convert.ToBoolean(rules And BattleRules.Random) Then
            Dim random As New Random(seed)
            For i As Integer = 0 To 3
                teams(i) = GameData.RandomTeam(random)
            Next
        Else
            For i As Integer = 0 To 3
                If i = myIndex - 1 Then
                    teams(i) = myTeamInfo
                Else
                    teams(i) = CType(client.Deserialize, TeamInfo)
                End If
            Next
        End If
        SetProgress(80)
        SetProgressText("���ڳ�ʼ����ս")
        StartBattle(teams, seed, players, rules)
    End Sub
    Protected Overrides Sub GetMessage(ByVal sender As NetworkClient, ByVal message As String) 
        MyBase.GetMessage(sender, message)
    End Sub

    Protected Overrides Sub SendTie()

    End Sub
    Protected Overrides Sub Tied(ByVal message As String)

    End Sub
    Protected Overrides Sub GetTie(ByVal message As String)

    End Sub
    Protected Overrides Sub RefuseTie(ByVal message As String)

    End Sub
End Class