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

                // Berechnung mit 3 Joints
                Vergleich.Achsel_links_pitch(GetBodySegmentAngle(ElbowLeft, ShoulderLeft, HipLeft));
                Vergleich.Achsel_rechts_pitch(GetBodySegmentAngle(ElbowRight, ShoulderRight, HipRight));
                Vergleich.Ellenbogen_rechts_roll(GetBodySegmentAngle(ShoulderRight, ElbowRight, WristRight));
                Vergleich.Ellenbogen_links_roll(GetBodySegmentAngle(ShoulderLeft, ElbowLeft, WristLeft));
                //Die beiden Achsel Roll mit 4 Joints
                Vergleich.Achsel_rechts_roll(GetBodySegmentAngle(HipRight, HipLeft, ShoulderRight, ElbowRight));
                Vergleich.Achsel_links_roll(GetBodySegmentAngle(HipLeft, HipRight, ShoulderLeft, ElbowLeft));
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

         
        // Zusammenhängende Knochen
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
             degrees = degrees % 360;

             // Calculate whether the coordinates should be reversed to account for different sides
             if (_ReverseCoordinates)
             {
                 degrees = CalculateReverseCoordinates(degrees);
             }

             return Convert.ToInt32(degrees);
         }
        
        // Getrennte Knochen
         public int GetBodySegmentAngle(Joint joint1, Joint joint2, Joint joint3, Joint joint4)
         {
             Vector3D vectorJoint1ToJoint2 = new Vector3D(joint1.Position.X - joint2.Position.X, joint1.Position.Y - joint2.Position.Y, joint1.Position.Z - joint2.Position.Z);
             Vector3D vectorJoint3ToJoint4 = new Vector3D(joint3.Position.X - joint4.Position.X, joint3.Position.Y - joint4.Position.Y, joint3.Position.Z - joint4.Position.Z);
             vectorJoint1ToJoint2.Normalize();
             vectorJoint3ToJoint4.Normalize();

             double dotProduct = Vector3D.DotProduct(vectorJoint1ToJoint2, vectorJoint3ToJoint4);
             double segmentAngle = Math.Acos(dotProduct);

             // Convert the result to degrees.
             double degrees = segmentAngle * (180 / Math.PI);

             // Add the angular offset.  Use modulo 360 to convert the value calculated above to a range
             // from 0 to 360.
             degrees = degrees % 360;

             // Calculate whether the coordinates should be reversed to account for different sides
             if (_ReverseCoordinates)
             {
                 degrees = CalculateReverseCoordinates(degrees);
             }

             return Convert.ToInt32(degrees);

         }
    }
}
