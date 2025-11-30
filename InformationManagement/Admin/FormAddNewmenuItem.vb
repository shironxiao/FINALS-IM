Imports MySqlConnector
Imports System.IO
Imports System.Drawing.Imaging

Public Class FormAddNewmenuItem

    ' Store the selected image bytes
    Private SelectedImageBytes As Byte() = Nothing

    Private Sub FormAddNewmenuItem_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        cmbCategory.Items.AddRange({
            "SPAGHETTI MEAL",
            "DESSERT",
            "DRINKS & BEVERAGES",
            "PLATTER",
            "RICE MEAL",
            "RICE",
            "Bilao",
            "Snacks"
        })
        cmbCategory.SelectedIndex = -1

        cmbMealTime.Items.Clear()
        cmbMealTime.Items.AddRange({"All Day", "Breakfast", "Lunch", "Dinner"})
        cmbMealTime.SelectedIndex = 0

        numericPrice.Value = 0
        numericPrice.DecimalPlaces = 2
        numericPrice.Maximum = 999999

        ProductID.Text = GenerateNextProductID()
        ProductID.ReadOnly = True
        ProductID.BackColor = Color.LightGray

        DateTimePicker1.Value = DateTime.Now
        DateTimePicker1.Enabled = False

        OrderCount.Text = "0"
        OrderCount.ReadOnly = True
        OrderCount.BackColor = Color.LightGray

        PictureBox1.Image = Nothing
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom

        SelectedImageBytes = Nothing
    End Sub

    ' =======================================================
    ' GENERATE NEXT PRODUCT ID
    ' =======================================================
    Private Function GenerateNextProductID() As String
        Try
            openConn()
            Dim query As String = "SELECT COALESCE(MAX(ProductID), 0) + 1 AS NextID FROM products"
            Dim cmd As New MySqlCommand(query, conn)
            Return cmd.ExecuteScalar().ToString()
        Catch
            Return "1"
        Finally
            conn.Close()
        End Try
    End Function

    ' =======================================================
    ' VALIDATION
    ' =======================================================
    Private Function ValidateForm() As Boolean
        If txtProductName.Text.Trim() = "" Then Return ShowError(txtProductName, "Please enter a product name.")
        If cmbCategory.SelectedIndex = -1 Then Return ShowError(cmbCategory, "Please select a category.")
        If Description.Text.Trim() = "" Then Return ShowError(Description, "Please enter a description.")
        If numericPrice.Value <= 0 Then Return ShowError(numericPrice, "Price must be greater than 0.")
        If Availability.SelectedIndex = -1 Then Return ShowError(Availability, "Please select availability.")
        If ServingSize.Text.Trim() = "" Then Return ShowError(ServingSize, "Please enter serving size.")
        If PrepTime.Text.Trim() = "" Then Return ShowError(PrepTime, "Please enter preparation time.")
        If cmbMealTime.SelectedIndex = -1 Then Return ShowError(cmbMealTime, "Please select meal time.")

        Return True
    End Function

    Private Function ShowError(ctrl As Control, msg As String) As Boolean
        MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ctrl.Focus()
        Return False
    End Function

    ' =======================================================
    ' PICTUREBOX IMAGE LOADING
    ' =======================================================
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        BrowseAndLoadImage()
    End Sub

    Private Sub BrowseAndLoadImage()
        Dim ofd As New OpenFileDialog()
        ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"

        If ofd.ShowDialog() = DialogResult.OK Then
            Try
                Using fs As New FileStream(ofd.FileName, FileMode.Open, FileAccess.Read)
                    PictureBox1.Image = Image.FromStream(fs)
                    ' Store the image bytes when loaded
                    SelectedImageBytes = PictureBoxImageToBytes()
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading image: " & ex.Message)
                SelectedImageBytes = Nothing
            End Try
        End If
    End Sub

    ' =======================================================
    ' IMAGE → BYTE()
    ' =======================================================
    Private Function PictureBoxImageToBytes() As Byte()
        If PictureBox1.Image Is Nothing Then Return Nothing

        Using ms As New MemoryStream()
            PictureBox1.Image.Save(ms, ImageFormat.Jpeg)
            Return ms.ToArray()
        End Using
    End Function

    ' =======================================================
    ' ADD ITEM BUTTON CLICK
    ' =======================================================
    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click

        If Not ValidateForm() Then Exit Sub

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

            If ProductCode.Text.Trim() = "" Then
                cmd.Parameters.AddWithValue("@ProductCode", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ProductCode", ProductCode.Text.Trim())
            End If

            cmd.Parameters.AddWithValue("@PrepTime", PrepTime.Text.Trim())
            cmd.Parameters.AddWithValue("@MealTime", cmbMealTime.Text)

            ' ===================== IMAGE SAVE LOGIC =====================
            ' Use stored image bytes if available
            If SelectedImageBytes IsNot Nothing Then
                cmd.Parameters.Add("@Image", MySqlDbType.LongBlob).Value = SelectedImageBytes
            Else
                cmd.Parameters.Add("@Image", MySqlDbType.LongBlob).Value = DBNull.Value
            End If
            ' ============================================================

            cmd.ExecuteNonQuery()

            MessageBox.Show("Menu item added successfully!", "Success")

            ClearForm()

        Catch ex As Exception
            MessageBox.Show("Error adding item: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    ' =======================================================
    ' MAP DISPLAY CATEGORY TO DB CATEGORY
    ' =======================================================
    Private Function GetDatabaseCategory(displayCategory As String) As String
        If displayCategory = "Bilao" Then Return "NOODLES & PASTA"
        Return displayCategory
    End Function

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
        PrepTime.Text = ""
        cmbMealTime.SelectedIndex = 0
        PictureBox1.Image = Nothing
        SelectedImageBytes = Nothing ' Clear stored image bytes
        ProductID.Text = GenerateNextProductID()
        txtProductName.Focus()
    End Sub

End Class