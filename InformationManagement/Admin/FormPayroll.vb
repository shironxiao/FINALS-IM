Imports MySql.Data.MySqlClient
Imports System.Windows.Forms.DataVisualization.Charting

Public Class FormPayroll
    ' Database connection string
    Private connectionString As String = "Server=localhost;Database=tabeya_system;Uid=root;Pwd=;"

    Private Sub FormPayroll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadPayrollData()
        LoadPayrollChart()
    End Sub

    '===================== PUBLIC REFRESH METHOD ======================
    Public Sub RefreshPayroll()
        LoadPayrollData()
        LoadPayrollChart()
    End Sub
    '==================================================================

    Private Sub LoadPayrollData()
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                ' Get total payroll (sum of all salaries)
                Dim cmdTotalPayroll As New MySqlCommand("SELECT IFNULL(SUM(Salary), 0) FROM employee WHERE EmploymentStatus = 'Active'", conn)
                Dim totalPayroll As Object = cmdTotalPayroll.ExecuteScalar()
                Label4.Text = "₱" & Convert.ToDecimal(totalPayroll).ToString("N2")

                ' Estimated hours = employees * 160
                Dim cmdTotalHours As New MySqlCommand("SELECT COUNT(*) * 160 FROM employee WHERE EmploymentStatus = 'Active'", conn)
                Label6.Text = Convert.ToString(cmdTotalHours.ExecuteScalar())

                ' Active employees count
                Dim cmdActiveEmployees As New MySqlCommand("SELECT COUNT(*) FROM employee WHERE EmploymentStatus = 'Active'", conn)
                Label7.Text = Convert.ToString(cmdActiveEmployees.ExecuteScalar())

            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading payroll data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadPayrollChart()
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                Chart1.Series.Clear()

                Dim series As New Series("Monthly Payroll")
                series.ChartType = SeriesChartType.Column
                series.Color = Color.MediumSlateBlue

                Dim cmdPayrollBreakdown As New MySqlCommand("SELECT CONCAT(FirstName, ' ', LastName) AS FullName, Salary 
                                                             FROM employee 
                                                             WHERE EmploymentStatus = 'Active'
                                                             ORDER BY EmployeeID", conn)

                Using reader As MySqlDataReader = cmdPayrollBreakdown.ExecuteReader()
                    While reader.Read()
                        Dim salary As Decimal = If(reader.IsDBNull(reader.GetOrdinal("Salary")), 0D,
                                                   Convert.ToDecimal(reader("Salary")))
                        series.Points.AddXY(reader("FullName").ToString(), salary)
                    End While
                End Using

                If series.Points.Count = 0 Then
                    series.Points.AddXY("No Data", 0)
                End If

                Chart1.Series.Add(series)

                Chart1.ChartAreas(0).AxisX.MajorGrid.Enabled = False
                Chart1.ChartAreas(0).AxisY.MajorGrid.LineColor = Color.LightGray
                Chart1.Legends(0).Enabled = False

            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading chart data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                Dim cmdExport As New MySqlCommand("SELECT EmployeeID, FirstName, LastName, Position, Salary, HireDate, EmploymentType 
                                                   FROM employee 
                                                   WHERE EmploymentStatus = 'Active'", conn)

                Dim dt As New DataTable()
                Dim adapter As New MySqlDataAdapter(cmdExport)
                adapter.Fill(dt)

                Dim saveDialog As New SaveFileDialog()
                saveDialog.Filter = "CSV Files (*.csv)|*.csv"
                saveDialog.FileName = "Payroll_Report_" & DateTime.Now.ToString("yyyyMMdd") & ".csv"

                If saveDialog.ShowDialog() = DialogResult.OK Then
                    Dim csv As New System.Text.StringBuilder()

                    csv.AppendLine(String.Join(",", dt.Columns.Cast(Of DataColumn).Select(Function(c) c.ColumnName)))

                    For Each row As DataRow In dt.Rows
                        Dim fields = row.ItemArray.Select(Function(field) $"""{field.ToString().Replace("""", """""")}""")
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
End Class