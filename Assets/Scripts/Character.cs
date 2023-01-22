using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Character : MonoBehaviour
{
    [SerializeField] public Pickaxe pickaxe;
    [SerializeField] public CollectableBase collectable;
    [SerializeField] public GameManagement gameManager;
    [HideInInspector] public bool onAir;
    [HideInInspector] public bool shieldActive;
    [HideInInspector] public bool controllable;
    private Rigidbody _rigidbody;
    private bool _finish;
    private int _direct;
    
    void Start()
    {
        onAir = false;
        _finish = false;
        _rigidbody = GetComponent<Rigidbody>();
        controllable = true;
        _direct = gameObject.name == "Character" ? 1 : -1;
    }

    void Update()
    {
        CarryPickaxe();
        CheckWin();
        if (_finish)
        {
            Finish();
        }
    }

    private void CheckWin()
    {
        if (transform.position.y > gameManager.topLevel.position.y && !gameManager.gameHasEnded)
        {
            controllable = false;
            if (_direct == 1)
            {
                gameManager.CompleteLevel();
            }
            else
            {
                gameManager.FailLevel();
            }

            _finish = true;
        }
    }

    private void Finish()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f,gameManager.topLevel.position.y + 0.2f,0f), 0.01f);
    }
    
    private void CarryPickaxe()
    {
        pickaxe.transform.position = transform.position + _direct * new Vector3(0.08f, _direct*0.5f, -0.2f);
    }

    public void Jump(Vector3 force)
    {
        pickaxe.transform.eulerAngles = new Vector3(0, 0, 0);
        Debug.Log("Forced: " + force);
        _rigidbody.AddForce(Vector3.Min(force,3000*Vector3.up));
        Debug.Log("Jumped: " + Vector3.Min(force,3000*Vector3.up));
        onAir = true;
    }

    public void Fall(float fallingTime)
    {
        controllable = false;
        pickaxe.transform.Rotate(Vector3.back, _direct * 30f);
        Invoke(nameof(TakeControl), fallingTime);
    }

    private void TakeControl()
    {
        controllable = true;
        pickaxe.transform.rotation = Quaternion.identity;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Step"))
        {
            onAir = false;
        }
        else if (collision.collider.gameObject.CompareTag("Finito"))
        {
            _finish = false;
        }
    }
}
