-- Crear base de datos
USE EnergySaver;

-- =========================
-- TABLA: Usuario
-- =========================
CREATE TABLE Usuario (
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    correo VARCHAR(150),
    contraseña VARCHAR(255) NOT NULL,
    estado VARCHAR(20)
);

-- =========================
-- TABLA: Cuenta
-- =========================
CREATE TABLE Cuenta (
    id_cuenta INT PRIMARY KEY,
    id_usuario INT,
    estado VARCHAR(20),
    fecha_registro DATE NOT NULL,
    
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

-- =========================
-- TABLA: Dispositivo
-- =========================
CREATE TABLE Dispositivos 
(
    id_dispositivo INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    tipo VARCHAR(100),
    marca VARCHAR(100),
    consumoWatts FLOAT,
    ubicacion VARCHAR(100),
    estado VARCHAR(20) DEFAULT 'Activo',
    fechaRegistro DATETIME2 DEFAULT SYSDATETIME()
);
SELECT * FROM Dispositivos;
ALTER TABLE Dispositivos
ADD id_usuario INT NULL;
ALTER TABLE Dispositivos
ADD CONSTRAINT FK_Dispositivos_Usuario
FOREIGN KEY (id_usuario)
REFERENCES Usuario(id_usuario);
-- =========================
-- TABLA: Consumo
-- =========================
CREATE TABLE Consumo (
        id_consumo INT PRIMARY KEY,
        id_usuario INT,
        id_dispositivo INT,
        fecha DATE NOT NULL,
        valor FLOAT NOT NULL,
     
        FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario),
        FOREIGN KEY (id_dispositivo) REFERENCES Dispositivo(id_dispositivo)
);
INSERT INTO Consumo
(id_consumo, id_usuario, id_dispositivo, fecha, valor)

VALUES
(1, 1, 1, '2025-07-05', 120),
(2, 1, 1, '2025-07-10', 90),
(3, 1, 1, '2025-08-02', 180),
(4, 1, 1, '2025-08-08', 150);

-- =========================
-- TABLA: Alerta
-- =========================
CREATE TABLE Alerta (
    id_alerta INT PRIMARY KEY,
    id_usuario INT,
    tipo VARCHAR(50) NOT NULL,
    mensaje VARCHAR(100),
    fecha DATETIME NOT NULL,
    
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

-- =========================
-- TABLA: Reporte
-- =========================
CREATE TABLE Reporte (
    id_reporte INT PRIMARY KEY,
    id_usuario INT,
    fecha_inicio DATE,
    fecha_fin DATE,
    tipo VARCHAR(50) NOT NULL,
    
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);
-- =========================
-- TABLA: CONFIGURACION
-- =========================
CREATE TABLE Configuracion (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TarifaCFE DECIMAL(10,2),
    Impuesto DECIMAL(10,2),
    HoraInicio VARCHAR(10),
    HoraFin VARCHAR(10)
);

-- =========================
-- TABLA: CONFIGURACION
-- =========================
ALTER TABLE Configuracion
ADD LimiteConsumo FLOAT;

ALTER TABLE Configuracion
DROP COLUMN ClimaAutomatico;
select * from Configuracion;

INSERT INTO Usuario (nombre, correo, contraseña, estado)
VALUES ('Admin', 'admin@gmail.com', '1234', 'Admin');

SELECT * FROM Usuario;
SELECT * FROM Dispositivos;

INSERT INTO Consumo
(id_consumo, id_usuario, id_dispositivo, fecha, valor)

VALUES
(1, 4, 1, '2025-07-05', 120),

(2, 4, 1, '2025-07-10', 90),

(3, 4, 1, '2025-08-02', 180),

(4, 4, 1, '2025-08-08', 150);