Imports System.Drawing.Drawing2D
Imports System.ComponentModel

<DefaultEvent("TextChanged")>
Public Class RoundedTextBox
    Inherits Panel

    Private WithEvents txt As New TextBox()
    Private isFocused As Boolean = False
    Private _borderRadius As Integer = 8
    Private _backColor As Color = Color.White
    Private _focusBorderColor As Color = Color.DarkGray
    Private _normalBorderColor As Color = Color.FromArgb(200, 200, 200)

    Public Sub New()
        ' Configure Panel
        Me.Size = New Size(200, 30)
        Me.BackColor = Color.Transparent
        Me.MinimumSize = New Size(50, 20)

        ' Configure TextBox
        txt.BorderStyle = BorderStyle.None
        txt.Font = New Font("Segoe UI", 10)
        txt.TextAlign = HorizontalAlignment.Left
        txt.Anchor = AnchorStyles.Left Or AnchorStyles.Right

        ' Add textbox to panel
        Me.Controls.Add(txt)

        ' Apply colors after adding to panel
        txt.BackColor = _backColor
        txt.ForeColor = Color.Black

        UpdateTextBoxPosition()

        ' Set double buffering
        Me.SetStyle(ControlStyles.UserPaint Or
                    ControlStyles.AllPaintingInWmPaint Or
                    ControlStyles.OptimizedDoubleBuffer Or
                    ControlStyles.ResizeRedraw, True)
        Me.UpdateStyles()
    End Sub

    ' Properties to access TextBox
    <Category("Appearance")>
    <Description("The text content of the textbox")>
    Public Overrides Property Text As String
        Get
            Return txt.Text
        End Get
        Set(value As String)
            txt.Text = value
        End Set
    End Property

    <Category("Appearance")>
    <Description("The font of the text")>
    Public Property TextFont As Font
        Get
            Return txt.Font
        End Get
        Set(value As Font)
            txt.Font = value
        End Set
    End Property

    <Category("Appearance")>
    <Description("The radius of the rounded corners")>
    <DefaultValue(8)>
    Public Property BorderRadius As Integer
        Get
            Return _borderRadius
        End Get
        Set(value As Integer)
            If value >= 0 Then
                _borderRadius = value
                Me.Invalidate()
            End If
        End Set
    End Property

    ' Custom color properties
    <Category("Appearance")>
    <Description("The background color of the textbox")>
    Public Property TextBoxBackColor As Color
        Get
            Return _backColor
        End Get
        Set(value As Color)
            _backColor = value
            txt.BackColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    <Description("The text color of the textbox")>
    Public Property TextColor As Color
        Get
            Return txt.ForeColor
        End Get
        Set(value As Color)
            txt.ForeColor = value
        End Set
    End Property

    <Category("Appearance")>
    <Description("The border color when the textbox is focused")>
    Public Property FocusBorderColor As Color
        Get
            Return _focusBorderColor
        End Get
        Set(value As Color)
            _focusBorderColor = value
            If isFocused Then
                Me.Invalidate()
            End If
        End Set
    End Property

    <Category("Appearance")>
    <Description("The border color when the textbox is not focused")>
    Public Property NormalBorderColor As Color
        Get
            Return _normalBorderColor
        End Get
        Set(value As Color)
            _normalBorderColor = value
            If Not isFocused Then
                Me.Invalidate()
            End If
        End Set
    End Property

    <Category("Behavior")>
    <Description("The password character for the textbox")>
    Public Property PasswordChar As Char
        Get
            Return txt.PasswordChar
        End Get
        Set(value As Char)
            txt.PasswordChar = value
        End Set
    End Property

    <Category("Behavior")>
    <Description("Whether the textbox accepts multiline text")>
    Public Property Multiline As Boolean
        Get
            Return txt.Multiline
        End Get
        Set(value As Boolean)
            txt.Multiline = value
        End Set
    End Property

    <Category("Behavior")>
    <Description("The maximum length of text")>
    Public Property MaxLength As Integer
        Get
            Return txt.MaxLength
        End Get
        Set(value As Integer)
            txt.MaxLength = value
        End Set
    End Property

    <Category("Behavior")>
    <Description("Whether the textbox is read-only")>
    Public Property [ReadOnly] As Boolean
        Get
            Return txt.ReadOnly
        End Get
        Set(value As Boolean)
            txt.ReadOnly = value
        End Set
    End Property

    ' Add Clear method
    Public Sub Clear()
        txt.Clear()
    End Sub

    ' Focus method
    Public Shadows Sub Focus()
        txt.Focus()
    End Sub

    Private Sub UpdateTextBoxPosition()
        ' Calculate vertical centering
        Dim textBoxHeight As Integer = txt.PreferredHeight
        Dim verticalPadding As Integer = (Me.Height - textBoxHeight) \ 2
        Dim horizontalPadding As Integer = 10

        ' Ensure minimum padding
        If verticalPadding < 5 Then verticalPadding = 5

        ' Position the textbox
        txt.Location = New Point(horizontalPadding, verticalPadding)
        txt.Width = Me.Width - (horizontalPadding * 2)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Create rounded rectangle path
        Dim rect As New Rectangle(0, 0, Me.Width - 1, Me.Height - 1)
        Dim path As GraphicsPath = GetRoundedRectangle(rect, _borderRadius)

        ' Fill background
        Using brush As New SolidBrush(_backColor)
            g.FillPath(brush, path)
        End Using

        ' Draw border
        Dim borderColor As Color
        Dim borderWidth As Single

        If isFocused Then
            borderColor = _focusBorderColor
            borderWidth = 2.5F
        Else
            borderColor = _normalBorderColor
            borderWidth = 1.5F
        End If

        Using pen As New Pen(borderColor, borderWidth)
            g.DrawPath(pen, path)
        End Using
    End Sub

    Private Function GetRoundedRectangle(bounds As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim diameter As Integer = radius * 2
        Dim arc As New Rectangle(bounds.X, bounds.Y, diameter, diameter)

        path.AddArc(arc, 180, 90)
        arc.X = bounds.Right - diameter
        path.AddArc(arc, 270, 90)
        arc.Y = bounds.Bottom - diameter
        path.AddArc(arc, 0, 90)
        arc.X = bounds.Left
        path.AddArc(arc, 90, 90)
        path.CloseFigure()

        Return path
    End Function

    Private Sub txt_Enter(sender As Object, e As EventArgs) Handles txt.Enter
        isFocused = True
        Me.Invalidate()
    End Sub

    Private Sub txt_Leave(sender As Object, e As EventArgs) Handles txt.Leave
        isFocused = False
        Me.Invalidate()
    End Sub

    Private Sub txt_TextChanged(sender As Object, e As EventArgs) Handles txt.TextChanged
        ' Raise the TextChanged event for the parent control
        OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        UpdateTextBoxPosition()
        Me.Invalidate()
    End Sub

    Protected Overrides Sub OnFontChanged(e As EventArgs)
        MyBase.OnFontChanged(e)
        txt.Font = Me.Font
        UpdateTextBoxPosition()
    End Sub
End Class