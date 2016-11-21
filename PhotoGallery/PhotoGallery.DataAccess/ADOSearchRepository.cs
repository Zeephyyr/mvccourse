using PhotoGallery.AppCommonCore.Contracts.DataAccess;
using PhotoGallery.Common;
using PhotoGallery.AppCommonCore.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PhotoGallery.DataAccess
{
    public class ADOSearchRepository : ISearchRepository
    {
        private string _connectionString;

        public ADOSearchRepository()
        {
            _connectionString = ConfigurationElements.ConnectionString;
        }

        public List<AlbumShort> ExtendedAlbumSearch(ExtendedAlbumSearchRequest data)
        {
            List<AlbumShort> result = new List<AlbumShort>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                result = ExecuteExtendedSearch<AlbumShort, ExtendedAlbumSearchRequest>(connection, "ExtendedAlbumSearch", data);

                connection.Close();
            }

            return result;
        }

        public List<PhotoShort> ExtendedPhotoSearch(ExtendedPhotoSearchRequest data)
        {
            List<PhotoShort> result = new List<PhotoShort>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                result = ExecuteExtendedSearch<PhotoShort, ExtendedPhotoSearchRequest>(connection, "ExtendedPhotoSearch", data);

                connection.Close();
            }

            return result;
        }

        public List<UserShort> ExtendedUserSearch(ExtendedUserSearchRequest data)
        {
            List<UserShort> result = new List<UserShort>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                result = ExecuteExtendedSearch<UserShort, ExtendedUserSearchRequest>(connection, "ExtendedUserSearch", data);

                connection.Close();
            }

            return result;
        }

        public SearchResult Search(string keyWord)
        {
            SearchResult result = new SearchResult();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                result.Albums = ExecuteBasicSearch<AlbumShort>(connection, "BasicAlbumSearch", keyWord) as List<AlbumShort> ?? new List<AlbumShort>();
                result.Photos = ExecuteBasicSearch<PhotoShort>(connection, "BasicPhotoSearch", keyWord) as List<PhotoShort> ?? new List<PhotoShort>();
                result.Users = ExecuteBasicSearch<UserShort>(connection, "BasicUserSearch", keyWord) as List<UserShort> ?? new List<UserShort>();

                connection.Close();
            }

            return result;
        }

        private List<TRes> ExecuteExtendedSearch<TRes,TReq>(SqlConnection conn, string procName, TReq requestData) where TRes:class
        {
            using (SqlCommand cmd = new SqlCommand(procName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                Type requestType = typeof(TReq);
                
                if(requestType==typeof(ExtendedPhotoSearchRequest))
                {
                    ExtendedPhotoSearchRequest castedData = requestData as ExtendedPhotoSearchRequest;
                    List<PhotoShort> res = new List<PhotoShort>();

                    cmd.Parameters.Add(new SqlParameter("@PhotoName", ValueOrNull(castedData.PhotoName)));
                    cmd.Parameters.Add(new SqlParameter("@UniqueUserName", ValueOrNull(castedData.UniqueUserName)));
                    cmd.Parameters.Add(new SqlParameter("@Description", ValueOrNull(castedData.Description)));
                    cmd.Parameters.Add(new SqlParameter("@CameraModel", ValueOrNull(castedData.CameraModel)));
                    cmd.Parameters.Add(new SqlParameter("@Flash", ValueOrNull(castedData.Flash)));
                    cmd.Parameters.Add(new SqlParameter("@ISO", ValueOrNull(castedData.ISO)));
                    cmd.Parameters.Add(new SqlParameter("@LensFocus", ValueOrNull(castedData.LensFocus)));
                    cmd.Parameters.Add(new SqlParameter("@ShutterSpeed", ValueOrNull(castedData.ShutterSpeed)));
                    cmd.Parameters.Add(new SqlParameter("@Place", ValueOrNull(castedData.Place)));
                    if(castedData.Diaphragm==0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@Diaphragm", DBNull.Value));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@Diaphragm", castedData.Diaphragm));
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add(new PhotoShort
                            {
                                PhotoName = reader.GetString(0),
                                UniqueUserName = reader.GetString(1),
                                ImageData = (byte[])reader.GetValue(2),
                                ImageMimeType = reader.GetString(3)
                            });
                        }
                        return res as List<TRes>;
                    }
                }
                else if (requestType == typeof(ExtendedAlbumSearchRequest))
                {
                    ExtendedAlbumSearchRequest castedData = requestData as ExtendedAlbumSearchRequest;
                    List<AlbumShort> res = new List<AlbumShort>();

                    cmd.Parameters.Add(new SqlParameter("@AlbumName", ValueOrNull(castedData.AlbumName)));
                    cmd.Parameters.Add(new SqlParameter("@Description", ValueOrNull(castedData.Description)));
                    cmd.Parameters.Add(new SqlParameter("@UniqueUserName", ValueOrNull(castedData.UniqueUserName)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add(new AlbumShort
                            {
                                AlbumName = reader.GetString(0),
                                UniqueUserName = reader.GetString(1),
                                ImageData = (byte[])reader.GetValue(2),
                                ImageMimeType = reader.GetString(3)
                            });
                        }
                        return res as List<TRes>;
                    }
                }
                else if (requestType == typeof(ExtendedUserSearchRequest))
                {
                    ExtendedUserSearchRequest castedData = requestData as ExtendedUserSearchRequest;
                    List<UserShort> res = new List<UserShort>();

                    cmd.Parameters.Add(new SqlParameter("@Name", ValueOrNull(castedData.Name)));
                    cmd.Parameters.Add(new SqlParameter("@UniqueUserName", ValueOrNull(castedData.UniqueUserName)));
                    cmd.Parameters.Add(new SqlParameter("@Description", ValueOrNull(castedData.Description)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add(new UserShort
                            {
                                Name = reader.GetString(0),
                                UniqueUserName = reader.GetString(1)
                            });
                        }
                        return res as List<TRes>;
                    }
                }
            }
            return null as List<TRes>;
        }

        private object ValueOrNull(bool? value)
        {
            return (object)value ?? DBNull.Value;
        }

        private object ValueOrNull(int? value)
        {
            var val= (object)value ?? DBNull.Value;
            return val;
        }

        private object ValueOrNull(object value)
        {
            return value ?? DBNull.Value;
        }

        private IEnumerable<T> ExecuteBasicSearch<T>(SqlConnection conn,string procName,string keyWord)
        {
            using (SqlCommand cmd = new SqlCommand(procName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@KeyWord", keyWord));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Type expectedType = typeof(T);

                    if (expectedType == typeof(AlbumShort))
                    {
                        var result = new List<AlbumShort>();
                        while (reader.Read())
                        {
                            result.Add(new AlbumShort
                            {
                                AlbumName = reader.GetString(0),
                                UniqueUserName = reader.GetString(1),
                                ImageData = (byte[])reader.GetValue(2),
                                ImageMimeType=reader.GetString(3)
                            });
                        }
                        return result as IEnumerable<T>;
                    }
                    else if (expectedType == typeof(PhotoShort))
                    {
                        var result = new List<PhotoShort>();
                        while (reader.Read())
                        {
                            result.Add(new PhotoShort
                            {
                                PhotoName = reader.GetString(0),
                                UniqueUserName = reader.GetString(1),
                                ImageData = (byte[])reader.GetValue(2),
                                ImageMimeType = reader.GetString(3)
                            });
                        }
                        return result as IEnumerable<T>;
                    }
                    else if (expectedType == typeof(UserShort))
                    {
                        var result = new List<UserShort>();
                        while (reader.Read())
                        {
                            result.Add(new UserShort
                            {
                                Name = reader.GetString(0),
                                UniqueUserName = reader.GetString(1)
                            });
                        }
                        return result as IEnumerable<T>;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
