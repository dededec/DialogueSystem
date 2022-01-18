using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public List<Intervention> intervenciones = new List<Intervention>();

    public Dialogue Copy()
    {
        Dialogue aux = new Dialogue();
        Intervention[] copia = new Intervention[intervenciones.Count];
        this.intervenciones.CopyTo(copia);
        aux.intervenciones = new List<Intervention>(copia);

        return aux;
    }

}
