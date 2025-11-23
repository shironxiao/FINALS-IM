<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditEmployee
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Add = New System.Windows.Forms.Button()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.WorkShift = New InformationManagement.RoundedTextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Salary = New InformationManagement.RoundedTextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.EmergencyContact = New InformationManagement.RoundedTextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Position = New InformationManagement.RoundedTextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.HireDate = New System.Windows.Forms.DateTimePicker()
        Me.MaritalStatus = New InformationManagement.RoundedTextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Address = New InformationManagement.RoundedTextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.Email = New InformationManagement.RoundedTextBox()
        Me.RoundedTextBox1 = New InformationManagement.RoundedTextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.FirstName = New InformationManagement.RoundedTextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ContactNumber = New InformationManagement.RoundedTextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.LastName = New InformationManagement.RoundedTextBox()
        Me.Gender = New InformationManagement.RoundedTextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.EmployeeID = New InformationManagement.RoundedTextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Add
        '
        Me.Add.BackColor = System.Drawing.Color.DarkRed
        Me.Add.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Add.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Add.Location = New System.Drawing.Point(569, 836)
        Me.Add.Name = "Add"
        Me.Add.Size = New System.Drawing.Size(162, 47)
        Me.Add.TabIndex = 118
        Me.Add.Text = "Edit Employee"
        Me.Add.UseVisualStyleBackColor = False
        '
        'Cancel
        '
        Me.Cancel.BackColor = System.Drawing.SystemColors.Highlight
        Me.Cancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cancel.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Cancel.Location = New System.Drawing.Point(445, 836)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(118, 47)
        Me.Cancel.TabIndex = 117
        Me.Cancel.Text = "Cancel"
        Me.Cancel.UseVisualStyleBackColor = False
        '
        'WorkShift
        '
        Me.WorkShift.BackColor = System.Drawing.Color.Transparent
        Me.WorkShift.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.WorkShift.Location = New System.Drawing.Point(383, 773)
        Me.WorkShift.Margin = New System.Windows.Forms.Padding(4)
        Me.WorkShift.MaxLength = 32767
        Me.WorkShift.MinimumSize = New System.Drawing.Size(67, 25)
        Me.WorkShift.Multiline = False
        Me.WorkShift.Name = "WorkShift"
        Me.WorkShift.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.WorkShift.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.WorkShift.Size = New System.Drawing.Size(307, 44)
        Me.WorkShift.TabIndex = 114
        Me.WorkShift.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.WorkShift.TextColor = System.Drawing.Color.Black
        Me.WorkShift.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label16.Location = New System.Drawing.Point(378, 746)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(132, 25)
        Me.Label16.TabIndex = 113
        Me.Label16.Text = "Work Shift  :"
        '
        'Salary
        '
        Me.Salary.BackColor = System.Drawing.Color.Transparent
        Me.Salary.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.Salary.Location = New System.Drawing.Point(28, 773)
        Me.Salary.Margin = New System.Windows.Forms.Padding(4)
        Me.Salary.MaxLength = 32767
        Me.Salary.MinimumSize = New System.Drawing.Size(67, 25)
        Me.Salary.Multiline = False
        Me.Salary.Name = "Salary"
        Me.Salary.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.Salary.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Salary.Size = New System.Drawing.Size(307, 44)
        Me.Salary.TabIndex = 116
        Me.Salary.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.Salary.TextColor = System.Drawing.Color.Black
        Me.Salary.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label17.Location = New System.Drawing.Point(26, 744)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(87, 25)
        Me.Label17.TabIndex = 115
        Me.Label17.Text = "Salary :"
        '
        'EmergencyContact
        '
        Me.EmergencyContact.BackColor = System.Drawing.Color.Transparent
        Me.EmergencyContact.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.EmergencyContact.Location = New System.Drawing.Point(28, 684)
        Me.EmergencyContact.Margin = New System.Windows.Forms.Padding(4)
        Me.EmergencyContact.MaxLength = 32767
        Me.EmergencyContact.MinimumSize = New System.Drawing.Size(67, 25)
        Me.EmergencyContact.Multiline = False
        Me.EmergencyContact.Name = "EmergencyContact"
        Me.EmergencyContact.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.EmergencyContact.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.EmergencyContact.Size = New System.Drawing.Size(307, 44)
        Me.EmergencyContact.TabIndex = 112
        Me.EmergencyContact.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.EmergencyContact.TextColor = System.Drawing.Color.Black
        Me.EmergencyContact.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label18.Location = New System.Drawing.Point(23, 655)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(214, 25)
        Me.Label18.TabIndex = 111
        Me.Label18.Text = "Emergency Contact :"
        '
        'Position
        '
        Me.Position.BackColor = System.Drawing.Color.Transparent
        Me.Position.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.Position.Location = New System.Drawing.Point(28, 588)
        Me.Position.Margin = New System.Windows.Forms.Padding(4)
        Me.Position.MaxLength = 32767
        Me.Position.MinimumSize = New System.Drawing.Size(67, 25)
        Me.Position.Multiline = False
        Me.Position.Name = "Position"
        Me.Position.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.Position.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Position.Size = New System.Drawing.Size(307, 44)
        Me.Position.TabIndex = 110
        Me.Position.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.Position.TextColor = System.Drawing.Color.Black
        Me.Position.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label14.Location = New System.Drawing.Point(26, 559)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(102, 25)
        Me.Label14.TabIndex = 109
        Me.Label14.Text = "Position :"
        '
        'HireDate
        '
        Me.HireDate.Location = New System.Drawing.Point(383, 588)
        Me.HireDate.Name = "HireDate"
        Me.HireDate.Size = New System.Drawing.Size(307, 22)
        Me.HireDate.TabIndex = 108
        '
        'MaritalStatus
        '
        Me.MaritalStatus.BackColor = System.Drawing.Color.Transparent
        Me.MaritalStatus.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.MaritalStatus.Location = New System.Drawing.Point(383, 684)
        Me.MaritalStatus.Margin = New System.Windows.Forms.Padding(4)
        Me.MaritalStatus.MaxLength = 32767
        Me.MaritalStatus.MinimumSize = New System.Drawing.Size(67, 25)
        Me.MaritalStatus.Multiline = False
        Me.MaritalStatus.Name = "MaritalStatus"
        Me.MaritalStatus.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.MaritalStatus.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.MaritalStatus.Size = New System.Drawing.Size(307, 44)
        Me.MaritalStatus.TabIndex = 107
        Me.MaritalStatus.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.MaritalStatus.TextColor = System.Drawing.Color.Black
        Me.MaritalStatus.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label4.Location = New System.Drawing.Point(378, 655)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(158, 25)
        Me.Label4.TabIndex = 106
        Me.Label4.Text = "Marital Status :"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label12.Location = New System.Drawing.Point(378, 559)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(115, 25)
        Me.Label12.TabIndex = 103
        Me.Label12.Text = "Hire Date :"
        '
        'Address
        '
        Me.Address.BackColor = System.Drawing.Color.Transparent
        Me.Address.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.Address.Location = New System.Drawing.Point(28, 494)
        Me.Address.Margin = New System.Windows.Forms.Padding(4)
        Me.Address.MaxLength = 32767
        Me.Address.MinimumSize = New System.Drawing.Size(67, 25)
        Me.Address.Multiline = False
        Me.Address.Name = "Address"
        Me.Address.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.Address.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Address.Size = New System.Drawing.Size(662, 44)
        Me.Address.TabIndex = 105
        Me.Address.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.Address.TextColor = System.Drawing.Color.Black
        Me.Address.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label13.Location = New System.Drawing.Point(23, 465)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(105, 25)
        Me.Label13.TabIndex = 104
        Me.Label13.Text = "Address :"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Location = New System.Drawing.Point(383, 305)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(307, 22)
        Me.DateTimePicker1.TabIndex = 102
        '
        'Email
        '
        Me.Email.BackColor = System.Drawing.Color.Transparent
        Me.Email.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.Email.Location = New System.Drawing.Point(383, 394)
        Me.Email.Margin = New System.Windows.Forms.Padding(4)
        Me.Email.MaxLength = 32767
        Me.Email.MinimumSize = New System.Drawing.Size(67, 25)
        Me.Email.Multiline = False
        Me.Email.Name = "Email"
        Me.Email.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.Email.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Email.Size = New System.Drawing.Size(307, 44)
        Me.Email.TabIndex = 101
        Me.Email.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.Email.TextColor = System.Drawing.Color.Black
        Me.Email.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'RoundedTextBox1
        '
        Me.RoundedTextBox1.BackColor = System.Drawing.Color.Transparent
        Me.RoundedTextBox1.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.RoundedTextBox1.Location = New System.Drawing.Point(383, 218)
        Me.RoundedTextBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.RoundedTextBox1.MaxLength = 32767
        Me.RoundedTextBox1.MinimumSize = New System.Drawing.Size(67, 25)
        Me.RoundedTextBox1.Multiline = False
        Me.RoundedTextBox1.Name = "RoundedTextBox1"
        Me.RoundedTextBox1.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.RoundedTextBox1.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.RoundedTextBox1.Size = New System.Drawing.Size(307, 44)
        Me.RoundedTextBox1.TabIndex = 94
        Me.RoundedTextBox1.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.RoundedTextBox1.TextColor = System.Drawing.Color.Black
        Me.RoundedTextBox1.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label8.Location = New System.Drawing.Point(378, 365)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(78, 25)
        Me.Label8.TabIndex = 100
        Me.Label8.Text = "Email :"
        '
        'FirstName
        '
        Me.FirstName.BackColor = System.Drawing.Color.Transparent
        Me.FirstName.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.FirstName.Location = New System.Drawing.Point(383, 129)
        Me.FirstName.Margin = New System.Windows.Forms.Padding(4)
        Me.FirstName.MaxLength = 32767
        Me.FirstName.MinimumSize = New System.Drawing.Size(67, 25)
        Me.FirstName.Multiline = False
        Me.FirstName.Name = "FirstName"
        Me.FirstName.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.FirstName.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.FirstName.Size = New System.Drawing.Size(307, 44)
        Me.FirstName.TabIndex = 90
        Me.FirstName.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.FirstName.TextColor = System.Drawing.Color.Black
        Me.FirstName.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label9.Location = New System.Drawing.Point(378, 276)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(149, 25)
        Me.Label9.TabIndex = 97
        Me.Label9.Text = "Date Of Birth :"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label6.Location = New System.Drawing.Point(378, 189)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(151, 25)
        Me.Label6.TabIndex = 93
        Me.Label6.Text = "Middle Name :"
        '
        'ContactNumber
        '
        Me.ContactNumber.BackColor = System.Drawing.Color.Transparent
        Me.ContactNumber.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.ContactNumber.Location = New System.Drawing.Point(28, 394)
        Me.ContactNumber.Margin = New System.Windows.Forms.Padding(4)
        Me.ContactNumber.MaxLength = 32767
        Me.ContactNumber.MinimumSize = New System.Drawing.Size(67, 25)
        Me.ContactNumber.Multiline = False
        Me.ContactNumber.Name = "ContactNumber"
        Me.ContactNumber.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.ContactNumber.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.ContactNumber.Size = New System.Drawing.Size(307, 44)
        Me.ContactNumber.TabIndex = 99
        Me.ContactNumber.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.ContactNumber.TextColor = System.Drawing.Color.Black
        Me.ContactNumber.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label5.Location = New System.Drawing.Point(378, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(129, 25)
        Me.Label5.TabIndex = 89
        Me.Label5.Text = "First Name :"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label10.Location = New System.Drawing.Point(23, 365)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(175, 25)
        Me.Label10.TabIndex = 98
        Me.Label10.Text = "ContactNumber :"
        '
        'LastName
        '
        Me.LastName.BackColor = System.Drawing.Color.Transparent
        Me.LastName.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.LastName.Location = New System.Drawing.Point(28, 218)
        Me.LastName.Margin = New System.Windows.Forms.Padding(4)
        Me.LastName.MaxLength = 32767
        Me.LastName.MinimumSize = New System.Drawing.Size(67, 25)
        Me.LastName.Multiline = False
        Me.LastName.Name = "LastName"
        Me.LastName.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.LastName.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.LastName.Size = New System.Drawing.Size(307, 44)
        Me.LastName.TabIndex = 92
        Me.LastName.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.LastName.TextColor = System.Drawing.Color.Black
        Me.LastName.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'Gender
        '
        Me.Gender.BackColor = System.Drawing.Color.Transparent
        Me.Gender.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.Gender.Location = New System.Drawing.Point(28, 305)
        Me.Gender.Margin = New System.Windows.Forms.Padding(4)
        Me.Gender.MaxLength = 32767
        Me.Gender.MinimumSize = New System.Drawing.Size(67, 25)
        Me.Gender.Multiline = False
        Me.Gender.Name = "Gender"
        Me.Gender.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.Gender.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Gender.Size = New System.Drawing.Size(307, 44)
        Me.Gender.TabIndex = 96
        Me.Gender.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.Gender.TextColor = System.Drawing.Color.Black
        Me.Gender.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label7.Location = New System.Drawing.Point(23, 189)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(128, 25)
        Me.Label7.TabIndex = 91
        Me.Label7.Text = "Last Name :"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label11.Location = New System.Drawing.Point(23, 276)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(96, 25)
        Me.Label11.TabIndex = 95
        Me.Label11.Text = "Gender :"
        '
        'EmployeeID
        '
        Me.EmployeeID.BackColor = System.Drawing.Color.Transparent
        Me.EmployeeID.FocusBorderColor = System.Drawing.Color.DarkGray
        Me.EmployeeID.Location = New System.Drawing.Point(28, 129)
        Me.EmployeeID.Margin = New System.Windows.Forms.Padding(4)
        Me.EmployeeID.MaxLength = 32767
        Me.EmployeeID.MinimumSize = New System.Drawing.Size(67, 25)
        Me.EmployeeID.Multiline = False
        Me.EmployeeID.Name = "EmployeeID"
        Me.EmployeeID.NormalBorderColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.EmployeeID.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.EmployeeID.Size = New System.Drawing.Size(307, 44)
        Me.EmployeeID.TabIndex = 88
        Me.EmployeeID.TextBoxBackColor = System.Drawing.Color.WhiteSmoke
        Me.EmployeeID.TextColor = System.Drawing.Color.Black
        Me.EmployeeID.TextFont = New System.Drawing.Font("Segoe UI", 10.0!)
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label3.Location = New System.Drawing.Point(23, 100)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(147, 25)
        Me.Label3.TabIndex = 87
        Me.Label3.Text = "Employee ID :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label2.Location = New System.Drawing.Point(25, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(333, 18)
        Me.Label2.TabIndex = 86
        Me.Label2.Text = "Edit the details of the new employee below."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label1.Location = New System.Drawing.Point(22, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(211, 32)
        Me.Label1.TabIndex = 85
        Me.Label1.Text = "Edit Employee"
        '
        'EditEmployee
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(745, 886)
        Me.Controls.Add(Me.Add)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.WorkShift)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Salary)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.EmergencyContact)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Position)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.HireDate)
        Me.Controls.Add(Me.MaritalStatus)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Address)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Controls.Add(Me.Email)
        Me.Controls.Add(Me.RoundedTextBox1)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.FirstName)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.ContactNumber)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.LastName)
        Me.Controls.Add(Me.Gender)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.EmployeeID)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "EditEmployee"
        Me.Text = "EditEmployee"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Add As Button
    Friend WithEvents Cancel As Button
    Friend WithEvents WorkShift As RoundedTextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents Salary As RoundedTextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents EmergencyContact As RoundedTextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents Position As RoundedTextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents HireDate As DateTimePicker
    Friend WithEvents MaritalStatus As RoundedTextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Address As RoundedTextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents Email As RoundedTextBox
    Friend WithEvents RoundedTextBox1 As RoundedTextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents FirstName As RoundedTextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents ContactNumber As RoundedTextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents LastName As RoundedTextBox
    Friend WithEvents Gender As RoundedTextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents EmployeeID As RoundedTextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
End Class
