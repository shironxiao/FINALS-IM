<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormEditMenu
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.cmbMealTime = New System.Windows.Forms.ComboBox()
        Me.lblMealTime = New System.Windows.Forms.Label()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.PrepTime = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.OrderCount = New System.Windows.Forms.TextBox()
        Me.ServingSize = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.ProductCode = New System.Windows.Forms.TextBox()
        Me.Description = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Product = New System.Windows.Forms.Label()
        Me.txtProductName = New System.Windows.Forms.TextBox()
        Me.ProductID = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.numericPrice = New System.Windows.Forms.NumericUpDown()
        Me.cmbCategory = New System.Windows.Forms.ComboBox()
        Me.Availability = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnUpdateItem = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.numericPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Panel1.Controls.Add(Me.btnClose)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1149, 64)
        Me.Panel1.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnClose.ForeColor = System.Drawing.Color.White
        Me.btnClose.Location = New System.Drawing.Point(1097, 11)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(46, 43)
        Me.btnClose.TabIndex = 25
        Me.btnClose.Text = "✕"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(17, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(214, 37)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Edit Menu Item"
        '
        'Panel2
        '
        Me.Panel2.AutoScroll = True
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.PictureBox1)
        Me.Panel2.Controls.Add(Me.cmbMealTime)
        Me.Panel2.Controls.Add(Me.lblMealTime)
        Me.Panel2.Controls.Add(Me.DateTimePicker1)
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.PrepTime)
        Me.Panel2.Controls.Add(Me.Label20)
        Me.Panel2.Controls.Add(Me.OrderCount)
        Me.Panel2.Controls.Add(Me.ServingSize)
        Me.Panel2.Controls.Add(Me.Label13)
        Me.Panel2.Controls.Add(Me.ProductCode)
        Me.Panel2.Controls.Add(Me.Description)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.Product)
        Me.Panel2.Controls.Add(Me.txtProductName)
        Me.Panel2.Controls.Add(Me.ProductID)
        Me.Panel2.Controls.Add(Me.Label10)
        Me.Panel2.Controls.Add(Me.numericPrice)
        Me.Panel2.Controls.Add(Me.cmbCategory)
        Me.Panel2.Controls.Add(Me.Availability)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 64)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Padding = New System.Windows.Forms.Padding(23, 21, 23, 21)
        Me.Panel2.Size = New System.Drawing.Size(1149, 610)
        Me.Panel2.TabIndex = 1
        '
        'cmbMealTime
        '
        Me.cmbMealTime.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cmbMealTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMealTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmbMealTime.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cmbMealTime.FormattingEnabled = True
        Me.cmbMealTime.Location = New System.Drawing.Point(587, 516)
        Me.cmbMealTime.Name = "cmbMealTime"
        Me.cmbMealTime.Size = New System.Drawing.Size(493, 31)
        Me.cmbMealTime.TabIndex = 58
        '
        'lblMealTime
        '
        Me.lblMealTime.AutoSize = True
        Me.lblMealTime.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblMealTime.Location = New System.Drawing.Point(587, 489)
        Me.lblMealTime.Name = "lblMealTime"
        Me.lblMealTime.Size = New System.Drawing.Size(90, 23)
        Me.lblMealTime.TabIndex = 57
        Me.lblMealTime.Text = "Meal Time"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Enabled = False
        Me.DateTimePicker1.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.DateTimePicker1.Location = New System.Drawing.Point(29, 516)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(308, 30)
        Me.DateTimePicker1.TabIndex = 55
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label9.Location = New System.Drawing.Point(29, 489)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(155, 23)
        Me.Label9.TabIndex = 56
        Me.Label9.Text = "Date Added (Auto)"
        '
        'PrepTime
        '
        Me.PrepTime.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PrepTime.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.PrepTime.Location = New System.Drawing.Point(587, 445)
        Me.PrepTime.Name = "PrepTime"
        Me.PrepTime.Size = New System.Drawing.Size(493, 30)
        Me.PrepTime.TabIndex = 9
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label20.Location = New System.Drawing.Point(587, 415)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(211, 23)
        Me.Label20.TabIndex = 48
        Me.Label20.Text = "Prep Time (e.g., ""15 mins"")"
        '
        'OrderCount
        '
        Me.OrderCount.BackColor = System.Drawing.Color.LightGray
        Me.OrderCount.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.OrderCount.ForeColor = System.Drawing.Color.Gray
        Me.OrderCount.Location = New System.Drawing.Point(587, 91)
        Me.OrderCount.Name = "OrderCount"
        Me.OrderCount.ReadOnly = True
        Me.OrderCount.Size = New System.Drawing.Size(493, 30)
        Me.OrderCount.TabIndex = 47
        '
        'ServingSize
        '
        Me.ServingSize.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ServingSize.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.ServingSize.Location = New System.Drawing.Point(29, 417)
        Me.ServingSize.Name = "ServingSize"
        Me.ServingSize.Size = New System.Drawing.Size(493, 30)
        Me.ServingSize.TabIndex = 5
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label13.Location = New System.Drawing.Point(587, 61)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(160, 23)
        Me.Label13.TabIndex = 45
        Me.Label13.Text = "Order Count (Auto)"
        '
        'ProductCode
        '
        Me.ProductCode.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ProductCode.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.ProductCode.Location = New System.Drawing.Point(29, 590)
        Me.ProductCode.Name = "ProductCode"
        Me.ProductCode.Size = New System.Drawing.Size(493, 30)
        Me.ProductCode.TabIndex = 6
        '
        'Description
        '
        Me.Description.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Description.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Description.Location = New System.Drawing.Point(29, 260)
        Me.Description.Multiline = True
        Me.Description.Name = "Description"
        Me.Description.Size = New System.Drawing.Size(493, 106)
        Me.Description.TabIndex = 2
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label12.Location = New System.Drawing.Point(29, 389)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(253, 23)
        Me.Label12.TabIndex = 36
        Me.Label12.Text = "Serving Size (e.g., ""Good for 5"")"
        '
        'Product
        '
        Me.Product.AutoSize = True
        Me.Product.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Product.Location = New System.Drawing.Point(29, 560)
        Me.Product.Name = "Product"
        Me.Product.Size = New System.Drawing.Size(198, 23)
        Me.Product.TabIndex = 40
        Me.Product.Text = "Product Code (Optional)"
        '
        'txtProductName
        '
        Me.txtProductName.BackColor = System.Drawing.Color.WhiteSmoke
        Me.txtProductName.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtProductName.Location = New System.Drawing.Point(29, 166)
        Me.txtProductName.Name = "txtProductName"
        Me.txtProductName.Size = New System.Drawing.Size(493, 30)
        Me.txtProductName.TabIndex = 1
        '
        'ProductID
        '
        Me.ProductID.BackColor = System.Drawing.Color.LightGray
        Me.ProductID.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.ProductID.ForeColor = System.Drawing.Color.Gray
        Me.ProductID.Location = New System.Drawing.Point(29, 91)
        Me.ProductID.Name = "ProductID"
        Me.ProductID.ReadOnly = True
        Me.ProductID.Size = New System.Drawing.Size(493, 30)
        Me.ProductID.TabIndex = 30
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label10.Location = New System.Drawing.Point(29, 230)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(96, 23)
        Me.Label10.TabIndex = 32
        Me.Label10.Text = "Description"
        '
        'numericPrice
        '
        Me.numericPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.numericPrice.DecimalPlaces = 2
        Me.numericPrice.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.numericPrice.Location = New System.Drawing.Point(587, 166)
        Me.numericPrice.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        Me.numericPrice.Name = "numericPrice"
        Me.numericPrice.Size = New System.Drawing.Size(494, 32)
        Me.numericPrice.TabIndex = 3
        '
        'cmbCategory
        '
        Me.cmbCategory.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmbCategory.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(587, 292)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(493, 31)
        Me.cmbCategory.TabIndex = 7
        '
        'Availability
        '
        Me.Availability.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Availability.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Availability.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Availability.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Availability.FormattingEnabled = True
        Me.Availability.Location = New System.Drawing.Point(587, 229)
        Me.Availability.Name = "Availability"
        Me.Availability.Size = New System.Drawing.Size(493, 31)
        Me.Availability.TabIndex = 4
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label8.Location = New System.Drawing.Point(587, 203)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(93, 23)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Availability"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label7.Location = New System.Drawing.Point(587, 331)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(269, 23)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "Image URL or File Path (Optional)"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label6.Location = New System.Drawing.Point(587, 265)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(81, 23)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Category"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label5.Location = New System.Drawing.Point(587, 137)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(47, 23)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Price"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label4.Location = New System.Drawing.Point(29, 137)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(121, 23)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Product Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(29, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(92, 23)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Product ID"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel3.Controls.Add(Me.btnUpdateItem)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 674)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Padding = New System.Windows.Forms.Padding(23, 21, 23, 21)
        Me.Panel3.Size = New System.Drawing.Size(1149, 85)
        Me.Panel3.TabIndex = 2
        '
        'btnUpdateItem
        '
        Me.btnUpdateItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpdateItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnUpdateItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdateItem.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnUpdateItem.ForeColor = System.Drawing.Color.White
        Me.btnUpdateItem.Location = New System.Drawing.Point(587, 21)
        Me.btnUpdateItem.Name = "btnUpdateItem"
        Me.btnUpdateItem.Size = New System.Drawing.Size(537, 48)
        Me.btnUpdateItem.TabIndex = 10
        Me.btnUpdateItem.Text = "💾 Update Menu Item"
        Me.btnUpdateItem.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PictureBox1.Location = New System.Drawing.Point(587, 357)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(493, 40)
        Me.PictureBox1.TabIndex = 59
        Me.PictureBox1.TabStop = False
        '
        'FormEditMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1149, 759)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormEditMenu"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Menu Item - Tabeya"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.numericPrice, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Availability As ComboBox
    Friend WithEvents cmbCategory As ComboBox
    Friend WithEvents numericPrice As NumericUpDown
    Friend WithEvents ProductID As TextBox
    Friend WithEvents txtProductName As TextBox
    Friend WithEvents Description As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents ServingSize As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents OrderCount As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents ProductCode As TextBox
    Friend WithEvents Product As Label
    Friend WithEvents PrepTime As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents Label9 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents btnUpdateItem As Button
    Friend WithEvents cmbMealTime As ComboBox
    Friend WithEvents lblMealTime As Label
    Friend WithEvents PictureBox1 As PictureBox
End Class