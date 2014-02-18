#include "qfreenect.h"
#include <QApplication>
#include <stdlib.h>

QFreenect* QFreenect::mySelf;

static inline void videoCallback(freenect_device *myDevice, void *myVideo, uint32_t myTimestamp=0)
{
    QFreenect::mySelf->processVideo(myVideo, myTimestamp);
}

static inline void depthCallback(freenect_device *myDevice, void *myVideo, uint32_t myTimestamp=0)
{
    QFreenect::mySelf->processDepth(myVideo, myTimestamp);
}


QFreenect::QFreenect(QObject *parent) :
    QObject(parent)
{
    myMutex=NULL;
    myRGBBuffer=NULL;

    myMutex=new QMutex();
    myWantDataFlag=false;
    myFlagFrameTaken=true;
    mySelf=this;

    if (freenect_init(&myContext, NULL) < 0)
    {
        qDebug("init failed");
        QApplication::exit(1);
    }
    freenect_set_log_level(myContext, FREENECT_LOG_FATAL);

    int nr_devices = freenect_num_devices (myContext);
    if (nr_devices < 1)
    {
        freenect_shutdown(myContext);
        qDebug("No Kinect found!");
        QApplication::exit(1);
    }

    if (freenect_open_device(myContext, &myDevice, 0) < 0)
    {
        qDebug("Open Device Failed!");
        freenect_shutdown(myContext);
        QApplication::exit(1);
    }
    myRGBBuffer = (uint8_t*)malloc(640*480*3);
    freenect_set_video_callback(myDevice, videoCallback);
    freenect_set_video_buffer(myDevice, myRGBBuffer);
    freenect_frame_mode vFrame = freenect_find_video_mode(FREENECT_RESOLUTION_MEDIUM,FREENECT_VIDEO_RGB);
    freenect_set_video_mode(myDevice,vFrame);
    freenect_start_video(myDevice);

    myDepthBuffer= (uint16_t*)malloc(640*480*2);
    freenect_set_depth_callback(myDevice, depthCallback);
    freenect_set_depth_buffer(myDevice, myDepthBuffer);
    freenect_frame_mode aFrame = freenect_find_depth_mode(FREENECT_RESOLUTION_MEDIUM,FREENECT_DEPTH_REGISTERED);
    freenect_set_depth_mode(myDevice,aFrame);
    freenect_start_depth(myDevice);

    myWorker=new QFreenectThread(this);
    myWorker->myActive=true;
    myWorker->myContext=myContext;
    myWorker->start();

}

QFreenect::~QFreenect()
{
    freenect_close_device(myDevice);
    freenect_shutdown(myContext);
    if(myRGBBuffer!=NULL)free(myRGBBuffer);
    if(myMutex!=NULL)delete myMutex;
}

void QFreenect::processVideo(void *myVideo, uint32_t myTimestamp)
{
    QMutexLocker locker(myMutex);
    if(myWantDataFlag && myFlagFrameTaken)
    {
        uint8_t* mySecondBuffer=(uint8_t*)malloc(640*480*3);
        memcpy(mySecondBuffer,myVideo,640*480*3);
        myFlagFrameTaken=false;
        emit videoDataReady(mySecondBuffer);
    }
}

void QFreenect::processDepth(void *myDepth, uint32_t myTimestamp)
{
    QMutexLocker locker(myMutex);
    if(myWantDataFlag && myFlagDFrameTaken)
    {
        uint16_t* mySecondBuffer=(uint16_t*)malloc(640*480*2);
        memcpy(mySecondBuffer,myDepth,640*480*2);
        myFlagDFrameTaken=false;
        emit depthDataReady(mySecondBuffer);
    }
}
