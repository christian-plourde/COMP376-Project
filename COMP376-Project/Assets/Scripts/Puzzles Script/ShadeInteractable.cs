using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeInteractable : MonoBehaviour
{
    Player player;
    Shader normal_shader;
    Shader outline_shader;
    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        renderer = GetComponent<Renderer>();
        normal_shader = Shader.Find("Standard");
        outline_shader = Shader.Find("Outlined/Custom");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.usingIron || player.usingSteel)
        {
            foreach(Material m in renderer.materials)
                m.shader = outline_shader;
        }
            
        else
            foreach (Material m in renderer.materials)
                m.shader = normal_shader;
    }
}
