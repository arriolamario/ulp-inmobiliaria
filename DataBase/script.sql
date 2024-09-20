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
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Crear la tabla 'tipo_inmueble'
CREATE TABLE IF NOT EXISTS tipo_inmueble (
    id INT AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
	fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Crear la tabla 'tipo_inmueble_uso'
CREATE TABLE IF NOT EXISTS tipo_inmueble_uso (
    id INT AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
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
    activo TINYINT(1) NOT NULL DEFAULT 1,
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
    avatar_url TEXT,
    rol ENUM('empleado', 'administrador') NOT NULL,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
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
    estado ENUM('Cancelado', 'Vigente', 'Finalizado') NOT NULL DEFAULT 'Vigente',
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

insert into tipo_inmueble (descripcion) 
values ('Duplex'),
('Casa'),
('Dos Ambientes');

insert into tipo_inmueble_uso (descripcion) 
values ('Comercial'),
('Laboral'),
('Personal');

-- password_hash '123456' myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=
INSERT INTO usuario (email, password_hash, nombre, apellido, avatar_url, rol)
VALUES
('admin@yopmail.com', 'myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=', 'NommbreAdm', 'ApellidoAdm', '', 'administrador'),
('empleado@yopmail.com', 'myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=', 'NombreEmp', 'ApellidoEmp', '', 'empleado');
