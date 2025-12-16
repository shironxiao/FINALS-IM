Imports MySqlConnector

Public Class UsersAccounts
    ' Pagination variables
    Private currentPage As Integer = 1
    Private pageSize As Integer = 20
    Private totalRecords As Integer = 0
    Private totalPages As Integer = 0
    Private allStaffData As DataTable
    Private initialLoadComplete As Boolean = False

    Private Sub UsersAccounts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeDataGridView()
        RoundButton(Adduserbtn)
        RoundPaginationButtons()
        LoadStaffData()
        initialLoadComplete = True
        AdjustControlsToScreen()
    End Sub

    Private Sub UsersAccounts_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If initialLoadComplete Then
            AdjustControlsToScreen()
        End If
    End Sub

    Private Sub AdjustControlsToScreen()
        Try
            ' Get the form's client area dimensions
            Dim formWidth As Integer = Me.ClientSize.Width
            Dim formHeight As Integer = Me.ClientSize.Height

            ' Set margins
            Dim leftMargin As Integer = 30
            Dim rightMargin As Integer = 30
            Dim topMargin As Integer = 30

            ' Title Label
            Label1.Location = New Point(leftMargin, topMargin)

            ' Stats Card
            RoundedPane22.Location = New Point(leftMargin, Label1.Bottom + 20)
            Dim statsCardWidth As Integer = Math.Min(300, (formWidth - leftMargin - rightMargin) \ 3)
            RoundedPane22.Width = statsCardWidth

            ' Add User Button (aligned to right)
            Adduserbtn.Location = New Point(formWidth - rightMargin - Adduserbtn.Width, RoundedPane22.Top + 10)

            ' DataGridView - Calculate available space
            Dim gridTop As Integer = RoundedPane22.Bottom + 20
            Dim gridWidth As Integer = formWidth - leftMargin - rightMargin
            Dim paginationHeight As Integer = 60
            Dim gridHeight As Integer = formHeight - gridTop - paginationHeight - 20

            UsersAccountData.Location = New Point(leftMargin, gridTop)
            UsersAccountData.Size = New Size(gridWidth, gridHeight)

            ' Adjust DataGridView column widths proportionally
            AdjustColumnWidths()

            ' Pagination Panel
            PaginationPanel.Location = New Point(leftMargin, UsersAccountData.Bottom + 10)
            PaginationPanel.Width = gridWidth

            ' Center pagination controls
            CenterPaginationControls()

        Catch ex As Exception
            ' Silently handle resize errors to prevent crashes
            Debug.WriteLine("Resize error: " & ex.Message)
        End Try
    End Sub

    Private Sub AdjustColumnWidths()
        Try
            If UsersAccountData.Columns.Count = 0 Then Return

            Dim totalWidth As Integer = UsersAccountData.Width - 20 ' Account for scrollbar
            Dim actionColumnWidth As Integer = 90

            ' Calculate available width for data columns
            Dim availableWidth As Integer = totalWidth - (actionColumnWidth * 2) ' 2 action columns

            ' Set column widths proportionally
            UsersAccountData.Columns("txtName").Width = CInt(availableWidth * 0.35) ' 35%
            UsersAccountData.Columns("colRole").Width = CInt(availableWidth * 0.3) ' 30%
            UsersAccountData.Columns("colStatus").Width = CInt(availableWidth * 0.15) ' 15%
            UsersAccountData.Columns("colJoinDate").Width = CInt(availableWidth * 0.2) ' 20%
            UsersAccountData.Columns("colEdit").Width = actionColumnWidth
            UsersAccountData.Columns("colDelete").Width = actionColumnWidth

        Catch ex As Exception
            Debug.WriteLine("Column width adjustment error: " & ex.Message)
        End Try
    End Sub

    Private Sub CenterPaginationControls()
        Try
            Dim panelWidth As Integer = PaginationPanel.Width
            Dim totalButtonWidth As Integer = btnFirstPage.Width + btnPreviousPage.Width +
                                              btnNextPage.Width + btnLastPage.Width
            Dim spacing As Integer = 10
            Dim labelWidth As Integer = 100

            Dim totalWidth As Integer = totalButtonWidth + (spacing * 3) + labelWidth
            Dim startX As Integer = (panelWidth - totalWidth) \ 2

            btnFirstPage.Location = New Point(startX, btnFirstPage.Top)
            btnPreviousPage.Location = New Point(btnFirstPage.Right + spacing, btnPreviousPage.Top)
            lblPageInfo.Location = New Point(btnPreviousPage.Right + spacing, lblPageInfo.Top)
            lblPageInfo.Width = labelWidth
            btnNextPage.Location = New Point(lblPageInfo.Right + spacing, btnNextPage.Top)
            btnLastPage.Location = New Point(btnNextPage.Right + spacing, btnLastPage.Top)

        Catch ex As Exception
            Debug.WriteLine("Pagination centering error: " & ex.Message)
        End Try
    End Sub

    Private Sub InitializeDataGridView()
        ' Enable double buffering for smoother rendering
        UsersAccountData.DoubleBuffered(True)
        UsersAccountData.SuspendLayout()
        UsersAccountData.Rows.Clear()
        UsersAccountData.ResumeLayout()

        ' Set alternating row colors for better readability
        UsersAccountData.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 252, 255)

        ' Remove selection highlighting on focus
        UsersAccountData.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 244, 250)
        UsersAccountData.DefaultCellStyle.SelectionForeColor = Color.Black
    End Sub

    Private Sub LoadStaffData()
        Try
            openConn()

            ' Load only staff members with improved query
            Dim query As String = "
                SELECT 
                    EmployeeID as ID,
                    CONCAT(FirstName, ' ', LastName) as FullName,
                    Position as Role,
                    EmploymentStatus as Status,
                    HireDate as DateCreated
                FROM employee
                WHERE Position LIKE '%Staff%'
                ORDER BY HireDate DESC"

            Dim cmd As New MySqlCommand(query, conn)
            Dim adapter As New MySqlDataAdapter(cmd)
            allStaffData = New DataTable()
            adapter.Fill(allStaffData)

            totalRecords = allStaffData.Rows.Count
            totalPages = If(totalRecords > 0, Math.Ceiling(totalRecords / pageSize), 1)

            ' Update staff count
            lblStaffs.Text = totalRecords.ToString()

            ' Load first page
            If totalRecords > 0 Then
                LoadPage(1)
            Else
                UsersAccountData.Rows.Clear()
                UpdatePaginationControls()
            End If

        Catch ex As MySqlException
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Error loading staff data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    Private Sub LoadPage(pageNumber As Integer)
        If allStaffData Is Nothing OrElse allStaffData.Rows.Count = 0 Then
            UpdatePaginationControls()
            Return
        End If

        ' Validate page number
        If pageNumber < 1 Then pageNumber = 1
        If pageNumber > totalPages Then pageNumber = totalPages

        currentPage = pageNumber
        Dim startIndex As Integer = (currentPage - 1) * pageSize
        Dim endIndex As Integer = Math.Min(startIndex + pageSize, totalRecords)

        ' Suspend layout for smoother loading
        UsersAccountData.SuspendLayout()
        UsersAccountData.Rows.Clear()

        Try
            ' Use bulk operation for better performance
            For i As Integer = startIndex To endIndex - 1
                Dim row As DataRow = allStaffData.Rows(i)

                ' Get full name
                Dim fullName As String = If(row("FullName") IsNot DBNull.Value, row("FullName").ToString().Trim(), "N/A")

                ' Get role
                Dim role As String = If(row("Role") IsNot DBNull.Value, row("Role").ToString(), "N/A")

                ' Get status
                Dim status As String = If(row("Status") IsNot DBNull.Value, row("Status").ToString(), "N/A")

                ' Format join date
                Dim joinDate As String = "N/A"
                If row("DateCreated") IsNot DBNull.Value Then
                    Try
                        joinDate = Convert.ToDateTime(row("DateCreated")).ToString("MMMM dd, yyyy")
                    Catch
                        joinDate = row("DateCreated").ToString()
                    End Try
                End If

                ' Add row to DataGridView
                Dim rowIndex As Integer = UsersAccountData.Rows.Add()
                Dim newRow As DataGridViewRow = UsersAccountData.Rows(rowIndex)

                newRow.Cells("txtName").Value = fullName
                newRow.Cells("colRole").Value = role
                newRow.Cells("colStatus").Value = status
                newRow.Cells("colJoinDate").Value = joinDate

                ' Store ID for edit/delete operations
                newRow.Tag = If(row("ID") IsNot DBNull.Value, Convert.ToInt32(row("ID")), 0)
            Next

        Catch ex As Exception
            MessageBox.Show("Error displaying data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            UsersAccountData.ResumeLayout()
            UpdatePaginationControls()
        End Try
    End Sub

    Private Sub UpdatePaginationControls()
        ' Update page info label
        lblPageInfo.Text = $"Page {currentPage} of {totalPages}"

        ' Enable/disable buttons based on current page
        btnFirstPage.Enabled = (currentPage > 1)
        btnPreviousPage.Enabled = (currentPage > 1)
        btnNextPage.Enabled = (currentPage < totalPages)
        btnLastPage.Enabled = (currentPage < totalPages)

        ' Visual feedback for disabled buttons
        btnFirstPage.BackColor = If(btnFirstPage.Enabled, Color.FromArgb(240, 244, 250), Color.FromArgb(230, 230, 230))
        btnPreviousPage.BackColor = If(btnPreviousPage.Enabled, Color.FromArgb(240, 244, 250), Color.FromArgb(230, 230, 230))
        btnNextPage.BackColor = If(btnNextPage.Enabled, Color.FromArgb(240, 244, 250), Color.FromArgb(230, 230, 230))
        btnLastPage.BackColor = If(btnLastPage.Enabled, Color.FromArgb(240, 244, 250), Color.FromArgb(230, 230, 230))
    End Sub

    Private Sub UsersAccountData_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles UsersAccountData.CellContentClick
        If e.RowIndex < 0 Then Return

        Dim selectedRow As DataGridViewRow = UsersAccountData.Rows(e.RowIndex)
        Dim username As String = If(selectedRow.Cells("txtName").Value IsNot Nothing,
                                    selectedRow.Cells("txtName").Value.ToString(), "Unknown")

        Dim userID As Integer = If(selectedRow.Tag IsNot Nothing, CInt(selectedRow.Tag), 0)

        If userID = 0 Then
            MessageBox.Show("Invalid user ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' EDIT BUTTON
        If e.ColumnIndex = UsersAccountData.Columns("colEdit").Index Then
            MessageBox.Show($"Edit functionality for {username} (ID: {userID}) coming soon!",
                          "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' DELETE BUTTON
        ElseIf e.ColumnIndex = UsersAccountData.Columns("colDelete").Index Then
            Dim result As DialogResult = MessageBox.Show(
                $"Are you sure you want to delete {username}?{vbNewLine}{vbNewLine}This action cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            )

            If result = DialogResult.Yes Then
                DeleteStaffMember(userID, username)
            End If
        End If
    End Sub

    Private Sub DeleteStaffMember(userID As Integer, username As String)
        Try
            openConn()
            Dim query As String = "DELETE FROM employee WHERE EmployeeID = @id"
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@id", userID)

            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

            If rowsAffected > 0 Then
                MessageBox.Show($"{username} has been deleted successfully.",
                              "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadStaffData() ' Reload data
            Else
                MessageBox.Show("No records were deleted.",
                              "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As MySqlException
            MessageBox.Show($"Database error while deleting staff member: {ex.Message}",
                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show($"Error deleting staff member: {ex.Message}",
                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    ' Pagination button handlers
    Private Sub btnFirstPage_Click(sender As Object, e As EventArgs) Handles btnFirstPage.Click
        If currentPage > 1 Then
            LoadPage(1)
        End If
    End Sub

    Private Sub btnPreviousPage_Click(sender As Object, e As EventArgs) Handles btnPreviousPage.Click
        If currentPage > 1 Then
            LoadPage(currentPage - 1)
        End If
    End Sub

    Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        If currentPage < totalPages Then
            LoadPage(currentPage + 1)
        End If
    End Sub

    Private Sub btnLastPage_Click(sender As Object, e As EventArgs) Handles btnLastPage.Click
        If currentPage < totalPages Then
            LoadPage(totalPages)
        End If
    End Sub
    Private Sub Adduserbtn_Click(sender As Object, e As EventArgs) Handles Adduserbtn.Click
        Dim addUserForm As New FormAddUser()
        addUserForm.StartPosition = FormStartPosition.CenterScreen

        If addUserForm.ShowDialog() = DialogResult.OK Then
            ' Refresh data after adding new staff
            LoadStaffData()
        End If
    End Sub

    ' UI Helper Methods
    Private Sub RoundButton(btn As Button)
        Dim radius As Integer = 10
        Dim path As New Drawing2D.GraphicsPath()
        path.StartFigure()
        path.AddArc(New Rectangle(0, 0, radius, radius), 180, 90)
        path.AddArc(New Rectangle(btn.Width - radius, 0, radius, radius), 270, 90)
        path.AddArc(New Rectangle(btn.Width - radius, btn.Height - radius, radius, radius), 0, 90)
        path.AddArc(New Rectangle(0, btn.Height - radius, radius, radius), 90, 90)
        path.CloseFigure()
        btn.Region = New Region(path)
    End Sub

    Private Sub RoundPaginationButtons()
        RoundButton(btnFirstPage)
        RoundButton(btnPreviousPage)
        RoundButton(btnNextPage)
        RoundButton(btnLastPage)
    End Sub

    ' Public methods for external refresh
    Public Sub RefreshData()
        LoadStaffData()
    End Sub

    Public Sub LoadUsers()
        LoadStaffData()
    End Sub
End Class

' Extension module for DataGridView double buffering
Module DataGridViewExtensions
    <System.Runtime.CompilerServices.Extension()>
    Public Sub DoubleBuffered(ByVal dgv As DataGridView, ByVal setting As Boolean)
        Dim dgvType As Type = dgv.GetType()
        Dim pi As Reflection.PropertyInfo = dgvType.GetProperty("DoubleBuffered",
            Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
        If pi IsNot Nothing Then
            pi.SetValue(dgv, setting, Nothing)
        End If
    End Sub
End Module