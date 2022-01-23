using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollisionManager : MonoBehaviour
{
    public float lifeTime = 7;
    public GameObject explosionParticle;

    private PlayerManager player;
    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        StartCoroutine(nameof(LifecCycle));
    }
    public void OnTriggerEnter(Collider other)
    {
        //Missile - EnnemyMissile
        if (gameObject.tag == "Missile")
        {
            if (!other.CompareTag("Box") && !other.CompareTag("InvincibilityCapsule"))
            {
                Instantiate(explosionParticle, transform.position, transform.rotation);
            }
            if (other.CompareTag("EnnemyMissile"))
            {
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Mine"))
            {
                player.AddScore(other.tag);
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Ennemy"))
            {
                player.AddScore(other.tag);
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("InvincibleStar") || other.CompareTag("HealthHeart") || other.CompareTag("SpeedDiamond") || other.CompareTag("ShootCubie"))
            {
                player.ManageCollisions(other.tag);
                player.AddScore("Bonus");
            }
            if (!other.CompareTag("InvincibilityCapsule"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                player.ManageCollisions(gameObject.tag);
            }
            Destroy(gameObject);
        }
    }

    IEnumerator LifecCycle()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
