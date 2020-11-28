using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GithubSearch.Common.Models
{
    public class BaseMongoModel
    {
        /// <summary>
        /// 基础ID
        /// </summary>
        public ObjectId _id { get; set; }
    }
}

