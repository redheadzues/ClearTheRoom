using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent (typeof(MeshRenderer))]

public class Clearer : MonoBehaviour
{
    [SerializeField] private int _circleSize;
    [SerializeField] private Texture2D _mainTexture;
    [SerializeField] private int _maskTextureResolution;
    [SerializeField] private RequiredThirdStageItem _stageItem;

    private Texture2D _maskTexture;
    private MeshCollider _collider;
    private MeshRenderer _renderer;

    public int TotalMaskPixels => _maskTextureResolution * _maskTextureResolution;
    public int TotalClearedPixels { get; private set; }

    private void Start()
    {
        _collider = GetComponent<MeshCollider>();
        _renderer = GetComponent<MeshRenderer>();

        _maskTexture = new Texture2D(_maskTextureResolution, _maskTextureResolution, TextureFormat.ARGB32, false);

        _renderer.material.SetTexture("_Mask", _maskTexture);
        _renderer.material.mainTexture = _mainTexture;
        _maskTexture.filterMode = FilterMode.Point;

        for (int x = 0; x < _maskTexture.width; x++)
        {
            for (int y = 0; y < _maskTexture.height; y++)
            {
                _maskTexture.SetPixel(x, y, Color.red);
            }
        }

        _maskTexture.Apply();
    }

    private void OnValidate()
    {
        if (_maskTextureResolution < 128)
            _maskTextureResolution = 512;
        if (_circleSize <= 0)
            _circleSize = 80;
        if (_mainTexture == null)
            throw new System.Exception($"Нет основной текстуры на объекте {gameObject}");
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if(_collider.Raycast(_stageItem.Ray, out hit, 100f))
            {
                int rayPointX = (int)(hit.textureCoord.x * _maskTexture.width);
                int rayPointY = (int)(hit.textureCoord.y * _maskTexture.height);

                DrawCircle(rayPointX, rayPointY);

                _maskTexture.Apply();
            }
        }
    }

    private void DrawCircle(int pointX, int pointY)
    {       
        for (int y = 0; y < _circleSize; y++)
        {
            for (int x = 0; x < _circleSize; x++)
            {
                float x2 = Mathf.Pow(x - _circleSize /2, 2);
                float y2 = Mathf.Pow(y - _circleSize / 2, 2);
                float r2 = Mathf.Pow(_circleSize /2, 2);

                if ((x2 + y2) < r2)
                {
                    int settingPixelX = pointX + x - _circleSize / 2;
                    int settingPixelY = pointY + y - _circleSize / 2;
                    if (_maskTexture.GetPixel(settingPixelX, settingPixelY) != Color.green)
                    {
                        _maskTexture.SetPixel(settingPixelX, settingPixelY, Color.green);
                        TotalClearedPixels++;
                    }                   
                }
            }
        }
    }
}
