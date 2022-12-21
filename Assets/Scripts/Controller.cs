using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] public Character character;
    [SerializeField] public Pickaxe pickaxe;
    [SerializeField] public int forceFactor;
    private float _y;
    private float _pullRate;
    private Vector3 _force;
    
    private void Start()
    {
        _y = 0;
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
            if (Input.GetMouseButtonDown(0))
            {
                pickaxe.rotate = true;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _y = Input.mousePosition.y;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _pullRate = Input.mousePosition.y - _y;
                _force =  forceFactor * _pullRate * Vector3.down;
                if(_pullRate < 0) character.Jump(_force);
            }
        }
    }
}
