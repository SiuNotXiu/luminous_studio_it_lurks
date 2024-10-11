using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight_fov_with_damage : MonoBehaviour
{
    //this script is used by the mask of flashlight

    #region sector drawing
    [HideInInspector] private Mesh mesh;
    [HideInInspector] private float fov = 90f;
    [HideInInspector] private Vector3 origin;
    [HideInInspector] private Vector3 player_position;
    [HideInInspector] private float angle;
    [HideInInspector] private LayerMask layer_that_detects_flashlight;
    #endregion

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        #region initialize variable
        origin = Vector3.zero;
        angle = 0f;
        layer_that_detects_flashlight = LayerMask.GetMask("monster", "wall");
        #endregion
    }

    private void LateUpdate()
    {
        //angle = 0f;//commented to allow the modification of flashlight rotation
        int ray_count = 100;
        float angle_increase = -fov / ray_count;
        float view_distance = 7f;

        Vector3[] vertices = new Vector3[ray_count + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[ray_count * 3];

        #region triangle mesh for example
        /*vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(50, 0, 0);
        vertices[2] = new Vector3(0, -50, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;*/
        #endregion

        vertices[0] = origin;

        int vertex_index = 1;
        int triangle_index = 0;
        for (int i = 0; i <= ray_count; i++)
        {
            #region origin to edge but cannot mask wall
            Vector3 vertex;
            RaycastHit2D raycast_hit_2d = Physics2D.Raycast(player_position, get_vector_from_angle(angle), view_distance, layer_that_detects_flashlight);

            if (raycast_hit_2d.collider != null)
            {
                if (raycast_hit_2d.collider.gameObject.GetComponent<light_blocking>() != null)
                {
                    //hit something, such as wall
                    //i realized that vertex writting is based on local position because origin remains but the shape can move
                    vertex = gameObject.transform.InverseTransformPoint(raycast_hit_2d.point);
                    //Debug.Log(i + ">" + raycast_hit_2d.collider.gameObject.name);

                    /*Debug.DrawLine(player_position,
                    raycast_hit_2d.point,
                    Color.green, 0.1f);*/
                }
                else
                {
                    //hit something, but don't block light
                    vertex = origin + get_vector_from_angle(angle) * view_distance;
                }

                if (raycast_hit_2d.collider.gameObject.name == "flashlight_trigger_area")
                {
                    raycast_hit_2d.collider.gameObject.transform.parent.gameObject.GetComponent<monster_database>().flashed = true;
                }
            }
            else
            {
                //hit nothing
                vertex = origin + get_vector_from_angle(angle) * view_distance;
            }
            #endregion
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
        //Debug.Log("origin > " + origin + " | vertices[0] > " + vertices[0]);
    }

    #region update variables called by other script
    public void set_origin(Vector3 origin_new, Vector3 player_position_new)
    {
        player_position = player_position_new;
    }

    public void set_aim_direction(Vector3 aim_direction)
    {
        //Debug.Log("aim_direction > " + aim_direction);
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
