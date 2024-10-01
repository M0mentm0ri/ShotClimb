using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public int health = 3;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);  // オブジェクトの破壊
        }
    }
}
