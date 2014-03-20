using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpielNaoKinect.Kinect
{
        [Serializable]
        public struct Vector3 : IEquatable<Vector3>
        {
            public static bool operator ==(Vector3 v1, Vector3 v2) 
            {
                return v1.Equals(v2);
            }
            public double X;
            public double Y;
            public double Z;

            public static Vector3 Zero
            {
                get
                {
                    return new Vector3(0, 0, 0);
                }
            }

            public Vector3(double x, double y, double z)
            {
                X = x; Y = y; Z = z;
            }

            public double Length()
            {
                return (double)Math.Sqrt(X * X + Y * Y + Z * Z);
            }

            public static Vector3 operator -(Vector3 left, Vector3 right)
            {
                return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
            }

            public static Vector3 operator +(Vector3 left, Vector3 right)
            {
                return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
            }

            public static Vector3 operator *(Vector3 left, double value)
            {
                return new Vector3(left.X * value, left.Y * value, left.Z * value);
            }

            public static Vector3 operator *(double value, Vector3 left)
            {
                return left * value;
            }

            public static Vector3 operator /(Vector3 left, double value)
            {
                return new Vector3(left.X / value, left.Y / value, left.Z / value);
            }

            public static Vector3 ToVector3(SkeletonPoint point)
            {
                return new Vector3(point.X, point.Y, point.Z);
            }

            public static Vector3 Cross(Vector3 vectorJoint1ToJoint2, Vector3 vectorJoint2ToJoint3)
           {
                return
                (
                   new Vector3
                   (
                      vectorJoint1ToJoint2.Y * vectorJoint2ToJoint3.Z - vectorJoint1ToJoint2.Z * vectorJoint2ToJoint3.Y,
                      vectorJoint1ToJoint2.Z * vectorJoint2ToJoint3.X - vectorJoint1ToJoint2.X * vectorJoint2ToJoint3.Z,
                      vectorJoint1ToJoint2.X * vectorJoint2ToJoint3.Y - vectorJoint1ToJoint2.Y * vectorJoint2ToJoint3.X
                   )
                );
            }

            public static double Dot(Vector3 vectorJoint1ToJoint2, Vector3 vectorJoint2ToJoint3)
            {
                return
                (
                   vectorJoint1ToJoint2.X * vectorJoint2ToJoint3.X +
                   vectorJoint1ToJoint2.Y * vectorJoint2ToJoint3.Y +
                   vectorJoint1ToJoint2.Z * vectorJoint2ToJoint3.Z
                );
            }

            public static Vector3 Normalize(Vector3 v1)
            {
                // Check for divide by zero errors
                if (v1.Magnitude == 0)
                {
                    throw new DivideByZeroException(NORMALIZE_0);
                }
                else
                {
                    // find the inverse of the vectors magnitude
                    double inverse = 1 / v1.Magnitude;
                    return
                    (
                       new Vector3 
                       (
                        // multiply each component by the inverse of the magnitude
                          v1.X * inverse,
                          v1.Y * inverse,
                          v1.Z * inverse
                       )
                    );
                }
            }

            public void Normalize()
            {
                this = Normalize(this);
            }
            public static Vector3 origin = new Vector3(0, 0, 0);

            private const string NORMALIZE_0 = "Can not normalize a vector when" +
                "it's magnitude is zero";

            public double Magnitude
            {
                get
                {
                    return Math.Sqrt(SumComponentSqrs());
                }
                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("value", value,
                          NEGATIVE_MAGNITUDE);
                    }

                    if (this == origin)
                    { throw new ArgumentException(ORAGIN_VECTOR_MAGNITUDE, "this"); }

                    this = this * (value / Magnitude);
                }
            }

            private const string NEGATIVE_MAGNITUDE =
              "The magnitude of a Vector must be a positive value, (i.e. greater than 0)";

            private const string ORAGIN_VECTOR_MAGNITUDE =
               "Cannot change the magnitude of Vector(0,0,0)";


            public static double SumComponents(Vector3 v1)
            {
                return (v1.X + v1.Y + v1.Z);
            }

            public double SumComponents()
            {
                return SumComponents(this);
            }
            
            public static double SumComponentSqrs(Vector3 v1)
            {
                Vector3 v2 = SqrComponents(v1);
                return v2.SumComponents();
            }

            public double SumComponentSqrs()
            {
                return SumComponentSqrs(this);
            }
            public static Vector3 SqrComponents(Vector3 v1)
            {
                return
                (
                   new Vector3
                   (
                       v1.X * v1.X,
                       v1.Y * v1.Y,
                       v1.Z * v1.Z
                   )
                 );
            }

            public void SqrComponents()
            {
                this = SqrtComponents(this);
            }
            public static Vector3 SqrtComponents(Vector3 v1)
            {
                return
                (
                   new Vector3
                   (
                      Math.Sqrt(v1.X),
                      Math.Sqrt(v1.Y),
                      Math.Sqrt(v1.Z)
                   )
                );
            }

            public void SqrtComponents()
            {
                this = SqrtComponents(this);
            }
        }
}
