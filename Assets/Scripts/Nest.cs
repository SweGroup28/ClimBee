using UnityEngine;

public class Nest : CollectableBase
{
    [SerializeField] public GameObject character;
    [SerializeField] public GameObject bot;
    [SerializeField] public GameObject redBird;
    [SerializeField] public GameObject blueBird;
    private Vector3 _pos;
    private GameObject[] _birds;
    private void Start()
    {
        _birds = new GameObject[4];
    }

    private void ReleaseBirds(string who)
    {
        if (who == "Character")
        {
            _pos += character.transform.position;
            for (var i = 0; i < 4; i++)
            {
                _birds[i] = Instantiate(redBird, _pos + i * 0.1f * Vector3.right, Quaternion.identity);
                //flyTo
            }
        }
        else
        {
            _pos += bot.transform.position;
            for (var i = 0; i < 4; i++)
            {
                _birds[i] = Instantiate(blueBird, _pos - i * 0.1f * Vector3.right, Quaternion.identity);
                //flyTo
            }
        }
        foreach (var b in _birds)
        {
            Destroy(b, 5f);
        }
    }
    
    public override void Use(GameObject usedBy)
    {
        ReleaseBirds(usedBy.name);
    }
}