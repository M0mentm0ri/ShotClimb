using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Reload(2);  // ƒvƒŒƒCƒ„[‚Ì’e–ò‚ğ•â[
            Destroy(gameObject);  // E‚Á‚½‚çÁ–Å
        }
    }
}
