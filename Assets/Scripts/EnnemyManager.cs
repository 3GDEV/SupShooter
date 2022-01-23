using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyManager : MonoBehaviour
{
    public GameObject ennemyMissilePrefab;
    public float secondsToWaitBeforeShoot = 3;
    public float stopShootingTime = 10;

    private float fireRate = 0.85f;
    private float nextFire;
    private bool allowFire = false;
    public float distanceMissile = 20f;

    private GameObject player;
    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(WaitForPlayerVisibility));
        StartCoroutine(nameof(TimeLimitToStopShoot));
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        bool playerIsDead = playerManager.isDead;
        if (!playerIsDead && allowFire && Time.time > nextFire)
        {
            var positionMissile = transform.position + transform.forward * distanceMissile;
            var missile = Instantiate(ennemyMissilePrefab, positionMissile, Quaternion.identity);
            missile.transform.LookAt(player.transform);
            nextFire = Time.time + fireRate;
        }
    }

    IEnumerator WaitForPlayerVisibility()
    {
        yield return new WaitForSeconds(secondsToWaitBeforeShoot);
        allowFire = true;
    }

    IEnumerator TimeLimitToStopShoot()
    {
        float time = secondsToWaitBeforeShoot + stopShootingTime;
        yield return new WaitForSeconds(time);
        allowFire = false;
    }
}
