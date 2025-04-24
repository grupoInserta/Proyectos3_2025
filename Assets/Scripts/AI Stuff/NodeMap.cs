using UnityEngine;
using System.Collections.Generic;



public class NodeMap : MonoBehaviour
{

    public Color gizmoColor = Color.red;
    
    public List<Node> nodes = new List<Node>();

    public uint selectNodeIndex;

    [Header("Editor Tools")]
    public Node debugNode;
    public Node selectedPrimaryNode = null;
    public Node selectedSecondaryNode = null;
    public bool bidirectionalConnection;

    public Node AddNode(Vector3 position)
    {
        Node newNode = new Node(position);
        nodes.Add(newNode);
        newNode.nodeMap = this;
        UpdateNodeIDs();
        return newNode;
    }

    public void AddNodeFromSelected()
    {
        Vector3 pos = debugNode != null ? debugNode.position : transform.position;
        AddNode(pos);
    }
    public void SelectPrimaryNode()
    {
        Node[] nodes = this.nodes.ToArray();
        //selectedPrimaryNode = (int)selectNodeIndex >= nodes.Length? nodes[nodes.Length - 1]: nodes[(int)selectNodeIndex];
        if (debugNode != null) selectedPrimaryNode = debugNode;
    }
    public void SelectSecondaryNode()
    {
        Node[] nodes = this.nodes.ToArray();
        if(debugNode != null) selectedSecondaryNode = debugNode;
        debugNode = null;
    }
    public void DeselectNodes()
    {
        selectedPrimaryNode = null;
        selectedSecondaryNode = null;
    }

    public void ConnectPrimaryAndSecondaryNodes()
    {
        if(selectedPrimaryNode != null && selectedSecondaryNode != null &&
        nodes.Contains(selectedPrimaryNode) && nodes.Contains(selectedSecondaryNode))
    {
            selectedPrimaryNode.Connect(selectedSecondaryNode);
        }
    }
    public void DisconnectNodes(Node a, Node b)
    {
        if (a != null && b != null)
        {
            a.Disconnect(b);
        }
    }
    public void RemoveNode(Node node)
    {
        if (node != null && nodes.Contains(node))
        {
            node.Isolate(true);
            nodes.Remove(node);
            UpdateNodeIDs();
        }
    }
    public void RemoveSelectedNode()
    {
        RemoveNode(debugNode);
    }
   
    public void WeightAllConnections()
    {
        foreach (Node node in nodes)
        {
            foreach (Connection conn in node.connections)
            {
                if (conn.to != null)
                {
                    conn.weight = Vector3.Distance(node.position, conn.to.position);
                }
            }
        }
    }
    public void IsolateAllNodes()
    {
        foreach (Node node in nodes)
        {
            node.Isolate(true);
        }

    }
    public List<Node> CalculatePathBetweenNodes(Node start, Node finish)
    {
        return null;
    }

    #region
    public void UpdateNodeIDs()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] != null && nodes[i].nodeData != null)
            {
                nodes[i].nodeData.id = i;
            }
        }
    }
    #endregion

}
