using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Light directionalLight;

    //public Transform controlPoint;
    public Vector2 x;
    public Vector2 y;
    public Vector2 z;

    public LayerMask detect;
    public LayerMask checkShadow;
    public LayerMask walkable;
    public LayerMask obstancle;

    public float heightDif = 0.6f;

    public float gridSize = 1;
    public float gridSizeDisplay = 0.9f;
    public float aboveThanHitPoint = 0.2f;


    public float range = 0.00001f;
    public float controlY;

    public List<BridgeNodeCotroller> bridges;

    //public StageGridInfomation stageGridInfomation;
    public PathfindingSystem pathfindingSystem;

    #region instance
    static GridManager instance = null;

    public static GridManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GridManager>();
            }
            return instance;
        }
    }
    #endregion





    [Header("Gizmo")]
    public float gizmoPointSphereSize = 0.2f;
    public float gizmoPlaneHeight = 0.2f;
    public bool showLT;
    public bool showLB;
    public bool showRT;
    public bool showRB;

    public Color colorLR;
    public Color colorRL;
    public Color colorTB;
    public Color colorBT;

    public Color colorUnwalkable;

    public Color colorHillLR;
    public Color colorHillRL;
    public Color colorHillTB;
    public Color colorHillBT;

    public Mesh mesh;
    public bool showMesh;
    public Vector3 rotateOffset;
    public float gizmoMeshSize;


    public GridNode gridNode;

    public static Vector2Int GetVector2IntFromPosition(Vector3 pos)
    {
        return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
    }

    public bool GetNodePosition(Vector3 pos, ref Vector3 targetPos)
    {
        var temp = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
        if (gridNode.NodeNullable(temp.x, temp.y))
        {
            var node = gridNode[temp.x, temp.y];
            targetPos = node.pos;
            return true;
        }
        else
        {
            return false;
        }
    }
    public Node GetNode(Vector3 pos)
    {
        //pos += transform.position;
        var temp = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));

        //Debug.Log($"{pos}, {temp}");

        if (gridNode.NodeNullable(temp.x, temp.y))
        {
            //Debug.Log($"{gridNode[temp.x, temp.y].index} : {pos}, {temp}");
            return gridNode[temp.x, temp.y];
        }
        else
        {
            return null;
        }
    }
    public Node GetNode(Vector2Int vector2Int)
    {
        if (gridNode.NodeNullable(vector2Int.x, vector2Int.y))
        {
            return gridNode[vector2Int.x, vector2Int.y];
        }
        else
        {
            return null;
        }
    }

    public Node GetNode(int x, int y)
    {
        if (gridNode.NodeNullable(x, y))
        {
            return gridNode[x, y];
        }
        else
        {
            return null;
        }
    }
    public void Initiate()
    {
        DrawGrid();
        pathfindingSystem.Initiate();
    }


    [Button]
    private void DrawGrid()
    {
        CreateGridMap();
        //SortIndexNode();
        gridNode.SortNode();
        gridNode.ConnectNode();
        CalculateShadow();
        CreateDrawBridge();
    }

    public void CreateDrawBridge() {
        foreach (var bridge in bridges)
        {

            Debug.Log($"{bridge.gameObject.name} : {bridge.endNodeTrans.position} : {bridge.startNodeTrans.position}");

            var endNode = GetNode(bridge.endNodeTrans.position);
            var startNode = GetNode(bridge.startNodeTrans.position);
            //시작노드와 끝나는 노드 둘다 Null이 아닐경우 해당브리지는 존재함
            if (endNode != null && startNode != null)
            {
                endNode.bridgeIndex = startNode.index;
                endNode.bridge = bridge;
                bridge.startNode = startNode;

                startNode.bridgeIndex = endNode.index;
                startNode.bridge = bridge;
                bridge.endNode = endNode;
            }
        }
    }

    public static bool Approximately(float a, float b, float range = 1E-06f)
    {
        return (double)Mathf.Abs(b - a) < (double)Mathf.Max(range * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)), Mathf.Epsilon * 8f);
    }

    public void CalculateShadow()
    {
        var inve = -directionalLight.transform.forward;
        foreach (var node in gridNode.nodesDic)
        {
            Debug.DrawLine(node.Value.pos, node.Value.pos + inve * 20, Color.red, 5);
            if (Physics.Raycast(node.Value.pos, inve, Mathf.Infinity, checkShadow))
            {
                //Debug.Log($"ShadowNode : {node.Key}");
                node.Value.nodeInShadow = true;
            }
        }
    }
    public int GetRange(Vector3 posA, Vector3 posB, bool fourDir = true)
    {
        return GetRange(GetNode(posA), GetNode(posB), fourDir);
    }
    public void CreateGridMap()
    {
        int xCount = (int)((x.y - x.x) / gridSize);
        int zCount = (int)((z.y - z.x) / gridSize);
        gridNode = new GridNode(xCount, zCount);

        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < zCount; j++)
            {
                RaycastHit hitCenter;
                RaycastHit hitLT;
                RaycastHit hitLB;
                RaycastHit hitRT;
                RaycastHit hitRB;


                float xSize = (transform.position.x + x.x + gridSize * i);
                float zSize = (transform.position.z + z.x + gridSize * j);

                if (Physics.Raycast(new Vector3(xSize, y.y + 10, zSize), transform.TransformDirection(Vector3.down), out hitCenter, Mathf.Infinity, obstancle)
                     //Left-Top
                     || Physics.Raycast(new Vector3(xSize - gridSize * gridSizeDisplay, y.y + 10, zSize - gridSize * gridSizeDisplay), transform.TransformDirection(Vector3.down), out hitLT, Mathf.Infinity, obstancle)
                     //Left-Bottom
                     || Physics.Raycast(new Vector3(xSize - gridSize * gridSizeDisplay, y.y + 10, zSize + gridSize * gridSizeDisplay), transform.TransformDirection(Vector3.down), out hitLB, Mathf.Infinity, obstancle)
                     //Right-Top
                     || Physics.Raycast(new Vector3(xSize + gridSize * gridSizeDisplay, y.y + 10, zSize - gridSize * gridSizeDisplay), transform.TransformDirection(Vector3.down), out hitRT, Mathf.Infinity, obstancle)
                     //Right-Bottom
                     || Physics.Raycast(new Vector3(xSize + gridSize * gridSizeDisplay, y.y + 10, zSize + gridSize * gridSizeDisplay), transform.TransformDirection(Vector3.down), out hitRB, Mathf.Infinity, obstancle)
                     )
                {
                    continue;
                }

                //Center
                if (Physics.Raycast(new Vector3(xSize, y.y + 10, zSize), transform.TransformDirection(Vector3.down), out hitCenter, Mathf.Infinity, detect)
                    //Left-Top
                    && Physics.Raycast(new Vector3(xSize - gridSize * gridSizeDisplay, y.y + 10, zSize - gridSize * gridSizeDisplay), transform.TransformDirection(Vector3.down), out hitLT, Mathf.Infinity, detect)
                    //Left-Bottom
                    && Physics.Raycast(new Vector3(xSize - gridSize * gridSizeDisplay, y.y + 10, zSize + gridSize * gridSizeDisplay), transform.TransformDirection(Vector3.down), out hitLB, Mathf.Infinity, detect)
                    //Right-Top
                    && Physics.Raycast(new Vector3(xSize + gridSize * gridSizeDisplay, y.y + 10, zSize - gridSize * gridSizeDisplay), transform.TransformDirection(Vector3.down), out hitRT, Mathf.Infinity, detect)
                    //Right-Bottom
                    && Physics.Raycast(new Vector3(xSize + gridSize * gridSizeDisplay, y.y + 10, zSize + gridSize * gridSizeDisplay), transform.TransformDirection(Vector3.down), out hitRB, Mathf.Infinity, detect)
                    )
                {

                    //레이에 감지된 객체들이 같은 객체가 아닌경우 타일로 인정하지 않음
                    if (!(hitCenter.transform.gameObject.layer == hitLB.transform.gameObject.layer
                        && hitCenter.transform.gameObject.layer == hitLT.transform.gameObject.layer
                        && hitCenter.transform.gameObject.layer == hitRB.transform.gameObject.layer
                        && hitCenter.transform.gameObject.layer == hitRT.transform.gameObject.layer))
                    {
                        continue;
                    }
                    if (((1 << hitCenter.transform.gameObject.layer) & walkable) == 0) return;

                    //행당하는 객체가 갈수있는지 없는지의 여부


                    var tempNode = new Node(new Vector2Int(i, j), hitCenter.point, Quaternion.identity);



                    //평면
                    if (Approximately(hitLT.point.y, hitLB.point.y, range)
                        && Approximately(hitLT.point.y, hitRT.point.y, range)
                        && Approximately(hitLT.point.y, hitRB.point.y, range)
                        && Approximately(hitRT.point.y, hitLB.point.y, range)
                        && Approximately(hitRT.point.y, hitRB.point.y, range)
                        && Approximately(hitLB.point.y, hitRB.point.y, range))
                    {
                        tempNode.tileType = TileType.Moveable_Plane;
                    }
                    //경사로 L -> R
                    else if (Approximately(hitLT.point.y, hitLB.point.y, range) && (hitLT.point.y < hitRT.point.y))
                    {
                        //stageGridInfomation.gridNode[i, j].tileType = TileType.Moveable_Slope_LR;
                        tempNode.pos.y = (hitLT.point.y + hitRT.point.y) * 0.5f;
                        float height = hitRT.point.y - tempNode.pos.y;


                        if (height > heightDif)
                        {
                            //var temp = tempNode.rotation.eulerAngles;
                            //temp.z = 90;
                            //tempNode.rotation = Quaternion.Euler(temp);
                            //tempNode.position.y += temp.x * controlY;
                            //tempNode.tileType = TileType.Moveable_Hill_LR;
                            continue;
                        }
                        else
                        {
                            float slope = (hitRT.point - hitLT.point).magnitude * 0.5f;
                            var temp = tempNode.rotation.eulerAngles;
                            temp.z += Mathf.Rad2Deg * Mathf.Asin(height / slope);
                            //temp.y += 180;
                            tempNode.rotation = Quaternion.Euler(temp);
                            tempNode.pos.y -= temp.z * controlY;
                            tempNode.tileType = TileType.Moveable_Slope_LR;
                        }

                    }
                    //경사로 R -> L
                    else if (Approximately(hitLT.point.y, hitLB.point.y, range) && (hitLT.point.y > hitRT.point.y))
                    {
                        //stageGridInfomation.tempNode.tileType = TileType.Moveable_Slope_RL;
                        tempNode.pos.y = (hitLT.point.y + hitRT.point.y) * 0.5f;
                        float height = hitLT.point.y - tempNode.pos.y;


                        if (height > heightDif)
                        {
                            //var temp = tempNode.rotation.eulerAngles;
                            //temp.z = 90;
                            //tempNode.rotation = Quaternion.Euler(temp);
                            //tempNode.position.y += temp.x * controlY;
                            //tempNode.tileType = TileType.Moveable_Hill_RL;
                            continue;
                        }
                        else
                        {
                            float slope = (hitLT.point - hitRT.point).magnitude * 0.5f;
                            var temp = tempNode.rotation.eulerAngles;
                            temp.z -= Mathf.Rad2Deg * Mathf.Asin(height / slope);
                            tempNode.rotation = Quaternion.Euler(temp);
                            tempNode.pos.y += temp.z * controlY;
                            tempNode.tileType = TileType.Moveable_Slope_RL;
                        }
                    }
                    //경사로 B -> T
                    else if (Approximately(hitLT.point.y, hitRT.point.y, range) && (hitLT.point.y > hitLB.point.y))
                    {
                        //tempNode.tileType = TileType.Moveable_Slope_BT;
                        tempNode.pos.y = (hitLT.point.y + hitLB.point.y) * 0.5f;
                        float height = hitLT.point.y - tempNode.pos.y;
                        if (height > heightDif)
                        {
                            //var temp = tempNode.rotation.eulerAngles;
                            //temp.x = 90;
                            //tempNode.rotation = Quaternion.Euler(temp);
                            //tempNode.position.y += temp.x * controlY;
                            //tempNode.tileType = TileType.Moveable_Hill_BT;
                            continue;
                        }
                        else
                        {
                            float slope = (hitLT.point - hitLB.point).magnitude * 0.5f;
                            var temp = tempNode.rotation.eulerAngles;
                            temp.x += Mathf.Rad2Deg * Mathf.Asin(height / slope);
                            tempNode.rotation = Quaternion.Euler(temp);
                            tempNode.pos.y -= temp.x * controlY;
                            tempNode.tileType = TileType.Moveable_Slope_BT;
                        }
                    }
                    //경사로 T -> B
                    else if (Approximately(hitLT.point.y, hitRT.point.y, range) && (hitLT.point.y < hitLB.point.y))
                    {
                        tempNode.pos.y = (hitLT.point.y + hitLB.point.y) * 0.5f;
                        float height = hitLB.point.y - tempNode.pos.y;
                        if (height > heightDif)
                        {
                            //var temp = tempNode.rotation.eulerAngles;
                            //temp.x = 90;
                            //tempNode.rotation = Quaternion.Euler(temp);
                            //tempNode.position.y += temp.x * controlY;
                            //tempNode.tileType = TileType.Moveable_Hill_TB;
                            continue;
                        }
                        else
                        {
                            float slope = (hitLB.point - hitLT.point).magnitude * 0.5f;
                            var temp = tempNode.rotation.eulerAngles;
                            temp.x -= Mathf.Rad2Deg * Mathf.Asin(height / slope);
                            tempNode.rotation = Quaternion.Euler(temp);
                            tempNode.pos.y += temp.x * controlY;
                            tempNode.tileType = TileType.Moveable_Slope_TB;
                        }
                    }

                    if (((1 << hitCenter.transform.gameObject.layer) & walkable) != 0)
                    {
                        tempNode.isWalkable = true;
                    }

                    gridNode.AddNode(i, j, tempNode);
                    gridNode[i, j].positionLT = hitLT.point;
                    gridNode[i, j].positionLB = hitLB.point;
                    gridNode[i, j].positionRT = hitRT.point;
                    gridNode[i, j].positionRB = hitRB.point;
                    gridNode[i, j].heightLevel = Mathf.RoundToInt(hitCenter.point.y);
                }
            }


        }
    }
    public static int GetRange(Node originNode, Node TargetNode, bool fourDir)
    {
        if (fourDir)
        {
            return Mathf.Abs(originNode.index.x - TargetNode.index.x) + Mathf.Abs(originNode.index.y - TargetNode.index.y);
        }
        else
        {
            ///TODO 거리계산을 8방향으로 계산하기
            return Mathf.Abs(originNode.index.x - TargetNode.index.x) + Mathf.Abs(originNode.index.y - TargetNode.index.y);
        }

    }



    public Dictionary<Vector2Int, KeyValuePair<int, Node>> GetNeighborNodesInRange(Vector3 position, int range)
    {
        return GetNeighborNodesInRange(GetNode(position), range);
    }

    public Dictionary<Vector2Int, KeyValuePair<int, Node>> GetAllNodesInRange(Vector3 position, int range)
    {
        return GetAllNodesInRange(GetNode(position), range);
    }

    public Dictionary<Vector2Int, KeyValuePair<int, Node>> GetAllNodesInRange(Node originNode, int range)
    {
        Dictionary<Vector2Int, KeyValuePair<int, Node>> nodeInRange = new Dictionary<Vector2Int, KeyValuePair<int, Node>>();

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                //범위밖일경우 컨티뉴
                int newRange = Mathf.Abs(x) + Mathf.Abs(y);
                if (newRange > range) continue;
                //해당노드가 존재하지않을경우 컨티뉴
                var newNodeIndex = originNode.index + new Vector2Int(x, y);
                var newNode = GetNode(newNodeIndex);
                if (newNode == null) continue;
                nodeInRange.Add(newNodeIndex, new KeyValuePair<int, Node>(newRange, newNode));
            }
        }

        foreach (var item in nodeInRange)
        {

            //Debug.Log($"{item.Key}");

        }
        return nodeInRange;
    }

    //해당 지점을 이동가능한 거리의 노드를 탐색
    public Dictionary<Vector2Int, KeyValuePair<int, Node>> GetNeighborNodesInRange(Node originNode, int range)
    {
        Dictionary<Vector2Int, KeyValuePair<int, Node>> nodeInRange = new Dictionary<Vector2Int, KeyValuePair<int, Node>>();
        Queue<KeyValuePair<int, Node>> searchNode = new Queue<KeyValuePair<int, Node>>();
        searchNode.Enqueue(new KeyValuePair<int, Node>(0, originNode));
        nodeInRange.Add(originNode.index, searchNode.Peek());
        while (searchNode.Count > 0)
        {
            var tempNode = searchNode.Dequeue();
            int remainRange = tempNode.Key + 1;
            //bridge Node가 존재함
            if (tempNode.Value.bridge != null)
            {
                nodeInRange.Add(tempNode.Value.bridgeIndex, new KeyValuePair<int, Node>(remainRange, GetNode(tempNode.Value.bridgeIndex)));
            }
        
            if (range >= remainRange)
            {
                for (int i = 0; i < 4; i++)
                {
                    var neighberNode = gridNode.GetNeighborNode(tempNode.Value.index, i);
                    if (neighberNode == null) continue;
                    if (nodeInRange.ContainsKey(neighberNode.index)) continue;
                    var pair = new KeyValuePair<int, Node>(remainRange, neighberNode);
                    searchNode.Enqueue(pair);
                    nodeInRange.Add(neighberNode.index, pair);
                }
            }
        }
        return nodeInRange;
    }
    private void OnDrawGizmos()
    {
        if (gridNode == null)
        {
            return;
        }
        Gizmos.color = Color.red;

        float xL = transform.position.x - x.x;
        float xR = transform.position.x + x.y;
        float xCenter = (xL + xR) * 0.5f;
        float xRange = (xR - xL);

        float yL = transform.position.y - y.x;
        float yR = transform.position.y + y.y;
        float yCenter = (yL + yR) * 0.5f;
        float yRange = (yR - yL);

        float zL = transform.position.z - z.x;
        float zR = transform.position.z + z.y;
        float zCenter = (zL + zR) * 0.5f;
        float zRange = (zR - zL);
        Gizmos.DrawWireCube(new Vector3(xCenter, yCenter, zCenter), new Vector3(xRange, yRange, zRange));
        if (gridNode != null && gridNode.nodesDic != null)
        {
            foreach (var item in gridNode.nodesDic)
            {
                if (item.Value.tileType != TileType.Unmoveable)
                {
                    Gizmos.color = Color.blue;
                    switch (item.Value.tileType)
                    {
                        case TileType.Moveable_Slope_LR:
                            Gizmos.color = colorLR;
                            break;
                        case TileType.Moveable_Slope_RL:
                            Gizmos.color = colorRL;
                            break;
                        case TileType.Moveable_Slope_TB:
                            Gizmos.color = colorTB;
                            break;
                        case TileType.Moveable_Slope_BT:
                            Gizmos.color = colorBT;
                            break;
                        case TileType.Moveable_Hill_LR:
                            Gizmos.color = colorHillLR;
                            break;
                        case TileType.Moveable_Hill_TB:
                            Gizmos.color = colorHillTB;
                            break;
                        case TileType.Moveable_Hill_RL:
                            Gizmos.color = colorHillRL;
                            break;
                        case TileType.Moveable_Hill_BT:
                            Gizmos.color = colorHillBT;
                            break;
                    }

                    if (!item.Value.isWalkable)
                    {
                        Gizmos.color = colorUnwalkable;
                    }


                    if (mesh != null && showMesh)
                    {
                        Gizmos.DrawWireMesh(mesh, item.Value.pos + new Vector3(0, aboveThanHitPoint, 0), Quaternion.Euler(item.Value.rotation.eulerAngles + rotateOffset), Vector3.one * gizmoMeshSize);
                    }
                    else
                    {
                        Gizmos.DrawWireCube(item.Value.pos + new Vector3(0, aboveThanHitPoint, 0), new Vector3(gridSize, gizmoPlaneHeight, gridSize));
                    }

                    //if (showLT)
                    //{
                    //    Gizmos.color = colorLR;
                    //    Gizmos.DrawWireSphere(stageGridInfomation.gridNode[i, j].positionLT + new Vector3(0, aboveThanHitPoint, 0), gizmoPointSphereSize + 0.1f);
                    //}
                    //if (showLB)
                    //{
                    //    Gizmos.color = colorRL;
                    //    Gizmos.DrawWireSphere(stageGridInfomation.gridNode[i, j].positionLB + new Vector3(0, aboveThanHitPoint, 0), gizmoPointSphereSize + 0.06f);
                    //}
                    //if (showRT)
                    //{
                    //    Gizmos.color = colorTB;
                    //    Gizmos.DrawWireSphere(stageGridInfomation.gridNode[i, j].positionRT + new Vector3(0, aboveThanHitPoint, 0), gizmoPointSphereSize + 0.03f);
                    //}
                    //if (showRB)
                    //{
                    //    Gizmos.color = colorBT;
                    //    Gizmos.DrawWireSphere(stageGridInfomation.gridNode[i, j].positionRB + new Vector3(0, aboveThanHitPoint, 0), gizmoPointSphereSize);
                    //}



                }

            }
            //for (int i = 0; i < gridNode.GetLength(0); i++)
            //{
            //    for (int j = 0; j < gridNode.GetLength(1); j++)
            //    {

            //        //Debug.Log($"{tiles.GetLength(1)}");
            //    }
            //}
        }

    }
}

