Imports System.IO
Imports System.Net
Imports MySqlConnector

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

        cmbMealTime.Items.Clear()
        cmbMealTime.Items.AddRange({"All Day", "Breakfast", "Lunch", "Dinner"})
        cmbMealTime.SelectedIndex = 0

        numericPrice.DecimalPlaces = 2
        numericPrice.Maximum = 999999

        ' ProductID will be auto-generated
        ProductID.ReadOnly = True
        ProductID.BackColor = Color.LightGray

        DateTimePicker1.Value = DateTime.Now
        DateTimePicker1.Enabled = False

        OrderCount.Text = "0"
        OrderCount.ReadOnly = True
        OrderCount.BackColor = Color.LightGray

        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox1.Image = Nothing

        SelectedImagePath = Nothing

        ' Load next ProductID on form initialization
        ProductID.Text = GenerateNextProductID()
    End Sub

    ' ================================
    ' VALIDATION
    ' ================================
    Private Function ValidateForm() As Boolean
        If txtProductName.Text.Trim() = "" Then Return ShowError(txtProductName, "Enter product name.")
        If cmbCategory.SelectedIndex = -1 Then Return ShowError(cmbCategory, "Select category.")
        If Description.Text.Trim() = "" Then Return ShowError(Description, "Enter description.")
        If numericPrice.Value <= 0 Then Return ShowError(numericPrice, "Price must be greater than 0.")
        If ServingSize.Text.Trim() = "" Then Return ShowError(ServingSize, "Enter serving size.")
        If PrepTime.Text.Trim() = "" Then Return ShowError(PrepTime, "Enter prep time.")

        Return True
    End Function

    Private Function ShowError(ctrl As Control, msg As String) As Boolean
        MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ctrl.Focus()
        Return False
    End Function

    ' ================================
    ' IMAGE BROWSE
    ' ================================
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim ofd As New OpenFileDialog()
        ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
        ofd.Title = "Select Product Image"

        If ofd.ShowDialog() = DialogResult.OK Then
            Try
                ' Dispose old image if exists to free memory
                If PictureBox1.Image IsNot Nothing Then
                    PictureBox1.Image.Dispose()
                    PictureBox1.Image = Nothing
                End If

                ' Load image into PictureBox using FileStream to avoid file locking
                Using fs As New FileStream(ofd.FileName, FileMode.Open, FileAccess.Read)
                    PictureBox1.Image = Image.FromStream(fs)
                End Using

                ' Store the selected file path
                SelectedImagePath = ofd.FileName

                ' Debug: Show confirmation
                ' MessageBox.Show("Image loaded: " & Path.GetFileName(ofd.FileName), "Success")

            Catch ex As Exception
                MessageBox.Show("Error loading image: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                SelectedImagePath = Nothing
                PictureBox1.Image = Nothing
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

    ' ================================
    ' ADD BUTTON
    ' ================================
    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        If Not ValidateForm() Then Exit Sub

        Dim imagePath As String = Nothing
        Dim insertedId As String = Nothing

        Try
            ' Save image first before database operation
            imagePath = SaveImageToFolder()

            openConn()

            Dim sql As String = "
                INSERT INTO products 
                (ProductName, Category, Description, Price, Availability, ServingSize, 
                 DateAdded, LastUpdated, ProductCode, OrderCount, PrepTime, MealTime, Image)
                VALUES
                (@ProductName, @Category, @Description, @Price, @Availability, @ServingSize,
                 NOW(), NOW(), @ProductCode, 0, @PrepTime, @MealTime, @Image);
                SELECT LAST_INSERT_ID();"

            Dim cmd As New MySqlCommand(sql, conn)

            cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim())
            cmd.Parameters.AddWithValue("@Category", GetDatabaseCategory(cmbCategory.Text.Trim()))
            cmd.Parameters.AddWithValue("@Description", Description.Text.Trim())
            cmd.Parameters.AddWithValue("@Price", numericPrice.Value)
            cmd.Parameters.AddWithValue("@Availability", Availability.Text)
            cmd.Parameters.AddWithValue("@ServingSize", ServingSize.Text.Trim())
            cmd.Parameters.AddWithValue("@PrepTime", PrepTime.Text.Trim())
            cmd.Parameters.AddWithValue("@MealTime", cmbMealTime.Text)

            ' Handle ProductCode
            If ProductCode.Text.Trim() = "" Then
                cmd.Parameters.AddWithValue("@ProductCode", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ProductCode", ProductCode.Text.Trim())
            End If

            ' Handle Image path
            If imagePath IsNot Nothing Then
                cmd.Parameters.AddWithValue("@Image", imagePath)
            Else
                cmd.Parameters.AddWithValue("@Image", DBNull.Value)
            End If

            ' Execute and get inserted ID
            insertedId = cmd.ExecuteScalar().ToString()
            ProductID.Text = insertedId

            MessageBox.Show("Menu item added successfully!" & vbCrLf &
                          "Product ID: " & insertedId & vbCrLf &
                          If(imagePath IsNot Nothing, "Image saved to: " & imagePath, "No image uploaded"),
                          "Success")

            ClearForm()

        Catch ex As Exception
            MessageBox.Show("Save error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' ================================
    ' LOAD IMAGE FROM FILE PATH
    ' ================================
    Private Sub LoadProductImage(relativeWebPath As String)
        Try
            ' Convert web path back to local file path
            ' Example: "uploads/products/image.jpg" -> "C:\xampp\htdocs\...\uploads\products\image.jpg"
            Dim fileName As String = Path.GetFileName(relativeWebPath)
            Dim fullPath As String = Path.Combine(UPLOAD_FOLDER, fileName)

            If File.Exists(fullPath) Then
                PictureBox1.Image = Image.FromFile(fullPath)
            Else
                MessageBox.Show("Image file not found: " & fullPath, "Warning")
            End If
        Catch ex As Exception
            MessageBox.Show("Image load failed: " & ex.Message, "Error")
        End Try
    End Sub

    ' ================================
    ' CATEGORY MAP
    ' ================================
    Private Function GetDatabaseCategory(displayCategory As String) As String
        If displayCategory = "Bilao" Then Return "NOODLES & PASTA"
        Return displayCategory
    End Function

    ' ================================
    ' GENERATE NEXT PRODUCT ID (HELPER)
    ' ================================
    Private Function GenerateNextProductID() As String
        Try
            openConn()
            Dim sql As String = "SELECT IFNULL(MAX(ProductID), 0) + 1 FROM products"
            Dim cmd As New MySqlCommand(sql, conn)
            Dim nextId = cmd.ExecuteScalar()
            conn.Close()
            Return nextId.ToString()
        Catch ex As Exception
            Return "Auto-Generated"
        End Try
    End Function

    ' ================================
    ' CLEAR FORM
    ' ================================
    Private Sub ClearForm()
        txtProductName.Text = ""
        cmbCategory.SelectedIndex = -1
        Description.Text = ""
        numericPrice.Value = 0
        ServingSize.Text = ""
        ProductCode.Text = ""
        PrepTime.Text = ""
        cmbMealTime.SelectedIndex = 0

        ' Dispose image properly
        If PictureBox1.Image IsNot Nothing Then
            PictureBox1.Image.Dispose()
            PictureBox1.Image = Nothing
        End If

        SelectedImagePath = Nothing
        ProductID.Text = GenerateNextProductID()  ' Show next auto-incremented ID
        txtProductName.Focus()
    End Sub

End Class