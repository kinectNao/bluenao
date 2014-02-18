#-------------------------------------------------
#
# Project created by QtCreator 2013-02-19T18:11:50
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = QtNite1
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    qkinectthread.cpp

HEADERS  += mainwindow.h \
    qkinectthread.h

FORMS    += mainwindow.ui


#Kinect files
INCLUDEPATH += "/usr/include/ni" \
"/usr/include/nite"

LIBS += -L"/usr/lib" \
-l"OpenNI"
