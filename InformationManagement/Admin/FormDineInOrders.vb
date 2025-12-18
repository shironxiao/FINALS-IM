Imports MySqlConnector
Imports System.Data
Imports System.Threading.Tasks
Imports System.Drawing.Drawing2D

Public Class FormDineInOrders
    Private ReadOnly connectionString As String = modDB.strConnection
    Private _isLoading As Boolean = False
    Private _baseTitle As String = ""
    Private _dataCache As DataTable = Nothing
    Private _lastRefresh As DateTime = DateTime.MinValue
    Private ReadOnly _cacheTimeout As TimeSpan = TimeSpan.FromSeconds(30)

    Private Sub FormDineInOrders_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeModernUI()
        ConfigureGrid()
        _baseTitle = Label2.Text
        BeginLoadDineInOrders()
    End Sub

    Private Sub InitializeModernUI()
        ' Enhanced form appearance
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)

        ' Modern DataGridView styling
        With DataGridView1
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .BackgroundColor = Color.White
            .GridColor = Color.FromArgb(240, 240, 240)
            .RowTemplate.Height = 45
            .EnableHeadersVisualStyles = False
            .AllowUserToResizeRows = False

            ' Modern header style
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Padding = New Padding(5)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .ColumnHeadersHeight = 50

            ' Modern row style
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219)
            .DefaultCellStyle.SelectionForeColor = Color.White
            .DefaultCellStyle.BackColor = Color.White
            .DefaultCellStyle.ForeColor = Color.FromArgb(44, 62, 80)
            .DefaultCellStyle.Padding = New Padding(5, 8, 5, 8)

            ' Alternating row colors for better readability
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250)
            .AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185)
        End With

        ' Style the export button
        StyleButton(Export)

        ' Style the label
        Label2.Font = New Font("Segoe UI", 14, FontStyle.Bold)
        Label2.ForeColor = Color.FromArgb(44, 62, 80)
    End Sub

    Private Sub StyleButton(btn As Button)
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.BackColor = Color.FromArgb(46, 204, 113)
        btn.ForeColor = Color.White
        btn.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        btn.Cursor = Cursors.Hand
        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 174, 96)
        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(34, 153, 84)
    End Sub

    Private Async Sub BeginLoadDineInOrders()
        If _isLoading Then Return

        ' Use cached data if available and fresh
        If _dataCache IsNot Nothing AndAlso (DateTime.Now - _lastRefresh) < _cacheTimeout Then
            DataGridView1.DataSource = _dataCache
            ConfigureGrid()
            ' FIXED: Apply colors when using cached data too
            ApplyStatusColors()
            Return
        End If

        _isLoading = True
        SetLoadingState(True)

        Try
            Dim table As DataTable = Await Task.Run(Function() FetchDineInOrdersTable())

            If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return

            ' Cache the data
            _dataCache = table
            _lastRefresh = DateTime.Now

            DataGridView1.DataSource = table
            ConfigureGrid()

            ' FIXED: Force refresh and apply colors after binding is complete
            DataGridView1.Refresh()
            Application.DoEvents()
            ApplyStatusColors()

            If DataGridView1.Rows.Count > 0 Then
                DataGridView1.FirstDisplayedScrollingRowIndex = 0
            End If
        Catch ex As Exception
            If Not Me.IsDisposed Then
                MessageBox.Show("Error loading dine-in orders: " & ex.Message, "Database Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            If Not Me.IsDisposed Then SetLoadingState(False)
            _isLoading = False
        End Try
    End Sub

    Private Function FetchDineInOrdersTable() As DataTable
        ' OPTIMIZED QUERY - Removed index hints for compatibility
        Dim query As String =
            "SELECT " &
            "  o.OrderID, " &
            "  (SELECT GROUP_CONCAT(CONCAT(oi2.Quantity, 'x ', oi2.ProductName) SEPARATOR ', ') " &
            "   FROM order_items oi2 " &
            "   WHERE oi2.OrderID = o.OrderID " &
            "   LIMIT 10) AS ItemsOrdered, " &
            "  o.TotalAmount, " &
            "  o.OrderStatus AS Status, " &
            "  DATE_FORMAT(CONCAT(o.OrderDate, ' ', o.OrderTime), '%Y-%m-%d %H:%i') AS OrderDateTime " &
            "FROM orders o " &
            "WHERE o.OrderType = 'Dine-in' " &
            "ORDER BY o.OrderID DESC " &
            "LIMIT 300;"

        Using conn As New MySqlConnection(connectionString)
            conn.Open()
            Using cmd As New MySqlCommand(query, conn)
                cmd.CommandTimeout = 20
                Using adapter As New MySqlDataAdapter(cmd)
                    adapter.SelectCommand.CommandType = CommandType.Text
                    Dim table As New DataTable()
                    adapter.Fill(table)
                    Return table
                End Using
            End Using
        End Using
    End Function

    ' FIXED: Improved status color application
    Private Sub ApplyStatusColors()
        Try
            ' Add visual indicators for order status
            For Each row As DataGridViewRow In DataGridView1.Rows
                If Not row.IsNewRow AndAlso row.Cells("Status").Value IsNot Nothing Then
                    Dim status As String = row.Cells("Status").Value.ToString().Trim().ToLower()

                    ' Reset to default first
                    row.Cells("Status").Style.ForeColor = Color.FromArgb(44, 62, 80)
                    row.Cells("Status").Style.Font = New Font("Segoe UI", 9, FontStyle.Regular)

                    ' Apply status-specific colors
                    Select Case status
                        Case "completed", "paid"
                            row.Cells("Status").Style.ForeColor = Color.FromArgb(39, 174, 96)
                            row.Cells("Status").Style.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                        Case "pending", "preparing"
                            row.Cells("Status").Style.ForeColor = Color.FromArgb(241, 196, 15)
                            row.Cells("Status").Style.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                        Case "cancelled", "canceled"
                            row.Cells("Status").Style.ForeColor = Color.FromArgb(231, 76, 60)
                            row.Cells("Status").Style.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                    End Select
                End If
            Next

            ' Force redraw
            DataGridView1.InvalidateColumn(DataGridView1.Columns("Status").Index)
        Catch ex As Exception
            ' Silently handle errors in color application
            Debug.WriteLine("Error applying status colors: " & ex.Message)
        End Try
    End Sub

    ' ALTERNATIVE: Use CellFormatting event for automatic color application
    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
        Try
            If DataGridView1.Columns(e.ColumnIndex).Name = "Status" AndAlso e.Value IsNot Nothing Then
                Dim status As String = e.Value.ToString().Trim().ToLower()

                Select Case status
                    Case "completed", "paid"
                        e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96)
                        e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                    Case "pending", "preparing"
                        e.CellStyle.ForeColor = Color.FromArgb(241, 196, 15)
                        e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                    Case "cancelled", "canceled"
                        e.CellStyle.ForeColor = Color.FromArgb(231, 76, 60)
                        e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                End Select
            End If
        Catch ex As Exception
            ' Silently handle formatting errors
        End Try
    End Sub

    Private Sub SetLoadingState(isLoading As Boolean)
        Try
            DataGridView1.Enabled = Not isLoading
            Export.Enabled = Not isLoading

            If isLoading Then
                Label2.Text = _baseTitle & " ⏳"
                Export.Text = "   Loading..."
            Else
                Label2.Text = _baseTitle
                Export.Text = "   Export"
            End If
        Catch
        End Try
    End Sub

    Private Sub ConfigureGrid()
        With DataGridView1
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AllowUserToOrderColumns = False
            .AllowUserToAddRows = False
        End With

        ' Optimized column configuration
        If DataGridView1.Columns.Contains("OrderID") Then
            With DataGridView1.Columns("OrderID")
                .HeaderText = "Order #"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .DefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
            End With
        End If

        If DataGridView1.Columns.Contains("ItemsOrdered") Then
            With DataGridView1.Columns("ItemsOrdered")
                .HeaderText = "Items Ordered"
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                .MinimumWidth = 300
                .DefaultCellStyle.WrapMode = DataGridViewTriState.False
            End With
        End If

        If DataGridView1.Columns.Contains("TotalAmount") Then
            With DataGridView1.Columns("TotalAmount")
                .HeaderText = "Amount"
                .Width = 130
                .DefaultCellStyle.Format = "₱#,##0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94)
            End With
        End If

        If DataGridView1.Columns.Contains("Status") Then
            With DataGridView1.Columns("Status")
                .HeaderText = "Status"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
        End If

        If DataGridView1.Columns.Contains("OrderDateTime") Then
            With DataGridView1.Columns("OrderDateTime")
                .HeaderText = "Date & Time"
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
        End If
    End Sub

    Private Sub Export_Click(sender As Object, e As EventArgs) Handles Export.Click
        ExportToCSV()
    End Sub

    Private Sub ExportToCSV()
        If DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            Dim saveDialog As New SaveFileDialog With {
                .Filter = "CSV Files (*.csv)|*.csv",
                .FileName = String.Format("DineIn_Orders_{0:yyyyMMdd_HHmmss}.csv", DateTime.Now),
                .Title = "Export Dine-In Orders Report"
            }

            If saveDialog.ShowDialog() = DialogResult.OK Then
                Export.Enabled = False
                Export.Text = "   Exporting..."

                Using writer As New IO.StreamWriter(saveDialog.FileName, False, System.Text.Encoding.UTF8)
                    ' Write headers
                    Dim headers As New List(Of String)
                    For Each column As DataGridViewColumn In DataGridView1.Columns
                        If column.Visible Then
                            headers.Add(EscapeCSV(column.HeaderText))
                        End If
                    Next
                    writer.WriteLine(String.Join(",", headers))

                    ' Write data rows (optimized)
                    For Each row As DataGridViewRow In DataGridView1.Rows
                        If Not row.IsNewRow Then
                            Dim values As New List(Of String)
                            For Each column As DataGridViewColumn In DataGridView1.Columns
                                If column.Visible Then
                                    Dim cellValue As String = GetCellValueAsString(row.Cells(column.Index))
                                    values.Add(EscapeCSV(cellValue))
                                End If
                            Next
                            writer.WriteLine(String.Join(",", values))
                        End If
                    Next
                End Using

                MessageBox.Show("Export completed successfully!" & vbCrLf &
                              DataGridView1.Rows.Count & " orders exported.",
                              "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Process.Start("explorer.exe", String.Format("/select,""{0}""", saveDialog.FileName))
            End If

        Catch ex As Exception
            MessageBox.Show("Export failed: " & ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Export.Enabled = True
            Export.Text = "   Export"
        End Try
    End Sub

    Private Function GetCellValueAsString(cell As DataGridViewCell) As String
        If cell.Value Is Nothing Then Return ""
        Dim value As String = cell.Value.ToString()
        Return value.Replace("₱", "").Trim()
    End Function

    Private Function EscapeCSV(value As String) As String
        If String.IsNullOrEmpty(value) Then Return ""
        If value.Contains(",") OrElse value.Contains("""") OrElse value.Contains(vbCrLf) Then
            Return """" & value.Replace("""", """""") & """"
        End If
        Return value
    End Function

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        _dataCache?.Dispose()
        MyBase.OnFormClosing(e)
    End Sub
End Class