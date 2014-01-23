// WebAppUtil.cpp : Defines the entry point for the DLL application.
//
// most of this code derived from the DBVIEWER smaple code
//

#include "stdafx.h"
#include "WebAppUtil.h"

#define RESULT_SIZE 40960

/*
BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
    return TRUE;
}
*/

void getHRtext(HRESULT hr, char *result)
{
	USES_CONVERSION;
	BSTR	bstrDescription   = NULL;
    IErrorInfo *pIErrorInfo   = NULL;
   	// Obtain the current error object, if any, by using the
    // Automation GetErrorInfo function, which will give
    // us back an IErrorInfo interface pointer if successful.
    HRESULT lhr = GetErrorInfo(0, &pIErrorInfo);

	if (SUCCEEDED(lhr) && pIErrorInfo ) {
		lhr = pIErrorInfo->GetDescription(&bstrDescription);	// Get the description of this error.
		if (SUCCEEDED(lhr)) {
			sprintf (result, "ERROR: = %X, ",hr);
			strcat (result, OLE2A(bstrDescription));
			::SysFreeString(bstrDescription);			
		}
	}
}


//
// Build the Database connection string
//
extern "C" WEBAPPUTIL_API char * DBConnstr(char *provider, char *server, char *userid, char *password)
{	// char* are really C# string(s)
	USES_CONVERSION;
	CLSID		clsid;
	HRESULT		hr;

	IUnknown *pIUnknown = NULL;
	BOOL *pbValue		= FALSE;
	IDataInitialize*    pIDataInitialize = NULL;
	IDBInitialize *		pIDBInitialize = NULL;
	IDBProperties*		pIDBProperties = NULL;

	char *result = (char*)CoTaskMemAlloc( 1024 );
	//Invoke the OLE DB Service Components with the MSDAINITIALIZE
	//provider. Session pooling is enabled while the
	//IDataInitialize interface pointer is held...
	try {
		hr = CoCreateInstance(CLSID_MSDAINITIALIZE, NULL, CLSCTX_ALL, //CLSCTX_INPROC_SERVER,	
							IID_IDataInitialize, (void**)&pIDataInitialize);
		if (FAILED(hr)) throw(1);
		// get the CLSID for this provider
		hr = CLSIDFromProgID(A2W(provider), &clsid);
		if (FAILED(hr)) throw(2);
		const ULONG nProps = 4;
		DBPROP InitProperties[nProps];
		DBPROPSET rgInitPropSet;	
		for (ULONG i = 0; i < nProps; i++)
		{
			VariantInit(&InitProperties[i].vValue);
			InitProperties[i].dwOptions = DBPROPOPTIONS_REQUIRED;
			InitProperties[i].colid = DB_NULLID;
		}	//Level of prompting performed to complete the connection process.
		InitProperties[0].dwPropertyID = DBPROP_INIT_PROMPT;
		InitProperties[0].vValue.vt = VT_I2;
		InitProperties[0].vValue.iVal = DBPROMPT_NOPROMPT;		// no prompting !!!
		InitProperties[1].dwPropertyID = DBPROP_INIT_DATASOURCE;
		InitProperties[1].vValue.vt = VT_BSTR;
		InitProperties[1].vValue.bstrVal = A2BSTR(server);		// Server or filename
		InitProperties[2].dwPropertyID = DBPROP_AUTH_USERID;
		InitProperties[2].vValue.vt = VT_BSTR;
		InitProperties[2].vValue.bstrVal = A2BSTR(userid);		// Userid.
		InitProperties[3].dwPropertyID = DBPROP_AUTH_PASSWORD;
		InitProperties[3].vValue.vt = VT_BSTR;
		InitProperties[3].vValue.bstrVal = A2BSTR(password);	// password
		//Assign the property structures to the property set.
		rgInitPropSet.guidPropertySet = DBPROPSET_DBINIT;
		rgInitPropSet.cProperties = nProps;
		rgInitPropSet.rgProperties = InitProperties;

		//Use the IDataInitialize interface to load the SQLOLEDB provider and create an IDBInitialize interface.
		hr = pIDataInitialize->CreateDBInstance(clsid, NULL, CLSCTX_ALL, NULL, IID_IDBInitialize, (IUnknown**)&pIDBInitialize);		
		if (FAILED(hr)) throw(3);
		hr = pIDBInitialize->QueryInterface(IID_IDBProperties, (void **)&pIDBProperties);
		if (FAILED(hr)) throw(4);
		hr = pIDBProperties->SetProperties(1, &rgInitPropSet);
		pIDBProperties->Release();	
		hr = pIDBInitialize->Initialize();
		SysFreeString(InitProperties[1].vValue.bstrVal);
		SysFreeString(InitProperties[2].vValue.bstrVal);
		SysFreeString(InitProperties[3].vValue.bstrVal);
		if (FAILED(hr)) throw(5);
		// now with a connection, extract the connection string
		LPWSTR x;
		pIDataInitialize->GetInitializationString((IUnknown*)pIDBInitialize, true, &x);
		pIDBInitialize->Uninitialize();
		pIDBInitialize->Release();
		pIDataInitialize->Release();
		CoTaskMemFree(result);
//		CoUninitialize();
		return (W2A(x));
	}
	catch (...)
	{
		getHRtext(hr, result);
		return result;
	}
}

//
// Get the Procedure Names or the Procedure Parameters
//
extern "C" WEBAPPUTIL_API char *DBGetProcs(char *connectStr, char *dbName, char *schemaName, char *procName)
{
	USES_CONVERSION;

	bool		rc = true;
	CDataSource m_Connect;
	CSession    m_Session;
	char *result = (char*)CoTaskMemAlloc( RESULT_SIZE );


	strcpy(result, "ERROR: trying m_Connect.Open(connectStr");
	try {
		if (m_Connect.OpenFromInitializationString(A2OLE(connectStr)) != S_OK)
		{
			strcpy(result, "ERROR: m_Connect.Open(connectStr) failed");
			return result;
		}
		if (m_Session.Open(m_Connect) != S_OK)
		{
			strcpy(result, "ERROR: m_Session.Open(m_Connect) failed");
			return result;
		}

		// List the procedures in the data source
		if (procName == NULL) 
		{
			CProcedures *pProcSet = new CProcedures;
			strcpy(result, "ERROR: pProcSet->Open(m_Session, ...) failed");
			if (pProcSet->Open(m_Session, dbName, schemaName, NULL) != S_OK)
				return result;
			*result = 0;
			while(pProcSet->MoveNext() == S_OK)
			{
				strcat (result,pProcSet->m_szName);
				strcat (result,"|");
			}
			pProcSet->Close();
			delete pProcSet;
			pProcSet = NULL;
		}
		else
		{
			CProcedureParameters *pProcParmInfo = new CProcedureParameters;
			strcpy(result, "pProcParmInfo->Open(m_Session, ...) failed");
			if (pProcParmInfo->Open(m_Session, dbName, schemaName, procName) != S_OK)
				return result;
			*result = 0;
			while(pProcParmInfo->MoveNext() == S_OK)
			{
				strcat (result,pProcParmInfo->m_szParameterName);
				strcat (result,"|");
			}
			pProcParmInfo->Close();
			delete pProcParmInfo;
			pProcParmInfo = NULL;
		}


		if (*result == 0)
			strcpy(result, "");
		else
			*(result+(strlen(result)-1)) = 0x00;	// remove last '|'
	}
	catch (COleException *e) {
			e->GetErrorMessage(result,RESULT_SIZE);
			e->Delete();
	}
	catch (...) {
		strcpy(result, "ERROR: caught unknown exception");
	}

	m_Session.Close();
	m_Connect.Close();

	return result;
	
}
char *DBGetItems(char *connectStr, char *dbName, char *schemaName, char *dataType)
{
	USES_CONVERSION;

	bool		rc = true;
	CDataSource m_Connect;
	CSession    m_Session;
	CTables*        pTableSet = NULL;
	char *result = (char*)CoTaskMemAlloc( RESULT_SIZE );


	strcpy(result, "ERROR: trying m_Connect.Open(connectStr");
	try {
		if (m_Connect.OpenFromInitializationString(A2OLE(connectStr)) != S_OK)
		{
			strcpy(result, "ERROR: m_Connect.Open(connectStr) failed");
			return result;
		}
		if (m_Session.Open(m_Connect) != S_OK)
		{
			strcpy(result, "ERROR: m_Session.Open(m_Connect) failed");
			return result;
		}
		// List the tables in the data source
		// Add an entry for all tables in the system
		pTableSet = new CTables;

		strcpy(result, "ERROR: pTableSet->Open(m_Session, ...) failed");

		if (pTableSet->Open(m_Session, dbName, schemaName, NULL, dataType) != S_OK)
			return result;

		*result = 0;
		while(pTableSet->MoveNext() == S_OK)
		{
			strcat (result,pTableSet->m_szName);
			strcat (result,"|");
		}
		if (*result == 0)
			strcpy(result, "");
		else
			*(result+(strlen(result)-1)) = 0x00;	// remove last '|'
	}
	catch (COleException *e) {
			e->GetErrorMessage(result,RESULT_SIZE);
			e->Delete();
			return result;
	}
	catch (...) {
		strcpy(result, "ERROR: caught unknown exception");
		return result;
	}
	pTableSet->Close();
	delete pTableSet;
	pTableSet = NULL;
	m_Session.Close();
	m_Connect.Close();

	return result;
	
}
extern "C" WEBAPPUTIL_API char *DBGetTables(char *connectStr, char *dbName, char *schemaName, bool systemTables)
{
	return (DBGetItems(connectStr, dbName, schemaName, systemTables ? "SYSTEM TABLE" : "TABLE"));
}
extern "C" WEBAPPUTIL_API char *DBGetViews(char *connectStr, char *dbName, char *schemaName, bool systemView)
{
	return (DBGetItems(connectStr, dbName, schemaName, systemView ? "SYSTEM VIEW" : "VIEW"));
}

extern "C" WEBAPPUTIL_API char * DBGetIndexes(char *connectStr, char *tableName)
{	
	USES_CONVERSION;

	bool		rc = true;
	CDataSource m_Connect;
	CSession    m_Session;
	CIndexes*		pIndexesSet = NULL;
	char *result = (char*)CoTaskMemAlloc( 1024 );

	strcpy(result, "ERROR: trying m_Connect.Open(connectStr");

	try {
		if (m_Connect.OpenFromInitializationString(A2OLE(connectStr)) != S_OK)
		{
			strcpy(result, "ERROR: m_Connect.Open(connectStr) failed");
			return result;
		}
		if (m_Session.Open(m_Connect) != S_OK)
		{
			strcpy(result, "ERROR: m_Session.Open(m_Connect) failed");
			return result;
		}
		// List the indexes in the table
		pIndexesSet = new CIndexes;

		strcpy(result, "ERROR: pIndexesSet->Open(m_Session, ...) failed");

		if (pIndexesSet->Open(m_Session, NULL, NULL, NULL, NULL, tableName) != S_OK)
			return result;

		*result = 0;

		long i = 0;
		while(pIndexesSet->MoveNext() == S_OK)
		{
			strcat (result,pIndexesSet->m_szColumnName);
			strcat (result,"|");
//			if (pIndexesSet->m_bPrimaryKey == TRUE){
//				strcat (result,pIndexesSet->m_szColumnName);
//				strcat (result,"|");
//			}
		}
	}
	catch (COleException *e) {
			e->GetErrorMessage(result,RESULT_SIZE);
			e->Delete();
			return result;
	}
	catch (...) {
		strcpy(result, "ERROR: caught unknown exception");
		return result;
	}
	pIndexesSet->Close();
	delete pIndexesSet;
	pIndexesSet = NULL;
	m_Session.Close();
	m_Connect.Close();

	return result;
	
}
extern "C" WEBAPPUTIL_API char * DBGetQuote(char *connectStr)
{	
	USES_CONVERSION;

	bool		rc = true;
	CDataSource m_Connect;
	CSession    m_Session;
	char *result = (char*)CoTaskMemAlloc( 128 );

	strcpy(result, "ERROR: trying m_Connect.Open(connectStr");

	try {
		if (m_Connect.OpenFromInitializationString(A2OLE(connectStr)) != S_OK)
		{
			strcpy(result, "ERROR: m_Connect.Open(connectStr) failed");
			return result;
		}

		// Determine if there are any literal characters that specify
		// table and column names.
		CComQIPtr<IDBInfo> spInfo(m_Connect.m_spInit);

		if (spInfo != NULL)
		{
			DBLITERAL dbLit = DBLITERAL_QUOTE;
			ULONG ulLiteralInfo = 0;
			DBLITERALINFO* pLiteralInfo = NULL;
			OLECHAR* pChar = NULL;

			HRESULT hr = spInfo->GetLiteralInfo(1, &dbLit, &ulLiteralInfo,
							&pLiteralInfo, &pChar);

			if (SUCCEEDED(hr))
				strcpy(result, OLE2T(pLiteralInfo[0].pwszLiteralValue));
			else 
				getHRtext(hr, result);

			if (pLiteralInfo != NULL)
				CoTaskMemFree(pLiteralInfo);

			if (pChar != NULL)
				CoTaskMemFree(pChar);
		}
	}
	catch (COleException *e) {
			e->GetErrorMessage(result,RESULT_SIZE);
			e->Delete();
			return result;
	}
	catch (...) {
		strcpy(result, "ERROR: caught unknown exception");
		return result;
	}
	m_Connect.Close();

	return result;	
}