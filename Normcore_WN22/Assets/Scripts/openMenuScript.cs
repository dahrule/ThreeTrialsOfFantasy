using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class openMenuScript : MonoBehaviour
{
    [SerializeField] InputActionReference openMenu;
    bool isOpen = false;
    [SerializeField] GameObject quitMenu;
    [SerializeField] GameObject rayCastHand;
    // Start is called before the first frame update
    void Start()
    {
        openMenu.action.performed += Opening;
        
    }

    private void Opening(InputAction.CallbackContext obj)
    {      Debug.Log("menu script opening");
        if (!isOpen)
        {
            quitMenu.SetActive(true);
            rayCastHand.SetActive(true);
            isOpen = true;
        }
        
    }

    public void close()
    {
        quitMenu.SetActive(false);
        rayCastHand.SetActive(false);
        isOpen = false;
    }

    //private Action<InputAction.CallbackContext> OpenMenu()
    //{
    //    if (!isOpen)
    //    {
    //        quitMenu.SetActive(true);
    //        isOpen = true;
    //    }
    //    else
    //    {
    //        quitMenu.SetActive(false);
    //        isOpen = false;
    //    }
        
    //    //throw new NotImplementedException();
    //}

 
}
