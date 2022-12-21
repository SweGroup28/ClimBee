using System;
using UnityEngine;

public class ThrowSnow : CollectableBase
{
    [SerializeField] public GameObject snowball;
    [SerializeField] public int rotationSpeed;
    private bool _rotate;
    private Vector3 _rot;

    private void Update()
    {
        if (_rotate)
        {
            snowball.transform.Rotate(_rot);
        }
    }

    private void ThrowSnowBall(GameObject character)
    {
        var pos = character.transform.position + Vector3.right;
        snowball = Instantiate(snowball, pos, character.name == "Character" ? Quaternion.identity : new Quaternion(0,0.38f,0,0.92f));
        _rot = rotationSpeed * 100 * Time.deltaTime * (character.name == "Character" ? Vector3.down : Vector3.up);
        _rotate = true;
        Invoke(nameof(Stop),0.5f);
    }

    private void Stop()
    {
        _rotate = false;
        Destroy(snowball);
    }
    
    public override void Use(GameObject usedBy)
    {
        ThrowSnowBall(usedBy);
    }
}