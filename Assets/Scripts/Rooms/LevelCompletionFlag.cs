using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletionFlag : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            uiManager.LevelCompleted();
    }
}
