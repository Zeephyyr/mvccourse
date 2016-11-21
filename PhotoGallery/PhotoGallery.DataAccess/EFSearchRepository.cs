using System;
using System.Collections.Generic;
using PhotoGallery.AppCommonCore.Contracts.DataAccess;
using PhotoGallery.AppCommonCore.Entities;
using System.Data.Entity;
using System.Data.SqlClient;

namespace PhotoGallery.DataAccess
{
    public class EFSearchRepository : ISearchRepository
    {
        private DbContext _context;

        public EFSearchRepository(DbContext context)
        {
            _context = context;
        }

        public List<AlbumShort> ExtendedAlbumSearch(ExtendedAlbumSearchRequest data)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@AlbumName", ValueOrNull(data.AlbumName)),
                new SqlParameter("@Description", ValueOrNull(data.Description)),
                new SqlParameter("@UniqueUserName", ValueOrNull(data.UniqueUserName))
            };

            return ExecuteProc<AlbumShort>("ExtendedAlbumSearch", parameters.ToArray());
        }

        public List<PhotoShort> ExtendedPhotoSearch(ExtendedPhotoSearchRequest data)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@PhotoName", ValueOrNull(data.PhotoName)),
                new SqlParameter("@UniqueUserName", ValueOrNull(data.UniqueUserName)),
                new SqlParameter("@Description", ValueOrNull(data.Description)),
                new SqlParameter("@CameraModel", ValueOrNull(data.CameraModel)),
                new SqlParameter("@Flash", ValueOrNull(data.Flash)),
                new SqlParameter("@ISO", ValueOrNull(data.ISO)),
                new SqlParameter("@LensFocus", ValueOrNull(data.LensFocus)),
                new SqlParameter("@ShutterSpeed", ValueOrNull(data.ShutterSpeed)),
                new SqlParameter("@Place", ValueOrNull(data.Place))
            };
            if (data.Diaphragm == 0)
            {
                parameters.Add(new SqlParameter("@Diaphragm", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@Diaphragm", data.Diaphragm));
            }

            return ExecuteProc<PhotoShort>("ExtendedPhotoSearch", parameters.ToArray());
        }

        public List<UserShort> ExtendedUserSearch(ExtendedUserSearchRequest data)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Name", ValueOrNull(data.Name)),
                new SqlParameter("@UniqueUserName", ValueOrNull(data.UniqueUserName)),
                new SqlParameter("@Description", ValueOrNull(data.Description))
            };

            return ExecuteProc<UserShort>("ExtendedUserSearch", parameters.ToArray());
        }

        public SearchResult Search(string keyWord)
        {
            SearchResult result = new SearchResult
            {
                Albums = ExecuteProc<AlbumShort>("BasicAlbumSearch", new SqlParameter("@KeyWord", keyWord)),
                Photos = ExecuteProc<PhotoShort>("BasicPhotoSearch", new SqlParameter("@KeyWord", keyWord)),
                Users = ExecuteProc<UserShort>("BasicUserSearch", new SqlParameter("@KeyWord", keyWord))
            };

            return result;
        }

        private List<T> ExecuteProc<T>(string command,params SqlParameter[] parameters)
        {
            List<T> searchResult = new List<T>();
            command = AppendParamsToCmd(command, parameters);

            var result = _context.Database.SqlQuery<T>(command, parameters);
            foreach(var el in result)
            {
                searchResult.Add(el);
            }
            return searchResult;
        }

        private string AppendParamsToCmd(string cmd, params SqlParameter[] parameters)
        {
            for(int i=0;i<parameters.Length;i++)
            {
                if (i != 0)
                    cmd += ",";
                cmd += string.Format(" {0}", parameters[i].ParameterName);
            }
            return cmd;
        }

        private object ValueOrNull(bool? value)
        {
            return (object)value ?? DBNull.Value;
        }

        private object ValueOrNull(int? value)
        {
            var val = (object)value ?? DBNull.Value;
            return val;
        }

        private object ValueOrNull(object value)
        {
            return value ?? DBNull.Value;
        }
    }
}
