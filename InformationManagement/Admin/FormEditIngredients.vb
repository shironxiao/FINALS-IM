Imports MySqlConnector

Public Class FormEditIngredients

    Private currentProductId As Integer = 0
    Private currentProductName As String = ""

    ' =======================================================
    ' LOAD PRODUCT DATA
    ' =======================================================
    Public Sub LoadProductData(productId As Integer)
        currentProductId = productId

        Try
            openConn()

            ' Get product name
            Dim cmdProduct As New MySqlCommand("SELECT ProductName FROM products WHERE ProductID = @id", conn)
            cmdProduct.Parameters.AddWithValue("@id", productId)
            currentProductName = cmdProduct.ExecuteScalar()?.ToString()

            conn.Close()

            ' Update UI
            InitializeUI()
            LoadCurrentIngredients()
            LoadAvailableIngredients()

        Catch ex As Exception
            MessageBox.Show("Error loading product data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' =======================================================
    ' INITIALIZE UI COMPONENTS
    ' =======================================================
    Private Sub InitializeUI()
        ' Form settings
        Me.Text = "Edit Ingredients - " & currentProductName
        Me.Size = New Size(900, 700)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(245, 245, 245)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False

        ' Clear existing controls
        Me.Controls.Clear()

        ' Main panel
        Dim mainPanel As New Panel()
        mainPanel.Dock = DockStyle.Fill
        mainPanel.Padding = New Padding(20)
        Me.Controls.Add(mainPanel)

        ' Title
        Dim lblTitle As New Label()
        lblTitle.Text = "✏️ Edit Ingredients for: " & currentProductName
        lblTitle.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lblTitle.Location = New Point(20, 20)
        lblTitle.Size = New Size(860, 40)
        lblTitle.ForeColor = Color.FromArgb(52, 73, 94)
        mainPanel.Controls.Add(lblTitle)

        ' Left panel - Current Ingredients
        Dim leftPanel As New Panel()
        leftPanel.Location = New Point(20, 70)
        leftPanel.Size = New Size(420, 500)
        leftPanel.BackColor = Color.White
        leftPanel.BorderStyle = BorderStyle.FixedSingle
        mainPanel.Controls.Add(leftPanel)

        Dim lblCurrent As New Label()
        lblCurrent.Text = "📋 Current Ingredients"
        lblCurrent.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblCurrent.Location = New Point(10, 10)
        lblCurrent.Size = New Size(400, 30)
        lblCurrent.ForeColor = Color.FromArgb(41, 128, 185)
        leftPanel.Controls.Add(lblCurrent)

        ' DataGridView for current ingredients
        Dim dgvCurrent As New DataGridView()
        dgvCurrent.Name = "dgvCurrentIngredients"
        dgvCurrent.Location = New Point(10, 45)
        dgvCurrent.Size = New Size(400, 400)
        dgvCurrent.AllowUserToAddRows = False
        dgvCurrent.ReadOnly = False
        dgvCurrent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvCurrent.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvCurrent.BackgroundColor = Color.White
        dgvCurrent.BorderStyle = BorderStyle.None
        leftPanel.Controls.Add(dgvCurrent)

        ' Remove button
        Dim btnRemove As New Button()
        btnRemove.Name = "btnRemove"
        btnRemove.Text = "🗑️ Remove Selected"
        btnRemove.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnRemove.Location = New Point(10, 455)
        btnRemove.Size = New Size(400, 35)
        btnRemove.BackColor = Color.FromArgb(231, 76, 60)
        btnRemove.ForeColor = Color.White
        btnRemove.FlatStyle = FlatStyle.Flat
        btnRemove.FlatAppearance.BorderSize = 0
        btnRemove.Cursor = Cursors.Hand
        AddHandler btnRemove.Click, AddressOf RemoveIngredient_Click
        leftPanel.Controls.Add(btnRemove)

        ' Right panel - Add New Ingredient
        Dim rightPanel As New Panel()
        rightPanel.Location = New Point(460, 70)
        rightPanel.Size = New Size(420, 500)
        rightPanel.BackColor = Color.White
        rightPanel.BorderStyle = BorderStyle.FixedSingle
        mainPanel.Controls.Add(rightPanel)

        Dim lblAdd As New Label()
        lblAdd.Text = "➕ Add Ingredient to Product"
        lblAdd.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblAdd.Location = New Point(10, 10)
        lblAdd.Size = New Size(400, 30)
        lblAdd.ForeColor = Color.FromArgb(39, 174, 96)
        rightPanel.Controls.Add(lblAdd)

        ' Radio buttons for selection mode
        Dim rbExisting As New RadioButton()
        rbExisting.Name = "rbExisting"
        rbExisting.Text = "Select from existing ingredients"
        rbExisting.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        rbExisting.Location = New Point(10, 45)
        rbExisting.Size = New Size(250, 20)
        rbExisting.Checked = True
        AddHandler rbExisting.CheckedChanged, AddressOf RadioButton_CheckedChanged
        rightPanel.Controls.Add(rbExisting)

        Dim rbNewIngredient As New RadioButton()
        rbNewIngredient.Name = "rbNewIngredient"
        rbNewIngredient.Text = "Create new ingredient"
        rbNewIngredient.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        rbNewIngredient.Location = New Point(270, 45)
        rbNewIngredient.Size = New Size(150, 20)
        AddHandler rbNewIngredient.CheckedChanged, AddressOf RadioButton_CheckedChanged
        rightPanel.Controls.Add(rbNewIngredient)

        ' === EXISTING INGREDIENT PANEL ===
        Dim pnlExisting As New Panel()
        pnlExisting.Name = "pnlExisting"
        pnlExisting.Location = New Point(10, 75)
        pnlExisting.Size = New Size(400, 180)
        pnlExisting.BackColor = Color.FromArgb(250, 250, 250)
        pnlExisting.BorderStyle = BorderStyle.FixedSingle
        rightPanel.Controls.Add(pnlExisting)

        ' Ingredient selection
        Dim lblSelectIngredient As New Label()
        lblSelectIngredient.Text = "Select Ingredient:"
        lblSelectIngredient.Font = New Font("Segoe UI", 10)
        lblSelectIngredient.Location = New Point(10, 10)
        lblSelectIngredient.Size = New Size(150, 25)
        pnlExisting.Controls.Add(lblSelectIngredient)

        Dim cmbIngredients As New ComboBox()
        cmbIngredients.Name = "cmbIngredients"
        cmbIngredients.Font = New Font("Segoe UI", 10)
        cmbIngredients.Location = New Point(10, 40)
        cmbIngredients.Size = New Size(380, 30)
        cmbIngredients.DropDownStyle = ComboBoxStyle.DropDownList
        pnlExisting.Controls.Add(cmbIngredients)

        ' Quantity for existing
        Dim lblQuantityExist As New Label()
        lblQuantityExist.Text = "Quantity:"
        lblQuantityExist.Font = New Font("Segoe UI", 10)
        lblQuantityExist.Location = New Point(10, 80)
        lblQuantityExist.Size = New Size(150, 25)
        pnlExisting.Controls.Add(lblQuantityExist)

        Dim txtQuantityExist As New TextBox()
        txtQuantityExist.Name = "txtQuantityExist"
        txtQuantityExist.Font = New Font("Segoe UI", 10)
        txtQuantityExist.Location = New Point(10, 110)
        txtQuantityExist.Size = New Size(180, 30)
        pnlExisting.Controls.Add(txtQuantityExist)

        ' Unit for existing
        Dim lblUnitExist As New Label()
        lblUnitExist.Text = "Unit:"
        lblUnitExist.Font = New Font("Segoe UI", 10)
        lblUnitExist.Location = New Point(210, 80)
        lblUnitExist.Size = New Size(150, 25)
        pnlExisting.Controls.Add(lblUnitExist)

        Dim txtUnitExist As New TextBox()
        txtUnitExist.Name = "txtUnitExist"
        txtUnitExist.Font = New Font("Segoe UI", 10)
        txtUnitExist.Location = New Point(210, 110)
        txtUnitExist.Size = New Size(180, 30)
        pnlExisting.Controls.Add(txtUnitExist)

        ' === NEW INGREDIENT PANEL ===
        Dim pnlNewIngredient As New Panel()
        pnlNewIngredient.Name = "pnlNewIngredient"
        pnlNewIngredient.Location = New Point(10, 75)
        pnlNewIngredient.Size = New Size(400, 300)
        pnlNewIngredient.BackColor = Color.FromArgb(250, 250, 250)
        pnlNewIngredient.BorderStyle = BorderStyle.FixedSingle
        pnlNewIngredient.Visible = False
        rightPanel.Controls.Add(pnlNewIngredient)

        ' New ingredient name
        Dim lblNewName As New Label()
        lblNewName.Text = "Ingredient Name:"
        lblNewName.Font = New Font("Segoe UI", 10)
        lblNewName.Location = New Point(10, 10)
        lblNewName.Size = New Size(150, 25)
        pnlNewIngredient.Controls.Add(lblNewName)

        Dim txtNewName As New TextBox()
        txtNewName.Name = "txtNewName"
        txtNewName.Font = New Font("Segoe UI", 10)
        txtNewName.Location = New Point(10, 40)
        txtNewName.Size = New Size(380, 30)
        pnlNewIngredient.Controls.Add(txtNewName)

        ' Stock unit type
        Dim lblStockUnit As New Label()
        lblStockUnit.Text = "Stock Unit (for inventory):"
        lblStockUnit.Font = New Font("Segoe UI", 10)
        lblStockUnit.Location = New Point(10, 80)
        lblStockUnit.Size = New Size(200, 25)
        pnlNewIngredient.Controls.Add(lblStockUnit)

        Dim txtStockUnit As New TextBox()
        txtStockUnit.Name = "txtStockUnit"
        txtStockUnit.Font = New Font("Segoe UI", 10)
        txtStockUnit.Location = New Point(10, 110)
        txtStockUnit.Size = New Size(180, 30)
        txtStockUnit.Text = "kg"
        pnlNewIngredient.Controls.Add(txtStockUnit)

        ' Is Perishable checkbox
        Dim chkPerishable As New CheckBox()
        chkPerishable.Name = "chkPerishable"
        chkPerishable.Text = "Perishable"
        chkPerishable.Font = New Font("Segoe UI", 9)
        chkPerishable.Location = New Point(210, 110)
        chkPerishable.Size = New Size(180, 30)
        chkPerishable.Checked = True
        pnlNewIngredient.Controls.Add(chkPerishable)

        ' Quantity used in recipe
        Dim lblQuantityNew As New Label()
        lblQuantityNew.Text = "Quantity Used (in recipe):"
        lblQuantityNew.Font = New Font("Segoe UI", 10)
        lblQuantityNew.Location = New Point(10, 150)
        lblQuantityNew.Size = New Size(200, 25)
        pnlNewIngredient.Controls.Add(lblQuantityNew)

        Dim txtQuantityNew As New TextBox()
        txtQuantityNew.Name = "txtQuantityNew"
        txtQuantityNew.Font = New Font("Segoe UI", 10)
        txtQuantityNew.Location = New Point(10, 180)
        txtQuantityNew.Size = New Size(180, 30)
        pnlNewIngredient.Controls.Add(txtQuantityNew)

        ' Unit for recipe
        Dim lblUnitNew As New Label()
        lblUnitNew.Text = "Unit (recipe):"
        lblUnitNew.Font = New Font("Segoe UI", 10)
        lblUnitNew.Location = New Point(210, 150)
        lblUnitNew.Size = New Size(150, 25)
        pnlNewIngredient.Controls.Add(lblUnitNew)

        Dim txtUnitNew As New TextBox()
        txtUnitNew.Name = "txtUnitNew"
        txtUnitNew.Font = New Font("Segoe UI", 10)
        txtUnitNew.Location = New Point(210, 180)
        txtUnitNew.Size = New Size(180, 30)
        pnlNewIngredient.Controls.Add(txtUnitNew)

        ' Helper text
        Dim lblHelper As New Label()
        lblHelper.Text = "💡 Tip: Stock unit is for inventory (e.g., kg)," & vbCrLf &
                        "Recipe unit is for cooking (e.g., g, ml)"
        lblHelper.Font = New Font("Segoe UI", 8, FontStyle.Italic)
        lblHelper.Location = New Point(10, 220)
        lblHelper.Size = New Size(380, 40)
        lblHelper.ForeColor = Color.Gray
        pnlNewIngredient.Controls.Add(lblHelper)

        ' Add button
        Dim btnAdd As New Button()
        btnAdd.Name = "btnAdd"
        btnAdd.Text = "➕ Add to Product"
        btnAdd.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnAdd.Location = New Point(10, 395)
        btnAdd.Size = New Size(400, 40)
        btnAdd.BackColor = Color.FromArgb(39, 174, 96)
        btnAdd.ForeColor = Color.White
        btnAdd.FlatStyle = FlatStyle.Flat
        btnAdd.FlatAppearance.BorderSize = 0
        btnAdd.Cursor = Cursors.Hand
        AddHandler btnAdd.Click, AddressOf AddIngredient_Click
        rightPanel.Controls.Add(btnAdd)

        ' Instructions panel
        Dim instructionsPanel As New Panel()
        instructionsPanel.Location = New Point(10, 445)
        instructionsPanel.Size = New Size(400, 45)
        instructionsPanel.BackColor = Color.FromArgb(250, 250, 250)
        instructionsPanel.BorderStyle = BorderStyle.FixedSingle
        rightPanel.Controls.Add(instructionsPanel)

        Dim lblInstructions As New Label()
        lblInstructions.Text = "📝 Choose to add from existing ingredients" & vbCrLf &
                              "or create a new ingredient and add it to this product."
        lblInstructions.Font = New Font("Segoe UI", 9)
        lblInstructions.Location = New Point(10, 10)
        lblInstructions.Size = New Size(380, 30)
        lblInstructions.ForeColor = Color.FromArgb(127, 140, 141)
        instructionsPanel.Controls.Add(lblInstructions)

        ' Bottom buttons
        Dim btnSave As New Button()
        btnSave.Text = "💾 Save Changes"
        btnSave.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        btnSave.Location = New Point(20, 580)
        btnSave.Size = New Size(200, 40)
        btnSave.BackColor = Color.FromArgb(39, 174, 96)
        btnSave.ForeColor = Color.White
        btnSave.FlatStyle = FlatStyle.Flat
        btnSave.FlatAppearance.BorderSize = 0
        btnSave.Cursor = Cursors.Hand
        AddHandler btnSave.Click, AddressOf SaveChanges_Click
        mainPanel.Controls.Add(btnSave)

        Dim btnCancel As New Button()
        btnCancel.Text = "✕ Cancel"
        btnCancel.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        btnCancel.Location = New Point(680, 580)
        btnCancel.Size = New Size(200, 40)
        btnCancel.BackColor = Color.FromArgb(149, 165, 166)
        btnCancel.ForeColor = Color.White
        btnCancel.FlatStyle = FlatStyle.Flat
        btnCancel.FlatAppearance.BorderSize = 0
        btnCancel.Cursor = Cursors.Hand
        AddHandler btnCancel.Click, Sub()
                                        Me.DialogResult = DialogResult.Cancel
                                        Me.Close()
                                    End Sub
        mainPanel.Controls.Add(btnCancel)
    End Sub

    ' =======================================================
    ' LOAD CURRENT INGREDIENTS
    ' =======================================================
    Private Sub LoadCurrentIngredients()
        Dim dgv As DataGridView = CType(Me.Controls.Find("dgvCurrentIngredients", True)(0), DataGridView)

        Try
            openConn()

            Dim query As String = "
                SELECT 
                    pi.ProductIngredientID,
                    i.IngredientName AS 'Ingredient',
                    pi.QuantityUsed AS 'Quantity',
                    pi.UnitType AS 'Unit'
                FROM product_ingredients pi
                INNER JOIN ingredients i ON pi.IngredientID = i.IngredientID
                WHERE pi.ProductID = @productId
                ORDER BY i.IngredientName ASC
            "

            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@productId", currentProductId)

            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            dgv.DataSource = dt

            ' Hide ID column
            If dgv.Columns.Contains("ProductIngredientID") Then
                dgv.Columns("ProductIngredientID").Visible = False
            End If

            ' Make quantity and unit editable
            If dgv.Columns.Contains("Quantity") Then
                dgv.Columns("Quantity").ReadOnly = False
            End If
            If dgv.Columns.Contains("Unit") Then
                dgv.Columns("Unit").ReadOnly = False
            End If

            ' Make ingredient name read-only
            If dgv.Columns.Contains("Ingredient") Then
                dgv.Columns("Ingredient").ReadOnly = True
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading current ingredients: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' =======================================================
    ' LOAD AVAILABLE INGREDIENTS
    ' =======================================================
    Private Sub LoadAvailableIngredients()
        Dim cmb As ComboBox = CType(Me.Controls.Find("cmbIngredients", True)(0), ComboBox)
        cmb.Items.Clear()

        Try
            openConn()

            Dim query As String = "
                SELECT IngredientID, IngredientName 
                FROM ingredients 
                WHERE IsActive = 1 
                ORDER BY IngredientName ASC
            "

            Dim cmd As New MySqlCommand(query, conn)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            While reader.Read()
                Dim item As New IngredientItem()
                item.Id = Convert.ToInt32(reader("IngredientID"))
                item.Name = reader("IngredientName").ToString()
                cmb.Items.Add(item)
            End While

            reader.Close()

        Catch ex As Exception
            MessageBox.Show("Error loading available ingredients: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' =======================================================
    ' RADIO BUTTON CHANGED - TOGGLE PANELS
    ' =======================================================
    Private Sub RadioButton_CheckedChanged(sender As Object, e As EventArgs)
        Dim rb As RadioButton = CType(sender, RadioButton)

        If Not rb.Checked Then Return

        ' Find panels
        Dim pnlExisting As Panel = CType(Me.Controls.Find("pnlExisting", True)(0), Panel)
        Dim pnlNewIngredient As Panel = CType(Me.Controls.Find("pnlNewIngredient", True)(0), Panel)

        If rb.Name = "rbExisting" Then
            pnlExisting.Visible = True
            pnlNewIngredient.Visible = False
        ElseIf rb.Name = "rbNewIngredient" Then
            pnlExisting.Visible = False
            pnlNewIngredient.Visible = True
        End If
    End Sub

    ' =======================================================
    ' ADD INGREDIENT
    ' =======================================================
    Private Sub AddIngredient_Click(sender As Object, e As EventArgs)
        ' Check which radio button is selected
        Try
            Dim rbExisting As RadioButton = CType(Me.Controls.Find("rbExisting", True)(0), RadioButton)

            If rbExisting.Checked Then
                ' Add from existing ingredient
                AddExistingIngredient()
            Else
                ' Create new ingredient and add it
                CreateAndAddNewIngredient()
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =======================================================
    ' ADD EXISTING INGREDIENT TO PRODUCT
    ' =======================================================
    Private Sub AddExistingIngredient()
        Dim cmbIngredients As ComboBox = CType(Me.Controls.Find("cmbIngredients", True)(0), ComboBox)
        Dim txtQuantityExist As TextBox = CType(Me.Controls.Find("txtQuantityExist", True)(0), TextBox)
        Dim txtUnitExist As TextBox = CType(Me.Controls.Find("txtUnitExist", True)(0), TextBox)

        ' Validation
        If cmbIngredients.SelectedItem Is Nothing Then
            MessageBox.Show("Please select an ingredient.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim quantity As Decimal
        If Not Decimal.TryParse(txtQuantityExist.Text, quantity) OrElse quantity <= 0 Then
            MessageBox.Show("Please enter a valid quantity (positive number).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrWhiteSpace(txtUnitExist.Text) Then
            MessageBox.Show("Please enter a unit (e.g., g, kg, ml, pcs).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedIngredient As IngredientItem = CType(cmbIngredients.SelectedItem, IngredientItem)

        Try
            openConn()

            ' Check if ingredient already exists for this product
            Dim checkQuery As String = "
                SELECT COUNT(*) 
                FROM product_ingredients 
                WHERE ProductID = @productId AND IngredientID = @ingredientId
            "
            Dim checkCmd As New MySqlCommand(checkQuery, conn)
            checkCmd.Parameters.AddWithValue("@productId", currentProductId)
            checkCmd.Parameters.AddWithValue("@ingredientId", selectedIngredient.Id)

            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If count > 0 Then
                MessageBox.Show("This ingredient is already added. Please edit it in the current ingredients list.", "Duplicate Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                conn.Close()
                Return
            End If

            ' Insert new ingredient
            Dim insertQuery As String = "
                INSERT INTO product_ingredients 
                (ProductID, IngredientID, QuantityUsed, UnitType, CreatedDate, UpdatedDate) 
                VALUES 
                (@productId, @ingredientId, @quantity, @unit, NOW(), NOW())
            "
            Dim insertCmd As New MySqlCommand(insertQuery, conn)
            insertCmd.Parameters.AddWithValue("@productId", currentProductId)
            insertCmd.Parameters.AddWithValue("@ingredientId", selectedIngredient.Id)
            insertCmd.Parameters.AddWithValue("@quantity", quantity)
            insertCmd.Parameters.AddWithValue("@unit", txtUnitExist.Text.Trim())

            insertCmd.ExecuteNonQuery()

            MessageBox.Show("✓ Ingredient added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Clear fields
            cmbIngredients.SelectedIndex = -1
            txtQuantityExist.Clear()
            txtUnitExist.Clear()

            ' Refresh current ingredients list
            LoadCurrentIngredients()

        Catch ex As Exception
            MessageBox.Show("Error adding ingredient: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' =======================================================
    ' CREATE NEW INGREDIENT AND ADD TO PRODUCT
    ' =======================================================
    Private Sub CreateAndAddNewIngredient()
        Dim txtNewName As TextBox = CType(Me.Controls.Find("txtNewName", True)(0), TextBox)
        Dim txtStockUnit As TextBox = CType(Me.Controls.Find("txtStockUnit", True)(0), TextBox)
        Dim chkPerishable As CheckBox = CType(Me.Controls.Find("chkPerishable", True)(0), CheckBox)
        Dim txtQuantityNew As TextBox = CType(Me.Controls.Find("txtQuantityNew", True)(0), TextBox)
        Dim txtUnitNew As TextBox = CType(Me.Controls.Find("txtUnitNew", True)(0), TextBox)

        ' Validation
        If String.IsNullOrWhiteSpace(txtNewName.Text) Then
            MessageBox.Show("Please enter an ingredient name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNewName.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txtStockUnit.Text) Then
            MessageBox.Show("Please enter a stock unit (e.g., kg, pack, liter).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtStockUnit.Focus()
            Return
        End If

        Dim quantity As Decimal
        If Not Decimal.TryParse(txtQuantityNew.Text, quantity) OrElse quantity <= 0 Then
            MessageBox.Show("Please enter a valid quantity used in recipe (positive number).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtQuantityNew.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txtUnitNew.Text) Then
            MessageBox.Show("Please enter a recipe unit (e.g., g, ml, pcs).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUnitNew.Focus()
            Return
        End If

        Try
            openConn()

            ' Check if ingredient name already exists
            Dim checkQuery As String = "SELECT IngredientID FROM ingredients WHERE IngredientName = @name"
            Dim checkCmd As New MySqlCommand(checkQuery, conn)
            checkCmd.Parameters.AddWithValue("@name", txtNewName.Text.Trim())
            Dim existingId As Object = checkCmd.ExecuteScalar()

            Dim ingredientId As Integer

            If existingId IsNot Nothing Then
                ' Ingredient already exists, use existing ID
                ingredientId = Convert.ToInt32(existingId)
                Dim result As DialogResult = MessageBox.Show(
                    $"An ingredient named '{txtNewName.Text}' already exists." & vbCrLf & vbCrLf &
                    "Do you want to use the existing ingredient?",
                    "Ingredient Exists",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                )

                If result = DialogResult.No Then
                    conn.Close()
                    Return
                End If
            Else
                ' Create new ingredient in ingredients table
                Dim insertIngredientQuery As String = "
                    INSERT INTO ingredients 
                    (IngredientName, UnitType, StockQuantity, IsActive, IsPerishable, LastRestockedDate) 
                    VALUES 
                    (@name, @unitType, 0, 1, @isPerishable, NOW());
                    SELECT LAST_INSERT_ID();
                "
                Dim insertIngredientCmd As New MySqlCommand(insertIngredientQuery, conn)
                insertIngredientCmd.Parameters.AddWithValue("@name", txtNewName.Text.Trim())
                insertIngredientCmd.Parameters.AddWithValue("@unitType", txtStockUnit.Text.Trim())
                insertIngredientCmd.Parameters.AddWithValue("@isPerishable", If(chkPerishable.Checked, 1, 0))

                ingredientId = Convert.ToInt32(insertIngredientCmd.ExecuteScalar())
                Console.WriteLine($"Created new ingredient with ID: {ingredientId}")
            End If

            ' Check if this ingredient is already added to the product
            Dim checkProductQuery As String = "
                SELECT COUNT(*) 
                FROM product_ingredients 
                WHERE ProductID = @productId AND IngredientID = @ingredientId
            "
            Dim checkProductCmd As New MySqlCommand(checkProductQuery, conn)
            checkProductCmd.Parameters.AddWithValue("@productId", currentProductId)
            checkProductCmd.Parameters.AddWithValue("@ingredientId", ingredientId)

            Dim count As Integer = Convert.ToInt32(checkProductCmd.ExecuteScalar())

            If count > 0 Then
                MessageBox.Show("This ingredient is already added to this product.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                conn.Close()
                Return
            End If

            ' Add ingredient to product
            Dim insertProductIngredientQuery As String = "
                INSERT INTO product_ingredients 
                (ProductID, IngredientID, QuantityUsed, UnitType, CreatedDate, UpdatedDate) 
                VALUES 
                (@productId, @ingredientId, @quantity, @unit, NOW(), NOW())
            "
            Dim insertProductCmd As New MySqlCommand(insertProductIngredientQuery, conn)
            insertProductCmd.Parameters.AddWithValue("@productId", currentProductId)
            insertProductCmd.Parameters.AddWithValue("@ingredientId", ingredientId)
            insertProductCmd.Parameters.AddWithValue("@quantity", quantity)
            insertProductCmd.Parameters.AddWithValue("@unit", txtUnitNew.Text.Trim())

            insertProductCmd.ExecuteNonQuery()

            MessageBox.Show("✓ New ingredient created and added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Clear fields
            txtNewName.Clear()
            txtStockUnit.Text = "kg"
            chkPerishable.Checked = True
            txtQuantityNew.Clear()
            txtUnitNew.Clear()

            ' Refresh lists
            LoadCurrentIngredients()
            LoadAvailableIngredients()

        Catch ex As Exception
            MessageBox.Show("Error creating ingredient: " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' =======================================================
    ' REMOVE INGREDIENT
    ' =======================================================
    Private Sub RemoveIngredient_Click(sender As Object, e As EventArgs)
        Dim dgv As DataGridView = CType(Me.Controls.Find("dgvCurrentIngredients", True)(0), DataGridView)

        If dgv.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select an ingredient to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim result As DialogResult = MessageBox.Show(
            "Are you sure you want to remove this ingredient?" & vbCrLf & vbCrLf &
            "Ingredient: " & dgv.SelectedRows(0).Cells("Ingredient").Value.ToString(),
            "Confirm Remove",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        )

        If result = DialogResult.Yes Then
            Try
                openConn()

                Dim productIngredientId As Integer = Convert.ToInt32(dgv.SelectedRows(0).Cells("ProductIngredientID").Value)

                Dim deleteQuery As String = "DELETE FROM product_ingredients WHERE ProductIngredientID = @id"
                Dim cmd As New MySqlCommand(deleteQuery, conn)
                cmd.Parameters.AddWithValue("@id", productIngredientId)
                cmd.ExecuteNonQuery()

                MessageBox.Show("✓ Ingredient removed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Refresh list
                LoadCurrentIngredients()

            Catch ex As Exception
                MessageBox.Show("Error removing ingredient: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End If
    End Sub

    ' =======================================================
    ' SAVE CHANGES (UPDATE QUANTITIES/UNITS)
    ' =======================================================
    Private Sub SaveChanges_Click(sender As Object, e As EventArgs)
        Dim dgv As DataGridView = CType(Me.Controls.Find("dgvCurrentIngredients", True)(0), DataGridView)

        Try
            openConn()

            For Each row As DataGridViewRow In dgv.Rows
                If row.IsNewRow Then Continue For

                Dim productIngredientId As Integer = Convert.ToInt32(row.Cells("ProductIngredientID").Value)
                Dim quantity As Decimal = Convert.ToDecimal(row.Cells("Quantity").Value)
                Dim unit As String = row.Cells("Unit").Value.ToString()

                Dim updateQuery As String = "
                    UPDATE product_ingredients 
                    SET QuantityUsed = @quantity, UnitType = @unit, UpdatedDate = NOW()
                    WHERE ProductIngredientID = @id
                "
                Dim cmd As New MySqlCommand(updateQuery, conn)
                cmd.Parameters.AddWithValue("@quantity", quantity)
                cmd.Parameters.AddWithValue("@unit", unit)
                cmd.Parameters.AddWithValue("@id", productIngredientId)
                cmd.ExecuteNonQuery()
            Next

            MessageBox.Show("✓ All changes saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error saving changes: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' =======================================================
    ' HELPER CLASS FOR COMBOBOX ITEMS
    ' =======================================================
    Private Class IngredientItem
        Public Property Id As Integer
        Public Property Name As String

        Public Overrides Function ToString() As String
            Return Name
        End Function
    End Class

End Class