import random
import mysql.connector
from datetime import datetime, timedelta
from decimal import Decimal

# Database connection configuration
DB_CONFIG = {
    'host': 'localhost',
    'user': 'root',
    'password': '',  # Update with your MySQL password if you have one
    'database': 'tabeya_system'
}

# Sample products (ProductID, ProductName, Price)
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
    (11, 'Mixed Vegetable Platter', 320.00),
    (12, 'Fruit Salad Bowl', 280.00),
    (13, 'Leche Flan Tray', 350.00),
    (14, 'Buko Pandan Bowl', 300.00),
    (15, 'Spaghetti Bilao', 420.00),
    (16, 'Palabok Bilao', 380.00),
    (17, 'Grilled Chicken Platter', 480.00),
    (18, 'Sinigang na Baboy', 460.00),
    (19, 'Bicol Express Platter', 440.00),
    (20, 'Sisig Platter', 490.00)
]

EVENT_TYPES = [
    'Birthday Party', 'Wedding Reception', 'Corporate Event', 'Anniversary',
    'Graduation Party', 'Family Reunion', 'Christening', 'Debut',
    'Business Meeting', 'Holiday Party', 'Engagement Party', 'Baby Shower'
]

SPECIAL_REQUESTS = [
    'Need extra chairs', 'Vegetarian options required', 'Kids menu needed',
    'Halal food please', 'Less spicy', 'Extra rice', 'Party decorations allowed',
    'Early setup needed', 'Late cleanup ok', 'Wheelchair accessible area',
    None, None, None  # 30% no special requests
]

def generate_phone_number():
    """Generate Philippine mobile number"""
    prefixes = ['0917', '0918', '0919', '0920', '0921', '0922', '0923', '0925', '0926', '0927']
    return f"{random.choice(prefixes)}{random.randint(1000000, 9999999)}"

def generate_event_datetime():
    """Generate future event date and time"""
    days_ahead = random.randint(7, 180)  # 7 days to 6 months ahead
    event_date = datetime.now().date() + timedelta(days=days_ahead)
    
    # Event times between 10 AM and 9 PM
    hour = random.randint(10, 21)
    minute = random.choice([0, 15, 30, 45])
    event_time = f"{hour:02d}:{minute:02d}:00"
    
    return event_date, event_time

def generate_reservations(num_records=10000):
    """Generate reservation data matching VB.NET application schema"""
    
    conn = None
    cursor = None
    
    try:
        # Connect to database
        print("Connecting to database...")
        conn = mysql.connector.connect(**DB_CONFIG)
        cursor = conn.cursor()
        
        print(f"\nStarting generation of {num_records} reservations...")
        print("="*70)
        
        for i in range(1, num_records + 1):
            try:
                # Generate customer info
                customer_id = random.randint(1, 1000)  # Assuming you have customers
                
                # Get customer name or generate one
                try:
                    cursor.execute(
                        "SELECT FirstName, LastName FROM customers WHERE CustomerID = %s LIMIT 1",
                        (customer_id,)
                    )
                    result = cursor.fetchone()
                    if result:
                        full_name = f"{result[0]} {result[1]}"
                    else:
                        full_name = f"Customer {i}"
                except:
                    full_name = f"Customer {i}"
                
                # Generate staff ID
                assigned_staff_id = random.randint(1, 100)
                
                # Contact number
                contact_number = generate_phone_number()
                
                # Reservation type (70% Online, 30% Walk-in)
                if random.random() < 0.7:
                    reservation_type = 'Online'
                    payment_source = 'Website'
                else:
                    reservation_type = 'Walk-in'
                    payment_source = 'POS'
                
                # Event details
                event_type = random.choice(EVENT_TYPES)
                event_date, event_time = generate_event_datetime()
                number_of_guests = random.randint(10, 200)
                
                # Delivery option (60% Delivery, 40% Pickup)
                if random.random() < 0.6:
                    delivery_option = 'Delivery'
                    delivery_address = f"Barangay {random.choice(['Poblacion', 'Sabang', 'Mangcamagong'])}, Vinzons, Camarines Norte"
                else:
                    delivery_option = 'Pickup'
                    delivery_address = None
                
                # Status (70% Confirmed, 20% Pending, 10% Completed)
                status_rand = random.random()
                if status_rand < 0.70:
                    reservation_status = 'Confirmed'
                elif status_rand < 0.90:
                    reservation_status = 'Pending'
                else:
                    reservation_status = 'Completed'
                
                special_requests = random.choice(SPECIAL_REQUESTS)
                
                # Reservation date (1-30 days ago)
                reservation_date = datetime.now() - timedelta(days=random.randint(1, 30))
                
                # Generate product selection (1-5 items)
                num_items = random.randint(1, 5)
                selected_products = random.sample(PRODUCTS, min(num_items, len(PRODUCTS)))
                product_names = [p[1] for p in selected_products]
                product_selection = ", ".join(product_names)
                
                # Insert reservation
                reservation_query = """
                    INSERT INTO reservations (
                        CustomerID, FullName, AssignedStaffID, ContactNumber,
                        ReservationType, EventType, EventDate, EventTime,
                        NumberOfGuests, ProductSelection, SpecialRequests,
                        ReservationStatus, ReservationDate, DeliveryAddress, DeliveryOption
                    ) VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)
                """
                
                cursor.execute(reservation_query, (
                    customer_id, full_name, assigned_staff_id, contact_number,
                    reservation_type, event_type, event_date, event_time,
                    number_of_guests, product_selection, special_requests,
                    reservation_status, reservation_date, delivery_address, delivery_option
                ))
                
                reservation_id = cursor.lastrowid
                
                # Generate reservation items
                total_amount = Decimal('0.00')
                
                for product in selected_products:
                    product_id, product_name, unit_price = product
                    quantity = random.randint(1, 5)
                    total_price = Decimal(str(unit_price)) * quantity
                    
                    # Insert reservation item
                    item_query = """
                        INSERT INTO reservation_items (
                            ReservationID, ProductName, Quantity, UnitPrice, TotalPrice
                        ) VALUES (%s, %s, %s, %s, %s)
                    """
                    
                    cursor.execute(item_query, (
                        reservation_id, product_name, quantity,
                        float(unit_price), float(total_price)
                    ))
                    
                    total_amount += total_price
                
                # Generate payment record
                if reservation_type == 'Online':
                    if random.random() < 0.95:
                        payment_method = 'GCash'
                        proof_of_payment = 'uploads/gcash_receipts/2025/12/gcashImage.png'
                        receipt_filename = 'gcashImage.png'
                        transaction_id = f"GCASH-{random.randint(100000000000, 999999999999)}"
                    else:
                        payment_method = 'COD'
                        proof_of_payment = None
                        receipt_filename = None
                        transaction_id = None
                else:
                    payment_method = 'Cash'
                    proof_of_payment = None
                    receipt_filename = None
                    transaction_id = f"CASH-{reservation_id}-{int(datetime.now().timestamp())}"
                
                # Payment status based on reservation status
                if reservation_status == 'Completed':
                    payment_status = 'Completed'
                    amount_paid = float(total_amount)
                elif reservation_status == 'Confirmed':
                    if random.random() < 0.6:
                        payment_status = 'Paid'
                        amount_paid = float(total_amount)
                    else:
                        payment_status = 'Partial'
                        amount_paid = float(total_amount) * 0.5
                else:  # Pending
                    payment_status = 'Pending'
                    amount_paid = 0.00
                
                # Insert payment (note: table is reservation_payments, singular reservation)
                payment_query = """
                    INSERT INTO reservation_payments (
                        ReservationID, PaymentDate, PaymentMethod, PaymentStatus,
                        AmountPaid, PaymentSource, ProofOfPayment, ReceiptFileName, TransactionID
                    ) VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s)
                """
                
                cursor.execute(payment_query, (
                    reservation_id, reservation_date, payment_method, payment_status,
                    amount_paid, payment_source, proof_of_payment, receipt_filename, transaction_id
                ))
                
                # Commit every 100 records
                if i % 100 == 0:
                    conn.commit()
                    print(f"[OK] Generated {i:,} / {num_records:,} reservations...")
                    
            except Exception as e:
                print(f"[ERROR] Error generating reservation {i}: {e}")
                conn.rollback()
                continue
        
        # Final commit
        conn.commit()
        
        print("="*70)
        print(f"\n[SUCCESS] Successfully generated {num_records:,} reservations!")
        print(f"\nSummary:")
        print(f"  - Reservations: {num_records:,} records")
        print(f"  - Reservation Items: ~{num_records * 3:,} records (avg 3 per reservation)")
        print(f"  - Payments: {num_records:,} records")
        print("\nStatus Distribution:")
        print(f"  - Confirmed: ~{int(num_records * 0.7):,} (70%)")
        print(f"  - Pending: ~{int(num_records * 0.2):,} (20%)")
        print(f"  - Completed: ~{int(num_records * 0.1):,} (10%)")
        print("="*70)
        
    except mysql.connector.Error as err:
        print(f"\n[ERROR] Database error: {err}")
        if conn:
            conn.rollback()
    except Exception as e:
        print(f"\n[ERROR] Error: {e}")
        if conn:
            conn.rollback()
    finally:
        if cursor:
            cursor.close()
        if conn:
            conn.close()
        print("\n[OK] Database connection closed.")

if __name__ == "__main__":
    print("\n" + "="*70)
    print("     RESERVATION DATA GENERATOR FOR TABEYA SYSTEM")
    print("="*70)
    
    # Generate 10,000 reservations
    generate_reservations(10000)
    
    print("\n[OK] Script completed!\n")