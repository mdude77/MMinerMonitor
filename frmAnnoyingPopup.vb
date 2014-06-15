Public Class frmAnnoyingPopup

    Private Sub timer_beep_Tick(sender As Object, e As System.EventArgs) Handles timer_beep.Tick

        Beep()
        Beep()

    End Sub

    Private Sub frmAnnoyingPopup_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.timer_beep.Enabled = False

    End Sub
End Class