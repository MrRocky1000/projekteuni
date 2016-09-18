#include "image.h"

namespace my {

image::image( size_t width, size_t height, std::uint32_t color )
: width_   { width }
, height_  { height }
, bgColor_ { color }
, data_    ( width * height, color )
{}

}
