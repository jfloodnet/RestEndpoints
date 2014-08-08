using System.Linq;
using System.Web.Mvc;

namespace RestEndpoints.Core.Models
{
    public static class Editor
    {
        public static string For(ContractDescriptor model)
        {
            TagBuilder form= new TagBuilder("form");
            form.MergeAttribute("action", model.Name);
            form.MergeAttribute("method", "POST");
            form.MergeAttribute("title", model.Name);
            form.GenerateId(model.Name);

            var br = new TagBuilder(
                "br").ToString(TagRenderMode.SelfClosing);

            var helpers = PropertyHelper.GetProperties(model.Type);
            var maxPropertyLength = helpers.Max(x => x.Name.Length);

            foreach (var helper in helpers)
            {
                TagBuilder label = new TagBuilder("label");
                TagBuilder input = new TagBuilder("input");

                label.MergeAttribute("for", helper.Name);
                label.MergeAttribute("style", "width:" + maxPropertyLength * 8 + ";display:inline-block");
                label.SetInnerText(helper.Name);
                input.MergeAttribute("type", "text");
                input.MergeAttribute("name", helper.Name);

                form.InnerHtml += (
                    label + input.ToString(TagRenderMode.SelfClosing) + br);
            }

            TagBuilder submit = new TagBuilder("input");
            submit.MergeAttribute("type", "submit");
            submit.MergeAttribute("value", "Send");
            submit.MergeAttribute("name", "submit");

            form.InnerHtml += submit.ToString(TagRenderMode.SelfClosing) + br;

     
            foreach (var msg in model.ErrorMessages)
            {
                TagBuilder error = new TagBuilder("label");
                error.MergeAttribute("style", "color:red");
                error.SetInnerText(msg);
                form.InnerHtml += error + br;
            }
            

            return form.ToString();

            //                <form action="@Model.Name" method="POST" title="@Model.Name">
            //    <input name="CommandName" type="hidden" value="@Model.Name" /><br />
            //    @foreach (PropertyInfo pi in @Model.Type.GetProperties())
            //    {
            //        <label style="width:150px;display:inline-block" for="@pi.Name">@pi.Name</label><input name="@pi.Name" type="text" value="" /><br/>
            //    }
            //    <input name="submit" type="submit" value="Send" /><label style="color:red">@Model.ResponseMessage</label>
            //</form>
        }
    }
}