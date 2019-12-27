using UnityEngine;

public class SaveDataObject
{
    public float time;
    public float end_effector_x;
    public float end_effector_y;
    public float end_effector_z;
    public float collision_x;
    public float collision_y;
    public float collision_z;
    public float pos1_x;
    public float pos1_y;
    public float pos1_z;
    public float pos2_x;
    public float pos2_y;
    public float pos2_z;
    public float pos3_x;
    public float pos3_y;
    public float pos3_z;
    public float pos4_x;
    public float pos4_y;
    public float pos4_z;
    public float pos5_x;
    public float pos5_y;
    public float pos5_z;
    public float pos6_x;
    public float pos6_y;
    public float pos6_z;



    public SaveDataObject()
    {
    }

    public SaveDataObject(float time, Vector3 end_effector, Vector3 collision, Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, Vector3 pos5, Vector3 pos6 )
    {
        this.time = time;
        this.end_effector_x = end_effector.x;
        this.end_effector_y = end_effector.y;
        this.end_effector_z = end_effector.z;
        this.collision_x = collision.x;
        this.collision_y = collision.y;
        this.collision_z = collision.z;
        this.pos1_x = pos1.x;
        this.pos1_y = pos1.y;
        this.pos1_z = pos1.z;
        this.pos2_x = pos2.x;
        this.pos2_y = pos2.y;
        this.pos2_z = pos2.z;
        this.pos3_x = pos3.x;
        this.pos3_y = pos3.y;
        this.pos3_z = pos3.z;
        this.pos4_x = pos4.x;
        this.pos4_y = pos4.y;
        this.pos4_z = pos4.z;
        this.pos5_x = pos5.x;
        this.pos5_y = pos5.y;
        this.pos5_z = pos5.z;
        this.pos6_x = pos6.x;
        this.pos6_y = pos6.y;
        this.pos6_z = pos6.z;
    }
}