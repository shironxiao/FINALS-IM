Imports MySqlConnector
Imports System.Data

Public Class Employee

    Private Sub Employee_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadEmployees()
    End Sub

    ' ===============================
    ' LOAD EMPLOYEES INTO DGV
    ' ===============================
    Private Sub LoadEmployees()
        Try
            Dim query As String = "SELECT * FROM Employee"
            LoadToDGV(query, DataGridView1)
        Catch ex As Exception
            MessageBox.Show("Error loading employees: " & ex.Message)
        End Try
    End Sub

    ' ===============================
    ' OPEN ADD EMPLOYEE FORM
    ' ===============================
    Private Sub AddEmployee_Click(sender As Object, e As EventArgs) Handles AddEmployee.Click
        Dim frm As New AddEmployee()

        With frm
            .StartPosition = FormStartPosition.CenterScreen
            .Show()
            .BringToFront()
        End With
    End Sub

End Class
