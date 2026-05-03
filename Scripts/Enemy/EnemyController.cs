using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //Vision Cone
    public float rayLength = 10f;
    public string playerTag = "Player";
    public int rayCount = 33;
    public float visionAngle = 65f;

    //Player Tracking
    private NavMeshAgent agent;
    private bool playerLastFrame = false;
    private bool playerThisFrame = false;
    private Vector3 playerPosition;
    public float chaseDelay = 2f;
    private float delayTimer = 0f;
    private bool CountingChaseDelay = false;
    public GameObject muzzleFlash;

    //States
    private string currentState = "Patrol"; // Patrol, Shoot, Chase, Search
    private bool soundHeard;
    public Vector3 soundPosition;

     //Patrol
    public GameObject[] patrolPath;
    private int currentPatrolPoint = 0;

    //Searching
    public float searchTime = 5f;
    public float searchTimer;
    private float moveTimer;
    public float searchRadius = 5f;

    //Combat
    public GameObject projectile;
    public Transform startPosition;
    public float bulletSpeed;
    public float fireRate;
    public float inaccuracy;
    private Vector3 accuracyOffset;
    private Vector3 hitPos;
    private Vector3 lookPos;

    private bool canFire = true;

    public Animator animator;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void OnCollisionEnter(Collision collision)
    {
        //If the object collided with is a bullet
        if (collision.collider.tag == "Bullet" && currentState != "Combat")
        {
            hitPos = collision.collider.transform.position;
            hitPos.y = transform.position.y;
            //Make the enemy look at where the bullet came hit them
            transform.LookAt(hitPos);
        }
    }
    void Update()
    {
        playerThisFrame = false;
         // Vision cone detection
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate angle for current ray
            float angle = -visionAngle / 2f + (visionAngle / (rayCount - 1)) * i;
            // Rotate the forward vector by the calculated angle
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, rayLength))
            {
                //Hit Player
                if (hit.collider.CompareTag(playerTag))
                {
                    Debug.DrawRay(transform.position, dir * hit.distance, Color.green);
                    playerPosition = hit.collider.transform.position;
                    playerThisFrame = true;
                }
                else
                {
                    Debug.DrawRay(transform.position, dir * hit.distance, Color.white);
                }
            }
            // Hit nothing
            else
            {
                Debug.DrawRay(transform.position, dir * rayLength, Color.white);
            }
        }

        //Player seen
        if (playerThisFrame)
        {
            agent.ResetPath();
            currentState = "Combat";
            delayTimer = chaseDelay;
            CountingChaseDelay = false;
            soundHeard = false;
        }

        //Player lost
        else if (playerLastFrame && !playerThisFrame && !CountingChaseDelay)
        {
            CountingChaseDelay = true;
            delayTimer = chaseDelay;
            currentState = "Idle";
        }

        //Chase count down
        if (CountingChaseDelay)
        {
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0f)
            {
                currentState = "Chase";
                CountingChaseDelay = false;
            }
        }
        
        //Enemy States
        if (currentState == "Combat")
        {
            Combat();
        }
        else if (soundHeard == true)
        {
            if (currentState != "Search")
            {
                agent.ResetPath();
                currentState = "Search";
                searchTimer = 0f;
            }

            Search(soundPosition);
        }
        else
        {
            if (currentState == "Patrol")
            {
                Patrol();
            }
            else if (currentState == "Chase")
            {
                Chase();
            }
            else if (currentState == "Search")
            {
                Search(playerPosition);
            }
            else if (currentState == "Idle")
            {
                //Update animation states
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("Shoot", false);
            }
        }

        playerLastFrame = playerThisFrame;
    }

    void Patrol()
    {
        //Update animation states
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("Shoot", false);
        //If enemy has reached patrol point
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            //Set the enemys speed to a waking speed
            agent.speed = 1;
            
            //Move enemy to patrol point
            agent.SetDestination(patrolPath[currentPatrolPoint].transform.position);
            if (currentPatrolPoint > patrolPath.Length - 2)
            {
                currentPatrolPoint = 0;
            }
            else
            {
                currentPatrolPoint += 1;
            }
        }
    }

    void Combat()
    {
        //Update animation states
        animator.SetBool("Shoot", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        //Make enemy look at the player
        lookPos = playerPosition;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
        
        if (canFire == true)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        canFire = false;
        //Shoot Bullet
        GameObject bullet = Instantiate(projectile, startPosition.position, transform.rotation * Quaternion.Euler(90, 0, 0));

        //Inaccuracy
        accuracyOffset.x = Random.Range(-inaccuracy, inaccuracy);
        accuracyOffset.z = Random.Range(-inaccuracy, inaccuracy);

        bullet.GetComponent<Rigidbody>().AddForce((transform.forward+accuracyOffset) * bulletSpeed, ForceMode.Impulse);
        //Play shoot animation
        animator.SetBool("Shoot", true);
        //Spawn the muzzl flash prefab
        GameObject flash = Instantiate(muzzleFlash,startPosition);
        //Destroy the muzzle flash and bullet
        Destroy(flash,1);
        Destroy(bullet, 3);
        yield return new WaitForSeconds(fireRate);
        canFire = true;

    }
    void Chase()
    {
        //Update animation states
        animator.SetBool("isRunning", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("Shoot", false);

        //Make the enemy move at a running speed
        agent.speed = 2.5f;

        //Move to the players position
        agent.SetDestination(playerPosition);
        
        //Once the enemy reaches the players last position start searching
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentState = "Search";
            moveTimer = 0f;
        }
    }

    void Search(Vector3 searchPosition)
    {
        //Update animation states
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("Shoot", false);

        //Make the enemy move at a running speed
        agent.speed = 2.5f;

        //Increase search timer
        searchTimer += Time.deltaTime;
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            //Inrease the movement timer
            moveTimer += Time.deltaTime;
            
            if (moveTimer > Random.Range(0.5f, 2f))
            {
                //Move to random location around the search position
                Vector3 randomOffset = Random.insideUnitSphere * searchRadius;
                randomOffset.y = 0;
                agent.SetDestination(searchPosition + randomOffset);
                moveTimer = 0f;
            }

        }
        //Finished searching
        if (searchTimer > searchTime)
        {
            currentState = "Patrol";
            searchTimer = 0f;
            soundHeard = false;
        }
    }

    public void SetSoundHeard(bool heard)
    {
        soundHeard = heard;
    }
    public void SetSoundPosition(Vector3 pos)
    {
        soundPosition = pos;
    }
}