Imports MySqlConnector
Imports System.Data

Public Class OrderPayment

    Private Sub OrderPayment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadPayments()
        UpdateTotal()
    End Sub

    ' =================================================
    ' LOAD PAYMENTS INTO DATAGRIDVIEW
    ' =================================================
    Private Sub LoadPayments(Optional condition As String = "")
        Try
            Dim query As String =
            "SELECT 
                PaymentID,
                OrderID,
                PaymentDate,
                PaymentMethod,
                PaymentStatus,
                AmountPaid,
                PaymentSource,
                TransactionID,
                Notes
             FROM payments"

            If condition <> "" Then
                query &= " WHERE " & condition
            End If

            LoadToDGV(query, Order, "")
            FormatGrid()

        Catch ex As Exception
            MessageBox.Show("Error loading payments: " & ex.Message)
        End Try
    End Sub

    ' Dummy wrapper to call modDB loader
    Private Sub LoadToDGV(query As String, dgv As DataGridView, filter As String)
        modDB.LoadToDGV(query, dgv, filter)
    End Sub

    ' =================================================
    ' FORMAT GRID + HIDE INTERNAL COLUMNS
    ' =================================================
    Private Sub FormatGrid()
        If Order.Columns.Count = 0 Then Exit Sub

        Dim hideCols() As String = {
            "PaymentID",
            "OrderID",
            "TransactionID"
        }

        For Each colName In hideCols
            If Order.Columns.Contains(colName) Then
                Order.Columns(colName).Visible = False
            End If
        Next

        ' Formatting
        Order.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Order.RowHeadersVisible = False
        Order.DefaultCellStyle.Font = New Font("Segoe UI", 10)
        Order.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 10)

        ' Format AmountPaid to Peso currency
        If Order.Columns.Contains("AmountPaid") Then
            Order.Columns("AmountPaid").DefaultCellStyle.Format = "₱ #,##0.00"
            Order.Columns("AmountPaid").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End If
    End Sub

    ' =================================================
    ' ENSURE COLUMNS STAY HIDDEN AFTER RELOAD
    ' =================================================
    Private Sub Order_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles Order.DataBindingComplete
        FormatGrid()
    End Sub

    ' =================================================
    ' SEARCH
    ' =================================================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim keyword As String = txtSearch.Text.Trim()

        If keyword = "" Then
            LoadPayments()
        Else
            LoadPayments(
                $"OrderID LIKE '%{keyword}%'
                  OR PaymentID LIKE '%{keyword}%'
                  OR PaymentStatus LIKE '%{keyword}%'
                  OR PaymentMethod LIKE '%{keyword}%'")
        End If

        UpdateTotal()
    End Sub

    ' =================================================
    ' REFRESH BUTTON
    ' =================================================
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        txtSearch.Clear()
        LoadPayments()
        UpdateTotal()
    End Sub

    ' =================================================
    ' UPDATE TOTAL COUNT
    ' =================================================
    Private Sub UpdateTotal()
        lblTotalRecords.Text = "Total: " & Order.Rows.Count.ToString()
    End Sub

End Class