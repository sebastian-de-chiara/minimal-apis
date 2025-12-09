DROP SCHEMA IF EXISTS main CASCADE;
CREATE SCHEMA main;

-- Dimension Tables

CREATE TABLE main.patients (
    patient_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name TEXT NOT NULL,
    email TEXT NOT NULL
);

CREATE TABLE main.doctors (
    doctor_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name TEXT NOT NULL,
    specialty TEXT NOT NULL
);

CREATE TABLE main.services (
    service_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name TEXT NOT NULL,
    price DECIMAL(10,2) NOT NULL
);

CREATE TABLE main.locations (
    location_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name TEXT NOT NULL,
    city TEXT NOT NULL
);

-- Fact Table

CREATE TABLE main.appointments (
    appointment_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    patient_id INT NOT NULL REFERENCES main.patients(patient_id),
    doctor_id INT NOT NULL REFERENCES main.doctors(doctor_id),
    service_id INT NOT NULL REFERENCES main.services(service_id),
    location_id INT NOT NULL REFERENCES main.locations(location_id),
    appointment_time TIMESTAMPTZ NOT NULL,
    duration_minutes INT NOT NULL,
    status TEXT NOT NULL
);

-- Insert Mock Data

INSERT INTO main.patients (name, email) VALUES
('John Smith', 'john@email.com'),
('Sarah Jones', 'sarah@email.com'),
('Mike Brown', 'mike@email.com'),
('Emily Davis', 'emily@email.com'),
('James Wilson', 'james@email.com'),
('Lisa Anderson', 'lisa@email.com'),
('Robert Taylor', 'robert@email.com'),
('Jennifer Martin', 'jennifer@email.com'),
('David Lee', 'david@email.com'),
('Mary Garcia', 'mary@email.com');

INSERT INTO main.doctors (name, specialty) VALUES
('Dr. Wilson', 'Cardiology'),
('Dr. Lee', 'Dermatology'),
('Dr. Patel', 'Orthopedics'),
('Dr. Kim', 'Neurology'),
('Dr. Rodriguez', 'Pediatrics');

INSERT INTO main.services (name, price) VALUES
('Consultation', 150.00),
('Follow-up', 75.00),
('Physical Exam', 200.00),
('Lab Work', 120.00),
('X-Ray', 180.00);

INSERT INTO main.locations (name, city) VALUES
('Main Clinic', 'Boston'),
('Downtown Office', 'Boston'),
('West Branch', 'Cambridge');

INSERT INTO main.appointments (patient_id, doctor_id, service_id, location_id, appointment_time, duration_minutes, status) VALUES
(1, 1, 1, 1, '2025-11-15 09:00:00+00', 30, 'scheduled'),
(2, 2, 3, 2, '2025-11-15 10:00:00+00', 45, 'completed'),
(3, 3, 2, 1, '2025-11-15 11:00:00+00', 20, 'scheduled'),
(4, 4, 1, 3, '2025-11-15 14:00:00+00', 30, 'cancelled'),
(5, 5, 3, 1, '2025-11-15 15:00:00+00', 60, 'scheduled'),
(6, 1, 4, 2, '2025-11-16 09:00:00+00', 30, 'completed'),
(7, 2, 5, 1, '2025-11-16 10:00:00+00', 45, 'completed'),
(8, 3, 1, 3, '2025-11-16 11:00:00+00', 30, 'scheduled'),
(9, 4, 2, 1, '2025-11-16 13:00:00+00', 20, 'scheduled'),
(10, 5, 3, 2, '2025-11-16 14:00:00+00', 45, 'completed'),
(1, 2, 1, 1, '2025-11-17 09:00:00+00', 30, 'scheduled'),
(2, 3, 4, 2, '2025-11-17 10:00:00+00', 30, 'completed'),
(3, 4, 5, 3, '2025-11-17 11:00:00+00', 45, 'scheduled'),
(4, 5, 1, 1, '2025-11-17 14:00:00+00', 30, 'cancelled'),
(5, 1, 2, 2, '2025-11-17 15:00:00+00', 20, 'scheduled'),
(6, 2, 3, 1, '2025-11-18 09:00:00+00', 45, 'completed'),
(7, 3, 1, 3, '2025-11-18 10:00:00+00', 30, 'scheduled'),
(8, 4, 4, 2, '2025-11-18 11:00:00+00', 30, 'completed'),
(9, 5, 5, 1, '2025-11-18 13:00:00+00', 45, 'scheduled'),
(10, 1, 1, 3, '2025-11-18 14:00:00+00', 30, 'completed'),
(1, 3, 2, 1, '2025-11-19 09:00:00+00', 20, 'scheduled'),
(2, 4, 3, 2, '2025-11-19 10:00:00+00', 45, 'completed'),
(3, 5, 1, 3, '2025-11-19 11:00:00+00', 30, 'scheduled'),
(4, 1, 4, 1, '2025-11-19 14:00:00+00', 30, 'cancelled'),
(5, 2, 5, 2, '2025-11-19 15:00:00+00', 45, 'scheduled'),
(6, 3, 1, 1, '2025-11-20 09:00:00+00', 30, 'completed'),
(7, 4, 2, 3, '2025-11-20 10:00:00+00', 20, 'scheduled'),
(8, 5, 3, 2, '2025-11-20 11:00:00+00', 45, 'completed'),
(9, 1, 4, 1, '2025-11-20 13:00:00+00', 30, 'scheduled'),
(10, 2, 5, 3, '2025-11-20 14:00:00+00', 45, 'completed'),
(1, 4, 1, 2, '2025-11-21 09:00:00+00', 30, 'scheduled'),
(2, 5, 2, 1, '2025-11-21 10:00:00+00', 20, 'completed'),
(3, 1, 3, 3, '2025-11-21 11:00:00+00', 45, 'scheduled'),
(4, 2, 4, 2, '2025-11-21 14:00:00+00', 30, 'cancelled'),
(5, 3, 5, 1, '2025-11-21 15:00:00+00', 45, 'scheduled'),
(6, 4, 1, 3, '2025-11-22 09:00:00+00', 30, 'completed'),
(7, 5, 2, 2, '2025-11-22 10:00:00+00', 20, 'scheduled'),
(8, 1, 3, 1, '2025-11-22 11:00:00+00', 45, 'completed'),
(9, 2, 4, 3, '2025-11-22 13:00:00+00', 30, 'scheduled'),
(10, 3, 5, 2, '2025-11-22 14:00:00+00', 45, 'completed'),
(1, 5, 1, 1, '2025-11-23 09:00:00+00', 30, 'scheduled'),
(2, 1, 2, 3, '2025-11-23 10:00:00+00', 20, 'completed'),
(3, 2, 3, 2, '2025-11-23 11:00:00+00', 45, 'scheduled'),
(4, 3, 4, 1, '2025-11-23 14:00:00+00', 30, 'cancelled'),
(5, 4, 5, 3, '2025-11-23 15:00:00+00', 45, 'scheduled'),
(6, 5, 1, 2, '2025-11-24 09:00:00+00', 30, 'completed'),
(7, 1, 2, 1, '2025-11-24 10:00:00+00', 20, 'scheduled'),
(8, 2, 3, 3, '2025-11-24 11:00:00+00', 45, 'completed'),
(9, 3, 4, 2, '2025-11-24 13:00:00+00', 30, 'scheduled'),
(10, 4, 5, 1, '2025-11-24 14:00:00+00', 45, 'completed'),
(1, 1, 1, 3, '2025-11-25 09:00:00+00', 30, 'scheduled'),
(2, 2, 2, 2, '2025-11-25 10:00:00+00', 20, 'completed'),
(3, 3, 3, 1, '2025-11-25 11:00:00+00', 45, 'scheduled'),
(4, 4, 4, 3, '2025-11-25 14:00:00+00', 30, 'cancelled'),
(5, 5, 5, 2, '2025-11-25 15:00:00+00', 45, 'scheduled'),
(6, 1, 1, 1, '2025-11-26 09:00:00+00', 30, 'completed'),
(7, 2, 2, 3, '2025-11-26 10:00:00+00', 20, 'scheduled'),
(8, 3, 3, 2, '2025-11-26 11:00:00+00', 45, 'completed'),
(9, 4, 4, 1, '2025-11-26 13:00:00+00', 30, 'scheduled'),
(10, 5, 5, 3, '2025-11-26 14:00:00+00', 45, 'completed'),
(1, 2, 1, 2, '2025-11-27 09:00:00+00', 30, 'scheduled'),
(2, 3, 2, 1, '2025-11-27 10:00:00+00', 20, 'completed'),
(3, 4, 3, 3, '2025-11-27 11:00:00+00', 45, 'scheduled'),
(4, 5, 4, 2, '2025-11-27 14:00:00+00', 30, 'cancelled'),
(5, 1, 5, 1, '2025-11-27 15:00:00+00', 45, 'scheduled'),
(6, 2, 1, 3, '2025-11-28 09:00:00+00', 30, 'completed'),
(7, 3, 2, 2, '2025-11-28 10:00:00+00', 20, 'scheduled'),
(8, 4, 3, 1, '2025-11-28 11:00:00+00', 45, 'completed'),
(9, 5, 4, 3, '2025-11-28 13:00:00+00', 30, 'scheduled'),
(10, 1, 5, 2, '2025-11-28 14:00:00+00', 45, 'completed'),
(1, 3, 1, 1, '2025-11-29 09:00:00+00', 30, 'scheduled'),
(2, 4, 2, 3, '2025-11-29 10:00:00+00', 20, 'completed'),
(3, 5, 3, 2, '2025-11-29 11:00:00+00', 45, 'scheduled'),
(4, 1, 4, 1, '2025-11-29 14:00:00+00', 30, 'cancelled'),
(5, 2, 5, 3, '2025-11-29 15:00:00+00', 45, 'scheduled'),
(6, 3, 1, 2, '2025-11-30 09:00:00+00', 30, 'completed'),
(7, 4, 2, 1, '2025-11-30 10:00:00+00', 20, 'scheduled'),
(8, 5, 3, 3, '2025-11-30 11:00:00+00', 45, 'completed'),
(9, 1, 4, 2, '2025-11-30 13:00:00+00', 30, 'scheduled'),
(10, 2, 5, 1, '2025-11-30 14:00:00+00', 45, 'completed'),
(1, 4, 1, 3, '2025-12-01 09:00:00+00', 30, 'scheduled'),
(2, 5, 2, 2, '2025-12-01 10:00:00+00', 20, 'completed'),
(3, 1, 3, 1, '2025-12-01 11:00:00+00', 45, 'scheduled'),
(4, 2, 4, 3, '2025-12-01 14:00:00+00', 30, 'cancelled'),
(5, 3, 5, 2, '2025-12-01 15:00:00+00', 45, 'scheduled'),
(6, 4, 1, 1, '2025-12-02 09:00:00+00', 30, 'completed'),
(7, 5, 2, 3, '2025-12-02 10:00:00+00', 20, 'scheduled'),
(8, 1, 3, 2, '2025-12-02 11:00:00+00', 45, 'completed'),
(9, 2, 4, 1, '2025-12-02 13:00:00+00', 30, 'scheduled'),
(10, 3, 5, 3, '2025-12-02 14:00:00+00', 45, 'completed'),
(1, 5, 1, 2, '2025-12-03 09:00:00+00', 30, 'scheduled'),
(2, 1, 2, 1, '2025-12-03 10:00:00+00', 20, 'completed'),
(3, 2, 3, 3, '2025-12-03 11:00:00+00', 45, 'scheduled'),
(4, 3, 4, 2, '2025-12-03 14:00:00+00', 30, 'cancelled'),
(5, 4, 5, 1, '2025-12-03 15:00:00+00', 45, 'scheduled'),
(6, 5, 1, 3, '2025-12-04 09:00:00+00', 30, 'completed'),
(7, 1, 2, 2, '2025-12-04 10:00:00+00', 20, 'scheduled'),
(8, 2, 3, 1, '2025-12-04 11:00:00+00', 45, 'completed'),
(9, 3, 4, 3, '2025-12-04 13:00:00+00', 30, 'scheduled'),
(10, 4, 5, 2, '2025-12-04 14:00:00+00', 45, 'completed');