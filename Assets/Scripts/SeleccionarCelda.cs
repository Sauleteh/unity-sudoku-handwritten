using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeleccionarCelda : MonoBehaviour
{
    private void Start()
    {
        GameObject.Find("CamaraParaDibujo").GetComponent<Camera>().aspect = 1;  // Cámara para dibujo en forma cuadrada
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   // Si le das al botón escape...
        {
            Camera.main.transform.position = new Vector3(0, 0, -10);    // Centrar cámara
            Camera.main.orthographicSize = 63.35262f;   // Alejar el zoom
            GameObject.Find("Scripter").GetComponent<VariablesGlobales>().setDibujable(0);
        }
    }

    private void OnMouseDown()
    {
        Camera.main.transform.position = new Vector3(gameObject.transform.position.x-3.75f, gameObject.transform.position.y-2f, -10);   // Mover la cámara a la celda
        Camera.main.orthographicSize = 10;  // Hacer zoom a la celda
        GameObject.Find("CamaraParaDibujo").transform.position = new Vector3(gameObject.transform.position.x - 3.75f, gameObject.transform.position.y - 2f, -10);   // Mover la cámara para dibujo
        GameObject.Find("Scripter").GetComponent<VariablesGlobales>().setCoords(gameObject.transform.position.x - 3.75f, gameObject.transform.position.y - 2f);
    }

    private void OnMouseUp()
    {
        GameObject.Find("Scripter").GetComponent<VariablesGlobales>().setDibujable(GameObject.Find("Scripter").GetComponent<VariablesGlobales>().getDibujable()+1);
    }
}
