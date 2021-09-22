using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public GameObject Trail;
    public Bird TargetBird; //burung yang akan diberi trails

    public float timeSpwanTrail = .1f;

    private List<GameObject> _trail;

    // Start is called before the first frame update
    void Start()
    {
        _trail = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBird(Bird bird)
    {
        TargetBird = bird;

        for(int i = 0; i < _trail.Count; i++)
        {
            Destroy(_trail[i].gameObject);
        }

        _trail.Clear();
    }
    
    public IEnumerator SpawnTrail()
    {
        _trail.Add(Instantiate(Trail, TargetBird.transform.position, Quaternion.identity));


        yield return new WaitForSeconds(timeSpwanTrail);

        if(TargetBird != null && TargetBird.State != Bird.BirdState.HitSomething)
        {
            StartCoroutine(SpawnTrail());
        }
    }
}
