#pragma warning disable CA1069 // Enums values should not be duplicated
#pragma warning disable CA1401 // P/Invokes should not be visible

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.IO;

namespace SpreadCommander.Common.Code
{
    public static class WinAPI
    {
        #region Constants
        public const int WM_NULL						= 0x0000;
        public const int WM_CREATE						= 0x0001;
        public const int WM_DESTROY						= 0x0002;
        public const int WM_MOVE						= 0x0003;
        public const int WM_SIZE						= 0x0005;

        public const int WM_ACTIVATE					= 0x0006;
        /*
        * WM_ACTIVATE state values
        */
        public const int WA_INACTIVE					= 0;
        public const int WA_ACTIVE						= 1;
        public const int WA_CLICKACTIVE					= 2;

        public const int WM_SETFOCUS					= 0x0007;
        public const int WM_KILLFOCUS					= 0x0008;
        public const int WM_ENABLE						= 0x000A;
        public const int WM_SETREDRAW					= 0x000B;
        public const int WM_SETTEXT						= 0x000C;
        public const int WM_GETTEXT						= 0x000D;
        public const int WM_GETTEXTLENGTH				= 0x000E;
        public const int WM_PAINT						= 0x000F;
        public const int WM_CLOSE						= 0x0010;

        public const int WM_QUERYENDSESSION				= 0x0011;
        public const int WM_QUERYOPEN					= 0x0013;
        public const int WM_ENDSESSION					= 0x0016;

        public const int WM_QUIT						= 0x0012;
        public const int WM_ERASEBKGND					= 0x0014;
        public const int WM_SYSCOLORCHANGE				= 0x0015;
        public const int WM_SHOWWINDOW					= 0x0018;
        public const int WM_WININICHANGE				= 0x001A;
        public const int WM_SETTINGCHANGE				= WM_WININICHANGE;

        public const int WM_DEVMODECHANGE				= 0x001B;
        public const int WM_ACTIVATEAPP					= 0x001C;
        public const int WM_FONTCHANGE					= 0x001D;
        public const int WM_TIMECHANGE					= 0x001E;
        public const int WM_CANCELMODE					= 0x001F;
        public const int WM_SETCURSOR					= 0x0020;
        public const int WM_MOUSEACTIVATE				= 0x0021;
        public const int WM_CHILDACTIVATE				= 0x0022;
        public const int WM_QUEUESYNC					= 0x0023;

        public const int WM_GETMINMAXINFO				= 0x0024;
        public const int WM_PAINTICON					= 0x0026;
        public const int WM_ICONERASEBKGND				= 0x0027;
        public const int WM_NEXTDLGCTL					= 0x0028;
        public const int WM_SPOOLERSTATUS				= 0x002A;
        public const int WM_DRAWITEM					= 0x002B;
        public const int WM_MEASUREITEM					= 0x002C;
        public const int WM_DELETEITEM					= 0x002D;
        public const int WM_VKEYTOITEM					= 0x002E;
        public const int WM_CHARTOITEM					= 0x002F;
        public const int WM_SETFONT						= 0x0030;
        public const int WM_GETFONT						= 0x0031;
        public const int WM_SETHOTKEY					= 0x0032;
        public const int WM_GETHOTKEY					= 0x0033;
        public const int WM_QUERYDRAGICON				= 0x0037;
        public const int WM_COMPAREITEM					= 0x0039;
        public const int WM_GETOBJECT					= 0x003D;
        public const int WM_COMPACTING					= 0x0041;
        public const int WM_COMMNOTIFY					= 0x0044;  /* no longer supported */
        public const int WM_WINDOWPOSCHANGING			= 0x0046;
        public const int WM_WINDOWPOSCHANGED			= 0x0047;

        public const int WM_POWER						= 0x0048;

        public const int WM_HELP						= 0x0053;				

        public const int WM_KEYFIRST					= 0x0100;
        public const int WM_KEYDOWN						= 0x0100;
        public const int WM_KEYUP						= 0x0101;
        public const int WM_CHAR						= 0x0102;
        public const int WM_DEADCHAR					= 0x0103;
        public const int WM_SYSKEYDOWN					= 0x0104;
        public const int WM_SYSKEYUP					= 0x0105;
        public const int WM_SYSCHAR						= 0x0106;
        public const int WM_SYSDEADCHAR					= 0x0107;
        public const int WM_KEYLAST						= 0x0108;

        public const int WM_IME_STARTCOMPOSITION		= 0x010D;
        public const int WM_IME_ENDCOMPOSITION			= 0x010E;
        public const int WM_IME_COMPOSITION				= 0x010F;
        public const int WM_IME_KEYLAST					= 0x010F;

        public const int WM_INITDIALOG					= 0x0110;
        public const int WM_COMMAND						= 0x0111;
        public const int WM_SYSCOMMAND					= 0x0112;
        public const int WM_TIMER						= 0x0113;
        public const int WM_HSCROLL						= 0x0114;
        public const int WM_VSCROLL						= 0x0115;
        public const int WM_INITMENU					= 0x0116;
        public const int WM_INITMENUPOPUP				= 0x0117;
        public const int WM_MENUSELECT					= 0x011F;
        public const int WM_MENUCHAR					= 0x0120;
        public const int WM_ENTERIDLE					= 0x0121;

        public const int WM_MENURBUTTONUP				= 0x0122;
        public const int WM_MENUDRAG					= 0x0123;
        public const int WM_MENUGETOBJECT				= 0x0124;
        public const int WM_UNINITMENUPOPUP				= 0x0125;
        public const int WM_MENUCOMMAND					= 0x0126;

        public const int WM_CHANGEUISTATE				= 0x0127;
        public const int WM_UPDATEUISTATE				= 0x0128;
        public const int WM_QUERYUISTATE				= 0x0129;

        public const int WM_CUT							= 0x0300;
        public const int WM_COPY						= 0x0301;
        public const int WM_PASTE						= 0x0302;
        public const int WM_CLEAR						= 0x0303;
        public const int WM_UNDO						= 0x0304;
        public const int WM_RENDERFORMAT				= 0x0305;
        public const int WM_RENDERALLFORMATS			= 0x0306;
        public const int WM_DESTROYCLIPBOARD			= 0x0307;
        public const int WM_DRAWCLIPBOARD				= 0x0308;
        public const int WM_PAINTCLIPBOARD				= 0x0309;
        public const int WM_VSCROLLCLIPBOARD			= 0x030A;
        public const int WM_SIZECLIPBOARD				= 0x030B;
        public const int WM_ASKCBFORMATNAME				= 0x030C;
        public const int WM_CHANGECBCHAIN				= 0x030D;
        public const int WM_HSCROLLCLIPBOARD			= 0x030E;
        public const int WM_QUERYNEWPALETTE				= 0x030F;
        public const int WM_PALETTEISCHANGING			= 0x0310;
        public const int WM_PALETTECHANGED				= 0x0311;
        public const int WM_HOTKEY						= 0x0312;
        
        public const int WT_DEFBASE						= 0x7FF0;
        public const int WT_PACKET						= WT_DEFBASE;
        public const int WM_MOUSEMOVE					= 0x0200;
        public const int WM_LBUTTONDOWN					= 0x0201;
        public const int WM_LBUTTONUP					= 0x0202;
        public const int WM_LBUTTONDBLCLK				= 0x0203;
        public const int WM_RBUTTONDOWN					= 0x0204;
        public const int WM_RBUTTONUP					= 0x0205;
        public const int WM_RBUTTONDBLCLK				= 0x0206;
        public const int WM_MBUTTONDOWN					= 0x0207;
        public const int WM_MBUTTONUP					= 0x0208;
        public const int WM_MBUTTONDBLCLK				= 0x0209;
        public const int WM_MOUSEWHEEL					= 0x020A;
        public const int WM_USER						= 0x400;
        public const int WM_APP							= 0x8000;

        public const int EM_GETSEL						= 0x00B0;
        public const int EM_SETSEL						= 0x00B1;
        public const int EM_GETRECT						= 0x00B2;
        public const int EM_SETRECT						= 0x00B3;
        public const int EM_SETRECTNP					= 0x00B4;
        public const int EM_SCROLL						= 0x00B5;
        public const int EM_LINESCROLL					= 0x00B6;
        public const int EM_SCROLLCARET					= 0x00B7;
        public const int EM_GETMODIFY					= 0x00B8;
        public const int EM_SETMODIFY					= 0x00B9;
        public const int EM_GETLINECOUNT				= 0x00BA;
        public const int EM_LINEINDEX					= 0x00BB;
        public const int EM_SETHANDLE					= 0x00BC;
        public const int EM_GETHANDLE					= 0x00BD;
        public const int EM_GETTHUMB					= 0x00BE;
        public const int EM_LINELENGTH					= 0x00C1;
        public const int EM_REPLACESEL					= 0x00C2;
        public const int EM_GETLINE						= 0x00C4;
        public const int EM_LIMITTEXT					= 0x00C5;
        public const int EM_CANUNDO						= 0x00C6;
        public const int EM_UNDO						= 0x00C7;
        public const int EM_FMTLINES					= 0x00C8;
        public const int EM_LINEFROMCHAR				= 0x00C9;
        public const int EM_SETTABSTOPS					= 0x00CB;
        public const int EM_SETPASSWORDCHAR				= 0x00CC;
        public const int EM_EMPTYUNDOBUFFER				= 0x00CD;
        public const int EM_GETFIRSTVISIBLELINE			= 0x00CE;
        public const int EM_SETREADONLY					= 0x00CF;
        public const int EM_SETWORDBREAKPROC			= 0x00D0;
        public const int EM_GETWORDBREAKPROC			= 0x00D1;
        public const int EM_GETPASSWORDCHAR				= 0x00D2;
        public const int EM_SETMARGINS					= 0x00D3;
        public const int EM_GETMARGINS					= 0x00D4;
        public const int EM_SETLIMITTEXT				= EM_LIMITTEXT;   /* ;win40 Name change */
        public const int EM_GETLIMITTEXT				= 0x00D5;
        public const int EM_POSFROMCHAR					= 0x00D6;
        public const int EM_CHARFROMPOS					= 0x00D7;

        public const int EM_SETIMESTATUS				= 0x00D8;
        public const int EM_GETIMESTATUS				= 0x00D9;

        public const int VK_LBUTTON						= 0x01;
        public const int VK_RBUTTON						= 0x02;
        public const int VK_CANCEL						= 0x03;
        public const int VK_MBUTTON						= 0x04;	/* NOT contiguous with L & RBUTTON */

        public const int VK_XBUTTON1					= 0x05;	/* NOT contiguous with L & RBUTTON */
        public const int VK_XBUTTON2					= 0x06;	/* NOT contiguous with L & RBUTTON */

        /*
        *= 0x07 : unassigned
        */

        public const int VK_BACK						= 0x08;
        public const int VK_TAB							= 0x09;

        /*
        *= 0x0A -= 0x0B : reserved
        */

        public const int VK_CLEAR						= 0x0C;
        public const int VK_RETURN						= 0x0D;

        public const int VK_SHIFT						= 0x10;
        public const int VK_CONTROL						= 0x11;
        public const int VK_MENU						= 0x12;
        public const int VK_PAUSE						= 0x13;
        public const int VK_CAPITAL						= 0x14;

        public const int VK_KANA						= 0x15;
        public const int VK_HANGEUL						= 0x15;  /* old name - should be here for compatibility */
        public const int VK_HANGUL						= 0x15;
        public const int VK_JUNJA						= 0x17;
        public const int VK_FINAL						= 0x18;
        public const int VK_HANJA						= 0x19;
        public const int VK_KANJI						= 0x19;

        public const int VK_ESCAPE						= 0x1B;

        public const int VK_CONVERT						= 0x1C;
        public const int VK_NONCONVERT					= 0x1D;
        public const int VK_ACCEPT						= 0x1E;
        public const int VK_MODECHANGE					= 0x1F;

        public const int VK_SPACE						= 0x20;
        public const int VK_PRIOR						= 0x21;
        public const int VK_NEXT						= 0x22;
        public const int VK_END							= 0x23;
        public const int VK_HOME						= 0x24;
        public const int VK_LEFT						= 0x25;
        public const int VK_UP							= 0x26;
        public const int VK_RIGHT						= 0x27;
        public const int VK_DOWN						= 0x28;
        public const int VK_SELECT						= 0x29;
        public const int VK_PRINT						= 0x2A;
        public const int VK_EXECUTE						= 0x2B;
        public const int VK_SNAPSHOT					= 0x2C;
        public const int VK_INSERT						= 0x2D;
        public const int VK_DELETE						= 0x2E;
        public const int VK_HELP						= 0x2F;

        /*
        * VK_0 - VK_9 are the same as ASCII '0' - '9' (= 0x30 -= 0x39)
        *= 0x40 : unassigned
        * VK_A - VK_Z are the same as ASCII 'A' - 'Z' (= 0x41 -= 0x5A)
        */

        public const int VK_LWIN						= 0x5B;
        public const int VK_RWIN						= 0x5C;
        public const int VK_APPS						= 0x5D;

        /*
        *= 0x5E : reserved
        */

        public const int VK_SLEEP						= 0x5F;

        public const int VK_NUMPAD0						= 0x60;
        public const int VK_NUMPAD1						= 0x61;
        public const int VK_NUMPAD2						= 0x62;
        public const int VK_NUMPAD3						= 0x63;
        public const int VK_NUMPAD4						= 0x64;
        public const int VK_NUMPAD5						= 0x65;
        public const int VK_NUMPAD6						= 0x66;
        public const int VK_NUMPAD7						= 0x67;
        public const int VK_NUMPAD8						= 0x68;
        public const int VK_NUMPAD9						= 0x69;
        public const int VK_MULTIPLY					= 0x6A;
        public const int VK_ADD							= 0x6B;
        public const int VK_SEPARATOR					= 0x6C;
        public const int VK_SUBTRACT					= 0x6D;
        public const int VK_DECIMAL						= 0x6E;
        public const int VK_DIVIDE						= 0x6F;
        public const int VK_F1							= 0x70;
        public const int VK_F2							= 0x71;
        public const int VK_F3							= 0x72;
        public const int VK_F4							= 0x73;
        public const int VK_F5							= 0x74;
        public const int VK_F6							= 0x75;
        public const int VK_F7							= 0x76;
        public const int VK_F8							= 0x77;
        public const int VK_F9							= 0x78;
        public const int VK_F10							= 0x79;
        public const int VK_F11							= 0x7A;
        public const int VK_F12							= 0x7B;
        public const int VK_F13							= 0x7C;
        public const int VK_F14							= 0x7D;
        public const int VK_F15							= 0x7E;
        public const int VK_F16							= 0x7F;
        public const int VK_F17							= 0x80;
        public const int VK_F18							= 0x81;
        public const int VK_F19							= 0x82;
        public const int VK_F20							= 0x83;
        public const int VK_F21							= 0x84;
        public const int VK_F22							= 0x85;
        public const int VK_F23							= 0x86;
        public const int VK_F24							= 0x87;

        /*
        *= 0x88 -= 0x8F : unassigned
        */

        public const int VK_NUMLOCK						= 0x90;
        public const int VK_SCROLL						= 0x91;

        /*
        * NEC PC-9800 kbd definitions
        */
        public const int VK_OEM_NEC_EQUAL				= 0x92;   // '=' key on numpad

        /*
        * Fujitsu/OASYS kbd definitions
        */
        public const int VK_OEM_FJ_JISHO				= 0x92;   // 'Dictionary' key
        public const int VK_OEM_FJ_MASSHOU				= 0x93;   // 'Unregister word' key
        public const int VK_OEM_FJ_TOUROKU				= 0x94;   // 'Register word' key
        public const int VK_OEM_FJ_LOYA					= 0x95;   // 'Left OYAYUBI' key
        public const int VK_OEM_FJ_ROYA					= 0x96;   // 'Right OYAYUBI' key

        /*
        *= 0x97 -= 0x9F : unassigned
        */

        /*
        * VK_L* & VK_R* - left and right Alt, Ctrl and Shift virtual keys.
        * Used only as parameters to GetAsyncKeyState() and GetKeyState().
        * No other API or message will distinguish left and right keys in this way.
        */
        public const int VK_LSHIFT						= 0xA0;
        public const int VK_RSHIFT						= 0xA1;
        public const int VK_LCONTROL					= 0xA2;
        public const int VK_RCONTROL					= 0xA3;
        public const int VK_LMENU						= 0xA4;
        public const int VK_RMENU						= 0xA5;

        public const int VK_BROWSER_BACK				= 0xA6;
        public const int VK_BROWSER_FORWARD				= 0xA7;
        public const int VK_BROWSER_REFRESH				= 0xA8;
        public const int VK_BROWSER_STOP				= 0xA9;
        public const int VK_BROWSER_SEARCH				= 0xAA;
        public const int VK_BROWSER_FAVORITES			= 0xAB;
        public const int VK_BROWSER_HOME				= 0xAC;

        public const int VK_VOLUME_MUTE					= 0xAD;
        public const int VK_VOLUME_DOWN					= 0xAE;
        public const int VK_VOLUME_UP					= 0xAF;
        public const int VK_MEDIA_NEXT_TRACK			= 0xB0;
        public const int VK_MEDIA_PREV_TRACK			= 0xB1;
        public const int VK_MEDIA_STOP					= 0xB2;
        public const int VK_MEDIA_PLAY_PAUSE			= 0xB3;
        public const int VK_LAUNCH_MAIL					= 0xB4;
        public const int VK_LAUNCH_MEDIA_SELECT			= 0xB5;
        public const int VK_LAUNCH_APP1					= 0xB6;
        public const int VK_LAUNCH_APP2					= 0xB7;

        /*
        *= 0xB8 -= 0xB9 : reserved
        */

        public const int VK_OEM_1						= 0xBA;   // ';:' for US
        public const int VK_OEM_PLUS					= 0xBB;   // '+' any country
        public const int VK_OEM_COMMA					= 0xBC;   // ',' any country
        public const int VK_OEM_MINUS					= 0xBD;   // '-' any country
        public const int VK_OEM_PERIOD					= 0xBE;   // '.' any country
        public const int VK_OEM_2						= 0xBF;   // '/?' for US
        public const int VK_OEM_3						= 0xC0;   // '`~' for US

        /*
        *= 0xC1 -= 0xD7 : reserved
        */

        /*
        *= 0xD8 -= 0xDA : unassigned
        */

        public const int VK_OEM_4						= 0xDB;  //  '[{' for US
        public const int VK_OEM_5						= 0xDC;  //  '\|' for US
        public const int VK_OEM_6						= 0xDD;  //  ']}' for US
        public const int VK_OEM_7						= 0xDE;  //  ''"' for US
        public const int VK_OEM_8						= 0xDF;

        /*
        *= 0xE0 : reserved
        */

        /*
        * Various extended or enhanced keyboards
        */
        public const int VK_OEM_AX						= 0xE1;  //  'AX' key on Japanese AX kbd
        public const int VK_OEM_102						= 0xE2;  //  "<>" or "\|" on RT 102-key kbd.
        public const int VK_ICO_HELP					= 0xE3;  //  Help key on ICO
        public const int VK_ICO_00						= 0xE4;  //  00 key on ICO

        public const int VK_PROCESSKEY					= 0xE5;

        public const int VK_ICO_CLEAR					= 0xE6;

        public const int VK_PACKET						= 0xE7;

        /*
        *= 0xE8 : unassigned
        */

        /*
        * Nokia/Ericsson definitions
        */
        public const int VK_OEM_RESET					= 0xE9;
        public const int VK_OEM_JUMP					= 0xEA;
        public const int VK_OEM_PA1						= 0xEB;
        public const int VK_OEM_PA2						= 0xEC;
        public const int VK_OEM_PA3						= 0xED;
        public const int VK_OEM_WSCTRL					= 0xEE;
        public const int VK_OEM_CUSEL					= 0xEF;
        public const int VK_OEM_ATTN					= 0xF0;
        public const int VK_OEM_FINISH					= 0xF1;
        public const int VK_OEM_COPY					= 0xF2;
        public const int VK_OEM_AUTO					= 0xF3;
        public const int VK_OEM_ENLW					= 0xF4;
        public const int VK_OEM_BACKTAB					= 0xF5;

        public const int VK_ATTN						= 0xF6;
        public const int VK_CRSEL						= 0xF7;
        public const int VK_EXSEL						= 0xF8;
        public const int VK_EREOF						= 0xF9;
        public const int VK_PLAY						= 0xFA;
        public const int VK_ZOOM						= 0xFB;
        public const int VK_NONAME						= 0xFC;
        public const int VK_PA1							= 0xFD;
        public const int VK_OEM_CLEAR					= 0xFE;



        /*
         * Dialog Box Command IDs
         */
        public const int IDOK							= 1;
        public const int IDCANCEL						= 2;
        public const int IDABORT						= 3;
        public const int IDRETRY						= 4;
        public const int IDIGNORE						= 5;
        public const int IDYES							= 6;
        public const int IDNO							= 7;
        public const int IDCLOSE						= 8;
        public const int IDHELP							= 9;
        public const int IDTRYAGAIN						= 10;
        public const int IDCONTINUE						= 11;

        public const int PM_NOREMOVE					= 0x0000;
        public const int PM_REMOVE						= 0x0001;
        public const int PM_NOYIELD						= 0x0002;
        
        public static readonly IntPtr HWND_TOP					= new (0);
        public static readonly IntPtr HWND_BOTTOM				= new (1);
        public static readonly IntPtr HWND_TOPMOST				= new (-1);
        public static readonly IntPtr HWND_NOTOPMOST		    = new (-2);

        /*
         * SetWindowPos Flags
         */
        public const int SWP_NOSIZE						= 0x0001;
        public const int SWP_NOMOVE						= 0x0002;
        public const int SWP_NOZORDER					= 0x0004;
        public const int SWP_NOREDRAW					= 0x0008;
        public const int SWP_NOACTIVATE					= 0x0010;
        public const int SWP_FRAMECHANGED				= 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
        public const int SWP_SHOWWINDOW					= 0x0040;
        public const int SWP_HIDEWINDOW					= 0x0080;
        public const int SWP_NOCOPYBITS					= 0x0100;
        public const int SWP_NOOWNERZORDER				= 0x0200;  /* Don't do owner Z ordering */
        public const int SWP_NOSENDCHANGING				= 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */

        public const int SWP_DRAWFRAME					= SWP_FRAMECHANGED;
        public const int SWP_NOREPOSITION				= SWP_NOOWNERZORDER;

        public const int SWP_DEFERERASE					= 0x2000;
        public const int SWP_ASYNCWINDOWPOS				= 0x4000;
        
        //Redraw Flags
        public const int RDW_INVALIDATE					= 0x0001;
        public const int RDW_INTERNALPAINT				= 0x0002;
        public const int RDW_ERASE						= 0x0004;

        public const int RDW_VALIDATE					= 0x0008;
        public const int RDW_NOINTERNALPAINT			= 0x0010;
        public const int RDW_NOERASE					= 0x0020;

        public const int RDW_NOCHILDREN					= 0x0040;
        public const int RDW_ALLCHILDREN				= 0x0080;

        public const int RDW_UPDATENOW					= 0x0100;
        public const int RDW_ERASENOW					= 0x0200;

        public const int RDW_FRAME						= 0x0400;
        public const int RDW_NOFRAME					= 0x0800;

        
        public const int MAX_PATH						= 260;

        
        public const int DRIVE_UNKNOWN					= 0;
        public const int DRIVE_NO_ROOT_DIR				= 1;
        public const int DRIVE_REMOVABLE				= 2;
        public const int DRIVE_FIXED					= 3;
        public const int DRIVE_REMOTE					= 4;
        public const int DRIVE_CDROM					= 5;
        public const int DRIVE_RAMDISK					= 6;

        /*
         * Window Styles
         */
        public const int WS_OVERLAPPED       			= 0x00000000;
        public const int WS_POPUP            			= unchecked((int)0x80000000);
        public const int WS_CHILD            			= 0x40000000;
        public const int WS_MINIMIZE         			= 0x20000000;
        public const int WS_VISIBLE          			= 0x10000000;
        public const int WS_DISABLED         			= 0x08000000;
        public const int WS_CLIPSIBLINGS     			= 0x04000000;
        public const int WS_CLIPCHILDREN     			= 0x02000000;
        public const int WS_MAXIMIZE         			= 0x01000000;
        public const int WS_CAPTION          			= 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
        public const int WS_BORDER           			= 0x00800000;
        public const int WS_DLGFRAME         			= 0x00400000;
        public const int WS_VSCROLL          			= 0x00200000;
        public const int WS_HSCROLL          			= 0x00100000;
        public const int WS_SYSMENU          			= 0x00080000;
        public const int WS_THICKFRAME       			= 0x00040000;
        public const int WS_GROUP            			= 0x00020000;
        public const int WS_TABSTOP          			= 0x00010000;

        public const int WS_MINIMIZEBOX      			= 0x00020000;
        public const int WS_MAXIMIZEBOX      			= 0x00010000;


        public const int WS_TILED						= WS_OVERLAPPED;
        public const int WS_ICONIC						= WS_MINIMIZE;
        public const int WS_SIZEBOX						= WS_THICKFRAME;
        public const int WS_TILEDWINDOW					= WS_OVERLAPPEDWINDOW;

        /*
         * Common Window Styles
         */
        public const int WS_OVERLAPPEDWINDOW			= WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;
        public const int WS_POPUPWINDOW					= WS_POPUP | WS_BORDER | WS_SYSMENU;
        public const int WS_CHILDWINDOW					= WS_CHILD;

        /*
         * Extended Window Styles
         */
        public const int WS_EX_DLGMODALFRAME     		= 0x00000001;
        public const int WS_EX_NOPARENTNOTIFY    		= 0x00000004;
        public const int WS_EX_TOPMOST           		= 0x00000008;
        public const int WS_EX_ACCEPTFILES       		= 0x00000010;
        public const int WS_EX_TRANSPARENT       		= 0x00000020;

        public const int WS_EX_MDICHILD          		= 0x00000040;
        public const int WS_EX_TOOLWINDOW        		= 0x00000080;
        public const int WS_EX_WINDOWEDGE        		= 0x00000100;
        public const int WS_EX_CLIENTEDGE        		= 0x00000200;
        public const int WS_EX_CONTEXTHELP       		= 0x00000400;

        public const int WS_EX_RIGHT             		= 0x00001000;
        public const int WS_EX_LEFT              		= 0x00000000;
        public const int WS_EX_RTLREADING        		= 0x00002000;
        public const int WS_EX_LTRREADING        		= 0x00000000;
        public const int WS_EX_LEFTSCROLLBAR     		= 0x00004000;
        public const int WS_EX_RIGHTSCROLLBAR    		= 0x00000000;

        public const int WS_EX_CONTROLPARENT     		= 0x00010000;
        public const int WS_EX_STATICEDGE        		= 0x00020000;
        public const int WS_EX_APPWINDOW         		= 0x00040000;


        public const int WS_EX_OVERLAPPEDWINDOW			= WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE;
        public const int WS_EX_PALETTEWINDOW			= WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST;

        public const int WS_EX_LAYERED           		= 0x00080000;

        public const int WS_EX_NOINHERITLAYOUT   		= 0x00100000; // Disable inheritence of mirroring by children
        public const int WS_EX_LAYOUTRTL         		= 0x00400000; // Right to left mirroring

        public const int WS_EX_NOACTIVATE        		= 0x08000000;


        /*
         * Class styles
         */
        public const int CS_VREDRAW          			= 0x0001;
        public const int CS_HREDRAW          			= 0x0002;
        public const int CS_DBLCLKS          			= 0x0008;
        public const int CS_OWNDC            			= 0x0020;
        public const int CS_CLASSDC          			= 0x0040;
        public const int CS_PARENTDC         			= 0x0080;
        public const int CS_NOCLOSE          			= 0x0200;
        public const int CS_SAVEBITS         			= 0x0800;
        public const int CS_BYTEALIGNCLIENT  			= 0x1000;
        public const int CS_BYTEALIGNWINDOW  			= 0x2000;
        public const int CS_GLOBALCLASS      			= 0x4000;

        public const int CS_IME              			= 0x00010000;



        /*
         * Predefined Clipboard Formats
         */
        public const int CF_TEXT						= 1;
        public const int CF_BITMAP          			= 2;
        public const int CF_METAFILEPICT     			= 3;
        public const int CF_SYLK             			= 4;
        public const int CF_DIF              			= 5;
        public const int CF_TIFF             			= 6;
        public const int CF_OEMTEXT          			= 7;
        public const int CF_DIB              			= 8;
        public const int CF_PALETTE          			= 9;
        public const int CF_PENDATA          			= 10;
        public const int CF_RIFF             			= 11;
        public const int CF_WAVE             			= 12;
        public const int CF_UNICODETEXT      			= 13;
        public const int CF_ENHMETAFILE      			= 14;

        public const int CF_HDROP            			= 15;
        public const int CF_LOCALE           			= 16;

        public const int CF_DIBV5            			= 17;

        public const int CF_MAX              			= 18;

        public const int CF_OWNERDISPLAY     			= 0x0080;
        public const int CF_DSPTEXT          			= 0x0081;
        public const int CF_DSPBITMAP        			= 0x0082;
        public const int CF_DSPMETAFILEPICT  			= 0x0083;
        public const int CF_DSPENHMETAFILE   			= 0x008E;

        /*
         * "Private" formats don't get GlobalFree()'d
         */
        public const int CF_PRIVATEFIRST     			= 0x0200;
        public const int CF_PRIVATELAST      			= 0x02FF;

        /*
         * "GDIOBJ" formats do get DeleteObject()'d
         */
        public const int CF_GDIOBJFIRST      			= 0x0300;
        public const int CF_GDIOBJLAST       			= 0x03FF;


        [Obfuscation(Exclude = true)]
        public enum SystemMetric
        {
            SM_CXSCREEN									= 0,
            SM_CYSCREEN									= 1,
            SM_CXVSCROLL								= 2,
            SM_CYHSCROLL								= 3,
            SM_CYCAPTION								= 4,
            SM_CXBORDER									= 5,
            SM_CYBORDER									= 6,
            SM_CXDLGFRAME								= 7,
            SM_CYDLGFRAME								= 8,
            SM_CYVTHUMB									= 9,
            SM_CXHTHUMB									= 10,
            SM_CXICON									= 11,
            SM_CYICON									= 12,
            SM_CXCURSOR									= 13,
            SM_CYCURSOR									= 14,
            SM_CYMENU									= 15,
            SM_CXFULLSCREEN								= 16,
            SM_CYFULLSCREEN								= 17,
            SM_CYKANJIWINDOW							= 18,
            SM_MOUSEPRESENT								= 19,
            SM_CYVSCROLL								= 20,
            SM_CXHSCROLL								= 21,
            SM_DEBUG									= 22,
            SM_SWAPBUTTON								= 23,
            SM_RESERVED1								= 24,
            SM_RESERVED2								= 25,
            SM_RESERVED3								= 26,
            SM_RESERVED4								= 27,
            SM_CXMIN									= 28,
            SM_CYMIN									= 29,
            SM_CXSIZE									= 30,
            SM_CYSIZE									= 31,
            SM_CXFRAME									= 32,
            SM_CYFRAME									= 33,
            SM_CXMINTRACK								= 34,
            SM_CYMINTRACK								= 35,
            SM_CXDOUBLECLK								= 36,
            SM_CYDOUBLECLK								= 37,
            SM_CXICONSPACING							= 38,
            SM_CYICONSPACING							= 39,
            SM_MENUDROPALIGNMENT						= 40,
            SM_PENWINDOWS								= 41,
            SM_DBCSENABLED								= 42,
            SM_CMOUSEBUTTONS							= 43,

            SM_CXFIXEDFRAME								= SM_CXDLGFRAME,  /* ;win40 name change */
            SM_CYFIXEDFRAME								= SM_CYDLGFRAME,  /* ;win40 name change */
            SM_CXSIZEFRAME								= SM_CXFRAME,	 /* ;win40 name change */
            SM_CYSIZEFRAME								= SM_CYFRAME,	 /* ;win40 name change */

            SM_SECURE									= 44,
            SM_CXEDGE									= 45,
            SM_CYEDGE									= 46,
            SM_CXMINSPACING								= 47,
            SM_CYMINSPACING								= 48,
            SM_CXSMICON									= 49,
            SM_CYSMICON									= 50,
            SM_CYSMCAPTION								= 51,
            SM_CXSMSIZE									= 52,
            SM_CYSMSIZE									= 53,
            SM_CXMENUSIZE								= 54,
            SM_CYMENUSIZE								= 55,
            SM_ARRANGE									= 56,
            SM_CXMINIMIZED								= 57,
            SM_CYMINIMIZED								= 58,
            SM_CXMAXTRACK								= 59,
            SM_CYMAXTRACK								= 60,
            SM_CXMAXIMIZED								= 61,
            SM_CYMAXIMIZED								= 62,
            SM_NETWORK									= 63,
            SM_CLEANBOOT								= 67,
            SM_CXDRAG									= 68,
            SM_CYDRAG									= 69,

            SM_SHOWSOUNDS								= 70,

            SM_CXMENUCHECK								= 71,   /* Use instead of GetMenuCheckMarkDimensions()! */
            SM_CYMENUCHECK								= 72,
            SM_SLOWMACHINE								= 73,
            SM_MIDEASTENABLED							= 74,

            SM_MOUSEWHEELPRESENT						= 75,
            SM_XVIRTUALSCREEN							= 76,
            SM_YVIRTUALSCREEN							= 77,
            SM_CXVIRTUALSCREEN							= 78,
            SM_CYVIRTUALSCREEN							= 79,
            SM_CMONITORS								= 80,
            SM_SAMEDISPLAYFORMAT						= 81,
            SM_IMMENABLED								= 82
        }
        
        // Commands to pass to WinHelp()
        [Obfuscation(Exclude = true)]
        public enum HelpConstants
        {
            HELP_CONTEXT								= 0x0001,  /* Display topic in ulTopic */
            HELP_QUIT									= 0x0002,  /* Terminate help */
            HELP_INDEX									= 0x0003,  /* Display index */
            HELP_CONTENTS								= 0x0003,
            HELP_HELPONHELP								= 0x0004,  /* Display help on using help */
            HELP_SETINDEX								= 0x0005,  /* Set current Index for multi index help */
            HELP_SETCONTENTS							= 0x0005,
            HELP_CONTEXTPOPUP							= 0x0008,
            HELP_FORCEFILE								= 0x0009,
            HELP_KEY									= 0x0101, /* Display topic for keyword in offabData */
            HELP_COMMAND								= 0x0102,
            HELP_PARTIALKEY								= 0x0105,
            HELP_MULTIKEY								= 0x0201,
            HELP_SETWINPOS								= 0x0203,
            HELP_CONTEXTMENU							= 0x000a,
            HELP_FINDER									= 0x000b,
            HELP_WM_HELP								= 0x000c,
            HELP_SETPOPUP_POS							= 0x000d,

            HELP_TCARD									= 0x8000,
            HELP_TCARD_DATA								= 0x0010,
            HELP_TCARD_OTHER_CALLER						= 0x0011,

            IDH_NO_HELP									= 28440,
            IDH_MISSING_CONTEXT							= 28441, // Control doesn't have matching help context
            IDH_GENERIC_HELP_BUTTON						= 28442, // Property sheet help button
            IDH_OK										= 28443,
            IDH_CANCEL									= 28444,
            IDH_HELP									= 28445
        }

        [Obfuscation(Exclude = true)]
        public enum WindowShowStyle
        {
            SW_HIDE										= 0,
            SW_SHOWNORMAL								= 1,
            SW_NORMAL									= 1,
            SW_SHOWMINIMIZED							= 2,
            SW_SHOWMAXIMIZED							= 3,
            SW_MAXIMIZE									= 3,
            SW_SHOWNOACTIVATE							= 4,
            SW_SHOW										= 5,
            SW_MINIMIZE									= 6,
            SW_SHOWMINNOACTIVE							= 7,
            SW_SHOWNA									= 8,
            SW_RESTORE									= 9,
            SW_SHOWDEFAULT								= 10,
            SW_FORCEMINIMIZE							= 11
        }

        [Obfuscation(Exclude = true)]
        public enum BinaryRasterOperations 
        {
            R2_INVALID									= 0,
            R2_BLACK									= 1,
            R2_NOTMERGEPEN								= 2,
            R2_MASKNOTPEN								= 3,
            R2_NOTCOPYPEN								= 4,
            R2_MASKPENNOT								= 5,
            R2_NOT										= 6,
            R2_XORPEN									= 7,
            R2_NOTMASKPEN								= 8,
            R2_MASKPEN									= 9,
            R2_NOTXORPEN								= 10,
            R2_NOP										= 11,
            R2_MERGENOTPEN								= 12,
            R2_COPYPEN									= 13,
            R2_MERGEPENNOT								= 14,
            R2_MERGEPEN									= 15,
            R2_WHITE									= 16
        }

        [Obfuscation(Exclude = true)]
        [FlagsAttribute]
        public enum SOUND_MODE 
        {
            SND_SYNC									= 0x0000,  /* play synchronously (default) */
            SND_ASYNC									= 0x0001,  /* play asynchronously */
            SND_NODEFAULT								= 0x0002,  /* silence (!default) if sound not found */
            SND_MEMORY									= 0x0004,  /* pszSound points to a memory file */
            SND_LOOP									= 0x0008,  /* loop the sound until next sndPlaySound */
            SND_NOSTOP									= 0x0010,  /* don't stop any currently playing sound */

            SND_NOWAIT									= 0x00002000, /* don't wait if the driver is busy */
            SND_ALIAS									= 0x00010000, /* name is a registry alias */
            SND_ALIAS_ID								= 0x00110000, /* alias is a predefined ID */
            SND_FILENAME								= 0x00020000, /* name is file name */
            SND_RESOURCE								= 0x00040004, /* name is resource name or atom */

            SND_PURGE									= 0x0040,  /* purge non-static events for task */
            SND_APPLICATION								= 0x0080   /* look for application specific association */
        }

        [Obfuscation(Exclude = true)]
        [Flags]
        public enum DrawTextFormat
        {
            DT_TOP										= 0x00000000,
            DT_LEFT										= 0x00000000,
            DT_CENTER									= 0x00000001,
            DT_RIGHT									= 0x00000002,
            DT_VCENTER									= 0x00000004,
            DT_BOTTOM									= 0x00000008,
            DT_WORDBREAK								= 0x00000010,
            DT_SINGLELINE								= 0x00000020,
            DT_EXPANDTABS								= 0x00000040,
            DT_TABSTOP									= 0x00000080,
            DT_NOCLIP									= 0x00000100,
            DT_EXTERNALLEADING							= 0x00000200,
            DT_CALCRECT									= 0x00000400,
            DT_NOPREFIX									= 0x00000800,
            DT_INTERNAL									= 0x00001000,

            DT_EDITCONTROL								= 0x00002000,
            DT_PATH_ELLIPSIS							= 0x00004000,
            DT_END_ELLIPSIS								= 0x00008000,
            DT_MODIFYSTRING								= 0x00010000,
            DT_RTLREADING								= 0x00020000,

            DT_WORD_ELLIPSIS							= 0x00040000,
            DT_NOFULLWIDTHCHARBREAK						= 0x00080000,
            
            DT_HIDEPREFIX								= 0x00100000,
            DT_PREFIXONLY								= 0x00200000
        }

        [Obfuscation(Exclude = true)]
        [Flags]
        public enum ToolhelpSnapshotFlags: uint
        {
            TH32CS_SNAPHEAPLIST							= 0x00000001,
            TH32CS_SNAPPROCESS							= 0x00000002,
            TH32CS_SNAPTHREAD							= 0x00000004,
            TH32CS_SNAPMODULE							= 0x00000008,
            TH32CS_SNAPMODULE32							= 0x00000010,
            TH32CS_SNAPALL								= TH32CS_SNAPHEAPLIST | TH32CS_SNAPPROCESS | TH32CS_SNAPTHREAD | TH32CS_SNAPMODULE,
            TH32CS_INHERIT								= 0x80000000
        }

        [Obfuscation(Exclude = true)]
        [Flags]
        public enum FileSystemFeature: uint
        {
            /// <summary>
            /// The file system supports case-sensitive file names.
            /// </summary>
            CaseSensitiveSearch = 1,
            /// <summary>
            /// The file system preserves the case of file names when it places a name on disk.
            /// </summary>
            CasePreservedNames = 2,
            /// <summary>
            /// The file system supports Unicode in file names as they appear on disk.
            /// </summary>
            UnicodeOnDisk = 4,
            /// <summary>
            /// The file system preserves and enforces access control lists (ACL).
            /// </summary>
            PersistentACLS = 8,
            /// <summary>
            /// The file system supports file-based compression.
            /// </summary>
            FileCompression = 0x10,
            /// <summary>
            /// The file system supports disk quotas.
            /// </summary>
            VolumeQuotas = 0x20,
            /// <summary>
            /// The file system supports sparse files.
            /// </summary>
            SupportsSparseFiles = 0x40,
            /// <summary>
            /// The file system supports re-parse points.
            /// </summary>
            SupportsReparsePoints = 0x80,
            /// <summary>
            /// The specified volume is a compressed volume, for example, a DoubleSpace volume.
            /// </summary>
            VolumeIsCompressed = 0x8000,
            /// <summary>
            /// The file system supports object identifiers.
            /// </summary>
            SupportsObjectIDs = 0x10000,
            /// <summary>
            /// The file system supports the Encrypted File System (EFS).
            /// </summary>
            SupportsEncryption = 0x20000,
            /// <summary>
            /// The file system supports named streams.
            /// </summary>
            NamedStreams = 0x40000,
            /// <summary>
            /// The specified volume is read-only.
            /// </summary>
            ReadOnlyVolume = 0x80000,
            /// <summary>
            /// The volume supports a single sequential write.
            /// </summary>
            SequentialWriteOnce = 0x100000,
            /// <summary>
            /// The volume supports transactions.
            /// </summary>
            SupportsTransactions = 0x200000,
        }
        #endregion

        #region Structures
        public struct MSG
        {
            public IntPtr	 hwnd;
            public uint		 message;
            public IntPtr	 wParam;
            public IntPtr	 lParam;
            public uint		 time;
            public Point	 pt;
        }

        public struct HELPINFO
        {
            public uint		cbSize;
            public int		iContextType;
            public int		iCtrlId;
            public IntPtr	hItemHandle;
            public uint		dwContextId;
            public Point	MousePos;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESSENTRY32
        {
            public uint		dwSize;
            public uint		cntUsage;
            public uint		th32ProcessID;
            public IntPtr	th32DefaultHeapID;
            public uint		th32ModuleID;
            public uint		cntThreads;
            public uint		th32ParentProcessID;
            public int		pcPriClassBase;
            public uint		dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string	szExeFile;
        };
        
        [StructLayout(LayoutKind.Sequential)]
            public struct MODULEENTRY32
        {
            public uint		dwSize;
            public uint		th32ModuleID;	   // This module
            public uint		th32ProcessID;	  // owning process
            public uint		GlblcntUsage;	   // Global usage count on the module
            public uint		ProccntUsage;	   // Module usage count in th32ProcessID's context
            public IntPtr	modBaseAddr;		// Base address of module in th32ProcessID's context
            public uint		modBaseSize;		// Size in bytes of module starting at modBaseAddr
            public uint		hModule;            // The hModule of this module in th32ProcessID's context
#pragma warning disable IDE0044 // Add readonly modifier
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]string szModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]string szExePath;
#pragma warning restore IDE0044 // Add readonly modifier
        };
        #endregion

        #region User32
        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool ReplyMessage(int lResult);

        [DllImport("user32.dll")]
        public static extern bool PeekMessage(ref MSG lpMsg,
            IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage(ref MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage(ref MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern bool ReplyMessage(IntPtr lResult);

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetTimer(IntPtr hWnd, uint nIDEvent, uint uElapse,
            IntPtr lpTimerFunc);

        public static IntPtr SetTimer(IntPtr hWnd, uint nIDEvent, uint uElapse)
        {
            return SetTimer(hWnd, nIDEvent, uElapse, IntPtr.Zero);
        }

        public static IntPtr SetTimer(Control ctrl, uint nIDEvent, uint uElapse,
            IntPtr lpTimerFunc)
        {
            return SetTimer(ctrl.Handle, nIDEvent, uElapse, lpTimerFunc);
        }

        public static IntPtr SetTimer(Control ctrl, uint nIDEvent, uint uElapse)
        {
            return SetTimer(ctrl.Handle, nIDEvent, uElapse);
        }

        [DllImport("user32.dll")]
        public static extern bool KillTimer(IntPtr hWnd, uint uIDEvent);

        public static bool KillTimer(Control ctrl, uint uIDEvent)
        {
            return KillTimer(ctrl.Handle, uIDEvent);
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SystemMetric nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool WinHelp(IntPtr hWndMain, string lpszHelp, HelpConstants uCommand,
            int dwData);

        [DllImport("user32.dll")]
        public static extern bool DragDetect(IntPtr hwnd, Point point);

        public static bool DragDetect(Control ctrl, Point point)
        {
            return DragDetect(ctrl.Handle, point);
        }

        public static bool DragDetect(Control ctrl)
        {
            return DragDetect(ctrl, Control.MousePosition);
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        public static bool ShowWindow(Control ctrl, WindowShowStyle nCmdShow)
        {
            return ShowWindow(ctrl.Handle, nCmdShow);
        }

        public static bool SetVisible(Control ctrl, bool Visible)
        {
            return ShowWindow(ctrl, Visible ? WindowShowStyle.SW_SHOW : WindowShowStyle.SW_HIDE);
        }

        [DllImport("user32.dll")]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
            int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool DrawText(IntPtr hDC, string lpString, int nCount, ref Rectangle lpRect, DrawTextFormat uFormat);

        public static bool DrawText(Graphics Canvas, string str, ref Rectangle rect, DrawTextFormat format)
        {
            if (str == null)
                return false;

            IntPtr hDC = Canvas.GetHdc();
            try
            {
                return DrawText(hDC, str, str.Length, ref rect, format);
            }
            finally
            {
                Canvas.ReleaseHdc(hDC);
            }
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint RegisterClipboardFormat(string lpszFormat);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(int nVirtKey);

        public static bool IsKeyPressed(int nVirtKey)
        {
            return ((GetKeyState(nVirtKey) & ~0x7FFF) != 0);
        }

        [DllImport("user32.dll", EntryPoint = "ShowCaret")]
        public static extern long ShowCaret(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "HideCaret")]
        public static extern long HideCaret(IntPtr hwnd);
        #endregion

        #region Kernel32
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern UInt64 GetTickCount64();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateToolhelp32Snapshot([MarshalAs(UnmanagedType.U4)] ToolhelpSnapshotFlags dwFlags,
            uint th32ProcessID);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll")]
        public static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport("kernel32.dll")]
        public static extern bool Module32Next(IntPtr hSnapshot, out MODULEENTRY32 lpme);

        [DllImport("kernel32.dll", EntryPoint = "GetLongPathName", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetLongPathName(
            string lpszShortPath,	// file name
            string lpszLongPath,	// path buffer
            int cchBuffer			// size of path buffer 
            );

        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr hProcess, IntPtr
           dwMinimumWorkingSetSize, IntPtr dwMaximumWorkingSetSize);

        public static string GetLongFileName(string sFileName)
        {
            var s = new string('\0', MAX_PATH);
            int i = GetLongPathName(sFileName, s, s.Length);
            if (i > MAX_PATH)
            {
                s = new string('\0', i);
                i = GetLongPathName(sFileName, s, s.Length);
            }
            if (i == 0)
                return null;
            else
                return s[..i];
        }

        [DllImport("kernel32.dll", EntryPoint = "GetDriveType", SetLastError = false, CharSet = CharSet.Unicode)]
        public static extern int GetDriveType(string lpRootPathName);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public extern static bool GetVolumeInformation(string RootPathName, StringBuilder VolumeNameBuffer,
            int VolumeNameSize, out uint VolumeSerialNumber, out uint MaximumComponentLength,
            out FileSystemFeature FileSystemFlags, StringBuilder FileSystemNameBuffer, int nFileSystemNameSize);

        public static bool GetVolumeInformation(string volume,	//sample volume = "c:\"
            out string volumeName, out string fileSystemName, out uint serialNumber, out uint maximumLength,
            out FileSystemFeature features)
        {
            volumeName     = null;
            fileSystemName = null;
            serialNumber   = 0;
            maximumLength  = 0;
            features       = 0;

            var volname = new StringBuilder(1024);
            var fsname  = new StringBuilder(1024);

            bool result = GetVolumeInformation(volume, volname, volname.Capacity,
                out uint sernum, out uint maxlen, out FileSystemFeature flags, fsname, fsname.Capacity);
            if (!result)
                return false;

            volumeName     = volname.ToString();
            fileSystemName = fsname.ToString();
            serialNumber   = sernum;
            maximumLength  = maxlen;
            features       = flags;
            
            return true;
        }
        #endregion

        #region Mapi
        [DllImport("mapi32.DLL", CharSet = CharSet.Unicode)]
        public static extern uint MAPISendDocuments(IntPtr ulUIParam, string
            lpszDelimChar, string lpszFullPaths, string lpszFileNames, uint ulReserved); 
        #endregion

        #region WinMM
        [DllImport("winmm.dll")]
        public static extern bool PlaySound([MarshalAs(UnmanagedType.LPWStr)] string pszSound,
            IntPtr hmod, SOUND_MODE fdwSound);

        public static bool PlaySound(string fileName, SOUND_MODE fwdSound)
        {
            try
            {
                return PlaySound(fileName, IntPtr.Zero, fwdSound);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool PlaySound(string fileName)
        {
            try
            {
                return PlaySound(fileName, IntPtr.Zero, SOUND_MODE.SND_FILENAME |
                    SOUND_MODE.SND_SYNC | SOUND_MODE.SND_NOSTOP | SOUND_MODE.SND_NOWAIT |
                    SOUND_MODE.SND_NODEFAULT);
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("winmm.dll")]
        public static extern bool PlaySound([MarshalAs(UnmanagedType.LPArray)] byte[] Sound,
            IntPtr hmod, SOUND_MODE fdwSound);

        public static bool PlaySound(byte[] Sound, SOUND_MODE fwdSound)
        {
            try
            {
                return PlaySound(Sound, IntPtr.Zero, fwdSound);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool PlaySound(byte[] Sound)
        {
            try
            {
                return PlaySound(Sound, IntPtr.Zero, SOUND_MODE.SND_MEMORY |
                    SOUND_MODE.SND_SYNC | SOUND_MODE.SND_NOSTOP | SOUND_MODE.SND_NOWAIT |
                    SOUND_MODE.SND_NODEFAULT);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool PlaySoundSync(byte[] Sound)
        {
            try
            {
                return PlaySound(Sound, IntPtr.Zero, SOUND_MODE.SND_MEMORY |
                    SOUND_MODE.SND_NOSTOP | SOUND_MODE.SND_NOWAIT | SOUND_MODE.SND_SYNC |
                    SOUND_MODE.SND_NODEFAULT);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region UtlMon
        [DllImport(@"urlmon.dll", CharSet = CharSet.Unicode)]
        private extern static uint FindMimeFromData(
            uint pBC,
            [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            uint cbSize,
            [MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed,
            uint dwMimeFlags,
            out uint ppwzMimeOut,
            uint dwReserverd
        );

#pragma warning disable IDE0060 // Remove unused parameter
        public static string FindMimeFromData(byte[] data)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            //Do not use IE
            return null;

            /*
            if (data == null || data.Length <= 256)
                return null;

            try
            {
                byte[] buffer = new byte[Math.Min((uint)data.Length, 256)];
                Array.Copy(data, buffer, buffer.Length);

                var result = FindMimeFromData(0, null, buffer, (uint)buffer.Length, null, 0, out uint mimeType, 0);
                if (result != 0)
                    return null;

                IntPtr mimeTypePtr = new IntPtr(mimeType);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch (Exception)
            {
                return null;
            }
            */
        }

        public static string FindMimeFromFile(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"{filename} not found");

            using var fs = new FileStream(filename, FileMode.Open);
            byte[] buffer = new byte[Math.Min(fs.Length, 256)];
            Utils.ReadStreamToBuffer(fs, buffer);
            fs.Close();
            return FindMimeFromData(buffer);
        }
        #endregion
    }
}