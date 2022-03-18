using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class Path
{
    public List<Node> path;

    //startnode를 진입점으로 잡힌 브릿지들
    public List<BridgeNodeCotroller> bridgesFirst;

    //endnode를 진입점으로 잡힌 브릿지들
    public List<BridgeNodeCotroller> bridgesLast;

    public bool hasBridge;

    public int Count { get { return path.Count; } }

    public Path()
    {
        path = new List<Node>();
        bridgesFirst = new List<BridgeNodeCotroller>();
        bridgesLast = new List<BridgeNodeCotroller>();
    }

    public void Clear() {
        path.Clear();
    }

    public void RemoveAt(int i)
    {
        path.RemoveAt(i);
    }

    public Node this[int index]
    {
        get
        {

            if (path != null
                && path.Count > index)
            {
                return path[index];
            }
            else
            {
                return null;
            }
        }
        set => path[index] = value;
    }

    public void AddNode(Node node) {
        path.Add(node);
    }

    public void CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        this.path = path;
    }
}

