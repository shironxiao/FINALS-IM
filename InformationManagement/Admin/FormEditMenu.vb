Imports MySqlConnector
Imports System.IO

Public Class FormEditMenu

    Public SelectedProductID As Integer

    Private Sub FormEditMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        cmbMealTime.SelectedIndex = 0

        ' Set default values
        numericPrice.Value = 0
        numericPrice.DecimalPlaces = 2
        numericPrice.Maximum = 999999

        ' ProductID is read-only
        ProductID.ReadOnly = True
        ProductID.BackColor = Color.LightGray
        ProductID.ForeColor = Color.Gray

        ' OrderCount is read-only
        OrderCount.ReadOnly = True
        OrderCount.BackColor = Color.LightGray
        OrderCount.ForeColor = Color.Gray

        ' DateTimePicker is disabled
        DateTimePicker1.Enabled = False
    End Sub

    ' =======================================================
    ' LOAD PRODUCT DATA
    ' =======================================================
    Public Sub LoadProduct(id As Integer)
        SelectedProductID = id
        Try
            openConn()
            Dim sql As String = "SELECT * FROM products WHERE ProductID = @id"
            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@id", id)
            Dim rd = cmd.ExecuteReader()

            If rd.Read() Then
                ProductID.Text = rd("ProductID").ToString()
                txtProductName.Text = rd("ProductName").ToString()
                Description.Text = rd("Description").ToString()
                numericPrice.Value = Convert.ToDecimal(rd("Price"))
                ServingSize.Text = rd("ServingSize").ToString()
                ProductCode.Text = rd("ProductCode").ToString()
                PrepTime.Text = rd("PrepTime").ToString()
                OrderCount.Text = rd("OrderCount").ToString()

                ' Set Availability
                Dim avail As String = rd("Availability").ToString()
                If avail = "Available" Then
                    Availability.SelectedIndex = 0
                ElseIf avail = "Unavailable" Then
                    Availability.SelectedIndex = 1
                End If

                ' Set Category - Map "NOODLES & PASTA" to "Bilao" for display
                Dim cat As String = rd("Category").ToString()
                If cat = "NOODLES & PASTA" Then
                    cmbCategory.Text = "Bilao"
                Else
                    cmbCategory.Text = cat
                End If

                ' Set MealTime
                Dim mealTime As String = rd("MealTime").ToString()
                Select Case mealTime
                    Case "All Day"
                        cmbMealTime.SelectedIndex = 0
                    Case "Breakfast"
                        cmbMealTime.SelectedIndex = 1
                    Case "Lunch"
                        cmbMealTime.SelectedIndex = 2
                    Case "Dinner"
                        cmbMealTime.SelectedIndex = 3
                    Case Else
                        cmbMealTime.SelectedIndex = 0
                End Select

                ' Set DateAdded
                If Not IsDBNull(rd("DateAdded")) Then
                    DateTimePicker1.Value = Convert.ToDateTime(rd("DateAdded"))
                End If

                ' Note: Image URL not stored separately, leave txtImageUrl empty
                txtImageUrl.Text = ""
            End If
            rd.Close()

        Catch ex As Exception
            MessageBox.Show("Error loading product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

    ' =======================================================
    ' VALIDATE FORM
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
    ' MAP CATEGORY - "Bilao" saves as "NOODLES & PASTA"
    ' =======================================================
    Private Function GetDatabaseCategory(displayCategory As String) As String
        If displayCategory = "Bilao" Then
            Return "NOODLES & PASTA"
        End If
        Return displayCategory
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
    ' UPDATE ITEM BUTTON
    ' =======================================================
    Private Sub btnUpdateItem_Click(sender As Object, e As EventArgs) Handles btnUpdateItem.Click

        If Not ValidateForm() Then
            Exit Sub
        End If

        Try
            openConn()

            Dim sql As String
            Dim updateImage As Boolean = Not String.IsNullOrWhiteSpace(txtImageUrl.Text.Trim())

            If updateImage Then
                sql = "UPDATE products SET 
                        ProductName = @ProductName,
                        Category = @Category,
                        Description = @Description,
                        Price = @Price,
                        Availability = @Availability,
                        ServingSize = @ServingSize,
                        ProductCode = @ProductCode,
                        PrepTime = @PrepTime,
                        MealTime = @MealTime,
                        LastUpdated = NOW(),
                        Image = @Image
                       WHERE ProductID = @id"
            Else
                sql = "UPDATE products SET 
                        ProductName = @ProductName,
                        Category = @Category,
                        Description = @Description,
                        Price = @Price,
                        Availability = @Availability,
                        ServingSize = @ServingSize,
                        ProductCode = @ProductCode,
                        PrepTime = @PrepTime,
                        MealTime = @MealTime,
                        LastUpdated = NOW()
                       WHERE ProductID = @id"
            End If

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
            cmd.Parameters.AddWithValue("@id", SelectedProductID)

            ' Handle image only if new URL/path provided
            If updateImage Then
                Dim imageBytes As Byte() = LoadImageAsBytes(txtImageUrl.Text.Trim())
                If imageBytes IsNot Nothing Then
                    cmd.Parameters.AddWithValue("@Image", imageBytes)
                Else
                    cmd.Parameters.AddWithValue("@Image", DBNull.Value)
                End If
            End If

            cmd.ExecuteNonQuery()

            MessageBox.Show("Menu item updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error updating item: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

    ' =======================================================
    ' CLOSE BUTTON
    ' =======================================================
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Dim result As DialogResult = MessageBox.Show(
            "Are you sure you want to close? Unsaved changes will be lost.",
            "Confirm Close",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End If
    End Sub

End Class