using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{

    [System.Serializable]
    public class Respuestas : SerializableDictionaryBase<string, Dialogue> { }

    [System.Serializable]
    public class EndEvents : SerializableDictionaryBase<string, UnityEvent> { }

    public EndEvents finalizacionDialogo;

    public Dialogue dialogueBase;
    protected DialogueManager manager;

    public Respuestas respuestasDialogo;

    private string ultimaRespuesta;

    [SerializeField] private Event EndDialogueEvent;

    public void PlayerAnswer(string pregunta, string respuesta)
    {
        Debug.Log("El jugador contesta: " + respuesta + " a la pregunta: " + pregunta);

        if(respuestasDialogo.ContainsKey(respuesta))
        {
            // Debug.Log("Se produce la contestacion - Count: " + respuestasDialogo[respuesta].intervenciones.Count);
            if(respuesta == manager.respuestaSinTiempo)
            {
                manager.StartDialogue(this, respuestasDialogo[respuesta]);
                ultimaRespuesta = "ERROR";
            }
            else 
            {
                manager.AddDialogue(respuestasDialogo[respuesta]);
                ultimaRespuesta = respuesta;
            }
        }
        else
        {
            manager.EndDialogue();
        }
    }

    public void StartDialogue() => StartDialogue(dialogueBase);

    public void StartDialogue(Dialogue dialogue)
    {
        if(!manager.DialogueMode()){
            GameObject.Find("Player").GetComponent<PlayerMovement>().stop = true;
            manager.StartDialogue(this, dialogue);
        }
    }

    public void EndDialogue(string ultimaRespuesta)
    {
        print(ultimaRespuesta);
        if (string.IsNullOrEmpty(ultimaRespuesta))
            return;

        if (finalizacionDialogo.ContainsKey(ultimaRespuesta))
        {
            finalizacionDialogo[ultimaRespuesta].Invoke();
        }
        else
        {
            Debug.Log("No function added for last response.");
        }
        GameObject.Find("Player").GetComponent<PlayerMovement>().stop = false;
    }

    public void PruebaFinal()
    {
        Debug.Log("Vamoss");
    }

    void Start()
    {
        var aux = GameObject.FindGameObjectWithTag("DialogueManager");
        if (aux != null)
        {
            manager = aux.GetComponent<DialogueManager>();
            if (manager == null)
            {
                Debug.LogError("Error: Componente DialogueManager no encontrada.");
            }
        }
        else
        {
            Debug.LogError("Error: Objeto DialogueManager no encontrado.");
        }
    }
}
