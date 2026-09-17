-- =====================================================================
-- BASE DE DATOS: flomar_db
-- VERSIÓN: 5.0 FINAL (Solo tablas - Normalizada 3FN)
-- DESCRIPCIÓN: Sistema de ventas e inventario para FLOMAR
--              Comercializadora de repuestos automotrices - Cochabamba
-- =====================================================================
DROP DATABASE IF EXISTS flomar_db;
CREATE DATABASE flomar_db
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;
USE flomar_db;

-- =====================================================================
-- TABLAS CATÁLOGO / MAESTRAS
-- =====================================================================

CREATE TABLE ROL (
    id_rol          INT AUTO_INCREMENT PRIMARY KEY,
    nombre_rol      VARCHAR(30)  NOT NULL UNIQUE,
    descripcion     VARCHAR(150) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE ESTADO_USUARIO (
    id_estado_usuario INT AUTO_INCREMENT PRIMARY KEY,
    nombre_estado     VARCHAR(20) NOT NULL UNIQUE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE ESTADO_GENERAL (
    id_estado_general INT AUTO_INCREMENT PRIMARY KEY,
    nombre_estado     VARCHAR(20) NOT NULL UNIQUE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE CATEGORIA (
    id_categoria     INT AUTO_INCREMENT PRIMARY KEY,
    nombre_categoria VARCHAR(60)  NOT NULL UNIQUE,
    descripcion      VARCHAR(150) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE METODO_PAGO (
    id_metodo_pago INT AUTO_INCREMENT PRIMARY KEY,
    nombre_metodo  VARCHAR(30)  NOT NULL UNIQUE,
    descripcion    VARCHAR(100) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IMPUESTO (
    id_impuesto     INT AUTO_INCREMENT PRIMARY KEY,
    nombre_impuesto VARCHAR(50)  NOT NULL,
    porcentaje      DECIMAL(5,2) NOT NULL,
    vigente         TINYINT(1)   NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE ESTADO_VENTA (
    id_estado_venta INT AUTO_INCREMENT PRIMARY KEY,
    nombre_estado   VARCHAR(20) NOT NULL UNIQUE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE TIPO_MOVIMIENTO (
    id_tipo_movimiento INT AUTO_INCREMENT PRIMARY KEY,
    nombre_tipo        VARCHAR(20)  NOT NULL UNIQUE,
    descripcion        VARCHAR(100) NULL,
    signo              TINYINT(1)   NOT NULL COMMENT '1=Suma stock, -1=Resta stock'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =====================================================================
-- TABLA: USUARIO (CON AUDITORÍA)
-- =====================================================================
CREATE TABLE USUARIO (
    id_usuario         INT AUTO_INCREMENT PRIMARY KEY,
    nombre_usuario     VARCHAR(50)  NOT NULL UNIQUE,
    contrasena_hash    VARCHAR(255) NOT NULL,
    nombre_completo    VARCHAR(100) NOT NULL,
    id_rol             INT          NOT NULL,
    id_estado_usuario  INT          NOT NULL,
    fecha_creacion     DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ultimo_acceso      DATETIME     NULL,
    CONSTRAINT FK_USUARIO_ROL    FOREIGN KEY (id_rol)            REFERENCES ROL(id_rol),
    CONSTRAINT FK_USUARIO_ESTADO FOREIGN KEY (id_estado_usuario) REFERENCES ESTADO_USUARIO(id_estado_usuario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =====================================================================
-- TABLA: CLIENTE (Para recibo personalizado y futura facturación)
-- =====================================================================
CREATE TABLE CLIENTE (
    id_cliente     INT AUTO_INCREMENT PRIMARY KEY,
    nombre         VARCHAR(120) NOT NULL,
    nit            VARCHAR(20)  NULL UNIQUE,
    telefono       VARCHAR(20)  NULL,
    email          VARCHAR(100) NULL,
    direccion      VARCHAR(150) NULL,
    fecha_registro DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id_estado      INT          NOT NULL,
    CONSTRAINT FK_CLIENTE_ESTADO FOREIGN KEY (id_estado) REFERENCES ESTADO_GENERAL(id_estado_general)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =====================================================================
-- TABLA: PROVEEDOR (CON ESTADO INDEPENDIENTE)
-- =====================================================================
CREATE TABLE PROVEEDOR (
    id_proveedor     INT AUTO_INCREMENT PRIMARY KEY,
    nombre           VARCHAR(120) NOT NULL,
    nit              VARCHAR(20)  NULL UNIQUE,
    telefono         VARCHAR(20)  NULL,
    direccion        VARCHAR(150) NULL,
    email            VARCHAR(100) NULL,
    id_estado        INT          NOT NULL,
    CONSTRAINT FK_PROVEEDOR_ESTADO FOREIGN KEY (id_estado) REFERENCES ESTADO_GENERAL(id_estado_general)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =====================================================================
-- TABLA: REPUESTO (CATÁLOGO)
-- =====================================================================
CREATE TABLE REPUESTO (
    id_repuesto         INT AUTO_INCREMENT PRIMARY KEY,
    codigo              VARCHAR(30)    NOT NULL UNIQUE,
    nombre              VARCHAR(120)   NOT NULL,
    id_categoria        INT            NOT NULL,
    costo_adquisicion   DECIMAL(10,2)  NOT NULL,
    precio_venta        DECIMAL(10,2)  NOT NULL,
    stock_actual        INT            NOT NULL DEFAULT 0,
    stock_minimo        INT            NOT NULL DEFAULT 0,
    id_estado           INT            NOT NULL,
    CONSTRAINT FK_REPUESTO_CATEGORIA FOREIGN KEY (id_categoria) REFERENCES CATEGORIA(id_categoria),
    CONSTRAINT FK_REPUESTO_ESTADO    FOREIGN KEY (id_estado)    REFERENCES ESTADO_GENERAL(id_estado_general),
    CONSTRAINT chk_stock_positivo    CHECK (stock_actual >= 0),
    CONSTRAINT chk_precio_no_perdida CHECK (precio_venta >= costo_adquisicion)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =====================================================================
-- TABLA: VENTA (CABECERA - CON FK A CLIENTE Y TOTALES PERSISTIDOS)
-- =====================================================================
CREATE TABLE VENTA (
    id_venta            INT AUTO_INCREMENT PRIMARY KEY,
    numero_transaccion  VARCHAR(20)   NOT NULL UNIQUE,
    fecha_hora          DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id_cliente          INT           NULL COMMENT 'NULL = Cliente Mostrador',
    id_vendedor         INT           NOT NULL,
    id_metodo_pago      INT           NOT NULL,
    id_impuesto         INT           NOT NULL,
    id_estado_venta     INT           NOT NULL,
    subtotal            DECIMAL(12,2) NOT NULL DEFAULT 0,
    monto_impuesto      DECIMAL(12,2) NOT NULL DEFAULT 0,
    total_venta         DECIMAL(12,2) NOT NULL DEFAULT 0,
    tipo_comprobante    VARCHAR(30)   NOT NULL DEFAULT 'Recibo Interno',
    CONSTRAINT FK_VENTA_CLIENTE    FOREIGN KEY (id_cliente)     REFERENCES CLIENTE(id_cliente),
    CONSTRAINT FK_VENTA_VENDEDOR   FOREIGN KEY (id_vendedor)    REFERENCES USUARIO(id_usuario),
    CONSTRAINT FK_VENTA_METODO     FOREIGN KEY (id_metodo_pago) REFERENCES METODO_PAGO(id_metodo_pago),
    CONSTRAINT FK_VENTA_IMPUESTO   FOREIGN KEY (id_impuesto)    REFERENCES IMPUESTO(id_impuesto),
    CONSTRAINT FK_VENTA_ESTADO     FOREIGN KEY (id_estado_venta) REFERENCES ESTADO_VENTA(id_estado_venta),
    CONSTRAINT chk_total_positivo  CHECK (total_venta >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =====================================================================
-- TABLA: DETALLE_VENTA (LÍNEAS DE LA VENTA)
-- =====================================================================
CREATE TABLE DETALLE_VENTA (
    id_detalle_venta INT AUTO_INCREMENT PRIMARY KEY,
    id_venta         INT           NOT NULL,
    id_repuesto      INT           NOT NULL,
    cantidad         INT           NOT NULL,
    precio_unitario  DECIMAL(10,2) NOT NULL,
    subtotal         DECIMAL(12,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_DETALLE_VENTA    FOREIGN KEY (id_venta)    REFERENCES VENTA(id_venta) ON DELETE CASCADE,
    CONSTRAINT FK_DETALLE_REPUESTO FOREIGN KEY (id_repuesto) REFERENCES REPUESTO(id_repuesto),
    CONSTRAINT uk_venta_repuesto   UNIQUE (id_venta, id_repuesto),
    CONSTRAINT chk_cantidad_positiva        CHECK (cantidad > 0),
    CONSTRAINT chk_precio_unitario_positivo CHECK (precio_unitario > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =====================================================================
-- TABLA: COMPRA (CABECERA DE INGRESO DE MERCADERÍA)
-- =====================================================================
CREATE TABLE COMPRA (
    id_compra        INT AUTO_INCREMENT PRIMARY KEY,
    numero_compra    VARCHAR(20)   NOT NULL UNIQUE,
    fecha_ingreso    DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id_proveedor     INT           NOT NULL,
    id_usuario       INT           NOT NULL,
    monto_total      DECIMAL(12,2) NOT NULL DEFAULT 0,
    observaciones    VARCHAR(200)  NULL,
    CONSTRAINT FK_COMPRA_PROVEEDOR FOREIGN KEY (id_proveedor) REFERENCES PROVEEDOR(id_proveedor),
    CONSTRAINT FK_COMPRA_USUARIO   FOREIGN KEY (id_usuario)   REFERENCES USUARIO(id_usuario),
    CONSTRAINT chk_monto_compra_positivo CHECK (monto_total >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =====================================================================
-- TABLA: DETALLE_COMPRA (LÍNEAS DE LA COMPRA)
-- =====================================================================
CREATE TABLE DETALLE_COMPRA (
    id_detalle_compra INT AUTO_INCREMENT PRIMARY KEY,
    id_compra         INT           NOT NULL,
    id_repuesto       INT           NOT NULL,
    cantidad          INT           NOT NULL,
    costo_unitario    DECIMAL(10,2) NOT NULL,
    subtotal          DECIMAL(12,2) NOT NULL,
    CONSTRAINT FK_DETALLE_COMPRA     FOREIGN KEY (id_compra)   REFERENCES COMPRA(id_compra) ON DELETE CASCADE,
    CONSTRAINT FK_DETALLE_COMPRA_REP FOREIGN KEY (id_repuesto) REFERENCES REPUESTO(id_repuesto),
    CONSTRAINT chk_cantidad_compra_positiva  CHECK (cantidad > 0),
    CONSTRAINT chk_costo_unitario_positivo   CHECK (costo_unitario > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =====================================================================
-- TABLA: MOVIMIENTO_INVENTARIO (SOLO PARA AJUSTES/MERMAS/DEVOLUCIONES)
-- =====================================================================
CREATE TABLE MOVIMIENTO_INVENTARIO (
    id_movimiento       INT AUTO_INCREMENT PRIMARY KEY,
    fecha_movimiento    DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id_repuesto         INT          NOT NULL,
    cantidad            INT          NOT NULL,
    id_tipo_movimiento  INT          NOT NULL,
    id_usuario          INT          NOT NULL,
    observaciones       VARCHAR(200) NULL,
    motivo              VARCHAR(100) NULL,
    CONSTRAINT FK_MOV_REPUESTO FOREIGN KEY (id_repuesto)        REFERENCES REPUESTO(id_repuesto),
    CONSTRAINT FK_MOV_TIPO     FOREIGN KEY (id_tipo_movimiento) REFERENCES TIPO_MOVIMIENTO(id_tipo_movimiento),
    CONSTRAINT FK_MOV_USUARIO  FOREIGN KEY (id_usuario)         REFERENCES USUARIO(id_usuario),
    CONSTRAINT chk_cantidad_movimiento_positiva CHECK (cantidad > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =====================================================================
-- INSERTS: DATOS MAESTROS
-- =====================================================================

INSERT INTO ROL (nombre_rol, descripcion) VALUES
('Administrador', 'Acceso total: gestión de productos, usuarios, reportes e inventario.'),
('Vendedor',      'Acceso limitado: ventas en mostrador y consulta de stock.');

INSERT INTO ESTADO_USUARIO (nombre_estado) VALUES
('Activo'),
('Inactivo'),
('Suspendido');

INSERT INTO ESTADO_GENERAL (nombre_estado) VALUES
('Activo'),
('Inactivo'),
('Descontinuado');

INSERT INTO CATEGORIA (nombre_categoria, descripcion) VALUES
('Frenos',     'Pastillas, discos, zapatas y líquido de frenos.'),
('Suspensión', 'Amortiguadores, rótulas, terminales y bujes.'),
('Motor',      'Correas, filtros, bujías y sensores.'),
('Eléctrico',  'Baterías, alternadores y motores de arranque.'),
('Aceites',    'Lubricantes de motor, transmisión y dirección.');

INSERT INTO METODO_PAGO (nombre_metodo, descripcion) VALUES
('Efectivo', 'Pago en billetes y monedas en mostrador.'),
('QR',       'Pago móvil mediante código QR (Tigo Money, BNB, etc.).'),
('Tarjeta',  'Pago con tarjeta de débito o crédito.');

INSERT INTO IMPUESTO (nombre_impuesto, porcentaje, vigente) VALUES
('IVA', 13.00, 1);

INSERT INTO ESTADO_VENTA (nombre_estado) VALUES
('Confirmada'),
('Anulada'),
('Pendiente');

INSERT INTO TIPO_MOVIMIENTO (nombre_tipo, descripcion, signo) VALUES
('Ingreso',    'Entrada de mercadería desde proveedor.',              1),
('Ajuste+',    'Ajuste positivo de inventario por el administrador.',  1),
('Ajuste-',    'Ajuste negativo de inventario por el administrador.', -1),
('Merma',      'Pérdida de mercadería por daño o vencimiento.',       -1),
('Devolución', 'Devolución de mercadería al proveedor.',              -1);

-- =====================================================================
-- INSERTS: USUARIOS (1 Admin + 2 Vendedores)
-- NOTA: Reemplazar los hashes por reales antes de producción
-- =====================================================================
INSERT INTO USUARIO (nombre_usuario, contrasena_hash, nombre_completo, id_rol, id_estado_usuario) VALUES
('admin',     '$2a$11$REEMPLAZAR_HASH_REAL_ADMIN',     'Carlos Flores Martinez', 1, 1),
('vendedor1', '$2a$11$REEMPLAZAR_HASH_REAL_VENDEDOR1', 'Brayan Flores Marzana',  2, 1),
('vendedor2', '$2a$11$REEMPLAZAR_HASH_REAL_VENDEDOR2', 'Nestor Maraza Acarapi',  2, 1);

-- =====================================================================
-- INSERTS: CLIENTES (Registro inicial de clientes conocidos)
-- =====================================================================
INSERT INTO CLIENTE (nombre, nit, telefono, email, direccion, id_estado) VALUES
('Juan Pérez',              '10234567',   '70712345', 'juan.perez@gmail.com', 'Calle Bolívar #100, Cochabamba',    1),
('María González',          NULL,         '70767890', NULL,                   NULL,                                1),
('Taller Mecánico El Rayo', '3045678012', '4449876',  'taller@elrayo.com',    'Av. Panamericana Km 5, Cochabamba', 1);

-- =====================================================================
-- INSERTS: PROVEEDORES
-- =====================================================================
INSERT INTO PROVEEDOR (nombre, nit, telefono, direccion, email, id_estado) VALUES
('Repuestos del Sur S.R.L.', '1023456011', '4441234', 'Av. Ayacucho #123, Cochabamba',     'ventas@repsur.com',   1),
('Importadora Andina Ltda.', '1023456022', '4445678', 'Calle 25 de Mayo #456, Cochabamba', 'contacto@andina.com', 1);

-- =====================================================================
-- FIN DEL SCRIPT
-- =====================================================================