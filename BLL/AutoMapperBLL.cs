using Microsoft.Extensions.DependencyInjection;

public class AutoMapperBLL
{
    public void ConfigureServices(IServiceCollection services)
    {
        /*var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new ArticleProfile());
            mc.AddProfile(new CommentaryProfile());
            mc.AddProfile(new UserProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
        */
    }
}