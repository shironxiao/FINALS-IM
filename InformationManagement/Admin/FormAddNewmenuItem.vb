Imports MySqlConnector
Imports System.IO
Imports System.Drawing.Imaging

Public Class FormAddNewmenuItem

    ' =======================================================
    ' CONFIGURATION: Match your MenuItems.vb settings
    ' =======================================================
    Private Const UPLOAD_FOLDER As String = "C:\xampp\htdocs\TrialWeb\TrialWorkIM\Tabeya\uploads\products\"
    Private Const WEB_BASE_PATH As String = "uploads/products/"

    ' Store the selected image file path
    Private SelectedImagePath As String = Nothing

    Private Sub FormAddNewmenuItem_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeForm()
        EnsureUploadFolderExists()
    End Sub

    ' =======================================================
    ' ENSURE UPLOAD FOLDER EXISTS
    ' =======================================================
    Private Sub EnsureUploadFolderExists()
        Try
            If Not Directory.Exists(UPLOAD_FOLDER) Then
                Directory.CreateDirectory(UPLOAD_FOLDER)
                MessageBox.Show("Upload folder created at: " & UPLOAD_FOLDER, "Info")
            End If
        Catch ex As Exception
            MessageBox.Show("Warning: Could not create upload folder." & vbCrLf & ex.Message, "Warning")
        End Try
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

        SelectedImagePath = Nothing
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
                ' Load image into PictureBox
                PictureBox1.Image = Image.FromFile(ofd.FileName)

                ' Store the selected file path
                SelectedImagePath = ofd.FileName

            Catch ex As Exception
                MessageBox.Show("Error loading image: " & ex.Message)
                SelectedImagePath = Nothing
            End Try
        End If
    End Sub

    ' =======================================================
    ' SAVE IMAGE TO UPLOAD FOLDER
    ' =======================================================
    Private Function SaveImageToFolder() As String
        If SelectedImagePath Is Nothing OrElse Not File.Exists(SelectedImagePath) Then
            Return Nothing
        End If

        Try
            ' Generate unique filename
            Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
            Dim extension As String = Path.GetExtension(SelectedImagePath)
            Dim newFileName As String = $"product_{timestamp}_{Guid.NewGuid().ToString().Substring(0, 8)}{extension}"

            ' Full destination path
            Dim destinationPath As String = Path.Combine(UPLOAD_FOLDER, newFileName)

            ' Copy file to uploads folder
            File.Copy(SelectedImagePath, destinationPath, True)

            ' Return the relative web path (for database storage)
            Return WEB_BASE_PATH & newFileName

        Catch ex As Exception
            MessageBox.Show("Error saving image: " & ex.Message, "Error")
            Return Nothing
        End Try
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

            ' ===================== SAVE IMAGE AS FILE PATH =====================
            Dim imagePath As String = SaveImageToFolder()

            If imagePath IsNot Nothing Then
                cmd.Parameters.AddWithValue("@Image", imagePath)
            Else
                cmd.Parameters.AddWithValue("@Image", DBNull.Value)
            End If
            ' ===================================================================

            cmd.ExecuteNonQuery()

            MessageBox.Show("Menu item added successfully!" & vbCrLf &
                          If(imagePath IsNot Nothing, "Image saved to: " & imagePath, "No image uploaded"),
                          "Success")

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
        SelectedImagePath = Nothing
        ProductID.Text = GenerateNextProductID()
        txtProductName.Focus()
    End Sub

End Class