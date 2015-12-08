using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Manage.Service.MongoDb;
using Manage.Service.MongoDb.SQLAnalyzer;
using System.Diagnostics;

namespace Service.Test
{
    [TestClass]
    public class UnitTest1
    {
        private void Translate(string sql)
        {
            var str = TSQLTranslator.TranslateQuery(sql);
            Debug.WriteLine(str);
        }

        [TestMethod]
        public void sql_query_full_test()
        {
            var sql = @"select  a,c,dd FROM t1 where a like '%ss%' and b=1 or c!='3' or d in('1',2,'A') or e not like '%s' 
and f>5 and g<='x' and h>100 or i>=200 and j between 1 and 100 or k not in(3,4) and  L like 'vvv%' 
Order by Id desc,ss asc offset 20 ROWS
FETCH next 10 ROWS ONLY";
            Translate(sql);
        }

        [TestMethod]
        public void sql_all_column_test()
        {
            var sql = "select * from tb";
            Translate(sql);
        }

        [TestMethod]
        public void sql_some_column_no_condition_test()
        {
            var sql = "select id from tb";
            Translate(sql);
        }

        [TestMethod]
        public void sql_equals_test()
        {
            var sql = "select * from tb where id=2";
            Translate(sql);
        }

        [TestMethod]
        public void sql_not_equals_test()
        {
            var sql = "select * from tb where id<>'2'";
            Translate(sql);
        }

        [TestMethod]
        public void sql_top_count_test()
        {
            var sql = "select top 3 * from tb";
            Translate(sql);
        }

        [TestMethod]
        public void sql_skip_take_test()
        {
            var sql = "select * from tb order by id offset 1 rows fetch next 1 rows only";
            Translate(sql);
        }

        [TestMethod]
        public void sql_in_test()
        {
            var sql = "select * from tb where (id=1 and type=2)or(id=2 and type=3)";
            Translate(sql);
        }
    }
}
