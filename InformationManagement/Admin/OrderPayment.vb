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

            ' FIXED — Use modDB loader
            LoadToDGV(query, Order, "")

        Catch ex As Exception
            MessageBox.Show("Error loading payments: " & ex.Message)
        End Try
    End Sub

    ' Dummy loader required for call compatibility
    Private Sub LoadToDGV(query As String, dgv As DataGridView, filter As String)
        modDB.LoadToDGV(query, dgv, filter)
    End Sub

    ' =================================================
    ' SEARCH
    ' =================================================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim keyword As String = txtSearch.Text.Trim()

        If keyword = "" Then
            LoadPayments()
        Else
            LoadPayments($"OrderID LIKE '%{keyword}%' OR PaymentID LIKE '%{keyword}%'")
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