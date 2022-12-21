using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UseCollectable : MonoBehaviour
{
    
    [SerializeField] public List<Sprite> sprites;
    private GameObject _character;
    private Image _image;
    private Sprite _sprite;
    
    private void Start()
    {
        GetComponent<Button>().interactable = false;
    }
    public void UseOnClick()
    {
        _character = GameObject.Find("Character");
        if (_character.GetComponent<Character>().collectable)
        {
            Debug.Log("Used: " + _character.GetComponent<Character>().collectable.name);
            _character.GetComponent<Character>().collectable.Use(_character);
            _character.GetComponent<Character>().collectable = null;
            ChangeIcon("Empty");
        }
    }
    
    public void ChangeIcon(string collectableName)
    {
        _image = transform.Find("Feature").gameObject.GetComponent<Image>();
        if (collectableName != "Empty")
        {
            _sprite = sprites.Find(sp => sp.name == collectableName);
            _image.sprite = _sprite;
            _image.color = Color.white;
        }
        else
        {
            _image.sprite = null;
            _image.color = Color.red;
            GetComponent<Button>().interactable = false;
        }
    }
}
