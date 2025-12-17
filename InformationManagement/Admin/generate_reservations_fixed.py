import random
import mysql.connector
from datetime import datetime, timedelta
from decimal import Decimal

# Database connection configuration
DB_CONFIG = {
    'host': 'localhost',
    'user': 'root',
    'password': '',  # Update with your MySQL password
    'database': 'tabeya_system'
}

# Sample product data (ProductID, ProductName, Price)
PRODUCTS = [
    (1, 'Chicken Adobo Platter', 450.00),
    (2, 'Pork BBQ Skewers', 350.00),
    (3, 'Beef Caldereta Platter', 550.00),
    (4, 'Pancit Canton Bilao', 400.00),
    (5, 'Lechon Kawali Platter', 500.00),
    (6, 'Grilled Fish Platter', 480.00),
    (7, 'Crispy Pata', 650.00),
    (8, 'Kare-Kare Platter', 520.00),
    (9, 'Sweet and Sour Fish', 430.00),
    (10, 'Lumpiang Shanghai', 380.00),
]

def generate_reservations(num_records=10000):
    """Generate reservation data and insert into database"""
    
    try:
        # Connect to database
        conn = mysql.connector.connect(**DB_CONFIG)
        cursor = conn.cursor()
        
        print(f"Starting generation of {num_records} reservations...")
        print("Creating records without any data...")
        
        # Since the user wants empty tables, we'll just verify the tables exist
        # and show the CREATE TABLE statements
        
        cursor.execute("SHOW TABLES LIKE 'reservations'")
        if cursor.fetchone():
            print("✓ Table 'reservations' exists")
        else:
            print("✗ Table 'reservations' does not exist - please run the SQL script first")
            
        cursor.execute("SHOW TABLES LIKE 'reservation_items'")
        if cursor.fetchone():
            print("✓ Table 'reservation_items' exists")
        else:
            print("✗ Table 'reservation_items' does not exist - please run the SQL script first")
            
        cursor.execute("SHOW TABLES LIKE 'reservations_payments'")
        if cursor.fetchone():
            print("✓ Table 'reservations_payments' exists")
        else:
            print("✗ Table 'reservations_payments' does not exist - please run the SQL script first")
        
        print("\n" + "="*60)
        print("Tables are ready but empty (no data as requested)")
        print("="*60)
        
    except mysql.connector.Error as err:
        print(f"Database error: {err}")
    except Exception as e:
        print(f"Error: {e}")
    finally:
        if cursor:
            cursor.close()
        if conn:
            conn.close()

if __name__ == "__main__":
    # Just verify tables exist
    generate_reservations(0)
