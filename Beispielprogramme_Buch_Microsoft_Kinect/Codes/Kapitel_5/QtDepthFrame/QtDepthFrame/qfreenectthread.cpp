#include "qfreenectthread.h"
#include <QApplication>

QFreenectThread::QFreenectThread(QObject *parent) :
    QThread(parent)
{
}

void QFreenectThread::run()
{
    while(myActive)
    {
        if(freenect_process_events(myContext) < 0)
        {
            qDebug("Cannot process events!");
            QApplication::exit(1);
        }
    }
}
