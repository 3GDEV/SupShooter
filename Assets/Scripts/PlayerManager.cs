using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region EditorSettings
    [Space]

    [Header("Movements")]
    public float rotationLimit = 40f;
    public float slerpSpeed = 10f;
    public int movementSpeed = 35;

    [Space]

    [Header("Shooting")]
    public float distanceMissile = 20f;
    public float distanceMissileBonus = 5f;
    public float fireRate = 0.25f;
    public LayerMask layerMaskNotPlayer;

    [Space]

    [Header("Life gestion")]
    public int remainingLives = 3;
    public int maxLives = 3;
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int missilesDamages = 15;
    public int minesDamages = 10;
    public bool isDead = false;

    [Space]

    [Header("Bonus")]
    public int invincibilityDuration = 10;
    public float missileBonusDuration = 5;
    public float bonusFireRate = 0.10f;
    public float speedBonusDuration = 5;
    public float bonusSpeed = 70;
    public GameObject invincibilityCapsule;

    [Space]

    [Header("Scoring")]
    public int score = 0;
    public int mineScore = 10;
    public int ennemyScore = 100;
    public int bonusScore = 50;

    [Space]

    [Header("Prefabs")]
    public GameObject projectilePrefab;
    public CinemachineDollyCart dolly;
    public Animation damageAnimation;

    [Space]

    [Header("UI")]
    public Slider healthSlider;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    public PauseMenuManager pauseMenuManager;

    [Space]

    [Header("Cursor")]
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    [Space]

    [Header("Audio")]
    public AudioSource shootAudioSource;
    public AudioSource hitAudioSource;
    public AudioSource deathAudioSource;
    public AudioSource cameraMusic;


    [Space]

    [Header("Particle effect")]
    public GameObject deathExplosion;

    #endregion
    private bool isInvicible = false;
    private bool missileBonus = false;
    private bool allowFire = false;

    private float nextFire;

    private Transform parent;
    private Transform localTransform;



    // Start is called before the first frame update
    void Start()
    {
        hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        transform.localPosition = new Vector3(-15, -7.5f, 15);
        parent = GetComponentInParent<Transform>();
        localTransform = GetComponent<Transform>();
        allowFire = true;
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        SetHealth(maxHealth);
        SetLives(remainingLives);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.localPosition += new Vector3(horizontalInput, verticalInput, 0) * movementSpeed * Time.deltaTime;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 15);
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.1f, 0.9f);
        pos.y = Mathf.Clamp(pos.y, 0.1f, 0.9f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100000, layerMaskNotPlayer))
        {
            Debug.Log(transform.localRotation);
            if(transform.localRotation.x >= -0.4f && transform.localRotation.x <= 0.4f
               && transform.localRotation.y >= -0.4f && transform.localRotation.y <= 0.4f
               && transform.localRotation.z >= -0.4f && transform.localRotation.z <= 0.4f)
            transform.LookAt(hit.point);
        }*/
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-rotationLimit * verticalInput, rotationLimit * horizontalInput, -rotationLimit * horizontalInput), Time.deltaTime * slerpSpeed);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time > nextFire && allowFire)
        {
            var positionMissile = transform.position + transform.forward * distanceMissile;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100000, layerMaskNotPlayer))
            {
                shootAudioSource.Play();
                var missile1 = Instantiate(projectilePrefab, positionMissile, Quaternion.identity);
                missile1.transform.LookAt(hit.point);
                if (missileBonus)
                {
                    var missile2 = Instantiate(projectilePrefab, positionMissile + transform.right * distanceMissileBonus, Quaternion.identity);
                    var missile3 = Instantiate(projectilePrefab, positionMissile - transform.right * distanceMissileBonus, Quaternion.identity);
                    missile2.transform.LookAt(hit.point);
                    missile3.transform.LookAt(hit.point);
                }

                nextFire = Time.time + fireRate;
            }
        }
    }

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        ManageCollisions(other.tag);
    }
    public void ManageCollisions(string tagCollider)
    {
        switch (tagCollider)
        {
            case "InvincibleStar":
                StartCoroutine(nameof(ToggleInvincibility));
                break;
            case "HealthHeart":
                IncreaseLives();
                break;
            case "ShootCubie":
                StartCoroutine(nameof(ToggleMissileBonus));
                break;
            case "EnnemyMissile":
                TakeDamageFromMissile();
                break;
            case "Mine":
                TakeDamageFromMine();
                break;
            case "Terrain":
                TakeDamageFromTerrain();
                break;
            default:
                break;
        }
    }

    private void IncreaseLives()
    {
        if (remainingLives < 3)
        {
            SetLives(++remainingLives);
        }
    }

    private void TakeDamageFromMissile()
    {
        TakeDamage(15);
    }

    private void TakeDamageFromMine()
    {
        TakeDamage(5);
    }

    private void TakeDamageFromTerrain()
    {
        Debug.Log("Coucou");
        TakeDamage(20);
        transform.localPosition = new Vector3(-15, -7.5f, 15);
        transform.localRotation = Quaternion.identity;
    }

    private void TakeDamage(int damages)
    {
        if (!isInvicible)
        {
            if (currentHealth - damages <= 0)
            {
                if (remainingLives <= 0)
                {
                    SetHealth(0);
                    Death();
                }
                else
                {
                    hitAudioSource.Play();
                    damageAnimation.Play();
                    SetLives(--remainingLives);
                    SetHealth(maxHealth);
                }
            }
            else
            {
                hitAudioSource.Play();
                damageAnimation.Play();
                SetHealth(currentHealth - damages);
            }
        }
    }
    #endregion

    #region Methods
    public void Death()
    {
        isDead = true;
        Instantiate(deathExplosion, transform.position, transform.rotation);
        dolly.m_Speed = 0;
        cameraMusic.Stop();
        deathAudioSource.Play();
        pauseMenuManager.ToggleGameOver();
        gameObject.SetActive(false);
    }

    public void SetHealth(int health)
    {
        currentHealth = health;
        healthSlider.value = health;
    }

    public void SetLives(int lives)
    {
        remainingLives = lives;
        livesText.text = lives + "/" + maxLives;
    }

    public void AddScore(string tagScore)
    {
        switch (tagScore)
        {
            case "Mine":
                score += mineScore;
                break;
            case "Ennemy":
                score += ennemyScore;
                break;
            case "Bonus":
                score += bonusScore;
                break;
            default:
                score += 0;
                break;
        }
        scoreText.text = score.ToString();
        gameOverScoreText.text = score.ToString();
    }

    public void SetAllowFire(bool _allowFire)
    {
        allowFire = _allowFire;
    }
    #endregion
    #region Coroutines
    IEnumerator ToggleInvincibility()
    {
        isInvicible = true;
        invincibilityCapsule.SetActive(true);
        yield return new WaitForSeconds(invincibilityDuration);
        isInvicible = false;
        invincibilityCapsule.SetActive(false);
    }
    IEnumerator ToggleMissileBonus()
    {
        missileBonus = true;
        float oldFireRate = fireRate;
        fireRate = bonusFireRate;
        yield return new WaitForSeconds(missileBonusDuration);
        missileBonus = false;
        fireRate = oldFireRate;
    }

    IEnumerator ToggleSpeedBonus()
    {
        float oldSpeed = dolly.m_Speed;
        dolly.m_Speed = bonusSpeed;
        yield return new WaitForSeconds(speedBonusDuration);
        dolly.m_Speed = oldSpeed;
    }
    #endregion
}

