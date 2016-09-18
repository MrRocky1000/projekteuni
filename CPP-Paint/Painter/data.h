#ifndef DATA_H
#define DATA_H

#include <vector>
#include <iostream>
#include <image.h>
#include <QFile>

namespace my
{

    class data
    {
    public:
        data(image actual = {});

        void setNew(image newi);
        image undo();
        image redo();
        void save(QString fileName);
        image load(QString fileName);

        int getHistorySize();

    private:
        image actual;
        int pointer;
        std::vector<image> savedata;
    };

}

#endif // DATA_H
