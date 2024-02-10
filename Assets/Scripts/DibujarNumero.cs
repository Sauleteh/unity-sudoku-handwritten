using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEngine;

public class DibujarNumero : MonoBehaviour
{
    public GameObject Pincel;
    private int resWidth = 32;
    private int resHeight = 32;
    public bool puedesDibujar = true;

    public GameObject Num1;
    public GameObject Num2;
    public GameObject Num3;
    public GameObject Num4;
    public GameObject Num5;
    public GameObject Num6;
    public GameObject Num7;
    public GameObject Num8;
    public GameObject Num9;

    private List<bool>[] iHash2 = new List<bool>[9*2];
    private bool primerPixel = true;
    private Vector3 coordAnterior;

    private void Start()
    {
        for (int i = 0; i < iHash2.Length; i++)
        {
            iHash2[i] = GetHash(new Bitmap(Application.dataPath + "/Sprites/NumerosParaProcesado/procesar" + (i+1) + ".png"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && GameObject.Find("Scripter").GetComponent<VariablesGlobales>().getDibujable() > 0 && puedesDibujar)    // Si se está manteniendo el ratón...
        {
            Vector3 coordenada1 = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 9;
            var dibujar = Instantiate(Pincel, coordenada1, Quaternion.identity, transform);
            dibujar.GetComponent<SpriteRenderer>().color = UnityEngine.Color.black;
            dibujar.transform.localScale = Vector3.one * 0.5f;
            if (!primerPixel)
            {
                Vector3 coordenadaMedia = (coordenada1 + coordAnterior) / 2;
                dibujar = Instantiate(Pincel, coordenadaMedia, Quaternion.identity, transform);
                dibujar.GetComponent<SpriteRenderer>().color = UnityEngine.Color.black;
                dibujar.transform.localScale = Vector3.one * 0.5f;
            }
            else primerPixel = false;
            coordAnterior = coordenada1;
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0) && GameObject.Find("Scripter").GetComponent<VariablesGlobales>().getDibujable() > 1 && puedesDibujar) // Si se levantó el ratón...
        {
            primerPixel = true;
            puedesDibujar = false;
            foreach (Transform child in transform)  // Borrar el dibujo hecho
            {
                if (child.name == "Pincel(Clone)") GameObject.Destroy(child.gameObject);
            }

            // Hacer foto del dibujo
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            GameObject.Find("CamaraParaDibujo").GetComponent<Camera>().targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            GameObject.Find("CamaraParaDibujo").GetComponent<Camera>().Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            GameObject.Find("CamaraParaDibujo").GetComponent<Camera>().targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/Sprites/Dibujo.png", bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", Application.dataPath + "/Sprites/Dibujo.png"));

            procesarDibujado();
            GameObject.Find("Scripter").GetComponent<Partida>().actualizarEstado();
        }
    }

    public void procesarDibujado()
    {
        List<bool> iHash1 = GetHash(new Bitmap(Application.dataPath + "/Sprites/Dibujo.png"));
        int[] procesados = new int[iHash2.Length];
        for (int n = 0; n < iHash2.Length; n++)
        {
            //determine the number of equal pixel (x of 256)
            procesados[n] = iHash1.Zip(iHash2[n], (i, j) => i == j).Count(eq => eq);
            //Debug.Log(string.Format("{0}:{1}", n, procesados[n]));

            int cont = 0;
            for (int b = 0; b < iHash1.Count(); b++)
            {
                if (iHash1[b] && iHash1[b] == iHash2[n][b]) cont++;
            }

            //procesados[n] = cont;
        }

        int[] resultado = mayorValor(procesados);
        Debug.Log(string.Format("Valor: {0}, Número: {1}", resultado[0], resultado[1]+1));

        StartCoroutine(esperarPorDesbloqueo(Application.dataPath + "/Sprites/Dibujo.png"));

        float[] coordenadas = GameObject.Find("Scripter").GetComponent<VariablesGlobales>().getCoords();
        GameObject ElNumero = Num1;
        if (resultado[1] + 1 == 2) ElNumero = Num2;
        else if (resultado[1] + 1 == 3) ElNumero = Num3;
        else if (resultado[1] + 1 == 4) ElNumero = Num4;
        else if (resultado[1] + 1 == 5) ElNumero = Num5;
        else if (resultado[1] + 1 == 6) ElNumero = Num6;
        else if (resultado[1] + 1 == 7) ElNumero = Num7;
        else if (resultado[1] + 1 == 8) ElNumero = Num8;
        else if (resultado[1] + 1 == 9) ElNumero = Num9;
        var clonDibujo = Instantiate(ElNumero, new Vector3(coordenadas[0], coordenadas[1], -1), Quaternion.identity, transform);
        clonDibujo.GetComponent<SpriteRenderer>().color = UnityEngine.Color.black;
        GameObject.Find("Scripter").GetComponent<VariablesGlobales>().setDibujable(0);
    }

    /// <summary>
    /// Retorna el mayor valor y el índice de un vector de enteros.
    /// </summary>
    /// <param name="vec"></param>
    /// <returns>El index 0 es el mayor valor y el index 1 la posición del valor en el vector.</returns>
    public int[] mayorValor(int[] vec)
    {
        int mayorEntero = vec[0];
        int posicion = 0;
        int[] valores = new int[2];
        for (int i = 1; i < vec.Length; i++)
        {
            if (vec[i] > mayorEntero)
            {
                mayorEntero = vec[i];
                posicion = i%9;
            }
        }
        valores[0] = mayorEntero;
        valores[1] = posicion;
        return valores;
    }

    public static List<bool> GetHash(Bitmap bmpSource)
    {
        List<bool> lResult = new List<bool>();
        //create new image with 16x16 pixel
        Bitmap bmpMin = new Bitmap(bmpSource, new Size(32, 32));
        for (int j = 0; j < bmpMin.Height; j++)
        {
            for (int i = 0; i < bmpMin.Width; i++)
            {
                //reduce colors to true / false                
                lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);
            }
        }
        return lResult;
    }

    IEnumerator esperarPorDesbloqueo(string ruta)
    {
        while (IsFileLocked(new FileInfo(ruta)))
        {
            yield return new WaitForSeconds(0.1f);
        }
        puedesDibujar = true;
    }

    protected virtual bool IsFileLocked(System.IO.FileInfo file)
    {
        try
        {
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
            {
                stream.Close();
            }
        }
        catch (IOException)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }

        //file is not locked
        return false;
    }
}
