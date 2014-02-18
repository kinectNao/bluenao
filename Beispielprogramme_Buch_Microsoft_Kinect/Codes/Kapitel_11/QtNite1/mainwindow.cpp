#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <XnOpenNI.h>
#include <XnCppWrapper.h>


MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    myKinect=new QKinectThread();
    connect(myKinect,SIGNAL(dataNotification()),this,SLOT(kinectDataHere()));
    myKinect->start();

}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::kinectDataHere()
{
    myKinect->myMutex.lock();
    myColorImage=myKinect->myExportRGB;
    myDepthImage=myKinect->myExportDepth;
    myKinect->myMutex.unlock();
    this->repaint();
}

void MainWindow::paintEvent(QPaintEvent *pe)
{
    QPainter newPainter(this);
    newPainter.drawImage(0,0,myColorImage);
    newPainter.drawImage(641,0,myDepthImage);
}
