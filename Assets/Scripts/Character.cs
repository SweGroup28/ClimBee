using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public Pickaxe pickaxe;
    [SerializeField] public CollectableBase collectable;
    [HideInInspector] public bool onAir;
    [HideInInspector] public bool shieldActive;
    [HideInInspector] public bool controllable;
    private Rigidbody _rigidbody;
    private int _direct;
    
    void Start()
    {
        onAir = false;
        _rigidbody = GetComponent<Rigidbody>();
        controllable = true;
        _direct = gameObject.name == "Character" ? 1 : -1;
    }

    void Update()
    {
        CarryPickaxe();
    }

    private void CarryPickaxe()
    {
        pickaxe.transform.position = transform.position + _direct * new Vector3(0.2f, 0, -0.3f);
    }

    public void Jump(Vector3 force)
    {
        pickaxe.transform.eulerAngles = new Vector3(0, 0, 0);
        _rigidbody.AddForce(force);
        onAir = true;
    }

    public void Fall(float fallingTime)
    {
        controllable = false;
        transform.Rotate(Vector3.forward, 30f);
        Invoke(nameof(TakeControl), fallingTime);
    }

    private void TakeControl()
    {
        controllable = true;
        transform.rotation = Quaternion.identity;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Step"))
        {
            onAir = false;
        }
    }
}
