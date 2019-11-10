file(GLOB SOURCES *.c)
file(GLOB HEADERS *.h)

add_library(jpeg STATIC ${SOURCES} ${HEADERS})
