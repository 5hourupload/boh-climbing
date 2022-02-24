using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSwitch : MonoBehaviour
{
    private Color color;
    private GameObject Hand_L;
    private Transform Master_L_Mesh;
    private Transform Ghost_L_Mesh;


    private GameObject Hand_R;
    private Transform Master_R_Mesh;
    private Transform Ghost_R_Mesh;

    void Awake(){
        Hand_L = GameObject.Find("Hand.L");
        Master_L_Mesh = Hand_L.transform.Find("Master.L").transform.Find("l_handMeshNode");
        Ghost_L_Mesh = Hand_L.transform.Find("Ghost.L").transform.Find("l_handMeshNode");

        Hand_R = GameObject.Find("Hand.R");
        Master_R_Mesh = Hand_R.transform.Find("Master.R").transform.Find("r_handMeshNode");
        Ghost_R_Mesh = Hand_R.transform.Find("Ghost.R").transform.Find("r_handMeshNode");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Ghost_R_Mesh);
        ChangeColor(Ghost_L_Mesh, 0);
        ChangeColor(Ghost_R_Mesh, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float R_Dist = Vector3.Distance(this.transform.position, Master_R_Mesh.position);
        float L_Dist = Vector3.Distance(this.transform.position, Master_L_Mesh.position);
        
        if (R_Dist > 0.25){
            ChangeColor(Master_R_Mesh, 1);
            ChangeColor(Ghost_R_Mesh, 0);
        }

        if (L_Dist > 0.25){
            ChangeColor(Master_L_Mesh, 1);
            ChangeColor(Ghost_L_Mesh, 0);
        }
    }

    private void ChangeColor(Transform mesh, float alpha){
        color = mesh.GetComponent<SkinnedMeshRenderer>().material.color;
        color.a = alpha;
        mesh.GetComponent<SkinnedMeshRenderer>().material.color = color;
    }

    private void OnTriggerEnter(Collider other){
        Debug.Log(other.name);
        
        if (other.name != "Wall04_basemesh"){
            string Hand_Name = other.transform.parent.transform.parent.transform.parent.gameObject.name;
            Debug.Log(Hand_Name);

            if (Hand_Name == "Master.L"){
                ChangeColor(Master_L_Mesh, 0);
                ChangeColor(Ghost_L_Mesh, 1);
            }

            if (Hand_Name == "Master.R"){
                ChangeColor(Master_R_Mesh, 0);
                ChangeColor(Ghost_R_Mesh, 1);
            }

        }
    } 

}
