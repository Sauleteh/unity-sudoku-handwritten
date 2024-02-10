using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorrarNumero : MonoBehaviour
{
    public bool esBorrable = true;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && esBorrable)
        {
            Destroy(gameObject);
            if (Camera.main.orthographicSize <= 10) GameObject.Find("Scripter").GetComponent<VariablesGlobales>().setDibujable(GameObject.Find("Scripter").GetComponent<VariablesGlobales>().getDibujable() + 1);
            GameObject.Find("Scripter").GetComponent<Partida>().actualizarEstado();
        }
    }

    private void OnMouseDown()
    {
        GameObject.Find("Scripter").GetComponent<VariablesGlobales>().setDibujable(0);
        Camera.main.transform.position = new Vector3(gameObject.GetComponent<Collider2D>().bounds.center.x, gameObject.GetComponent<Collider2D>().bounds.center.y, -10);   // Mover la cámara a la celda
        Camera.main.orthographicSize = 10;  // Hacer zoom a la celda
        GameObject.Find("CamaraParaDibujo").transform.position = new Vector3(gameObject.GetComponent<Collider2D>().bounds.center.x, gameObject.GetComponent<Collider2D>().bounds.center.y, -10);   // Mover la cámara para dibujo
        GameObject.Find("Scripter").GetComponent<VariablesGlobales>().setCoords(gameObject.GetComponent<Collider2D>().bounds.center.x, gameObject.GetComponent<Collider2D>().bounds.center.y);
    }
}
