// See https://aka.ms/new-console-template for more information

using EFCoreExample;
using EFCoreExample.Data;
using Microsoft.EntityFrameworkCore;

using (var context = new BloggingContext())
{
	FirstSteps(context);
}

void FirstSteps(BloggingContext context)
{
	Console.WriteLine("inserting a new blog.");
	context.Blogs.Add(new EFCoreExample.Blog { url = "http://blogs.com" });
	context.SaveChanges();

	//Read
	Console.WriteLine("Querying for a blog");
	var blog = context.Blogs.First();
	Console.WriteLine(blog.url);


	blog = context.Blogs.Include(c => c.publicaciones).Where(b => b.Id.Equals(1)).FirstOrDefault();

	foreach (var post in blog.publicaciones)
	{
		Console.WriteLine(post.Title);

	}
	//update
	//Console.WriteLine("Updateing the blog and adding a post");
	//blog.url = "https://facebook.com";
	//blog.publicaciones.Add(new Post { Title = "Hello World", Content = "My first EF Core app." });
	//context.Posts.Add(new Post { Title = "Hola Mundo", Content = "Mi primera app con EF core", BlogId = 2 });
	//context.SaveChanges();
}