using NLog;
using System.Linq;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

try
{
while (true){
    Console.WriteLine("Choose and option: ");
    Console.WriteLine("1. Display all Blogs");
    Console.WriteLine("2. Add a Blog");
    Console.WriteLine("3. Create Post ");
    Console.WriteLine("4. Exit");

    var option = Console.ReadLine();

    var db = new BloggingContext();

    switch (option)
    {
        case "1":
        var query = db.Blogs.OrderBy(b => b.Name);

        if (!query.Any())
        {
            Console.WriteLine("No blogs found in the database.");
            break;
        }

        Console.WriteLine("All blogs in the database: ");
        foreach (var item in query)
        {
            Console.WriteLine(item.Name);
        }
        break;
        

    case "2":
        Console.Write("Enter a name for a new Blog: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Blog name cannot be empty.");
            break;
        }

        var blog = new Blog { Name = name };

        db.AddBlog(blog);
        logger.Info("Blog added - {name}", name);
        break;
        

    case "3":
        try
    {
        // Display all blogs
        var blogs = db.Blogs.OrderBy(b => b.Name).ToList();
        if (!blogs.Any())
        {
            Console.WriteLine("No blogs found in the database.");
            break;
        }

        Console.WriteLine("Select a blog to post to:");
        for (int i = 0; i < blogs.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {blogs[i].Name}");
        }

        // Get blog selection from user
        int selectedBlogIndex = Convert.ToInt32(Console.ReadLine()) - 1;
        if (selectedBlogIndex < 0 || selectedBlogIndex >= blogs.Count)
        {
            Console.WriteLine("Invalid selection. Please select a valid blog.");
            break;
        }

        var selectedBlog = blogs[selectedBlogIndex];

        // Get post details from user
        Console.Write("Enter post title: ");
        var title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Post title cannot be empty.");
            break;
        }

        Console.Write("Enter post content: ");
        var content = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(content))
        {
            Console.WriteLine("Post content cannot be empty.");
            break;
        }

        // Create and save post
        var post = new Post { Title = title, Content = content, BlogId = selectedBlog.BlogId };
        db.Posts.Add(post);
        db.SaveChanges();

        Console.WriteLine("Post added successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }

        break;

    case "4":
        logger.Info("Program ended");
        return;

    default:
        Console.WriteLine("Invalid choice. Please choose a valid option.");
        break;
    }
}
}
catch (Exception ex)
{
    logger.Error(ex.Message);
}

