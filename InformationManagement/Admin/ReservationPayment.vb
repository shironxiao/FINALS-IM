Imports MySqlConnector
Imports System.Data

Public Class ReservationPayment

    Private Sub ReservationPayment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadReservationPayments()
        UpdateTotal()
    End Sub

    ' =============================================================
    ' LOAD RESERVATION PAYMENTS INTO DATAGRIDVIEW
    ' =============================================================
    Private Sub LoadReservationPayments(Optional condition As String = "")
        Try
            Dim query As String =
                "SELECT 
                    ReservationPaymentID,
                    ReservationID,
                    PaymentDate,
                    PaymentMethod,
                    PaymentStatus,
                    AmountPaid,
                    PaymentSource,
                    ProofOfPayment,
                    ReceiptFileName,
                    TransactionID,
                    Notes,
                    UpdatedDate
                FROM reservation_payments"

            If condition <> "" Then
                query &= " WHERE " & condition
            End If

            LoadToDGV(query, Reservation)

        Catch ex As Exception
            MessageBox.Show("Error loading reservation payments: " & ex.Message)
        End Try
    End Sub

    ' =============================================================
    ' SEARCH
    ' =============================================================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim keyword As String = txtSearch.Text.Trim()

        If keyword = "" Then
            LoadReservationPayments()
        Else
            LoadReservationPayments($"ReservationID LIKE '%{keyword}%' OR ReservationPaymentID LIKE '%{keyword}%'")
        End If

        UpdateTotal()
    End Sub

    ' =============================================================
    ' REFRESH
    ' =============================================================
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        txtSearch.Clear()
        LoadReservationPayments()
        UpdateTotal()
    End Sub

    ' =============================================================
    ' UPDATE TOTAL ROW COUNT
    ' =============================================================
    Private Sub UpdateTotal()
        lblTotalRecords.Text = "Total: " & Reservation.Rows.Count.ToString()
    End Sub

End Class