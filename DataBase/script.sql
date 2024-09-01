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
    coordenada_lat VARCHAR(255) NOT NULL,
    coordenada_lon VARCHAR(255) NOT NULL,
    precio DECIMAL(10, 2) NOT NULL,
    estado INT NOT NULL DEFAULT 1,
    id_propietario INT,
	fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (id_propietario) REFERENCES Propietario(id),
    FOREIGN KEY (id_tipo_inmueble_uso) REFERENCES tipo_inmueble_uso(id),
	FOREIGN KEY (id_tipo_inmueble) REFERENCES tipo_inmueble(id)
);

-- Insertar datos en la tabla 'tipo_inmueble' hasta tener el AMB de tipo inmueble
insert into tipo_inmueble (descripcion) 
values ('Tipo Inmueble 1'),
('Tipo Inmueble 2'),
('Tipo Inmueble 3');

-- Insertar datos en la tabla 'tipo_inmueble_uso' hasta tener el ABM de tipo inmueble uso
insert into tipo_inmueble_uso (descripcion) 
values ('Tipo Inmueble Uso 1'),
('Tipo Inmueble Uso 2'),
('Tipo Inmueble Uso 3');