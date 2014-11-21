#pragma once

#include "resource.h"

#define STRINGIZE2(s) #s
#define STRINGIZE(s) STRINGIZE2(s)

#define VERSION_MAJOR               1
#define VERSION_MINOR               0
#define VERSION_REVISION            0
#define VERSION_BUILD               0

#define VER_FILE_VERSION            VERSION_MAJOR, VERSION_MINOR, VERSION_REVISION, VERSION_BUILD
#define VER_FILE_VERSION_STR        STRINGIZE(VERSION_MAJOR)        \
    "." STRINGIZE(VERSION_MINOR)    \
    "." STRINGIZE(VERSION_REVISION) \
    "." STRINGIZE(VERSION_BUILD)    \

#define VER_PRODUCT_VERSION         VER_FILE_VERSION
#define VER_PRODUCT_VERSION_STR     VER_FILE_VERSION_STR
#define VER_PRODUCTNAME_STR         "Version Code Sample"
#define VER_COPYRIGHT_STR           "Copyright (C) 2012"

#ifdef _DEBUG
#define VER_VER_DEBUG             VS_FF_DEBUG
#else
#define VER_VER_DEBUG             0
#endif

#define VER_FILEOS                  VOS_NT_WINDOWS32
#define VER_FILEFLAGS               VER_VER_DEBUG
#define VER_FILETYPE                VFT_APP
