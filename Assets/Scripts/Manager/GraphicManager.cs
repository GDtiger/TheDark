using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicManager : MonoBehaviour
{
    public ObjectPoolSCO objectPool;
    public GameManager gm;
    public GridManager gridManager;
    public TurnManager turnManager;
 

    public Vector3 lineOffset;
    public Vector3 tileOffset;


    //public Dictionary<PlayerController, Transform> targetMarks;
    [TabGroup("Ref", "Need")] public TargetMarkController targetMarkPrefab;
    [TabGroup("Ref", "Need")] public GameObject targetObject;

    public LayerMask hitGroundLayer;
    public LayerMask uiLayer;
    public LayerMask hitInterectionLayer;
    public LayerMask checkObstacleLayer;
    public LayerMask enemyRacastLayer;
    public Material moveableTile;
    public Material skillRangeTile;
    public Material enemyStayMat;
    public Material noneStayMat;
    public Material playerStayMat;
    public Material soundTile;
    public float displayPathCoolTime = 0.5f;
    public float displayPathCurTime = 0.5f;

    //public Transform checkCollider;


    //public 
    public bool showEnemySightTile;

    #region instance
    static GraphicManager instance = null;

    public static GraphicManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GraphicManager>();
            }
            return instance;
        }
    }
    #endregion


    public Dictionary<Vector2Int, TileController> tileControllers;


    public Dictionary<Vector2Int, KeyValuePair<int, Node>> nodes;

    public Dictionary<Vector2Int, KeyValuePair<int, Node>> soundNodes;
    public List<Vector2Int> clickAbleNode;
    public Dictionary<Vector2Int, TileController> detectionNode;
    //public Dictionary<Vector2Int, TileController> recognitionNode;
    //public List<Node> paths;
    public LineRenderer pathLine;
   

    //위치까지 갈경로가 존재함
    bool hasPathToPoint;
    //적경로안에서 이동해야함
    bool needToMoveUnderEnemySight;

    public Vector2Int nodeIndex;

    public void Initiate()
    {
        if (gm == null)
            gm = GameManager.Instance;
        if (gridManager == null)
            gridManager = GridManager.Instance;
        if (objectPool == null)
            objectPool = GameManager.Instance.objectPool;
        if (turnManager == null)
            turnManager = TurnManager.Instance;
        detectionNode = new Dictionary<Vector2Int, TileController>();
        soundNodes = new Dictionary<Vector2Int, KeyValuePair<int, Node>>();
        tileControllers = new Dictionary<Vector2Int, TileController>();
        //targetMarks = new Dictionary<PlayerController, Transform>();
    }

    public void TurnOffTargetMark()
    {
        targetObject.gameObject.SetActive(false);
    }

    public void TurnOnTargetMark(Vector3 pos) {
        targetObject.transform.position = pos + tileOffset;
        targetObject.gameObject.SetActive(true);
    }

    public void SetPathLine(Path path) {
        pathLine.Reset(path.path.Count);
       
        for (int i = 0; i < path.path.Count; i++)
        {
            pathLine.SetPosition(i, path.path[i].pos + lineOffset);
        }
    }

    public void ClearPath() {
        pathLine.Reset();
        TurnOffTargetMark();
    }

    public void CleaAllTileNode()
    {
        ClearSoundNode();
        ClearTileNode();
        ClearPath();
    }

    private void ClearTileNode()
    {
        foreach (var tile in clickAbleNode)
        {
            var node = gridManager.GetNode(tile);
            if (node == null) continue;
            node.isSoundRangeNode = false;
            node.isSkillRangeNode = false;
            if (!detectionNode.ContainsKey(tile))
            {
				if (tileControllers.ContainsKey(tile))
				{
                    tileControllers[tile].Return();
                    tileControllers.Remove(tile);
				}
				else
				{
                    Debug.LogError($"tile missing : {tile}");
				}
            }
            else
            {
                SetTileFromNode(tileControllers[tile], node, Vector3.zero, false);
            }
        }
        clickAbleNode.Clear();

    }

    public TargetMarkController LeaveTargetMark(Node tile) {

        Debug.Log("wow");

        return Instantiate(targetMarkPrefab, tile.pos, Quaternion.identity, transform);
    }

    //public void LeaveTargetMark(PlayerController player)
    //{
    //    targetMarks[player].position = player.onTile.pos;
    //    targetMarks[player].gameObject.SetActive(true);
    //}

    //public void HideTargetMark(PlayerController player)
    //{
    //    targetMarks[player].gameObject.SetActive(false);
    //}

    public void ClearSoundNode()
    {
        foreach (var node in soundNodes)
        {
            node.Value.Value.isSoundRangeNode = false;
            if (!clickAbleNode.Contains(node.Key) && !detectionNode.ContainsKey(node.Key))
            {
                if (tileControllers.ContainsKey(node.Key))
                {
                    tileControllers[node.Key].Return();
                    tileControllers.Remove(node.Key);
                }

            }
            else
            {
				if (tileControllers.ContainsKey(node.Key))
				{
                    SetTileFromNode(tileControllers[node.Key], node.Value.Value, Vector3.zero, false);
                }
             
            }
        }
        soundNodes.Clear();
    }





    public Dictionary<Vector2Int, KeyValuePair<int, Node>> ShowSoundNode(Vector2Int position, int soundRange)
    {
        ClearSoundNode();

        soundNodes = gridManager.GetAllNodesInRange(gridManager.GetNode(position), soundRange);
        foreach (var node in soundNodes)
        {
            node.Value.Value.isSoundRangeNode = true;
            if (tileControllers.ContainsKey(node.Value.Value.index))
            {
                SetTileFromNode(tileControllers[node.Value.Value.index], node.Value.Value, Vector3.zero, false);
            }
            else
            {
                var tile = (TileController)objectPool.RequestObject(PrefabID.Tile);
                SetTileFromNode(tile, node.Value.Value, Vector3.zero, false);
                tileControllers.Add(node.Key, tile);
            }
        }
        return soundNodes;
    }

    public Dictionary<Vector2Int, KeyValuePair<int, Node>> ShowClickAbleTile(
        Vector3 position,
        int range,
        bool move,
        bool useSkill = false,
        bool ignoreObstacle = false)
    {

        Debug.Log($"Show Click AbleTile");

        if (ignoreObstacle)
        {

            nodes = gridManager.GetAllNodesInRange(position, range);

        }
        else
        {
            nodes = gridManager.GetNeighborNodesInRange(position, range);
        }




        ClearTileNode();
        foreach (var node in nodes)
        {
            clickAbleNode.Add(node.Key);
            node.Value.Value.isSkillRangeNode = useSkill;
            if (!tileControllers.ContainsKey(node.Key))
            {
                var tile = (TileController)objectPool.RequestObject(PrefabID.Tile);
                tileControllers.Add(node.Key, tile);

               
                SetTileFromNode(tile, node.Value.Value, Vector3.zero, move);
            }
            else
            {
                SetTileFromNode(tileControllers[node.Value.Value.index], node.Value.Value, Vector3.zero, move);
            }
        }
        return nodes;
    }

    private void ClearNodeIndetection()
    {
        foreach (var tile in detectionNode)
        {
            if (clickAbleNode.Contains(tile.Key))
            {
                gridManager.GetNode(tile.Key).isSkillRangeNode = false;
                gridManager.GetNode(tile.Key).isSoundRangeNode = false;
                SetTileFromNode(tile.Value, gridManager.GetNode(tile.Key), Vector3.zero, false);
            }
            else
            {
                tile.Value.Return();
            }
        }
        detectionNode.Clear();
    }





    //감지 노드를 추가
    public Node AddDetectionNode(EnemyController enemyController, int x, int y)
    {
        var node = gridManager.GetNode(x, y);
        if (node != null)
        {
            //checkCollider.position = node.position;
            node = AddDetectionNode(enemyController, node);
        }
        return node;
    }

    public Node AddDetectionNode(EnemyController enemyController, Vector2Int index)
    {
        var node = gridManager.GetNode(index);
        if (node != null)
        {


            //checkCollider.position = node.position;
            node = AddDetectionNode(enemyController, node);
            
        }
        return node;
    }

    public Node AddDetectionNode(EnemyController enemyController, Node node)
    {
        Vector3 direction = node.pos - enemyController.sightPos.position;
        //높이차이가 일정값이상 날시 포함제외
        if (Mathf.Abs(gridManager.GetNode(enemyController.transform.position).heightLevel - node.heightLevel) > 1) return null;
        RaycastHit raycastHit;

        if (Physics.Raycast(enemyController.sightPos.position, direction, out raycastHit, Mathf.Infinity, enemyRacastLayer))
        {
            //아무런 장애물이 없을경우
            if (gridManager.GetNode(raycastHit.point) != null && gridManager.GetNode(raycastHit.point).index == node.index)
            {
                node.AddEnemyWhoWatchingThisNode(enemyController);
                if (detectionNode == null)
                {
                    detectionNode = new Dictionary<Vector2Int, TileController>();
                }
                if (showEnemySightTile && !detectionNode.ContainsKey(node.index))
                {
                    if (tileControllers.ContainsKey(node.index))
                    {
                        SetTileFromNode(tileControllers[node.index], node, Vector3.zero, false);
                        detectionNode.Add(node.index, tileControllers[node.index]);
                    }
                    else
                    {
                        var tempNode = (TileController)objectPool.RequestObject(PrefabID.Tile);
                        detectionNode.Add(node.index, tempNode);
                        tileControllers.Add(node.index, tempNode);
                        SetTileFromNode(tempNode, node, Vector3.zero, false);
                    }
                }
            }
            //Debug.Log($"{checkCollider.position} + {enemyController.sightPos.position} + {node.position} + {raycastHit.transform.gameObject.name}");
            //Debug.DrawLine(enemyController.sightPos.position, raycastHit.point, Color.red, 5);

            return node;
        }
        return null;
    }

   
    public void RemoveDetectionNode(EnemyController enemyController, Node node)
    {
        node.RemoveEnemyWhoWontWatchingThisNodeAnymore(enemyController);

        if (!node.enemyWatch && detectionNode.ContainsKey(node.index))
        {
            // DrawTile(node);
            tileControllers.Remove(node.index);
            detectionNode[node.index].Return();
            detectionNode.Remove(node.index);
        }
    }

    public void DrawTile(Node node) {
        if (detectionNode.ContainsKey(node.index))
        {
            SetTileFromNode(detectionNode[node.index], node, Vector3.zero, false);
        }
    }

    //유닛이 있을경우
    void SetTileFromNode(TileController tileController, Node node, Vector3 offset, bool moveAble)
    {
        tileController.transform.position = node.pos + offset;
        tileController.transform.rotation = node.rotation;

        tileController.DrawTile(node,
                node.nodeInShadow,
               node.tileWhoStay,
               node.enemyWatch,
               node.isSkillRangeNode,
               node.isSoundRangeNode,
               moveAble);
    }
}