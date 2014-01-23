// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the WEBAPPUTIL_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// WEBAPPUTIL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef WEBAPPUTIL_EXPORTS
#define WEBAPPUTIL_API __declspec(dllexport)
#else
#define WEBAPPUTIL_API __declspec(dllimport)
#endif

extern "C" WEBAPPUTIL_API char * DBConnstr(char *provider, char *server, char *userid, char *password);

extern "C" WEBAPPUTIL_API char * DBGetTables(char *connectStr, char *dbName, char *schemaName, bool systemTables);

extern "C" WEBAPPUTIL_API char * DBGetViews(char *connectStr, char *dbName, char *schemaName, bool systemView);

extern "C" WEBAPPUTIL_API char * DBGetIndexes(char *connectStr, char *tableName);

extern "C" WEBAPPUTIL_API char * DBGetProcs(char *connectStr, char *dbName, char *schemaName, char *procName=NULL);

extern "C" WEBAPPUTIL_API char * DBGetQuote(char *connectStr);
