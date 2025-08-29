using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
