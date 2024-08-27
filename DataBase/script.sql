-- Crear la base de datos si no existe
CREATE DATABASE IF NOT EXISTS inmo_ca;
USE inmo_ca;

-- Crear la tabla 'propietario'
CREATE TABLE IF NOT EXISTS propietario (
    id INT AUTO_INCREMENT PRIMARY KEY,
    dni VARCHAR(20) UNIQUE NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    telefono VARCHAR(20),
    email VARCHAR(100),
    direccion VARCHAR(255),
    estado INT DEFAULT 1,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Crear la tabla 'inquilino'
CREATE TABLE IF NOT EXISTS inquilino (
    id INT AUTO_INCREMENT PRIMARY KEY,
    dni VARCHAR(20) UNIQUE NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    telefono VARCHAR(20),
    email VARCHAR(100),
    direccion VARCHAR(255),
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    activo int(1) DEFAULT 1
);

-- Crear la tabla 'tipo_inmueble'
CREATE TABLE tipo_inmueble (
    id INT AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
	fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Crear la tabla 'tipo_inmueble_uso'
CREATE TABLE tipo_inmueble_uso (
    id INT AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
	fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Crear la tabla de 'inmueble'
CREATE TABLE inmueble (
    id INT AUTO_INCREMENT PRIMARY KEY,
    direccion VARCHAR(255) NOT NULL,
    id_tipo_inmueble_uso INT NOT NULL,
    id_tipo_inmueble INT NOT NULL,
    ambientes INT NOT NULL,
    coordenada_lat FLOAT NOT NULL,
    coordenada_lon FLOAT NOT NULL,
    precio DECIMAL(10, 2) NOT NULL,
    estado INT NOT NULL DEFAULT 1,
    id_propietario INT,
	fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (id_propietario) REFERENCES Propietario(id),
    FOREIGN KEY (id_tipo_inmueble_uso) REFERENCES tipo_inmueble_uso(id),
	FOREIGN KEY (id_tipo_inmueble) REFERENCES tipo_inmueble(id)
);

-- Insertar datos en la tabla 'propietario'
INSERT INTO propietario (dni, nombre, apellido, telefono, email, direccion)
VALUES 
('20345678', 'Juan', 'Pérez', '11-5555-1234', 'juan.perez@example.com', 'Calle Falsa 123, Buenos Aires'),
('27456321', 'María', 'López', '11-5555-5678', 'maria.lopez@example.com', 'Avenida Siempreviva 742, Buenos Aires'),
('30112233', 'Carlos', 'García', '11-5555-9101', 'carlos.garcia@example.com', 'Plaza Mayor 5, Buenos Aires'),
('32987654', 'Lucía', 'Martínez', '11-5555-1122', 'lucia.martinez@example.com', 'Calle Real 10, Buenos Aires'),
('34123456', 'Luis', 'Fernández', '11-5555-3344', 'luis.fernandez@example.com', 'Avenida del Sol 21, Buenos Aires');

-- Insertar datos en la tabla 'inquilino'
INSERT INTO inquilino (dni, nombre, apellido, telefono, email, direccion, activo)
VALUES
('10000001', 'Laura', 'Sánchez', '555-2345', 'laura.sanchez@example.com', 'Calle Primavera 12, Ciudad Jardín', 1),
('10000002', 'Ricardo', 'Hernández', '555-6789', 'ricardo.hernandez@example.com', 'Avenida Libertad 45, Sector 3', 1),
('10000003', 'Beatriz', 'Moreno', '555-9876', 'beatriz.moreno@example.com', 'Calle de la Luna 88, Barrio Nuevo', 1),
('10000004', 'Fernando', 'Álvarez', '555-3456', 'fernando.alvarez@example.com', 'Plaza del Sol 34, Centro Histórico', 1),
('10000005', 'Carmen', 'Fernández', '555-4567', 'carmen.fernandez@example.com', 'Avenida del Mar 78, Playa Norte', 1);


