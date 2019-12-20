// GolfBot.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "windows.h"
#include <fstream>
#include <TlHelp32.h>
#include <iostream>
#include <fstream>
#include <string>
#include <iostream>
#include <conio.h>
#include <d3d11.h>


void LeftDown();
void LeftUp();
void mouseUp(LONG y);
void mouseLR(LONG x);
void go(int &level, int &course);
void KeyPress(char key);
void reset(int &level);
void MouseMove(int x, int y);
void LeftClick();
void shoot(LONG power);
void toggleShoot(LONG power);
void displayCurrentCourse(int &course);
void devmode();
void done(int &totalLR);
void login();
void toHole();
void enter();
void toggleCourse(int &course);
std::string GetActiveWindowTitle();
void testDraw(bool *draw);

int main()
{
	HWND consoleWindowHandle = GetConsoleWindow();
	if (consoleWindowHandle) 
	{
		SetWindowPos(
			consoleWindowHandle, // window handle
			HWND_TOPMOST, // "handle to the window to precede
						  // the positioned window in the Z order
						  // OR one of the following:"
						  // HWND_BOTTOM or HWND_NOTOPMOST or HWND_TOP or HWND_TOPMOST
			0, 0, // X, Y position of the window (in client coordinates)
			300, 500, // cx, cy => width & height of the window in pixels
			SWP_DRAWFRAME | SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW // The window sizing and positioning flags.
			);
	}

	int level = 1; //DEV CODE /login 131230 //login 131230 tohole 18; /login 131230 setgrav 0
	int course = 1;
	POINT p;        //900 is quarter turn
	LONG power = 600;
	int tlr = 0;
	displayCurrentCourse(course);
	std::cout << "Power: " << power << std::endl;
	int test = 2410;
	bool draw = true;
	bool *drawPointer = &draw;
	while (1)
	{
		//GetCursorPos(&p);
		//std::cout << "x " << p.x << " " << "y " << p.y << std::endl;
		//system("cls");
	//	testDraw(drawPointer);
		if (GetActiveWindowTitle() == "Golf With Your Friends")
		{
			if (GetAsyncKeyState(VK_F1) & 0x8000)
			{
				while (GetAsyncKeyState(VK_F1) & 0x8000) {}
				go(level, course);
			}
			if (GetAsyncKeyState(VK_F3) & 0x8000)
			{
				while (GetAsyncKeyState(VK_F3) & 0x8000) {}
				toggleCourse(course);
			}
			if (GetAsyncKeyState(VK_F5) & 0x8000)
			{
				while (GetAsyncKeyState(VK_F5) & 0x8000) {}
				reset(level);
				std::cout << "RESET!" << std::endl;
			}
			if (GetAsyncKeyState(0x50) & 0x8000) //p
			{
				while (GetAsyncKeyState(0x50) & 0x8000) {}
				if (GetAsyncKeyState(VK_CONTROL) & 0x8000)
				{
					mouseLR(10);
					tlr += 10;
				}
				else if (GetAsyncKeyState(VK_LSHIFT) & 0x8000)
				{
					mouseLR(100);
					tlr += 100;
				}
				else
				{
					mouseLR(1);
					tlr += 1;
				}
			}
			if (GetAsyncKeyState(0x4F) & 0x8000) //o
			{
				while (GetAsyncKeyState(0x4F) & 0x8000) {}
				if (GetAsyncKeyState(VK_CONTROL) & 0x8000)
				{
					mouseLR(-10);
					tlr -= 10;
				}
				else if (GetAsyncKeyState(VK_LSHIFT) & 0x8000)
				{
					mouseLR(-100);
					tlr -= 100;
				}
				else
				{
					mouseLR(-1);
					tlr -= 1;
				}
			}
			if (GetAsyncKeyState(0x54) & 0x8000) //t
			{
				while (GetAsyncKeyState(0x54) & 0x8000) {}
				shoot(power);
				std::cout << "shot at power " << power << std::endl;
			}
			if (GetAsyncKeyState(0x59) & 0x8000) //y
			{
				while (GetAsyncKeyState(0x59) & 0x8000) {}
				shoot(500);
				//std::cout << "shot at power " << power << std::endl;
				Sleep(test += 5);
				std::cout << "Slept " << test << " miliseconds" << std::endl;
				LeftDown();
				Sleep(30);
				LeftUp();
			}
			if (GetAsyncKeyState(VK_F7) & 0x8000)
			{
				while (GetAsyncKeyState(VK_F7) & 0x8000) {}
				if (GetAsyncKeyState(VK_CONTROL) & 0x8000)
				{
					if (power > 9)
						power -= 10;
					else
						power = 0;
				}
				else if (GetAsyncKeyState(VK_LSHIFT) & 0x8000)
				{
					if (power > 99)
						power -= 100;
					else
						power = 0;
				}
				else
				{
					if (power > 0)
						power--;
				}
				std::cout << "Power: " << power << std::endl;
			}
			if (GetAsyncKeyState(VK_F8) & 0x8000)
			{
				while (GetAsyncKeyState(VK_F8) & 0x8000) {}
				if (GetAsyncKeyState(VK_CONTROL) & 0x8000)
				{
					if (power < 591)
						power += 10;
					else
						power = 600;
				}
				else if (GetAsyncKeyState(VK_LSHIFT) & 0x8000)
				{
					if (power < 501)
						power += 100;
					else
						power = 600;
				}
				else
				{
					if (power < 600)
						power++;
				}
				std::cout << "Power: " << power << std::endl;
			}
			if (GetAsyncKeyState(VK_F9) & 0x8000)
			{
				while (GetAsyncKeyState(VK_F9) & 0x8000) {}
				done(tlr);
				std::cout << " and final shot power was: " << power << std::endl;
				tlr = 0;
			}
			if (GetAsyncKeyState(VK_INSERT) & 0x8000)
			{
				while (GetAsyncKeyState(VK_INSERT) & 0x8000) {}
				devmode();
			}
			if (GetAsyncKeyState(VK_RIGHT) & 0x8000)
			{
				while (GetAsyncKeyState(VK_RIGHT) & 0x8000) {}
				mouseLR(900);
				tlr += 900;
			}
			if (GetAsyncKeyState(VK_LEFT) & 0x8000)
			{
				while (GetAsyncKeyState(VK_LEFT) & 0x8000) {}
				mouseLR(-900);
				tlr -= 900;
			}
		}
	}
	return 1;
}

void devmode()
{
	login();
	enter();
	Sleep(100);
	enter();
	login();
	toHole();
	KeyPress('1');
	KeyPress('8');
	enter();
}

std::string GetActiveWindowTitle()
{
	char wnd_title[256];
	HWND hwnd = GetForegroundWindow(); // get handle of currently active window
	GetWindowText(hwnd, wnd_title, sizeof(wnd_title));
	return wnd_title;
}

void login()
{
	enter();
	Sleep(100);
	keybd_event(0xBF, 0, 0, 0);
	keybd_event(0xBF, 0, 0x02, 0);
	Sleep(100);
	KeyPress('l');
	KeyPress('o');
	KeyPress('g');
	KeyPress('i');
	KeyPress('n');
	KeyPress(' ');
	KeyPress('1'); // login 131230
	KeyPress('3');
	KeyPress('1');
	KeyPress('2');
	KeyPress('3');
	KeyPress('0');
}

void toHole()
{
	KeyPress(' ');
	KeyPress('t');
	KeyPress('o');
	KeyPress('h');
	KeyPress('o');
	KeyPress('l');
	KeyPress('e');
	KeyPress(' ');
}

void enter() 
{
	const int KEYEVENT_KEYUP = 0x02;
	keybd_event(VK_RETURN, 0, 0, 0);
	keybd_event(VK_RETURN, 0, KEYEVENT_KEYUP, 0);
}

void reset(int &level)
{
	level = 1;
}

void LeftClick()
{
	INPUT    Input = { 0 };
	// left down 
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
	::SendInput(1, &Input, sizeof(INPUT));

	// left up
	::ZeroMemory(&Input, sizeof(INPUT));
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_LEFTUP;
	::SendInput(1, &Input, sizeof(INPUT));
}

void MouseMove(int x, int y)
{
	double fScreenWidth = ::GetSystemMetrics(SM_CXSCREEN) - 1;
	double fScreenHeight = ::GetSystemMetrics(SM_CYSCREEN) - 1;
	double fx = x*(65535.0f / fScreenWidth);
	double fy = y*(65535.0f / fScreenHeight);
	INPUT  Input = { 0 };
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
	Input.mi.dx = fx;
	Input.mi.dy = fy;
	::SendInput(1, &Input, sizeof(INPUT));
}

void KeyPress(char key)
{
	INPUT input = { 0 };
	INPUT shift = { 0 };
	shift.type = INPUT_KEYBOARD;
	input.type = INPUT_KEYBOARD;
	shift.ki.wVk = VK_SHIFT;
	input.ki.wVk = VkKeyScanEx(key, GetKeyboardLayout(0));
	if (key > 64 && key < 91) // if caps key send shift
	{
		shift.ki.dwFlags = 0;
		SendInput(1, &shift, sizeof(INPUT)); //send shift down
		input.ki.dwFlags = 0;
		SendInput(1, &input, sizeof(INPUT)); //send key down
		ZeroMemory(&shift, sizeof(INPUT));
		shift.type = INPUT_KEYBOARD;
		shift.ki.wVk = VK_SHIFT;
		shift.ki.dwFlags = KEYEVENTF_KEYUP;
		SendInput(1, &shift, sizeof(INPUT)); //shift up
		ZeroMemory(&input, sizeof(INPUT));
		input.type = INPUT_KEYBOARD;
		input.ki.wVk = VkKeyScanEx(key, GetKeyboardLayout(0));
		input.ki.dwFlags = KEYEVENTF_KEYUP;
		SendInput(1, &input, sizeof(INPUT)); //key up
	}
	else
	{
		SendInput(1, &input, sizeof(INPUT));
		ZeroMemory(&input, sizeof(INPUT));
		input.type = INPUT_KEYBOARD;
		input.ki.wVk = VkKeyScanEx(key, GetKeyboardLayout(0));
		input.ki.dwFlags = KEYEVENTF_KEYUP;
		SendInput(1, &input, sizeof(INPUT));
	}
	Sleep(50);

}

void LeftDown()
{
	INPUT    Input = { 0 };
	// left down 
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
	::SendInput(1, &Input, sizeof(INPUT));
}

void LeftUp()
{
	INPUT    Input = { 0 };
	// left up
	::ZeroMemory(&Input, sizeof(INPUT));
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_LEFTUP;
	::SendInput(1, &Input, sizeof(INPUT));
}

void mouseUp(LONG y)
{
	INPUT  Input = { 0 };
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_MOVE;
	Input.mi.dy = y;
	::SendInput(1, &Input, sizeof(INPUT));
}

void mouseLR(LONG x)
{
	INPUT  Input = { 0 };
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_MOVE;
	Input.mi.dx = x;
	::SendInput(1, &Input, sizeof(INPUT));
	Sleep(250);
}

void shoot(LONG power) // maxpower 600
{
	LeftDown();
	mouseUp(-power);
	LeftUp();
}

void displayCurrentCourse(int &course)
{
	if (course == 1)
	{
		std::cout << "Current level is Oasis." << std::endl;
	}
	else if (course == 2)
	{
		std::cout << "Current level is Forest." << std::endl;
	}
	else
	{
		std::cout << "Current level is Twilight." << std::endl;
	}
}

void toggleCourse(int &course)
{
	switch (course)
	{
	case 1:
		course = 2;
		displayCurrentCourse(course);
		break;
	case 2:
		course = 3;
		displayCurrentCourse(course);
		break;
	case 3:
		course = 1;
		displayCurrentCourse(course);
		break;
	}
}

void toggleShoot(LONG power)
{
	std::cout << "Shoot with F2 when ready..." << std::endl;
	while (1)
	{
		if (GetAsyncKeyState(VK_F2) & 0x8000)
		{
			while (GetAsyncKeyState(VK_F2) & 0x8000) {}
			shoot(power);
			return;
		}
	}
}

void done(int &totalLR)
{
	int dif = 0;
	totalLR = totalLR % 3600;
	if (totalLR > 1800)
	{
		totalLR = totalLR - 3600;
	}
	else if (totalLR < -1800)
	{
		totalLR = totalLR + 3600;
	}
	std::cout << "Total pixels moved was: " << totalLR;
}

void skip(int &level) {
	std::cout << "Skipped hole " << level << std::endl;
}

void testDraw(bool *draw)
{
	PAINTSTRUCT ps;
	HDC hdc;
	if (GetActiveWindowTitle() == "Golf With Your Friends")
	{
		if (*draw = true) {
			HWND hwnd = GetForegroundWindow();
			/*hdc = BeginPaint(hwnd, &ps);
			TextOut(hdc, 1500, 100, "TEST DRAW", 9);
			EndPaint(hwnd, &ps);*/
			*draw = false;
		}
	}

}

void go(int &level, int &course)
{
	switch (course) {
	case 1: //OASIS
		switch (level)
		{
		case 1:
			shoot(250);
			Sleep(3500);
			mouseLR(-990);
			shoot(150);
			level++;
			break;
		case 2:
			mouseLR(1350);
			shoot(320);
			level++;
			break;
		case 3:
			mouseLR(-30);
			shoot(510);
			level++;
			break;
		case 4:
			shoot(410);
			level++;
			break;
		case 5:
			mouseLR(40);
			shoot(200);
			Sleep(6000);
			mouseLR(-284);
			shoot(512);
			level++;
			break;
		case 6:
			mouseLR(-110);
			shoot(560);
			level++;
			break;
		case 7:
			mouseLR(1333);
			shoot(600);
			level++;
			break;
		case 8:
			shoot(375);
			level++;
			break;
		case 9:
			shoot(550);
			Sleep(8000);
			mouseLR(1813);
			shoot(400);
			level++;
			break;
		case 10:
			mouseLR(80);
			shoot(80);
			Sleep(4500);
			mouseLR(1467);
			shoot(350);
			level++;
			break;
		case 11:
			mouseLR(-120); //water level try sleeps/jumps again
			shoot(490);
			Sleep(12000);
			LeftDown();
			Sleep(30);
			LeftUp();
			level++;
			break;
		case 12:
			mouseLR(30);
			shoot(481);
			Sleep(7000);
			mouseLR(970);
			shoot(554);
			level++;
			break;
		case 13:
			mouseLR(450);
			toggleShoot(554);
			Sleep(6500);
			mouseLR(-585);//should be good
			shoot(420);
			level++;
			break;
		case 14: //fix
			/*shoot(400);
			Sleep(12000);
			mouseLR(30);
			shoot(290);
			Sleep(10000);
			mouseLR(-30);
			shoot(190);*/
			skip(level);
			level++;
			break;
		case 15:
			mouseLR(20);
			shoot(510);
			Sleep(5000);
			mouseLR(1076);
			shoot(575);
			level++;
			break;
		case 16:
			mouseLR(-66);
			shoot(600);
			Sleep(8000); 
			mouseLR(125); //fix last shot
			shoot(522); //might not be consistent
			level++;
			break;
		case 17:
			mouseLR(320);
			shoot(600);
			Sleep(8000);
			mouseLR(-480);
			shoot(100);
			level++;
			break;
		case 18:
			mouseLR(227);
			shoot(480);
			level++;//227 480 CLOSE -560 toggle180
			break;
		}
		break;
	case 2: //FOREST
		switch (level)
		{
		case 1:
			mouseLR(95);
			shoot(350);
			level++;
			break;
		case 2:
			shoot(350);
			level++;
			break;
		case 3:
			shoot(350);
			level++;
			break;
		case 4:
			shoot(350);
			level++;
			break;
		case 5:
			shoot(350); //fix
			level++;
			break;
		case 6:
			shoot(400);
			level++;
			break;
		case 7:
			shoot(276);
			level++;
			break;
		case 8:
			mouseLR(-49);
			toggleShoot(540);
			level++;
			break;
		case 9: //not OK
			shoot(300);
			level++;
			break;
		case 10:
			mouseLR(-105);
			toggleShoot(555);
			level++;
			break;
		case 11:
			mouseLR(-389);
			shoot(600);
			level++;
			break;
		case 12:					
			shoot(260);
			Sleep(5500);
			mouseLR(905);
			shoot(425);
			level++;
			break;
		case 13:
			mouseLR(470);
			shoot(420);
			level++;
			break;
		case 14:
			mouseLR(-143); //test/fix
			shoot(599);
			level++;
			break;
		case 15:
			mouseLR(320);
			shoot(500);
			Sleep(8000);
			mouseLR(-1895);
			shoot(250);
			level++;
			break;
		case 16:
			mouseLR(10);
			toggleShoot(550); //TIME WHEN WOOD FAR LEFT -- test -- close?
			level++;
			break;
		case 17:
			mouseLR(280);
			shoot(470);
			level++;
			break;
		case 18:
			//shoot(120);
			//Sleep(9000);
			//mouseLR(670);
			//toggleShoot(320);
			//reset(level);
			break;
		}
		break;
	case 3: //TWILIGHT
		switch (level)
		{
		case 1:
			mouseLR(-140);
			shoot(170);
			level++;
			break;
		case 2:
			mouseLR(-180);
			shoot(150);
			Sleep(6000);
			mouseLR(1300);
			shoot(120);
			//skip(level);
			level++;
			break;
		case 3:
			shoot(550);
			Sleep(10000);
			//mouseLR(-1330);//fix
			//shoot(50);
			level++;
			break;
		case 4:
			shoot(350);
			level++;
			break;
		case 5:
			mouseLR(110);
			shoot(330);
			level++;
			break;
		case 6:
			mouseLR(-61);
			shoot(464);// -- close
			level++;
			break;
		case 7:
			//idk keep trying LR600 shoot420
			skip(level);
			level++;
			break;
		case 8:
			shoot(600);
			Sleep(5000); //possible hi1	
			shoot(500);
			level++;
			break;
		case 9:
			shoot(560); 
			Sleep(6000);
			mouseLR(1790); //test
			shoot(260);
			level++;
			break;
		case 10:
			shoot(600);//fix
			Sleep(7500);
			mouseLR(900);
			shoot(470);
			level++;
			break;
		case 11:
			//IDK
			skip(level);
			level++;
			break;
		case 12:
			shoot(50);
			level++;
			break;
		case 13:
			mouseLR(185);
			shoot(600);
			Sleep(7000);
			mouseLR(-60);
			shoot(200);
			level++;
			break;
		case 14:
			mouseLR(-125);
			toggleShoot(350);
			level++;
			break;
		case 15:
			//BOTTOM LEFT HOLE
			skip(level);
			level++;
			break;
		case 16:
			mouseLR(300);
			shoot(380);
			Sleep(10000);
			mouseLR(-1060); //test
			shoot(130);//
			level++;
			break; //check in freecam
		//case 17:
			//test
		//case 18:
			//test
		}
	}
	return;
}