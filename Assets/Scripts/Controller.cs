using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    [SerializeField] public Character character;
    [SerializeField] public Character bot;
    [SerializeField] public Pickaxe pickaxe;
    [SerializeField] public Pickaxe pickaxeBot;
    [SerializeField] public int forceFactor;
    private float _y;
    private float _pullRate;
    private Vector3 _force;
    private Vector3 _botForce;
    private float _rnd;
    private IEnumerator _coroutine;

    private void Start()
    {
        _y = 0;
        InvokeRepeating( nameof(WaitAndGiveInput),2.0f, 0.5f);
    }
    
    private void Update()
    {
        if (character.controllable)
        {
            GetInput();
        }
    }

    private void GetInput()
    {
        if (character.onAir)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                pickaxe.rotate = true;
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _y = Input.mousePosition.y;
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                _pullRate = Input.mousePosition.y - _y;
                _force =  forceFactor * _pullRate * Vector3.down;
                if(_pullRate < 0) character.Jump(_force);
            }
        }
    }

    private void WaitAndGiveInput()
    {
        _rnd = Random.Range(0, 100);
        if (bot.collectable != null)
        {
            if (_rnd < 20)
            {
                try
                {
                    if (bot.collectable.gameObject.name != "Hammer")
                    {
                        bot.collectable.Use(bot.gameObject);
                    }
                    DestroyImmediate(bot.collectable);
                }
                catch (Exception e)
                {
                    Debug.Log("No collectable object " + e);
                }
            }
        }
        if (bot.onAir)
        {
            if (_rnd < 80)
            {
                pickaxeBot.rotate = true;
            }
            else
            {
                pickaxeBot.rotate = false;
                pickaxeBot.transform.eulerAngles = new Vector3(0, 0, 15);
            }
        }
        else
        {
            if (_rnd < 50)
            {
                _botForce = -40 * forceFactor * _rnd * Vector3.down;
                bot.Jump(_botForce);
            }
        }
    }
}
