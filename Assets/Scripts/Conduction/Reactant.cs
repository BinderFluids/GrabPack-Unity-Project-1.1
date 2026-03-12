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

        public void React(ElementType elementType)
        {
            if (!canReact) return;
            if (elementType != null && elementType != this.elementType) return;

            OnReactWrapper.Raise(elementType); 
        }
        
        public void React(Conductor conductor)
        {
            React(conductor.ElementType);
        }

        public void TryReactWithGameObject(GameObject other)
        {
            if (other.TryGetComponent(out Conductor conductor))
                React(conductor);
        }
    }
}