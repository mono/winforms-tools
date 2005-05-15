// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;

namespace MWFResourceEditor
{
	public interface IResourceRenderer
	{
		Bitmap RenderContent
		{ get; }
	}
}

