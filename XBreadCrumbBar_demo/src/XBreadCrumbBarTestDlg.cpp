// XBreadCrumbBarTestDlg.cpp : implementation file
//

#include "stdafx.h"
#include "XBreadCrumbBarTest.h"
#include "XBreadCrumbBarTestDlg.h"
#include "about.h"
#include "XMessageBox.h"
#include "XTrace.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#pragma warning(disable : 4996)	// disable bogus deprecation warning

const int g_uStatic[] = { IDC_STATIC1, IDC_STATIC2, IDC_STATIC3 };

//=============================================================================
// sample bread crumb text
//=============================================================================
#if 0  // -----------------------------------------------------------
const TCHAR * SAMPLE_1 [] = 
{
	// Books >› Mystery & Thrillers › Mystery › Sherlock Holmes
	_T("&nbsp;Books"),
	_T("Mystery & Thrillers"),
	_T("Mystery"),
	_T("Sherlock Holmes"),
	NULL
};
#endif // -----------------------------------------------------------

const TCHAR * SAMPLE_1 = _T("&nbsp;Books~Mystery & Thrillers~Mystery~Sherlock Holmes");

const TCHAR * SAMPLE_2 [] = 
{
	// VMware > Products >  VMware Workstation > Evaluate
	_T("&nbsp;<a href=\"http://www.vmware.com\"><font color=\"#0000CC\">VMware</font></a>"),
	_T("<a href=\"http://www.vmware.com/products\"><font color=\"#0000CC\">Products</font></a>"),
	_T("<a href=\"http://www.vmware.com/products/ws\"><font color=\"#0000CC\">VMware Workstation</font></a>"),
	_T("<font color=\"#CC6600\">Evaluate</font>"),
	NULL
};

const TCHAR * SAMPLE_3 [] = 
{
	// DefCon 1 > DefCon 2 >  DefCon 3 > DefCon 4 > DefCon 5
	_T("&nbsp;<a href=\"app:WM_APP_COMMAND1\"><font color=\"green\">DefCon 1</font></a>"),
	_T("<a href=\"app:WM_APP_COMMAND2\"><font color=\"dodgerblue\">DefCon 2</font></a>"),
	_T("<a href=\"app:WM_APP_COMMAND3\"><font color=\"blue\">DefCon 3</font></a>"),
	_T("<a href=\"app:WM_APP_COMMAND4\"><font color=\"magenta\">DefCon 4</font></a>"),
	_T("<a href=\"app:WM_APP_COMMAND5\"><font color=\"red\">DefCon 5</font></a>"),
	NULL
};

//=============================================================================
// define app command message used by <a href=\"app:WM_APP_COMMAND\">
//=============================================================================
const UINT WM_APP_COMMAND_1 = WM_APP+101;
const UINT WM_APP_COMMAND_2 = WM_APP+102;
const UINT WM_APP_COMMAND_3 = WM_APP+103;
const UINT WM_APP_COMMAND_4 = WM_APP+104;
const UINT WM_APP_COMMAND_5 = WM_APP+105;


//=============================================================================
BEGIN_MESSAGE_MAP(CXBreadCrumbBarTestDlg, CDialog)
//=============================================================================
	//{{AFX_MSG_MAP(CXBreadCrumbBarTestDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_MOUSEMOVE()
	ON_WM_LBUTTONUP()
	//}}AFX_MSG_MAP
	ON_MESSAGE(WM_APP_COMMAND_1, OnAppCommand1)
	ON_MESSAGE(WM_APP_COMMAND_2, OnAppCommand1)
	ON_MESSAGE(WM_APP_COMMAND_3, OnAppCommand1)
	ON_MESSAGE(WM_APP_COMMAND_4, OnAppCommand1)
	ON_MESSAGE(WM_APP_COMMAND_5, OnAppCommand1)
END_MESSAGE_MAP()

//=============================================================================
CXBreadCrumbBarTestDlg::CXBreadCrumbBarTestDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CXBreadCrumbBarTestDlg::IDD, pParent)
//=============================================================================
{
	//{{AFX_DATA_INIT(CXBreadCrumbBarTestDlg)
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_rgbText = GetSysColor(COLOR_WINDOWTEXT);
	m_rgbBackground = GetSysColor(COLOR_WINDOW);
}

//=============================================================================
void CXBreadCrumbBarTestDlg::DoDataExchange(CDataExchange* pDX)
//=============================================================================
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CXBreadCrumbBarTestDlg)
	//}}AFX_DATA_MAP
}

//=============================================================================
BOOL CXBreadCrumbBarTestDlg::OnInitDialog()
//=============================================================================
{
	CDialog::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	
	for (int i = 0; i < 3; i++)
		InitDrawStruct(i);

	// set up app command for DEF CON links -
	// see above SAMPLE_2 strings
	CXHtmlDraw::XHTMLDRAW_APP_COMMAND AppCommands[] = 
	{
		{ m_hWnd, WM_APP_COMMAND_1, 1, _T("WM_APP_COMMAND1") },
		{ m_hWnd, WM_APP_COMMAND_2, 2, _T("WM_APP_COMMAND2") },
		{ m_hWnd, WM_APP_COMMAND_3, 3, _T("WM_APP_COMMAND3") },
		{ m_hWnd, WM_APP_COMMAND_4, 4, _T("WM_APP_COMMAND4") },
		{ m_hWnd, WM_APP_COMMAND_5, 5, _T("WM_APP_COMMAND5") },
	};

	m_ccb[2].SetAppCommands(AppCommands, 
		sizeof(AppCommands)/sizeof(AppCommands[0]));

	return TRUE;  // return TRUE  unless you set the focus to a control
}

//=============================================================================
void CXBreadCrumbBarTestDlg::OnSysCommand(UINT nID, LPARAM lParam)
//=============================================================================
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

//=============================================================================
HCURSOR CXBreadCrumbBarTestDlg::OnQueryDragIcon()
//=============================================================================
{
	return (HCURSOR) m_hIcon;
}

//=============================================================================
void CXBreadCrumbBarTestDlg::OnPaint() 
//=============================================================================
{
	CPaintDC dc(this);		// device context for painting

	if (IsIconic())
	{
		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();

		// paint CXBreadCrumbBar objects
		DrawSample(&dc, 0);
		DrawSample(&dc, 1);
		DrawSample(&dc, 2);
	}
}

//=============================================================================
void CXBreadCrumbBarTestDlg::InitDrawStruct(int index)
//=============================================================================
{
	ASSERT((index >=0) && (index < 3));

	LOGFONT lf;
	memset(&lf, 0, sizeof(LOGFONT));
	CFont *pFont = GetFont();	// get font for the dialog
	if (pFont)
		pFont->GetLogFont(&lf);

	CWnd *pWnd = GetDlgItem(g_uStatic[index]);
	ASSERT(pWnd);

	CRect rect;
	pWnd->GetWindowRect(&rect);
	ScreenToClient(&rect);
	rect.DeflateRect(5, 5);
	rect.top += 10;

	CXBreadCrumbBar::XHTMLDRAWSTRUCT ds;

	ds.crText           = m_rgbText;
	ds.nID              = index;
	if (index == 0)
		ds.crText = RGB(119,121,118);
	if (index == 1)
		ds.crBackground = GetSysColor(COLOR_BTNFACE);
	else
		ds.crBackground = m_rgbBackground;
	ds.rect             = rect;
	if (index != 2)
		ds.bBold        = TRUE;
	else
		ds.bBold        = FALSE;
	ds.bLogFont         = TRUE;
	if (index == 0)
		_tcscpy(lf.lfFaceName, _T("Arial"));
	memcpy(&ds.lf, &lf, sizeof(LOGFONT));

	if (index == 0)
	{
		//m_ccb[index].SetSeparator(_T(" » "));
		//m_ccb[index].SetSeparator(_T(" › "));
		m_ccb[index].SetSeparator(_T(" > "));
	}

	// must set crumbs before calling CXBreadCrumbBar::InitDrawStruct

	switch (index)
	{
		default:
		case 0: 
			// all crumbs in one string, separated by '~'
			m_ccb[index].SetCrumbs(SAMPLE_1, _T('~'));	
			break;

		case 1: 
			// array of 4 strings
			m_ccb[index].SetCrumbs(SAMPLE_2, 4);	
			break;

		case 2: 
			// array of 5 strings
			m_ccb[index].SetCrumbs(SAMPLE_3, 5);	
			break;
	}

	// don't use bold separators in second bread crumb bar
	m_ccb[index].InitDrawStruct(&ds, (index == 1) ? FALSE : TRUE);
}

//=============================================================================
void CXBreadCrumbBarTestDlg::DrawSample(CDC *pDC, int index)
//=============================================================================
{
	ASSERT(pDC);
	m_ccb[index].Draw(pDC->m_hDC, FALSE);
}

//=============================================================================
LRESULT CXBreadCrumbBarTestDlg::OnAppCommand1(WPARAM wParam, LPARAM)
//=============================================================================
{
	TRACE(_T("in CXBreadCrumbBarTestDlg::OnAppCommand1\n"));

	CString caption = _T("");
	caption.Format(_T("DEFCON %d"), wParam);

	UINT nType = MB_ICONINFORMATION;
	CString s = _T("");

	switch (wParam)
	{
		default:
		case 1:
			s = _T("Situation nominal.");
			nType = MB_ICONINFORMATION;
			break;

		case 2:
			s = _T("Oh oh.  What just happened?");
			nType = MB_ICONQUESTION;
			break;

		case 3:
			s = _T("What are they up to?");
			nType = MB_ICONQUESTION;
			break;

		case 4:
			s = _T("This doesn't look good!");
			nType = MB_ICONEXCLAMATION;
			break;

		case 5:
			s = _T("Goodbye, my friend.");
			nType = MB_ICONSTOP;
			break;
	}

	XMessageBox(m_hWnd, s, caption, nType | MB_NOSOUND);

#ifdef _DEBUG
	for (int index = 0; index < 3; index++)
	{
		TCHAR **crumbs;
		int n = m_ccb[index].GetCrumbs(&crumbs);
		for (int i = 0; i < n; i++)
		{
			TRACE(_T("%d: <%s>\n"), i, crumbs[i]);
		}
	}

	TCHAR szCrumb[100];
	m_ccb[0].GetCrumb(3, FALSE, szCrumb, 50);
	TRACE(_T("GetCrumb test:  <%s>\n"), szCrumb);
	int n = m_ccb[0].GetCrumb(_T("Sherlock Holmes"), TRUE);
	TRACE(_T("GetCrumb test:  n=%d\n"), n);
	n = m_ccb[0].GetCrumb(_T("foo"), TRUE);
	TRACE(_T("GetCrumb test:  n=%d\n"), n);
#endif

	return 0;
}

//=============================================================================
void CXBreadCrumbBarTestDlg::OnMouseMove(UINT nFlags, CPoint point) 
//=============================================================================
{
	for (int i = 0; i < 3; i++)
		m_ccb[i].RelayMouseMove(m_hWnd);

	CDialog::OnMouseMove(nFlags, point);
}

//=============================================================================
void CXBreadCrumbBarTestDlg::OnLButtonUp(UINT nFlags, CPoint point) 
//=============================================================================
{
	for (int i = 0; i < 3; i++)
	{
		if (m_ccb[i].RelayClick(m_hWnd))
			break;
	}

	CDialog::OnLButtonUp(nFlags, point);
}
