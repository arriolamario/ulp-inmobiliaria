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
CREATE TABLE IF NOT EXISTS tipo_inmueble (
    id INT AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    estado INT DEFAULT 1,
	fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Crear la tabla 'tipo_inmueble_uso'
CREATE TABLE IF NOT EXISTS tipo_inmueble_uso (
    id INT AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    estado INT DEFAULT 1,
	fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Crear la tabla de 'inmueble'
CREATE TABLE IF NOT EXISTS inmueble (
    id INT AUTO_INCREMENT PRIMARY KEY,
    direccion VARCHAR(255) NOT NULL,
    id_tipo_inmueble_uso INT NOT NULL,
    id_tipo_inmueble INT NOT NULL,
    ambientes INT NOT NULL,
    coordenada_lat VARCHAR(255) NOT NULL,
    coordenada_lon VARCHAR(255) NOT NULL,
    precio DECIMAL(10, 2) NOT NULL,
    estado INT NOT NULL DEFAULT 1,
    id_propietario INT,
	fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (id_propietario) REFERENCES propietario(id),
    FOREIGN KEY (id_tipo_inmueble_uso) REFERENCES tipo_inmueble_uso(id),
	FOREIGN KEY (id_tipo_inmueble) REFERENCES tipo_inmueble(id)
);

CREATE TABLE IF NOT EXISTS usuario (
    id INT AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    nombre VARCHAR(100),
    apellido VARCHAR(100),
    telefono VARCHAR(20),
    avatar_url TEXT,
    rol ENUM('empleado', 'administrador') NOT NULL,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS pago (
    id int NOT NULL AUTO_INCREMENT,
    contrato_id int NOT NULL,
    numero_pago int NOT NULL UNIQUE,
    fecha_pago datetime NOT NULL,
    detalle varchar(255) NOT NULL DEFAULT '',
    importe decimal(10, 2) NOT NULL,
    multa decimal(10, 2) NOT NULL,
    estado varchar(50) NOT NULL DEFAULT '',
    creado_por_id int NOT NULL,
    anulado_por_id int DEFAULT NULL,
    fecha_anulacion datetime DEFAULT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (contrato_id) REFERENCES contrato (Id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (creado_por_id) REFERENCES usuario (Id) ON DELETE NO ACTION ON UPDATE CASCADE,
    FOREIGN KEY (anulado_por_id) REFERENCES usuario (Id) ON DELETE NO ACTION ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS contrato (
    id INT AUTO_INCREMENT PRIMARY KEY,
    id_inmueble INT NOT NULL,
    id_inquilino INT NOT NULL,
    fecha_desde DATE NOT NULL,
    fecha_hasta DATE NOT NULL,
    monto_alquiler DECIMAL(10, 2) NOT NULL,
    fecha_finalizacion_anticipada DATE,
    multa DECIMAL(10, 2),
    estado BOOLEAN NOT NULL DEFAULT TRUE,
    id_usuario_creacion INT NOT NULL,
    id_usuario_finalizacion INT,
    fecha_creacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    pagado BOOLEAN NOT NULL DEFAULT FALSE,
    cantidad_cuotas int DEFAULT NULL,
    cuotas_pagas int DEFAULT '0',
    FOREIGN KEY (id_inmueble) REFERENCES inmueble(id),
    FOREIGN KEY (id_inquilino) REFERENCES inquilino(id),
    FOREIGN KEY (id_usuario_creacion) REFERENCES usuario(id),
    FOREIGN KEY (id_usuario_finalizacion) REFERENCES usuario(id)
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

insert into tipo_inmueble (descripcion) 
values ('Duplex'),
('Casa'),
('Dos Ambientes');

insert into tipo_inmueble_uso (descripcion) 
values ('Comercial'),
('Laboral'),
('Personal');

INSERT INTO inmueble ( direccion, id_tipo_inmueble_uso, id_tipo_inmueble, ambientes, coordenada_lat, coordenada_lon, precio, estado, id_propietario) VALUES
('Calle Falsa 123, Springfield', 1, 1, 3, 34.0522, -118.2437, 1500.00, 1, 1),
('742 Evergreen Terrace, Springfield', 2, 2, 5, 34.0522, -118.2437, 2500.00, 1, 2),
('123 Elm Street, West Springfield', 1, 1, 4, 34.0522, -118.2437, 2000.00, 1, 3),
('555 North Oak Trafficway, Springfield', 2, 3, 6, 34.0522, -118.2437, 3000.00, 1, 4);

INSERT INTO usuario (email, password_hash, nombre, apellido, telefono, avatar_url, rol)
VALUES
('john.doe@example.com', '$2y$10$abcdefg1234567890hijklmnopqrstuv', 'John', 'Doe', '+123456789', 'https://example.com/avatar1.png', 'empleado'),
('jane.smith@example.com', '$2y$10$1234567890abcdefg1234567890abcd', 'Jane', 'Smith', '+987654321', 'https://example.com/avatar2.png', 'administrador'),
('maria.lopez@example.com', '$2y$10$hijklmnopqrstuv1234567890abcdefg', 'Maria', 'Lopez', '+1122334455', 'https://example.com/avatar3.png', 'empleado'),
('carlos.martin@example.com', '$2y$10$1234567890abcdefg1234567890abcd', 'Carlos', 'Martin', '+9988776655', 'https://example.com/avatar4.png', 'empleado'),
('laura.garcia@example.com', '$2y$10$abcdefg1234567890hijklmnopqrstuv', 'Laura', 'Garcia', '+6655443322', 'https://example.com/avatar5.png', 'administrador');

