using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Node : MonoBehaviour
{
    public Vector3 position;
    [SerializeField] private List<int> connectionIndices = new List<int>();
    [NonSerialized] public List<Node> connections; // This will be rebuilt at runtime


}
