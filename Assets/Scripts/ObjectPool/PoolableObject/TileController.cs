using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TileController : PoolableObject
{
    public int rangeFromOrigin;

    public TileWhoStay tileState;

    GraphicManager graphicManager;


    public GameObject iconTile;
    public MeshRenderer iconTileMesh;

    public GameObject sightTile;
    public GameObject shadowTile;


    public GameObject outlineTile;
    public MeshRenderer outlineTileMesh;

    public Node node;


    public void DrawTile(Node _node, bool nodeInShadow, TileWhoStay whoStay, bool enemySight, bool skillRange, bool soundRange, bool moveAbleRange) {
        if (graphicManager == null)
        {
            graphicManager = GraphicManager.Instance;
        }
        node = _node;
        if (nodeInShadow)
        {
            shadowTile.SetActive(true);
        }
        else
        {
            shadowTile.SetActive(false);
        }

        if (enemySight)
        {
            sightTile.SetActive(true);
        }
        else
        {
            sightTile.SetActive(false);
        }

        switch (whoStay)
        {
            case TileWhoStay.None:
               // outlineTile.SetActive(false);
                tileState = whoStay;
                outlineTileMesh.material = graphicManager.noneStayMat;
                break;
            case TileWhoStay.Enemy:
               // outlineTile.SetActive(true);
                tileState = whoStay;
                outlineTileMesh.material = graphicManager.enemyStayMat;
                break;
            case TileWhoStay.Player:
                //outlineTile.SetActive(true);
                tileState = whoStay;
                outlineTileMesh.material = graphicManager.playerStayMat;
                break;
            default:
                //outlineTile.SetActive(false);
                tileState = whoStay;
                outlineTileMesh.material = graphicManager.noneStayMat;
                break;
        }

        if (soundRange)
        {
            iconTile.SetActive(true);
            iconTileMesh.material = graphicManager.soundTile;
        }
        else if(skillRange)
        {
            iconTile.SetActive(true);
            iconTileMesh.material = graphicManager.skillRangeTile;
        }

        else if (moveAbleRange)
        {
            iconTile.SetActive(true);
            iconTileMesh.material = graphicManager.moveableTile;
        }
        else
        {
            iconTile.SetActive(false);
        }


    }
}