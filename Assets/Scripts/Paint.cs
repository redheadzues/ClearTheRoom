using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{
    [SerializeField] private Texture2D _texture;

    private void Start()
    {
        var copyTexture = new Texture2D(_texture.width, _texture.height);

        for (int x = 0; x < _texture.width / 2; x++)
        {
            for (int y = 0; y < _texture.height; y++)
            {
                var color = _texture.GetPixel(x, y);
                copyTexture.SetPixel(x, y, color);
            }
        }
        _texture.Apply();
        GetComponent<Renderer>().material.mainTexture = copyTexture;

        _texture.filterMode = FilterMode.Point;



        for (int x = 0; x < copyTexture.width / 2; x++)
        {
            for (int y = 0; y < copyTexture.height; y++)
            {
                copyTexture.SetPixel(x, y, Color.red);
            }

        }

        _texture.Apply();
    }
}
