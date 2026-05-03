using UnityEngine;

public class Enemyhealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Enemy diesS
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        } 
    }
    public void damage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy Health: " + currentHealth);
    }
    void OnCollisionEnter(Collision collision)
    {
        //If the object collided with is a bullet
        if (collision.collider.tag == "Bullet")
        {
            //Reduce health
            currentHealth -= 15;
        }
    }
}