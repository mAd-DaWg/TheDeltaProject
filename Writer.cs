using System;
using System.Collections.Generic;
using System.Text;

namespace TheDeltaProject
{
	public class Writer
	{
		private const int TITLE_SIZE = 40;
		private const int LINE_LENGTH = 60;

		public Writer ()
		{
		}

		public static void write (string message)
		{
			Console.WriteLine(message);
		}

		public static void writeTitle (string title, string content)
		{
			string[] contentArray = content.Split (' ');

			List<string> contentLines = new List<string> ();

			int j = 0;
			string stringHolder;
			for (int i = 0; i < contentArray.Length; i++) {
				try {
					stringHolder = contentLines [j];
					contentLines [j] = stringHolder + contentArray [i] + " ";
				} catch (System.ArgumentOutOfRangeException e) {
					contentLines.Add (contentArray [i] + " ");
				}

				if (contentLines [j].Length > LINE_LENGTH) { // Limit the line length
					j++;
				}
			}

			// Make spacers
			int titleSize = title.Length;
			StringBuilder sb = new StringBuilder(TITLE_SIZE - titleSize);
			for (int i = 0; i < TITLE_SIZE - titleSize; i++) {
				sb.Append(" ");
			}
			string titleSpace = sb.ToString();

			sb = new StringBuilder(TITLE_SIZE);
			for (int i = 0; i < TITLE_SIZE; i++) {
				sb.Append(" ");
			}
			string spacer = sb.ToString();

			// Print message
			Console.WriteLine (title + titleSpace + contentLines [0]);
			for (int i = 1; i < contentLines.Count; i++) {
				Console.WriteLine (spacer + contentLines [i]);
			}
		}


	}
}

