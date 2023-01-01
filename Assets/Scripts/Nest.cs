using System;
using TMPro;
using UnityEngine;

public class Nest : CollectableBase, IBirdCollisionHandler
{
    [SerializeField] public GameObject character;
    [SerializeField] public GameObject bot;
    [SerializeField] public GameObject redBird;
    [SerializeField] public GameObject blueBird;
    private Vector3 _pos;
    private Bird[] _birds;
    private Vector3[] _startPos;
    private Vector3 _endPos;
    private bool _fly;
    private GameObject _target;

    public void HandleBirdCollision()
    {
        _fly = false;
    }

    private void Start()
    {
        _birds = new Bird[4];
        _startPos = new Vector3[4];
        _fly = false;
        _target = character;
    }

    private void Update()
    {
        if (_fly)
        {
            FlyTo();
        }
    }

    private void ReleaseBirds(string who)
    {
        var choose = who == "Character";
        _target = choose ? bot : character;
        var triggeredBy = choose ? character : bot;
        _pos = triggeredBy.transform.position + Vector3.back + Vector3.right;
        _endPos = _target.transform.position;
        for (var i = 0; i < 4; i++)
        {
            _startPos[i] = _pos + i * 0.2f * (choose ? Vector3.right : Vector3.left) + i * 0.5f * Vector3.up;
            _birds[i] = Instantiate(choose ? redBird : blueBird, _startPos[i], Quaternion.identity).GetComponent<Bird>();
            _birds[i].collisionHandler = this;
        }
        _fly = true;
    }
    
    private void FlyTo()
    {
        _endPos = _target.transform.position;
        for (var i = 0; i < 4; i++)
        {
            _birds[i].transform.position = Vector3.MoveTowards(_startPos[i], _endPos, 0.02f);
            _startPos[i] = _birds[i].transform.position;
            Destroy(_birds[i].gameObject, 5f);
        }
    }
    
    public override void Use(GameObject usedBy)
    {
        ReleaseBirds(usedBy.name);
    }
}