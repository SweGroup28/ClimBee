using System;
using UnityEngine;

public class Shield : CollectableBase
{
    private GameObject _character;
    private void ActivateShield()
    {
        _character.transform.Find("Protection").gameObject.SetActive(true);
        _character.GetComponent<Character>().shieldActive = true;
        Invoke(nameof(DeactivateShield),3f);
    }
    
    private void DeactivateShield()
    {
        _character.transform.Find("Protection").gameObject.SetActive(false);
        _character.GetComponent<Character>().shieldActive = false;
    }
    
    public override void Use(GameObject usedBy)
    {
        _character = usedBy;
        ActivateShield();
    }
}