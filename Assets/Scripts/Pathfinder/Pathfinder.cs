using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinder : MonoBehaviour
{

    public const int MOVE_STRAIGHT_COST = 20;
    public const int MOVE_DIAGONAL_COST = 28;

    public GridManager grid;


    public GameObject initial;
    public GameObject end;

    public PathNode start_node;
    public PathNode end_node;


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(initial.transform.position, grid.moduleSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(end.transform.position, grid.moduleSize);



    }

    
    private void Update()
    {

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GetPathfind(initial.transform.position, end.transform.position));
        }*/

        start_node = new PathNode(grid.GetGridPosition(initial.transform.position), null);
        end_node = new PathNode(grid.GetGridPosition(end.transform.position), null);

        start_node.Calcule(this, start_node, end_node);
        end_node.Calcule(this, start_node, end_node);

        Vector2 pos = new Vector2(start_node.x, start_node.y);
        grid.DrawCube(initial.transform.position, grid.moduleSize, Color.cyan);
        grid.DrawCube(pos, grid.moduleSize, Color.blue);

        Vector2 _pos = new Vector2(end_node.x, end_node.y);
        grid.DrawCube(end.transform.position, grid.moduleSize, Color.cyan);
        grid.DrawCube(_pos, grid.moduleSize, Color.blue);




        List<PathNode> openList = new List<PathNode> { start_node };
        List<PathNode> closedList = new List<PathNode>();

        PathNode currentNode = new PathNode();
        while (openList.Count > 0)
        {

            currentNode = GetLowestFCostNode(openList);

            Vector2 _position = new Vector2(currentNode.x, currentNode.y);
            grid.DrawCube(_position, grid.moduleSize, Color.blue);

            if (end_node.IsSameNode(currentNode))
            {

                break;
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<PathNode> neighbourd = CheckNeighbord(currentNode, start_node, end_node);

            foreach (PathNode neightbourNode in neighbourd)
            {
                if (ContainsNode(neightbourNode, closedList)) continue;
                if (neightbourNode.module.isSolid)
                {
                    closedList.Add(neightbourNode);
                    continue;
                }


                int tentativeGcost = currentNode.gCost + CalculateDistanceCost(currentNode, neightbourNode);

                if (tentativeGcost > neightbourNode.gCost)
                {




                }

                if (!ContainsNode(neightbourNode, openList))
                {
                    openList.Add(neightbourNode);
                }


            }



        }

        foreach(var node in CalculatePath(currentNode))
        {
            Vector2 _position = new Vector2(node.x, node.y);
            grid.DrawCube(_position, grid.moduleSize, Color.green);

        }

    }

    public List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }


    

    public bool ContainsNode(PathNode node, List<PathNode> nodes)
    {
        foreach (var _node in nodes)
        {
            if (node.IsSameNode(_node)) return true;
        }

        return false;
    }

    public PathNode GetLowestFCostNode(List<PathNode> nodes)
    {
        PathNode lowestFCostNode = nodes[0];

        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = nodes[i];
            }
        }

        return lowestFCostNode;
    }

    public List<PathNode> CheckNeighbord(PathNode current, PathNode startNode, PathNode endNode)
    {
        //PathNode node = null;

        List<PathNode> nodes = new List<PathNode>();

        // UP
        nodes.Add(new PathNode(grid.GetGridPosition(new Vector2(current.x, current.y + grid.moduleSize.y)),
        current, startNode, endNode, this));
        // UP LEFT
        nodes.Add(new PathNode(grid.GetGridPosition(new Vector2(current.x - grid.moduleSize.x, current.y + grid.moduleSize.y)),
        current, startNode, endNode, this));
        // UP RIGHT
        nodes.Add(new PathNode(grid.GetGridPosition(new Vector2(current.x + grid.moduleSize.x, current.y + grid.moduleSize.y)),
        current, startNode, endNode, this));
        // LEFT
        nodes.Add(new PathNode(grid.GetGridPosition(new Vector2(current.x - grid.moduleSize.x, current.y)),
        current, startNode, endNode, this));
        // RIGHT
        nodes.Add(new PathNode(grid.GetGridPosition(new Vector2(current.x + grid.moduleSize.x, current.y)),
        current, startNode, endNode, this));
        // DOWN
        nodes.Add(new PathNode(grid.GetGridPosition(new Vector2(current.x, current.y - grid.moduleSize.y)),
        current, startNode, endNode, this));
        // DOWN LEFT
        nodes.Add(new PathNode(grid.GetGridPosition(new Vector2(current.x - grid.moduleSize.x, current.y - grid.moduleSize.y)),
        current, startNode, endNode, this));
        // DOWN RIGHT
        nodes.Add(new PathNode(grid.GetGridPosition(new Vector2(current.x + grid.moduleSize.x, current.y - grid.moduleSize.y)),
        current, startNode, endNode, this));
        

        return nodes;
    }

    public int CalculateDistanceCost(PathNode a, PathNode b)
    {
        float xDistance = Mathf.Abs(a.x - b.x);
        float yDistance = Mathf.Abs(a.y - b.y);

        float remaning = Mathf.Abs(xDistance - yDistance);

        return (int)(MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + (MOVE_STRAIGHT_COST * remaning));

    }
}

[System.Serializable]
public class PathNode
{

    public int gCost;
    public int hCost;
    public int fCost;

    public float x;
    public float y;

    public PathNode cameFromNode;

    public PathNode()
    {

    }

    public PathNode(Vector2 position, PathNode _LastNode)
    {
        module = new GridModule();

        x = position.x;
        y = position.y;

        cameFromNode = _LastNode;
    }

    public PathNode(Vector2 position, PathNode _LastNode, PathNode startNode, PathNode endNode, Pathfinder pathfinder)
    {

        Vector2 _position = pathfinder.grid.GetGridPosition(position);

        x = _position.x;
        y = _position.y;

        gCost = pathfinder.CalculateDistanceCost(startNode, this);
        hCost = pathfinder.CalculateDistanceCost(this, endNode);
        CalculeFCost();

        module = new GridModule();
        module = pathfinder.grid.GetGridModule(_position);

        cameFromNode = _LastNode;

        pathfinder.grid.grid[_position].SetCost(this);

    }

    public GridModule module;

    public float CalculeFCost()
    {
        fCost = gCost + hCost;
        return fCost;
    }

    public void Calcule(Pathfinder pathfinder, PathNode start_node, PathNode end_node)
    {

        gCost = pathfinder.CalculateDistanceCost(start_node, this);
        hCost = pathfinder.CalculateDistanceCost(this, end_node);
        CalculeFCost();
        module = pathfinder.grid.GetGridModule(new Vector2(x, y));

    }

    public bool IsSameNode(PathNode path)
    {
        if (path.x == x && path.y == y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}