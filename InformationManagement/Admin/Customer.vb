Imports MySql.Data.MySqlClient

Public Class Customer

    Private dbConnection As MySqlConnection
    Private connectionString As String = "Server=127.0.0.1;User=root;Password=;Database=tabeya_system"

    ' ==============================
    ' FORM LOAD
    ' ==============================
    Private Sub Customer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeDatabase()
        LoadCustomerData()
    End Sub

    Private Sub InitializeDatabase()
        dbConnection = New MySqlConnection(connectionString)
    End Sub

    ' ==============================
    ' LOAD CUSTOMER LIST
    ' ==============================
    Private Sub LoadCustomerData(Optional searchText As String = "")
        Try
            If dbConnection.State = ConnectionState.Open Then dbConnection.Close()
            dbConnection.Open()

            Dim query As String =
                "SELECT CustomerID, FirstName, LastName, Email, ContactNumber, CustomerType,
                        FeedbackCount, TotalOrdersCount, ReservationCount, LastTransactionDate,
                        LastLoginDate, CreatedDate, AccountStatus, SatisfactionRating
                 FROM customers
                 WHERE CONCAT(FirstName,' ',LastName,Email,ContactNumber) LIKE @search
                 ORDER BY CustomerID DESC"

            Dim cmd As New MySqlCommand(query, dbConnection)
            cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")

            Dim adapter As New MySqlDataAdapter(cmd)
            Dim table As New DataTable()
            adapter.Fill(table)

            DataGridView1.DataSource = table
            FormatDataGridView()

        Catch ex As Exception
            MessageBox.Show("Error loading customers: " & ex.Message)
        Finally
            If dbConnection.State = ConnectionState.Open Then dbConnection.Close()
        End Try
    End Sub

    ' ==============================
    ' FORMAT GRID
    ' ==============================
    Private Sub FormatDataGridView()
        For Each col As DataGridViewColumn In DataGridView1.Columns
            If col.Name.Contains("Date") Then col.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm"
            If col.Name = "SatisfactionRating" Then col.DefaultCellStyle.Format = "0.00"
        Next

        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    ' ==============================
    ' BUTTON: REFRESH
    ' ==============================
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        txtSearch.Text = ""
        LoadCustomerData()
    End Sub

    ' ==============================
    ' BUTTON: DELETE
    ' ==============================
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a customer to delete.")
            Return
        End If

        Dim customerId As Integer = DataGridView1.SelectedRows(0).Cells("CustomerID").Value

        If MessageBox.Show("Delete this customer?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then Exit Sub

        Try
            dbConnection.Open()

            Dim cmd As New MySqlCommand("CALL ArchiveCustomer(@id)", dbConnection)
            cmd.Parameters.AddWithValue("@id", customerId)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Customer archived & deleted!")
            LoadCustomerData()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            dbConnection.Close()
        End Try
    End Sub

    ' ==============================
    ' SEARCH
    ' ==============================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadCustomerData(txtSearch.Text)
    End Sub

End Class