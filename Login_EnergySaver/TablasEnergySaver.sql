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

CREATE TABLE Configuracion (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TarifaCFE DECIMAL(10,2),
    Impuesto DECIMAL(10,2),
    HoraInicio VARCHAR(10),
    HoraFin VARCHAR(10)
);
select * from Configuracion;

INSERT INTO Usuario (nombre, correo, contraseña, estado)
VALUES ('Admin', 'admin@gmail.com', '1234', 'Admin');

