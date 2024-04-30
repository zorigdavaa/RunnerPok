using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour, ICollisionAction
{
    [SerializeField] List<Renderer> TextureAnimrenderer;
    Material[] matwithTextures;
    Vector2 Offset = Vector3.zero;
    float speed = 2;
    public void CollisionAction(Character character)
    {
        if (character is Player player)
        {
            // player.rb.AddForce(Vector3.forward * 1000);
            player.rb.velocity = Vector3.forward * 50;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        matwithTextures = new Material[TextureAnimrenderer.Count];
        for (int i = 0; i < TextureAnimrenderer.Count; i++)
        {
            matwithTextures[i] = TextureAnimrenderer[i].material;
        }
        // foreach (var item in TextureAnimrenderer)
        // {

        //     matwithTexture = TextureAnimrenderer[i].material;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        Offset -= Vector2.up * speed * Time.deltaTime;
        foreach (var item in matwithTextures)
        {
            // matwithTextures[i].SetTextureOffset("_MainTex", Offset);
            item.SetTextureOffset("_MainTex", Offset);
        }
    }
}
