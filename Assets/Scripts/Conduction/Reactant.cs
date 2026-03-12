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
        [SerializeField] private UnityEvent<ElementType> onReactUnityEvent;
        public event Action<ElementType> onReact = delegate {}; 
        
        public void SetReact(bool value) => canReact = value;
        
        public void React(Conductor conductor)
        {
            if (!canReact) return;
            if (elementType != null && conductor.ElementType != elementType) return;
            
            onReact?.Invoke(conductor.ElementType);
            onReactUnityEvent?.Invoke(conductor.ElementType);
        }

        public void TryReactWithGameObject(GameObject other)
        {
            if (other.TryGetComponent(out Conductor conductor))
                React(conductor);
        }
    }
}