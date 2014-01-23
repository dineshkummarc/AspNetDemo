// XBreadCrumbBarTest.h : main header file for the XBREADCRUMBBARTEST application
//

#ifndef XBREADCRUMBBARTEST_H
#define XBREADCRUMBBARTEST_H

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CXBreadCrumbBarTestApp:
// See XBreadCrumbBarTest.cpp for the implementation of this class
//

class CXBreadCrumbBarTestApp : public CWinApp
{
public:
	CXBreadCrumbBarTestApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CXBreadCrumbBarTestApp)
public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CXBreadCrumbBarTestApp)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif //XBREADCRUMBBARTEST_H
