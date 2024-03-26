namespace LaptopStore.Web.MiddleWare
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Kiểm tra xem người dùng đã xác thực hay chưa
            if (!context.User.Identity.IsAuthenticated)
            {
                // Nếu chưa xác thực, chuyển hướng đến trang đăng nhập
                context.Response.Redirect("/Auth");
                return;
            }

            // Nếu đã xác thực, tiếp tục xử lý request
            await _next(context);
        }
    }
}
