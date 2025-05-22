namespace UserContacts.Server.Middlewares;

public class TimeCheckMiddleware
{
    private readonly RequestDelegate _next;

    public TimeCheckMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var currentHour = DateTime.Now.Hour;

        if (currentHour <= 9 && currentHour >= 18|| context.Request.Path.ToString().Contains("get"))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(new
            {
                message = "The API is closed !"
            });

            return;
        }

        await _next(context);
    }
}
