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
    Source/FreeImage/PluginJP2.cpp
    Source/FreeImage/PluginJPEG.cpp
    Source/FreeImage/PluginKOALA.cpp
    Source/FreeImage/PluginMNG.cpp
    Source/FreeImage/PluginPCD.cpp
    Source/FreeImage/PluginPCX.cpp
    Source/FreeImage/PluginPFM.cpp
    Source/FreeImage/PluginPICT.cpp
    Source/FreeImage/PluginPNG.cpp
    Source/FreeImage/PluginPNM.cpp
    Source/FreeImage/PluginPSD.cpp
    Source/FreeImage/PluginRAS.cpp
    Source/FreeImage/PluginRAW.cpp
    Source/FreeImage/PluginSGI.cpp
    Source/FreeImage/PluginTARGA.cpp
    Source/FreeImage/PluginTIFF.cpp
    Source/FreeImage/PluginWBMP.cpp
    Source/FreeImage/PluginWebP.cpp
    Source/FreeImage/PluginXBM.cpp
    Source/FreeImage/PluginXPM.cpp
)

file(GLOB INCLUDES Source/*.h)

include(FindPkgConfig)

# Find installed OpenEXR library, if any
pkg_search_module(EXR OpenEXR)
if(EXR_MODULE_NAME)
    message(STATUS "OpenEXR module found: ${EXR_MODULE_NAME}")
    list(APPEND PLUGIN_SOURCES Source/FreeImage/PluginEXR.cpp)
    list(APPEND PLUGIN_DEPS ${EXR_MODULE_NAME})
endif()

# Find installed JXR library, if any
pkg_search_module(JXR JXR)
if(JXR_MODULE_NAME)
    message(STATUS "JXR module found: ${JXR_MODULE_NAME}")
    list(APPEND PLUGIN_SOURCES Source/FreeImage/PluginJXR.cpp)
    list(APPEND PLUGIN_DEPS ${JXR_MODULE_NAME})
endif()

add_library(${TARGET} STATIC ${SOURCES} ${PLUGIN_SOURCES} ${INCLUDES})

target_include_directories(${TARGET}
    PRIVATE
        ${CMAKE_CURRENT_LIST_DIR}/Source

)
