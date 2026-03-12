# 🚀 Ejercicios Prácticos para tu ABM

Si estás leyendo esto, es porque estás listo para empezar a escribir código y darle más vida a este Gestor de Tareas. No te preocupes si nunca antes programaste; estos ejercicios están diseñados paso a paso para que entiendas **qué** estamos haciendo y **por qué** lo estamos haciendo.

La programación es como armar bloques de Lego: empezamos con piezas pequeñas, entendemos cómo encajan, y luego construimos cosas más grandes. ¡Vamos a empezar!

---

## 🛠️ Ejercicio 1: Agregar "Prioridad" a las Tareas (Nivel: Muy Básico)

**Contexto:** En la vida real, algunas tareas son urgentes y otras pueden esperar. Queremos que nuestro sistema pueda registrar la "Prioridad" de cada tarea (por ejemplo: "Alta", "Media", "Baja").

**Conceptos a aprender:**

- Entender qué es un "Modelo" (la "plantilla" de nuestros datos).
- Asignar valores por defecto.

### Paso a paso:

1. **Modificar el Modelo:**
   Ve a la carpeta `Modelos` y abre el archivo `Tarea.cs`.
   Este archivo define cómo es una Tarea. Vamos a agregarle una nueva "característica" (en programación se llama "propiedad").
   Debajo de `public string Descripcion { get; set; }`, agrega esta línea:

   ```csharp
   public string Prioridad { get; set; }
   ```

   _Explicación:_ Le estamos diciendo al sistema que todas las tareas ahora tendrán un texto (`string`) llamado "Prioridad". El `get; set;` significa que podemos "leer" su valor y también "escribir" o asignarle un valor.

2. **Dando un valor por defecto (Lógica de Negocio):**
   A veces, el usuario se olvidará de mandar la prioridad. No queremos que quede vacía.
   Ve a la carpeta `Services` y abre `TareaService.cs`.
   Busca el método llamado `CrearTareaAsync`. Este lugar es donde se prepara la tarea antes de guardarse.
   Justo antes de la línea `await _context.Tareas.AddAsync(nuevaTarea);`, agrega este código:

   ```csharp
   // Si la prioridad viene vacía o nula, le ponemos "Normal" por defecto
   if (string.IsNullOrEmpty(nuevaTarea.Prioridad))
   {
       nuevaTarea.Prioridad = "Normal";
   }
   ```

   _Explicación:_ Un condicional `if` (si) pregunta algo. Aquí pregunta: "¿Está vacío el texto de la Prioridad?". Si la respuesta es sí, ejecuta el bloque de código entre `{ }` y le asigna la palabra "Normal".

3. **¡La Base de Datos tiene que enterarse! (Migraciones):**
   Hasta acá todo viene bien pero debemos actualizar nuestra base de datos para que tenga en cuenta que las tareas ahora tienen una "Prioridad", ¿Cómo lo hacemos? Usando las migraciones de Entity Framework.

   Abri una terminal ahí en tu editor, fijate de estar adentro de la carpeta donde está el archivo `TaskManagerProject.csproj` y tira estos dos comandos:

   a. **Primero preparamos las instrucciones:**

   ```powershell
   dotnet ef migrations add AgregarPrioridadATarea
   ```

   _Explicación:_ Con esto básicamente le decimos al programa: _"Che, fijate que cambié los Modelos. armame el codigo sql con los cambios que hay que hacerle a la base de datos y ponele de nombre 'AgregarPrioridadATarea'"_. Ojo, que esto todavía no cambia la base de datos, solo arma un archivito con las instrucciones.

   b. **Ahora sí, aplicamos el cambio:**

   ```bash
   dotnet ef database update
   ```

   _Explicación:_ Este es el paso clave. Acá le decimos: _"Agarrá ese machete que armaste antes y aplicalo de verdad en la base de datos"_. ¡Listo! Ahora tu base de datos SQLite tiene una columna 'Prioridad' nuevita en la tabla 'Tareas'.

🎉 ¡Felicidades! Acabas de agregar tu primera funcionalidad real al proyecto modificando la base de datos y la lógica.

---

## 🛡️ Ejercicio 2: Validar el tamaño del Título (Nivel: Básico)

**Contexto:** A veces la gente escribe cosas raras, como títulos de una sola letra ("a") para describir una tarea. Queremos evitar eso y obligar al usuario a ser más descriptivo.

**Conceptos a aprender:**

- Lanzar errores (Excepciones) para proteger el sistema.
- Validaciones simples.

### Paso a paso:

1. **Agregar la regla al crear una tarea:**
   Abre nuevamente `Services/TareaService.cs`.
   Busca el método `CrearTareaAsync`. Al principio de este bloque de código, debajo de donde empieza el método `{`, vamos a poner nuestra regla tipo "guardia de seguridad".

   Agrega lo siguiente:

   ```csharp
   if (nuevaTarea.Titulo.Length < 3)
   {
       throw new Exception("¡Ey! El título de la tarea es muy corto. Debe tener al menos 3 letras.");
   }
   ```

   _Explicación:_ `.Length` cuenta la cantidad de letras que tiene el título de la nueva tarea. Si (`if`) esa cantidad es menor que (`<`) 3, usamos `throw new Exception` para lanzar un error y detener inmediatamente el programa. Así nos aseguramos de que no se guarde "basura" en la base de datos.

2. **(Opcional pero recomendado) Hacer lo mismo al editar:**
   Busca el método `EditarTareaAsync` en el mismo archivo.
   Antes de actualizar el título (`tarea.Titulo = titulo;`), agrega el mismo bloque `if` que usaste arriba. ¡Cuidado! En este caso debes verificar la variable `titulo` en lugar de `nuevaTarea.Titulo`.

---

## 🚫 Ejercicio 3: Evitar Tareas Duplicadas (Nivel: Intermedio)

**Contexto:** Un usuario despistado puede intentar crear dos veces exactamente la misma tarea pulsando un botón rápidamente u olvidando que ya la creó. ¡No lo vamos a permitir!

**Conceptos a aprender:**

- Realizar preguntas (consultas o _queries_) a la Base de Datos.
- Funciones Lambda (`=>`).

### Paso a paso:

1. **Buscar si ya existe esa tarea:**
   Abre `Services/TareaService.cs`. Ve a `CrearTareaAsync`.
   Después de verificar que el usuario existe, digamos a la mitad del método, agrega esta mágica línea de código:

   ```csharp
   // Buscamos si existe ALGUNA tarea para este usuario con exactamente el mismo título
   bool tareaYaExiste = await _context.Tareas.AnyAsync(t =>
       t.Titulo == nuevaTarea.Titulo && t.UsuarioId == nuevaTarea.UsuarioId
   );
   ```

   _Explicación:_ `_context.Tareas` es nuestra caja donde están todas las tareas de la base de datos. `.AnyAsync(...)` le pregunta a la base de datos: "¿Hay ALGUIEN que cumpla con esta condición?".
   La letra `t` representa "cada tarea". `=>` se lee como "tal que".
   Todo junto dice: "Dime si hay alguna tarea `t`, tal que el título de `t` sea EXACTAMENTE IGUAL AL (`==`) título nuevo, Y (`&&`) pertenezca a este mismo usuario".

2. **Frenar si existe:**
   Como `tareaYaExiste` guardará la respuesta (`true` o `false`), ahora hacemos un `if`:
   ```csharp
   if (tareaYaExiste == true)
   {
       throw new Exception("¡Ups! Ya tienes una tarea pendiente con ese mismo nombre.");
   }
   ```

💡 ¡MIRA! Ya sabes cómo preguntarle información a la base de datos en tiempo real antes de guardar cosas.

---

## 🗑️ Ejercicio 4: Borrado Lógico de Usuarios - "Desactivar" cuenta (Nivel: Avanzado/Conceptual)

**Contexto:** En sistemas reales, _casi nunca borramos algo de forma definitiva_ (porque si nos equivocamos, perdemos la información). Lo que hacemos usualmente es un "borrado lógico": ocultar al usuario apagándolo (poner que `Activo` es `Falso`).

**Conceptos a aprender:**

- Actualización de campos booleanos (`true` / `false`).
- Crear nuevos métodos completos.

### Paso a paso:

1. **Observa el modelo Usuario:**
   Si abres `Modelos/Usuario.cs`, verás que ya existe una propiedad llamada `public bool Activo { get; set; } = true;`. Esto es como un interruptor de luz encendido.

2. **Crear el método en el Servicio:**
   Ve a `Services/UsuarioService.cs` (si el archivo existe y hay otros métodos allí; si no, vamos a suponer cómo sería). Tienes que crear el mecanismo para apagar el interruptor.

   ```csharp
   public async Task DesactivarUsuarioAsync(int usuarioId)
   {
       // 1. Buscamos el usuario en la base de datos con ese Id
       var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);

       // 2. Si no lo encontramos, avisamos del error
       if (usuario == null)
       {
           throw new Exception("No encontramos al usuario que quieres desactivar.");
       }

       // 3. APAGAMOS EL INTERRUPTOR
       usuario.Activo = false;

       // 4. Guardamos los cambios en la base de datos
       await _context.SaveChangesAsync();
   }
   ```

3. _(Para pensar luego...)_ Una vez que la cuenta esté inactiva (`Activo = false`), ¿no deberíamos agregar en `TareaService.cs` una regla que prohíba a los usuarios inactivos crear nuevas tareas? ¡Intenta aplicar esa validación tú mismo!

---

### 🔥 ¿Qué aprendiste hoy?

- Modificar Modelos y Base de datos (Propiedades).
- Lógica de Programación (Usar condicionales `if`).
- Proteger tu aplicación lanzando **Excepciones** de validación.
- Consultar registros con **LINQ** (`AnyAsync`).

¡Mucho éxito con tu aprendizaje, poco a poco vas dominando la Matrix! 🚀
