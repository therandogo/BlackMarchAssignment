using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInputActions playerIA;
    private InputAction mouseLeftButon;

    private void Awake()
    {
        playerIA = new PlayerInputActions();        //Invoking the new input system and storing its reference
        mouseLeftButon = playerIA.Player.Mouse;     //Storing the input refernce from the input map to this variable
    }

    private void OnEnable()
    {
        mouseLeftButon.Enable();
    }

    private void OnDisable()
    {
        mouseLeftButon.Disable();
    }

    //Left mouse button tap function for other classes to access, allows decoupling of the input system from other systems
    public bool LMBTap()                
    {
        return mouseLeftButon.WasPressedThisFrame()?true:false;
    }
}
