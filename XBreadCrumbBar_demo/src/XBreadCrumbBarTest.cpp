// XBreadCrumbBarTest.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "XBreadCrumbBarTest.h"
#include "XBreadCrumbBarTestDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CXBreadCrumbBarTestApp

BEGIN_MESSAGE_MAP(CXBreadCrumbBarTestApp, CWinApp)
	//{{AFX_MSG_MAP(CXBreadCrumbBarTestApp)
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CXBreadCrumbBarTestApp construction

CXBreadCrumbBarTestApp::CXBreadCrumbBarTestApp()
{
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CXBreadCrumbBarTestApp object

CXBreadCrumbBarTestApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CXBreadCrumbBarTestApp initialization

BOOL CXBreadCrumbBarTestApp::InitInstance()
{
#if _MFC_VER < 0x700
#ifdef _AFXDLL
	Enable3dControls();			// Call this when using MFC in a shared DLL
#else
	Enable3dControlsStatic();	// Call this when linking to MFC statically
#endif
#endif

	CXBreadCrumbBarTestDlg dlg;
	m_pMainWnd = &dlg;
	dlg.DoModal();
	return FALSE;
}
