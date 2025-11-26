Imports MySqlConnector
Imports System.IO

Public Class FormEditMenu

    Public SelectedProductID As Integer
    Private SelectedImageBytes As Byte() = Nothing

    Private Sub FormEditMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeForm()
    End Sub

    ' =======================================================
    ' INITIALIZE FORM
    ' =======================================================
    Private Sub InitializeForm()
        Availability.Items.Clear()
        Availability.Items.Add("Available")
        Availability.Items.Add("Unavailable")
        Availability.SelectedIndex = 0

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

        cmbMealTime.Items.Clear()
        cmbMealTime.Items.Add("All Day")
        cmbMealTime.Items.Add("Breakfast")
        cmbMealTime.Items.Add("Lunch")
        cmbMealTime.Items.Add("Dinner")
        cmbMealTime.SelectedIndex = 0

        numericPrice.Value = 0
        numericPrice.DecimalPlaces = 2

        ProductID.ReadOnly = True
        OrderCount.ReadOnly = True
        DateTimePicker1.Enabled = False

        PictureBox1.Image = Nothing
        SelectedImageBytes = Nothing
    End Sub

    ' =======================================================
    ' LOAD PRODUCT WITH IMAGE
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
                Availability.Text = rd("Availability").ToString()
                cmbCategory.Text = rd("Category").ToString()
                cmbMealTime.Text = rd("MealTime").ToString()

                ' Load image if exists
                If Not IsDBNull(rd("Image")) Then
                    SelectedImageBytes = DirectCast(rd("Image"), Byte())

                    Using ms As New MemoryStream(SelectedImageBytes)
                        PictureBox1.Image = Image.FromStream(ms)
                        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                    End Using
                Else
                    PictureBox1.Image = Nothing
                    SelectedImageBytes = Nothing
                End If

            End If
            rd.Close()

        Catch ex As Exception
            MessageBox.Show("Error loading product: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    ' =======================================================
    ' BROWSE IMAGE BUTTON
    ' =======================================================
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim ofd As New OpenFileDialog()
        ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"

        If ofd.ShowDialog() = DialogResult.OK Then
            PictureBox1.Image = Image.FromFile(ofd.FileName)
            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage

            SelectedImageBytes = File.ReadAllBytes(ofd.FileName)
        End If
    End Sub

    ' =======================================================
    ' UPDATE ITEM WITH IMAGE
    ' =======================================================
    Private Sub btnUpdateItem_Click(sender As Object, e As EventArgs) Handles btnUpdateItem.Click

        Try
            openConn()

            Dim sql As String =
                "UPDATE products SET 
                ProductName=@ProductName,
                Category=@Category,
                Description=@Description,
                Price=@Price,
                Availability=@Availability,
                ServingSize=@ServingSize,
                ProductCode=@ProductCode,
                PrepTime=@PrepTime,
                MealTime=@MealTime,
                LastUpdated=NOW(),
                Image=@Image
                WHERE ProductID=@id"

            Dim cmd As New MySqlCommand(sql, conn)

            cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim())
            cmd.Parameters.AddWithValue("@Category", cmbCategory.Text.Trim())
            cmd.Parameters.AddWithValue("@Description", Description.Text.Trim())
            cmd.Parameters.AddWithValue("@Price", numericPrice.Value)
            cmd.Parameters.AddWithValue("@Availability", Availability.Text)
            cmd.Parameters.AddWithValue("@ServingSize", ServingSize.Text.Trim())

            If String.IsNullOrWhiteSpace(ProductCode.Text.Trim()) Then
                cmd.Parameters.AddWithValue("@ProductCode", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ProductCode", ProductCode.Text.Trim())
            End If

            cmd.Parameters.AddWithValue("@PrepTime", PrepTime.Text.Trim())
            cmd.Parameters.AddWithValue("@MealTime", cmbMealTime.Text)
            cmd.Parameters.AddWithValue("@id", SelectedProductID)

            ' Add Image Bytes
            If SelectedImageBytes IsNot Nothing Then
                cmd.Parameters.AddWithValue("@Image", SelectedImageBytes)
            Else
                cmd.Parameters.AddWithValue("@Image", DBNull.Value)
            End If

            cmd.ExecuteNonQuery()

            MessageBox.Show("Menu item updated successfully!")
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error updating item: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    ' =======================================================
    ' CLOSE BUTTON
    ' =======================================================
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class