Imports MySqlConnector
Imports System.Data

Public Class FormTakeOutOrders

    Private ReadOnly connectionString As String = modDB.strConnection

    Private Sub FormTakeOutOrders_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadTakeoutOrders()
    End Sub

    Private Sub LoadTakeoutOrders()
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                Dim query As String =
                    "SELECT " &
                    "OrderID, " &
                    "ItemsOrderedCount AS Items, " &
                    "TotalAmount AS Amount, " &
                    "OrderStatus AS Status, " &
                    "DATE_FORMAT(OrderTime, '%Y-%m-%d %H:%i') AS Time " &  ' ✅ FIXED TIME FORMAT
                    "FROM orders " &
                    "WHERE OrderType = 'Takeout' " &
                    "ORDER BY OrderID DESC"

                Using cmd As New MySqlCommand(query, conn)
                    Using adapter As New MySqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        adapter.Fill(dt)

                        DataGridView1.AutoGenerateColumns = True
                        DataGridView1.DataSource = dt
                    End Using
                End Using
            End Using

            FormatGrid()
            HideOrderID()

        Catch ex As Exception
            MessageBox.Show("Error loading takeout orders: " & ex.Message)
        End Try
    End Sub

    ' =============================
    ' HIDE ORDER ID COLUMN
    ' =============================
    Private Sub HideOrderID()
        If DataGridView1.Columns.Contains("OrderID") Then
            DataGridView1.Columns("OrderID").Visible = False
        End If
    End Sub

    ' =============================
    ' FORMAT GRID
    ' =============================
    Private Sub FormatGrid()
        With DataGridView1
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .RowHeadersVisible = False
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False

            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 10)
        End With

        ' ✅ SAFE: Time is now a string, no DateTime formatting applied
        If DataGridView1.Columns.Contains("Time") Then
            DataGridView1.Columns("Time").DefaultCellStyle.Format = ""
        End If

        ' ✅ SAFE currency formatting
        If DataGridView1.Columns.Contains("Amount") Then
            DataGridView1.Columns("Amount").DefaultCellStyle.Format = "₱ #,##0.00"
        End If
    End Sub

    ' =============================
    ' PREVENT ERROR POPUPS
    ' =============================
    Private Sub DataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) _
        Handles DataGridView1.DataError
        e.ThrowException = False
    End Sub

End Class