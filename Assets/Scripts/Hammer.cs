using System;
using UnityEngine;

public class Hammer : CollectableBase
{
    private Pickaxe _pickAxe;

    private void TransformHammer(GameObject character)
    {
        _pickAxe = character.GetComponent<Character>().pickaxe;
        _pickAxe.hammerMode = true;
        _pickAxe.gameObject.transform.Find("Top").localScale = new Vector3(0.25f, 0.25f, 0.2f);
        _pickAxe.gameObject.GetComponentInChildren<MeshRenderer>().material = _pickAxe.hammerMaterial;
    }
    
    public override void Use(GameObject usedBy)
    {
        TransformHammer(usedBy);
    }
}