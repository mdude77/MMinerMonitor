Public Class frmGetMinerInfo

    Private Sub cmdCopy_Click(sender As System.Object, e As System.EventArgs) Handles cmdCopy.Click

        Clipboard.SetText(Me.txt1.Text & vbCrLf & vbCrLf & Me.txt2.Text & vbCrLf & vbCrLf & Me.txt3.Text & vbCrLf & Me.txt4.Text)

    End Sub
End Class