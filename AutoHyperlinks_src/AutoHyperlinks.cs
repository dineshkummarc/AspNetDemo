/// <summary>
/// Convert domain names and url paths to real web links. Handles the most common web transport protocols.
/// Existing <a> tag contents are ignored. Any valid urls within a <nolink></nolink> tag are ignored.
/// </summary>
/// <param name="strvar">String to process.</param>
/// <param name="param">String of parameters to insert into the resultant <a> tags, like target="_blank".</param>
/// <returns>A string with links</returns>
public static string AutoHyperlinks(string strvar, string param)
{
	// (c)2006 Michael Argentini, http://www.nonsequiturs.com.
	//
	// Please keep this copyright intact.
	// You may use or modify this code however you see fit,
	// within the scope of application or web site functionality.
	// Distribution of this code as an example or snippet is
	// prohibited. In this case, please link to the code example
	// on the nonsequiturs.com site directly!

	
	// First, process all <nolink> areas and change period characters temporarily to avoid auto-hyperlink processing.
	string final = strvar;

	Regex regex = new Regex(@"<nolink>(.*?)</nolink>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
	
	MatchCollection theMatches = regex.Matches(strvar);
	
	for (int index = 0; index < theMatches.Count; index++)
	{
		final = final.Replace(theMatches[index].ToString(), theMatches[index].ToString().Replace(".", "[[[pk:period]]]"));
	}

	// Second, process all existing <a> tags and change period characters in them temporarily to avoid auto-hyperlink processing.
	regex = new Regex(@"<a(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
	
	theMatches = regex.Matches(final);
	
	for (int index = 0; index < theMatches.Count; index++)
	{
		final = final.Replace(theMatches[index].ToString(), theMatches[index].ToString().Replace(".", "[[[pk:period]]]"));
	}

	// Third, temporarily alter any digit sequences that are formatted like domain names.
	final = Regex.Replace(final, @"(?<=\d)\.(?=\d)", "[[[pk:period]]]");
	
	// Fourth, look for, and process, any linkable domain names or URLs.
	Regex tags = new Regex(@"([a-zA-Z0-9\:/\-]*[a-zA-Z0-9\-_]\.[a-zA-Z0-9\-_][a-zA-Z0-9\-_][a-zA-Z0-9\?\=&#_\-/\.]*[^<>,;\.\s\)\(\]\[\""])");

	// Fifth, fix any inadvertently altered protocol strings.
	final = tags.Replace(final, "<a href=\"http://$&\"" + param + ">$&</a>");
	final = final.Replace("http://https://", "https://");
	final = final.Replace("http://http://", "http://");
	final = final.Replace("http://ftp://", "ftp://");
	final = final.Replace("http://rtsp://", "rtsp://");
	final = final.Replace("http://mms://", "mms://");
	final = final.Replace("http://pcast://", "pcast://");
	final = final.Replace("http://sftp://", "sftp://");

	final = final.Replace("[[[pk:period]]]", ".");
	final = final.Replace("<nolink>", "");
	final = final.Replace("</nolink>", "");

	// Lastly, return the processed string.
	return final;
}
