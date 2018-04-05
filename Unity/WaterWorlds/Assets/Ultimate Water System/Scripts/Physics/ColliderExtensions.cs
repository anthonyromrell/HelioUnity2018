namespace UltimateWater
{
    using UnityEngine;

    /// <summary>
    ///     A bunch of useful extension methods for colliders.
    /// </summary>
    public static class ColliderExtensions
    {
        #region Volume

        public static float ComputeVolume(this Collider that)
        {
            var collider = that as BoxCollider;
            if (collider != null)
                return collider.ComputeVolume();

            var sphereCollider = that as SphereCollider;
            if (sphereCollider != null)
                return sphereCollider.ComputeVolume();

            var meshCollider = that as MeshCollider;
            if (meshCollider != null)
                return meshCollider.ComputeVolume();

            var capsuleCollider = that as CapsuleCollider;
            if (capsuleCollider != null)
                return capsuleCollider.ComputeVolume();

            throw new System.NotImplementedException("UltimateWater: Unknown collider type.");
        }

        public static float ComputeVolume(this BoxCollider that)
        {
            Vector3 size = that.size;
            Vector3 scale = that.transform.lossyScale;

            return size.x * scale.x * size.y * scale.y * size.z * scale.z;
        }

        public static float ComputeVolume(this SphereCollider that)
        {
            float r = that.radius;
            Vector3 scale = that.transform.lossyScale;

            return (4.0f / 3.0f) * Mathf.PI * r * r * r * scale.x * scale.y * scale.z;
        }

        public static float ComputeVolume(this MeshCollider that)
        {
            float volume = 0;

            var mesh = that.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            int numTriangles = triangles.Length;

            Vector3 center = that.transform.InverseTransformPoint(that.bounds.center);

            for (int i = 0; i < numTriangles;)
            {
                Vector3 p1 = vertices[triangles[i++]] - center;
                Vector3 p2 = vertices[triangles[i++]] - center;
                Vector3 p3 = vertices[triangles[i++]] - center;

                volume += SignedVolumeOfTriangle(p1, p2, p3);
            }

            Vector3 scale = that.transform.lossyScale;
            return Mathf.Abs(volume) * scale.x * scale.y * scale.z;
        }

        public static float ComputeVolume(this CapsuleCollider that)
        {
            float r = that.radius;
            float sphere = (4.0f / 3.0f) * Mathf.PI * r * r * r;
            float cylinder = Mathf.PI * r * r * that.height;
            Vector3 scale = that.transform.lossyScale;

            return (cylinder + sphere) * scale.x * scale.y * scale.z;
        }

        public static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var v321 = p3.x * p2.y * p1.z;
            var v231 = p2.x * p3.y * p1.z;
            var v312 = p3.x * p1.y * p2.z;
            var v132 = p1.x * p3.y * p2.z;
            var v213 = p2.x * p1.y * p3.z;
            var v123 = p1.x * p2.y * p3.z;
            return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
        }

        #endregion Volume

        #region Area

        public static float ComputeArea(this Collider that)
        {
            var collider = that as MeshCollider;
            if (collider != null)
                return collider.ComputeArea();

            var boxCollider = that as BoxCollider;
            if (boxCollider != null)
                return boxCollider.ComputeArea();

            var sphereCollider = that as SphereCollider;
            if (sphereCollider != null)
                return sphereCollider.ComputeArea();

            var capsuleCollider = that as CapsuleCollider;
            if (capsuleCollider != null)
                return capsuleCollider.ComputeArea();

            throw new System.NotImplementedException("UltimateWater: Unknown collider type.");
        }

        public static float ComputeArea(this MeshCollider that)
        {
            float area = 0;

            var mesh = that.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            int numTriangles = triangles.Length;

            Vector3 scale = that.transform.lossyScale;

            for (int i = 0; i < numTriangles;)
            {
                Vector3 origin = vertices[triangles[i++]];
                Vector3 a = vertices[triangles[i++]] - origin;
                Vector3 b = vertices[triangles[i++]] - origin;

                a.Scale(scale);
                b.Scale(scale);

                area += Vector3.Cross(a, b).magnitude;
            }

            return area * 0.5f;
        }

        public static float ComputeArea(this BoxCollider that)
        {
            Vector3 size = that.size;
            size.Scale(that.transform.lossyScale);

            return 2.0f * (size.x * size.y + size.y * size.z + size.x * size.z);
        }

        public static float ComputeArea(this SphereCollider that)
        {
            float s = that.transform.lossyScale.magnitude;
            float r = that.radius * s;

            return 4.0f * Mathf.PI * r * r;
        }

        public static float ComputeArea(this CapsuleCollider that)
        {
            Vector3 scale = that.transform.lossyScale;
            float r = that.radius * scale.magnitude;
            float height = that.height;

            switch (that.direction)
            {
                case 0:
                height *= scale.x;
                break;

                case 1:
                height *= scale.y;
                break;

                case 2:
                height *= scale.z;
                break;

                default:
                throw new System.InvalidOperationException();
            }

            return 2.0f * Mathf.PI * r * (2.0f * r + height);
        }

        #endregion Area

        #region Random

        /// <summary>
        /// Returns random local point in a collider.
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public static Vector3 RandomPoint(this Collider that)
        {
            var collider = that as MeshCollider;
            if (collider != null)
            {
                return collider.RandomPoint();
            }

            var boxCollider = that as BoxCollider;
            if (boxCollider != null)
            {
                return boxCollider.RandomPoint();
            }

            var capsuleCollider = that as CapsuleCollider;
            if (capsuleCollider != null)
            {
                return capsuleCollider.RandomPoint();
            }

            var sphereCollider = that as SphereCollider;
            if (sphereCollider != null)
            {
                return sphereCollider.RandomPoint();
            }

            throw new System.NotImplementedException("UltimateWater: Unknown collider type.");
        }

        public static Vector3 RandomPoint(this MeshCollider that)
        {
            var bounds = that.sharedMesh.bounds;
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            Vector3 range = (max - min);

            Vector3 p = new Vector3();

            for (int i = 0; i < 40; ++i)
            {
                p.x = min.x + Random.value * range.x;
                p.y = min.y + Random.value * range.y;
                p.z = min.z + Random.value * range.z;

                if (that.IsPointInside(that.transform.TransformPoint(p)))
                    break;
            }

            return p;
        }

        public static Vector3 RandomPoint(this BoxCollider that)
        {
            Vector3 center = that.center;
            Vector3 halfSize = that.size * 0.5f;

            float x = center.x + Random.Range(-halfSize.x, halfSize.x);
            float y = center.y + Random.Range(-halfSize.y, halfSize.y);
            float z = center.z + Random.Range(-halfSize.z, halfSize.z);

            return new Vector3(x, y, z);
        }

        public static Vector3 RandomPoint(this CapsuleCollider that)
        {
            float r = that.radius;
            float cylinderHeight = that.height;

            float cylinderVolume = Mathf.PI * r * r * cylinderHeight;
            float spheresVolume = (4.0f / 3.0f) * Mathf.PI * r * r * r;

            float f = Random.Range(0.0f, cylinderVolume + spheresVolume);

            Vector3 p;

            if (f < cylinderVolume)
            {
                p = RandomPointInCircle(r);
                p.z = p.y;

                p.y = Random.Range(-cylinderHeight * 0.5f, cylinderHeight * 0.5f);
            }
            else
            {
                p = RandomPointInSphere(r);

                if (p.y < 0.0f)
                    p.y -= cylinderHeight * 0.5f;
                else
                    p.y += cylinderHeight * 0.5f;
            }

            switch (that.direction)
            {
                case 0:
                {
                    float t = p.y;
                    p.y = p.x;
                    p.x = t;
                    break;
                }

                case 2:
                {
                    float t = p.y;
                    p.y = p.z;
                    p.z = t;
                    break;
                }
            }

            return p;
        }

        public static Vector3 RandomPoint(this SphereCollider that)
        {
            return RandomPointInSphere(that.radius);
        }

        public static Vector3 RandomPointInSphere(float radius)
        {
            float rvals = Random.Range(-1.0f, 1.0f);
            float elevation = Mathf.Asin(rvals);

            float azimuth = 2 * Mathf.PI * Random.Range(0.0f, 1.0f);

            float radii = 3 * Mathf.Pow(Random.Range(0.0f, 1.0f), 0.33333333f);

            float se = Mathf.Sin(elevation);

            return new Vector3(
                radii * se * Mathf.Cos(azimuth),
                radii * se * Mathf.Sin(azimuth),
                radii * Mathf.Cos(elevation)
                );
        }

        public static Vector2 RandomPointInCircle(float radius)
        {
            float t = 2 * Mathf.PI * Random.Range(0.0f, 1.0f);
            float u = Random.Range(0.0f, 1.0f) + Random.Range(0.0f, 1.0f);
            float r = (u > 1 ? 2 - u : u) * radius;
            return new Vector2(r * Mathf.Cos(t), r * Mathf.Sin(t));
        }

        #endregion Random

        public static void GetLocalMinMax(Collider collider, out Vector3 min, out Vector3 max)
        {
            var meshCollider = collider as MeshCollider;
            if (meshCollider != null)
            {
                var bounds = meshCollider.sharedMesh.bounds;
                min = bounds.min;
                max = bounds.max;
                return;
            }

            var boxCollider = collider as BoxCollider;
            if (boxCollider != null)
            {
                var box = boxCollider;
                min = box.center - box.size * 0.5f;
                max = box.center + box.size * 0.5f;
                return;
            }

            var sphereCollider = collider as SphereCollider;
            if (sphereCollider != null)
            {
                var sphere = sphereCollider;
                Vector3 center = sphere.center;
                float halfRadius = sphere.radius * 0.5f;

                min = new Vector3(center.x - halfRadius, center.y - halfRadius, center.z - halfRadius);
                max = new Vector3(center.x + halfRadius, center.y + halfRadius, center.z + halfRadius);
                return;
            }

            var capsuleCollider = collider as CapsuleCollider;
            if (capsuleCollider != null)
            {
                var capsule = capsuleCollider;
                Vector3 center = capsule.center;
                float halfRadius = capsule.radius * 0.5f;
                float halfHeight = capsule.height * 0.5f + halfRadius;

                switch (capsule.direction)
                {
                    case 0:
                    {
                        min = new Vector3(center.x - halfHeight, center.y - halfRadius, center.z - halfRadius);
                        max = new Vector3(center.x + halfHeight, center.y + halfRadius, center.z + halfRadius);
                        break;
                    }

                    case 1:
                    {
                        min = new Vector3(center.x - halfRadius, center.y - halfHeight, center.z - halfRadius);
                        max = new Vector3(center.x + halfRadius, center.y + halfHeight, center.z + halfRadius);
                        break;
                    }

                    case 2:
                    {
                        min = new Vector3(center.x - halfRadius, center.y - halfRadius, center.z - halfHeight);
                        max = new Vector3(center.x + halfRadius, center.y + halfRadius, center.z + halfHeight);
                        break;
                    }

                    default:
                    throw new System.InvalidOperationException();
                }
                return;
            }

            throw new System.InvalidOperationException();
        }

        public static bool IsPointInside(this Collider convex, Vector3 point)
        {
            var bounds = convex.bounds;

            if (!bounds.Contains(point))
                return false;

            Vector3 dir = bounds.center - point;

            float magnitude = dir.magnitude;

            if (magnitude < 0.00001f)
                return true;

            RaycastHit hitInfo;
            return !convex.Raycast(new Ray(point, dir), out hitInfo, magnitude);
        }
    }
}