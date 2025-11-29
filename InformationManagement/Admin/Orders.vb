Imports MySqlConnector
Imports System.Data

Public Class Orders

    Private ButtonsAdded As Boolean = False

    Private Sub Orders_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadOrders()
        lblFilter.Text = "Showing: All Orders"

        With DataGridView2
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .BorderStyle = BorderStyle.None
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245)
        End With
    End Sub

    ' ============================================================
    ' LOAD ORDERS
    ' ============================================================
    Private Sub LoadOrders(Optional condition As String = "")
        Try
            Dim query As String =
            "SELECT OrderID, CustomerID, EmployeeID, OrderType, OrderSource,
                    ReceiptNumber, NumberOfDiners, OrderDate, OrderTime,
                    ItemsOrderedCount, TotalAmount, OrderStatus, Remarks,
                    OrderPriority, PreparationTimeEstimate, SpecialRequestFlag,
                    CreatedDate, UpdatedDate
             FROM orders"

            If condition <> "" Then
                query &= " WHERE " & condition
            End If

            LoadToDGV(query, DataGridView2)

            ' FORMAT
            With DataGridView2

                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
                .RowHeadersVisible = False

                If .Columns.Contains("OrderID") Then .Columns("OrderID").Visible = False
                If .Columns.Contains("CustomerID") Then .Columns("CustomerID").Visible = False
                If .Columns.Contains("EmployeeID") Then .Columns("EmployeeID").Visible = False

                If .Columns.Contains("OrderType") Then .Columns("OrderType").Width = 100
                If .Columns.Contains("OrderSource") Then .Columns("OrderSource").Width = 120
                If .Columns.Contains("ItemsOrderedCount") Then .Columns("ItemsOrderedCount").Width = 80
                If .Columns.Contains("TotalAmount") Then .Columns("TotalAmount").Width = 120
                If .Columns.Contains("OrderDate") Then .Columns("OrderDate").Width = 120
                If .Columns.Contains("OrderTime") Then .Columns("OrderTime").Width = 100
                If .Columns.Contains("OrderStatus") Then .Columns("OrderStatus").Width = 120
                If .Columns.Contains("Remarks") Then .Columns("Remarks").Width = 160

                .RowTemplate.Height = 35
                .ColumnHeadersHeight = 40
                .AllowUserToResizeColumns = False
                .AllowUserToResizeRows = False

            End With

            ' ADD BUTTONS ONCE
            If Not ButtonsAdded Then

                ' Confirm Button
                Dim btnConfirm As New DataGridViewButtonColumn()
                btnConfirm.HeaderText = ""
                btnConfirm.Text = "Confirm"
                btnConfirm.UseColumnTextForButtonValue = True
                btnConfirm.Name = "ConfirmBtn"
                btnConfirm.Width = 90
                DataGridView2.Columns.Add(btnConfirm)

                ' Cancel Button
                Dim btnCancel As New DataGridViewButtonColumn()
                btnCancel.HeaderText = ""
                btnCancel.Text = "Cancel"
                btnCancel.UseColumnTextForButtonValue = True
                btnCancel.Name = "CancelBtn"
                btnCancel.Width = 90
                DataGridView2.Columns.Add(btnCancel)

                ' NEW: DELETE BUTTON
                Dim btnDelete As New DataGridViewButtonColumn()
                btnDelete.HeaderText = ""
                btnDelete.Text = "Delete"
                btnDelete.UseColumnTextForButtonValue = True
                btnDelete.Name = "DeleteBtn"
                btnDelete.Width = 90
                DataGridView2.Columns.Add(btnDelete)

                ButtonsAdded = True
            End If

            ' BUTTON LOGIC (color for pending)
            For Each row As DataGridViewRow In DataGridView2.Rows
                Dim status As String = row.Cells("OrderStatus").Value.ToString()

                If status = "Pending" Then
                    row.Cells("ConfirmBtn").Style.BackColor = Color.LightGreen
                    row.Cells("CancelBtn").Style.BackColor = Color.LightCoral
                Else
                    row.Cells("ConfirmBtn").Style.BackColor = Color.LightGray
                    row.Cells("CancelBtn").Style.BackColor = Color.LightGray
                End If

                ' DELETE always active
                row.Cells("DeleteBtn").Style.BackColor = Color.IndianRed
                row.Cells("DeleteBtn").Style.ForeColor = Color.White
            Next

            lblTotalOrders.Text = "Total Orders: " & DataGridView2.Rows.Count

        Catch ex As Exception
            MessageBox.Show("Error loading orders: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadToDGV(query As String, dgv As DataGridView)
        Try
            Using conn As New MySqlConnection("Server=127.0.0.1;User=root;Password=;Database=tabeya_system")
                conn.Open()

                Using cmd As New MySqlCommand(query, conn)
                    Using da As New MySqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        dgv.DataSource = dt
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Database Error: " & ex.Message)
        End Try
    End Sub

    ' ============================================================
    ' UPDATE ORDER STATUS
    ' ============================================================
    Private Sub UpdateOrderStatus(orderID As Integer, newStatus As String)
        Try
            Using conn As New MySqlConnection("Server=127.0.0.1;User=root;Password=;Database=tabeya_system")
                conn.Open()

                Dim query As String =
                    "UPDATE orders SET OrderStatus = @status, UpdatedDate = NOW()
                     WHERE OrderID = @orderID"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@status", newStatus)
                    cmd.Parameters.AddWithValue("@orderID", orderID)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error updating status: " & ex.Message)
        End Try
    End Sub

    ' ============================================================
    ' DELETE ORDER
    ' ============================================================
    Private Sub DeleteOrder(orderID As Integer)
        Try
            If MessageBox.Show("Are you sure you want to DELETE this order?",
                               "Confirm Delete", MessageBoxButtons.YesNo,
                               MessageBoxIcon.Warning) = DialogResult.No Then Exit Sub

            Using conn As New MySqlConnection("Server=127.0.0.1;User=root;Password=;Database=tabeya_system")
                conn.Open()

                Dim query As String = "DELETE FROM orders WHERE OrderID = @orderID"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@orderID", orderID)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Order deleted successfully!", "Deleted")
            LoadOrders()

        Catch ex As Exception
            MessageBox.Show("Delete Error: " & ex.Message)
        End Try
    End Sub

    ' ============================================================
    ' HANDLE CONFIRM / CANCEL / DELETE CLICKS
    ' ============================================================
    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = DataGridView2.Rows(e.RowIndex)
        Dim orderID As Integer = row.Cells("OrderID").Value
        Dim status As String = row.Cells("OrderStatus").Value.ToString()

        ' Confirm
        If DataGridView2.Columns(e.ColumnIndex).Name = "ConfirmBtn" Then
            If status = "Pending" Then
                UpdateOrderStatus(orderID, "Completed")
                LoadOrders()
            End If
        End If

        ' Cancel
        If DataGridView2.Columns(e.ColumnIndex).Name = "CancelBtn" Then
            If status = "Pending" Then
                UpdateOrderStatus(orderID, "Cancelled")
                LoadOrders()
            End If
        End If

        ' DELETE
        If DataGridView2.Columns(e.ColumnIndex).Name = "DeleteBtn" Then
            DeleteOrder(orderID)
        End If

    End Sub

    ' FILTER BUTTONS
    Private Sub btnViewAll_Click(sender As Object, e As EventArgs) Handles btnViewAll.Click
        LoadOrders()
        lblFilter.Text = "Showing: All Orders"
    End Sub

    Private Sub btnViewPending_Click(sender As Object, e As EventArgs) Handles btnViewPending.Click
        LoadOrders("OrderStatus = 'Pending'")
        lblFilter.Text = "Showing: Pending Orders"
    End Sub

    Private Sub btnViewCompleted_Click(sender As Object, e As EventArgs) Handles btnViewCompleted.Click
        LoadOrders("OrderStatus = 'Completed'")
        lblFilter.Text = "Showing: Completed Orders"
    End Sub

    Private Sub btnViewCancelled_Click(sender As Object, e As EventArgs) Handles btnViewCancelled.Click
        LoadOrders("OrderStatus = 'Cancelled'")
        lblFilter.Text = "Showing: Cancelled Orders"
    End Sub

    ' SEARCH
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim search As String = txtSearch.Text.Trim()

        If search = "" Then
            LoadOrders()
            lblFilter.Text = "Showing: All Orders"
            Exit Sub
        End If

        LoadOrders($"OrderID LIKE '%{search}%'
                    OR CustomerID LIKE '%{search}%'
                    OR OrderStatus LIKE '%{search}%'")

        lblFilter.Text = "Search Results"
    End Sub

    ' REFRESH
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadOrders()
        txtSearch.Text = ""
        lblFilter.Text = "Showing: All Orders"
    End Sub

End Class
