using UnityEngine;
using System.Collections;

public class Capsuleball : MonoBehaviour
{
    public GameObject targetMonster; // Monster objetivo
    public float speed = 5f;         // Velocidad horizontal de la Capsule
    public float arcHeight = 2f;     // Altura máxima de la parábola
    public ParticleSystem captureEffect; // Efecto al capturar
    public ParticleSystem failEffect;    // Efecto al fallar

    public int maxHP = 100;          // HP máximo del Monster
    public int currentHP = 20;       // HP actual del Monster
    public int captureRate = 45;     // Ratio de captura del Monster
    public float ballBonus = 2.0f;   // Bonificación de la Capsule
    public float statusBonus = 1.5f; // Bonificación por estado alterado

    private Vector3 startPoint;  // Posición inicial de la Capsule
    private Vector3 targetPoint; // Posición del Monster
    private float journeyProgress = 0f; // Progreso del movimiento [0,1]
    private bool isMoving = true; // Controla el movimiento de la Capsule
    private Renderer monsterRenderer;//Render del Mounstro para controlar su visibilidad
    private Vector3 originalMonsterScale;//Escala original del Mousntro
    private bool hasCollided = false;


    void Start()
    {
        if (targetMonster != null)
        {
            startPoint = transform.position;
            targetPoint = targetMonster.transform.position;
	    monsterRenderer = targetMonster.GetComponent<Renderer>();
	    originalMonsterScale = targetMonster.transform.localScale;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveParabolic();
        }
    }

    void MoveParabolic()
    {
        // Incrementa el progreso del movimiento
        journeyProgress += Time.deltaTime * speed / Vector3.Distance(startPoint, targetPoint);

        // Interpolación entre el punto inicial y el Monster
        Vector3 nextPosition = Vector3.Lerp(startPoint, targetPoint, journeyProgress);

        // Añade la altura de la parábola
        float heightOffset = arcHeight * Mathf.Sin(journeyProgress * Mathf.PI); // Movimiento parabólico
        nextPosition.y += heightOffset;

        // Actualiza la posición de la Capsule
        transform.position = nextPosition;

        // Comprueba si alcanzó el objetivo
        if (journeyProgress >= 1f)
        {
	//NOTA: VALIDAR EL COMPORTAMIENTO DE LA BALL AL LLEGAR A ESE RANGO
//            isMoving = false;
//	    hasCollided = true;
//	    OnImpcat();
//            StartCoroutine(AttemptCapture());
        }
    }
    
    void OnCOllisionEnter(Collision collision){
	    //Comprueba si la colision sucede
	    if(collision.gameObject == targetMonster){
		    hasCollided = true;
		    OnImpact();
	    }
    }

    void OnImpact(){
	// Detén el momvimiento y ajusta la pocisión exacta
    	isMoving = false;
	//Mueve al Mounstro a la CapsuleBall
	StartCoroutine(MoveMonsterToBall());
    }

    IEnumerator MoveMonsterToBall(){
	//Duración de la animación
	float duration = 0.5f;
	Vector3 startPos = targetMonster.transform.position;
	Vector3 endPos = transform.position; //Posición de la capsuleball

	float elapsed = 0f;
	while ( elapsed < duration){
	    elapsed += Time.deltaTime;
	    float t = elapsed / duration;
	    targetPokemon.transform.position = Vector3.Lerp(startPos, endPos, t);
	    yield return null;
	}

	// Comienza la animación de entrada a la capsuleball
	StartCOroutine(AnimatieMosnterIntoBall();
    }

    IEnumerator AnimateMonsterIntoBall()
    {
    	//Reduce el tamaño del Mounstro gradualmente
    	float duration = 0.5f; // Duracion de la animación
    	Vector3 startScale = targetMonster.transform.localScale;
    	Vector endScale = Vector3.zero; //Se reduce hasta desaparecer

    	float elapsed = 0f;
    	while (elapsed < duration)
    	{
    	    elapsed += Time.deltaTime;
	    float t = elapsed / duration;
	    targetMonster.transform.localScale = Vector3.Lerp(startScale, endScale, t);
    	}

    	targetMonster.transform.localScale = endScale; //Asegura que esté comletamente reducido
    	monsterRenderer.enabled = false; // Oculta el Mounstro

    	// Hace qe la CapsuleBall caiga al suelo
    	StartCoroutine(DropBallToGround());
    }

    IEnumerator DropBallToGround()
    {
	// Simula la caída al suelo
	float furation = 0.3f;
	Vector3 startPos = transform.position;
	Vector3 endPos = startPos;
	endPos.y = 0; //Asume que el suelo está en y = 0

	float elapsed = 0f;
	while (elapsed < duration)
	{
	    elapsed += Time.DeltaTime;
	    float t = elapsed / duration;
	    transform.position = Vector.Lerp(startPos, endPos, t);
	    yield return null;
	}

	// COmienza las sacudidas para decidir la captura
	StartCoroutine(AttemptCapture());
    }

    IEnumerator AttemptCapture()
    {
        // Cálculo del valor base "a"
        float a = ((3.0f * maxHP - 2.0f * currentHP) / (3.0f * maxHP)) * captureRate * ballBonus * statusBonus;
        a = Mathf.Clamp(a, 0, 255); // Limitar el rango de "a" entre 0 y 255

        // Probabilidad base
        float b = a / 255.0f;

        // Simular cuatro sacudidas con animación
        for (int i = 0; i < 4; i++)
        {
            yield return StartCoroutine(ShakeBall()); // Realiza la sacudida
            if (Random.value >= b)
            {
                OnFailCapture();
                yield break;
            }
        }

        // Si todas las sacudidas son exitosas
        OnSuccessCapture();
    }

    IEnumerator ShakeBall()
    {
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        // Realiza una pequeña rotación y vuelve al estado original
        for (int i = 0; i < 2; i++) // Dos movimientos por sacudida
        {
            transform.Rotate(Vector3.forward, 15f);
            yield return new WaitForSeconds(0.1f);
            transform.Rotate(Vector3.forward, -30f);
            yield return new WaitForSeconds(0.1f);
            transform.Rotate(Vector3.forward, 15f);
        }

        // Regresa a la posición original para evitar desajustes
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    void OnSuccessCapture()
    {
        Debug.Log("¡Monster capturado!");
        Instantiate(captureEffect, targetMonster.transform.position, Quaternion.identity); // Efecto de captura
        Destroy(targetMonster); // Capturar el Monster (eliminar el GameObject)
        Destroy(gameObject);    // Destruir la Capsule
    }

    void OnFailCapture()
    {
        Debug.Log("El Monster escapó.");
        Instantiate(failEffect, transform.position, Quaternion.identity); // Efecto de falla
        Destroy(gameObject); // Destruir la Capsule
    }
}
