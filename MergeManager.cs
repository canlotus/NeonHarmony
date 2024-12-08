using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public float pushRadius = 0.2f;
    public float pushForce = 2.0f;

    public void MergeShapes(Circle shape1, Circle shape2)
    {
        if (shape1.CircleNumber == shape2.CircleNumber && !shape1.IsMerging && !shape2.IsMerging)
        {
            int newNumber = shape1.CircleNumber * 2;

            shape1.IsMerging = true;
            shape2.IsMerging = true;

            GameObject prefabToSpawn;

            // GameMode'a g?re birle?tirme tipi
            int gameMode = PlayerPrefs.GetInt("GameMode", 0);
            if (gameMode == 2) // Kar???k mod
            {
                prefabToSpawn = Random.value > 0.5f ? Spawner.Instance.circlePrefab : Spawner.Instance.squarePrefab;
            }
            else
            {
                prefabToSpawn = Spawner.Instance.circlePrefab; // Sadece Circle
            }

            Circle newShape = Spawner.Instance.Spawn(newNumber, GetMergePosition(shape1, shape2), prefabToSpawn);

            newShape.SetScale(newNumber);
            newShape.SetColor(Spawner.Instance.GetColor(newNumber));
            newShape.CircleRigidbody.gravityScale = 1;
            newShape.CircleRigidbody.velocity = Vector2.zero;

            ApplyPushToNearbyCircles(newShape.transform.position, newShape);

            ScoreManager.Instance.AddScore(newNumber);

            Destroy(shape1.gameObject);
            Destroy(shape2.gameObject);
        }
    }

    private Vector3 GetMergePosition(Circle shape1, Circle shape2)
    {
        return (shape1.transform.position + shape2.transform.position) / 2;
    }

    private void ApplyPushToNearbyCircles(Vector3 mergePosition, Circle mergedShape)
    {
        Collider2D[] nearbyShapes = Physics2D.OverlapCircleAll(mergePosition, pushRadius);

        foreach (Collider2D collider in nearbyShapes)
        {
            Circle nearbyShape = collider.GetComponent<Circle>();

            if (nearbyShape != null && nearbyShape.CircleRigidbody != null && !nearbyShape.IsMainCircle)
            {
                Vector2 pushDirection = nearbyShape.transform.position - mergePosition;
                nearbyShape.CircleRigidbody.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}