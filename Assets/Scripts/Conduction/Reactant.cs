using System;
using UnityEngine;
using UnityEngine.Events;

namespace Conduction
{
    public class Reactant : MonoBehaviour
    {
        [Tooltip("If null, reacts to any element")]
        [SerializeField] private ElementType elementType;

        [SerializeField] private bool canReact;

        public EventWrapper<ElementType> OnReactWrapper = new(); 
        
        public void SetReact(bool value) => canReact = value;

        public bool TryReact(ElementType elementType)
        {
            if (!canReact) return false;
            if (elementType != null && elementType != this.elementType) return false; 
            
            OnReactWrapper.Raise(elementType);
            return true;
        }
        public bool TryReact(Conductor conductor)
        {
            bool didReact = TryReact(conductor.ElementType);
            if (didReact) conductor.Conduct(this);
            return didReact;
        }
        public void React(Conductor conductor) => TryReact(conductor);
        public void React(ElementType type) => TryReact(type);

        public void TryReactWithGameObject(GameObject other)
        {
            if (other.TryGetComponent(out Conductor conductor))
                TryReact(conductor);
            else if (other.GetComponentInChildren<Conductor>() != null)
                TryReact(other.GetComponentInChildren<Conductor>());
        }
    }
}