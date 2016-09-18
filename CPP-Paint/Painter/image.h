#ifndef IMAGE_H
#define IMAGE_H

#pragma once

#include <vector>
#include <iostream>
#include <algorithm>

namespace my
{

   class image
   {
   public:

      using rgba_t = std::uint32_t;


       image( size_t width = {} , size_t height = {}, std::uint32_t color = {} );

      void set_pixel( size_t x, size_t y, rgba_t pixel )
      {
         data_[ y*width_ + x ] = pixel;
      }

      auto width() const  -> size_t          { return width_; }
      auto height() const -> size_t          { return height_; }
      auto data() const   -> rgba_t const*   { return data_.data(); }
      auto getData() const -> std::vector<rgba_t> { return data_; }

      void clearImage()
      {
         for(size_t i=0; i<width_*height_; i++){
             data_[i] = bgColor_;
         }
      }

      void setBackgroundColor(std::uint32_t color)
      {
         std::replace(data_.begin(), data_.end(), bgColor_, color);
         bgColor_ = color;

      }

      std::vector<rgba_t>  data_;
   private:

      size_t  width_;
      size_t  height_;
      std::uint32_t bgColor_;


   };

}

#endif // IMAGE_H
