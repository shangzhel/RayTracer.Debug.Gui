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

        [SerializeField]
        [Range(1, 100)]
        private int rayPool = 20;

        private int itemsPerPage = 10;
        private int page = 0;
        private int? selected;
        private int? selectedPrev;
        private GameObject[] rays;
        private float countdown;
        private bool selectHierarchy = true;

        private const float c_countdown = 0.5f;

        private void Start()
        {
            rays = new GameObject[rayPool];
            for (int i = 0; i < rayPool; ++i)
            {
                rays[i] = Instantiate(rayPrefab, transform);
                rays[i].SetActive(false);
            }
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
                    SelectNext();
                    countdown = c_countdown;
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    SelectPrev();
                    countdown = c_countdown;
                }

                if (countdown <= 0)
                {
                    if (Input.GetKey(KeyCode.DownArrow))
                        SelectNext();

                    if (Input.GetKey(KeyCode.UpArrow))
                        SelectPrev();
                }

                selected = Mathf.Clamp(selected.Value, 0, asset.hits.Length);
            }

            if (selected != selectedPrev)
            {
                if (selected is null)
                {
                    for (int i = 0; i < rays.Length; ++i)
                    {
                        rays[i].SetActive(false);
                    }
                }
                else
                {
                    var hits = asset.hits;
                    var i = selected.Value;
                    int j = 0;
                    for (; j < rays.Length && i + j < hits.Length; ++j)
                    {
                        var hit = hits[i + j];

                        if (selectHierarchy && !IsChild(hits[i].id, hits[i + j].id))
                            break;

                        rays[j].SetActive(true);
                        var ray = rays[j].transform;
                        var path = hit.to - hit.from;
                        ray.localScale = new Vector3(1, 1, path.magnitude);
                        ray.localRotation = Quaternion.FromToRotation(Vector3.forward, path);
                        ray.localPosition = hit.from;

                        if (!selectHierarchy)
                        {
                            ++j;
                            break;
                        }
                    }

                    for (; j < rays.Length && rays[j].activeSelf; ++j)
                    {
                        rays[j].SetActive(false);
                    }
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
            GUILayout.Box(selected is null ? "None selected" : IdToString(hits[selected.Value].id, false));

            selectHierarchy = GUILayout.Toggle(selectHierarchy, "Select hierarchy");

            var offset = page * itemsPerPage;
            for (int i = 0; i < itemsPerPage; ++i)
            {
                var idx = offset + i;
                if (idx >= hits.Length)
                    break;

                var id = hits[idx].id;
                var value = selected.HasValue && selected.Value == idx;
                var showChecked = selectHierarchy && selected.HasValue && selected.Value != idx && IsChild(hits[selected.Value].id, id) && i < rays.Length;

                if (GUILayout.Toggle(value, IdToString(id, showChecked)))
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

        private void SelectPrev()
        {
            UnityEngine.Debug.Assert(selected.HasValue, "selected.HasValue");

            if (!selectHierarchy)
            {
                --selected;
                return;
            }

            var hits = asset.hits;
            var j = selected.Value;
            var i = j - 1;
            for (; i > 0 && hits[i].id.Length > hits[j].id.Length; --i) { }

            if (i < 0)
                i = 0;
            selected = i;
        }

        private void SelectNext()
        {
            UnityEngine.Debug.Assert(selected.HasValue, "selected.HasValue");

            if (!selectHierarchy)
            {
                ++selected;
                return;
            }

            var hits = asset.hits;
            var j = selected.Value;
            var i = j + 1;
            for (; i < hits.Length && IsChild(hits[j].id, hits[i].id); ++i) { }

            if (i >= hits.Length)
                return;
            selected = i;
        }

        private static string IdToString(int[] id, bool showChecked)
        {
            var sb = new StringBuilder();
            if (showChecked)
                sb.Append('*');
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

        private static bool IsChild(int[] pid, int[] cid)
        {
            for (int i = 0; i < pid.Length; ++i)
            {
                if (i >= cid.Length)
                    return false;

                if (pid[i] != cid[i])
                    return false;
            }

            return true;
        }
    }
}
