using System;

public interface IInteractable
{
    event Action onInteract;
    void Interact();
}