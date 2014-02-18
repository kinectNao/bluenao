#ifndef QFREENECTTHREAD_H
#define QFREENECTTHREAD_H

#include <QThread>
#include <libfreenect.h>


class QFreenectThread : public QThread
{
    Q_OBJECT
public:
    explicit QFreenectThread(QObject *parent = 0);
    void run();

signals:

public slots:

public:
    bool myActive;
    freenect_context *myContext;
};

#endif // QFREENECTTHREAD_H
