
using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : MonoBehaviour
{
    public bool useEvents;
    [SerializeField]
    public string promptMessage;

    public UnityEvent OnInteract;

    public virtual string OnLook()
    {
        return promptMessage;
    }

    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        
    }
}
