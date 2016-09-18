#include "data.h"
#include "image.h"

#include <iostream>

#include <QFile>
#include <QFileDialog>
#include <QMessageBox>

namespace my {

    data::data(my::image img):
        actual{img},
        pointer{0},
        savedata{img}
    {}

    void data::setNew(image newi){
        std::cout << "set new " << std::endl;
        if(savedata.size() > pointer+1){
            std::cout << "delete old undos" << std::endl;
            std::vector<image> newsave = savedata;
            for(int i = savedata.size();i > pointer+1;i--){
                newsave.pop_back();
            }
            savedata = newsave;
        }
        savedata.push_back(newi);
        pointer++;
        actual = newi;
    }

    image data::undo(){
        if(pointer == 0){
            std::cout << "nothing to undo " << std::endl;
            return actual;
        }
        pointer--;
        actual = savedata[pointer];
        return actual;
    }

    image data::redo(){
        if(pointer+1 == savedata.size()){
            std::cout << "nothing to redo " << std::endl;
            return actual;
        }
        pointer++;
        actual = savedata[pointer];
        return actual;
    }

    void data::save(QString fileName){
        std::cout << "save met " << std::endl;
        auto qimage = QImage(
           reinterpret_cast<uchar const*>(actual.data()), actual.width(), actual.height(),
           sizeof(my::image::rgba_t)*actual.width(), QImage::Format_ARGB32
        );

        QFile file(fileName);
        std::cout << "open File " << std::endl;
        if (!file.open(QIODevice::WriteOnly)) {
            std::cout << "OPEN ERROR " << std::endl;
            std::cout << file.errorString().toStdString() << std::endl;
            return;
        }
        std::cout << "Start Open " << std::endl;
        QDataStream out(&file);
        out.setVersion(QDataStream::Qt_DefaultCompiledVersion);
        std::cout << "start write " << std::endl;
        out << QPixmap::fromImage( qimage );
        std::cout << "finish write " << std::endl;
    }

    image data::load(QString fileName){
        std::cout << "load met " << std::endl;
        QFile file(fileName);
        std::cout << "open File " << std::endl;
        if (!file.open(QIODevice::ReadOnly)) {
            std::cout << "OPEN ERROR " << std::endl;
            std::cout << file.errorString().toStdString() << std::endl;
            return actual;
        }
        QDataStream in(&file);
        std::cout << "start read " << std::endl;
        QPixmap loaded;
        in >> loaded;
        QImage loadimg = loaded.toImage();



        std::cout << (uint32_t) loadimg.pixel(0,0) << std::endl;


        using rgba_t = std::uint32_t;
        //newi.data() = (reinterpret_cast<rgba_t const*> (loadimg.bits()));
        //std::vector<rgba_t> inew(reinterpret_cast<rgba_t> (loadimg.bits()));
        image newi = actual;
        //newi.data_ = {reinterpret_cast<rgba_t> (loadimg.bits())};
        //std::replace(newi.data_.begin(), newi.data_.end(),newi.data(),reinterpret_cast<rgba_t const*> (loadimg.bits()));
        std::cout << "finish read " << std::endl;

        setNew(newi);

        return newi;
    }

    int data::getHistorySize(){
        return savedata.size();
    }
}
