using System;
using System.Web;
using System.Web.UI.WebControls;

public partial class Views_shared_tinyMCE : System.Web.UI.UserControl
{

  public string Text
  {
    get { return Target.Text; }
	  set { Target.Text = value; }
  }

  public short TabIndex
  {
    get { return txtContent.TabIndex; }
    set { txtContent.TabIndex = value; }
  }

	public Unit Width
	{
		get { return txtContent.Width; }
		set { txtContent.Width = value; }
	}

	public Unit Height
	{
		get { return txtContent.Height; }
		set { txtContent.Height = value; }
	}
	/// <summary>
	/// target textbox applying tinyMCE
	/// </summary>
	public TextBox Target { get; set; }

	protected override void OnLoad(EventArgs e)
	{
		if (Target==null)
		{
			phEditorWrapper.Visible = true;
			Target = txtContent;
		}
		else {
			phEditorWrapper.Visible = false;
		}
		base.OnLoad(e);
	}
}
