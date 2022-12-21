using UnityEngine;

public class Fall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var character = collision.gameObject.GetComponent<Character>();
            if (!character.shieldActive)
            {
                Debug.Log("Touched");
                collision.gameObject.GetComponent<Character>().Fall(2f);
                Destroy(gameObject);
            }
        }
    }
}
