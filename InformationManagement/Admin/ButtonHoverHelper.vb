Imports System.Drawing
Imports System.Windows.Forms

Public Module ButtonHoverHelper
    Public Sub AddHoverEffect(btn As Button)
        AddHandler btn.MouseEnter, Sub(sender, e)
                                       btn.BackColor = Color.FromArgb(40, 50, 70) ' Darker shade
                                       btn.ForeColor = Color.White
                                       btn.Cursor = Cursors.Hand
                                   End Sub

        AddHandler btn.MouseLeave, Sub(sender, e)
                                       btn.BackColor = Color.FromArgb(26, 38, 50) ' Original color
                                       btn.ForeColor = Color.White
                                       btn.Cursor = Cursors.Default
                                   End Sub
    End Sub
End Module
