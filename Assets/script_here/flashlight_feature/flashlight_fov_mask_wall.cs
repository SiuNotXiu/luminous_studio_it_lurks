using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class flashlight_fov_wall_mask : MonoBehaviour
{
    //this script is used by the mask of flashlight

    #region sector drawing
    [HideInInspector] private Mesh mesh;
    [HideInInspector] private float fov = 90f;
    [HideInInspector] private Vector3 origin;
    [HideInInspector] private Vector3 player_position;
    [HideInInspector] private float angle;
    [HideInInspector] private LayerMask layer_that_detects_flashlight;
    [HideInInspector] private GameObject camera_main;
    [HideInInspector] private float mask_z_local_position;
    #endregion

    private void Start()
    {
        #region initialize variable
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        //shape drawing
        camera_main = GameObject.Find("camera_main_dont_change_name");
        origin = Vector3.zero;
        origin.z = camera_main.transform.position.z - 1;
        angle = 0f;
        layer_that_detects_flashlight = LayerMask.GetMask("flashlight_monster_dont_change_name", "flashlight_wall_dont_change_name");

        #endregion
    }

    private void LateUpdate()
    {
        int ray_count = 10;
        float angle_increase = -fov / ray_count;
        float view_distance = 7f;

        Vector3[] vertices = new Vector3[ray_count + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[ray_count * 3];

        origin.z = mask_z_local_position;
        vertices[0] = origin;

        int vertex_index = 1;
        int triangle_index = 0;
        for (int i = 0; i <= ray_count; i++)
        {
            //edge to origin and mask wall
            Vector3 vertex;
            Vector3 end_of_sector = player_position + get_vector_from_angle(angle) * view_distance;
            #region reverse raycast
            float angle_reverse;
            if (angle > 180)
            {
                angle_reverse = angle - 180;
            }
            else
            {
                angle_reverse = angle + 180;
            }
            RaycastHit2D[] raycast_hit_2d = Physics2D.RaycastAll(end_of_sector, get_vector_from_angle(angle_reverse), view_distance, layer_that_detects_flashlight);
            #endregion

            //mesh rendering using local_position
            vertex = gameObject.transform.InverseTransformPoint(end_of_sector);
            if (raycast_hit_2d.Length != 0)
            {
                if (raycast_hit_2d[raycast_hit_2d.Length - 1].collider != null && raycast_hit_2d[raycast_hit_2d.Length - 1].collider.gameObject.GetComponent<light_blocking>() != null)
                {
                    //mesh rendering using local_position
                    vertex = gameObject.transform.InverseTransformPoint(raycast_hit_2d[raycast_hit_2d.Length - 1].point);
                    #region debug
                    //drawline using world position
                    /*Debug.DrawLine(end_of_sector,
                    raycast_hit_2d[raycast_hit_2d.Length - 1].point,
                    Color.green, 0.1f);*/

                    /*Debug.DrawLine(player_position,
                    raycast_hit_2d[raycast_hit_2d.Length - 1].point,
                    Color.red, 0.1f);*/

                    /*Debug.Log(raycast_hit_2d[raycast_hit_2d.Length - 1].collider.gameObject.name);*/
                    #endregion
                }
                #region monster detection
                for (int j = 0; j < raycast_hit_2d.Length; j++)
                {
                    Debug.Log(raycast_hit_2d[j].collider.gameObject.name);
                    if (raycast_hit_2d[j].collider.gameObject.name == "flashlight_trigger_area_dont_change_name")
                    {
                        raycast_hit_2d[j].collider.gameObject.transform.parent.gameObject.GetComponent<monster_database>().flashed = true;
                    }
                }
                #endregion
            }
            //ensure mask is between camera and
            //origin is 0 localposition
            //vertex.z is also localposition
            vertex.z = origin.z;

            vertices[vertex_index] = vertex;

            if (i > 0)
            {
                triangles[triangle_index + 0] = 0;
                triangles[triangle_index + 1] = vertex_index - 1;
                triangles[triangle_index + 2] = vertex_index;

                triangle_index += 3;
            }
            vertex_index++;
            //the tutorial is using -= but -= vertex arrangement is terbalik for me
            angle += angle_increase;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    #region update variables called by other script
    public void set_origin(Vector3 origin_new, Vector3 player_position_new)
    {
        player_position = player_position_new;
    }

    public void set_aim_direction(Vector3 aim_direction)
    {
        angle = get_angle_from_vector_float(aim_direction) + fov / 2.0f;
    }
    #endregion

    #region general functions
    Vector3 get_vector_from_angle(float angle_degree)
    {
        //angle range 0 -> 360
        float angle_radian = angle_degree * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angle_radian), Mathf.Sin(angle_radian));
    }
    float get_angle_from_vector_float(Vector3 direction)
    {
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }
    #endregion
}
