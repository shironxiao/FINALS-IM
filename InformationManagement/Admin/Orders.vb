Imports MySqlConnector
Imports System.Data

Public Class Orders

    Private Sub Orders_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadOrders()
        lblFilter.Text = "Showing: All Orders"
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

            lblTotalOrders.Text = "Total Orders: " & DataGridView2.Rows.Count

        Catch ex As Exception
            MessageBox.Show("Error loading orders: " & ex.Message)
        End Try
    End Sub

    ' ============================================================
    ' VIEW ALL ORDERS
    ' ============================================================
    Private Sub btnViewAll_Click(sender As Object, e As EventArgs) Handles btnViewAll.Click
        LoadOrders()
        lblFilter.Text = "Showing: All Orders"
    End Sub

    ' ============================================================
    ' VIEW PENDING ORDERS
    ' ============================================================
    Private Sub btnViewPending_Click(sender As Object, e As EventArgs) Handles btnViewPending.Click
        LoadOrders("OrderStatus = 'Pending'")
        lblFilter.Text = "Showing: Pending Orders"
    End Sub

    ' ============================================================
    ' VIEW COMPLETED ORDERS
    ' ============================================================
    Private Sub btnViewCompleted_Click(sender As Object, e As EventArgs) Handles btnViewCompleted.Click
        LoadOrders("OrderStatus = 'Completed'")
        lblFilter.Text = "Showing: Completed Orders"
    End Sub

    ' ============================================================
    ' VIEW CANCELLED ORDERS
    ' ============================================================
    Private Sub btnViewCancelled_Click(sender As Object, e As EventArgs) Handles btnViewCancelled.Click
        LoadOrders("OrderStatus = 'Cancelled'")
        lblFilter.Text = "Showing: Cancelled Orders"
    End Sub

    ' ============================================================
    ' SEARCH ORDERS
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
    ' REFRESH ORDERS
    ' ============================================================
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadOrders()
        txtSearch.Text = ""
        lblFilter.Text = "Showing: All Orders"
    End Sub

    ' OPTIONAL HANDLE CELL CLICK
    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

    End Sub

End Class