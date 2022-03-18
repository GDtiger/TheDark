using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu(fileName = "new Stage Grid Info", menuName = "Data/Grid", order = 1)]
public class StageGridInfomation : SerializedScriptableObject
{

}


[System.Serializable]
public class GridNode
{
    [SerializeReference] public Dictionary<Vector2Int, Node> nodesDic;
    //int countX = 0;
    //int countY = 0;
    public GridNode(int indexX, int indexY)
    {
        nodesDic = new Dictionary<Vector2Int, Node>();
    }

    public Node this[int indexX, int indexY]
    {
        get
        {

            if (nodesDic != null
                && nodesDic.ContainsKey(new Vector2Int(indexX, indexY)))
            {
                return nodesDic[new Vector2Int(indexX, indexY)];
            }
            else
            {
                return null;
            }
        }
        set => nodesDic[new Vector2Int(indexX, indexY)] = value;
    }

    public Node this[Vector2Int index]
    {
        get
        {

            if (nodesDic != null
                && nodesDic.ContainsKey(index))
            {
                return nodesDic[index];
            }
            else
            {
                return null;
            }
        }
        set => nodesDic[index] = value;
    }


    //노드 추가
    public void AddNode(int indexX, int indexY, Node node)
    {
        var index = new Vector2Int(indexX, indexY);
        if (!nodesDic.ContainsKey(index))
        {
            nodesDic.Add(index, node);
            //if (countX < indexX)
            //{
            //    countX = indexX + 2;
            //}
            //if (countY < indexY)
            //{
            //    countY = indexY + 2;
            //}
        }
        else
        {
            Debug.LogError($"({indexX}, {indexY})에는 이미 타일이 존재합니다.");
        }
    }

    public Node GetNeighborNode(Vector2Int index, int dir)
    {
        switch (dir)
        {
            case (int)DirEight.T:
                return this[index.x, index.y + 1];
            case (int)DirEight.L:
                return this[index.x - 1, index.y];
            case (int)DirEight.B:
                return this[index.x, index.y - 1];
            case (int)DirEight.R:
                return this[index.x + 1, index.y];
            case (int)DirEight.LT:
                return this[index.x - 1, index.y + 1];
            case (int)DirEight.LB:
                return this[index.x - 1, index.y - 1];
            case (int)DirEight.RB:
                return this[index.x + 1, index.y - 1];
            case (int)DirEight.RT:
                return this[index.x + 1, index.y + 1];
            default:
                return null;
        }
    }

    public Node GetNeighborNode(Vector2Int index, DirEight dir)
    {
        switch (dir)
        {
            case DirEight.T:
                return this[index.x, index.y + 1];
            case DirEight.L:
                return this[index.x - 1, index.y];
            case DirEight.B:
                return this[index.x, index.y - 1];
            case DirEight.R:
                return this[index.x + 1, index.y];
            case DirEight.LT:
                return this[index.x - 1, index.y + 1];
            case DirEight.LB:
                return this[index.x - 1, index.y - 1];
            case DirEight.RB:
                return this[index.x + 1, index.y - 1];
            case DirEight.RT:
                return this[index.x + 1, index.y + 1];
            default:
                return null;
        }
    }

    //public Node[] GetNeighborNodes()
    //{
    //    Node[] neighborNode = [index.x - 1, index.y + 1];
    //    return neighborNode;
    //}


    //public int GetLength(int index)
    //{
    //    switch (index)
    //    {
    //        case 0:
    //            return countX;
    //        case 1:
    //            return countY;
    //        default:
    //            return -1;
    //    }
    //}

    public void ConnectNode()
    {
        foreach (var node in nodesDic)
        {

            //T
            if (NodeNullable(node.Value.index.x, node.Value.index.y + 1))
            {
                this[node.Value.index.x, node.Value.index.y].neightborIndex.Add(new Vector2Int(node.Value.index.x, node.Value.index.y + 1));
            }
            //LT
            if (NodeNullable(node.Value.index.x - 1, node.Value.index.y + 1))
            {
                this[node.Value.index.x, node.Value.index.y].neightborIndex.Add(new Vector2Int(node.Value.index.x - 1, node.Value.index.y + 1));
            }

            //L
            if (NodeNullable(node.Value.index.x - 1, node.Value.index.y))
            {
                this[node.Value.index.x, node.Value.index.y].neightborIndex.Add(new Vector2Int(node.Value.index.x - 1, node.Value.index.y));
            }

            //LB
            if (NodeNullable(node.Value.index.x - 1, node.Value.index.y - 1))
            {
                this[node.Value.index.x, node.Value.index.y].neightborIndex.Add(new Vector2Int(node.Value.index.x -1, node.Value.index.y -1));
            }

            //B
            if (NodeNullable(node.Value.index.x, node.Value.index.y - 1))
            {
                this[node.Value.index.x, node.Value.index.y].neightborIndex.Add(new Vector2Int(node.Value.index.x, node.Value.index.y - 1));
            }

            //RB
            if (NodeNullable(node.Value.index.x + 1, node.Value.index.y - 1))
            {
                this[node.Value.index.x, node.Value.index.y].neightborIndex.Add(new Vector2Int(node.Value.index.x + 1, node.Value.index.y - 1));
            }

            //R
            if (NodeNullable(node.Value.index.x + 1, node.Value.index.y))
            {
                this[node.Value.index.x, node.Value.index.y].neightborIndex.Add(new Vector2Int(node.Value.index.x + 1, node.Value.index.y));
            }

            //RT
            if (NodeNullable(node.Value.index.x + 1, node.Value.index.y + 1))
            {
                this[node.Value.index.x, node.Value.index.y].neightborIndex.Add(new Vector2Int(node.Value.index.x + 1, node.Value.index.y + 1));
            }
        }

    }

    public void SortNode()
    {
        var temp = nodesDic.OrderBy(x => x.Value.index.x).Select(x => x.Value).ToList();

        Debug.Log(temp.Count);

        nodesDic.Clear();
        foreach (var item in temp)
        {
            nodesDic.Add(item.index, item);
        }
    }

    public bool NodeNullable(int i, int j)
    {
        return this[i, j] != null && this[i, j].tileType != TileType.Unmoveable;
    }
}

[System.Serializable]
public class Nodes
{
    [SerializeReference] public Dictionary<int, Node> nodesDic;
    public Nodes()
    {
        nodesDic = new Dictionary<int, Node>();
    }
}

[System.Serializable]
public class Node
{
    public Vector2Int index;
    public Vector3 pos;
    public Quaternion rotation;
    public bool isWalkable;
    public bool isSkillRangeNode;
    public bool isSoundRangeNode;
    public int heightLevel;
    //public bool showSoundNode;

    public List<Vector2Int> neightborIndex;
    public Vector2Int bridgeIndex;
    public bool nodeInShadow;
    public BridgeNodeCotroller bridge;

    public EnemyController whoStandEnemy;
    public PlayerController whoStandPlayer;

    public void AddEnemy(EnemyController enemy) {
        whoStandEnemy = enemy;
        tileWhoStay = TileWhoStay.Enemy;
    }

    public void AddPlayer(PlayerController player) {
        whoStandPlayer = player;
        tileWhoStay = TileWhoStay.Player;
    }

    public void RemoveEnemy() {
        whoStandEnemy = null;
        tileWhoStay = TileWhoStay.None;
    }

    public void RemovePlayer() {
        whoStandPlayer = null;
        tileWhoStay = TileWhoStay.None;
    }

    //Ingame
    //해당타일을 보는 적 목록
     public List<EnemyController> enemyWhoWatchingThisNode;
    //[SerializeField] List<EnemyController> enemyWhoRecognitionThisNode;
    public int gCost;
    public int hCost;
    public int fCost;
    public Node cameFromNode;
    public TileType tileType;
    public TileWhoStay tileWhoStay;
    public int remainMove;
    [Header("적이 감지하는 구역")]
    public bool enemyWatch;

    //[Header("적이 경계하는 구역")]
    //public bool enemyRecognition;

    public void AddEnemyWhoWatchingThisNode(EnemyController enemyController)
    {
        ///TODO 이거 나중에 제거하기 (버그 체크용)
        if (enemyWhoWatchingThisNode.Contains(enemyController))
        {
            Debug.LogError($"이웃노드에 이미존재하는 적이 추가되었습니다. {enemyController.gameObject.name}");
		}
		else
		{
            enemyWhoWatchingThisNode.Add(enemyController);
            enemyWatch = true;
        }
 
    }

    //해당 타일을 더이상 안보이는 적을 노드를 보는 적목록에서 제거
    public bool RemoveEnemyWhoWontWatchingThisNodeAnymore(EnemyController enemyController)
    {
        enemyWhoWatchingThisNode.Remove(enemyController);

        //더이상 이노드를 지켜보는적이없을경우
        if (!(enemyWhoWatchingThisNode.Count > 0))
        {
            enemyWatch = false;
            return true;
        }
        return false;
    }
    //public void AddEnemyWhoRecognitionThisNode(EnemyController enemyController)
    //{

    //    ///TODO 이거 나중에 제거하기 (버그 체크용)
    //    if (enemyWhoRecognitionThisNode.Contains(enemyController))
    //    {
    //        Debug.LogError($"이웃노드에 이미존재하는 적이 추가되었습니다. (index: {index}) {enemyController.gameObject.name}");
    //    }
    //    else
    //    {
    //        enemyWhoRecognitionThisNode.Add(enemyController);
    //        enemyRecognition = true;
    //    }

    //}
    //public bool RemoveEnemyWhoWontRecognitionThisNodeAnymore(EnemyController enemyController)
    //{
    //    enemyWhoRecognitionThisNode.Remove(enemyController);

    //    //더이상 이노드를 지켜보는적이없을경우
    //    if (!(enemyWhoRecognitionThisNode.Count > 0))
    //    {
    //        enemyRecognition = false;
    //        return true;
    //    }
    //    return false;
    //}

    public void SetEnemyHasTarget(PlayerController player)
    {
        
        for (int i = 0; i < enemyWhoWatchingThisNode.Count; i++)
        {
            Debug.Log($"적이 발견함 {enemyWhoWatchingThisNode[i].gameObject.name}");
            enemyWhoWatchingThisNode[i].FoundPlayer(player);
        }
    }

    //public void SetEnemyTrackToTile(Node node)
    //{
    //    for (int i = 0; i < enemyWhoRecognitionThisNode.Count; i++)
    //    {
    //        enemyWhoRecognitionThisNode[i].SetNodeToTrack(node);
    //    }
    //}

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    //[Header("Debug")]
    public Vector3 positionLT;
    public Vector3 positionLB;
    public Vector3 positionRT;
    public Vector3 positionRB;


    public Node(Vector2Int _index, Vector3 position, Quaternion rotation)
    {
        index = _index;
        this.pos = position;
        this.rotation = rotation;
        this.tileType = TileType.Unmoveable;
        //neighborNode = new Node[8];
        enemyWhoWatchingThisNode = new List<EnemyController>();
        //enemyWhoRecognitionThisNode = new List<EnemyController>();
        bridge = null;
        bridgeIndex = new Vector2Int(-1,-1);
        neightborIndex = new List<Vector2Int>();
    }

    public Node(Vector2Int _index)
    {
        index = _index;
        this.tileType = TileType.Unmoveable;
        //neighborNode = new Vector2Int[8];
        enemyWhoWatchingThisNode = new List<EnemyController>();
        //enemyWhoRecognitionThisNode = new List<EnemyController>();
        bridge = null;
        bridgeIndex = new Vector2Int(-1, -1);
        neightborIndex = new List<Vector2Int>();
    }
}


