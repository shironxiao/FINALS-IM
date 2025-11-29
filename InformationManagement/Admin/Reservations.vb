Imports MySqlConnector
Imports System.Data

Public Class Reservations

    Private Sub Reservations_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadReservations()
    End Sub

    ' ==========================================
    ' UPDATE STATUS BUTTON (Form Button)
    ' ==========================================
    Private Sub btnUpdateStatus_Click(sender As Object, e As EventArgs) Handles btnUpdateStatus.Click
        Try
            ' Check if a row is selected
            If Reservation.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a reservation to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Get selected reservation details
            Dim selectedRow As DataGridViewRow = Reservation.SelectedRows(0)
            Dim reservationID As Integer = Convert.ToInt32(selectedRow.Cells("ReservationID").Value)
            Dim currentStatus As String = selectedRow.Cells("ReservationStatus").Value.ToString().Trim()

            ' Show dialog to select new status
            Dim statusForm As New Form()
            statusForm.Text = "Update Reservation Status"
            statusForm.Size = New Size(400, 250)
            statusForm.StartPosition = FormStartPosition.CenterParent
            statusForm.FormBorderStyle = FormBorderStyle.FixedDialog
            statusForm.MaximizeBox = False
            statusForm.MinimizeBox = False

            ' Label
            Dim lblInfo As New Label()
            lblInfo.Text = $"Reservation ID: {reservationID}" & vbCrLf &
                          $"Current Status: {currentStatus}" & vbCrLf & vbCrLf &
                          "Select new status:"
            lblInfo.Location = New Point(20, 20)
            lblInfo.Size = New Size(350, 70)
            lblInfo.Font = New Font("Segoe UI", 10)
            statusForm.Controls.Add(lblInfo)

            ' Radio buttons for status options
            Dim rbPending As New RadioButton()
            rbPending.Text = "Pending"
            rbPending.Location = New Point(30, 100)
            rbPending.Size = New Size(100, 25)
            rbPending.Font = New Font("Segoe UI", 10)
            rbPending.Checked = (currentStatus.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            statusForm.Controls.Add(rbPending)

            Dim rbConfirmed As New RadioButton()
            rbConfirmed.Text = "Confirmed"
            rbConfirmed.Location = New Point(140, 100)
            rbConfirmed.Size = New Size(110, 25)
            rbConfirmed.Font = New Font("Segoe UI", 10)
            rbConfirmed.Checked = (currentStatus.Equals("Confirmed", StringComparison.OrdinalIgnoreCase))
            statusForm.Controls.Add(rbConfirmed)

            Dim rbCancelled As New RadioButton()
            rbCancelled.Text = "Cancelled"
            rbCancelled.Location = New Point(260, 100)
            rbCancelled.Size = New Size(110, 25)
            rbCancelled.Font = New Font("Segoe UI", 10)
            rbCancelled.Checked = (currentStatus.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
            statusForm.Controls.Add(rbCancelled)

            ' Ensure at least one is checked if none match
            If Not (rbPending.Checked Or rbConfirmed.Checked Or rbCancelled.Checked) Then
                rbPending.Checked = True
            End If

            ' Buttons
            Dim btnOK As New Button()
            btnOK.Text = "Update"
            btnOK.Location = New Point(200, 160)
            btnOK.Size = New Size(80, 35)
            btnOK.DialogResult = DialogResult.OK
            btnOK.Font = New Font("Segoe UI", 9)
            statusForm.Controls.Add(btnOK)

            Dim btnCancel As New Button()
            btnCancel.Text = "Cancel"
            btnCancel.Location = New Point(290, 160)
            btnCancel.Size = New Size(80, 35)
            btnCancel.DialogResult = DialogResult.Cancel
            btnCancel.Font = New Font("Segoe UI", 9)
            statusForm.Controls.Add(btnCancel)

            statusForm.AcceptButton = btnOK
            statusForm.CancelButton = btnCancel

            ' Show the dialog
            If statusForm.ShowDialog() = DialogResult.OK Then
                Dim newStatus As String = ""

                If rbPending.Checked Then
                    newStatus = "Pending"
                ElseIf rbConfirmed.Checked Then
                    newStatus = "Confirmed"
                ElseIf rbCancelled.Checked Then
                    newStatus = "Cancelled"
                End If

                ' Check if status is actually changing (case-insensitive comparison)
                If newStatus.Equals(currentStatus, StringComparison.OrdinalIgnoreCase) Then
                    MessageBox.Show($"Reservation status is already '{currentStatus}'.", "No Change", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If

                ' Update the reservation status
                UpdateReservationStatus(reservationID, newStatus)
            End If

        Catch ex As Exception
            MessageBox.Show("Error updating reservation status: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ==========================================
    ' UPDATE RESERVATION STATUS
    ' ==========================================
    Private Sub UpdateReservationStatus(reservationID As Integer, newStatus As String)
        Dim connection As MySqlConnection = Nothing

        Try
            connection = New MySqlConnection(strConnection)
            connection.Open()

            ' Use parameterized query to prevent SQL injection
            Dim query As String = "UPDATE reservations SET ReservationStatus = @status, UpdatedDate = @updated WHERE ReservationID = @id"

            Using cmd As New MySqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@status", newStatus)
                cmd.Parameters.AddWithValue("@updated", DateTime.Now)
                cmd.Parameters.AddWithValue("@id", reservationID)

                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                If rowsAffected = 0 Then
                    MessageBox.Show("No reservation was updated. Please check if the reservation exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            End Using

            MessageBox.Show($"Reservation #{reservationID} has been updated to '{newStatus}'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As MySqlException
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Error updating reservation: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If connection IsNot Nothing AndAlso connection.State = ConnectionState.Open Then
                connection.Close()
            End If
            LoadReservations()
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

            ' Update count label
            lblTotalReservations.Text = "Total: " & Reservation.Rows.Count.ToString()

        Catch ex As Exception
            MessageBox.Show("Error loading reservations: " & ex.Message)
        End Try
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
            MessageBox.Show("Select a reservation to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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