using Service.CommunicationStandard.Const;

namespace API.Base.Filters.ActionFilters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var modelState = context.ModelState;

                var response = new ActionResultModel
                {
                    Code = ActionCode.InvalidDataFormat,
                    Data = modelState.Keys
                        .Where(k => modelState[k].Errors.Count > 0)
                        .Select(k => new { propertyName = k, errorMessages = modelState[k].Errors.Select(e => e.ErrorMessage) }),

                    Message = "Invalid data"
                };

                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}
