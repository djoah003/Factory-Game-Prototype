using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class MachineRotationButton : MonoBehaviour
{
    private bool Rotate;
    private UIScript uiscript;
    private RectTransform Parent;
    private float distance;
    private bool LockIn;
    private float PreviousRotation;
    private void Awake()
    {
        Canvas.ForceUpdateCanvases();
        uiscript = GameObject.Find("Machine info panel").GetComponent<UIScript>();
        Parent = GameObject.Find("RotateParent").GetComponent<RectTransform>();
        distance = Vector2.Distance(this.GetComponent<RectTransform>().position, Parent.position);
    }
    private void Update()
    {
        //Debug.Log(Mathf.Atan2(Parent.position.y - this.GetComponent<RectTransform>().position.y, Parent.position.x - this.GetComponent<RectTransform>().position.x) * Mathf.Rad2Deg);
        if (Vector2.Distance(this.GetComponent<RectTransform>().position, Parent.position) != distance)
        {
            transform.position = (this.GetComponent<RectTransform>().position - Parent.position).normalized * distance + Parent.position;
        }
        if (Rotate)
        {
            if(LockIn == false)
                this.GetComponent<RectTransform>().position = Input.mousePosition;
            if(LockIn == false)
            {
                uiscript.dragscript._object.transform.rotation = Quaternion.Euler(0, Mathf.Atan2(Parent.position.y - this.GetComponent<RectTransform>().position.y, Parent.position.x - this.GetComponent<RectTransform>().position.x) * Mathf.Rad2Deg, 0);
            }
            if (Vector2.Distance(this.GetComponent<RectTransform>().position, Parent.position) != distance)
            {
                transform.position = (this.GetComponent<RectTransform>().position - Parent.position).normalized * distance + Parent.position;
            }
        }
        if(uiscript.dragscript.ShowUI == true)
        {
            Parent.position = Camera.main.WorldToScreenPoint(uiscript.dragscript._object.transform.position);
        }
    }
    public void OnPress()
    {
        Rotate = true;
        PreviousRotation = uiscript.dragscript._object.transform.rotation.eulerAngles.y;
    }
    public void OnRelease()
    {
        if(uiscript.dragscript._object.transform.rotation.eulerAngles.y < 135 && uiscript.dragscript._object.transform.rotation.eulerAngles.y > 45)
        {
            Debug.Log("works");
            uiscript.dragscript._object.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (uiscript.dragscript._object.transform.rotation.eulerAngles.y > 135 && uiscript.dragscript._object.transform.rotation.eulerAngles.y < 225)
        {
            Debug.Log("works");
            uiscript.dragscript._object.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (uiscript.dragscript._object.transform.rotation.eulerAngles.y > 225 && uiscript.dragscript._object.transform.rotation.eulerAngles.y < 315)
        {
            Debug.Log("works");
            uiscript.dragscript._object.transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        if (uiscript.dragscript._object.transform.rotation.eulerAngles.y > 315 || uiscript.dragscript._object.transform.rotation.eulerAngles.y < 45)
        {
            Debug.Log("works");
            uiscript.dragscript._object.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        Rotate = false;
        LockIn = false;
    }
}
