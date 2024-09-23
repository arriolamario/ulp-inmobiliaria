-- MySQL dump 10.13  Distrib 9.0.1, for Linux (x86_64)
--
-- Host: localhost    Database: inmo_ca
-- ------------------------------------------------------
-- Server version	9.0.1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `contrato`
--

DROP TABLE IF EXISTS `contrato`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contrato` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_inmueble` int NOT NULL,
  `id_inquilino` int NOT NULL,
  `fecha_desde` date NOT NULL,
  `fecha_hasta` date NOT NULL,
  `monto_alquiler` decimal(10,2) NOT NULL,
  `fecha_finalizacion_anticipada` date DEFAULT NULL,
  `multa` decimal(10,2) DEFAULT NULL,
  `estado` enum('Cancelado','Vigente','Finalizado') NOT NULL DEFAULT 'Vigente',
  `id_usuario_creacion` int NOT NULL,
  `id_usuario_finalizacion` int DEFAULT NULL,
  `fecha_creacion` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `fecha_actualizacion` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `pagado` tinyint(1) NOT NULL DEFAULT '0',
  `cantidad_cuotas` int DEFAULT NULL,
  `cuotas_pagas` int DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `id_inmueble` (`id_inmueble`),
  KEY `id_inquilino` (`id_inquilino`),
  KEY `id_usuario_creacion` (`id_usuario_creacion`),
  KEY `id_usuario_finalizacion` (`id_usuario_finalizacion`),
  CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`id_inmueble`) REFERENCES `inmueble` (`id`),
  CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilino` (`id`),
  CONSTRAINT `contrato_ibfk_3` FOREIGN KEY (`id_usuario_creacion`) REFERENCES `usuario` (`id`),
  CONSTRAINT `contrato_ibfk_4` FOREIGN KEY (`id_usuario_finalizacion`) REFERENCES `usuario` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contrato`
--

--
-- Table structure for table `inmueble`
--

DROP TABLE IF EXISTS `inmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inmueble` (
  `id` int NOT NULL AUTO_INCREMENT,
  `direccion` varchar(255) NOT NULL,
  `id_tipo_inmueble_uso` int NOT NULL,
  `id_tipo_inmueble` int NOT NULL,
  `ambientes` int NOT NULL,
  `coordenada_lat` varchar(255) NOT NULL,
  `coordenada_lon` varchar(255) NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `id_propietario` int DEFAULT NULL,
  `fecha_creacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `fecha_actualizacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `activo` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `id_propietario` (`id_propietario`),
  KEY `id_tipo_inmueble_uso` (`id_tipo_inmueble_uso`),
  KEY `id_tipo_inmueble` (`id_tipo_inmueble`),
  CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`id_propietario`) REFERENCES `propietario` (`id`),
  CONSTRAINT `inmueble_ibfk_2` FOREIGN KEY (`id_tipo_inmueble_uso`) REFERENCES `tipo_inmueble_uso` (`id`),
  CONSTRAINT `inmueble_ibfk_3` FOREIGN KEY (`id_tipo_inmueble`) REFERENCES `tipo_inmueble` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inmueble`
--


--
-- Table structure for table `inquilino`
--

DROP TABLE IF EXISTS `inquilino`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inquilino` (
  `id` int NOT NULL AUTO_INCREMENT,
  `dni` varchar(20) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `fecha_creacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `fecha_actualizacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `dni` (`dni`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilino`
--

LOCK TABLES `inquilino` WRITE;
/*!40000 ALTER TABLE `inquilino` DISABLE KEYS */;
INSERT INTO `inquilino` VALUES (1,'10000001','Lauraa','Sánchez','555-234567','laura.sanchez@example.com','Calle Primavera 12, Ciudad Jardín','2024-09-18 19:45:39','2024-09-22 18:55:16'),(2,'10000002','Ricardo','Hernández','555-6789','ricardo.hernandez@example.com','Avenida Libertad 45, Sector 3','2024-09-18 19:45:39','2024-09-18 19:45:39'),(3,'10000003','Beatriz','Moreno','555-9876','beatriz.moreno@example.com','Calle de la Luna 88, Barrio Nuevo','2024-09-18 19:45:39','2024-09-18 19:45:39'),(4,'10000004','Fernando','Álvarez','555-3456','fernando.alvarez@example.com','Plaza del Sol 34, Centro Histórico','2024-09-18 19:45:39','2024-09-18 19:45:39'),(5,'10000005','Carmen','Fernández','555-4567','carmen.fernandez@example.com','Avenida del Mar 78, Playa Norte','2024-09-18 19:45:39','2024-09-18 19:45:39');
/*!40000 ALTER TABLE `inquilino` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pago`
--

DROP TABLE IF EXISTS `pago`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pago` (
  `id` int NOT NULL AUTO_INCREMENT,
  `contrato_id` int NOT NULL,
  `numero_pago` int NOT NULL,
  `fecha_pago` datetime NOT NULL,
  `detalle` varchar(255) NOT NULL DEFAULT '',
  `importe` decimal(10,2) NOT NULL,
  `multa` decimal(10,2) DEFAULT NULL,
  `creado_por_id` int NOT NULL,
  `estado` enum('Pagado','Anulado','Pendiente') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'Pendiente',
  `anulado_por_id` int DEFAULT NULL,
  `fecha_anulacion` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `numero_pago` (`numero_pago`),
  KEY `contrato_id` (`contrato_id`),
  KEY `creado_por_id` (`creado_por_id`),
  KEY `anulado_por_id` (`anulado_por_id`),
  CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`contrato_id`) REFERENCES `contrato` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `pago_ibfk_2` FOREIGN KEY (`creado_por_id`) REFERENCES `usuario` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `pago_ibfk_3` FOREIGN KEY (`anulado_por_id`) REFERENCES `usuario` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pago`
--

--
-- Table structure for table `propietario`
--

DROP TABLE IF EXISTS `propietario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `propietario` (
  `id` int NOT NULL AUTO_INCREMENT,
  `dni` varchar(20) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `fecha_creacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `fecha_actualizacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `dni` (`dni`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propietario`
--

--
-- Table structure for table `tipo_inmueble`
--

DROP TABLE IF EXISTS `tipo_inmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipo_inmueble` (
  `id` int NOT NULL AUTO_INCREMENT,
  `descripcion` varchar(100) NOT NULL,
  `estado` int DEFAULT '1',
  `fecha_creacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `fecha_actualizacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo_inmueble`
--

LOCK TABLES `tipo_inmueble` WRITE;
/*!40000 ALTER TABLE `tipo_inmueble` DISABLE KEYS */;
INSERT INTO `tipo_inmueble` VALUES (1,'Duplex',1,'2024-09-18 19:45:46','2024-09-18 19:45:46'),(2,'Casa',1,'2024-09-18 19:45:46','2024-09-18 19:45:46'),(3,'Dos Ambientes',1,'2024-09-18 19:45:46','2024-09-18 19:45:46');
/*!40000 ALTER TABLE `tipo_inmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipo_inmueble_uso`
--

DROP TABLE IF EXISTS `tipo_inmueble_uso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipo_inmueble_uso` (
  `id` int NOT NULL AUTO_INCREMENT,
  `descripcion` varchar(100) NOT NULL,
  `estado` int DEFAULT '1',
  `fecha_creacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `fecha_actualizacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo_inmueble_uso`
--

LOCK TABLES `tipo_inmueble_uso` WRITE;
/*!40000 ALTER TABLE `tipo_inmueble_uso` DISABLE KEYS */;
INSERT INTO `tipo_inmueble_uso` VALUES (1,'Comercial',1,'2024-09-18 19:45:46','2024-09-18 19:45:46'),(2,'Laboral',1,'2024-09-18 19:45:46','2024-09-18 19:45:46'),(3,'Personal',1,'2024-09-18 19:45:46','2024-09-18 19:45:46');
/*!40000 ALTER TABLE `tipo_inmueble_uso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuario` (
  `id` int NOT NULL AUTO_INCREMENT,
  `email` varchar(255) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `nombre` varchar(100) DEFAULT NULL,
  `apellido` varchar(100) DEFAULT NULL,
  `avatar_url` text,
  `rol` enum('empleado','administrador') NOT NULL,
  `fecha_creacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `fecha_actualizacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `telefono` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (6,'john.doe@example.com','$2y$10$abcdefg1234567890hijklmnopqrstuv','John','Doe','https://example.com/avatar1.png','empleado','2024-09-19 15:56:53','2024-09-19 15:56:53','+123456789'),(7,'jane.smith@example.com','$2y$10$1234567890abcdefg1234567890abcd','Jane','Smith','https://example.com/avatar2.png','administrador','2024-09-19 15:56:53','2024-09-19 15:56:53','+987654321'),(8,'maria.lopez@example.com','$2y$10$hijklmnopqrstuv1234567890abcdefg','Maria','Lopez','https://example.com/avatar3.png','empleado','2024-09-19 15:56:53','2024-09-19 15:56:53','+1122334455'),(9,'carlos.martin@example.com','$2y$10$1234567890abcdefg1234567890abcd','Carlos','Martin','https://example.com/avatar4.png','empleado','2024-09-19 15:56:53','2024-09-19 15:56:53','+9988776655'),(10,'laura.garcia@example.com','$2y$10$abcdefg1234567890hijklmnopqrstuv','Laura','Garcia','https://example.com/avatar5.png','administrador','2024-09-19 15:56:53','2024-09-19 15:56:53','+6655443322'),(11,'admin@yopmail.com','myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=','NommbreAdm','ApellidoAdm','','administrador','2024-09-20 13:31:26','2024-09-20 13:31:26',NULL),(12,'empleado@yopmail.com','myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=','NombreEmp','ApellidoEmp','','empleado','2024-09-20 13:31:26','2024-09-20 13:31:26',NULL);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-09-23  5:33:42
