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
    Console.WriteLine("1. Display all blogs");
    Console.WriteLine("2. Add a blog");
    Console.WriteLine("3. Exit");

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

        var blog = new Blog { Name = name };

        db.AddBlog(blog);
        logger.Info("Blog added - {name}", name);
        break;
        

    case "3":
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

