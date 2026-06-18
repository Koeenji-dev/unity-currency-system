# Unity Currency System

Sistema de recogida de monedas 2D reutilizable construido con Unity 6, diseñado como parte del portfolio de sistemas Unity de Koeenji Dev.

---

## Descripción general

Este proyecto demuestra un sistema de recogida de monedas autocontenido para un contexto 2D top-down. Cubre:

- monedas coleccionables con valores `1`, `5` y `10`;
- almacenamiento del saldo en una cartera;
- actualizaciones de UI basadas en eventos;
- protección contra recogida duplicada;
- sonido de recogida opcional;
- efecto visual de giro ligero;
- pruebas automatizadas en Edit Mode.

---

## Demo

El proyecto incluye una escena de demo 2D top-down en la que el jugador se mueve con el teclado y recoge monedas repartidas por el nivel.

- Movimiento con teclado: WASD y teclas de dirección.
- Cinco monedas están colocadas en la escena.
- Saldo total disponible: `22`.
- El HUD se actualiza después de cada recogida válida.

> El vídeo de demo y las capturas de pantalla se añadirán tras la grabación final del portfolio.

---

## Portfolio y medios

| Recurso | Descripción |
|---|---|
| [`Documentation/MEDIA_GUIDE.md`](Documentation/MEDIA_GUIDE.md) | Especificaciones de capturas y GIFs, checklist de OBS, convención de nombres y checklist final de medios. |
| [`Documentation/VIDEO_SCRIPT.es.md`](Documentation/VIDEO_SCRIPT.es.md) | Guion completo en español con estructura por secciones, frases clave y lista de tomas OBS. |

---

## Funcionalidades

- `CurrencyWallet` — almacena el saldo, valida cantidades positivas, protege contra desbordamiento de entero, emite `BalanceChanged`.
- `CurrencyCollector` — mantiene la referencia explícita a la cartera; actúa como punto de entrada para las monedas recogidas.
- `CurrencyPickup` — valor de recogida configurable; se destruye solo tras una adición exitosa; previene la recogida duplicada.
- `CurrencyDisplay` — se suscribe a `BalanceChanged` en `OnEnable`; se desuscribe en `OnDisable`; muestra el saldo actual inmediatamente al activarse.
- Nuevo Unity Input System a través de `PlayerControls.inputactions`.
- Prefab de jugador reutilizable.
- `CoinVisualSpin` — efecto visual ligero basado en escala para las monedas.
- 9 pruebas automatizadas en Edit Mode para la lógica de la cartera.

---

## Arquitectura

```text
CurrencyPickup
→ CurrencyCollector
→ CurrencyWallet
→ BalanceChanged
→ CurrencyDisplay
```

### Responsabilidades de los componentes

**CurrencyWallet**
Almacena el saldo actual. Acepta cantidades positivas válidas, rechaza cero y valores negativos, y protege contra desbordamiento de `int`. Expone `Balance` como solo lectura y emite `BalanceChanged` con el total actualizado tras cada adición válida.

**CurrencyCollector**
Mantiene una referencia serializada a `CurrencyWallet` y la expone como solo lectura. Sirve como punto de entrada que los pickups buscan en la entidad recolectora. No almacena saldo ni interactúa con la UI.

**CurrencyPickup**
Contiene un valor positivo configurable. Detecta un `CurrencyCollector` en el collider que entra usando `GetComponentInParent`. Llama a `TryAdd` en la cartera y se destruye solo si la adición tiene éxito. Deshabilita su collider inmediatamente tras la recogida para evitar disparos duplicados.

**CurrencyDisplay**
Recibe referencias explícitas a un `CurrencyWallet` y a un componente `TMP_Text`. Se suscribe a `BalanceChanged` al activarse y se desuscribe al desactivarse. Muestra el saldo actual inmediatamente al activarse. Nunca realiza consultas en `Update`.

**PlayerMovement2D**
Lee la entrada `Vector2` del nuevo Input System y aplica el movimiento a un `Rigidbody2D` en `FixedUpdate`. Permanece completamente independiente del sistema de monedas.

**CoinVisualSpin**
Anima un `Transform` objetivo oscilando su escala en X usando `Mathf.Cos`, produciendo un efecto visual de giro ligero. No tiene conocimiento del sistema de monedas, colliders, audio ni UI.

El código de demo y la presentación visual están separados de la lógica económica tanto en el espacio de nombres como en la estructura de carpetas.

---

## Estructura del proyecto

```text
Assets/_Project/
├── Audio/
├── Demo/
│   ├── Input/
│   ├── Prefabs/
│   └── Runtime/
├── Scenes/
└── Systems/
    └── Currency/
        ├── Prefabs/
        ├── Runtime/
        ├── Tests/
        └── UI/
```

Los assets visuales de terceros se encuentran en:

```text
Assets/ThirdParty/2DRPK/
```

---

## Requisitos

- Unity `6000.3.17f1`
- Plantilla Universal 2D / URP 17.3.0
- Unity Input System `1.19.0`
- Unity Test Framework `1.6.0`
- TextMeshPro / uGUI

---

## Controles

```
WASD            Mover
Arrow Keys      Mover
```

---

## Pruebas

Pruebas en Edit Mode para `CurrencyWallet`:

```
9 pruebas
9 pasadas
0 fallidas
```

Casos cubiertos:

- el saldo inicial es cero;
- una cantidad positiva aumenta el saldo y devuelve `true`;
- varias cantidades válidas se acumulan correctamente;
- el cero es rechazado y devuelve `false`;
- las cantidades negativas son rechazadas y devuelven `false`;
- las adiciones válidas emiten `BalanceChanged` exactamente una vez;
- las adiciones inválidas no emiten `BalanceChanged`;
- los eventos sucesivos emiten el saldo acumulado actual;
- las adiciones que causarían desbordamiento de `int` son rechazadas.

---

## Decisiones de diseño

- **Sin singleton.** Todas las dependencias se inyectan mediante referencias serializadas explícitas en el Inspector.
- **Sin gestor global.** Cada componente mantiene únicamente las referencias que necesita.
- **Sin ScriptableObjects en v1.0.** La cartera es un `MonoBehaviour` simple.
- **Sin persistencia.** El saldo se reinicia al reiniciar el Play Mode por diseño.
- **UI basada en eventos.** `CurrencyDisplay` reacciona a `BalanceChanged`; nunca realiza consultas en `Update`.
- **Assembly Definitions mínimas.** Solo se usan dos: una para el aislamiento en runtime (`KoeenjiDev.CurrencySystem`) y otra para las pruebas en Edit Mode (`Tests`).
- **Sin búsquedas globales.** Se usa `GetComponentInParent` para la búsqueda local; `GameObject.Find` y `FindFirstObjectByType` no se usan.
- **Solo nuevo Input System.** El antiguo Input Manager no está referenciado en ningún punto del proyecto.

---

## Assets de terceros

### 2D Animated Coin — 2D RPK

El sprite utilizado para la presentación visual de los pickups proviene del asset 2D RPK.

- Se utiliza únicamente para la presentación visual de la moneda.
- El componente `Animator` original permanece desactivado porque la animación importada no funcionó correctamente en la versión actual de Unity.
- La animación visual la gestiona `CoinVisualSpin` en su lugar.

### Sonido de recogida de moneda

Se utiliza un clip de audio externo como sonido de recogida opcional.

- Se reproduce mediante `AudioSource.PlayClipAtPoint` en la posición de la moneda.
- La recogida funciona correctamente aunque no haya ningún clip asignado.

Los assets de terceros están sujetos a sus respectivas licencias y condiciones de uso.

---

## Alcance actual

**Implementado:**

- recogida de monedas;
- gestión del saldo;
- HUD basado en eventos;
- sonido de recogida opcional;
- retroalimentación visual de giro;
- pruebas automatizadas de la cartera.

**Fuera del alcance en v1.0:**

- gasto de monedas;
- tiendas;
- guardar y cargar;
- persistencia entre escenas;
- multijugador;
- pooling de objetos;
- recogida magnética;
- gestión global de audio.

---

## Mejoras futuras

- gasto de monedas;
- persistencia entre escenas;
- definiciones de moneda basadas en ScriptableObject;
- pooling de objetos para escenas grandes;
- efectos visuales mejorados;
- soporte de mando tras pruebas adecuadas;
- extracción como paquete Unity reutilizable.

---

## English Version

The English version of this README is available in `README.md`.
