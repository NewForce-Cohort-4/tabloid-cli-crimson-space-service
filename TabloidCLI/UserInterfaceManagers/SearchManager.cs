using System;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    internal class SearchManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private TagRepository _tagRepository;

        public SearchManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _tagRepository = new TagRepository(connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Search Menu");
            Console.WriteLine(" 1) Search Authors");
            Console.WriteLine(" 2) Search Blogs");
            Console.WriteLine(" 3) Search Posts");
            Console.WriteLine(" 4) Search All");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string tagName;
            string choice = Console.ReadLine();
            bool resultsFound = true;
            switch (choice)
            {
                case "1":
                    Console.Write("Tag > ");
                    tagName = Console.ReadLine();
                    resultsFound = SearchAuthors(tagName);
                    if (resultsFound == false)
                    {
                        Console.WriteLine($"No results for {tagName}");
                    }
                    return this;
                case "2":
                    Console.Write("Tag > ");
                    tagName = Console.ReadLine();
                    resultsFound = SearchBlogs(tagName);
                    if (resultsFound == false)
                    {
                        Console.WriteLine($"No results for {tagName}");
                    }
                    return this;
                case "3":
                    Console.Write("Tag > ");
                    tagName = Console.ReadLine();
                    resultsFound = SearchPosts(tagName);
                    if (resultsFound == false)
                    {
                        Console.WriteLine($"No results for {tagName}");
                    }
                    return this;
                case "4":
                    Console.Write("Tag > ");
                    tagName = Console.ReadLine();
                    SearchAll(tagName);
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private bool SearchAuthors(string tagName)
        {
            SearchResults<Author> results = _tagRepository.SearchAuthors(tagName);

            if (results.NoResultsFound)
            {
                
                return false;
            }
            else
            {
                Console.WriteLine("Author Search Results: ");
                results.Display();
                return true;
            }
        }

        private bool SearchBlogs(string tagName)
        {
            SearchResults<Blog> results = _tagRepository.SearchBlogs(tagName);

            if (results.NoResultsFound)
            {
                Console.WriteLine($"No results for {tagName}");
                return false;
            }
            else
            {
                Console.WriteLine("Blog Search Results: ");
                results.Display();
                return true;
            }
        }

        private bool SearchPosts(string tagName)
        {
            SearchResults<Post> results = _tagRepository.SearchPosts(tagName);

            if (results.NoResultsFound)
            {
                Console.WriteLine($"No results for {tagName}");
                return false;
            }
            else
            {
                Console.WriteLine("Post Search Results: ");
                results.Display();
                return true;
            }
        }

        private void SearchAll(string tagName)
        {
            
            bool resultsFoundAuthors = SearchAuthors(tagName);
            
            bool resultsFoundBlogs = SearchBlogs(tagName);
            
            bool resultsFoundPosts = SearchPosts(tagName);
            if (!resultsFoundAuthors && !resultsFoundBlogs && !resultsFoundPosts)
            {
                Console.WriteLine($"No results found for {tagName}.");
            }
        }
    }
}