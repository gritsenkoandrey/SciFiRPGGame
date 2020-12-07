using UnityEngine;
using UnityEngine.Networking;


public class Interactable : NetworkBehaviour
{
    public Transform interactionTransform;
    public float radius = 2.0f;

    private bool _hasInteract = true;

    public bool HasInteract
    {
        get { return _hasInteract; }
        protected set { _hasInteract = value; }
    }

    public virtual bool Interact(GameObject user)
    {
        return false;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}