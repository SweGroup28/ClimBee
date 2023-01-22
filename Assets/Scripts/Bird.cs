using System;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private IBirdCollisionHandler m_CollisionHandler;
    private Bird[] _birds;

    public IBirdCollisionHandler collisionHandler
    {
        get => m_CollisionHandler;
        set => m_CollisionHandler = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var character = other.gameObject.GetComponent<Character>();
            if (!character.shieldActive)
            {
                Debug.Log("Touched");
                other.gameObject.GetComponent<Character>().Fall(2f);
            }
            m_CollisionHandler.HandleBirdCollision();
            _birds = FindObjectsOfType<Bird>();
            foreach (var bird in _birds)
            {
                Destroy(bird.gameObject);
            }
        }
    }
}

public interface IBirdCollisionHandler
{
    void HandleBirdCollision();
}
