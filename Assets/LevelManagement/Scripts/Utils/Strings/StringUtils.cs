using System;
using System.Text.RegularExpressions;

public static class StringUtils
{
	public static byte[] GetBytes(string str)
	{
	    byte[] bytes = new byte[str.Length * sizeof(char)];
	    System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
	    return bytes;
	}
	
	public static string GetString(byte[] bytes)
	{
	    char[] chars = new char[bytes.Length / sizeof(char)];
	    System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
	    return new string(chars);
	}

	public static T ParseEnum<T> (string value) where T : struct, IComparable, IFormattable, IConvertible
	{
		try {
			return (T)Enum.Parse (typeof (T), value, true);
		} catch (Exception e) {
			Console.Write (e);
		}

		return (T)Enum.GetValues (typeof (T)).GetValue (0);
	}

	public static bool IsValidEmail(string email)
	{
		Regex regex = new Regex(@"^[^@\s]+@[^@\s]+(\.[^@\s]+)+$");
		Match match = regex.Match(email);
		return match.Success;
	}

	public static bool IsValidPassword(string password)
	{
		return password.Length >= 6 && !password.Contains (" ");
	}
}

public static class StringExtension
{
	public static string GetEndingCharacters(this string source, int tail_length)
	{
		if(tail_length >= source.Length)
			return source;
		return source.Substring(source.Length - tail_length);
	}
	
	public static string GetStartingCharacters(this string source, int length)
	{
		if(length >= source.Length)
			return source;
		return source.Substring(0, length);
	}

	public static string Cypher(this string s, int x)
	{
		char[] chars = s.ToCharArray();
		
		for(int i = 0; i < chars.Length; i++)
		{
			chars[i] = (char)(chars[i] ^ x);
		}
		
		return new string(chars);
	}
	
	public static string FirstLetterToUpper(this string str)
	{
		if (str == null) {
			return null;
		}
		
		if (str.Length > 1) {
			return char.ToUpper(str[0]) + str.Substring(1);
		}
		
		return str.ToUpper();
	}
	
	public static string GetRealAssetPath(this string assetPath)
	{
		return assetPath.Substring(assetPath.IndexOf("Resources/") + "Resources/".Length)
			.Replace(".prefab", "")
			.Replace(".png", "")
			.Replace(".mat", "");
	}
}


