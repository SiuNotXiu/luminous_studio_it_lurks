using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class flashlight_fov_wall_mask : MonoBehaviour
{
    //this script is used by flashlight_mask under player

    #region sector drawing
    [HideInInspector] public static bool this_frame_calculated_shape = false;
    [HideInInspector] public static Mesh mesh;
    [HideInInspector] public static float fov = 90f;
    [HideInInspector] public static int ray_count;
    [HideInInspector] public static float angle_increase;
    [HideInInspector] public static float view_distance_initial = 7f;
    [HideInInspector] public static float view_distance = 7f;//default value follow initial
    [HideInInspector] private Vector3 origin;
    [HideInInspector] public static Vector3 player_position;
    [HideInInspector] public static float angle;
    [HideInInspector] private LayerMask layer_that_detects_flashlight;
    [HideInInspector] private GameObject object_camera_main;
    [HideInInspector] private float mask_z_local_position;
    #endregion

    [HideInInspector] private flashlight_battery_blink script_flashlight_battery_blink;

    private void OnValidate()
    {
        #region set z depth
        object_camera_main = GameObject.Find("camera_main_dont_change_name");
        if (object_camera_main != null)
        {
            //z + 1 is used for black circle
            transform.position = new Vector3(transform.position.x,
            transform.position.y,
            object_camera_main.transform.position.z + 1);
        }
        else
        {
            Debug.Log("(can ignore) object_camera_main is null, cannot move the z depth of flashlight");
        }
        #endregion
    }

    private void Start()
    {
        #region initialize variable
        mesh = new Mesh();
        view_distance = view_distance_initial;
        //shape drawing
        origin = Vector3.zero;
        angle = 0f;
        layer_that_detects_flashlight = LayerMask.GetMask("flashlight_monster_dont_change_name",
            "flashlight_wall_dont_change_name",
            "flashlight_bubble");
        #endregion

        //monster damage don't participate the visuals
        script_flashlight_battery_blink = gameObject.GetComponent<flashlight_battery_blink>();
    }

    private void LateUpdate()
    {
        if (player_database.is_flashlight_on == false)
        {
            //manually closed
            //Debug.Log("1 > " + gameObject.name + " is off");
            GetComponent<MeshFilter>().mesh = null;
        }
        if (gameObject.name == "flashlight_mask" && flashlight_battery_blink.flashlight_during_blink == true)
        {
            //visually closed
            //priority for close, close can cover open
            //flashlight_got_monster_damage shouldnt close the flash
            //Debug.Log("2 > " + gameObject.name + " is off");
            GetComponent<MeshFilter>().mesh = null;
        }
        else if ((gameObject.name == "flashlight_mask" && flashlight_battery_blink.flashlight_during_blink == false && player_database.is_flashlight_on == true) ||
                  player_database.is_flashlight_on == true)
        {
            //manually and visually opened
            //first row for flashlight_mask , second row for mask
            //Debug.Log("3 > " + gameObject.name + " is on");
            GetComponent<MeshFilter>().mesh = mesh;
        }
        if (this_frame_calculated_shape == false)
        {
            this_frame_calculated_shape = true;
            if (player_database.is_flashlight_on == true)
            {
                //if fov changed, the calculation will follow up
                ray_count = (int)fov * 2;
                angle_increase = -fov / (float)ray_count;//gap between each angle

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
                    end_of_sector.z = origin.z;
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
                    RaycastHit2D[] raycast_hit_2d = Physics2D.RaycastAll(end_of_sector, get_vector_from_angle(angle_reverse), view_distance, layer_that_detects_flashlight);//from the arc to the center
                    #endregion
                    //mesh rendering using local_position
                    vertex = gameObject.transform.InverseTransformPoint(end_of_sector);
                    if (raycast_hit_2d.Length != 0)//if this raycast collided something
                    {
                        //check last first, if last is wall (light blocking)
                        #region check collided wall or not for vertex first
                        List<GameObject> monster_flashed = new List<GameObject>();
                        List<GameObject> bubble_flashed = new List<GameObject>();
                        //for (int j = raycast_hit_2d.Length - 1; j >= 0; j--)
                        for (int j = 0; j < raycast_hit_2d.Length; j++)//it works
                        {
                            //both flashlight check the same thing, but only one uses
                            //as long as collided wall, monster and bubble shouldnt detect flash
                            if (raycast_hit_2d[j].collider.gameObject.name == "flashlight_trigger_area_dont_change_name")
                            {
                                //monster found
                                monster_flashed.Add(raycast_hit_2d[j].collider.gameObject.transform.parent.gameObject);
                            }
                            if (raycast_hit_2d[j].collider.gameObject.GetComponent<flashlight_dissolve_bubble>() != null)
                            {
                                //bubble found
                                bubble_flashed.Add(raycast_hit_2d[j].collider.gameObject);
                            }
                            if (raycast_hit_2d[j].collider.gameObject.GetComponent<light_blocking>() != null)
                            {
                                //wall found
                                //mesh rendering using local_position
                                vertex = gameObject.transform.InverseTransformPoint(raycast_hit_2d[j].point);

                                //if wall found, monster behind wall shouldn't get flashed
                                monster_flashed.Clear();
                                bubble_flashed.Clear();
                                #region debug
                                //drawline using world position
                                /*Debug.DrawLine(end_of_sector,
                                raycast_hit_2d[raycast_hit_2d.Length - 1].point,
                                Color.green, 0.1f);*/

                                /*Debug.DrawLine(player_position,
                                raycast_hit_2d[j].point,
                                Color.red, 0.1f);*/

                                /*Debug.Log(raycast_hit_2d[raycast_hit_2d.Length - 1].collider.gameObject.name);*/
                                #endregion
                            }
                        }
                        #endregion
                        #region which monster flashed confirmed, bubble alpha modification
                        if (gameObject.name == "flashlight_got_monster_damage")
                        {
                            for (int j = 0; j < monster_flashed.Count; j++)
                            {
                                monster_flashed[j].GetComponent<monster_database>().flashed = true;
                            }
                        }
                        else if (gameObject.name == "flashlight_mask")
                        {
                            for (int j = 0; j < bubble_flashed.Count; j++)//everything being hit by the raycast
                            {
                                bubble_flashed[j].GetComponent<flashlight_dissolve_bubble>().dissolve_bubble();
                                #region debug
                                //Debug.Log("bubble count > " + bubble_flashed.Count);
                                /*Debug.DrawLine(player_position,
                                                bubble_flashed[j].transform.position,
                                                Color.red, 0.1f);*/
                                #endregion
                            }
                        }
                        #endregion
                    }
                    //ensure mask is between camera and
                    //origin is 0 localposition
                    //vertex.z is also localposition
                    vertex.z = origin.z;
                    /*Debug.DrawLine(player_position,
                                    new Vector3(vertex.x,
                                                vertex.y,
                                                player_position.z),
                                    Color.red, 0.1f);*/

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
                //Debug.Log("mesh > " + mesh);
                mesh.vertices = vertices;
                mesh.uv = uv;
                mesh.triangles = triangles;
            }
        }
        StartCoroutine(this_frame_calculated_shape_coroutine());
        /*else
        {
            //this only works if there's only 2 flashlight
            this_frame_calculated_shape = false;
        }*/
    }

    #region update variables called by other script
    public void set_origin(Vector3 player_position_new)
    {
        player_position = player_position_new;
    }

    public void set_aim_direction(Vector3 aim_direction)
    {
        angle = get_angle_from_vector_float(aim_direction) + fov / 2.0f;
    }
    #endregion

    IEnumerator this_frame_calculated_shape_coroutine()
    {
        yield return null;
        this_frame_calculated_shape = false;
    }

    #region general functions
    Vector3 get_vector_from_angle(float angle_degree)
    {
        //angle range 0 -> 360
        float angle_radian = angle_degree * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angle_radian), Mathf.Sin(angle_radian));
    }
    public static float get_angle_from_vector_float(Vector3 direction)
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
