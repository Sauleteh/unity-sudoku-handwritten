# Sudoku, pero tienes que dibujar los números
Un sudoku programado en Unity donde los números necesitan ser dibujados en vez de escritos desde el teclado. Para ejecutar el programa, simularlo desde Unity pues no he construido el programa para ser ejecutado sin el IDE.

![image](https://github.com/Sauleteh/unity-sudoku-handwritten/assets/22859905/dd356e13-de4a-459e-8318-bac5ff3bf206)

Al darle a iniciar el juego, se generará un tablero de sudoku aleatorio con número determinado de números. La cantidad de números del tablero generado dependerá del valor de la variable <i>borrarNums</i> localizado en el script <i>Partida.cs</i>:

![image](https://github.com/Sauleteh/unity-sudoku-handwritten/assets/22859905/e10573dd-858a-491b-983a-ca7ed975b894)

Para escribir en una celda, haz click en ella y hará un zoom hacia ella. Después, dibuja en ella y cuando sueltes el click del ratón el programa intentará predecir qué número has dibujado y lo escribirá en esa casilla.
Click derecho en la casilla para borrar el número. Escape para quitar el zoom.

En la consola de Unity puedes ver si hay números en celdas incorrectas o si has terminado la partida, a modo de ayuda:

![image](https://github.com/Sauleteh/unity-sudoku-handwritten/assets/22859905/199ecf9c-81d3-4952-b07b-1fcbcf3ce7ad)

A modo de demostración, he sacado dos capturas: una antes de soltar el click del ratón y otra después de detectar y procesar el dibujo:

<img src="https://github.com/Sauleteh/unity-sudoku-handwritten/assets/22859905/611a50ca-de77-4dd7-88cd-52cad49ffd7f" width="350px"></img>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<img src="https://github.com/Sauleteh/unity-sudoku-handwritten/assets/22859905/f374243f-1f6b-4342-a2fd-5390110efd44" width="350px"></img>
