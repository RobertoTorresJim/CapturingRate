using UnityEngine;

public class Capture : MonoBehaviour
{
    // Propiedades del Mounstro y Capsule
    public int maxHP;       // HP máximo del Mounstro
    public int currentHP;   // HP actual del Mounstro
    public int captureRate; // Ratio de captura del Mounstro
    public float ballBonus; // Bonificación de la Capsule (por ejemplo, 2 para Ultra Ball)
    public float statusBonus; // Bonificación por estado alterado (1.5 para parálisis, 2 para sueño/quemadura)

    void Start()
    {
        // Ejemplo de valores iniciales
        maxHP = 100;          // HP máximo del Mounstro
        currentHP = 20;       // HP actual
        captureRate = 45;     // Ratio de captura (e.g., Mounstro legendario)
        ballBonus = 2.0f;     // Ultra Ball
        statusBonus = 1.5f;   // Parálisis

        // Calcular la probabilidad y simular captura
        bool isCaptured = TryCapture();
        Debug.Log(isCaptured ? "¡Mounstro capturado!" : "El Mounstro escapó.");
    }

    bool TryCapture()
    {
        // Paso 1: Calcular el valor base "a"
        float a = ((3.0f * maxHP - 2.0f * currentHP) / (3.0f * maxHP)) * captureRate * ballBonus * statusBonus;

        // Paso 2: Limitar el valor "a" al rango permitido (0 a 255)
        a = Mathf.Clamp(a, 0, 255);

        // Paso 3: Calcular el valor "b" y determinar el éxito de la captura
        float b = a / 255.0f; // Probabilidad normalizada entre 0 y 1

        // Simular cuatro sacudidas (según el sistema de tercera generación)
        for (int i = 0; i < 4; i++)
        {
            float randomValue = Random.value; // Generar un número aleatorio entre 0 y 1
            if (randomValue >= b)
            {
                return false; // Fallo en una sacudida, captura fallida
            }
        }

        return true; // Todas las sacudidas fueron exitosas, Mounstro capturado
    }
}
