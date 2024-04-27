namespace MahdyASP.NETCore.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static int _counter = 0;
        private static DateTime _lastrequestDate = DateTime.Now;

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            _counter++;

            if (DateTime.Now.Subtract(_lastrequestDate).Seconds > 10)
            {
                _counter = 1;
                _lastrequestDate = DateTime.Now;

                await _next(context);
            }
            else 
            {
                _lastrequestDate = DateTime.Now;

                if (_counter > 5)
                {                    
                    await context.Response.WriteAsync("Rate limit exceeded");                    
                }
                else 
                {
                    _counter++;
                    await _next(context);
                }
            }            
        }
    }
}
