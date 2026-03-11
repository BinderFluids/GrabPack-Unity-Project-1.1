
using System.Diagnostics.Contracts;
using UnityEngine;


[CreateAssetMenu(menuName = "Create HandType", fileName = "HandType", order = 0)]
public class HandType : ScriptableObject
{
    [SerializeField] private GameObject handPrefab;
    public GameObject HandPrefab => handPrefab;
}