using System.Collections.Generic;
using UnityEngine;

public class DetectionController : MonoBehaviour
{
    public string tagTargetDetection = "Player";
    public List<Collider2D> detectedObjs = new List<Collider2D>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == tagTargetDetection)
        {
            detectedObjs.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == tagTargetDetection)
        {
            detectedObjs.Remove(collision);
        }
    }
}
