Imports MySql.Data.MySqlClient
Imports System.Windows.Forms.DataVisualization.Charting

Public Class FormPayroll
    ' Database connection string
    Private connectionString As String = "Server=localhost;Database=tabeya_system;Uid=root;Pwd=;"

    Private Sub FormPayroll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadPayrollData()
        LoadPayrollChart()
    End Sub

    Private Sub LoadPayrollData()
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                ' Get total payroll for all active employees
                Dim cmdTotalPayroll As New MySqlCommand("SELECT IFNULL(SUM(Salary), 0) FROM employee WHERE EmploymentStatus = 'Active'", conn)
                Dim totalPayroll As Object = cmdTotalPayroll.ExecuteScalar()
                If totalPayroll IsNot Nothing AndAlso Not IsDBNull(totalPayroll) Then
                    Label4.Text = "₱" & Convert.ToDecimal(totalPayroll).ToString("N2")
                Else
                    Label4.Text = "₱0.00"
                End If

                ' Get total hours (estimated: employees * 160 hours/month)
                Dim cmdTotalHours As New MySqlCommand("SELECT COUNT(*) * 160 FROM employee WHERE EmploymentStatus = 'Active'", conn)
                Dim totalHours As Object = cmdTotalHours.ExecuteScalar()
                If totalHours IsNot Nothing AndAlso Not IsDBNull(totalHours) Then
                    Label6.Text = totalHours.ToString()
                Else
                    Label6.Text = "0"
                End If

                ' Get active employees count
                Dim cmdActiveEmployees As New MySqlCommand("SELECT COUNT(*) FROM employee WHERE EmploymentStatus = 'Active'", conn)
                Dim activeEmployees As Object = cmdActiveEmployees.ExecuteScalar()
                If activeEmployees IsNot Nothing AndAlso Not IsDBNull(activeEmployees) Then
                    Label7.Text = activeEmployees.ToString()
                Else
                    Label7.Text = "0"
                End If

            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading payroll data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadPayrollChart()
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                ' Clear existing series
                Chart1.Series.Clear()

                ' Create new series
                Dim series As New Series("Monthly Payroll")
                series.ChartType = SeriesChartType.Column
                series.Color = Color.MediumSlateBlue

                ' Load actual salary values per employee for the chart
                Dim cmdPayrollBreakdown As New MySqlCommand("SELECT CONCAT(FirstName, ' ', LastName) AS FullName, Salary FROM employee WHERE EmploymentStatus = 'Active' ORDER BY EmployeeID", conn)

                Using reader As MySqlDataReader = cmdPayrollBreakdown.ExecuteReader()
                    While reader.Read()
                        Dim salaryValue As Decimal = 0D
                        If Not reader.IsDBNull(reader.GetOrdinal("Salary")) Then
                            salaryValue = Convert.ToDecimal(reader("Salary"))
                        End If
                        series.Points.AddXY(reader("FullName").ToString(), salaryValue)
                    End While
                End Using

                If series.Points.Count = 0 Then
                    series.Points.AddXY("No Data", 0)
                End If

                Chart1.Series.Add(series)

                ' Configure chart appearance
                Chart1.ChartAreas(0).AxisX.MajorGrid.Enabled = False
                Chart1.ChartAreas(0).AxisY.MajorGrid.LineColor = Color.LightGray
                Chart1.Legends(0).Enabled = False

            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading chart data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Export payroll data
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                Dim cmdExport As New MySqlCommand("SELECT EmployeeID, FirstName, LastName, Position, Salary, HireDate, EmploymentType FROM employee WHERE EmploymentStatus = 'Active'", conn)

                Dim dt As New DataTable()
                Dim adapter As New MySqlDataAdapter(cmdExport)
                adapter.Fill(dt)

                ' Save to CSV
                Dim saveDialog As New SaveFileDialog()
                saveDialog.Filter = "CSV Files (*.csv)|*.csv"
                saveDialog.FileName = "Payroll_Report_" & DateTime.Now.ToString("yyyyMMdd") & ".csv"

                If saveDialog.ShowDialog() = DialogResult.OK Then
                    Dim csv As New System.Text.StringBuilder()

                    ' Add header
                    Dim headerLine As String = String.Join(",", dt.Columns.Cast(Of DataColumn)().Select(Function(column) column.ColumnName))
                    csv.AppendLine(headerLine)

                    ' Add rows
                    For Each row As DataRow In dt.Rows
                        Dim fields = row.ItemArray.Select(Function(field) String.Format("""{0}""", field.ToString().Replace("""", """""")))
                        csv.AppendLine(String.Join(",", fields))
                    Next

                    System.IO.File.WriteAllText(saveDialog.FileName, csv.ToString())
                    MessageBox.Show("Payroll report exported successfully!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            End Using
        Catch ex As Exception
            MessageBox.Show("Error exporting data: " & ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click
    End Sub
End Class