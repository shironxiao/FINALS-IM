-- =============================================
-- Tabeya System - CORRECTED Reservation Tables
-- Matches VB.NET Application Requirements
-- Generated: 2025-12-17
-- =============================================

DROP TABLE IF EXISTS `reservations_payments`;
DROP TABLE IF EXISTS `reservation_items`;
DROP TABLE IF EXISTS `reservations`;

-- =============================================
-- Table: reservations
-- =============================================
CREATE TABLE `reservations` (
  `ReservationID` int(11) NOT NULL AUTO_INCREMENT,
  `CustomerID` int(11) DEFAULT NULL,
  `FullName` varchar(200) DEFAULT NULL,
  `AssignedStaffID` int(11) DEFAULT NULL,
  `ContactNumber` varchar(20) DEFAULT NULL,
  `ReservationType` enum('Online','Walk-in') DEFAULT 'Online',
  `EventType` varchar(100) DEFAULT NULL,
  `EventDate` date DEFAULT NULL,
  `EventTime` time DEFAULT NULL,
  `NumberOfGuests` int(11) DEFAULT NULL,
  `ProductSelection` text DEFAULT NULL,
  `SpecialRequests` text DEFAULT NULL,
  `ReservationStatus` enum('Pending','Confirmed','Cancelled','Completed') DEFAULT 'Pending',
  `ReservationDate` datetime DEFAULT current_timestamp(),
  `DeliveryAddress` text DEFAULT NULL,
  `DeliveryOption` enum('Delivery','Pickup') DEFAULT 'Pickup',
  `UpdatedDate` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`ReservationID`),
  KEY `CustomerID` (`CustomerID`),
  KEY `AssignedStaffID` (`AssignedStaffID`),
  CONSTRAINT `reservations_ibfk_1` FOREIGN KEY (`CustomerID`) REFERENCES `customers` (`CustomerID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- =============================================
-- Table: reservation_items
-- =============================================
CREATE TABLE `reservation_items` (
  `ReservationItemID` int(11) NOT NULL AUTO_INCREMENT,
  `ReservationID` int(11) DEFAULT NULL,
  `ProductName` varchar(100) DEFAULT NULL,
  `Quantity` int(11) DEFAULT 1,
  `UnitPrice` decimal(10,2) DEFAULT 0.00,
  `TotalPrice` decimal(10,2) DEFAULT 0.00,
  `Notes` text DEFAULT NULL,
  PRIMARY KEY (`ReservationItemID`),
  KEY `ReservationID` (`ReservationID`),
  CONSTRAINT `reservation_items_ibfk_1` FOREIGN KEY (`ReservationID`) REFERENCES `reservations` (`ReservationID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ==============================================
-- Table: reservation_payments
-- ==============================================
CREATE TABLE `reservation_payments` (
  `ReservationPaymentID` int(11) NOT NULL AUTO_INCREMENT,
  `ReservationID` int(11) DEFAULT NULL,
  `PaymentDate` datetime DEFAULT current_timestamp(),
  `PaymentMethod` enum('Cash','Credit Card','Debit Card','GCash','PayMaya','COD') DEFAULT 'Cash',
  `PaymentStatus` enum('Pending','Paid','Refunded','Partial','Completed','Failed') DEFAULT 'Pending',
  `AmountPaid` decimal(10,2) DEFAULT 0.00,
  `PaymentSource` enum('POS','Website') DEFAULT 'Website',
  `ProofOfPayment` varchar(255) DEFAULT NULL,
  `ReceiptFileName` varchar(255) DEFAULT NULL,
  `TransactionID` varchar(100) DEFAULT NULL,
  `UpdatedDate` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`ReservationPaymentID`),
  KEY `ReservationID` (`ReservationID`),
  CONSTRAINT `reservation_payments_ibfk_1` FOREIGN KEY (`ReservationID`) REFERENCES `reservations` (`ReservationID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- =============================================
-- End of Script
-- =============================================
