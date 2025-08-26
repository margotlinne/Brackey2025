using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;

    void Start()
    {
        GameManager.Instance.poolManager.InitiatePool(bulletPrefab, 50, "Bullet");
    }


    public GameObject GetBulletFromPool()
    {
        GameObject bullet = GameManager.Instance.poolManager.TakeFromPool("Bullet");
        bullet.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);

        return bullet;
    }
}
