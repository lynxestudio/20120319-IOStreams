# Flujos de entrada y salida en .NET

Todos los programas de computadora hacen uso de dispositivos de entrada y salida, los más clásicos para estos fines son el teclado (entrada estándar) y la consola salida (salida estándar).
.NET hace una abstracción de cada uno de estos dispositivos con el modelo de flujos (bajo el (concepto) de flujo), haciendo la analogía como si se tratase de una corriente de agua, solo que para el caso de las aplicaciones .NET se trata de corrientes de bytes. En resumen la comunicación entre .NET y el hardware de la computadora se realiza mediante el concepto de flujos.
La clase base para el tratamiento de estos flujos es la clase Stream, de la cuál derivan los flujos necesarios para la comunicación de los programas hacia el respaldo (persistencia) o en términos de .NET el Backing Store. .NET a diferencia de Java utiliza la misma clase para los flujos de entrada como para los de salida. A continuación la lista de los flujos básicos de entrada/salida:



FileStream: hacia archivos de disco

MemoryStream: hacia estructuras de memoria

NetworkStream: hacia conexiones de red


A continuación, mostramos como ejemplo un proyecto GTK# realizado en MonoDevelop cuyo diseño de pantalla se muestra en la siguiente imagen:




Ahora se muestra el listado que utiliza algunos de estos conceptos para leer un archivo binario .mp3 y obtener la información correspondiente al los datos del ID3 tag en estos archivos.



Primeramente en el código utilizamos las clases FileStream, BinaryReader, FileInfo del ensamblado System.IO, el cuál utilizamos en el encabezado:


using System.IO;

Con las siguientes lineas utilizamos la clase FileInfo para obtener algunas propiedades acerca del archivo, propiedades que mostramos en las etiquetas de la interfaz.

file = new FileInfo(fc.Filename);
int size = Convert.ToInt32(file.Length);
lbFileName.Text = file.Name;
lbWriteTime.Text = file.LastWriteTime.ToString();
lbFileSize.Text = size.ToString();

Ahora con el siguiente código implementamos toda la funcionalidad para la lectura de un archivo binario, utilizando un FileStream hacia un archivo creando un flujo de bytes como entrada que dirigimos hacia un BinaryReader (Lector binario) con el cuál utilizando el metódo ReadBytes para leer un arreglo de 128 bytes que son los bytes que contienen los datos del ID3 tag.

FileStream fis = new FileStream(file.FullName,FileMode.Open,
                         FileAccess.Read,FileShare.Read);
using(BinaryReader reader = new BinaryReader(fis))
{
int offset = size - 128;
reader.BaseStream.Position = offset;
b = reader.ReadBytes(128);
}

Una vez obtenido el arreglo de bytes,lo convertimos a caracteres, para que utilicemos la longitud y la posición correcta de cada dato según el estándar ID3, esto se logra con el siguiente código:
<pre>
char[] c = new char[128];
for(int i = 0;i < b.Length;i++)
 c[i] = (char)b[i];
string strTag = new string(c,0,3);
if(strTag.Equals("TAG")){
 PutMsg("File loaded");
txtTitle.Text = new string(c,3,30);
txtArtist.Text = new string(c,33,30);
txtAlbum.Text = new string(c,63,30);
txtYear.Text = new string(c,93,4);
txtComments.Buffer.Text = new string(c,97,30);
}
</pre>
Una vez compilada la aplicación, al ejecutarse utilizaremos el botón "open" para seleccionar un archivo mp3 del sistema de archivos.


Una vez que el archivo ha sido cargado se mostrará la información correspondiente:
