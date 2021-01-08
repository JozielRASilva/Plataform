using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour, Iinput
{
    public Vector2 DirectionsInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}

public interface Iinput
{
    Vector2 DirectionsInput();
}
