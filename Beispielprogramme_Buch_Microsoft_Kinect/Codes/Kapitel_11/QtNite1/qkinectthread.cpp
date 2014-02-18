#include "qkinectthread.h"

QKinectThread* QKinectThread::myStaticAccessor;

QKinectThread::QKinectThread(QObject *parent) :
    QThread(parent)
{
    myStopFlag=false;
    myDataTakenFlag=true;
}

void QKinectThread::run()
{
    myStaticAccessor=this;
    if(init()==false)
    {
       return;
    }

    while(!myStopFlag)
    {
       XnStatus status = myContext.WaitAndUpdateAll();

       //Export data
       myMutex.lock();
       xn::DepthMetaData depthMD;
       myDepthGenerator.GetMetaData(depthMD);
       double timestamp = (double)depthMD.Timestamp()/1000000.0;
       std::vector<Body> bodies = createBodies();
       QImage imageCamera = createCameraImage();
       QImage imageDepth = createDepthImage();
       myExportBodies=bodies;
       myExportRGB=imageCamera;
       myExportDepth=imageDepth;
       emit dataNotification();
       myMutex.unlock();


    }
    myContext.Shutdown();
}

bool QKinectThread::init()
{
    XnStatus retVal=XN_STATUS_OK;


    XnMapOutputMode outputMode;
    XnCallbackHandle hUserCallbacks;
    XnCallbackHandle hCalibrationStart, hCalibrationComplete,hPoseDetected;

    retVal = myContext.Init();
    if(retVal!=XN_STATUS_OK)
    {
       return false;
    }

    //Tiefendaten
    retVal = myDepthGenerator.Create(myContext);
    if(retVal!=XN_STATUS_OK)
    {
        return false;
    }
    outputMode.nXRes = 640;
    outputMode.nYRes = 480;
    outputMode.nFPS = 30;
    retVal = myDepthGenerator.SetMapOutputMode(outputMode);
    if(retVal!=XN_STATUS_OK)
    {
        return false;
    }


     // Farbdaten
     retVal = myImageGenerator.Create(myContext);
     if(retVal!=XN_STATUS_OK)
     {
        return false;
     }
     outputMode.nXRes = 640;
     outputMode.nYRes = 480;
     outputMode.nFPS = 30;
     retVal = myImageGenerator.SetMapOutputMode(outputMode);
     if(retVal!=XN_STATUS_OK)
     {
        return false;
     }

     // Skelettaldaten
     retVal = myUserGenerator.Create(myContext);
     if(retVal!=XN_STATUS_OK)
     {
        return false;
     }

     if (myUserGenerator.IsCapabilitySupported(XN_CAPABILITY_SKELETON)==false)
     {
        return false;
     }

     myUserGenerator.RegisterUserCallbacks(User_NewUser, User_LostUser, this, hUserCallbacks);
     myUserGenerator.GetSkeletonCap().RegisterToCalibrationStart(UserCalibration_CalibrationStart,this,hCalibrationStart);
     myUserGenerator.GetSkeletonCap().RegisterToCalibrationComplete(UserCalibration_CalibrationComplete,this,hCalibrationComplete);

     myNeedPose=false;
     if (myUserGenerator.GetSkeletonCap().NeedPoseForCalibration())
     {
        myNeedPose = true;
        if (!myUserGenerator.IsCapabilitySupported(XN_CAPABILITY_POSE_DETECTION))
        {
           return false;
        }
        myUserGenerator.GetPoseDetectionCap().RegisterToPoseDetected(UserPose_PoseDetected, this, hPoseDetected);

        myUserGenerator.GetSkeletonCap().GetCalibrationPose(mystrPose);
     }

     myUserGenerator.GetSkeletonCap().SetSkeletonProfile(XN_SKEL_PROFILE_ALL);

     // Start producting data
     retVal = myContext.StartGeneratingAll();

     return true;

}


/**
  \brief Fill the Bodies data structure holding all the infos about the detected users.
**/
std::vector<Body> QKinectThread::createBodies()
{

   std::vector<Body> bodies;

   Body b;


   XnUserID aUsers[15];
   XnUInt16 nUsers = 15;
   myUserGenerator.GetUsers(aUsers, nUsers);
   for (int i = 0; i < nUsers; ++i)
   {
      // id
      b.id = aUsers[i];
      // Get the center of mass and its projection
      myUserGenerator.GetCoM(aUsers[i], b.com);
      myDepthGenerator.ConvertRealWorldToProjective(1, &b.com, &b.proj_com);
      if(isnan(b.proj_com.X) || isnan(b.proj_com.Y) || isnan(b.proj_com.Z))
         b.proj_com_valid=false;
      else
         b.proj_com_valid=true;

      if(myUserGenerator.GetSkeletonCap().IsTracking(aUsers[i]))
      {
         // If the user is tracked, get the skeleton
         b.status=Tracking;
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_HEAD, b.joints[Head]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_NECK, b.joints[Neck]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_LEFT_SHOULDER, b.joints[LeftShoulder]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_LEFT_ELBOW, b.joints[LeftElbow]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_LEFT_HAND, b.joints[LeftHand]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_RIGHT_SHOULDER, b.joints[RightShoulder]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_RIGHT_ELBOW, b.joints[RightElbow]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_RIGHT_HAND, b.joints[RightHand]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_TORSO, b.joints[Torso]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_LEFT_HIP, b.joints[LeftHip]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_LEFT_KNEE, b.joints[LeftKnee]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_LEFT_FOOT, b.joints[LeftFoot]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_RIGHT_HIP, b.joints[RightHip]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_RIGHT_KNEE, b.joints[RightKnee]);
         myUserGenerator.GetSkeletonCap().GetSkeletonJointPosition(aUsers[i], XN_SKEL_RIGHT_FOOT, b.joints[RightFoot]);

         // Get the projection of the skeleton
         for(unsigned i=Head;i<=RightFoot;i++)
            getJointProj(b.joints[i],b.proj_joints[i],b.proj_joints_valid[i]);
      }
      else
      {
         if (myUserGenerator.GetSkeletonCap().IsCalibrating(aUsers[i]))
            b.status=Calibrating;
         else
            b.status=LookingForPose;
      }
      bodies.push_back(b);
   }
   return bodies;
}

void QKinectThread::getJointProj(XnSkeletonJointPosition joint,XnPoint3D &proj,bool &valid)
{
   if(joint.fConfidence<0.5)
   {
      valid = false;
   }
   else
   {
      valid = true;
      myDepthGenerator.ConvertRealWorldToProjective(1, &joint.position, &proj);
   }
}


QImage QKinectThread::createDepthImage()
{
   xn::SceneMetaData smd;
   xn::DepthMetaData dmd;
   myDepthGenerator.GetMetaData(dmd);
   myUserGenerator.GetUserPixels(0, smd);

   XnUInt16 g_nXRes = dmd.XRes();
   XnUInt16 g_nYRes = dmd.YRes();

   QImage image(g_nXRes,g_nYRes,QImage::Format_RGB32);


   const XnDepthPixel* pDepth = dmd.Data();
   const XnLabel* pLabels = smd.Data();

   for (unsigned nY=0; nY<g_nYRes; nY++)
   {
      uchar *imageptr = image.scanLine(nY);

      for (unsigned nX=0; nX < g_nXRes; nX++)
      {
         unsigned depth = *pDepth;
         unsigned label = *pLabels;


         unsigned maxdist=10000;
         if(depth>maxdist) depth=maxdist;
         if(depth)
         {
            depth = (maxdist-depth)*255/maxdist+1;
         }
         // depth: 0: invalid
         // depth: 255: closest
         // depth: 1: furtherst (maxdist distance)


         if(label)
         {
             int BodyColors[][3] =
             {
                {0,127,127},
                {0,0,127},
                {0,127,0},
                {127,127,0},
                {127,0,0},
                {127,63,0},
                {63,127,0},
                {0,63,127},
                {63,0,127},
                {127,127,63},
                {127,127,127}
             };

            imageptr[0] = BodyColors[label][0]*2*depth/255;
            imageptr[1] = BodyColors[label][1]*2*depth/255;
            imageptr[2] = BodyColors[label][2]*2*depth/255;
            imageptr[3] = 0xff;
         }
         else
         {
            // Here we could do depth*color, to show the colored depth
            imageptr[0] = depth;
            imageptr[1] = depth;
            imageptr[2] = depth;
            imageptr[3] = 0xff;
         }
         pDepth++;
         imageptr+=4;
         pLabels++;
      }
   }


   QPainter painter;
   painter.begin(&image);
   painter.end();
   return image;

}

QImage QKinectThread::createCameraImage()
{
   xn::DepthMetaData dmd;
   xn::ImageMetaData imd;
   myDepthGenerator.GetMetaData(dmd);
   myImageGenerator.GetMetaData(imd);

   XnUInt16 g_nXRes = dmd.XRes();
   XnUInt16 g_nYRes = dmd.YRes();

   QImage image(g_nXRes,g_nYRes,QImage::Format_RGB32);

   const XnUInt8 *idata = imd.Data();
   for (unsigned nY=0; nY<g_nYRes; nY++)
   {
      uchar *imageptr = image.scanLine(nY);

      for (unsigned nX=0; nX < g_nXRes; nX++)
      {
         imageptr[0] = idata[2];
         imageptr[1] = idata[1];
         imageptr[2] = idata[0];
         imageptr[3] = 0xff;

         imageptr+=4;
         idata+=3;

      }
   }
   QPainter painter;
   painter.begin(&image);
   QPen aPen;
   aPen.setWidth(3);
    aPen.setBrush(Qt::green);
      painter.setPen(aPen);
      drawSkeleton(&painter);
   painter.end();

   return image;

}

void XN_CALLBACK_TYPE QKinectThread::User_NewUser(xn::UserGenerator& generator, XnUserID nId, void* pCookie)
{
   if (myStaticAccessor->myNeedPose)
      myStaticAccessor->myUserGenerator.GetPoseDetectionCap().StartPoseDetection(myStaticAccessor->mystrPose, nId);
   else
      myStaticAccessor->myUserGenerator.GetSkeletonCap().RequestCalibration(nId, TRUE);
   //emit pthis->userNotification(nId,true);
}

void XN_CALLBACK_TYPE QKinectThread::User_LostUser(xn::UserGenerator& generator, XnUserID nId, void* pCookie)
{
   //QKinectWrapper *pthis = (QKinectWrapper*)pCookie;
   //emit pthis->userNotification(nId,false);
}

void XN_CALLBACK_TYPE QKinectThread::UserPose_PoseDetected(xn::PoseDetectionCapability& capability, const XnChar* strPose, XnUserID nId, void* pCookie)
{
   myStaticAccessor->myUserGenerator.GetPoseDetectionCap().StopPoseDetection(nId);
   myStaticAccessor->myUserGenerator.GetSkeletonCap().RequestCalibration(nId, TRUE);
   //emit pthis->poseNotification(nId,QString(strPose));
}


void XN_CALLBACK_TYPE QKinectThread::UserCalibration_CalibrationStart(xn::SkeletonCapability& capability, XnUserID nId, void* pCookie)
{

}

void XN_CALLBACK_TYPE QKinectThread::UserCalibration_CalibrationComplete(xn::SkeletonCapability& capability, XnUserID nId, XnCalibrationStatus calibrationError, void* pCookie)
{
   if (calibrationError == XN_CALIBRATION_STATUS_OK)
   {
      myStaticAccessor->myUserGenerator.GetSkeletonCap().StartTracking(nId);
      //emit pthis->calibrationNotification(nId,CalibrationEndSuccess);
      return;
   }
   // Calibration failed
   if (myStaticAccessor->myNeedPose)
   {
      myStaticAccessor->myUserGenerator.GetPoseDetectionCap().StartPoseDetection(myStaticAccessor->mystrPose, nId);
   }
   else
   {
      myStaticAccessor->myUserGenerator.GetSkeletonCap().RequestCalibration(nId, TRUE);
   }
   //emit pthis->calibrationNotification(nId,CalibrationEndFail);
}

void QKinectThread::drawSkeleton(QPainter *painter)
{
   for(unsigned i=0;i<myExportBodies.size();i++)
   {
      drawLimb(painter,myExportBodies[i],Head,Neck);

      drawLimb(painter,myExportBodies[i],Neck,LeftShoulder);
      drawLimb(painter,myExportBodies[i],LeftShoulder,LeftElbow);
      drawLimb(painter,myExportBodies[i],LeftElbow,LeftHand);

      drawLimb(painter,myExportBodies[i],Neck,RightShoulder);
      drawLimb(painter,myExportBodies[i],RightShoulder,RightElbow);
      drawLimb(painter,myExportBodies[i],RightElbow,RightHand);

      drawLimb(painter,myExportBodies[i],LeftShoulder,Torso);
      drawLimb(painter,myExportBodies[i],RightShoulder,Torso);

      drawLimb(painter,myExportBodies[i],Torso,LeftHip);
      drawLimb(painter,myExportBodies[i],LeftHip,LeftKnee);
      drawLimb(painter,myExportBodies[i],LeftKnee,LeftFoot);

      drawLimb(painter,myExportBodies[i],Torso,RightHip);
      drawLimb(painter,myExportBodies[i],RightHip,RightKnee);
      drawLimb(painter,myExportBodies[i],RightKnee,RightFoot);

      drawLimb(painter,myExportBodies[i],LeftHip,RightHip);
   }
}


void QKinectThread::drawLimb(QPainter *painter,const Body &body, BodyJoints j1,BodyJoints j2)
{
   if(body.proj_joints_valid[j1]==false || body.proj_joints_valid[j2]==false)
      return;
   painter->drawLine(body.proj_joints[j1].X,body.proj_joints[j1].Y,body.proj_joints[j2].X,body.proj_joints[j2].Y);
}
