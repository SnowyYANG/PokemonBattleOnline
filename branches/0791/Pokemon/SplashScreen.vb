Public NotInheritable Class SplashScreen


    Private Sub SplashScreen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ApplicationTitle.Text = "Pokemon Battle Online"

        Version.Text = System.String.Format(Version.Text, _
            My.Application.Info.Version.Major, My.Application.Info.Version.Minor, _
            My.Application.Info.Version.Build, My.Application.Info.Version.Revision)

        ApplicationTitle.Location = New Point((Me.Width - ApplicationTitle.Width) \ 2, ApplicationTitle.Location.Y)
        Version.Location = New Point((Me.Width - Version.Width) \ 2, Version.Location.Y)

        picIcon.Image = My.Resources.mew.ToBitmap
    End Sub

    Private Sub lblExit_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) _
        Handles lblExit.LinkClicked
        Environment.Exit(Environment.ExitCode)
    End Sub
End Class
