using UnityEngine;

[System.Serializable]
public class Connection 
{
    public Node from; //The node where the connection starts
    public Node to; //The node where the connection ends

    public bool bidirectional;
    public float weight;

    public Connection(Node from, Node to, bool bidirectional)
    {
        this.from = from;
        this.to = to;
        this.bidirectional = bidirectional;
        this.weight = Vector3.Distance(from.position, to.position);
    }
    public Connection(Node from, Node to, float weight)
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
        this.weight = Vector3.Distance(from.position, to.position);
    }

}
