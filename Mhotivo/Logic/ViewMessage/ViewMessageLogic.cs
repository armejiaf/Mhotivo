using System.Web.Mvc;
using Mhotivo.Models;

namespace Mhotivo.Logic.ViewMessage
{
    public class ViewMessageLogic
    {
        public readonly static string MessageIdentifier = "MessageInfo";
        private readonly Controller _controller;

        public ViewMessageLogic(Controller controller)
        {
            _controller = controller;
        }

        public void SetViewMessageIfExist()
        {
            var message = (MessageModel)_controller.TempData[MessageIdentifier];
            if (message == null) return;
            _controller.ViewBag.MessageType = message.Type;
            _controller.ViewBag.MessageTitle = message.Title;
            _controller.ViewBag.MessageContent = message.Content;
        }

        public void SetNewMessage(string title, string content, string type)
        {
            _controller.TempData[MessageIdentifier] = new MessageModel
            {
                Type = type,
                Title = title,
                Content = content
            };
        }
    }
}