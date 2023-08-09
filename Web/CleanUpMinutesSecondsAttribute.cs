using Microsoft.AspNetCore.Mvc.Filters;

namespace Web
{
    /* 
        From requirements: "Time-points of the requested/persisted 
            prices should have hour-accuracy (no minutes/seconds allowed)"
        decided to do actionFilter, it's faster and easier but might confuse users 
            if they will use minutes or seconds,
        second option is to return bad request from API if minutes or seconds are not 0,
        third option - more complex API parameters                                   
    */

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CleanUpMinutesSecondsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments)
            {
                if (argument.Value is DateTimeOffset dateTime)
                {
                    var cleanedDateTime = new DateTimeOffset(
                        dateTime.Year,
                        dateTime.Month,
                        dateTime.Day,
                        dateTime.Hour,
                        0,
                        0,
                        dateTime.Offset);
                    context.ActionArguments[argument.Key] = cleanedDateTime;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
