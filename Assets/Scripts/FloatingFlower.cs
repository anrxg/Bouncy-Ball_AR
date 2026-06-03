using UnityEngine;

public class FloatingFlower : MonoBehaviour
{
    [SerializeField] private float moveRadius = 100f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotationSpeed = 30f;

    private RectTransform rectTransform;
    private Vector2 startPos;
    private Vector2 targetPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        startPos = rectTransform.anchoredPosition;
        PickNewTarget();
    }

    void Update()
    {
        // Move
        rectTransform.anchoredPosition =
            Vector2.Lerp(
                rectTransform.anchoredPosition,
                targetPos,
                moveSpeed * Time.deltaTime);

        // Rotate
        rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Pick another target when close
        if (Vector2.Distance(rectTransform.anchoredPosition, targetPos) < 5f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        targetPos = startPos +
                    Random.insideUnitCircle * moveRadius;
    }
}