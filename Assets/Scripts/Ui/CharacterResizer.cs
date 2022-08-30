using System.Collections;
using TMPro;
using UnityEngine;

public class CharacterResizer : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _startCharSize;

    private Coroutine _coroutine;
    private const int _baseCharSize = 1;
    private float _stepChangeCharSize = 0.1f;
    private const int _countVerticesInChar = 4;
    private float _delay = 0.005f;
    private int _leftBottomIndex = 0;
    private int _rightTopIndex = 2;
    private int _half = 2;
    private char _emptyChar = ' ';
    private int _zeroIndex;

    private void OnValidate()
    {
        if(_text == null)
            throw new System.Exception($"Не назначен изменяемый текст на объекте {gameObject}");
        if(_startCharSize <= 0)
            _startCharSize = 2;
    }

    public void ResizeCharacter(int index)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(OnResizeCharacter(index));
    }

    private IEnumerator OnResizeCharacter(int index)
    {
        var waitingTime = new WaitForSeconds(_delay);

        _text.ForceMeshUpdate();

        TMP_TextInfo textInfo = _text.textInfo;
        TMP_MeshInfo[] cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();
        cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();
        int materialIndex = textInfo.characterInfo[index].materialReferenceIndex;
        int vertexIndex = textInfo.characterInfo[index].vertexIndex;
        Vector3[] sourceVertices = cachedMeshInfoVertexData[materialIndex].vertices;
        Vector3 offset = (sourceVertices[vertexIndex + _leftBottomIndex] + sourceVertices[vertexIndex + _rightTopIndex]) / _half;
        Matrix4x4 matrix;
        float charSize = _startCharSize;

        while ((charSize != _baseCharSize) && (_text.text[index] != _emptyChar))
        {
            charSize = Mathf.MoveTowards(charSize, _baseCharSize, _stepChangeCharSize);

            Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
            matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * charSize);

            for (int i = 0; i < _countVerticesInChar; i++)
            {
                destinationVertices[vertexIndex + i] = sourceVertices[vertexIndex + i] - offset;
                destinationVertices[vertexIndex + i] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + i]);
                destinationVertices[vertexIndex + i] += offset;
            }

            textInfo.meshInfo[_zeroIndex].mesh.vertices = textInfo.meshInfo[_zeroIndex].vertices;
            _text.UpdateGeometry(textInfo.meshInfo[_zeroIndex].mesh, _zeroIndex);


            yield return waitingTime;
        }
    }
}
