# Guion de vídeo — Unity Currency System

Unity Currency System v1.0 · Koeenji Dev

---

## Objetivo del vídeo

Presentar el Unity Currency System como un sistema técnico de portfolio. El objetivo no es mostrar un videojuego completo, sino demostrar:

- la arquitectura modular de un sistema de recolección de monedas;
- la separación de responsabilidades entre pickup, wallet y UI;
- el flujo de eventos que conecta las capas sin acoplamiento directo;
- la suite de pruebas automatizadas que valida el contrato del sistema;
- la calidad documental y el proceso de desarrollo.

---

## Duración estimada

**6 a 9 minutos.**

---

## Estructura por secciones

| Sección | Título                              | Duración estimada |
|---------|-------------------------------------|-------------------|
| 1       | Introducción y contexto             | 1 minuto          |
| 2       | Problema y solución                 | 1–1.5 minutos     |
| 3       | Arquitectura del sistema            | 1.5–2 minutos     |
| 4       | Demo en Play Mode                   | 1.5–2 minutos     |
| 5       | Pruebas automatizadas               | 1 minuto          |
| 6       | Estructura del proyecto             | 0.5–1 minuto      |
| 7       | Limitaciones y cierre               | 0.5–1 minuto      |

---

## Guion completo

---

### Sección 1 — Introducción y contexto

**[Toma: pantalla del Editor con la escena demo abierta en modo edición. Hierarchy, Scene view e Inspector visibles.]**

> Hola, soy Koeenji Dev. Este es el primer proyecto de mi portfolio de sistemas Unity.

> El Unity Currency System es exactamente eso: un sistema técnico. No es un juego completo, no tiene historia, no tiene niveles. Es un sistema de recolección de monedas construido con Unity 6, diseñado para ser modular, probado y documentado.

> Su propósito dentro del portfolio es demostrar cómo separar la lógica económica, la interacción de pickup y la presentación de UI en componentes independientes y reutilizables.

**Frase clave:**
> "Esto no es un juego. Es la base de la economía de cualquier juego."

---

### Sección 2 — Problema y solución

**[Toma: Inspector con los componentes del Player seleccionado. CurrencyWallet y CurrencyCollector visibles.]**

> El problema clásico al implementar monedas en Unity es mezclar todo en un mismo componente. El pickup acumula el saldo, actualiza la UI y reproduce el sonido, todo desde el mismo `OnTriggerEnter2D`. Eso funciona para un prototipo de una hora, pero crea un sistema frágil, acoplado e imposible de probar de forma aislada.

**[Toma: diagrama o documento ARCHITECTURE.md con el flujo visible.]**

> La solución aquí es la separación de responsabilidades. Cada componente hace exactamente una cosa:

> El `CurrencyPickup` detecta la colisión y delega.
> El `CurrencyCollector` expone el acceso a la cartera.
> El `CurrencyWallet` valida y acumula el saldo.
> El `CurrencyDisplay` escucha el evento y actualiza la UI.

> Ninguno de estos componentes sabe qué hacen los demás. Se comunican a través de un evento.

**Frase clave:**
> "Separar responsabilidades no es una complicación. Es lo que hace que el sistema pueda probarse, reutilizarse y extenderse."

---

### Sección 3 — Arquitectura del sistema

**[Toma: Editor con el flujo del sistema visible. Alternar entre el Inspector de CurrencyPickup, CurrencyWallet y CurrencyDisplay.]**

> Voy a recorrer el flujo completo, componente por componente.

**[Toma: Inspector de un prefab de pickup con el campo Value visible.]**

> El `CurrencyPickup` almacena un valor positivo configurable: `1`, `5` o `10`. Cuando detecta un trigger con un objeto que tiene `CurrencyCollector`, llama a `TryAdd` en la cartera. Si la adición es válida, deshabilita su propio collider, reproduce el sonido opcional y se destruye. Si algo falla, no ocurre nada.

**[Toma: Inspector del Player con CurrencyWallet y su campo Balance visible.]**

> El `CurrencyWallet` es el único componente que posee el saldo. El balance es de solo lectura externamente. `TryAdd` valida que el valor sea positivo y que no cause desbordamiento de entero. Solo después de una adición válida emite `BalanceChanged` con el total acumulado.

**[Toma: Inspector del HUD con CurrencyDisplay y sus referencias visibles.]**

> El `CurrencyDisplay` no sondea el saldo en `Update`. Se suscribe al evento `BalanceChanged` en `OnEnable` y se desuscribe en `OnDisable`. También muestra el saldo actual al activarse, por si la cartera ya tenía monedas.

**Frase clave:**
> "El flujo completo es: CurrencyPickup → CurrencyCollector → CurrencyWallet → BalanceChanged → CurrencyDisplay."

---

### Sección 4 — Demo en Play Mode

**[Toma: Game view con el HUD mostrando `0`. Entrar en Play Mode.]**

> La escena demo es top-down 2D. El jugador se mueve con WASD o las teclas de dirección. Hay cinco monedas repartidas por el nivel.

> El HUD empieza en `0`.

**[Toma: movimiento del jugador hacia la primera moneda. HUD actualiza a `1`.]**

> Primera moneda: valor `1`. El HUD pasa a `1`.

**[Toma: segunda moneda recogida. HUD actualiza.]**

> Segunda moneda: valor `1`. HUD: `2`.

**[Toma: tercera y cuarta moneda recogidas. Valores 5 y 5.]**

> Tercera y cuarta: valores `5` y `5`. HUD: `7`, luego `12`.

**[Toma: quinta moneda recogida. HUD llega a `22`.]**

> Quinta moneda: valor `10`. Total final: `22`.

> `1 + 1 + 5 + 5 + 10 = 22`. Exactamente lo esperado.

**[Toma: pausa en el HUD mostrando `22`. Console visible con cero errores.]**

> La consola no muestra ningún error durante todo el flujo.

**Frase clave:**
> "El total siempre es 22. Cualquier desviación indicaría un bug."

---

### Sección 5 — Pruebas automatizadas

**[Toma: Test Runner abierto en modo Edit Mode. Lista de 9 pruebas con checkmarks verdes.]**

> El sistema incluye 9 pruebas automatizadas en Edit Mode para la lógica de `CurrencyWallet`.

> Las pruebas cubren:

> — que el saldo inicial es cero;
> — que una cantidad positiva aumenta el saldo correctamente;
> — que se acumulan varias cantidades;
> — que el cero es rechazado;
> — que los valores negativos son rechazados;
> — que las adiciones válidas emiten el evento exactamente una vez;
> — que las adiciones inválidas no emiten el evento;
> — que los eventos sucesivos emiten el total acumulado;
> — que las adiciones que causarían desbordamiento de entero son rechazadas.

**[Toma: Test Runner mostrando `9 passed / 0 failed`.]**

> 9 pruebas, 9 pasadas, 0 fallidas.

**Frase clave:**
> "Las pruebas prueban el contrato del sistema, no la implementación interna."

---

### Sección 6 — Estructura del proyecto

**[Toma: Project window con `Assets/_Project/Systems/Currency` expandido.]**

> La estructura sigue la separación que marca la arquitectura. El sistema de monedas está en `Assets/_Project/Systems/Currency`, con subdirectorios para el runtime, la UI, los prefabs y las pruebas.

> El código de demo, el prefab del jugador y los controles de input están en `Assets/_Project/Demo`, completamente separados del sistema reutilizable.

**[Toma: Project window con `Runtime/` visible, mostrando los cuatro scripts del sistema.]**

> `CurrencyWallet`, `CurrencyCollector`, `CurrencyPickup` y `CurrencyDisplay`. Cuatro scripts, cuatro responsabilidades distintas.

**Frase clave:**
> "La estructura de carpetas refleja la arquitectura. Si ves `Systems/Currency`, sabes exactamente qué contiene."

---

### Sección 7 — Limitaciones y cierre

**[Toma: Editor en modo edición. Vista general de la escena.]**

> Antes de cerrar, las limitaciones conocidas de v1.0.

> Este sistema no incluye gasto de monedas, tiendas, save/load, persistencia entre escenas, pooling de objetos ni soporte para multijugador. Tampoco tiene soporte para mando validado ni pruebas en Play Mode automatizadas.

> Estas limitaciones son intencionales. El objetivo de v1.0 era demostrar la arquitectura de recolección de forma limpia y completamente validada. Las extensiones vendrán en proyectos posteriores.

**[Toma: repositorio de GitHub visible en el navegador, página principal.]**

> El proyecto está publicado en GitHub bajo `Koeenji-dev/unity-currency-system`, con documentación pública en inglés y en español.

**[Toma: vuelta al Editor. Escena con todos los pickups visibles.]**

> Si estás construyendo un sistema de monedas para un prototipo o un proyecto propio, esta arquitectura te da una base limpia y comprobable. Puedes añadir gasto de monedas, tiendas o persistencia sin necesidad de reescribir desde cero.

> Gracias por ver el vídeo. El siguiente sistema del portfolio es el 02 — Slope and Surface Physics.

**Frase clave final:**
> "No es un juego. Es el sistema que haría que el juego funcione."

---

## Tomas OBS — Resumen

| Toma | Contenido                                              | Tipo     |
|------|--------------------------------------------------------|----------|
| 1    | Editor completo, escena demo en modo edición           | Estática |
| 2    | Inspector del Player con CurrencyWallet/Collector      | Estática |
| 3    | Diagrama de arquitectura o ARCHITECTURE.md             | Estática |
| 4    | Inspector del prefab de pickup (campo Value)           | Estática |
| 5    | Inspector del Player (CurrencyWallet y Balance)        | Estática |
| 6    | Inspector del HUD (CurrencyDisplay y referencias)      | Estática |
| 7    | Play Mode: HUD en `0` al iniciar                       | Dinámica |
| 8    | Recogida secuencial de los 5 pickups hasta `22`        | Dinámica |
| 9    | HUD final en `22` + Console sin errores                | Estática |
| 10   | Test Runner: 9 pruebas, 9 passed, 0 failed             | Estática |
| 11   | Project window: `Systems/Currency` expandido           | Estática |
| 12   | Project window: `Runtime/` con los 4 scripts           | Estática |
| 13   | GitHub: página del repositorio                         | Estática |

---

## Frases clave — Resumen

> "Esto no es un juego. Es la base de la economía de cualquier juego."

> "Separar responsabilidades no es una complicación. Es lo que hace que el sistema pueda probarse, reutilizarse y extenderse."

> "El flujo completo es: CurrencyPickup → CurrencyCollector → CurrencyWallet → BalanceChanged → CurrencyDisplay."

> "El total siempre es 22. Cualquier desviación indicaría un bug."

> "Las pruebas prueban el contrato del sistema, no la implementación interna."

> "La estructura de carpetas refleja la arquitectura."

> "No es un juego. Es el sistema que haría que el juego funcione."

---

## Errores que no se deben mostrar

No mostrar en ningún momento durante la grabación:

- Errores de consola de cualquier tipo (rojo o amarillo).
- Scripts con `Missing` o referencias nulas en el Inspector.
- Pickups que se recojan dos veces sin desaparecer.
- HUD que no actualice tras una recogida.
- Errores de compilación o recompilación del Editor durante la grabación.
- Pantallas de configuración de Unity, diálogos de importación o ventanas modales no relacionadas con la demo.
- Rutas de carpetas privadas o nombres de workspace visibles en la barra de título.
- Contenido de `_LocalContext/` o `.cursor/` visible en ningún panel del Editor.

---

## Checklist antes de grabar

- [ ] Proyecto abierto en `CurrencyDemo.unity`.
- [ ] Consola limpia: `0` errores, `0` advertencias relevantes.
- [ ] Test Runner ejecutado y mostrando `9 passed / 0 failed`.
- [ ] Play Mode validado manualmente: total `22`, sin errores en consola.
- [ ] Los 5 pickups están presentes en la jerarquía de la escena.
- [ ] El jugador está en la posición de inicio.
- [ ] El sonido de recogida es audible y no distorsiona.
- [ ] `CoinVisualSpin` visible en todos los pickups antes de recoger ninguno.
- [ ] Resolución del Game view fijada (recomendado: `1920×1080`).
- [ ] OBS configurado y grabación de prueba revisada.
- [ ] Notificaciones del sistema desactivadas.
- [ ] Guion leído y secciones preparadas.
- [ ] Grabación de prueba corta realizada para verificar audio y vídeo.
