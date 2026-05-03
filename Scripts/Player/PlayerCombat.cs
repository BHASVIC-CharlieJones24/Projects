using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private PlayerInput playerInput;

    //Shooting
    private InputAction shootAction;
    public GameObject projectile;
    public Transform startPosition;
    public float bulletSpeed;
    private bool canFire = true;
    public float fireRate;
    public GameObject muzzleFlash;


    //Sound
    public float runVolume;
    public float shootVolume;
    private SoundOutput soundOutput;

    public float throwStrength;

    //Ability 1
    private InputAction ability1Action;
    public GameObject ablility1Prefab;
    private int ability1Count = 0;
    public int maxAbility1;
    private bool ability1Ready = true;
    public int ability1Delay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        soundOutput = GetComponent<SoundOutput>();
        shootAction = playerInput.actions["Shoot"];
        ability1Action = playerInput.actions["Ability1"];
    }

    // Update is called once per frame
    void Update()
    {
        // Gun
        if (shootAction.IsPressed() && canFire == true)
        {
            StartCoroutine(Shoot());
        }
        // Grenade
        if (ability1Action.IsPressed() && ability1Count < maxAbility1 && ability1Ready == true)
        {
            //Spawn grenade
            GameObject ability1 = Instantiate(ablility1Prefab, startPosition.position, Quaternion.Euler(0, 0, 0));
            //Add force to the grenade
            ability1.GetComponent<Rigidbody>().AddForce(transform.forward * throwStrength, ForceMode.Impulse);
            ability1.GetComponent<Rigidbody>().AddForce(transform.up * throwStrength/2, ForceMode.Impulse);
            ability1Count += 1;
            StartCoroutine(Ability1Cooldown());
        }
    }

    IEnumerator Shoot()
    {
        canFire = false;
        //Spawn a bullte
        GameObject bullet = Instantiate(projectile, startPosition.position, transform.rotation * Quaternion.Euler(90, 0, 0));
        //Add force to the bullet
        bullet.GetComponent<Rigidbody>().AddForce(startPosition.forward * bulletSpeed, ForceMode.Impulse);
        soundOutput.OutputSound(shootVolume);
        //Spawn muzzle flash
        GameObject flash = Instantiate(muzzleFlash,startPosition);
        //Delete the muzzle flash and bullet
        Destroy(flash,1);
        Destroy(bullet, 3);
        Destroy(bullet, 3);
        //Delay before being able to fire again
        yield return new WaitForSeconds(fireRate);
        canFire = true;

    }
    IEnumerator Ability1Cooldown()
    {
        ability1Ready = false;
        //Add delay before being able to use the ability again
        yield return new WaitForSeconds(ability1Delay);
        ability1Ready = true;

    }
}
