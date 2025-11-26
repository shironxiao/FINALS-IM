Imports MySqlConnector

Public Class BatchManagement
    Private _ingredientID As Integer
    Private _ingredientName As String

    ' Constructor
    Public Sub New(ingredientID As Integer, ingredientName As String)
        InitializeComponent()
        _ingredientID = ingredientID
        _ingredientName = ingredientName
    End Sub

    Private Sub BatchManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.Text = "Batch Management - " & _ingredientName
            lblIngredientName.Text = _ingredientName
            LoadBatchData()
        Catch ex As Exception
            MessageBox.Show("Error loading form: " & ex.Message,
                          "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Load all batches for this ingredient
    Private Sub LoadBatchData()
        Try
            openConn()

            Dim sql As String = "
                SELECT 
                    BatchID AS 'Batch ID',
                    BatchNumber AS 'Batch Number',
                    StockQuantity AS 'Current Stock',
                    OriginalQuantity AS 'Original Qty',
                    UnitType AS 'Unit',
                    CostPerUnit AS 'Cost/Unit',
                    (StockQuantity * CostPerUnit) AS 'Total Cost',
                    PurchaseDate AS 'Purchase Date',
                    ExpirationDate AS 'Expiration',
                    CASE 
                        WHEN ExpirationDate IS NULL THEN NULL
                        ELSE DATEDIFF(ExpirationDate, CURDATE())
                    END AS 'Days Left',
                    CASE 
                        WHEN BatchStatus = 'Expired' THEN 'EXPIRED'
                        WHEN BatchStatus = 'Depleted' THEN 'Depleted'
                        WHEN ExpirationDate IS NULL THEN 'No Expiry'
                        WHEN ExpirationDate <= CURDATE() THEN 'EXPIRED'
                        WHEN DATEDIFF(ExpirationDate, CURDATE()) <= 3 THEN 'CRITICAL'
                        WHEN DATEDIFF(ExpirationDate, CURDATE()) <= 7 THEN 'WARNING'
                        WHEN DATEDIFF(ExpirationDate, CURDATE()) <= 14 THEN 'Monitor'
                        ELSE 'Fresh'
                    END AS 'Alert',
                    BatchStatus AS 'Status',
                    StorageLocation AS 'Storage Location',
                    ROUND((StockQuantity / OriginalQuantity) * 100, 1) AS 'Remaining %',
                    Notes
                FROM inventory_batches
                WHERE IngredientID = @ingredientID
                ORDER BY 
                    CASE BatchStatus
                        WHEN 'Active' THEN 1
                        WHEN 'Expired' THEN 2
                        WHEN 'Depleted' THEN 3
                        ELSE 4
                    END,
                    CASE WHEN ExpirationDate IS NULL THEN 1 ELSE 0 END,
                    ExpirationDate ASC,
                    PurchaseDate ASC
            "

            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@ingredientID", _ingredientID)

            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            dgvBatches.DataSource = Nothing
            dgvBatches.Columns.Clear()
            dgvBatches.DataSource = dt

            FormatBatchGrid()
            ColorCodeBatches()
            LoadBatchStatistics()

        Catch ex As Exception
            MessageBox.Show("Error loading batches: " & ex.Message,
                          "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    ' Format the batch grid
    Private Sub FormatBatchGrid()
        Try
            With dgvBatches
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                .RowTemplate.Height = 35
                .DefaultCellStyle.Font = New Font("Segoe UI", 9)
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250)
                .ReadOnly = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect

                ' Hide columns
                If .Columns.Contains("Batch ID") Then .Columns("Batch ID").Visible = False
                If .Columns.Contains("Notes") Then .Columns("Notes").Visible = False

                ' Format currency
                If .Columns.Contains("Cost/Unit") Then
                    .Columns("Cost/Unit").DefaultCellStyle.Format = "₱#,##0.00"
                    .Columns("Cost/Unit").ReadOnly = True
                End If

                If .Columns.Contains("Total Cost") Then
                    .Columns("Total Cost").DefaultCellStyle.Format = "₱#,##0.00"
                    .Columns("Total Cost").ReadOnly = True
                End If

                ' Format dates
                If .Columns.Contains("Purchase Date") Then
                    .Columns("Purchase Date").DefaultCellStyle.Format = "MMM dd, yyyy"
                    .Columns("Purchase Date").ReadOnly = True
                End If

                If .Columns.Contains("Expiration") Then
                    .Columns("Expiration").DefaultCellStyle.Format = "MMM dd, yyyy"
                    .Columns("Expiration").ReadOnly = True
                End If

                ' Center alignment
                Dim centerColumns() As String = {"Current Stock", "Unit", "Days Left", "Alert", "Status", "Remaining %", "Storage Location"}
                For Each colName In centerColumns
                    If .Columns.Contains(colName) Then
                        .Columns(colName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .Columns(colName).ReadOnly = True
                    End If
                Next

                ' Bold columns
                If .Columns.Contains("Alert") Then
                    .Columns("Alert").DefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                End If

                If .Columns.Contains("Status") Then
                    .Columns("Status").DefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                End If
            End With

            ' Add discard button
            If Not dgvBatches.Columns.Contains("btnDiscard") Then
                Dim btnDiscard As New DataGridViewButtonColumn()
                btnDiscard.Name = "btnDiscard"
                btnDiscard.HeaderText = "Actions"
                btnDiscard.Text = "Discard"
                btnDiscard.UseColumnTextForButtonValue = True
                btnDiscard.Width = 100
                btnDiscard.FlatStyle = FlatStyle.Flat
                dgvBatches.Columns.Add(btnDiscard)
            End If

        Catch ex As Exception
            MessageBox.Show("Error formatting grid: " & ex.Message)
        End Try
    End Sub

    ' Color code batches
    Private Sub ColorCodeBatches()
        Try
            For Each row As DataGridViewRow In dgvBatches.Rows
                If Not row.IsNewRow Then
                    ' Color Alert column
                    If row.Cells("Alert").Value IsNot Nothing Then
                        Dim alert As String = row.Cells("Alert").Value.ToString()

                        Select Case alert
                            Case "EXPIRED"
                                row.Cells("Alert").Style.BackColor = Color.FromArgb(139, 0, 0)
                                row.Cells("Alert").Style.ForeColor = Color.White
                            Case "CRITICAL"
                                row.Cells("Alert").Style.BackColor = Color.FromArgb(220, 53, 69)
                                row.Cells("Alert").Style.ForeColor = Color.White
                            Case "WARNING"
                                row.Cells("Alert").Style.BackColor = Color.FromArgb(255, 193, 7)
                                row.Cells("Alert").Style.ForeColor = Color.Black
                            Case "Monitor"
                                row.Cells("Alert").Style.BackColor = Color.FromArgb(255, 235, 59)
                                row.Cells("Alert").Style.ForeColor = Color.Black
                            Case "Fresh"
                                row.Cells("Alert").Style.BackColor = Color.FromArgb(40, 167, 69)
                                row.Cells("Alert").Style.ForeColor = Color.White
                        End Select
                    End If

                    ' Color Status column
                    If row.Cells("Status").Value IsNot Nothing Then
                        Dim status As String = row.Cells("Status").Value.ToString()

                        Select Case status
                            Case "Active"
                                row.Cells("Status").Style.BackColor = Color.FromArgb(40, 167, 69)
                                row.Cells("Status").Style.ForeColor = Color.White
                            Case "Depleted"
                                row.Cells("Status").Style.BackColor = Color.Gray
                                row.Cells("Status").Style.ForeColor = Color.White
                            Case "Expired"
                                row.Cells("Status").Style.BackColor = Color.FromArgb(220, 53, 69)
                                row.Cells("Status").Style.ForeColor = Color.White
                            Case "Discarded"
                                row.Cells("Status").Style.BackColor = Color.DarkGray
                                row.Cells("Status").Style.ForeColor = Color.White
                        End Select
                    End If

                    ' Color Remaining %
                    If row.Cells("Remaining %").Value IsNot Nothing AndAlso
                       Not IsDBNull(row.Cells("Remaining %").Value) Then
                        Dim remaining As Decimal = Convert.ToDecimal(row.Cells("Remaining %").Value)

                        If remaining <= 20 Then
                            row.Cells("Remaining %").Style.BackColor = Color.FromArgb(220, 53, 69)
                            row.Cells("Remaining %").Style.ForeColor = Color.White
                        ElseIf remaining <= 50 Then
                            row.Cells("Remaining %").Style.BackColor = Color.FromArgb(255, 193, 7)
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            MessageBox.Show("Error color coding: " & ex.Message)
        End Try
    End Sub

    ' Load statistics
    Private Sub LoadBatchStatistics()
        Try
            openConn()

            ' Total Stock
            Dim sqlTotal As String = "
                SELECT COALESCE(SUM(StockQuantity), 0)
                FROM inventory_batches
                WHERE IngredientID = @id AND BatchStatus = 'Active'
            "
            Dim cmdTotal As New MySqlCommand(sqlTotal, conn)
            cmdTotal.Parameters.AddWithValue("@id", _ingredientID)
            lblTotalStock.Text = Convert.ToDecimal(cmdTotal.ExecuteScalar()).ToString("#,##0.00")

            ' Active Batches
            Dim sqlActive As String = "
                SELECT COUNT(*)
                FROM inventory_batches
                WHERE IngredientID = @id AND BatchStatus = 'Active'
            "
            Dim cmdActive As New MySqlCommand(sqlActive, conn)
            cmdActive.Parameters.AddWithValue("@id", _ingredientID)
            lblActiveBatches.Text = cmdActive.ExecuteScalar().ToString()

            ' Total Value
            Dim sqlValue As String = "
                SELECT COALESCE(SUM(StockQuantity * CostPerUnit), 0)
                FROM inventory_batches
                WHERE IngredientID = @id AND BatchStatus = 'Active'
            "
            Dim cmdValue As New MySqlCommand(sqlValue, conn)
            cmdValue.Parameters.AddWithValue("@id", _ingredientID)
            lblTotalValue.Text = "₱" & Convert.ToDecimal(cmdValue.ExecuteScalar()).ToString("#,##0.00")

            ' Expiring Count
            Dim sqlExpiring As String = "
                SELECT COUNT(*)
                FROM inventory_batches
                WHERE IngredientID = @id 
                  AND BatchStatus = 'Active'
                  AND ExpirationDate IS NOT NULL
                  AND DATEDIFF(ExpirationDate, CURDATE()) <= 7
            "
            Dim cmdExpiring As New MySqlCommand(sqlExpiring, conn)
            cmdExpiring.Parameters.AddWithValue("@id", _ingredientID)
            Dim expiringCount As Integer = Convert.ToInt32(cmdExpiring.ExecuteScalar())

            lblExpiringCount.Text = expiringCount.ToString()
            lblExpiringCount.ForeColor = If(expiringCount > 0, Color.Red, Color.Green)

        Catch ex As Exception
            MessageBox.Show("Error loading statistics: " & ex.Message)
        Finally
            closeConn()
        End Try
    End Sub

    ' Handle button clicks
    Private Sub dgvBatches_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBatches.CellContentClick
        Try
            If e.RowIndex >= 0 AndAlso e.ColumnIndex = dgvBatches.Columns("btnDiscard").Index Then
                Dim batchID As Integer = Convert.ToInt32(dgvBatches.Rows(e.RowIndex).Cells("Batch ID").Value)
                Dim batchNumber As String = dgvBatches.Rows(e.RowIndex).Cells("Batch Number").Value.ToString()
                Dim currentStock As Decimal = Convert.ToDecimal(dgvBatches.Rows(e.RowIndex).Cells("Current Stock").Value)
                Dim batchStatus As String = dgvBatches.Rows(e.RowIndex).Cells("Status").Value.ToString()

                If batchStatus = "Discarded" OrElse batchStatus = "Depleted" Then
                    MessageBox.Show("This batch is already " & batchStatus & " and cannot be discarded again.",
                                  "Cannot Discard", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Dim result As DialogResult = MessageBox.Show(
                    "Are you sure you want to discard batch " & batchNumber & "?" & vbCrLf &
                    "Current stock: " & currentStock & vbCrLf & vbCrLf &
                    "This will mark the batch as discarded and remove it from active inventory.",
                    "Confirm Discard", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

                If result = DialogResult.Yes Then
                    DiscardBatch(batchID, batchNumber, currentStock)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error processing action: " & ex.Message, "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Discard batch
    Private Sub DiscardBatch(batchID As Integer, batchNumber As String, currentStock As Decimal)
        Try
            openConn()
            Dim transaction As MySqlTransaction = conn.BeginTransaction()

            Try
                ' Use stored procedure to update batch
                Dim cmd As New MySqlCommand("UpdateBatchStock", conn, transaction)
                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.AddWithValue("@p_batch_id", batchID)
                cmd.Parameters.AddWithValue("@p_quantity_change", -currentStock)
                cmd.Parameters.AddWithValue("@p_transaction_type", "Discard")
                cmd.Parameters.AddWithValue("@p_reference_id", Nothing)
                cmd.Parameters.AddWithValue("@p_performed_by", "System User")
                cmd.Parameters.AddWithValue("@p_reason", "Manual Discard")
                cmd.Parameters.AddWithValue("@p_notes", "Batch " & batchNumber & " discarded on " & DateTime.Now.ToString())

                cmd.ExecuteNonQuery()

                ' Update batch status to Discarded
                Dim sqlUpdate As String = "UPDATE inventory_batches SET BatchStatus = 'Discarded' WHERE BatchID = @id"
                Dim cmdUpdate As New MySqlCommand(sqlUpdate, conn, transaction)
                cmdUpdate.Parameters.AddWithValue("@id", batchID)
                cmdUpdate.ExecuteNonQuery()

                transaction.Commit()

                MessageBox.Show("Batch discarded successfully!", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadBatchData()

            Catch ex As Exception
                transaction.Rollback()
                Throw
            End Try

        Catch ex As Exception
            MessageBox.Show("Error discarding batch: " & ex.Message, "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    ' Add new batch for this ingredient
    Private Sub btnAddBatch_Click(sender As Object, e As EventArgs) Handles btnAddBatch.Click
        Try
            ' Determine the unit type from any existing active batch, or fall back to ingredient's unit
            Dim unitType As String = ""

            If dgvBatches.DataSource IsNot Nothing AndAlso dgvBatches.Rows.Count > 0 AndAlso
               dgvBatches.Columns.Contains("Unit") Then
                For Each row As DataGridViewRow In dgvBatches.Rows
                    If Not row.IsNewRow AndAlso row.Cells("Unit").Value IsNot Nothing Then
                        unitType = row.Cells("Unit").Value.ToString()
                        Exit For
                    End If
                Next
            End If

            ' If we still don't have a unit type, default to empty string; the AddNewBatch
            ' form can handle prompting/validation as needed.

            Dim addForm As New AddNewBatch(_ingredientID, _ingredientName, unitType)
            addForm.StartPosition = FormStartPosition.CenterScreen

            If addForm.ShowDialog() = DialogResult.OK Then
                LoadBatchData()
            End If
        Catch ex As Exception
            MessageBox.Show("Error opening add batch form: " & ex.Message)
        End Try
    End Sub

    ' View history
    Private Sub btnViewHistory_Click(sender As Object, e As EventArgs) Handles btnViewHistory.Click
        Try
            openConn()

            Dim sql As String = "
                SELECT 
                    bt.TransactionDate AS 'Date/Time',
                    bt.TransactionType AS 'Type',
                    ib.BatchNumber AS 'Batch #',
                    bt.QuantityChanged AS 'Qty Change',
                    bt.StockBefore AS 'Before',
                    bt.StockAfter AS 'After',
                    bt.ReferenceID AS 'Reference',
                    bt.PerformedBy AS 'Performed By',
                    bt.Notes
                FROM batch_transactions bt
                JOIN inventory_batches ib ON bt.BatchID = ib.BatchID
                WHERE ib.IngredientID = @id
                ORDER BY bt.TransactionDate DESC
                LIMIT 100
            "

            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@id", _ingredientID)

            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                Dim historyForm As New Form()
                historyForm.Text = "Transaction History - " & _ingredientName
                historyForm.Size = New Size(1000, 600)
                historyForm.StartPosition = FormStartPosition.CenterParent

                Dim dgv As New DataGridView()
                dgv.Dock = DockStyle.Fill
                dgv.DataSource = dt
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                dgv.ReadOnly = True
                dgv.AllowUserToAddRows = False
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect

                historyForm.Controls.Add(dgv)
                historyForm.ShowDialog()
            Else
                MessageBox.Show("No transaction history found.", "History",
                              MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading history: " & ex.Message, "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            closeConn()
        End Try
    End Sub

    ' Close
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class