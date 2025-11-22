namespace Application.Seeds;

public class DefaultDepartments
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // if (!departments.Any(d => d.Name == Roles.Admin))
        // await roleManger.CreateAsync(new IdentityRole<int>(Roles.Admin));

        var departments = new List<string>()
        {
            "وزارة الكهرباء",
            "وزارة المياه",
            "وزارة الاتصالات",
            "وزارة النقل",
            "وزارة الصحة",
            "وزارة التربية والتعليم",
            "وزارة الداخلية",
            "وزارة البيئة",
            "وزارة الأشغال العامة والإسكان",
            "وزارة الشؤون الاجتماعية",
            "وزارة الزراعة",
            "وزارة المالية",
            "وزارة السياحة",
            "بلدية دمشق",
            "شركة الكهرباء العامة",
            "شركة الاتصالات السورية",
            "وزارة العدل",
            "وزارة العمل",
            "وزارة الاقتصاد والتجارة",
            "وزارة الثقافة"
        };

        foreach (var department in departments)
        {
            if (context.Departments.Any(dep => dep.Name == department)) continue;
            
            var entity = new Department { Name = department };
            await context.Departments.AddAsync(entity);
        }

        await context.SaveChangesAsync();
    }
}