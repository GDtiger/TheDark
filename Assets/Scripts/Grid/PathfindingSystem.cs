
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathfindingSystem : MonoBehaviour
{
    protected const int MOVE_STRAIGHT_COST = 10;
    protected const int MOVE_DIAGONAL_COST = 14;
    //public StageGridInfomation stageGridInfomation;
    public Dictionary<Vector2Int, Node> openList;
    public Dictionary<Vector2Int, Node> closedList;
    public GridManager gridManager;

    public void Initiate()
    {
        if (gridManager == null)
        {
            gridManager = GameManager.Instance.gridManager;
        }
    }




    //상대의 감지거리 밖에서 이동하기
    public Path FindPath(Vector3 startPos, Vector3 endPos, int remainMove,  bool stealthMove, bool dirFour, float heightLevel = 0.5f, bool findPathIncludeBridge = false)
    {
        Node startNode = gridManager.GetNode(startPos);
        Node endNode = gridManager.GetNode(endPos);
        Path path = new Path();

        Debug.Log($"startPos : {startNode.pos} endPos : {endNode.pos} Node {endNode.index}");
        if (endNode.bridge != null)
        {
            if (CalculateRange(startNode, endNode) > CalculateRange(startNode, gridManager.GetNode(endNode.bridgeIndex)))
            {
                endNode = gridManager.GetNode(endNode.bridgeIndex);
            }  
        }
        else
        {

        }

        if (startNode != null && endNode != null)
        {
            openList = new Dictionary<Vector2Int, Node>();
            openList.Add(startNode.index, startNode);
            closedList = new Dictionary<Vector2Int, Node>();

            foreach (var node in gridManager.gridNode.nodesDic)
            {
                //Node pathNode = GetNode(x, y);
                if (node.Value != null)
                {
                    node.Value.gCost = 99999999;
                    node.Value.CalculateFCost();
                    node.Value.cameFromNode = null;
                }
            }

            int dir = 8;
            if (dirFour)
            {
                dir = 4;
            }
            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();
            while (openList.Count > 0)
            {
                Node currentNode = GetLowestFCostNode(openList);

                //Debug.Log($"current : {currentNode.index}");
                if (currentNode == endNode)
                {
                    path.CalculatePath(endNode);
                    return path;
                }

                openList.Remove(currentNode.index);
                closedList.Add(currentNode.index, currentNode);

                for (int i = 0; i < dir; i++)
                {
                    var neighbourNode = gridManager.gridNode.GetNeighborNode(currentNode.index, i);
                    //노드가 존재하지않으므로 제외
                    if (neighbourNode == null) continue;

                    //이미 조회한 노드는 제외
                    if (closedList.ContainsKey(neighbourNode.index)) continue;

                    //두 노드 사이의 높이가 일정값이상 차이날때
                    if (Mathf.Abs(currentNode.heightLevel - neighbourNode.heightLevel) >= heightLevel) continue;


                    

                    if (endNode != neighbourNode && ( neighbourNode.tileWhoStay == TileWhoStay.Enemy || neighbourNode.tileWhoStay == TileWhoStay.Player)) continue;
                    //Debug.Log("1");

                    //시작노드에서 거리가 길때
                    if (CalculateRange(startNode, neighbourNode) > remainMove) continue;

                    //걸을수 없는곳은 취소
                    if (!neighbourNode.isWalkable)
                    {
                        closedList.Add(neighbourNode.index, neighbourNode);
                        continue;
                    }

                    //적이 보는위치는 경로상 포함시키지 않음(잠행이동일때만)
                    if (stealthMove && neighbourNode.enemyWatch)
                    {
                        closedList.Add(neighbourNode.index, neighbourNode);
                        continue;
                    }

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();
                        if (!openList.ContainsKey(neighbourNode.index))
                        {
                            openList.Add(neighbourNode.index, neighbourNode);
                        }
                    }
                }
            }
            return null;
        }
        else
        {
            return null;
        }
    }

    int CalculateRange(Node a, Node b) {
        int xDistance = Mathf.Abs(a.index.x - b.index.x);
        int zDistance = Mathf.Abs(a.index.y - b.index.y);
        return Mathf.Abs(xDistance - zDistance);
    }

    int CalculateDistanceCost(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.index.x - b.index.x);
        int zDistance = Mathf.Abs(a.index.y - b.index.y);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    


    Node GetLowestFCostNode(Dictionary<Vector2Int, Node> pathNodeList)
    {
        bool first = false;
        Node lowestFCostNode = new Node(new Vector2Int(0, 0));
        lowestFCostNode.fCost = 99999999;
        foreach (var item in pathNodeList)
        {
            if (first)
            {
                lowestFCostNode = item.Value;
            }
            else
            {
                if (item.Value.fCost < lowestFCostNode.fCost)
                {
                    lowestFCostNode = item.Value;
                }
            }
        }
        return lowestFCostNode;
    }
}