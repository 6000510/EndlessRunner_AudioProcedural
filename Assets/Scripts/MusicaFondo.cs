using UnityEngine;

public class MusicaFondo : MonoBehaviour
{
    [Header("Ajustes de la Música")]
    public float velocidad = 0.25f; // Qué tan rápido cambia de nota
    [Range(0f, 1f)] public float volumenMusica = 0.1f; // Volumen bajito para no tapar los efectos

    // Arreglo con las frecuencias de las notas (Un bajo sencillo: La, La, Do, Sol)
    private float[] melodiaBajo = { 110f, 110f, 130.81f, 98f }; 
    private int indiceNota = 0;

    private float tiempoActual = 0f;
    private float frecuenciaMuestreo;
    private float fase = 0f;

    void Awake()
    {
        frecuenciaMuestreo = AudioSettings.outputSampleRate;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            // 1. RELOJ MUSICAL: Avanzamos el tiempo para saber cuándo cambiar de nota
            tiempoActual += 1f / frecuenciaMuestreo;

            if (tiempoActual >= velocidad)
            {
                tiempoActual = 0f; // Reiniciamos el reloj
                indiceNota = (indiceNota + 1) % melodiaBajo.Length; // Pasamos a la siguiente nota
            }

            // 2. GENERADOR DE ONDA: Creamos una onda "Cuadrada" para que suene a videojuego retro
            float frecActual = melodiaBajo[indiceNota];
            fase += 2f * Mathf.PI * frecActual / frecuenciaMuestreo;
            if (fase > 2f * Mathf.PI) fase -= 2f * Mathf.PI;

            // La onda cuadrada es simple: si el seno es positivo da 1, si es negativo da -1
            float valorOscilador = (Mathf.Sin(fase) > 0 ? 1f : -1f) * volumenMusica;

            // 3. ENVIAR A LOS PARLANTES
            for (int j = 0; j < channels; j++)
            {
                data[i + j] = valorOscilador;
            }
        }
    }
}