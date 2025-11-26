Imports System.Diagnostics
Imports MySqlConnector

Module OrderDataMaintenance

    Private Const SnapshotTriggerName As String = "trg_order_items_snapshot"
    Private snapshotInfrastructureEnsured As Boolean = False

    ''' <summary>
    ''' Ensures order item prices are captured at order creation time by creating (if needed)
    ''' the price snapshot trigger and backfilling existing rows that are missing a unit price.
    ''' </summary>
    Public Sub EnsureOrderItemPriceSnapshotInfrastructure()
        If snapshotInfrastructureEnsured Then Return

        Try
            Using maintenanceConn As New MySqlConnection(strConnection)
                maintenanceConn.Open()

                If Not TableExists(maintenanceConn, "order_items") _
                    OrElse Not TableExists(maintenanceConn, "products") Then
                    snapshotInfrastructureEnsured = True
                    Return
                End If

                EnsureSnapshotTrigger(maintenanceConn)
                BackfillMissingUnitPrices(maintenanceConn)
            End Using

            snapshotInfrastructureEnsured = True
        Catch ex As Exception
            Debug.WriteLine($"[OrderDataMaintenance] Failed to ensure snapshot trigger: {ex.Message}")
        End Try
    End Sub

    Private Function TableExists(connection As MySqlConnection, tableName As String) As Boolean
        Const sql As String = "
            SELECT COUNT(*) 
            FROM information_schema.tables
            WHERE table_schema = DATABASE()
              AND LOWER(table_name) = LOWER(@tableName)
        "

        Using cmd As New MySqlCommand(sql, connection)
            cmd.Parameters.AddWithValue("@tableName", tableName)
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            Return count > 0
        End Using
    End Function

    Private Sub EnsureSnapshotTrigger(connection As MySqlConnection)
        Const checkSql As String = "
            SELECT COUNT(*) 
            FROM information_schema.triggers
            WHERE trigger_schema = DATABASE()
              AND trigger_name = @triggerName
        "

        Using checkCmd As New MySqlCommand(checkSql, connection)
            checkCmd.Parameters.AddWithValue("@triggerName", SnapshotTriggerName)
            Dim exists As Boolean = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0
            If exists Then Return
        End Using

        ' Remove any existing trigger with the same name to avoid duplicate definitions.
        Using dropCmd As New MySqlCommand($"DROP TRIGGER IF EXISTS {SnapshotTriggerName};", connection)
            dropCmd.ExecuteNonQuery()
        End Using

        Const createSql As String = "
            CREATE TRIGGER trg_order_items_snapshot
            BEFORE INSERT ON order_items
            FOR EACH ROW
            BEGIN
                DECLARE snapshotPrice DECIMAL(12, 2);

                IF NEW.UnitPrice IS NULL OR NEW.UnitPrice <= 0 THEN
                    SELECT Price INTO snapshotPrice
                    FROM products
                    WHERE ProductID = NEW.ProductID
                    LIMIT 1;

                    SET NEW.UnitPrice = COALESCE(snapshotPrice, 0);
                END IF;
            END
        "

        Using createCmd As New MySqlCommand(createSql, connection)
            createCmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub BackfillMissingUnitPrices(connection As MySqlConnection)
        Const backfillSql As String = "
            UPDATE order_items oi
            JOIN products p ON oi.ProductID = p.ProductID
            SET oi.UnitPrice = p.Price
            WHERE oi.UnitPrice IS NULL OR oi.UnitPrice <= 0
        "

        Using cmd As New MySqlCommand(backfillSql, connection)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

End Module


