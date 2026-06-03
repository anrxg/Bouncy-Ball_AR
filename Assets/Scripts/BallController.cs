using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections.Generic;
using ExpObj;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawnPoint;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ExplosiveObject explosiveObject;
    [SerializeField] private float forceMultiplier = 15f;

    private Rigidbody rb;
    private List<ARRaycastHit> hits = new();
    private bool objSpawned = false;
    private bool hasLanded = false;
    private bool hitTarget = false;
    private bool throwFinished = false;


    private void Start()
    {
        gameObject.GetComponents<TrailRenderer>()[0].enabled = false;
        rb = ball.GetComponent<Rigidbody>();
        rb.useGravity = false;
        explosiveObject = ball.GetComponent<ExplosiveObject>();
        scoreManager = FindFirstObjectByType<ScoreManager>();
        hitTarget = false;
        throwFinished = false;
    }

    private void Update()
    {
            HandleTouch();
        HandleMouse();
        if (!objSpawned)
            SpawnObject();
    }

    public void SpawnObject()
    {
        if (objSpawned)
            return;

        // Mouse (Editor)
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Instantiate(gameManager.spawnPrefab,
                            hit.point,
                            Quaternion.identity);

                objSpawned = true;
                hasLanded = false;
                Debug.Log("Object Spawned");
            }
        }

        // Touch (Mobile AR)
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPos =
                Touchscreen.current.primaryTouch.position.ReadValue();

            if (raycastManager.Raycast(
                touchPos,
                hits,
                TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                Instantiate(gameManager.spawnPrefab,
                            hitPose.position,
                            hitPose.rotation);

                objSpawned = true;
                hasLanded = false;
                Debug.Log("Object Spawned");
            }
        }
    }

    void HandleTouch()
    {
        if (Touchscreen.current == null)
            return;

        TouchControl touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame && objSpawned)
        {
            Vector2 touchPos = touch.position.ReadValue();

            if (raycastManager.Raycast(
                touchPos,
                hits,
                TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                Vector3 direction =
                    (hitPose.position - ball.transform.position).normalized;

                rb.linearVelocity = Vector3.zero;
                hitTarget = false;
                gameObject.GetComponents<TrailRenderer>()[0].enabled = true;
                rb.AddForce(direction * forceMultiplier, ForceMode.Impulse);
                rb.useGravity = true;
                Debug.Log("Touch hit: " + hitPose.position);
            }
        }
    }

    void HandleMouse()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame && objSpawned)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 direction =
                    (hit.point - ball.transform.position).normalized;

                rb.linearVelocity = Vector3.zero;
                hitTarget = false;
                gameObject.GetComponents<TrailRenderer>()[0].enabled = true;
                rb.AddForce(direction * forceMultiplier, ForceMode.Impulse);
                rb.useGravity = true;
                Debug.Log("Mouse hit: " + hit.point);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {  
        if (throwFinished)
            return;
        gameObject.GetComponents<TrailRenderer>()[0].enabled = false;
        Debug.Log("Collided with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("obj"))
        {
            throwFinished = true;
            hitTarget = true;

            scoreManager.AddScore(1);

            ExplosiveObject explosive =
                collision.gameObject.GetComponent<ExplosiveObject>();

            if (explosive != null)
                explosive.Explode();

            Invoke(nameof(RespawnBall), 0.2f);
        }
        else
        {
            throwFinished = true;

            scoreManager.LoseLife();

            Invoke(nameof(RespawnBall), 0.2f);
        }
    }
    public void RespawnBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        ball.transform.position = ballSpawnPoint.position;
        ball.transform.rotation = Quaternion.identity;

        rb.useGravity = false;

        hitTarget = false;
        throwFinished = false;
        objSpawned = false;
    }
}
