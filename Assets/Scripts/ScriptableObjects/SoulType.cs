using UnityEngine;

//[CreateAssetMenu(fileName = "SoulType", menuName = "Scriptable Objects/SoulType")]
public abstract class SoulType : ScriptableObject
{
    [SerializeField] protected LayerMask hittable;
    public LayerMask Hittable => hittable;

    public void ActivateSpecialEffect()
    {
        // just some potential special effect activated whenever you feel like it.
    }
}
