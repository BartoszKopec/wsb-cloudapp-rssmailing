using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HtmlService
{
	public class HtmlWriter
	{
		private record Node(string Name, int Depth, string Content);

		public static string WriteHtml(HtmlMarkup mainHtml)
		{
			if (mainHtml is null) return "";

			StringBuilder builder = new();
			builder.Append(XmlStart(mainHtml.Markup));

			if (mainHtml.HasTextContent)
			{
				builder.Append(mainHtml.TextContent);
			}
			else
			{
				List<Node> nodes = new();
				foreach (var markup in mainHtml.Children)
				{
					var stack = new Stack<(HtmlMarkup, int depth)>();
					stack.Push((markup, 1));
					while (stack.Count > 0)
					{
						(HtmlMarkup current, int depth) = stack.Pop();
						if (current.HasTextContent)
						{
							nodes.Add(new Node(current.Markup, depth, current.TextContent));
						}
						else if (current.Children.Count > 0)
						{
							nodes.Add(new Node(current.Markup, depth, ""));
							var childrenRightToLeft = current.Children.AsEnumerable().Reverse();
							foreach (var child in childrenRightToLeft)
							{
								int childDepth = depth + 1;
								stack.Push((child, childDepth));
							}
						}
					}
				}

				for (int i = 0; i < nodes.Count; i++)
				{
					Node node = nodes[i];
					builder.Append(XmlStart(node.Name));
					if (!string.IsNullOrWhiteSpace(node.Content))
					{
						builder.Append(node.Content);
					}
					if (i + 1 == nodes.Count || nodes[i + 1].Depth <= node.Depth)
					{
						builder.Append(XmlEnd(node.Name));
					}
					if(i + 1 < nodes.Count && nodes[i + 1].Depth < node.Depth)
					{
						for (int j = i; j >= 0; j--)
						{
							Node previousNode = nodes[j];
							if (previousNode.Depth < node.Depth)
							{
								builder.Append(XmlEnd(previousNode.Name));
								break;
							}
						}
					}
				}
				int currentDepth = nodes[^1].Depth;
				for (int j = nodes.Count-2; j >= 0; j--)
				{
					Node previousNode = nodes[j];
					if (previousNode.Depth < currentDepth)
					{
						currentDepth--;
						builder.Append(XmlEnd(previousNode.Name));
					}
				}

			}
			builder.Append(XmlEnd(mainHtml.Markup));
			return builder.ToString();
		}

		private static string XmlStart(string markup) => $"<{markup}>";
		private static string XmlEnd(string markup) => $"</{markup}>";
	}
}
