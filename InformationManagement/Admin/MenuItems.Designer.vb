<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MenuItems
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As New System.Windows.Forms.DataGridViewCellStyle()

        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()

        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnToggleAvailability = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.AddMenuItemsbtn = New System.Windows.Forms.Button()
        Me.btnCheckIngredients = New System.Windows.Forms.Button()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblSearch = New System.Windows.Forms.Label()

        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Category = New System.Windows.Forms.ComboBox()
        Me.lblFilter = New System.Windows.Forms.Label()

        Me.DataGridMenu = New System.Windows.Forms.DataGridView()

        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lblTotalItems = New System.Windows.Forms.Label()

        ' ===== Panel1 =====
        Me.Panel1.BackColor = System.Drawing.Color.GhostWhite
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Size = New System.Drawing.Size(1250, 79)

        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(22, 21)
        Me.Label1.Text = "Menu Items Management"

        ' ===== Panel2 =====
        Me.Panel2.BackColor = System.Drawing.Color.GhostWhite
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Padding = New Padding(9)
        Me.Panel2.Size = New System.Drawing.Size(1250, 61)

        Me.lblSearch.Text = "Search:"
        Me.lblSearch.Font = New Font("Segoe UI", 11.0!, FontStyle.Bold)
        Me.lblSearch.Location = New Point(13, 22)

        Me.txtSearch.Font = New Font("Segoe UI", 11.0!)
        Me.txtSearch.ForeColor = Color.Gray
        Me.txtSearch.Location = New Point(77, 19)
        Me.txtSearch.Size = New Size(480, 27)
        Me.txtSearch.Text = "Search menu items..."

        Me.btnCheckIngredients.Text = "🥘 Check Ingredients"
        Me.btnCheckIngredients.BackColor = Color.FromArgb(52, 152, 219)
        Me.btnCheckIngredients.ForeColor = Color.White
        Me.btnCheckIngredients.FlatStyle = FlatStyle.Flat
        Me.btnCheckIngredients.Font = New Font("Segoe UI", 9.75!, FontStyle.Bold)
        Me.btnCheckIngredients.Location = New Point(570, 16)
        Me.btnCheckIngredients.Size = New Size(180, 30)

        Me.AddMenuItemsbtn.Text = "➕ Add Menu Item"
        Me.AddMenuItemsbtn.BackColor = Color.FromArgb(40, 167, 69)
        Me.AddMenuItemsbtn.ForeColor = Color.White
        Me.AddMenuItemsbtn.FlatStyle = FlatStyle.Flat
        Me.AddMenuItemsbtn.Location = New Point(760, 16)
        Me.AddMenuItemsbtn.Size = New Size(154, 30)

        Me.btnRefresh.Text = "🔄 Refresh"
        Me.btnRefresh.BackColor = Color.FromArgb(108, 117, 125)
        Me.btnRefresh.ForeColor = Color.White
        Me.btnRefresh.FlatStyle = FlatStyle.Flat
        Me.btnRefresh.Location = New Point(920, 16)
        Me.btnRefresh.Size = New Size(94, 30)

        Me.btnToggleAvailability.Text = "🔄 Toggle Status"
        Me.btnToggleAvailability.BackColor = Color.FromArgb(255, 193, 7)
        Me.btnToggleAvailability.FlatStyle = FlatStyle.Flat
        Me.btnToggleAvailability.Location = New Point(1020, 16)
        Me.btnToggleAvailability.Size = New Size(146, 30)

        Me.btnExport.Text = "📊 Export CSV"
        Me.btnExport.BackColor = Color.FromArgb(0, 123, 255)
        Me.btnExport.ForeColor = Color.White
        Me.btnExport.FlatStyle = FlatStyle.Flat
        Me.btnExport.Location = New Point(1175, 16)
        Me.btnExport.Size = New Size(111, 30)

        Me.Panel2.Controls.AddRange(New Control() {
            Me.lblSearch,
            Me.txtSearch,
            Me.btnCheckIngredients,
            Me.AddMenuItemsbtn,
            Me.btnRefresh,
            Me.btnToggleAvailability,
            Me.btnExport
        })

        ' ===== Panel3 =====
        Me.Panel3.Dock = DockStyle.Top
        Me.Panel3.Size = New Size(1250, 48)

        Me.lblFilter.Text = "Filter Category:"
        Me.lblFilter.Font = New Font("Segoe UI", 11.0!, FontStyle.Bold)
        Me.lblFilter.Location = New Point(13, 13)

        Me.Category.DropDownStyle = ComboBoxStyle.DropDownList
        Me.Category.Location = New Point(136, 11)
        Me.Category.Size = New Size(258, 25)

        Me.Panel3.Controls.Add(Me.lblFilter)
        Me.Panel3.Controls.Add(Me.Category)

        ' ===== DataGrid =====
        Me.DataGridMenu.Dock = DockStyle.Fill
        Me.DataGridMenu.ReadOnly = True
        Me.DataGridMenu.RowHeadersVisible = False
        Me.DataGridMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        ' ===== Panel4 =====
        Me.Panel4.Dock = DockStyle.Bottom
        Me.Panel4.Size = New Size(1250, 35)

        Me.lblTotalItems.Text = "Total Items: 0"
        Me.lblTotalItems.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)
        Me.lblTotalItems.Location = New Point(13, 9)

        Me.Panel4.Controls.Add(Me.lblTotalItems)

        ' ===== Form =====
        Me.ClientSize = New Size(1250, 609)
        Me.Controls.Add(Me.DataGridMenu)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Menu Items Management - Tabeya"
    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents AddMenuItemsbtn As Button
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnToggleAvailability As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents btnCheckIngredients As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents lblFilter As Label
    Friend WithEvents Category As ComboBox
    Friend WithEvents DataGridMenu As DataGridView
    Friend WithEvents Panel4 As Panel
    Friend WithEvents lblTotalItems As Label
End Class