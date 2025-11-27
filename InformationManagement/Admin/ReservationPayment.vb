Imports MySqlConnector
Imports System.Data

Public Class ReservationPayment

    Private Sub ReservationPayment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadReservationPayments()
        UpdateTotal()
    End Sub

    ' =============================================================
    ' LOAD RESERVATION PAYMENTS
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
                UpdatedDate
            FROM reservation_payments"

            If condition <> "" Then
                query &= " WHERE " & condition
            End If

            LoadToDGV(query, Reservation, "")

            FormatGrid()

        Catch ex As Exception
            MessageBox.Show("Error loading reservation payments: " & ex.Message)
        End Try
    End Sub

    ' Dummy wrapper to call modDB.LoadToDGV
    Private Sub LoadToDGV(query As String, dgv As DataGridView, filter As String)
        modDB.LoadToDGV(query, dgv, filter)
    End Sub

    ' =============================================================
    ' FORMAT GRID + HIDE COLUMNS
    ' =============================================================
    Private Sub FormatGrid()
        If Reservation.Columns.Count = 0 Then Exit Sub

        Dim hideCols() As String = {
            "ReservationPaymentID",
            "ReservationID",
            "ProofOfPayment",
            "ReceiptFileName",
            "TransactionID"
        }

        For Each colName In hideCols
            If Reservation.Columns.Contains(colName) Then
                Reservation.Columns(colName).Visible = False
            End If
        Next

        ' Optional formatting
        Reservation.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Reservation.RowHeadersVisible = False
        Reservation.DefaultCellStyle.Font = New Font("Segoe UI", 10)
        Reservation.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 10)
        Reservation.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 60, 85)
        Reservation.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        Reservation.EnableHeadersVisualStyles = False

        ' Format AmountPaid to Peso
        If Reservation.Columns.Contains("AmountPaid") Then
            Reservation.Columns("AmountPaid").DefaultCellStyle.Format = "₱ #,##0.00"
            Reservation.Columns("AmountPaid").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End If
    End Sub

    ' =============================================================
    ' DATA BIND COMPLETE (ensures hidden columns stay hidden)
    ' =============================================================
    Private Sub Reservation_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles Reservation.DataBindingComplete
        FormatGrid()
    End Sub

    ' =============================================================
    ' SEARCH
    ' =============================================================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim keyword As String = txtSearch.Text.Trim()

        If keyword = "" Then
            LoadReservationPayments()
        Else
            LoadReservationPayments(
                $"ReservationID LIKE '%{keyword}%' 
                  OR ReservationPaymentID LIKE '%{keyword}%' 
                  OR PaymentStatus LIKE '%{keyword}%'")
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
    ' UPDATE TOTAL COUNT
    ' =============================================================
    Private Sub UpdateTotal()
        lblTotalRecords.Text = "Total: " & Reservation.Rows.Count.ToString()
    End Sub

End Class
