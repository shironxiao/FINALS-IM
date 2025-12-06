Imports MySqlConnector

Public Class Payroll
    Private Sub Payroll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupDataGridView()
        LoadPayroll()
    End Sub

    Private Sub SetupDataGridView()
        Try
            ' Clear any existing columns
            DataGridView1.Columns.Clear()
            DataGridView1.Rows.Clear()

            ' Configure DataGridView
            DataGridView1.AutoGenerateColumns = False
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.AllowUserToDeleteRows = False
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.MultiSelect = False
            DataGridView1.ReadOnly = False

            ' Create columns
            ' Employee Name Column
            Dim colEmployee As New DataGridViewTextBoxColumn()
            colEmployee.Name = "Employee"
            colEmployee.HeaderText = "Employee Name"
            colEmployee.Width = 150
            colEmployee.ReadOnly = True
            DataGridView1.Columns.Add(colEmployee)

            ' Position Column
            Dim colPosition As New DataGridViewTextBoxColumn()
            colPosition.Name = "Position"
            colPosition.HeaderText = "Position"
            colPosition.Width = 120
            colPosition.ReadOnly = True
            DataGridView1.Columns.Add(colPosition)

            ' Hours Worked Column
            Dim colHours As New DataGridViewTextBoxColumn()
            colHours.Name = "Hours"
            colHours.HeaderText = "Hours Worked"
            colHours.Width = 100
            colHours.ReadOnly = True
            colHours.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DataGridView1.Columns.Add(colHours)

            ' Hourly Rate Column
            Dim colRate As New DataGridViewTextBoxColumn()
            colRate.Name = "HourlyRate"
            colRate.HeaderText = "Hourly Rate"
            colRate.Width = 110
            colRate.ReadOnly = True
            colRate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DataGridView1.Columns.Add(colRate)

            ' Overtime Column
            Dim colOvertime As New DataGridViewTextBoxColumn()
            colOvertime.Name = "Overtime"
            colOvertime.HeaderText = "Overtime"
            colOvertime.Width = 100
            colOvertime.ReadOnly = True
            colOvertime.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DataGridView1.Columns.Add(colOvertime)

            ' Gross Pay Column
            Dim colGross As New DataGridViewTextBoxColumn()
            colGross.Name = "GrossPay"
            colGross.HeaderText = "Gross Pay"
            colGross.Width = 100
            colGross.ReadOnly = True
            colGross.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DataGridView1.Columns.Add(colGross)

            ' Net Pay Column
            Dim colNet As New DataGridViewTextBoxColumn()
            colNet.Name = "NetPay"
            colNet.HeaderText = "Net Pay"
            colNet.Width = 100
            colNet.ReadOnly = True
            colNet.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DataGridView1.Columns.Add(colNet)

            ' Status Column
            Dim colStatus As New DataGridViewTextBoxColumn()
            colStatus.Name = "Status"
            colStatus.HeaderText = "Status"
            colStatus.Width = 80
            colStatus.ReadOnly = True
            colStatus.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns.Add(colStatus)

            ' Actions Button Column
            Dim colActions As New DataGridViewButtonColumn()
            colActions.Name = "Actions"
            colActions.HeaderText = "Actions"
            colActions.Text = "Edit"
            colActions.UseColumnTextForButtonValue = True
            colActions.Width = 80
            DataGridView1.Columns.Add(colActions)

        Catch ex As Exception
            MessageBox.Show("Error setting up DataGridView: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub LoadPayroll()
        Try
            openConn()

            ' Check if new columns exist in database
            Dim checkQuery As String = "SHOW COLUMNS FROM payroll LIKE 'HoursWorked'"
            Dim checkCmd As New MySqlCommand(checkQuery, conn)
            Dim result = checkCmd.ExecuteScalar()
            Dim hasNewColumns As Boolean = (result IsNot Nothing)

            ' Build query based on schema
            Dim query As String = ""
            If hasNewColumns Then
                query = "SELECT p.PayrollID, CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName, e.Position, " &
                        "IFNULL(p.HoursWorked, 0) as HoursWorked, IFNULL(p.HourlyRate, 0) as HourlyRate, " &
                        "p.BasicSalary, IFNULL(p.Overtime, 0) as Overtime, p.NetPay, p.Status " &
                        "FROM payroll p " &
                        "JOIN employee e ON p.EmployeeID = e.EmployeeID " &
                        "ORDER BY p.CreatedDate DESC"
            Else
                query = "SELECT p.PayrollID, CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName, e.Position, " &
                        "p.BasicSalary, IFNULL(p.Overtime, 0) as Overtime, p.NetPay, p.Status " &
                        "FROM payroll p " &
                        "JOIN employee e ON p.EmployeeID = e.EmployeeID " &
                        "ORDER BY p.CreatedDate DESC"
            End If

            Dim cmd As New MySqlCommand(query, conn)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            adapter.Fill(dt)

            ' Clear existing rows
            DataGridView1.Rows.Clear()

            ' Initialize totals
            Dim totalGross As Decimal = 0
            Dim totalNet As Decimal = 0
            Dim empCount As Integer = dt.Rows.Count
            Dim sumHours As Decimal = 0

            ' Populate DataGridView
            For Each row As DataRow In dt.Rows
                Dim hours As Decimal = 0
                Dim rate As Decimal = 0
                Dim overtime As Decimal = 0
                Dim basicSalary As Decimal = 0
                Dim netPay As Decimal = 0

                ' Get values from database
                If hasNewColumns Then
                    hours = If(IsDBNull(row("HoursWorked")), 0D, Convert.ToDecimal(row("HoursWorked")))
                    rate = If(IsDBNull(row("HourlyRate")), 0D, Convert.ToDecimal(row("HourlyRate")))
                End If

                basicSalary = Convert.ToDecimal(row("BasicSalary"))
                overtime = If(IsDBNull(row("Overtime")), 0D, Convert.ToDecimal(row("Overtime")))
                netPay = Convert.ToDecimal(row("NetPay"))

                ' Calculate gross pay
                Dim gross As Decimal = basicSalary + overtime

                ' Add row to DataGridView
                Dim rowIndex As Integer = DataGridView1.Rows.Add(
                    row("EmployeeName").ToString(),
                    row("Position").ToString(),
                    If(hasNewColumns, hours.ToString("F2"), "N/A"),
                    If(hasNewColumns, rate.ToString("C2"), "N/A"),
                    overtime.ToString("C2"),
                    gross.ToString("C2"),
                    netPay.ToString("C2"),
                    row("Status").ToString()
                )

                ' Store PayrollID in the row's Tag property
                DataGridView1.Rows(rowIndex).Tag = row("PayrollID")

                ' Update totals
                totalGross += gross
                totalNet += netPay
                If hasNewColumns Then
                    sumHours += hours
                End If
            Next

            ' Update summary labels if they exist
            Try
                If Me.Controls.Contains(lblTotalGrossPay) Then
                    lblTotalGrossPay.Text = totalGross.ToString("C2")
                End If

                If Me.Controls.Contains(lblTotalNetPay) Then
                    lblTotalNetPay.Text = totalNet.ToString("C2")
                End If

                If Me.Controls.Contains(TotalHours) Then
                    TotalHours.Text = If(hasNewColumns, sumHours.ToString("F2") & " hrs", "N/A")
                End If

                If Me.Controls.Contains(E) Then
                    E.Text = empCount.ToString()
                End If
            Catch
                ' Labels don't exist, skip updating them
            End Try

        Catch ex As Exception
            MessageBox.Show("Error loading payroll data: " & ex.Message & vbCrLf & vbCrLf &
                          "Stack Trace: " & ex.StackTrace, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        ' Check if click is on a valid row and on the Actions column
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            If DataGridView1.Columns(e.ColumnIndex).Name = "Actions" Then
                Try
                    Dim selectedRow As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

                    ' Get PayrollID from Tag
                    If selectedRow.Tag Is Nothing Then
                        MessageBox.Show("Unable to identify payroll record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    Dim payrollID As Integer = Convert.ToInt32(selectedRow.Tag)

                    ' Get Status
                    Dim statusCell = selectedRow.Cells("Status")
                    If statusCell.Value Is Nothing Then
                        MessageBox.Show("Unable to determine payroll status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    Dim status As String = statusCell.Value.ToString().Trim().ToLower()

                    ' Check if already paid
                    If status = "paid" Then
                        MessageBox.Show("Cannot edit a payroll record that has already been paid.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If

                    ' TODO: Open edit form
                    ' For now, show info message
                    MessageBox.Show("Edit Payroll Record" & vbCrLf & vbCrLf &
                                  "PayrollID: " & payrollID & vbCrLf &
                                  "Employee: " & selectedRow.Cells("Employee").Value.ToString() & vbCrLf &
                                  "Status: " & status & vbCrLf & vbCrLf &
                                  "Edit form will open here.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Uncomment when edit form is ready:
                    ' Dim editForm As New FormEditPayroll(payrollID)
                    ' editForm.ShowDialog()
                    ' LoadPayroll() ' Refresh after editing

                Catch ex As Exception
                    MessageBox.Show("Error processing edit action: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        ' Allow double-click as alternative to Edit button
        If e.RowIndex >= 0 Then
            Try
                Dim selectedRow As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

                If selectedRow.Tag Is Nothing Then
                    MessageBox.Show("Unable to identify payroll record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                Dim payrollID As Integer = Convert.ToInt32(selectedRow.Tag)
                Dim statusCell = selectedRow.Cells("Status")

                If statusCell.Value Is Nothing Then
                    Return
                End If

                Dim status As String = statusCell.Value.ToString().Trim().ToLower()

                If status = "paid" Then
                    MessageBox.Show("Cannot edit a payroll record that has already been paid.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' TODO: Open edit form
                MessageBox.Show("Double-click Edit - PayrollID: " & payrollID, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Public Sub UpdatePayrollStatus(payrollID As Integer, newStatus As String)
        Try
            openConn()

            Dim query As String = "UPDATE payroll SET Status = @status, ProcessedDate = NOW() WHERE PayrollID = @id"
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@status", newStatus)
            cmd.Parameters.AddWithValue("@id", payrollID)

            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

            If rowsAffected > 0 Then
                MessageBox.Show("Payroll status updated to '" & newStatus & "' successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadPayroll() ' Refresh the grid
            Else
                MessageBox.Show("No records were updated. Please verify the PayrollID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("Error updating payroll status: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub AddNewPayrollRecordbtn_Click(sender As Object, e As EventArgs) Handles AddNewPayrollRecordbtn.Click
        Try
            Dim form As New FormAddNewPayrollRecord()
            Dim result As DialogResult = form.ShowDialog()

            ' Refresh the grid after closing the form (whether saved or cancelled)
            If result = DialogResult.OK OrElse result = DialogResult.Cancel Then
                LoadPayroll()
            End If

        Catch ex As Exception
            MessageBox.Show("Error opening Add Payroll form: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class