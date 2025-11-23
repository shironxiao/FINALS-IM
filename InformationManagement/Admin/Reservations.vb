Imports MySqlConnector
Imports System.Data

Public Class Reservations

    Private Sub Reservations_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadReservations()
    End Sub

    ' ==========================================
    ' LOAD RESERVATIONS
    ' ==========================================
    Private Sub LoadReservations(Optional condition As String = "")
        Try
            Dim query As String =
                "SELECT 
                    ReservationID,
                    CustomerID,
                    AssignedStaffID,
                    ReservationType,
                    EventType,
                    EventDate,
                    EventTime,
                    NumberOfGuests,
                    ProductSelection,
                    SpecialRequests,
                    ReservationStatus,
                    ReservationDate,
                    Address,
                    DeliveryAddress,
                    ServiceType,
                    DeliveryOption,
                    ContactNumber,
                    UpdatedDate
                 FROM reservations"

            If condition <> "" Then
                query &= " WHERE " & condition
            End If

            LoadToDGV(query, Reservation)

            lblTotalReservations.Text = "Total: " & Reservation.Rows.Count.ToString()

        Catch ex As Exception
            MessageBox.Show("Error loading reservations: " & ex.Message)
        End Try
    End Sub

    ' ==========================================
    ' VIEW ALL
    ' ==========================================
    Private Sub btnViewAll_Click(sender As Object, e As EventArgs) Handles btnViewAll.Click
        LoadReservations()
        lblFilter.Text = "Showing: All Reservations"
    End Sub

    ' ==========================================
    ' VIEW PENDING
    ' ==========================================
    Private Sub btnViewPending_Click(sender As Object, e As EventArgs) Handles btnViewPending.Click
        LoadReservations("ReservationStatus = 'Pending'")
        lblFilter.Text = "Showing: Pending"
    End Sub

    ' ==========================================
    ' VIEW CONFIRMED
    ' ==========================================
    Private Sub btnViewConfirmed_Click(sender As Object, e As EventArgs) Handles btnViewConfirmed.Click
        LoadReservations("ReservationStatus = 'Confirmed'")
        lblFilter.Text = "Showing: Confirmed"
    End Sub

    ' ==========================================
    ' VIEW CANCELLED
    ' ==========================================
    Private Sub btnViewCancelled_Click(sender As Object, e As EventArgs) Handles btnViewCancelled.Click
        LoadReservations("ReservationStatus = 'Cancelled'")
        lblFilter.Text = "Showing: Cancelled"
    End Sub

    ' ==========================================
    ' REFRESH
    ' ==========================================
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadReservations()
        lblFilter.Text = "Showing: All Reservations"
    End Sub

    ' ==========================================
    ' SEARCH BAR
    ' ==========================================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim keyword As String = txtSearch.Text.Trim()

        If keyword = "" Then
            LoadReservations()
            Exit Sub
        End If

        LoadReservations($"ReservationID LIKE '%{keyword}%' 
                          OR CustomerID LIKE '%{keyword}%' 
                          OR EventType LIKE '%{keyword}%' 
                          OR ReservationStatus LIKE '%{keyword}%'")
    End Sub

    ' ==========================================
    ' DELETE RESERVATION
    ' ==========================================
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If Reservation.SelectedRows.Count = 0 Then
            MessageBox.Show("Select a reservation to delete.")
            Exit Sub
        End If

        Dim id As Integer = Reservation.SelectedRows(0).Cells("ReservationID").Value

        If MessageBox.Show("Are you sure to delete Reservation #" & id & "?",
                           "Confirm Delete",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Warning) = DialogResult.No Then Exit Sub

        Try
            openConn()

            Dim cmd As New MySqlCommand("DELETE FROM reservations WHERE ReservationID=@id", conn)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()

            closeConn()

            MessageBox.Show("Reservation deleted successfully.")
            LoadReservations()

        Catch ex As Exception
            MessageBox.Show("Error deleting reservation: " & ex.Message)
        End Try

    End Sub

End Class