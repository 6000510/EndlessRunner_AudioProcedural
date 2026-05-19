using UnityEngine;

public class SonidoUI : MonoBehaviour
{
    // Función para un clic de botón normal (Sonido corto y agradable)
    public void SonidoClickNormal()
    {
        if (Sintetizador.instancia != null)
        {
            // Onda senoidal, aguda (1000Hz), muy cortita (ataque 0.01, liberación 0.1)
            Sintetizador.instancia.Reproducir(1000f, Sintetizador.TipoOnda.Senoidal, 0.01f, 0.1f);
        }
    }

    // Función para el botón de "Jugar" o "Confirmar" (Sonido más agudo y alegre)
    public void SonidoConfirmar()
    {
        if (Sintetizador.instancia != null)
        {
            Sintetizador.instancia.Reproducir(1500f, Sintetizador.TipoOnda.Senoidal, 0.05f, 0.3f);
        }
    }

    // Función para el botón de "Pausa" o "Atrás" (Sonido más grave)
    public void SonidoPausa()
    {
        if (Sintetizador.instancia != null)
        {
            Sintetizador.instancia.Reproducir(400f, Sintetizador.TipoOnda.Senoidal, 0.05f, 0.2f);
        }
    }
}