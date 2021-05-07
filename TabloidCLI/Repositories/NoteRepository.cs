using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;


namespace TabloidCLI
{
    public class NoteRepository : DatabaseConnector, IRepository<Note>
    {
        public NoteRepository(string connectionString) : base(connectionString) { }

        public List<Note> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT n.Id as noteId, n.Title, n.Content, n.CreateDateTime, p.Id as postId, p.Title, p.URL, p.PublishDateTime, p.AuthorId, p.BlogId FROM Note n
                       Left JOIN Post p on n.PostId = p.Id";

                    List<Note> notes = new List<Note>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Note note = new Note()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("noteId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            Post = new Post()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("postId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Url = reader.GetString(reader.GetOrdinal("URL")),
                                PublishDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),

                                Author = new Author()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Bio = reader.GetString(reader.GetOrdinal("Bio")),
                                },
                                Blog = new Blog()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Url = reader.GetString(reader.GetOrdinal("URL")),
                                }
                            }
                        };

                        notes.Add(note);

                    };
                        
                    

                    reader.Close();

                    return notes;
                }
            }
        }

        public Note Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Note note)
        {
            throw new NotImplementedException();
        }

        public void Update(Note note)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM NOte WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        
    }
}
