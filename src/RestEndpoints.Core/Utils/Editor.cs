using System.Linq;
using System.Web.Mvc;
using RestEndpoints.Core.Models;

namespace RestEndpoints.Core.Utils
{
    public static class Editor
    {
        public static string For(ContractDescriptor model)
        {
            return MakeHtmlFormEditorFor(model);
        }

        private static string MakeHtmlFormEditorFor(ContractDescriptor model)
        {
            var descriptors = PropertyDescriptor.GetProperties(model.Type);
            var maxPropertyLength = descriptors.Max(x => x.Name.Length);

            var form = BuildFormTag(model.Name);

            foreach (var property in descriptors)
            {
                form.InnerHtml += MakeHtmlForProperty(property, maxPropertyLength);
            }

            form.InnerHtml += MakeHtmlForSubmitButton();

            foreach (var msg in model.ErrorMessages)
            {
                form.InnerHtml += MakeHtmlForError(msg);
            }

            return form.ToString();
        }

        private static string MakeHtmlForError(string msg)
        {
            TagBuilder error = new TagBuilder("label");
            error.MergeAttribute("style", "color:red");
            error.SetInnerText(msg);
            return error + BuildBrTag();
        }

        private static string MakeHtmlForProperty(PropertyDescriptor property, int maxPropertyLength)
        {
            var label = BuildLabelTag(property.Name, maxPropertyLength*8);
            var input = BuildInputTag(property.Name);
            return label + input.ToString(TagRenderMode.SelfClosing) + BuildBrTag();
        }

        private static string MakeHtmlForSubmitButton()
        {
            TagBuilder submit = new TagBuilder("input");
            submit.MergeAttribute("type", "submit");
            submit.MergeAttribute("value", "Send");
            submit.MergeAttribute("name", "submit");
            return submit.ToString(TagRenderMode.SelfClosing) + BuildBrTag();
        }

        private static TagBuilder BuildFormTag(string name)
        {
            TagBuilder form = new TagBuilder("form");
            form.MergeAttribute("action", name);
            form.MergeAttribute("method", "POST");
            form.MergeAttribute("title", name);
            form.GenerateId(name);
            return form;
        }

        private static TagBuilder BuildInputTag(string name)
        {
            TagBuilder input = new TagBuilder("input");
            input.MergeAttribute("type", "text");
            input.MergeAttribute("name", name);
            return input;
        }

        private static TagBuilder BuildLabelTag(string name, int width)
        {
            TagBuilder label = new TagBuilder("label");
            label.MergeAttribute("for", name);
            label.MergeAttribute("style", "width:" + width + ";display:inline-block");
            label.SetInnerText(name);
            return label;
        }

        private static string BuildBrTag()
        {
            return new TagBuilder("br").ToString(TagRenderMode.SelfClosing);
        }
    }
}