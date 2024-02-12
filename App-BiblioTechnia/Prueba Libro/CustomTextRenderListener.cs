using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Collections.Generic;
using System.Text;

namespace Prueba_Libro
{
    public class CustomTextRenderListener : IEventListener
    {
        private readonly StringBuilder htmlBuilder;

        public CustomTextRenderListener()
        {
            htmlBuilder = new StringBuilder();
        }

        public void EventOccurred(IEventData data, EventType type)
        {
            if (type == EventType.RENDER_TEXT)
            {
                TextRenderInfo renderInfo = (TextRenderInfo)data;

                string text = renderInfo.GetText();
                float x = renderInfo.GetBaseline().GetStartPoint().Get(0);
                float y = renderInfo.GetBaseline().GetStartPoint().Get(1);

                htmlBuilder.AppendLine($"<span style='position:absolute; left:{x}px; top:{y}px;'>{text}</span>");
            }
        }

        public string GetGeneratedHtml()
        {
            return htmlBuilder.ToString();
        }

        public ICollection<EventType> GetSupportedEvents()
        {
            return new List<EventType> { EventType.RENDER_TEXT };
        }
    }
}