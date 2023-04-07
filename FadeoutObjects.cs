using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeoutObjects : MonoBehaviour
{
    public GameObject player; // Player object to follow
    private float defaultAlpha = 1f; // Default alpha of objects to be hidden
    private float hiddenAlpha = 0.2f; // Alpha to set hidden objects to

    private void Update()
    {
        // Check if there are any objects blocking the view of the player
        RaycastHit[] hits = Physics.RaycastAll(transform.position, (player.transform.position - transform.position).normalized, Vector3.Distance(transform.position, player.transform.position), LayerMask.GetMask("BGobject"));
        foreach (RaycastHit hit in hits)
        {
            // Check if the object has a renderer component
            Debug.Log("니가가림" +hit.transform.gameObject.name);
            Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Change the alpha value of the material
                Color color = renderer.material.color;
                color.a = hiddenAlpha;
                renderer.material.color = color;
            }
        }
    }

    private void LateUpdate()
    {
        // Restore the alpha value of any previously hidden objects
        RaycastHit[] hits = Physics.RaycastAll(transform.position, (player.transform.position - transform.position).normalized, Vector3.Distance(transform.position, player.transform.position), LayerMask.GetMask("BGobject"));
        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color color = renderer.material.color;
                color.a = defaultAlpha;
                renderer.material.color = color;
            }
        }
    }
}