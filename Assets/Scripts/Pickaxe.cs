using System;
using UnityEngine;
using UnityEngine.UI;

public class Pickaxe : MonoBehaviour
{
    [SerializeField] public Character character;
	[SerializeField] public UseCollectable button;
	[SerializeField] public GameObject stepChar;
	[SerializeField] public GameObject stepAI;
	[SerializeField] public Material hammerMaterial;
    [SerializeField] public Material pickaxeMaterial; 
    [SerializeField] public bool rotate;
    [HideInInspector] public bool hammerMode;
    [SerializeField] public int rotateSpeed;
    private GameObject _step;
    private bool _cp;

    private void Start()
    {
        rotate = false;
        hammerMode = false;
        _cp = gameObject.name == "Pickaxe";
        _step = Instantiate(_cp ? stepChar : stepAI, Vector3.down, Quaternion.identity);
    }

    private void Update()
    {
        RotateAxe();
    }

    private void RotateAxe()
    {
        if (rotate)
        {
	        if (_cp)
	        {
		        transform.Rotate(rotateSpeed * 100 * Time.deltaTime * Vector3.back);
	        }
	        else
	        {
		        transform.Rotate(rotateSpeed * 50 * Time.deltaTime * Vector3.forward);
	        }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
	    rotate = false;
	    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
	    if (collision.gameObject.CompareTag("Holdable") && !hammerMode)
	    {
		    PutStep();
	    }
	    else
	    {
		    transform.eulerAngles = new Vector3(0, 0, (_cp ? -15 : 15));
		    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		    if (collision.gameObject.CompareTag("Concrete") && collision.gameObject.name != "Concrete")
		    {
			    if(_cp) button.GetComponent<Button>().interactable = true;
			    ChangeCollectable(collision.collider.gameObject.name);
			    try
			    {
				    Destroy(collision.transform.Find("Feature").gameObject);
			    }
			    catch (Exception e)
			    {
				    Debug.Log("No Destroyable Object " + e);
			    }
			    collision.gameObject.name = "Concrete";
		    }
		    if (hammerMode)
		    {
			    TransformPickaxe();
			    Debug.Log("TOUCHED TO " + collision.collider.gameObject.name);
			    collision.collider.gameObject.transform.position += (_cp ? Vector3.right : Vector3.left) * 1.1f;
			    if(_cp) button.ChangeIcon("Empty");
		    }
	    }
	    
    }
    
	private void ChangeCollectable(string collectableName)
	{
		character.collectable = collectableName switch
		{
			"Hammer" => FindObjectOfType<Hammer>(),
			"Nest" => FindObjectOfType<Nest>(),
			"Bomb" => FindObjectOfType<Bomb>(),
			"ThrowSnow" => FindObjectOfType<ThrowSnow>(),
			"Shield" => FindObjectOfType<Shield>(),
			_ => character.collectable
		};
		if (_cp) button.ChangeIcon(collectableName);
	}
    
    private void PutStep()
    {
        character.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _step.transform.position = transform.position - new Vector3(0.2f,0.7f,-0.2f);
    }

    private void TransformPickaxe()
    {
	    hammerMode = false;
	    transform.Find("Top").localScale = new Vector3(0.3f, 0.07f, 0.05f);
        GetComponentInChildren<MeshRenderer>().material = pickaxeMaterial;
    }
}
