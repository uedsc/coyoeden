using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Reflection;
using CoyoEden.Core;
using SystemX.Web;

public partial class User_controls_xmanager_SourceEditor : UserControl
{
    static protected string _errorMsg = string.Empty;
    static protected string _extensionName = string.Empty;

    /// <summary>
    /// Handles page load event
    /// </summary>
    /// <param name="sender">Page</param>
    /// <param name="e">Event args</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Enabled = true;
        _extensionName = Path.GetFileName(Request.QueryString["ext"]);
        txtEditor.Text = ReadFile(GetExtFileName());
    }
    
    /// <summary>
    /// Buttons save hanler
    /// </summary>
    /// <param name="sender">Button</param>
    /// <param name="e">Event args</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ext = Request.QueryString["ext"];

        if (WriteFile(GetExtFileName(), txtEditor.Text))
        {
            Response.Redirect("default.aspx");
        }
        else
        {
            txtEditor.Text = _errorMsg;
            txtEditor.ForeColor = System.Drawing.Color.Red;
            btnSave.Enabled = false;
        }
    }

    /// <summary>
    /// Returns extension file name
    /// </summary>
    /// <returns>File name</returns>
    string GetExtFileName()
    {
      string fileName = HttpContext.Current.Request.PhysicalApplicationPath;
      ArrayList codeAssemblies = Utils.CodeAssemblies("CoyoEdenExtension");
      foreach (Assembly a in codeAssemblies)
      {
        Type[] types = a.GetTypes();
        foreach (Type type in types)
        {
          if (type.Name == _extensionName)
          {
            string assemblyName = type.Assembly.FullName.Split(".".ToCharArray())[0];
            assemblyName = assemblyName.Replace("App_SubCode_", "App_Code\\");
            string fileExt = assemblyName.Contains("VB_Code") ? ".vb" : ".cs";
			fileName += String.Format("{0}\\Extensions\\{1}{2}", assemblyName, _extensionName, fileExt);
          }
        }
      }
      return fileName;
    }

    /// <summary>
    /// Read extension source file from disk
    /// </summary>
    /// <param name="fileName">File Name</param>
    /// <returns>Source file text</returns>
    string ReadFile(string fileName)
    {
		string val = String.Format("Source for [{0}] not found", fileName);
        try
        {
            val = File.ReadAllText(fileName);
        }
        catch (Exception)
        {
            btnSave.Enabled = false;
        }
        return val;
    }

    /// <summary>
    /// Writes file to the disk
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="val">File source (text)</param>
    /// <returns>True if successful</returns>
    static bool WriteFile(string fileName, string val)
    {
        try
        {
            StreamWriter sw = File.CreateText(fileName);
            sw.Write(val);
            sw.Close();
        }
        catch (Exception e)
        {
            _errorMsg = e.Message;
            return false;
        }
        return true;
    }
}
