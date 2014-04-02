using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;


namespace SpielNaoKinect.Kinect
{
    public class Angle
    {
        private MainWindow mw;
        public delegate void GuiAnzeigen();
        private int _RotationOffset = 0;
        private bool _ReverseCoordinates = false;
        private Vergleich Vergleich;
        public Angle(MainWindow mw)
        {
            this.mw = mw;
            Vergleich = new Vergleich(mw);
        }

        public void Berechnen(Skeleton currentSkeleton)
        {
            if (currentSkeleton != null)
            {
                Joint ElbowRight = currentSkeleton.Joints[JointType.ElbowRight];
                Joint ElbowLeft = currentSkeleton.Joints[JointType.ElbowLeft];
                Joint HipLeft = currentSkeleton.Joints[JointType.HipLeft];
                Joint HipRight = currentSkeleton.Joints[JointType.HipRight];
                Joint ShoulderLeft = currentSkeleton.Joints[JointType.ShoulderLeft];
                Joint ShoulderRight = currentSkeleton.Joints[JointType.ShoulderRight];
                Joint WristRight = currentSkeleton.Joints[JointType.WristRight];
                Joint WristLeft = currentSkeleton.Joints[JointType.WristLeft];

                Vergleich.Achsel_links_pitch(GetBodySegmentAngle(ElbowLeft, ShoulderLeft, HipLeft));
                Vergleich.Achsel_rechts_pitch(GetBodySegmentAngle(ElbowRight, ShoulderRight, HipRight));
                Vergleich.Ellenbogen_rechts_roll(GetBodySegmentAngle(ShoulderRight, ElbowRight, WristRight));
                Vergleich.Ellenbogen_links_roll(GetBodySegmentAngle(ShoulderLeft, ElbowLeft, WristLeft));
                Vergleich.Achsel_rechts_roll(GetBodySegmentAngle(ShoulderLeft, ShoulderRight, ElbowRight));
                Vergleich.Achsel_links_roll(GetBodySegmentAngle(ShoulderRight, ShoulderLeft, ElbowLeft));
            }
        }

        public int RotationOffset
         {
             get { return _RotationOffset; }
             set
             {
                 // Use the modulo in case the rotation value specified exceeds
                 // 360.
                 _RotationOffset = value % 360;
             }
         }

         public bool ReverseCoordinates
         {
             get { return _ReverseCoordinates; }
             set { _ReverseCoordinates = value; }
         }

         public double CalculateReverseCoordinates(double degrees)
         {
             return (-degrees + 180) % 360;
         }

         /// <summary>
         /// Calculates the angle between the segments of the body defined by the specified joints.
         /// </summary>
         /// <param name="joints"></param>
         /// <param name="joint1"></param>
         /// <param name="joint2">Must be between joint1 and joint3</param>
         /// <param name="joint3"></param>
         /// <returns>The angle in degrees between the specified body segmeents.</returns>
         public int GetBodySegmentAngle(Joint joint1, Joint joint2, Joint joint3)
         {

             Vector3D vectorJoint1ToJoint2 = new Vector3D(joint1.Position.X - joint2.Position.X, joint1.Position.Y - joint2.Position.Y, 0);
             Vector3D vectorJoint2ToJoint3 = new Vector3D(joint2.Position.X - joint3.Position.X, joint2.Position.Y - joint3.Position.Y, 0);
             vectorJoint1ToJoint2.Normalize();
             vectorJoint2ToJoint3.Normalize();

             Vector3D crossProduct = Vector3D.CrossProduct(vectorJoint1ToJoint2, vectorJoint2ToJoint3);
             double crossProductLength = crossProduct.Z;
             double dotProduct = Vector3D.DotProduct(vectorJoint1ToJoint2, vectorJoint2ToJoint3);
             double segmentAngle = Math.Atan2(crossProductLength, dotProduct);

             // Convert the result to degrees.
             double degrees = segmentAngle * (180 / Math.PI);

             // Add the angular offset.  Use modulo 360 to convert the value calculated above to a range
             // from 0 to 360.
             degrees = (degrees + _RotationOffset) % 360;

             // Calculate whether the coordinates should be reversed to account for different sides
             if (_ReverseCoordinates)
             {
                 degrees = CalculateReverseCoordinates(degrees);
             }

             return Convert.ToInt32(degrees);
         }  
    }
}
