Imports MySqlConnector
Imports System.IO

Public Class FormAddNewmenuItem

    Private Sub FormAddNewmenuItem_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeForm()
    End Sub

    ' =======================================================
    ' INITIALIZE FORM
    ' =======================================================
    Private Sub InitializeForm()
        ' Clear and load Availability options
        Availability.Items.Clear()
        Availability.Items.Add("Available")
        Availability.Items.Add("Unavailable")
        Availability.SelectedIndex = 0

        ' Clear and load Category options
        cmbCategory.Items.Clear()
        cmbCategory.Items.Add("SPAGHETTI MEAL")
        cmbCategory.Items.Add("DESSERT")
        cmbCategory.Items.Add("DRINKS & BEVERAGES")
        cmbCategory.Items.Add("PLATTER")
        cmbCategory.Items.Add("RICE MEAL")
        cmbCategory.Items.Add("RICE")
        cmbCategory.Items.Add("Bilao")
        cmbCategory.Items.Add("Snacks")
        cmbCategory.SelectedIndex = -1

        ' Clear and load MealTime options
        cmbMealTime.Items.Clear()
        cmbMealTime.Items.Add("All Day")
        cmbMealTime.Items.Add("Breakfast")
        cmbMealTime.Items.Add("Lunch")
        cmbMealTime.Items.Add("Dinner")
        cmbMealTime.SelectedIndex = 0  ' Default to "All Day"

        ' Set default values
        numericPrice.Value = 0
        numericPrice.DecimalPlaces = 2
        numericPrice.Maximum = 999999

        ' Generate next Product ID
        ProductID.Text = GenerateNextProductID()
        ProductID.ReadOnly = True
        ProductID.BackColor = Color.LightGray
        ProductID.ForeColor = Color.Gray

        ' Set date picker to today
        DateTimePicker1.Value = DateTime.Now
        DateTimePicker1.Enabled = False

        ' Set default order count
        OrderCount.Text = "0"
        OrderCount.ReadOnly = True
        OrderCount.BackColor = Color.LightGray
        OrderCount.ForeColor = Color.Gray
    End Sub

    ' =======================================================
    ' GENERATE NEXT PRODUCT ID
    ' =======================================================
    Private Function GenerateNextProductID() As String
        Try
            openConn()
            Dim query As String = "SELECT COALESCE(MAX(ProductID), 0) + 1 AS NextID FROM products"
            Dim cmd As New MySqlCommand(query, conn)
            Dim result = cmd.ExecuteScalar()
            Return result.ToString()
        Catch ex As Exception
            Return "1"
        Finally
            conn.Close()
        End Try
    End Function

    ' =======================================================
    ' VALIDATE FORM - Product Code is OPTIONAL
    ' =======================================================
    Private Function ValidateForm() As Boolean
        If String.IsNullOrWhiteSpace(txtProductName.Text) Then
            MessageBox.Show("Please enter a product name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtProductName.Focus()
            Return False
        End If

        If cmbCategory.SelectedIndex = -1 OrElse String.IsNullOrWhiteSpace(cmbCategory.Text) Then
            MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbCategory.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(Description.Text) Then
            MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Description.Focus()
            Return False
        End If

        If numericPrice.Value <= 0 Then
            MessageBox.Show("Please enter a valid price greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            numericPrice.Focus()
            Return False
        End If

        If Availability.SelectedIndex = -1 Then
            MessageBox.Show("Please select availability status.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Availability.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(ServingSize.Text) Then
            MessageBox.Show("Please enter serving size.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ServingSize.Focus()
            Return False
        End If

        ' Product Code is OPTIONAL - no validation

        If String.IsNullOrWhiteSpace(PrepTime.Text) Then
            MessageBox.Show("Please enter preparation time.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            PrepTime.Focus()
            Return False
        End If

        If cmbMealTime.SelectedIndex = -1 Then
            MessageBox.Show("Please select a meal time.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbMealTime.Focus()
            Return False
        End If

        Return True
    End Function

    ' =======================================================
    ' CHECK DUPLICATE PRODUCT CODE
    ' =======================================================
    Private Function IsDuplicateProductCode(code As String) As Boolean
        Try
            openConn()
            Dim query As String = "SELECT COUNT(*) FROM products WHERE ProductCode = @code"
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@code", code)
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            Return count > 0
        Catch ex As Exception
            Return False
        Finally
            conn.Close()
        End Try
    End Function

    ' =======================================================
    ' LOAD IMAGE FROM URL OR FILE PATH
    ' =======================================================
    Private Function LoadImageAsBytes(imagePath As String) As Byte()
        Try
            If imagePath.StartsWith("http://") OrElse imagePath.StartsWith("https://") Then
                Using client As New System.Net.WebClient()
                    Return client.DownloadData(imagePath)
                End Using
            ElseIf File.Exists(imagePath) Then
                Return File.ReadAllBytes(imagePath)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading image: " & ex.Message, "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End Try
    End Function

    ' =======================================================
    ' MAP CATEGORY - "Bilao" saves as "NOODLES & PASTA"
    ' =======================================================
    Private Function GetDatabaseCategory(displayCategory As String) As String
        If displayCategory = "Bilao" Then
            Return "NOODLES & PASTA"
        End If
        Return displayCategory
    End Function

    ' =======================================================
    ' ADD ITEM BUTTON
    ' =======================================================
    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click

        If Not ValidateForm() Then
            Exit Sub
        End If

        ' Check duplicate only if product code is provided
        If Not String.IsNullOrWhiteSpace(ProductCode.Text.Trim()) Then
            If IsDuplicateProductCode(ProductCode.Text.Trim()) Then
                MessageBox.Show("Product code already exists. Please use a different code.", "Duplicate Code", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                ProductCode.Focus()
                Exit Sub
            End If
        End If

        Try
            openConn()

            Dim sql As String =
                "INSERT INTO products 
                (ProductName, Category, Description, Price, Availability, ServingSize, 
                 DateAdded, LastUpdated, ProductCode, OrderCount, Image, PrepTime, MealTime)
                 VALUES
                (@ProductName, @Category, @Description, @Price, @Availability, @ServingSize,
                 NOW(), NOW(), @ProductCode, 0, @Image, @PrepTime, @MealTime)"

            Dim cmd As New MySqlCommand(sql, conn)

            cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim())
            cmd.Parameters.AddWithValue("@Category", GetDatabaseCategory(cmbCategory.Text.Trim()))
            cmd.Parameters.AddWithValue("@Description", Description.Text.Trim())
            cmd.Parameters.AddWithValue("@Price", numericPrice.Value)
            cmd.Parameters.AddWithValue("@Availability", Availability.Text)
            cmd.Parameters.AddWithValue("@ServingSize", ServingSize.Text.Trim())

            ' Product Code is OPTIONAL
            If String.IsNullOrWhiteSpace(ProductCode.Text.Trim()) Then
                cmd.Parameters.AddWithValue("@ProductCode", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ProductCode", ProductCode.Text.Trim())
            End If

            cmd.Parameters.AddWithValue("@PrepTime", PrepTime.Text.Trim())
            cmd.Parameters.AddWithValue("@MealTime", cmbMealTime.Text)

            ' Handle image
            If Not String.IsNullOrWhiteSpace(txtImageUrl.Text) Then
                Dim imageBytes As Byte() = LoadImageAsBytes(txtImageUrl.Text.Trim())
                If imageBytes IsNot Nothing Then
                    cmd.Parameters.AddWithValue("@Image", imageBytes)
                Else
                    cmd.Parameters.AddWithValue("@Image", DBNull.Value)
                End If
            Else
                cmd.Parameters.AddWithValue("@Image", DBNull.Value)
            End If

            cmd.ExecuteNonQuery()

            MessageBox.Show("Menu item added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Dim result As DialogResult = MessageBox.Show(
                "Do you want to add another menu item?",
                "Add Another?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                ClearForm()
            Else
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If

        Catch ex As Exception
            MessageBox.Show("Error adding item: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

    ' =======================================================
    ' CLEAR FORM
    ' =======================================================
    Private Sub ClearForm()
        txtProductName.Text = ""
        cmbCategory.SelectedIndex = -1
        Description.Text = ""
        numericPrice.Value = 0
        Availability.SelectedIndex = 0
        ServingSize.Text = ""
        ProductCode.Text = ""
        txtImageUrl.Text = ""
        PrepTime.Text = ""
        cmbMealTime.SelectedIndex = 0  ' Reset to "All Day"
        ProductID.Text = GenerateNextProductID()
        txtProductName.Focus()
    End Sub

    ' =======================================================
    ' CLOSE BUTTON
    ' =======================================================
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Dim result As DialogResult = MessageBox.Show(
            "Are you sure you want to close? Unsaved data will be lost.",
            "Confirm Close",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End If
    End Sub

End Class