# CapturingRate
Capture rate using pkmn ecuation for third generation games.
El código calcula la probabilidad de captura de un Pokémon con base en la fórmula:

<img align="" src="https://latex.codecogs.com/svg.image?&space;a=\left(\frac{3H_m-2H_c}{3H_m}\right)RBS"/>


## Explicación del código:
### Variables principales:

**maxHP y currentHP:** Representan los puntos de salud máximo y actual del Mounstro.
**captureRate:** El ratio de captura del Mounstro (por ejemplo, 3 para un legendario o 255 para un basico).
**ballBonus:** Representa la bonificación de la Capsula de captura usada (Capsula normal = 1, Ultra Capsula = 2, etc.).
**statusBonus:** El efecto de un estado alterado (por ejemplo, sueño o parálisis).
**Cálculo del valor base _𝑎_:** Se calcula usando la fórmula principal de tercera generación. Se asegura que _𝑎_ esté dentro del rango válido (0 a 255).

**Simulación de sacudidas:** Se realizan 4 comparaciones aleatorias contra la probabilidad normalizada _𝑏_. Si una de ellas falla, el Mounstro escapa.

**Probabilidad ajustada:** La probabilidad de captura total se ajusta según las mecánicas de las generaciones clásicas.
# Escena de Unity
**1. Capsule Ball (GameObject):**
- Crea un GameObject (por ejemplo, un cubo o una esfera) y asigna el script Capsuleball.cs.
- Configura su posición inicial lejos del Mounstro.
  
**2. Monster (GameObject):**
- Crea un segundo GameObject (por ejemplo, otro cubo o esfera) para representar al Mounstro.
- Coloca este GameObject en el campo de la escena.
  
**3. Configuración del Inspector:**
- Asigna el GameObject del Mounstro a la variable targetMonster en el componente Capsuleball del Inspector.
- Ajusta las estadísticas como maxHP, currentHP, captureRate, ballBonus y statusBonus.
