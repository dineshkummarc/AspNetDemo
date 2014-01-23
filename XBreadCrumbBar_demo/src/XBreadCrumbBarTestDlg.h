// XBreadCrumbBarTestDlg.h : header file
//

#ifndef XBREADCRUMBBARTESTDLG_H
#define XBREADCRUMBBARTESTDLG_H

#include "XBreadCrumbBar.h"

//=============================================================================
class CXBreadCrumbBarTestDlg : public CDialog
//=============================================================================
{
//=============================================================================
// Construction
//=============================================================================
public:
	CXBreadCrumbBarTestDlg(CWnd* pParent = NULL);	// standard constructor

//=============================================================================
// Dialog Data
//=============================================================================
	//{{AFX_DATA(CXBreadCrumbBarTestDlg)
	enum { IDD = IDD_XBREADCRUMBBARTEST_DIALOG };
	//}}AFX_DATA

//=============================================================================
// ClassWizard generated virtual function overrides
//=============================================================================
	//{{AFX_VIRTUAL(CXBreadCrumbBarTestDlg)
protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

//=============================================================================
// Implementation
//=============================================================================
protected:
	HICON	m_hIcon;
	COLORREF m_rgbText;
	COLORREF m_rgbBackground;
	CXBreadCrumbBar m_ccb[3];
	void	DrawSample(CDC *pDC, int index);
	void	InitDrawStruct(int index);

//=============================================================================
// Generated message map functions
//=============================================================================
	//{{AFX_MSG(CXBreadCrumbBarTestDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	//}}AFX_MSG
	afx_msg LRESULT OnAppCommand1(WPARAM, LPARAM);
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif //XBREADCRUMBBARTESTDLG_H
