Imports MySqlConnector
Imports System.Data

Public Class Reservations

    Private Sub Reservations_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadReservations()
        AddActionButtons()
    End Sub

    ' ==========================================
    ' ADD ACTION BUTTONS TO DATAGRIDVIEW
    ' ==========================================
    Private Sub AddActionButtons()
        ' Check if buttons already exist
        If Reservation.Columns.Contains("btnConfirm") Then
            Exit Sub
        End If

        ' Add Confirm Button Column
        Dim btnConfirm As New DataGridViewButtonColumn()
        btnConfirm.Name = "btnConfirm"
        btnConfirm.HeaderText = "Action"
        btnConfirm.Text = "Confirm"
        btnConfirm.UseColumnTextForButtonValue = True
        btnConfirm.Width = 80
        Reservation.Columns.Add(btnConfirm)

        ' Add Cancel Button Column
        Dim btnCancel As New DataGridViewButtonColumn()
        btnCancel.Name = "btnCancel"
        btnCancel.HeaderText = ""
        btnCancel.Text = "Cancel"
        btnCancel.UseColumnTextForButtonValue = True
        btnCancel.Width = 80
        Reservation.Columns.Add(btnCancel)
    End Sub

    ' ==========================================
    ' HANDLE BUTTON CLICKS IN DATAGRIDVIEW
    ' ==========================================
    Private Sub Reservation_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles Reservation.CellContentClick
        ' Check if click is on a button column and not on header
        If e.RowIndex < 0 Then Exit Sub

        Dim columnName As String = Reservation.Columns(e.ColumnIndex).Name
        Dim reservationID As Integer = Reservation.Rows(e.RowIndex).Cells("ReservationID").Value
        Dim currentStatus As String = Reservation.Rows(e.RowIndex).Cells("ReservationStatus").Value.ToString()

        ' CONFIRM BUTTON CLICKED
        If columnName = "btnConfirm" Then
            If currentStatus = "Confirmed" Then
                MessageBox.Show("This reservation is already confirmed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If MessageBox.Show("Confirm Reservation #" & reservationID & "?",
                               "Confirm Reservation",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question) = DialogResult.Yes Then
                UpdateReservationStatus(reservationID, "Confirmed")
            End If
        End If

        ' CANCEL BUTTON CLICKED
        If columnName = "btnCancel" Then
            If currentStatus = "Cancelled" Then
                MessageBox.Show("This reservation is already cancelled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If MessageBox.Show("Cancel Reservation #" & reservationID & "?",
                               "Cancel Reservation",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Warning) = DialogResult.Yes Then
                UpdateReservationStatus(reservationID, "Cancelled")
            End If
        End If
    End Sub

    ' ==========================================
    ' UPDATE RESERVATION STATUS
    ' ==========================================
    Private Sub UpdateReservationStatus(reservationID As Integer, newStatus As String)
        Try
            openConn()

            Dim query As String = "UPDATE reservations SET ReservationStatus = @status, UpdatedDate = @updated WHERE ReservationID = @id"
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@status", newStatus)
            cmd.Parameters.AddWithValue("@updated", DateTime.Now)
            cmd.Parameters.AddWithValue("@id", reservationID)

            cmd.ExecuteNonQuery()
            closeConn()

            MessageBox.Show($"Reservation #{reservationID} has been {newStatus.ToLower()}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadReservations()

        Catch ex As Exception
            MessageBox.Show("Error updating reservation: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
                    ReservationType,
                    EventType,
                    EventDate,
                    EventTime,
                    NumberOfGuests,
                    ProductSelection,
                    SpecialRequests,
                    ReservationStatus,
                    ReservationDate,
                    DeliveryAddress,
                    DeliveryOption,
                    ContactNumber,
                    UpdatedDate
                 FROM reservations"

            If condition <> "" Then
                query &= " WHERE " & condition
            End If

            query &= " ORDER BY ReservationDate DESC"

            ' Load results into DGV
            LoadToDGV(query, Reservation)

            ' Re-add buttons after loading data
            AddActionButtons()

            ' Update count label
            lblTotalReservations.Text = "Total: " & Reservation.Rows.Count.ToString()

            ' Style the buttons based on status
            StyleActionButtons()

        Catch ex As Exception
            MessageBox.Show("Error loading reservations: " & ex.Message)
        End Try
    End Sub

    ' ==========================================
    ' STYLE ACTION BUTTONS BASED ON STATUS
    ' ==========================================
    Private Sub StyleActionButtons()
        For Each row As DataGridViewRow In Reservation.Rows
            If row.Cells("ReservationStatus") IsNot Nothing Then
                Dim status As String = row.Cells("ReservationStatus").Value.ToString()

                ' Disable Confirm button if already confirmed
                If status = "Confirmed" Then
                    row.Cells("btnConfirm").Style.BackColor = Color.LightGray
                    row.Cells("btnConfirm").Style.ForeColor = Color.Gray
                Else
                    row.Cells("btnConfirm").Style.BackColor = Color.FromArgb(40, 167, 69)
                    row.Cells("btnConfirm").Style.ForeColor = Color.White
                End If

                ' Disable Cancel button if already cancelled
                If status = "Cancelled" Then
                    row.Cells("btnCancel").Style.BackColor = Color.LightGray
                    row.Cells("btnCancel").Style.ForeColor = Color.Gray
                Else
                    row.Cells("btnCancel").Style.BackColor = Color.FromArgb(220, 53, 69)
                    row.Cells("btnCancel").Style.ForeColor = Color.White
                End If
            End If
        Next
    End Sub

    ' ==========================================
    ' GENERIC LOAD FUNCTION
    ' ==========================================
    Private Sub LoadToDGV(query As String, dgv As DataGridView)
        Try
            openConn()

            Dim cmd As New MySqlCommand(query, conn)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()

            adapter.Fill(dt)
            dgv.DataSource = dt

            closeConn()

        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
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