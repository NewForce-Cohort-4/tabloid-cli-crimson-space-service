using System;
using System.Collections.Generic;
using System.Text;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class PostManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private AuthorManager _authorManager;
        private BlogManager _blogManager;
        private string _connectionString;

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
            Console.WriteLine();
            Console.WriteLine("Post Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Post Details");
            Console.WriteLine(" 3) Add Post");
            Console.WriteLine(" 4) Edit Post");
            Console.WriteLine(" 5) Remove Post");
            Console.WriteLine(" 0) Go Back");

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

        private void List()
        {
            List<Post> posts = _postRepository.GetAll();
            foreach (Post post in posts)
            {
                Console.WriteLine($"{post.Title} - {post.Url}");
            }
        }

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

            post.Author = _authorManager.Choose("Author: ");

            //post.Blog = new Blog()
            //{
            //    Id = 1,
            //    Title = "Google",
            //    Url = "www.google.com",
            //    Tags = new List<Tag>()
            //};


            post.Blog = _blogManager.Choose("Blog: ");

            _postRepository.Insert(post);
        }

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
                //Blog newBlog = new Blog()
                //                {
                //                    Id = 1,
                //                    Title = "Google",
                //                    Url = "www.google.com",
                //                    Tags = new List<Tag>()
                //                };
                postToEdit.Blog = newBlog;
                changes++;
            }
            if (changes > 0)
            {
                _postRepository.Update(postToEdit);
            }
        }

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


