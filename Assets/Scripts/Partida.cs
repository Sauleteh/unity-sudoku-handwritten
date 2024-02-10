using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partida : MonoBehaviour
{
    private int[,] tablero = new int[9,9];  // Inicialmente la matriz 9x9 está todo a 0

    public GameObject Num1;
    public GameObject Num2;
    public GameObject Num3;
    public GameObject Num4;
    public GameObject Num5;
    public GameObject Num6;
    public GameObject Num7;
    public GameObject Num8;
    public GameObject Num9;
    private int borrarNums = 40;

    private void Start()
    {
        // Aquí se creará el tablero inicial y totalmente aleatorio
        backtrack(tablero, 0, 0);
        borrarNumerosAleatorios(tablero, borrarNums);
        dibujarTablaANumero(tablero);
        comprobarPartida(tablero);
    }

    private void borrarNumerosAleatorios(int[,] tab, int n)
    {
        for (int i = 0; i < n; i++)
        {
            int fila = Random.Range(0, 9);
            int columna = Random.Range(0, 9);

            while (tab[fila, columna] == 0)
            {
                fila = Random.Range(0, 9);
                columna = Random.Range(0, 9);
            }

            tab[fila, columna] = 0;
        }
    }

    private static bool backtrack(int[,] board, int row, int col)
    {
        if (col == 9)
        {
            col = 0; ++row;
            if (row == 9) return true;
        }

        if (board[row, col] != 0)
            return backtrack(board, row, col + 1);

        List<int> nums = llenarLista();
        for (int i = 0; i < 9; i++)
        {
            int v = Random.Range(1, 10);
            while (!nums.Contains(v))
            {
                v = Random.Range(1, 10);
            }
            nums.Remove(v);

            if (isValid(board, row, col, v))
            {
                board[row, col] = v;
                if (backtrack(board, row, col + 1)) return true;
                else board[row, col] = 0;
            }
        }
        return false;
    }

    private static List<int> llenarLista()
    {
        List<int> l = new List<int>();
        l.Add(1);
        l.Add(2);
        l.Add(3);
        l.Add(4);
        l.Add(5);
        l.Add(6);
        l.Add(7);
        l.Add(8);
        l.Add(9);
        return l;
    }

    private static bool isValid(int[,] board, int row, int col, int val)
    {
        //check if value is present in column
        for (int r = 0; r < 9; r++)
            if (board[r, col] == val) return false;

        //check if value is present in the row
        for (int c = 0; c < 9; c++)
            if (board[row, c] == val) return false;

        //check for the value in the 3 X 3 block
        int I = row / 3; int J = col / 3;
        for (int a = 0; a < 3; a++)
            for (int b = 0; b < 3; b++)
                if (val == board[3 * I + a, 3 * J + b]) return false;

        return true;

    }

    private void dibujarTablaANumero(int [,] tab)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                // A partir de aquí se dibuja el número
                Vector3 posCelda = GameObject.Find("celda" + i.ToString() + j.ToString()).GetComponent<Collider2D>().bounds.center;

                GameObject ElNumero = Num1;
                if (tablero[i, j] == 2) ElNumero = Num2;
                else if (tablero[i, j] == 3) ElNumero = Num3;
                else if (tablero[i, j] == 4) ElNumero = Num4;
                else if (tablero[i, j] == 5) ElNumero = Num5;
                else if (tablero[i, j] == 6) ElNumero = Num6;
                else if (tablero[i, j] == 7) ElNumero = Num7;
                else if (tablero[i, j] == 8) ElNumero = Num8;
                else if (tablero[i, j] == 9) ElNumero = Num9;
                else if (tablero[i, j] == 0) ElNumero = null;

                if (ElNumero != null)
                {
                    var clonDibujo = Instantiate(ElNumero, new Vector3(posCelda.x, posCelda.y, -1), Quaternion.identity, GameObject.Find("Dibujo").transform);
                    clonDibujo.GetComponent<SpriteRenderer>().color = UnityEngine.Color.red;
                    clonDibujo.GetComponent<BorrarNumero>().esBorrable = false;
                }
            }
        }
    }

    public void actualizarEstado()
    {
        /* Recorrer todas las celdas que existen,
         * cogiendo los dos últimos números que hacen
         * referencia a su fila y columna y pasarlos a
         * analizarlos, guardando el número resultante
         * en el tablero.*/
        for (int f = 0; f < 9; f++)
        {
            for (int c = 0; c < 9; c++)
            {
                tablero[f,c] = analizarNumeroEnCelda(f, c); // Con esto se actualizará el tablero entero con cada cambio que pase
            }
        }

        //imprimirTablero(tablero);
        comprobarPartida(tablero);   // Se comprueba si la partida ya terminó, además de sus posibles errores
    }

    private int analizarNumeroEnCelda(int fila, int columna)
    {
        // Cast a ray straight down.
        Ray rayo = new Ray(GameObject.Find("celda"+fila.ToString()+columna.ToString()).GetComponent<Collider2D>().bounds.center, -Vector3.forward);
        RaycastHit2D hit = Physics2D.GetRayIntersection(rayo);
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.gameObject.name);
            // Retornar el número que sale en el nombre del objeto detectado
            return int.Parse(hit.collider.gameObject.name[hit.collider.gameObject.name.Length-8].ToString());
        }
        else return 0;  // No se encontró número
    }

    private void imprimirTablero(int[,] tab)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Debug.Log(string.Format("{0}\n{1},{2}", tab[i, j], i, j));
            }
        }
    }

    private void comprobarPartida(int[,] tab)
    {
        if (hayNumerosErroneos(tab) > 0)
        {
            Debug.Log("Hay números mal introducidos");
        }
        else
        {
            Debug.Log("No hay número erróneos");
            if (partidaTerminada(tab))
            {
                Debug.Log("Felicidades, has completado el sudoku");
            }
            else
            {
                Debug.Log("Aún faltan números por introducir");
            }
        }
    }

    /// <summary>
    /// Función para comprobar si hay números que entran en conflicto
    /// </summary>
    /// <param name="tab"></param>
    /// <returns>0 si no hay errores<br/>
    /// 1 si hay problema con la fila<br/>
    /// 2 si hay problema con la columna<br/>
    /// 3 si hay problema con la región</returns>
    private int hayNumerosErroneos(int[,] tab)
    {
        // Revisar filas y columnas
        for (int i = 0; i < 9; i++)
        {
            List<int> numsFila = new List<int>();
            List<int> numsColumna = new List<int>();
            for (int j = 0; j < 9; j++)
            {
                if (tab[i, j] != 0)
                {
                    if (numsFila.Contains(tab[i, j])) return 1;
                    else numsFila.Add(tab[i, j]);
                }

                if (tab[j, i] != 0)
                {
                    if (numsColumna.Contains(tab[j, i])) return 2;
                    else numsColumna.Add(tab[j, i]);
                }
            }
        }

        // Revisar regiones
        for (int i = 0; i < 9; i=i+3)
        {
            for (int j = 0; j < 9; j=j+3)
            {
                List<int> numsRegion = new List<int>();
                for (int f = i; f < i+3; f++)
                {
                    for (int c = j; c < j+3; c++)
                    {
                        if (tab[f, c] != 0)
                        {
                            if (numsRegion.Contains(tab[f, c])) return 3;
                            else numsRegion.Add(tab[f, c]);
                        }
                    }
                }
            }
        }

        return 0;
    }

    private bool partidaTerminada(int[,] tab)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (tab[i, j] == 0) return false;
            }
        }

        return true;
    }
}
