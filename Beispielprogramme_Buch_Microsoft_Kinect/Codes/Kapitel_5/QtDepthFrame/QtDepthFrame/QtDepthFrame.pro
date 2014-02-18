#-------------------------------------------------
#
# Project created by QtCreator 2012-09-30T15:00:46
#
#-------------------------------------------------

QT       += core gui

TARGET = QtDepthFrame
CONFIG += i386

DEFINES += USE_FREENECT
LIBS += -lfreenect

TEMPLATE = app

SOURCES += main.cpp\
        mainwindow.cpp \
    qfreenect.cpp \
    qfreenectthread.cpp

HEADERS  += mainwindow.h \
    qfreenect.h \
    qfreenectthread.h \
    qfreenect.h \
    qfreenectthread.h

FORMS    += mainwindow.ui
