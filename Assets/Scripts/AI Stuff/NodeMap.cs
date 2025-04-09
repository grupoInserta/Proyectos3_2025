using UnityEngine;
using System.Collections.Generic;



public class NodeMap : MonoBehaviour
{
    public List<Node> allNodes = new List<Node>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNode() // Will add a new node at the initial position
    {

    }
    public void OnDrawGizmosSelected()
    {
        if (allNodes == null) return;

        foreach (Node node in allNodes)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(node.position,0.5f);
        }

    }
}
