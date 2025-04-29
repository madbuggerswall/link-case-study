using UnityEngine;
using System.Collections.Generic;
using System.Text;

///<summary>
/// Mutable String class, optimized for speed and memory allocations while retrieving the final result as a string.
/// Similar use than StringBuilder, but avoid a lot of allocations done by StringBuilder (conversion of int and float to string, frequent capacity change, etc.)
/// Author: Nicolas Gadenne contact@gaddygames.com
///</summary>
public class FastString {
	///<summary>Immutable string. Generated at last moment, only if needed</summary>
	string stringGenerated = "";
	///<summary>Is stringGenerated is up to date ?</summary>
	bool isStringGenerated = false;

	///<summary>Working mutable string</summary>
	char[] buffer = null;
	int bufferPos = 0;
	int charsCapacity = 0;

	///<summary>Temporary string used for the Replace method</summary>
	List<char> replacement = null;

	object valueControl = null;
	int valueControlInt = int.MinValue;

	public FastString(int initialCapacity = 32) {
		buffer = new char[charsCapacity = initialCapacity];
	}

	public bool IsEmpty() {
		return (isStringGenerated ? (stringGenerated == null) : (bufferPos == 0));
	}

	///<summary>Return the string</summary>
	public override string ToString() {
		if (!isStringGenerated) // Regenerate the immutable string if needed
		{
			stringGenerated = new string(buffer, 0, bufferPos);
			isStringGenerated = true;
		}
		return stringGenerated;
	}

	// Value controls methods: use a value to check if the string has to be regenerated.

	///<summary>Return true if the valueControl has changed (and update it)</summary>
	public bool IsModified(int newControlValue) {
		bool changed = (newControlValue != valueControlInt);
		if (changed)
			valueControlInt = newControlValue;
		return changed;
	}

	///<summary>Return true if the valueControl has changed (and update it)</summary>
	public bool IsModified(object newControlValue) {
		bool changed = !(newControlValue.Equals(valueControl));
		if (changed)
			valueControl = newControlValue;
		return changed;
	}

	// Set methods: 

	///<summary>Set a string, no memorry allocation</summary>
	public void Set(string str) {
		// We fill the chars list to manage future appends, but we also directly set the final stringGenerated
		Clear();
		Append(str);
		stringGenerated = str;
		isStringGenerated = true;
	}
	///<summary>Caution, allocate some memory</summary>
	public void Set(object str) {
		Set(str.ToString());
	}

	///<summary>Append several params: no memory allocation unless params are of object type</summary>
	public void Set<T1, T2>(T1 str1, T2 str2) {
		Clear();
		Append(str1); Append(str2);
	}
	public void Set<T1, T2, T3>(T1 str1, T2 str2, T3 str3) {
		Clear();
		Append(str1); Append(str2); Append(str3);
	}
	public void Set<T1, T2, T3, T4>(T1 str1, T2 str2, T3 str3, T4 str4) {
		Clear();
		Append(str1); Append(str2); Append(str3); Append(str4);
	}
	///<summary>Allocate a little memory (20 byte)</summary>
	public void Set(params object[] str) {
		Clear();
		for (int i = 0; i < str.Length; i++)
			Append(str[i]);
	}

	// Append methods, to build the string without allocation

	///<summary>Reset the char array</summary>
	public FastString Clear() {
		bufferPos = 0;
		isStringGenerated = false;
		return this;
	}

	///<summary>Append a string without memory allocation</summary>
	public FastString Append(string value) {
		ReallocateIFN(value.Length);
		int n = value.Length;
		for (int i = 0; i < n; i++)
			buffer[bufferPos + i] = value[i];
		bufferPos += n;
		isStringGenerated = false;
		return this;
	}
	///<summary>Append an object.ToString(), allocate some memory</summary>
	public FastString Append(object value) {
		Append(value.ToString());
		return this;
	}

	///<summary>Append an int without memory allocation</summary>
	public FastString Append(int value) {
		// Allocate enough memory to handle any int number
		ReallocateIFN(16);

		// Handle the negative case
		if (value < 0) {
			value = -value;
			buffer[bufferPos++] = '-';
		}

		// Copy the digits in reverse order
		int nbChars = 0;
		do {
			buffer[bufferPos++] = (char) ('0' + value % 10);
			value /= 10;
			nbChars++;
		} while (value != 0);

		// Reverse the result
		for (int i = nbChars / 2 - 1; i >= 0; i--) {
			char c = buffer[bufferPos - i - 1];
			buffer[bufferPos - i - 1] = buffer[bufferPos - nbChars + i];
			buffer[bufferPos - nbChars + i] = c;
		}
		isStringGenerated = false;
		return this;
	}

	///<summary>Append a float without memory allocation.</summary>
	public FastString Append(float valueF) {
		double value = valueF;
		isStringGenerated = false;
		ReallocateIFN(32); // Check we have enough buffer allocated to handle any float number

		// Handle the 0 case
		if (value == 0) {
			buffer[bufferPos++] = '0';
			return this;
		}

		// Handle the negative case
		if (value < 0) {
			value = -value;
			buffer[bufferPos++] = '-';
		}

		// Get the 7 meaningful digits as a long
		int nbDecimals = 0;
		while (value < 1000000) {
			value *= 10;
			nbDecimals++;
		}
		long valueLong = (long) System.Math.Round(value);

		// Parse the number in reverse order
		int nbChars = 0;
		bool isLeadingZero = true;
		while (valueLong != 0 || nbDecimals >= 0) {
			// We stop removing leading 0 when non-0 or decimal digit
			if (valueLong % 10 != 0 || nbDecimals <= 0)
				isLeadingZero = false;

			// Write the last digit (unless a leading zero)
			if (!isLeadingZero)
				buffer[bufferPos + (nbChars++)] = (char) ('0' + valueLong % 10);

			// Add the decimal point
			if (--nbDecimals == 0 && !isLeadingZero)
				buffer[bufferPos + (nbChars++)] = '.';

			valueLong /= 10;
		}
		bufferPos += nbChars;

		// Reverse the result
		for (int i = nbChars / 2 - 1; i >= 0; i--) {
			char c = buffer[bufferPos - i - 1];
			buffer[bufferPos - i - 1] = buffer[bufferPos - nbChars + i];
			buffer[bufferPos - nbChars + i] = c;
		}

		return this;
	}

	///<summary>Replace all occurences of a string by another one</summary>
	public FastString Replace(string oldStr, string newStr) {
		if (bufferPos == 0)
			return this;

		if (replacement == null)
			replacement = new List<char>();

		// Create the new string into replacement
		for (int i = 0; i < bufferPos; i++) {
			bool isToReplace = false;
			// If first character found, check for the rest of the string to replace
			if (buffer[i] == oldStr[0]) {
				int k = 1;
				while (k < oldStr.Length && buffer[i + k] == oldStr[k])
					k++;
				isToReplace = (k >= oldStr.Length);
			}
			// Do the replacement
			if (isToReplace) {
				i += oldStr.Length - 1;
				if (newStr != null)
					for (int k = 0; k < newStr.Length; k++)
						replacement.Add(newStr[k]);
			} else {
				// No replacement, copy the old character
				replacement.Add(buffer[i]);
			}
		}

		// Copy back the new string into chars
		ReallocateIFN(replacement.Count - bufferPos);
		for (int k = 0; k < replacement.Count; k++)
			buffer[k] = replacement[k];
		bufferPos = replacement.Count;
		replacement.Clear();
		isStringGenerated = false;
		return this;
	}

	void ReallocateIFN(int nbCharsToAdd) {
		if (bufferPos + nbCharsToAdd > charsCapacity) {
			charsCapacity = System.Math.Max(charsCapacity + nbCharsToAdd, charsCapacity * 2);
			char[] newChars = new char[charsCapacity];
			buffer.CopyTo(newChars, 0);
			buffer = newChars;
		}
	}
}