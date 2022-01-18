using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueInteraction : MonoBehaviour
{
    private DialogueManager manager;

    // Lista que guarda todos los triggeres actuales, y activa el último en entrar (el más reciente encontrado)
    // También se van eliminando según se salga del collider.
    // Es una lista para que se adapte a varios colliders juntos.
    private List<DialogueTrigger> triggeres = new List<DialogueTrigger>();

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(manager.controlAvanzarDialogo) && !manager.DialogueMode()) // El jugador empieza dialogo
        {
            if (triggeres.Count > 0)
            {
                manager.StartDialogue(triggeres[triggeres.Count - 1], triggeres[triggeres.Count - 1].dialogueBase);
                return;
            }
        }
        else if (Input.GetKeyDown(manager.controlAvanzarDialogo) && manager.DialogueMode()) // Avanzar el dialogo / Elegir opcion
        {
            manager.ShowNextIntervention();
            return;
        }

        if (manager.IsChoiceInterventionActual()) // Elegir opcion
        {
            if (Input.GetKeyDown(manager.controlOpcionArriba))
            {
                manager.OpcionArriba();
            }
            else if (Input.GetKeyDown(manager.controlOpcionAbajo))
            {
                manager.OpcionAbajo();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DialogueTrigger>() != null)
        {
            triggeres.Add(other.gameObject.GetComponent<DialogueTrigger>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DialogueTrigger>() != null)
        {
            triggeres.Remove(other.gameObject.GetComponent<DialogueTrigger>());
        }
    }
}
