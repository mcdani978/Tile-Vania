using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int Int_pointsForCoinPickup = 100;

    bool Bol_wasCollected = false; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !Bol_wasCollected)
        {
            Bol_wasCollected = true;
            FindObjectOfType<GameSession>().AddToScore(Int_pointsForCoinPickup);
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
