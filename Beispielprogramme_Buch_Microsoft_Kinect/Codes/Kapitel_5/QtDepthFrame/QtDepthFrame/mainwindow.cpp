#include "mainwindow.h"
#include <stdlib.h>
#include <QDebug>
#include <QtEndian>
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    myFreenect=new QFreenect(this);
    connect(myFreenect,SIGNAL(videoDataReady(uint8_t*)),this,SLOT(videoDataReady(uint8_t*)));
    connect(myFreenect,SIGNAL(depthDataReady(uint16_t*)),this,SLOT(depthDataReady(uint16_t*)));
    myFreenect->myWantDataFlag=true;
    myFreenect->myFlagDFrameTaken=true;
    myFreenect->myFlagFrameTaken=true;
    myVideoImage=new QImage(640,480, QImage::Format_RGB32);
    myDepthImage=new QImage(640,480, QImage::Format_RGB32);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::videoDataReady(uint8_t* myRGBBuffer)
{
    if(myVideoImage!=NULL)delete myVideoImage;
    myVideoImage=new QImage(640,480, QImage::Format_RGB32);
    unsigned char r, g, b;
    for(int x=2; x<640;x++)
    {
         for(int y=0;y<480;y++)
         {
            r=(myRGBBuffer[3*(x+y*640)+0]);
            g=(myRGBBuffer[3*(x+y*640)+1]);
            b=(myRGBBuffer[3*(x+y*640)+2]);
            myVideoImage->setPixel(x,y,qRgb(r,g,b));
         }

    }
    repaint();
    myFreenect->myFlagFrameTaken=true;
    free(myRGBBuffer);
}

void MainWindow::depthDataReady(uint16_t* myDepthBuffer)
{
    if(myDepthImage!=NULL)delete myDepthImage;
    myDepthImage=new QImage(640,480,QImage::Format_RGB32);
    unsigned char r, g, b;
    for(int x=2; x<640;x++)
    {
         for(int y=0;y<480;y++)
         {
            int calcval=(myDepthBuffer[(x+y*640)]);

            if(calcval==FREENECT_DEPTH_MM_NO_VALUE)
            {
                r=255;
                g=0;
                b=0;
            }
            else if(calcval>1000 && calcval < 2000)
            {
                QRgb aVal=myVideoImage->pixel(x,y);
                r=qRed(aVal);
                g=qGreen(aVal);
                b=qBlue(aVal);
            }
            else
            {
                r=0;
                g=0;
                b=0;
            }
            myDepthImage->setPixel(x,y,qRgb(r,g,b));
         }
    }
    repaint();
    myFreenect->myFlagDFrameTaken=true;
    free(myDepthBuffer);
}



void MainWindow::paintEvent(QPaintEvent *pe)
{
        if(myVideoImage!=NULL && myDepthImage!=NULL)
        {
               QPainter p(this);
               p.drawImage(ui->widget->pos().rx(),ui->widget->pos().ry(), *myDepthImage);
        }
}

