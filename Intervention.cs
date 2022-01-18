using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Intervention
{
    public Sprite icon;
    public string name;

    [TextArea(1,10)]
    public List<string> sentences;

    public bool isChoice;
}

/*
Esta clase determina una frase de una conversación, junto al icono
y nombre que se muestran junto a ella.
Lo he implementado de forma que cada frase se guarde individualmente
porque permite cosas como diferentes caras en diferentes frases
y permite construir mejor una conversación (al estar tan generalizado,
se podrían realizar fácilmente conversaciones entre x personas, no solo dos).
Además, se tratará esta clase en otros métodos de forma que el icono y
nombre sean opcionales (y se usen los anteriores).
*/
