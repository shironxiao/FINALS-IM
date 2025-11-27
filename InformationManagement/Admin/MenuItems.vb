Imports MySqlConnector
Imports System.IO

Public Class MenuItems

    ' Track if buttons already added
    Dim ButtonsAdded As Boolean = False

    Private Sub MenuItems_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCategories()
        LoadMenuItems()
        InitializeSearchBox()
    End Sub

    ' =======================================================
    ' LOAD CATEGORIES
    ' =======================================================
    Private Sub LoadCategories()
        Category.Items.Clear()
        Category.Items.Add("All Categories")
        Category.Items.AddRange(New String() {
            "SPAGHETTI MEAL",
            "DESSERT",
            "DRINKS & BEVERAGES",
            "PLATTER",
            "RICE MEAL",
            "RICE",
            "Bilao",
            "Snacks"
        })
        Category.SelectedIndex = 0
    End Sub

    ' =======================================================
    ' INITIALIZE SEARCH BOX
    ' =======================================================
    Private Sub InitializeSearchBox()
        txtSearch.ForeColor = Color.Gray
        txtSearch.Text = "Search menu items..."
    End Sub

    ' =======================================================
    ' LOAD DATA
    ' =======================================================
    ' =======================================================
    ' LOAD DATA - FIXED VERSION
    ' =======================================================
    Private Sub LoadMenuItems(Optional searchTerm As String = "", Optional categoryFilter As String = "All Categories")

        Dim query As String = "
        SELECT 
            Image,
            ProductID, ProductName, Category, Description, Price,
            Availability, ServingSize, DateAdded, LastUpdated,
            ProductCode, OrderCount, PrepTime, PopularityTag, MealTime
        FROM products
        WHERE 1=1
    "

        ' Add search filter
        If Not String.IsNullOrWhiteSpace(searchTerm) AndAlso searchTerm <> "Search menu items..." Then
            query &= " AND (ProductName LIKE @search OR ProductCode LIKE @search OR Description LIKE @search)"
        End If

        ' Add category filter
        If categoryFilter <> "All Categories" Then
            query &= " AND Category = @category"
        End If

        ' Order by ProductID ascending (lowest to highest)
        query &= " ORDER BY ProductID ASC"

        Try
            openConn()

            Dim cmd As New MySqlCommand(query, conn)

            If Not String.IsNullOrWhiteSpace(searchTerm) AndAlso searchTerm <> "Search menu items..." Then
                cmd.Parameters.AddWithValue("@search", "%" & searchTerm & "%")
            End If

            ' FIXED: Map "Bilao" display label to "NOODLES & PASTA" database value
            If categoryFilter <> "All Categories" Then
                Dim actualCategory As String = categoryFilter

                ' Map display labels to actual database category values
                If categoryFilter = "Bilao" Then
                    actualCategory = "NOODLES & PASTA"
                End If

                cmd.Parameters.AddWithValue("@category", actualCategory)
            End If

            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            ' ---------------------------------------------------
            ' FIX IMAGE COLUMN
            ' ---------------------------------------------------
            If dt.Columns.Contains("Image") Then
                dt.Columns("Image").ColumnName = "ImageBlob"
                dt.Columns.Add("Image", GetType(Image))

                For Each row As DataRow In dt.Rows
                    If IsDBNull(row("ImageBlob")) OrElse Not TypeOf row("ImageBlob") Is Byte() Then
                        row("Image") = Nothing
                        Continue For
                    End If

                    Try
                        Dim imgBytes As Byte() = CType(row("ImageBlob"), Byte())
                        Using ms As New MemoryStream(imgBytes)
                            row("Image") = Image.FromStream(ms)
                        End Using
                    Catch
                        row("Image") = Nothing
                    End Try
                Next
            End If

            ' Remove ImageBlob column before binding
            If dt.Columns.Contains("ImageBlob") Then
                dt.Columns.Remove("ImageBlob")
            End If

            DataGridMenu.AutoGenerateColumns = True
            DataGridMenu.DataSource = dt

            ' BUTTONS ADDED ONLY ONCE
            If Not ButtonsAdded Then
                AddActionButtons()
                ButtonsAdded = True
            End If

            ' REORDER COLUMNS - MOVE ACTION BUTTONS TO THE END
            ReorderColumns()

            ' FORMAT IMAGE COLUMN
            If DataGridMenu.Columns.Contains("Image") Then
                Dim imgCol As DataGridViewImageColumn =
                CType(DataGridMenu.Columns("Image"), DataGridViewImageColumn)
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom
                imgCol.Width = 120
                imgCol.DisplayIndex = 0
            End If

            ' Format other columns
            FormatColumns()

            ' Update total items count
            lblTotalItems.Text = $"Total Items: {dt.Rows.Count}"

        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            conn.Close()
        End Try

    End Sub
    ' =======================================================
    ' FORMAT COLUMNS
    ' =======================================================
    Private Sub FormatColumns()
        With DataGridMenu
            If .Columns.Contains("ProductID") Then .Columns("ProductID").Width = 80
            If .Columns.Contains("ProductName") Then .Columns("ProductName").Width = 200
            If .Columns.Contains("Category") Then .Columns("Category").Width = 120
            If .Columns.Contains("Description") Then .Columns("Description").Width = 250
            If .Columns.Contains("Price") Then
                .Columns("Price").Width = 100
                .Columns("Price").DefaultCellStyle.Format = "₱#,##0.00"
            End If
            If .Columns.Contains("Availability") Then .Columns("Availability").Width = 100
            If .Columns.Contains("ServingSize") Then .Columns("ServingSize").Width = 100
            If .Columns.Contains("PrepTime") Then .Columns("PrepTime").Width = 100
            If .Columns.Contains("PopularityTag") Then .Columns("PopularityTag").Width = 120
            If .Columns.Contains("MealTime") Then .Columns("MealTime").Width = 100
            If .Columns.Contains("OrderCount") Then .Columns("OrderCount").Width = 90
            If .Columns.Contains("ProductCode") Then .Columns("ProductCode").Width = 120
        End With
    End Sub

    ' =======================================================
    ' REORDER COLUMNS - MOVE ACTION BUTTONS TO END
    ' =======================================================
    Private Sub ReorderColumns()
        With DataGridMenu
            ' Ensure action buttons are always at the end
            If .Columns.Contains("ViewButton") Then
                .Columns("ViewButton").DisplayIndex = .Columns.Count - 3
            End If
            If .Columns.Contains("UpdateButton") Then
                .Columns("UpdateButton").DisplayIndex = .Columns.Count - 2
            End If
            If .Columns.Contains("DeleteButton") Then
                .Columns("DeleteButton").DisplayIndex = .Columns.Count - 1
            End If
        End With
    End Sub

    ' =======================================================
    ' ADD UPDATE + DELETE BUTTONS
    ' =======================================================
    Private Sub AddActionButtons()

        ' VIEW BUTTON
        Dim viewBtn As New DataGridViewButtonColumn()
        viewBtn.HeaderText = "View"
        viewBtn.Text = "👁️ View"
        viewBtn.Name = "ViewButton"
        viewBtn.UseColumnTextForButtonValue = True
        viewBtn.Width = 80
        DataGridMenu.Columns.Add(viewBtn)

        ' EDIT BUTTON
        Dim updateBtn As New DataGridViewButtonColumn()
        updateBtn.HeaderText = "Edit"
        updateBtn.Text = "✏️ Edit"
        updateBtn.Name = "UpdateButton"
        updateBtn.UseColumnTextForButtonValue = True
        updateBtn.Width = 80
        DataGridMenu.Columns.Add(updateBtn)

        ' DELETE BUTTON
        Dim deleteBtn As New DataGridViewButtonColumn()
        deleteBtn.HeaderText = "Delete"
        deleteBtn.Text = "🗑️ Delete"
        deleteBtn.Name = "DeleteButton"
        deleteBtn.UseColumnTextForButtonValue = True
        deleteBtn.Width = 90
        DataGridMenu.Columns.Add(deleteBtn)

    End Sub

    ' =======================================================
    ' BUTTON HANDLERS
    ' =======================================================
    Private Sub DataGridMenu_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridMenu.CellContentClick

        If e.RowIndex < 0 Then Exit Sub

        Dim id As Integer = DataGridMenu.Rows(e.RowIndex).Cells("ProductID").Value

        ' VIEW
        If DataGridMenu.Columns(e.ColumnIndex).Name = "ViewButton" Then
            ViewProduct(id)
            Exit Sub
        End If

        ' EDIT
        If DataGridMenu.Columns(e.ColumnIndex).Name = "UpdateButton" Then
            Dim form As New FormEditMenu()
            form.LoadProduct(id)
            If form.ShowDialog() = DialogResult.OK Then
                LoadMenuItems(txtSearch.Text, Category.Text)
            End If
            Exit Sub
        End If

        ' DELETE
        If DataGridMenu.Columns(e.ColumnIndex).Name = "DeleteButton" Then
            DeleteProduct(id)
            LoadMenuItems(txtSearch.Text, Category.Text)
            Exit Sub
        End If

    End Sub

    ' =======================================================
    ' VIEW PRODUCT DETAILS
    ' =======================================================
    Private Sub ViewProduct(productId As Integer)
        Try
            openConn()
            Dim query As String = "SELECT * FROM products WHERE ProductID = @id"
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@id", productId)

            Dim reader As MySqlDataReader = cmd.ExecuteReader()
            If reader.Read() Then
                Dim details As String = $"Product ID: {reader("ProductID")}" & vbCrLf &
                                       $"Product Name: {reader("ProductName")}" & vbCrLf &
                                       $"Category: {reader("Category")}" & vbCrLf &
                                       $"Description: {reader("Description")}" & vbCrLf &
                                       $"Price: ₱{Convert.ToDecimal(reader("Price")):N2}" & vbCrLf &
                                       $"Availability: {reader("Availability")}" & vbCrLf &
                                       $"Serving Size: {reader("ServingSize")}" & vbCrLf &
                                       $"Prep Time: {reader("PrepTime")}" & vbCrLf &
                                       $"Product Code: {reader("ProductCode")}" & vbCrLf &
                                       $"Order Count: {reader("OrderCount")}" & vbCrLf &
                                       $"Date Added: {reader("DateAdded")}"

                MessageBox.Show(details, "Product Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error viewing product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

    ' =======================================================
    ' DELETE FUNCTION
    ' =======================================================
    Private Sub DeleteProduct(productId As Integer)

        Dim query As String = "DELETE FROM products WHERE ProductID = @id"

        Try
            openConn()

            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@id", productId)

            If MessageBox.Show(
                "Are you sure you want to delete this product?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            ) = DialogResult.Yes Then

                cmd.ExecuteNonQuery()
                MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error deleting: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            conn.Close()
        End Try

    End Sub

    ' =======================================================
    ' SEARCH FUNCTIONALITY
    ' =======================================================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If txtSearch.ForeColor = Color.Black Then
            LoadMenuItems(txtSearch.Text, Category.Text)
        End If
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        If txtSearch.Text = "Search menu items..." Then
            txtSearch.Text = ""
            txtSearch.ForeColor = Color.Black
        End If
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        If String.IsNullOrWhiteSpace(txtSearch.Text) Then
            txtSearch.Text = "Search menu items..."
            txtSearch.ForeColor = Color.Gray
        End If
    End Sub

    ' =======================================================
    ' CATEGORY FILTER
    ' =======================================================
    Private Sub Category_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Category.SelectedIndexChanged
        LoadMenuItems(txtSearch.Text, Category.Text)
    End Sub

    ' =======================================================
    ' BUTTON EVENTS
    ' =======================================================
    Private Sub AddMenuItemsbtn_Click(sender As Object, e As EventArgs) Handles AddMenuItemsbtn.Click
        With FormAddNewmenuItem
            .StartPosition = FormStartPosition.CenterScreen
            If .ShowDialog() = DialogResult.OK Then
                LoadMenuItems(txtSearch.Text, Category.Text)
            End If
        End With
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        txtSearch.Text = "Search menu items..."
        txtSearch.ForeColor = Color.Gray
        Category.SelectedIndex = 0
        LoadMenuItems()
        MessageBox.Show("Data refreshed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnToggleAvailability_Click(sender As Object, e As EventArgs) Handles btnToggleAvailability.Click
        If DataGridMenu.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a product to toggle availability.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim productId As Integer = Convert.ToInt32(DataGridMenu.SelectedRows(0).Cells("ProductID").Value)

        Try
            openConn()

            ' Get current value from DB
            Dim cmdGet As New MySqlCommand("SELECT Availability FROM products WHERE ProductID = @id", conn)
            cmdGet.Parameters.AddWithValue("@id", productId)
            Dim current As String = If(cmdGet.ExecuteScalar(), "").ToString()

            ' Toggle correctly based on your ENUM values
            Dim newStatus As String = If(current = "Available", "Not Available", "Available")

            ' Update database
            Dim cmdUpdate As New MySqlCommand("UPDATE products SET Availability = @status, LastUpdated = NOW() WHERE ProductID = @id", conn)
            cmdUpdate.Parameters.AddWithValue("@status", newStatus)
            cmdUpdate.Parameters.AddWithValue("@id", productId)
            cmdUpdate.ExecuteNonQuery()

            MessageBox.Show($"Availability changed to: {newStatus}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Force full refresh
            DataGridMenu.DataSource = Nothing
            LoadMenuItems(txtSearch.Text, Category.Text)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Try
            Dim saveDialog As New SaveFileDialog()
            saveDialog.Filter = "CSV files (*.csv)|*.csv"
            saveDialog.FileName = $"MenuItems_{DateTime.Now:yyyyMMdd_HHmmss}.csv"

            If saveDialog.ShowDialog() = DialogResult.OK Then
                Dim csv As New System.Text.StringBuilder()

                ' Add headers
                Dim headers As New List(Of String)
                For Each col As DataGridViewColumn In DataGridMenu.Columns
                    If col.Name <> "ViewButton" And col.Name <> "UpdateButton" And col.Name <> "DeleteButton" And col.Name <> "Image" Then
                        headers.Add(col.HeaderText)
                    End If
                Next
                csv.AppendLine(String.Join(",", headers))

                ' Add rows
                For Each row As DataGridViewRow In DataGridMenu.Rows
                    Dim values As New List(Of String)
                    For Each col As DataGridViewColumn In DataGridMenu.Columns
                        If col.Name <> "ViewButton" And col.Name <> "UpdateButton" And col.Name <> "DeleteButton" And col.Name <> "Image" Then
                            Dim value As String = If(row.Cells(col.Name).Value?.ToString(), "")
                            values.Add($"""{value}""")
                        End If
                    Next
                    csv.AppendLine(String.Join(",", values))
                Next

                System.IO.File.WriteAllText(saveDialog.FileName, csv.ToString())
                MessageBox.Show("Data exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error exporting data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DataGridMenu_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridMenu.DataBindingComplete
        If DataGridMenu.Columns.Contains("ProductID") Then
            DataGridMenu.Columns("ProductID").Visible = False
            DataGridMenu.Columns("ProductID").Width = 0
        End If
    End Sub

End Class