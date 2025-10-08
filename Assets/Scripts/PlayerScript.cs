using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float tilt = 5f;
    [SerializeField] private float jumpStrength = 3f;

    private Vector3 playerDirection;
    private Vector3 playerRotation;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl();
    }

    private void PlayerControl()
    {
        bool isJump = false;

        // Nếu đang nhấn vào UI (Pause, Resume, ...), bỏ qua input
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        // Nhảy bằng phím Space (PC test)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }

        // Nhảy bằng click chuột hoặc chạm màn hình (Mobile + PC)
        if (Input.GetMouseButtonDown(0))
        {
            isJump = true;
        }

        if (isJump)
        {
            playerDirection = Vector3.up * jumpStrength;
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxJump);
            }
        }

        ApplyGravity();
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0;
        transform.position = position;
        playerDirection = Vector3.zero;

    }

    private void ApplyGravity()
    {
        playerDirection.y += gravity * Time.deltaTime;
        transform.position += playerDirection * Time.deltaTime;

        playerRotation += transform.eulerAngles;
        playerRotation.z = playerDirection.y * tilt;
        transform.eulerAngles = playerRotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Scoring"))
        {
            gameManager.Scoring();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxPoint);
        }
        else if (other.CompareTag("Obstacles"))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxHit);
            }
            gameManager.GameOver();
        }
    }


}
