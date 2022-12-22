using UnityEngine;

public class Bird : MonoBehaviour
{
    private IBirdCollisionHandler m_CollisionHandler;

    public IBirdCollisionHandler collisionHandler
    {
        get => m_CollisionHandler;
        set => m_CollisionHandler = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var character = collision.gameObject.GetComponent<Character>();
            if (!character.shieldActive)
            {
                Debug.Log("Touched");
                collision.gameObject.GetComponent<Character>().Fall(2f);
                m_CollisionHandler.HandleBirdCollision();
                Destroy(gameObject);
            }
        }
    }
}

public interface IBirdCollisionHandler
{
    void HandleBirdCollision();
}
