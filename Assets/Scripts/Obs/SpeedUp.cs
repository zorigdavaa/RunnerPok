using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour, ICollisionAction
{
    [SerializeField] Renderer TextureAnimrenderer;
    Material matwithTexture;
    Vector2 Offset = Vector3.zero;
    float speed = 2;
    public void CollisionAction(Character character)
    {
        if (character is Player player)
        {
            player.rb.AddForce(Vector3.forward * 1000);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        matwithTexture = TextureAnimrenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        Offset -= Vector2.up * speed * Time.deltaTime;
        matwithTexture.SetTextureOffset("_MainTex", Offset);
    }
}