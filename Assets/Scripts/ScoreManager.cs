using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text lifeText;

    public int score = 0;
    public int life = 3;

    void Start()
    {
        scoreText.text = score.ToString();
        lifeText.text = life.ToString();
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();

        Debug.Log("Score: " + score);
    }

    public int GetScore()
    {
        return score;
    }

    public void LoseLife()
    {
        life--;

        if (life < 0)
            life = 0;

        lifeText.text = life.ToString();

        Debug.Log("Life: " + life);
    }

    public void UpdateLife(int newLife)
    {
        life = newLife;
        lifeText.text = life.ToString();

        Debug.Log("Life: " + life);
    }
}