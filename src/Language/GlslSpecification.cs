﻿using System;
using System.Collections.Generic;

namespace DMS.GLSL
{
	public static partial class GlslSpecification
	{
		private static HashSet<string> s_userKeyWords = new HashSet<string>();
		private static HashSet<string> s_userKeyWords2 = new HashSet<string>();

		public static IEnumerable<string> Keywords => s_keywords;
		public static IEnumerable<string> Functions => s_functions;
		public static IEnumerable<string> Variables => s_variables;

		public static IEnumerable<string> DefinedWords => s_variables;


		public static bool IsKeyword(string word) => s_keywords.Contains(word);
		public static bool IsVariable(string word) => s_variables.Contains(word);
		public static bool IsFunction(string word) => s_functions.Contains(word);
		public static bool IsUserKeyWord(string word) => s_userKeyWords.Contains(word);
		public static bool IsUserKeyWord2(string word) => s_userKeyWords2.Contains(word);

		public static bool IsIdentifierChar(char c)
		{
			return char.IsLetterOrDigit(c) || '_' == c;
		}

		public static bool IsIdentifierStartChar(char c)
		{
			return char.IsLetter(c) || '_' == c;
		}

		public static void SetUserKeyWords(string userKeyWords) => s_userKeyWords = ParseTokens(userKeyWords);
		public static void SetUserKeyWords2(string userKeyWords) => s_userKeyWords2 = ParseTokens(userKeyWords);

		private static HashSet<string> ParseTokens(string tokens)
		{
			char[] blanks = { ' ' };
			return new HashSet<string>(tokens.Split(blanks, StringSplitOptions.RemoveEmptyEntries));
		}
	}
}