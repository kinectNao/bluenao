#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "qkinectthread.h"

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
    
public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    void paintEvent(QPaintEvent *pe);

public slots:
    void kinectDataHere();

private:
    Ui::MainWindow *ui;
    QKinectThread *myKinect;
    QImage myDepthImage;
    QImage myColorImage;
};

#endif // MAINWINDOW_H
