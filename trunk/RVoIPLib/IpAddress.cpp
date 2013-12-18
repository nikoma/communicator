
//////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 1987-2006 LanScape Corporation.
// All Rights Reserved.
//
// Permission is hereby granted to use and develop additional software
// applications using the software contained within this source code module.
// Redistribution of this source code in whole or in part is strictly
// prohibited.
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// IN NO EVENT SHALL LANSCAPE BE LIABLE TO ANY PARTY FOR DIRECT, INDIRECT,
// SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES, INCLUDING LOST PROFITS,
// ARISING OUT OF THE USE OF THIS SOFTWARE AND ITS DOCUMENTATION, EVEN IF
// LANSCAPE HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// LANSCAPE SPECIFICALLY DISCLAIMS ANY WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE. THE SOFTWARE AND ACCOMPANYING DOCUMENTATION, IF
// ANY, PROVIDED HEREUNDER IS PROVIDED "AS IS". LANSCAPE HAS NO OBLIGATION
// TO PROVIDE MAINTENANCE, SUPPORT, UPDATES, ENHANCEMENTS, OR MODIFICATIONS.
//
//////////////////////////////////////////////////////////////////////////


#include "stdafx.h"
#include "IpAddress.h"









//////////////////////////////////////////////////////////////////////
//
// Determine if the machine name specified is an IP address or a
// UNC machine name.Input parameters are as follows:
//
//		MachineNameOrIp	-	The user specified IP address in dotted 
//							decimal format or the UNC machine name.
//
//		pIpAddress		-	If the specified machine name is an IP
//							address, the converted value is returned
//							to this location. If this value is NULL,
//							nothing will be written to this location.
//
// Return to the caller the following:
//
//		TRUE	-	The machine name is an IP address.
//
//		FALSE	-	The machine name is NOT an IP address.
//
//////////////////////////////////////////////////////////////////////
BOOL CIpAddress::IsIpAddress(char *_MachineNameOrIp, BYTE *_IpAddress)
{
	BOOL ret = TRUE;				// return value.
	int value;						// converted ip sub values.
	int cnt;						// loop counter.
	int NumLength;					// number length in ASCII diits.
	BOOL NumOk;						// number ok flag.
	char *str;						// temp string position.
	char tmp[MAX_IP_ADDRESS + 1];	// ip address numbers.
	BYTE LocIpAddress[4];			// the converted IP address.
	char *MachineNameOrIp;
	char *IpAddress;

	MachineNameOrIp = _MachineNameOrIp;
	IpAddress = (char *)_IpAddress;

	// see if ther are '.' characters in the string.
	if(!strchr(MachineNameOrIp,'.'))
	{
		// not an IP address.
		ret = FALSE;
	}
	else
	{
		// access the specified string.
		str = MachineNameOrIp;

		// process the digits and convert values.
		for(cnt=0;cnt<4;cnt++)
		{
			// check for a leading digit.
			if(!isdigit(*str))
			{
				// format error. terminate further processing.
				ret = FALSE;
				break;
			}
			else
			{
				// copy the digits.
				copytiln(tmp,str,'.',MAX_IP_ADDRESS);

				// get the number of digits copied.
				NumLength = strlen(tmp);

				// error check the string.
				if(cnt < 3)
				{
					// make sure the next character is the delimiter.
					if(*(str + NumLength) == '.')
					{
						NumOk = TRUE;
					}
					else
					{
						NumOk = FALSE;
					}
				}
				else
				{
					// make sure the next character is a NULL.
					if(*(str + NumLength) == 0)
					{
						NumOk = TRUE;
					}
					else
					{
						NumOk = FALSE;
					}
				}

				// make sure the next character is the delimiter.
				if(!NumOk)
				{
					// format error. terminate further processing.
					ret = FALSE;
					break;
				}
				else
				{
					// convert the number.
					value = atoi(tmp);

					// range check it.
					if((value < 0) || (value > 255))
					{
						// format error. terminate further processing.
						ret = FALSE;
						break;
					}
					else
					{
						// save the value.
						LocIpAddress[cnt] = (BYTE)value;

						// get to the next value.
						str = next(str,'.');
					}
				}
			}
		}
	}

	// see the name specified was an IP address.
	if(ret)
	{
		// see if the user wants the converted address.
		if(IpAddress)
		{
			// return it to the user.
			memcpy(IpAddress,LocIpAddress,4);
		}
	}

	return(ret);
}

char *CIpAddress::copytiln(char *dest, char *src, char ch, unsigned int len)
{
	unsigned int cnt;
	for(cnt = 0; ((cnt < len) && (*src != ch) && (*src)); cnt++)
	{
		*dest = *src;
		dest++;
		src++;
	}
	// null terminate string
	*dest = 0;
	return(src);
}


////////////////////////////////////////////////////////////////
//
// point to the next character after the first occurrence of the
// specified character.
//
////////////////////////////////////////////////////////////////
char *CIpAddress::next(char *str, char Schar)
{
	char *s = str;

	if(*s)
	{
		for(; *s && (*s!=Schar);)
		{
			s++;
		}

		if(*s)
		{
			s++;
		}
	}

	return(s);
}

