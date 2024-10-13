using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Models {
  public static class SettingsExt {
    public const string CommonPathAdd = "\\PrompterFiles";
    public const string SettingsAdd = "\\PlaygroundSettings.sft";    
    public static string DefaultPath {
      get {
        var DefaultDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + SettingsExt.CommonPathAdd;
        if (!Directory.Exists(DefaultDir)) {
          Directory.CreateDirectory(DefaultDir);
        }
        return DefaultDir;
      }
    }
    public static string SettingsFileName { get { return DefaultPath + SettingsAdd; } }    
  }

  public static class StandardExt {
    
    /// <summary>
    /// Remove all instances of CToRemove from content
    /// </summary>
    /// <param name="content"></param>
    /// <param name="CToRemove"></param>
    /// <returns></returns>    
    public static string RemoveChar(this string content, char CToRemove) {
      string text = content;
      while (text.Contains(CToRemove)) {
        text = text.Remove(text.IndexOf(CToRemove), 1);
      }

      return text;
    }

    /// <summary>
    ///   Splits content on each character in delims string returns string[]
    /// </summary>
    /// <param name="content"></param>
    /// <param name="delims"></param>
    /// <returns></returns>
    public static string[] Parse(this string content, string delims) {
      return content.Split(delims.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Splits contents by delims and takes first string
    /// </summary>
    /// <param name="content"></param>
    /// <param name="delims"></param>
    /// <returns></returns>
    public static string ParseFirst(this string content, string delims) {
      string[] sr = content.Parse(delims);
      if (sr.Length > 0) {
        return sr[0];
      }
      return "";
    }

    /// <summary>
    /// Splits contents by delims and takes last string
    /// </summary>
    /// <param name="content"></param>
    /// <param name="delims"></param>
    /// <returns></returns>
    public static string ParseLast(this string content, string delims) {
      string[] sr = content.Parse(delims);
      if (sr.Length > 0) {
        return sr[sr.Length - 1];
      }
      return "";
    }
    
    public static Color AsColor(this string htmlColor) { 
      return ColorTranslator.FromHtml(htmlColor);
    }

    public static string AsString(this Color value) { 
      return ColorTranslator.ToHtml(value);
    }

    /// <summary>
    /// general case conversion to string or empty if null
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string AsString(this object obj) {
      try {
        return Convert.ToString(obj) ?? string.Empty;
      } catch {
        return string.Empty;
      }
    }
    /// <summary>
    /// on fail or null return 0
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>int</returns>
    public static int AsInt(this string obj) {
      return int.TryParse(obj, out int r) ? r : 0;
    }
    public static int AsInt(this object obj) {
      string str = Convert.ToString(obj).ParseFirst(". ");
      if (int.TryParse(str, out int r)) {
        return r;
      }
      return 0;
    }

    public static bool AsBool(this string obj) { 
      if (bool.TryParse(obj,out bool b)) { return b; }
      return false;
    }

    public static float AsFloat(this string obj) { 
      return Convert.ToSingle(obj);
    }
    public static float AsFloat(this object obj) { 
      return Convert.ToSingle(obj);
    }
    public static decimal AsDecimal(this object obj) {
      return Convert.ToDecimal(obj);
    }
    /// <summary>
    /// byte[] to utf8 string; use AsString() to reverse
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static byte[] AsBytes(this string text) {
      return Encoding.UTF8.GetBytes(text);
    }

    /// <summary>
    /// Adds AsString support for byte[] 
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string AsString(this byte[] bytes) {
      return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    ///     Base 64 encodes string  encodes <null> for empty or null Text values. 
    /// </summary>
    /// <param name="Text"></param>
    /// <returns></returns>
    public static string AsBase64Encoded(this string Text) {
      if (Text == null || Text.Length < 1) { 
        return Convert.ToBase64String("<null>".AsBytes());
      }
      return Convert.ToBase64String(Text.AsBytes());
    }

    /// <summary>
    /// Base 64 decodes string  returns empty string if decode equals <null>
    /// </summary>
    /// <param name="Text"></param>
    /// <returns></returns>    
    public static string AsBase64Decoded(this string Text) {
      if (string.IsNullOrEmpty(Text)) return "";
      var decodedValue = Convert.FromBase64String(Text).AsString();
      if (decodedValue == "<null>") return "";
      return decodedValue;      
    }

    /// <summary>
    /// lower case first letter of content concat with remainder.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>    
    public static string AsLowerCaseFirstLetter(this string content) {
      if (string.IsNullOrEmpty(content)) return "";
      return content.Substring(0, 1).ToLower() + content.Substring(1);
    }
    /// <summary>
    /// Uppercase first letter of content concat with rest of content.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string AsUpperCaseFirstLetter(this string content) {
      if (string.IsNullOrEmpty(content)) return "";
      return content.Substring(0, 1).ToUpper() + content.Substring(1);
    }

    /// <summary>
    /// async read file from file system into string
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static async Task<string> ReadAllTextAsync(this string filePath) {
      using (var streamReader = new StreamReader(filePath)) {
        return await streamReader.ReadToEndAsync();
      }
    }

    /// <summary>
    /// async write content to fileName location on file system. 
    /// </summary>
    /// <param name="Content"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static async Task<int> WriteAllTextAsync(this string Content, string fileName) {
      using (var streamWriter = new StreamWriter(fileName)) {
        await streamWriter.WriteAsync(Content);
      }
      return 1;
    }

    public static StringDict AsDict(this string content, string delims) {
      var list = content.Parse(delims);
      var ret = new StringDict();
      foreach (string item in list) {
        ret.Add(item);
      }
      return ret;
    }
  }
}
