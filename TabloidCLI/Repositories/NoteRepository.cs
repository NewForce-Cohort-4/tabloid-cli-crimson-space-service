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
                    cmd.CommandText = @"SELECT n.Id as noteId, 
                                               n.Title as noteTitle, 
                                               n.Content, 
                                               n.CreateDateTime, 
                                               p.Id as postId, 
                                               p.Title, 
                                               p.URL as postUrl, 
                                               p.PublishDateTime, 
                                               p.AuthorId, 
                                               p.BlogId,
                                               a.Id as authorId,
                                               a.FirstName,
                                               a.LastName,
                                               a.Bio,
                                               b.Id as blogId,
                                               b.Title AS blogTitle,
                                               b.URL AS BlogUrl
                                          FROM Note n
                                     Left JOIN Post p on n.PostId = p.Id
                                     Left JOIN Author a on p.AuthorId = a.Id
                                     Left JOIN Blog b on p.BlogId = b.Id";

                    List<Note> notes = new List<Note>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Note note = new Note()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("noteId")),
                            Title = reader.GetString(reader.GetOrdinal("noteTitle")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            Post = new Post()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("postId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Url = reader.GetString(reader.GetOrdinal("postUrl")),
                                PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),

                                Author = new Author()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("authorId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Bio = reader.GetString(reader.GetOrdinal("Bio")),
                                },
                                Blog = new Blog()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("blogId")),
                                    Title = reader.GetString(reader.GetOrdinal("blogTitle")),
                                    Url = reader.GetString(reader.GetOrdinal("blogUrl")),
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
