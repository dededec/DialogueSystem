# DialogueSystem
Dialogue system made for Present! (https://alcaval.itch.io/present). Developed in 3 days.

I'm not uploading it as a tool because it's really rudimentary and made for an specific project, but it was developed in a really short time and it allows you to create dialogue straight from the Unity editor, so I was happy with it.


# How to use
In order to use it, you'll need Rotary Heart's Serializable Dictionary Lite, found here: https://assetstore.unity.com/packages/tools/utilities/serialized-dictionary-lite-110992

One object in your scene will need to have a DialogueManager script attached, and the player object will need to have a PlayerDialogueInteraction script attached, which will allow the player to start and carry on dialogues. Dialogues work via trigger collisions, so make sure objects (player and the object that starts the dialogue) are adjusted accordingly.

Also, you will need to attach whatever UI elements you have to the DialogueManager.

In order to create an actual dialogue, the object with which the players interacts has to have a DialogueTrigger script attached. In the editor you'll see various elements:

- Finalizacion Dialogo: Dictionary of string and UnityEvent which starts a function based on the last answer made by the player.
- Dialogue Base: List of Intervention representing the first dialogue that plays when the player interacts with the object.
- Respuestas Dialogo: Dictionary of string and Dialogue, which represents how the dialogue branches based on the last answer of the player.

In order to make it easy, all aspects can be set in editor, including dialogues.
