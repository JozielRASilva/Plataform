using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Iinput))]
[RequireComponent(typeof(IMove))]
public class EngineController : MonoBehaviour
{
    private Iinput inputControl;

    private IMove moveControl;


    void Start()
    {
        inputControl = GetComponent<Iinput>();
        moveControl = GetComponent<IMove>();
    }


    void Update()
    {
        moveControl.Move(inputControl.DirectionsInput());        
    }
}
