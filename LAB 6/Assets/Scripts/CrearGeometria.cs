using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class CrearGeometria : MonoBehaviour
{
    public enum formas {
        Triangulo,
        Plano,
        Cubo,
        Cilindro,
        Esfera,
        Suelo,
        HojasArbol,
        TroncoArbol

    };
    public formas forma;
    public int particiones = 20;
    public int particiones2 = 20;
    public float lado = 1;
    public float radio = 1;
    public float radio2 = 1;

    public float escalaPerlin = 0.5f;
    public float amplitudPerlin = 0.25f;

    public List<Vector3> vertices;
    public List<Vector3> normals;
    public List<Vector2> textCoords;
    public List<int> triangles;
    
    // Función generica para crear la geometría, se invoca desde el CustomInspector
    public void crear() {
        Debug.Log("Crear geometría: " + forma);
        switch (forma) {
            // Triangulo
            case formas.Triangulo: 
                crearTriangulo(); 
                break;

            case formas.Plano:
                crearPlanoV2();
                break;

            case formas.Esfera:
                crearEsferaV2();
                break;
            
            // ToDo: completar el resto de formas posibles

            // ---- Ejercicio 1 ----      
            case formas.Cubo:
                crearCuboV2();          // Temporalmente no pide el tamaño del lado
                break;
            case formas.Cilindro:
                crearCilindroV2();
                break;
            // ---- Ejercicio 2 ----
            case formas.Suelo:
                crearSueloV2();
                break;
            case formas.HojasArbol:
                crearHojasV2();
                break;
            case formas.TroncoArbol:
                crearTroncoV2();
                break;
        }
    }

    void crearTriangulo() {

        // Crear los vertices
        vertices = new List<Vector3>();
        vertices.Add(new Vector3(0, 0, 0));
        vertices.Add(new Vector3(0, 0, 1));
        vertices.Add(new Vector3(1, 0, 0));

        // Crear las normales de cada vertice
        normals = new List<Vector3>();
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));

        // Crear las coordenadas de textura de cada vertice
        textCoords = new List<Vector2>();
        textCoords.Add(new Vector2(0, 0));
        textCoords.Add(new Vector2(0, 1));
        textCoords.Add(new Vector2(1, 0));

        // Crear los triángulos, a partir de los indices de los vertices en el vector de vertices
        // Cuidado! orientación mano izquierda, vertices en sentido horario
        triangles = new List<int>();
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);

        // Crear una nueva malla de triángulos a partir de los datos anteriores
        crearMesh();
    }

    void crearPlanoV2() {

        // Crear los vertices
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        textCoords = new List<Vector2>();
        triangles = new List<int>();

        // Inicializar el contador de vertices
        int currIndex = 0;
        int tamTira = particiones + 1;

        // Calcular el tamaño de cada celda, 
        // al ser un cuadrado es igual en X y en Z
        float tamParticion = 1.0f/particiones;

        for (int i=0; i<=particiones; i++) {
            
            float X = tamParticion * i;             // Calculo de la coordenada X

            for (int j=0; j<=particiones; j++) {
                
                float Z = tamParticion * j;          // Calculo de la coordenada Z
                
                vertices.Add(new Vector3(X,0,Z));
                normals.Add(new Vector3(0,1,0));     // En el plano XZ el vector normal es el vector Y
                textCoords.Add(new Vector2(X,Z));

                if ((i>0) && (j>0)) {

                // A partir de la primera fila y la primera columna, 
                // cada vertice da lugar a 2 nuevos triángulos
                /*
                          currIndex - 1  -->   O --- X   <-- currIndex
                                               | 1 / |
                                               |  /  |
                                               | / 2 |
                currIndex - tamTira - 1  -->   O --- O   <-- currIndex - tamTira
                
                */

                    // Triángulo 1
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - 1);
                    triangles.Add(currIndex - tamTira - 1);
                    // Triángulo 2
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira - 1);
                    triangles.Add(currIndex - tamTira);
                }
                
                // Incrementar el contador de vertices
                currIndex ++;
            }
        }

        // Crear una nueva malla de triángulos a partir de los datos anteriores
        crearMesh();
    }

    void crearEsferaV2() {

        // Ecuación de la esfera por gajos
        // [U,V] en el rango [0,1]
        // x = radio * cos(2*PI*U) * sin(PI*V)
        // y = radio * cos(PI*V)
        // z = radio * sin(2*PI*U) * sin(PI*V)

        float radio = 1;

        // Crear los vertices
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        textCoords = new List<Vector2>();
        triangles = new List<int>();

        float paso = 1.0f / particiones;

        int currIndex = 0;
        int tamTira = particiones + 1;

        for (int i=0; i<=particiones; i++) {
            float u = paso * i;
            for (int j=0; j<=particiones; j++) {
                float v = paso * j;

                Vector3 aux = new Vector3(
                    Mathf.Cos(2*Mathf.PI*u) * Mathf.Sin(Mathf.PI * v),
                    Mathf.Cos(Mathf.PI*v),
                    Mathf.Sin(2*Mathf.PI*u) * Mathf.Sin(Mathf.PI * v)
                );

                vertices.Add(radio * aux);
                normals.Add(aux.normalized);    // Para una esfera centrada en 0, la normal coincide con las coordenadas del vertice
                textCoords.Add(new Vector2(u,1.0f-v));

                if (i>0 && j>0) {
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira);
                    triangles.Add(currIndex - tamTira - 1);

                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira - 1);
                    triangles.Add(currIndex - 1);
                }

                // Incrementar el contador de vertices
                currIndex ++;
            }
        }

        // Crear una nueva malla de triángulos a partir de los datos anteriores
        crearMesh();
    }

    void crearCuboV2 ()
    {
        // Crear los vertices
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        textCoords = new List<Vector2>();
        triangles = new List<int>();

        // Inicializar el contador de vertices
        int currIndex = 0;
        int tamTira = particiones + 1;

        // Calcular el tamaño de cada celda, 
        // al ser un cuadrado es igual en X y en Z
        float tamParticion = lado / particiones;

        // Tapa superior del cubo
        for (int i = 0; i <= particiones; i++)
        {

            float X = tamParticion * i;             // Calculo de la coordenada X

            for (int j = 0; j <= particiones; j++)
            {

                float Z = tamParticion * j;          // Calculo de la coordenada Z

                vertices.Add(new Vector3(X, lado, Z));
                normals.Add(new Vector3(0, 1, 0));     // En el plano XZ el vector normal es el vector Y
                textCoords.Add(new Vector2(X, Z));

                if ((i > 0) && (j > 0))
                {
                    // Triángulo 1
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - 1);
                    triangles.Add(currIndex - tamTira - 1);
                    // Triángulo 2
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira - 1);
                    triangles.Add(currIndex - tamTira);
                }

                // Incrementar el contador de vertices
                currIndex++;
            }

        }

        // Tapa inferior del cubo
        for (int i = 0; i <= particiones; i++)
        {

            float X = tamParticion * i;             // Calculo de la coordenada X

            for (int j = 0; j <= particiones; j++)
            {

                float Z = tamParticion * j;          // Calculo de la coordenada Z

                vertices.Add(new Vector3(Z, 0, X));
                normals.Add(new Vector3(0, -1, 0));     // En el plano XZ el vector normal es el vector Y
                textCoords.Add(new Vector2(X, Z));

                if ((i > 0) && (j > 0))
                {
                    // Triángulo 1
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - 1);
                    triangles.Add(currIndex - tamTira - 1);
                    // Triángulo 2
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira - 1);
                    triangles.Add(currIndex - tamTira);
                }

                // Incrementar el contador de vertices
                currIndex++;
            }

        }

        // Tapa frontal del cubo
        for (int i = 0; i <= particiones; i++)
        {

            float Y = tamParticion * i;             // Calculo de la coordenada Y

            for (int j = 0; j <= particiones; j++)
            {

                float Z = tamParticion * j;          // Calculo de la coordenada Z

                vertices.Add(new Vector3(0, Y, Z));
                normals.Add(new Vector3(-1, 0, 0));     // En el plano YZ el vector normal es el vector Y
                textCoords.Add(new Vector2(Y, Z));

                if ((i > 0) && (j > 0))
                {
                    // Triángulo 1
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - 1);
                    triangles.Add(currIndex - tamTira - 1);
                    // Triángulo 2
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira - 1);
                    triangles.Add(currIndex - tamTira);
                }

                // Incrementar el contador de vertices
                currIndex++;
            }

        }

        // Tapa posterior del cubo
        for (int i = 0; i <= particiones; i++)
        {

            float Y = tamParticion * i;             // Calculo de la coordenada Y

            for (int j = 0; j <= particiones; j++)
            {

                float Z = tamParticion * j;          // Calculo de la coordenada Z

                vertices.Add(new Vector3(lado, Z, Y));
                normals.Add(new Vector3(1, 0, 0));     // En el plano YZ el vector normal es el vector Y
                textCoords.Add(new Vector2(Z, Y));

                if ((i > 0) && (j > 0))
                {
                    // Triángulo 1
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - 1);
                    triangles.Add(currIndex - tamTira - 1);
                    // Triángulo 2
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira - 1);
                    triangles.Add(currIndex - tamTira);
                }

                // Incrementar el contador de vertices
                currIndex++;
            }

        }

        // Tapa derecha del cubo
        for (int i = 0; i <= particiones; i++)
        {

            float X = tamParticion * i;             // Calculo de la coordenada Y

            for (int j = 0; j <= particiones; j++)
            {

                float Y = tamParticion * j;          // Calculo de la coordenada Z

                vertices.Add(new Vector3(X, Y, 0));
                normals.Add(new Vector3(0, 0, -1));     // En el plano YZ el vector normal es el vector Y
                textCoords.Add(new Vector2(X, Y));

                if ((i > 0) && (j > 0))
                {
                    // Triángulo 1
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - 1);
                    triangles.Add(currIndex - tamTira - 1);
                    // Triángulo 2
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira - 1);
                    triangles.Add(currIndex - tamTira);
                }

                // Incrementar el contador de vertices
                currIndex++;
            }

        }

        // Tapa izquierda del cubo
        for (int i = 0; i <= particiones; i++)
        {

            float X = tamParticion * i;             // Calculo de la coordenada Y

            for (int j = 0; j <= particiones; j++)
            {

                float Y = tamParticion * j;          // Calculo de la coordenada Z

                vertices.Add(new Vector3(Y, X, lado));
                normals.Add(new Vector3(0, 0, 1));     // En el plano YZ el vector normal es el vector Y
                textCoords.Add(new Vector2(X, Y));

                if ((i > 0) && (j > 0))
                {
                    // Triángulo 1
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - 1);
                    triangles.Add(currIndex - tamTira - 1);
                    // Triángulo 2
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira - 1);
                    triangles.Add(currIndex - tamTira);
                }

                // Incrementar el contador de vertices
                currIndex++;
            }

        }
            // Crear una nueva malla de triángulos a partir de los datos anteriores
            crearMesh();
    }

    void crearCilindroV2()
    {
        // Crear los vertices
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        textCoords = new List<Vector2>();
        triangles = new List<int>();

        float paso = 1.0f / particiones2;
        float cara = 1.0f / particiones;

        int currIndex = 0;
        int tamTira = particiones2 + 1;

        // Centro tapas
        vertices.Add(radio * new Vector3(0, 1 / radio, 0));
        normals.Add(new Vector3(0, 1, 0));
        textCoords.Add(new Vector2(1, 0));
        currIndex++;

        vertices.Add(radio * new Vector3(0, 0, 0));
        normals.Add(new Vector3(0, -1, 0));
        textCoords.Add(new Vector2(0, 1));
        currIndex++;

        //Tapa superior del cilindro
        for (int i = 0; i <= particiones; i++)
        {
            float u = cara * i;

            Vector3 aux = new Vector3(
                    Mathf.Cos(2 * Mathf.PI * u),
                    1 / radio,
                    Mathf.Sin(2 * Mathf.PI * u)
                    );

            vertices.Add(radio * aux);
            normals.Add(new Vector3(0, 1, 0));
            textCoords.Add(new Vector2(0, 2* Mathf.PI * radio));

            if (i > 0)
            {
                triangles.Add(0);
                triangles.Add(currIndex);
                triangles.Add(currIndex - 1);
                
            }

            // Incrementar el contador de vertices
            currIndex++;
        }

        //Tapa inferior del cilindro
        for (int i = 0; i <= particiones; i++)
        {
            float u = cara * i;

            Vector3 aux = new Vector3(
                    Mathf.Cos(2 * Mathf.PI * u),
                    0,
                    Mathf.Sin(2 * Mathf.PI * u)
                    );

            vertices.Add(radio2 * aux);
            normals.Add(new Vector3(0, -1, 0));
            textCoords.Add(new Vector2(2 * Mathf.PI * radio2, 0));

            if (i > 0)
            {
                triangles.Add(currIndex - 1);
                triangles.Add(currIndex);
                triangles.Add(1);
            }

            // Incrementar el contador de vertices
            currIndex++;
        }

        // Paredes del cilindro
        for (int i = 0; i <= particiones; i++)
        {
            float u = cara * i;
            for (int j = 0; j <= particiones2; j++)
            {
                float v = paso * j;

                Vector3 aux = new Vector3(
                    Mathf.Cos(2 * Mathf.PI * u) * ((radio * v) + (radio2 * (1 - v))),
                    v,
                    Mathf.Sin(2 * Mathf.PI * u) * ((radio * v) + (radio2 * (1 - v)))
                );

                vertices.Add(aux);
                normals.Add(aux.normalized);    // Para una esfera centrada en 0, la normal coincide con las coordenadas del vertice
                textCoords.Add(new Vector2(u, v));

                if (i > 0 && j > 0)
                {
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira - 1);
                    triangles.Add(currIndex - tamTira);
                    
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - 1);
                    triangles.Add(currIndex - tamTira - 1);
                }

                // Incrementar el contador de vertices
                currIndex++;
            }
        }

        // Crear una nueva malla de triángulos a partir de los datos anteriores
        crearMesh();
    }

    void crearSueloV2()
    {
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        textCoords = new List<Vector2>();
        triangles = new List<int>();
        
        int currIndex = 0;
        int tamTira = particiones + 1;

        float tamParticion = 1.0f / particiones;

        for (int i = 0; i <= particiones; i++)
        {

            float X = tamParticion * i;             // Calculo de la coordenada X

            for (int j = 0; j <= particiones; j++)
            {

                float Z = tamParticion * j;          // Calculo de la coordenada Z

                float ruidoPerlin = Mathf.PerlinNoise(i * escalaPerlin, j * escalaPerlin);
                ruidoPerlin = ruidoPerlin * amplitudPerlin;

                vertices.Add(new Vector3(X, ruidoPerlin, Z));
                normals.Add(new Vector3(0, 1, 0));     // En el plano XZ el vector normal es el vector Y
                textCoords.Add(new Vector2(X, Z));

                if ((i > 0) && (j > 0))
                {
                    // Triángulo 1
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - 1);
                    triangles.Add(currIndex - tamTira - 1);
                    // Triángulo 2
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira - 1);
                    triangles.Add(currIndex - tamTira);
                }

                // Incrementar el contador de vertices
                currIndex++;
            }
        }

        // Crear una nueva malla de triángulos a partir de los datos anteriores
        crearMesh();
    }

    void crearHojasV2()
    {
        float radio = 1;

        // Crear los vertices
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        textCoords = new List<Vector2>();
        triangles = new List<int>();

        float paso = 1.0f / particiones;

        int currIndex = 0;
        int tamTira = particiones + 1;

        for (int i = 0; i <= particiones; i++)
        {
            float u = paso * i;
            for (int j = 0; j <= particiones; j++)
            {
                float v = paso * j;

                float ruidoPerlin = Mathf.PerlinNoise(i * escalaPerlin, j * escalaPerlin);
                ruidoPerlin = ruidoPerlin * amplitudPerlin;

                Vector3 aux = new Vector3(Mathf.Cos(2 * Mathf.PI * u) * Mathf.Sin(Mathf.PI * v) + ruidoPerlin, Mathf.Cos(Mathf.PI * v), Mathf.Sin(2 * Mathf.PI * u) * Mathf.Sin(Mathf.PI * v) + ruidoPerlin);
                
                if (i < particiones)
                    vertices.Add(radio * aux);
                else if (i == particiones)
                    vertices.Add(vertices[currIndex % (particiones+1)]);

                textCoords.Add(new Vector2(u, 1.0f - v));

                if (i > 0 && j > 0)
                {
                    // Triangulo 1
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira - 1);
                    triangles.Add(currIndex - 1);

                    // Triangulo 2
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira);
                    triangles.Add(currIndex - tamTira - 1);

                    // Normales
                    Vector3 vec1 = vertices[currIndex - (particiones + 1)] - vertices[currIndex];
                    Vector3 vec2 = vertices[currIndex - (particiones + 2)] - vertices[currIndex];
                    Vector3 vec3 = vertices[currIndex - 1] - vertices[currIndex];

                    normals.Add((Vector3.Cross(vec1, vec2) + Vector3.Cross(vec2, vec3)).normalized);
                }
                else
                    normals.Add(aux.normalized);

                // Incrementar el contador de vertices
                currIndex++;
            }
        }

        // Crear una nueva malla de triángulos a partir de los datos anteriores
        crearMesh();
    }

    void crearTroncoV2()
    {
        // Crear los vertices
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        textCoords = new List<Vector2>();
        triangles = new List<int>();

        float paso = 1.0f / particiones2;
        float cara = 1.0f / particiones;

        int currIndex = 0;
        int tamTira = particiones2 + 1;

        // Paredes del cilindro
        for (int i = 0; i <= particiones; i++)
        {
            float u = cara * i;
            for (int j = 0; j <= particiones2; j++)
            {
                float v = paso * j;

                float ruidoPerlin = Mathf.PerlinNoise(i * escalaPerlin, j * escalaPerlin);
                ruidoPerlin = ruidoPerlin * amplitudPerlin;

                Vector3 aux = new Vector3(
                    Mathf.Cos(2 * Mathf.PI * u) * ((radio * v) + (radio2 * (1 - v))) + ruidoPerlin,
                    v,
                    Mathf.Sin(2 * Mathf.PI * u) * ((radio * v) + (radio2 * (1 - v))) + ruidoPerlin
                );

                if (i < particiones)
                    vertices.Add(aux);
                else if (i == particiones)
                    vertices.Add(vertices[currIndex % (particiones * particiones2) - particiones]);

                textCoords.Add(new Vector2(u, v));

                if (i > 0 && j > 0)
                {
                    // Normales
                    Vector3 vec1 = vertices[currIndex] - vertices[currIndex - (particiones2 + 1)];
                    Vector3 vec2 = vertices[currIndex] - vertices[currIndex - (particiones2 + 2)];
                    Vector3 vec3 = vertices[currIndex] - vertices[currIndex - 1];

                    normals.Add((Vector3.Cross(vec2, vec1) + Vector3.Cross(vec3, vec2)).normalized);
                }
                else
                    normals.Add(aux.normalized);

                if (i > 0 && j > 0)
                {
                    triangles.Add(currIndex);
                    triangles.Add(currIndex - tamTira - 1);
                    triangles.Add(currIndex - tamTira);

                    triangles.Add(currIndex);
                    triangles.Add(currIndex - 1);
                    triangles.Add(currIndex - tamTira - 1);   
                }
                    

                // Incrementar el contador de vertices
                currIndex++;
            }
        }

        // Centro tapa arriba
        vertices.Add(radio * new Vector3(0, 1 / radio, 0));
        normals.Add(new Vector3(0, 1, 0));
        textCoords.Add(new Vector2(1, 0));

        // Crear una nueva malla de triángulos a partir de los datos anteriores
        crearMesh();
    }

    public void crearMesh() {

        // Crear la mesh con toda la información
        Mesh m = new Mesh();
        m.vertices = vertices.ToArray();
        m.normals = normals.ToArray();
        m.uv = textCoords.ToArray();
        m.triangles = triangles.ToArray();

        // Llamada obligatoria para recalcular la información 
        // de la malla a partir de los vectores asignados
        m.RecalculateBounds();  

        // Asignar la malla al componente MeshFilter del Gameobject
        GetComponent<MeshFilter>().sharedMesh = m;
    }
}
