namespace SCDH.Middlewares;

public class AuditoriaMiddleware
{
    private readonly RequestDelegate _next;

    public AuditoriaMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var horario = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var metodo = context.Request.Method;
        var url = context.Request.Path;

        Console.WriteLine($"[AUDITORIA] {horario} | {metodo} | {url}");

        await _next(context);
    }
}