using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class toneRowScript : MonoBehaviour
{
    [SerializeField] private AudioClip sn;
    [SerializeField] private int Bumps;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            StartCoroutine(plsa());

        }
    }


    public IOrderedEnumerable<float> generateRow(int bumps)
    {
        List<float> NthToneRow = new List<float>();
        float n;

        for (float i = 0; i < bumps; i++)
        {
            if (Random.Range(0, 2) > 0)
            {
                n = i;
            }
            else
            {
                n = -i;
            }

            float root12of2 = Mathf.Pow(2f, 1f / bumps);
            Debug.Log(root12of2);

            float ratio = Mathf.Pow(root12of2, n);

            //Debug.Log(ratio);
            NthToneRow.Add(ratio);
        }
        var rnd = new System.Random();
        var Randomized = NthToneRow.OrderBy(item => rnd.Next());
        return Randomized;

    }

    private IEnumerator plsa()
    {
        var ls = generateRow(Bumps);

        for (int i = 0; i < ls.Count(); i++)
        {
            AudioManager.instance.playSoundDesc(sn, 1, ls.ToArray()[i]);
            Debug.Log(i.ToString() + ": " + ls.ToArray()[i]);
            yield return new WaitForSeconds(0.9f);
        }
    }

}
