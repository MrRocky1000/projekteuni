#-------------------------------------------------
#
# Project created by QtCreator 2016-07-26T12:29:14
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = Painter
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    image.cpp \
    mylabel.cpp \
    data.cpp \
    pinsel.cpp

HEADERS  += mainwindow.h \
    image.h \
    mylabel.h \
    data.h \
    pinsel.h

FORMS    += mainwindow.ui
