cmake_minimum_required(VERSION 3.10)

set(TARGET FreeImage)
set(SOURCES
    Source/FreeImage/BitmapAccess.cpp
    Source/FreeImage/CacheFile.cpp
    Source/FreeImage/ColorLookup.cpp
    Source/FreeImage/Conversion16_555.cpp
    Source/FreeImage/Conversion16_565.cpp
    Source/FreeImage/Conversion24.cpp
    Source/FreeImage/Conversion32.cpp
    Source/FreeImage/Conversion4.cpp
    Source/FreeImage/Conversion8.cpp
    Source/FreeImage/Conversion.cpp
    Source/FreeImage/ConversionFloat.cpp
    Source/FreeImage/ConversionRGB16.cpp
    Source/FreeImage/ConversionRGBA16.cpp
    Source/FreeImage/ConversionRGBAF.cpp
    Source/FreeImage/ConversionRGBF.cpp
    Source/FreeImage/ConversionType.cpp
    Source/FreeImage/ConversionUINT16.cpp
    Source/FreeImage/FreeImageC.c
    Source/FreeImage/FreeImage.cpp
    Source/FreeImage/FreeImageIO.cpp
    Source/FreeImage/GetType.cpp
    Source/FreeImage/Halftoning.cpp
    Source/FreeImage/J2KHelper.cpp
    Source/FreeImage/J2KHelper.h
    Source/FreeImage/LFPQuantizer.cpp
    Source/FreeImage/MemoryIO.cpp
    Source/FreeImage/MNGHelper.cpp
    Source/FreeImage/MultiPage.cpp
    Source/FreeImage/NNQuantizer.cpp
    Source/FreeImage/PixelAccess.cpp
    Source/FreeImage/PSDParser.cpp
    Source/FreeImage/PSDParser.h
    Source/FreeImage/TIFFLogLuv.cpp
    Source/FreeImage/tmoColorConvert.cpp
    Source/FreeImage/tmoDrago03.cpp
    Source/FreeImage/tmoFattal02.cpp
    Source/FreeImage/tmoReinhard05.cpp
    Source/FreeImage/ToneMapping.cpp
    Source/FreeImage/WuQuantizer.cpp
    Source/FreeImage/ZLibInterface.cpp

    Source/Metadata/Exif.cpp
    Source/Metadata/FIRational.cpp
    Source/Metadata/FIRational.h
    Source/Metadata/FreeImageTag.cpp
    Source/Metadata/FreeImageTag.h
    Source/Metadata/IPTC.cpp
    Source/Metadata/TagConversion.cpp
    Source/Metadata/TagLib.cpp
    Source/Metadata/XTIFF.cpp

    Source/FreeImageToolkit/Background.cpp
    Source/FreeImageToolkit/BSplineRotate.cpp
    Source/FreeImageToolkit/Channels.cpp
    Source/FreeImageToolkit/ClassicRotate.cpp
    Source/FreeImageToolkit/Colors.cpp
    Source/FreeImageToolkit/CopyPaste.cpp
    Source/FreeImageToolkit/Display.cpp
    Source/FreeImageToolkit/Filters.h
    Source/FreeImageToolkit/Flip.cpp
    Source/FreeImageToolkit/JPEGTransform.cpp
    Source/FreeImageToolkit/MultigridPoissonSolver.cpp
    Source/FreeImageToolkit/Rescale.cpp
    Source/FreeImageToolkit/Resize.cpp
    Source/FreeImageToolkit/Resize.h
)

set(PLUGIN_SOURCES
    Source/FreeImage/PluginBMP.cpp
    Source/FreeImage/Plugin.cpp
    Source/FreeImage/PluginCUT.cpp
    Source/FreeImage/PluginDDS.cpp
    Source/FreeImage/PluginG3.cpp
    Source/FreeImage/PluginGIF.cpp
    Source/FreeImage/PluginHDR.cpp
    Source/FreeImage/PluginICO.cpp
    Source/FreeImage/PluginIFF.cpp
    Source/FreeImage/PluginJ2K.cpp
    Source/FreeImage/PluginJNG.cpp
    Source/FreeImage/PluginKOALA.cpp
    Source/FreeImage/PluginPCD.cpp
    Source/FreeImage/PluginPCX.cpp
    Source/FreeImage/PluginPFM.cpp
    Source/FreeImage/PluginPICT.cpp
    Source/FreeImage/PluginPNM.cpp
    Source/FreeImage/PluginPSD.cpp
    Source/FreeImage/PluginRAS.cpp
    Source/FreeImage/PluginSGI.cpp
    Source/FreeImage/PluginTARGA.cpp
    Source/FreeImage/PluginWBMP.cpp
    Source/FreeImage/PluginXBM.cpp
    Source/FreeImage/PluginXPM.cpp
)

file(GLOB INCLUDES Source/*.h)

include(FindPkgConfig)
message(STATUS "Finding optional FreeImage modules:")

function(find_installed_library LIB_NAME PLUGIN_NAME)
    # Find requested library, if any
    pkg_search_module(${LIB_NAME} IMPORTED_TARGET ${LIB_NAME})
    if(${LIB_NAME}_FOUND)
        message(STATUS "    ${LIB_NAME} module found: ${${LIB_NAME}_VERSION}")
        set(PLUGIN_SOURCES ${PLUGIN_SOURCES} Source/FreeImage/Plugin${PLUGIN_NAME}.cpp PARENT_SCOPE)
        set(PLUGIN_DEPS ${PLUGIN_DEPS} PkgConfig::${LIB_NAME} PARENT_SCOPE)
        set(FREEIMAGE_HAVE_${PLUGIN_NAME} 1)
    else()
        message(STATUS "    ${LIB_NAME} module not found, building without ${LIB_NAME} support")
        unset(FREEIMAGE_HAVE_${PLUGIN_NAME})
    endif()
endfunction()

find_installed_library(OpenEXR EXR)
find_installed_library(JXR JXR)
find_installed_library(libtiff-4 TIFF)
find_installed_library(libpng PNG)
find_installed_library(libjpeg JPEG)
find_installed_library(libmng MNG)
find_installed_library(libopenjp2 JP2)

# currently does not "find" properly on Ubuntu
#find_installed_library(libraw RAW)

# webp needs special handling since it requires two .pc files for a single plugin
pkg_check_modules(WebP IMPORTED_TARGET libwebp libwebpmux)
if(WebP_FOUND)
    message(STATUS "    WebP module found: ${WebP_VERSION}")
    list(APPEND PLUGIN_SOURCES Source/FreeImage/PluginWebP.cpp)
    list(APPEND PLUGIN_DEPS PkgConfig::WebP)
    set(FREEIMAGE_HAVE_WebP 1)
else()
    message(STATUS "    WebP module not found, building without WebP support")
    unset(FREEIMAGE_HAVE_WebP)
endif()


add_library(${TARGET} STATIC ${SOURCES} ${PLUGIN_SOURCES} ${INCLUDES})

target_include_directories(${TARGET}
    PRIVATE
        ${CMAKE_CURRENT_LIST_DIR}/Source
    PUBLIC
        $<BUILD_INTERFACE:${CMAKE_CURRENT_LIST_DIR}/Source>
)

target_link_libraries(${TARGET}
    PRIVATE
        ${PLUGIN_DEPS}
)
