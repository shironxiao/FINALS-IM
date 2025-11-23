<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Feedback
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnViewDetails = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnViewAll = New System.Windows.Forms.Button()
        Me.btnViewRejected = New System.Windows.Forms.Button()
        Me.btnViewApproved = New System.Windows.Forms.Button()
        Me.btnViewPending = New System.Windows.Forms.Button()
        Me.lblFilter = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lblTotalReviews = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.Panel1.Controls.Add(Me.lblTitle)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1200, 60)
        Me.Panel1.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(12, 12)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(379, 32)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Customer Review Management"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel2.Controls.Add(Me.btnExport)
        Me.Panel2.Controls.Add(Me.btnDelete)
        Me.Panel2.Controls.Add(Me.btnViewDetails)
        Me.Panel2.Controls.Add(Me.btnRefresh)
        Me.Panel2.Controls.Add(Me.txtSearch)
        Me.Panel2.Controls.Add(Me.lblSearch)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 60)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Padding = New System.Windows.Forms.Padding(10)
        Me.Panel2.Size = New System.Drawing.Size(1200, 60)
        Me.Panel2.TabIndex = 1
        '
        'btnExport
        '
        Me.btnExport.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnExport.ForeColor = System.Drawing.Color.White
        Me.btnExport.Location = New System.Drawing.Point(1050, 15)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(120, 30)
        Me.btnExport.TabIndex = 5
        Me.btnExport.Text = "Export to CSV"
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnDelete.ForeColor = System.Drawing.Color.White
        Me.btnDelete.Location = New System.Drawing.Point(940, 15)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 4
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnViewDetails
        '
        Me.btnViewDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnViewDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViewDetails.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnViewDetails.ForeColor = System.Drawing.Color.White
        Me.btnViewDetails.Location = New System.Drawing.Point(810, 15)
        Me.btnViewDetails.Name = "btnViewDetails"
        Me.btnViewDetails.Size = New System.Drawing.Size(120, 30)
        Me.btnViewDetails.TabIndex = 3
        Me.btnViewDetails.Text = "View Details"
        Me.btnViewDetails.UseVisualStyleBackColor = False
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnRefresh.ForeColor = System.Drawing.Color.White
        Me.btnRefresh.Location = New System.Drawing.Point(700, 15)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(100, 30)
        Me.btnRefresh.TabIndex = 2
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'txtSearch
        '
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.txtSearch.Location = New System.Drawing.Point(80, 17)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(600, 25)
        Me.txtSearch.TabIndex = 1
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.lblSearch.Location = New System.Drawing.Point(13, 20)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(61, 19)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "Search:"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.White
        Me.Panel3.Controls.Add(Me.btnViewAll)
        Me.Panel3.Controls.Add(Me.btnViewRejected)
        Me.Panel3.Controls.Add(Me.btnViewApproved)
        Me.Panel3.Controls.Add(Me.btnViewPending)
        Me.Panel3.Controls.Add(Me.lblFilter)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 120)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Padding = New System.Windows.Forms.Padding(10)
        Me.Panel3.Size = New System.Drawing.Size(1200, 50)
        Me.Panel3.TabIndex = 2
        '
        'btnViewAll
        '
        Me.btnViewAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnViewAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViewAll.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnViewAll.ForeColor = System.Drawing.Color.White
        Me.btnViewAll.Location = New System.Drawing.Point(450, 10)
        Me.btnViewAll.Name = "btnViewAll"
        Me.btnViewAll.Size = New System.Drawing.Size(100, 30)
        Me.btnViewAll.TabIndex = 4
        Me.btnViewAll.Text = "All"
        Me.btnViewAll.UseVisualStyleBackColor = False
        '
        'btnViewRejected
        '
        Me.btnViewRejected.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnViewRejected.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViewRejected.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnViewRejected.ForeColor = System.Drawing.Color.White
        Me.btnViewRejected.Location = New System.Drawing.Point(340, 10)
        Me.btnViewRejected.Name = "btnViewRejected"
        Me.btnViewRejected.Size = New System.Drawing.Size(100, 30)
        Me.btnViewRejected.TabIndex = 3
        Me.btnViewRejected.Text = "Rejected"
        Me.btnViewRejected.UseVisualStyleBackColor = False
        '
        'btnViewApproved
        '
        Me.btnViewApproved.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnViewApproved.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViewApproved.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnViewApproved.ForeColor = System.Drawing.Color.White
        Me.btnViewApproved.Location = New System.Drawing.Point(230, 10)
        Me.btnViewApproved.Name = "btnViewApproved"
        Me.btnViewApproved.Size = New System.Drawing.Size(100, 30)
        Me.btnViewApproved.TabIndex = 2
        Me.btnViewApproved.Text = "Approved"
        Me.btnViewApproved.UseVisualStyleBackColor = False
        '
        'btnViewPending
        '
        Me.btnViewPending.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(193, Byte), Integer), CType(CType(7, Byte), Integer))
        Me.btnViewPending.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViewPending.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnViewPending.ForeColor = System.Drawing.Color.Black
        Me.btnViewPending.Location = New System.Drawing.Point(120, 10)
        Me.btnViewPending.Name = "btnViewPending"
        Me.btnViewPending.Size = New System.Drawing.Size(100, 30)
        Me.btnViewPending.TabIndex = 1
        Me.btnViewPending.Text = "Pending"
        Me.btnViewPending.UseVisualStyleBackColor = False
        '
        'lblFilter
        '
        Me.lblFilter.AutoSize = True
        Me.lblFilter.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.lblFilter.Location = New System.Drawing.Point(13, 15)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(90, 19)
        Me.lblFilter.TabIndex = 0
        Me.lblFilter.Text = "Filter Status:"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(0, 170)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowTemplate.Height = 25
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(1200, 480)
        Me.DataGridView1.TabIndex = 3
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel4.Controls.Add(Me.lblTotalReviews)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel4.Location = New System.Drawing.Point(0, 650)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1200, 30)
        Me.Panel4.TabIndex = 4
        '
        'lblTotalReviews
        '
        Me.lblTotalReviews.AutoSize = True
        Me.lblTotalReviews.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.lblTotalReviews.Location = New System.Drawing.Point(12, 7)
        Me.lblTotalReviews.Name = "lblTotalReviews"
        Me.lblTotalReviews.Size = New System.Drawing.Size(95, 15)
        Me.lblTotalReviews.TabIndex = 0
        Me.lblTotalReviews.Text = "Total Reviews: 0"
        '
        'Feedback
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1200, 680)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "Feedback"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Customer Review Management - Tabeya"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnViewDetails As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents lblFilter As Label
    Friend WithEvents btnViewPending As Button
    Friend WithEvents btnViewApproved As Button
    Friend WithEvents btnViewRejected As Button
    Friend WithEvents btnViewAll As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Panel4 As Panel
    Friend WithEvents lblTotalReviews As Label
    Friend WithEvents btnExport As Button
End Class