#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "image.h"
#include "data.h"

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

    int pinsel;
    std::uint32_t p_Color;
    std::uint32_t bg_Color;
    class MyLabel* label_;
    my::image image_;
    my::data save_;
    int forms_clicked;
    std::vector<int> first_click;



public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    void updateHistoryLabel();

private:
    Ui::MainWindow *ui;
    void refreshImg();

public slots:
   void clearCanvas();
   void changeBgColor();
   void changePColor();
   void changePinsel();
   void save();
   void load();
   void undo();
   void redo();
   void addSquare();
   void addCircle();
   void addLine();
};

#endif // MAINWINDOW_H
