using UnityEngine;

public class Sintetizador : MonoBehaviour
{
    public static Sintetizador instancia;

    public enum TipoOnda { Senoidal, Ruido } // NUEVO: Tipos de sonido

    [Header("Ajustes del Sonido")]
    public float frecuenciaBase = 440f;
    public float volumenMaximo = 0.5f;
    public TipoOnda tipoOndaActual = TipoOnda.Senoidal;

    [Header("Envolvente (Tiempo en segundos)")]
    public float tiempoAtaque = 0.05f; 
    public float tiempoLiberacion = 0.3f; 

    private float fase = 0f;
    private float frecuenciaMuestreo;
    
    private bool estaSonando = false;
    private float tiempoActual = 0f;
    private float volumenEnvolvente = 0f;

    void Awake()
    {
        if (instancia == null) instancia = this; 
        frecuenciaMuestreo = AudioSettings.outputSampleRate;
    }

    // NUEVA VERSIÓN: Ahora le decimos qué frecuencia y qué tipo de onda queremos
    public void Reproducir(float nuevaFrecuencia, TipoOnda nuevoTipo, float nuevoAtaque = 0.05f, float nuevaLiberacion = 0.3f)
    {
        frecuenciaBase = nuevaFrecuencia;
        tipoOndaActual = nuevoTipo;
        tiempoAtaque = nuevoAtaque;
        tiempoLiberacion = nuevaLiberacion;
        
        tiempoActual = 0f;     
        estaSonando = true;    
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            if (estaSonando)
            {
                tiempoActual += 1f / frecuenciaMuestreo; 
                if (tiempoActual <= tiempoAtaque)
                    volumenEnvolvente = Mathf.Lerp(0f, volumenMaximo, tiempoActual / tiempoAtaque);
                else if (tiempoActual <= tiempoAtaque + tiempoLiberacion)
                    volumenEnvolvente = Mathf.Lerp(volumenMaximo, 0f, (tiempoActual - tiempoAtaque) / tiempoLiberacion);
                else
                {
                    volumenEnvolvente = 0f;
                    estaSonando = false;
                }
            }
            else volumenEnvolvente = 0f;

            float valorOscilador = 0f;

            // NUEVO: Elegimos cómo se genera el sonido matemático
            if (tipoOndaActual == TipoOnda.Senoidal)
            {
                fase += 2f * Mathf.PI * frecuenciaBase / frecuenciaMuestreo;
                if (fase > 2f * Mathf.PI) fase -= 2f * Mathf.PI;
                valorOscilador = Mathf.Sin(fase) * volumenEnvolvente;
            }
            else if (tipoOndaActual == TipoOnda.Ruido)
            {
                // El ruido blanco son números aleatorios entre -1 y 1
                valorOscilador = Random.Range(-1f, 1f) * volumenEnvolvente;
            }

            for (int j = 0; j < channels; j++) data[i + j] = valorOscilador;
        }
    }
}