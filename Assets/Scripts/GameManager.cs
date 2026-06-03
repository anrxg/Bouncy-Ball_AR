using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject spawnPrefab;
    private ScoreManager scoreManager;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject spawnBallButton;
    [SerializeField] private GameObject respsawnBallButton;
    private BallController ballController;

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        loseText.gameObject.SetActive(false);
        ballController = FindFirstObjectByType<BallController>();
    }
    void Update()
    {
        if(scoreManager.life < 1)
        {
            Time.timeScale = 0f;
            loseText.gameObject.SetActive(true);
            spawnBallButton.gameObject.SetActive(false);
            respsawnBallButton.gameObject.SetActive(false);
        }
         else
        {
            loseText.gameObject.SetActive(false);
        }
    }
    public void SpawnObject(Vector3 position)
    {
       if (Mouse.current == null)
            return;

        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 spawnPos = hit.point;
                Instantiate(spawnPrefab, spawnPos, Quaternion.identity);
                Debug.Log("Spawned object at: " + spawnPos);
            }
        }
    }


}
