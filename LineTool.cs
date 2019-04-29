/*
Titulo: "Wled"
Hecho en el año:2018 
-----
Title: "Wled"
Made in the year: 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LineTool : MonoBehaviour
{
    public Color onColor, offColor;
    public Transform obj, obj3DPrefab;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    public bool isActiveTool;
    public bool isOnLight;
    Transform obj1, obj3D;
    Menu menu;
    float pressTime;
    public bool blockTool;

    void Start()
    {
        menu = GetComponent<Menu>();
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            blockTool = false;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            pressTime += Time.deltaTime;
            if (pressTime > .5f)
            {
                blockTool = true;
                DisableTool();
                InactiveTool();
            }
                
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            pressTime = 0;

            if (blockTool)
                return;

            isActiveTool = !isActiveTool;
            switch (menu.stateOp)
            {
                case Menu.Operation.on:
                    isOnLight = true;
                    break;

                case Menu.Operation.off:
                    isOnLight = false;
                    break;

                case Menu.Operation.none:
                    DisableTool();
                    break;
            }
            Raycaster(true);
        }
        if (isActiveTool)
            Raycaster(false);
    }

    void Raycaster(bool isFirst)
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (isFirst)
            {
                //Primer toque
                if (result.gameObject.CompareTag("Mapa"))
                {
                    isFirst = false;
                    if (obj1 == null)
                    {
                        obj1 = Instantiate(obj, results[0].screenPosition, Quaternion.identity);
                        if(menu.stateOp== Menu.Operation.on)
                            obj1.GetComponent<Image>().color = onColor;
                        if (menu.stateOp == Menu.Operation.off)
                            obj1.GetComponent<Image>().color = offColor;
                        obj3D = Instantiate(obj3DPrefab, results[0].screenPosition, Quaternion.identity);
                    }
                    else
                    {
                        obj1.position = results[0].screenPosition;
                        obj3D.position = obj1.position;
                    }
                    obj1.parent = results[0].gameObject.transform;
                }
                else
                {
                    isActiveTool = false;
                }
            }
            else
            {
                //Arrastrando linea
                if (result.gameObject.CompareTag("Nodos"))
                {
                    if (result.gameObject.GetComponent<MarkerController>().isOn != isOnLight)
                    {
                        result.gameObject.GetComponent<MarkerController>().OnLight(isOnLight);
                    }
                }
            }

            if (isActiveTool)
            {
                obj1.position = results[0].screenPosition;
                obj3D.position = obj1.position;
            }
            else
            {
                InactiveTool();   
            }
        }
    }

    void InactiveTool()
    {
        if (obj1 != null)
        {
            Destroy(obj1.gameObject);
            Destroy(obj3D.gameObject);
        }
    }
    void DisableTool()
    {
        isActiveTool = false;
        isOnLight = false;
    }
}
