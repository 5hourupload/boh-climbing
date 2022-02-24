using System.Collections;
using System.Collections.Generic;
using HandPoseGen.Models.Interaction;
using HandPoseGen.ScriptableObjects;
using HandPoseGen.Views.Handlers;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CalculateClosest : MonoBehaviour
{

    private Color color;
    private GameObject[] climbPoint;
    private GameObject closest; 
    private Transform closestParent; 
    private Transform handModel;
    private HandPoseGen.Controllers.Interaction.PosePointController newPosePoint;

    private Transform Master_L_Mesh;
    private Transform Ghost_L_Mesh;
    private Transform Master_R_Mesh;
    private Transform Ghost_R_Mesh;

    void Awake(){

        if (this.name == "Master.L"){
            Master_L_Mesh = this.transform.Find("l_handMeshNode");
            Ghost_L_Mesh = this.transform.parent.Find("Ghost.L").transform.Find("l_handMeshNode");
        } 

        if (this.name == "Master.R"){
            Master_R_Mesh = this.transform.Find("r_handMeshNode");
            Ghost_R_Mesh = this.transform.parent.Find("Ghost.R").transform.Find("r_handMeshNode");
            Debug.Log(Ghost_R_Mesh);
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (this.name == "Master.L"){
            ChangeColor(Ghost_L_Mesh, 0);
        }

        if (this.name == "Master.R"){
            ChangeColor(Ghost_R_Mesh, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        climbPoint = GameObject.FindGameObjectsWithTag("ClimbPoint");  
        closest = GetClosestPoint(climbPoint);
        closestParent = closest.transform.parent;

        if (closestParent.Find(closest.name+".Snap") != null){
            newPosePoint = closestParent.Find(closest.name+".Snap").GetComponent<HandPoseGen.Controllers.Interaction.PosePointController>();

            if (this.name == "Master.L"){
                handModel = this.transform.parent.gameObject.transform.Find("Model.L");
                handModel.GetComponent<HandPoseGen.Models.Avatar.PosableHandModel>().posePoint = newPosePoint;
                
                float L_Dist = Vector3.Distance(this.transform.position, closest.transform.position);
                
                if (L_Dist > 0.25){
                    ChangeColor(Master_L_Mesh, 1);
                    ChangeColor(Ghost_L_Mesh, 0);
                }
            }

            if (this.name == "Master.R"){
                handModel = this.transform.parent.gameObject.transform.Find("Model.R");
                handModel.GetComponent<HandPoseGen.Models.Avatar.PosableHandModel>().posePoint = newPosePoint;

                float R_Dist = Vector3.Distance(this.transform.position, closest.transform.position);
                
                if (R_Dist > 0.25){
                    ChangeColor(Master_R_Mesh, 1);
                    ChangeColor(Ghost_R_Mesh, 0);
                }
            }
        }
        
    }

    private void ChangeColor(Transform mesh, float alpha){
        color = mesh.GetComponent<SkinnedMeshRenderer>().material.color;
        color.a = alpha;
        mesh.GetComponent<SkinnedMeshRenderer>().material.color = color;
    }

    GameObject GetClosestPoint(GameObject[] climbPoint){
        // Transform tMin = null;
        float minDist = Mathf.Infinity;
        GameObject output = null; 

        Vector3 currentPos = transform.position;
        foreach (GameObject t in climbPoint)
        {
            float dist = Vector3.Distance(t.GetComponent<Transform>().position, currentPos);
            if (dist < minDist)
            {
                minDist = dist;
                output = t;
            }
        }

        return output;
    }

    private void OnTriggerEnter(Collider other){
        if (closest!= null && other.name == closest.name){
    
            if (this.name == "Master.L"){
                ChangeColor(Master_L_Mesh, 0);
                ChangeColor(Ghost_L_Mesh, 1);
            }

            if (this.name == "Master.R"){
                ChangeColor(Master_R_Mesh, 0);
                ChangeColor(Ghost_R_Mesh, 1);
            }

        }
        
    } 

}
