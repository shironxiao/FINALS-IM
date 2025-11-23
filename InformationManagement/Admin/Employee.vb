Imports MySqlConnector
Imports System.Data

Public Class Employee

    Private Sub Employee_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadEmployees()
    End Sub

    '====================================
    ' MAIN LOADER
    '====================================
    Private Sub LoadEmployees(Optional condition As String = "")
        Try
            Dim query As String = "SELECT * FROM Employee"

            If condition <> "" Then
                query &= " WHERE " & condition
            End If

            LoadToDGV(query, DataGridView1)

            lblTotalEmployees.Text = "Total: " & DataGridView1.Rows.Count.ToString()

        Catch ex As Exception
            MessageBox.Show("Error loading employees: " & ex.Message)
        End Try
    End Sub

    '====================================
    ' ADD EMPLOYEE
    '====================================
    Private Sub AddEmployee_Click(sender As Object, e As EventArgs) Handles AddEmployee.Click
        Dim frm As New AddEmployee()

        frm.StartPosition = FormStartPosition.CenterScreen
        frm.Show()
        frm.BringToFront()
    End Sub

    '====================================
    ' EDIT EMPLOYEE (FIXED)
    '====================================
    Private Sub EditEmployee_Click(sender As Object, e As EventArgs) Handles EditEmployee.Click

        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Select an employee to edit.")
            Exit Sub
        End If

        ' Get the selected EmployeeID
        Dim empID As Integer = DataGridView1.SelectedRows(0).Cells("EmployeeID").Value

        ' Open EditEmployee form + pass employee ID
        Dim frm As New EditEmployee()
        frm.EmployeeIDValue = empID   ' <<< PASS THE ID HERE
        frm.StartPosition = FormStartPosition.CenterScreen
        frm.Show()
        frm.BringToFront()

    End Sub

    '====================================
    ' VIEW ALL EMPLOYEES
    '====================================
    Private Sub btnViewAll_Click(sender As Object, e As EventArgs) Handles btnViewAll.Click
        LoadEmployees()
        lblFilter.Text = "Showing: All Employees"
    End Sub

    '====================================
    ' VIEW ACTIVE EMPLOYEES
    '====================================
    Private Sub btnViewActive_Click(sender As Object, e As EventArgs) Handles btnViewActive.Click
        LoadEmployees("EmploymentStatus = 'Active'")
        lblFilter.Text = "Showing: Active Employees"
    End Sub

    '====================================
    ' VIEW INACTIVE EMPLOYEES
    '====================================
    Private Sub btnViewInactive_Click(sender As Object, e As EventArgs) Handles btnViewInactive.Click
        LoadEmployees("EmploymentStatus = 'Resigned'")
        lblFilter.Text = "Showing: Inactive Employees"
    End Sub

    '====================================
    ' REFRESH BUTTON
    '====================================
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadEmployees()
        lblFilter.Text = "Showing: All Employees"
    End Sub

    '====================================
    ' DELETE EMPLOYEE
    '====================================
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Select an employee to delete.")
            Exit Sub
        End If

        Dim empID As Integer = DataGridView1.SelectedRows(0).Cells("EmployeeID").Value

        If MessageBox.Show("Delete Employee #" & empID & "?",
                           "Confirm Deletion",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Warning) = DialogResult.No Then Exit Sub

        Try
            openConn()

            Dim cmd As New MySqlCommand("DELETE FROM Employee WHERE EmployeeID=@id", conn)
            cmd.Parameters.AddWithValue("@id", empID)
            cmd.ExecuteNonQuery()

            closeConn()

            MessageBox.Show("Employee deleted successfully.")
            LoadEmployees()

        Catch ex As Exception
            MessageBox.Show("Error deleting employee: " & ex.Message)
        End Try
    End Sub

End Class