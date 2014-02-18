#ifndef QKINECTTHREAD_H
#define QKINECTTHREAD_H

#include <QThread>
#include <QImage>
#include <QMutex>
#include <QPainter>
#include <XnOpenNI.h>
#include <XnCppWrapper.h>
#include <math.h>

enum BodyStatus
  {
     Tracking = 0,
     Calibrating = 1,
     LookingForPose = 2
  };

  enum BodyJoints
  {
     Head = 0,
     Neck = 1,
     LeftShoulder = 2,
     LeftElbow = 3,
     LeftHand = 4,
     RightShoulder = 5,
     RightElbow = 6,
     RightHand = 7,
     Torso = 8,
     LeftHip = 9,
     LeftKnee = 10,
     LeftFoot = 11,
     RightHip = 12,
     RightKnee = 13,
     RightFoot = 14
  };

typedef struct {
   XnUserID id;
   // Status: 0=tracking, 1=calibrating, 2=looking for pose
   BodyStatus status;
   // Center of mass and its projection.
   XnPoint3D com, proj_com;
   // Whether the com projection is valid
   bool proj_com_valid;
   // Whether the body tracked
   //bool tracked;
   // 3D coordinates of the joints
   XnSkeletonJointPosition joints[15];
   // Projected coordinates of the joints
   XnPoint3D proj_joints[15];
   // Whether the projection is valid / has been computed. Projections are not computed if the joint is too uncertain.
   bool proj_joints_valid[15];
} Body;


class QKinectThread : public QThread
{
    Q_OBJECT
public:
    explicit QKinectThread(QObject *parent = 0);
    
    void run();

    bool myStopFlag;
    bool myDataTakenFlag;
    std::vector<Body> myExportBodies;
    QImage myExportRGB;
    QImage myExportDepth;
    QMutex myMutex;

    void getJointProj(XnSkeletonJointPosition joint,XnPoint3D &proj,bool &valid);

    static void XN_CALLBACK_TYPE User_NewUser(xn::UserGenerator& generator, XnUserID nId, void* pCookie);
    static void XN_CALLBACK_TYPE User_LostUser(xn::UserGenerator& generator, XnUserID nId, void* pCookie);
    static void XN_CALLBACK_TYPE UserPose_PoseDetected(xn::PoseDetectionCapability& capability, const XnChar* strPose, XnUserID nId, void* pCookie);
    static void XN_CALLBACK_TYPE UserCalibration_CalibrationStart(xn::SkeletonCapability& capability, XnUserID nId, void* pCookie);
    static void XN_CALLBACK_TYPE UserCalibration_CalibrationComplete(xn::SkeletonCapability& capability, XnUserID nId, XnCalibrationStatus calibrationError, void* pCookie);

    void drawSkeleton(QPainter *painter);
    void drawLimb(QPainter *painter,const Body &body, BodyJoints j1,BodyJoints j2);

signals:
    void dataNotification();

public slots:
    
private:
    bool init();

    static QKinectThread* myStaticAccessor;

    QImage createCameraImage();
    QImage createDepthImage();
    std::vector<Body>  createBodies();


    xn::Context myContext;
    xn::DepthGenerator myDepthGenerator;
    xn::UserGenerator myUserGenerator;
    xn::ImageGenerator myImageGenerator;

    bool myNeedPose;
    XnChar mystrPose[20];

};

#endif // QKINECTTHREAD_H
