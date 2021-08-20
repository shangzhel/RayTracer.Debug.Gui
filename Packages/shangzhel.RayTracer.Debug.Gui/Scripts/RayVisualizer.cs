using System.Text;
using UnityEngine;

namespace shangzhel.RayTracer.Debug.Gui
{
    public class RayVisualizer : MonoBehaviour
    {
        [SerializeField]
        private RaysAsset asset;

        [SerializeField]
        private GameObject rayPrefab;

        private int itemsPerPage = 10;
        private int page = 0;
        private int? selected;
        private int? selectedPrev;
        private Transform ray;
        private float countdown;

        private const float c_countdown = 0.5f;

        private void Start()
        {
            ray = Instantiate(rayPrefab).transform;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                page -= 1;
                countdown = c_countdown;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                page += 1;
                countdown = c_countdown;
            }

            if (countdown < 0)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                    page -= 1;

                if (Input.GetKey(KeyCode.RightArrow))
                    page += 1;
            }

            var pageMin = 0;
            var pageMax = Mathf.FloorToInt((float)asset.hits.Length / itemsPerPage);
            page = Mathf.Clamp(page, pageMin - 1, pageMax - 1);

            if (!(selected is null))
            {

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    selected += 1;
                    countdown = c_countdown;
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    selected -= 1;
                    countdown = c_countdown;
                }

                if (countdown <= 0)
                {
                    if (Input.GetKey(KeyCode.DownArrow))
                        selected += 1;

                    if (Input.GetKey(KeyCode.UpArrow))
                        selected -= 1;
                }

                selected = Mathf.Clamp(selected.Value, 0, asset.hits.Length);
            }

            if (selected != selectedPrev)
            {
                if (selected is null)
                    ray.gameObject.SetActive(false);
                else
                {
                    ray.gameObject.SetActive(true);
                    var hit = asset.hits[selected.Value];
                    var path = hit.to - hit.from;
                    ray.localScale = new Vector3(1, 1, path.magnitude);
                    ray.localRotation = Quaternion.FromToRotation(Vector3.forward, path);
                    ray.localPosition = hit.from;
                }
            }

            selectedPrev = selected;

            if (countdown > 0)
                countdown -= Time.unscaledDeltaTime;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));

            GUILayout.BeginHorizontal();
            GUILayout.Label("Items per page");
            GUILayout.Label($"{itemsPerPage}");
            GUILayout.EndHorizontal();

            itemsPerPage = Mathf.FloorToInt(GUILayout.HorizontalSlider(itemsPerPage, 10, 50));

            GUILayout.BeginHorizontal();
            GUILayout.Label("Page");
            GUILayout.Label($"{page + 1}");
            GUILayout.EndHorizontal();

            var pageMin = 1;
            var pageMax = Mathf.CeilToInt((float)asset.hits.Length / itemsPerPage);
            page = Mathf.FloorToInt(GUILayout.HorizontalSlider(page + 1, pageMin, pageMax)) - 1;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("<"))
                --page;
            if (GUILayout.Button(">"))
                ++page;
            GUILayout.EndHorizontal();

            var hits = asset.hits;
            GUILayout.Box(selected is null ? "None selected" : IdToString(hits[selected.Value].id));

            var offset = page * itemsPerPage;
            for (int i = 0; i < itemsPerPage; ++i)
            {
                var idx = offset + i;
                if (idx >= hits.Length)
                    break;

                var id = hits[idx].id;
                var value = selected.HasValue && selected.Value == idx;
                if (GUILayout.Toggle(value, IdToString(id)))
                {
                    if (!value)
                        selected = idx;
                }
                else
                {
                    if (value)
                        selected = null;
                }
            }

            GUILayout.EndArea();

            page = Mathf.Clamp(page, pageMin - 1, pageMax - 1);
            if (!(selected is null))
                selected = Mathf.Clamp(selected.Value, 0, asset.hits.Length);
        }

        private static string IdToString(int[] id)
        {
            var sb = new StringBuilder();
            sb.Append('[');
            for (int i = 0; i < id.Length; ++i)
            {
                sb.Append(id[i]);
                sb.Append(',');
            }
            if (sb[sb.Length - 1] is ',')
                sb[sb.Length - 1] = ']';
            return sb.ToString();
        }
    }
}
