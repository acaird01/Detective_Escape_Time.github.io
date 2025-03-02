using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_NonGimicChair : MonoBehaviour
{
    Interaction_Items interaction;

    private void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Items>();
        interaction.audioSource_Object = gameObject;
    }
}
