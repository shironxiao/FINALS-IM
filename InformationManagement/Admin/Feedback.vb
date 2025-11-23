Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Feedback
    ' Database connection string
    Private connectionString As String = "Server=localhost;Database=tabeya_system;Uid=root;Pwd=;"
    Private conn As MySqlConnection
    Private adapter As MySqlDataAdapter
    Private dt As DataTable

    ' Form Load Event
    Private Sub Feedback_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeConnection()
        LoadReviews()
        SetupDataGridView()
        LoadReservations()
    End Sub

    ' Initialize Database Connection
    Private Sub InitializeConnection()
        Try
            conn = New MySqlConnection(connectionString)
        Catch ex As Exception
            MessageBox.Show("Connection Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Setup DataGridView Appearance
    Private Sub SetupDataGridView()
        With DataGridView1
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .AllowUserToAddRows = False
            .RowHeadersVisible = False

            ' Add Action Buttons if not exists
            If Not .Columns.Contains("Approve") Then
                Dim btnApprove As New DataGridViewButtonColumn()
                btnApprove.Name = "Approve"
                btnApprove.HeaderText = "Approve"
                btnApprove.Text = "Approve"
                btnApprove.UseColumnTextForButtonValue = True
                .Columns.Add(btnApprove)
            End If

            If Not .Columns.Contains("Reject") Then
                Dim btnReject As New DataGridViewButtonColumn()
                btnReject.Name = "Reject"
                btnReject.HeaderText = "Reject"
                btnReject.Text = "Reject"
                btnReject.UseColumnTextForButtonValue = True
                .Columns.Add(btnReject)
            End If
        End With
    End Sub

    ' Load All Reviews from Database
    Private Sub LoadReviews(Optional status As String = "")
        Try
            conn.Open()

            Dim query As String = "SELECT 
                cr.ReviewID,
                cr.CustomerID,
                CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
                cr.OverallRating,
                cr.FoodTasteRating,
                cr.PortionSizeRating,
                cr.CustomerServiceRating,
                cr.AmbienceRating,
                cr.CleanlinessRating,
                cr.GeneralComment,
                cr.Status,
                cr.CreatedDate,
                cr.ApprovedDate
                FROM customer_reviews cr
                INNER JOIN customers c ON cr.CustomerID = c.CustomerID"

            If status <> "" Then
                query &= " WHERE cr.Status = @status"
            End If

            query &= " ORDER BY cr.CreatedDate DESC"

            adapter = New MySqlDataAdapter(query, conn)

            If status <> "" Then
                adapter.SelectCommand.Parameters.AddWithValue("@status", status)
            End If

            dt = New DataTable()
            adapter.Fill(dt)

            DataGridView1.DataSource = dt

            ' Format columns
            FormatColumns()

            ' Update status label
            lblTotalReviews.Text = "Total Reviews: " & dt.Rows.Count

        Catch ex As Exception
            MessageBox.Show("Error loading reviews: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' Format DataGridView Columns
    Private Sub FormatColumns()
        With DataGridView1
            If .Columns.Contains("ReviewID") Then .Columns("ReviewID").HeaderText = "Review ID"
            If .Columns.Contains("CustomerID") Then .Columns("CustomerID").Visible = False
            If .Columns.Contains("CustomerName") Then .Columns("CustomerName").HeaderText = "Customer Name"
            If .Columns.Contains("OverallRating") Then .Columns("OverallRating").HeaderText = "Overall Rating"
            If .Columns.Contains("FoodTasteRating") Then .Columns("FoodTasteRating").HeaderText = "Food"
            If .Columns.Contains("PortionSizeRating") Then .Columns("PortionSizeRating").HeaderText = "Portion"
            If .Columns.Contains("CustomerServiceRating") Then .Columns("CustomerServiceRating").HeaderText = "Service"
            If .Columns.Contains("AmbienceRating") Then .Columns("AmbienceRating").HeaderText = "Ambience"
            If .Columns.Contains("CleanlinessRating") Then .Columns("CleanlinessRating").HeaderText = "Cleanliness"
            If .Columns.Contains("GeneralComment") Then .Columns("GeneralComment").HeaderText = "Comment"
            If .Columns.Contains("Status") Then .Columns("Status").HeaderText = "Status"
            If .Columns.Contains("CreatedDate") Then .Columns("CreatedDate").HeaderText = "Date Created"
            If .Columns.Contains("ApprovedDate") Then .Columns("ApprovedDate").HeaderText = "Date Approved"
        End With
    End Sub

    ' DataGridView Cell Click Event (for action buttons)
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.RowIndex < 0 Then Return

        Dim columnName As String = DataGridView1.Columns(e.ColumnIndex).Name
        Dim reviewId As Integer = Convert.ToInt32(DataGridView1.Rows(e.RowIndex).Cells("ReviewID").Value)
        Dim currentStatus As String = DataGridView1.Rows(e.RowIndex).Cells("Status").Value.ToString()

        If columnName = "Approve" Then
            If currentStatus = "Approved" Then
                MessageBox.Show("This review is already approved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If MessageBox.Show("Are you sure you want to approve this review?", "Confirm Approval", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                UpdateReviewStatus(reviewId, "Approved")
            End If

        ElseIf columnName = "Reject" Then
            If currentStatus = "Rejected" Then
                MessageBox.Show("This review is already rejected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If MessageBox.Show("Are you sure you want to reject this review?", "Confirm Rejection", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                UpdateReviewStatus(reviewId, "Rejected")
            End If
        End If
    End Sub

    ' Update Review Status (Approve/Reject)
    Private Sub UpdateReviewStatus(reviewId As Integer, status As String)
        Try
            conn.Open()

            Dim query As String = "UPDATE customer_reviews 
                                  SET Status = @status, 
                                      ApprovedDate = IF(@status = 'Approved', NOW(), NULL),
                                      UpdatedDate = NOW()
                                  WHERE ReviewID = @reviewId"

            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@status", status)
            cmd.Parameters.AddWithValue("@reviewId", reviewId)

            Dim result As Integer = cmd.ExecuteNonQuery()

            If result > 0 Then
                ' If approved, increment customer's FeedbackCount
                If status = "Approved" Then
                    IncrementFeedbackCount(reviewId)
                End If

                MessageBox.Show($"Review {status} successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadReviews() ' Refresh the grid
            Else
                MessageBox.Show("Failed to update review status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show("Error updating review: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' Increment Customer Feedback Count
    Private Sub IncrementFeedbackCount(reviewId As Integer)
        Try
            Dim query As String = "UPDATE customers c
                                  INNER JOIN customer_reviews cr ON c.CustomerID = cr.CustomerID
                                  SET c.FeedbackCount = c.FeedbackCount + 1
                                  WHERE cr.ReviewID = @reviewId"

            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@reviewId", reviewId)
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            ' Silent fail - not critical
        End Try
    End Sub

    ' Delete Review
    Private Sub DeleteReview(reviewId As Integer)
        Try
            If MessageBox.Show("Are you sure you want to delete this review? This action cannot be undone.",
                              "Confirm Deletion",
                              MessageBoxButtons.YesNo,
                              MessageBoxIcon.Warning) = DialogResult.No Then
                Return
            End If

            conn.Open()

            Dim query As String = "DELETE FROM customer_reviews WHERE ReviewID = @reviewId"
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@reviewId", reviewId)

            Dim result As Integer = cmd.ExecuteNonQuery()

            If result > 0 Then
                MessageBox.Show("Review deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadReviews()
            Else
                MessageBox.Show("Failed to delete review.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show("Error deleting review: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' View Review Details
    Private Sub ViewReviewDetails(reviewId As Integer)
        Try
            conn.Open()

            Dim query As String = "SELECT 
                cr.*,
                CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
                c.Email
                FROM customer_reviews cr
                INNER JOIN customers c ON cr.CustomerID = c.CustomerID
                WHERE cr.ReviewID = @reviewId"

            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@reviewId", reviewId)

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                Dim details As String = $"Review Details:" & vbCrLf & vbCrLf &
                                       $"Review ID: {reader("ReviewID")}" & vbCrLf &
                                       $"Customer: {reader("CustomerName")}" & vbCrLf &
                                       $"Email: {reader("Email")}" & vbCrLf & vbCrLf &
                                       $"Overall Rating: {reader("OverallRating")}/5" & vbCrLf &
                                       $"Food Taste: {If(IsDBNull(reader("FoodTasteRating")), "N/A", reader("FoodTasteRating").ToString())}" & vbCrLf &
                                       $"Portion Size: {If(IsDBNull(reader("PortionSizeRating")), "N/A", reader("PortionSizeRating").ToString())}" & vbCrLf &
                                       $"Service: {If(IsDBNull(reader("CustomerServiceRating")), "N/A", reader("CustomerServiceRating").ToString())}" & vbCrLf &
                                       $"Ambience: {If(IsDBNull(reader("AmbienceRating")), "N/A", reader("AmbienceRating").ToString())}" & vbCrLf &
                                       $"Cleanliness: {If(IsDBNull(reader("CleanlinessRating")), "N/A", reader("CleanlinessRating").ToString())}" & vbCrLf & vbCrLf &
                                       $"Comments:" & vbCrLf &
                                       $"General: {If(IsDBNull(reader("GeneralComment")), "None", reader("GeneralComment").ToString())}" & vbCrLf &
                                       $"Food: {If(IsDBNull(reader("FoodTasteComment")), "None", reader("FoodTasteComment").ToString())}" & vbCrLf &
                                       $"Service: {If(IsDBNull(reader("CustomerServiceComment")), "None", reader("CustomerServiceComment").ToString())}" & vbCrLf & vbCrLf &
                                       $"Status: {reader("Status")}" & vbCrLf &
                                       $"Created: {reader("CreatedDate")}"

                MessageBox.Show(details, "Review Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            reader.Close()

        Catch ex As Exception
            MessageBox.Show("Error viewing details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' Search Reviews
    Private Sub SearchReviews(searchTerm As String)
        Try
            conn.Open()

            Dim query As String = "SELECT 
                cr.ReviewID,
                cr.CustomerID,
                CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
                cr.OverallRating,
                cr.FoodTasteRating,
                cr.PortionSizeRating,
                cr.CustomerServiceRating,
                cr.AmbienceRating,
                cr.CleanlinessRating,
                cr.GeneralComment,
                cr.Status,
                cr.CreatedDate,
                cr.ApprovedDate
                FROM customer_reviews cr
                INNER JOIN customers c ON cr.CustomerID = c.CustomerID
                WHERE CONCAT(c.FirstName, ' ', c.LastName) LIKE @search
                OR cr.GeneralComment LIKE @search
                OR cr.Status LIKE @search
                ORDER BY cr.CreatedDate DESC"

            adapter = New MySqlDataAdapter(query, conn)
            adapter.SelectCommand.Parameters.AddWithValue("@search", "%" & searchTerm & "%")

            dt = New DataTable()
            adapter.Fill(dt)

            DataGridView1.DataSource = dt
            FormatColumns()

            lblTotalReviews.Text = $"Found: {dt.Rows.Count} reviews"

        Catch ex As Exception
            MessageBox.Show("Error searching: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' Load Reservations
    Private Sub LoadReservations()
        Try
            conn.Open()

            Dim query As String = "SELECT 
                r.ReservationID,
                CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
                r.EventType,
                r.EventDate,
                r.EventTime,
                r.NumberOfGuests,
                r.ReservationStatus,
                r.ReservationDate
                FROM reservations r
                INNER JOIN customers c ON r.CustomerID = c.CustomerID
                ORDER BY r.EventDate DESC"

            Dim dtReservations As New DataTable()
            Dim adapterRes As New MySqlDataAdapter(query, conn)
            adapterRes.Fill(dtReservations)

            ' Assuming you have a separate DataGridView for reservations
            ' DataGridViewReservations.DataSource = dtReservations

        Catch ex As Exception
            MessageBox.Show("Error loading reservations: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' Button Event Handlers
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadReviews()
    End Sub

    Private Sub btnViewPending_Click(sender As Object, e As EventArgs) Handles btnViewPending.Click
        LoadReviews("Pending")
    End Sub

    Private Sub btnViewApproved_Click(sender As Object, e As EventArgs) Handles btnViewApproved.Click
        LoadReviews("Approved")
    End Sub

    Private Sub btnViewRejected_Click(sender As Object, e As EventArgs) Handles btnViewRejected.Click
        LoadReviews("Rejected")
    End Sub

    Private Sub btnViewAll_Click(sender As Object, e As EventArgs) Handles btnViewAll.Click
        LoadReviews()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If txtSearch.Text.Trim() <> "" Then
            SearchReviews(txtSearch.Text.Trim())
        Else
            LoadReviews()
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim reviewId As Integer = Convert.ToInt32(DataGridView1.SelectedRows(0).Cells("ReviewID").Value)
            DeleteReview(reviewId)
        Else
            MessageBox.Show("Please select a review to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnViewDetails_Click(sender As Object, e As EventArgs) Handles btnViewDetails.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim reviewId As Integer = Convert.ToInt32(DataGridView1.SelectedRows(0).Cells("ReviewID").Value)
            ViewReviewDetails(reviewId)
        Else
            MessageBox.Show("Please select a review to view details.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' Export to CSV (Optional)
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Try
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv"
            saveFileDialog.FileName = $"Reviews_Export_{DateTime.Now:yyyyMMdd}.csv"

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                Using writer As New IO.StreamWriter(saveFileDialog.FileName)
                    ' Write headers
                    For i As Integer = 0 To dt.Columns.Count - 1
                        writer.Write(dt.Columns(i).ColumnName)
                        If i < dt.Columns.Count - 1 Then writer.Write(",")
                    Next
                    writer.WriteLine()

                    ' Write data
                    For Each row As DataRow In dt.Rows
                        For i As Integer = 0 To dt.Columns.Count - 1
                            writer.Write(row(i).ToString().Replace(",", ";"))
                            If i < dt.Columns.Count - 1 Then writer.Write(",")
                        Next
                        writer.WriteLine()
                    Next
                End Using

                MessageBox.Show("Reviews exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Export error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class

