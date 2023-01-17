using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private float minSpeed = 10f;//pour la jouabilité
    private float maxSpeed = 14f;//pour la jouabilité
    private float maxTorque = 10f;
    private float xRange = 4f;
    private float ySpawnPos = -1f; //pour la jouabilité
    private float boundLimit = -1f; //pour la jouabilité
    private GameManager gameManager;
    public int pointValue;
    public ParticleSystem explosionParticle;
    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(),ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(),ForceMode.Impulse);
        transform.position = RandomSpwnPos();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        OutOfBound();
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed,maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpwnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    private void OnMouseDown()
    {
        if (gameManager.isGameActive)
        {
            Destroy(gameObject);
            gameManager.UpdateScore(pointValue);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        }
    }


    private void OnCollisionEnter(Collision col)
    {
        //Pour que les bombes detruisent les autres GO mais pas elles memes !
        if (gameObject.CompareTag("Bad") && !col.gameObject.CompareTag("Bad"))
        {
                Destroy(col.gameObject);
                gameManager.UpdateScore(-10);
        }
    }
     
    // Pour definir la zone de GameOver
    void OutOfBound()
    {
        if (!gameObject.CompareTag("Bad") && transform.position.y < boundLimit)
        {
            gameManager.GameOver();
        }
    }
}
