# Guía Teórica: Proyecto TaskManager (ABM)

Este documento sirve como material de estudio para comprender la arquitectura y los conceptos fundamentales de este proyecto de Gestión de Tareas (Task Manager). Es ideal para perfiles **Trainee/Junior** que se preparan para entrevistas técnicas.

---

## 1. Estructura del Proyecto: Arquitectura por Capas

El proyecto sigue un patrón de diseño fundamental en el desarrollo web moderno: la **Separación de Responsabilidades**. En lugar de tener todo el código en un solo lugar, lo dividimos en "capas" donde cada una tiene una función específica.

### 📁 Estructura de Carpetas

- **Controllers/**: Capa de Presentación (API).
- **Services/**: Capa de Lógica de Negocio.
- **Modelos/**: Capa de Entidades (Datos).
- **DB/**: Capa de Acceso a Datos (Contexto de BD).
- **Program.cs**: Punto de entrada y Configuración.

---

## 2. Explicación de las Capas

### A. Capa de Modelos (`Modelos/`)

Aquí definimos **qué datos** maneja nuestra aplicación.

- **POO (Programación Orientada a Objetos)**: Usamos clases para representar objetos del mundo real (`Tarea`, `Usuario`).
- **Relaciones**: Una tarea "pertenece" a un Usuario (`UsuarioId`). Un Usuario puede tener muchas tareas.

### B. Capa de Acceso a Datos (`DB/`)

Utilizamos **Entity Framework Core**, que es un **ORM** (Object-Relational Mapper).

- **¿Qué hace?**: Permite interactuar con la base de datos (SQLite) usando código C# en lugar de escribir consultas SQL manualmente.
- `AppDBContext`: Es el "puente" entre nuestro código y la base de datos.

### C. Capa de Servicios (`Services/`) - ¡La más importante!

Aquí reside la **Lógica de Negocio**. No se trata solo de guardar datos, sino de aplicar reglas.

- **Ejemplos de Reglas en este proyecto**:
  - "No se puede crear una tarea con fecha vencida".
  - "Un usuario no puede tener más de 5 tareas pendientes".
  - "No se puede eliminar un usuario si tiene tareas activas".

### D. Capa de Controladores (`Controllers/`)

Es la "puerta de entrada" para el mundo exterior.

- Recibe peticiones HTTP (`GET`, `POST`, `PUT`, `DELETE`).
- **Validación de Respuesta**: Maneja errores usando bloques `try-catch` y devuelve códigos de estado HTTP adecuados (`200 OK`, `400 BadRequest`, `201 Created`).

---

## 3. Conceptos Clave para la Entrevista

### 💉 Inyección de Dependencias (Dependency Injection)

Es un patrón donde una clase no "crea" sus herramientas, sino que se las "pasan" ya listas.

- **En este proyecto**: El `TareasController` necesita el `TareaService`. En lugar de hacer `new TareaService()`, lo recibe por constructor.
- **Beneficio**: Facilita las pruebas unitarias (Testing) y hace que el código sea más flexible.

### 🌐 Programación Asíncrona (`async` / `await`)

Permite que la aplicación no se "bloquee" mientras espera que la base de datos responda.

- Usamos `Task`, `async` y `await` para que el servidor pueda atender otras peticiones mientras se procesan operaciones de entrada/salida (I/O).

### 🧪 Lógica de Programación y ABM

**ABM** significa Alta, Baja y Modificación (en inglés CRUD: Create, Read, Update, Delete).

- Es la base de cualquier sistema: saber cómo crear, listar, editar y borrar registros de forma segura y validada.

---

## 4. Casos de Uso Analizados (solo algunos ejemplos)

### Caso de Uso: Crear Tarea

1. El **Controller** recibe los datos.
2. El **Service** verifica:
   - ¿La fecha es válida?
   - ¿El usuario existe?
3. Si todo está ok, el **DBContext** guarda los cambios.

### Caso de Uso: Eliminar Usuario

1. El **Service** busca al usuario.
2. Verifica si tiene tareas pendientes.
3. Si tiene tareas sin completar, lanza una **Excepción** (Error) impidiendo la eliminación por seguridad de datos.

---
