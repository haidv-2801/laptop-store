"# laptop-store" 

# Vào thư mục LaptopStore.Data mở terminal và chạy lệnh sau để lấy lại database nếu sửa đổi

## Lưu ý thay connection string

### Không cần xóa lại dbcontext với model nữa )) đã thêm --force

###$ 
dotnet ef dbcontext scaffold "Data Source=DVHAI\SQLEXPRESS;Initial Catalog=laptop_store;User ID=sa;Password=123456;Integrated Security=False;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" Microsoft.EntityFrameworkCore.SqlServer -o ../LaptopStore.Data/Models --context ApplicationDbContext --force
