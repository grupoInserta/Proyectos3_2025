using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Node
{
    public Vector3 position;
    //public List<Node> connectedNodes = new List<Node>();
    [HideInInspector]
    public List<Connection> connections = new List<Connection>();
    public Node(Vector3 pos)
    {
        position = pos;
    }
    
    public int Connections()
    {
        return connections.Count;
    }
    /// <summary>
    /// Connects this node to another node. Optionally creates a bidirectional connection.
    /// </summary>
    /// <param name="other">The node to connect to.</param>
    /// <param name="bidirectional">If true, also connects the other node back to this one</param>

    public void Connect(Node other, bool bidirectional)
    {
        if (other == null || other == this)
            return;
        if (!connections.Exists(c => c.to == other))
        {
            float weight = Vector3.Distance(this.position, other.position);
            connections.Add(new Connection(this, other, weight));
        }
        if (bidirectional)
        {
            if (!other.connections.Exists(c => c.to == this))
            {
                float reverseWeight = Vector3.Distance(other.position, this.position);
                other.connections.Add(new Connection(other, this, reverseWeight));
            }
        }
    }
    /// <summary>
    /// Connects this node to another node with a default bidirectional connection.
    /// </summary>
    /// <param name="other">The node to connect to.</param>
    public void Connect(Node other)
    {
        Connect(other, true);
    }

    /// <summary>
    /// Disconnects this node from another node. Optionally removes the connection from the other node back to this one.
    /// </summary>
    /// <param name="other">The node to disconnect from.</param>
    /// <param name="bidirectional">If true, also disconnects this node from the other node.</param>
    public void Disconnect(Node other, bool bidirectional)
    {
        connections.RemoveAll(c => c.to == other);

        if (bidirectional)
        {
            // Remove connection from other node to this one
            other.connections.RemoveAll(c => c.to == this);
        }
    }
    /// <summary>
    /// Disconnects this node from another node, assuming a bidirectional connection.
    /// </summary>
    /// <param name="other">The node to disconnect from.</param>
    public void Disconnect(Node other)
    {
        Disconnect(other, true);
    }
    /// <summary>
    /// Removes all connections from this node. Can optionally remove connections from connected nodes back to this one.
    /// </summary>
    /// <param name="bidirectional">If true, also removes this node from each connected node's connection list.</param>
    public void Isolate(bool bidirectional)
    {
        List<Connection> connectionsToRemove = new List<Connection>(connections);
        
        foreach (Connection conn in connectionsToRemove)
        {
            Node other = conn.to;

            
            connections.Remove(conn);

            if (bidirectional)
            {
                Connection reverseConn = other.connections.Find(c => c.to == this);
                if (reverseConn != null)
                {
                    other.connections.Remove(reverseConn);
                }
            }
        }
    }
    /// <summary>
    /// Will create a bidirectional connection to all nodes within a certain radius
    /// </summary>
    /// <param name="radius"></param>
    public void ConnectAll(float radius, List<Node> allNodes)
    {
        foreach (Node other in allNodes)
        {
            if (other == this) continue;

            float distance = Vector3.Distance(this.position, other.position);
            if (distance <= radius && !IsConnectedTo(other))
            {
                Connect(other, true);
            }
        }
    }
    /// <summary>
    /// Checks if this node is connected to a given node.
    /// </summary>
    /// <param name="other">The node to check the connection with.</param>
    /// <returns>True if this node is connected to the given node; otherwise, false.</returns>
    public bool IsConnectedTo(Node other)
    {
        foreach (Connection conn in connections)
        {
            if (conn.to == other)
                return true;
        }
        return false;
    }


}
