using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;

public class AutocompleteForm : Form
{
	#region Static and externs
	
	public static Image Base64ToImage(string base64String)
	{
		byte[] imageBytes = Convert.FromBase64String(base64String);
		var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
		ms.Write(imageBytes, 0, imageBytes.Length);
		return Image.FromStream(ms, true);
	}
	
	[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
	static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
	
	[DllImport("user32.dll")]
	static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
	
	#endregion
	
	#region Consts and readonly variables
	
	// Win API
	const int SW_SHOWNOACTIVATE = 4;
	const int HWND_TOPMOST = -1;
	const uint SWP_NOACTIVATE = 0x0010;
	
	// Base 64
	const string ClassB64 = @"iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAIWSURBVDhPY0AGZ2Yas55cbLJl5zTDhf8ZGBihwvjB1VXabNfXme18fsDj992tVt+XtRsshErhBo9XWXI+2Wm5982JsD8fTnj/f3fA8v/VlTrfl7brz4cqAYPTxac7N6dvVgVzXq3S5nm2x/LoxwtJfz+ei/z//pj7/7f7zf4/2qzzd2mb3rFVoaHMIHW3m25PftTy6MehtCMXV4WukmN4vtvs1KerWf8+X0n7//FM6P/3R13+P9hs+HdRm9npooZOi9ym6YbXu++ueTXx9a9nLc//3y2+939LzJYTDM93mRa/OxX65tOl5P8fTgf+v7/D7v/sNq+fBQ1T/uQ3zHi/s+Pkt0/zP/17N+nd/yeNT/8fTz3xeH3k+mCwcx9vN2t4ctDn3dN9Hj+3TTba3NDgwJLfMP1ZfuMs9+c9z7M+zPzw9k3fm/8Xiy89Xhe1MQisCQYurTFvPzTHZHFDAwMTiA/TCGI/aH9QfbX62r1lGbshNuEDyBpBILH3bItX/fGbalGrzKBC2AGyxpxFt6pzlj74kLX84X/Lwr13lSKWm4AVYQMgjXkN01blzzx6rHrTs59lG1/8T19y/3/QhIu/1SI2zIIqwwRAjX25fZt3125+/qtp14f/xeue/4+ace2nRsz6BQwM/wkkx///GQuW31tUteXV7/RF934ap26fQ1gTFISu+s8cOfH8OueKgxOhQkDAwAAAXHIfIXfjIQ8AAAAASUVORK5CYII=";
	const string EnumB64 = @"iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAH2SURBVDhPrY/fT1JxGIfP3+LG39CN1ZhzLF3ioomZptwgrczWwPTbYu1kRQwTSVRWdJFsGV5gHibtZAwJBEE8qYUtOR054EE5FuG6dPv0Y+eGze58rp/n8+6lTpz95S6jlLhCtsOdZDPYTlZm20hk+gJZmNAS/8g5vaLVkl/U1ZU3Bw5+ii78yD1GOUuj9HEIxfRN5GMmzLnOi4paS4HVqcqf7siHRTe+bYxiPWEHt0QjlzBjL21CwNkkKGotf8MSNyBXd55gNTaK+YADAf8wMu/MKCUMmLFpjg+zjE4lpm7JFd4GdsEJ9/NxjE2OIDxnxm7sMnx0g0CVkob0fuZ6Tvhg5LdYA8+96eTjM+1i1Kc/Cnl0kNbMKHP92EsZIcW7UIxcxAurWqDK3A3hl+RBNe9EJfcI37eskDcs+DOGJZ/uiJloFYPuFn7W0cS/HG7kPUNq3tF3dpWSUn3CYeEpktFxuL2TcE05wfit/y6897bI93vOqJQPahHiJqEqOJBdsYNl7AgFaCRDZkjL3QhNNcuWjlPHh1/DRqGy/RA7azS4yD1k2Nv4vHgVu9FLCI5p/h+uM91iiRtEIdUPIdaLXLgHX97qkWW0eG1rOBjsqK9T1Fpi061t7DMtmXc1k1c2DfHebSQui5rYr50mD3rrjYp2UlDUb2UrO9FnoRzGAAAAAElFTkSuQmCC";
	const string EnumPropB64 = @"iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAHzSURBVDhPrY/fT1JxGIfP3+LG39Fcay23aouWbWWbN8ybVmvk8mu1xtwih6BMLJYDFSMTiQPYMQgJCPmlJzgi4g+OB050UA5Ds9W6cPtE27lhszuf6+f5vHupM+cg0aORknfIbvg2yS/eJGnXDRJxXCPMxFUyP3KpW9HaqYTUHfV8f+NINKNZeoH6pg613ACqq/dRifeBNl8WFbWdb0G1qr4xKB9XLdhbN4FLDiMb1aGU1GJ/tQ+e0S5BUdv5F9ay/fKPshFrcRN8HgM880NgP2lRS/ZiTn/x9HDTr1aJmQfyIa9HkBmFZXIcYy9HEKa1+B6/hVndeYHiihV2a0/is4Uyn2C3+eUEx38Ip0TfUuDknZeGxA2inr2H/YwG0koPqpHrsD3tFKidck349fsPGs1jiFIDrRG0RrDC7mDOGzzxTfaKi5YrvMvQxc8MXeCtA5284e65NarIS0Lz6CeiqTzM9gUYrE7MuBiEkwVMu5flx2N2lfJBO1yxLNTqh8jktuFmYnDSISwwX7AU5WB1fpQfDr86PcxwJaFclcHmeQRiX+ELpuAJpECHWIxP+f8fRlIb4vpWBencLiLpQutSFu5AGg5vDIbX7xuPnts6FLUd/+dMt4uJk2l3iFjfMMRop4l+4i15ZnSQJyabRtHOCor6C4FJUlBr+AXvAAAAAElFTkSuQmCC";
	const string EventB64 = @"iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAHaSURBVDhPY0AHby4Xz/t5r+L/z/vV/79eqfz/8WTl/7d7q/6/2FT1/8my6v+9Sb5PgMrkIKqh4NeDCsPvd8r//XzQ8v/H3c7/n89V/n93qPL/q+1V/5+trv7fk+L7l5mZwQuqHAL+/2dgfHWx8OyP+zX/fz9f+//r5cr/H45X/n+zB2jbhqr/k7P8/7ExM4dBlSMA0KY4kBN/P1/3/8ft1v+fzgBtOwi0bWvV//llof+5OFjToEoR4P+NMt73V4rf/3rY8f/Xo8n/v1wE2nas8v/rXVX/VzdF/efn5iyFKkUF768WTwDZBtL47VrF/0+ngQGyv+r/yy1V/3d0JP/Y05J+f3tl2r1AU71uoHImsKYfd0tVv94u+wPS+ON2xf8vFyr/vz8CtG1n1f/n66r+P1pU/f/KhLL/3oZaD4DKEaH5/34Dh6WRtI+ZgVSAsS4Qa0Lw/skpu54sr/5/rCv3v4my7AGgUh6IDjzgxfZKv6fA4N9Un/hfUUx4MVCIBSKDB7zfWy3/cE3Z19kFwf9EeLmLoML4wf39DRz31pTcaE/x+M7OzOwDFSYM7q4vmp/ma3YLyNSGiBABXu0t94xxN1wIZIpDRIgAj48Vcsb7GDoDmRwQEUKAgQEAnYj32I11B38AAAAASUVORK5CYII=";
	const string MethodB64 = @"iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAFhSURBVDhPzY1PSwJBHIa3omMd+hBB0CHwkEL0DTrFfIsMIejQIYS8RDdX0AklCAOZYLfFtW1XD4ISFlu6hlAuK1r4h0oJMtDVmMZt9NChOkUvPId5X575Mf8v2I3HTWgedv09N61+DsZ4zAz0oRnoYQvYD2GIJ+nMMN1nbu6hLBiXqmgIkmQcIGVh0Hf8JjuSKN1AT8IhPGWJGKOJSoW3Za5EGy/LNghV69cbV+G87W3Xvso1T41jWg3+yNCjSL0WUSIpIRSV0X4kMTsQc06trjnzzeZeKzeUyu77AuvwlYh4slLSBaBmRaAkJXAsKiAcPp0eigRM6FR36qnilp5l7b5Xr519G+zMyxO/W7yLxtOZWJyPyfFgRJ7/FHPbVLQgl6pEwoS2JbYeuSX9VgCpCxFwkgKC6GzGGkiya/nNoUiEBuGdXWRX6fx9yOWNkejwuWj9u2hObR0uQw99/nkY5gP00PjOIwyqvAAAAABJRU5ErkJggg==";
	const string NamespaceB64 = @"iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAEYSURBVDhPlZI5TgNBFETbvsHsF8AECJxglgAjiCDBV4DMmY8IKQ5GCAkQRtyARaQsErz/pwYxaieUVLL7Vf2ZnlaHVnmer+I5fsEnwsYnYvOiKAbCv+oR3OF3wj1bN9jVg43JPqxj6wajJEnWgd/4WigS2Y110jRdE3I40uCDUCSyR3U2hTpvfBWKRPZmnc4b+YZjDd4LRSJbWKcsyyMHWZZtAexgvvCBwyUiO1TnFo8CTxjyp8afPGRbvUiWabCuqmrDIYsptq2eO1gisgt1pkL+jTPBhVAkMj9V6wr54JkGa6FIZFfWYcunQiFwxLv/GNwRcvWB7ZUb27rBrj5sn6y9cn8z3+6A4BI/4c4lJ3u2jN+VhobwA5C9URWh7oODAAAAAElFTkSuQmCC";
	const string PropertyB64 = @"iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAHlSURBVDhPrdBPSJNxHMfx0SnwIpiIeutgRxUEEUkP0kHzL2kOHWHKsNCyFP/r46E6COKlLqkI4qSaYQ8huQcasnAMoa22nMyQ5qPTZ06dw+d5aNtP+/Rz8xHT3fIL78Pz4/fi+fFVXfrkpCek3spNzirMS8lUosdXNGzJrIYtNkdvnRtt9fU0brLIKy3XQUl01qHvcaZJM1MUoBCqL46D8rNNvGUbDbqKvbUFNbYWa0+hUsPMbUTgyw9eKA3p7ODeN2HZWIuP42X4PlcFn03zD6xX4CvWi+OGp5Zg0DfD/vk+Bvq1FFbAwlaBmyw5RVr1DU+NvlCOQBwRHGc2sRgdzMfzZ53Y3BXR86QAY/TbYbgDdeVNJF27OpIYH5dB0TxdkCMCDwnB7xDByi8efolg+4DAubqJFx15eNraiPFpA+jOCqKrO5k/hwShMIEUJNiXCXZEgq1AGLw/jB/rAbi2wxjTcxchoX/z+SUYLS7wPhGLSxvgzC7MLbjwifaNlzASCwbpE337Mix2N4UybD8FmGxuzFvdMH51w8rLeP0uBpTpE/fEEEUSPP4gVr0ynB4Rdl6EdU2EbSMUG+re6Kc7exmhvZsRWrsYoaWDER61M0IT7WEbIzygld69t06vZkfFf41K9RdXioLitfHasQAAAABJRU5ErkJggg==";
	const string KeywordB64 = @"iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAADISURBVDhPYxhYoK2tzWZubi4Ow0AhJogMAWBmZqZoYmJSCMN6enrcUCn8wNLSktPU1FQJGwYZClWGCYyNjeXM3BbvNXNfehAFuy3ZB5RLgSrDBLq6uoJmXke+m3ttmWrmfaQKhM29tk408zr8A6jRBqoMEwD9JAZU/MPUfd0shMZNU829j/wE+tkTqgwTAE3VMHOdfcbUde45ZGzmOucMUGMrVBkVgZGRkQrQ5CoYBsYrD1QKP3BwcGABRjwfDAOFGCEyAw8YGADaQUjTtACbhAAAAABJRU5ErkJggg==";
	const string CustomB64 = @"iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAFdSURBVDhPYxh84H/Df6azq/4vOrvyfwNUiDD4//8/I1DTTCD+D8ar/889M/M/K1SagaHsbKBm/bH0u1172u5O3bjk7vxVuwxA4qdX/J0M14TA249s/M8L1hi6KpS59kSmcc++duNp65caz5x5BmzqsuYXx44t+fMMXfP2KW/XMdScil7afDR/Vefe5lUTts1YNWPd2lWzVuxRA2lc3PDs+aKGZ2/3z/1xAaZp08SXV9NdV91jqD4V6dt4OD+0a29L6ITtM0NnrlsdumTJNj6YRiD+D8Q/dkz9fHhd15Pz6c4rP6c5r/wKkmeoPBve2Xy4eHfPzp7d0zYs2z1nxU4dkPiShud1UI1gnOay6ilQ038g/gLWWH4m0qblcFFo767e0GkbV4TOWbVDCCwBBIsanpbBNAJtewHU9DfVZWUwVBo/WNL4vBiu0WlFAVSYOAAMpLwcz3UtUC7dAQMDAHT55qhar+uFAAAAAElFTkSuQmCC";
	
	// Images
	readonly Image ClassImg = Base64ToImage(ClassB64);
	readonly Image EnumImg = Base64ToImage(EnumB64);
	readonly Image EnumPropImg = Base64ToImage(EnumPropB64);
	readonly Image EventImg = Base64ToImage(EventB64);
	readonly Image MethodImg = Base64ToImage(MethodB64);
	readonly Image NamespaceImg = Base64ToImage(NamespaceB64);
	readonly Image PropertyImg = Base64ToImage(PropertyB64);
	readonly Image KeywordImg = Base64ToImage(KeywordB64);
	readonly Image CustomImg = Base64ToImage(CustomB64);
	
	// Controls
	readonly ListView suggestionsLV;
	readonly MethodControl methodMC;
	
	readonly ImageList imagesIL;
	
	// Others
	readonly Size FSize = new Size(150, 240);

	#endregion
	
	#region Variables and roperties
	
	new Size Size
	{
		get { return methodMC.Visible ? methodMC.Size : FSize; }
		set { base.Size = value; }
	}
	
	int Idx
	{
		get { return suggestionsLV.SelectedIndices.Count > 0 ? suggestionsLV.SelectedIndices[0] : -1; }
		set
		{
			if (value > -1 && value < suggestionsLV.Items.Count)
			{
				suggestionsLV.SelectedIndices.Add(value);
				suggestionsLV.SelectedItems[0].EnsureVisible();
			}
		}
	}
	
	List<Suggestion> QueuedSuggestions = new List<Suggestion>();
	
	#endregion
	
	#region New methods and overloads
	
	new public void Show()
	{
		ShowWindow(Handle, SW_SHOWNOACTIVATE);
		SetWindowPos(Handle.ToInt32(), HWND_TOPMOST, Left, Top, Width, Height, SWP_NOACTIVATE);
	}
	
	public void Show(int x, int y)
	{
		ShowWindow(Handle, SW_SHOWNOACTIVATE);
		SetWindowPos(Handle.ToInt32(), HWND_TOPMOST, x, y, Width, Height, SWP_NOACTIVATE);
	}
	
	#endregion
	
	#region Public variables
	
	public bool Suggesting { get { return Visible && suggestionsLV.Visible; } }
	
	#endregion
	
	#region Constructors
	
	public AutocompleteForm()
	{
		imagesIL = new ImageList();
		imagesIL.Images.Add("class", ClassImg);
		imagesIL.Images.Add("enum", EnumImg);
		imagesIL.Images.Add("enumprop", EnumPropImg);
		imagesIL.Images.Add("event", EventImg);
		imagesIL.Images.Add("method", MethodImg);
		imagesIL.Images.Add("namespace", NamespaceImg);
		imagesIL.Images.Add("property", PropertyImg);
		imagesIL.Images.Add("keyword", KeywordImg);
		imagesIL.Images.Add("custom", CustomImg);
		
		suggestionsLV = new ListView { Dock = DockStyle.Fill, View = View.Details,
			HeaderStyle = ColumnHeaderStyle.None, SmallImageList = imagesIL,
			HideSelection = false, MultiSelect = false };
		suggestionsLV.Columns.Add("", -2);
		
		suggestionsLV.DoubleClick += suggestionsLV_DoubleClick;
		suggestionsLV.KeyDown += suggestionsLV_KeyDown;
		suggestionsLV.LostFocus += (s, e) => Hide();
		
		methodMC = new MethodControl { Dock = DockStyle.Fill, Visible = false };
		methodMC.SizeChanged += methodMC_SizeChanged;
		
		ClientSize = FSize;
		
		FormBorderStyle = FormBorderStyle.None;
		ShowInTaskbar = false;
		
		Controls.Add(suggestionsLV);
		Controls.Add(methodMC);
	}
	
	#endregion
	
	#region Toggle mode
	
	public void ToggleMode(IEnumerable<MethodInfo> mis)
	{
		SuspendLayout();
		if (mis == null)
		{
			suggestionsLV.Visible = true;
			methodMC.Visible = false;
			
			Size = FSize;
		}
		else
		{
			methodMC.SetMethods(mis.ToList());
			
			methodMC.Visible = true;
			suggestionsLV.Visible = false;
			
			Size = methodMC.Size;
		}
		ResumeLayout();
	}

	void methodMC_SizeChanged(object sender, EventArgs e)
	{
		if (methodMC.Visible)
			Size = methodMC.Size;
	}
	
	#endregion
	
	#region Suggestions
	
	public Suggestion SelectSuggestion()
	{
		Hide();
		return suggestionsLV.SelectedItems.Count > 0 ?
			QueuedSuggestions.FirstOrDefault(s => s.Name == suggestionsLV.SelectedItems[0].Text) :
			null;
	}
	
	public void ClearSuggestions()
	{
		QueuedSuggestions.Clear();
		suggestionsLV.Items.Clear();
	}
	
	public void AddSuggestion(Suggestion suggestion)
	{
		if (QueuedSuggestions.Any(s => s.Name == suggestion.Name))
			return;
		
		QueuedSuggestions.Add(suggestion);
	}
	
	#endregion
	
	#region Update
	
	public void BeginUpdate()
	{
		SuspendLayout();
		suggestionsLV.BeginUpdate();
		
		ClearSuggestions();
	}
	
	public void EndUpdate(string compareTo)
	{
		foreach (var kvp in Sort(QueuedSuggestions, compareTo))
			suggestionsLV.Items.Add(kvp.Name, kvp.Type);
		
		if (suggestionsLV.Items.Count == 0)
			Hide();
		else
			Idx = 0;
		
		ResumeLayout();
		suggestionsLV.EndUpdate();
	}
	
	#endregion
	
	#region Movement
	
	public void MoveUp()
	{
		if (methodMC.Visible)
			methodMC.MoveUp();
		
		else
		{
			int idx = Idx;
			idx--;
			if (idx < 0)
				idx = suggestionsLV.Items.Count - 1;
			
			Idx = idx;
		}
	}
	
	public void MoveDown()
	{
		if (methodMC.Visible)
			methodMC.MoveDown();
		
		else
		{
			int idx = Idx;
			idx++;
			if (idx >= suggestionsLV.Items.Count)
				idx = 0;
			
			Idx = idx;
		}
	}
	
	#endregion
	
	#region Suggestion events
	
	public delegate void RequestDelegate();
	public event RequestDelegate RequestSuggestion;

	void suggestionsLV_DoubleClick(object sender, EventArgs e)
	{
		RequestSuggestion();
	}

	void suggestionsLV_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Enter)
			RequestSuggestion();
	}
	
	#endregion
	
	public IEnumerable<Suggestion> Sort(List<Suggestion> list, string comparedTo)
	{	
		var first = new List<Suggestion>();
		var second = new List<Suggestion>();
		var last = new List<Suggestion>();
		
		foreach (var s in list)
		{
			if (s.Name.StartsWith(comparedTo, StringComparison.InvariantCulture))
				first.Add(s);
			
			else if (s.Name.StartsWith(comparedTo, StringComparison.InvariantCultureIgnoreCase))
				second.Add(s);
			
			else
				last.Add(s);
		}
		
		foreach (var sug in from s in first
			orderby s.Name.Length
			select s)
			yield return sug;
		
		foreach (var sug in from s in second
			orderby s.Name.Length
			select s)
			yield return sug;
		
		foreach (var sug in from s in last
			orderby s.Name.Length
			select s)
			yield return sug;
	}
	
	public bool ContainsMouse
	{
		get
		{
			bool contains = ClientRectangle.Contains(suggestionsLV.PointToClient(Cursor.Position));
			
			if (contains) // so it can loose the focus later, and hide
				suggestionsLV.Focus();
			
			return contains;
		}
	}
}