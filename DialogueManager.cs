using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Sprite spriteNinia = null;
    public Sprite spriteProfesor = null;
    public Sprite spriteGuardia = null;

    [SerializeField]
    private Dialogue dialogue;

    private DialogueTrigger triggerActual;

    private Coroutine rutinaTiempo;

    [SerializeField] private bool dialogueMode = false;
    public bool DialogueMode()
    {
        return dialogueMode;
    }

    private Intervention interventionActual;
    public bool IsChoiceInterventionActual()
    {
        if (interventionActual == null)
        {
            return false;
        }
        else return interventionActual.isChoice;
    }

    [Header("UI de intervenciones")]

    [Tooltip("GameObject que contiene la UI para realizar una intervención")]
    [SerializeField]
    private GameObject intervencionUI;

    [Tooltip("Elemento de imagen que mostrará el icono de quien habla")]
    [SerializeField]
    private Image iconHandler;

    [Tooltip("Elemento de texto que mostrará el nombre de quien habla")]
    [SerializeField]
    private Text nameHandler;

    [Tooltip("Elemento de texto que mostrará la frase a decir")]
    [SerializeField]
    private Text sentenceHandler;

    [Header("UI de opciones")]

    [Tooltip("GameObject que contiene la UI para realizar una elección")]
    [SerializeField]
    private GameObject choiceUI;

    [SerializeField]
    private List<Text> opciones;

    [SerializeField]
    private RectTransform selector;

    [SerializeField]
    private Slider timeBar;

    [Header("Valores de opciones")]

    [Tooltip("Tiempo aproximado que tiene el jugador para tomar una decision (en segundos, mínimo 1)")]
    [Min(1f)]
    [SerializeField]
    private float tiempoRespuesta;

    [Tooltip("Respuesta que se manda en caso de que el jugador se quede sin tiempo")]
    [SerializeField]
    public string respuestaSinTiempo = "ERROR";

    [SerializeField] private string preguntaFormulada;
    [SerializeField] private string opcionSeleccionada;
    private int indiceOpcionSeleccionada;

    [Header("Controles")]
    public KeyCode controlAvanzarDialogo = KeyCode.Space;
    public KeyCode controlOpcionArriba = KeyCode.W;
    public KeyCode controlOpcionAbajo = KeyCode.S;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void StartDialogue(DialogueTrigger dialogueTrigger, Dialogue newDialogue)
    {
        if (dialogueMode) // Estamos ya en un dialogo 
            EndDialogue();

        GameObject.Find("Player").GetComponent<PlayerMovement>().stop = true;
        GameObject.Find("Player").GetComponent<PlayerMovement>().speed = 0f;
        triggerActual = dialogueTrigger;
        dialogueMode = true;
        dialogue = newDialogue.Copy();
        ShowNextIntervention();
    }

    public void AddDialogue(Dialogue newDialogue)
    {
        dialogueMode = true;
        dialogue.intervenciones.AddRange(newDialogue.intervenciones);
    }

    public void OpcionArriba()
    {
        if (indiceOpcionSeleccionada == 0)
            return;

        --indiceOpcionSeleccionada;

        opcionSeleccionada = opciones[indiceOpcionSeleccionada].text;
        float newY = opciones[indiceOpcionSeleccionada].gameObject.GetComponent<RectTransform>().position.y;
        selector.position = new Vector3(selector.position.x, newY, 0);
    }

    public void OpcionAbajo()
    {
        if (indiceOpcionSeleccionada == interventionActual.sentences.Count - 1)
            return;

        ++indiceOpcionSeleccionada;

        opcionSeleccionada = opciones[indiceOpcionSeleccionada].text;
        float newY = opciones[indiceOpcionSeleccionada].gameObject.GetComponent<RectTransform>().position.y;
        selector.position = new Vector3(selector.position.x, newY, 0);
    }

    public void ShowNextIntervention()
    {
        if(rutinaTiempo != null)
        {
            StopCoroutine(rutinaTiempo);
            rutinaTiempo = null;
        }

        if (IsChoiceInterventionActual()) // El jugador ha respondido a algo
        {
            triggerActual.PlayerAnswer(preguntaFormulada, opcionSeleccionada);
        }

        // Debug.Log("ShowNextIntervention: Count pre-if: " + dialogue.intervenciones.Count);
        if (dialogue.intervenciones.Count < 1)
        {
            EndDialogue();
            return;
        }

        Intervention aux = dialogue.intervenciones[0];
        // ! Esto lo pongo abajo pero es para poder guardar la pregunta que se haga en caso de hacerse.
        // interventionActual = aux;
        dialogue.intervenciones.RemoveAt(0);

        if (aux.isChoice)
        {
            rutinaTiempo = StartCoroutine(tiempoRespuestaCoroutine(aux));
            preguntaFormulada = interventionActual.sentences[0]; // Guardamos la pregunta

            float newY = opciones[0].gameObject.GetComponent<RectTransform>().position.y;
            selector.position = new Vector3(selector.position.x, newY, 0);
            
            intervencionUI.SetActive(false);
            choiceUI.SetActive(true);

            for (int i = 0; i < aux.sentences.Count; ++i)
            {
                opciones[i].text = aux.sentences[i];
            }

            for (int i = aux.sentences.Count; i < opciones.Count; ++i) // Dejamos lo que quede en blanco
            {
                opciones[i].text = "";
            }

            opcionSeleccionada = opciones[0].text;
            indiceOpcionSeleccionada = 0;
        }
        else
        {
            intervencionUI.SetActive(true);
            choiceUI.SetActive(false);

            if (aux.icon != null)
            {
                iconHandler.sprite = aux.icon;
                if(aux.icon == spriteGuardia)
                {
                    Debug.Log("Guardia efefefe");
                    iconHandler.transform.localScale = Vector3.one * 1.5f;
                }
                else if(aux.icon == spriteNinia)
                {
                    Debug.Log("LA NINIA");
                    iconHandler.transform.localScale = Vector3.one * 2f;
                }
                else iconHandler.transform.localScale = Vector3.one * 2f;
            }

            if (!String.IsNullOrEmpty(aux.name))
                nameHandler.text = aux.name;

            if (!String.IsNullOrEmpty(aux.sentences[0]))
                sentenceHandler.text = aux.sentences[0];
        }

        // ! ESTO ESTA AQUI DONT FORGET QUE ES POR UNA BUENA CAUSA
        interventionActual = aux;
    }

    public void EndDialogue()
    {
        Debug.Log("ShowNextIntervention: Fin de dialogo. Count: " + dialogue.intervenciones.Count);
        intervencionUI.SetActive(false);
        choiceUI.SetActive(false);
        dialogueMode = false;
        
        if(String.IsNullOrEmpty(opcionSeleccionada))
        {
            triggerActual.EndDialogue(interventionActual.sentences[0]);
        }
        else
        {
            triggerActual.EndDialogue(opcionSeleccionada);
        }
        interventionActual = null;
        opcionSeleccionada = "";
        GameObject.Find("Player").GetComponent<PlayerMovement>().stop = false;
        GameObject.Find("Player").GetComponent<PlayerMovement>().speed = 5f;
    }

    private IEnumerator tiempoRespuestaCoroutine(Intervention inter)
    {
        timeBar.value = timeBar.maxValue;
        while(timeBar.value > timeBar.minValue)
        {
            timeBar.value -= (5/tiempoRespuesta) * Time.deltaTime;
            yield return null;
        }

        if(inter == interventionActual)
        {
            triggerActual.PlayerAnswer(preguntaFormulada, "ERROR");
        }
        else
        {
            Debug.Log("Todo OK Jose Luis");
        }
    }
}
