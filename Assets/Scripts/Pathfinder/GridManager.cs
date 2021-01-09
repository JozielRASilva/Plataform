using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public Dictionary<Vector2, GridModule> grid = new Dictionary<Vector2, GridModule>();

    public int initialRadius = 20;

    public Vector2 moduleSize = new Vector2(0.5f, 0.5f);

    public LayerMask solidBlocks;

    public Vector2 ajusteEixo = new Vector2(0.5f, 0.5f);

    public bool draw = true;

    private void Start()
    {

        StartGrid();

    }


    public void StartGrid()
    {

        float initialX = (moduleSize.x) * initialRadius + ajusteEixo.x;
        float initialY = (moduleSize.y) * initialRadius + ajusteEixo.y;
        float finalX = (moduleSize.x) * -initialRadius - ajusteEixo.x;
        float finalY = (moduleSize.y) * -initialRadius - ajusteEixo.y;

        for (float x = initialX; x > finalX; x -= (moduleSize.x))
        {
            for (float y = initialY; y > finalY; y -= (moduleSize.y))
            {

                GridModule path = new GridModule();

                Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(x, y), moduleSize - new Vector2(0.05f, 0.05f), 0f, solidBlocks);
                if (colliders.Length > 0)
                {
                    path.isSolid = true;
                }


                grid.Add(new Vector2(x, y), path);

                path.FCost = CreateWorldText(null, "F", new Vector2(x, y), 80, 0.03f, Color.white, TextAnchor.MiddleCenter);
                path.GCost = CreateWorldText(null, "G", new Vector2(x - (moduleSize.x/3f) , y + (moduleSize.y/3f)), 50, 0.03f, Color.white, TextAnchor.MiddleCenter);
                path.HCost = CreateWorldText(null, "H", new Vector2(x + (moduleSize.x/3f), y - (moduleSize.y/3f)), 50, 0.03f, Color.white, TextAnchor.MiddleCenter);



            }
        }
    }


    public void CheckArea(Vector2 position)
    {
        

        Vector2 initial_position = CheckPosition(position);

        float initialX = (moduleSize.x) * initialRadius + ajusteEixo.x + initial_position.x;
        float initialY = (moduleSize.y) * initialRadius + ajusteEixo.y + initial_position.y;
        float finalX = (moduleSize.x) * -initialRadius - ajusteEixo.x + initial_position.x;
        float finalY = (moduleSize.y) * -initialRadius - ajusteEixo.y + initial_position.y;

        for (float x = initialX; x > finalX; x -= (moduleSize.x))
        {
            for (float y = initialY; y > finalY; y -= (moduleSize.y))
            {

                if (!grid.ContainsKey(new Vector2(x, y)))
                {
                    GridModule path = new GridModule();

                    Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(x, y), moduleSize - new Vector2(0.05f, 0.05f), 0f, solidBlocks);
                    if (colliders.Length > 0)
                    {
                        path.isSolid = true;
                    }

                    grid.Add(new Vector2(x, y), path);

                    path.FCost = CreateWorldText(null, "F", new Vector2(x, y), 80, 0.03f, Color.white, TextAnchor.MiddleCenter);
                    path.GCost = CreateWorldText(null, "G", new Vector2(x - (moduleSize.x / 3f), y + (moduleSize.y / 3f)), 50, 0.03f, Color.white, TextAnchor.MiddleCenter);
                    path.HCost = CreateWorldText(null, "H", new Vector2(x + (moduleSize.x / 3f), y - (moduleSize.y / 3f)), 50, 0.03f, Color.white, TextAnchor.MiddleCenter);


                }
            }
        }
    }

    public GridModule GetGridModule(Vector2 position)
    {

        Vector2 _position = GetGridPosition(position);

        if (grid.ContainsKey(_position))
        {
            return grid[_position];
        }
        else
        {
            _position = GetGridPosition(position);
            return grid[_position];
        }
    }

    public Vector2 GetGridPosition(Vector2 position)
    {

        bool get = false;

        Vector2 final_position = new Vector2();

        foreach (var cell in grid)
        {
            Bounds area = new Bounds(cell.Key, moduleSize);
            if (area.Contains(position))
            {
                
                final_position = new Vector2(cell.Key.x, cell.Key.y);
                get = true;

                break;
            }
        }

        if (!get)
        {
            CheckArea(position);
        }



        return final_position;
    }

    public Vector2 CheckPosition(Vector2 position)
    {

        Vector2 direction_normalized = (position - ajusteEixo).normalized;

        Vector2 direction = new Vector2((direction_normalized.x / Mathf.Abs(direction_normalized.x)) * 1, (direction_normalized.y / Mathf.Abs(direction_normalized.y)) * 1);
        if (float.IsNaN(direction.x)) direction.x = 0;
        if (float.IsNaN(direction.y)) direction.y = 0;

        // Check pos in X
        float posX = ajusteEixo.x;
        while (Mathf.Abs(posX - position.x) > moduleSize.x / 2)
        {
            posX += (moduleSize.x) * direction.x;
        }
        // Check pos in Y
        float posY = ajusteEixo.y;
        while (Mathf.Abs(posY - position.y) > moduleSize.y / 2)
        {
            posY += (moduleSize.y) * direction.y;
        }

        DrawCube(new Vector2(posX, posY), moduleSize, Color.red);

        return new Vector2(posX, posY);
    }

    public void DrawCube(Vector2 position, Vector2 size, Color color)
    {

        Vector2 mSize = new Vector2(size.x / 2, size.y / 2);

        Debug.DrawLine(new Vector2(position.x - mSize.x, position.y - mSize.y), new Vector2(position.x + mSize.x, position.y - mSize.y), color);

        Debug.DrawLine(new Vector2(position.x - mSize.x, position.y + mSize.y), new Vector2(position.x + mSize.x, position.y + mSize.y), color);

        Debug.DrawLine(new Vector2(position.x - mSize.x, position.y + mSize.y), new Vector2(position.x - mSize.x, position.y - mSize.y), color);

        Debug.DrawLine(new Vector2(position.x + mSize.x, position.y + mSize.y), new Vector2(position.x + mSize.x, position.y - mSize.y), color);
    }

    private void OnDrawGizmos()
    {
        if (draw)
        {
            foreach (var modules in grid)
            {

                if (modules.Value.isSolid) Gizmos.color = Color.black;
                else Gizmos.color = Color.white;
                Gizmos.DrawWireCube(modules.Key, moduleSize);

            }
        }
    }


    public static TextMesh CreateWorldText(Transform parent, string text, Vector2 localPosition, int fontSize, float characterSize, Color color, TextAnchor textAnchor)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;

        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.anchor = textAnchor;
        textMesh.characterSize = characterSize;

        return textMesh;
    }

}

[System.Serializable]
public class GridModule
{
    public bool isSolid;

    public TextMesh FCost;
    public TextMesh HCost;
    public TextMesh GCost;

    public void SetCost(PathNode node)
    {
        FCost.text = node.fCost.ToString();
        HCost.text = node.hCost.ToString();
        GCost.text = node.gCost.ToString();
    }

}