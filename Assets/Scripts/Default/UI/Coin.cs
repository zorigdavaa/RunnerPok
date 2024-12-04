using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class Coin : MonoBehaviour
{
    public int Amount;
    float rotateSpeed;
    Transform rotChild;
    Camera cam;
    bool shouldRot = true;
    private void Start()
    {
        cam = Camera.main;
        rotChild = transform.GetChild(0);
        // rotateSpeed = Settings.Instance.CoinRotationSpeed;
        rotateSpeed = 45;
        // GotoPosAndAdd();
    }
    // Update is called once per frame
    void Update()
    {
        if (shouldRot)
        {
            rotChild.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.World);
        }
    }
    public void GotoPosAndAdd()
    {
        StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            shouldRot = false;
            transform.SetParent(null);
            float t = 0;
            float time = 0;
            float duration = 1f;
            // Vector3 initialPosition = transform.position;
            while (time < duration)
            {
                time += Time.deltaTime;
                t = time / duration;
                Vector3 toPos = cam.ScreenToWorldPoint(Z.CanM.Coin.transform.position + Vector3.forward * 10);
                transform.position = Vector3.Lerp(transform.position, toPos, t);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 0.3f, t);
                yield return null;
            }
            GameManager.Instance.Coin++;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // GameManager.Instance.Coin++;
            // Destroy(gameObject);
            GotoPosAndAdd();
        }
    }

}
