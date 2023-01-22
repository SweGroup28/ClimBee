using UnityEngine;

public abstract class CollectableBase : MonoBehaviour, ICollectable
{
    public abstract void Use(GameObject usedBy);
}
