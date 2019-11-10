file(GLOB SOURCES *.c)
file(GLOB HEADERS *.h)

add_library(zlib STATIC ${SOURCES} ${HEADERS})
