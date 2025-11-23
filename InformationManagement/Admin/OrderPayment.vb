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

            LoadToDGV(query, Order)

        Catch ex As Exception
            MessageBox.Show("Error loading payments: " & ex.Message)
        End Try
    End Sub

    ' =================================================
    ' SEARCH
    ' =================================================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged

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