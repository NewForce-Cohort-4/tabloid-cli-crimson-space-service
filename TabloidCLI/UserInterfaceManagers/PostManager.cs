using System;
using System.Collections.Generic;
using System.Text;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class PostManager : IUserInterfaceManager
    {
        // Properties of the PostManager Class
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private AuthorManager _authorManager;
        private BlogManager _blogManager;
        private string _connectionString;

        // Define a PostManager Constructor method to build an instance of the PostManager Type
        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _authorManager = new AuthorManager(parentUI, connectionString);
            _blogManager = new BlogManager(parentUI, connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            // Display a Post Manager Menu to the Console
            Console.WriteLine();
            Console.WriteLine("Post Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Post Details");
            Console.WriteLine(" 3) Add Post");
            Console.WriteLine(" 4) Edit Post");
            Console.WriteLine(" 5) Remove Post");
            Console.WriteLine(" 0) Go Back");

            // Use a switch to manage user selection from the above Post Manager Menu
            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Console.WriteLine();
                    Post post = Choose();
                    if (post == null)
                    {
                        return this;
                    }
                    else
                    {
                        return new PostDetailManager(this, _connectionString, post.Id);
                    }
                case "3":
                    Add();
                    return this;
                case "4":
                    Edit();
                    return this;
                case "5":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        // Define a List method which prints each Post Object in the database to the Console
        private void List()
        {
            List<Post> posts = _postRepository.GetAll();
            foreach (Post post in posts)
            {
                Console.WriteLine($"{post.Title} - {post.Url}");
            }
        }

        // Define a Choose method which is used to engage the user in selecting a Post from the List of Posts in the database.
        private Post Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Post: ";
            }

            Console.WriteLine(prompt);

            List<Post> posts = _postRepository.GetAll();

            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return posts[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        // Define an Add method to engage the user in adding a Post Object to the database
        private void Add()
        {
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.Write("Title: ");
            post.Title= Console.ReadLine();

            Console.Write("Url: ");
            post.Url = Console.ReadLine();

            Console.Write("Published Date (e.g. 1/12/2014): ");
            post.PublishDateTime = DateTime.Parse(Console.ReadLine());

            // Use the Choose() method that is predefined in the AuthorManager
            post.Author = _authorManager.Choose("Author: ");

            // Use the Choose() method that is predefined in the BlogManager
            post.Blog = _blogManager.Choose("Blog: ");

            _postRepository.Insert(post);
        }

        // Define an Edit method to engage the user in editing a Post Object in the database
        private void Edit()
        {
            Console.WriteLine();
            Post postToEdit = Choose("Which post would you like to edit?");
            int changes = 0;
            if (postToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New Title (blank to leave unchanged: ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                postToEdit.Title = title;
                changes++;
            }

            Console.Write("New URL (blank to leave unchanged: ");
            string url = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(url))
            {
                postToEdit.Url = url;
                changes++;
            }

            Console.Write("New Publish Date (blank to leave unchanged: ");
            string publishDate = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(publishDate))
            {
                postToEdit.PublishDateTime = DateTime.Parse(publishDate);
                changes++;
            }

            Console.Write("Select New Author? (y/n): ");
            string answerAuthor = Console.ReadLine();
            if (answerAuthor == "y")
            {
                Author newAuthor = _authorManager.Choose("Author: ");
                postToEdit.Author = newAuthor;
                changes++;
            }

            Console.Write("Select New Blog? (y/n): ");
            string answerBlog = Console.ReadLine();
            if (answerBlog == "y")
            {
                Blog newBlog = _blogManager.Choose("Blog: ");

                postToEdit.Blog = newBlog;
                changes++;
            }

            // Check to see if any changes have been made to the Object. If changes have been made, send to the database. if not, do nothing 
            // (don't open communication with the database if not necessary)
            if (changes > 0)
            {
                _postRepository.Update(postToEdit);
            }
        }

        // Define a Remove method to allow the user to select a Post Object to be removed from the database
        private void Remove()
        {
            Post postToDelete = Choose("Which post would you like to remove?");
            if (postToDelete != null)
            {
                _postRepository.Delete(postToDelete.Id);
            }
        }
    }
}


