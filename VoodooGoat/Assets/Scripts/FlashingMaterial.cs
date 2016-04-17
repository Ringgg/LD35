using UnityEngine;
using System.Collections;

public class FlashingMaterial : MonoBehaviour
{
    public float frequency;
    public float progress;
    
    bool _active;
    public bool active
    {
        get { return _active; }
        set
        {
            if (!active && value)
            {
                StopAllCoroutines();
                StartCoroutine("Appear");
            }
            else if (active && !value)
            {
                StopAllCoroutines();
                StartCoroutine("Disappear");
            }
            _active = value;
        }
    }

    [SerializeField] Material start;
    [SerializeField] Material finish;
    Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        active = true;
    }

    IEnumerator Appear()
    {
        while (progress != 1.0f)
        {
            progress = Mathf.Clamp(progress + Time.deltaTime * frequency, 0.0f, 1.0f);
            UpdateMat();
            yield return null;
        }
        if (active)
            StartCoroutine("Disappear");
        yield break;
    }


    IEnumerator Disappear()
    {
        while(progress != 0.0f)
        {
            progress = Mathf.Clamp(progress - Time.deltaTime * frequency, 0.0f, 1.0f);
            UpdateMat();
            yield return null;
        }
        if (active)
            StartCoroutine("Appear");
        else
            Destroy(gameObject);
        yield break;
    }

    void UpdateMat()
    {
        mat.Lerp(start, finish, progress);
    }
}
