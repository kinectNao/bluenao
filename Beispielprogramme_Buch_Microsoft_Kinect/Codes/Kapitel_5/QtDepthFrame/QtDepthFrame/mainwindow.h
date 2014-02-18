#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "qfreenect.h"
#include <QPainter>
#include <QPixmap>
#include <QImage>

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
    void videoDataReady(uint8_t* myRGBBuffer);
    void depthDataReady(uint16_t* myDepthBuffer);

private:
    Ui::MainWindow *ui;
    QFreenect* myFreenect;
    QImage* myVideoImage;
    QImage* myDepthImage;
};

#endif // MAINWINDOW_H
