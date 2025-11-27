Imports MySqlConnector
Imports System.Data

Public Class Orders

    Private ButtonsAdded As Boolean = False

    Private Sub Orders_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadOrders()
        lblFilter.Text = "Showing: All Orders"

        ' Optional enhanced UI
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

            ' ✅ FORMAT + HIDE COLUMNS + FIX SIZES
            With DataGridView2

                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
                .RowHeadersVisible = False

                If .Columns.Contains("OrderID") Then .Columns("OrderID").Visible = False
                If .Columns.Contains("CustomerID") Then .Columns("CustomerID").Visible = False
                If .Columns.Contains("EmployeeID") Then .Columns("EmployeeID").Visible = False

                If .Columns.Contains("OrderType") Then
                    .Columns("OrderType").HeaderText = "Type"
                    .Columns("OrderType").Width = 90
                End If

                If .Columns.Contains("OrderSource") Then
                    .Columns("OrderSource").HeaderText = "Source"
                    .Columns("OrderSource").Width = 110
                End If

                If .Columns.Contains("ItemsOrderedCount") Then
                    .Columns("ItemsOrderedCount").HeaderText = "Items"
                    .Columns("ItemsOrderedCount").Width = 70
                End If

                If .Columns.Contains("TotalAmount") Then
                    .Columns("TotalAmount").HeaderText = "Total ₱"
                    .Columns("TotalAmount").DefaultCellStyle.Format = "₱#,##0.00"
                    .Columns("TotalAmount").Width = 110
                End If

                If .Columns.Contains("OrderDate") Then
                    .Columns("OrderDate").DefaultCellStyle.Format = "yyyy-MM-dd"
                    .Columns("OrderDate").Width = 120
                End If

                If .Columns.Contains("OrderTime") Then
                    .Columns("OrderTime").Width = 90
                End If

                If .Columns.Contains("OrderStatus") Then
                    .Columns("OrderStatus").Width = 110
                End If

                If .Columns.Contains("Remarks") Then
                    .Columns("Remarks").Width = 150
                End If

                .RowTemplate.Height = 35
                .ColumnHeadersHeight = 40
                .AllowUserToResizeColumns = False
                .AllowUserToResizeRows = False
            End With

            ' ✅ ADD BUTTONS ONLY ONCE
            If Not ButtonsAdded Then
                Dim btnConfirm As New DataGridViewButtonColumn()
                btnConfirm.HeaderText = ""
                btnConfirm.Text = "Confirm"
                btnConfirm.UseColumnTextForButtonValue = True
                btnConfirm.Name = "ConfirmBtn"
                btnConfirm.Width = 90
                DataGridView2.Columns.Add(btnConfirm)

                Dim btnCancel As New DataGridViewButtonColumn()
                btnCancel.HeaderText = ""
                btnCancel.Text = "Cancel"
                btnCancel.UseColumnTextForButtonValue = True
                btnCancel.Name = "CancelBtn"
                btnCancel.Width = 90
                DataGridView2.Columns.Add(btnCancel)

                ButtonsAdded = True
            End If

            ' ✅ BUTTON VISUAL LOGIC
            For Each row As DataGridViewRow In DataGridView2.Rows
                Dim status As String = row.Cells("OrderStatus").Value.ToString()

                If status = "Pending" Then
                    row.Cells("ConfirmBtn").Style.BackColor = Color.LightGreen
                    row.Cells("CancelBtn").Style.BackColor = Color.LightCoral
                Else
                    row.Cells("ConfirmBtn").Style.ForeColor = Color.Gray
                    row.Cells("CancelBtn").Style.ForeColor = Color.Gray
                    row.Cells("ConfirmBtn").Style.BackColor = Color.LightGray
                    row.Cells("CancelBtn").Style.BackColor = Color.LightGray
                End If
            Next

            lblTotalOrders.Text = "Total Orders: " & DataGridView2.Rows.Count

        Catch ex As Exception
            MessageBox.Show("Error loading orders: " & ex.Message)
        End Try
    End Sub


    ' ============================================================
    ' LOAD DATA INTO DATAGRIDVIEW
    ' ============================================================
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
    ' HANDLE CONFIRM / CANCEL BUTTON CLICKS
    ' ============================================================
    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = DataGridView2.Rows(e.RowIndex)
        Dim orderID As Integer = row.Cells("OrderID").Value
        Dim status As String = row.Cells("OrderStatus").Value.ToString()

        If status <> "Pending" Then Exit Sub

        If DataGridView2.Columns(e.ColumnIndex).Name = "ConfirmBtn" Then
            UpdateOrderStatus(orderID, "Completed")
            LoadOrders()

        ElseIf DataGridView2.Columns(e.ColumnIndex).Name = "CancelBtn" Then
            UpdateOrderStatus(orderID, "Cancelled")
            LoadOrders()
        End If
    End Sub


    ' ============================================================
    ' FILTER BUTTONS
    ' ============================================================
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


    ' ============================================================
    ' SEARCH
    ' ============================================================
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


    ' ============================================================
    ' REFRESH
    ' ============================================================
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadOrders()
        txtSearch.Text = ""
        lblFilter.Text = "Showing: All Orders"
    End Sub

End Class
