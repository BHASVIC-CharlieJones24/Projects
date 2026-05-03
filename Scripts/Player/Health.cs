using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public GameObject healthBar;
    private bool canRegen = false;
    private bool isRegenerating = false;
    private int playerIndex;

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        //Get the playerIndex value stored int the PlayerInput component of the player
        playerIndex = GetComponent<PlayerInput>().playerIndex;

        //Assign healthbars
        if (playerIndex == 0)
        {
            healthBar = GameObject.Find("Player1Health");
        }
        else if (playerIndex == 1)
        {
            healthBar = GameObject.Find("Player2Health");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Update healthbar UI
        healthBar.GetComponent<Slider>().value = currentHealth;

        //Player dies
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Check if the object collided with is a bullet
        if (collision.collider.tag == "Bullet")
        {
            canRegen = false;
            //Reduce health
            currentHealth -= 15;
            
            if(isRegenerating == false)
            {
                StartCoroutine(Regeneration());
            }

        }
    }

    public void damage(int damage)
    {
        canRegen = false;
        currentHealth -= damage;
        Debug.Log("Pklayer Health: " + currentHealth);
        if(isRegenerating == false)
        {
            StartCoroutine(Regeneration());
        }
    }

    IEnumerator Regeneration()
    {
        isRegenerating = true;
         //Wait 5 seconds after taking damage
        yield return new WaitForSeconds(5);

        //Start  regenerating
        while (currentHealth < maxHealth)
        {
            currentHealth += 2;
            yield return new WaitForSeconds(0.5f);
        }
        //Prevent the health going over max
        currentHealth = maxHealth;
    
        isRegenerating = false;
    }
}
