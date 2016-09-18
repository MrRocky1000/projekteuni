#include "mainwindow.h"
#include "ui_mainwindow.h"

#include "mylabel.h"
#include <iostream>

#include <QPixmap>
#include <QFileDialog>

using namespace std;

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow),
    pinsel(1),
    p_Color(0xff000000),
    bg_Color(0xffffffff),
    label_ {new MyLabel(this)},
    image_ {471,371,bg_Color},
    save_{image_},
    forms_clicked{0},
    first_click{std::vector<int>()}
{
    ui->setupUi(this);

    // Updates the pixmap shown by label from the data of our custom image
        //save für die erste Mausposition
        int x1 = 0;
        int y1 = 0;

        auto update_label = [this]
        {
           refreshImg();
        };

        connect( label_, &MyLabel::onMouseMove, [update_label,this](int x, int y)
        {
            if(!forms_clicked == 0)return;

            std::cout << "mouse move: " << x << ", " << y << std::endl;
            if(x >= image_.width()-2 || x < 2 || y >= image_.height()-2 || y < 2){
                return;
            }
            switch(pinsel)
            {
                case 1:
                    image_.set_pixel( x, y, p_Color );
                    break;
                case 2:
                    image_.set_pixel( x, y, p_Color );
                    image_.set_pixel( x+1, y, p_Color );
                    image_.set_pixel( x, y+1, p_Color );
                    image_.set_pixel( x+1, y+1, p_Color );
                    break;
                case 3:
                    image_.set_pixel( x-1, y-1, p_Color );
                    image_.set_pixel( x, y-1, p_Color );
                    image_.set_pixel( x+1, y-1, p_Color );
                    image_.set_pixel( x-1, y, p_Color );
                    image_.set_pixel( x, y, p_Color );
                    image_.set_pixel( x+1, y, p_Color );
                    image_.set_pixel( x-1, y+1, p_Color );
                    image_.set_pixel( x, y+1, p_Color );
                    image_.set_pixel( x+1, y+1, p_Color );
                    break;
                case 4:
                    image_.set_pixel( x-1, y-1, p_Color );
                    image_.set_pixel( x, y-1, p_Color );
                    image_.set_pixel( x+1, y-1, p_Color );
                    image_.set_pixel( x+2, y-1, p_Color );
                    image_.set_pixel( x-1, y, p_Color );
                    image_.set_pixel( x, y, p_Color );
                    image_.set_pixel( x+1, y, p_Color );
                    image_.set_pixel( x+2, y, p_Color );
                    image_.set_pixel( x-1, y+1, p_Color );
                    image_.set_pixel( x, y+1, p_Color );
                    image_.set_pixel( x+1, y+1, p_Color );
                    image_.set_pixel( x+2, y+1, p_Color );
                    image_.set_pixel( x-1, y+2, p_Color );
                    image_.set_pixel( x, y+2, p_Color );
                    image_.set_pixel( x+1, y+2, p_Color );
                    image_.set_pixel( x+2, y+2, p_Color );
                    break;
                case 5:
                    image_.set_pixel( x-2, y-2, p_Color );
                    image_.set_pixel( x-1, y-2, p_Color );
                    image_.set_pixel( x, y-2, p_Color );
                    image_.set_pixel( x+1, y-2, p_Color );
                    image_.set_pixel( x+2, y-2, p_Color );
                    image_.set_pixel( x-2, y-1, p_Color );
                    image_.set_pixel( x-1, y-1, p_Color );
                    image_.set_pixel( x, y-1, p_Color );
                    image_.set_pixel( x+1, y-1, p_Color );
                    image_.set_pixel( x+2, y-1, p_Color );
                    image_.set_pixel( x-2, y, p_Color );
                    image_.set_pixel( x-1, y, p_Color );
                    image_.set_pixel( x, y, p_Color );
                    image_.set_pixel( x+1, y, p_Color );
                    image_.set_pixel( x+2, y, p_Color );
                    image_.set_pixel( x-2, y+1, p_Color );
                    image_.set_pixel( x-1, y+1, p_Color );
                    image_.set_pixel( x, y+1, p_Color );
                    image_.set_pixel( x+1, y+1, p_Color );
                    image_.set_pixel( x+2, y+1, p_Color );
                    image_.set_pixel( x-2, y+2, p_Color );
                    image_.set_pixel( x-1, y+2, p_Color );
                    image_.set_pixel( x, y+2, p_Color );
                    image_.set_pixel( x+1, y+2, p_Color );
                    image_.set_pixel( x+2, y+2, p_Color );
                    break;
            }
            update_label();
        });

        connect( label_, &MyLabel::onMouseDown, [update_label,this,x1,y1](int x, int y)
        {
           std::cout << "mouse down @ " << x << ", " << y << std::endl;
           if(!forms_clicked == 0){
               int size = ui->formsEdit->text().toInt();
               switch(forms_clicked){
                    //Rect
                    case 1:
                        for(int i=0;i<size;i++){
                            image_.set_pixel(x,y+i,p_Color);
                            image_.set_pixel(x+size,y+i,p_Color);
                            image_.set_pixel(x+i,y,p_Color);
                            image_.set_pixel(x+i,y+size,p_Color);
                        }
                        forms_clicked = 0;
                        refreshImg();
                        break;
                    //circle
                    case 3:
                        std::cout << " circle with center " << x << "|" << y << " and radius " << size << std::endl;
                        forms_clicked = 0;
                        break;
                    //Line
                    case 4:
                        first_click.push_back(x);
                        first_click.push_back(y);
               }
           }

           update_label();
        });

        connect( label_, &MyLabel::onMouseUp, [update_label,this,x1,y1](int x, int y)
        {
           std::cout << "mouse up @ " << x << ", " << y << std::endl;
           if(forms_clicked == 4){
               int x1 = first_click.at(0);
               int y1 = first_click.at(1);
               std::cout << " line from " << x1 << "|" << y1 << " to " << x << "|" << y << std::endl;
               first_click.pop_back();
               first_click.pop_back();
               forms_clicked = 0;
           }
           save_.setNew(image_);
           update_label();
        });


        update_label();

        label_ -> setParent(ui->paintCanvas);

        //Buttons
        connect(ui->bgButton, SIGNAL(clicked()), this, SLOT(changeBgColor()));
        connect(ui->pButton, SIGNAL(clicked()), this, SLOT(changePColor()));
        connect(ui->clearButton, SIGNAL(clicked()),this, SLOT(clearCanvas()));
        connect(ui->quitButton, SIGNAL(clicked()),this,SLOT(close()));
        connect(ui->saveButton, SIGNAL(clicked()),this,SLOT(save()));
        connect(ui->loadButton, SIGNAL(clicked()),this,SLOT(load()));
        connect(ui->undoButton, SIGNAL(clicked()),this,SLOT(undo()));
        connect(ui->redoButton, SIGNAL(clicked()),this,SLOT(redo()));
        connect(ui->squareButton, SIGNAL(clicked()),this,SLOT(addSquare()));
        connect(ui->circleButton, SIGNAL(clicked()),this,SLOT(addCircle()));
        connect(ui->lineButton, SIGNAL(clicked()),this,SLOT(addLine()));
        //Slider
        connect(ui->pinselSlider,SIGNAL(valueChanged(int)),this,SLOT(changePinsel()));

}
MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::refreshImg(){
    auto qimage = QImage(
       reinterpret_cast<uchar const*>(image_.data()), image_.width(), image_.height(),
       sizeof(my::image::rgba_t)*image_.width(), QImage::Format_ARGB32
    );
    label_->setPixmap(QPixmap::fromImage( qimage ));
    updateHistoryLabel();
}

void MainWindow::clearCanvas(){
    std::cout << "clear " << std::endl;
    image_.clearImage();
    save_.setNew(image_);
    //update_label
    refreshImg();
}

void MainWindow::changeBgColor(){
    std::cout << "change Background-Color " << std::endl;
    if(ui->bgColorEdit->text().isEmpty()){
        return;
    }
    string c_string = ui->bgColorEdit->text().toStdString();
    std::uint32_t color = std::stoul(c_string,nullptr,16);
    std::cout << color << std::endl;
    image_.setBackgroundColor(color);
    save_.setNew(image_);
    refreshImg();
}

void MainWindow::changePColor(){
    std::cout << "change Pinsel-Color " << std::endl;
    if(ui->pColorEdit->text().isEmpty()){
        return;
    }
    string c_string = ui->pColorEdit->text().toStdString();
    std::uint32_t color = std::stoul(c_string,nullptr,16);
    p_Color = color;
}

void MainWindow::changePinsel(){
    std::cout << "change Pinsel " << std::endl;
    pinsel = ui->pinselSlider->value();
    std::cout << pinsel << std::endl;
}

void MainWindow::save(){
    std::cout << "save " << std::endl;
    QString fileName = QFileDialog::getSaveFileName(this,
                tr("Save File"), "",
                tr("Picture (*.png);;All Files (*)"));
    if (fileName.isEmpty()) return;
    else save_.save(fileName);
}

void MainWindow::load(){
    std::cout << "load " << std::endl;
    QString fileName = QFileDialog::getOpenFileName(this,
                tr("Load File"), "",
                tr("Picture (*.png);;All Files (*)"));
    if (fileName.isEmpty()) return;
    else image_ = save_.load(fileName);
    refreshImg();
}

void MainWindow::undo(){
    std::cout << "undo " << std::endl;
    image_ = save_.undo();
    refreshImg();
}

void MainWindow::redo(){
    std::cout << "redo " << std::endl;
    image_ = save_.redo();
    refreshImg();
}

void MainWindow::updateHistoryLabel(){
    ui->historyLabel->setText(QString::number(save_.getHistorySize()-1));
}

void MainWindow::addSquare(){
    forms_clicked = 1;
}

void MainWindow::addCircle(){
    forms_clicked = 3;
}

void MainWindow::addLine(){
    forms_clicked = 4;
}
