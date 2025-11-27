Imports MySqlConnector
Imports System.Text

Public Class FormCustomerHistory

    Private ReadOnly connectionString As String = modDB.strConnection

    Private Sub FormCustomerHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCustomerHistory()
    End Sub

    Private Sub LoadCustomerHistory()

        Dim query As String =
            "SELECT o.OrderID,
                    o.OrderDate,
                    o.OrderType,
                    o.TotalAmount,
                    o.OrderStatus,
                    GROUP_CONCAT(CONCAT(oi.ProductName, ' (', oi.Quantity, ')') SEPARATOR ', ') AS Items
             FROM orders o
             LEFT JOIN order_items oi ON o.OrderID = oi.OrderID
             GROUP BY o.OrderID
             ORDER BY o.OrderDate DESC;"

        Try
            Using conn As New MySqlConnection(connectionString)
                Using cmd As New MySqlCommand(query, conn)
                    conn.Open()

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        DataGridView1.Rows.Clear()

                        While reader.Read()

                            ' Format date safely
                            Dim orderDate As String = ""
                            If Not reader.IsDBNull(reader.GetOrdinal("OrderDate")) Then
                                orderDate = CDate(reader("OrderDate")).ToString("yyyy-MM-dd")
                            End If

                            ' Add row to DataGridView
                            DataGridView1.Rows.Add(
                                orderDate,
                                reader("OrderID").ToString(),
                                reader("OrderType").ToString(),
                                reader("Items").ToString(),
                                Convert.ToDecimal(reader("TotalAmount")).ToString("0.00"),
                                reader("OrderStatus").ToString()
                            )
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading customer history:" & vbCrLf & ex.Message,
                            "Database Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try

    End Sub

End Class
