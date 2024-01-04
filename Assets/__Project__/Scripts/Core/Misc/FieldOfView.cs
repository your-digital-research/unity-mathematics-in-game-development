using Core.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Core.Misc
{
    public class FieldOfView : MonoBehaviour
    {
        #region SERIALIZED_VARIABLES

        [Header("Settings")]
        [SerializeField] [Range(0, 50)] private float viewRadius;
        [SerializeField] [Range(0, 360)] private float viewAngle;
        [SerializeField] [Range(0, 1)] private float meshResolution;
        [SerializeField] [Range(0, 1)] private float maskCutawayDistance;
        [SerializeField] [Range(0, 1)] private float targetFindFrequency;
        [SerializeField] [Range(0, 5)] private float edgeDstThreshold;
        [SerializeField] [Range(0, 10)] private int edgeResolveIterations;

        [Header("Layers")]
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstacleMask;

        [Header("References")]
        [SerializeField] private MeshFilter viewMeshFilter;
        [SerializeField] private MeshRenderer viewMeshRendered;

        [Header("Materials")]
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material triggeredMaterial;

        #endregion

        #region PRIVATE_VARIABLES

        private Mesh viewMesh;
        private IDisposable _targetFindingDisposable;

        #endregion

        #region PROPERTIES

        public float ViewAngle
        {
            get => viewAngle;
            set => viewAngle = value;
        }

        public float ViewRadius
        {
            get => viewRadius;
            set => viewRadius = value;
        }

        public List<Transform> VisibleTargets { get; private set; } = new List<Transform>();

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        private void LateUpdate()
        {
            DrawFieldOfView();
        }

        private void OnDestroy()
        {
            StopTargetFind();
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        public Vector3 GetDirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal) angleInDegrees += transform.eulerAngles.y;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        public void ToggleTrigger(bool value)
        {
            var newMaterials = value ? new[] { triggeredMaterial } : new[] { defaultMaterial };

            viewMeshRendered.materials = newMaterials;
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
        {
            float minAngle = minViewCast.Angle;
            float maxAngle = maxViewCast.Angle;

            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < edgeResolveIterations; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast(angle);

                bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.Distance - newViewCast.Distance) > edgeDstThreshold;

                if (newViewCast.Hit == minViewCast.Hit && !edgeDstThresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.Point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.Point;
                }
            }

            return new EdgeInfo(minPoint, maxPoint);
        }

        private ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 direction = GetDirectionFromAngle(globalAngle, true);

            return Physics.Raycast(transform.position, direction, out RaycastHit hit, viewRadius, obstacleMask)
                ? new ViewCastInfo(true, globalAngle, hit.distance, hit.point)
                : new ViewCastInfo(false, globalAngle, viewRadius, transform.position + direction * viewRadius);
        }

        private void Init()
        {
            InitMesh();
            InitTargets();
        }

        private void InitMesh()
        {
            viewMesh = new Mesh
            {
                name = "View Mesh"
            };

            viewMeshFilter.mesh = viewMesh;
        }

        private void InitTargets()
        {
            VisibleTargets = new List<Transform>();
        }

        private void StartTargetFind()
        {
            _targetFindingDisposable ??= Observable
                .Timer(TimeSpan.FromSeconds(targetFindFrequency))
                .Repeat()
                .Subscribe(_ =>
                {
                    FindVisibleTargets();
                });
        }

        private void StopTargetFind()
        {
            _targetFindingDisposable?.Dispose();
            _targetFindingDisposable = null;
        }

        private void FindVisibleTargets()
        {
            VisibleTargets.Clear();

            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) >= viewAngle / 2) continue;

                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask)) continue;

                VisibleTargets.Add(target);
            }
        }

        private void DrawFieldOfView()
        {
            int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
            float stepAngleSize = viewAngle / stepCount;

            List<Vector3> viewPoints = new List<Vector3>();
            ViewCastInfo oldViewCast = new ViewCastInfo();

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
                ViewCastInfo newViewCast = ViewCast(angle);

                if (i > 0)
                {
                    bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.Distance - newViewCast.Distance) > edgeDstThreshold;

                    if (oldViewCast.Hit != newViewCast.Hit || (oldViewCast.Hit && newViewCast.Hit && edgeDistanceThresholdExceeded))
                    {
                        EdgeInfo edge = FindEdge(oldViewCast, newViewCast);

                        if (edge.PointA != Vector3.zero) viewPoints.Add(edge.PointA);
                        if (edge.PointB != Vector3.zero) viewPoints.Add(edge.PointB);
                    }
                }

                viewPoints.Add(newViewCast.Point);

                oldViewCast = newViewCast;
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = Vector3.zero;

            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.forward * maskCutawayDistance;

                if (i >= vertexCount - 2) continue;

                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

            viewMesh.Clear();

            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
        }

        #endregion
    }
}