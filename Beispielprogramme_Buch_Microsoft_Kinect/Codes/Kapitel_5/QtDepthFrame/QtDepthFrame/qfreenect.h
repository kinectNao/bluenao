#ifndef QFREENECT_H
#define QFREENECT_H

#include <QObject>
#include <QMutex>
#include <libfreenect.h>
#include <vector>
#include "qfreenectthread.h"

class QFreenect : public QObject
{
    Q_OBJECT
public:
    explicit QFreenect(QObject *parent = 0);
    ~QFreenect();
    void processVideo(void *myVideo, uint32_t myTimestamp=0);
    void processDepth(void *myDepth, uint32_t myTimestamp=0);

signals:
    void videoDataReady(uint8_t* myRGBBuffer);
    void depthDataReady(uint16_t* myDepthBuffer);

public slots:

private:
    freenect_context *myContext;
    freenect_device *myDevice;
    QFreenectThread *myWorker;
    uint8_t* myRGBBuffer;
    uint16_t* myDepthBuffer;
    QMutex* myMutex;

public:
    bool myWantDataFlag;
    bool myFlagFrameTaken;
    bool myFlagDFrameTaken;
    static QFreenect* mySelf;
};

#endif // QFREENECT_H
