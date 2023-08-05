using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ColorPicker : MonoBehaviour
{
    [Header("Variable")]
    public Color color; //색상
    public GameObject satvalGO;
    public GameObject satvalKnob;
    public GameObject hueGO;
    public GameObject hueKnob;
    public UnityAction<Color> applyColor; //색상을 적용하는 함수
    public UnityAction dragAction; //Color Picker를 드래그하면 실행되는 함수

    [Header("Cache")]
    private UnityAction update;
    private UnityAction dragH;
    private UnityAction dragSV;

    private void Awake()
    {
        SetUp();
    }

    private void SetUp()
    {
        /* 텍스쳐 생성 */
        Color[] hueColors = new Color[] {
            Color.red,
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            Color.magenta,
        };

        Color[] satvalColors = new Color[] {
            new Color( 0f, 0f, 0f ),
            new Color( 0f, 0f, 0f ),
            new Color( 1f, 1f, 1f ),
            hueColors[0],
        };

        var hueTex = new Texture2D(1, 7);
        for (int i = 0; i < 7; i++)
        {
            hueTex.SetPixel(0, i, hueColors[i % 6]);
        }
        hueTex.Apply();
        hueGO.GetComponent<Image>().sprite = Sprite.Create(hueTex, new Rect(0, 0.5f, 1, 6), new Vector2(0.5f, 0.5f));
        var hueSz = GetWidgetSize(hueGO);

        var satvalTex = new Texture2D(2, 2);
        satvalGO.GetComponent<Image>().sprite = Sprite.Create(satvalTex, new Rect(0.5f, 0.5f, 1, 1), new Vector2(0.5f, 0.5f));

        void resetSatValTexture()
        {
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 2; i++)
                {
                    satvalTex.SetPixel(i, j, satvalColors[i + j * 2]);
                }
            }
            satvalTex.Apply();
        }

        /* RGB 값을 HSV로 변경하여 ColorPicker 초기화 */
        var satvalSz = GetWidgetSize(satvalGO);

        RGBToHSV(color, out float Hue, out float Saturation, out float Value);

        void applyHue()
        {
            var i0 = Mathf.Clamp((int)Hue, 0, 5);
            var i1 = (i0 + 1) % 6;
            var resultColor = Color.Lerp(hueColors[i0], hueColors[i1], Hue - i0);
            satvalColors[3] = resultColor;
            resetSatValTexture();

            hueKnob.transform.localPosition = new Vector2(hueKnob.transform.localPosition.x, Hue / 6 * satvalSz.y);
        }

        void applySaturationValue()
        {
            var sv = new Vector2(Saturation, Value);
            var isv = new Vector2(1 - sv.x, 1 - sv.y);
            var c0 = isv.x * isv.y * satvalColors[0];
            var c1 = sv.x * isv.y * satvalColors[1];
            var c2 = isv.x * sv.y * satvalColors[2];
            var c3 = sv.x * sv.y * satvalColors[3];
            var resultColor = c0 + c1 + c2 + c3;
            satvalKnob.transform.localPosition = new Vector2(Saturation * satvalSz.x, Value * satvalSz.y);

            color = new Color(resultColor.r, resultColor.g, resultColor.b, color.a); //색상 갱신
        }

        applyHue();
        applySaturationValue();

        /* 커스텀 함수 */
        dragH = () =>
        {
            GetLocalMouse(hueGO, out Vector2 mp);
            Hue = mp.y / hueSz.y * 6;
            applyHue();
            applySaturationValue();

            dragAction?.Invoke(); //Drag Action 실행
        };

        dragSV = () =>
        {
            GetLocalMouse(satvalGO, out Vector2 mp);
            Saturation = mp.x / satvalSz.x;
            Value = mp.y / satvalSz.y;
            applySaturationValue();

            dragAction?.Invoke(); //Drag Action 실행
        };

        applyColor = (Color color) => //색상을 적용하는 함수
        {
            this.color = color;
            RGBToHSV(color, out Hue, out Saturation, out Value);
            applyHue();
            applySaturationValue();
        };
    }

    private void Update()
    {
        update?.Invoke();
    }

    /* 이미지에서 포인터를 눌렀을 때 실행되는 함수 */
    public void OnPointerDown()
    {
        if (GetLocalMouse(hueGO, out _))
        {
            update = dragH;
        }
        else if (GetLocalMouse(satvalGO, out _))
        {
            update = dragSV;
        }
    }

    /* 이미지에서 포인터를 땠을 때 실행되는 함수 */
    public void OnPointerUp()
    {
        update = null;
    }

    private void RGBToHSV(Color color, out float h, out float s, out float v)
    {
        var cmin = Mathf.Min(color.r, color.g, color.b);
        var cmax = Mathf.Max(color.r, color.g, color.b);
        var d = cmax - cmin;
        if (d == 0)
        {
            h = 0;
        }
        else if (cmax == color.r)
        {
            h = Mathf.Repeat((color.g - color.b) / d, 6);
        }
        else if (cmax == color.g)
        {
            h = (color.b - color.r) / d + 2;
        }
        else
        {
            h = (color.r - color.g) / d + 4;
        }
        s = cmax == 0 ? 0 : d / cmax;
        v = cmax;
    }

    /* 마우스가 오브젝트 위에 올려져 있는지 확인하는 함수 */
    private bool GetLocalMouse(GameObject go, out Vector2 result)
    {
        var rt = (RectTransform)go.transform;
        var mp = rt.InverseTransformPoint(Input.mousePosition);
        result.x = Mathf.Clamp(mp.x, rt.rect.min.x, rt.rect.max.x);
        result.y = Mathf.Clamp(mp.y, rt.rect.min.y, rt.rect.max.y);
        return rt.rect.Contains(mp);
    }

    private Vector2 GetWidgetSize(GameObject go)
    {
        var rt = (RectTransform)go.transform;
        return rt.rect.size;
    }
}