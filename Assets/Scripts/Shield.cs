using System;
using UnityEngine;

public class Shield : CollectableBase
{
    private GameObject _character;
    private GameObject _shield;
    private void ActivateShield()
    {
        _shield.SetActive(true);
        _character.GetComponent<Character>().shieldActive = true;
    }
    
    private void DeactivateShield()
    {
        _shield.SetActive(false);
        _character.GetComponent<Character>().shieldActive = false;
    }
    
    public override void Use(GameObject usedBy)
    {
        _character = usedBy;
        _shield = _character.transform.Find("Protection").gameObject;
        ActivateShield();
        Invoke(nameof(DeactivateShield),3f);
    }
}